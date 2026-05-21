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
    public class LoadControlBLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        LoadControlDAL LCDAL = null;
        public LoadControlBLL()
        {
            LCDAL = new LoadControlDAL();
        }
        public void InsertData(IEntity LCEntity)
        {
            LCDAL.InsertData(LCEntity);
        }
        public string GetData(long MeterData_ID)
        {

           LoadControlEntity DCEntity = LCDAL.GetData(MeterData_ID);
            return DCEntity.LCData;
        }
        public void DeleteData(long meterDataId)
        {
            LCDAL.DeleteData(meterDataId);
        }
    }
}
