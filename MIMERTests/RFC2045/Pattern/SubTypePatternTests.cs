using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIMER.RFC2045.Pattern;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MIMERTests.RFC2045.Pattern
{
    [TestFixture]
    public class SubTypePatternTests
    {
        [Test]
        public void MultipartMixedMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("multipart/mixed"));
        }

        [Test]
        public void MultipartAlternativeMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("multipart/alternative"));
        }

        [Test]
        public void MultipartParallelMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("multipart/parallel"));
        }

        [Test]
        public void MultipartDigestMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("multipart/digest"));
        }

        [Test]
        public void MultipartRelatedMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("multipart/related"));
        }

        [Test]
        public void NoMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("unknown/??"), Iz.False);
        }

        [Test]
        public void TextCalendarMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("text/calendar"), Iz.True);
        }

        [Test]
        public void TextPlainMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("text/plain"), Iz.True);
        }

        [Test]
        public void TextEnrichedMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("text/enriched"), Iz.True);
        }

        [Test]
        public void TextHtmlMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("text/html"), Iz.True);
        }

        [Test]
        public void TextXvcardMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("text/x-vcard"), Iz.True);
        }

        [Test]
        public void ImageJpegMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/jpeg"), Iz.True);
        }

        [Test]
        public void ImageGifMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/gif"), Iz.True);
        }

        [Test]
        public void ImageBmpMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/bmp"), Iz.True);
        }

        [Test]
        public void ImagePngMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/png"), Iz.True);
        }

        [Test]
        public void ImageTiffMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/tiff"), Iz.True);
        }

        [Test]
        public void ImagePjpegMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("image/pjpeg"), Iz.True);
        }

        [Test]
        public void ApplicationGifMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/gif"), Iz.True);
        }

        [Test]
        public void ApplicationJpegMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/jpeg"), Iz.True);
        }


        [Test]
        public void ApplicationMswordMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/msword"), Iz.True);
        }

        [Test]
        public void ApplicationOctetStreamMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/octet-stream"), Iz.True);
        }


        [Test]
        public void ApplicationPostScriptMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/PostScript"), Iz.True);
        }

        [Test]
        public void ApplicationBmpMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/bmp"), Iz.True);
        }

        [Test]
        public void ApplicationMsaccessMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/msaccess"), Iz.True);
        }

        [Test]
        public void ApplicationMsexcelMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/msexcel"), Iz.True);
        }

        [Test]
        public void ApplicationVndmsexcelMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/vnd.ms-excel"), Iz.True);
        }

        [Test]
        public void ApplicationXMSExcelMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/X-MS-Excel"), Iz.True);
        }

        [Test]
        public void ApplicationvndmspowerpointMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/vnd.ms-powerpoint"), Iz.True);
        }

        [Test]
        public void ApplicationxzipcompressedMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/x-zip-compressed"), Iz.True);
        }

        [Test]
        public void ApplicationvndopenxmlformatsofficedocumentspreadsheetmlsheetMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            pattern.Compile();
            Assert.That(pattern.RegularExpression.IsMatch("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"), Iz.True);
        }

        [Test]
        public void MessageRfc822MatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("message/rfc822"), Iz.True);
        }

        [Test]
        public void MessagePartialMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("message/partial"), Iz.True);
        }

        
        [Test]
        public void MessageExternalBodyMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("message/external-body"), Iz.True);
        }

        [Test]
        public void AudioMpegMatchTest()
        {
            SubTypePattern pattern = new SubTypePattern();
            Assert.That(pattern.RegularExpression.IsMatch("audio/mpeg"), Iz.True);
        }
    }
}