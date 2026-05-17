using System;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;
using CAB.Framework.Utility;

namespace CAB.BLL
{
    public class GSMConfigBLL : IBLL
    {
       // private ConsumerMeterDAL consumerMeterDAL = new ConsumerMeterDAL();
        GSMConfigDAL gSMConfigDAL=new GSMConfigDAL();


        public int GetCount(long id)
        {
            return gSMConfigDAL.GetCount(id);
        }
        public bool UpdateData(IEntity entity)
        {
            return gSMConfigDAL.UpdateData(entity);
        }
        public bool InsertData(int MID, string Simno, string Status, string Reason,string Taskname)
        {
            return gSMConfigDAL.InsertDataconfig(MID, Simno, Status, Reason, Taskname);
        }
        public bool InsertReadData(int MID, string Simno, string Status, string Reason, string Taskname)
        {
            return gSMConfigDAL.InsertDataRead(MID, Simno, Status, Reason, Taskname);
        }
        public List<GSMConfigReportEntity> GetReportData(DateTime fromDate, DateTime toDate,string taskname)
        {
            return gSMConfigDAL.GetReportData(fromDate, toDate, taskname);
        }

        public List<GSMConfigReportEntity> GetReadReportData(DateTime fromDate, DateTime toDate, string taskname)
        {
            return gSMConfigDAL.GetReadReportData(fromDate, toDate, taskname);
        }
        public DataSet GetTaskIDName(DateTime fromDate, DateTime toDate)
        {
            return gSMConfigDAL.GetTaskName(fromDate, toDate);
        }
        public DataSet ReadTaskID  (DateTime fromDate, DateTime toDate)
        {
            return gSMConfigDAL.ReadTaskName(fromDate, toDate);
        }
        
        public bool UpdateTaskStatus(string MeterSrNo, string Status, string Reason, string Taskname)
        {
            return gSMConfigDAL.UpdateStatus(MeterSrNo,Status, Reason, Taskname);
        }
        public bool UpdateReadStatus(string MeterSrNo, string Status, string Reason, string Taskname)
        {
            return gSMConfigDAL.UpdateRead(MeterSrNo,Status, Reason, Taskname);
        }
        

        public bool UpdateAbortTaskStatus(string MeterSrNo, string Status, string Reason, string Taskname)
        {
            return gSMConfigDAL.UpdateAbortStatus(MeterSrNo, Status, Reason, Taskname);
        }
        
    }
}
