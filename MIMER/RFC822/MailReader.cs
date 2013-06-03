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

using System.IO;
using System.Text.RegularExpressions;

namespace MIMER.RFC822
{
    public class MailReader:IMailReader
    {
        protected FieldParser m_FieldParser;
        protected StringBuilder m_Source;
        protected long m_BytesRead;

        private long m_UpdateInterval = 1;
        private IPattern m_UnfoldPattern;
        protected IEndCriteriaStrategy m_EndOfMessageCriteria;

        public MailReader()
        {
            m_FieldParser = new FieldParser();
            m_FieldParser.CompilePattern();

            m_UnfoldPattern = PatternFactory.GetInstance().Get(typeof (Pattern.UnfoldPattern));
        }

        #region IMailReader Members

        public event DataReadEventHandler DataRead = null;

        public IMailMessage Read(ref System.IO.Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            m_EndOfMessageCriteria = endOfMessageCriteria;
            m_BytesRead = 0;

            Message m = new Message();
            IMailMessage im = m as IMailMessage;            
            m_Source = new StringBuilder();
            var result = m_FieldParser.ParseFields(ref dataStream, new DataReader(endOfMessageCriteria));
            m.Fields = result.Data;

            if (result.FulfilledCritera >= 0)
            {
                ReadBody(ref dataStream, ref im);
            }
            m.Source = m_Source.ToString();
            return m;            
        }

        public long DataReadUpdateInterval
        {
            get
            {
                return m_UpdateInterval;
            }
            set
            {
                m_UpdateInterval = value;
            }
        }

        #endregion             

        protected int ParseFields(ref Stream dataStream, out IList<MIMER.RFC822.Field> fields)
        {
            var result = ReadHeaders(ref dataStream);
            var headers = new string(result.Data);
            m_Source.Append(headers);
            headers = m_UnfoldPattern.RegularExpression.Replace(headers, " ");
            fields = new List<RFC822.Field>();
            m_FieldParser.Parse(ref fields, ref headers);
            return result.FulfilledCritera;
        }

        protected DataReader.Result ReadHeaders(ref Stream dataStream)
        {
            var dataReader = new DataReader(m_EndOfMessageCriteria, new NullLineStrategy());
            return dataReader.ReadData(ref dataStream);                                    
        }      

        protected void ReadBody(ref Stream dataStream, ref IMailMessage message)
        {           
            var result = new DataReader(m_EndOfMessageCriteria).ReadData(ref dataStream);
            string body = new string(result.Data);
            m_Source.Append(body);
            message.TextMessage = body;
        }                
                
    }
}
