using Avalonia.Diagnostics.ViewModels;
using FormGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaApplication1.ViewModels
{
    public class ReportDialogViewModel : ViewModelBase
    {
        private string subject;
        private DateTime date;
        private string secretary;
        private readonly ReportDialog reportDialog;

        public string Subject
        {
            get => subject;
            set
            {
                subject = value;
                RaisePropertyChanged(nameof(Subject));
            }
        }
        public DateTime Date { 
            get => date;
            set
            {
                date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }
        public string Secretary { 
            get => secretary;
            set 
            {
                secretary = value;
                RaisePropertyChanged(nameof(Secretary));
            }
        }
        public ObservableCollection<ParticipantViewModel> Participants { get; }
        public ObservableCollection<DecisionViewModel> Decisions { get; }

        public ReportDialogViewModel(ReportDialog reportDialog, MeetingNotes meetingNotes)
        {
            Subject = meetingNotes.Subject;
            Date = meetingNotes.Date;
            Secretary = meetingNotes.Secretary;

            Participants = new ObservableCollection<ParticipantViewModel>(
                meetingNotes.Participants.Select(p => new ParticipantViewModel(p)));
            Decisions = new ObservableCollection<DecisionViewModel>(
                meetingNotes.Decisions.Select(d => new DecisionViewModel(d)));
            this.reportDialog = reportDialog;
        }

        public void AddParticipant() 
        {
            Participants.Add(new ParticipantViewModel(new Participant { Name = "Participant" }));
        }

        public void AddDecision()
        {
            Decisions.Add(new DecisionViewModel(new Decision { Problem = "New Problem" }));
        }

        public void OK()
        {
            reportDialog.Close(ToMeetingNotes());
        }

        public void Cancel()
        {
            reportDialog.Close(null);
        }

        private MeetingNotes ToMeetingNotes()
        {
            return new MeetingNotes
            {
                Subject = this.Subject,
                Secretary = this.Secretary,
                Date = this.Date,
                Decisions = new List<Decision>(this.Decisions.Select(d => new Decision 
                { 
                    Solution = d.Solution,
                    Responsible = d.Responsible,
                    Problem = d.Problem,
                    ControlDate = d.ControlDate
                })),
                Participants = new List<Participant>(this.Participants.Select(p => new Participant 
                { 
                    Name = p.Name
                }))
            };
        }
    }

    public class DecisionViewModel : ViewModelBase
    {
        private string problem;
        private string solution;
        private string responsible;
        private DateTime controlDate;

        public DecisionViewModel(Decision decision)
        {
            Problem = decision.Problem;
            Solution = decision.Solution;
            Responsible = decision.Responsible;
            ControlDate = decision.ControlDate;
        }

        public string Problem { 
            get => problem;
            set 
            {
                if (problem == value) 
                    return;
                problem = value;
                RaisePropertyChanged(nameof(Problem));
            }
        }
        public string Solution { 
            get => solution;
            set 
            {
                if (solution == value)
                    return;
                solution = value;
                RaisePropertyChanged(nameof(Solution));
            } 
        }
        public string Responsible { 
            get => responsible;
            set 
            {
                if (responsible == value)
                    return; 
                responsible = value;
                RaisePropertyChanged(nameof(Responsible));
            }
        }
        public DateTime ControlDate { 
            get => controlDate;
            set 
            {
                if (controlDate == value)
                    return;
                controlDate = value;
                RaisePropertyChanged(nameof(ControlDate));
            }
        }
    }

    public class ParticipantViewModel : ViewModelBase
    {
        private string name;

        public ParticipantViewModel(Participant participant)
        {
            Name = participant.Name;
        }
        
        public string Name 
        { 
            get => name;
            set 
            {
                if (name == value)
                    return;
                name = value;
                RaisePropertyChanged(nameof(Name));
            } 
        }
    }
}
