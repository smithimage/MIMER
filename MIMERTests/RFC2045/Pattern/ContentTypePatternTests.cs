using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MIMER.RFC2045.Pattern;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MIMERTests.RFC2045.Pattern
{
    [TestFixture]
    public class ContentTypePatternTests
    {
        [Test]
        public void MatchTesst()
        {
            string field = "Content-Type: Multipart/Mixed;boundary=\"Boundary-00=_T7P340MWKGMMYJ0CCJD0\"";
            ContentTypePattern pattern = new ContentTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch(field), Iz.True);
        }

        [Test]
        public void MatchComponentsTest()
        {
            string field = "Content-Type: Multipart/Mixed;boundary=\"Boundary-00=_T7P340MWKGMMYJ0CCJD0\"";
            ContentTypePattern pattern = new ContentTypePattern();
            Match match = pattern.RegularExpression.Match(field);
            Assert.That(match.Value, Iz.EqualTo("Content-Type: Multipart/Mixed;boundary=\"Boundary-00=_T7P340MWKGMMYJ0CCJD0\""));
        }
    }
}
