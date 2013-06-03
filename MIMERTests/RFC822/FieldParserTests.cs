using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MIMER.RFC822;

namespace MIMERTests.RFC822
{
    [TestFixture]
    public class FieldParserTests
    {
        private FieldParser m_Parser;        

        [SetUp]
        public void Setup()
        {
            m_Parser = new FieldParser();
        }

        [Test]
        public void ParseTest()
        {
            IList<Field> fields = new List<Field>();
            string sFields = MIMERTests.Strings.Fields;
            m_Parser.Parse(ref fields, ref sFields);
            Assert.AreEqual(15, fields.Count);                   
        }
    }
}
