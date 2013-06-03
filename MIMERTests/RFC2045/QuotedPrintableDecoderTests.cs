using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using MIMER.RFC2045;

namespace MIMERTests.RFC2045
{
    [TestFixture]
    public class QuotedPrintableDecoderTests
    {
        private QuotedPrintableDecoder m_Decoder;

        [SetUp]
        public void Setup()
        {
            m_Decoder = new QuotedPrintableDecoder();
        }

        [Test]
        public void DecodeStringTest()
        {
            string encodedText = MIMERTests.Strings.QuotedPrintableEncoded;
            string decodedText = Encoding.Default.GetString(m_Decoder.Decode(ref encodedText));
            StringAssert.IsMatch(MIMERTests.Strings.QuotedPrintableDecoded, decodedText);
        }

        [Test]
        public void CanDecodeTest()
        {
            Assert.IsTrue(m_Decoder.CanDecode("quoted-printable"));
            Assert.IsTrue(m_Decoder.CanDecode("7bit"));
            Assert.IsTrue(m_Decoder.CanDecode(null));
            Assert.IsTrue(m_Decoder.CanDecode(string.Empty));

            Assert.IsFalse(m_Decoder.CanDecode("shdfgksdhfhs"));
        }
    }
}
