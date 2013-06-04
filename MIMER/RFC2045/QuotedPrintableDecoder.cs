/*
Copyright (c) 2007, Robert Wallström, smithimage.com
All rights reserved.
 
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
	
	* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
	* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
          in the documentation and/or other materials provided with the distribution. 
	* Neither the name of the SMITHIMAGE nor the names of its contributors may be used to endorse or promote products derived from 
           this software without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH  DAMAGE.
*/
using System;
using System.Text;
using System.Linq;

namespace MIMER.RFC2045
{
    public class QuotedPrintableDecoder : IDecoder
    {

        public event EventHandler<EventArgs> DecodeError = null;


        #region IDecoder Members

        public bool CanDecode(string encodign)
        {
            return string.IsNullOrEmpty(encodign) || encodign.ToLower().Equals("quoted-printable") || 
                encodign.ToLower().Equals("7bit") || encodign.ToLower().Equals("8bit") || encodign.ToLower().Equals("q");
        }

        public byte[] Decode(ref System.IO.Stream dataStream)
        {            
            string coded = ReadData(ref dataStream);
            return Decode(ref coded);   
        }
        #endregion

        private void OnDecodeError(object sender, EventArgs e)
        {
            if (DecodeError != null)
                DecodeError(sender, e);
        }

        private string ReadData(ref System.IO.Stream dataStream)
        {
            int size, pos, c;
            char[] buffer, data;
            char bBeforePrevious, beforePrevious, previous, current;

            size = 1;
            pos = 0;
            buffer = new char[size];

            while ((c = dataStream.ReadByte()) != -1)
            {
                if (pos >= size)
                {
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

                if (pos >= 3)
                {
                    bBeforePrevious = buffer[pos - 3];
                    beforePrevious = buffer[pos - 2];
                    previous = buffer[pos - 1];
                    current = buffer[pos];

                    //TODO:need to check against Delimiterline as well
                    //First null line separates headers from body (rfc822)
                    if ((bBeforePrevious == 13 && beforePrevious == 10 &&
                        previous == 13 && current == 10)||
                        (bBeforePrevious == 10 && beforePrevious == 10))
                    {
                        break;
                    }
                }
                pos++;
            }

            data = new char[pos + 1];
            Array.Copy(buffer, data, pos);
            buffer = null;
            return new string(data);
        }

        public byte[] Decode(ref string data)
        {
            return Decode(ref data, "iso-8859-1");
        }

        public byte[] Decode(ref string data, string charset)
        {
            var encoding = Encoding.GetEncoding(charset);
            return encoding.GetBytes(ConvertHexContent(data, encoding, 0));
        }

        private string ConvertHexToString(string hex, Encoding encoding)
        {
            if (string.IsNullOrEmpty(hex))
                return string.Empty;

            if (hex.StartsWith("="))
                hex = hex.Substring(1);

            string[] hexstrings = hex.Split(new[] { '=' });

            var result = hexstrings.ToList()
                .Select(hs => (byte)int.Parse(hs, System.Globalization.NumberStyles.HexNumber))
                .ToArray();
            return encoding.GetString(result);
        }


        public string ConvertHexContent(string hex, Encoding encoding, long nStart)
        {
            if (nStart >= hex.Length)
                return hex;

            var sbHex = new StringBuilder().Append("");
            var sbEncoded = new StringBuilder().Append("");
            var i = (int)nStart;

            while (i < hex.Length)
            {
                sbHex.Remove(0, sbHex.Length);
                bool hasBegun = false;

                while (i < hex.Length)
                {
                    string temp = hex.Substring(i, 1);
                    if (temp.StartsWith("="))
                    {
                        temp = hex.Substring(i, 3);
                        if (temp.EndsWith("\r\n"))
                        {
                            if (hasBegun)
                                break;
                            i = i + 3;
                        }
                        else if (!temp.EndsWith("3D"))
                        {
                            sbHex.Append(temp);
                            hasBegun = true;
                            i = i + 3;
                        }
                        else
                        {
                            if (hasBegun)
                                break;

                            sbEncoded.Append("=");
                            i = i + 3;
                        }

                    }
                    else
                    {
                        if (hasBegun)
                            break;
                        sbEncoded.Append(temp);
                        i++;
                    }
                }
                sbEncoded.Append(ConvertHexToString(sbHex.ToString(), encoding));
            }

            return sbEncoded.ToString();
        }

        public string ConvertHexContent(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return hex;

            return ConvertHexContent(hex, Encoding.Default, 0);

        }
    }
}
