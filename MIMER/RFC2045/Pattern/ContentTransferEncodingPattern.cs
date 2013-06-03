
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ContentTransferEncodingPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public ContentTransferEncodingPattern()
        {
            IPattern mechanismPattern = PatternFactory.GetInstance().Get(typeof (MechanismPattern));
            m_TextPattern = "(?i)Content-Transfer-Encoding(?i):[ ]*" + mechanismPattern.TextPattern;
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
