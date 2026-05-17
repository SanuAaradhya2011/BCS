using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using LTCTDAL;

namespace LTCTBLL
{
   public  class RS232BLL : IBLL
    {
       public IEntity Insertdata(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
       {
           return new RS232DAL().InsertData(entity, fileUploadID, MeterData_ID);
       }
       public IECRS232LockEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
       {
           return new RS232DAL().GetData(meterID, fileUploadID, MeterData_ID);
       }
    }
}
