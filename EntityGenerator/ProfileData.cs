#region Namespaces
using System.Collections.Generic;

using CAB.Parser.Entity;

#endregion

namespace CAB.EntityGenerator
{
    /// <summary>
    /// This class is used to contain the 
    /// List of meter data packet .
    /// </summary>
    public class ProfileData
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        /// <summary>
        /// Unique Id for Profile Under processing
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// List of record information for profile data
        /// </summary>
        public List<MeterDataPacket> ListMeterDataPacket { get; set; }
        
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }
}
