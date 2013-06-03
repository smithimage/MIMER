using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class QuotedPairPattern:IPattern
    {
        private const string m_TextPattern = "\x5C\x5C[\x00-\x7F]";
        private Regex m_Regex;

        public QuotedPairPattern()
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