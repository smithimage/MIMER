
using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class DisplayNamePattern:IPattern
    {
        private readonly IPattern m_PhrasePattern;

        public DisplayNamePattern()
        {
            m_PhrasePattern = PatternFactory.GetInstance().Get(typeof (PhrasePattern));
        }

        public string TextPattern
        {
            get { return m_PhrasePattern.TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_PhrasePattern.RegularExpression; }
        }
    }
}
