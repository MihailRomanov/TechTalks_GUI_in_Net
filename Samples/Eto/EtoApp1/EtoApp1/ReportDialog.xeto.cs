using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using FormGenerator.Models;

namespace EtoApp1
{
    public class MyModel : INotifyPropertyChanged
    {
        string myString;
        public string MyString
        {
            get { return myString; }
            set
            {
                if (myString != value)
                {
                    myString = value;
                    OnPropertyChanged();
                }
            }
        }

        void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ReportDialog : Dialog
    {
        public TextBox textSubject = null;
        public TextBox textSecretary = null;
        public DateTimePicker dtDate = null;
        public GridColumn gcParticipantName = null;
        public GridView gvParticipants = null;
        public GridView gvDecisions = null;
        //public GridColumn gcDecision

        public MeetingNotes MeetingNotes { get; set; }

        public ReportDialog(MeetingNotes meetingNotes)
        {
            this.MeetingNotes = meetingNotes;

            XamlReader.Load(this);
            textSubject.TextBinding.BindDataContext((MeetingNotes m) => m.Subject);
            textSecretary.TextBinding.BindDataContext((MeetingNotes m) => m.Secretary);
            dtDate.BindDataContext(c => c.Value, (MeetingNotes m) => m.Date);

            gvParticipants.DataStore = MeetingNotes.Participants;
            gcParticipantName.DataCell = new TextBoxCell("Name");



            DataContext = MeetingNotes;
        }

        public void AddParticipant(object sender, EventArgs e)
        {

        }

        public void AddDecision(object sender, EventArgs e)
        {
        }

    }
}
