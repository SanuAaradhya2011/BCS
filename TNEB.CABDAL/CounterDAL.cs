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
     public class CounterDAL : DALBase
    {  
         private string Counter_ID = "Counter_ID";
        private string MDResetCounter = "MDResetCounter";
        private string ReadoutCounter = "ReadoutCounter";
        private string ProgrammingCounter = "ProgrammingCounter";
           private string CTRatioProgrammingCounter = "CTRatioProgrammingCounter";
        public CounterDAL()
            : base("MeterData_Counter", "Counter_ID")
        {
        }

        public override bool InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public override IEntity InsertData(IEntity entity, DbTransaction transaction, DbConnection connection)
        {
            CounterEntity counterEntity = entity as CounterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into MeterData_Counter(MDResetCounter,ReadoutCounter,ProgrammingCounter,CTRatioProgrammingCounter) values(");
                builder.Append(string.Concat(ParameterName(MDResetCounter), ","));
                builder.Append(string.Concat(ParameterName(ReadoutCounter), ",")); 
                builder.Append(string.Concat(ParameterName(ProgrammingCounter), ","));
                builder.Append(string.Concat(ParameterName(CTRatioProgrammingCounter), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(MDResetCounter), counterEntity.MDResetCounter, DbType.String, 40);
                request.AddParamter(ParameterName(ReadoutCounter), counterEntity.ReadoutCounter, DbType.String, 40);
                request.AddParamter(ParameterName(ProgrammingCounter), counterEntity.ProgrammingCounter, DbType.String, 40);
                request.AddParamter(ParameterName(CTRatioProgrammingCounter), counterEntity.CTRatioProgrammingCounter, DbType.String, 40); 
                helper.ExecuteNonQuery(request, transaction, connection);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Added."));
                Flag = true;
            }
            catch (Exception) { }
            if (Flag)
                counterEntity.Counter_ID = long.Parse(this.GetPK());
            return counterEntity;
        } 
        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            CounterEntity counterEntity = entity as CounterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from MeterData_Counter where ");
                builder.Append(string.Concat(Counter_ID, "=", ParameterName(Counter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Counter_ID), counterEntity.Counter_ID, DbType.UInt32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter Deleted."));
                Flag = true;
            }
            catch (Exception) { }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            CounterEntity counterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Counter_ID,MDResetCounter,ReadoutCounter,ProgrammingCounter,CTRatioProgrammingCounter from MeterData_Counter where ");
                builder.Append(string.Concat(Counter_ID, "=", ParameterName(Counter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Counter_ID), id, DbType.UInt32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    counterEntity = (CounterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter viewed."));

            }
            catch (CABException)
            { 
            }
            return counterEntity;
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
                builder.Append("Select Counter_ID,MDResetCounter,ReadoutCounter,ProgrammingCounter,CTRatioProgrammingCounter from MeterData_Counter ");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meter Data Counter viewed."));
            }
            catch (CABException)
            {
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        { 
            if (row == null) return null;
            CounterEntity counterEntity = new CounterEntity();
            if (NotNullAndNotDBNull(row, Counter_ID)) counterEntity.Counter_ID = Convert.ToInt32(row[Counter_ID]);
            if (NotNullAndNotDBNull(row, MDResetCounter)) counterEntity.MDResetCounter = Convert.ToString(row[MDResetCounter]);
            if (NotNullAndNotDBNull(row, ReadoutCounter)) counterEntity.ReadoutCounter = Convert.ToString(row[ReadoutCounter]);
            if (NotNullAndNotDBNull(row, ProgrammingCounter)) counterEntity.ProgrammingCounter = Convert.ToString(row[ProgrammingCounter]);
            if (NotNullAndNotDBNull(row, CTRatioProgrammingCounter)) counterEntity.CTRatioProgrammingCounter = Convert.ToString(row[CTRatioProgrammingCounter]);
            return counterEntity;
        } 
    }
}
