using MIMER.RFC822.Pattern;
using NUnit.Framework;

namespace MIMERTests.RFC822.Pattern
{
    [TestFixture]
    public class AddrSpecPatternTests
    {
        [Test]
        public void TextPatternTest()
        {
            AddrSpecPattern pattern = new AddrSpecPattern();
            Assert.AreEqual(pattern.TextPattern, pattern.RegularExpression.ToString());
        }
    }
}
