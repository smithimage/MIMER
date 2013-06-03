
using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class PhrasePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public PhrasePattern()
        {
            IPattern wordPattern = PatternFactory.GetInstance().Get(typeof (WordPattern));
            m_TextPattern = wordPattern.TextPattern;
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
