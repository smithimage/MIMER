
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class TextSubTypePattern:IPattern
    {
        private const string m_TextPattern = "(calendar|plain|enriched|html|x-vcard)";
        private Regex m_Regex;

        public TextSubTypePattern()
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
