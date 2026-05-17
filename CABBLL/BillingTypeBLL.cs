using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using LTCTDAL;
using CAB.Framework;
using CAB.Framework.Entity;

namespace LTCTBLL
{
    public class BillingTypeBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity)
        {
            return new BillingTypeDAL().InsertData(entity);
        }
        public BillingTypeEntity GetData( Int64 MeterData_ID)
        {
            return new BillingTypeDAL().GetData( MeterData_ID);
        }

         public bool DeleteData(long meterData_ID)
        {
            return new BillingTypeDAL().DeleteData(meterData_ID);
        }

    }
}
