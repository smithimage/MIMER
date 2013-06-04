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
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using MIMER.RFC2045;
using MIMER.RFC2183;
using MIMER.RFC822;

namespace MIMER.RFC2047
{
    public class ExtendedFieldParser:FieldParserDecorator
    {
        private ContentTransferEncodingFieldParser m_Original;
        private Encoding m_TargetEncoding;
        private RFC2045.QuotedPrintableDecoder m_QPDecoder;
        private RFC2045.Base64Decoder m_B64decoder;
        private IPattern m_CharsetPattern;
        private IPattern m_EncodingPattern;
        private IPattern m_EncodedTextPattern;
        private IPattern m_EncodedWordPattern;

        public Encoding TargetEncoding
        {
            get { return m_TargetEncoding; }
            set { m_TargetEncoding = value; }
        }

        public ContentTransferEncodingFieldParser Original
        {
            get { return m_Original; }
            set { m_Original = value; }
        }

        public ExtendedFieldParser(ContentTransferEncodingFieldParser original)
            : base(original)
        {
            Original = original;
            m_CharsetPattern = PatternFactory.GetInstance().Get(typeof (Pattern.CharsetPattern));
            m_EncodingPattern = PatternFactory.GetInstance().Get(typeof (Pattern.EncodingPattern));
            m_EncodedTextPattern = PatternFactory.GetInstance().Get(typeof (Pattern.EncodedTextPattern));
            m_EncodedWordPattern = PatternFactory.GetInstance().Get(typeof (Pattern.EncodedWordPattern));

            m_TargetEncoding = Encoding.UTF8;
            m_QPDecoder = new MIMER.RFC2045.QuotedPrintableDecoder();
            m_B64decoder = new MIMER.RFC2045.Base64Decoder();
        }

        #region IFieldParser Members

        public override void Parse(ref IList<MIMER.RFC822.Field> fields, ref string fieldString)
        {   
            CompilePattern();

            DecoratedFieldParser.Parse(ref fields, ref fieldString);

            foreach (RFC822.Field field in fields)
            {
                if (m_EncodedWordPattern.RegularExpression.IsMatch(field.Body))
                {
                    string charset = m_CharsetPattern.RegularExpression.Match(field.Body).Value;
                    string text = m_EncodedTextPattern.RegularExpression.Match(field.Body).Value;
                    string encoding = m_EncodingPattern.RegularExpression.Match(field.Body).Value;

                    Encoding enc = Encoding.GetEncoding(charset);

                    byte[] bytes;

                    if (m_QPDecoder.CanDecode(encoding))
                    {
                        bytes = m_QPDecoder.Decode(ref text, charset);
                    }
                    else
                    {
                        bytes = m_B64decoder.Decode(ref text);
                    }                    
                    text = enc.GetString(bytes);

                    field.Body = Regex.Replace(field.Body,
                        m_EncodedWordPattern.TextPattern, text);
                    field.Body = field.Body.Replace('_', ' ');

                }
            }
      
        }

        public override void CompilePattern()
        {
            DecoratedFieldParser.CompilePattern();
        }
        #endregion
    }
}
