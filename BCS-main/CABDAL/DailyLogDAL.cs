using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using Hunt.EPIC.Logging;

namespace LTCTDAL
{
    public class DailyLogDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string CumulativeKWh = "CumulativeKWh";
        private string CumulativeKVARhLag = "CumulativeKVARhLag";
        private string CumulativeKVARhLead = "CumulativeKVARhLead";
        private string CumulativeKVAh = "CumulativeKVAh";
        private string DailyMD1 = "DailyMD1";
        private string DailyMD2 = "DailyMD2";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DailyLogDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            DailyLogEntity dailylogentity = entity as DailyLogEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into DailyLog(MeterID,MeterData_ID,CumulativeKWh,CumulativeKVARhLag,CumulativeKVARhLead,CumulativeKVAh,DailyMD1,FileUploadID,DailyMD2) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(CumulativeKWh), ","));
                builder.Append(string.Concat(ParameterName(CumulativeKVARhLag), ","));
                builder.Append(string.Concat(ParameterName(CumulativeKVARhLead), ","));
                builder.Append(string.Concat(ParameterName(CumulativeKVAh), ","));
                builder.Append(string.Concat(ParameterName(DailyMD1), ","));
                builder.Append(string.Concat(ParameterName(DailyMD2), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CumulativeKWh), dailylogentity.CumulativeKWh, DbType.String, 10);
                request.AddParamter(ParameterName(MeterDataID), MeterDataID,DbType.Int64);
                request.AddParamter(ParameterName(CumulativeKVARhLag), dailylogentity.CumulativeKVARhLag, DbType.String, 10);
                request.AddParamter(ParameterName(CumulativeKVARhLead), dailylogentity.CumulativeKVARhLead, DbType.String, 10);
                request.AddParamter(ParameterName(CumulativeKVAh), dailylogentity.CumulativeKVAh, DbType.String, 10);
                request.AddParamter(ParameterName(DailyMD1), dailylogentity.DailyMD1, DbType.String, 10);
                request.AddParamter(ParameterName(DailyMD2), dailylogentity.DailyMD2, DbType.String, 10);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Daily Log configuration inserted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return dailylogentity;
           
        }
        public DailyLogEntity GetData( Int64 MeterData_ID)
        {
            DailyLogEntity dailylogentity = new DailyLogEntity();
            try
            {//CumulativeKWh,CumulativeKVARhLag,CumulativeKVARhLead,CumulativeKVAh,DailyMD1,FileUploadID,DailyMD2
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from DailyLog where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    //dailylogentity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["CumulativeKWh"] != null)
                        dailylogentity.CumulativeKWh = iDataset.Tables[0].Rows[0]["CumulativeKWh"].ToString();
                    if (iDataset.Tables[0].Rows[0]["CumulativeKVARhLag"] != null)
                        dailylogentity.CumulativeKVARhLag = iDataset.Tables[0].Rows[0]["CumulativeKVARhLag"].ToString();
                    if (iDataset.Tables[0].Rows[0]["CumulativeKVARhLead"] != null)
                        dailylogentity.CumulativeKVARhLead = iDataset.Tables[0].Rows[0]["CumulativeKVARhLead"].ToString();
                    if (iDataset.Tables[0].Rows[0]["CumulativeKVAh"] != null)
                        dailylogentity.CumulativeKVAh = iDataset.Tables[0].Rows[0]["CumulativeKVAh"].ToString();
                    if (iDataset.Tables[0].Rows[0]["DailyMD1"] != null)
                        dailylogentity.DailyMD1 = iDataset.Tables[0].Rows[0]["DailyMD1"].ToString();
                    if (iDataset.Tables[0].Rows[0]["DailyMD2"] != null)
                        dailylogentity.DailyMD2 = iDataset.Tables[0].Rows[0]["DailyMD2"].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Daily Log Read from Db"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData( Int64 MeterData_ID)", ex);
                return null;
            }
            return dailylogentity;
        }

        public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from DailyLog where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data deleted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterData_ID)", ex);
            }
            return Flag;
        }

       
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

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
