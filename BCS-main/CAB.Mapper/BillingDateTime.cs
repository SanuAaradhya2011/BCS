#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CABEntity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// Holds Billing Type data 
    /// </summary>
    public class BillingDateTime
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
        /// </summary>
        /// <param name="namePlateData"></param>
        /// <returns></returns>
        public BillingResetEntity GetData(List<ProfileData> billingTypeData)
        {
            BillingResetEntity billingType = new BillingResetEntity();
            try
            {
                if (billingTypeData[0].ListMeterDataPacket.Count > 0 && billingTypeData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {

                    string actualData = billingTypeData[0].ListMeterDataPacket[0].ListDataElementValue[0].Value;
                    string billingMode = actualData.Substring(0, 2);
                    if (billingMode == "00")
                    {
                        billingType.ModeOfBilling = IECBillingMode.EndofMonth;
                        billingType.Day = "01";
                        billingType.Hours = "00";
                        billingType.Minutes = "00";                        
                    }
                    else 
                    {
                        billingType.ModeOfBilling = IECBillingMode.UserDefined;
                        billingType.Day = actualData.Substring(0, 2);                        
                        billingType.Hours = actualData.Substring(2, 2);                      
                        billingType.Minutes = actualData.Substring(4, 2);                        
                    }

                    
                    
                }

            }
            catch (Exception)
            {

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
