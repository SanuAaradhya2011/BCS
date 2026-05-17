using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.IECChannel
{
    public class GPRSCommunication : IECChannelBase
    {
        public override bool OpenPort()
        {
            throw new NotImplementedException();
        }

        public override bool ClosePort()
        {
            throw new NotImplementedException();
        }

        public override bool SendCommand()
        {
            throw new NotImplementedException();
        }

        public override void DelayExecution()
        {
            throw new NotImplementedException();
        }

        public override bool Timeout()
        {
            throw new NotImplementedException();
        }

        public override void OnPortDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}