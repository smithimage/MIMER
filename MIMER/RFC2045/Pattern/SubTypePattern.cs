
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class SubTypePattern:ICompiledPattern
    {
        private string m_TextPattern;
        private Regex m_Regex;
        private IPattern m_MultipartSubTypePattern;
        private IPattern m_TextSubTypePattern;
        private IPattern m_ImageSubTypePattern;
        private IPattern m_ApplicationSubTypePattern;
        private IPattern m_MessageSubTypePattern;
        private IPattern m_AudioSubTypePattern;

        public SubTypePattern()
        {
            m_MultipartSubTypePattern = PatternFactory.GetInstance().Get(typeof (MultipartSubTypePattern));
            m_TextSubTypePattern = PatternFactory.GetInstance().Get(typeof (TextSubTypePattern));
            m_ImageSubTypePattern = PatternFactory.GetInstance().Get(typeof (ImageSubTypePattern));
            ApplicationSubTypePattern = PatternFactory.GetInstance().Get(typeof (ApplicationSubTypePatern));
            m_MessageSubTypePattern = PatternFactory.GetInstance().Get(typeof (MessageSubTypePattern));
            m_AudioSubTypePattern = PatternFactory.GetInstance().Get(typeof (AudioSubTypePattern));
            Compile();
        }

        public string TextPattern
        {
            get { return m_TextPattern; }
        }

        public Regex RegularExpression
        {
            get { return m_Regex; }
        }

        public IPattern ApplicationSubTypePattern
        {
            get { return m_ApplicationSubTypePattern; }
            set { m_ApplicationSubTypePattern = value; }
        }

        public void Compile()
        {
            m_TextPattern = "(((?<=multipart/)" + m_MultipartSubTypePattern.TextPattern + ")|(" +
                           "(?<=text/)" + m_TextSubTypePattern.TextPattern + ")|(" + "(?<=image/)" +
                           m_ImageSubTypePattern.TextPattern + ")|(" + "(?<=application/)" +
                           ApplicationSubTypePattern.TextPattern
                           + ")|(" + "(?<=message/)" + m_MessageSubTypePattern.TextPattern + ")|(" + "(?<=audio/)" +
                           m_AudioSubTypePattern.TextPattern + "))";
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
