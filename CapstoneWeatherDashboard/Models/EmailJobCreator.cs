using System;
using System.Collections.Generic;
using System.IO;

namespace CapstoneWeatherDashboard.Models
{
    public class EmailJobCreator
    {
        private readonly string _pathToSmtpPipeDirectory = "C:/SMTPPipe";
        private readonly string _processsPrefix = "WIVS";
        private readonly string _fromEmailAddress = "noreply@email.com";

        public byte[] FileBytes
        {
            get;
            private set;
        }

        public string EmailCsv
        {
            get;
            private set;
        }
        public EmailJobCreator(byte[] pdfBytes, string emails)
        {
            FileBytes = pdfBytes;
            EmailCsv = emails;
        }

        public void Send()
        {
            string[] emails = EmailCsv.Split(',');
            var emailList = new List<string>();
            foreach (var email in emails)
            {
                emailList.Add(email.Trim());
            }
            var randomGenerator = new Random((int)DateTime.Now.ToBinary());
            string uniqueJobFolder = string.Format("{0}{1}{2}", _processsPrefix, DateTime.Now.ToString("yyyyMdhmmss"), randomGenerator.Next());

            string fullPathToJobFolder = Path.Combine(_pathToSmtpPipeDirectory, uniqueJobFolder);
            Directory.CreateDirectory(fullPathToJobFolder);

            string namePrefix = "00000001";

            WriteHeaderFile(emailList, fullPathToJobFolder, namePrefix);

            WriteBodyFile(fullPathToJobFolder, namePrefix);

            WriteAttachment(FileBytes, fullPathToJobFolder, namePrefix);

            NotifyReady(fullPathToJobFolder);
        }

        private void NotifyReady(string pathToJobFolder)
        {
            string filePath = Path.Combine(pathToJobFolder, "allfiles.mrk");

            FileStream fs = File.Create(filePath);
            fs.Close();
        }

        private void WriteAttachment(byte[] pdfBytes, string pathToJobFolder, string namePrefix)
        {
            // create the ATT file
            string fileName = string.Format("{0}.att", namePrefix);
            using (TextWriter writer = new StreamWriter(Path.Combine(pathToJobFolder, fileName)))
            {
                writer.WriteLine("IncidentReport.pdf");
            }

            // create the attachment directory where the file will be saved
            string attachmentDirectory = Path.Combine(pathToJobFolder, "Attachments");
            Directory.CreateDirectory(attachmentDirectory);

            // create the file
            string attachmentFileName = Path.Combine(attachmentDirectory, "IncidentReport.pdf");

            using (var fs = new FileStream(attachmentFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(pdfBytes);
                }
            }
        }

        private void WriteBodyFile(string pathToJobFolder, string namePrefix)
        {
            string fileName = string.Format("{0}.ptx", namePrefix);
            using (TextWriter writer = new StreamWriter(Path.Combine(pathToJobFolder, fileName)))
            {
                writer.WriteLine("The summary of the weather incident you requested is attached.");
            }
        }

        private void WriteHeaderFile(IList<string> emailList, string pathToJobFolder, string namePrefix)
        {
            string toField = string.Empty;
            for (int i = 0; i < emailList.Count; i++)
            {
                toField += emailList[i];
                if (i != emailList.Count - 1)
                {
                    toField += ";";
                }
            }

            string fileName = string.Format("{0}.hdr", namePrefix);
            using (TextWriter writer = new StreamWriter(Path.Combine(pathToJobFolder, fileName)))
            {
                writer.WriteLine("FROM:{0}", _fromEmailAddress);
                writer.WriteLine("TO:{0}", toField);
                writer.WriteLine("CC:");
                writer.WriteLine("BCC:");
                writer.WriteLine("SUBJECT:Weather Incident Verification System Report");
            }
        }
    }
}
