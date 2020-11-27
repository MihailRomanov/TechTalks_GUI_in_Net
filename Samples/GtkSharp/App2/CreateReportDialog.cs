using System;
using FormGenerator.Models;
using Gtk;
using System.Linq;
using UI = Gtk.Builder.ObjectAttribute;
using Common;

namespace App2
{
    class CreateReportDialog : Dialog
    {
        [UI] private readonly Entry entrySubject = null;
        [UI] private readonly Entry entryDate = null;
        [UI] private readonly Entry entrySecretary = null;
        [UI] private readonly Button buttonAddParticipant = null;
        [UI] private readonly Button buttonAddDecision = null;
        [UI] private readonly Alignment alignmentParticiapnts = null;
        [UI] private readonly Alignment alignmentDecisions = null;

        private readonly NodeView nodeViewParticiapnts = null;
        private readonly NodeStore nodeStoreParticiapnts = null;

        private readonly NodeView nodeViewDecisions = null;
        private readonly NodeStore nodeStoreDecisions = null;


        public CreateReportDialog() : this(new Builder("CreateReportDialog.glade")) { }

        private CreateReportDialog(Builder builder) : base(builder.GetObject("CreateReportDialog").Handle)
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;

            buttonAddParticipant.Clicked += ButtonAddParticipant_Clicked;
            buttonAddDecision.Clicked += ButtonAddDecision_Clicked;

            nodeStoreParticiapnts = new NodeStore(typeof(ParticipantModel));
            nodeStoreParticiapnts.AddNode(new ParticipantModel() { Fake = true });

            nodeStoreDecisions = new NodeStore(typeof(DecisionModel));
            nodeStoreDecisions.AddNode(new DecisionModel() { Fake = true });

            nodeViewParticiapnts = new NodeView(nodeStoreParticiapnts);
            nodeViewParticiapnts.AppendColumn("Name", new CellRendererText(), "text", 0);
            alignmentParticiapnts.Add(nodeViewParticiapnts);
            nodeViewParticiapnts.ShowAll();

            nodeViewDecisions = new NodeView(nodeStoreDecisions);
            nodeViewDecisions.AppendColumn("Problem", new CellRendererText(), "text", 0);
            nodeViewDecisions.AppendColumn("Solution", new CellRendererText(), "text", 1);
            nodeViewDecisions.AppendColumn("Responsible", new CellRendererText(), "text", 2);
            nodeViewDecisions.AppendColumn("ControlDate", new CellRendererText(), "text", 3);
            alignmentDecisions.Add(nodeViewDecisions);
            nodeViewDecisions.ShowAll();
        }

        private void ButtonAddDecision_Clicked(object sender, EventArgs e)
        {
            var models = nodeStoreDecisions.OfType<DecisionModel>();

            if (models.Count() == 1 && models.First().Fake == true)
                nodeStoreDecisions.Clear();
            nodeStoreDecisions.AddNode(new DecisionModel
            {
                Problem = "Problem",
                Solution = "Solution",
                Responsible = "Responsible",
                ControlDate = DateTime.Now.ToString()
            }); ;
        }

        private void ButtonAddParticipant_Clicked(object sender, EventArgs e)
        {
            var models = nodeStoreParticiapnts.OfType<ParticipantModel>();

            if (models.Count() == 1 && models.First().Fake == true)
                nodeStoreParticiapnts.Clear();
            nodeStoreParticiapnts.AddNode(new ParticipantModel { Name = "Participant" });
        }

        private void Dialog_Response(object o, ResponseArgs args)
        {
            Hide();
        }

        public void FromMeetingNotes(MeetingNotes meetingNotes)
        {
            entrySubject.Text = meetingNotes.Subject;
            entryDate.Text = meetingNotes.Date.ToString();
            entrySecretary.Text = meetingNotes.Secretary;
        }

        public MeetingNotes ToMeetingNotes()
        {
            return new MeetingNotes
            {
                Subject = entrySubject.Text,
                Date = DateTime.Parse(entryDate.Text),
                Secretary = entrySecretary.Text,
                Participants = nodeStoreParticiapnts
                    .OfType<ParticipantModel>()
                    .Select(m => m.ToParticipant())
                    .ToList(),
                Decisions = nodeStoreDecisions
                    .OfType<DecisionModel>()
                    .Select(m => m.ToDecision())
                    .ToList()
            };
        }
    }
}
