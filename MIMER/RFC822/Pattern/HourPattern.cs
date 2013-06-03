using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class HourPattern:IPattern
    {
        private const string m_TextPattern = "[0-9]{2,2}:[0-9]{2,2}(:[0-9]{2,2})*";
        private Regex m_Regex;

        public HourPattern()
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