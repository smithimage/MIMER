using System;
using System.Collections.Generic;
using System.Text;
using MIMER;
using MIMER.RFC2045;
using MIMER.RFC2047;
using MIMER.RFC2183;
using MIMER.RFC822;
using MIMER.RFC822.Pattern;
using NUnit.Framework;

namespace MIMERTests.RFC2183
{
    [TestFixture]
    public class ContentDispositionFieldParserTests
    {
        private string m_Fields;

        [SetUp]
        public void Setup()
        {
            m_Fields = MIMERTests.Strings.ContentDispositionFields;
        }

        [Test]
        public void ParseTest() 
        {
            IList<MIMER.RFC822.Field> fields = new List<MIMER.RFC822.Field>();
            MIMER.RFC2183.ContentDispositionFieldParser parser =
                new ContentDispositionFieldParser(
                    new ExtendedFieldParser(
                        new ContentTransferEncodingFieldParser(new ContentTypeFieldParser(new FieldParser()))));
            parser.CompilePattern();
            IPattern unfoldPattern = PatternFactory.GetInstance().Get(typeof (UnfoldPattern));
            m_Fields = unfoldPattern.RegularExpression.Replace(m_Fields, " ");
            parser.Parse(ref fields, ref m_Fields);
            Assert.AreEqual(5, fields.Count);
            ContentDispositionField field = fields[4] as ContentDispositionField;
            Assert.IsNotNull(field);
            Assert.AreEqual("attachment", field.Disposition);
            Assert.AreEqual(2, field.Parameters.Count);
            Assert.AreEqual("genome.jpeg", field.Parameters["filename"]);
            Assert.AreEqual("Wed, 12 Feb 1997 16:29:51 -0500", field.Parameters["modification-date"]);
        }
    }
}
