/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System;

namespace CAB.BLL
{
    public class TamperCounterGeneralBLL : IBLL
    {
        TamperCounterGeneralDAL tamperCounterGeneralDAL;

        public TamperCounterGeneralBLL()
        {
            tamperCounterGeneralDAL = new TamperCounterGeneralDAL();
        }

        public IEntity InsertData(IEntity entity, bool flag)
        {
            return tamperCounterGeneralDAL.InsertData(entity, flag);
        }
        public DataSet GetTamperCounter(int meterDataId,string tamperName)
        {
            DataSet ds = tamperCounterGeneralDAL.GetTamperCounter(meterDataId, tamperName);
            return CommonBLL.ConvertamperCounterToColumn(ds);
        }
        public int GetTamperCount(int meterDataId, string tamperName)
        {
            try
            {
                object obj = tamperCounterGeneralDAL.GetTamperCount(meterDataId, tamperName);
                if (obj == null)
                    return 0;
                else
                    return Convert.ToInt32(obj);
            }
            catch (Exception)
            {
                return 0;
            }
        } 
    }
}



