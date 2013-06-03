
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ExtensionTokenPattern:IPattern
    {
        private readonly IPattern m_XTokenPattern;

        public ExtensionTokenPattern()
        {
            m_XTokenPattern = PatternFactory.GetInstance().Get(typeof (XTokenPattern));
        }

        public string TextPattern
        {
            get { return m_XTokenPattern.TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_XTokenPattern.RegularExpression; }
        }
    }
}
