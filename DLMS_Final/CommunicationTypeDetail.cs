using CAB.Framework;
namespace DLMS_Final
{
    /// <summary>
    /// Class contains the functionlity for getting the communication type details
    /// </summary>
    public static class CommunicationTypeDetail
    {
        /// <summary>
        /// Gets the communciation type saved in the system.
        /// </summary>
        /// <returns></returns>
        public static CommunicationType GetCommunicationType()
        {
            CommunicationType commType = CommunicationType.DIRECT;
            try
            {
                commType =  (CommunicationType)System.Enum.Parse(typeof(CommunicationType), SerialPortSettings.Default.CommunicationType);
            }
            catch
            {
            }
            return commType;
        }
    }
}
