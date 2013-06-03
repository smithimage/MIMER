using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class ZonePattern:IPattern
    {
        private const string m_TextPattern = "(UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|1ALPHA|[+-]{1,1}[0-9]{4,4})";
        private readonly Regex m_Regex;

        public ZonePattern()
        {
            m_Regex = new Regex(m_TextPattern);
        }

        public Regex RegularExpression
        {
            get { return m_Regex ; }
        }

        public string TextPattern
        {
            get { return m_TextPattern; }
        }
    }   
    
}