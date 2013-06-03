
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class ContentDispositionPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public ContentDispositionPattern()
        {
            IPattern dispositionTypePattern = PatternFactory.GetInstance().Get(typeof (DispositionTypePattern));
            IPattern dispostionParmPattern = PatternFactory.GetInstance().Get(typeof (DispositionParmPattern));

            m_TextPattern = "Content-Disposition:\x5C\x73" + dispositionTypePattern.TextPattern +
                            "(;\x5C\x73" + dispostionParmPattern.TextPattern + ")*";
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
