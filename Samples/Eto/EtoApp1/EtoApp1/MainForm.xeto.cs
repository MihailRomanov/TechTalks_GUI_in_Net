using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using FormGenerator;
using FormGenerator.Models;

namespace EtoApp1
{ 
    public class MainForm : Form
    {
        private MeetingNotes meetingNotes = new MeetingNotes();
        public MainForm()
        {
            XamlReader.Load(this);
        }

        protected void CreateNew(object sender, EventArgs e)
        {
            meetingNotes = new MeetingNotes
            {
                Subject = "Sub",
                Date = DateTime.Today,
                Secretary = "Sec",
                Participants = new List<Participant> { new Participant { Name = "P1"}  },
                Decisions = new List<Decision> { new Decision { Problem = "s1"} }
            };
            var dialog = new ReportDialog(meetingNotes);
            dialog.ShowModal();
        }

        protected void Open(object sender, EventArgs e)
        {
            var dialog = new ReportDialog(meetingNotes);
            dialog.ShowModal();
        }

        protected void Generate(object sender, EventArgs e)
        {
            if (meetingNotes == null)
                return;

            var saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog(this) == DialogResult.Cancel)
                return;

            var filename = saveFileDialog.FileName;

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

        protected void HandleAbout(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog(this);
        }

        protected void HandleQuit(object sender, EventArgs e)
        {
            Application.Instance.Quit();
        }
    }
}
