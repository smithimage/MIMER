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
using System.Net.Mail;

namespace MIMER.RFC2045
{
    /// <summary>
    /// This class should be used instead of null checks.
    /// Instanciate a NullMessage instead of returning null.
    /// and use IsNull() method to determine if message is empty.
    /// </summary>
    public class NullMessage:INullable, IMimeMailMessage
    {
        private IDictionary<string, string> m_Body;
        private IList<IAttachment> m_Attachments;


        public NullMessage()
        {
            m_Body = new Dictionary<string, string>();
            m_Attachments = new List<IAttachment>();
        }

        #region INullable Members

        public bool IsNull()
        {
            return true;
        }

        #endregion

        #region IMimeMailMessage Members

        public IDictionary<string, string> Body
        {
            get
            {
                return m_Body;
            }
            set
            {                
            }
        }

        public IList<IAttachment> Attachments
        {
            get
            {
                return m_Attachments;
            }
            set
            {
                
            }
        }

        #endregion

        #region IMailMessage Members

        public MailAddressCollection From
        {
            get
            {
                return new MailAddressCollection();
            }
            set
            {
                
            }
        }

        public MailAddressCollection To
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add(new MailAddress("null@null.null", "This is a nullmessage no receivers can be read.."));
                return addresses;
            }
            set
            {
                
            }
        }

        public MailAddressCollection CarbonCopy
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add(new MailAddress("null@null.null", "This is a nullmessage no carboncopy can be read.."));
                return addresses;                
            }
            set
            {
                
            }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add(new MailAddress("null@null.null", "This is a nullmessage no blindcarboncopy can be read.."));
                return addresses;                                
            }
            set
            {
                
            }
        }

        public string Subject
        {
            get
            {
                return "This is a nullmessage no subject can be read..";
            }
            set
            {
                
            }
        }

        public string Source
        {
            get
            {
                return "This is a nullmessage no source can be read..";
            }
            set
            {
                
            }
        }

        public string TextMessage
        {
            get
            {
                return "This is a nullmessage no Textmessage can be read..";
            }
            set
            {
                
            }
        }

        public IList<AlternateView> Views
        {
            get 
            {
                IList<AlternateView> views = new List<AlternateView>();
                return views;
            }
            set { }
        }

        public System.Net.Mail.MailMessage ToMailMessage()
        {
            return new System.Net.Mail.MailMessage();
        }

        #endregion

        #region IMimeMailMessage Members


        public IList<IMimeMailMessage> Messages
        {
            get
            {
                IList<IMimeMailMessage> list = new List<IMimeMailMessage>();
                list.Add(this);
                return list;
            }
            set
            {   
            }
        }

        #endregion
    }
}
