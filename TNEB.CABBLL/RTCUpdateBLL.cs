
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;

namespace CAB.BLL
{
    public class RTCUpdateBLL : IBLL
    {
        RTCUpdateDAL rTCUpdateDAL;

        public RTCUpdateBLL()
        {
            rTCUpdateDAL = new RTCUpdateDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return rTCUpdateDAL.DeleteData(meterDataId);
        }

        public IEntity InsertData(IEntity entity)
        {
            return rTCUpdateDAL.InsertData(entity);
        }
        public DataSet GetRTCUpdateList(long meterDataId)
        {
            return CommonBLL.ConvertRTCUpdatesRowToColumn(rTCUpdateDAL.GetRTCUpdateList(meterDataId));
        }
        public int GetTotalRTCUpdates(long meterDataId)
        {
            return rTCUpdateDAL.GetTotalRTCUpdates(meterDataId);
        }
 
    }
}

