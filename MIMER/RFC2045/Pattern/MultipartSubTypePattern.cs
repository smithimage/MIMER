
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MIMER.RFC2045.Pattern
{
    public class MultipartSubTypePattern:ICompiledPattern
    {
        private string m_TextPattern;
        private IList<string> m_Types;
        private Regex m_Regex;

        public MultipartSubTypePattern()
        {
            SubTypes = new List<string>();
            SubTypes.Add("mixed");
            SubTypes.Add("alternative");
            SubTypes.Add("parallel");
            SubTypes.Add("digest");
            SubTypes.Add("related");
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

        public IList<string> SubTypes
        {
            get { return m_Types; }
            set { m_Types = value; }
        }

        public void Compile()
        {
            m_TextPattern = "(";
            int count = 0;
            foreach (var type in SubTypes)
            {
                m_TextPattern += type;
                if (count < SubTypes.Count)
                    m_TextPattern += "|";
                count++;
            }
            m_TextPattern += ")";
            m_Regex = new Regex(m_TextPattern, RegexOptions.Compiled);
        }
    }
}
