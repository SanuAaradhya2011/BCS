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
    public class NamePlateDetailBLL : IBLL
    {
        NamePlateDetailDAL namePlateDetailDAL;

        public NamePlateDetailBLL()
        {
            namePlateDetailDAL = new NamePlateDetailDAL();
        }

        public bool DeleteData(long meterDataId)
        {
            return namePlateDetailDAL.DeleteData(meterDataId);
        }
        public IEntity InsertData(IEntity entity)
        {
            return namePlateDetailDAL.InsertData(entity);
        }
        public DataSet ListDataSet(long meterDataId)
        {
            return CommonBLL.ConvertNamePlateDetailRowToColumn(namePlateDetailDAL.ListDataSet(meterDataId));
        } 
    }
}


