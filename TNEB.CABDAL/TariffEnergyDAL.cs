/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon								|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
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
    public class TariffEnergyDAL : DALBase
    {
        private string TariffEnergy_ID = "TariffEnergy_ID";
        private string Kwh = "Kwh";
        private string KVARhLag = "KVARhLag";
        private string KVARhLead = "KVARhLead";
        private string KVAh = "KVAh";
        private string TariffInformation_ID = "TariffInformation_ID";
        private string BillingTariffInformation_ID = "BillingTariffInformation_ID";
        public TariffEnergyDAL()
            : base("MeterData_TariffEnergy", "TariffEnergy_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            TariffEnergyEntity tariffEnergyEntity = entity as TariffEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_TariffEnergy(Kwh,KVARhLag,KVARhLead,KVAh,TariffInformation_ID,BillingTariffInformation_ID) values(");
                builder.Append(string.Concat(ParameterName(Kwh), ","));
                builder.Append(string.Concat(ParameterName(KVARhLag), ","));
                builder.Append(string.Concat(ParameterName(KVARhLead), ","));
                builder.Append(string.Concat(ParameterName(KVAh), ","));
                builder.Append(string.Concat(ParameterName(TariffInformation_ID), ","));
                builder.Append(string.Concat(ParameterName(BillingTariffInformation_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Kwh), tariffEnergyEntity.Kwh, DbType.String, 40);
                request.AddParamter(ParameterName(KVARhLag), tariffEnergyEntity.KVARhLag, DbType.String, 40);
                request.AddParamter(ParameterName(KVARhLead), tariffEnergyEntity.KVARhLead, DbType.String, 40);
                request.AddParamter(ParameterName(KVAh), tariffEnergyEntity.KVAh, DbType.String, 40);
                request.AddParamter(ParameterName(TariffInformation_ID), tariffEnergyEntity.TariffInformation_ID, DbType.Int32);
                request.AddParamter(ParameterName(BillingTariffInformation_ID), tariffEnergyEntity.BillingTariffInformation_ID, DbType.Int32);
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Energy Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tariffEnergyEntity.TariffEnergy_ID = long.Parse(this.GetPK());
            return tariffEnergyEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            TariffEnergyEntity tariffEnergyEntity = entity as TariffEnergyEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TariffEnergy where ");
                builder.Append(string.Concat(TariffEnergy_ID, "=", ParameterName(TariffEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffEnergy_ID), tariffEnergyEntity.TariffEnergy_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Energy Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TariffEnergyEntity tariffEnergyEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TariffEnergy_ID,Kwh,KVARhLag,KVARhLead,KVAh,TariffInformation_ID,BillingTariffInformation_ID  from MeterData_TariffEnergy where ");
                builder.Append(string.Concat(TariffEnergy_ID, "=", ParameterName(TariffEnergy_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffEnergy_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tariffEnergyEntity = (TariffEnergyEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Energy viewed."));

            }
            catch (CABException)
            {
            }
            return tariffEnergyEntity;
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
                builder.Append("Select TariffEnergy_ID,Kwh,KVARhLag,KVARhLead,KVAh,TariffInformation_ID,BillingTariffInformation_ID  from MeterData_TariffEnergy ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Energy viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TariffEnergyEntity tariffEnergyEntity = new TariffEnergyEntity();
            if (NotNullAndNotDBNull(row, TariffEnergy_ID)) tariffEnergyEntity.TariffEnergy_ID = Convert.ToInt32(row[TariffEnergy_ID]);
            if (NotNullAndNotDBNull(row, Kwh)) tariffEnergyEntity.Kwh = Convert.ToString(row[Kwh]);
            if (NotNullAndNotDBNull(row, KVARhLag)) tariffEnergyEntity.KVARhLag = Convert.ToString(row[KVARhLag]);
            if (NotNullAndNotDBNull(row, KVARhLead)) tariffEnergyEntity.KVARhLead = Convert.ToString(row[KVARhLead]);
            if (NotNullAndNotDBNull(row, KVAh)) tariffEnergyEntity.KVAh = Convert.ToString(row[KVAh]);
            if (NotNullAndNotDBNull(row, TariffInformation_ID)) tariffEnergyEntity.TariffInformation_ID = Convert.ToInt32(row[TariffInformation_ID]);
            if (NotNullAndNotDBNull(row, BillingTariffInformation_ID)) tariffEnergyEntity.BillingTariffInformation_ID = Convert.ToInt32(row[BillingTariffInformation_ID]);
            return tariffEnergyEntity;
        }
    }
}
