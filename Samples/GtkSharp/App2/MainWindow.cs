using System;
using System.Diagnostics;
using System.IO;
using FormGenerator;
using FormGenerator.Models;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace App2
{
    class MainWindow : Window
    {
        [UI] TextView textviewCurrent = null;

        MeetingNotes meetingNotes = null;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
           
         }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Exit(object sender, EventArgs a)
        {
            Application.Quit();
        }

        private void NewReport(object sender, EventArgs a)
        {
            var dialog = new CreateReportDialog();
            dialog.FromMeetingNotes(new MeetingNotes());
            var result = dialog.Run();

            if (result == (int)ResponseType.Ok)
            {
                meetingNotes = dialog.ToMeetingNotes();
                textviewCurrent.Buffer.Text = meetingNotes.Subject + " " + meetingNotes.Secretary;
            }
        }

        private void EditReport(object sender, EventArgs a)
        {
            var dialog = new CreateReportDialog();
            dialog.FromMeetingNotes(meetingNotes);
            var result = dialog.Run();

            if (result == (int)ResponseType.Ok)
            {
                meetingNotes = dialog.ToMeetingNotes();
                textviewCurrent.Buffer.Text = meetingNotes.Subject + " " + meetingNotes.Secretary;
            }
        }

        private void Generate(object sender, EventArgs a)
        {
            if (meetingNotes == null)
                return;

            var saveFileDialog = new FileChooserDialog("Save report as ...", this, FileChooserAction.Save,
                "OK", ResponseType.Ok, "Cancel", ResponseType.Cancel);
            saveFileDialog.SetFilename("report.docx");

            var dialogResult = saveFileDialog.Run();
            saveFileDialog.Hide();

            if (dialogResult != (int)ResponseType.Ok)
                return;
                        
            var filename = saveFileDialog.Filename;

            Debug.Print(filename);

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
