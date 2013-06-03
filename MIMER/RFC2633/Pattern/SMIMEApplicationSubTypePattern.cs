using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MIMER.RFC2045.Pattern;

namespace MIMER.RFC2633.Pattern
{
    public class SMIMEApplicationSubTypePattern:PatternDecorator
    {
        private ApplicationSubTypePatern m_Original;

        public SMIMEApplicationSubTypePattern(ApplicationSubTypePatern decoratedPattern):base(decoratedPattern)
        {
            m_Original = decoratedPattern;
            m_Original.SubTypes.Add("pkcs7-mime");
            m_Original.SubTypes.Add("pkcs7-signature");
            m_Original.SubTypes.Add("x-pkcs7-mime");
            Compile();
        }

        public override string TextPattern
        {
            get { return m_Original.TextPattern; }
        }

        public override Regex RegularExpression
        {
            get { return m_Original.RegularExpression; }
        }

        public override void Compile()
        {
            DecoratedPattern.Compile();
        }
    }
}
