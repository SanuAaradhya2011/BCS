#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace CAB.Parser.Entity
{

    public class Profile
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        /// <summary>
        /// holds the raw data as hex string of data capture objects 
        /// </summary>
        public string DataCaptureObjects { get; set; }
        /// <summary>
        /// holds the raw data as hex string of scalar objects
        /// </summary>
        public string DataScalarObjects { get; set; }
        /// <summary>
        /// holds the raw data as hex string of scalar buffer
        /// </summary>
        public string DataScalarBuffer { get; set; }
        /// <summary>
        /// holds the raw data as hex string of data buffer
        /// </summary>
        public string DataBuffer { get; set; }
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
