using System.Text.RegularExpressions;


namespace MIMER.RFC822.Pattern
{
    public class DomainLiteralPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public DomainLiteralPattern()
        {
            IPattern dtextPattern = PatternFactory.GetInstance().Get(typeof(DtextPattern));
            IPattern quotedPairPattern = PatternFactory.GetInstance().Get(typeof(QuotedPairPattern));
            m_TextPattern = "(\x5C\x5B(?:" + dtextPattern.TextPattern + 
                            "|" + quotedPairPattern.TextPattern + ")*\x5C\x5D)";
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