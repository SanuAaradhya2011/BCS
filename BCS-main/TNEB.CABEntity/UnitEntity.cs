/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity
{
    public class UnitEntity : EntityBase
    {
        private int meterUnit_ID;
        private string meterUnit_Type;
        public int MeterUnit_ID
        {
            get
            {
                return meterUnit_ID;
            }
            set
            {
                meterUnit_ID = value;
            }
        }
        public string MeterUnit_Type
        {
            get
            {
                return meterUnit_Type;
            }
            set
            {
                meterUnit_Type = value;
            }
        }

    }
}


