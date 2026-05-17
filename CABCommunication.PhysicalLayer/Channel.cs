#region Namesoaces
using CABCommunication.Common;

#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// Channel Class to create a communication object at run time
    /// </summary>
    public abstract class Channel : IPhysicalChannel
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static object syncRoot;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        public abstract void SetBaud(byte baud);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool OpenSession();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool CloseSession();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public abstract Result Send(byte[] data, int dataLength);

        /// <summary>
        /// Creates an object at run time depending on the communication type using Com Port/IMEI number
        /// </summary>
        /// <param name="channelType"></param>
        /// <param name="comPort"></param>
        /// <returns></returns>
        public static IPhysicalChannel CreateChannel(ChannelInformation channelInfo )
        {
            syncRoot = new object();
            IPhysicalChannel channel = null;
            //for thread safety
            lock (syncRoot)
            {
                switch (channelInfo.CommunicationMode)
                {
                    case "GPRS":
                        channel = new GPRS(channelInfo);
                        break;
                    case "Direct":
                        channel = new Serial(channelInfo);
                        break;
                    case "IrDA":
                        channel = new SerialIrDA(channelInfo);
                        break;
                    case "GSM":
                        channel = new GSM(channelInfo);
                        break;
                    case "PSTN":
                        channel = new PSTN(channelInfo);
                        break;
                    case "TCP":
                        channel = new TCP(channelInfo);
                        break;
                    default:
                        channel = new Serial(channelInfo);
                        break;
                }
            }
            return channel;
        }

        /// <summary>
        /// Creates an object at run time depending on the communication type using Channel Details
        /// </summary>
        /// <param name="channelType"></param>
        /// <param name="channelDetails"></param>
        /// <returns></returns>
        public static IPhysicalChannel CreateChannel(string channelType, ChannelDetail channelDetails)
        {
            IPhysicalChannel channel = new Serial(channelDetails);           
            return channel;

        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion
    }
}
