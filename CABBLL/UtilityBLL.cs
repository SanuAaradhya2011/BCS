using CAB.DALC.Data;
using CAB.Framework;
using System.Data;
using System;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.BLL
{
    public class UtilityBLL : IBLL
    {
        UtilityDAL utilityDAL = null;
        DataSet dataSet = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UtilityBLL).ToString());
    
        public UtilityBLL()
        {
            utilityDAL = new UtilityDAL();
        }
        public void InsertData(string utilityPassword, string utilityName)
        {
            
                utilityDAL.InsertData(utilityPassword, utilityName);
          
        }

        public DataSet FetchUtilityPassword()
        {
            dataSet = utilityDAL.GetUtilityPassword();
            return dataSet;
        }
    }
    public static class UtilityDetails
    {
        static DataSet utilityDataSet = null;
        static object syncRoot = new object();
        static UtilityBLL utilityBLL = null;
        static UtilityEntity utilityEntity = UtilityEntity.DEFAULT;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UtilityDetails).ToString());
        public static UtilityEntity Utility
        {
            get
            {
                if (utilityEntity == UtilityEntity.DEFAULT)
                {
                    lock (syncRoot)
                    {
                        utilityEntity = GetUtilityDetails();
                        return utilityEntity;
                    }
                }
                else
                {
                    return utilityEntity;
                }
            }
        }
        public static UtilityEntity GetUtilityDetails()
        {
            try
            {
                utilityBLL = new UtilityBLL();
                utilityDataSet = utilityBLL.FetchUtilityPassword();
                if (utilityDataSet != null && utilityDataSet.Tables[0].Rows.Count > 0)
                {
                    if (utilityDataSet.Tables[0].Rows[0][0].ToString().Trim() == EnumUtil.stringValueOf(UtilityPasswords.Generic))
                    {
                        return UtilityEntity.Generic;
                    }                    
                    else if (utilityDataSet.Tables[0].Rows[0][0].ToString().Trim() == EnumUtil.stringValueOf(UtilityPasswords.DLMS))
                    {
                        return UtilityEntity.Generic;
                    }                          
                    else
                        return UtilityEntity.DEFAULT; 
                }
                else
                {
                    return UtilityEntity.DEFAULT;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetUtilityDetails()", ex);
                return UtilityEntity.DEFAULT;
            }
        }
        /* GKG 21/01/2013 TANGEDCO ISSUE*/
        /// <summary>
        /// Returns true if RYB is coming in phasor data
        /// </summary>
        public static bool RYBInPhasor
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("RYBInPhasor", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// Returns Primary Utility Name. 
        /// Primary utility is the actual entity whose password is entered at the time of login.
        /// </summary>
        /// <returns></returns>
        public static UtilityEntity GetPrimaryUtilityDetails()
        {
            try
            {
                utilityBLL = new UtilityBLL();
                utilityDataSet = utilityBLL.FetchUtilityPassword();
                if (utilityDataSet != null && utilityDataSet.Tables[0].Rows.Count > 0)
                {
                    if (utilityDataSet.Tables[0].Rows[0][0].ToString().Trim() == EnumUtil.stringValueOf(UtilityPasswords.Generic))
                    {
                        return UtilityEntity.Generic;
                    }
                    else if (utilityDataSet.Tables[0].Rows[0][0].ToString().Trim() == EnumUtil.stringValueOf(UtilityPasswords.DLMS))
                    {
                        return UtilityEntity.DLMS;
                    }
                    else
                    {
                        return UtilityEntity.DEFAULT;
                    }
                   
                }
                else
                {
                    return UtilityEntity.DEFAULT;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UtilityEntity GetPrimaryUtilityDetails()", ex);
                return UtilityEntity.DEFAULT;
            }
        }
        //public string UtilityName
        //{
        //    get;
        //    set;
        //}
        //public string UtilityPassword
        //{
        //    get;
        //    set;
        //}
        static UtilityEntity utilityName = UtilityEntity.DEFAULT;

        /// <summary>
        /// Return primary utility Name
        /// </summary>
        public static string PrimaryUtlityName
        {
            get
            {
                if (utilityName == UtilityEntity.DEFAULT)
                {
                    utilityName = GetPrimaryUtilityDetails();
                }
                return utilityName.ToString();
            }
        }

       
        public static bool ShowAnamolyParameters
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowAnamolyParameters", PrimaryUtlityName);
            }
        }
        
        //To display Tou congiguration 
        public static bool ShowTouConfiguration
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowTouConfiguration", PrimaryUtlityName);
            }
        }
        //To enable midnight read 
        public static bool ShowMidnight
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowMidnight", PrimaryUtlityName);
            }
        }
        //To disable CTPT programming  
        public static bool DisableProgrammingCTPTRatio
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableProgrammingCTPTRatio", PrimaryUtlityName);
            }
        }
        //To disable MD reset   
        public static bool DisableProgrammingMDReset
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableProgrammingMDReset", PrimaryUtlityName);
            }
        }
        //To disable DIP programming  
        public static bool DisableProgrammingDemandIntegrationPeriod
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableProgrammingDemandIntegrationPeriod", PrimaryUtlityName);
            }
        }
        //BhardwajG : To manipulate CT/PT values
        public static bool ShowCTRatioFormula
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowCTRatioFormula", PrimaryUtlityName);
            }
        }
        //BhardwajG : To manipulate CT/PT values
        public static bool ShowPTRatioFormula
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowPTRatioFormula", PrimaryUtlityName);
            }
        }
        //To disable SIP programming 
        public static bool DisableProgrammingSurveyCapturePeriod
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableProgrammingSurveyCapturePeriod", PrimaryUtlityName);
            }
        }
       
       
           
        

        //VBM - To disaply ShowKVAHSelectionTab under programming section in US mode.
        public static bool ShowKVAHSelectionTabInUSMode
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowKVAHSelectionTabInUSMode", PrimaryUtlityName);
            }
        }    
        
        public static bool ShowCumulativeExportEnergyKWH
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowCumulativeExportEnergyKWH", PrimaryUtlityName);
            }
        }
        //VBM - To disaply Apparant energy in Tamper
        public static bool ShowApparantEnergyInTamper
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowApparantEnergyInTamper", PrimaryUtlityName);
            }
        } 
       
            
        /* VBM - To display Average power factor in Load Survey */
        /* VBM - To display kvah selection changed tamper in transaction */
        public static bool ShowkVAhSelectionTamperInTransaction
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowkVAhSelectionTamperInTransaction", PrimaryUtlityName);
            }
        }
       
     
        public static bool ShowAnomalyFastDownloadInNormalMode
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowAnomalyFastDownloadInNormalMode", PrimaryUtlityName);
            }
        }
        
        /// <summary>
        /// Gets the GPRS enable bool for login utility
        /// </summary>
        public static bool ShowGPRSCommunication
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowGPRSCommunication", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// VBM - Show display parameter in US mode .
        /// </summary>
        public static bool ShowDisplayParametersInUSMode
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowDisplayParametersInUSMode", PrimaryUtlityName);
            }
        }       

      
        
        /// <summary>
        /// VBM - To show vlotagephasereversal tamper.
        /// </summary>
        public static bool ShowVoltagePhaseReversalTamper
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowVoltagePhaseReversalTamper", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// VBM - To disable programming billing date and time.
        /// </summary>
        public static bool DisableProgrammingBillingDateTime
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableProgrammingBillingDateTime", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// Show MD Reset tamper 
        /// </summary>
        public static bool ShowMDResetTamper
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowMDResetTamper", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// Show display parameter tamper 
        /// </summary>
        public static bool ShowDisplayParemeterTamper
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowDisplayParemeterTamper", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// Show display CUM MD KW and KVA feature wise.
        /// </summary>
        public static bool ShowCumulativeMDKWKVA
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowCumulativeMDKWKVA", PrimaryUtlityName);
            }
        }
       
        /// <summary>
        /// Show Two Byte PT ratio 
        /// </summary>
        public static bool ShowTwoBytePTRatio
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowTwoBytePTRatio", PrimaryUtlityName);
            }
        }
       /// <summary>
        /// Support IEC Communication
        /// </summary>
        public static bool IECSupport
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("IECSupport", PrimaryUtlityName);
            }
        }
       
        
        /// <summary>
        /// Show power failure duration in billing
        /// </summary>
        public static bool ShowPowerOffDurationInBilling
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowPowerOffDurationInBilling", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// to enable and disable fast download read.
        /// </summary>
        public static bool FastDownloadRead
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("FastDownloadRead", PrimaryUtlityName);
            }
        }

    }

  
  
}
