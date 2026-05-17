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
    public class  RJLockUnlockBLL : IBLL
    {
        RJLockUnlockDAL RJDAL = null;
        public RJLockUnlockBLL()
        {
            RJDAL = new RJLockUnlockDAL();
        }
        public void InsertData(IEntity RJEntity)
        {
            RJDAL.InsertData(RJEntity);
        }
        public string GetData(long MeterData_ID)
        {

            RJLockUnlockEntity  RJEntity = RJDAL.GetData(MeterData_ID);
            return RJEntity.RJData;
        }
        public void DeleteData(long meterDataId)
        {
            RJDAL.DeleteData(meterDataId);
        }
    }
}
