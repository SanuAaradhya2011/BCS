#region Namespaces
#endregion
namespace CAB.Framework.Utility
{
    /// <summary>
    /// contains values for selecting communication mode which is used to get data from the meter.
    /// </summary>
    public enum CommunicationMode
    {
        /// <summary>
        /// selected for normal communication i.e DLMS
        /// </summary>

        Normal,
        /// <summary>
        /// selected for fastdownload communication
        /// </summary>
        FastDownload
    }
}
