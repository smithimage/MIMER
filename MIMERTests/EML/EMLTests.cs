using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using MIMER.RFC2045;
using MIMERTests.RFC2045;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MIMERTests.EML
{
    [TestFixture]
    public class EMLTests
    {
        private System.IO.Stream m_Stream;

        [SetUp]
        public void SetUp()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = path.Replace("file:///", string.Empty);
            path = path.Replace("/", "\\");
            FileInfo finf = new FileInfo(path);
            m_Stream = new FileStream(finf.DirectoryName + "\\Resources\\Testing the _eml format.eml", FileMode.Open,
                FileAccess.Read);
        }

        [Test]
        public void TestParse()
        {
            Assert.IsNotNull(m_Stream);
            MIMER.RFC2045.MailReader m = new MailReader();
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();

            MIMER.RFC2045.IMimeMailMessage message = m.ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.AreEqual("eml@test.com", message.To[0].Address);
            Assert.AreEqual("eml2@test.com", message.CarbonCopy[0].Address);
            Assert.AreEqual("Testing the .eml format", message.Subject);
            Assert.AreEqual(3, message.Attachments.Count);
            Assert.That(message.Attachments.Count(x=>x.Name != null && x.Name.Equals("cp_bg_black800.gif")), Iz.EqualTo(1));
            System.Net.Mail.MailMessage mailmessage = message.ToMailMessage();
            Assert.IsNull(mailmessage.From);
            Assert.AreEqual("eml2@test.com", mailmessage.CC[0].Address);
            Assert.AreEqual(message.TextMessage, mailmessage.Body);
            Assert.AreEqual(message.Attachments.Count, mailmessage.Attachments.Count);
        }


    }
}
