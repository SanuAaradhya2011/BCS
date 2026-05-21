using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication
{
    public class Response
    {
        public string IMEINumber { get; set; }

        public byte[] Data { get; set; }

        public string CommandId { get; set; }
    }
}
