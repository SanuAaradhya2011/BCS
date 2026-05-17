#region Namespaces

#endregion

namespace CABCommunication.Common
{
    /// <summary>
    /// Specifies the command type.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// 
        /// </summary>
        I = 0x10,

        /// <summary>
        /// 
        /// </summary>
        SNRM = 0x93,

        /// <summary>
        /// 
        /// </summary>
        UA = 0x73,

        /// <summary>
        /// 
        /// </summary>
        DISC = 0x53,

        /// <summary>
        /// 
        /// </summary>
        DM = 0x1F,

        /// <summary>
        /// 
        /// </summary>
        Nothing = 5

    }
    /// <summary>
    /// Specifies the action type.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 
        /// </summary>
        READ = 0x01,

        /// <summary>
        /// 
        /// </summary>
        WRITE = 0x02,

        /// <summary>
        /// 
        /// </summary>
        RESET = 0x03,

        /// <summary>
        /// 
        /// </summary>
        WRITEBUFFER = 0x04,
        /// <summary>
        /// 
        /// </summary>
        ACTIONREQUEST=0x05
    }
}
