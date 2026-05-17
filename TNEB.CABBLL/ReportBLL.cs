/* |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 22/04/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Collections.Generic;

namespace CAB.BLL
{
    public class ReportBLL : IBLL
    {

        ReportDAL reportDAL;

        public ReportBLL()
        {
            reportDAL = new ReportDAL();
        }

        public DataSet GetGeneralReportData(long activeMeterData_ID)
        {
            return reportDAL.GetGeneralReportData(activeMeterData_ID);
        }

        public DataSet GetBillingResetType(long activeMeterData_ID)
        {
            return reportDAL.GetBillingResetType(activeMeterData_ID);
        }

        public DataSet GetInstantReportData(long activeMeterData_ID)
        {
            return reportDAL.GetInstantReportData(activeMeterData_ID);
        }

        public DataSet GetTamperReportData(long activeMeterData_ID)
        {
            return reportDAL.GetTamperReportData(activeMeterData_ID);
        }
        public DataSet GetPowerOnHoursReportData(long activeMeterData_ID)
        {
            return reportDAL.GetPowerOnHoursReportData(activeMeterData_ID);
        }

        public DataSet GetPowerFactorReportData(long activeMeterData_ID)
        {
            return reportDAL.GetPowerFactorReportData(activeMeterData_ID);
        }

        public DataSet GetCTRatioReportData(long activeMeterData_ID)
        {
            return reportDAL.GetCTRatioReportData(activeMeterData_ID);
        }

        public DataSet GetLoadFactorReportData(long activeMeterData_ID)
        {
            return reportDAL.GetLoadFactorReportData(activeMeterData_ID);
        }

        public DataSet GetBillingTamperCounterReportData(long activeMeterData_ID)
        {
            return reportDAL.GetBillingTamperCounterReportData(activeMeterData_ID);
        }

        public DataSet GetMainEnergyReportData(long activeMeterData_ID)
        {
            return reportDAL.GetMainEnergyReportData(activeMeterData_ID);
        }
        
        // Added for detailed tamper report TNEB
        public DataSet GetTamperReportDataTNEB(long activeMeterData_ID)
        {
            return reportDAL.GetTamperReportDataTNEB(activeMeterData_ID);
        }
        public Dictionary<string, string> MergeDictionary(params Dictionary<string, string>[] dictionaries)
        {
            Dictionary<string, string> finalDictionary = new Dictionary<string, string>();
            foreach (Dictionary<string, string> dictionary in dictionaries)
            {
                foreach (KeyValuePair<string, string> kvp in dictionary)
                {
                    finalDictionary.Add(kvp.Key, kvp.Value);
                }
            }
            return finalDictionary;
        }
    }
}