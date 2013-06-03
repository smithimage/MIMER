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
using System;
using System.Collections.Generic;
using System.Text;

namespace MIMER.RFC822
{
    class NullLineStrategy:IEndCriteriaStrategy
    {
        #region IEndCriteriaStrategy Members

        public bool IsEndReached(char[] data, int size)
        {
            if (size >= 3)
            {
                int fourth = data[size - 3];
                int third = data[size - 2];
                int second = data[size - 1];
                int first = data[size];

                //First null line separates headers from body (rfc822)
                if (fourth == 13 && third == 10 &&
                    second == 13 && first == 10 ||
                    (second == 10 && first == 10))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
