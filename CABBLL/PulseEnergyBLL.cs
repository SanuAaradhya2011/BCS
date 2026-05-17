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
    public class PulseEnergyBLL : IBLL
    {
        #region Constants & Variables
        private PulseEnergyDAL pulseEnergyDAL;
        #endregion

        #region Constructer
        public PulseEnergyBLL()
        {
            pulseEnergyDAL = new PulseEnergyDAL();
        }
        #endregion

        #region Public Methods
        public IEntity InsertData(IEntity entity)
        {
            return pulseEnergyDAL.InsertData(entity);
        }
        public string GetData(Int64 MeterData_ID)
        {
            return pulseEnergyDAL.GetData(MeterData_ID);
        }
        public bool DeleteData(long meterDataId)
        {
            return pulseEnergyDAL.DeleteData(meterDataId);
        }
        #endregion
    }
}
