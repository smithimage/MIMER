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

using System.Collections.Specialized;
using System.Linq;
using MIMER.RFC2045;

namespace MIMER.RFC2183
{
    public class ContentDispositionField:MIMER.RFC822.Field
    {
        private StringDictionary m_Parameters;
        private string m_Disposition;

        internal ContentDispositionField()
        {
            m_Parameters = new StringDictionary();
        }

        public StringDictionary Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        public string Disposition
        {
            get { return this.m_Disposition; }
            set { this.m_Disposition = value; }
        }
    }

    public static class ContentDispositionFieldExtensions
    {
        public static bool IsAttachment(this ContentDispositionField field)
        {
            return field.Disposition.ToLower().Equals("attachment");
        }

        public static ContentDispositionField GetDispositionField(this IEntity entity)
        {
            return entity.Fields.FirstOrDefault(field => field is ContentDispositionField) as ContentDispositionField;
        }

        public static bool IsAttachment(this IEntity entity)
        {
            return entity.GetDispositionField() != null && entity.GetDispositionField().IsAttachment();
        }
    }
}
