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
    public class PaymentModeBLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        PaymentModeDAL PMDAL = null;
        public PaymentModeBLL()
        {
            PMDAL = new PaymentModeDAL();
        }
        public void InsertData(IEntity PMEntity)
        {
            PMDAL.InsertData(PMEntity);
        }
        public string GetData(long MeterData_ID)
        {

            PaymentModeEntity DCEntity = PMDAL.GetData(MeterData_ID);
            return DCEntity.PMData;
        }
        public void DeleteData(long meterDataId)
        {
            PMDAL.DeleteData(meterDataId);
        }
    }
}
