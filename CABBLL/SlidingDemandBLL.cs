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
    public class  SlidingDemandBLL : IBLL
    {
        SlidingDemandDAL SDDAL = null;
       public SlidingDemandBLL()
        {
            SDDAL = new SlidingDemandDAL();
        }
        public void InsertData(IEntity SDEntity)
        {
            SDDAL.InsertData(SDEntity);
        }
        public string GetData(long MeterData_ID)
        {

            SlidingDemandEntity SDEntity = SDDAL.GetData(MeterData_ID);
         return SDEntity.SDData;
        }
        public void DeleteData(long meterDataId)
        {
            SDDAL.DeleteData(meterDataId);
        }
    }
}
