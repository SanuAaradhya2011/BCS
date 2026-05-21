#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CABEntity;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Holds Billing Type data 
    /// </summary>
    public class BillingDateTime
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingDateTime).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public BillingTypeEntity GetData(List<ProfileData> billingTypeData, List<ProfileData> billingMonthTypeData)
        {
            BillingTypeEntity billingType = new BillingTypeEntity();
            try
            {
                if (billingTypeData[0].ListMeterDataPacket.Count > 0 && billingTypeData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {

                    string actualData = billingTypeData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    string billingMode = actualData.Substring(0, 2);
                    if (billingMode == "00")
                    {
                        billingType.ModeOfBilling = BillingMode.EndofMonth;
                        billingType.Day = "01";
                        billingType.Hours = "00";
                        billingType.Minutes = "00";
                        billingType.ResetLockOutDays = "00";
                    }
                    else 
                    {
                        billingType.ModeOfBilling = BillingMode.UserDefined;
                        billingType.Day = actualData.Substring(0, 2);                        
                        billingType.Hours = actualData.Substring(2, 2);                      
                        billingType.Minutes = actualData.Substring(4, 2);
                        //if (actualData.Length == 10)
                        //{
                        //    billingType.ResetLockOutDays = actualData.Substring(6, 4);
                        //}
                        //else
                        //{
                        //    billingType.ResetLockOutDays = "----";
                        //}

                    }
                    billingType.ResetLockOutDays = "----";
					// [BillingType_Month]
                    if (billingMonthTypeData != null && billingMonthTypeData.Count > 0)
                    {
                        billingType.BillingType = billingMonthTypeData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value.ToString().PadLeft(2,'0');

                    }
                    else
                    {
                        billingType.BillingType = "255";
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> billingTypeData, List<ProfileData> billingMonthTypeData)", ex);
            }
            return billingType;

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
