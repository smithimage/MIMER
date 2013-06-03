using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class DatePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DatePattern()
        {
            IPattern monthPattern = PatternFactory.GetInstance().Get(typeof (MonthPattern));
            m_TextPattern = "([0-9]{2,2}\x5C\x73" + monthPattern.TextPattern + "\x5C\x73[0-9]{2,4}){1,1}";
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