/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using System.Data;
using CAB.Framework.Entity;
using CAB.Framework;

namespace CAB.BLL
{
    public class FraudEnergyBLL : IBLL
    {
        FraudEnergyDAL fraudEnergyDAL;

        public FraudEnergyBLL()
        {
            fraudEnergyDAL = new FraudEnergyDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return fraudEnergyDAL.InsertData(entity);
        }
        public IEntity GetFraudEnergy(long meterDataId)
        {
            return fraudEnergyDAL.GetFraudEnergy(meterDataId);          
        }

        public DataSet GetFraudEnergyDataSet(long meterDataId)
        {
            DataSet ds = fraudEnergyDAL.GetFraudEnergyDataSet(meterDataId);
            return CommonBLL.ConvertFraudEnergyRowToColumn(ds);
        }

        public bool DeleteData(long meterDataId)
        {
            return fraudEnergyDAL.DeleteData(meterDataId);
        }
    }
}


