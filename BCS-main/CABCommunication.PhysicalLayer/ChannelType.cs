#region Namespaces

#endregion


namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// Specifies the communication type.
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        ///  Includes optical and RS232 communication. No baud rate change expected.
        /// </summary>
        Direct = 0,
        /// <summary>
        /// Includes GSM modem commands handling.
        /// </summary>
        GSM = 1,
        /// <summary>
        /// Includes PSTN modem commands handling.
        /// </summary>
        PSTN = 2,
        /// <summary>
        /// 
        /// </summary>
        GPRS = 3,
        /// <summary>
        /// 
        /// </summary>
        TCP = 4,
        /// <summary>
        /// 
        /// </summary>
        FTP = 5
    }
}
