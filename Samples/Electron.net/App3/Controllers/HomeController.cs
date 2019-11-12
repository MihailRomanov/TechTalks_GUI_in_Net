using App3.Storage;
using ElectronNET.API;
using ElectronNET.API.Entities;
using FormGenerator;
using FormGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App3.Controllers
{
    public class HomeController : Controller
    {
        private readonly MeetingNotesStorage meetingNotesStorage;

        public HomeController(MeetingNotesStorage meetingNotesStorage)
        {
            this.meetingNotesStorage = meetingNotesStorage;
        }

        public IActionResult Index()
        {
            var Uri = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value, "index.html").Uri;

            var uri = Url.Action("ReportDialog", "Home", null, Request.Scheme);

            var menu = new MenuItem[] 
            {
                new MenuItem { Label = "Файл", 
                    Submenu = new MenuItem[] 
                    { 
                        new MenuItem { Label = "Создать", 
                            Click = async () => 
                            {
                                var window = await Electron.WindowManager.CreateWindowAsync(
                                    new BrowserWindowOptions 
                                    { 
                                        Frame = true,
                                        UseContentSize = true
                                    },
                                    uri.ToString());
                                window.SetMenu(new MenuItem[] { });
                            } 
                        },
                        new MenuItem { Label = "Открыть" },
                        new MenuItem { Type = MenuType.separator },
                        new MenuItem { Label = "Выйти", 
                            Click = () => { Electron.App.Quit(); } }
                    }
                },
                new MenuItem { Label = "Генерировать",
                    Click = async () => 
                    {
                        await Generate();
                    }
                },
                new MenuItem { Label = "Справка" }
            };

            Electron.Menu.SetApplicationMenu(menu);
            

            return View();
        }

        public IActionResult ReportDialog()
        {
            var saveUrl = Url.Action("Save", "Home", null, Request.Scheme);
            return View("ReportDialog", saveUrl);
        }

        [HttpPost]
        public IActionResult Save([FromBody]MeetingNotes meetingNotes)
        {
            meetingNotesStorage.MeetingNotes = meetingNotes;
            Electron.WindowManager.BrowserWindows.Last().Close();
            return Ok();
        }

        private async Task Generate()
        {
            var main = Electron.WindowManager.BrowserWindows.First();
            var filename = await Electron.Dialog.ShowSaveDialogAsync(
                main, new SaveDialogOptions { Title = "Save ..." });

            if (string.IsNullOrWhiteSpace(filename))
                return;

            var generator = new WordFormReportGenerator();
            var stream = generator.GenerateDocument("Template1", meetingNotesStorage.MeetingNotes);

            var fileStream = new FileStream(filename, FileMode.Create);
            stream.CopyTo(fileStream);
            fileStream.Close();

            new Process
            {
                StartInfo = new ProcessStartInfo(filename)
                {
                    UseShellExecute = true
                }
            }.Start();

        }
    }
}