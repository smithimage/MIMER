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
using System.Text.RegularExpressions;

namespace MIMER.RFC822
{
    class Message : MIMER.IMailMessage
    {
        protected MailAddressCollection m_ToAddresses;
        protected MailAddressCollection m_CarbonCopyAddresses;
        protected MailAddressCollection m_BlindCarbonCopyAddresses;

        protected string m_Source;
        protected IList<Field> m_Fields;
        protected string m_Body;
        protected MailAddressCollection m_From;

        public IList<Field> Fields
        {
            get
            {
                return m_Fields;
            }
            set
            {
                m_Fields = value;
            }
        }        

        #region IMailMessage Members

        public MailAddressCollection From
        {
            get
            {
                m_From = new MailAddressCollection();
                foreach (Field field in m_Fields)
                {
                    if (field.Name != null)
                    {
                        if (field.Name.ToLower().Equals("from"))
                        {
                            MailAddress address;
                            try
                            {
                                address = new MailAddress(field.Body);
                                m_From.Add(address);
                            }
                            catch (FormatException)
                            {
                                string sAddress = GetSAddress(field);
                                address = new MailAddress(sAddress);
                            }
                            return m_From;                            
                        }
                    }
                }
                return null;
            }
            set
            {
                m_From = value;
            }
        }

        public MailAddressCollection To
        {
            get
            {
                if(m_ToAddresses == null)
                    m_ToAddresses = LoadTo();
                return m_ToAddresses;
            }
            set
            {
                m_ToAddresses = value;
            }
        }

        public MailAddressCollection CarbonCopy
        {
            get
            {
                if (m_CarbonCopyAddresses == null)
                    m_CarbonCopyAddresses = LoadCarbonCopy();
                return m_CarbonCopyAddresses;
            }
            set
            {
                m_CarbonCopyAddresses = value;
            }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                if (m_BlindCarbonCopyAddresses == null)
                    m_BlindCarbonCopyAddresses = LoadBlindCarbonCopy();
                return m_BlindCarbonCopyAddresses;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string TextMessage
        {
            get
            {
                return this.m_Body;
            }
            set
            {
                this.m_Body = value;
            }
        }

        public string Subject
        {
             get
            {
                foreach (Field f in m_Fields)
                {
                    if (f.Name != null)
                    {
                        if (f.Name.ToLower().Equals("subject"))
                        {
                            return f.Body;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Field f in m_Fields)
                {
                    if (f.Name != null)
                    {
                        if (f.Name.ToLower().Equals("subject"))
                        {
                            f.Body = value;
                            return;
                        }
                    }
                }
                Field nField = new Field();
                nField.Name = "Subject";
                nField.Body = value;
                m_Fields.Add(nField);
            }
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

        #region IMailMessage Members


        public bool IsNull()
        {
            return false;
        }

        #endregion

        private MailAddressCollection LoadTo()
        {
            foreach (Field field in m_Fields)
            {
                if (field.Name != null)
                {
                    if (field.Name.ToLower().Equals("to"))
                    {
                        return GetAddresses(field);
                    }
                }
            }
            return null;
        }

        public static MailAddressCollection GetAddresses(Field field)
        {
            IPattern addrSpecPattern =
                PatternFactory.GetInstance().Get(typeof (RFC822.Pattern.AddrSpecPattern));
            MailAddressCollection addresses = null;
            if(addrSpecPattern.RegularExpression.IsMatch(field.Body))
            {
                //TODO: Make supprt for multiple addresses with names
                addresses = new MailAddressCollection();
                try
                {
                    MailAddress address = new MailAddress(field.Body);
                    addresses.Add(address);
                    
                }
                catch(FormatException)
                {
                    MatchCollection matches = addrSpecPattern.RegularExpression.Matches(field.Body);
                    foreach (Match match in matches)
                    {
                        addresses.Add(match.Value);
                    }
                }
            }
            return addresses;
        }

        private MailAddressCollection LoadCarbonCopy()
        {
            foreach (MIMER.RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("cc"))
                {
                    return GetAddresses(field);
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
                    return GetAddresses(field);
                }
            }
            return null;
        }

        private string GetSAddress(Field field)
        {
            string sAddress = string.Empty;
            IPattern addrSpecPattern =
                PatternFactory.GetInstance().Get(typeof(RFC822.Pattern.AddrSpecPattern));
            if (addrSpecPattern.RegularExpression.IsMatch(field.Body))
            {
                sAddress = addrSpecPattern.RegularExpression.Match(field.Body).Value;
            }
            return sAddress;
        }
    }
}
