using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.DALC.Data;
using System.Collections.ObjectModel;
using CAB.Entity;

namespace LTCTBLL
{
    public class DisplayParameterBLL :IBLL
    {
        DisplayParamaterDAL displayParamaterDAL;

        public DisplayParameterBLL()
        {
            displayParamaterDAL = new DisplayParamaterDAL();
        }
        public bool InsertData(Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity,Int64 MeterData_ID)
        {                      
            return displayParamaterDAL.InsertData(collDisplayParamatersDBEntity, MeterData_ID);
        }
        public Collection<Collection<string>> GetData( Int64 MeterData_ID)
        {
            return displayParamaterDAL.GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return displayParamaterDAL.DeleteAllData(meterDataId);
        }
    }
}
