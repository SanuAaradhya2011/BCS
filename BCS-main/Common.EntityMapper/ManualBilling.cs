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
    /// Used for  Filling ManualBilling data 
    /// </summary>
    public class ManualBilling
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ManualBilling).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill Manual Billing data  Selection data .
        /// </summary>
        /// <param name="manualBillingData"></param>
        /// <returns></returns>
        public ManualBillingEntity GetData(List<ProfileData> manualBillingData)
        {
            ManualBillingEntity manualBillingEntity = new ManualBillingEntity();
            try
            {
                if (manualBillingData[0].ListMeterDataPacket.Count > 0 && manualBillingData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    manualBillingEntity.ManualBillingStatus = manualBillingData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "0"
                                                 ? "Disable" : "Enable";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> manualBillingData)", ex);
            }
            return manualBillingEntity;

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
