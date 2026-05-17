#region NameSpaces
using System;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
#endregion
namespace CAB.BLL
{
    /// <summary>
    /// DIP BLL
    /// </summary>
    public class DIPBLL : IBLL
    {
        #region Constants & Variables
        private DIPDAL dipDAL;
        #endregion

        #region Constructer
        public DIPBLL()
        {
            dipDAL = new DIPDAL();
        }
        #endregion

        #region Public Methods
        public IEntity InsertData(IEntity entity)
        {
            return dipDAL.InsertData(entity);
        }
        public int GetData(Int64 MeterData_ID)
        {
            return dipDAL.GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return dipDAL.DeleteData(meterDataId);
        }
        #endregion
    }
}
