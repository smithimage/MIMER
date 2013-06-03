
using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC822.Pattern
{
    public class AddrSpecPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public AddrSpecPattern()
        {
            IPattern localPartPattern = PatternFactory.GetInstance().Get(typeof (LocalPartPattern));
            IPattern domainPattern = PatternFactory.GetInstance().Get(typeof (DomainPattern));
            m_TextPattern = "(" + localPartPattern.TextPattern + "@" + domainPattern.TextPattern + ")";            
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