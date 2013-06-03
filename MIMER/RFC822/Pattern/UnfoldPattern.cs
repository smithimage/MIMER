using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class UnfoldPattern:IPattern
    {
        private const string m_TextPattern = "\x0D\x0A\x5Cs";//"|\x0A\x5Cs|\x2C{1,}\x5Cs*\x0A";
        private Regex m_Regex;

        public UnfoldPattern()
        {
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