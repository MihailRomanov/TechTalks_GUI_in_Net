using System;
using System.Linq;
using Eto.Forms;
using Eto.Serialization.Xaml;
using FormGenerator.Models;

namespace EtoApp1
{
    public class ReportDialog : Dialog
    {
        public MeetingNotes MeetingNotes { get; set; }

        public ReportDialog(MeetingNotes meetingNotes)
        {
            this.MeetingNotes = meetingNotes;

            XamlReader.Load(this);

            DataContext = MeetingNotes;
        }

        public void AddParticipant(object sender, EventArgs e)
        {
            MeetingNotes.Participants.Add(new Participant
            {
                Name = $"Participant {MeetingNotes.Participants.Count + 1}"
            });
            UpdateBindings(BindingUpdateMode.Destination);
        }

        public void AddDecision(object sender, EventArgs e)
        {
            var count = MeetingNotes.Decisions.Count() + 1;
            MeetingNotes.Decisions.Add(new Decision
            {
                Problem = $"Problem {count}",
                Solution = $"Solution {count}",
                Responsible = $"Responsible {count}",
                ControlDate = DateTime.Today.AddDays(1)
            });
            UpdateBindings(BindingUpdateMode.Destination);
        }
    }
}
