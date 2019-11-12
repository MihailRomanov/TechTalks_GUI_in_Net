using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace App3.Controllers
{
    public class HomeController : Controller
    {
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
                        var main = Electron.WindowManager.BrowserWindows.First();
                        var file = await Electron.Dialog.ShowSaveDialogAsync(
                            main, new SaveDialogOptions { Title = "Save ..." });


                    }
                },
                new MenuItem { Label = "Справка" }
            };

            Electron.Menu.SetApplicationMenu(menu);
            

            return View();
        }

        public IActionResult ReportDialog()
        {
            return View();
        }
    }
}