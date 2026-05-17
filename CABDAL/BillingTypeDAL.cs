using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using CAB.Framework.Entity;
using CAB.Framework;
using Hunt.EPIC.Logging;

namespace LTCTDAL
{
    public class BillingTypeDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string ModeOfBilling = "ModeOfBilling";
        private string BillingPeriod = "BillingPeriod";
        private string Day = "Day";
        private string Hours = "Hours";
        private string Minutes = "Minutes";
        private string BillingType = "BillingType";
        private string ResetLockOutDays = "ResetLockOutDays";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingTypeDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            BillingTypeEntity billingTypeEntity = entity as BillingTypeEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into BillingType(MeterData_ID,ModeOfBilling,BillingPeriod,Day,Hours,Minutes,BillingType,ResetLockOutDays) values(");
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(ModeOfBilling), ","));
                builder.Append(string.Concat(ParameterName(BillingPeriod), ","));
                builder.Append(string.Concat(ParameterName(Day), ","));
                builder.Append(string.Concat(ParameterName(Hours), ","));
                builder.Append(string.Concat(ParameterName(Minutes), ","));
                builder.Append(string.Concat(ParameterName(BillingType), ","));
                builder.Append(string.Concat(ParameterName(ResetLockOutDays), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterDataID), billingTypeEntity.MeterDataID, DbType.Int64);
                request.AddParamter(ParameterName(ModeOfBilling), billingTypeEntity.ModeOfBilling.ToString(), DbType.String, 10);
                request.AddParamter(ParameterName(BillingPeriod), billingTypeEntity.BillingPeriod, DbType.String, 10);
                request.AddParamter(ParameterName(Day), billingTypeEntity.Day, DbType.String, 10);
                request.AddParamter(ParameterName(Hours), billingTypeEntity.Hours, DbType.String, 10);
                request.AddParamter(ParameterName(Minutes), billingTypeEntity.Minutes, DbType.String, 10);
                request.AddParamter(ParameterName(BillingType), billingTypeEntity.BillingType, DbType.String, 10);
                request.AddParamter(ParameterName(ResetLockOutDays), billingTypeEntity.ResetLockOutDays == null ? string.Empty : billingTypeEntity.ResetLockOutDays, DbType.String, 10);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing Reset configuration inserted"));
                Flag = true;

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
            }
            return billingTypeEntity;


        }
        public BillingTypeEntity GetData(Int64 MeterData_ID)
        {
            BillingTypeEntity BillingResetEntity = new BillingTypeEntity();
            try
            {//ModeOfBilling,BillingPeriod,Day,Hours,Minutes,FileUploadID,ResetLockOutDays
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from BillingType where  MeterData_ID=" + MeterData_ID);
               // request.AddParamter(ParameterName(MeterDataID),MeterData_ID, DbType.Int16, 20);               
               
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count !=0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    //BillingResetEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["ModeOfBilling"] != null)
                    {
                        if (iDataset.Tables[0].Rows[0]["ModeOfBilling"].ToString().ToLower() == BillingMode.EndofMonth.ToString().ToLower())
                            BillingResetEntity.ModeOfBilling = BillingMode.EndofMonth;
                        else if (iDataset.Tables[0].Rows[0]["ModeOfBilling"].ToString().ToLower() == BillingMode.UserDefined.ToString().ToLower())
                            BillingResetEntity.ModeOfBilling = BillingMode.UserDefined;

                    }
                    if (iDataset.Tables[0].Rows[0]["BillingPeriod"] != null)
                    {
                        if (iDataset.Tables[0].Rows[0]["BillingPeriod"].ToString().ToLower() == BillingPeriod1.EvenMonth.ToString().ToLower())
                            BillingResetEntity.BillingPeriod = BillingPeriod1.EvenMonth;
                        else if (iDataset.Tables[0].Rows[0]["BillingPeriod"].ToString().ToLower() == BillingPeriod1.Monthly.ToString().ToLower())
                            BillingResetEntity.BillingPeriod = BillingPeriod1.Monthly;
                        else if (iDataset.Tables[0].Rows[0]["BillingPeriod"].ToString().ToLower() == BillingPeriod1.OddMonth.ToString().ToLower())
                            BillingResetEntity.BillingPeriod = BillingPeriod1.OddMonth;
                    }
                    if (iDataset.Tables[0].Rows[0]["Day"] != null)
                        BillingResetEntity.Day = iDataset.Tables[0].Rows[0]["Day"].ToString();
                    if (iDataset.Tables[0].Rows[0]["Hours"] != null)
                        BillingResetEntity.Hours = iDataset.Tables[0].Rows[0]["Hours"].ToString();
                    if (iDataset.Tables[0].Rows[0]["Minutes"] != null)
                        BillingResetEntity.Minutes = iDataset.Tables[0].Rows[0]["Minutes"].ToString();
                    if (iDataset.Tables[0].Rows[0]["BillingType"] != null)
                        BillingResetEntity.BillingType = iDataset.Tables[0].Rows[0]["BillingType"].ToString();
                    if (iDataset.Tables[0].Rows[0]["ResetLockOutDays"] != null)
                        BillingResetEntity.ResetLockOutDays = iDataset.Tables[0].Rows[0]["ResetLockOutDays"].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvarSelection Paramater Read from Db"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(Int64 MeterData_ID)", ex);
                return null;
            }
            return BillingResetEntity;
        }
      
         public bool DeleteData(long meterData_ID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from BillingType where ");
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
