
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class MessageSubTypePattern:IPattern
    {
        private const string m_TextPattern = "(rfc822|partial|external-body)";
        private readonly Regex m_Regex;

        public MessageSubTypePattern()
        {
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
