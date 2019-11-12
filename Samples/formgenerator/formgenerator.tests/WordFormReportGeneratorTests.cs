using FormGenerator.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace FormGenerator.Tests
{
    [TestClass]
    public class WordFormReportGeneratorTests
    {
        [TestMethod]
        public void GenerateForTemplate1()
        {
            var generator = new WordFormReportGenerator();

            var meetingNotes = new MeetingNotes
            {
                Subject = "Meeting Subject",
                Date = DateTime.Now,
                Secretary = "Ivan Markov",
                Participants = new List<Participant>
                {
                    new Participant { Name = "Henry Loyd" }
                },
                Decisions = new List<Decision>
                {
                    new Decision
                    {
                        Solution = "Make it",
                        ControlDate = DateTime.Now.AddDays(5),
                        Problem = "Big problem",
                        Responsible = "Mary Stomler"
                    }
                }
            };

            var reportStream = generator.GenerateDocument("Template1", meetingNotes);
            var outFileStream = new FileStream("report.docx", FileMode.Create);

            reportStream.CopyTo(outFileStream);
            outFileStream.Close();
        }
    }
}
