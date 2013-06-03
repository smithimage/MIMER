
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class CompositeTypePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public CompositeTypePattern()
        {
            m_TextPattern = "(message|multipart)";
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled |RegexOptions.IgnoreCase);
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
