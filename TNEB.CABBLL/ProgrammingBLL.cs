
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
using System.Collections.Generic;

namespace CAB.BLL
{
    public class ProgrammingBLL : IBLL
    {
        ProgrammingDAL programmingDAL;

        public ProgrammingBLL()
        {
            programmingDAL = new ProgrammingDAL();
        }
        public bool DeleteData(long meterDataId)
        {
            return programmingDAL.DeleteData(meterDataId);
        }
        public IEntity InsertData(IEntity entity)
        {
            return programmingDAL.InsertData(entity);
        }
        public IEntity InsertData(List<IEntity> entities)
        {
            return programmingDAL.InsertData(entities);
        }
        public DataSet GetProgrammingList(long meterDataId)
        { 
            DataSet dataSet= programmingDAL.GetProgrammingList(meterDataId);
            return CommonBLL.ConvertProgramming(dataSet);
        }  
        public int GetTotalProgrammingUpdates(int meterDataId)
        {
            return programmingDAL.GetTotalProgrammingUpdates(meterDataId);
        }
    }
}

