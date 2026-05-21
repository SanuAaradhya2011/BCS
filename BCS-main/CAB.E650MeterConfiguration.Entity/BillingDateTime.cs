#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace CAB.E650MeterConfiguration.Entity
{
    /// <summary>
    /// Represents Billing Type Entity
    /// </summary>
    public class BillingDateTime
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        /// <summary>
        /// Holds Day value 
        /// </summary>
        public byte Date { get; set; }
        /// <summary>
        /// Holds Hour value
        /// </summary>
        public byte Hour { get; set; }
        /// <summary>
        /// Holds Minute value 
        /// </summary>
        public byte Minute { get; set; }
        /// <summary>
        /// Holds Billing Period ( odd/Even/Monthly) 
        /// </summary>
        public byte BillingType { get; set; }
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
