#region Namespaces
using System;
using System.Data;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using CAB.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using Hunt.EPIC.Logging;
using CAB.Framework.Utility;

#endregion

namespace CAB.BLL
{
    /// <summary>
    /// Text File Export BLL
    /// </summary>
    public class TextExportBLL : IBLL
    {
        #region Constants & Variables
        private TextExportDAL textExportDAL;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TextExportBLL).ToString());
        #endregion

        #region Constructor
        public TextExportBLL()
        {
            textExportDAL = new TextExportDAL();
        }
        #endregion

        #region Public Methods       
        public DataSet GetDataForTextExport(long meterDataID)
        {
            return FormatDataForTextExport(textExportDAL.ListDataSetForTextExport(meterDataID));
        }

        /// <summary>
        /// Get WB Data For Text Export of  WBSEDCL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetWBDataForTextExport(long meterID)
        {
            return textExportDAL.GetWBDataForTextExport(meterID);

        }
        /// <summary>
        /// Get BSES Data For Text Export of  BRPL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetBSESDataForTextExport(long meterID)
        {
            return textExportDAL.GetBSESDataForTextExport(meterID);

        }
        public DataSet GetBSESDataForTextExport1(long meterID)
        {
            return textExportDAL.GetBSESDataForTextExport1(meterID);

        }

        public DataSet GetBSESDataForTextInstant(long meterID)
        {
            return textExportDAL.GetBSESDataForTextInstant(meterID);

        }

        /// <summary>
        /// Get RELIANCE Data For Text Export of  BRPL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceMumbaiDataForTextExport(string meterID, string meterVariant)
        {
            return textExportDAL.GetRelianceMumbaiDataForTextExport(meterID, meterVariant);

        }
        public DataSet GetRelianceMumbaiDataForTextExportwithsolar(string meterID, string meterVariant, int solartype, int historyNo)
        {
            return textExportDAL.GetRelianceMumbaiDataForTextExportwithsolar(meterID, meterVariant, solartype, historyNo);

        }





        /// <summary>
        /// Get Puducherry Data For Text Export of  BRPL 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetPuducherryDataForTextExport(long meterdataID, string meterid)
        {
            return textExportDAL.GetPuducherryDataForTextExport(meterdataID, meterid);

        }

        // SarkarA code change start 20171206//Reliance Mumbai Text Export
        /// <summary>
        /// Get Reliance LoadSurvey DataSet with MeterDataID as Parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceLoadSurveyDataForTextExport(string meterDataID)
        {
            return textExportDAL.GetRelianceLoadSurveyDataForTextExport(meterDataID);

        }

        /// <summary>
        /// Get Reliance Instant DataSet with MeterDataID as Parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceInstantData1ForTextExport(string meterDataID)
        {
            return textExportDAL.GetRelianceInstantData1ForTextExport(meterDataID);

        }

        /// <summary>
        /// Get Reliance Instant DataSet with MeterDataID as Parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceInstantData2ForTextExport(string meterDataID)
        {
            return textExportDAL.GetRelianceInstantData2ForTextExport(meterDataID);

        }

        /// <summary>
        /// Get Reliance Event DataSet with MeterDataID as Parameter 
        /// </summary>
        /// <param name="meterDataID"></param>
        /// <returns></returns>
        public DataSet GetRelianceEventDataForTextExport(string meterDataID)
        {
            return textExportDAL.GetRelianceEventDataForTextExport(meterDataID);

        }
        // SarkarA code change end 20171206

        public DataSet GetTorrentDataForTextExport(string meterDataID)
        {
            try
            {
                DataSet dataSet = textExportDAL.GetTorrentDataForTextExport(meterDataID);
                DataSet instantds = textExportDAL.GetTorrentDataInstantForTextExport(meterDataID);
                dataSet.Tables[0].Columns.Add("Cumulative MDKW", typeof(string));
                dataSet.Tables[0].Columns.Add("RESET", typeof(string));
                dataSet.Tables[0].Columns.Add("Total Reverse KWH", typeof(string));

                string strReset = "";
                string strMDkw = "";
                string strCumMDKvarLAG = "";

                foreach (DataRow drow in instantds.Tables[0].Rows)
                {
                    if (drow["InstantPowerObisCode"].ToString().Equals("0.0.0.1.0.255"))
                        strReset = drow["InstantPowerColumnValue"].ToString();
                    else if (drow["InstantPowerObisCode"].ToString().Equals("0.0.96.1.149.255"))
                        strMDkw = drow["InstantPowerColumnValue"].ToString();
                    else if (drow["InstantPowerObisCode"].ToString().Equals("1.0.5.8.0.255"))
                        strCumMDKvarLAG = drow["InstantPowerColumnValue"].ToString();
                }

                dataSet.Tables[0].Rows[0]["RESET"] = strReset;
                dataSet.Tables[0].Rows[0]["Cumulative MDKW"] = strMDkw;
                dataSet.Tables[0].Rows[0]["CumulativeEnergykvarhLag"] = strCumMDKvarLAG.Equals(String.Empty) ? strCumMDKvarLAG : dataSet.Tables[0].Rows[0]["CumulativeEnergykvarhLag"];
                dataSet.Tables[0].Rows[0]["Total Reverse KWH"] = "NA";

                foreach (DataRow drow in dataSet.Tables[0].Rows)
                {
                    foreach (DataColumn col in dataSet.Tables[0].Columns)
                    {
                        bool bNum = new string[] {"BillingDate"}.Any(s => col.ColumnName.Contains(s));
                        if (!bNum)
                        {
                            drow[col] = CommonBLL.RemoveUnitIfExist(drow[col].ToString());
                        }
                    }
                }
                dataSet.AcceptChanges();
                return dataSet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTataPowerAdaniDataForTextExport(string meterDataID)
        {
            try
            {
                DataSet dsMain = new DataSet();
                DataSet dsGeneral = textExportDAL.GetTataPowerAdaniGeneralData(meterDataID);
                DataSet dsInstant = textExportDAL.GetTataPowerAdaniInstantData(meterDataID);
                DataSet dsBilling = textExportDAL.GetTataPowerAdaniBillingData(meterDataID);
                DataSet dsBillingPowerOnDuration = new DLMS650BillingBLL().GetPowerOnDuration(int.Parse(meterDataID));
                DataSet dsTamperCount = new DLMS650TamperMasterBLL().GetTamperCountDetail(long.Parse(meterDataID));
                DataSet dsTamper = textExportDAL.GetTataPowerAdaniTamperData(meterDataID);
                DataSet dsLoadSurvey = textExportDAL.GetTataPowerAdaniLoadSurveyData(meterDataID);

                DataTable tableMain = new DataTable();

                tableMain.Columns.Add("MeterSerialNo", typeof(string));
                tableMain.Columns.Add("MeterMake", typeof(string));
                tableMain.Columns.Add("MRIDate", typeof(string));
                tableMain.Columns.Add("MRITime", typeof(string));
                tableMain.Columns.Add("MeterRTCDate", typeof(string));
                tableMain.Columns.Add("MeterRTCTime", typeof(string));
                tableMain.Columns.Add("MeterType", typeof(string));
                tableMain.Columns.Add("MeterTypeDescription", typeof(string));
                tableMain.Columns.Add("MeterPTRating", typeof(string));
                tableMain.Columns.Add("MeterCTRating", typeof(string));

                tableMain.Columns.Add("KWH-H0", typeof(string));
                tableMain.Columns.Add("KWH-H1", typeof(string));
                tableMain.Columns.Add("KWH-H2", typeof(string));
                tableMain.Columns.Add("KWH-H3", typeof(string));
                tableMain.Columns.Add("KWH-H4", typeof(string));
                tableMain.Columns.Add("KWH-H5", typeof(string));
                tableMain.Columns.Add("KWH-H6", typeof(string));
                tableMain.Columns.Add("KWH-H7", typeof(string));
                tableMain.Columns.Add("KWH-H8", typeof(string));
                tableMain.Columns.Add("KWH-H9", typeof(string));
                tableMain.Columns.Add("KWH-H10", typeof(string));
                tableMain.Columns.Add("KWH-H11", typeof(string));
                tableMain.Columns.Add("KWH-H12", typeof(string));

                tableMain.Columns.Add("KVARHLAG-H0", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H1", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H2", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H3", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H4", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H5", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H6", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H7", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H8", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H9", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H10", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H11", typeof(string));
                tableMain.Columns.Add("KVARHLAG-H12", typeof(string));

                tableMain.Columns.Add("KVARHLEAD-H0", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H1", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H2", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H3", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H4", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H5", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H6", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H7", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H8", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H9", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H10", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H11", typeof(string));
                tableMain.Columns.Add("KVARHLEAD-H12", typeof(string));

                tableMain.Columns.Add("KVAH-H0", typeof(string));
                tableMain.Columns.Add("KVAH-H1", typeof(string));
                tableMain.Columns.Add("KVAH-H2", typeof(string));
                tableMain.Columns.Add("KVAH-H3", typeof(string));
                tableMain.Columns.Add("KVAH-H4", typeof(string));
                tableMain.Columns.Add("KVAH-H5", typeof(string));
                tableMain.Columns.Add("KVAH-H6", typeof(string));
                tableMain.Columns.Add("KVAH-H7", typeof(string));
                tableMain.Columns.Add("KVAH-H8", typeof(string));
                tableMain.Columns.Add("KVAH-H9", typeof(string));
                tableMain.Columns.Add("KVAH-H10", typeof(string));
                tableMain.Columns.Add("KVAH-H11", typeof(string));
                tableMain.Columns.Add("KVAH-H12", typeof(string));

                tableMain.Columns.Add("KWHExport-H0", typeof(string));
                tableMain.Columns.Add("KWHExport-H1", typeof(string));
                tableMain.Columns.Add("KWHExport-H2", typeof(string));
                tableMain.Columns.Add("KWHExport-H3", typeof(string));
                tableMain.Columns.Add("KWHExport-H4", typeof(string));
                tableMain.Columns.Add("KWHExport-H5", typeof(string));
                tableMain.Columns.Add("KWHExport-H6", typeof(string));
                tableMain.Columns.Add("KWHExport-H7", typeof(string));
                tableMain.Columns.Add("KWHExport-H8", typeof(string));
                tableMain.Columns.Add("KWHExport-H9", typeof(string));
                tableMain.Columns.Add("KWHExport-H10", typeof(string));
                tableMain.Columns.Add("KWHExport-H11", typeof(string));
                tableMain.Columns.Add("KWHExport-H12", typeof(string));

                tableMain.Columns.Add("KWHDefraudMagNDSW", typeof(string));
                tableMain.Columns.Add("KWHDefraudMagnet", typeof(string));
                tableMain.Columns.Add("KWHDefraudNeutral", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H3", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H4", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H5", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H6", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H7", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H8", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H9", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H10", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H11", typeof(string));
                tableMain.Columns.Add("KWHDefraud-H12", typeof(string));

                tableMain.Columns.Add("KW-H0", typeof(string));
                tableMain.Columns.Add("KW-H1", typeof(string));
                tableMain.Columns.Add("KW-H2", typeof(string));
                tableMain.Columns.Add("KW-H3", typeof(string));
                tableMain.Columns.Add("KW-H4", typeof(string));
                tableMain.Columns.Add("KW-H5", typeof(string));
                tableMain.Columns.Add("KW-H6", typeof(string));
                tableMain.Columns.Add("KW-H7", typeof(string));
                tableMain.Columns.Add("KW-H8", typeof(string));
                tableMain.Columns.Add("KW-H9", typeof(string));
                tableMain.Columns.Add("KW-H10", typeof(string));
                tableMain.Columns.Add("KW-H11", typeof(string));
                tableMain.Columns.Add("KW-H12", typeof(string));

                tableMain.Columns.Add("KVA-H0", typeof(string));
                tableMain.Columns.Add("KVA-H1", typeof(string));
                tableMain.Columns.Add("KVA-H2", typeof(string));
                tableMain.Columns.Add("KVA-H3", typeof(string));
                tableMain.Columns.Add("KVA-H4", typeof(string));
                tableMain.Columns.Add("KVA-H5", typeof(string));
                tableMain.Columns.Add("KVA-H6", typeof(string));
                tableMain.Columns.Add("KVA-H7", typeof(string));
                tableMain.Columns.Add("KVA-H8", typeof(string));
                tableMain.Columns.Add("KVA-H9", typeof(string));
                tableMain.Columns.Add("KVA-H10", typeof(string));
                tableMain.Columns.Add("KVA-H11", typeof(string));
                tableMain.Columns.Add("KVA-H12", typeof(string));

                tableMain.Columns.Add("BillPF-H0", typeof(string));
                tableMain.Columns.Add("BillPF-H1", typeof(string));
                tableMain.Columns.Add("BillPF-H2", typeof(string));
                tableMain.Columns.Add("BillPF-H3", typeof(string));
                tableMain.Columns.Add("BillPF-H4", typeof(string));
                tableMain.Columns.Add("BillPF-H5", typeof(string));
                tableMain.Columns.Add("BillPF-H6", typeof(string));
                tableMain.Columns.Add("BillPF-H7", typeof(string));
                tableMain.Columns.Add("BillPF-H8", typeof(string));
                tableMain.Columns.Add("BillPF-H9", typeof(string));
                tableMain.Columns.Add("BillPF-H10", typeof(string));
                tableMain.Columns.Add("BillPF-H11", typeof(string));
                tableMain.Columns.Add("BillPF-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-A-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H4", typeof(string));

                tableMain.Columns.Add("KWH-TOD-B-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H4", typeof(string));

                tableMain.Columns.Add("KWH-TOD-C-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H4", typeof(string));

                tableMain.Columns.Add("KWH-TOD-D-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H4", typeof(string));

                tableMain.Columns.Add("KWH-TOD-E-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H4", typeof(string));

                tableMain.Columns.Add("KWH-TOD-F-H0", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H1", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H2", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H3", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H4", typeof(string));

                tableMain.Columns.Add("BillingDate-H0", typeof(string));
                tableMain.Columns.Add("BillingDate-H1", typeof(string));
                tableMain.Columns.Add("BillingDate-H2", typeof(string));
                tableMain.Columns.Add("BillingDate-H3", typeof(string));
                tableMain.Columns.Add("BillingDate-H4", typeof(string));
                tableMain.Columns.Add("BillingDate-H5", typeof(string));
                tableMain.Columns.Add("BillingDate-H6", typeof(string));
                tableMain.Columns.Add("BillingDate-H7", typeof(string));
                tableMain.Columns.Add("BillingDate-H8", typeof(string));
                tableMain.Columns.Add("BillingDate-H9", typeof(string));
                tableMain.Columns.Add("BillingDate-H10", typeof(string));
                tableMain.Columns.Add("BillingDate-H11", typeof(string));
                tableMain.Columns.Add("BillingDate-H12", typeof(string));

                tableMain.Columns.Add("Count-MagnetEvent", typeof(string));
                tableMain.Columns.Add("Count-NDEvent", typeof(string));
                tableMain.Columns.Add("Count-RevCurrEvent", typeof(string));
                tableMain.Columns.Add("Count-EarthLoadEvent", typeof(string));
                tableMain.Columns.Add("Count-SingleWireEvent", typeof(string));
                tableMain.Columns.Add("Count-ESDEvent", typeof(string));
                tableMain.Columns.Add("Count-PowerFailEvent", typeof(string));

                tableMain.Columns.Add("InstantVoltageR", typeof(string));
                tableMain.Columns.Add("InstantVoltageY", typeof(string));
                tableMain.Columns.Add("InstantVoltageB", typeof(string));
                tableMain.Columns.Add("InstantCurrentR", typeof(string));
                tableMain.Columns.Add("InstantCurrentY", typeof(string));
                tableMain.Columns.Add("InstantCurrentB", typeof(string));
                tableMain.Columns.Add("InstantCurrentN", typeof(string));
                tableMain.Columns.Add("InstantActiveCurrentR", typeof(string));
                tableMain.Columns.Add("InstantActiveCurrentY", typeof(string));
                tableMain.Columns.Add("InstantActiveCurrentB", typeof(string));
                tableMain.Columns.Add("InstantPFR", typeof(string));
                tableMain.Columns.Add("InstantPFY", typeof(string));
                tableMain.Columns.Add("InstantPFB", typeof(string));
                tableMain.Columns.Add("InstantPFAvg", typeof(string));
                tableMain.Columns.Add("InstantActiveTotalKW", typeof(string));
                tableMain.Columns.Add("InstantReactiveTotalKW", typeof(string));
                tableMain.Columns.Add("InstantApparentTotalKW", typeof(string));

                tableMain.Columns.Add("Status-Register", typeof(string));
                tableMain.Columns.Add("Status-Memory", typeof(string));
                tableMain.Columns.Add("Status-RTC", typeof(string));
                tableMain.Columns.Add("Status-BatteryMain", typeof(string));
                tableMain.Columns.Add("Status-BatteryRTC", typeof(string));

                tableMain.Columns.Add("Status-CoverOpen", typeof(string));
                tableMain.Columns.Add("Status-OverVoltage", typeof(string));
                tableMain.Columns.Add("Status-ND", typeof(string));
                tableMain.Columns.Add("Status-SingleWire", typeof(string));
                tableMain.Columns.Add("Status-Magnetic", typeof(string));
                tableMain.Columns.Add("Status-CurrentRev", typeof(string));
                tableMain.Columns.Add("Status-OverCurrent", typeof(string));
                tableMain.Columns.Add("Status-CurrentWithoutVoltage", typeof(string));
                tableMain.Columns.Add("Status-InvalidVoltage", typeof(string));
                tableMain.Columns.Add("Status-VoltageImbalance", typeof(string));
                tableMain.Columns.Add("Status-LowPF", typeof(string));

                tableMain.Columns.Add("PowerOnTime-H0", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H0", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H1", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H1", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H2", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H2", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H3", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H3", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H4", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H4", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H5", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H5", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H6", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H6", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H7", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H7", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H8", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H8", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H9", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H9", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H10", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H10", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H11", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H11", typeof(string));
                tableMain.Columns.Add("PowerOnTime-H12", typeof(string));
                tableMain.Columns.Add("PowerOffTime-H12", typeof(string));

                tableMain.Columns.Add("Count-LSDays", typeof(string));
                tableMain.Columns.Add("InitialLSDate", typeof(string));
                tableMain.Columns.Add("FinalLSDate", typeof(string));

                tableMain.Columns.Add("RevCurrentR", typeof(string));
                tableMain.Columns.Add("RevCurrentY", typeof(string));
                tableMain.Columns.Add("RevCurrentB", typeof(string));
                tableMain.Columns.Add("FailCurrentR", typeof(string));
                tableMain.Columns.Add("FailCurrentY", typeof(string));
                tableMain.Columns.Add("FailCurrentB", typeof(string));

                tableMain.Columns.Add("ANAMOLY", typeof(string));

                tableMain.Columns.Add("InstantPowerR", typeof(string));
                tableMain.Columns.Add("InstantPowerY", typeof(string));
                tableMain.Columns.Add("InstantPowerB", typeof(string));
                tableMain.Columns.Add("InstantKvarR", typeof(string));
                tableMain.Columns.Add("InstantKvarY", typeof(string));
                tableMain.Columns.Add("InstantKvarB", typeof(string));
                tableMain.Columns.Add("PhaseSeqCurrent", typeof(string));
                tableMain.Columns.Add("PhaseSeqVoltage", typeof(string));

                tableMain.Columns.Add("MDKWDateTime-H0", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H1", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H2", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H3", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H4", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H5", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H6", typeof(string));

                tableMain.Columns.Add("MDKVADateTime-H0", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H1", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H2", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H3", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H4", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H5", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H6", typeof(string));

                tableMain.Columns.Add("MDKWDateTime-H7", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H8", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H9", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H10", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H11", typeof(string));
                tableMain.Columns.Add("MDKWDateTime-H12", typeof(string));

                tableMain.Columns.Add("MDKVADateTime-H7", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H8", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H9", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H10", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H11", typeof(string));
                tableMain.Columns.Add("MDKVADateTime-H12", typeof(string));

                tableMain.Columns.Add("LowPFDurationR", typeof(string));
                tableMain.Columns.Add("LowPFDurationY", typeof(string));
                tableMain.Columns.Add("LowPFDurationB", typeof(string));

                tableMain.Columns.Add("KWH-TOD-A-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-A-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-B-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-B-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-C-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-C-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-D-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-D-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-E-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-E-H12", typeof(string));

                tableMain.Columns.Add("KWH-TOD-F-H5", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H6", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H7", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H8", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H9", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H10", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H11", typeof(string));
                tableMain.Columns.Add("KWH-TOD-F-H12", typeof(string));

                foreach (DataColumn col in tableMain.Columns)
                {
                    col.DefaultValue = string.Empty;
                }

                DataRow dataRow = tableMain.NewRow();

                DataTable TableTamper = dsTamper.Tables[0];



                if (dsGeneral.Tables[0].Rows.Count > 0)
                {
                    DataRow rowGeneral = dsGeneral.Tables[0].Rows[0];

                    dataRow["MeterSerialNo"] = rowGeneral["MeterID"].ToString();
                    dataRow["Metermake"] = rowGeneral["manufacturername"].ToString();
                    DateTime readDateTime = DateUtility.LongToDateTime(long.Parse(rowGeneral["ReadingDateTime"].ToString()));
                    dataRow["MRIDate"] = readDateTime.ToString("dd/MM/yyyy");
                    dataRow["MRITime"] = readDateTime.ToString("hh:mm:ss");

                    dataRow["MeterType"] = rowGeneral["metertype"].ToString();
                    dataRow["MeterTypeDescription"] = "";
                    dataRow["MeterPTRating"] = rowGeneral["internalPTratio"].ToString().Contains('-') ? string.Empty : rowGeneral["internalPTratio"].ToString();
                    dataRow["MeterCTRating"] = rowGeneral["internalCTratio"].ToString().Contains('-') ? string.Empty : rowGeneral["internalCTratio"].ToString();

                    dataRow["Status-Register"] = Boolean.Parse(rowGeneral["FlashStatus"].ToString())==true ?  "NO" : "YES";
                    dataRow["Status-Memory"] = Boolean.Parse(rowGeneral["EepRamStatus"].ToString()) == true ? "NO" : "YES";
                    dataRow["Status-RTC"] = Boolean.Parse(rowGeneral["RtcStatus"].ToString()) == true ? "NO" : "YES";
                    dataRow["Status-BatteryMain"] = Boolean.Parse(rowGeneral["MainBatteryStatus"].ToString()) == true ? "NO" : "YES";
                    dataRow["Status-BatteryRTC"] = Boolean.Parse(rowGeneral["RTCBatteryStatus"].ToString()) == true ? "NO" : "YES";
                }

                if (dsBilling.Tables[0].Rows.Count > 0)
                {
                    DataTable tableBilling = dsBilling.Tables[0];

                    for (int idx = 0; idx < 13; idx++)
                    {
                        if (idx <= tableBilling.Rows.Count - 1)
                        {
                            dataRow["KWH-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ0"].ToString());
                            dataRow["KVARHLAG-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykvarhLag"].ToString());
                            dataRow["KVARHLEAD-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykvarhLead"].ToString());
                            dataRow["KVAH-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykVAhTZ0"].ToString());
                            dataRow["KWHExport-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ0Export"].ToString());
                            dataRow["BillPF-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["SystemPowerFactorforBillingPeriod"].ToString());

                            dataRow["KWH-TOD-A-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ1"].ToString());
                            dataRow["KWH-TOD-B-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ2"].ToString());
                            dataRow["KWH-TOD-C-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ3"].ToString());
                            dataRow["KWH-TOD-D-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ4"].ToString());
                            dataRow["KWH-TOD-E-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ5"].ToString());
                            dataRow["KWH-TOD-F-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["CumulativeEnergykWhTZ6"].ToString());
                            dataRow["BillingDate-H" + idx] = DateUtility.LongToDateTime(long.Parse(tableBilling.Rows[idx]["BillingDate"].ToString())).ToString("dd/MM/yyyy");
                            dataRow["PowerOnTime-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["PowerOnDuration"].ToString());
                            dataRow["PowerOffTime-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["BillingWisePowerOffDuration"].ToString());

                            //dataRow["PowerOnTime-H" + idx] = CommonBLL.RemoveUnitIfExist(tableBilling.Rows[idx]["PowerOnDuration"].ToString());
                            dataRow["PowerOffTime-H" + idx] = CommonBLL.ConvertReadableStringToTimeSpan(dsBillingPowerOnDuration.Tables[0].Rows[idx][3].ToString()).TotalMinutes.ToString();


                            if (!tableBilling.Rows[idx]["MDkWDateTimeTZ0"].ToString().Equals("0"))
                            {
                                dataRow["MDKWDateTime-H" + idx] = DateUtility.LongToDateTime(long.Parse(tableBilling.Rows[idx]["MDkWDateTimeTZ0"].ToString())).ToString("dd/MM/yyyy");
                            }
                            if (!tableBilling.Rows[idx]["MDkVADateTimeTZ0"].ToString().Equals("0"))
                            {
                                dataRow["MDKVADateTime-H" + idx] = DateUtility.LongToDateTime(long.Parse(tableBilling.Rows[idx]["MDkVADateTimeTZ0"].ToString())).ToString("dd/MM/yyyy");
                            }
                        }
                    }
                }

                if (dsInstant.Tables[0].Rows.Count > 0)
                {
                    DataTable tableInstant = dsInstant.Tables[0];

                    int reverseCTCounter = 0;

                    double VR = -1, VY = -1, VB = -1, IR = 0, IY = 0, IB = 0, PFR = 0, PFY = 0, PFB = 0;

                    foreach (DataRow row in tableInstant.Rows)
                    {
                        string key = string.Empty;

                        switch (row["InstantPowerObisCode"].ToString())
                        {
                            case "0.0.96.1.221.255":
                                key = "KWHDefraudMagNDSW";
                                break;
                            case "0.0.96.1.218.255":
                                key = "KWHDefraudMagnet";
                                break;
                            case "1.0.157.128.128.255":
                                key = "KWHDefraudNeutral";
                                break;
                            case "0.0.96.2.177.255":
                                key = "Count-MagnetEvent";
                                break;
                            case "0.0.96.2.178.255":
                                key = "Count-NDEvent";
                                break;
                            case "0.0.96.7.0.255":
                                key = "Count-PowerFailEvent";
                                break;
                            case "0.0.96.2.168.255":
                            case "0.0.96.2.169.255":
                            case "0.0.96.2.170.255":
                                reverseCTCounter += int.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;

                            case "1.0.32.7.0.255":
                            case "1.0.12.7.0.255":
                                key = "InstantVoltageR";
                                VR = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.52.7.0.255":
                                key = "InstantVoltageY";
                                VY = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.72.7.0.255":
                                key = "InstantVoltageB";
                                VB = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.31.7.0.255":
                            case "1.0.11.7.0.255":
                                key = "InstantCurrentR";
                                IR = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.51.7.0.255":
                                key = "InstantCurrentY";
                                IY = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.71.7.0.255":
                                key = "InstantCurrentB";
                                IB = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.91.7.0.255":
                                key = "InstantCurrentN";
                                break;
                            case "1.0.33.7.0.255":
                                key = "InstantPFR";
                                PFR = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.53.7.0.255":
                                key = "InstantPFY";
                                PFY = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.73.7.0.255":
                                key = "InstantPFB";
                                PFB = double.Parse(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()));
                                break;
                            case "1.0.13.7.0.255":
                                key = "InstantPFAvg";
                                break;
                            case "1.0.1.7.0.255":
                                key = "InstantActiveTotalKW";
                                break;
                            case "1.0.9.7.0.255":
                                key = "InstantApparentTotalKW";
                                break;
                            case "1.0.3.7.0.255":
                                key = "InstantReactiveTotalKW";
                                break;
                            case "0.0.1.0.0.255":
                                DateTime rtc = DateTime.MinValue;       
                                if(!DateTime.TryParseExact(CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString()), "dd/MM/yyyy HH:mm:ss",System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out rtc))
                                {
                                    rtc = DateUtility.LongToDateTime(long.Parse(row["InstantPowerColumnValue"].ToString()));
                                }
                                dataRow["MeterRTCDate"] = rtc.ToString("dd/MM/yyyy");
                                dataRow["MeterRTCTime"] = rtc.ToString("hh:mm:ss");
                                break;
                            default:
                                break;
                        }
                        if (!string.IsNullOrEmpty(key))
                        {
                            dataRow[key] = CommonBLL.RemoveUnitIfExist(row["InstantPowerColumnValue"].ToString());
                        }
                    }


                    dataRow["Count-RevCurrEvent"] = reverseCTCounter.ToString();

                    if (VR != -1 && VY != -1 && VB != -1)
                    {
                        dataRow["InstantPowerR"] = ((VR * IR * PFR)/1000).ToString("0.00000");
                        dataRow["InstantPowerY"] = ((VY * IY * PFY) / 1000).ToString("0.00000");
                        dataRow["InstantPowerB"] = ((VB * IB * PFB) / 1000).ToString("0.00000");
                        dataRow["InstantKvarR"] = ((VR * IR * Math.Sin(Math.Acos(PFR))) / 1000).ToString("0.00000");
                        dataRow["InstantKvarY"] = ((VY * IY * Math.Sin(Math.Acos(PFY))) / 1000).ToString("0.00000");
                        dataRow["InstantKvarB"] = ((VB * IB * Math.Sin(Math.Acos(PFB))) / 1000).ToString("0.00000");
                    }
                }

                if (dsTamper.Tables[0].Rows.Count > 0)
                {
                    int revCount = 0;

                    foreach (DataRow row in dsTamper.Tables[0].Rows)
                    {
                        string key = "";
                        switch (row["EventCode"].ToString())
                        {
                            case "51":
                            case "53":
                            case "55":
                                revCount += int.Parse(row["count(EventCode)"].ToString());
                                dataRow["Count-RevCurrEvent"] = revCount.ToString();
                                break;
                            case "203":
                                key = "Count-NDEvent";
                                break;
                            case "201":
                                key = "Count-MagnetEvent";
                                break;
                            case "101":
                                key = "Count-PowerFailEvent";
                                break;
                            case "69":
                                key = "Count-EarthLoadEvent";
                                break;
                            case "207":
                                key = "Count-SingleWireEvent";
                                break;
                            case "801":
                                key = "Count-ESDEvent";
                                break;
                            default:
                                break;
                        }
                        if (!string.IsNullOrEmpty(key))
                        {
                            dataRow[key] = row["count(EventCode)"].ToString();
                        }
                    }

                }

                if (dsLoadSurvey.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsLoadSurvey.Tables[0].Rows[0][0].ToString()) && !string.IsNullOrEmpty(dsLoadSurvey.Tables[0].Rows[0][1].ToString()))
                {
                    DateTime initialDate = DateUtility.LongToDateTime(long.Parse(dsLoadSurvey.Tables[0].Rows[0]["InitialLoadSurveyDate"].ToString()));
                    DateTime finalDate = DateUtility.LongToDateTime(long.Parse(dsLoadSurvey.Tables[0].Rows[0]["FinalLoadSurveyDate"].ToString()));

                    dataRow["Count-LSDays"] = (finalDate.Date - initialDate.Date).TotalDays.ToString();
                    dataRow["InitialLSDate"] = initialDate.ToString("dd/MM/yyyy");
                    dataRow["FinalLSDate"] = finalDate.ToString("dd/MM/yyyy");
                }

                dataRow["KWHDefraud-H3"] = string.Empty;
                dataRow["KWHDefraud-H4"] = string.Empty;
                dataRow["KWHDefraud-H5"] = string.Empty;
                dataRow["KWHDefraud-H6"] = string.Empty;
                dataRow["KWHDefraud-H7"] = string.Empty;
                dataRow["KWHDefraud-H8"] = string.Empty;
                dataRow["KWHDefraud-H9"] = string.Empty;
                dataRow["KWHDefraud-H10"] = string.Empty;
                dataRow["KWHDefraud-H11"] = string.Empty;
                dataRow["KWHDefraud-H12"] = string.Empty;

                dataRow["KW-H0"] = string.Empty;
                dataRow["KW-H1"] = string.Empty;
                dataRow["KW-H2"] = string.Empty;
                dataRow["KW-H3"] = string.Empty;
                dataRow["KW-H4"] = string.Empty;
                dataRow["KW-H5"] = string.Empty;
                dataRow["KW-H6"] = string.Empty;
                dataRow["KW-H7"] = string.Empty;
                dataRow["KW-H8"] = string.Empty;
                dataRow["KW-H9"] = string.Empty;
                dataRow["KW-H10"] = string.Empty;
                dataRow["KW-H11"] = string.Empty;
                dataRow["KW-H12"] = string.Empty;

                dataRow["KVA-H0"] = string.Empty;
                dataRow["KVA-H1"] = string.Empty;
                dataRow["KVA-H2"] = string.Empty;
                dataRow["KVA-H3"] = string.Empty;
                dataRow["KVA-H4"] = string.Empty;
                dataRow["KVA-H5"] = string.Empty;
                dataRow["KVA-H6"] = string.Empty;
                dataRow["KVA-H7"] = string.Empty;
                dataRow["KVA-H8"] = string.Empty;
                dataRow["KVA-H9"] = string.Empty;
                dataRow["KVA-H10"] = string.Empty;
                dataRow["KVA-H11"] = string.Empty;
                dataRow["KVA-H12"] = string.Empty;


                dataRow["InstantActiveCurrentR"] = "";
                dataRow["InstantActiveCurrentY"] = "";
                dataRow["InstantActiveCurrentB"] = "";



                dataRow["Status-CoverOpen"] = "";
                dataRow["Status-OverVoltage"] = "";
                dataRow["Status-ND"] = "";
                dataRow["Status-SingleWire"] = "";
                dataRow["Status-Magnetic"] = "";
                dataRow["Status-CurrentRev"] = "";
                dataRow["Status-OverCurrent"] = "";
                dataRow["Status-CurrentWithoutVoltage"] = "";
                dataRow["Status-InvalidVoltage"] = "";
                dataRow["Status-VoltageImbalance"] = "";
                dataRow["Status-LowPF"] = "";

                dataRow["RevCurrentR"] = "";
                dataRow["RevCurrentY"] = "";
                dataRow["RevCurrentB"] = "";
                dataRow["FailCurrentR"] = "";
                dataRow["FailCurrentY"] = "";
                dataRow["FailCurrentB"] = "";

                dataRow["ANAMOLY"] = "";

                dataRow["PhaseSeqCurrent"] = "";
                dataRow["PhaseSeqVoltage"] = "";

                dataRow["LowPFDurationR"] = "";
                dataRow["LowPFDurationY"] = "";
                dataRow["LowPFDurationB"] = "";

                tableMain.Rows.Add(dataRow);
                dsMain.Tables.Add(tableMain);
                dsMain.AcceptChanges();
                return dsMain;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetCSPDCLDataForTextExport(string meterDataID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = textExportDAL.GetCSPDCLDataForTextExport(meterDataID);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetTorrentAgraDataForTextExport(string meterDataID)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = textExportDAL.GetTorrentAgraDataForTextExport(meterDataID);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["MeterID"] = new string(row["MeterID"].ToString().Where(c=>char.IsDigit(c)).ToArray());     //only digits
                    row["Consumer_Number"] = string.Concat(row["Consumer_Number"].ToString().Split());              //remove all whitespace
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        bool bNum = new string[] { "Consumer_Number", "MeterID", "BillingDate", "DateTime", "Meter_Location", "Consumer_HNumber" }.Any(s => col.ColumnName.Contains(s)); 
                        if (!bNum)
                        {
                            var tempstring = CommonBLL.RemoveUnitIfExist(row[col].ToString());
                            row[col] = CommonBLL.ConvertFromKiloToUnit(tempstring);
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet FormatDataForTextExport(DataSet inputData)
        {
            if (inputData != null && inputData.Tables != null && inputData.Tables.Count > 0)
            {
                DataSet formattedData = new DataSet();
            }
            return inputData;
        }

        public DataSet GetMeterDataIDByFileName(string fileName)
        {
            return textExportDAL.GetMeterDataIDByFileName(fileName);
        }

        public List<string> GetMeterDataIDListByFileName(string fileName)
        {
            List<string> lstMeterDataID = new List<string>();
            try
            {
                DataSet ds =  GetMeterDataIDByFileName(fileName);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        lstMeterDataID.Add(Convert.ToString(item["MeterData_ID"]));
                    }
                }
            }
            catch (System.Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterDataIDListByFileName(string fileName)", ex);
                
            }

            return lstMeterDataID;            
        }
        #endregion



       
    }
}
