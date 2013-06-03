using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class LocalPartPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public LocalPartPattern()
        {
            IPattern wordPattern = PatternFactory.GetInstance().Get(typeof(WordPattern));
            m_TextPattern = "(" + wordPattern.TextPattern + "(?:\x5C\x2E" + 
                            wordPattern.TextPattern + ")*)";
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