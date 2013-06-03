using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace MIMER
{
    public interface IPattern
    {
        string TextPattern { get; }
        Regex RegularExpression { get;}  
    }
}
