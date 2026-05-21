#region Namespaces

#endregion


namespace CABCommunication.Common
{
    /// <summary>
    /// Specifies the communication type.
    /// </summary>
    public enum CommunicationTypeInfo
    {
        /// <summary>
        /// Includes Fast Download Mode commands handling.
        /// </summary>
        FastDownload = 0,

        /// <summary>
        /// Includes CMRI Mode commands handling.
        /// </summary>
        Normal = 1,

        /// <summary>
        /// Includes optical and RS232 communication. Baud rate change expected.
        /// </summary>
        IEC = 2

    }
}
