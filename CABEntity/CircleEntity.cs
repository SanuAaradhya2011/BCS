/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.Framework.Entity;


namespace CAB.Entity
{
    public class CircleEntity : EntityBase
    {
        private int circleID;
        private string circleName;
        private int regionID;
        public int CircleID
        {
            get
            {
                return circleID;
            }
            set
            {
                circleID = value;
            }
        }
        public string CircleName
        {
            get
            {
                return circleName;
            }
            set
            {
                circleName = value;
            }
        }
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

    }
}


