
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class DiscreteTypePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DiscreteTypePattern()
        {
            m_TextPattern = "(text|image|audio|video|application)";
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
