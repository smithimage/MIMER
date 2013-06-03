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
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using System.Linq;

namespace MIMER.RFC2045
{
    public class QuotedPrintableDecoder : IDecoder
    {
        private static readonly string s_CRLF = "(?<crlf>\x0D\x0A)";
        private static readonly string s_LF = "(?<lf>\x0A)";
        private static readonly string s_TransportPadding = "[\x5Cs]*";
        private static readonly string s_HexOctet = "=[0-9A-F]{2,2}";
        private static readonly string s_SafeChar = "[\x21-\x3C\x3E-\x7E]";
        private static readonly string s_Ptext = "(" + s_HexOctet + "|" + s_SafeChar + ")";
        private static readonly string s_QPsection = "((" + s_Ptext + "|\x20|\x09)*" + s_Ptext + ")";
        private static readonly string s_QPsegment = "(" + s_QPsection + "(\x20|\x09)*=)";
        private static readonly string s_QPpart = s_QPsection;

        private static readonly string s_QPline = string.Format("(?<lines>({0}{1}({2}|{3}))*{4}{5})", s_QPsegment, 
            s_TransportPadding, s_CRLF, s_LF, s_QPpart, s_TransportPadding);

        private static readonly string s_QuotedPrintable = s_QPline + "(" + s_CRLF + s_QPline + ")*$";

        private Regex m_REquotedPrintable = new Regex(s_QuotedPrintable, RegexOptions.Compiled);
        private Regex m_REhexOctet = new Regex(s_HexOctet, RegexOptions.Compiled);

        public event EventHandler<EventArgs> DecodeError = null;


        #region IDecoder Members

        public bool CanDecode(string encodign)
        {
            return string.IsNullOrEmpty(encodign) || encodign.ToLower().Equals("quoted-printable") || 
                encodign.ToLower().Equals("7bit") || encodign.ToLower().Equals("8bit");
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
            string decoded = DecodeToString(ref data);
            return decoded.Select(c => Convert.ToByte(c)).ToArray();
        }

        private string DecodeToString(ref string data)
        {
            string decoded = String.Empty;
            Match match = m_REquotedPrintable.Match(data);            

            if (match.Groups["lines"] != null)
            {
                Group matchGroup = match.Groups["lines"];
                int linefeeds = -1;

                if (match.Groups["crlf"] != null)
                {
                    linefeeds = match.Groups["crlf"].Captures.Count;
                }
                else if(match.Groups["lf"] != null)
                {
                    linefeeds = match.Groups["lf"].Captures.Count;
                }
                
                for (int i = 0; i < matchGroup.Captures.Count; i++)
                {
                    decoded += matchGroup.Captures[i].Value;

                    if (linefeeds > 0)
                    {
                        decoded += Environment.NewLine;
                        linefeeds--;
                    }                    
                }
            }            


            while (m_REhexOctet.IsMatch(decoded))
            {
                Match hexMatch = m_REhexOctet.Match(decoded);
                string hex = hexMatch.Value.Substring(1);

                try
                {
                    int number = Convert.ToInt32(hex, 16);
                    char c = (char)number;
                    decoded = Regex.Replace(decoded, hexMatch.Value, c.ToString());
                }
                catch(FormatException)
                {
                    OnDecodeError(this, new EventArgs());
                    break;
                }
                
            }
            return decoded;
        }
    }
}
