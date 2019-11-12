using System;
using System.Xml.Serialization;

namespace FormGenerator.Models
{
    [XmlType("decision")]
    public class Decision
    {
        [XmlAttribute("problem")]
        public string Problem { get; set; }

        [XmlAttribute("solution")]
        public string Solution { get; set; }

        [XmlAttribute("responsible")]
        public string Responsible { get; set; }

        [XmlAttribute("controlDate")]
        public DateTime ControlDate { get; set; }
    }
}