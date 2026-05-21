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
    public class RS485BLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        RS485DAL RSDAL = null;
        public RS485BLL()
        {
            RSDAL = new RS485DAL();
        }
        public void InsertData(IEntity DCEntity)
        {
            RSDAL.InsertData(DCEntity);
        }
        public string GetData(long MeterData_ID)
        {

            RS485Entity DCEntity = RSDAL.GetData(MeterData_ID);
            return DCEntity.DCData;
        }
        public void DeleteData(long meterDataId)
        {
            RSDAL.DeleteData(meterDataId);
        }
    }
}
