using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class FieldBodyPattern:IPattern
    {
        private const string m_TextPattern = "(?<=[^\x00-\x20\x7F:]{1,}: )[^\xA\xD]+";
        private Regex m_Regex;

        public FieldBodyPattern()
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