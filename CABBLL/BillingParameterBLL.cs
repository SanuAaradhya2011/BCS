using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;

namespace CAB.BLL
{
    public class BillingParameterBLL : IBLL
    {
         BillingParameterDAL billingParameterDAL;

         public BillingParameterBLL()
        {
            billingParameterDAL = new BillingParameterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return billingParameterDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return billingParameterDAL.InsertData(entity);
        }

        public IEntity GetColumn(long meterDataId)
        {
            return billingParameterDAL.GetDetailData(meterDataId);
        }

        /// <summary>
        /// Gets Column Names of midnight parameters For MeterID
        /// </summary>
        /// <param name="meterId">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet GetColumnNamesForMeterID(string meterId)
        {
            return billingParameterDAL.GetColumnNamesForMeterID(meterId);
        }

        public DataSet GetColumnNames(long meterDataId)
        {
            return billingParameterDAL.GetColumnNames(meterDataId);
        }
    }
}
