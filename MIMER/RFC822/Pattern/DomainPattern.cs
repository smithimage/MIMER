using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class DomainPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DomainPattern()
        {
            IPattern subDomainPattern = PatternFactory.GetInstance().Get(typeof (SubDomainPattern));
            m_TextPattern = "(" + subDomainPattern.TextPattern + "(?:\x5C\x2E" + subDomainPattern.TextPattern + ")*)";
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