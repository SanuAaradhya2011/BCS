/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class BillingTariffInformationDAL : DALBase
    {

        private string BillingTariffInformation_ID = "BillingTariffInformation_ID";
        private string AveragePowerFactor = "AveragePowerFactor";
        private string Tariff_Number = "Tariff_Number";
        private string History_ID = "History_ID";
        private string MeterData_ID = "MeterData_ID";
        private string CumulativeEnergy_ID = "CumulativeEnergy_ID";
        private string CumulativeDemandBillingTimeStamp_ID = "CumulativeDemandBillingTimeStamp_ID";
        private string BillingFactor_ID = "BillingFactor_ID";
        public BillingTariffInformationDAL()
            : base("MeterData_BillingTariffInformation", "BillingTariffInformation_ID")
        {
        }
        
        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            BillingTariffInformationEntity billingTariffInformationEntity = entity as BillingTariffInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_BillingTariffInformation(AveragePowerFactor,Tariff_Number,History_ID,MeterData_ID,CumulativeEnergy_ID,CumulativeDemandBillingTimeStamp_ID,BillingFactor_ID) values(");
                builder.Append(string.Concat(ParameterName(AveragePowerFactor), ","));
                builder.Append(string.Concat(ParameterName(Tariff_Number), ","));
                builder.Append(string.Concat(ParameterName(History_ID), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ","));
                builder.Append(string.Concat(ParameterName(CumulativeEnergy_ID), ","));
                builder.Append(string.Concat(ParameterName(CumulativeDemandBillingTimeStamp_ID), ",")); 
                builder.Append(string.Concat(ParameterName(BillingFactor_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(AveragePowerFactor), billingTariffInformationEntity.AveragePowerFactor, DbType.String, 40);
                request.AddParamter(ParameterName(Tariff_Number), billingTariffInformationEntity.Tariff_Number, DbType.Int32);
                request.AddParamter(ParameterName(History_ID), billingTariffInformationEntity.History_ID, DbType.Int64);
                request.AddParamter(ParameterName(MeterData_ID), billingTariffInformationEntity.MeterData_ID, DbType.Int64);
                request.AddParamter(ParameterName(CumulativeEnergy_ID), billingTariffInformationEntity.CumulativeEnergy_ID, DbType.Int64);
                request.AddParamter(ParameterName(CumulativeDemandBillingTimeStamp_ID), billingTariffInformationEntity.CumulativeDemandBillingTimeStamp_ID, DbType.Int64);
                request.AddParamter(ParameterName(BillingFactor_ID), billingTariffInformationEntity.BillingFactor_ID, DbType.Int64); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Tariff Information Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                billingTariffInformationEntity.BillingTariffInformation_ID = long.Parse(this.GetPK());
            return billingTariffInformationEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            BillingTariffInformationEntity billingTariffInformationEntity = entity as BillingTariffInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_BillingTariffInformation where ");
                builder.Append(string.Concat(BillingTariffInformation_ID, "=", ParameterName(BillingTariffInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(BillingTariffInformation_ID), billingTariffInformationEntity.BillingTariffInformation_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Tariff Information Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            BillingTariffInformationEntity billingTariffInformationEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select BillingTariffInformation_ID,AveragePowerFactor,Tariff_Number,History_ID,MeterData_ID,CumulativeEnergy_ID,CumulativeDemandBillingTimeStamp_ID,BillingFactor_ID from MeterData_BillingTariffInformation where ");
                builder.Append(string.Concat(BillingTariffInformation_ID, "=", ParameterName(BillingTariffInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(BillingTariffInformation_ID), id, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    billingTariffInformationEntity = (BillingTariffInformationEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Tariff Information viewed."));

            }
            catch (CABException)
            {
            }
            return billingTariffInformationEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select BillingTariffInformation_ID,AveragePowerFactor,Tariff_Number,History_ID,MeterData_ID,CumulativeEnergy_ID,CumulativeDemandBillingTimeStamp_ID,BillingFactor_ID from MeterData_BillingTariffInformation ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Billing Tariff Information viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            BillingTariffInformationEntity billingTariffInformationEntity = new BillingTariffInformationEntity();
            if (NotNullAndNotDBNull(row, BillingTariffInformation_ID)) billingTariffInformationEntity.BillingTariffInformation_ID = Convert.ToInt64(row[BillingTariffInformation_ID]);
            if (NotNullAndNotDBNull(row, AveragePowerFactor)) billingTariffInformationEntity.AveragePowerFactor = Convert.ToString(row[AveragePowerFactor]);
            if (NotNullAndNotDBNull(row, Tariff_Number)) billingTariffInformationEntity.Tariff_Number = Convert.ToInt32(row[Tariff_Number]);
            if (NotNullAndNotDBNull(row, History_ID)) billingTariffInformationEntity.History_ID = Convert.ToInt64(row[History_ID]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) billingTariffInformationEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            if (NotNullAndNotDBNull(row, CumulativeEnergy_ID)) billingTariffInformationEntity.CumulativeEnergy_ID = Convert.ToInt64(row[CumulativeEnergy_ID]);
            if (NotNullAndNotDBNull(row, CumulativeDemandBillingTimeStamp_ID)) billingTariffInformationEntity.CumulativeDemandBillingTimeStamp_ID = Convert.ToInt64(row[CumulativeDemandBillingTimeStamp_ID]);
            if (NotNullAndNotDBNull(row, BillingFactor_ID)) billingTariffInformationEntity.BillingFactor_ID = Convert.ToInt64(row[BillingFactor_ID]);
            return billingTariffInformationEntity;
        }
    }
}
