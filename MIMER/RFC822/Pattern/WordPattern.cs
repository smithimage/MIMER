using System.Text.RegularExpressions;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC822.Pattern
{
    public class WordPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public WordPattern()
        {
            IPattern atomPattern = PatternFactory.GetInstance().Get(typeof(AtomPattern));
            IPattern quotedStringPattern = PatternFactory.GetInstance().Get(typeof(QuotedStringPattern));
            m_TextPattern = "(" + atomPattern.TextPattern + "|" + quotedStringPattern.TextPattern + ")";
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