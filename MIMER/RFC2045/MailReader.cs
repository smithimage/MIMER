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
using System.Collections.Specialized;
using MIMER.RFC2047;
using MIMER.RFC2183;
using MIMER.RFC822;
using MIMER.RFC822.Pattern;

namespace MIMER.RFC2045
{
    public class MailReader : MIMER.RFC822.MailReader, MIMER.IMailReader
    {        
        private IList<RFC2045.IDecoder> m_Decoders;
        protected new IFieldParser m_FieldParser;
        private IPattern m_UnfoldPattern;
        private IPattern m_DiscretePattern;
        private IPattern m_CompositePattern;
        private IPattern m_MimeVersionPattern;
        private static DelimiterFinder m_delimiterFinder;


        public IList<RFC2045.IDecoder> Decoders
        {
            get { return m_Decoders; }
            set { m_Decoders = value; }
        }


        /// <summary>
        /// Creates a MIME competent MailReader, with QuotedPrintable &
        /// Base64 decoders.
        /// </summary>
        public MailReader()
        {   
            m_Decoders = new List<RFC2045.IDecoder>();
            m_Decoders.Add(new QuotedPrintableDecoder());
            m_Decoders.Add(new Base64Decoder());
            m_FieldParser =
                new RFC2633.SMIMEFieldParser(
                    new ContentDispositionFieldParser(
                        new ExtendedFieldParser(
                            new ContentTransferEncodingFieldParser(new ContentTypeFieldParser(new FieldParser())))));

            
            m_FieldParser.CompilePattern();

            m_UnfoldPattern = PatternFactory.GetInstance().Get(typeof (UnfoldPattern));
            m_DiscretePattern = PatternFactory.GetInstance().Get(typeof (Pattern.DiscreteTypePattern));
            m_CompositePattern = PatternFactory.GetInstance().Get(typeof (Pattern.CompositeTypePattern));
            m_MimeVersionPattern = PatternFactory.GetInstance().Get(typeof (Pattern.MIMEVersionPattern));
        }

        /// <summary>
        /// Creates a MIME competent MailReader, with QuotedPrintable &
        /// Base64 decoders. Adds parameter decoders after qoutdePrintable and Base64
        /// decoders in list.
        /// </summary>
        public MailReader(IList<IDecoder> decoders)
            : this()
        {
            foreach (IDecoder decoder in decoders)
                m_Decoders.Add(decoder);
        }

        /// <summary>
        /// Creates a MIME competent MailReader, with QuotedPrintable &
        /// Base64 decoders. Adds parameter decoders after qoutdePrintable and Base64
        /// decoders in list. 
        /// </summary>        
        public MailReader(IList<IDecoder> decoders, IFieldParser parser):this(decoders)
        {
            m_FieldParser = parser;            
            m_FieldParser.CompilePattern();
        }

        static MailReader()
        {
            m_delimiterFinder = new DelimiterFinder();
        }

        #region IMailReader Members

        /// <summary>
        /// Reads and parses a mail message from the supplied Stream.
        /// </summary>
        /// <param name="dataStream">The Stream to read from. </param>
        /// <param name="endOfMessageCriteria">The refereance to the object which can determine
        /// the end of a message.</param>
        /// <returns>A new IMailMessage.</returns>
        public new IMailMessage Read(ref System.IO.Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            return ReadMimeMessage(ref dataStream, endOfMessageCriteria) as IMailMessage;
        }      

       #endregion        

        /// <summary>
        /// Reads and parses a mail message from the supplied Stream.
        /// </summary>
        /// <param name="dataStream">The Stream to read from. </param>
        /// <param name="endOfMessageCriteria">The refereance to the object which can determine
        /// the end of a message.</param>
        /// <returns>A new IMimeMailMessage.</returns>
        public IMimeMailMessage ReadMimeMessage(ref System.IO.Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            m_DataReader = new DataReader();
            m_DataReader.Criterias.Add(endOfMessageCriteria);
            
            if (dataStream == null)
                throw new ArgumentNullException("dataStream");

            m_BytesRead = 0;
            m_Source = new StringBuilder();
            IList<MIMER.RFC822.Field> fields;
            int cause = ParseFields(ref dataStream, out fields);            

            if (fields.Count == 0)
                return new NullMessage();

            Message m = new Message();
            m.Fields = fields;

            if (cause < 0)
            {
                return m as IMimeMailMessage;
            }

            if (isMIME(ref fields))
            {                
                string delimiter = ParseMessage(ref dataStream, ref m, fields);                
            }
            else
            {
                IMailMessage im = m as IMailMessage;
                base.ReadBody(ref dataStream, ref im);             
            }

            m.Source = m_Source.ToString();
            m_Source = null;
            return m as IMimeMailMessage;
        }

        #region private methods

        private string ParseMessage(ref Stream dataStream, ref Message message, IList<RFC822.Field> fields)
        {            
            foreach (RFC822.Field contentField in fields)
            {
                if (contentField is ContentTypeField)
                {
                    ContentTypeField contentTypeField = contentField as ContentTypeField;

                    if (m_CompositePattern.RegularExpression.IsMatch(contentTypeField.Type))
                    {
                        IMultipartEntity e = message as IMultipartEntity;
                        e.Delimiter = ReadDelimiter(ref contentTypeField);                        
                        return ReadCompositeEntity(ref dataStream, ref e);                        
                    }
                    else if (m_DiscretePattern.RegularExpression.IsMatch(contentTypeField.Type))
                    {
                        IEntity e = message as IEntity;
                        message.BodyParts.Add(e);// This is a message witch body lies within its own entity                        
                        return ReadDiscreteEntity(ref dataStream, ref e);                        
                    }
                }
            }
            return string.Empty;
        }

        private string ReadCompositeEntity(ref Stream dataStream, ref IMultipartEntity parent)
        {            
            IEntity child;
            string delimiter = null;
            
            IEntity e = parent as IEntity;
            if (parent.BodyParts.Count < 1)
            {
                delimiter = FindStartDelimiter(ref dataStream, ref e);
                if (!delimiter.Equals("--" + parent.Delimiter))
                    return string.Empty;
            }

            delimiter = CreateEntity(ref dataStream, ref parent, out child);          
           
            if (child == null || delimiter == string.Empty)
            {
                return string.Empty;
            }

            if (child is MultipartEntity)
            {
                IMultipartEntity mChild = child as IMultipartEntity;
                delimiter = ReadCompositeEntity(ref dataStream, ref mChild);
            }
            else
                delimiter = ReadDiscreteEntity(ref dataStream, ref child);                

            // until we reach end of mail
            while (delimiter != string.Empty && parent != null)
            {
                if (parent.Delimiter == null)
                {
                    parent = parent.Parent;
                }
                else if (delimiter.Contains(parent.Delimiter))
                {
                    if (delimiter.Equals("--" + parent.Delimiter))
                    {
                        delimiter = ReadCompositeEntity(ref dataStream, ref parent);
                    }
                    else if (delimiter.Equals("--" + parent.Delimiter + "--"))
                    {
                        parent = parent.Parent;
                        IEntity ent = parent as IEntity;
                        delimiter = FindStartDelimiter(ref dataStream, ref ent);                        
                    }
                }                         
            }
            return delimiter;
        }

        private string ReadDiscreteEntity(ref Stream dataStream, ref IEntity entity)
        {
            string body;
            string delimiter;            

            body = string.Empty;
            delimiter = ReadDiscreteBody(ref dataStream,ref entity, ref body);

            string transferEncoding = string.Empty;            
            
            foreach (RFC822.Field field in entity.Fields)
            {                
                if (field is ContentTransferEncodingField)
                {
                    ContentTransferEncodingField transferField = field as ContentTransferEncodingField;
                    transferEncoding = transferField.Encoding;                    
                    break;
                }
            }            

            foreach (RFC2045.IDecoder decoder in m_Decoders)
            {
                if (decoder.CanDecode(transferEncoding))
                {
                    entity.Body = decoder.Decode(ref body);
                    break;
                }
            }

            return delimiter;
        }



        private string CreateEntity(ref Stream dataStream, ref IMultipartEntity parent, out IEntity entity)
        {
            entity = null;
            IList<RFC822.Field> fields;
            int cause = ParseFields(ref dataStream, out fields);
            if (cause >= 0)
            {
                foreach (RFC822.Field contentField in fields)
                {
                    if (contentField is ContentTypeField)
                    {
                        ContentTypeField contentTypeField = contentField as ContentTypeField;

                        if (m_CompositePattern.RegularExpression.IsMatch(contentTypeField.Type))
                        {
                            MultipartEntity mEntity = new MultipartEntity();                            
                            mEntity.Fields = fields;
                            entity = mEntity;
                            entity.Parent = parent;
                            parent.BodyParts.Add(entity);

                            if (Regex.IsMatch(contentTypeField.Type, "(?i)message") &&
                                Regex.IsMatch(contentTypeField.SubType, "(?i)rfc822"))
                            {
                                Message message = new Message();
                                IList<RFC822.Field> messageFields;
                                cause = ParseFields(ref dataStream, out messageFields);
                                message.Fields = messageFields;                                
                                mEntity.BodyParts.Add(message);
                                message.Parent = mEntity;
                                if(cause > 0)
                                    return ParseMessage(ref dataStream, ref message, messageFields);
                                break;
                            }
                            else
                            {
                                mEntity.Delimiter = ReadDelimiter(ref contentTypeField);
                                return parent.Delimiter;                                
                            }                           
                        }
                        else if (m_DiscretePattern.RegularExpression.IsMatch(contentTypeField.Type))
                        {
                            entity = new Entity();
                            entity.Fields = fields;
                            entity.Parent = parent;
                            parent.BodyParts.Add(entity);
                            return parent.Delimiter;                                   
                        }
                    }
                }
            }
            return string.Empty;
        }       

        private string ReadDiscreteBody(ref Stream dataStream, ref IEntity entity, ref string body)
        {
            StringBuilder sBuilder;
            char[] cLine;
            string currentLine, boundary;
            bool bContinue = true;            
            
            int fulFilledCriteria;            

            var dataReader = new DataReader(new EndOfLineStrategy(), new NullLineStrategy());

            sBuilder = new StringBuilder();
            do
            {
                var result = dataReader.ReadData(ref dataStream);
                cLine = result.Data;
                currentLine = new string(cLine);
                boundary = m_delimiterFinder.FindDelimiter(ref entity, ref currentLine);
                if (!string.IsNullOrEmpty(boundary))
                {
                    break;
                }

                // Have we found the end of this message?                
                if (result.FulfilledCritera < 0)
                {
                    boundary = string.Empty;
                    break;
                }
                sBuilder.Append(result.Data);
            }
            while (bContinue);
                        
            body = sBuilder.ToString();
            sBuilder.Append(cLine);
            m_Source.Append(sBuilder);
            return boundary;
        }

        private new int ParseFields(ref Stream dataStream, out IList<MIMER.RFC822.Field> fields)
        {
            var result =ReadHeaders(ref dataStream);
            var headers = new string(result.Data);
            m_Source.Append(headers);

            headers = m_UnfoldPattern.RegularExpression.Replace(headers, " ");
            fields = new List<RFC822.Field>();

            m_FieldParser.Parse(ref fields, ref headers);            
            return result.FulfilledCritera;
        }

        private string ReadDelimiter(ref ContentTypeField contentTypeField)
        {
            if (contentTypeField.Parameters["boundary"] != null)
            {
                return contentTypeField.Parameters["boundary"];
            }
            return string.Empty;
        }

        private string FindStartDelimiter(ref Stream dataStream, ref IEntity entity)
        {            
            int fulFilledCriteria;
            string line, delimiter;                        

            var dataReader = new DataReader(new EndOfLineStrategy(), new NullLineStrategy());
            delimiter = string.Empty;

            do
            {
                var result = dataReader.ReadData(ref dataStream);
                fulFilledCriteria = result.FulfilledCritera;
                line = new string(result.Data);
                m_Source.Append(line);
                if (result.FulfilledCritera == 0)
                {
                    delimiter = string.Empty;
                    break;
                }

                delimiter = m_delimiterFinder.FindDelimiter(ref entity, ref line);
                if (!string.IsNullOrEmpty(delimiter))
                {                    
                    break;
                }                
            }
            while (fulFilledCriteria >= 0);
            return delimiter;
        }
      
        private bool isMIME(ref IList<RFC822.Field> fields)
        {
            foreach (RFC822.Field field in fields)
            {                
                if (m_MimeVersionPattern.RegularExpression.IsMatch(field.Name + ":" + field.Body))
                    return true;
            }
            return false;
        }
        #endregion

    }
}
