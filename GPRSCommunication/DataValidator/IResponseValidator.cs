using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication.DataValidator
{
    interface IResponseValidator
    {
        bool validateResponse(byte[] response);
    }
}
