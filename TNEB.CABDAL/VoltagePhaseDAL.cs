/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Dhananjay Prasad Verma. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
namespace CAB.DALC.Data
{
    public class VoltagePhaseDAL : DALBase
    {   
       private string VoltagePhase_ID = "VoltagePhase_ID";
        private string VoltagePhaseSequence = "VoltagePhaseSequence";
        private string VoltageRPhase = "VoltageRPhase";
        private string VoltageYPhase = "VoltageYPhase"; 
        private string VoltageBPhase = "VoltageBPhase"; 
        public VoltagePhaseDAL()
            : base("MeterData_VoltagePhase", "VoltagePhase_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            VoltagePhaseEntity voltagePhaseEntity = entity as VoltagePhaseEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_VoltagePhase(VoltagePhaseSequence,VoltageRPhase,VoltageYPhase,VoltageBPhase) values(");
                builder.Append(string.Concat(ParameterName(VoltagePhaseSequence), ","));
                builder.Append(string.Concat(ParameterName(VoltageRPhase), ","));
                builder.Append(string.Concat(ParameterName(VoltageYPhase), ","));
                builder.Append(string.Concat(ParameterName(VoltageBPhase), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(VoltagePhaseSequence), voltagePhaseEntity.VoltagePhaseSequence, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageRPhase), voltagePhaseEntity.VoltageRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageYPhase), voltagePhaseEntity.VoltageYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(VoltageYPhase), voltagePhaseEntity.VoltageYPhase, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Voltage Phase Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                voltagePhaseEntity.VoltagePhase_ID = long.Parse(this.GetPK());
            return voltagePhaseEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            VoltagePhaseEntity voltagePhaseEntity = entity as VoltagePhaseEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_VoltagePhase where ");
                builder.Append(string.Concat(VoltagePhase_ID, "=", ParameterName(VoltagePhase_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(VoltagePhase_ID), voltagePhaseEntity.VoltagePhase_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Voltage Phase Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            VoltagePhaseEntity voltagePhaseEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select VoltagePhase_ID,VoltagePhaseSequence,VoltageRPhase,VoltageYPhase,VoltageBPhase from MeterData_VoltagePhase where ");
                builder.Append(string.Concat(VoltagePhase_ID, "=", ParameterName(VoltagePhase_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(VoltagePhase_ID), id, DbType.UInt64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    voltagePhaseEntity = (VoltagePhaseEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Voltage Phase viewed."));

            }
            catch (CABException)
            { 
            }
            return voltagePhaseEntity;
        }

        

        public override DataSet ListDataSet()
        {
            DataSet dataSet = null;
            try  
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select VoltagePhase_ID,VoltagePhaseSequence,VoltageRPhase,VoltageYPhase,VoltageBPhase from MeterData_VoltagePhase");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Voltage Phase viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            VoltagePhaseEntity voltagePhaseEntity = new VoltagePhaseEntity();
            if (NotNullAndNotDBNull(row, VoltagePhase_ID)) voltagePhaseEntity.VoltagePhase_ID = Convert.ToInt32(row[VoltagePhase_ID]);
            if (NotNullAndNotDBNull(row, VoltagePhaseSequence)) voltagePhaseEntity.VoltagePhaseSequence = Convert.ToString(row[VoltagePhaseSequence]);
            if (NotNullAndNotDBNull(row, VoltageRPhase)) voltagePhaseEntity.VoltageRPhase = Convert.ToString(row[VoltageRPhase]);
            if (NotNullAndNotDBNull(row, VoltageYPhase)) voltagePhaseEntity.VoltageYPhase = Convert.ToString(row[VoltageYPhase]);
            if (NotNullAndNotDBNull(row, VoltageBPhase)) voltagePhaseEntity.VoltageBPhase = Convert.ToString(row[VoltageBPhase]);
            return voltagePhaseEntity;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
    }
}
