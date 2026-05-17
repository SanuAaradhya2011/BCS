using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CABDAL;
using CABEntity;
using CAB.BLL;
using CAB.Framework.Entity;

namespace LTCTBLL
{
    public class  OpticalLockUnlockBLL : IBLL
    {
        OpticalLockUnlockDAL OPDAL = null;
        public OpticalLockUnlockBLL()
        {
            OPDAL = new OpticalLockUnlockDAL();
        }
        public void InsertData(IEntity OPEntity)
        {
            OPDAL.InsertData(OPEntity);
        }
        public string GetData(long MeterData_ID)
        {

            OpticalLockUnlockEntity OPEntity = OPDAL.GetData(MeterData_ID);
            return OPEntity.OPData;
        }
        public void DeleteData(long meterDataId)
        {
            OPDAL.DeleteData(meterDataId);
        }
    }
}
