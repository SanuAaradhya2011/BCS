#region NameSpaces
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
#endregion
namespace CAB.BLL
{
    /// <summary>
    /// TOD BLL
    /// </summary>
    public class TodBLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        TodDAL todDAL = null;
        public TodBLL()
        {
            todDAL = new TodDAL();
        }
        public void InsertData(IEntity todEntity)
        {
            todDAL.InsertData(todEntity);
        }
        public string GetData(long MeterData_ID)
        {

            TODEntity todEntity = todDAL.GetData(MeterData_ID);
            return todEntity.TODData;
        }
        public void DeleteData(long meterDataId)
        {
            todDAL.DeleteData(meterDataId);
        }
    }
}
