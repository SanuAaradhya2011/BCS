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
using System.Text;
using System.Data;
using CAB.Framework;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework.Entity;
using System.Data.Common;

namespace CAB.DALC.Data
{
      public class PowerFactorDAL : DALBase
    { 
          private string PowerFactor_ID = "PowerFactor_ID";
        private string TotalPowerFactor = "TotalPowerFactor";
        private string PowerFactorRPhase = "PowerFactorRPhase";
        private string PowerFactorYPhase = "PowerFactorYPhase";
           private string PowerFactorBPhase = "PowerFactorBPhase";
        private string AveragePowerFactor = "AveragePowerFactor";
        public PowerFactorDAL()
            : base("MeterData_PowerFactor", "PowerFactor_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            PowerFactorEntity powerFactorEntity = entity as PowerFactorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_PowerFactor(TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,AveragePowerFactor) values(");
                builder.Append(string.Concat(ParameterName(TotalPowerFactor), ","));
                builder.Append(string.Concat(ParameterName(PowerFactorRPhase), ",")); 
                builder.Append(string.Concat(ParameterName(PowerFactorYPhase), ","));
                builder.Append(string.Concat(ParameterName(PowerFactorBPhase), ","));
                builder.Append(string.Concat(ParameterName(AveragePowerFactor), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TotalPowerFactor), powerFactorEntity.TotalPowerFactor, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorRPhase), powerFactorEntity.PowerFactorRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorYPhase), powerFactorEntity.PowerFactorYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(PowerFactorBPhase), powerFactorEntity.PowerFactorBPhase, DbType.String, 40);
                request.AddParamter(ParameterName(AveragePowerFactor), powerFactorEntity.AveragePowerFactor, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Power Factor Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                powerFactorEntity.PowerFactor_ID = long.Parse(this.GetPK());
            return powerFactorEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            PowerFactorEntity powerFactorEntity = entity as PowerFactorEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_PowerFactor where ");
                builder.Append(string.Concat(PowerFactor_ID, "=", ParameterName(PowerFactor_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(PowerFactor_ID), powerFactorEntity.PowerFactor_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Power Factor Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            PowerFactorEntity powerFactorEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select PowerFactor_ID,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,AveragePowerFactor from MeterData_PowerFactor where ");
                builder.Append(string.Concat(PowerFactor_ID, "=", ParameterName(PowerFactor_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(PowerFactor_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    powerFactorEntity = (PowerFactorEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Power Factor viewed."));

            }
            catch (CABException)
            { 
            }
            return powerFactorEntity;
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
                builder.Append("Select PowerFactor_ID,TotalPowerFactor,PowerFactorRPhase,PowerFactorYPhase,PowerFactorBPhase,AveragePowerFactor from MeterData_PowerFactor ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Power Factor viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            PowerFactorEntity powerFactorEntity = new PowerFactorEntity();
            if (NotNullAndNotDBNull(row, PowerFactor_ID)) powerFactorEntity.PowerFactor_ID = Convert.ToInt32(row[PowerFactor_ID]);
            if (NotNullAndNotDBNull(row, TotalPowerFactor)) powerFactorEntity.TotalPowerFactor = Convert.ToString(row[TotalPowerFactor]);
            if (NotNullAndNotDBNull(row, PowerFactorRPhase)) powerFactorEntity.PowerFactorRPhase = Convert.ToString(row[PowerFactorRPhase]);
            if (NotNullAndNotDBNull(row, PowerFactorYPhase)) powerFactorEntity.PowerFactorYPhase = Convert.ToString(row[PowerFactorYPhase]);
            if (NotNullAndNotDBNull(row, PowerFactorBPhase)) powerFactorEntity.PowerFactorBPhase = Convert.ToString(row[PowerFactorBPhase]);
            if (NotNullAndNotDBNull(row, AveragePowerFactor)) powerFactorEntity.AveragePowerFactor = Convert.ToString(row[AveragePowerFactor]);
            return powerFactorEntity;
        } 
    }
}
