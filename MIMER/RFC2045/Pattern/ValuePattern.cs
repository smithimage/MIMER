
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ValuePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public ValuePattern()
        {
            IPattern tokenPattern = PatternFactory.GetInstance().Get(typeof (MIMER.RFC822.Pattern.TokenPattern));
            IPattern qoutedStringPattern =
                PatternFactory.GetInstance().Get(typeof (MIMER.RFC822.Pattern.QuotedStringPattern));

            m_TextPattern = "(" + tokenPattern.TextPattern + "|" + qoutedStringPattern.TextPattern + ")";
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
