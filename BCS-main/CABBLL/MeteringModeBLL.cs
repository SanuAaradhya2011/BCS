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
    public class MeteringModeBLL : IBLL
    {
        
        MeteringModeDAL MMDAL = null;
        public MeteringModeBLL()
        {
            MMDAL = new MeteringModeDAL();
        }
        public void InsertData(IEntity MMEntity)
        {
            MMDAL.InsertData(MMEntity);
        }
        public string GetData(long MeterData_ID)
        {

            MeteringModeEntity MMEntity = MMDAL.GetData(MeterData_ID);
            return MMEntity.MMData;
        }
        public void DeleteData(long meterDataId)
        {
            MMDAL.DeleteData(meterDataId);
        }
    }
}
