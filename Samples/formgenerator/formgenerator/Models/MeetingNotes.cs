using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FormGenerator.Models
{
    [XmlRoot("meetingNotes", Namespace = "urn:MeetingNotes")]
    public class MeetingNotes
    {
        [XmlAttribute("subject")]
        public string Subject { get; set; }

        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlAttribute("secretary")]
        public string Secretary { get; set; }

        [XmlArray("participants")]
        public List<Participant> Participants { get; set; }

        [XmlArray("decisions")]
        public List<Decision> Decisions { get; set; }

    }
}