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
using CAB.IECFramework;
using CAB.IECFramework.Entity;

namespace CAB.BLL
{
    public class DTMDailyProfileParameterBLL : IBLL
    {
        DTMDailyProfileParameterDAL dTMDailyProfileParameterDAL;

        public DTMDailyProfileParameterBLL()
        {
            dTMDailyProfileParameterDAL = new DTMDailyProfileParameterDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return dTMDailyProfileParameterDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return dTMDailyProfileParameterDAL.InsertData(entity);
        }

        public IEntity GetColumn(long meterDataId)
        {
            return dTMDailyProfileParameterDAL.GetDetailData(meterDataId);
        }
    }
}

