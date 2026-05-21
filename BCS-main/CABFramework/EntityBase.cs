/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

namespace CAB.Framework.Entity
{
    /// <summary>
    /// Base class for All functional Entity class.
    /// </summary>
    public  class EntityBase : IEntity
    {
        /// <summary>
        /// Private variable to holds the value entity status.
        /// </summary>
        private bool _new=true;
        private bool dirty = false;
        private bool deleted = false;

        /// <summary>
        /// To Add a New Record.
        /// </summary>
        public bool New 
        { 
            get
            {
                return _new;
            }
            set
            {
                _new = value;
            }
        }

        /// <summary>
        /// To Insert/Update the record.
        /// </summary>
        public bool Dirty
        {
            get
            {
                return dirty;
            }
            set
            {
                dirty = value;
            }
        }
        /// <summary>
        /// To Delete the record.
        /// </summary>
        public bool Deleted
        {
            get
            {
                return deleted;
            }
            set
            {
                deleted = value;
            }
        }
    }
} 
