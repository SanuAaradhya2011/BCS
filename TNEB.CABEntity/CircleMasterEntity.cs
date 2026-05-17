/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;


namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of Circle Master Table.
    /// </summary>
    public class CircleMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Circle ID.
        /// </summary>
        private ulong _circleID;

        /// <summary>
        /// Private variable for Circle Name.
        /// </summary>
        private string _circleName;

        /// <summary>
        /// Private variable for Region ID.
        /// </summary>
        private int _regionID;

       
        /// <summary>
        /// This property is used to get the value of Circle ID.
        /// </summary>
        public ulong Circle_ID
        {
            get
            {
                return _circleID;
            }
            set
            {
                _circleID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Circle Name.
        /// </summary>
        public string Circle_Name
        {
            get
            {
                return _circleName;
            }
            set
            {
                _circleName = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Region ID.
        /// </summary>
        public int Region_ID
        {
            get
            {
                return _regionID;
            }
            set
            {
                _regionID = value;
            }
        }

    }
}
