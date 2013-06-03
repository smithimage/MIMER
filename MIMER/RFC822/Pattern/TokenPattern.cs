using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class TokenPattern:IPattern
    {
        private readonly IPattern m_Pattern;
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public TokenPattern()
        {
            m_Pattern = PatternFactory.GetInstance().Get(typeof (TSpecialsPattern));
            m_TextPattern = "[^" + m_Pattern.TextPattern + "\x00-\x20]+";
            m_Regex = new Regex(m_TextPattern);
        }
        public string TextPattern
        {
            get { return m_TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_Regex;}
        }
    }
}