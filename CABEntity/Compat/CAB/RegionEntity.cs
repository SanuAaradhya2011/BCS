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
    public class RegionEntity : EntityBase
    {
        private int regionID;
        private string regionName;
        public int RegionID
        {
            get
            {
                return regionID;
            }
            set
            {
                regionID = value;
            }
        }
        public string RegionName
        {
            get
            {
                return regionName;
            }
            set
            {
                regionName = value;
            }
        }

    }
}


