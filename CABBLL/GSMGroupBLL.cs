using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
    public class GSMGroupBLL
    {
        GSMGroupDAL gsmGroupDAL = null;
        public GSMGroupBLL()
        {
            gsmGroupDAL = new GSMGroupDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return gsmGroupDAL.InsertData(entity);
        }
        //public DataSet ListMeterSimNumbers()
        //{
        //    return gsmGroupDAL.ListMeterSimNumbers();
        //}
        public List<MeterMasterEntity> ListMeterSimNumbers(int groupID, int taskid, int taskRetries)
        {
            List<MeterMasterEntity> MeterMasterEntity = new List<MeterMasterEntity>();
            int count = 0;
            bool Status = false;
            List<GSMLoggingEntity> logsToBeDeleted = new List<GSMLoggingEntity>();
            MeterMasterEntity = gsmGroupDAL.ListMeterSimNumbers(groupID);
            List<GSMLoggingEntity> GSMLoggingEntity = new List<GSMLoggingEntity>();
            GSMLoggingEntity = ListAlreadyReadMeterNumbers(taskid, out count);
            if (GSMLoggingEntity != null)
            {
                if (count == 0)
                    return MeterMasterEntity;
                else
                {

                    foreach (GSMLoggingEntity gsmEntity in GSMLoggingEntity)
                    {
                        foreach (MeterMasterEntity meterNumbersEntity in MeterMasterEntity)
                        {
                            if (gsmEntity.Meter_ID == meterNumbersEntity.Meter_ID)
                            {
                                if (gsmEntity.Status == "C")
                                {
                                    MeterMasterEntity.Remove(meterNumbersEntity);
                                    break;
                                }
                                else if (gsmEntity.Status == "IP")
                                {
                                    logsToBeDeleted.Add(gsmEntity);
                                    break;
                                }
                                else
                                {
                                    if (gsmEntity.Retries >= taskRetries)
                                    {
                                        MeterMasterEntity.Remove(meterNumbersEntity);
                                        break;

                                    }
                                    else
                                    {
                                        logsToBeDeleted.Add(gsmEntity);
                                        break;
                                    }

                                }

                            }
                        }
                    }

                }
            }
            Status = gsmGroupDAL.DeletePendingLogs(logsToBeDeleted);
            return MeterMasterEntity;
        }
        // Added to get meters from gsm tasks log.
        public List<GSMLoggingEntity> ListAlreadyReadMeterNumbers(int taskID, out int count)
        {
            return gsmGroupDAL.ListAlreadyReadMeterNumbers(taskID, out count);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meterNumberStartsWith"></param>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public DataSet ListMeterSimNumbers(string meterNumberStartsWith,string communicationType)
        {
            return gsmGroupDAL.ListMeterSimNumbers(meterNumberStartsWith, communicationType);
        }
        public DataSet ListGroupData()
        {
            return FormatGroupType(gsmGroupDAL.ListGroupData());
        }
        private DataSet FormatGroupType(DataSet dataSet)
        {
            if (dataSet != null)
            {
                if (dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (row["Group Type"].ToString() == "A")
                            row["Group Type"] = "Area Wise";
                        else if (row["Group Type"].ToString() == "M")
                            row["Group Type"] = "Meter Wise";
                        else if (row["Group Type"].ToString() == "S")
                            row["Group Type"] = "Select All";

                    }
                }
            }
            return dataSet;

        }
        public string DeleteGroup(long groupID)
        {
            return gsmGroupDAL.DeleteGroup(groupID,CommonBLL.GetEnumDescription(TaskStatus.InProgress));
        }
        public IEntity GetGroupDatabyGroupID(int groupID)
        {
            return gsmGroupDAL.GetGroupDatabyGroupID(groupID);
        }
        public bool ValidateGroupName(string groupName)
        {
            return gsmGroupDAL.ValidateGroupName(groupName);
        }
        public bool UpdateData(GSMGroupEntity entity)
        {
            return gsmGroupDAL.UpdateData(entity);
        }
        public int GetMeterExistance(string MeterId)
        {
            return gsmGroupDAL.GetMeterExistance(MeterId);
        }
    }
}
