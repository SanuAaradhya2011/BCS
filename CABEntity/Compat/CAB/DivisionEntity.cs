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
    public class DivisionEntity : EntityBase
    {
        private int divisionID;
        private string divisionName;
        private int regionID;
        private int circleID;
        public int DivisionID
        {
            get
            {
                return divisionID;
            }
            set
            {
                divisionID = value;
            }
        }
        public string DivisionName
        {
            get
            {
                return divisionName;
            }
            set
            {
                divisionName = value;
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

    }
}




