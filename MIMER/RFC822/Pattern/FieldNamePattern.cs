using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class FieldNamePattern:IPattern
    {
        private const string m_TextPattern = "[^\x00-\x20\x7F:]{1,}(?=:)";
        private Regex m_Regex;

        public FieldNamePattern()
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