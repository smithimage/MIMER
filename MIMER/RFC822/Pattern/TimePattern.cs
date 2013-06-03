using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC822.Pattern
{
    public class TimePattern:IPattern
    {
        private string m_TextPattern;
        private Regex m_Regex;

        public TimePattern()
        {
            IPattern hourPattern = PatternFactory.GetInstance().Get(typeof(HourPattern));
            IPattern zonePattern = PatternFactory.GetInstance().Get(typeof(ZonePattern));
            m_TextPattern = hourPattern.TextPattern + zonePattern.TextPattern;
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