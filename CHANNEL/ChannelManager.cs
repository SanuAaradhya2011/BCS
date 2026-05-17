using CAB.Framework;
using CAB.Framework.Utility;

namespace CAB.Channel
{
    public class ChannelManager
    {
        public static IChannel GetChannel()
        {
            IChannel channel;
            ChannelType channelType = ConfigInfo.ChannelType;
            if (channelType.Equals(ChannelType.RS232))
                channel = new LocalCommunication();
            else if (channelType.Equals(ChannelType.GSM))
                channel = new GSMCommunication();
            else if (channelType.Equals(ChannelType.GPRS))
                channel = new GPRSCommunication();
            else
                channel = null;
            return channel;
        }
    }
}
