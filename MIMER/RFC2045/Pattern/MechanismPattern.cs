
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class MechanismPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public MechanismPattern()
        {
            IPattern xtokenPattern = PatternFactory.GetInstance().Get(typeof (XTokenPattern));
            m_TextPattern = "(?i)(7bit|8bit|binary|quoted-printable|base64|" +
                xtokenPattern.TextPattern + ")(?i)"; //or ief-token
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
