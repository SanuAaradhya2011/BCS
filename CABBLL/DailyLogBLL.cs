using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using LTCTDAL;

namespace LTCTBLL
{
   public class DailyLogBLL : IBLL
    {
       public IEntity Insertdata(IEntity entity, Int64 fileUploadID,Int64 MeterData_ID)
       {
           return new DailyLogDAL().InsertData(entity);
       }
       public DailyLogEntity GetData(Int64 MeterData_ID)
       {
           return new DailyLogDAL().GetData( MeterData_ID);
       }

       public bool DeleteData(long meterData_ID)
       {
           return new DailyLogDAL().DeleteData(meterData_ID);
       }
       

    }
}
