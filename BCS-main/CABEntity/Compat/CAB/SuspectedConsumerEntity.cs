/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 29 March 2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;


namespace LNG.Entity
{
	public class SuspectedConsumerEntity: EntityBase
	{
		/// <summary>
		/// Private variable for Suspected Consumer ID.
		/// </summary>
		private int _suspectedConsumer_ID;

		/// <summary>
		/// Private variable for Consumer Number.
		/// </summary>
		private string _consumer_Number;

		/// <summary>
		/// Private variable for Suspection Start Date.
		/// </summary>
		private long _suspectionStartDate;

		/// <summary>
		/// Private variable for Suspection End Date.
		/// </summary>
		private long _suspectionEndDate;

		/// <summary>
		/// This property is used to get and set the value of CMRI ID.
		/// </summary>
		public int SuspectedConsumer_ID
		{
			get
			{
				return _suspectedConsumer_ID;
			}
			set
			{
				_suspectedConsumer_ID = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of CMRI Number.
		/// </summary>
		public string Consumer_Number
		{
			get
			{
				return _consumer_Number;
			}
			set
			{
				_consumer_Number = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of SuspectionStartDate.
		/// </summary>
		public long SuspectionStartDate
		{
			get
			{
				return _suspectionStartDate;
			}
			set
			{
				_suspectionStartDate = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of SuspectionEndDate.
		/// </summary>
		public long SuspectionEndDate
		{
			get
			{
				return _suspectionEndDate;
			}
			set
			{
				_suspectionEndDate = value;
			}
		}
	}
}

