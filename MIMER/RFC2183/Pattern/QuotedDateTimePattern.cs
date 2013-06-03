
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class QuotedDateTimePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public QuotedDateTimePattern()
        {
            IPattern datetimePattern = PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.DateTimePattern));
            m_TextPattern = "\"" + datetimePattern.TextPattern + "\"";
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled);
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
