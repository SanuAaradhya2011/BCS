using CAB.DALC.Data;
using System.Data;
using CAB.DALC.Data.DataServices;
using System;
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;

namespace CAB.BLL
{
    public class kvarSelectionBLL : IBLL 
    {
        public IEntity InsertData(IEntity entity)
        {
            return new kvarSelectionDAL().InsertData(entity);
        }
        public MeterConfigurationsNFEntity GetData( Int64 MeterData_ID)
        {
            return new kvarSelectionDAL().GetData( MeterData_ID);
        }

        public bool DeleteData(long meterData_ID)
        {
            return new kvarSelectionDAL().DeleteData(meterData_ID);
        }



    }
}
