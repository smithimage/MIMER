
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class AudioSubTypePattern:IPattern
    {
        private const string m_TextPattern = "(mpeg)";
        private readonly Regex m_Regex;

        public AudioSubTypePattern()
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
