/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CAB.IECFramework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
    public class NamePlateDetailDAL : DALBase
    {
        private string NamePlateDetailID = "NamePlateDetailID";
        private string MeterID = "MeterID";
        private string MeterType = "MeterType";
        private string CurrentRating = "CurrentRating";
        private string VoltageRating = "VoltageRating";
        private string MeterConstant = "MeterConstant";
        private string ManufacturingDate = "ManufacturingDate";
        private string MeterData_ID = "MeterData_ID";

        public NamePlateDetailDAL()
            : base("meterdata_NamePlateDetail", "NamePlateDetailID")
        {
        }


        public override IEntity InsertData(IEntity entity)
        {
            NamePlateDetailEntity namePlateDetailEntity = entity as NamePlateDetailEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into meterdata_NamePlateDetail(MeterID,MeterType,CurrentRating,VoltageRating,MeterConstant,ManufacturingDate,MeterData_ID) values(");
                builder.Append(string.Concat(ParameterName(MeterID), ","));
                builder.Append(string.Concat(ParameterName(MeterType), ","));
                builder.Append(string.Concat(ParameterName(CurrentRating), ","));
                builder.Append(string.Concat(ParameterName(VoltageRating), ","));
                builder.Append(string.Concat(ParameterName(MeterConstant), ","));
                builder.Append(string.Concat(ParameterName(ManufacturingDate), ","));
                builder.Append(string.Concat(ParameterName(MeterData_ID), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterID), namePlateDetailEntity.MeterID, DbType.String, 50);
                request.AddParamter(ParameterName(MeterType), namePlateDetailEntity.MeterType, DbType.String, 50);
                request.AddParamter(ParameterName(CurrentRating), namePlateDetailEntity.CurrentRating, DbType.String, 50);
                request.AddParamter(ParameterName(VoltageRating), namePlateDetailEntity.VoltageRating, DbType.String, 50);
                request.AddParamter(ParameterName(MeterConstant), namePlateDetailEntity.MeterConstant, DbType.String, 50);
                request.AddParamter(ParameterName(ManufacturingDate), namePlateDetailEntity.ManufacturingDate, DbType.String, 50);
                request.AddParamter(ParameterName(MeterData_ID), namePlateDetailEntity.MeterData_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail added"));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                namePlateDetailEntity.NamePlateDetailID = long.Parse(this.GetPK());
            return namePlateDetailEntity;
        }
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteData(long meterDataID)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_NamePlateDetail where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            NamePlateDetailEntity namePlateDetailEntity = entity as NamePlateDetailEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from meterdata_NamePlateDetail where ");
                builder.Append(string.Concat(NamePlateDetailID, "=", ParameterName(NamePlateDetailID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(NamePlateDetailID), namePlateDetailEntity.NamePlateDetailID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail deleted"));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            NamePlateDetailEntity namePlateDetailEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select NamePlateDetailID,MeterID,MeterType,CurrentRating,VoltageRating,MeterConstant,ManufacturingDate,MeterData_ID from meterdata_NamePlateDetail where ");
                builder.Append(string.Concat(NamePlateDetailID, "=", ParameterName(NamePlateDetailID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(NamePlateDetailID), id, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    namePlateDetailEntity = (NamePlateDetailEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail viewed"));

            }
            catch (CABException)
            {
            }
            return namePlateDetailEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }
        public DataSet ListDataSet(long meterDataId)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select ");
                builder.Append("MeterID as 'Meter Id',");
                builder.Append("MeterType as 'Meter Type',");
                builder.Append("CurrentRating as 'Current Rating',");
                builder.Append("VoltageRating as 'Voltage Rating',");
                builder.Append("MeterConstant as 'Meter Constant',");
                builder.Append("ManufacturingDate as 'Manufacturing Date' from meterdata_NamePlateDetail where ");
                builder.Append(string.Concat(MeterData_ID, "=", ParameterName(MeterData_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MeterData_ID), meterDataId, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail viewed"));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            NamePlateDetailEntity namePlateDetailEntity = new NamePlateDetailEntity();
            if (NotNullAndNotDBNull(row, NamePlateDetailID)) namePlateDetailEntity.NamePlateDetailID = Convert.ToInt64(row[NamePlateDetailID]);
            if (NotNullAndNotDBNull(row, MeterID)) namePlateDetailEntity.MeterID = Convert.ToString(row[MeterID]);
            if (NotNullAndNotDBNull(row, MeterType)) namePlateDetailEntity.MeterType = Convert.ToString(row[MeterType]);
            if (NotNullAndNotDBNull(row, CurrentRating)) namePlateDetailEntity.CurrentRating = Convert.ToString(row[CurrentRating]);
            if (NotNullAndNotDBNull(row, VoltageRating)) namePlateDetailEntity.VoltageRating = Convert.ToString(row[VoltageRating]);
            if (NotNullAndNotDBNull(row, MeterConstant)) namePlateDetailEntity.MeterConstant = Convert.ToString(row[MeterConstant]);
            if (NotNullAndNotDBNull(row, ManufacturingDate)) namePlateDetailEntity.ManufacturingDate = Convert.ToString(row[ManufacturingDate]);
            if (NotNullAndNotDBNull(row, MeterData_ID)) namePlateDetailEntity.MeterData_ID = Convert.ToInt64(row[MeterData_ID]);
            UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter data Name Plate Detail viewed"));
            return namePlateDetailEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
