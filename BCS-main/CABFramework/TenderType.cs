using System;
using System.Collections.Generic;
using System.Text;

namespace CAB.Framework
{
    public enum TenderType
    {
        UGVCL,
        DGVCL,
        JUSCO
    }
    public enum MeterStatus
    { 
       LocalModemNotConnected,
       LocalModemConnectedModemBusy,
       Connected,
       LocalModemConnectedNoCarrier,
       NotConnected,
       LocalModemConnectedNotMeter
       
    }
}
