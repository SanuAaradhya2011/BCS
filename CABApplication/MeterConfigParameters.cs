#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace CAB.UI
{
    /// <summary>
    /// parameters to parameters from the meter .
    /// </summary>
    class MeterConfigParameters
    {
        
        #region Nested Types
        #endregion

        #region Constants and Variables
        private byte classID;

        private byte attributeID;

        private string obisCode;

        private byte permission;

        #endregion

        #region Properties
        #endregion

        #region Public Methods


        /// <summary>
        /// stores class ID.
        /// </summary>
        public byte ClassID
        {
            set
            {
                classID = value;
            }
            get
            {
                return classID;
            }
        }
        /// <summary>
        /// stores Attribute ID.
        /// </summary>
        public byte AttributeID
        {
            set
            {
                attributeID = value;
            }
            get
            {
                return attributeID;
            }
        }
        /// <summary>
        /// stores OBIS code
        /// </summary>
        public string OBISCode
        {
            set
            {
                obisCode = value;
            }
            get
            {
                return obisCode;
            }
        }
        /// <summary>
        /// stores permissions.
        /// </summary>
        public byte Permission
        {
            set
            {
                permission = value;
            }
            get
            {
                return permission;
            }
        }


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion


    }
}
