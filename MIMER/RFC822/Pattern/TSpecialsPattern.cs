using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class TSpecialsPattern:IPattern
    {
        private const string m_TextPattern = "]\x5C[()<>@,;\x5C\x5C:\x22/?=";
        private Regex m_Regex;


        public TSpecialsPattern()
        {
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