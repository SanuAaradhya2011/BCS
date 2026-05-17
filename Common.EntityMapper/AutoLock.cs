#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABEntity;
using CAB.Parser;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Used for  Filling AutoLock data 
    /// </summary>
    public class AutoLock
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AutoLock).ToString());
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
        public AutoLockEntity GetData(List<ProfileData> rs232LOckUnlockData)
        {
            AutoLockEntity autoLockEntity = new AutoLockEntity();
            try
            {
                if (rs232LOckUnlockData[0].ListMeterDataPacket.Count > 0 && rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    autoLockEntity.AutoLockStatus = rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "0"
                                                 ? "Locked" : "NotLocked";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> rs232LOckUnlockData)", ex);
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
