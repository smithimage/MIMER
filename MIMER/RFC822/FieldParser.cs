/*
Copyright (c) 2007, Robert Wallström, smithimage.com
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	
	* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
	* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
	* Neither the name of the SMITHIMAGE nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH  DAMAGE.
*/

using System.Collections.Generic;
using MIMER.RFC822.Pattern;

using System.Text.RegularExpressions;

namespace MIMER.RFC822
{
    public class FieldParser:IFieldParser
    {   
        protected IPattern m_UnfoldPattern;      
        protected IPattern m_FieldPattern;
        protected IPattern m_HeaderNamePattern;
        protected IPattern m_HeaderBodyPattern;
       
       
        public FieldParser()
        {
            m_UnfoldPattern = PatternFactory.GetInstance().Get(typeof (UnfoldPattern));
            m_FieldPattern = PatternFactory.GetInstance().Get(typeof (FieldPattern));
            m_HeaderNamePattern = PatternFactory.GetInstance().Get(typeof (FieldNamePattern));
            m_HeaderBodyPattern = PatternFactory.GetInstance().Get(typeof (FieldBodyPattern));
        }

        public virtual void CompilePattern()
        {
            
        }
       
        #region IFieldParser Members

        public virtual void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {   
            MatchCollection matches = m_FieldPattern.RegularExpression.Matches(fieldString);
            foreach (Match m in matches)
            {
                RFC822.Field f = new RFC822.Field();
                Match tmp = m_HeaderNamePattern.RegularExpression.Match(m.Value);
                f.Name = tmp.Value;
                tmp = m_HeaderBodyPattern.RegularExpression.Match(m.Value);
                f.Body = tmp.Value;          
                fields.Add(f);
            }            
        }

        #endregion
    }
}
