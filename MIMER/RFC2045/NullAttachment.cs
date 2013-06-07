using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIMER.RFC2045
{
    class NullAttachment:IAttachment
    {
        public bool IsNull()
        {
            return true;
        }

        public string Type { get; set; }
        public string SubType { get; set; }
        public string Disposition { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
