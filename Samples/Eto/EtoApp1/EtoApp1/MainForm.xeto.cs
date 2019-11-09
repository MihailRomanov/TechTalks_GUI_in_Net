using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
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

        protected void Generate(object sender, EventArgs e)
        {
            MessageBox.Show("Generated!");
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
