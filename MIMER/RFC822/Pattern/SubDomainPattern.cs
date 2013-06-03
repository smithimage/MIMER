using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;


namespace MIMER.RFC822.Pattern
{
    public class SubDomainPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public SubDomainPattern()
        {
            IPattern domainRefPattern = PatternFactory.GetInstance().Get(typeof (DomainRefPattern));
            IPattern domainLiteralPattern = PatternFactory.GetInstance().Get(typeof(DomainLiteralPattern));
            m_TextPattern =  "(?:(" + domainRefPattern.TextPattern + "|" + domainLiteralPattern.TextPattern + "))";
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