/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									            |
 * |											Date   : 25 March 2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
	/// <summary>
	/// This class is used to hold the value of metertype_master Table.
	/// </summary>
	public class MeterTypeMasterEntity : EntityBase
	{
		/// <summary>
		/// Private variable for MeterType_ID.
		/// </summary>
		private int _meterType_ID;

		/// <summary>
		/// Private variable for MeterType_Name.
		/// </summary>
		private string _meterType_Name;

		/// <summary>
		/// This property is used to get the value of MeterType_ID.
		/// </summary>
		public int MeterType_ID
		{
			get
			{
				return _meterType_ID;
			}
			set
			{
				_meterType_ID = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of MeterTypeName.
		/// </summary>
		public string MeterType_Name
		{
			get
			{
				return _meterType_Name;
			}
			set
			{
				_meterType_Name = value;
			}
		}
	}
}
