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
        protected IList<IEndCriteriaStrategy> m_Criterias;
        protected IEndCriteriaStrategy m_EndOfMessageStrategy;
        protected IEndCriteriaStrategy m_EndOfLineStrategy;
        protected IEndCriteriaStrategy m_NullLineStrategy;
        protected StringBuilder m_Source;
        protected long m_BytesRead;

        private long m_UpdateInterval = 1;
        private IPattern m_UnfoldPattern;

        public MailReader()
        {
            m_FieldParser = new FieldParser();
            m_FieldParser.CompilePattern();
            m_Criterias = new List<IEndCriteriaStrategy>();
            m_EndOfLineStrategy = new EndOfLineStrategy();
            m_NullLineStrategy = new NullLineStrategy();

            m_UnfoldPattern = PatternFactory.GetInstance().Get(typeof (Pattern.UnfoldPattern));
        }

        #region IMailReader Members

        public event DataReadEventHandler DataRead = null;

        public IMailMessage Read(ref System.IO.Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            m_BytesRead = 0;
            m_EndOfMessageStrategy = endOfMessageCriteria;
            Message m = new Message();
            IMailMessage im = m as IMailMessage;            
            m_Source = new StringBuilder();
            IList<RFC822.Field> fields;
            int cause = ParseFields(ref dataStream, out fields);
            m.Fields = fields;

            if (cause >= 0)
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
            string headers;
            int cause = ReadHeaders(ref dataStream, out headers);
            m_Source.Append(headers);
            headers = m_UnfoldPattern.RegularExpression.Replace(headers, " ");
            fields = new List<RFC822.Field>();
            m_FieldParser.Parse(ref fields, ref headers);
            return cause;
        }

        protected int ReadHeaders(ref System.IO.Stream dataStream, out string sHeaders)
        {
            sHeaders = string.Empty;            
            char[] headers;
            int fulfilledCriteria;

            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
            m_Criterias.Add(m_NullLineStrategy);
            
            headers = ReadData(ref dataStream, m_Criterias, out fulfilledCriteria);                                    
            sHeaders = new string(headers);
            return fulfilledCriteria;
        }      

        protected void ReadBody(ref Stream dataStream, ref IMailMessage message)
        {            
            char[] buffer;            
            int fulFilledCriteria;

            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
           
            buffer = ReadData(ref dataStream, m_Criterias, out fulFilledCriteria);
            string body = new string(buffer);
            m_Source.Append(body);
            message.TextMessage = body;
        }                

        protected char[] ReadData(ref Stream dataStream, IList<IEndCriteriaStrategy> criterias, out int fulfilledCritera)
        {
            fulfilledCritera = -1;
            int size, pos, c;
            char[] buffer, data;

            size = 1;
            pos = 0;
            buffer = new char[size];

            while ((c = dataStream.ReadByte()) != -1)
            {
                m_BytesRead++;
                if ((m_BytesRead % m_UpdateInterval) == 0 && pos > 0)
                {
                    DataReadEventArgs args = new DataReadEventArgs();
                    args.AmountRead = m_BytesRead;
                    OnDataRead(this, args);
                }

                if (pos >= (size - 1))
                {
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

                if (pos > 0)
                {
                    int i = 0;
                    foreach (IEndCriteriaStrategy criteria in criterias)
                    {
                        if (criteria.IsEndReached(buffer, pos))
                        {
                            fulfilledCritera = i;
                            break;
                        }
                        i++;
                    }
                }

                if (fulfilledCritera > -1)
                {
                    break;
                }
                pos++;
            }

            data = new char[pos + 1];
            Array.Copy(buffer, data, pos + 1);
            buffer = null;
            return data;
        }

        protected void OnDataRead(object sender, DataReadEventArgs args)
        {
            if (DataRead != null)
            {
                DataRead(sender, args);
            }
        }
        
    }
}
