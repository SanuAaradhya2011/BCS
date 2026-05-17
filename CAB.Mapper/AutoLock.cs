#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABEntity;
using CAB.Parser;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Used for  Filling AutoLock data 
    /// </summary>
    public class AutoLock
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill auto lock data  Selection data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public IECAutoLockEntity GetData(List<ProfileData> rs232LOckUnlockData)
        {
            IECAutoLockEntity autoLockEntity = new IECAutoLockEntity();
            try
            {
                if (rs232LOckUnlockData[0].ListMeterDataPacket.Count > 0 && rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    autoLockEntity.AutoLockStatus = rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "1"
                                                 ? "Locked" : "NotLocked";
                }
            }
            catch (Exception)
            {

            }
            return autoLockEntity;

        }


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods


        #endregion
    }
}
