using System;
using System.Collections.Generic;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.IECFramework.Entity;
using CAB.DALC.Data;
using CAB.IECFramework;
using CABEntity;
using System.Collections.ObjectModel;
using System.Data;



namespace LTCTDAL
{
    public class DisplayParamaterDAL : DALBase
    {
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool InsertData(Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity, string meterID, Int64 fileUploadID,Int64 meterDataID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < collDisplayParamatersDBEntity.Count; i++)
                {
                    builder = new StringBuilder();
                    builder.Append("Insert Into displayparamater(MeterID,MeterData_ID,FileUploadID,DisplayParamaterType,DisplayParamaterName,DisplayParamaterValue) values(");
                    builder.Append(string.Concat(ParameterName("MeterID"), ","));
                    builder.Append(string.Concat(ParameterName("MeterData_ID"), ","));
                    builder.Append(string.Concat(ParameterName("FileUploadID"), ","));
                    builder.Append(string.Concat(ParameterName("DisplayParamaterType"), ","));
                    builder.Append(string.Concat(ParameterName("DisplayParamaterName"), ","));
                    builder.Append(string.Concat(ParameterName("DisplayParamaterValue"), ")"));
                    DataRequest request = new DataRequest(builder.ToString());
                    request.AddParamter(ParameterName("MeterID"), meterID, DbType.String);
                    request.AddParamter(ParameterName("MeterData_ID"), meterDataID, DbType.Int64);
                    request.AddParamter(ParameterName("FileUploadID"), fileUploadID, DbType.Int64);
                    request.AddParamter(ParameterName("DisplayParamaterType"), (int)collDisplayParamatersDBEntity[i].displayParamaterType, DbType.Int32);
                    request.AddParamter(ParameterName("DisplayParamaterName"), collDisplayParamatersDBEntity[i].paramaterName, DbType.String);
                    request.AddParamter(ParameterName("DisplayParamaterValue"), collDisplayParamatersDBEntity[i].paramaterValue, DbType.Int32);
                    helper.ExecuteNonQuery(request);
                    UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater " + collDisplayParamatersDBEntity[i].paramaterName + " added"));
                }
                return true;
            }
            catch (CABException)
            {
                return false;
            }
        }
        public Collection<Collection<string>> GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                Collection<Collection<string>> collDisplayParamaters = new Collection<Collection<string>>();
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                collDisplayParamaters.Add(new Collection<string>());
                DataRequest request = new DataRequest("select * from displayparamater where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset=helper.FillDataSet(request,new DataSet());
                int i = 0;
                while (iDataset.Tables.Count != 0 && (iDataset.Tables[0].Rows.Count > i))
                {
                    if (DisplayParameter.PushMode == ((DisplayParameter)iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[0].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else if (DisplayParameter.ScrollMode == ((DisplayParameter)iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[1].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else if (DisplayParameter.HighResolutionMode == ((DisplayParameter)iDataset.Tables[0].Rows[i]["DisplayParamaterType"]))
                        collDisplayParamaters[2].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString());
                    else
                        collDisplayParamaters[3].Add(iDataset.Tables[0].Rows[i]["DisplayParamaterName"].ToString() + "/" + iDataset.Tables[0].Rows[i]["DisplayParamaterValue"].ToString());
                    i++;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater Read from Db"));
                return collDisplayParamaters;
            }
            catch (CABException)
            {
                return null;
            }
        }

        public bool DeleteAllData(string meterID, Int64 fileUploadID,Int64 MeterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("Delete from  displayparamater where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Display Paramater data Deleted"));
                return true;
            }
            catch (CABException)
            {
                return false;
            }
        }
        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
        public override IList<IEntity> ListData()
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
