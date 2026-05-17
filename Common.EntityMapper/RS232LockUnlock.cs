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
    /// Used for  Filling RS232 lock/Unlock data 
    /// </summary>
    public class RS232LockUnlock
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RS232LockUnlock).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill RS232Lock data  Selection data .
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public RS232LockEntity GetData(List<ProfileData> rs232LOckUnlockData)
        {
            RS232LockEntity rs232LOckUnlock = new RS232LockEntity();
            try
            {
                if (rs232LOckUnlockData[0].ListMeterDataPacket.Count > 0 && rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    rs232LOckUnlock.LockStatus = rs232LOckUnlockData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "1" 
                                                 ? "Locked" : "NotLocked";                   
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> rs232LOckUnlockData)", ex);
            }
            return rs232LOckUnlock;

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
