/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 											|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using LNG.Framework.Entity;

namespace LNG.Entity
{
	public class UserRightEntity : EntityBase
	{
		private int _right_ID;
		private int _module_ID;
		private int _userInformation_ID;
		private int _permission;

		public int Right_ID
		{
			get 
			{
				return _right_ID;
			}
			set 
			{
				_right_ID = value;
			}
		}

		public int Module_ID
		{
			get
			{
				return _module_ID;
			}
			set
			{
				_module_ID = value;
			}
		}

		public int UserInformation_ID
		{
			get
			{
				return _userInformation_ID;
			}
			set
			{
				_userInformation_ID = value;
			}
		}

		public int Permission
		{
			get
			{
				return _permission;
			}
			set
			{
				_permission = value;
			}
		}


	}
}

