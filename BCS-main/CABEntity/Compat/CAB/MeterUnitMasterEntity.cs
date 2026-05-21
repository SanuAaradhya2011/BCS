/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									            |
 * |											Date   : 25 March 2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;

namespace LNG.Entity
{
	/// <summary>
	/// This class is used to hold the value of meterunit_master Table.
	/// </summary>
	public class MeterUnitMasterEntity : EntityBase
	{
		/// <summary>
		/// Private variable for Meterunit_ID.
		/// </summary>
		private int _meterUnit_ID;

		/// <summary>
		/// Private variable for MeterUnitType.
		/// </summary>
		private string _meterunit_Type;

		/// <summary>
		/// This property is used to get the value of MeterUnitID.
		/// </summary>
		public int MeterUnit_ID
		{
			get
			{
				return _meterUnit_ID;
			}
			set
			{
				_meterUnit_ID = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of MeterUnitType.
		/// </summary>
		public string Meterunit_Type
		{
			get
			{
				return _meterunit_Type;
			}
			set
			{
				_meterunit_Type = value;
			}
		}
	}
}
