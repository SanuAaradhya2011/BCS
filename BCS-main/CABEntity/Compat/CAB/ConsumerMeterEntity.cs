/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 											Modified : Piyush Singh                                                |
 * |                                            Date   : 29 March 2010                                              |
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;


namespace LNG.Entity
{
	/// <summary>
	/// This class is used to hold the value of Consumer Meter Table.
	/// </summary>
	public class ConsumerMeterEntity : EntityBase
	{
		/// <summary>
		/// Private variable for Consumer Meter ID.
		/// </summary>
		private int _consumerMeterID;

		/// <summary>
		/// Private variable for Meter ID.
		/// </summary>
		private string _meterID;

		/// <summary>
		/// Private variable for Consumer Number.
		/// </summary>
		private string _consumerNumber;

		/// <summary>
		/// Private variable for Meter Allocation Date.
		/// </summary>
		private long _meterAllocationDate;

		/// <summary>
		/// Private variable for Meter Location.
		/// </summary>
		private string _meter_Location;

		/// <summary>
		/// Private variable for Meter Allocation Status for Consumer.
		/// </summary>
		private int _status;

        private int region_ID;
        private int circle_ID;
        private int division_ID;
        private string communication_Type;

		/// <summary>
		/// This property is used to get the value of ConsumerMeter ID.
		/// </summary>
		public int ConsumerMeter_ID
		{
			get
			{
				return _consumerMeterID;
			}
			set
			{
				_consumerMeterID = value;
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
		/// This property is used to get and set the value of Consumer Number.
		/// </summary>
		public string Consumer_Number
		{
			get
			{
				return _consumerNumber;
			}
			set
			{
				_consumerNumber = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter Allocation Date.
		/// </summary>
		public long Meter_AllocationDate
		{
			get
			{
				return _meterAllocationDate;
			}
			set
			{
				_meterAllocationDate = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter Location.
		/// </summary>
		public string Meter_Location
		{
			get
			{
				return _meter_Location;
			}
			set
			{
				_meter_Location = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter Allocation Date.
		/// </summary>
		public int Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}
        public int Region_ID
        {
            get
            {
                return region_ID;
            }
            set
            {
                region_ID = value;
            }
        }
        public int Circle_ID
        {
            get
            {
                return circle_ID;
            }
            set
            {
                circle_ID = value;
            }
        }
        public int Division_ID
        {
            get
            {
                return division_ID;
            }
            set
            {
                division_ID = value;
            }
        }
        public string Communcation_Type
        {
            get
            {
                return communication_Type;
            }
            set
            {
                communication_Type = value;
            }
        }

	}
}

   

