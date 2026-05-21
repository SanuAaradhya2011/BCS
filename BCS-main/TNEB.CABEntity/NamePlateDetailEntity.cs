/* 
 * |--------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 						|
 * | 																										|
 * |											Author : Piyush Singh. 	 						|
 * | 																										|
 * |--------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class NamePlateDetailEntity : EntityBase
    {
        private long namePlateDetailID;
        public long NamePlateDetailID
        {
            get
            {
                return namePlateDetailID;
            }
            set
            {
                namePlateDetailID = value;
            }
        }
        private string meterID;
        public string MeterID
        {
            get
            {
                return meterID;
            }
            set
            {
                meterID = value;
            }
        }
        private string meterType;
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
        private string currentRating;
        public string CurrentRating
        {
            get
            {
                return currentRating;
            }
            set
            {
                currentRating = value;
            }
        }
        private string voltageRating;
        public string VoltageRating
        {
            get
            {
                return voltageRating;
            }
            set
            {
                voltageRating = value;
            }
        }
        private string meterConstant;
        public string MeterConstant
        {
            get
            {
                return meterConstant;
            }
            set
            {
                meterConstant = value;
            }
        }
        private string manufacturingDate;
        public string ManufacturingDate
        {
            get
            {
                return manufacturingDate;
            }
            set
            {
                manufacturingDate = value;
            }
        }
        private long mMeterData_ID;
        public long MeterData_ID
        {
            get
            {
                return mMeterData_ID;
            }
            set
            {
                mMeterData_ID = value;
            }
        }
        public long ReadingDateTime { get; set; } 
        private string cmriID;
        public string CMRIID
        {

            get
            {
                return cmriID;
            }
            set
            {
                cmriID = value;
            }
        }
        private string cmriType;
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
