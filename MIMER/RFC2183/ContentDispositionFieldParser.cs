/*
Copyright (c) 2007, Robert Wallstrm, smithimage.com
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:
	
	* Redistributions of source code must retain the above copyright notice, 
		this list of conditions and the following disclaimer. 
	* Redistributions in binary form must reproduce the above copyright notice, 
		this list of conditions and the following disclaimer in the documentation and/or 
		other materials provided with the distribution. 
	* Neither the name of the SMITHIMAGE nor the names of its contributors may be used to 
		endorse or promote products derived from this software 
		without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY 
OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH  DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MIMER.RFC2045;
using MIMER.RFC2047;

namespace MIMER.RFC2183
{
    public class ContentDispositionFieldParser: FieldParserDecorator
    {
        private ExtendedFieldParser m_Original;
        protected List<string> m_DispositionTypes;

        private IPattern m_DispositionPattern;
        private IPattern m_TokenPattern;
        private IPattern m_DispositionTypePattern;
        private IPattern m_ValuePattern;

        public ContentDispositionFieldParser(ExtendedFieldParser original)
            : base(original)
        {
            Original = original;
            m_DispositionTypes = new List<string>();            

            m_DispositionPattern = PatternFactory.GetInstance().Get(typeof (Pattern.DispositionParmPattern));
            m_TokenPattern = PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.TokenPattern));
            m_DispositionTypePattern = PatternFactory.GetInstance().Get(typeof (Pattern.DispositionTypePattern));
            m_ValuePattern = PatternFactory.GetInstance().Get(typeof (RFC2045.Pattern.ValuePattern));
        }

        public ExtendedFieldParser Original
        {
            get { return m_Original; }
            set { m_Original = value; }
        }

        #region IFieldParser Members

        public override void Parse(ref IList<MIMER.RFC822.Field> fields, ref string fieldString)
        {
            if (fields.Count == 0)
            {
                DecoratedFieldParser.Parse(ref fields, ref fieldString);
            }

            IList<RFC822.Field> tmpFields = new List<RFC822.Field>();

            foreach(RFC822.Field field in fields)
            {
                if(field.Name.Equals("Content-Disposition"))
                {
                    Match typeMatch, tmpMatch;
                    string key, val;
                    
                    typeMatch = m_DispositionTypePattern.RegularExpression.Match(field.Body);
                    MatchCollection parameterMatches = m_DispositionPattern.RegularExpression.Matches(field.Body);

                    ContentDispositionField dispositionField = new ContentDispositionField();
                    dispositionField.Name = field.Name;
                    dispositionField.Body = field.Body;
                    dispositionField.Disposition = typeMatch.Value;

                    foreach(Match parameterMatch in parameterMatches)
                    {
                        tmpMatch = Regex.Match(parameterMatch.Value, m_TokenPattern.TextPattern + "=");
                        key = tmpMatch.Value.TrimEnd(new char[] { '=' });
                        tmpMatch = Regex.Match(parameterMatch.Value, "(?<==)" + m_ValuePattern.TextPattern);                    
                        val = tmpMatch.Value.Trim(new char[] { '\\', '"' });
                        dispositionField.Parameters.Add(key, val);
                    }
                    tmpFields.Add(dispositionField);
                }
            }

            foreach (RFC822.Field field in tmpFields)
            {
                fields.Add(field);
            }         
        }

        public override void CompilePattern()
        {
            DecoratedFieldParser.CompilePattern();
        }
      
        #endregion
    }
}
