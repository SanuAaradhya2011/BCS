using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.Entity;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
	public class ConsumerMasterBLL : IBLL
	{
		private ConsumerMasterDAL consumerMasterDAL; 
		public ConsumerMasterBLL()
        {
			consumerMasterDAL = new ConsumerMasterDAL();
        }
		public IEntity GetDetailData(string consumer_Number)
		{
			return consumerMasterDAL.GetDetailData(consumer_Number);
		}
        public IEntity InsertData(IEntity entity)
		{
			return consumerMasterDAL.InsertData(entity);
		}

		public bool UpdateData(IEntity entity)
		{
			return consumerMasterDAL.UpdateData(entity);
		}

		public bool DeleteData(IEntity entity)
		{
			return consumerMasterDAL.DeleteData(entity);
		}
		public bool ValidateConsumerNumber(IEntity entity)
		{
			return consumerMasterDAL.ValidateConsumerNumber(entity);
		}
        public long GetMaxConsumerNumber()
        {
            return consumerMasterDAL.GetMaxConsumerNumber();
        }
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        public void BatchInsert(List<IEntity> entities)
        {
            consumerMasterDAL.BatchInsert(entities);
        }
	}
}
