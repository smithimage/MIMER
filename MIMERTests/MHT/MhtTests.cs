using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMER.RFC2045;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MIMERTests.MHT
{
    [TestFixture]
    public class MhtTests
    {

        private System.IO.Stream m_Stream;

        [SetUp]
        public void SetUp()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = path.Replace("file:///", string.Empty);
            path = path.Replace("/", "\\");
            FileInfo finf = new FileInfo(path);
            m_Stream = new FileStream(finf.DirectoryName + "\\Resources\\Test.mht", FileMode.Open,
                FileAccess.Read);
        }

        [Test]
        public void TestThatCssFileGetsParsed()
        {
            Assert.IsNotNull(m_Stream);
            var reader = new MailReader();
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();
            var message = reader.ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.That(message.Attachments.Count(a=>a.Type.Equals("text") && a.SubType.Equals("css")), Iz.EqualTo(1));
            
        }

        [Test]
        public void TestThatXmlFilesGetsParsed()
        {
            Assert.That(m_Stream, Iz.Not.Null);
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();
            var message = new MailReader().ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.That(message.Attachments.Count(a => a.Type.Equals("text") && a.SubType.Equals("xml")), Iz.EqualTo(1));
        }

        [Test]
        public void TestThatCorrectNumberOfAttachmentsGetsParsed()
        {
            Assert.That(m_Stream, Iz.Not.Null);
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();
            var message = new MailReader().ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.That(message.Attachments.Count(), Iz.EqualTo(117));
        }

        [Test]
        public void TestThatCorrectNumberOfImagesGetsParsed()
        {
            Assert.That(m_Stream, Iz.Not.Null);
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();
            var message = new MailReader().ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.That(message.Attachments.Count(a => a.Type.Equals("image")), Iz.EqualTo(105));
        }

        [Test]
        public void TestThatCorrectNumberOfHtmlDocumentsGetsParsed()
        {
            Assert.That(m_Stream, Iz.Not.Null);
            MIMER.IEndCriteriaStrategy endofmessage = new BasicEndOfMessageStrategy();
            var message = new MailReader().ReadMimeMessage(ref m_Stream, endofmessage);
            Assert.That(message.Attachments.Count(a => a.Type.Equals("text") && a.SubType.Equals("html")), Iz.EqualTo(10));
        }


    }
}
