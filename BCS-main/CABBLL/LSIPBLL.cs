#region NameSpaces
using System;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
#endregion
namespace CAB.BLL
{
    /// <summary>
    /// LSIP BLL
    /// </summary>
    public class LSIPBLL : IBLL
    {
        #region Constants & Variables
        private LSIPDAL lsipDAL;
        #endregion

        #region Constructer
        public LSIPBLL()
        {
            lsipDAL = new LSIPDAL();
        }
        #endregion

        #region Public Methods
        public IEntity InsertData(IEntity entity)
        {
            return lsipDAL.InsertData(entity);
        }
        public int GetData(Int64 MeterData_ID)
        {
            return lsipDAL.GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return lsipDAL.DeleteData(meterDataId);
        }
        #endregion
    }
}
