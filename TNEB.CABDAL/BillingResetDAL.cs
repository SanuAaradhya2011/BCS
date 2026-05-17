using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CABEntity;

namespace LTCTDAL
{
    public class BillingResetDAL : DALBase
    {
        private string MeterID = "MeterID";
        private string ModeOfBilling = "ModeOfBilling";
        private string BillingPeriod = "BillingPeriod";
        private string Day = "Day";
        private string Hours = "Hours";
        private string Minutes = "Minutes";
        private string ResetLockOutDays = "ResetLockOutDays";
        private string FileUploadID = "FileUploadID";
        private string MeterDataID = "MeterData_ID";

        public IEntity InsertData(IEntity entity, Int64 fileUploadID,Int64 MeterData_ID)
        {
            BillingResetEntity billingresetentity = entity as BillingResetEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into billingreset(MeterID,MeterData_ID,ModeOfBilling,BillingPeriod,Day,Hours,Minutes,FileUploadID,ResetLockOutDays) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterDataID), ","));
                builder.Append(string.Concat(ParameterName(ModeOfBilling), ","));
                builder.Append(string.Concat(ParameterName(BillingPeriod), ","));
                builder.Append(string.Concat(ParameterName(Day), ","));
                builder.Append(string.Concat(ParameterName(Hours), ","));
                builder.Append(string.Concat(ParameterName(Minutes), ","));
                builder.Append(string.Concat(ParameterName(FileUploadID), ","));
                builder.Append(string.Concat(ParameterName(ResetLockOutDays), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), billingresetentity.MeterID, DbType.String, 20);
                request.AddParamter(ParameterName(MeterDataID), MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(ModeOfBilling), billingresetentity.ModeOfBilling.ToString(), DbType.String, 10);
                request.AddParamter(ParameterName(BillingPeriod), billingresetentity.BillingPeriod, DbType.String, 10);
                request.AddParamter(ParameterName(Day), billingresetentity.Day, DbType.String, 10);
                request.AddParamter(ParameterName(Hours), billingresetentity.Hours, DbType.String, 10);
                request.AddParamter(ParameterName(Minutes), billingresetentity.Minutes, DbType.String, 10);
                request.AddParamter(ParameterName(FileUploadID), fileUploadID, DbType.Int64);
                request.AddParamter(ParameterName(ResetLockOutDays), billingresetentity.ResetLockOutDays, DbType.String, 10);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Billing Reset configuration inserted"));
                Flag = true;

            }
            catch (Exception)
            {
            }
            return billingresetentity;


        }
        public BillingResetEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            BillingResetEntity BillingResetEntity = new BillingResetEntity();
            try
            {//ModeOfBilling,BillingPeriod,Day,Hours,Minutes,FileUploadID,ResetLockOutDays
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("select * from billingreset where meterid='" + meterID + "' and FileUploadID='" + fileUploadID + "' and MeterData_ID=" + MeterData_ID);
                DataSet iDataset = helper.FillDataSet(request, new DataSet());
                int i = 0;
                if (iDataset.Tables.Count !=0 && iDataset.Tables[0].Rows.Count > 0)
                {
                    BillingResetEntity.MeterID = meterID;
                    if (iDataset.Tables[0].Rows[0]["ModeOfBilling"] != null)
                    {
                        if (iDataset.Tables[0].Rows[0]["ModeOfBilling"].ToString().ToLower() == IECBillingMode.EndofMonth.ToString().ToLower())
                            BillingResetEntity.ModeOfBilling = IECBillingMode.EndofMonth;
                        else if (iDataset.Tables[0].Rows[0]["ModeOfBilling"].ToString().ToLower() == IECBillingMode.UserDefined.ToString().ToLower())
                            BillingResetEntity.ModeOfBilling = IECBillingMode.UserDefined;

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
                    if (iDataset.Tables[0].Rows[0]["ResetLockOutDays"] != null)
                        BillingResetEntity.ResetLockOutDays = iDataset.Tables[0].Rows[0]["ResetLockOutDays"].ToString();
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("kvarSelection Paramater Read from Db"));
            }
            catch (CABException)
            {
                return null;
            }
            return BillingResetEntity;
        }
        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
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
