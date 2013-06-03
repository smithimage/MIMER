
using System.Text.RegularExpressions;

namespace MIMER.RFC2047.Pattern
{
    public class EncodedTextPattern:IPattern
    {
        private const string m_TextPattern = "(?<=\x5C?)[^\x3F\x20]+(?=\x5C?=)";
        private readonly Regex m_Regex;

        public EncodedTextPattern()
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
