
using System.Text.RegularExpressions;

namespace MIMER.RFC2183.Pattern
{
    public class DispositionParmPattern:IPattern
    {
        private readonly string m_TextPattern;
        private Regex m_Regex;

        public DispositionParmPattern()
        {
            IPattern filenamePattern = PatternFactory.GetInstance().Get(typeof (FilenameParmPattern));
            IPattern creationDatePattern = PatternFactory.GetInstance().Get(typeof (CreationDateParmPattern));
            IPattern modificationDatePattern = PatternFactory.GetInstance().Get(typeof (ModificationDateParmPattern));
            IPattern readDatePattern = PatternFactory.GetInstance().Get(typeof (ReadDateParmPattern));
            IPattern sizePattern = PatternFactory.GetInstance().Get(typeof (SizeParmPattern));
            IPattern parameterPattern = PatternFactory.GetInstance().Get(typeof (RFC2045.Pattern.ParameterPattern));

            m_TextPattern = "(" + filenamePattern.TextPattern + "|" + creationDatePattern.TextPattern +
                "|" + modificationDatePattern.TextPattern + "|" + readDatePattern.TextPattern + "|" +
                sizePattern.TextPattern + "|" + parameterPattern.TextPattern + ")";
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
