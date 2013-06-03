
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class MIMEVersionPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public MIMEVersionPattern()
        {
            IPattern commentPattern = PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.CommentPattern));
            m_TextPattern = "(?i)MIME-Version:(?-i)([0-9]{1,1}|\\x2E{1,1}|" + commentPattern.TextPattern + ")*";
            m_Regex = new Regex(m_TextPattern);
        }

        public string TextPattern
        {
            get { return m_TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_Regex; }
        }
    }
}
