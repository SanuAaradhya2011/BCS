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
    public class LoadLimitBLL : IBLL
    {
       
        LoadLimitDAL LLDAL = null;
        public LoadLimitBLL()
        {
            LLDAL = new LoadLimitDAL();
        }
        public void InsertData(IEntity LLEntity)
        {
            LLDAL.InsertData(LLEntity);
        }
        public string GetData(long MeterData_ID)
        {

            LoadLimitEntity LLEntity = LLDAL.GetData(MeterData_ID);
            return LLEntity.LLData;
        }
        public void DeleteData(long meterDataId)
        {
            LLDAL.DeleteData(meterDataId);
        }
    }
}
