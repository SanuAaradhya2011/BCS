#region NameSpaces
using System;
using System.Collections.Generic;
using CAB.Parser;
using CABEntity;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Used for  Filling ManualMDReset data 
    /// </summary>
    public class ManualMDReset
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ManualMDReset).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill Software Billing data  Selection data .
        /// </summary>
        /// <param name="ManualMDReset"></param>
        /// <returns></returns>
        public ManualMDResetEntity GetData(List<ProfileData> ManualMDResetData)
        {
            ManualMDResetEntity ManualMDResetEntity = new ManualMDResetEntity();
            try
            {
                if (ManualMDResetData[0].ListMeterDataPacket.Count > 0 && ManualMDResetData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    ManualMDResetEntity.ManualMDResetStatus = ManualMDResetData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "0"
                                                 ? "Disable" : "Enable";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> ManualMDResetData)", ex);
            }
            return ManualMDResetEntity;

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
