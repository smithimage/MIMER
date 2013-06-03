using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC822.Pattern
{
    public class DateTimePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DateTimePattern()
        {
            IPattern dayPattern = PatternFactory.GetInstance().Get(typeof (DayPattern));
            IPattern datePattern = PatternFactory.GetInstance().Get(typeof (DatePattern));
            IPattern timePattern = PatternFactory.GetInstance().Get(typeof (TimePattern));
            m_TextPattern = "(" + dayPattern.TextPattern + ", )" + 
                            datePattern.TextPattern + " " + timePattern.TextPattern;
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