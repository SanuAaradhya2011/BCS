/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of DivisionMaster Table.
    /// </summary>
    public class DivisionMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Division ID.
        /// </summary>
        private long _divisionID;

        /// <summary>
        /// Private variable for Division Name.
        /// </summary>
        private string _divisionName;

        /// <summary>
        /// Private variable for Circle ID.
        /// </summary>
        private long  _circleID;

        /// <summary>
        /// This property is used to get and set the value of Division ID.
        /// </summary>
        public long Division_ID
        {
            get
            {
                return _divisionID;
            }
            set
            {
                _divisionID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Division Name.
        /// </summary>
        public string Division_Name
        {
            get
            {
                return _divisionName;
            }
            set
            {
                _divisionName = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Circle ID.
        /// </summary>
        public long Circle_ID
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
        
    }
}  