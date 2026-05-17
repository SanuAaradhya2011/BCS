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
    public class TariffDemandDAL : DALBase
    {

        private string TariffDemand_ID = "TariffDemand_ID";
        private string DemandMD1 = "DemandMD1";
        private string DemandMD1TimeStamp = "DemandMD1TimeStamp";
        private string DemandMD2 = "DemandMD2";
        private string DemandMD2TimeStamp = "DemandMD2TimeStamp";
        private string DemandMD3 = "DemandMD3";
        private string DemandMD3TimeStamp = "DemandMD3TimeStamp";
        private string TariffInformation_ID = "TariffInformation_ID";
        private string BillingTariffInformation_ID = "BillingTariffInformation_ID";

        public TariffDemandDAL()
            : base("MeterData_TariffDemand", "TariffDemand_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            TariffDemandEntity tariffDemandEntity = entity as TariffDemandEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_TariffDemand(DemandMD1,DemandMD1TimeStamp,DemandMD2,DemandMD2TimeStamp,DemandMD3,DemandMD3TimeStamp,TariffInformation_ID,BillingTariffInformation_ID) values(");
                builder.Append(string.Concat(ParameterName(DemandMD1), ","));
                builder.Append(string.Concat(ParameterName(DemandMD1TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(DemandMD2), ","));
                builder.Append(string.Concat(ParameterName(DemandMD2TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(DemandMD3), ","));
                builder.Append(string.Concat(ParameterName(DemandMD3TimeStamp), ","));
                builder.Append(string.Concat(ParameterName(TariffInformation_ID), ","));
                builder.Append(string.Concat(ParameterName(BillingTariffInformation_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(DemandMD1), tariffDemandEntity.DemandMD1, DbType.String, 40);
                request.AddParamter(ParameterName(DemandMD1TimeStamp), tariffDemandEntity.DemandMD1TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(DemandMD2), tariffDemandEntity.DemandMD2, DbType.String, 40);
                request.AddParamter(ParameterName(DemandMD2TimeStamp), tariffDemandEntity.DemandMD2TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(DemandMD3), tariffDemandEntity.DemandMD3, DbType.String, 40);
                request.AddParamter(ParameterName(DemandMD3TimeStamp), tariffDemandEntity.DemandMD3TimeStamp, DbType.String, 40);
                request.AddParamter(ParameterName(TariffInformation_ID), tariffDemandEntity.TariffInformation_ID, DbType.Int32);
                request.AddParamter(ParameterName(BillingTariffInformation_ID), tariffDemandEntity.BillingTariffInformation_ID, DbType.Int32);
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Demand Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tariffDemandEntity.TariffDemand_ID = long.Parse(this.GetPK());
            return tariffDemandEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            TariffDemandEntity tariffDemandEntity = entity as TariffDemandEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TariffDemand where ");
                builder.Append(string.Concat(TariffDemand_ID, "=", ParameterName(TariffDemand_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffDemand_ID), tariffDemandEntity.TariffDemand_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Demand Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TariffDemandEntity tariffDemandEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TariffDemand_ID,DemandMD1,DemandMD1TimeStamp,DemandMD2,DemandMD2TimeStamp,DemandMD3,DemandMD3TimeStamp,TariffInformation_ID,BillingTariffInformation_ID from MeterData_TariffDemand where ");
                builder.Append(string.Concat(TariffDemand_ID, "=", ParameterName(TariffDemand_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffDemand_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tariffDemandEntity = (TariffDemandEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Demand viewed."));

            }
            catch (CABException)
            {
            }
            return tariffDemandEntity;
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
                builder.Append("Select TariffDemand_ID,DemandMD1,DemandMD1TimeStamp,DemandMD2,DemandMD2TimeStamp,DemandMD3,DemandMD3TimeStamp,TariffInformation_ID,BillingTariffInformation_ID from MeterData_TariffDemand ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Demand viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TariffDemandEntity tariffDemandEntity = new TariffDemandEntity();
            if (NotNullAndNotDBNull(row, TariffDemand_ID)) tariffDemandEntity.TariffDemand_ID = Convert.ToInt32(row[TariffDemand_ID]);
            if (NotNullAndNotDBNull(row, DemandMD1)) tariffDemandEntity.DemandMD1 = Convert.ToString(row[DemandMD1]);
            if (NotNullAndNotDBNull(row, DemandMD1TimeStamp)) tariffDemandEntity.DemandMD1TimeStamp = Convert.ToString(row[DemandMD1TimeStamp]);
            if (NotNullAndNotDBNull(row, DemandMD2)) tariffDemandEntity.DemandMD2 = Convert.ToString(row[DemandMD2]);
            if (NotNullAndNotDBNull(row, DemandMD2TimeStamp)) tariffDemandEntity.DemandMD2TimeStamp = Convert.ToString(row[DemandMD2TimeStamp]);
            if (NotNullAndNotDBNull(row, DemandMD3)) tariffDemandEntity.DemandMD3 = Convert.ToString(row[DemandMD3]);
            if (NotNullAndNotDBNull(row, DemandMD3TimeStamp)) tariffDemandEntity.DemandMD3TimeStamp = Convert.ToString(row[DemandMD3TimeStamp]);
            if (NotNullAndNotDBNull(row, TariffInformation_ID)) tariffDemandEntity.TariffInformation_ID = Convert.ToInt32(row[TariffInformation_ID]);
            if (NotNullAndNotDBNull(row, BillingTariffInformation_ID)) tariffDemandEntity.BillingTariffInformation_ID = Convert.ToInt32(row[BillingTariffInformation_ID]);
            return tariffDemandEntity;
        }
    }
}
