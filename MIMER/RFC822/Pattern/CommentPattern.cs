using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMER.RFC822.Pattern
{
    public class CommentPattern:IPattern
    {
        private readonly string m_TextPattern;
        private readonly Regex m_Regex;

        public CommentPattern()
        {
            IPattern ctextPattern = PatternFactory.GetInstance().Get(typeof (CtextPattern));
            IPattern quotedPairPattern = PatternFactory.GetInstance().Get(typeof (QuotedPairPattern));
            m_TextPattern = "(?:[(]{1,1} *(" + ctextPattern.TextPattern +
                            "|" + quotedPairPattern.TextPattern + ")[)]{1,1})";
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