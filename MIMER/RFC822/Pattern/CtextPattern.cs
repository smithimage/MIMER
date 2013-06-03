using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class CtextPattern:IPattern
    {
        private const string m_TextPattern = "[^()\x5C\x5C]+";
        private readonly Regex m_Regex;

        public CtextPattern()
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