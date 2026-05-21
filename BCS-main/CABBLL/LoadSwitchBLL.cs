/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Deep. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using System.Data;
using CAB.Framework.Entity;
using System.Windows.Forms;
using System.Collections.Generic;
using Hunt.EPIC.Logging;
using CAB.Framework;


namespace CAB.BLL
{
    public class LoadSwitchBLL : IBLL
    {
        LoadSwitchDAL loadswitchDAL;
        DLMS650CommonBLL dLMS650CommonBLL;
        public LoadSwitchBLL()
        {
            dLMS650CommonBLL = new DLMS650CommonBLL();
            loadswitchDAL = new LoadSwitchDAL();
        }

             
        public IEntity InsertData(IList<IEntity> entities)
        {
            return loadswitchDAL.InsertData(entities);
        }
        public DataSet GetMeterData(int meterDataID)
        {
            DataSet dataSet = null;
            dataSet = dLMS650CommonBLL.ConvertLoadSwitchRowToColumn(loadswitchDAL.GetMeterData(meterDataID), meterDataID);
          
             return dataSet;   
            
           
        }
        //public bool DeleteData(long meterDataId)
        //{
        //    return loadswitchDAL.DeleteData(meterDataId);
        //}
    }
}


