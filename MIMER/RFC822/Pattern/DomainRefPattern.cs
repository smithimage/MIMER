using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC822.Pattern
{
    public class DomainRefPattern:IPattern
    {
        private IPattern m_AtomPattern;

        public DomainRefPattern()
        {
            m_AtomPattern = PatternFactory.GetInstance().Get(typeof (AtomPattern));
        }
        public string TextPattern
        {
            get { return m_AtomPattern.TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_AtomPattern.RegularExpression; }
        }
    }
}