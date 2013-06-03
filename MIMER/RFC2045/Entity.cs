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

namespace MIMER.RFC2045
{
    class Entity : MIMER.RFC2045.IEntity
    {
        private byte[] m_Body;        
        
        protected IList<RFC822.Field> m_Fields;        
        private string m_Delimiter;
        private MIMER.RFC2045.IMultipartEntity m_Parent; 
        private Encoding m_Encoding;


        public Entity()
        {            
            m_Fields = new List<RFC822.Field>();            
        }

        public byte[] Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }        

        public IList<RFC822.Field> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public string Delimiter
        {
            get { return m_Delimiter; }
            set { m_Delimiter = value; }
        }

        public MIMER.RFC2045.IMultipartEntity Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        public Encoding Encoding
        {
            get
            {
                if (m_Encoding == null)
                {
                    LoadEncoding();
                }
                return m_Encoding;
            }
            set
            {
                m_Encoding = value;
            }
        }

        private void LoadEncoding()
        {
            foreach (RFC822.Field field in Fields)
            {
                if (field is RFC2045.ContentTypeField)
                {
                    ContentTypeField contentTypeField = field as ContentTypeField;
                    if (contentTypeField.Parameters["charset"] != null)
                    {
                        m_Encoding = Encoding.GetEncoding(contentTypeField.Parameters["charset"]);
                    }
                }
            }
        }
    }
}
