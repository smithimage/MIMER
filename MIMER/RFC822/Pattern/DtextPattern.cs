using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class DtextPattern:IPattern
    {
        private const string m_TextPattern = "[^]\x0D\x5B\x5C\x5C\x80-\xFF]";
        private Regex m_Regex;

        public DtextPattern()
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