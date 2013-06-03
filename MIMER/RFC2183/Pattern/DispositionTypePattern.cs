
using System.Text.RegularExpressions;
using MIMER.RFC2045.Pattern;

namespace MIMER.RFC2183.Pattern
{
    public class DispositionTypePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DispositionTypePattern()
        {
            IPattern xtokenPattern = PatternFactory.GetInstance().Get(typeof (XTokenPattern));
            m_TextPattern = "(?i)(inline|Attachment|" + xtokenPattern.TextPattern + ")(?i)";
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