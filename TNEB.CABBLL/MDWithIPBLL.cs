using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.DALC.Data.DataServices;
using System;
using System.Collections.Generic;
using CAB.Entity;

namespace CAB.BLL
{
    public class MDWithIPBLL : IBLL
    {
        public IEntity InsertData(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            return new MDWithIPDAL().InsertData(entity, fileUploadID, MeterData_ID);
        }
        public IECMeterConfigurationsNFEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            return new MDWithIPDAL().GetData(meterID, fileUploadID, MeterData_ID);
        }
    }
}
