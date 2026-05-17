using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using LTCTDAL;
using CAB.IECFramework.Utility;

namespace LTCTBLL
{
    public class UtilityBLL : IBLL
    {
        public void InsertData(string UtilityPassword,string UtilityName)
        {
            new UtilityDAL().InsertData(UtilityPassword, UtilityName);
        }
        public DataSet FetchUtilityPassword()
        {
            DataSet ds = new DataSet();
            ds = new UtilityDAL().GetUtilityPassword();
            return ds;
        }
    }
    public static class UtilityDetails
    {
        public static CAB.Entity.IECUtilityEntity UtilityName;
        static UtilityDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = new UtilityBLL().FetchUtilityPassword();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "m7UboMvP")
                        UtilityName = CAB.Entity.IECUtilityEntity.UGVCL;
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "7GD7WpNp")
                        UtilityName = CAB.Entity.IECUtilityEntity.TNEB;
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "7GD8WpNp")
                        UtilityName = CAB.Entity.IECUtilityEntity.TNEB1;
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "q6goJ8s9")
                        UtilityName = CAB.Entity.IECUtilityEntity.PVVNL;
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "23rtYuw1") 
                        UtilityName = CAB.Entity.IECUtilityEntity.WBEXPORTVCL;
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "87ftBpm1")
                        UtilityName = CAB.Entity.IECUtilityEntity.JDVVNL;
                }
                else
                    UtilityName = CAB.Entity.IECUtilityEntity.Any;
            }
            catch (Exception eE)
            {
            }
        }

         /// <summary>
        /// Returns Primary Utility Name. 
        /// Primary utility is the actual entity whose password is entered at the time of login.
        /// </summary>
        /// <returns></returns>
        public static CAB.Entity.IECUtilityEntity GetPrimaryUtilityDetails()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = new UtilityBLL().FetchUtilityPassword();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString().Trim() == "m7UboMvP")
                        return CAB.Entity.IECUtilityEntity.UGVCL;
                    else if (ds.Tables[0].Rows[0][0].ToString().Trim() == "7GD7WpNp")
                        return CAB.Entity.IECUtilityEntity.TNEB;
                   else if (ds.Tables[0].Rows[0][0].ToString().Trim() == "7GD8WpNp")
                        return CAB.Entity.IECUtilityEntity.TNEB1;
                    else if (ds.Tables[0].Rows[0][0].ToString().Trim() == "q6goJ8s9")
                        return CAB.Entity.IECUtilityEntity.PVVNL;
                    else if (ds.Tables[0].Rows[0][0].ToString().Trim() == "23rtYuw1")
                       return CAB.Entity.IECUtilityEntity.WBEXPORTVCL;
                    else if (ds.Tables[0].Rows[0][0].ToString().Trim() == "87ftBpm1")
                        return CAB.Entity.IECUtilityEntity.JDVVNL;
                    else
                        return CAB.Entity.IECUtilityEntity.Any;
                }
                else
                    return CAB.Entity.IECUtilityEntity.Any;
            }
            catch (Exception eE)
            {
                return CAB.Entity.IECUtilityEntity.Any;
            }
        }

        static CAB.Entity.IECUtilityEntity utilityName = CAB.Entity.IECUtilityEntity.Any;

        /// <summary>
        /// Return primary utility Name
        /// </summary>
        public static string PrimaryUtlityName
        {
            get
            {
                if (utilityName == CAB.Entity.IECUtilityEntity.Any)
                {
                    utilityName = GetPrimaryUtilityDetails();
                }
                return utilityName.ToString();
            }
        }
        /// <summary>
        /// Returns true if POH is applicable in dtm daily profile
        /// </summary>
        public static bool ShowPowerOnHours
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowPohInDTMDailyProfile",PrimaryUtlityName);
            }
        }
        /// <summary>
        /// Returns true if Dynamic Phaor Tabe has to visible for utility
        /// </summary>
        public static bool ShowPowerOnHoursInMinutes
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("ShowPohMinutesInGeneralBilling", PrimaryUtlityName);
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
        /// To hide "End of Month" option in billing mode under meter configuration section.
        /// </summary>
        public static bool DisableEndOfMonthOptionBillingMode
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisableEndOfMonthOptionBillingMode", PrimaryUtlityName);
            }
        }
        /// <summary>
        /// To hide power on hours option in billing .
        /// </summary>
        public static bool DisablePowerOnHourInBilling
        {
            get
            {
                return CommonMethods.ShowFunctionalityToUtility("DisablePowerOnHourInBilling", PrimaryUtlityName);
            }
        }
    }
    
    

         
}
