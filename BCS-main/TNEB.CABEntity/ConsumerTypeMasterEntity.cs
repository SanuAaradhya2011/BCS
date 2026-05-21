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
    /// This class is used to hold the value of ConsumerTypeMaster Table.
    /// </summary>
    public class ConsumerTypeMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Consumer Type ID.
        /// </summary>
        private int _consumerTypeID;

        /// <summary>
        /// Private variable for Consumer Type Name.
        /// </summary>
        private string _consumerTypeName;

        /// <summary>
        /// This property is used to get and set the value of Consumer Type ID.
        /// </summary>
        public int ConsumerType_ID
        {
            get
            {
                return _consumerTypeID;
            }
            set
            {
                _consumerTypeID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Consumer Type Name.
        /// </summary>
        public string ConsumerType_Name
        {
            get
            {
                return _consumerTypeName;
            }
            set
            {
                _consumerTypeName = value;
            }
        }
    }
}
