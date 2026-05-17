using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.DALC.Data;
using System.Collections.Generic;

namespace CAB.BLL
{
	public class MeterMasterBLL : IBLL
	{
		private MeterMasterDAL meterMasterDAL;
        private bool isPUMA = false;
		public MeterMasterBLL()
        {

            meterMasterDAL = new MeterMasterDAL(UtilityDetails.GetUtilityDetails());
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

        public IEntity InsertDataIntoLog(IEntity entity)
        {
            return meterMasterDAL.InsertDataIntoLog(entity);
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

        /// <summary>
        /// Mehtod to validate meter number passed
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public bool ValidateMeterNumber(string meterId)
        {
            return meterMasterDAL.ValidateMeterNumber(meterId);
        }

        /// <summary>
        /// GPRS: Method to check whether IMEI exists witg some meter into database
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public bool IsIMEIAlreadyExists(string IMEI)
        {
            return meterMasterDAL.IsIMEIAlreadyExists(IMEI);
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
        public IEntity GetMultiplyingFactors(long meterDataID)
        {
            return meterMasterDAL.GetMultiplyingFactors(meterDataID);
        }
        public void GetLatestInternalCTPTRatio(string meter_ID,out int internalCTratio,out int internalPTratio,out string meterType)
        {
            meterMasterDAL.GetLatestInternalCTPTRatio(meter_ID,out internalCTratio,out internalPTratio,out meterType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetMeterNumber(long phoneNumber)
        {
            return meterMasterDAL.GetMeterNumber(phoneNumber);
        }

        /// <summary>
        /// GPRS: Method to find the meter mapped with IMEI
        /// </summary>
        /// <param name="IMEI"></param>
        /// <returns></returns>
        public string GetMeterNumber(string IMEI)
        {
            return meterMasterDAL.GetMeterNumber(IMEI);
        }
        /// <summary>
        /// Get Meter Serial Number and Sim number.
        /// </summary>
        /// <returns></returns>
        public DataSet GetMeterIdAndSimNumber(string communicationType)
        {
            return meterMasterDAL.GetMeterIdAndSimNumber(communicationType);
        }


        public bool UpdateMeterModelAndType(IEntity entity)
        {
            return meterMasterDAL.UpdateMeterModelAndType(entity);
        }

        public IEntity GetMultiplyingFactorsForMeterID(string meterID)
        {
            return meterMasterDAL.GetMultiplyingFactorsForMeterID(meterID);
        }
        /// <summary>
        /// get meter details for  customer import validation and insertion
        /// </summary>
        /// <returns></returns>
        public DataSet GetMeterDetails()
        {
            return meterMasterDAL.GetMeterDetails();
        }
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities"></param>
        public void BatchInsert(List<IEntity> entities)
        {
            meterMasterDAL.BatchInsert(entities);
        }

	}
}
