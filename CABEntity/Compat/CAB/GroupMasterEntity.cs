/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 														|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using LNG.Framework.Entity;

namespace LNG.Entity
{
    /// <summary>
    /// This class is used to hold the value of Group Master Table.
    /// </summary>
    public class GroupMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Group ID.
        /// </summary>
        private int _groupID;

        /// <summary>
        /// Private variable for Group Name.
        /// </summary>
        private string _groupName;

        /// <summary>
        /// This property is used to get and set the value of group id.
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


        /// <summary>
        /// This property is used to get and set the value of group Name.
        /// </summary>
        public string Group_Name
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }
    }
}

