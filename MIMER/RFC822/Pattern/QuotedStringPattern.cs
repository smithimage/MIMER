using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class QuotedStringPattern:IPattern
    {
        private const string m_TextPattern = "\x22(?:(?:(?:\x5C\x5C{2})+|\x5C\x5C[^\x5C\x5C]|[^\x5C\x5C\x22])*)\x22";
        private readonly Regex m_Regex;

        public QuotedStringPattern()
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