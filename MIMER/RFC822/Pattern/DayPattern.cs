using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class DayPattern:IPattern
    {
        private const string m_TextPattern = "(Mon|Tue|Wed|Thu|Fri|Sat|Sun)";
        private Regex m_Regex;

        public DayPattern()
        {
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