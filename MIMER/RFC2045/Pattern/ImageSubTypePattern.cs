
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ImageSubTypePattern:IPattern
    {
        private const string m_TextPattern = "(jpeg|gif|bmp|png|tiff|pjpeg)";
        private readonly Regex m_Regex;

        public ImageSubTypePattern()
        {
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled);
        }

        public string TextPattern
        {
            get {return m_TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_Regex; }
        }
    }
}
