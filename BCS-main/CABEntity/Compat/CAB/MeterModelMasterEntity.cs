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
	/// This class is used to hold the value of metermodel_master Table.
	/// </summary>
	public class MeterModelMasterEntity : EntityBase
	{
		/// <summary>
		/// Private variable for MeterModel_ID.
		/// </summary>
		private int _meterModel_ID;

		/// <summary>
		/// Private variable for MeterModelName.
		/// </summary>
		private string _meterModel_Name;

		/// <summary>
		/// This property is used to get the value of MeterModelID.
		/// </summary>
		public int MeterModel_ID
		{
			get
			{
				return _meterModel_ID;
			}
			set
			{
				_meterModel_ID = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of MeterModelName.
		/// </summary>
		public string MeterModel_Name
		{
			get
			{
				return _meterModel_Name;
			}
			set
			{
				_meterModel_Name = value;
			}
		}
	}
}

