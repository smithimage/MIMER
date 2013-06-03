using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MIMER.RFC2045;
using MIMER.RFC822;

namespace MIMERTests.RFC2045
{
    [TestFixture]
    public class ContentTypeFieldParserTests
    {
        private ContentTypeFieldParser m_Parser;

        [SetUp]
        public void Setup()
        {
            m_Parser = new ContentTypeFieldParser(new FieldParser());
        }

        [Test]
        public void ParseTest()
        {
            IList<Field> fields = new List<Field>();
            string sFields = MIMERTests.Strings.ContentTypeFields;
            m_Parser.Parse(ref fields, ref sFields);
            Assert.AreEqual(6, fields.Count);

        }
    }
}
