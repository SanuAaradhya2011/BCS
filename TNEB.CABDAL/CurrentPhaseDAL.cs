/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to  Cabcon							|
 * | 																												|
 * |											Author :Piyush Singh. 	 												|
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
   public class CurrentPhaseDAL : DALBase
    {   
       private string CurrentPhase_ID = "CurrentPhase_ID";
        private string CurrentPhaseSequence = "CurrentPhaseSequence";
        private string CurrentRPhase = "CurrentRPhase";
        private string CurrentYPhase = "CurrentYPhase";
         private string CurrentBPhase = "CurrentBPhase";
         public CurrentPhaseDAL()
            : base("MeterData_CurrentPhase", "CurrentPhase_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CurrentPhaseEntity currentPhaseEntity = entity as CurrentPhaseEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_CurrentPhase(CurrentPhaseSequence,CurrentRPhase,CurrentYPhase,CurrentBPhase) values(");
                builder.Append(string.Concat(ParameterName(CurrentPhaseSequence), ","));
                builder.Append(string.Concat(ParameterName(CurrentRPhase), ",")); 
                builder.Append(string.Concat(ParameterName(CurrentYPhase), ","));
                builder.Append(string.Concat(ParameterName(CurrentBPhase), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentPhaseSequence), currentPhaseEntity.CurrentPhaseSequence, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentRPhase), currentPhaseEntity.CurrentRPhase, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentYPhase), currentPhaseEntity.CurrentYPhase, DbType.String, 40);
                request.AddParamter(ParameterName(CurrentBPhase), currentPhaseEntity.CurrentBPhase, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Phase Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                currentPhaseEntity.CurrentPhase_ID = long.Parse(this.GetPK());
            return currentPhaseEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CurrentPhaseEntity currentPhaseEntity = entity as CurrentPhaseEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_CurrentPhase where ");
                builder.Append(string.Concat(CurrentPhase_ID, "=", ParameterName(CurrentPhase_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentPhase_ID), currentPhaseEntity.CurrentPhase_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Phase Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CurrentPhaseEntity currentPhaseEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select CurrentPhase_ID,CurrentPhaseSequence,CurrentRPhase,CurrentYPhase,CurrentBPhase from MeterData_CurrentPhase where ");
                builder.Append(string.Concat(CurrentPhase_ID, "=", ParameterName(CurrentPhase_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(CurrentPhase_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    currentPhaseEntity = (CurrentPhaseEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Phase viewed."));

            }
            catch (CABException)
            { 
            }
            return currentPhaseEntity;
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
                builder.Append("Select CurrentPhase_ID,CurrentPhaseSequence,CurrentRPhase,CurrentYPhase,CurrentBPhase from MeterData_CurrentPhase ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Current Phase viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            CurrentPhaseEntity currentPhaseEntity = new CurrentPhaseEntity();
            if (NotNullAndNotDBNull(row, CurrentPhase_ID)) currentPhaseEntity.CurrentPhase_ID = Convert.ToInt32(row[CurrentPhase_ID]);
            if (NotNullAndNotDBNull(row, CurrentPhaseSequence)) currentPhaseEntity.CurrentPhaseSequence = Convert.ToString(row[CurrentPhaseSequence]);
            if (NotNullAndNotDBNull(row, CurrentRPhase)) currentPhaseEntity.CurrentRPhase = Convert.ToString(row[CurrentRPhase]);
            if (NotNullAndNotDBNull(row, CurrentYPhase)) currentPhaseEntity.CurrentYPhase = Convert.ToString(row[CurrentYPhase]);
            if (NotNullAndNotDBNull(row, CurrentBPhase)) currentPhaseEntity.CurrentBPhase = Convert.ToString(row[CurrentBPhase]);
            
            return currentPhaseEntity;
        } 
    }
}
