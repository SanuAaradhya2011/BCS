using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class AreaMeterDAL : DALBase
    {
        private string Area_ID = "Area_ID";
        private string Meter_ID = "Meter_ID";
        private string AreaMeter_ID = "AreaMeter_ID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AreaMeterDAL).ToString());
        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                AreaMeterEntity areameterEntity = entity as AreaMeterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update areameter_master Set ");
                builder.Append(string.Concat(Area_ID, "= ", ParameterName(Area_ID), ","));
                builder.Append(string.Concat(Meter_ID, "= ", ParameterName(Meter_ID), ""));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), areameterEntity.Area_ID, DbType.Int32);
                request.AddParamter(ParameterName(Meter_ID), areameterEntity.Meter_ID, DbType.Int32);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meters for specific area modified"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                AreaMeterEntity areameterEntity = entity as AreaMeterEntity;
                if (areameterEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From AreaMaster ");
                builder.Append(string.Concat(" Where ", AreaMeter_ID, "=", ParameterName(AreaMeter_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(AreaMeter_ID), areameterEntity.AreaMeter_ID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected area deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public bool DeleteMeters(long id)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from areameter_master ");
                builder.Append(string.Concat("Where ", Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), id, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meters assigned to specified area disallocated"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteMeters(long id)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            AreaMeterEntity areameterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select AreaMeter_ID, Area_ID, Meter_ID from  areameter_master where ");
                builder.Append(string.Concat(Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    areameterEntity = (AreaMeterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meters for the specified area selected"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                areameterEntity = null;
            }
            return areameterEntity;
        }



        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public DataSet ListUnassignedMeters()
        {
            DataSet dataSet = new DataSet();
            try
            {
                AreaMeterEntity areameterEntity = new AreaMeterEntity();
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select Area_ID, Meter_ID from  areameter_master");//where Meter_ID not in (Select Meter_ID from areamaster)");
                builder.Append(string.Concat(" Where ", Area_ID, " not in ("));
                builder.Append(string.Concat("Select Area_ID from areamaster)"));
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meters of a specified area selected"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListUnassignedMeters()", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public DataSet ListDataSet(long id)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select AreaMeter_ID, Area_ID, Meter_ID from  areameter_master where ");
                builder.Append(string.Concat(Area_ID, "=", ParameterName(Area_ID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Area_ID), id, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Meters of a specified area selected"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(long id)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            AreaMeterEntity areameterEntity = new AreaMeterEntity();
            if (NotNullAndNotDBNull(row, AreaMeter_ID)) areameterEntity.AreaMeter_ID = Convert.ToInt32(row[AreaMeter_ID]);
            if (NotNullAndNotDBNull(row, Area_ID)) areameterEntity.Area_ID = Convert.ToInt32(row[Area_ID]);
            if (NotNullAndNotDBNull(row, Meter_ID)) areameterEntity.Meter_ID = row[Meter_ID].ToString();

            return areameterEntity;
        }

        private DataRequest GetRequest(IEntity entity)
        {
            AreaMeterEntity areameterEntity = entity as AreaMeterEntity;
            StringBuilder builder = new StringBuilder();
            builder.Append("Insert Into areameter_master(Area_ID,Meter_ID) values(");
            builder.Append(string.Concat(ParameterName(Area_ID), ","));
            builder.Append(string.Concat(ParameterName(Meter_ID), ")"));
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName(Area_ID), areameterEntity.Area_ID, DbType.Int64);
            request.AddParamter(ParameterName(Meter_ID), areameterEntity.Meter_ID, DbType.String, 20);
            return request;
        }
        public override IEntity InsertData(IList<IEntity> entities)
        {
            List<DataRequest> requests = new List<DataRequest>();

            foreach (IEntity entity in entities)
                requests.Add(this.GetRequest(entity));
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(requests);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New area added"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IList<IEntity> entities)", ex);
            }
            return null;
        }
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BatchInsert(IList<IEntity> entities)
        {
            //List<DataRequest> requests = new List<DataRequest>();
            IDataHelper helper = DatabaseFactory.GetHelper();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Area_ID", typeof(System.Int64)));
            table.Columns.Add(new DataColumn("Meter_ID", typeof(System.String)));
            try
            {
                foreach (IEntity entity in entities)
                {
                    AreaMeterEntity areameterEntity = entity as AreaMeterEntity;
                    DataRow dr = table.NewRow();
                    dr["Area_ID"] = areameterEntity.Area_ID;
                    dr["Meter_ID"] = areameterEntity.Meter_ID;
                    table.Rows.Add(dr);
                }
                //requests.Add(this.GetRequest(entity));

                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into areameter_master(Area_ID,Meter_ID) values(");
                builder.Append(string.Concat(ParameterName(Area_ID), ","));
                builder.Append(string.Concat(ParameterName(Meter_ID), ")"));

                MySqlCommand command = new MySqlCommand(builder.ToString());
                command.CommandType = CommandType.Text;
                command.Parameters.Add("?Area_ID", MySqlDbType.Int64).SourceColumn = "Area_ID";
                command.Parameters.Add("?Meter_ID", MySqlDbType.String).SourceColumn = "Meter_ID";
                command.UpdatedRowSource = UpdateRowSource.None;

                helper.BatchInsert(table, command);
                //IDataHelper helper = DatabaseFactory.GetHelper();
                //helper.ExecuteNonQuery(requests);
                //UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New area added"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchInsert(IList<IEntity> entities)", ex);
            }
            
        }
        public override IEntity InsertData(IEntity entity)
        {
            AreaMeterEntity areameterEntity = null;
            if (entity == null)
                return areameterEntity;
            areameterEntity = entity as AreaMeterEntity;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                helper.ExecuteNonQuery(this.GetRequest(entity));
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New area added"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                areameterEntity = null;
            }
            return areameterEntity;
        }

        public bool ValidateMeter(IList<IEntity> entities)
        {
            if (entities.Count == 0)
                return false;
            bool Flag = true;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                foreach (IEntity entity in entities)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("select Count(*) from  areameter_master");
                    builder.Append(string.Concat(" Where ", Meter_ID, " = ("));
                    AreaMeterEntity areameterEntity = entity as AreaMeterEntity;
                    builder.Append(string.Concat("'", areameterEntity.Meter_ID, "',"));
                    string qry = builder.ToString();
                    qry = qry.Substring(0, qry.Length - 1) + ")";
                    DataRequest request = new DataRequest(qry);
                    object obj = helper.ExecuteScalar(request);
                    if (Convert.ToInt32(obj) > 0)
                        Flag = false;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified meter viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateMeter(IList<IEntity> entities)", ex);
                Flag = false;
            }
            return Flag;
        }


    }
}
