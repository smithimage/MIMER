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

namespace MIMER
{
    /// <summary>
    /// Interface to be used to implement the current algorithm which decides how a 
    /// message stream is ended.
    /// </summary>
    public interface IEndCriteriaStrategy
    {        
        /// <param name="data">The currently read characters from stream,
        /// since last read sequence.</param>
        /// <param name="size">The amount of data currently within the 'data' array.</param>
        /// <returns>Should return true as soon as the data array meets the wanted
        /// endcriteria.</returns>
        bool IsEndReached(char[] data, int size);
    }
}
