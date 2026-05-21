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
using System.Collections.Generic;


namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of Sub Group Meter Master Table.
    /// </summary>
    public class SubGroupMeterMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for SubGroupMeter ID.
        /// </summary>
        private ulong _subGroupMeterID;

        /// <summary>
        /// Private variable for Sub Group ID.
        /// </summary>
        private int _subGroupID;

        /// <summary>
        /// Private variable for Meter ID.
        /// </summary>
        /////////private string _meterID;
		private string _meterID;


        /// <summary>
        /// Private variable for Group Allocation Date.
        /// </summary>
        private DateTime _groupAllocationDate;

        /// <summary>
        /// This property is used to get the value of SubGroupMeter ID.
        /// </summary>
        public ulong SubGroupMeter_ID
        {
            get
            {
                return _subGroupMeterID;
            }
            set
            {
                _subGroupMeterID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Sub Group ID.
        /// </summary>
        public int SubGroup_ID
        {
            get
            {
                return _subGroupID;
            }
            set
            {
                _subGroupID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Meter ID.
        /// </summary>
        public string Meter_ID
        {
            get
            {
                return _meterID;
            }
            set
            {
                _meterID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Group Allocation Date.
        /// </summary>
        public DateTime GroupAllocation_Date
        {
            get
            {
                return _groupAllocationDate;
            }
            set
            {
                _groupAllocationDate = value;
            }
        }



    }
}
