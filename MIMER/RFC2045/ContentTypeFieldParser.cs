/*
Copyright (c) 2007, Robert Wallström, smithimage.com
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	
	* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
	* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
	* Neither the name of the SMITHIMAGE nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH  DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Text.RegularExpressions;
using MIMER.RFC822;

namespace MIMER.RFC2045
{
    public class ContentTypeFieldParser:FieldParserDecorator
    {
        private readonly FieldParser m_Original;
       
        protected IList<string> m_DiscreteTypes;
        protected IList<string> m_CompositeTypes;
        protected IList<string> m_MultipartSubTypes;
        protected IList<string> m_TextSubtypes;
        protected IList<string> m_ImageSubTypes;
        private IList<string> m_ApplicationSubtypes;
        protected IList<string> m_MessageSubtypes;
        protected IList<string> m_AudioSubtypes;

        private bool m_StrictMatch = false;
        private IPattern m_TokenPattern;
        private IPattern m_ContentPattern;
        private IPattern m_TypePattern;
        private IPattern m_SubTypePattern;
        private IPattern m_ParameterPattern;
        private IPattern m_ValuePattern;

        public bool StrictMatch
        {
            get { return m_StrictMatch; }
            set { m_StrictMatch = value; }
        }

        public FieldParser Original
        {
            get { return m_Original; }
        }

        public IPattern SubTypePattern
        {
            get { return m_SubTypePattern; }
            set { m_SubTypePattern = value; }
        }
        
        public ContentTypeFieldParser(FieldParser original):base(original) 
        {
            m_TokenPattern = PatternFactory.GetInstance().Get(typeof(RFC822.Pattern.TokenPattern));
            m_ContentPattern = PatternFactory.GetInstance().Get(typeof (Pattern.ContentTypePattern));
            m_TypePattern = PatternFactory.GetInstance().Get(typeof (Pattern.TypePattern));
            SubTypePattern = PatternFactory.GetInstance().Get(typeof (Pattern.SubTypePattern));
            m_ParameterPattern = PatternFactory.GetInstance().Get(typeof (Pattern.ParameterPattern));
            m_ValuePattern = PatternFactory.GetInstance().Get(typeof (Pattern.ValuePattern));
        }

        /// <summary>
        /// Compiles the underlying Regex object which performes matches
        /// when parsing fields.
        /// </summary>
        public override void CompilePattern()
        {  
            DecoratedFieldParser.CompilePattern();
        }

        /// <summary>
        /// Parses the any content type fields found within argument fieldstring.
        /// </summary>
        /// <param name="fields">The target list for the parsed fields.</param>
        /// <param name="fieldString">The source field string</param>
        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {
            CompilePattern();

            if (fields.Count == 0)
                DecoratedFieldParser.Parse(ref fields, ref fieldString);

            MatchCollection matches = m_ContentPattern.RegularExpression.Matches(fieldString);

            foreach (Match match in matches)
            {
                ContentTypeField tmpContent = new ContentTypeField();
                tmpContent.Name = "Content-Type";
                MatchCollection parameters;
                string key, val;

                Match tmpMatch = Regex.Match(match.Value, ":.+");

                tmpContent.Body = tmpMatch.Value.TrimStart(new char[] { ':' });
                tmpMatch = m_TypePattern.RegularExpression.Match(match.Value);
                tmpContent.Type = tmpMatch.Value;
                tmpMatch = SubTypePattern.RegularExpression.Match(match.Value);
                tmpContent.SubType = tmpMatch.Value;
                parameters = m_ParameterPattern.RegularExpression.Matches(match.Value);
                foreach (Match m in parameters)
                {
                    tmpMatch = Regex.Match(m.Value, m_TokenPattern.TextPattern + "=");
                    key = tmpMatch.Value.TrimEnd(new char[] { '=' });
                    tmpMatch = Regex.Match(m.Value, "(?<==)" + m_ValuePattern.TextPattern);                    
                    val = tmpMatch.Value.Trim(new char[] { '\\', '"' });
                    AddParamerter(tmpContent.Parameters, key, val);
                }   

                fields.Add(tmpContent);
            }
        }

        private void AddParamerter(StringDictionary parameters, string key, string val)
        {
            if (!parameters.ContainsKey(key))
            {
                parameters.Add(key, val);
            }
            else if (parameters.ContainsKey(key) && !parameters[key].Equals(val))
            {
                string tmpKey = key;
                int count = 0;
                while (parameters.ContainsKey(tmpKey))
                {
                    tmpKey = string.Format("{0}-{1}", key, count);
                    count++;
                }
                key = tmpKey;
                parameters.Add(key, val);
            }
        }
    }
}
