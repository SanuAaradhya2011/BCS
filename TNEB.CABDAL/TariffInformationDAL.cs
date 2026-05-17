using System;
using System.Collections.Generic;
using System.Text;
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon							|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class TariffInformationDAL : DALBase
    { 
        private string TariffInformation_ID = "TariffInformation_ID";
        private string AveragePowerFactor = "AveragePowerFactor";
        private string Tariff_Number = "Tariff_Number";
        private string History_ID = "History_ID"; 
        public TariffInformationDAL()
            : base("MeterData_TariffInformation", "TariffInformation_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            TariffInformationEntity tariffInformationEntity = entity as TariffInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_TariffInformation(AveragePowerFactor,Tariff_Number,History_ID) values(");
                builder.Append(string.Concat(ParameterName(AveragePowerFactor), ","));
                builder.Append(string.Concat(ParameterName(Tariff_Number), ",")); 
                builder.Append(string.Concat(ParameterName(History_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(AveragePowerFactor), tariffInformationEntity.AveragePowerFactor, DbType.String, 40);
                request.AddParamter(ParameterName(Tariff_Number), tariffInformationEntity.Tariff_Number, DbType.String, 40);
                request.AddParamter(ParameterName(History_ID), tariffInformationEntity.History_ID, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Information Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                tariffInformationEntity.TariffInformation_ID = long.Parse(this.GetPK());
            return tariffInformationEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            TariffInformationEntity tariffInformationEntity = entity as TariffInformationEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_TariffInformation where ");
                builder.Append(string.Concat(TariffInformation_ID, "=", ParameterName(TariffInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffInformation_ID), tariffInformationEntity.TariffInformation_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Information Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TariffInformationEntity tariffInformationEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TariffInformation_ID,AveragePowerFactor,Tariff_Number,History_ID from MeterData_TariffInformation where ");
                builder.Append(string.Concat(TariffInformation_ID, "=", ParameterName(TariffInformation_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TariffInformation_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    tariffInformationEntity = (TariffInformationEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Information viewed."));

            }
            catch (CABException)
            { 
            }
            return tariffInformationEntity;
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
                builder.Append("Select TariffInformation_ID,AveragePowerFactor,Tariff_Number,History_ID from MeterData_TariffInformation ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Tariff Information viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            TariffInformationEntity tariffInformationEntity = new TariffInformationEntity();
            if (NotNullAndNotDBNull(row, TariffInformation_ID)) tariffInformationEntity.TariffInformation_ID = Convert.ToInt32(row[TariffInformation_ID]);
            if (NotNullAndNotDBNull(row, AveragePowerFactor)) tariffInformationEntity.AveragePowerFactor = Convert.ToString(row[AveragePowerFactor]);
            if (NotNullAndNotDBNull(row, Tariff_Number)) tariffInformationEntity.Tariff_Number = Convert.ToInt32(row[Tariff_Number]);
            if (NotNullAndNotDBNull(row, History_ID)) tariffInformationEntity.History_ID = Convert.ToInt32(row[History_ID]);
            return tariffInformationEntity;
        } 
    }
}
