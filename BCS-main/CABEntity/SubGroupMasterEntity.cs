/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.Framework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of Group Master Table.
    /// </summary>
    public class SubGroupMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Sub Group ID.
        /// </summary>
        private int _subGroupID;

        /// <summary>
        /// Private variable for Sub Group Name.
        /// </summary>
        private string _subGroupName;

        /// <summary>
        /// Private variable for Sub Group Description.
        /// </summary>
        private string _subGroupDescription;

        /// <summary>
        /// Private variable for Group ID.
        /// </summary>
        private int _groupID;

        /// <summary>
        /// This property is used to get the value of Sub Group ID.
        /// </summary>
        public int SubGroup_ID
        {
            get
            {
                return _subGroupID; 
            }
            set
            {
                _subGroupID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Sub Group Name.
        /// </summary>
        public string SubGroup_Name
        {
            get
            {
                return _subGroupName;
            }
            set
            {
                _subGroupName = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Sub Group Description.
        /// </summary>
        public string SubGroup_Description
        {
            get
            {
                return _subGroupDescription;
            }
            set
            {
                _subGroupDescription = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Group Id.
        /// </summary>
        public int Group_ID
        {
            get
            {
                return _groupID;
            }
            set
            {
                _groupID = value;
            }
        }


       
    }
}
