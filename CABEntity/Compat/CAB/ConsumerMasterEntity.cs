
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 16/02/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;


namespace LNG.Entity
{
    /// <summary>
    /// This class is used to hold the value of Consumer Master Table.
    /// </summary>
    public class ConsumerMasterEntity : EntityBase
    {
        /// <summary>
        /// Private variable for Consumer Number.
        /// </summary>
        private string _consumerNumber;

        /// <summary>
        /// Private variable for Consumer Name.
        /// </summary>
        private string _consumerName;

        /// <summary>
        /// Private variable for Consumer Type ID.
        /// </summary>
        private int _consumerTypeID;

        /// <summary>
        /// Private variable for Consumer Phone.
        /// </summary>
        private string _consumerPhone;

            /// <summary>
        /// Private variable for Consumer House Number.
        /// </summary>
        private string _consumerHNumber;

            /// <summary>
        /// Private variable for Consumer Street.
        /// </summary>
        private string _consumerStreet;

            /// <summary>
        /// Private variable for Consumer City.
        /// </summary>
        private string _consumerCity;


         /// <summary>
        /// Private variable for Consumer Email.
        /// </summary>
        private string _consumerEmail;

        /// <summary>
        /// This property is used to get and set the value of Consumer Number.
        /// </summary>
        public string Consumer_Number
        {
            get
            {
                return _consumerNumber;
            }
            set
            {
                _consumerNumber = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Consumer Name.
        /// </summary>
        public string Consumer_Name
        {
            get
            {
                return _consumerName;
            }
            set
            {
                _consumerName = value;
            }
        }


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
        /// This property is used to get and set the value of Consumer Phone.
        /// </summary>
        public string Consumer_Phone
        {
            get
            {
                return _consumerPhone;
            }
            set
            {
                _consumerPhone = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Consumer House Number.
        /// </summary>
        public string Consumer_HNumber
        {
            get
            {
                return _consumerHNumber;
            }
            set
            {
                _consumerHNumber = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Consumer Street.
        /// </summary>
        public string Consumer_Street
        {
            get
            {
                return _consumerStreet;
            }
            set
            {
                _consumerStreet = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Consumer City.
        /// </summary>
        public string Consumer_City
        {
            get
            {
                return _consumerCity;
            }
            set
            {
                _consumerCity = value;
            }
        }


        /// <summary>
        /// This property is used to get and set the value of Consumer Email.
        /// </summary>
        public string Consumer_Email
        {
            get
            {
                return _consumerEmail;
            }
            set
            {
                _consumerEmail = value;
            }
        }


    }
}
