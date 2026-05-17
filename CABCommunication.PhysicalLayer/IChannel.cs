#region Namespaces
using CABCommunication.Common;
#endregion

namespace CABCommunication.PhysicalLayer
{
    /// <summary>
    /// Interface to enable multiple inheritance
    /// </summary>
    public interface IPhysicalChannel
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        /// <summary>
        /// Opens Session
        /// </summary>
        /// <returns></returns>
        bool OpenSession();

        /// <summary>
        /// Closes Session
        /// </summary>
        /// <returns></returns>
        bool CloseSession();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        Result Send(byte[] data, int dataLength);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baud"></param>
        void SetBaud(byte baud);
      
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

    }
}

