using Common;
using FormGenerator.Models;
using Gtk;
using System;
using System.Linq;

namespace App1
{
    class MeetingNotesDialog : Dialog
    {
        private Entry entrySubject = null;
        private Entry entryDate = null;
        private Entry entrySecretary = null;


        private NodeView nodeViewParticiapnts = null;
        private NodeView nodeViewDecisions = null;


        public MeetingNotesDialog(Window parent, MeetingNotes meetingNotes) : base("Meeting Notes", parent, DialogFlags.Modal)
        {
            Initialize();
            FromMeetingNotes(meetingNotes);
            ShowAll();
        }

        private void FromMeetingNotes(MeetingNotes meetingNotes)
        {
            entrySubject.Text = meetingNotes.Subject ?? string.Empty;
            entrySecretary.Text = meetingNotes.Secretary?? string.Empty;
            entryDate.Text = meetingNotes.Date.ToShortDateString();

            if (meetingNotes.Decisions != null && meetingNotes.Decisions.Any())
            {
                meetingNotes.Decisions
                    .Select(d => d.ToDecisionModel())
                    .ToList()
                    .ForEach(nodeViewDecisions.NodeStore.AddNode);
            }
            else
                nodeViewDecisions.NodeStore.AddNode(new DecisionModel { Fake = true });

            if (meetingNotes.Participants != null && meetingNotes.Participants.Any())
            {
                meetingNotes.Participants
                    .Select(p => p.ToParticipantModel())
                    .ToList()
                    .ForEach(nodeViewParticiapnts.NodeStore.AddNode);
            }
            else
                nodeViewParticiapnts.NodeStore.AddNode(new ParticipantModel { Fake = true });
        }

        private void Initialize()
        {
            SetDefaultSize(300, 0);

            AddButton("Ok", ResponseType.Ok);
            AddButton("Cancel", ResponseType.Cancel);

            var grid = new Grid
            {
                ColumnSpacing = 10,
                RowSpacing = 10,
                Hexpand = true,
            };
            entrySubject = new Entry { Hexpand = true };
            entrySecretary = new Entry { Hexpand = true };
            entryDate = new Entry { Hexpand = true };

            grid.Attach(new Label("Subject") { Halign = Align.End }, 0, 0, 1, 1);
            grid.Attach(entrySubject, 1, 0, 1, 1);
            grid.Attach(new Label("Date") { Halign = Align.End }, 0, 1, 1, 1);
            grid.Attach(entryDate, 1, 1, 1, 1);
            grid.Attach(new Label("Secretary") { Halign = Align.End }, 0, 2, 1, 1);
            grid.Attach(entrySecretary, 1, 2, 1, 1);


            nodeViewParticiapnts = CreateNodeView<ParticipantModel>();

            grid.Attach(new Label("Participants") { Halign = Align.End, Valign = Align.Start }, 0, 3, 1, 1);
            var participantsVBox = new VBox() { Spacing = 10 };
            var participantsButtonBox = new HButtonBox { Halign = Align.Start };
            var addParticipantButton = new Button { Label = "Add participant" };
            addParticipantButton.Clicked += AddParticipantButton_Clicked; ;
            participantsButtonBox.PackStart(addParticipantButton, false, false, 0);
            participantsVBox.PackStart(participantsButtonBox, false, false, 0);
            participantsVBox.PackStart(nodeViewParticiapnts, true, true, 0);
            grid.Attach(participantsVBox, 1, 3, 1, 1);


            nodeViewDecisions = CreateNodeView<DecisionModel>();

            grid.Attach(new Label("Decisions") { Halign = Align.End, Valign = Align.Start }, 0, 4, 1, 1);
            var decisionsVBox = new VBox() { Spacing = 10 };
            var decisionsButtonBox = new HButtonBox() { Halign = Align.Start };
            var addDecisionButton = new Button { Label = "Add decision" };
            addDecisionButton.Clicked += AddDecisionButton_Clicked; ;
            decisionsButtonBox.PackStart(addDecisionButton, false, false, 0);
            decisionsVBox.PackStart(decisionsButtonBox, false, false, 0);
            decisionsVBox.PackStart(nodeViewDecisions, true, true, 0);
            grid.Attach(decisionsVBox, 1, 4, 1, 1);

            ContentArea.PackStart(grid, true, true, 10);

        }

        private void AddDecisionButton_Clicked(object sender, EventArgs e)
        {
            nodeViewDecisions.NodeStore.AddNode(new DecisionModel { });
        }

        private void AddParticipantButton_Clicked(object sender, EventArgs e)
        {
            nodeViewParticiapnts.NodeStore.AddNode(new ParticipantModel { });
        }

        private NodeView CreateNodeView<TModel>() where TModel : ITreeNode
        {
            var modelType = typeof(TModel);

            var store = new NodeStore(modelType);
            var nodeView = new NodeView(store) {  };

            var bindingProperties = modelType.GetProperties()
                .Select(p => new
                {
                    prop = p,
                    column = p.GetCustomAttributes(false).OfType<TreeNodeValueAttribute>().SingleOrDefault()?.Column ?? -1,
                    name = p.Name
                })
                .Where(p => p.column >= 0)
                .OrderBy(p => p.column);

            foreach (var property in bindingProperties)
            {
                var cellRender = new CellRendererText() { Editable = true };
                var prop = property;

                cellRender.Edited += (o, args) =>
                {
                    var node = store.GetNode(new TreePath(args.Path));
                    prop.prop.SetValue(node, args.NewText);
                };
                var coll = nodeView.AppendColumn(prop.name, cellRender, "text", prop.column);
                coll.Resizable = true;
                coll.Expand = true;
            }

            return nodeView;
        }

        public MeetingNotes ToMeetingNotes()
        {
            return new MeetingNotes
            {
                Subject = entrySubject.Text,
                Date = DateTime.Parse(entryDate.Text),
                Secretary = entrySecretary.Text,
                Participants = nodeViewParticiapnts.NodeStore
                    .OfType<ParticipantModel>()
                    .Select(m => m.ToParticipant())
                    .ToList(),
                Decisions = nodeViewDecisions.NodeStore
                    .OfType<DecisionModel>()
                    .Select(m => m.ToDecision())
                    .ToList()
            };
        }
    }
}
