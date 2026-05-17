using System;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity.Base;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.Entity;
using System.Data;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
	public class SuspectedConsumerDAL : DALBase
	{
		private string Consumer_Number = "Consumer_Number";
		private string SuspectionStartDate = "SuspectionStartDate";
		private string SuspectionEndDate = "SuspectionEndDate";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SuspectedConsumerDAL).ToString());

        public override IEntity InsertData(IEntity entity)
		{
            SuspectedConsumerEntity suspectedConsumerEntity = null;
            if (entity == null)
                return suspectedConsumerEntity;
			  suspectedConsumerEntity = entity as SuspectedConsumerEntity;
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Insert Into suspectedconsumer(Consumer_Number,SuspectionStartDate,SuspectionEndDate) values(");
				builder.Append(string.Concat(ParameterName(Consumer_Number), ","));
				builder.Append(string.Concat(ParameterName(SuspectionStartDate), ","));
				builder.Append(string.Concat(ParameterName(SuspectionEndDate), ")"));
				
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Consumer_Number), suspectedConsumerEntity.Consumer_Number, DbType.String, 20);
				request.AddParamter(ParameterName(SuspectionStartDate), suspectedConsumerEntity.SuspectionStartDate, DbType.Int64);
				request.AddParamter(ParameterName(SuspectionEndDate), suspectedConsumerEntity.SuspectionEndDate, DbType.Int64);
				helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data inserted"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                suspectedConsumerEntity = null;
			}
            return suspectedConsumerEntity;
		}

		public override bool UpdateData(IEntity entity)
		{
			throw new System.NotImplementedException();
		}

		public override bool DeleteData(IEntity entity)
		{
			bool Flag = false;
			try
			{
				SuspectedConsumerEntity suspectedConsumerEntity = entity as SuspectedConsumerEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Delete from suspectedconsumer where ");
				builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Consumer_Number), suspectedConsumerEntity.Consumer_Number, DbType.String, 20);
				helper.ExecuteNonQuery(request);
				Flag = true;
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data deleted"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

		public override IEntity GetDetailData(int id)
		{
			throw new System.NotImplementedException();
		}

		public IEntity GetDetailData(string consumer_Number)
		{
			SuspectedConsumerEntity suspectedConsumerEntity = null;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select SuspectedConsumer_ID,Consumer_Number,SuspectionStartDate,SuspectionEndDate from suspectedconsumer where ");
				builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Consumer_Number), consumer_Number, DbType.String, 20);
				DataSet ds = new DataSet();
				ds = helper.FillDataSet(request, ds);
				if (ds.Tables[0].Rows.Count > 0)
					suspectedConsumerEntity = (SuspectedConsumerEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Consumer data retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetDetailData(string consumer_Number)", ex);
				suspectedConsumerEntity = null;
			}
			return suspectedConsumerEntity;
		}

		public bool GetSuspectedConsumerData(string consumer_Number)
		{
			bool Flag = false;
			try
			{
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select count(*) from suspectedconsumer where ");
				builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Consumer_Number), consumer_Number, DbType.String, 20);
				object data = helper.ExecuteScalar(request);
				if (Convert.ToInt32(data) > 0)
				{
					Flag = true;
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for suspected consumers retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetSuspectedConsumerData(string consumer_Number)", ex);
				Flag = false;
			}
			return Flag;
		}


		public bool GetConsumerAvailability(IEntity entity)
		{
			bool Flag = false;
			try
			{
				SuspectedConsumerEntity suspectedConsumerEntity = entity as SuspectedConsumerEntity;
				IDataHelper helper = DatabaseFactory.GetHelper();
				StringBuilder builder = new StringBuilder();
				builder.Append("Select count(*) from suspectedconsumer where SuspectionStartDate is Not Null and SuspectionEndDate is Null and ");
				builder.Append(string.Concat(Consumer_Number, "=", ParameterName(Consumer_Number)));
				DataRequest request = new DataRequest(builder.ToString());
				request.AddParamter(ParameterName(Consumer_Number), suspectedConsumerEntity.Consumer_Number, DbType.String, 20);
				object data = helper.ExecuteScalar(request);
				if (Convert.ToInt32(data.ToString()) > 0)
				{
					Flag = true;
				}
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data for specified consumers retrieved"));
			}
			catch (CABException ex)    //Exception log for catch block
			{
                logger.Log(LOGLEVELS.Error, "GetConsumerAvailability(IEntity entity)", ex);
				Flag = false;
			}
			return Flag;
		}

		public override DataSet ListDataSet()
		{
			throw new System.NotImplementedException();
		}

		public override IEntity RowToEntity(DataRow row)
		{
			if (row == null) return null;
			SuspectedConsumerEntity suspectedConsumerEntity = new SuspectedConsumerEntity();
			if (NotNullAndNotDBNull(row, Consumer_Number)) suspectedConsumerEntity.Consumer_Number = Convert.ToString(row[Consumer_Number]);
			return suspectedConsumerEntity;
		}

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
    }
}
