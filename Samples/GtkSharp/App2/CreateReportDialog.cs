using System;
using FormGenerator.Models;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace App2
{
    class CreateReportDialog : Dialog
    {
        [UI] private Entry entrySubject = null;
        [UI] private Entry entryDate = null;
        [UI] private Entry entrySecretary = null;
        
        public CreateReportDialog() : this(new Builder("CreateReportDialog.glade")) { }

        private CreateReportDialog(Builder builder) : base(builder.GetObject("CreateReportDialog").Handle)
        {
            builder.Autoconnect(this);
            DefaultResponse = ResponseType.Cancel;

            Response += Dialog_Response;
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
