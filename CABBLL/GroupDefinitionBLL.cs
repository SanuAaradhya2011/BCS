using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;

namespace CAB.BLL
{
	public class GroupDefinitionBLL : IBLL
	{
		GroupMasterDAL groupMasterDAL;
		SubGroupMasterDAL subGroupMasterDAL;
		SubGroupMeterMasterDAL subGroupMeterMasterDAL;

		public GroupDefinitionBLL()
		{
			groupMasterDAL = new GroupMasterDAL();
			subGroupMasterDAL = new SubGroupMasterDAL();
			subGroupMeterMasterDAL = new SubGroupMeterMasterDAL();
		}
		
		public IEntity InsertSubGroupMasterData(SubGroupMasterEntity entity)
		{
			return subGroupMasterDAL.InsertData(entity);
		}

		public bool UpdateSubGroupMasterData(SubGroupMasterEntity entity)
		{
			return subGroupMasterDAL.UpdateData(entity);
		}

		public bool InsertSubGroupMeterMasterData(SubGroupMeterMasterEntity entity)
		{
			subGroupMeterMasterDAL.InsertData(entity);
			return true;
		}
		
		public DataSet GetAllGroupMasterData()
		{
			return groupMasterDAL.ListDataSet();
		}

		public int GetSubGroupID(int groupID, string subGroupName)
		{
			return subGroupMasterDAL.GetSubGroupID(groupID, subGroupName);
		}

		public bool GetSubGroupName(string subGroupName)
		{
			return subGroupMasterDAL.GetSubGroupName(subGroupName);
		}

		public DataSet GetAssignedMeterID(int groupID, string subGroupName)
		{
			return subGroupMeterMasterDAL.GetAssignedMeterID(groupID, subGroupName);
		}

		public DataSet GetGroupNameList(int groupID)
		{
			return subGroupMasterDAL.GetGroupNameListValues(groupID);
		}

		public DataSet GetAllGroupMeterValues(int groupID, string groupName)
		{
			return subGroupMeterMasterDAL.GetAllMeterValues(groupID, groupName);
		}

		public bool DeleteSubGroupMeterMasterValues(int groupID, string subGroupName)
		{
			int subGroupID = GetSubGroupID(groupID, subGroupName);
			return  subGroupMeterMasterDAL.DeleteData(subGroupID);
		}

		public string GetDescriptionForSubGroupName(string subGroupName)
		{
			return subGroupMasterDAL.GetDescriptionForSubGroupName(subGroupName);
		}

		public bool DeletegroupMasterValues(int subGroupID)
		{
			return subGroupMasterDAL.DeleteData(subGroupID);
		}

		public bool CheckForAllInSuspectedConsumers()
		{
            //return false;
			return subGroupMasterDAL.CheckForAllInSuspectedConsumers();
		}

		public DataSet GetAllSuspectedMeterValues()
		{
			return subGroupMeterMasterDAL.GetAllSuspectedMeterValues();
		}

        public int ListDefaultValues()
        {
            return groupMasterDAL.ListDefaultValues();
        }

        public void InsertDefaultValues(string[] defaultSubGroups)
        {
             groupMasterDAL.InsertDefaultValues(defaultSubGroups); 
        }

        public void DeleteAllData()
        {
            groupMasterDAL.DeleteAllData();
        }
	}
}
