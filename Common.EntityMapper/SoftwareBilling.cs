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
    /// Used for  Filling SoftwareBilling data 
    /// </summary>
    public class SoftwareBilling
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SoftwareBilling).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

       /// <summary>
        /// Used to fill Software Billing data  Selection data .
       /// </summary>
       /// <param name="softwareBillingData"></param>
       /// <returns></returns>
        public SoftwareBillingEntity GetData(List<ProfileData> softwareBillingData)
        {
            SoftwareBillingEntity softwareBillingEntity = new SoftwareBillingEntity();
            try
            {
                if (softwareBillingData[0].ListMeterDataPacket.Count > 0 && softwareBillingData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    softwareBillingEntity.SoftwareBillingStatus = softwareBillingData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString() == "0"
                                                 ? "Disable" : "Enable";
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> softwareBillingData)", ex);
            }
            return softwareBillingEntity;

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
