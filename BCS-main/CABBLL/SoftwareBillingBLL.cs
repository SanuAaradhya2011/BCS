#region NameSpaces
using System;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using LTCTDAL;
#endregion
namespace LTCTBLL
{
    public class SoftwareBillingBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity)
        {
            return new SoftwareBillingDAL().InsertData(entity);
        }
        public SoftwareBillingEntity GetData(Int64 MeterData_ID)
        {
            return new SoftwareBillingDAL().GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return new SoftwareBillingDAL().DeleteData(meterDataId);
        }
    }
}
