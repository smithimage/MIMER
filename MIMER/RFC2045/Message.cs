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
using System.Net.Mail;
using MIMER.RFC2183;
using MIMER.RFC822;
using System.Linq;

namespace MIMER.RFC2045
{
    class Message : MultipartEntity, MIMER.IMailMessage, MIMER.RFC2045.IMimeMailMessage, INullable
    {
        private string m_Source;
        private MailAddressCollection m_From;
        private MailAddressCollection m_Sender;
        private MailAddressCollection m_To;
        private MailAddressCollection m_CarbonCopy;
        private MailAddressCollection m_BlindCarbonCopy;
        private IList<AlternateView> m_Views;
        
        private string m_Text;
        private IDictionary<string, string> m_Body;
        private IList<IAttachment> m_Attachments;
        private IList<IMimeMailMessage> m_Messages;        
        private string m_Subject;

        public Message()
        {
            m_Views = new List<AlternateView>();
        }

        public IList<AlternateView> Views
        {
            get 
            {
                if (m_Views.Count == 0)
                    LoadViews();
                return m_Views; 
            }
            set { m_Views = value; }
        }

        #region IMailMessage Members

        /// <summary>
        /// The adress of this message sender.
        /// </summary>
        public MailAddressCollection From
        {
            get
            {
                if (m_From == null)
                {
                    m_From = LoadFrom();
                }

                return m_From;
            }
            set
            {
                m_From = value;
            }
        }

        public MailAddressCollection Sender
        {
            get 
            {
                if (m_Sender == null)
                {
                    m_Sender = this.From;
                }
                return m_Sender;
            }
            set
            {
                m_Sender = value;
                m_From = value;
            }

        }

        /// <summary>
        /// A adress to the receiver of this message.
        /// Should be a string recognized by mail sending routines.
        /// </summary>
        public MailAddressCollection To
        {
            get
            {
                if (m_To == null)
                {
                    m_To = LoadTo();
                }
                return m_To;
            }
            set
            {
                m_To = value;
            }
        }
                
        public MailAddressCollection CarbonCopy
        {
            get
            {
                if (m_CarbonCopy == null)
                {
                    m_CarbonCopy = LoadCarbonCopy();
                }
                return m_CarbonCopy;
            }
            set
            {
                m_CarbonCopy = value;
            }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                if (m_BlindCarbonCopy == null)
                    m_BlindCarbonCopy = LoadBlindCarbonCopy();
                return m_BlindCarbonCopy;
            }
            set
            {
                m_BlindCarbonCopy = value;
            }
        }     

        public string Subject
        {
            get
            {
                if (m_Subject == null)
                {
                    LoadSubject();
                }
                return m_Subject;
            }
            set
            {
                m_Subject = value;
            }
        }

        /// <summary>
        /// The text body of this message.
        /// If there are multiple bodies of text in this message, 
        /// this method returnes the first body of text.
        /// </summary>
        public string TextMessage
        {
            get 
            {
                if (m_Text == null && Body != null && Body.Values != null)
                {
                    m_Text = Body.Values.FirstOrDefault();
                }
                return m_Text;
            }
            set { m_Text = value; }
        }

        public string Source
        {
            get
            {
                return m_Source;
            }
            set
            {
                m_Source = value;
            }
        }

        #endregion

        #region IMimeMailMessage Members

        public new IDictionary<string, string> Body
        {
            get
            {
                if (m_Body == null)
                {
                    //Must be recursive??
                    m_Body = new Dictionary<string, string>();
                    LoadBody(this);
                }
                return m_Body;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public IList<IAttachment> Attachments
        {
            get
            {
                if (m_Attachments == null)
                {
                    m_Attachments = new List<IAttachment>();
                    LoadAttachments(this);
                }
                    
                return m_Attachments;
            }
            set
            {
                m_Attachments = value;
            }
        }

        public IList<IMimeMailMessage> Messages
        {
            get
            {
                if (m_Messages == null)
                {
                    m_Messages = new List<IMimeMailMessage>();
                    LoadMessages(this);
                }
                return m_Messages;
            }
            set { m_Messages = value; }
        }

        #endregion        

        #region INullable Members

        public virtual bool IsNull()
        {
            return false;
        }

        #endregion

        #region Private methods
        private static string ToString(byte[] data)
        {
            if (data == null)
                return string.Empty;

            char[] stringdata = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
                stringdata[i] = Convert.ToChar(data[i]);
            return new string(stringdata);
        }

        private void LoadSubject()
        {
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("subject"))
                {
                    m_Subject = field.Body; 
                }                   
            }            
        }

        private void LoadBody(IEntity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;

                mpe.BodyParts
                    .Where(child=>child != parent && !(child is Message))
                    .ToList()
                    .ForEach(LoadBody);
            }



            ContentTypeField contentTypeField = null;
            ContentDispositionField contentDispositionField = null;

            foreach (RFC822.Field field in parent.Fields)
            {                
                if (field is ContentTypeField)
                {
                    ContentTypeField contentField = field as ContentTypeField;
                    if (contentField.Type.ToLower().Equals("text"))
                    {
                        contentTypeField = contentField;
                    }
                }

                if (field is ContentDispositionField)
                {
                    contentDispositionField = field as ContentDispositionField;                    
                }
            }

            if (contentTypeField != null && 
                (contentDispositionField == null || 
                contentDispositionField.Disposition.ToLower().Equals("inline")))
            {
                string text = string.Empty;
                if (parent.Encoding != null)
                {
                    if (parent.Body != null)
                    {
                        text = parent.Encoding.GetString(parent.Body);
                    }
                }
                else
                {
                    text = ToString(parent.Body);
                }

                string key = GetKey(contentTypeField);
                m_Body.Add(key, text);
            }
        }

        private string GetKey(ContentTypeField contentTypeField)
        {
            string key = string.Format("{0}/{1}", contentTypeField.Type, contentTypeField.SubType);
            int i = 0;
            while (m_Body.Keys.Contains(key))
            {
                key = string.Format("{0}/{1}-{2}", contentTypeField.Type, contentTypeField.SubType, i);
                i++;
            }
            return key;
        }

        private void LoadMessages(IEntity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;
                foreach (Entity child in mpe.BodyParts)
                {
                    if (child is MultipartEntity && !(child is Message))
                    {
                        LoadMessages(child);
                    }
                    else if (child is Message)
                    {
                        ContentDispositionField field = child.GetDispositionField();
                        if (field == null || !(field.IsAttachment() && field.IsInline()))
                        {
                            m_Messages.Add(child as Message);
                        }

                    }   
                }
            }
        }

        private void LoadAttachments(IEntity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;
                foreach (Entity entity in mpe.BodyParts)
                {
                    if (entity is MultipartEntity && !(entity is Message))
                    {
                        LoadAttachments(entity);
                    }
                    else if ((!(entity is MultipartEntity) && !(entity is Message)) || entity.IsAttachment())
                    { 
                        m_Attachments.Add(entity.AsAttachment());
                    }
                }                    
            }            
        }
        
        private MailAddressCollection LoadFrom()
        {
            MailAddressCollection addressCollection = new MailAddressCollection();
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("from"))
                {
                    MailAddress address ;
                    try
                    {
                        address = new MailAddress(field.Body);
                        addressCollection.Add(address);
                    }
                    catch(FormatException)
                    {
                        string sAddress = GetSAddress(field);
                        address = new MailAddress(sAddress);
                        addressCollection.Add(address);
                    }
                    return addressCollection;
                }
            }
            return null;
        }

        private string GetSAddress(Field field)
        {
            string sAddress = string.Empty;
            IPattern addrSpecPattern =
                PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.AddrSpecPattern));
            if(addrSpecPattern.RegularExpression.IsMatch(field.Body))
            {
                sAddress = addrSpecPattern.RegularExpression.Match(field.Body).Value;
            }
            return sAddress;
        }

        private MailAddressCollection LoadTo()
        {
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("to"))
                {
                    return RFC822.Message.GetAddresses(field);                    
                }
                 
            }
            return null;
        }

        private MailAddressCollection LoadCarbonCopy()
        {
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("cc"))
                {
                    return RFC822.Message.GetAddresses(field);                    
                }                    
            }
            return null;
        }

        private MailAddressCollection LoadBlindCarbonCopy()
        {
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("bcc"))
                {
                    return RFC822.Message.GetAddresses(field);
                }
            }
            return null;       
        }
      
        private void LoadViews()
        {
            IEnumerator<KeyValuePair<string,string>> enu = Body.GetEnumerator();
            while(enu.MoveNext())
            {               
                System.IO.MemoryStream stream =
                    new System.IO.MemoryStream(System.Text.Encoding.ASCII.GetBytes(enu.Current.Value));
                AlternateView view = new AlternateView(stream, enu.Current.Key);       
                m_Views.Add(view);

            }
        }

        #endregion

        /// <summary>
        /// Transforms this MIMER.RFC2045.Messsage to a System.Net.Mail.MailMessage
        /// </summary>
        /// <returns>The new copied message.</returns>
        public System.Net.Mail.MailMessage ToMailMessage()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            if(To != null)
                message.To.Add(this.To.ToString());
            if (CarbonCopy != null)
                message.CC.Add(this.CarbonCopy.ToString());
            if(From != null)
                message.Sender = new System.Net.Mail.MailAddress(this.From.ToString());
            if(Subject != null)
                message.Subject = this.Subject.ToString();
            if(TextMessage != null)
                message.Body = this.TextMessage.ToString();

            Attachments
                .Where(attachment => !attachment.IsNull())
                .ToList()
                .ForEach(attachment =>
                    {
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(attachment.Data);
                        message.Attachments.Add(new System.Net.Mail.Attachment(stream, attachment.Name,
                            attachment.Type + "/" + attachment.SubType));
                    });


            Views.Select(view => new AlternateView(view.ContentStream, view.ContentType.MediaType))
                    .ToList()
                    .ForEach(alternateView => message.AlternateViews.Add(alternateView));

            return message;
        }
    }
}
