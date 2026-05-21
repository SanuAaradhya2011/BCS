using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.BCS.DLMS.Utility
{
    interface IConnection
    {
        void Disconnect();
        bool Connect();
    }
}
