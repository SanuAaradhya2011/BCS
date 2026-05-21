/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class DLMS650InstantaneousEntity : EntityBase
    {
        private long instantPower_ID;
        private string instantPowerColumnName;
        private string instantPowerColumnValue;
        private string instantPowerObisCode;
        private string instantPowerClassID;
        private string instantPowerAttribute;
        private long instantPowerDataIndex;
        private long meterDataID;
       

        public long InstantPower_ID
        {
            get { return instantPower_ID; }
            set { instantPower_ID = value; }
        }

        public string InstantPowerColumnName
        {
            get { return instantPowerColumnName; }
            set { instantPowerColumnName = value; }
        }
        public string InstantPowerColumnValue
        {
            get { return instantPowerColumnValue; }
            set { instantPowerColumnValue = value; }
        }
        public string InstantPowerObisCode
        {
            get { return instantPowerObisCode; }
            set { instantPowerObisCode = value; }
        }
        public string InstantPowerClassID
        {
            get { return instantPowerClassID; }
            set { instantPowerClassID = value; }
        }
        public string InstantPowerAttribute
        {
            get { return instantPowerAttribute; }
            set { instantPowerAttribute = value; }
        }
        public long InstantPowerDataIndex
        {
            get { return instantPowerDataIndex; }
            set { instantPowerDataIndex = value; }
        }

        public long MeterDataID
        {
            get { return meterDataID; }
            set { meterDataID = value; }
        }
    }
}

