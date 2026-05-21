/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Collections;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;

namespace CAB.BLL
{
	public class UserRightBLL : IBLL
	{
		UserRightDAL userRightDAL;

		public UserRightBLL()
		{
			userRightDAL = new UserRightDAL();
		}

		public void InsertData(IList<UserRightEntity> entity)
		{
            if (entity == null)
                return;
			foreach (UserRightEntity userRightEntity in entity)
			{
				userRightDAL.InsertData(userRightEntity);
			}
		}
		public void UpdateData(IList<UserRightEntity> entity)
		{
			foreach (UserRightEntity userRightEntity in entity)
			{
                //if (((UserRightEntity)userRightDAL.GetDetailData(userRightEntity.UserInformation_ID, userRightEntity.Module_ID)).Right_ID == 0)
                //    userRightDAL.InsertData(userRightEntity);
                //else
					userRightDAL.UpdateData(userRightEntity);
			}
		}

		public IList<UserRightEntity> ListData(int userId)
		{
			return userRightDAL.ListData(userId);
		}
        public bool CheckPermission(int userID, string moduleName)
        {
            return userRightDAL.CheckPermission(userID, moduleName); 
        }
	}
}
