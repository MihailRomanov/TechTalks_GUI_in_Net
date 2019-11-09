using System;
using FormGenerator.Models;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace App2
{
    [TreeNode(ListOnly = true)]
    public class ParticipantModel : TreeNode
    { 
        [TreeNodeValue(Column = 0)]
        public string Name { get; set; }
    }

    [TreeNode(ListOnly = true)]
    public class DecisionModel : TreeNode
    {
        [TreeNodeValue(Column = 0)]
        public string Problem { get; set; }

        [TreeNodeValue(Column = 1)]
        public string Solution { get; set; }

        [TreeNodeValue(Column = 2)]
        public string Responsible { get; set; }

        [TreeNodeValue(Column = 3)]
        public string ControlDate { get; set; }
    }

    class CreateReportDialog : Dialog
    {
        [UI] private Entry entrySubject = null;
        [UI] private Entry entryDate = null;
        [UI] private Entry entrySecretary = null;
        [UI] private Button buttonAddParticipant = null;
        [UI] private Alignment alignmentParticiapnts = null;
        [UI] private Alignment alignmentDecisions = null;

        private NodeView nodeViewParticiapnts = null;
        private NodeStore nodeStoreParticiapnts = null;

        private NodeView nodeViewDecisions = null;
        private NodeStore nodeStoreDecisions = null;


        public CreateReportDialog() : this(new Builder("CreateReportDialog.glade")) { }

        private CreateReportDialog(Builder builder) : base(builder.GetObject("CreateReportDialog").Handle)
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;

            buttonAddParticipant.Clicked += ButtonAddParticipant_Clicked;

            nodeStoreParticiapnts = new NodeStore(typeof(ParticipantModel));
            //nodeStoreParticiapnts.AddNode(new ParticipantModel());

            nodeStoreDecisions = new NodeStore(typeof(DecisionModel));
            nodeStoreDecisions.AddNode(new DecisionModel());

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

        private void ButtonAddParticipant_Clicked(object sender, EventArgs e)
        {
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
                Secretary = entrySecretary.Text
            };
        }
    }
}
