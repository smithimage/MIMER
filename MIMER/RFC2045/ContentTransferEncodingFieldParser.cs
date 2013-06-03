/*
Copyright (c) 2007, Robert Wallström, smithimage.com
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	
	* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
	* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
	* Neither the name of the SMITHIMAGE nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH  DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace MIMER.RFC2045
{
    public class ContentTransferEncodingFieldParser:FieldParserDecorator
    {
        private ContentTypeFieldParser m_Original;
        private readonly IPattern m_MechanismPattern;
        private readonly IPattern m_ContentTransferPattern;

        public ContentTransferEncodingFieldParser(ContentTypeFieldParser original):base(original)
        {
            Original = original;
            m_MechanismPattern = PatternFactory.GetInstance().Get(typeof (Pattern.MechanismPattern));
            m_ContentTransferPattern = PatternFactory.GetInstance().Get(typeof (Pattern.ContentTransferEncodingPattern));
        }

        public ContentTypeFieldParser Original
        {
            get { return m_Original; }
            set { m_Original = value; }
        }

        public override void CompilePattern()
        {            
            DecoratedFieldParser.CompilePattern();
        }

        #region IFieldParser Members

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {            
            DecoratedFieldParser.Parse(ref fields, ref fieldString);
            MatchCollection matches = m_ContentTransferPattern.RegularExpression.Matches(fieldString);
            foreach (Match match in matches)
            {
                Match enc;
                ContentTransferEncodingField tmpTransfer = new ContentTransferEncodingField();
                enc = m_MechanismPattern.RegularExpression.Match(match.Value);
                tmpTransfer.Name = "Content-Transfer-Encoding";
                tmpTransfer.Encoding = enc.Value;
                fields.Add(tmpTransfer);
            }         
        }

        #endregion
    }
}
