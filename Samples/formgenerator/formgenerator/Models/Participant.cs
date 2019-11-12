using System.Xml.Serialization;

namespace FormGenerator.Models
{
    [XmlType("participant")]
    public class Participant
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}