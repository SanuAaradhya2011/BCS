using CAB.DALC.Data;
using System.Data;
using CAB.DALC.Data.DataServices;
using System;
using System.Collections.Generic;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.Framework;

namespace CAB.BLL
{
    public class MDWithIPBLL : IBLL
    {
        public IEntity InsertData(IEntity entity)
        {
            return new MDWithIPDAL().InsertData(entity);
        }
        public MeterConfigurationsNFEntity GetData( Int64 MeterData_ID)
        {
            return new MDWithIPDAL().GetData( MeterData_ID);
        }
        public bool DeleteData(long meterData_ID)
        {
            return new MDWithIPDAL().DeleteData(meterData_ID);
        }
    }
}
