
using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class SpecialsPattern:IPattern
    {
        private const string m_TextPattern = "()<>@,;:\x5C\x5C\x22.]\x5C[";
        private readonly Regex m_Regex;

        public SpecialsPattern()
        {
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
