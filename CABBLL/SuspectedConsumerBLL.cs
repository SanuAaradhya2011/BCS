using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
	public class SuspectedConsumerBLL : IBLL
	{
		private SuspectedConsumerDAL suspectedConsumerDAL;
		public SuspectedConsumerBLL()
        {
			suspectedConsumerDAL = new SuspectedConsumerDAL();
        }
		public bool GetDetailData(string consumer_Number)
		{
			return suspectedConsumerDAL.GetSuspectedConsumerData(consumer_Number);//GetDetailData(consumer_Number);
		}
		public bool IsConsumerSuspected(string consumer_Number)
		{
			return suspectedConsumerDAL.GetSuspectedConsumerData(consumer_Number);
		}
        public IEntity InsertData(IEntity entity)
		{
			return suspectedConsumerDAL.InsertData(entity);
		}
		public bool UpdateData(IEntity entity)
		{
			return suspectedConsumerDAL.UpdateData(entity);
		}
		public bool DeleteData(IEntity entity)
		{
			return suspectedConsumerDAL.DeleteData(entity);
		}

		public bool GetConsumerAvailability(IEntity entity)
		{
			return suspectedConsumerDAL.GetConsumerAvailability(entity);
		}

	}
}
