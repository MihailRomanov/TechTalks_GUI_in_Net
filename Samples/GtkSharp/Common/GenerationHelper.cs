using FormGenerator;
using FormGenerator.Models;
using System.Diagnostics;
using System.IO;

namespace Common
{
    public static class GenerationHelper
    {
        public static void GenerateAndOpen(string filename, MeetingNotes meetingNotes)
        {
            var generator = new WordFormReportGenerator();
            var stream = generator.GenerateDocument("Template1", meetingNotes);

            var fileStream = new FileStream(filename, FileMode.Create);
            stream.CopyTo(fileStream);
            fileStream.Close();

            new Process
            {
                StartInfo = new ProcessStartInfo(filename)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
    }
}
