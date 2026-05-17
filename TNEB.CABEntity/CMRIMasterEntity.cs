/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of CMRI Master Table.
    /// </summary>
    public class IECCMRIMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for CMRI ID.
        /// </summary>
        private long _cmriID;

        /// <summary>
        /// Private variable for CMRI Number.
        /// </summary>
        private string _cmriNumber;

        /// <summary>
        /// Private variable for CMRI Description.
        /// </summary>
        private string _cmriDescription;
        /// <summary>
        /// private variable for cmri type.
        /// </summary>
        private string cmriType;
        /// <summary>
        /// This property is used to get and set the value of CMRI ID.
        /// </summary>
        public long CMRI_ID
        {
            get
            {
                return _cmriID;
            }
            set
            {
                _cmriID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of CMRI Number.
        /// </summary>
        public string CMRI_Number
        {
            get
            {
                return _cmriNumber;
            }
            set
            {
                _cmriNumber = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of CMRI Description.
        /// </summary>
        public string CMRI_Description
        {
            get
            {
                return _cmriDescription;
            }
            set
            {
                _cmriDescription = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of CMRI Type
        /// </summary>
        public string CMRIType
        {
            get
            {
                return cmriType;
            }
            set
            {
                cmriType = value;
            }
        }

    }
}
