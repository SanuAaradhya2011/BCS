#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using Hunt.EPIC.Logging;
#endregion

namespace LTCTDAL
{
    public class SoftwareBillingDAL : DALBase
    {

        private string MeterID = "MeterID";
        private string SoftwareBillingId = "SoftwareBillingId";
        private string FileUploadID = "FileUploadID";
        private string SoftwareBillingStatus = "SoftwareBillingStatus";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SoftwareBillingDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            SoftwareBillingEntity softwareBillingEntity = entity as SoftwareBillingEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into SoftwareBilling(MeterData_ID,SoftwareBillingStatus) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(SoftwareBillingStatus), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), softwareBillingEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(SoftwareBillingStatus), softwareBillingEntity.SoftwareBillingStatus.ToString(), DbType.String, 20);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("SoftwareBilling lock configuration inserted"));

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return softwareBillingEntity;

        }
        public SoftwareBillingEntity GetData(Int64 MeterData_ID)
        {
            SoftwareBillingEntity SoftwareBillingEntity = new SoftwareBillingEntity();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from SoftwareBilling where  MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                if (iDataset.Tables.Count != 0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    // SoftwareBillingEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["SoftwareBillingStatus"] != null)
                        SoftwareBillingEntity.SoftwareBillingStatus = iDataset.Tables[0].Rows[0]["SoftwareBillingStatus"].ToString();

                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Software Billing read from DB"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return null;
            }
            return SoftwareBillingEntity;
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
        /// <summary>
        /// Delete Data 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from SoftwareBilling where ");
                builder.Append(string.Concat(MeterDataID, "=", ParameterName(MeterDataID.ToString())));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), meterDataID, DbType.Int32);
                int i = helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Software Billing data for a specified meter deleted."));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(long meterDataID)", ex);
            }
            return Flag;
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
