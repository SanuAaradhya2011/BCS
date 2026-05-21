#region NameSpaces
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
#endregion
namespace LTCTBLL
{
    public class AutoLockBLL : IBLL
    {
        public IEntity Insertdata(IEntity entity, Int64 fileUploadID, Int64 MeterData_ID)
        {
            return new AutoLockDAL().InsertData(entity, fileUploadID, MeterData_ID);
        }
        public IECAutoLockEntity GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            return new AutoLockDAL().GetData(meterID, fileUploadID, MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return new AutoLockDAL().DeleteData(meterDataId);
        }
    }
}
