/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 												|
 * | 											Date   : 25 March 2010												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class ConsumerMasterDAL : DALBase
    {
        private string Consumer_Number = "Consumer_Number";
        private string Consumer_Name = "Consumer_Name";
        private string ConsumerType_ID = "ConsumerType_ID";
        private string Consumer_Phone = "Consumer_Phone";
        private string Consumer_HNumber = "Consumer_HNumber";
        private string Consumer_Street = "Consumer_Street";
        private string Consumer_City = "Consumer_City";
        private string Consumer_Email = "Consumer_Email";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ConsumerMasterDAL).ToString());
        public ConsumerMasterDAL()
            : base("consumer_master", "Consumer_Number")
        {
        }
        public override IEntity InsertData(IEntity entity)
        {
            ConsumerMasterEntity consumerMasterEntity = null;
            if (entity == null)
                return consumerMasterEntity;
            consumerMasterEntity = entity as ConsumerMasterEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into consumer_master(Consumer_Number,Consumer_Name,ConsumerType_ID,Consumer_Phone,Consumer_HNumber,Consumer_Street,Consumer_City,Consumer_Email) values(");
                builder.Append(string.Concat(ParameterName(Consumer_Number), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Name), ","));
                builder.Append(string.Concat(ParameterName(ConsumerType_ID), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Phone), ","));
                builder.Append(string.Concat(ParameterName(Consumer_HNumber), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Street), ","));
                builder.Append(string.Concat(ParameterName(Consumer_City), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Email), ")"));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerMasterEntity.Consumer_Number, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Name), consumerMasterEntity.Consumer_Name, DbType.String, 40);
                request.AddParamter(ParameterName(ConsumerType_ID), consumerMasterEntity.ConsumerType_ID, DbType.Int32);
                request.AddParamter(ParameterName(Consumer_Phone), consumerMasterEntity.Consumer_Phone, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_HNumber), consumerMasterEntity.Consumer_HNumber, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Street), consumerMasterEntity.Consumer_Street, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_City), consumerMasterEntity.Consumer_City, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Email), consumerMasterEntity.Consumer_Email, DbType.String, 50);

                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("New user created"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                consumerMasterEntity = null;
            }
            //if (Flag)
            //    consumerMasterEntity.Consumer_Number = Convert.ToString(this.GetPK());
            return consumerMasterEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMasterEntity consumerMasterEntity = entity as ConsumerMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update consumer_master Set ");
                //builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number) , ","));
                builder.Append(string.Concat(Consumer_Name, "=", ParameterName(Consumer_Name), ","));
                builder.Append(string.Concat(ConsumerType_ID, "=", ParameterName(ConsumerType_ID), ","));
                builder.Append(string.Concat(Consumer_Phone, "=", ParameterName(Consumer_Phone), ","));
                builder.Append(string.Concat(Consumer_HNumber, "=", ParameterName(Consumer_HNumber), ","));
                builder.Append(string.Concat(Consumer_Street, "=", ParameterName(Consumer_Street), ","));
                builder.Append(string.Concat(Consumer_City, "=", ParameterName(Consumer_City), ","));
                builder.Append(string.Concat(Consumer_Email, "=", ParameterName(Consumer_Email)));
                builder.Append(string.Concat(" Where ", Consumer_Number, "=", ParameterName(Consumer_Number)));

                DataRequest request = new DataRequest(builder.ToString());

                request.AddParamter(ParameterName(Consumer_Name), consumerMasterEntity.Consumer_Name, DbType.String, 20);
                request.AddParamter(ParameterName(ConsumerType_ID), consumerMasterEntity.ConsumerType_ID, DbType.Int32);
                request.AddParamter(ParameterName(Consumer_Phone), consumerMasterEntity.Consumer_Phone, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_HNumber), consumerMasterEntity.Consumer_HNumber, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Street), consumerMasterEntity.Consumer_Street, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_City), consumerMasterEntity.Consumer_City, DbType.String, 20);
                request.AddParamter(ParameterName(Consumer_Email), consumerMasterEntity.Consumer_Email, DbType.String, 50);
                request.AddParamter(ParameterName(Consumer_Number), consumerMasterEntity.Consumer_Number, DbType.String, 20);

                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected user updated"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;

            //throw new System.NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMasterEntity consumerMasterEntity = entity as ConsumerMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete from consumer_master ");
                builder.Append(string.Concat(" Where ", Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerMasterEntity.Consumer_Number, DbType.String, 20);
                helper.ExecuteNonQuery(request);
                Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Selected user deleted"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
            //throw new System.NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            ConsumerMasterEntity consumerMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Consumer_Number,Consumer_Name,ConsumerType_ID,Consumer_Phone,Consumer_HNumber,Consumer_Street," +
                "Consumer_City,Consumer_Email from consumer_master where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), id, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    consumerMasterEntity = (ConsumerMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified consumer viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                consumerMasterEntity = null;
            }
            return consumerMasterEntity;
        }

        public IEntity GetDetailData(string consumer_Number)
        {
            ConsumerMasterEntity consumerMasterEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Consumer_Number,Consumer_Name,ConsumerType_ID,Consumer_Phone,Consumer_HNumber,Consumer_Street," +
                "Consumer_City,Consumer_Email from consumer_master where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumer_Number, DbType.String, 20);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    consumerMasterEntity = (ConsumerMasterEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for a specified consumer viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string consumer_Number)", ex);
                consumerMasterEntity = null;
            }
            return consumerMasterEntity;
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new System.NotImplementedException();
        }
        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            ConsumerMasterEntity consumerMasterEntity = new ConsumerMasterEntity();

            if (NotNullAndNotDBNull(row, Consumer_Number)) consumerMasterEntity.Consumer_Number = Convert.ToString(row[Consumer_Number]);
            if (NotNullAndNotDBNull(row, Consumer_Name)) consumerMasterEntity.Consumer_Name = Convert.ToString(row[Consumer_Name]);
            if (NotNullAndNotDBNull(row, ConsumerType_ID)) consumerMasterEntity.ConsumerType_ID = Convert.ToInt32(row[ConsumerType_ID]);
            if (NotNullAndNotDBNull(row, Consumer_Phone)) consumerMasterEntity.Consumer_Phone = Convert.ToString(row[Consumer_Phone]);
            if (NotNullAndNotDBNull(row, Consumer_HNumber)) consumerMasterEntity.Consumer_HNumber = Convert.ToString(row[Consumer_HNumber]);
            if (NotNullAndNotDBNull(row, Consumer_Street)) consumerMasterEntity.Consumer_Street = Convert.ToString(row[Consumer_Street]);
            if (NotNullAndNotDBNull(row, Consumer_City)) consumerMasterEntity.Consumer_City = Convert.ToString(row[Consumer_City]);
            if (NotNullAndNotDBNull(row, Consumer_Email)) consumerMasterEntity.Consumer_Email = Convert.ToString(row[Consumer_Email]);

            return consumerMasterEntity;
        }


        public bool ValidateConsumerNumber(IEntity entity)
        {
            bool Flag = false;
            try
            {
                ConsumerMasterEntity consumerMasterEntity = entity as ConsumerMasterEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select count(*) from  consumer_master where ");
                builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(Consumer_Number), consumerMasterEntity.Consumer_Number, DbType.String, 20);
                object data = helper.ExecuteScalar(request);
                if ((Convert.ToInt64(data.ToString())) > 0)
                {
                    return Flag = true;
                }
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data validated"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateConsumerNumber(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }



        /// <summary>
        /// Get Max Consumer Number 
        /// </summary>
        /// <returns></returns>
        public long GetMaxConsumerNumber()
        {
            long ConsumerNumber = 0;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select IFNULL(MAX(CAST(consumer_number as UNSIGNED)),0) as consumer_number from  consumer_master ");
                DataRequest request = new DataRequest(builder.ToString());
                object data = helper.ExecuteScalar(request);
                ConsumerNumber = Convert.ToInt64(data.ToString());
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Max Consumer Number Retrived"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMaxConsumerNumber()", ex);
            }
            return ConsumerNumber;
        }
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void BatchInsert(IList<IEntity> entities)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            List<DataRequest> requests = new List<DataRequest>();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Consumer_Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Name", typeof(System.String)));
            table.Columns.Add(new DataColumn("ConsumerType_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Phone", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_HNumber", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Street", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_City", typeof(System.String)));
            table.Columns.Add(new DataColumn("Consumer_Email", typeof(System.String)));
            try
            {
                foreach (IEntity entity in entities)
                {
                    ConsumerMasterEntity consumerMasterEntity = entity as ConsumerMasterEntity;
                    DataRow dr = table.NewRow();
                    dr["Consumer_Number"] = consumerMasterEntity.Consumer_Number;
                    dr["Consumer_Name"] = consumerMasterEntity.Consumer_Name;
                    dr["ConsumerType_ID"] = consumerMasterEntity.ConsumerType_ID;
                    dr["Consumer_Phone"] = consumerMasterEntity.Consumer_Phone;
                    dr["Consumer_HNumber"] = consumerMasterEntity.Consumer_HNumber;
                    dr["Consumer_Street"] = consumerMasterEntity.Consumer_Street;
                    dr["Consumer_City"] = consumerMasterEntity.Consumer_City;
                    dr["Consumer_Email"] = consumerMasterEntity.Consumer_Email;
                    table.Rows.Add(dr);
                }

                builder.Append("Insert Into consumer_master(Consumer_Number,Consumer_Name,ConsumerType_ID,Consumer_Phone,Consumer_HNumber,Consumer_Street,Consumer_City,Consumer_Email) values(");
                builder.Append(string.Concat(ParameterName(Consumer_Number), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Name), ","));
                builder.Append(string.Concat(ParameterName(ConsumerType_ID), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Phone), ","));
                builder.Append(string.Concat(ParameterName(Consumer_HNumber), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Street), ","));
                builder.Append(string.Concat(ParameterName(Consumer_City), ","));
                builder.Append(string.Concat(ParameterName(Consumer_Email), ")"));

                MySqlCommand command = new MySqlCommand(builder.ToString());
                command.CommandType = CommandType.Text;
                command.Parameters.Add("?Consumer_Number", MySqlDbType.String).SourceColumn = "Consumer_Number";
                command.Parameters.Add("?Consumer_Name", MySqlDbType.String).SourceColumn = "Consumer_Name";
                command.Parameters.Add("?ConsumerType_ID", MySqlDbType.String).SourceColumn = "ConsumerType_ID";
                command.Parameters.Add("?Consumer_Phone", MySqlDbType.String).SourceColumn = "Consumer_Phone";
                command.Parameters.Add("?Consumer_HNumber", MySqlDbType.String).SourceColumn = "Consumer_HNumber";
                command.Parameters.Add("?Consumer_Street", MySqlDbType.String).SourceColumn = "Consumer_Street";
                command.Parameters.Add("?Consumer_City", MySqlDbType.String).SourceColumn = "Consumer_City";
                command.Parameters.Add("?Consumer_Email", MySqlDbType.String).SourceColumn = "Consumer_Email";
                command.UpdatedRowSource = UpdateRowSource.None;
                helper.BatchInsert(table, command);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BatchInsert(IList<IEntity> entities)", ex);
            }
            
        }
    }
}
