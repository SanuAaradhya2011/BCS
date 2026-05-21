
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GSMGroupDAL : DALBase
    {
        private string Group_ID = "Group_ID";
        private string Meter_ID = "Meter_ID";
        private string Group_Name = "Group_Name";
        private string Group_Type = "Group_Type";
        private string Region_ID = "Region_ID";
        private string Circle_ID = "Circle_ID";
        private string Division_ID = "Division_ID";
        private string Status = "Status";
        private string Task_ID = "Task_ID";
        private string Log_ID = "Log_ID";
        private string Communication_Type = "CommunicationType";
        private string groupId = "groupId";
        private string taskStatus = "taskStatus";
        int rowAffected = 0;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GSMGroupDAL).ToString());
        public GSMGroupDAL()
            : base("gsm_groups", "Group_ID")
        {

        }
        public override IEntity InsertData(IEntity entity)
        {
            GSMGroupEntity gsmGroupEntity = null;
            if (entity == null)
                return gsmGroupEntity;
            gsmGroupEntity = entity as GSMGroupEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into gsm_groups(Group_Name,Group_Type,Region_ID,Circle_ID,Division_ID,CommunicationType) values(");
                builder.Append(string.Concat(ParameterName(Group_Name), ","));
                builder.Append(string.Concat(ParameterName(Group_Type), ","));
                builder.Append(string.Concat(ParameterName(Region_ID), ","));
                builder.Append(string.Concat(ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(ParameterName(Division_ID), ","));
                builder.Append(string.Concat(ParameterName(Communication_Type), ")"));


                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), gsmGroupEntity.GroupName, DbType.String, 50);
                request.AddParamter(ParameterName(Group_Type), gsmGroupEntity.GroupType, DbType.String, 1);
                request.AddParamter(ParameterName(Region_ID), gsmGroupEntity.RegionID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), gsmGroupEntity.CircleID, DbType.Int32);
                request.AddParamter(ParameterName(Division_ID), gsmGroupEntity.DivisionID, DbType.Int32);
                request.AddParamter(ParameterName(Communication_Type), gsmGroupEntity.CommunicationType, DbType.String);
                rowAffected = helper.ExecuteNonQuery(request);
                if (rowAffected > 0)
                {
                    StringBuilder metersBuilder = null;
                    string groupID;
                    groupID = GetPK();
                    if (!String.IsNullOrEmpty(groupID))
                        gsmGroupEntity.GroupID = Convert.ToInt64(groupID);
                    if (gsmGroupEntity.SelectedMeterList != null)
                    {
                        foreach (string selectedMeterID in gsmGroupEntity.SelectedMeterList)
                        {
                            metersBuilder = new StringBuilder();
                            metersBuilder.Append("Insert into gsm_group_meters(Group_ID,Meter_ID,Status) values(");
                            metersBuilder.Append(string.Concat(ParameterName(Group_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Meter_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Status), ")"));

                            DataRequest meterRequest = new DataRequest(metersBuilder.ToString());
                            meterRequest.AddParamter(ParameterName(Group_ID), gsmGroupEntity.GroupID, DbType.Int64);
                            meterRequest.AddParamter(ParameterName(Meter_ID), selectedMeterID, DbType.String, 20);
                            meterRequest.AddParamter(ParameterName(Status), "S", DbType.String, 1);
                            helper.ExecuteNonQuery(meterRequest);
                        }
                    }
                    if (gsmGroupEntity.AvailableMeterList != null)
                    {
                        foreach (string availableMeterID in gsmGroupEntity.AvailableMeterList)
                        {
                            metersBuilder = new StringBuilder();
                            metersBuilder.Append("Insert into gsm_group_meters(Group_ID,Meter_ID,Status) values(");
                            metersBuilder.Append(string.Concat(ParameterName(Group_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Meter_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Status), ")"));

                            DataRequest availableMeterRequest = new DataRequest(metersBuilder.ToString());
                            availableMeterRequest.AddParamter(ParameterName(Group_ID), gsmGroupEntity.GroupID, DbType.Int64);
                            availableMeterRequest.AddParamter(ParameterName(Meter_ID), availableMeterID, DbType.String, 20);
                            availableMeterRequest.AddParamter(ParameterName(Status), "A", DbType.String, 1);
                            helper.ExecuteNonQuery(availableMeterRequest);
                        }
                    }

                }
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("GSM Group inserted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                gsmGroupEntity = null;
            }
            return gsmGroupEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            GSMGroupEntity gsmGroupEntity = null;
            if (entity == null)
                return false;
            gsmGroupEntity = entity as GSMGroupEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update gsm_groups set ");
                builder.Append(string.Concat(Group_Name, " = ", ParameterName(Group_Name), ","));
                builder.Append(string.Concat(Group_Type, " = ", ParameterName(Group_Type), ","));
                builder.Append(string.Concat(Region_ID, " = ", ParameterName(Region_ID), ","));
                builder.Append(string.Concat(Circle_ID, " = ", ParameterName(Circle_ID), ","));
                builder.Append(string.Concat(Division_ID, " = ", ParameterName(Division_ID), ","));
                builder.Append(string.Concat(Communication_Type, " = ", ParameterName(Communication_Type)));
                builder.Append(string.Concat(" where ", Group_ID, " = ", ParameterName(Group_ID)));

                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), gsmGroupEntity.GroupName, DbType.String, 20);
                request.AddParamter(ParameterName(Group_Type), gsmGroupEntity.GroupType, DbType.String, 1);
                request.AddParamter(ParameterName(Region_ID), gsmGroupEntity.RegionID, DbType.Int32);
                request.AddParamter(ParameterName(Circle_ID), gsmGroupEntity.CircleID, DbType.Int32);
                request.AddParamter(ParameterName(Division_ID), gsmGroupEntity.DivisionID, DbType.Int32);
                request.AddParamter(ParameterName(Communication_Type), gsmGroupEntity.CommunicationType, DbType.String);
                request.AddParamter(ParameterName(Group_ID), gsmGroupEntity.GroupID, DbType.Int64);
                rowAffected = helper.ExecuteNonQuery(request);

                StringBuilder metersBuilder = null;
                if (DeleteGroupMeters(gsmGroupEntity.GroupID))
                {
                    if (gsmGroupEntity.SelectedMeterList != null)
                    {
                        foreach (string selectedMeterID in gsmGroupEntity.SelectedMeterList)
                        {
                            metersBuilder = new StringBuilder();
                            metersBuilder.Append("Insert into gsm_group_meters(Group_ID,Meter_ID,Status) values(");
                            metersBuilder.Append(string.Concat(ParameterName(Group_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Meter_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Status), ")"));

                            DataRequest meterRequest = new DataRequest(metersBuilder.ToString());
                            meterRequest.AddParamter(ParameterName(Group_ID), gsmGroupEntity.GroupID, DbType.Int64);
                            meterRequest.AddParamter(ParameterName(Meter_ID), selectedMeterID, DbType.String, 20);
                            meterRequest.AddParamter(ParameterName(Status), "S", DbType.String, 1);
                            helper.ExecuteNonQuery(meterRequest);
                        }
                    }
                    if (gsmGroupEntity.AvailableMeterList != null)
                    {
                        foreach (string availableMeterID in gsmGroupEntity.AvailableMeterList)
                        {
                            metersBuilder = new StringBuilder();
                            metersBuilder.Append("Insert into gsm_group_meters(Group_ID,Meter_ID,Status) values(");
                            metersBuilder.Append(string.Concat(ParameterName(Group_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Meter_ID), ","));
                            metersBuilder.Append(string.Concat(ParameterName(Status), ")"));

                            DataRequest availableMeterRequest = new DataRequest(metersBuilder.ToString());
                            availableMeterRequest.AddParamter(ParameterName(Group_ID), gsmGroupEntity.GroupID, DbType.Int64);
                            availableMeterRequest.AddParamter(ParameterName(Meter_ID), availableMeterID, DbType.String, 20);
                            availableMeterRequest.AddParamter(ParameterName(Status), "A", DbType.String, 1);
                            helper.ExecuteNonQuery(availableMeterRequest);
                        }
                    }
                }
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                return false;
            }
            return true;
        }
        public DataSet ListGroupData()
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Group_ID as 'Group ID', Group_Name as 'Group Name',Group_Type as 'Group Type',CommunicationType  from gsm_groups g left outer join region_master r on g.Region_ID=r.Region_ID left outer join circle_master c on g.Circle_ID  = c.Circle_ID left outer join division_master d on g.Division_ID=d.Division_ID");
                //builder.Append("Select Group_ID as 'Group ID',Group_Name as 'Group Name',Group_Type as 'Group Type',r.Region_Name as 'Region',c.Circle_Name as 'Circle',d.Division_Name as 'Division' from gsm_groups g left outer join region_master r on g.Region_ID=r.Region_ID left outer join circle_master c on g.Circle_ID  = c.Circle_ID left outer join division_master d on g.Division_ID=d.Division_ID");
                DataRequest request = new DataRequest(builder.ToString());
                ds = new DataSet();
                helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListGroupData()", ex);
                ds = null;
            }
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterNumberStartsWith"></param>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public DataSet ListMeterSimNumbers(string meterNumberStartsWith,string communicationType)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                //BhardwajG : If the communication type is GSM or PSTN
                //BhardwajG according to communication type selected.
                builder.Append("Select m.Meter_ID from meter_master m join consumermeter c on m.Meter_ID = c.Meter_ID where c.Status = 1 and c.Communication_Type in ('" + communicationType.ToString() + "') and m.Meter_ID LIKE ");
                builder.Append(ParameterName(Meter_ID));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Meter_ID), meterNumberStartsWith + "%", DbType.String);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListMeterSimNumbers(string meterNumberStartsWith,string communicationType)", ex);
                ds = null;
            }
            return ds;
        }
        public List<MeterMasterEntity> ListMeterSimNumbers(int groupID)
        {
            DataSet ds = null;
            List<MeterMasterEntity> meterMasterEntityList = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //BhardwajG : If the communication type is GSM or PSTN and include communication type in select list
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("Select m.Meter_Phone,m.Meter_ID,c.Communication_Type,m.Meter_GPRSModem_IMEI from gsm_groups gg join gsm_group_meters gm on gg.Group_ID = gm.Group_ID");
                builder.Append(" join meter_master m on gm.Meter_ID = m.Meter_ID join consumermeter c on m.Meter_ID = c.Meter_ID");
                builder.Append(" where c.Status = 1 and c.Communication_Type in ('" + CommunicationType.GSM.ToString() + "','" + CommunicationType.GPRS.ToString() + "','" + CommunicationType.TCP.ToString() + "','"  +CommunicationType.FTP.ToString() + "','" + CommunicationType.PSTN.ToString() + "') and gm.Status='S'");
                builder.Append(" and gm.Group_ID = ");
                builder.Append(ParameterName(Group_ID));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_ID), groupID, DbType.Int32);
                ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                meterMasterEntityList = ConvertRowToEntityCollection(ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID and sim retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListMeterSimNumbers(int groupID)", ex);
                meterMasterEntityList = null;
            }
            return meterMasterEntityList;
        }
        // Added to get the meters having status log in gsm_tasks_log.
        public List<GSMLoggingEntity> ListAlreadyReadMeterNumbers(int taskID, out int count)
        {
            count = 0;
            DataSet ds = new DataSet();
            List<GSMLoggingEntity> GSMLoggingEntityList = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select g.Meter_ID,g.tasksId, g.Status,g.taskRetries,g.Log_ID,gt.taskRetries as taskRetries from gsm_task_logs g join gsm_tasks gt on g.tasksId = gt.tasksId  where g.tasksId = ");
                builder.Append(ParameterName(Task_ID));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Task_ID), taskID, DbType.Int32);
                ds = helper.FillDataSet(request, ds);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        count = ds.Tables[0].Rows.Count;
                    }
                    else
                        count = 0;

                }
                GSMLoggingEntityList = ConvertRowToEntityCollectionForGSM(ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter ID retrieved"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListAlreadyReadMeterNumbers(int taskID, out int count)", ex);
                throw ex;
            }
            return GSMLoggingEntityList;
        }
        public bool DeletePendingLogs(List<GSMLoggingEntity> toBeDeletedLogs)
        {
            bool Flag = false;
            foreach (GSMLoggingEntity entity in toBeDeletedLogs)
            {

                try
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    StringBuilder builder2 = new StringBuilder();
                    builder2.Append("Delete from gsm_task_logs ");
                    builder2.Append(string.Concat(" Where ", Log_ID, "=", ParameterName(Log_ID)));
                    DataRequest request2 = new DataRequest(builder2.ToString());
                    request2.AddParamter(ParameterName(Log_ID), entity.Log_ID, DbType.Int64);
                    helper.ExecuteNonQuery(request2);

                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Pending logs deleted"));
                    Flag = true;


                }
                catch (CABException ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "DeletePendingLogs(List<GSMLoggingEntity> toBeDeletedLogs)", ex);
                    Flag = false;
                }

            }
            return Flag;

        }

        public List<MeterMasterEntity> ConvertRowToEntityCollection(DataSet dataSet)
        {
            List<MeterMasterEntity> meterMasterEntityList = null;
            MeterMasterEntity metermasterEntity = null;
            if (dataSet != null)
            {

                if (dataSet.Tables.Count > 0)
                {
                    meterMasterEntityList = new List<MeterMasterEntity>();
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        metermasterEntity = new MeterMasterEntity();
                        metermasterEntity.Meter_Phone = row["Meter_Phone"].ToString();
                        metermasterEntity.Meter_ID = row["Meter_ID"].ToString();
                        //BhardwajG : 
                        metermasterEntity.CommunicationType = row["Communication_Type"].ToString();
                        metermasterEntity.MeterGPRSModemIMEI = row["Meter_GPRSModem_IMEI"].ToString();
                        meterMasterEntityList.Add(metermasterEntity);
                    }

                }
            }
            return meterMasterEntityList;
        }
        public List<GSMLoggingEntity> ConvertRowToEntityCollectionForGSM(DataSet dataSet)
        {
            List<GSMLoggingEntity> GSMLoggingEntityList = null;
            GSMLoggingEntity GSMLoggingEntity = null;
            if (dataSet != null)
            {

                if (dataSet.Tables.Count > 0)
                {
                    GSMLoggingEntityList = new List<GSMLoggingEntity>();
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        GSMLoggingEntity = new GSMLoggingEntity();
                        GSMLoggingEntity.Status = row["Status"].ToString();
                        GSMLoggingEntity.Meter_ID = row["Meter_ID"].ToString();
                        GSMLoggingEntity.Retries = (int)row["taskRetries"];
                        GSMLoggingEntity.Log_ID = Convert.ToInt32(row["Log_ID"]);
                        GSMLoggingEntityList.Add(GSMLoggingEntity);
                    }

                }
            }
            GSMLoggingEntityList.Add(GSMLoggingEntity);
            return GSMLoggingEntityList;
        }
        public bool ValidateGroupName(string groupName)
        {
            DataSet ds = null;
            bool groupNameExist = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("Select Group_ID from gsm_groups where ");
                builder.Append(string.Concat(Group_Name, " = ", ParameterName(Group_Name)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_Name), groupName, DbType.String);
                ds = new DataSet();
                helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateGroupName(string groupName)", ex);
                return groupNameExist;
            }
            if (ds != null)
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        groupNameExist = true;
            return groupNameExist;
        }
        public GSMGroupEntity ConvertRowToEntity(DataSet dataSet)
        {

            GSMGroupEntity gsmGroupEntity = null;
            int counter = 0;
            gsmGroupEntity = new GSMGroupEntity();
            gsmGroupEntity.AvailableMeterList = new List<string>();
            gsmGroupEntity.SelectedMeterList = new List<string>();
            if (dataSet != null)
            {
                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {

                        if (counter < 1)
                        {
                            if (NotNullAndNotDBNull(row, "Group ID"))
                                gsmGroupEntity.GroupID = Convert.ToInt64(row["Group ID"]);
                            if (NotNullAndNotDBNull(row, "Group Name"))
                                gsmGroupEntity.GroupName = row["Group Name"].ToString();
                            if (NotNullAndNotDBNull(row, "Group Type"))
                                gsmGroupEntity.GroupType = row["Group Type"].ToString();
                            if (NotNullAndNotDBNull(row, "Region ID"))
                                gsmGroupEntity.RegionID = Convert.ToInt32(row["Region ID"]);
                            if (NotNullAndNotDBNull(row, "Circle ID"))
                                gsmGroupEntity.CircleID = Convert.ToInt32(row["Circle ID"]);
                            if (NotNullAndNotDBNull(row, "Division ID"))
                                gsmGroupEntity.DivisionID = Convert.ToInt32(row["Division ID"]);
                            if (NotNullAndNotDBNull(row, "Communication Type"))
                                gsmGroupEntity.CommunicationType = row["Communication Type"].ToString();
                            if (NotNullAndNotDBNull(row, "Status"))
                                if (row["Status"].ToString() == "A")
                                    gsmGroupEntity.AvailableMeterList.Add(row["Meter ID"].ToString());
                                else
                                    gsmGroupEntity.SelectedMeterList.Add(row["Meter ID"].ToString());
                        }
                        else
                        {
                            if (NotNullAndNotDBNull(row, "Status"))
                                if (row["Status"].ToString() == "A")
                                    gsmGroupEntity.AvailableMeterList.Add(row["Meter ID"].ToString());
                                else
                                    gsmGroupEntity.SelectedMeterList.Add(row["Meter ID"].ToString());

                        }
                        counter++;
                    }

                }
            }
            return gsmGroupEntity;
        }

        public int GetMeterExistance(string MeterId)
        {
            int rows = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from gsm_group_meters where Meter_ID = '" + MeterId + "'");

                DataRequest request = new DataRequest(builder.ToString());
                object count = helper.ExecuteScalar(request);

                if (count != null && Convert.ToInt32(count) > 0)
                    rows = 1;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterExistance(string MeterId)", ex);
                rows = 0;
            }
            return rows;
        }
        public string DeleteGroup(long groupID,string inputTaskStatus)
        {
            string Flag = string.Empty;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();

                StringBuilder builder1 = new StringBuilder();
                builder1.Append("Select count(*) from gsm_tasks where ");
                builder1.Append(string.Concat(groupId, "=", ParameterName(groupId), " and "));
                builder1.Append(string.Concat(taskStatus, "=", ParameterName(taskStatus)));
                DataRequest request1 = new DataRequest(builder1.ToString());
                request1.AddParamter(ParameterName(groupId), groupID, DbType.Int64);
                request1.AddParamter(ParameterName(taskStatus), inputTaskStatus, DbType.String);
                object count = helper.ExecuteScalar(request1);

                if (count != null && Convert.ToInt32(count) > 0)
                    Flag = "Exist";
                else
                {
               
                    StringBuilder builder2 = new StringBuilder();
                    builder2.Append("Delete from gsm_groups ");
                    builder2.Append(string.Concat(" Where ", Group_ID, "=", ParameterName(Group_ID)));
                    DataRequest request2 = new DataRequest(builder2.ToString());
                    request2.AddParamter(ParameterName(Group_ID), groupID, DbType.Int64);
                    helper.ExecuteNonQuery(request2);
                    Flag = "Success";
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected group deleted"));
                }
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteGroup(long groupID,string inputTaskStatus)", ex);
                Flag = "Execption";
            }
            return Flag;
        }

        public bool DeleteGroupMeters(long groupID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from gsm_group_meters ");
                builder.Append(string.Concat(" Where ", Group_ID, "=", ParameterName(Group_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_ID), groupID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected group meters deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteGroupMeters(long groupID)", ex);
                Flag = false;
            }
            return Flag;
            //throw new System.NotImplementedException();
        }

        public IEntity GetGroupDatabyGroupID(int groupID)
        {
            DataSet ds = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                //if Meter_Status = 0 then the Inactive Meters are selected.Here the consumer Meter table and consumer_master tables are not included.
                builder.Append("Select g.Group_ID as 'Group ID',g.Group_Name as 'Group Name',g.Group_Type as 'Group Type',g.Region_ID as 'Region ID',g.Circle_ID as 'Circle ID',g.Division_ID as 'Division ID',g.CommunicationType as 'Communication Type',m.Meter_ID as 'Meter ID',m.Status as 'Status' from gsm_groups g join gsm_group_meters m on g.Group_ID = m.Group_ID");
                builder.Append(string.Concat(" where g.", Group_ID, " = ", ParameterName(Group_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Group_ID), groupID, DbType.Int64);
                ds = new DataSet();
                helper.FillDataSet(request, ds);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Group data retrieved"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGroupDatabyGroupID(int groupID)", ex);
                ds = null;
            }
            return ConvertRowToEntity(ds);
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }



        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        //public override bool UpdateData(IEntity entity)
        //{
        //    throw new NotImplementedException();
        //}
        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
