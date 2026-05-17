
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
    public class PhasorBLL : IBLL
    {
        PhasorDAL phasorDAL;

        public PhasorBLL()
        {
            phasorDAL = new PhasorDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return phasorDAL.DeleteData(meterDataId);
        }
        public IEntity InsertData(IEntity entity)
        {
            return phasorDAL.InsertData(entity);
        }
        public DataSet ListDataSet(long meterDataId)
        {
            return CommonBLL.ConvertPhasorToColumn( phasorDAL.ListDataSet(meterDataId));
        }
        public IEntity GetPhasorDataEntity(int meterDataId)
        {
           return phasorDAL.GetDetailData(meterDataId);
        }
        public IEntity GetPhasorDataEntity(DataTable dtTable)
        {
            return phasorDAL.RowToEntity(dtTable.Rows[0],true);
        }
    }
}


