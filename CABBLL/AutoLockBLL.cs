#region NameSpaces
using System;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using LTCTDAL;
#endregion
namespace LTCTBLL
{
    public class AutoLockBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity)
        {
            return new AutoLockDAL().InsertData(entity );
        }
        public AutoLockEntity GetData( Int64 MeterData_ID)
        {
            return new AutoLockDAL().GetData( MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return new AutoLockDAL().DeleteData(meterDataId);
        }
    }
}
