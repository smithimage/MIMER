
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class TypePattern:IPattern
    {
        private readonly string m_TextPattern;
        private Regex m_Regex;

        public TypePattern()
        {
            IPattern descritePattern = PatternFactory.GetInstance().Get(typeof (DiscreteTypePattern));
            IPattern compositePattern = PatternFactory.GetInstance().Get(typeof (CompositeTypePattern));
            m_TextPattern = "(" + descritePattern.TextPattern + "|" + compositePattern.TextPattern + ")";
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
