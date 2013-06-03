
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class ContentTypePattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public ContentTypePattern()
        {
            IPattern typePattern = PatternFactory.GetInstance().Get(typeof (TypePattern));
            IPattern subtypePattern = PatternFactory.GetInstance().Get(typeof (SubTypePattern));
            IPattern extensionTokenPattern = PatternFactory.GetInstance().Get(typeof (ExtensionTokenPattern));
            IPattern tokenPattern = PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.TokenPattern));
            IPattern parameterPattern = PatternFactory.GetInstance().Get(typeof (ParameterPattern));

            m_TextPattern = string.Format("(?i)Content-Type(?i):[\x5C\x73]{{1,1}}{0}/{1}({2}|{3})*(;[\x5Cs]?{4})*",
                                     typePattern.TextPattern,
                                     subtypePattern.TextPattern, extensionTokenPattern.TextPattern,
                                     tokenPattern.TextPattern, parameterPattern.TextPattern);

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
