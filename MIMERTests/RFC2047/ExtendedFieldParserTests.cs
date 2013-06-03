using System;
using System.Collections.Generic;
using System.Text;
using MIMER.RFC2045;
using MIMER.RFC2047;
using MIMER.RFC822;
using NUnit.Framework;

namespace MIMERTests.RFC2047
{
    [TestFixture]
    public class ExtendedFieldParserTests
    {
        private ExtendedFieldParser m_Parser;
        [SetUp]
        public void setup()
        {
            m_Parser =
                new ExtendedFieldParser(
                    new ContentTransferEncodingFieldParser(new ContentTypeFieldParser(new FieldParser())));
        }

        [Test]
        public void TestParse()
        {
            IList<MIMER.RFC822.Field> fields = new List<MIMER.RFC822.Field>();
            string sFields = MIMERTests.Strings.ExtendedFields;
            m_Parser.Parse(ref fields, ref sFields);
            Assert.AreEqual(15, fields.Count);

            fields.Clear();
            string sField = MIMERTests.Strings.ExtendedField;
            m_Parser.Parse(ref fields, ref sField);
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual("Robert Wallström <mrx@unknown.com>", fields[0].Body);
        }
    }
}
