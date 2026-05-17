/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 							        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;

namespace CAB.BLL
{
    public class MidnightParameterBLL : IBLL
    {
        MidnightParameterDAL midnightParameterDAL;

        public MidnightParameterBLL()
        {
            midnightParameterDAL = new MidnightParameterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return midnightParameterDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return midnightParameterDAL.InsertData(entity);
        }

        public IEntity GetColumn(long meterDataId)
        {
            return midnightParameterDAL.GetDetailData(meterDataId);
        }

        /// <summary>
        /// Gets Column Names of midnight parameters For MeterID
        /// </summary>
        /// <param name="meterId">A meterID can have multiple files (each having different MeterData_ID</param>
        /// <returns></returns>
        public DataSet GetColumnNamesForMeterID(string meterId)
        {
            return midnightParameterDAL.GetColumnNamesForMeterID(meterId);
        }

        public DataSet GetColumnNames(long meterDataId)
        {
            return midnightParameterDAL.GetColumnNames(meterDataId);
        }

    }
}
