#region NameSpaces
using System;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using LTCTDAL;
#endregion
namespace LTCTBLL
{
    public class ManualMDResetBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity)
        {
            return new ManualMDResetDAL().InsertData(entity);
        }
        public ManualMDResetEntity GetData(Int64 MeterData_ID)
        {
            return new ManualMDResetDAL().GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return new ManualMDResetDAL().DeleteData(meterDataId);
        }
    }
}
