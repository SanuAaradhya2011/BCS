/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 									|
 * | 																												|
 * |											Author : Piyush Singh         									|
 * |											Date   : 10/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using CAB.Framework.Entity;

namespace CAB.Entity
{
    /// <summary>
    /// This class is used to hold the value of Category Master Table.
    /// </summary>
    public class CategoryMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Category ID.
        /// </summary>
        private int _categoryID;

        /// <summary>
        /// Private variable for Category Name.
        /// </summary>
        private string _categoryName;

        /// <summary>
        /// This property is used to get and set the value of Category ID.
        /// </summary>
        public int Category_ID
        {
            get
            {
                return _categoryID;
            }
            set
            {
                _categoryID = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Category Name.
        /// </summary>
        public string Category_Name
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }
    }
}
