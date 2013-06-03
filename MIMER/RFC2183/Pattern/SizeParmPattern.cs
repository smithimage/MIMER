
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class SizeParmPattern:IPattern
    {
        private const string m_TextPattern = "size=[0-9]{1,1}";
        private readonly Regex m_Regex;

        public SizeParmPattern()
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
