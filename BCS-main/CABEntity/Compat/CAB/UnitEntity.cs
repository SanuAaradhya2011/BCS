/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;


namespace LNG.Entity
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



