using System.Data;
using CAB.DALC.Data;
using CAB.Framework.Entity;
using CAB.Entity;
using System.Collections.Generic;

namespace CAB.BLL
{
    public class GSMLoggingBLL
    {
        GSMLoggingDAL gsmLoggingDAL;

        public GSMLoggingBLL()
        {
            gsmLoggingDAL = new GSMLoggingDAL();
        }

        public List<GSMLoggingEntity> GetLogsByTaskID(int id)
        {
            return gsmLoggingDAL.GetLogsByTaskID(id);
        }

        public IEntity InsertData(IEntity entity, bool flag)
        {
            return gsmLoggingDAL.InsertData(entity, flag);
        }

        public IEntity InsertorUpdateData(IEntity entity, bool flag)
        {
            return gsmLoggingDAL.InsertorUpdateData(entity, flag);
        }

        public IEntity RowToEntity(DataRow row)
        {
            return gsmLoggingDAL.RowToEntity(row);
        }
        public bool UpdateData(IEntity entity)
        {
            return gsmLoggingDAL.UpdateData(entity);
        }
        //public bool UpdateRetries()
    }
}