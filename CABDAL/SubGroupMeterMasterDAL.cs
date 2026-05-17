/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 											|
 * | 										    Date   : 24 May 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;
using System;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
	public class SubGroupMeterMasterDAL : DALBase
	{

		private string SubGroupMeter_ID = "SubGroupMeter_ID";
		private string SubGroup_ID = "SubGroup_ID";
		private string Meter_ID = "Meter_ID";
		private string GroupAllocation_Date = "GroupAllocation_Date";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SubGroupMeterMasterDAL).ToString());

		public override IEntity InsertData(IEntity entity)
		{
			SubGroupMeterMasterEntity subGroupMeterMasterEntity = null;
			if (entity == null)
				return subGroupMeterMasterEntity;
			subGroupMeterMasterEntity = entity as SubGroupMeterMasterEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into subgroupmeter_master(SubGroup_ID,Meter_ID,GroupAllocation_Date) values(");
				builder.Append(string.Concat(ParameterName(SubGroup_ID), ","));
				builder.Append(string.Concat(ParameterName(Meter_ID), ","));
				builder.Append(string.Concat(ParameterName(GroupAllocation_Date), ")"));

				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupMeterMasterEntity.SubGroup_ID, DbType.String, 35);
				request.AddParamter(ParameterName(Meter_ID), subGroupMeterMasterEntity.Meter_ID, DbType.String, 50);
				request.AddParamter(ParameterName(GroupAllocation_Date), DateUtility.DateTimeToLong(subGroupMeterMasterEntity.GroupAllocation_Date), DbType.Int64);

				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub Group added"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
				subGroupMeterMasterEntity = null;
			}
			return subGroupMeterMasterEntity;
		}

		public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateData(IEntity entity)
		{
			bool Flag = false;
			try
			{
				
				SubGroupMeterMasterEntity subGroupMeterMasterEntity = entity as SubGroupMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Update subgroupmeter_master Set ");
				builder.Append(string.Concat(SubGroupMeter_ID, "=", ParameterName(SubGroupMeter_ID), ","));
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID), ","));
				builder.Append(string.Concat(Meter_ID, "=", ParameterName(Meter_ID)));
				builder.Append(string.Concat(GroupAllocation_Date, "=", ParameterName(GroupAllocation_Date)));

				builder.Append(string.Concat(" Where ", SubGroupMeter_ID, "=", ParameterName(SubGroupMeter_ID), " and "));
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroupMeter_ID), subGroupMeterMasterEntity.SubGroupMeter_ID, DbType.Int64);
				request.AddParamter(ParameterName(SubGroup_ID), subGroupMeterMasterEntity.SubGroup_ID, DbType.String, 35);
				request.AddParamter(ParameterName(Meter_ID), subGroupMeterMasterEntity.Meter_ID, DbType.String, 50);
				request.AddParamter(ParameterName(GroupAllocation_Date), subGroupMeterMasterEntity.GroupAllocation_Date, DbType.String, 20);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub Group updated"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

		public override bool DeleteData(IEntity entity)
		{
			bool Flag = false;
			try
			{

				SubGroupMeterMasterEntity subGroupMeterMasterEntity = entity as SubGroupMeterMasterEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete * from subgroupmeter_master where ");
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupMeterMasterEntity.SubGroup_ID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub Group deleted"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

		public bool DeleteData(int subGroupID)
		{
			bool Flag = false;
			try
			{

				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete from subgroupmeter_master where ");
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupID, DbType.Int32);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub Group deleted"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "DeleteData(int subGroupID)", ex);
				Flag = false;
			}
			return Flag;
		}



		public override IEntity GetDetailData(int id)
		{
			throw new NotImplementedException();
		}

		public override System.Collections.Generic.IList<IEntity> ListData()
		{
			throw new NotImplementedException();
		}

		public override DataSet ListDataSet()
		{
			DataSet ds = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT * from subgroupmeter_master");

				DataRequest request = new DataRequest(builder.ToString());
				ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Sub Group data retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, " ListDataSet()", ex);
				ds = null;
			}
			return ds;
		}



		public DataSet GetAssignedMeterID(int groupID, string subGroupName)
		{
			DataSet dSet = null;
			try
			{
				SubGroupMasterDAL subGroupMasterDAL = new SubGroupMasterDAL();
				int subGroupID = subGroupMasterDAL.GetSubGroupID(groupID, subGroupName);
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("SELECT Meter_ID from subgroupmeter_master where ");
				builder.Append(string.Concat(SubGroup_ID, "=", ParameterName(SubGroup_ID)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(SubGroup_ID), subGroupID, DbType.Int16);
				dSet = new DataSet();
				dSet = helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID for the specified sub group retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetAssignedMeterID(int groupID, string subGroupName)", ex);
				dSet = null;
			}
			return dSet;
		}

		public DataSet GetAllMeterValues(int groupID,string subGroupName)
		{
			DataSet dSet = null;
			try
			{
				DataSet meterDSet = new DataSet();
				meterDSet = GetAssignedMeterID(groupID, subGroupName);
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();

				builder.Append("select M.meter_ID as 'Meter ID', CM .Consumer_Number as 'Consumer ID',C.Consumer_Name as 'Consumer Name'," + 
				"MT.MeterType_Name as 'Meter Type Name',MM.MeterModel_Name as 'Meter Model Name',M.Meter_EMF as 'Meter EMF',M.Meter_ContractDemand as 'Meter Contract Demand'," +
				"MU.MeterUnit_Type as 'Meter Unit',M.Meter_CTPrimary as 'Meter CT Primary',M.Meter_CTSecondary as 'Meter CT Secondary',M.Meter_PTPrimary as 'Meter PT Primary'," +
				"M.Meter_PTSecondary as 'Meter PT Secondary',M.Meter_InstalledCTPrimary as 'Installed CT Primary',M.Meter_InstalledCTSecondary as 'Installed CT Secondary'," +
				"M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',CM.Meter_AllocationDate as 'Meter Allocation Date'," +
				"CM.Meter_Location as 'Meter Location',C.ConsumerType_ID as 'Consumer Type' from meter_master M inner join consumermeter CM on M.Meter_ID = CM.Meter_ID" +
				" inner join consumer_master C on C.Consumer_Number = CM.Consumer_Number inner Join metertype_master MT on M.MeterType_ID = MT.MeterType_ID" +
				" Inner Join MeterModel_master MM on MM.MeterModel_ID = M.MeterModel_ID Inner Join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID" + 
				" Inner Join Consumertype_Master CT on CT.ConsumerType_ID = C.ConsumerType_ID where CM.Meter_ID in (");
				
				for (int count = 0; count < meterDSet.Tables[0].Rows.Count; count++)
				{
					builder.Append(string.Concat("'",meterDSet.Tables[0].Rows[count].ItemArray.GetValue(0),"'"));
					if (count != meterDSet.Tables[0].Rows.Count - 1)
					{
						builder.Append(",");
					}
					else
					{
						builder.Append(")");
					}
				}

				dSet = new DataSet();
				DataRequest request = new DataRequest(builder.ToString());
				dSet = helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetAllMeterValues(int groupID,string subGroupName)", ex);
				dSet = null;
			}
			return dSet;
		}

		public DataSet GetAllSuspectedMeterValues()
		{
			DataSet dSet = null;
			try
			{
				DataSet meterDSet = new DataSet();
				meterDSet = GetAllSuspectedMeterID();
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();

				builder.Append("select M.meter_ID as 'Meter ID', CM .Consumer_Number as 'Consumer ID',C.Consumer_Name as 'Consumer Name'," +
				"MT.MeterType_Name as 'Meter Type Name',MM.MeterModel_Name as 'Meter Model Name',M.Meter_EMF as 'Meter EMF',M.Meter_ContractDemand as 'Meter Contract Demand'," +
				"MU.MeterUnit_Type as 'Meter Unit',M.Meter_CTPrimary as 'Meter CT Primary',M.Meter_CTSecondary as 'Meter CT Secondary',M.Meter_PTPrimary as 'Meter PT Primary'," +
				"M.Meter_PTSecondary as 'Meter PT Secondary',M.Meter_InstalledCTPrimary as 'Installed CT Primary',M.Meter_InstalledCTSecondary as 'Installed CT Secondary'," +
				"M.Meter_InstalledPTPrimary as 'Installed PT Primary',M.Meter_InstalledPTSecondary as 'Installed PT Secondary',CM.Meter_AllocationDate as 'Meter Allocation Date'," +
				"CM.Meter_Location as 'Meter Location',C.ConsumerType_ID as 'Consumer Type' from meter_master M inner join consumermeter CM on M.Meter_ID = CM.Meter_ID" +
				" inner join consumer_master C on C.Consumer_Number = CM.Consumer_Number inner Join metertype_master MT on M.MeterType_ID = MT.MeterType_ID" +
				" Inner Join MeterModel_master MM on MM.MeterModel_ID = M.MeterModel_ID Inner Join meterunit_master MU on M.MeterUnit_ID = MU.MeterUnit_ID" +
				" Inner Join Consumertype_Master CT on CT.ConsumerType_ID = C.ConsumerType_ID where CM.Status = 1 and CM.Meter_ID in (");

				for (int count = 0; count < meterDSet.Tables[0].Rows.Count; count++)
				{
					builder.Append("'");
					builder.Append(meterDSet.Tables[0].Rows[count].ItemArray.GetValue(0));
					builder.Append("'");

					if (count != meterDSet.Tables[0].Rows.Count - 1)
					{
						builder.Append(",");
					}
					else
					{
						builder.Append(")");
					}
				}

				dSet = new DataSet();
				DataRequest request = new DataRequest(builder.ToString());
				dSet = helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetAllSuspectedMeterValues()", ex);
				dSet = null;
			}
			return dSet;
		}

		public DataSet GetAllSuspectedMeterID()
		{
			DataSet dSet = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("select Meter_ID from consumermeter where Consumer_Number in (select consumer_Number from suspectedconsumer)");
				DataRequest request = new DataRequest(builder.ToString());
				dSet = new DataSet();
				dSet = helper.FillDataSet(request, dSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID for suspected meters retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetAllSuspectedMeterID()", ex);
				return null;
			}
			return dSet;
		}
	
		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
			SubGroupMeterMasterEntity subGroupMeterMasterEntity = new SubGroupMeterMasterEntity();

			if (NotNullAndNotDBNull(row, SubGroupMeter_ID)) subGroupMeterMasterEntity.SubGroupMeter_ID = (ulong)Convert.ToUInt64(row[SubGroupMeter_ID]);
			if (NotNullAndNotDBNull(row, SubGroup_ID)) subGroupMeterMasterEntity.SubGroup_ID = Convert.ToInt32(row[SubGroup_ID]);
			if (NotNullAndNotDBNull(row, Meter_ID)) subGroupMeterMasterEntity.Meter_ID = Convert.ToString(row[Meter_ID]);
			if (NotNullAndNotDBNull(row, GroupAllocation_Date)) subGroupMeterMasterEntity.GroupAllocation_Date = Convert.ToDateTime(row[GroupAllocation_Date]);

			return subGroupMeterMasterEntity;
		}

		
	}
}
