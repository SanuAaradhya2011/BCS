using CAB.IECFramework;
using CAB.IECFramework.Utility;

namespace CAB.IECChannel
{
    public class ChannelManager
    {
        public static IChannel GetChannel()
        {
            IChannel channel;
            ChannelType channelType = ConfigInfo.ChannelType;
            //if (channelType.Equals(ChannelType.RS232))
                channel = new IECLocalCommunication();
            //else if (channelType.Equals(ChannelType.GSM))
             //   channel = new GSMCommunication();
            //else if (channelType.Equals(ChannelType.GPRS))
            //    channel = new GPRSCommunication();
            //else
            //    channel = null;
            return channel;
        }
    }
}
