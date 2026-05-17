using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using CAB.Entity;
using CAB.BLL;
using CABApplication.Reports.RPTFilesNew;
using CAB.IECFramework.Utility;
using CAB.IECFramework.Entity;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;
using LTCTBLL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CAB.UI
{
    public partial class SelectDialog : CABForm
    {
        private string MeterId = string.Empty;
        private string CABFileName = string.Empty;
        private ReportDataSet reportXSD = new ReportDataSet();
        private string[] MDHeadings = { "Cumulative MD1 (kW)", "Cumulative MD2 (kVA)", "MD3" };
        const string dateUnavailable = "--------";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        static List<string> lsHeadings;
        public  CheckBox[] chkBox;
        private const string NOTAPPLICABLE="NA";
        private int seasonNumber = 1;
        //Dictionary<string, string> tamperCounterDictionary = new Dictionary<string, string>();

        public SelectDialog()
        {            

            InitializeComponent();

        }

        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }

        private DataSet ListGeneralData(long activeMeterDataId)
        {
            return new ReportBLL().GetGeneralReportData(activeMeterDataId);
        }

        private DataSet ListInstantData(long activeMeterDataId)
        {
            return new ReportBLL().GetInstantReportData(activeMeterDataId);
        }

        private DataSet ListPhasorData(long activeMeterDataId)
        {
            return new PhasorBLL().ListDataSet(activeMeterDataId);
        }

        private DataSet ListTamperData(long activeMeterDataId)
        {
            return new ReportBLL().GetTamperReportData(activeMeterDataId);
        }

        private DataSet ListPowerOnHoursData(long activeMeterDataId)
        {
            return new ReportBLL().GetPowerOnHoursReportData(activeMeterDataId);
        }

        private DataSet ListPowerFactorData(long activeMeterDataId)
        {
            return new ReportBLL().GetPowerFactorReportData(activeMeterDataId);
        }

        private DataSet GetTariffPF(long activeMeterDataId)
        {
            return new TariffBLL().GetTariffPF(activeMeterDataId);
        }

        private DataSet ListCTRatioData(long activeMeterDataId)
        {
            return new ReportBLL().GetCTRatioReportData(activeMeterDataId);
        }

        private DataSet ListLoadFactorData(long activeMeterDataId)
        {
            return new ReportBLL().GetLoadFactorReportData(activeMeterDataId);
        }

        private DataSet ListBillingTamperCounterData(long activeMeterDataId)
        {
            return new ReportBLL().GetBillingTamperCounterReportData(activeMeterDataId);
        }

        private DataSet ListTamperOccResData(long activeMeterDataId)
        {
            return new BillingBLL().TamperOccurRestore(activeMeterDataId);
        }

        public DataSet GetTamperOccurRestoreDetail(int tamperSnapShotID, long activeMeterDataId)
        {
            return CommonBLL.GetTamperOccurRestoreDetail(tamperSnapShotID, activeMeterDataId);
        }

        private DataSet ListMainEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }

        private DataSet ListTariffEnergyData(long activeMeterDataId, int historyID)
        {
            return new TariffBLL().GetMeterData(activeMeterDataId, historyID);
        }

        private DataSet ListTODMDData(long activeMeterDataId, int historyID)
        {
            return new TariffBLL().GetTODMDMeterData(Convert.ToInt16(activeMeterDataId), historyID);
        }

        private DataSet ListConsumptionEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }

        private DataSet ListLoadSurveyData(long activeMeterDataId,string types)
        {
            DataSet dataSet = new DataSet();
            LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
            long sDate=loadSurveyBLL.GetFromDate(activeMeterDataId);
            long eDate= loadSurveyBLL.GetToDate(activeMeterDataId);
            dataSet = loadSurveyBLL.ListDataSet(activeMeterDataId,sDate,eDate,types);
            return dataSet;
        }

        private DataSet ListDTMLoadSurveyData(long activeMeterDataId)
        {
            DTMLoadSurveyBLL dtmLoadSurveyBLL = new DTMLoadSurveyBLL();
            return dtmLoadSurveyBLL.ListDataSet(activeMeterDataId);
        }

        private DataSet ListTransactionData(long activeMeterDataId)
        {
            ProgrammingBLL programmingBLL = new ProgrammingBLL();
            return programmingBLL.GetProgrammingList(activeMeterDataId);
        }

        private DataSet ListRTCUpdatesData(long activeMeterDataId)
        {
            return new RTCUpdateBLL().GetRTCUpdateList(activeMeterDataId);
        }

        private IEntity ListFraudEnergyData(long activeMeterDataId)
        {
            return new FraudEnergyBLL().GetFraudEnergy(activeMeterDataId);
        }

        private void FillMeterID(DataSet meterIdDS)
        {
            DataRow reportRow;
            if (meterIdDS != null && meterIdDS.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in meterIdDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = dateUnavailable;
                }
                reportRow["ActiveMeter"] = "Inactive";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }

        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            DataRow reportRow;

            if (detailsDS.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in detailsDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                        reportRow["ConsumerNo"] = CommonBLL.GetFormattedData(row["Consumer_Number"].ToString());
                    else
                        reportRow["ConsumerNo"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_Location"].ToString()))
                        reportRow["Location"] = CommonBLL.GetFormattedData(row["Meter_Location"].ToString());
                    else
                        reportRow["Location"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_AllocationDate"].ToString()))
                        reportRow["InstallationDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
                    else
                        reportRow["InstallationDate"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterType_Name"].ToString()))
                        reportRow["MeterType"] = CommonBLL.GetFormattedData(row["MeterType_Name"].ToString());
                    else
                        reportRow["MeterType"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterModel_Name"].ToString()))
                        reportRow["MeterModel"] = CommonBLL.GetFormattedData(row["MeterModel_Name"].ToString());
                    else
                        reportRow["MeterModel"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                        reportRow["EMF"] = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                    else
                        reportRow["EMF"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Region_Name"].ToString()))
                        reportRow["Region"] = CommonBLL.GetFormattedData(row["Region_Name"].ToString());
                    else
                        reportRow["Region"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Circle_Name"].ToString()))
                        reportRow["Circle"] = CommonBLL.GetFormattedData(row["Circle_Name"].ToString());
                    else
                        reportRow["Circle"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Division_Name"].ToString()))
                        reportRow["Division"] = CommonBLL.GetFormattedData(row["Division_Name"].ToString());
                    else
                        reportRow["Division"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["CMRI_Number"].ToString()))
                        reportRow["CMRINumber"] = CommonBLL.GetFormattedData(row["CMRI_Number"].ToString());
                    else
                        reportRow["CMRINumber"] = dateUnavailable;

                    if (row["Status"].ToString().Equals("0"))
                        reportRow["ActiveMeter"] = "Inactive";
                    else
                        reportRow["ActiveMeter"] = "Active";

                    if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                        reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                    else
                        reportRow["ContractDemand"] = dateUnavailable;

                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillGeneralXSD(DataSet generalData)
        {
            DataRow reportRow;

            if (generalData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingGeneralTable"].NewRow();
                foreach (DataRow row in generalData.Tables[0].Rows)
                {
                    reportRow["MeterID"] = row["MeterID"].ToString();
                    reportRow["MeterDateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(row["MeterDateTime"].ToString())).ToString(dateFormat);
                    reportRow["ErrorCode"] = CommonBLL.GetFormattedData(row["ErrorCode"].ToString());
                    reportRow["MeterConstant"] = string.Concat(CommonBLL.GetFormattedData(row["MeterConstant"].ToString()), " imp/kWh,imp/kvarh");
                    reportRow["FirmwareVersion"] = CommonBLL.GetFormattedData(row["FirmwareVersion"].ToString());
                    reportRow["CTRatio"] = CommonBLL.GetFormattedData(row["CTRatio"].ToString());
                    reportRow["VoltagePhaseSequence"] = CommonBLL.GetFormattedData(row["VoltagePhaseSequence"].ToString());
                    reportRow["TotalActiveEnergy"] = CommonBLL.GetFormattedData(row["TotalFundamentalActiveEnergy"].ToString());
                    reportRow["CumulativeMD1"] = CommonBLL.GetFormattedData(row["CMD1"].ToString());
                    reportRow["CumulativeMD2"] = CommonBLL.GetFormattedData(row["CMD2"].ToString());
                    reportRow["CumulativeMD3"] = CommonBLL.GetFormattedData(row["CMD3"].ToString());
                    /*GKG : 03/01/2013 137641 */
                    //reportRow["TotalPowerOnHours"] = CommonBLL.FormatPowerOnHours(row["TotalPowerOnHours"].ToString(), true) + " " + "HH:MM";
                    if (UtilityDetails.ShowPowerOnHoursInMinutes)
                    {
                        reportRow["TotalPowerOnHours"] = CommonBLL.FormatPowerOnHours(row["TotalPowerOnHours"].ToString(),true) +" " +"HH:MM";
                    }
                    else
                    {
                        reportRow["TotalPowerOnHours"] = CommonBLL.GetFormattedData(row["TotalPowerOnHours"].ToString());
                    }
                    /*GKG : 03/01/2013 137641 */
                    reportRow["BatteryPowerOnHours"] = CommonBLL.GetFormattedData(row["BateryModePowerOnHour"].ToString());
                    reportRow["MD Reset Count"] = CommonBLL.GetFormattedData(row["MDResetCounter"].ToString());
                    reportRow["ReadOut Counters"] = CommonBLL.GetFormattedData(row["ReadoutCounter"].ToString());
                    reportRow["Programming Counters"] = CommonBLL.GetFormattedData(row["ProgrammingCounter"].ToString());
                    reportRow["CT Ratio Programming Counters"] = CommonBLL.GetFormattedData(row["CTRatioProgrammingCounter"].ToString());
                    string val = Convert.ToString(row["LatestTamperOccurrenceID"]);
                    if (val.ToUpper().IndexOf("PF") > 0)
                        val = val.Substring(0, val.ToUpper().IndexOf("PF") - 1);

                    if (val.ToUpper().IndexOf("LOW") > 0)
                        val = val + " PF";
                    reportRow["Latest Tamper Occurence"] = val;

                    if (row["OccurrenceTime"].ToString() != "0")
                        reportRow["Occurrence Time"] = DateUtility.LongToDateTime(Convert.ToInt64(row["OccurrenceTime"].ToString())).ToString(dateFormat);
                    else
                        reportRow["Occurrence Time"] = dateUnavailable;

                    //reportRow["Occurrence Time"] = CommonBLL.GetFormattedData(row["OccurrenceTime"].ToString());
                    val = CommonBLL.GetFormattedData(row["LatestTamperRestorationID"].ToString());
                    if (val.ToUpper().IndexOf("PF") > 0)
                        val = val.Substring(0, val.ToUpper().IndexOf("PF") - 1);
                    if (val.ToUpper().IndexOf("PHASE LOW") > 0)
                        val = val + " PF";
                    reportRow["Latest Tamper Restored"] = val;


                    if (row["RestorationTime"].ToString() != "0")
                        reportRow["Restoration Time"] = DateUtility.LongToDateTime(Convert.ToInt64(row["RestorationTime"].ToString())).ToString(dateFormat);
                    else
                        reportRow["Restoration Time"] = dateUnavailable;

                    //reportRow["Restoration Time"] = CommonBLL.GetFormattedData(row["RestorationTime"].ToString());
                    reportRow["Cumulative Active Energy"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKWH"].ToString());
                    reportRow["Cumulative Apparent Energy"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVAH"].ToString());
                    reportRow["Cumulative Reactive Energy (Lag)"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVARHLag"].ToString());
                    reportRow["Cumulative Reactive Energy (Lead)"] = CommonBLL.GetFormattedData(row["CumulativeEnergyKVARHLead"].ToString());

                    reportRow["Current Month MD1"] = CommonBLL.GetFormattedData(row["CumulativeMD1"].ToString());
                    if (row["CumulativeMD1TimeStamp"].ToString() != "0")
                        reportRow["MD1 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD1TimeStamp"].ToString()) + "00")).ToString(dateFormat);
                    else
                        reportRow["MD1 TimeStamp"] = dateUnavailable;

                    reportRow["Current Month MD2"] = CommonBLL.GetFormattedData(row["CumulativeMD2"].ToString());
                    if (row["CumulativeMD2TimeStamp"].ToString() != "0")
                        reportRow["MD2 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD2TimeStamp"].ToString()) + "00")).ToString(dateFormat);
                    else
                        reportRow["MD2 TimeStamp"] = dateUnavailable;

                    reportRow["CumulativeExportEnergyKWH"] = string.IsNullOrEmpty(row["CumulativeExportEnergyKWH"].ToString()) ? NOTAPPLICABLE : CommonBLL.GetFormattedData(row["CumulativeExportEnergyKWH"].ToString()) ;
                    reportRow["CumulativeExportEnergyKVAH"] = string.IsNullOrEmpty(row["CumulativeExportEnergyKVAH"].ToString()) ? NOTAPPLICABLE : CommonBLL.GetFormattedData(row["CumulativeExportEnergyKVAH"].ToString());  
					//reportRow["Current Month MD3"] = CommonBLL.GetFormattedData(row["CumulativeMD3"].ToString());
					//if (row["CumulativeMD3TimeStamp"].ToString() != "0")
					//    reportRow["MD3 TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["CumulativeMD3TimeStamp"].ToString()) + "00")).ToString(dateFormat);
					//else
					//    reportRow["MD3 TimeStamp"] = dateUnavailable;
                    reportXSD.Tables["BillingGeneralTable"].Rows.Add(reportRow);
                   
                }
            }
        }

        private void FillInstantXSD(DataSet instantData)
        {
            DataRow reportRow;
            if (instantData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in instantData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["InstantTable"].NewRow();
                    reportRow["voltage_RPhase"] = CommonBLL.GetFormattedData(row["VoltageRPhase"].ToString());
                    reportRow["voltage_YPhase"] = CommonBLL.GetFormattedData(row["VoltageYPhase"].ToString());
                    reportRow["voltage_BPhase"] = CommonBLL.GetFormattedData(row["VoltageBPhase"].ToString());
                    reportRow["Current_RPhase"] = CommonBLL.GetFormattedData(row["CurrentRPhase"].ToString());
                    reportRow["Current_YPhase"] = CommonBLL.GetFormattedData(row["CurrentYPhase"].ToString());
                    reportRow["Current_BPhase"] = CommonBLL.GetFormattedData(row["CurrentBPhase"].ToString());
                    reportRow["activePower"] = CommonBLL.GetFormattedData(row["InstantActivepower"].ToString());
                    reportRow["apparentPower"] = CommonBLL.GetFormattedData(row["InstantApparentPower"].ToString());
                    reportRow["reactivePowerLag"] = CommonBLL.GetFormattedData(row["InstantReactiveLagPower"].ToString());
                    reportRow["reaactivePowerLead"] = CommonBLL.GetFormattedData(row["InstantReactiveLeadPower"].ToString());
                    reportRow["totalPowerFactor"] = CommonBLL.GetFormattedData(row["TotalPowerFactor"].ToString());
                    reportRow["PowerFactor_Rphase"] = CommonBLL.GetFormattedData(row["PowerFactorRPhase"].ToString());
                    reportRow["PowerFactor_Yphase"] = CommonBLL.GetFormattedData(row["PowerFactorYPhase"].ToString());
                    reportRow["PowerFactor_Bphase"] = CommonBLL.GetFormattedData(row["PowerFactorBPhase"].ToString());
                    reportRow["AveragePowerFactor"] = CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString());
                    reportRow["Frequency"] = CommonBLL.GetFormattedData(row["Frequency"].ToString());
                    reportRow["RisingDemand_KW"] = CommonBLL.GetFormattedData(row["RisingDemandKW"].ToString());
                    reportRow["ElapsedTime_KW"] = CommonBLL.GetFormattedData(row["ElapsedTimeKW"].ToString());
                    reportRow["RisingDemand_KVA"] = CommonBLL.GetFormattedData(row["RisingDemandKVA"].ToString());
                    reportRow["ElapsedTime_KVA"] = CommonBLL.GetFormattedData(row["ElapsedTimeKVA"].ToString());
                    reportXSD.Tables["InstantTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillPhasorXSD(DataSet phasorData)
        {
            DataRow reportRow;
            if (phasorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PhasorTable"].NewRow();
                reportRow["RVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[0][1].ToString());
                reportRow["YVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[1][1].ToString());
                reportRow["BVoltage"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[2][1].ToString());
                reportRow["RCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[3][1].ToString());
                reportRow["YCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[4][1].ToString());
                reportRow["BCurrent"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[5][1].ToString());
				reportRow["TotalActivePower"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[6][1].ToString());
				reportRow["TotalInductivePower"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[7][1].ToString());
				reportRow["TotalCapacitivePower"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[8][1].ToString());
				reportRow["TotalApparentPower"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[9][1].ToString());
				reportRow["RPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[10][1].ToString());
				reportRow["YPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[11][1].ToString());
				reportRow["BPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[12][1].ToString());
				reportRow["TotalPF"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[13][1].ToString());
				reportRow["Frequency"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[14][1].ToString());
				reportRow["PhaseSequence"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[15][1].ToString());
				reportRow["TotalImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[16][1].ToString());
				reportRow["RImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[17][1].ToString());
                reportRow["YImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[18][1].ToString());
                reportRow["BImportExport"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[19][1].ToString());

                reportRow["RChannelMissing"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[20][1].ToString());
                reportRow["YChannelMissing"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[21][1].ToString());
                reportRow["BChannelMissing"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[22][1].ToString());

                reportRow["RLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[23][1].ToString());
                reportRow["YLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[24][1].ToString());
                reportRow["BLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[25][1].ToString());
                reportRow["TotalLagLead"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[26][1].ToString());
                reportRow["YPhaseAngle"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[27][1].ToString());
                reportRow["BPhaseAngle"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[28][1].ToString());
                reportRow["PhaseAngleDifference"] = CommonBLL.GetFormattedData(phasorData.Tables[0].Rows[29][1].ToString());
                reportXSD.Tables["PhasorTable"].Rows.Add(reportRow);
            }
        }

        private void FillPhasorDigram(DataSet phasorData)
        {
            try
            {
                PhasorDiagram phasorGraph = new PhasorDiagram();
                phasorGraph.PhasorDataset = phasorData;
                DataRow reportRow;
                Bitmap graphBitmap = new Bitmap(phasorGraph.Width, phasorGraph.Height);
                phasorGraph.DrawToBitmap(graphBitmap, phasorGraph.ClientRectangle);
                using (Graphics g = phasorGraph.CreateGraphics())
                {
                    g.DrawImageUnscaled(graphBitmap, new Point(0, 0));
                }
                graphBitmap.Save(AppDomain.CurrentDomain.BaseDirectory + "PhasorDiagram.jpg");

                byte[] DiagramByte = ImageTable(AppDomain.CurrentDomain.BaseDirectory + "PhasorDiagram.jpg");

                reportRow = reportXSD.Tables["PhasorDiagramTable"].NewRow();
                reportRow["Image"] = DiagramByte;
                reportXSD.Tables["PhasorDiagramTable"].Rows.Add(reportRow);
            }
            catch (Exception)
            {
            }
        }

        private void FillPowerOnHoursXSD(DataSet powerOnHoursData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerOnHoursData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerFactorTable"].NewRow();
                foreach (DataRow row in powerOnHoursData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        reportRow["Current"] = CommonBLL.GetFormattedHourData(row["Values (HH:MM)"].ToString());
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        reportRow[colName] = CommonBLL.GetFormattedHourData(row["Values (HH:MM)"].ToString());
                    }
                    historyID++;
                }
                reportXSD.Tables["PowerFactorTable"].Rows.Add(reportRow);
            }
        }
        /// <summary>
        /// Used to fill data in TOU table
        /// </summary>
        /// <param name="powerOnHoursData"></param>
        private void FillTouXSD(DataSet touData)
        {
            DataRow reportRow;
            foreach (DataRow row in touData.Tables[0].Rows)
            {
                reportRow = reportXSD.Tables["TouConfiguration"].NewRow();
                reportRow["S. No."] = row["S.NO."].ToString();
                reportRow["Slot No."] = row["Slot No."].ToString();
                reportRow["Zone Start Time(HH:MM)"] = row["Zone Start Time(HH:MM)"].ToString();
                reportRow["Zone End Time(HH:MM)"] = row["Zone End Time(HH:MM)"].ToString();
                reportRow["Tariff Zone"] = row["Tariff Zone"].ToString();
                reportXSD.Tables["TouConfiguration"].Rows.Add(reportRow);
            }
        }


        private void FillPowerFactorXSD(DataSet powerFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            if (powerFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["PowerOnHoursTable"].NewRow();
                foreach (DataRow row in powerFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        if (row["AveragePowerFactor"].ToString() == "")
                            reportRow["Current"] = "----";
                        else
                            reportRow["Current"] = CommonBLL.RemoveUnit(row["AveragePowerFactor"].ToString());//CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString()).Remove(row["AveragePowerFactor"].ToString().Length);
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        //reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["AveragePowerFactor"].ToString().ToString()));
                        reportRow[colName] = CommonBLL.RemoveUnit(row["AveragePowerFactor"].ToString().ToString());
                    }
                    historyID++;
                }
                reportXSD.Tables["PowerOnHoursTable"].Rows.Add(reportRow);
            }
        }

        private void FillTariffPowerFactorXSD(DataSet tariffPF)
        {
            DataRow reportRow;
            string colName = string.Empty;
            if (tariffPF.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in tariffPF.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["TODPowerFactorTable"].NewRow();
                    foreach (DataColumn col in tariffPF.Tables[0].Columns)
                    {
                        if (col.Ordinal == 0)
                            reportRow["History"] = CommonBLL.RemoveUnit(row[col].ToString());//CommonBLL.GetFormattedData(row[col].ToString());
                        else
                            reportRow[string.Concat("Tariff", col.Ordinal.ToString())] = CommonBLL.RemoveUnit(row[col].ToString());//CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[col].ToString()));
                    }
                    reportXSD.Tables["TODPowerFactorTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillLoadFactorXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["LoadFactorTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        //reportRow["Current"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["LoadFactor"].ToString()));
                        reportRow["Current"] = Convert.ToInt32(CommonBLL.RemoveUnit(row["LoadFactor"].ToString())).ToString("0");
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        //reportRow[colName] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["LoadFactor"].ToString().ToString()));
                        reportRow[colName] = Convert.ToInt32(CommonBLL.RemoveUnit(row["LoadFactor"].ToString().ToString())).ToString("0");
                    }
                    historyID++;
                }
                reportXSD.Tables["LoadFactorTable"].Rows.Add(reportRow);
            }
        }

        private void FillCTRatioXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["CTRatioTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    if (historyID == 0)
                    {
                        //reportRow["Current"] = CommonBLL.GetFormattedData(row["CTRatio"].ToString());
                        reportRow["Current"] = "------";
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        string val = CommonBLL.GetFormattedData(row["BillingResetType"].ToString().ToString());
                        if (val.IndexOf('-') > 0)
                            val = val.Substring(0, val.IndexOf('-'));
                        try
                        {
							if (!(val.Contains("A") || val.Contains("M") || val.Contains("B")))
							{
								val = Int32.Parse(val).ToString();
							}
                        }
                        catch (Exception)
                        {
                            val = "0";
                        }
                        reportRow[colName] = val;
                    }
                    historyID++;
                }
                reportXSD.Tables["CTRatioTable"].Rows.Add(reportRow);
            }
        }

        private void FillBillingMechanismXSD(DataSet loadFactorData)
        {
            DataRow reportRow;
            int historyID = 0;
            string colName = string.Empty;
            string sTemp = string.Empty;

            if (loadFactorData.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingMechanismTable"].NewRow();
                foreach (DataRow row in loadFactorData.Tables[0].Rows)
                {
                    sTemp = row["CTRatio"].ToString();
                    if (historyID == 0)
                    {
                        if (sTemp.IndexOf('-') != -1)
                            reportRow["Current"] = sTemp.Substring(sTemp.IndexOf('-') + 1);
                        else
                            reportRow["Current"] = "-------";
                    }
                    else
                    {
                        colName = "History -" + historyID.ToString();
                        if (sTemp.IndexOf('-') != -1)
                            reportRow[colName] = sTemp.Substring(sTemp.IndexOf('-') + 1);
                        else
                            reportRow[colName] = "-------";
                    }
                    historyID++;
                }
                reportXSD.Tables["BillingMechanismTable"].Rows.Add(reportRow);
            }
        }

        private void FillBillingTamperCounterXSD(DataSet billingTamperCounterData)
        {
            DataRow reportRow;
            int historyID = 0;
            string tamperDescription = string.Empty;
            string colName = string.Empty;
            Dictionary<string, string> tamperCounterDictionary = new TamperCounterBLL().CreateTamperCounterDictionary();
            if (billingTamperCounterData.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn col in billingTamperCounterData.Tables[0].Columns)
                {
                    if (!tamperCounterDictionary.TryGetValue(col.ColumnName, out tamperDescription))
                        tamperDescription = string.Empty;
                    else
                    {
                        historyID = 0;
                        reportRow = reportXSD.Tables["BillingTamperCounterTable"].NewRow();
                        reportRow["TamperParameters"] = tamperDescription;
                        if (tamperDescription == "Current Phase Reversal Tamper")
                            continue;
                        foreach (DataRow row in billingTamperCounterData.Tables[0].Rows)
                        {
                            if (historyID == 0)
                                reportRow["Current"] = row[col.ColumnName].ToString();
                            else
                            {
                                colName = "History" + historyID.ToString();
                                reportRow[colName] = row[col.ColumnName].ToString().ToString();
                            }
                            historyID++;
                        }
                        reportXSD.Tables["BillingTamperCounterTable"].Rows.Add(reportRow);
                    }
                }
            }
        }

        private void FillMainEnergyXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["MainEnergyTable"].NewRow();
                    string val = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                    if (val == "0")
                        reportRow["History"] = "Current";
                    else
                        reportRow["History"] = "History-" + val;
                    if (row["BillingTimestamp"].ToString() != "0")
                        reportRow["BillingDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["BillingTimestamp"].ToString() + "00")).ToString(dateFormat);
                    else
                        reportRow["BillingDate"] = dateUnavailable;
                    reportRow["kWh"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                    reportRow["kVAh"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                    reportRow["KVARh (Lag)"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                    reportRow["KVARh (Lead)"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                    reportRow["kWh (Export)"] = string.IsNullOrEmpty(row["CumulativeExportEnergyKWH"].ToString())  ? NOTAPPLICABLE : CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKWH"].ToString()));
                    reportRow["kVAh (Export)"] = string.IsNullOrEmpty(row["CumulativeExportEnergyKVAH"].ToString()) ? NOTAPPLICABLE : CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKVAH"].ToString()));
                    reportXSD.Tables["MainEnergyTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTariffEnergyXSD(DataSet mainEnergyData, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                row_kWh = reportXSD.Tables["TODKWhTable"].NewRow();
                row_kVAh = reportXSD.Tables["TODKVAhTable"].NewRow();
                row_kVArh_lag = reportXSD.Tables["TODKVARhLagTable"].NewRow();
                row_kVAhrg_lead = reportXSD.Tables["TODKVARhLeadTable"].NewRow();

                if (historyID == 0)
                    row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = "Current";
                else
                    row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = string.Concat("History ", String.Format("{0:00}", historyID));

                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    row_kWh[string.Concat("Tariff", row["Tariff Number"])] = row["kWh"].ToString();
                    row_kVAh[string.Concat("Tariff", row["Tariff Number"])] = row["kVAh"].ToString();
                    row_kVArh_lag[string.Concat("Tariff", row["Tariff Number"])] = row["kVArh (Lag)"].ToString();
                    row_kVAhrg_lead[string.Concat("Tariff", row["Tariff Number"])] = row["kVArh (Lead)"].ToString();
                }
                reportXSD.Tables["TODKWhTable"].Rows.Add(row_kWh);
                reportXSD.Tables["TODKVAhTable"].Rows.Add(row_kVAh);
                reportXSD.Tables["TODKVARhLagTable"].Rows.Add(row_kVArh_lag);
                reportXSD.Tables["TODKVARhLeadTable"].Rows.Add(row_kVAhrg_lead);
            }
        }

        private void FillMaximumDemandXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["MaximumDemandTable"].NewRow();
                    string val = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                    if (val == "0")
                        reportRow["History"] = "Current";
                    else
                        reportRow["History"] = "History-" + val;
                    if (row["BillingTimestamp"].ToString() != "0")
                        reportRow["BillingDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["BillingTimestamp"].ToString())).ToString(dateFormat);
                    else
                        reportRow["BillingDate"] = dateUnavailable;
                    reportRow["MD1"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD1"].ToString()));
                    if (row["CumulativeMD1TimeStamp"].ToString() != "0")
                        reportRow["MD1_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD1TimeStamp"].ToString())).ToString(dateFormat);
                    else
                        reportRow["MD1_TimeStamp"] = dateUnavailable;
                    /*GKG 25/02/13 : 13709 Unit coming wrong */
                    //reportRow["MD2"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD2"].ToString()));
                    if (row["CumulativeMD2"].ToString().IndexOf("KVA") < 0)
                        reportRow["MD2"] = "------";
                    else
                        reportRow["MD2"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD2"].ToString())); 
                    /*GKG 25/02/13 : 13709 Unit coming wrong */

                    if (row["CumulativeMD2TimeStamp"].ToString() != "0")
                        reportRow["MD2_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD2TimeStamp"].ToString())).ToString(dateFormat);
                    else
                        reportRow["MD2_TimeStamp"] = dateUnavailable;

					//reportRow["MD3"] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeMD3"].ToString()));
					//if (row["CumulativeMD3TimeStamp"].ToString() != "0")
					//    reportRow["MD3_TimeStamp"] = DateUtility.LongToDateTime(Convert.ToInt64(row["CumulativeMD3TimeStamp"].ToString())).ToString(dateFormat);
					//else
					//    reportRow["MD3_TimeStamp"] = dateUnavailable;
                    reportXSD.Tables["MaximumDemandTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTODMDXSD(DataSet TODMDData, int historyID)
        {
            DataRow reportRow;
            if (TODMDData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in TODMDData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["TODDemandTable"].NewRow();
                    reportRow["History"] = "History " + historyID;
                    reportRow["Tariff"] = row["Tariff Number"];
                    reportRow["MD1"] = row[1];
                    if (row[2].ToString() != "0")
                        reportRow["MD1_TimeStamp"] = row[2];
                    else
                        reportRow["MD1_TimeStamp"] = dateUnavailable;
                    reportRow["MD2"] = row[3];
                    if (row[4].ToString() != "0")
                        reportRow["MD2_TimeStamp"] = row[4];
                    else
                        reportRow["MD2_TimeStamp"] = dateUnavailable;
					//reportRow["MD3"] = row[5];
					//if (row[6].ToString() != "0")
					//    reportRow["MD3_TimeStamp"] = row[6];
					//else
					//    reportRow["MD3_TimeStamp"] = dateUnavailable;
                    reportXSD.Tables["TODDemandTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillEnergyConsumptionXSD(DataSet mainEnergyData)
        {
            DataRow reportRow;
            int i = 0;
            string prevKWh = string.Empty;
            string prevKVAh = string.Empty;
            string prevKVARhLag = string.Empty;
            string prevKVARhLead = string.Empty;
            string prevHistory = string.Empty;
            string prevExportKVAh = string.Empty;
            string prevExportKWh = string.Empty;

            if (mainEnergyData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in mainEnergyData.Tables[0].Rows)
                {
                    if (i == 0)
                    {
                        prevHistory = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                        prevKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                        prevKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                        prevKVARhLag = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                        prevKVARhLead = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                        prevExportKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKWH"].ToString()));
                        prevExportKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKVAH"].ToString()));
                        i++;
                    }
                    else
                    {
                        reportRow = reportXSD.Tables["EnergyConsumptionTable"].NewRow();
                        reportRow["History"] = string.Concat("History ", prevHistory, " - ", CommonBLL.GetFormattedData(row["History_ID"].ToString()));
                        reportRow["kWh"] = (Convert.ToDecimal(prevKWh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString())))).ToString("0.000");
                        reportRow["kVAh"] = (Convert.ToDecimal(prevKVAh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString())))).ToString("0.000");
                        reportRow["KVARhLag"] = (Convert.ToDecimal(prevKVARhLag) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString())))).ToString("0.000");
                        reportRow["KVARhLead"] = (Convert.ToDecimal(prevKVARhLead) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString())))).ToString("0.000");

                        //If Export kWh parameter is not present then show Not Applicable text
                        if (!string.IsNullOrEmpty(row["CumulativeExportEnergyKWH"].ToString()))
                        {
                            reportRow["kWh (Export)"] = (Convert.ToDecimal(prevExportKWh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKWH"].ToString())))).ToString("0.000");
                        }
                        else
                        {
                            reportRow["kWh (Export)"] = NOTAPPLICABLE;
                        }
                        //If Export kVAh parameter is not present then show Not Applicable text
                        if (!string.IsNullOrEmpty(row["CumulativeExportEnergyKWH"].ToString()))
                        {
                            reportRow["kVAh (Export)"] = (Convert.ToDecimal(prevExportKVAh) - Convert.ToDecimal(CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKVAH"].ToString())))).ToString("0.000");
                        }
                        else
                        {
                            reportRow["kVAh (Export)"] = NOTAPPLICABLE;
                        }
                        
                        reportXSD.Tables["EnergyConsumptionTable"].Rows.Add(reportRow);
                        prevHistory = CommonBLL.GetFormattedData(row["History_ID"].ToString());
                        prevKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKWh"].ToString()));
                        prevKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVAh"].ToString()));
                        prevKVARhLag = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLag"].ToString()));
                        prevKVARhLead = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeEnergyKVARhLead"].ToString()));
                        prevExportKWh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKWH"].ToString()));
                        prevExportKVAh = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row["CumulativeExportEnergyKVAH"].ToString()));
                    }
                }
            }
        }

        private void FillTODEnergyConsumptionXSD(DataSet currentTariffEnergyDS, DataSet nextTariffEnergyDS, int historyID)
        {
            DataRow row_kWh;
            DataRow row_kVAh;
            DataRow row_kVArh_lag;
            DataRow row_kVAhrg_lead;

            int i = 0;

            row_kWh = reportXSD.Tables["TODKWhConsumptionTable"].NewRow();
            row_kVAh = reportXSD.Tables["TODKVAhConsumptionTable"].NewRow();
            row_kVArh_lag = reportXSD.Tables["TODKVARhLagConsumptionTable"].NewRow();
            row_kVAhrg_lead = reportXSD.Tables["TODKVARhLeadConsumptionTable"].NewRow();

            row_kWh["History"] = row_kVAh["History"] = row_kVArh_lag["History"] = row_kVAhrg_lead["History"] = string.Concat(String.Format("{0:00}", historyID), " - ", String.Format("{0:00}", historyID + 1));
            foreach (DataRow row in currentTariffEnergyDS.Tables[0].Rows)
            {
                row_kWh[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kWh"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kWh"].ToString()))).ToString("0.000");
                row_kVAh[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVAh"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVAh"].ToString()))).ToString("0.000");
                row_kVArh_lag[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVArh (Lag)"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVArh (Lag)"].ToString()))).ToString("0.000");
                row_kVAhrg_lead[string.Concat("Tariff", row["Tariff Number"])] = (Convert.ToDecimal(row["kVArh (Lead)"].ToString()) - Convert.ToDecimal(CommonBLL.GetFormattedData(nextTariffEnergyDS.Tables[0].Rows[i]["kVArh (Lead)"].ToString()))).ToString("0.000");
                i++;
            }
            reportXSD.Tables["TODKWhConsumptionTable"].Rows.Add(row_kWh);
            reportXSD.Tables["TODKVAhConsumptionTable"].Rows.Add(row_kVAh);
            reportXSD.Tables["TODKVARhLagConsumptionTable"].Rows.Add(row_kVArh_lag);
            reportXSD.Tables["TODKVARhLeadConsumptionTable"].Rows.Add(row_kVAhrg_lead);
        }

        private void FillLoadSurveyXSD(DataSet loadSurveyData)
        {
            //if (SMD_rbtnLoadSurveyDemand.Checked == true)
                FillLoadSurveyDemandXSD(loadSurveyData);
            //else
            //    FillLoadSurveyEnergyXSD(loadSurveyData);
        }

        private void FillLoadSurveyDemandXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            int dateTimeCount = 0;
            DateTime PreviousDate = DateTime.Now;
            try
            {
                if (loadSurveyData == null || loadSurveyData.Tables[0].Rows.Count == 0)
                    return;
                if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                    loadSurveyData.Tables[0].Columns.Remove("TamperStatus"); 

                reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();

                foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                    lsHeadings.Add(col.ColumnName);

                foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    if (dateTimeCount == 0)
                    {
                        PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                        reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0])));
                        dateTimeCount++;
                    }
                    else
                    {
                        string dates = "";
                        DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                        TimeSpan ts = currentDate - PreviousDate;
                        if (ts.Days > 0)
                        {
                            currentDate = currentDate.AddDays(-1);
                            long datesval = DateUtility.DateTimeToLong(currentDate);
                            dates = DateUtility.LongToStringDateFormat(datesval);
                            reportRow["GroupDateTime"] = dates;
                            PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0]))).Date;
                        }
                        else
                        {
                            dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(row[0])));
                            reportRow["GroupDateTime"] = dates;
                        }
                    }
                    for (int colCount = 1; colCount < loadSurveyData.Tables[0].Columns.Count - 1; colCount++)
                    {
                        string ParameterColValue = "Parameter" + Convert.ToString(colCount);
                        reportRow[ParameterColValue] = row[colCount].ToString();
                    }
                    string dateTimes = Convert.ToString(row[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeColumn"] = dateTimes;
                    reportXSD.Tables["LoadSurveyTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillLoadSurveyEnergyXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            int dateTimeCount = 1;
            int IntervalValue = 4;
            DateTime PreviousDate = DateTime.Now;
            try
            {
                reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();

                if (loadSurveyData == null)
                {
                    return;
                }
                if (loadSurveyData.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                for (int ParamCount = 0; ParamCount < loadSurveyData.Tables[0].Columns.Count; ParamCount++)
                {
                    if (loadSurveyData.Tables[0].Columns[ParamCount].ColumnName.Contains("Demand"))
                    {
                        lsHeadings.Add(GetEnergyColumnName(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName));
                    }
                    else
                    {
                        lsHeadings.Add(loadSurveyData.Tables[0].Columns[ParamCount].ColumnName);
                    }
                }
                foreach (DataRow Drow in loadSurveyData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    //For the date Time to split at 00:15 hours
                    if (dateTimeCount == 1)
                    {
                        PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                        reportRow["GroupDateTime"] = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                        dateTimeCount++;
                    }
                    else
                    {
                        string dates = "";
                        if (!string.IsNullOrEmpty(Convert.ToString(Drow[0])))
                        {
                            DateTime currentDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                            TimeSpan ts = currentDate.Date  - PreviousDate.Date  ;
                            if (ts.Days > 0)
                            {
                                currentDate = currentDate.AddDays(-1);
                                long datesval = DateUtility.DateTimeToLong(currentDate);
                                dates = DateUtility.LongToStringDateFormat(datesval);
                                reportRow["GroupDateTime"] = dates;
                                PreviousDate = DateUtility.LongToDateTime(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                            }
                            else
                            {
                                dates = DateUtility.LongToStringDateFormat(CommonBLL.SplitLoadsurveyDateUnit(Convert.ToString(Drow[0])));
                                reportRow["GroupDateTime"] = dates;
                            }
                        }
                    }
                    string dateTimes = Convert.ToString(Drow[0]);
                    if (dateTimes.Length > 10)
                        dateTimes = dateTimes.Substring(11, dateTimes.Length - 11);
                    reportRow["TimeColumn"] = dateTimes;
                    for (int ParamCount = 1; ParamCount < loadSurveyData.Tables[0].Columns.Count - 1; ParamCount++)
                    {
                        string ParameterColValue = "Parameter" + Convert.ToString(ParamCount);
                        if (loadSurveyData.Tables[0].Columns[ParamCount].ColumnName.Contains("Demand"))
                        {
                            if (Convert.ToDouble(Drow[ParamCount].ToString()) != -1)
                            {
                                string str = string.Format("{0:0.0000}", (Convert.ToDouble(Drow[ParamCount].ToString()) / IntervalValue)).ToString();
                                str = str.Substring(0, str.Length - 1);
                                reportRow[ParameterColValue] = str;
                            }
                            else
                            {
                                reportRow[ParameterColValue] = "-1";
                            }
                        }
                        else
                            reportRow[ParameterColValue] = Drow[ParamCount].ToString();
                    }
                    reportXSD.Tables["LoadSurveyTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillTamperXSD(DataSet tamperData)
        {
            DataRow reportRow;

            if (tamperData.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in tamperData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["TamperTable"].NewRow();

                    reportRow["Tamper Description"] = row["TamperType"].ToString();
                    reportRow["Tamper Counter"] = row["TamperCounter"].ToString().ToString();

                    reportRow["Occured DateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    reportRow["Restored DateTime"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperRestoredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                    reportRow["RVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["RVoltageRestored"].ToString()), " V");
                    reportRow["YVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["YVoltageRestored"].ToString()), " V");
                    reportRow["BVoltage_restored"] = string.Concat(CommonBLL.GetFormattedData(row["BVoltageRestored"].ToString()), " V");
                    reportRow["RCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["RCurrentRestored"].ToString()), " A");
                    reportRow["YCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["YCurrentRestored"].ToString()), " A");
                    reportRow["BCurrent_restored"] = string.Concat(CommonBLL.GetFormattedData(row["BCurrentRestored"].ToString()), " A");
                    reportRow["RPF_restored"] = CommonBLL.GetFormattedData(row["RPFRestored"].ToString());
                    reportRow["YPF_restored"] = CommonBLL.GetFormattedData(row["YPFRestored"].ToString());
                    reportRow["BPF_restored"] = CommonBLL.GetFormattedData(row["BPFRestored"].ToString());
                    reportRow["TotalPF_restored"] = CommonBLL.GetFormattedData(row["TotalPFRestored"].ToString());
                    reportRow["kWh_restored"] = string.Concat(CommonBLL.GetFormattedData(row["kWhRestored"].ToString()), " kWh");
                    reportRow["kVAh_restored"] = string.Concat(CommonBLL.GetFormattedData(row["kVAhRestored"].ToString()), " kVAh");
                    reportRow["RVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["RVoltageOccurred"].ToString()), " V");
                    reportRow["YVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["YVoltageOccurred"].ToString()), " V");
                    reportRow["BVoltage_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["BVoltageOccurred"].ToString()), " V");
                    reportRow["RCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["RCurrentOccurred"].ToString()), " A");
                    reportRow["YCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["YCurrentOccurred"].ToString()), " A");
                    reportRow["BCurrent_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["BCurrentOccurred"].ToString()), " A");
                    reportRow["RPF_occurred"] = CommonBLL.GetFormattedData(row["RPFOccurred"].ToString());
                    reportRow["YPF_occurred"] = CommonBLL.GetFormattedData(row["YPFOccurred"].ToString());
                    reportRow["BPF_occurred"] = CommonBLL.GetFormattedData(row["BPFOccurred"].ToString());
                    reportRow["TotalPF_occurred"] = CommonBLL.GetFormattedData(row["TotalPFOccurred"].ToString());
                    reportRow["kWh_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["kWhOccurred"].ToString()), " kWh");
                    reportRow["kVAh_occurred"] = string.Concat(CommonBLL.GetFormattedData(row["kVAhOccurred"].ToString()), " kVAh");

                    reportXSD.Tables["TamperTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillAllTamperXSD(DataSet tamperData, string tamperDescription, string count, string occurrenceTime, string restorationTime, string duration)
        {
            try
            {
                DataRow reportRow;

                if (tamperData.Tables[0].Rows.Count > 0)
                {
                    if (tamperData.Tables[0].Rows[0]["TamperCode"].ToString() != "225")
                    {
                        reportRow = reportXSD.Tables["TamperTable"].NewRow();
                        reportRow["Tamper Description"] = tamperDescription;//row["TamperType"].ToString();
                        reportRow["Tamper Counter"] = count;//row["TamperCounter"].ToString().ToString();
                        if (occurrenceTime.Contains("1900")) occurrenceTime = dateUnavailable;
                        else
                            reportRow["Occured DateTime"] = occurrenceTime;//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperOccurredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                        if (restorationTime.Contains("1900")) restorationTime = dateUnavailable;
                        else
                            reportRow["Restored DateTime"] = restorationTime;//DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["TamperRestoredTime"].ToString()))).ToString("dd/MM/yyyy HH:mm:ss");
                        reportRow["RVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[0][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["RVoltageRestored"].ToString()), " V");
                        reportRow["YVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[1][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["YVoltageRestored"].ToString()), " V");
                        reportRow["BVoltage_restored"] = string.Concat(tamperData.Tables[0].Rows[2][2].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["BVoltageRestored"].ToString()), " V");
                        reportRow["RCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[3][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["RCurrentRestored"].ToString()), " A");
                        reportRow["YCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[4][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["YCurrentRestored"].ToString()), " A");
                        reportRow["BCurrent_restored"] = string.Concat(tamperData.Tables[0].Rows[5][2].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["BCurrentRestored"].ToString()), " A");
                        reportRow["RPF_restored"] = tamperData.Tables[0].Rows[6][2].ToString();//CommonBLL.GetFormattedData(row["RPFRestored"].ToString());
                        reportRow["YPF_restored"] = tamperData.Tables[0].Rows[7][2].ToString();//CommonBLL.GetFormattedData(row["YPFRestored"].ToString());
                        reportRow["BPF_restored"] = tamperData.Tables[0].Rows[8][2].ToString();//CommonBLL.GetFormattedData(row["BPFRestored"].ToString());
                        reportRow["TotalPF_restored"] = tamperData.Tables[0].Rows[9][2].ToString();//CommonBLL.GetFormattedData(row["TotalPFRestored"].ToString());
                        reportRow["kWh_restored"] = string.Concat(tamperData.Tables[0].Rows[10][2].ToString(), "kWh");//string.Concat(CommonBLL.GetFormattedData(row["kWhRestored"].ToString()), " kWh");
                        reportRow["kVAh_restored"] = string.Concat(tamperData.Tables[0].Rows[11][2].ToString(), "kVAh");//string.Concat(CommonBLL.GetFormattedData(row["kVAhRestored"].ToString()), " kVAh");
                        reportRow["RVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[0][1].ToString(), " V"); //string.Concat(CommonBLL.GetFormattedData(row["RVoltageOccurred"].ToString()), " V");
                        reportRow["YVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[1][1].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["YVoltageOccurred"].ToString()), " V");
                        reportRow["BVoltage_occurred"] = string.Concat(tamperData.Tables[0].Rows[2][1].ToString(), " V");//string.Concat(CommonBLL.GetFormattedData(row["BVoltageOccurred"].ToString()), " V");
                        reportRow["RCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[3][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["RCurrentOccurred"].ToString()), " A");
                        reportRow["YCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[4][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["YCurrentOccurred"].ToString()), " A");
                        reportRow["BCurrent_occurred"] = string.Concat(tamperData.Tables[0].Rows[5][1].ToString(), " A");//string.Concat(CommonBLL.GetFormattedData(row["BCurrentOccurred"].ToString()), " A");
                        reportRow["RPF_occurred"] = tamperData.Tables[0].Rows[6][1].ToString();//CommonBLL.GetFormattedData(row["RPFOccurred"].ToString());
                        reportRow["YPF_occurred"] = tamperData.Tables[0].Rows[7][1].ToString(); //CommonBLL.GetFormattedData(row["YPFOccurred"].ToString());
                        reportRow["BPF_occurred"] = tamperData.Tables[0].Rows[8][1].ToString(); //CommonBLL.GetFormattedData(row["BPFOccurred"].ToString());
                        reportRow["TotalPF_occurred"] = tamperData.Tables[0].Rows[9][1].ToString(); //CommonBLL.GetFormattedData(row["TotalPFOccurred"].ToString());
                        reportRow["kWh_occurred"] = string.Concat(tamperData.Tables[0].Rows[10][1].ToString(), "kWh"); //string.Concat(CommonBLL.GetFormattedData(row["kWhOccurred"].ToString()), " kWh");
                        reportRow["kVAh_occurred"] = string.Concat(tamperData.Tables[0].Rows[11][1].ToString(), "kVAh"); //string.Concat(CommonBLL.GetFormattedData(row["kVAhOccurred"].ToString()), " kVAh");

                        reportXSD.Tables["TamperTable"].Rows.Add(reportRow);
                    }
                    else
                    {
                        reportRow = reportXSD.Tables["PowerOnOffTamperTable"].NewRow();
                        reportRow["counter"] = count;
                        reportRow["OccurredDateTime"] = occurrenceTime;
                        reportRow["RestoredDateTime"] = restorationTime;
                        reportRow["Duration"] = duration;
                        reportXSD.Tables["PowerOnOffTamperTable"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        private void FillDTMLoadSurveyXSD(DataSet dtmLoadSurveyDS)
        {
            DataRow reportRow;
            DateTime DTMPreviousDate = DateTime.Now;
            int index = 0;
            int i = 0;
            foreach (DataRow row in dtmLoadSurveyDS.Tables[0].Rows)
            {
                System.Diagnostics.Debug.Print(i++.ToString());
                reportRow = reportXSD.Tables["DTMLoadSurveyTable"].NewRow();
                if (index == 0)
                {
                    DTMPreviousDate = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString())));
                    reportRow["GroupDate"] = DTMPreviousDate.ToString("dd/MM/yyyy");
                    index++;
                }
                else
                {
                    if (row["DTMDateTime"].ToString() != "0")
                    {
                        TimeSpan ts = Convert.ToDateTime(DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy")) - Convert.ToDateTime(DTMPreviousDate.ToString("dd/MM/yyyy"));
                        if (ts.Days > 0)
                        {
                            reportRow["GroupDate"] = Convert.ToDateTime(DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy")).AddDays(-1).ToString("dd/MM/yyyy");
                            DTMPreviousDate = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString())));
                        }
                        else
                        {
                            reportRow["GroupDate"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("dd/MM/yyyy");
                        }
                    }
                }
                if (row["DTMDateTime"].ToString() != "0")
                {
                    reportRow["Parameter1"] = row["KWh"];
                    reportRow["Parameter2"] = row["KVAh"];
                    reportRow["Parameter3"] = row["RPhaseKW"];
                    reportRow["Parameter4"] = row["YPhaseKW"];
                    reportRow["Parameter5"] = row["BPhaseKW"];
                    reportRow["Parameter6"] = string.Concat(row["RPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["RPhaseKVAr"]);
                    reportRow["Parameter7"] = string.Concat(row["YPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["YPhaseKVAr"]);
                    reportRow["Parameter8"] = string.Concat(row["BPhaseType"].ToString() == "Lag" ? "lg" : "ld", " ", row["BPhaseKVAr"]);
                    reportRow["Parameter9"] = row["RPhaseVoltage"];
                    reportRow["Parameter10"] = row["YPhaseVoltage"];
                    reportRow["Parameter11"] = row["BPhaseVoltage"];
                    reportRow["Parameter12"] = row["PowerDownTime"];
                    reportRow["TimeColumn"] = DateUtility.LongToDateTime(Convert.ToInt64(CommonBLL.GetFormattedData(row["DTMDateTime"].ToString()))).ToString("HH:mm");
                    reportXSD.Tables["DTMLoadSurveyTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillTransactionXSD(DataSet transactionDS)
        {
            DataRow reportRow;
            if (transactionDS == null)
                return;

            if (transactionDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in transactionDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["ProgrammingUpdatesTable"].NewRow();

                    reportRow["LastTimeStamp"] = row["UpdateSequence"].ToString();
                    reportRow["TimeStamp"] = row["LastTimeStamp"].ToString();
                    reportRow["Parameter1"] = row["Description1"].ToString();
                    reportRow["Parameter2"] = row["Description2"].ToString();
                    reportRow["Parameter3"] = row["Description3"].ToString();
                    reportRow["Parameter4"] = row["Description4"].ToString();
                    reportRow["Parameter5"] = row["Description5"].ToString();
                    reportRow["Parameter6"] = row["Description6"].ToString();
                    reportRow["Parameter7"] = row["Description7"].ToString();
                    reportRow["Parameter8"] = row["Description8"].ToString();
                    reportRow["Parameter9"] = row["Description9"].ToString();
                    reportRow["Parameter10"] = row["Description10"].ToString();
                    reportRow["Parameter11"] = row["Description11"].ToString();
                    reportRow["Parameter12"] = row["Description12"].ToString();
                    reportRow["Parameter13"] = row["Description13"].ToString();
                    reportRow["Parameter14"] = row["Description14"].ToString();
                    reportRow["Parameter15"] = row["Description15"].ToString();
                    reportRow["Parameter16"] = row["Description16"].ToString();
                    reportRow["Parameter17"] = row["Description17"].ToString();

                    reportXSD.Tables["ProgrammingUpdatesTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillRTCUpdatesXSD(DataSet rtcUpdatesDS, int totalRTCUpdates)
        {
            DataRow reportRow;
            if (rtcUpdatesDS == null)
                return;

            if (rtcUpdatesDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in rtcUpdatesDS.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["RTCUpdatesTable"].NewRow();
                    reportRow["RTCUpdates"] = row["RTC Updates"].ToString();
                    if (!string.IsNullOrEmpty(row["Old Time Stamp"].ToString()))
                        reportRow["previousRTC"] = row["Old Time Stamp"].ToString();
                    else
                        reportRow["previousRTC"] = dateUnavailable;

                    if (!string.IsNullOrEmpty(row["Updated Time Stamp"].ToString()))
                        reportRow["currentRTC"] = row["Updated Time Stamp"].ToString();
                    else
                        reportRow["currentRTC"] = dateUnavailable;
                    reportRow["NumberOfUpdates"] = totalRTCUpdates.ToString();
                    reportXSD.Tables["RTCUpdatesTable"].Rows.Add(reportRow);
                }
            }
        }

        private void FillFraudEnergyXSD(FraudEnergyEntity entity)
        {
            DataRow reportRow;
            if (entity != null)
            {
                reportRow = reportXSD.Tables["FraudEnergyTable"].NewRow();
				reportRow["MagneticInfluenceKWh"] = entity.MagneticInfluenceKWh;
				reportRow["MagneticInflueneceKVARhLag"] = entity.MagneticInflueneceKVARhLag;
				reportRow["MagneticInflueneceKVARhLead"] = entity.MagneticInflueneceKVARhLead;
				reportRow["MagneticInflueneceKVAh"] = entity.MagneticInflueneceKVAh;
				reportRow["ReverseEnergyKWh"] = entity.ReverseEnergyKWh;
				reportRow["ReverseEnergyKVAh"] = entity.ReverseEnergyKVAh;
                reportXSD.Tables["FraudEnergyTable"].Rows.Add(reportRow);
            }
        }

        private void ShowReport(string[] message)
        {

            ReportForm ObjRptForm = new ReportForm();
            //Report generalReport = new Report();
            MasterReport generalReport = new MasterReport();
            DataSet MaximumDemandDSet = new DataSet();
            string s = "";
            for (int i = 0; i < 16; i++)
            {
                if (!string.IsNullOrEmpty(message[i]))
                {
                    s += message[i] + "\n";
                }
            }
            if (!string.IsNullOrEmpty(s))
            {
                MessageBox.Show(s,"BCS");
            }
            //ArrayList LoadSurveyHeadings = new ArrayList();

            if (reportXSD.Tables["BillingGeneralTable"].Rows.Count == 0)
            {
                generalReport.SecGeneral.SectionFormat.EnableSuppress = true;
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam1 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[0].ReportDefinition.ReportObjects["TextMD1"];
                TextParam1.Text = MDHeadings[0];
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam2 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[0].ReportDefinition.ReportObjects["TextMD2"];
                TextParam2.Text = MDHeadings[1];
				//CrystalDecisions.CrystalReports.Engine.TextObject TextParam3 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[0].ReportDefinition.ReportObjects["TextMD3"];
				//TextParam3.Text = MDHeadings[2];

                // If Utility is not WBEXPORTVCL then do not show export parameters
                if (UtilityDetails.UtilityName != UtilityEntity.WBEXPORTVCL)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecGeneral.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                            //Get the static text that shows static text on reports
                            TextObject txtCumulativeExportKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtCumExportEnergyKWH"];
                            TextObject txtCumulativeExportKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtCumExportEnergyKVAH"];

                            //Get the static text that shows ":" text on reports
                            TextObject txtColon45 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text45"];
                            TextObject txtColon35 = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text35"];

                            //Get the data object that displays export values.
                            FieldObject objDBExportEnergyKVAH = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["CumulativeExportEnergyKVAH1"];
                            FieldObject objCumulativeExportEnergyKWH1 = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["CumulativeExportEnergyKWH1"];

                            //Set width as 0. this will hide the object on reports.
                            txtCumulativeExportKWH.Width = 0;
                            txtCumulativeExportKVAH.Width = 0;
                            txtColon35.Width = 0;
                            txtColon45.Width = 0;
                            objDBExportEnergyKVAH.Width = 0;
                            objCumulativeExportEnergyKWH1.Width = 0;

                        }
                    }
                }
                else
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecGeneral.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text32"];
                            objText.Width = 0;
                            TextObject objColValue = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text6"];
                            objColValue.Width = 0;
                            FieldObject objSeprator = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["MeterConstant1"];
                            objSeprator.Width = 0;

                        }
                    }
                }
            }

            if (reportXSD.Tables["InstantTable"].Rows.Count == 0)
            {
                generalReport.SecInstant.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["MainEnergyTable"].Rows.Count == 0)
            {
                generalReport.SecMainEnergy.SectionFormat.EnableSuppress = true;

            }
            else
            {
                // If Utility is not WBEXPORTVCL then do not show export parameters
                if (UtilityDetails.UtilityName != UtilityEntity.WBEXPORTVCL)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMainEnergy.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);

                            //Get the static text that shows static text on reports
                            TextObject txtCumulativeExportKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text9"];
                            TextObject txtCumulativeExportKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text10"];
                            txtCumulativeExportKWH.Width = 0;
                            txtCumulativeExportKVAH.Width = 0;

                            //Get the data object that displays export values.
                            FieldObject objDBExportEnergyKVAH = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["kVAhExport1"];
                            FieldObject objDBExportEnergyKWH = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["kWhExport1"];
                            
                            //Set width as 0. this will hide the object on reports.
                            objDBExportEnergyKVAH.Width = 0;
                            objDBExportEnergyKWH.Width = 0;
                        }
                    }
                }
            }
            if (reportXSD.Tables["EnergyConsumptionTable"].Rows.Count == 0)
            {
                generalReport.SecMainEnergyConsumption.SectionFormat.EnableSuppress = true;
            }
            else
            {
                // If Utility is not WBEXPORTVCL then do not show export parameters
                if (UtilityDetails.UtilityName != UtilityEntity.WBEXPORTVCL)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.SecMainEnergyConsumption.ReportObjects;
                    CrystalDecisions.CrystalReports.Engine.SubreportObject repMainEnergy = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                    foreach (ReportObject reportObject in rebObjCol)
                    {
                        if (reportObject.Kind == ReportObjectKind.SubreportObject)
                        {
                            SubreportObject subreportObject = (SubreportObject)reportObject;
                            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                            TextObject txtCumulativeExportKWH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtCumExportEnergyKWH"];
                            TextObject txtCumulativeExportKVAH = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtCumExportEnergyKVAH"];
                            txtCumulativeExportKWH.Width = 0;
                            txtCumulativeExportKVAH.Width = 0;

                            //Get the data object that displays export values.
                            FieldObject objDBExportEnergyKVAH = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["kVAhExport1"];
                            FieldObject objDBExportEnergyKWH = (FieldObject)subReportDocument.ReportDefinition.ReportObjects["kWhExport1"];

                            //Set width as 0. this will hide the object on reports.
                            objDBExportEnergyKVAH.Width = 0;
                            objDBExportEnergyKWH.Width = 0;
                       
                        }
                    }
                }
            }

            if ((reportXSD.Tables["TODKWhTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhTable"].Rows.Count == 0) ||
                (reportXSD.Tables["TODKVARhLagTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVARhLeadTable"].Rows.Count == 0))
            {
                generalReport.SecTODkVAhEnergy.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkVArhLagEnergy.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkVArhLeadEnergy.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkWhEnergy.SectionFormat.EnableSuppress = true;
            }

            if ((reportXSD.Tables["TODKWhConsumptionTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhConsumptionTable"].Rows.Count == 0) ||
                (reportXSD.Tables["TODKVARhLagConsumptionTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVARhLeadConsumptionTable"].Rows.Count == 0))
            {
                generalReport.SecTODkVAhConsumption.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkVArhLagConsumption.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkVArhLeadConsumption.SectionFormat.EnableSuppress = true;
                generalReport.SecTODkWhConsumption.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["MaximumDemandTable"].Rows.Count == 0)
            {
                generalReport.SecMaximumDemand.SectionFormat.EnableSuppress = true;
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam1 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[9].ReportDefinition.ReportObjects["TextMD1"];
                TextParam1.Text = MDHeadings[0];
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam3 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[9].ReportDefinition.ReportObjects["TextMD2"];
                TextParam3.Text = MDHeadings[1];
				//CrystalDecisions.CrystalReports.Engine.TextObject TextParam5 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[10].ReportDefinition.ReportObjects["TextMD3"];
				//TextParam5.Text = MDHeadings[2];
            }
            if (reportXSD.Tables["TODDemandTable"].Rows.Count == 0)
            {
                generalReport.SecTODDemand.SectionFormat.EnableSuppress = true;
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam1 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[17].ReportDefinition.ReportObjects["TextMD1"];
                TextParam1.Text = MDHeadings[0];
                CrystalDecisions.CrystalReports.Engine.TextObject TextParam3 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[17].ReportDefinition.ReportObjects["TextMD2"];
                TextParam3.Text = MDHeadings[1];
				//CrystalDecisions.CrystalReports.Engine.TextObject TextParam5 = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[18].ReportDefinition.ReportObjects["TextMD3"];
				//TextParam5.Text = MDHeadings[2];
            }
            if (reportXSD.Tables["PowerOnHoursTable"].Rows.Count == 0)
            {
                generalReport.SecPowerOnHours.SectionFormat.EnableSuppress = true;
            }
            //VBM - Display only dummy data in tou report for JDVVNL
            if (reportXSD.Tables["TouConfiguration"].Rows.Count == 0)
            {
                generalReport.DetailSection1.SectionFormat.EnableSuppress = true;
            }
            else
            {
                CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = generalReport.DetailSection1.ReportObjects;
                foreach (ReportObject reportObject in rebObjCol)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        TextObject objText = (TextObject)subReportDocument.ReportDefinition.ReportObjects["txtSeasonNumber"];
                        objText.Text = seasonNumber.ToString();

                    }
                }
            }
            //VBM - Display only dummy data in tou report for JDVVNL
            if (reportXSD.Tables["PowerFactorTable"].Rows.Count == 0)
            {
                generalReport.SecPowerFactor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TODPowerFactorTable"].Rows.Count == 0)
            {
                generalReport.SecTODPowerFactor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["LoadFactorTable"].Rows.Count == 0)
            {
                generalReport.SecLoadFactor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["BillingTamperCounterTable"].Rows.Count == 0)
            {
                generalReport.SecBillingTamperCounter.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["CTRatioTable"].Rows.Count == 0)
            {
                generalReport.SecCTRatio.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["BillingMechanismTable"].Rows.Count == 0)
            {
                generalReport.SecBillingMechanism.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["LoadSurveyTable"].Rows.Count == 0)
            {
                generalReport.SecLoadSurvey.SectionFormat.EnableSuppress = true;
            }
            else
            {
                for (int ColHeadCount = 1; ColHeadCount < lsHeadings.Count - 1; ColHeadCount++)
                {
                    string TextParameterCount = "TextParameter" + ColHeadCount;
                    CrystalDecisions.CrystalReports.Engine.TextObject TextParam = (CrystalDecisions.CrystalReports.Engine.TextObject)generalReport.Subreports[6].ReportDefinition.ReportObjects[TextParameterCount];
                    TextParam.Text = (string.Equals(lsHeadings[ColHeadCount].ToString(),"MDIntervalPeriod",StringComparison.OrdinalIgnoreCase))? "Integration Period": lsHeadings[ColHeadCount].ToString();
                    TextParam.ObjectFormat.EnableSuppress = false;
                }
            }
			//if (reportXSD.Tables["DTMLoadSurveyTable"].Rows.Count == 0)
			//{
			//    generalReport.SecDTMLoadSurvey.SectionFormat.EnableSuppress = true;
			//}
            if (reportXSD.Tables["TamperTable"].Rows.Count == 0)
            {
                generalReport.SecTamper.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PowerOnOffTamperTable"].Rows.Count == 0)
            {
                generalReport.SecPowerOnOffTamper.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["ProgrammingUpdatesTable"].Rows.Count == 0)
            {
                generalReport.SecProgrammingUpdates.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PhasorTable"].Rows.Count == 0)
            {
                generalReport.SecPhasor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["RTCUpdatesTable"].Rows.Count == 0)
            {
                generalReport.SecRTCUpdates.SectionFormat.EnableSuppress = true;
            }
			if (reportXSD.Tables["FraudEnergyTable"].Rows.Count == 0)
			{
				generalReport.SecFraudEnergy.SectionFormat.EnableSuppress = true;
			}
            
			generalReport.SetDataSource(reportXSD);
			ObjRptForm.rptViewer.ReportSource = generalReport;
            Cursor.Current = Cursors.Default;
            ObjRptForm.rptViewer.Zoom(1);
            this.Hide();
            ObjRptForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }






        /*public static DataTable ImageTable(string ImageFile)
        {
            DataTable data = new DataTable();
            DataRow row;
            data.TableName = "Images";
            data.Columns.Add("img", System.Type.GetType("System.Byte[]"));
            FileStream fs = new FileStream(ImageFile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            row = data.NewRow();
            row[0] = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));
            data.Rows.Add(row);
            br = null;
            fs.Close();
            fs = null;
            return data;
        }*/

        public static byte[] ImageTable(string ImageFile)
        {
            FileStream fs = new FileStream(ImageFile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] imageData = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));

            br = null;
            fs.Close();
            fs = null;
            return imageData;
        }


        private void SMD_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls; //this.ChkgroupBox.Controls;
            Control.ControlCollection LoadSurveyCollection = this.groupBoxLoadSurvey.Controls;
            Control.ControlCollection TamperCollection = this.groupBoxOthers.Controls;

            if (SMD_SelectAll.Checked == true)
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = true;
                }
                foreach (Control C1 in TamperCollection)
                {
                    ((CheckBox)C1).Checked = true;
                }
                foreach (Control C2 in LoadSurveyCollection)
                {
                    if (C2.Text == "Load Survey")
                    {
                        ((CheckBox)C2).Checked = true;
                    }
                }
            }
            else
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = false;
                }
                foreach (Control C1 in TamperCollection)
                {
                    ((CheckBox)C1).Checked = false;
                }
                foreach (Control C2 in LoadSurveyCollection)
                {
                    if ((C2.Text == "Load Survey") || (C2.Text == "DTM Load Survey"))
                    {
                        ((CheckBox)C2).Checked = false;
                    }
                }
            }
        }

        bool ValidateForm()
        {
            bool boolCheck = false;

            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls; //this.ChkgroupBox.Controls;
            Control.ControlCollection LoadSurveyCollection = this.groupBoxLoadSurvey.Controls;
            Control.ControlCollection TamperCollection = this.groupBoxOthers.Controls;
            foreach (Control C in BillingCollection)
            {
                if (((CheckBox)C).Checked == true)
                {
                    boolCheck = true;
                }
                else
                {
                    foreach (Control C1 in TamperCollection)
                    {
                        if (((CheckBox)C1).Checked == true)
                        {
                            boolCheck = true;
                        }
                    }
                    foreach (Control C2 in LoadSurveyCollection)
                    {
                        if (C2.Text.Contains("Load Survey"))
                        {
                            if (((CheckBox)C2).Checked == true)
                            {
                                boolCheck = true;
                            }
                        }
                    }
                }
            }
            return boolCheck;
        }

        string GetEnergyColumnName(string ColName)
        {
            if (ColName == "Demand kW") return ColName = "Energy kWh";
            else if (ColName == "Demand kVA") return ColName = "Energy kVAh";
            else if (ColName == "Demand kVAr Lag" || ColName == "Demand kvar (lag)") return ColName = "Energy kvarh (lag)";
            else if (ColName == "Demand kVAr Lead" || ColName == "Demand kvar (lead)") return ColName = "Energy kvarh (lead)";
            else return null;

        }

        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            this.Close();
        }



        private DataTable GetFormatedtable(DataTable dt)
        {
            DataTable dtnew = new DataTable();
            DataColumn dc = new DataColumn();
            dc.ColumnName = "Parameters";
            dtnew.Columns.Add(dc);
            DataColumn dc1 = new DataColumn();

            dc1.ColumnName = "Values";


            dtnew.Columns.Add(dc1);
            foreach (DataColumn clm in dt.Columns)
            {
                DataRow dr = dtnew.NewRow();
                dr["Parameters"] = clm.ColumnName;
                dr["Values"] = (dt.Rows[0][clm.ColumnName]).ToString();
                dtnew.Rows.Add(dr);
            }
            return dtnew;
        }

        private PointF DegreesToXY(float degrees, float radius, Point origin)
        {
            PointF xy = new PointF();
            double radians = degrees * Math.PI / 180.0;
            xy.X = (float)Math.Cos(radians) * radius + origin.X;
            xy.Y = (float)Math.Sin(radians) * radius + origin.Y; //In Reverse direction sign should be +ve
            return xy;
        }

        private void SMD_chkLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLoadSurvey.Checked == true)
            {
                SMD_chkDTMLoadSurvey.Checked = false;
                SMD_chkDTMLoadSurvey.Enabled = false;
                SMD_rbtnLoadSurveyDemand.Enabled = true;
                SMD_rbtnLoadSurveyEnergy.Enabled = true;
            }
            else if (chkLoadSurvey.Checked == false)
            {
                SMD_chkDTMLoadSurvey.Enabled = true;
                //SMD_chkDTMLoadSurvey.Checked = true;
                //SMD_rbtnLoadSurveyDemand.Enabled = false;
                //SMD_rbtnLoadSurveyEnergy.Enabled = false;
            }
        }

        private void SMD_chkDTMLoadSurvey_CheckedChanged(object sender, EventArgs e)
        {
            if (SMD_chkDTMLoadSurvey.Checked == true)
            {
                chkLoadSurvey.Checked = false;
                chkLoadSurvey.Enabled = false;
                SMD_rbtnLoadSurveyDemand.Enabled = false;
                SMD_rbtnLoadSurveyEnergy.Enabled = false;
            }
            else
            {
                //SMD_chkLoadSurvey.Checked = true;
                chkLoadSurvey.Enabled = true;
                SMD_rbtnLoadSurveyDemand.Enabled = true;
                SMD_rbtnLoadSurveyEnergy.Enabled = true;
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            int errCount = 0;
            int selectedParams = 0;
            int showReport = 0;
            string[] message = new string[17];
            try
            {
                if (string.IsNullOrEmpty(ConfigInfo.ActiveMeterDataId))
                {
                    this.StatusMessage = "Please select a CAB file.";    
                    return;
                }

                if (ValidateForm() == false)
                {
                    this.StatusMessage = "Please select an option for viewing report.";
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();

                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));


                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetails(detailsDS);
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                        FillMeterID(meterIDDS);
                }

                if (chkGeneralReport.Checked == true)
                {
                    selectedParams++;
                    DataSet generalDS = new DataSet();
                    generalDS = ListGeneralData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (generalDS != null && generalDS.Tables[0].Rows.Count > 0)
                    {
                        FillGeneralXSD(generalDS);
                        showReport++;
                    }
                    else
                    {
                        message[0] = "General Report data is not available.";
                        errCount++;
                    }
                }
                if (chkInstantReport.Checked == true)
                {
                    selectedParams++;
                    DataSet instantDS = new DataSet();
                    instantDS = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (instantDS != null && instantDS.Tables[0].Rows.Count > 0)
                    {
                        FillInstantXSD(instantDS);
                        showReport++;
                    }
                    else
                    {
                        message[1] = "Instant Report data is not available.";
                        errCount++;
                    }
                }

                if (chkPhasorReport.Checked == true)
                {
                    selectedParams++;
                    DataSet phasorDS = new DataSet();
                    phasorDS = ListPhasorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                    PhasorDiagramForm phasorDiagramForm = new PhasorDiagramForm();
                    phasorDiagramForm.MeterDataID = ConfigInfo.ActiveMeterDataId;
                    phasorDiagramForm.ShowDialog();

                    if (phasorDS != null && phasorDS.Tables[0].Rows.Count > 0)
                    {

                        FillPhasorTable();
                        FillPhasorXSD(phasorDS);
                        //FillPhasorDigram(phasorDS);
                        showReport++;
                    }
                    else
                    {
                        message[2] = "Phasor Report data is not available.";
                        errCount++;
                    }
                }

                if (chkPowerOnHours.Checked == true)
                {
                    BillingBLL billingBLL = new BillingBLL();
                    selectedParams++;
                    DataSet powerOnHoursDS = new DataSet();
                    //powerOnHoursDS = ListPowerOnHoursData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                    powerOnHoursDS = billingBLL.GetPowerOnHour(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (powerOnHoursDS != null && powerOnHoursDS.Tables[0].Rows.Count > 0)
                    {
                        FillPowerOnHoursXSD(powerOnHoursDS);
                        showReport++;
                    }
                    else
                    {
                        message[3] = "Power on hours data is not available.";
                        errCount++;
                    }
                }                

                if (chkPowerFactor.Checked == true)
                {
                    selectedParams++;
                    DataSet powerFactorDS = new DataSet();
                    powerFactorDS = ListPowerFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (powerFactorDS != null && powerFactorDS.Tables[0].Rows.Count > 0)
                    {
                        FillPowerFactorXSD(powerFactorDS);
                        showReport++;
                    }
                    else
                    {
                        message[4] = "Power Factor data is not available.";
                        errCount++;
                    }

                    DataSet tariffPowerFactorDS = new DataSet();
                    tariffPowerFactorDS = GetTariffPF(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (tariffPowerFactorDS != null && tariffPowerFactorDS.Tables[0].Rows.Count > 0)
                        FillTariffPowerFactorXSD(tariffPowerFactorDS);

                }

                if (chkLoadFactor.Checked == true)
                {
                    selectedParams++;
                    DataSet loadFactorDS = new DataSet();
                    loadFactorDS = ListLoadFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (loadFactorDS != null && loadFactorDS.Tables[0].Rows.Count > 0)
                    {
                        FillLoadFactorXSD(loadFactorDS);
                        showReport++;
                    }
                    else
                    {
                        message[5] = "Load Factor data is not available.";
                        errCount++;
                    }
                }

                if (chkBillingTamperCounter.Checked == true)
                {
                    selectedParams++;
                    DataSet billingTamperCounterDS = new DataSet();
                    billingTamperCounterDS = ListBillingTamperCounterData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (billingTamperCounterDS != null && billingTamperCounterDS.Tables[0].Rows.Count > 0)
                    {
                        FillBillingTamperCounterXSD(billingTamperCounterDS);
                        showReport++;
                    }
                    else
                    {
                        message[6] = "Billing Tamper Counter data is not available.";
                        errCount++;
                    }
                }

                if (SMD_chkTamper.Checked == true)
                {
                    selectedParams++;
                    DataSet tamperDS = new DataSet();
                    DataSet tamperCounterDS = new DataSet();
                    DataSet tamperDetailsDset = new DataSet();
                    tamperCounterDS = ListTamperData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    tamperDS = ListTamperOccResData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    string tamperCounter = string.Empty;
                    string occurranceTime = string.Empty;
                    string restorationTime = string.Empty;
                    if (tamperDS != null && tamperDS.Tables[0].Rows.Count > 0)
                    {
                        showReport++;
                        foreach (DataRow drow in tamperDS.Tables[0].Rows)
                        {
                            foreach (DataRow row in tamperCounterDS.Tables[0].Rows)
                            {
                                if (Convert.ToString(drow["Description"]).ToLower().Trim().Replace(" ", "").Equals("onephaseneutralabsenttamper"))
                                    drow["Description"]=row["TamperType"] = "One Phase and Neutral Absent Tamper";
                                 if (row["TamperType"].ToString() == drow["Description"].ToString())
                                {
                                    tamperCounter = row["TamperCounter"].ToString();
                                    break;
                                }
                            }
                            string duration = Convert.ToString(drow["Duration (Days:HH:MM)"]);
                            occurranceTime = drow["Occurrence Time Stamp"].ToString();
                            restorationTime = drow["Restoration Time Stamp"].ToString();
                            tamperDetailsDset = GetTamperOccurRestoreDetail(Convert.ToInt16(drow["TamperSnapShot_ID"].ToString()), Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                            FillAllTamperXSD(tamperDetailsDset, drow["Description"].ToString(), tamperCounter, occurranceTime, restorationTime, duration);
                        }
                        //FillTamperXSD(tamperDS);
                    }
                    else
                    {
                        message[7] = "Tamper Report data is not available.";
                        errCount++;
                    }
                }
                if (chkMainEnergy.Checked == true)
                {
                    selectedParams++;
                    DataSet mainEnergyDS = new DataSet();
                    mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                    {
                        FillMainEnergyXSD(mainEnergyDS);
                        showReport++;
                    }
                    else
                    {
                        message[8] = "Main Energy data is not available.";
                        errCount++;
                    }
                }

                if (chkEnergyConsumption.Checked == true)
                {
                    selectedParams++;
                    DataSet mainEnergyDS = new DataSet();
                    mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                    {
                        FillEnergyConsumptionXSD(CommonBLL.ConvertMainEnergyConsumption(mainEnergyDS));
                        showReport++;
                    }
                    else
                    {
                        message[9] = "Energy Consumption data is not available.";
                        errCount++;
                    }
                }

                if (chkCTRatio.Checked == true)
                {
                    selectedParams++;
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                    {
                        FillCTRatioXSD(ctRatioDS);
                        showReport++;
                    }
                    else
                    {
                        message[10] = "Billing type data is not available.";
                        errCount++;
                    }
                }

                if (chkMaximumDemand.Checked == true)
                {
                    selectedParams++;
                    DataSet maximumDemandDS = new DataSet();
                    maximumDemandDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (maximumDemandDS != null && maximumDemandDS.Tables[0].Rows.Count > 0)
                    {
                        FillMaximumDemandXSD(maximumDemandDS);
                        showReport++;
                    }
                    else
                    {
                        message[11] = "Maximum Demand data is not available.";
                        errCount++;
                    }

                    DataSet TODMDDS = new DataSet();
                    for (int historyID = 0; historyID <= 12; historyID++)
                    {
                        TODMDDS = ListTODMDData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
                        if (TODMDDS != null && TODMDDS.Tables[0].Rows.Count > 0)
                            FillTODMDXSD(TODMDDS, historyID);
                    }
                }

                if (chkTODEnergy.Checked == true)
                {
                    int history = 0;
                    selectedParams++;
                    DataSet tariffEnergyDS = new DataSet();
                    for (int historyID = 0; historyID <= 12; historyID++)
                    {
                        tariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
                        if (tariffEnergyDS != null && tariffEnergyDS.Tables[0].Rows.Count > 0)
                        {
                            FillTariffEnergyXSD(tariffEnergyDS, historyID);
                            history++;
                        }
                    }
                    if (history == 0)
                    {
                        
                        message[12] = "TOD Energy data is not available.";
                        errCount++;
                    
                    }
                    else
                    {
                        showReport++;
                    }
                }

                if (chkBillingMechanism.Checked == true)
                {
                    selectedParams++;
                    DataSet ctRatioDS = new DataSet();
                    ctRatioDS = ListCTRatioData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (ctRatioDS != null && ctRatioDS.Tables[0].Rows.Count > 0)
                        FillBillingMechanismXSD(ctRatioDS);
                }


                if (chkTODConsumption.Checked == true)
                {
                    int TODConsumpt = 0;
                    selectedParams++;
                    DataSet currentTariffEnergyDS = new DataSet();
                    DataSet nextTariffEnergyDS = new DataSet();
                    for (int historyID = 0; historyID < 12; historyID++)
                    {
                        if (historyID == 0)
                            currentTariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID);
                        else
                            currentTariffEnergyDS = nextTariffEnergyDS;

                        nextTariffEnergyDS = ListTariffEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), historyID + 1);
                        if (nextTariffEnergyDS == null)
                            break;
                        if (currentTariffEnergyDS == null)
                            break;

                         if (currentTariffEnergyDS.Tables[0].Rows.Count > 0 && nextTariffEnergyDS.Tables[0].Rows.Count > 0)
                            {
                                FillTODEnergyConsumptionXSD(currentTariffEnergyDS, nextTariffEnergyDS, historyID);
                                TODConsumpt++;
                            }
                    }
                    if (TODConsumpt == 0)
                    {
                        message[13] = "TOD Consumption data is not available.";
                        errCount++;
                    }
                    else
                    {
                        showReport++;
                    }
                }

                //if ( == true)
                //{ 
                string val = String.Empty;
                if (chkLoadSurvey.Checked)
                {
                    if (SMD_rbtnLoadSurveyDemand.Checked)
                        val = "Demand";
                    else
                        val = "Energy";
                    selectedParams++;
                    DataSet loadSurveyDS = new DataSet();
                    loadSurveyDS = ListLoadSurveyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), val);

                    if (loadSurveyDS != null && loadSurveyDS.Tables[0].Rows.Count > 0)
                    {

                        FillLoadSurveyXSD(loadSurveyDS);
                        showReport++;
                    }


                    else
                    {

                        message[14] = "Load Survey data is not available.";
                        errCount++;
                    }
                }
        
                if (SMD_chkDTMLoadSurvey.Checked == true)
                {
                    DataSet dtmLoadSurveyDS = new DataSet();
                    dtmLoadSurveyDS = ListDTMLoadSurveyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (dtmLoadSurveyDS != null && dtmLoadSurveyDS.Tables[0].Rows.Count > 0)
                        FillDTMLoadSurveyXSD(dtmLoadSurveyDS);
                }

                if (chkTransactions.Checked == true)
                {
                    selectedParams++;
					DataSet transactionDS = new DataSet();
					transactionDS = ListTransactionData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (transactionDS != null && transactionDS.Tables[0].Rows.Count > 0)
                    {
                        FillTransactionXSD(transactionDS);
                        showReport++;
                    }
                    

					DataSet rtcUpdatesDS;

					int totalRTCUpdates = new RTCUpdateBLL().GetTotalRTCUpdates(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
					rtcUpdatesDS = ListRTCUpdatesData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (rtcUpdatesDS != null && rtcUpdatesDS.Tables[0].Rows.Count > 0)
                    {
                        FillRTCUpdatesXSD(rtcUpdatesDS, totalRTCUpdates);
                        showReport++;
                    }
                    

                    FraudEnergyEntity entity = ListFraudEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as FraudEnergyEntity;
                    if (entity != null)
                    {
                        FillFraudEnergyXSD(entity);
                        showReport++;
                    }
                    if (entity == null && transactionDS == null && rtcUpdatesDS == null)
                    {
                        message[15] = "Transactions data is not available.";
                        errCount++;
                    }
                    
                }
                //VBM - Diaply Dummy data for JDVVNL as asked by PDM.
                if (chkTouConfig.Checked && chkTouConfig.Visible)
                {
                    selectedParams++;
                    DataSet touDataSet = new DataSet();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("S.NO.", typeof(string));
                    dataTable.Columns.Add("Slot No.", typeof(string));
                    dataTable.Columns.Add("Zone Start Time(HH:MM)", typeof(string));
                    dataTable.Columns.Add("Zone End Time(HH:MM)", typeof(string));
                    dataTable.Columns.Add("Tariff Zone", typeof(string));
                    dataTable.Rows.Add("1", "1", "00:00", "05:00", "T5");
                    dataTable.Rows.Add("2", "2", "05:00", "06:00", "T1");
                    dataTable.Rows.Add("3", "3", "06:00", "09:00", "T2");
                    dataTable.Rows.Add("4", "4", "09:00", "18:00", "T3");
                    dataTable.Rows.Add("5", "5", "18:00", "23:00", "T4");
                    dataTable.Rows.Add("6", "6", "23:00", "24:00", "T5");
                    dataTable.AcceptChanges();
                    touDataSet.Tables.Add(dataTable);
                    touDataSet.AcceptChanges();
                    if (touDataSet != null && touDataSet.Tables[0].Rows.Count > 0)
                    {
                        FillTouXSD(touDataSet);
                        showReport++;
                    }
                    else
                    {
                        message[16] = "TOU configuration data is not available.";
                        errCount++;
                    }
                }
                //VBM - Diaply Dummy data for JDVVNL

                if (errCount == 0)
                    ShowReport(message);
                else
                {
                    if (errCount == selectedParams)
                    { MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    if (showReport > 0)
                    { ShowReport(message); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void FillPhasorTable()
        {
            //get the image file into a stream reader.
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
            filePath = filePath + "PrintPage.jpg";

            FileStream FilStr = new FileStream(filePath, FileMode.Open);
            BinaryReader BinRed = new BinaryReader(FilStr);
            //PhasorDiagramTable
            DataRow reportrow = reportXSD.Tables["PhasorDiagramTable"].NewRow();
            reportrow["Image"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
            reportXSD.Tables["PhasorDiagramTable"].Rows.Add(reportrow);
            FilStr.Close();
            BinRed.Close();
        }

        private void SelectDialog_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            //VBM - Display only dummy data in tou report for JDVVNL
            if (UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
            {
                chkTouConfig.Visible = true;               
            }
            else
            {
                chkTouConfig.Visible = false;
                chkTouConfig.Checked = false;
            }
            //VBM - Display only dummy data in tou report for JDVVNL
        }

        private void chkBilling_CheckedChanged(object sender, EventArgs e)
        {
            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls;
            bool allchecked = false;
            if (chkBilling.Checked == true)
            {
                foreach (Control C in BillingCollection)
                {
                    ((CheckBox)C).Checked = true;
                }
                
            }
            else
            {
                foreach (Control C in BillingCollection)
                {
                    if (C.Name != "chkBilling")
                    {
                        if (((CheckBox)C).Checked == true)
                        {
                            allchecked = true;
                        }
                        else
                        {
                            allchecked = false;
                            break;
                        }
                    }
                }
                if (allchecked)
                {
                    foreach (Control C in BillingCollection)
                    {
                        ((CheckBox)C).Checked = false;
                    }
                }
            }
        }

        private void OnCheckChanged(CheckBox chkbx)
        {
            Control.ControlCollection BillingCollection = this.groupBoxBilling.Controls;
            bool allBilling = false;
            if (!chkbx.Checked)
            {
                foreach (Control billingParam in BillingCollection)
                {
                    if (((CheckBox)billingParam).Checked)
                        allBilling = true;
                    else
                    {
                        allBilling = false;
                        break;
                    }
                }
                if (!allBilling)
                    chkBilling.Checked = false;
                else
                    chkBilling.Checked = true;
            }
            else
            {
                foreach (Control billingParam in BillingCollection)
                {
                    if (billingParam.Name != "chkBilling")
                    {
                        if (((CheckBox)billingParam).Checked)
                            allBilling = true;
                        else
                        {
                            allBilling = false;
                            break;
                        }
                    }
                }
                if (!allBilling)
                    chkBilling.Checked = false;
                else
                    chkBilling.Checked = true;
            }
        }

        private void chkGeneralReport_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkGeneralReport); 
        }

        private void chkInstantReport_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkInstantReport); 
        }

        private void chkPhasorReport_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkPhasorReport); 
        }

        private void chkMainEnergy_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkMainEnergy);
        }

        private void chkEnergyConsumption_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkEnergyConsumption);
        }

        private void chkTODEnergy_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkTODEnergy);
        }

        private void chkTODConsumption_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkTODConsumption);
        }

        private void chkMaximumDemand_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkMaximumDemand);
        }

        private void chkPowerFactor_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkPowerFactor);
        }

        private void chkPowerOnHours_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkPowerOnHours);
        }

        private void chkLoadFactor_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkLoadFactor);
        }

        private void chkBillingTamperCounter_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkBillingTamperCounter);
        }

        private void chkCTRatio_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckChanged(chkCTRatio);
        }


    }
}