using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class FieldPattern:IPattern
    {
        private const string m_TextPattern = "^[^\x00-\x20\x7F:]{1,}:{1,1}.+";
        private readonly Regex m_Regex;

        public FieldPattern()
        {
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled|RegexOptions.Multiline);
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