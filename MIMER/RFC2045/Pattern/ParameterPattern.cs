
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ParameterPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public ParameterPattern()
        {
            IPattern tokenPattern = PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.TokenPattern));
            IPattern valuePattern = PatternFactory.GetInstance().Get(typeof (ValuePattern));
            m_TextPattern = tokenPattern.TextPattern + "=" + valuePattern.TextPattern;
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
