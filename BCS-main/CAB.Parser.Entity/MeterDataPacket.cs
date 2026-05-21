#region Namespaces
using System;
using System.Collections.Generic;
#endregion

namespace CAB.Parser.Entity
{
    /// <summary>
    /// This class contains the collection of actual 
    /// data elements along with their units 
    /// </summary>
    public class MeterDataPacket
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties

        /// <summary>
        /// Interval date for Profile
        /// </summary>
        public DateTime ReadingDate { get; set; }


        /// <summary>
        /// list of column information for each profile
        /// </summary>
        public List<DataElement> ListDataElementValue { get; set; }



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
