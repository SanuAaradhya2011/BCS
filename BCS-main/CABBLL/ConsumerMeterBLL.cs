using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
	public class ConsumerMeterBLL : IBLL
	{
		private ConsumerMeterDAL consumerMeterDAL; 
		public ConsumerMeterBLL()
        {
			consumerMeterDAL = new ConsumerMeterDAL();
        }
		public IEntity GetDetailData(string consumer_Number,string meter_ID)
		{
			return consumerMeterDAL.GetDetailData(consumer_Number,meter_ID);
		}
        public IEntity InsertData(IEntity entity)
		{
			return consumerMeterDAL.InsertData(entity);

		}

       /// <summary>
        /// GPRS : Overload of InsertData(IEntity entity) added to set the syncing status of endpoint 
       /// </summary>
       /// <param name="entity"></param>
       /// <param name="isBulkUpload"></param>
       /// <returns></returns>
        public IEntity InsertData(IEntity entity,bool isBulkUpload)
        {
            return consumerMeterDAL.InsertData(entity,isBulkUpload);
        }

		public bool UpdateData(IEntity entity)
		{
			return consumerMeterDAL.UpdateData(entity);
		}
		public DataSet GetActiveMeterID(string consumer_Number)
		{
			return consumerMeterDAL.ListActiveMeterID(consumer_Number);
		}
		public DataSet GetInactiveMeterID()
		{
			return consumerMeterDAL.ListInactiveMeterID();
		}
        public IEntity InsertUpdateData(IEntity entity)
		{
            if (consumerMeterDAL.GetDataAvailability(entity))
            {
                consumerMeterDAL.UpdateData(entity);
                return entity;
            }
            else
                return consumerMeterDAL.InsertData(entity); 
		}
		public bool DeleteData(IEntity entity)
		{
			return consumerMeterDAL.DeleteData(entity);
		}

		public bool DeleteData(string consumerID)
		{
			return consumerMeterDAL.DeleteData(consumerID);
		}

		public bool GetConsumerAvailable(string consumerID)
		{
			return consumerMeterDAL.GetConsumerCount(consumerID);
		}
		
		public bool GetConsumerMeterAvailable(IEntity entity)
		{
			return consumerMeterDAL.GetConsumerMeterAvailability(entity);
		}
        
        public DataSet ComboList(bool isConsumer)
        {
            return consumerMeterDAL.ComboList(isConsumer);
        }
        
        public DataSet ListDataSet(string types, string value)
        {
            return consumerMeterDAL.ListDataSet(types, value);
        }

        /// <summary>
        /// Method to search meters for the division which are not present in the group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="divisionID"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public DataSet GetMetersbyDivisionID(long groupId, int divisionID, string commType)
       {
           return consumerMeterDAL.GetMetersbyDivisionID(groupId,divisionID, commType);
       }

        /// <summary>
        /// Method to gets meters with particular communication type
        /// </summary>
        /// <param name="divisionID"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public DataSet GetMetersbyDivisionID(int divisionID, string commType)
        {
            return consumerMeterDAL.GetMetersbyDivisionID(divisionID, commType);
        }

       
        public DataSet GetActiveMeterList()
        {
            return consumerMeterDAL.GetActiveMeterList();
        }
        public string GetConsumerNumber(long meterNumber)
        {
            return consumerMeterDAL.GetConsumerNumber(meterNumber);
        }

        /// <summary>
        /// GPRS: Method to check whether GPRS meter exists
        /// </summary>
        /// <param name="meterNumber"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        public bool IsMeterExists(string meterNumber,string commType)
        {
            DataSet ds = consumerMeterDAL.IsMeterExists(meterNumber, commType);

            return ds == null ? false : ds.Tables[0].Rows.Count >= 1 ? true : false;
        }
        /// <summary>
        /// bulk insert with origional => GPRS: Overload the method to support GPRS bulk upload specific changes
        /// </summary>
        /// <param name="entities"></param>
        public void BatchInsert(List<IEntity> entities , bool isBulkUpload)
        {
            consumerMeterDAL.BatchData(entities , isBulkUpload);
        }
	}
}
