using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
	public class MeterMasterBLL : IBLL
	{
		private MeterMasterDAL meterMasterDAL;
 
		public MeterMasterBLL()
        {
			meterMasterDAL = new MeterMasterDAL();
        }

		public IEntity GetDetailData(string meter_ID,int meter_Status)
		{
			return meterMasterDAL.GetDetailData(meter_ID,meter_Status);
		}

        public IEntity InsertData(IEntity entity)
		{
			return meterMasterDAL.InsertData(entity);
		}

		public bool UpdateData(IEntity entity)
		{
			return meterMasterDAL.UpdateData(entity);
		}

		public bool DeleteData(IEntity entity)
		{
			return meterMasterDAL.DeleteData(entity);
		}

		public bool DeleteMeterData(IEntity entity)
		{
			return meterMasterDAL.DeleteMeterData(entity);
		}

		public bool ValidateMeterNumber(IEntity entity)
		{
			return meterMasterDAL.ValidateMeterNumber(entity);
		}

        public DataSet ListInactiveMeterID()
        {
            return meterMasterDAL.ListInactiveMeterID();
        }

        public DataSet ListMeterID()
        {
            return meterMasterDAL.ListMeterID();
        }

		public DataSet ListUnAssignedAreaMeterID()
		{
			return meterMasterDAL.ListUnAssignedAreaMeterID();
		}
        public int GetEMF(long meterDataID)
        {
            return meterMasterDAL.GetEMF(meterDataID);
        }
	}
}
