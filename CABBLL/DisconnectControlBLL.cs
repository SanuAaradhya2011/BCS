using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.BLL;
using CABDAL;
using CABEntity;
using CAB.Framework.Entity;

namespace LTCTBLL
{
    public class DisconnectControlBLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        DisconnectControlDAL DCDAL = null;
        public DisconnectControlBLL()
        {
            DCDAL = new DisconnectControlDAL();
        }
        public void InsertData(IEntity DCEntity)
        {
            DCDAL.InsertData(DCEntity);
        }
        public string GetData(long MeterData_ID)
        {

            DisconnectControlEntity DCEntity = DCDAL.GetData(MeterData_ID);
            return DCEntity.DCData;
        }
        public void DeleteData(long meterDataId)
        {
            DCDAL.DeleteData(meterDataId);
        }
    }
}
