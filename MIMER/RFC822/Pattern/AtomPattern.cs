using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class AtomPattern:IPattern
    {
        private const string m_TextPattern = "[^][()<>@,;:.\x5C\x5C\x22\x00-\x20\x7F]+";
        private Regex m_Regex;

        public AtomPattern()
        {
            m_Regex = new Regex(m_TextPattern);
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