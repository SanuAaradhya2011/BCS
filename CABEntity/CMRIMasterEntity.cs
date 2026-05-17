/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.Framework.Entity;


namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of CMRI Master Table.
    /// </summary>
    public class CMRIMasterEntity : EntityBase
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

    }
}
