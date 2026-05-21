using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework.Entity;
using ExceptionServices.Data;
using CAB.DALC.Data;
using System.Data;
using CAB.IECFramework;

namespace LTCTDAL
{
    public class TODDAL : DALBase
    {
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool InsertData(string todData, string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();

                builder = new StringBuilder();
                builder.Append("Insert Into TOD(MeterID,MeterData_ID,FileUploadID,TODData) values(");
                builder.Append(string.Concat(ParameterName("MeterID"), ","));
                builder.Append(string.Concat(ParameterName("MeterData_ID"), ","));
                builder.Append(string.Concat(ParameterName("FileUploadID"), ","));
                builder.Append(string.Concat(ParameterName("TODData"), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("MeterID"), meterID, DbType.String);
                request.AddParamter(ParameterName("MeterData_ID"), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName("FileUploadID"), fileUploadID, DbType.String);
                request.AddParamter(ParameterName("TODData"), todData, DbType.String);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("TOD Data added"));
                return true;
            }
            catch (CABException)
            {
                return false;
            }
        }
        public string GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from TOD where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("TOD Data Read from Db"));

                if (iDataset.Tables.Count != 0 &&  iDataset.Tables[0].Rows.Count > 0)
                    return iDataset.Tables[0].Rows[0]["TODData"].ToString();
                else
                    return null;
            }
            catch (CABException)
            {
                return null;
            }
        }

        public bool DeleteAllData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("Delete from  TOD where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("TOD data Deleted"));
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
