using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;

namespace CAB.BLL
{
    public class TamperParameterBLL : IBLL
    {
        TamperParameterDAL tamperParameterDAL;

        public TamperParameterBLL()
        {
            tamperParameterDAL = new TamperParameterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return tamperParameterDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return tamperParameterDAL.InsertData(entity);
        }

        public IEntity GetColumn(long meterDataId)
        {
            return tamperParameterDAL.GetDetailData(meterDataId);
        }

        /// <summary>
        /// Gets Column Names of midnight parameters For MeterID
        /// </summary>
        /// <param name="meterId">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet GetColumnNamesForMeterID(string meterId)
        {
            return tamperParameterDAL.GetColumnNamesForMeterID(meterId);
        }

        public DataSet GetColumnNames(long meterDataId)
        {
            return tamperParameterDAL.GetColumnNames(meterDataId);
        }

    }
}
