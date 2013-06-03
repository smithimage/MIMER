
using System.Text.RegularExpressions;

namespace MIMER.RFC2047.Pattern
{
    public class EncodingPattern:IPattern
    {
        private const string m_TextPattern = "(?i)(?<=\x5C?)(Q|B)(?=\x5C?)";
        private readonly Regex m_Regex;

        public EncodingPattern()
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
