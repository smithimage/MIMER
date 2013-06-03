
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class XTokenPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public XTokenPattern()
        {
            IPattern tokenPattern = PatternFactory.GetInstance().Get(typeof(MIMER.RFC822.Pattern.TokenPattern));
            m_TextPattern = "(X-|x-)" + tokenPattern.TextPattern;
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
