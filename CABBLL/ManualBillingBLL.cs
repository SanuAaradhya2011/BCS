#region NameSpaces
using System;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using LTCTDAL;
#endregion
namespace LTCTBLL
{
    public class ManualBillingBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity)
        {
            return new ManualBillingDAL().InsertData(entity);
        }
        public ManualBillingEntity GetData(Int64 MeterData_ID)
        {
            return new ManualBillingDAL().GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return new ManualBillingDAL().DeleteData(meterDataId);
        }
    }
}
