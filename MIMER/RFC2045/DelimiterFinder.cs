using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MIMER.RFC2045
{
    public class DelimiterFinder
    {
        private IPattern m_StartBoundaryPattern;
        private IPattern m_EndBoundaryPattern;

        public DelimiterFinder()
        {
            m_StartBoundaryPattern = PatternFactory.GetInstance().Get(typeof(Pattern.BoundaryStartDelimiterPattern));
            m_EndBoundaryPattern = PatternFactory.GetInstance().Get(typeof(Pattern.BoundaryEndDelimiterPattern));
        }

        public string FindDelimiter(ref IEntity entity, ref string line)
        {
            string boundary = string.Empty;
            if (m_EndBoundaryPattern.RegularExpression.IsMatch(line) ||
                m_StartBoundaryPattern.RegularExpression.IsMatch(line))
            {
                Match match;
                char[] cTrims = new char[] { '\\', '"' };
                if (IsDelimiter(ref entity, ref line))
                {
                    if (m_EndBoundaryPattern.RegularExpression.IsMatch(line))
                    {
                        match = m_EndBoundaryPattern.RegularExpression.Match(line);
                        boundary = match.Value.Trim();
                        boundary = boundary.Trim(cTrims);
                    }
                    else if (m_StartBoundaryPattern.RegularExpression.IsMatch(line))
                    {
                        match = m_StartBoundaryPattern.RegularExpression.Match(line);
                        boundary = match.Value.Trim();
                        boundary = boundary.Trim(cTrims);
                    }
                }
            }
            return boundary;
        }

        private bool IsDelimiter(ref IEntity entity, ref string line)
        {
            IEntity test = entity;
            while (test != null)
            {
                if (test.Delimiter != null && line.Contains(test.Delimiter))
                    return true;
                test = test.Parent;
            }
            return false;
        }
    }
}
