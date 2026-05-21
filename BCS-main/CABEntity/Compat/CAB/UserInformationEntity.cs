/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 09/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */


using LNG.Framework.Entity; 

namespace LNG.Entity
{
    public class UserInformationEntity : EntityBase
    {
        /// <summary>
        /// Private members for User Information.
        /// </summary>
        private int userInformation_ID;
        private string users_Name;
        private string user_Password;
		private string user_Confirm_Password;
        private int category_ID;
        private int designation_ID;
		private string login_ID;
        private int isActive;


        /// <summary>
        /// This property is used to get and set the value of User Information ID.
        /// </summary>
        public int UserInformation_ID
        {
            get
            {
                return userInformation_ID;
            }
            set
            {
                userInformation_ID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of User Name.
        /// </summary>
        public string User_Name
        {
            get
            {
                return users_Name;
            }
            set
            {
                users_Name = value;
            }
        }

		
		/// <summary>
		/// This property is used to get and set the value of Login ID.
		/// </summary>
		public string Login_ID
		{
			get
			{
				return login_ID;
			}
			set
			{
				login_ID = value;
			}
		}

        /// <summary>
        /// This property is used to get and set the value of User Password.
        /// </summary>
        public string User_Password
        {
            get
            {
                return user_Password;
            }
            set
            {
                user_Password = value;
            }
        }

		/// <summary>
		/// This property is used to get and set the value of User Confirm Password.
		/// </summary>
		public string User_Confirm_Password
		{
			get
			{
				return user_Confirm_Password;
			}
			set
			{
				user_Confirm_Password = value;
			}
		}

        /// <summary>
        /// This property is used to get and set the value of Category ID.
        /// </summary>
        public int Category_ID
        {
            get
            {
                return category_ID;
            }
            set
            {
                category_ID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of Designation ID.
        /// </summary>
        public int Designation_ID
        {
            get
            {
                return designation_ID;
            }
            set
            {
                designation_ID = value;
            }
        }

        /// <summary>
        /// This property is used to get and set the value of IsActive flag to identify whether user is active or not.
        /// </summary>
        public int IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }
    }
}

