/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using LNG.Framework.Entity;

namespace LNG.Entity
{
    /// <summary>
    /// This class is used to hold the value of DesignationMaster Table.
    /// </summary>
    public class DesignationMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Designation ID.
        /// </summary>
        private int _designationID;

        /// <summary>
        /// Private variable for Designation Name.
        /// </summary>
        private string _designationName;

        /// <summary>
        /// This property is used to get and set the value of Designation ID.
        /// </summary>
        public int Designation_ID
        {
            get
            {
                return _designationID;
            }
            set
            {
                _designationID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Designation Name.
        /// </summary>
        public string Designation_Name
        {
            get
            {
                return _designationName;
            }
            set
            {
                _designationName = value;
            }
        }
    }
}

    
