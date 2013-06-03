
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class CreationDateParmPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public CreationDateParmPattern()
        {
            IPattern quotedDatetimePattern = PatternFactory.GetInstance().Get(typeof (QuotedDateTimePattern));
            m_TextPattern = "creation-date=" + quotedDatetimePattern.TextPattern;
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
