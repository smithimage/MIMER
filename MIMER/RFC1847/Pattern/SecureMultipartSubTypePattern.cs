using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MIMER.RFC2045.Pattern;

namespace MIMER.RFC1847.Pattern
{
    public class SecureMultipartSubTypePattern: PatternDecorator
    {
        private MultipartSubTypePattern m_Original;

        public SecureMultipartSubTypePattern(MultipartSubTypePattern decoratedPattern):base(decoratedPattern)
        {
            m_Original = decoratedPattern;
            m_Original.SubTypes.Add("signed");
            m_Original.SubTypes.Add("encrypted");
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
