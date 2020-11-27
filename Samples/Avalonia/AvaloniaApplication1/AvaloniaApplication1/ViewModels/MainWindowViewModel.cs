using Avalonia.Controls;
using FormGenerator;
using FormGenerator.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MeetingNotes meetingNotes = new MeetingNotes();
        private readonly MainWindow mainWindow;

        public MainWindowViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public async Task Create()
        {
            meetingNotes = new MeetingNotes
            {
                Subject = "Sub",
                Secretary = "Secret",
                Date = DateTime.Today,
                Participants = new List<Participant>
                {
                    new Participant { Name = "Part 1" }
                },
                Decisions = new List<Decision>
                {
                    new Decision{
                        Solution = "Solution 0",
                        Problem = "Problem 0",
                        Responsible = "Responsible 0",
                        ControlDate = DateTime.Today.AddDays(2)
                    }
                }
            };
            var dialog = new ReportDialog(meetingNotes);
            var result = await dialog.ShowDialog<MeetingNotes>(mainWindow);

            if (result != null)
                meetingNotes = result;
        }
        public async Task Open()
        {
            var dialog = new ReportDialog(meetingNotes);
            var result = await dialog.ShowDialog<MeetingNotes>(mainWindow);

            if (result != null)
                meetingNotes = result;
        }
        public void Quit()
        {
            mainWindow.Close();
        }

        public async Task Generate()
        {
            if (meetingNotes == null)
                return;

            var fileDialog = new SaveFileDialog();
            var filename = await fileDialog.ShowAsync(mainWindow);

            if (string.IsNullOrWhiteSpace(filename))
                return;

            var generator = new WordFormReportGenerator();
            var stream = generator.GenerateDocument("Template1", meetingNotes);

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
