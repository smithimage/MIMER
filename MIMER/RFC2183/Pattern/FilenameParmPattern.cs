
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class FilenameParmPattern:IPattern
    {
        private const string m_TextPattern = "filename=.*?(?=;)|filename=.*?(?= )|filename=.*?(?=\x0A)|filename=.*?(?=\x0D)";
        private readonly Regex m_Regex;

        public FilenameParmPattern()
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
