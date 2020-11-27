using FormGenerator.Models;
using Gtk;
using System;

namespace Common
{
    [TreeNode(ListOnly = true)]
    public class ParticipantModel : TreeNode
    {
        [TreeNodeValue(Column = 0)]
        public string Name { get; set; }

        public bool Fake { get; set; }
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

        public bool Fake { get; set; }
    }

    public static class ModelsExtensions
    {
        public static ParticipantModel ToParticipantModel(this Participant participant)
        {
            return new ParticipantModel
            {
                Name = participant.Name
            };
        }

        public static Participant ToParticipant(this ParticipantModel participantModel)
        {
            return new Participant
            {
                Name = participantModel.Name
            };
        }

        public static DecisionModel ToDecisionModel(this Decision decision)
        {
            return new DecisionModel
            {
                ControlDate = decision.ControlDate.ToShortDateString(),
                Problem = decision.Problem,
                Responsible = decision.Responsible,
                Solution = decision.Solution,
            };
        }

        public static Decision ToDecision(this DecisionModel decisionModel)
        {
            if (!DateTime.TryParse(decisionModel.ControlDate, out var date))
                date = DateTime.Today;

            return new Decision
            {
                ControlDate = date,
                Problem = decisionModel.Problem,
                Responsible = decisionModel.Responsible,
                Solution = decisionModel.Solution,
            };
        }
    }
}
