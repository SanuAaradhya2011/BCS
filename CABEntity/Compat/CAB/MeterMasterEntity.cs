/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh		 									        |
 * |											Date   : 25 March 2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;
using LNG.Framework;

namespace LNG.Entity
{
	/// <summary>
	/// This class is used to hold the value of Consumer Master Table.
	/// </summary>
	public class MeterMasterEntity : EntityBase
	{
		/// <summary>
		/// Private variable for Meter ID.
		/// </summary>
		private string _meter_ID;

		/// <summary>
		/// Private variable for Meter Type ID.
		/// </summary>
		private int _meterType_ID;

		/// <summary>
		/// Private variable for Meter Model ID.
		/// </summary>
		private int _meterModel_ID;

		/// <summary>
		/// Private variable for Meter EMF.
		/// </summary>
		private decimal _meter_EMF;

		/// <summary>
		/// Private variable for Meter Contract Demand.
		/// </summary>
		private double _meter_ContractDemand;

		/// <summary>
		/// Private variable for Meter Unit ID.
		/// </summary>
		private int _meterUnit_ID;

		/// <summary>
		/// Private variable for Meter CT Primary.
		/// </summary>
		private int _meter_CTPrimary;

		/// <summary>
		/// Private variable for meter CT Secondary.
		/// </summary>
		private int _meter_CTSecondary;

		/// <summary>
		/// Private variable for meter PT Primary.
		/// </summary>
		private int _meter_PTPrimary;

		/// <summary>
		/// Private variable for meter PT Secondary.
		/// </summary>
		private int _meter_PTSecondary;

        /// <summary>
        /// Private variable for meter CT Ratio
        /// </summary>
        private int meterCTRatio;

        ///<summary>
        ///private variable for meter PT Ratio
        /// </summary>
        private int meterPTRatio;

		/// <summary>
		/// Private variable for meter Installed CT Primary.
		/// </summary>
		private int _meter_InstalledCTPrimary;

		/// <summary>
		/// Private variable for meter Installed CT Secondary.
		/// </summary>
		private int _meter_InstalledCTSecondary;


        ///<summary>
        ///private variable for meter installed CT Ratio
        /// </summary>
        private int meterInstalledCTRatio;

        ///<summary>
        ///private variable for meter installed PT Ratio
        /// </summary>
        private int meterInstalledPTRatio;
		/// <summary>
		/// Private variable for meter Installed PT Primary.
		/// </summary>
		private int _meter_InstalledPTPrimary;

		/// <summary>
		/// Private variable for meter Installed PT Secondary.
		/// </summary>
		private int _meter_InstalledPTSecondary;

		/// <summary>
		/// Private variable for meter Phone.
		/// </summary>
		private string _meter_Phone;

		/// <summary>
		/// Private variable for meter Status.
		/// </summary>
		private int _meter_Status;

        /// <summary>
        /// Private variable for meter Status.
        /// </summary>
        private int useEMFInCalculations;

        /// <summary>
        ///private variable of internal CT Ratio
        /// </summary>
        private int internalCTRatio;

        /// <summary>
        /// private variable of internal PT Ratio
        /// </summary>
        private int internalPTRatio;
        /// <summary>
        /// This property is used for getting and setting the value of meter type
        /// </summary>
        private string meterType;

		/// <summary>
		/// This property is used to get and set the value of MeterID.
		/// </summary>
        private string communicationType;

     

		public string Meter_ID
		{
			get
			{
				return _meter_ID;
			}
			set
			{
				_meter_ID = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of MeterTypeID.
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
		/// This property is used to get and set the value of MeterModelID.
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
		/// This property is used to get and set the value of Meter EMF.
		/// </summary>
		public Decimal Meter_EMF
		{
			get
			{
				return _meter_EMF;
			}
			set
			{
				_meter_EMF = value;
			}
		}


		/// <summary>
		/// This property is used to get and set the value of Meter ContractDemand.
		/// </summary>
		public double Meter_ContractDemand
		{
			get
			{
				return _meter_ContractDemand;
			}
			set
			{
				_meter_ContractDemand = value;
			}
		}


		/// <summary>
		/// This property is used to get and set the value of MeterUnitID.
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

        ///<summary>
        ///This property is used to get and set the value of meter CT Ratio
        /// </summary>
        public int MeterCTRatio
        {
            get
            {
                return meterCTRatio;
            }
            set
            {
                meterCTRatio = value;
            }
        }
        ///<summary>
        ///This property is used to get and set the value of meter PT Ratio
        /// </summary>
        public int MeterPTRatio
        {
            get
            {
                return meterPTRatio;
            }
            set
            {
                meterPTRatio = value;
            }
        }
		/// <summary>
		/// This property is used to get and set the value of Meter CTPrimary.
		/// </summary>
		public int Meter_CTPrimary
		{
			get
			{
				return _meter_CTPrimary;
			}
			set
			{
				_meter_CTPrimary = value;
			}
		}


		/// <summary>
		/// This property is used to get and set the value of Meter CTSecondary.
		/// </summary>
		public int Meter_CTSecondary
		{
			get
			{
				return _meter_CTSecondary;
			}
			set
			{
				_meter_CTSecondary = value;
			}
		}
   
   
		/// <summary>
		/// This property is used to get and set the value of Meter PTPrimary.
		/// </summary>
		public int Meter_PTPrimary
		{
			get
			{
				return _meter_PTPrimary;
			}
			set
			{
				_meter_PTPrimary = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter PTSecondary.
		/// </summary>
		public int Meter_PTSecondary
		{
			get
			{
				return _meter_PTSecondary;
			}
			set
			{
				_meter_PTSecondary = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter InstalledCTPrimary.
		/// </summary>
		public int Meter_InstalledCTPrimary
		{
			get
			{
				return _meter_InstalledCTPrimary;
			}
			set
			{
				_meter_InstalledCTPrimary = value;
			}
		}
        ///<summary>
        ///This property is used to get and set the values of installed Meter PT Ratio
        /// </summary>
        public int MeterInstalledPTRatio
        {
            get
            {
                return meterInstalledPTRatio;
            }
            set
            {
                meterInstalledPTRatio = value;
            }
        }

        ///<summary>
        ///This property is used to get and set the values of installed CT Ratio
        /// </summary>
        public int MeterInstalledCTRatio
        {
            get
            {
                return meterInstalledCTRatio;
            }
            set
            {
                meterInstalledCTRatio = value;
            }
        
        }
		/// <summary>
		/// This property is used to get and set the value of Meter InstalledCTSecondary.
		/// </summary>
		public int Meter_InstalledCTSecondary
		{
			get
			{
				return _meter_InstalledCTSecondary;
			}
			set
			{
				_meter_InstalledCTSecondary = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter InstalledPTPrimary.
		/// </summary>
		public int Meter_InstalledPTPrimary
		{
			get
			{
				return _meter_InstalledPTPrimary;
			}
			set
			{
				_meter_InstalledPTPrimary = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter InstalledPTSecondary.
		/// </summary>
		public int Meter_InstalledPTSecondary
		{
			get
			{
				return _meter_InstalledPTSecondary;
			}
			set
			{
				_meter_InstalledPTSecondary = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter Phone.
		/// </summary>
		public string Meter_Phone
		{
			get
			{
				return _meter_Phone;
			}
			set
			{
				_meter_Phone = value;
			}
		}

		/// <summary>
		/// This property is used to get and set the value of Meter Status.
		/// </summary>
		public int Meter_Status
		{
			get
			{
				return _meter_Status;
			}
			set
			{
				_meter_Status = value;
			}
		}
        /// <summary>
        /// This property is used to get and set the value of UseEMFInCalculations
        /// </summary>
        public int UseEMFInCalculations
        {
            get
            {
                return useEMFInCalculations;
            }
            set
            {
                useEMFInCalculations = value;
            }
        }
        public int InternalCTRatio
        {
            get
            {
                return internalCTRatio;
            }
            set
            {
                internalCTRatio = value;
            }
        }
        public int InternalPTRatio
        {
            get
            {
                return internalPTRatio;
            }
            set
            {
                internalPTRatio = value;
            }
        }
        public string MeterType
        {
            get
            {
                return meterType;
            }
            set
            {
                meterType = value;
            }
        }
        /// <summary>
        /// Property for storing communication type
        /// </summary>
        public string CommunicationType
        {
            get
            {
                return communicationType;
            }
            set
            {
                communicationType = value;
            }
        }

        /// <summary>
        /// Gets or Sets IMEI number of GPRS modem attached to meter 
        /// </summary>
        public string MeterGPRSModemIMEI { get; set; }
        
        /// <summary>
        /// Gets or Sets Connection Type configured on Modem
        /// </summary>
        public GPRSConnectionMode? GPRSModemConnectionType { get; set; }
        
        /// <summary>
        /// Gets or Sets Ip type allocated to Modem
        /// </summary>
        public GPRSIPType? GPRSModemIpType { get; set; }
	}
}


