using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Utility;
using System.Collections.ObjectModel;
using LTCTBLL;
using CABEntity;
using CAB.IECChannel.Programming;
using CABApplication;
using System.IO;

namespace CAB.UI
{
    public partial class MeterDataList : MdiChildForm
    {
        private BillingBLL billingBLL = new BillingBLL();
        private TariffBLL tariffBLL = new TariffBLL();
        TamperGeneralBLL tamperGeneralBLL = new TamperGeneralBLL();
        private string meterDataID;
        private string fileType;
        public Int64 fileUploadID;
        private bool isDTMLoadSurvey = false;
        string currTODReadResult;
        string futureTODReadResult;
        Control group = new Control();
        Control subGroup = new Control();
        DateTime FromDate;
        DateTime ToDate;
        string billingType;
        DateTime tmpDTPickFromDate, tmpDTPickToDate;
        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
        //Utility utility = new Utility();
        private const string NOTAPPLICABLE = "NA";
        public string MeterDataID
        {
            get
            {
                return meterDataID;
            }
            set
            {
                meterDataID = value;
            }
        }
        public string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                fileType = value;
            }
        }
        public MeterDataList()
        {
            dateInfo.ShortDatePattern = ConfigInfo.DateFormat(); //"dd/MM/yyyy";
            InitializeComponent();
        }

        public void MeterDataList_Load(object sender, EventArgs e)
        {
            try
            {
                //tabControlReport.TabPages.Remove(tabNamePlateDetail);                
                if (UtilityDetails.UtilityName == UtilityEntity.JDVVNL)
                {
                    label8.Visible = false;
                    label9.Visible = false;
                }

                this.Text = "Analysis Report";
                FillList();

                if (!string.IsNullOrEmpty(meterDataID))
                {
                    DataSet dataSet = new NamePlateDetailBLL().ListDataSet(Convert.ToInt32(meterDataID));
                    lgcNamePlateDetail.Data = dataSet;
                    if (dataSet != null)
                    {
                        lgcNamePlateDetail.SetWidth("Parameters", 270);
                        lgcNamePlateDetail.SetWidth("Values", 270);

                    }
                    lgcNamePlateDetail.HSrollBar();
                    // To remove tabs when utility is other than TNEB.
                    if (UtilityDetails.UtilityName == UtilityEntity.UGVCL || UtilityDetails.UtilityName == UtilityEntity.JDVVNL || UtilityDetails.UtilityName == UtilityEntity.PVVNL || UtilityDetails.UtilityName == UtilityEntity.WBEXPORTVCL)
                    {
                        tabControlReport.TabPages.Remove(tabHeaderInfo);
                        tabControlReport.TabPages.Remove(tabNamePlateDetail);
                        tabControlReport.TabPages.Remove(tabMeterConfigurations);

                    }
                    if (UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                    {
                        tabControlReport.TabPages.Remove(tabNamePlateDetail);

                        tabCtrlMeterConfiguration.TabPages.Remove(tabRS232);

                    }
                    //VBM to hide Power on hours in billingt
                    if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                    {
                        tabControl2.TabPages.Remove(tabPageBillingPowerOnHours);
                    }

                    dataSet = new GeneralBLL().GetMeterData(Convert.ToInt32(meterDataID));

                    lngGeneral.Data = dataSet;

                    // Added Not Applicable if parameter is not available in old utilities

                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drow in dataSet.Tables[0].Rows)
                        {
                            if (string.IsNullOrEmpty(drow["Value"].ToString()))
                            {

                                drow["Value"] = NOTAPPLICABLE;

                            }

                        }
                    }
                    if (dataSet != null)
                    {
                        lngGeneral.SetWidth("Descriptions", 270);
                        lngGeneral.SetWidth("Value", 180);
                        lngGeneral.SetWidth("Unit", 290);
                    }
                    lngGeneral.HSrollBar();
                    lngGeneral.IsSorting = false;
                    dataSet = new InstantPowerBLL().GetMeterData(Convert.ToInt32(meterDataID));
                    lngInstant.Data = dataSet;
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drow in dataSet.Tables[0].Rows)
                        {
                            if (string.IsNullOrEmpty(drow["Value"].ToString()))
                            {

                                drow["Value"] = NOTAPPLICABLE;

                            }

                        }
                    }
                    if (dataSet != null)
                    {
                        lngInstant.SetWidth("Descriptions", 200);
                        lngInstant.SetWidth("Value", 130);
                        lngInstant.SetWidth("Unit", 90);
                    }
                    lngInstant.HSrollBar();
                    lngInstant.IsSorting = false;
                    dataSet = billingBLL.GetMaximumDemand(Convert.ToInt32(meterDataID));
                    lngMaximumDemand.Data = dataSet;
                    lngMaximumDemand.SetWidth("Billing Date Time", 140);
                    lngMaximumDemand.SetWidth("MD1 Time Stamp", 140);
                    lngMaximumDemand.SetWidth("MD2 Time Stamp", 140);
                    lngMaximumDemand.HSrollBar();
                    lngMaximumDemand.IsSorting = false;
                    dataSet = billingBLL.GetCumulativeEnergy(Convert.ToInt32(meterDataID));
                    lngMainEnergy.Data = dataSet;

                    string noBilling = "---------";
                    int month1 = 0;
                    int month2 = 0;
                    dataSet = billingBLL.GetCumulativeEnergy(Convert.ToInt32(meterDataID));

                    // To calculate the billing type from billing data
                    billingType = String.Empty;
                    if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                    {
                        //
                        if (dataSet.Tables[0].Rows.Count == 1 || (dataSet.Tables[0].Rows.Count > 2 && dataSet.Tables[0].Rows[2][1].ToString() == noBilling))
                            billingType = "User Defined";

                        //If meter has only two billing history then it is not possible to find the billing type of last month
                        //As a generic approach this will be used as "User Defined".
                        else if (dataSet.Tables[0].Rows.Count == 2)
                        {
                            billingType = "User Defined";
                        }
                        else
                        {
                            if (dataSet.Tables[0].Rows[1][1].ToString() == noBilling)
                                billingType = "User Defined";
                            else
                            {
                                if (dataSet.Tables[0].Rows.Count > 2)
                                {
                                    month1 = Convert.ToInt16(Convert.ToDateTime(dataSet.Tables[0].Rows[1][1], dateInfo).Month);
                                    month2 = Convert.ToInt16(Convert.ToDateTime(dataSet.Tables[0].Rows[2][1], dateInfo).Month);

                                    if (month1 - month2 == 0)
                                        billingType = "User Defined";
                                    else if (month1 - month2 == 1)
                                        billingType = "Monthly";
                                    else if (month1 % 2 == 0)
                                        billingType = "Even";
                                    else
                                        billingType = "Odd";
                                }
                            }
                        }
                    }
                    else
                        billingType = "User Defined";
                    /////////////////////////////////////////////////////
                    if (dataSet != null)
                    {
                        lngMainEnergy.SetWidth("History", 90);
                        lngMainEnergy.SetWidth("Billing DateTime", 140);
                        lngMainEnergy.SetWidth("kWh", 80);
                        lngMainEnergy.SetWidth("kVAh", 80);
                        lngMainEnergy.SetWidth("kvarh (Lag)", 110);
                        lngMainEnergy.SetWidth("kvarh (Lag) Time Stamp", 220);

                    }
                    lngMainEnergy.HSrollBar();
                    lngMainEnergy.IsSorting = false;
                    dataSet = billingBLL.GetCumulativeEnergyCalculated(Convert.ToInt32(meterDataID));
                    lngBillingEnergyConsumption.Data = dataSet;
                    if (dataSet != null)
                    {
                        lngBillingEnergyConsumption.SetWidth("History", 140);
                        lngBillingEnergyConsumption.SetWidth("kWh", 80);
                        lngBillingEnergyConsumption.SetWidth("kVAh", 80);
                        lngBillingEnergyConsumption.SetWidth("kvarh (Lag)", 110);
                        lngBillingEnergyConsumption.SetWidth("kvarh (Lag) Time Stamp", 220);
                    }
                    lngBillingEnergyConsumption.HSrollBar();
                    lngBillingEnergyConsumption.IsSorting = false;
                    lngLoadFactor.Data = billingBLL.GetLoadFactor(Convert.ToInt32(meterDataID));
                    lngLoadFactor.HSrollBar();
                    lngLoadFactor.IsSorting = false;
                    dataSet = billingBLL.GetAveragePowerFactor(Convert.ToInt32(meterDataID));
                    lngAveragePowerFactor.Data = dataSet;
                    if (dataSet != null)
                    {
                        lngAveragePowerFactor.SetWidth("History", 90);
                        lngAveragePowerFactor.SetWidth("Values", 150);
                    }
                    lngAveragePowerFactor.HSrollBar();
                    lngAveragePowerFactor.IsSorting = false;
                    dataSet = billingBLL.GetPowerOnHour(Convert.ToInt32(meterDataID));
                    lngPowerOnHour.Data = dataSet;
                    if (dataSet != null)
                    {
                        lngPowerOnHour.SetWidth("History", 90);
                        lngPowerOnHour.SetWidth("Values (HH:MM)", 150);
                    }
                    lngPowerOnHour.HSrollBar();
                    lngPowerOnHour.IsSorting = false;
                    lngCTRatio.Data = billingBLL.GetCTRatio(Convert.ToInt32(meterDataID));
                    lngCTRatio.SetEqualWidth();
                    lngCTRatio.HSrollBar();
                    lngCTRatio.IsSorting = false;
                    FraudEnergyBLL fraudEnergyBLL = new FraudEnergyBLL();
                    DataSet farudData = fraudEnergyBLL.GetFraudEnergyDataSet(Convert.ToInt32(meterDataID));
                    lngFraudEnergy.Data = farudData;
                    if (farudData != null && farudData.Tables[0].Rows.Count > 0)
                    {
                        for (int rowCount = 0; rowCount < farudData.Tables[0].Rows.Count;rowCount++ )
                        {
                            if (farudData.Tables[0].Rows[rowCount]["Value"].Equals("----"))
                            {
                                farudData.Tables[0].Rows.Remove(farudData.Tables[0].Rows[rowCount]);
                                rowCount--;
                            }                            
                        }
                        farudData.Tables[0].AcceptChanges();
                    }
                    if (farudData != null)
                    {
                        lngFraudEnergy.SetWidth("Descriptions", 250);
                        lngFraudEnergy.SetWidth("Value", 150);
                        lngFraudEnergy.SetWidth("Unit", 90);
                    }
                    lngFraudEnergy.HSrollBar();
                    lngFraudEnergy.IsSorting = false;

                    ProgrammingBLL programmingBLL = new ProgrammingBLL();

                    txtTotalProgrammingUpdates.Text = programmingBLL.GetTotalProgrammingUpdates(Convert.ToInt32(meterDataID)).ToString();
                    DataSet programmingData = programmingBLL.GetProgrammingList(Convert.ToInt64(meterDataID));
                    lngProgrammingUpdate.Data = programmingData;
                    lngProgrammingUpdate.HiddenColumn = "TotalProgrammingUpdates";
                    lngProgrammingUpdate.SetEqualWidth();
                    lngProgrammingUpdate.RefreshGrid();
                    lngProgrammingUpdate.IsSorting = false;
                    lngProgrammingUpdate.HSrollBar();
                    lngProgrammingUpdate.SetWidth("UpdateSequence", 150);
                    lngProgrammingUpdate.SetWidth("LastTimestamp", 150);
                    for (int paramIndex = 1; paramIndex <= 19; paramIndex++)
                        lngProgrammingUpdate.SetWidth("Description" + paramIndex, 150);

                    RTCUpdateBLL rTCUpdateBLL = new RTCUpdateBLL();
                    txtrtcUpdates.Text = rTCUpdateBLL.GetTotalRTCUpdates(Convert.ToInt32(meterDataID)).ToString();
                    lngRTCUpdate.Data = rTCUpdateBLL.GetRTCUpdateList(Convert.ToInt32(meterDataID));
                    lngRTCUpdate.SetEqualWidth();
                    lngRTCUpdate.HSrollBar();
                    lngRTCUpdate.IsSorting = false;
                    PhasorEntity phasorEntity = (PhasorEntity)new PhasorBLL().GetPhasorDataEntity(Convert.ToInt32(meterDataID));
                    if (phasorEntity != null)
                    {
                        if (string.IsNullOrEmpty(phasorEntity.PhaseSequence) || phasorEntity.PhaseSequence.ToUpper() != "CORRECT")
                        {
                            lbkNoDataFound.Visible = true;
                            lbkNoDataFound.Text = "Phase Sequence is not correct. Phasor can not be shown";
                            lngPhasorDiagram.Visible = false;
                        }
                        else
                        {
                            lbkNoDataFound.Visible = false;
                            lngPhasorDiagram.Visible = true;
                            lngPhasorDiagram.PhasorData = phasorEntity;
                        }
                    }

                    lngPhasorData.Data = new PhasorBLL().ListDataSet(Convert.ToInt32(meterDataID));
                    if (lngPhasorDiagram.PhasorData != null)
                    {
                        lngPhasorData.SetWidth("Parameters", 170);
                        lngPhasorData.SetWidth("Values", 80);
                    }
                    lngPhasorData.HSrollBar();
                    lngPhasorData.IsSorting = false;
                    long lsFromDate = DateUtility.DateTimeToLong(System.DateTime.Now.Date);
                    long lsToDate = DateUtility.DateTimeToLong(System.DateTime.Now.Date);
                    LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
                    DataSet dateData = loadSurveyBLL.GetDates(Convert.ToInt64(meterDataID));
                    tmpDTPickFromDate = new DateTime();
                    tmpDTPickToDate = new DateTime();
                    if (dateData != null)
                    {
                        if (dateData.Tables.Count != 0)
                        {
                            if (dateData.Tables[0].Rows.Count != 0)
                            {
                                lsFromDate = Convert.ToInt64(dateData.Tables[0].Rows[0][0]);
                                lsToDate = Convert.ToInt64(dateData.Tables[0].Rows[dateData.Tables[0].Rows.Count - 1][0]);
                            }
                        }
                    }
                    if (lsFromDate == 0 && lsToDate == 0)
                    {
                        DTMLoadSurveyBLL dtmLoadSurveyBLL = new DTMLoadSurveyBLL();
                        lsFromDate = dtmLoadSurveyBLL.GetFromDate(Convert.ToInt64(meterDataID));
                        lsToDate = dtmLoadSurveyBLL.GetToDate(Convert.ToInt64(meterDataID));
                        isDTMLoadSurvey = true;
                    }
                    if (lsFromDate != 0)
                    {
                        dtpFromDate.Value = DateUtility.LongToDateTime(lsFromDate);
                        FromDate = dtpFromDate.Value;
                        tmpDTPickFromDate = dtpFromDate.Value;
                    }


                    if (lsToDate != 0)
                    {
                        dtpToDate.Value = DateUtility.LongToDateTime(lsToDate);
                        ToDate = dtpToDate.Value;
                        tmpDTPickToDate = dtpToDate.Value;
                    }

                    dataSet = new DTMDailyProfileBLL().ListData(Convert.ToInt64(meterDataID));

                    dGVDailyProfile.Data = CommonBLL.ConvertDTMDailyProfileRowToColumn(dataSet);
                    dGVDailyProfile.SetWidth("MD1 Time Stamp", 110);
                    dGVDailyProfile.SetWidth("MD2 Time Stamp", 110);
                    dGVDailyProfile.SetWidth("Daily MD1", 75);
                    dGVDailyProfile.SetWidth("Daily MD2", 75);
                    dGVDailyProfile.HSrollBar();
                    dGVDailyProfile.IsSorting = false;
                    DataSet headerInfoDataSet = new MeterDataHeaderInfoBLL().GetMeterDataHeaderInfoForAnalysisReport(Convert.ToInt32(meterDataID));
                    if (dataSet != null)
                    {
                        if (dataSet.Tables.Count > 0)
                            if (dataSet.Tables[0].Rows.Count > 0)
                                txtBoxAvailableDays.Text = dataSet.Tables[0].Rows.Count.ToString();
                    }

                    if (headerInfoDataSet != null && headerInfoDataSet.Tables.Count > 0)
                    {
                        /* GKG 28/01/2013 TANGEDCO ISSUE*/
                        ////To replace the Billing tpye from one calculated from billing data
                        //for (int index = 0; index <= headerInfoDataSet.Tables[0].Rows.Count - 1; index++)
                        //{
                        //    if (headerInfoDataSet.Tables[0].Rows[index].ItemArray[0].ToString() == "Type of Billing")
                        //    {
                        //        headerInfoDataSet.Tables[0].Rows[index][1] = billingType;
                        //    }
                        //}

                        /* GKG 28/01/2013 TANGEDCO ISSUE*/
                        ///////////////////////////////////////////////////////
                        lngHeaderInfo.Data = headerInfoDataSet;
                        if (headerInfoDataSet.Tables[0].Columns.Contains("Description"))
                        {
                            lngHeaderInfo.SetWidth("Description", 200);
                            lngHeaderInfo.SetWidth("Value", 250);

                        }
                        lngHeaderInfo.HSrollBar();
                    }

                    //Meter Configuration
                    MeterDataEntity meterDataEntity = (MeterDataEntity)new MeterDataBLL().GetDetailData(Convert.ToInt64(meterDataID));
                    bool ifMeterConfigDataFound = false;
                    tabCtrlMeterConfiguration.Visible = true;
                    lblMeterConfigIfDataFound.Text = "";
                    string rtcValue = new RTCBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));
                    if (rtcValue != null)
                    {
                        DisplayRTC(rtcValue);
                        ifMeterConfigDataFound = true;
                    }

                    DisplayMDWithIP(new MDWithIPBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID)), ref ifMeterConfigDataFound);
                    DisplaykvarSelection(new kvarSelectionBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID)), ref ifMeterConfigDataFound);

                    BillingResetEntity billingresetentity = new BillingResetBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));

                    DisplayModeOfbilling(billingresetentity);
                    DisplayBillingPeriod(billingresetentity);
                    Control group = this.billingReset1.Controls["gbManual"];
                    ComboBox cmbResetLockoutDays = (ComboBox)group.Controls["cmbResetLockoutdays"];
                    cmbResetLockoutDays.SelectedIndex = getSelectedIndex(cmbResetLockoutDays, Convert.ToString(billingresetentity.ResetLockOutDays));
                    billingReset1.Enabled = false;

                    DailyLogEntity DailyLogEntity = new DailyLogBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));
                    DisplayDailyLog(DailyLogEntity);
                    dailyLog1.Enabled = false;

                    RS232LockEntity rs232LockEntity = new RS232BLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));
                    DisplayRS232(rs232LockEntity);

                    AutoLockEntity autoLockEntity = new AutoLockBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));
                    if (autoLockEntity.AutoLockStatus != null)
                    {
                        tabCtrlMeterConfiguration.TabPages["tabPageAutoLock"].Show();
                        DisplayAutoLock(autoLockEntity);
                    }
                    else
                    {
                        tabCtrlMeterConfiguration.TabPages.Remove(tabPageAutoLock);
                    }

                    Collection<Collection<string>> collDisplayParamaters = new DisplayParametersBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID));
                    if (collDisplayParamaters != null && collDisplayParamaters[0] != null && collDisplayParamaters[1] != null &&
                        collDisplayParamaters[2] != null && collDisplayParamaters[3] != null)
                    {
                        displayParameters.Controls[0].Controls[1].Controls["chkboxSelectAll"].Enabled = false;
                        displayParameters.Controls[0].Controls[1].Controls["btnUpScroll"].Enabled = false;
                        displayParameters.Controls[0].Controls[1].Controls["btnDownScroll"].Enabled = false;
                        for (int i = 0; i < 3; i++)
                        {
                            if (collDisplayParamaters[i].Count > 0)
                            {
                                SelectRowsInDataGridByDisplayParamaterReadOutput(collDisplayParamaters[i], i == 0 ? DisplayParameter.PushMode : i == 1 ? DisplayParameter.ScrollMode : DisplayParameter.HighResolutionMode);
                                ifMeterConfigDataFound = true;
                            }
                            else
                                disableDataGrids(i == 0 ? DisplayParameter.PushMode : i == 1 ? DisplayParameter.ScrollMode : DisplayParameter.HighResolutionMode);

                        }
                        if (collDisplayParamaters[3].Count > 0)
                        { SetDisplayTimeOutsByDisplayParamaterReadOutput(collDisplayParamaters[3]); ifMeterConfigDataFound = true; }
                        else
                        {
                            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Enabled = false;
                            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Enabled = false;
                            ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Enabled = false;
                            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
                        }

                    }

                    ProcessTOUData(new TODBLL().GetData(meterDataEntity.MeterID, fileUploadID, Convert.ToInt32(meterDataID)), ref ifMeterConfigDataFound);


                    tabControlReport.SelectedIndex = 0;

                    if (ifMeterConfigDataFound == false)
                    {
                        tabCtrlMeterConfiguration.Visible = false;
                        lblMeterConfigIfDataFound.Text = "No Data Found.";
                    }
                }
                //NO need to display tamper counter in billing for PUMA meters as data is not provided by f/w
                if (FileType == "DLMS")
                {
                    tabControl2.TabPages.Remove(tabPageBillingTamperCounter);
                }

            }
            catch (Exception)
            {

            }
        }

        #region BillingReset

        // To display Mode of billing
        private void DisplayModeOfbilling(BillingResetEntity billingresetentity)
        {
            Control group = this.billingReset1.Controls["gbAutoMode"];
            ComboBox cmbModeofBilling = (ComboBox)group.Controls["cmbModeofBilling"];
            cmbModeofBilling.SelectedIndex = getSelectedIndexModeOfBilling((int)(billingresetentity.ModeOfBilling));
            ComboBox cmbSelectDay = (ComboBox)group.Controls["cmbSelectDay"];
            cmbSelectDay.SelectedIndex = getSelectedIndexBillingReset(cmbSelectDay, Convert.ToString(billingresetentity.Day));
            ComboBox cmbSelectHour = (ComboBox)group.Controls["cmbSelectHour"];
            cmbSelectHour.SelectedIndex = getSelectedIndexBillingReset(cmbSelectHour, Convert.ToString(billingresetentity.Hours));
            ComboBox cmbSelectMinutes = (ComboBox)group.Controls["cmbSelectMinutes"];
            cmbSelectMinutes.SelectedIndex = getSelectedIndexBillingReset(cmbSelectMinutes, Convert.ToString(billingresetentity.Minutes));
            //RadioButton rdbMonthly = (RadioButton)group.Controls["rbtnMonthly"];
            //RadioButton rdbOddMonth = (RadioButton)group.Controls["rbtnOddMonth"];
            //RadioButton rdbEvenMonth = (RadioButton)group.Controls["rbtnEvenMonth"];
        }
        // For Billing Period

        private void DisplayBillingPeriod(BillingResetEntity billingresetentity)
        {
            Control group = this.billingReset1.Controls["gbAutoMode"];
            RadioButton rdbMonthly = (RadioButton)group.Controls["rbtnMonthly"];
            RadioButton rdbOddMonth = (RadioButton)group.Controls["rbtnOddMonth"];
            RadioButton rdbEvenMonth = (RadioButton)group.Controls["rbtnEvenMonth"];
            if ((int)(billingresetentity.BillingPeriod) == 0)
                rdbMonthly.Checked = true;
            if ((int)(billingresetentity.BillingPeriod) == 1)
                rdbOddMonth.Checked = true;
            if ((int)(billingresetentity.BillingPeriod) == 2)
                rdbEvenMonth.Checked = true;

        }
        private int getSelectedIndexModeOfBilling(int ModeofBilling)
        {
            if (ModeofBilling == 0)
                return ModeofBilling;
            if (ModeofBilling == 1)
                return ModeofBilling;

            return -1;
        }

        private int getSelectedIndexBillingReset(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString().Length == 1)
                    {
                        string s = cmb.Items[i].ToString().PadLeft(2, '0');

                        if (s == strValue)
                        {
                            return i;
                        }
                    }
                    else if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }

                }
            }

            return -1;
        }

        #endregion

        #region DisplayDailyLog
        //To display the daily log parameters
        private void DisplayDailyLog(DailyLogEntity DailyLogEntity)
        {
            Control group = this.dailyLog1.Controls["gbDailyLog"];
            CheckBox chk1 = (CheckBox)group.Controls["chkSelectAll"];
            CheckBox chk = (CheckBox)group.Controls["chkCumulativeKwh"];
            if (DailyLogEntity.CumulativeKWh == "1")
                chk.Checked = true;
            CheckBox chk2 = (CheckBox)group.Controls["chkCumulativeKVARhLag"];
            if (DailyLogEntity.CumulativeKVARhLag == "1")
                chk2.Checked = true;
            CheckBox chk3 = (CheckBox)group.Controls["chkCumulativeKVARhLead"];
            if (DailyLogEntity.CumulativeKVARhLead == "1")
                chk3.Checked = true;
            CheckBox chk4 = (CheckBox)group.Controls["chkCumulativeKVAh"];
            if (DailyLogEntity.CumulativeKVAh == "1")
                chk4.Checked = true;
            CheckBox chk5 = (CheckBox)group.Controls["chkDailyMD1"];
            if (DailyLogEntity.DailyMD1 == "1")
                chk5.Checked = true;
            CheckBox chk6 = (CheckBox)group.Controls["chkDailyMD2"];
            if (DailyLogEntity.DailyMD2 == "1")
                chk6.Checked = true;
            if (chk.Checked == true && chk2.Checked == true && chk3.Checked == true && chk4.Checked == true && chk5.Checked == true && chk6.Checked == true)
            {
                chk1.Checked = true;
            }


        }

        #endregion

        #region Display RS232
        private void DisplayRS232(RS232LockEntity rs232LockEntity)
        {
            if (rs232LockEntity.LockStatus == "Locked")
            {
                chkLockRS232Port.Checked = true;
            }
            else
                chkLockRS232Port.Checked = false;
        }
        #endregion

        #region Display AutoLock
        private void DisplayAutoLock(AutoLockEntity autoLockEntity)
        {
            if (autoLockEntity.AutoLockStatus == "NotLocked")
            {
                rdbAutoUnlock.Checked = true;
            }
            else if (autoLockEntity.AutoLockStatus == "Locked")
            {
                rdbAutoLock.Checked = true;
            }

        }
        #endregion

        private int getSelectedIndex(ComboBox cmb, string strValue)
        {
            int i = 0;
            if (cmb.Items.Count > 0)
            {
                for (i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString() == strValue)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #region Set DisplayTimeOuts by Display ParamaterREad Output
        private void SetDisplayTimeOutsByDisplayParamaterReadOutput(Collection<string> displayTimeOuts)
        {
            string[] displayTimeOutValues;
            int tmp;
            for (int i = 0; i < displayTimeOuts.Count; i++)
            {
                displayTimeOutValues = displayTimeOuts[i].Split('/');
                if (displayTimeOutValues[0] == "Scroll Time Out")
                    displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Text = displayTimeOutValues[1];
                else if (displayTimeOutValues[0] == "Push Time Out")
                    displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Text = displayTimeOutValues[1];
                else
                {
                    if (displayTimeOutValues[0] == "Auto Scroll Resume Time")
                    {
                        ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = true;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = displayTimeOutValues[1];
                    }
                    else
                    {
                        ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Checked = false;
                        displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Text = "";
                    }
                }
            }
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtScrollTime"].Enabled = false;
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtPushTimeout"].Enabled = false;
            ((CheckBox)(displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["chkBoxAutoScrollTime"])).Enabled = false;
            displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabDisplayTimeouts"].Controls[0].Controls["txtAutoScrollTime"].Enabled = false;
        }
        #endregion

        private void disableDataGrids(DisplayParameter displayParameter)
        {
            DataGridView dataGridView = new DataGridView();
            tabControlReport.SelectedIndex = 9;
            tabCtrlMeterConfiguration.SelectedIndex = 2;
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
            {
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
            }
            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 1;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            }
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 2;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null)
                    ((DataGridViewCheckBoxCell)dataGridView[0, i]).Value = false;
            }
            int chkboxColIndex = 0, descColIndex = 0;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == "Description")
                    descColIndex = i;
                if (dataGridView.Columns[i].Name == "colInclude")
                    chkboxColIndex = i;

            }
            for (int n = 0; n < dataGridView.Rows.Count; n++)
            {
                dataGridView.Rows[n].ReadOnly = true;
            }

            dataGridView.EndEdit();
            tabCtrlMeterConfiguration.SelectedIndex = 0;
            ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 0;
        }
        #region SelectRowsInDataGridByDisplayParamaterReadOutput
        private void SelectRowsInDataGridByDisplayParamaterReadOutput(Collection<string> paramatersToSelect, DisplayParameter displayParameter)
        {
            DataGridView dataGridView = new DataGridView();
            if (UtilityDetails.UtilityName == UtilityEntity.TNEB)
                tabControlReport.SelectedIndex = 11;
            else
                tabControlReport.SelectedIndex = 10;
            tabCtrlMeterConfiguration.SelectedIndex = 2;
            //Get dataGridview for PushMode display parameter.
            if (displayParameter == DisplayParameter.PushMode)
            {
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabPushButton"].Controls["dgridPushDisplayParams"];
            }
            //Get dataGridview for ScrollMode display parameter.
            else if (displayParameter == DisplayParameter.ScrollMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 1;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabScrollButton"].Controls["dgridScrollDisplayParams"];
            }
            //Get dataGridview for HighResolutionMode display parameter.
            else if (displayParameter == DisplayParameter.HighResolutionMode)
            {
                ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 2;
                dataGridView = (DataGridView)displayParameters.Controls[0].Controls["tabControlDisplayParams"].Controls["tabHighResolution"].Controls["dgridHighResolution"];
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell cell = dataGridView[0, i] as DataGridViewCheckBoxCell;
                if (cell != null)
                    ((DataGridViewCheckBoxCell)dataGridView[0, i]).Value = false;
            }
            int chkboxColIndex = 0, descColIndex = 0;
            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                if (dataGridView.Columns[i].Name == "Description")
                    descColIndex = i;
                if (dataGridView.Columns[i].Name == "colInclude")
                    chkboxColIndex = i;

            }
            #region Code to bind paramaters in data grid by Priority.
            for (int i = 0; i < paramatersToSelect.Count; i++)
            {//Get paramater name in Current row.
                try
                {
                    DataGridViewTextBoxCell txtcell = dataGridView[descColIndex, i] as DataGridViewTextBoxCell;
                    if (txtcell != null && txtcell.Value != null)
                    {//If paramater name in current row is same as the paramater to write then no shuffling is required.
                        //But if paramater name in current row is different to paramater to write
                        //then the paramater in the current row is shifted to the old position of paramater to be written in current row.
                        if (txtcell.Value.ToString() != paramatersToSelect[i])
                        {//if paramater name is not same as paramater name in current row then find the 
                            //row position of old occurence of paramater to be written in current row.
                            for (int j = i; j < dataGridView.Rows.Count; j++)
                            {
                                DataGridViewTextBoxCell txtcell2 = dataGridView[descColIndex, j] as DataGridViewTextBoxCell;
                                //if this condition is true then we get the old position of the paramater to be written in current row.
                                if (txtcell2 != null && txtcell2.Value != null && txtcell2.Value.ToString() == paramatersToSelect[i])
                                {//move the paramater in current row to this new position.
                                    txtcell2.Value = txtcell.Value;
                                    DataGridViewCheckBoxCell cell2 = dataGridView[chkboxColIndex, j] as DataGridViewCheckBoxCell;
                                    if (cell2 != null)
                                        ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, j]).Value = false;
                                }
                            }
                        }//Set the paramater to be written to cell in the current row.
                        txtcell.Value = paramatersToSelect[i];
                        DataGridViewCheckBoxCell cell = dataGridView[chkboxColIndex, i] as DataGridViewCheckBoxCell;
                        if (cell != null)
                            ((DataGridViewCheckBoxCell)dataGridView[chkboxColIndex, i]).Value = true;
                        //for (int n = 0; n < dataGridView.Rows.Count; n++)
                        //{
                        //    dataGridView.Rows[n].ReadOnly = true;
                        //}
                    }
                }
                catch
                {

                }
            }
            for (int n = 0; n < dataGridView.Rows.Count; n++)
            {
                dataGridView.Rows[n].ReadOnly = true;
            }
            #endregion
            dataGridView.EndEdit();
            tabCtrlMeterConfiguration.SelectedIndex = 0;
            ((TabControl)displayParameters.Controls[0].Controls["tabControlDisplayParams"]).SelectedIndex = 0;
        }
        #endregion

        #region Display RTC
        private void DisplayRTC(string rtc)
        {
            if (rtc != null)
            {
                rtcCtrl.Controls[0].Controls["txtRTC"].Text = rtc;
                rtcCtrl.Controls[0].Controls["txtRTC"].Enabled = false;
                DataGridView dataGridView = (DataGridView)rtcCtrl.Controls[0].Controls["dGridRTC"];
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                DataGridViewCell srlNoCell = new DataGridViewTextBoxCell();
                srlNoCell.Value = (dataGridView.Rows.Count + 1).ToString();
                dataGridViewRow.Cells.Add(srlNoCell);
                DataGridViewCell rtcDatetimeCell = new DataGridViewTextBoxCell();
                rtcDatetimeCell.Value = rtc;
                dataGridViewRow.Cells.Add(rtcDatetimeCell);
                dataGridView.Rows.Add(dataGridViewRow);
            }
        }
        #endregion

        private void DisplayMDWithIP(MeterConfigurationsNFEntity meterConfig, ref bool ifMeterConfigDataFound)
        {
            if (meterConfig.mdWithIPEntity != null)
            {

                MeterConfigurations meterConfigurations = new MeterConfigurations();
                if (meterConfig.mdWithIPEntity.KWDemandType != null)
                {
                    cmbDemandType.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandType, meterConfig.mdWithIPEntity.KWDemandType);
                    ifMeterConfigDataFound = true;
                }

                cmbDemandInterval.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandInterval, Convert.ToString(meterConfig.mdWithIPEntity.KWInterval));

                if (meterConfigurations.getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval)) != -1)
                {
                    cmbDemandSubInterlavTime.SelectedIndex = meterConfigurations.getSelectedIndex(cmbDemandSubInterlavTime, Convert.ToString(meterConfig.mdWithIPEntity.KWSubInterval));
                    ifMeterConfigDataFound = true;
                }
                else
                {
                    cmbDemandSubInterlavTime.SelectedIndex = -1;
                }

                grouBoxMDWithIP.Enabled = false;
            }
        }

        private void DisplaykvarSelection(MeterConfigurationsNFEntity meterConfig, ref bool ifMeterConfigDataFound)
        {
            if (meterConfig.kvarselectionEntity != null)
            {
                Control group = this.kvarSelection1.Controls["grpkvarSelection"];
                RadioButton rdb = (RadioButton)group.Controls["rdbLagOnly"];

                if (meterConfig.kvarselectionEntity.LagOnly == "1")
                { rdb.Checked = true; ifMeterConfigDataFound = true; }
                rdb = (RadioButton)group.Controls["rdbLagnLead"];

                if (meterConfig.kvarselectionEntity.LagandLead == "1")
                { rdb.Checked = true; ifMeterConfigDataFound = true; }

                group.Enabled = false;
            }
        }

        private void ProcessTOUData(string todData, ref bool ifMeterConfigDataFound)
        {
            if (todData != null && todData != string.Empty)
            {
                ifMeterConfigDataFound = true;
                rdbCurrentTOD.Checked = true;
                //dtPickerFutureActivationDate
                //tabTOD.Controls[2].Controls[0].Controls[1].Controls[6].Controls[1]
                tabTOD.Controls[2].Controls[0].Controls[1].Controls[6].Controls["dtPickerFutureActivationDate"].Enabled = false;
                tabTOD.Controls[2].Controls[0].Controls["grpButtons"].Controls["btnUpload"].Visible = false;
                tabTOD.Controls[2].Controls[0].Controls["grpButtons"].Controls["btnResetAll"].Visible = false;

                string touType = "";
                string todReadResult = "";
                currTODReadResult = "";
                futureTODReadResult = "";
                if (todData.StartsWith("DLMS"))
                {
                    futureTODReadResult = todData;
                    currTODReadResult = todData;
                }
                else
                {
                    currTODReadResult = todData.Substring(todData.IndexOf("/TU/"), todData.IndexOf("/FU/") - todData.IndexOf("/TU/"));
                    futureTODReadResult = todData.Substring(todData.IndexOf("/FU/"));
                }
                if (rdbFutureTOD.Checked)
                {
                    touType = "Future";
                    todReadResult = futureTODReadResult;
                }
                if (rdbCurrentTOD.Checked)
                {
                    touType = "Current";
                    todReadResult = currTODReadResult;
                }
                DisplayTOU(todReadResult, touType);
            }
            else
            {
                DisbaleDisplayControls();
            }
        }
        /// <summary>
        /// Disable display controls when no data in TOU .
        /// </summary>
        private void DisbaleDisplayControls()
        {
            tabTOD.Controls[2].Controls[0].Controls["grpButtons"].Controls["btnUpload"].Visible = false;
            tabTOD.Controls[2].Controls[0].Controls["grpButtons"].Controls["btnResetAll"].Visible = false;

            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();
            DataGridView[] dayAssignmentGrids = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];
            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Enabled = false;
            }
            foreach (DataGridView holidayGrid in holidayGrids)
            {
                holidayGrid.Enabled = false;
            }
            foreach (DataGridView dayAssignmentGrid in dayAssignmentGrids)
            {
                dayAssignmentGrid.Enabled = false;
            }
            foreach (DateTimePicker dtPickern in dtPickerCollection)
            {
                dtPickern.Enabled = false;
            }
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];
            gridActivation.Enabled = false;
            dtPickerFutureActivationDate.Enabled = false;
        }
      
        private void DisplayTOU(string touData, string touType)
        {
            group = new Control();
            int tableIndex = 0;
            int rowIndex = 0;
            DataSet ds = new DataSet();
            
            this.touOperation1.buttonclicked = true;
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            MeterConfigurations meterConfigurations = new MeterConfigurations();
            foreach (DataGridView seasonGrid in seasonGrids)
            {                
                if (seasonGrid.Columns.Count == 0)
                {
                    seasonGrid.Columns.Add(meterConfigurations.GetSNo());
                    seasonGrid.Columns.Add(meterConfigurations.GetRates());
                    seasonGrid.Columns.Add(meterConfigurations.GetStartHour());
                    seasonGrid.Columns.Add(meterConfigurations.GetStartMinute());
                }
            }

            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView holidayGrid in holidayGrids)
            {
                if (holidayGrid.Columns.Count == 0)
                {
                    holidayGrid.Columns.Add(meterConfigurations.GetSNo());
                    holidayGrid.Columns.Add(meterConfigurations.GetRates());
                    holidayGrid.Columns.Add(meterConfigurations.GetStartHour());
                    holidayGrid.Columns.Add(meterConfigurations.GetStartMinute());
                }
            }

            DataGridView[] dayAssignmentGrids = GetAssignmentGridCollection();
            DateTimePicker[] dtPickerCollection = GetActivationDateCollection();
            ds = ProgrammingCommon.DisplayTOUData(touData, touType);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                DisbaleDisplayControls();
                //this.StatusMessage = "Invalid TOU";
                return;
            }

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                seasonGrid.Rows.Clear();
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    seasonGrid.Rows.Add();
                    seasonGrid.Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    seasonGrid.Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                tableIndex++;
                seasonGrid.Enabled = false;
            }

            for (int holidayIndex = 0; holidayIndex <= holidayGrids.GetUpperBound(0); holidayIndex++)
            {
                holidayGrids[holidayIndex].Rows.Clear();               
                for (rowIndex = 0; rowIndex < ds.Tables[tableIndex].Rows.Count; rowIndex++)
                {
                    holidayGrids[holidayIndex].Rows.Add();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["SNo."].Value = ds.Tables[tableIndex].Rows[rowIndex]["S No"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Hour"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Hour"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Start Minute"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Start Minute"].ToString();
                    holidayGrids[holidayIndex].Rows[rowIndex].Cells["Rate"].Value = ds.Tables[tableIndex].Rows[rowIndex]["Rate"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                {
                    DateTime dt;
                    if (!string.IsNullOrEmpty(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString()))
                        if (DateTime.TryParse(ds.Tables[tableIndex].Rows[0]["Activation Date"].ToString(), new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out dt))
                        {
                            dtPickerCollection[holidayIndex].Value = dt;
                            dtPickerCollection[holidayIndex].CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                        }
                }
                tableIndex++;
                holidayGrids[holidayIndex].Enabled = false;
                dtPickerCollection[holidayIndex].Enabled = false;
            }


            for (int dayAssignment = 0; dayAssignment <= dayAssignmentGrids.GetUpperBound(0); dayAssignment++)
            {
                rowIndex = 0;
                dayAssignmentGrids[dayAssignment].Rows.Clear();
                foreach (DataRow row in ds.Tables[tableIndex].Rows)
                {
                    dayAssignmentGrids[dayAssignment].Rows.Add();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[0].Value = row["Day"].ToString();
                    dayAssignmentGrids[dayAssignment].Rows[rowIndex].Cells[1].Value = row["Day Table"].ToString();
                    rowIndex++;
                }
                tableIndex++;
                dayAssignmentGrids[dayAssignment].Enabled = false;
            }

            rowIndex = 0;
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"].Controls["tbPgActivationDate"];
            DateTimePicker dtPickerFutureActivationDate = (DateTimePicker)group.Controls["dtPickerFutureActivationDate"];

            if (touType == "Future")
            {
                dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
                dtPickerFutureActivationDate.Value = ProgrammingCommon.GetDate(ProgrammingCommon.futureActivationDate, true);
            }
            DataGridView gridActivation = (DataGridView)group.Controls["gridActivation"];
            if (ds.Tables[tableIndex].Rows.Count > 0)
            {
                gridActivation.Rows.Clear();
                foreach (DataRow row in ds.Tables[tableIndex].Rows)
                {
                    gridActivation.Rows.Add();
                    gridActivation.Rows[rowIndex].Cells[0].Value = row["Season Activation Date"].ToString();
                    gridActivation.Rows[rowIndex].Cells[1].Value = Convert.ToInt32(row["Season Number"].ToString()).ToString();
                    rowIndex++;
                }
            }
            gridActivation.Enabled = false;
        }

       

        private void rdbCurrentTOD_CheckedChanged(object sender, EventArgs e)
        {
            if (currTODReadResult != "" && currTODReadResult != null && futureTODReadResult != null && futureTODReadResult != "")
            {
                if (rdbCurrentTOD.Checked)
                    DisplayTOU(currTODReadResult, "Current");
                else
                    DisplayTOU(futureTODReadResult, "Future");
            }
        }

        private void rdbFutureTOD_CheckedChanged(object sender, EventArgs e)
        {
            if (currTODReadResult != "" && futureTODReadResult != "")
            {
                if (rdbFutureTOD.Checked)
                    DisplayTOU(futureTODReadResult, "Future");
                else
                    DisplayTOU(currTODReadResult, "Current");
            }
        }

        private DataGridView[] GetSeasonGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgS1"].Controls["tbSeason1"];
            DataGridView gridS1Day1 = (DataGridView)subGroup.Controls["tbPgS1D1"].Controls["gridS1Day1"];
            DataGridView gridS1Day2 = (DataGridView)subGroup.Controls["tbPgS1D2"].Controls["gridS1Day2"];
            DataGridView gridS1Day3 = (DataGridView)subGroup.Controls["tbPgS1D3"].Controls["gridS1Day3"];
            DataGridView gridS1Day4 = (DataGridView)subGroup.Controls["tbPgS1D4"].Controls["gridS1Day4"];
            DataGridView gridS1Day5 = (DataGridView)subGroup.Controls["tbPgS1D5"].Controls["gridS1Day5"];
            DataGridView gridS1Day6 = (DataGridView)subGroup.Controls["tbPgS1D6"].Controls["gridS1Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS2"].Controls["tbSeason2"];
            DataGridView gridS2Day1 = (DataGridView)subGroup.Controls["tbPgS2D1"].Controls["gridS2Day1"];
            DataGridView gridS2Day2 = (DataGridView)subGroup.Controls["tbPgS2D2"].Controls["gridS2Day2"];
            DataGridView gridS2Day3 = (DataGridView)subGroup.Controls["tbPgS2D3"].Controls["gridS2Day3"];
            DataGridView gridS2Day4 = (DataGridView)subGroup.Controls["tbPgS2D4"].Controls["gridS2Day4"];
            DataGridView gridS2Day5 = (DataGridView)subGroup.Controls["tbPgS2D5"].Controls["gridS2Day5"];
            DataGridView gridS2Day6 = (DataGridView)subGroup.Controls["tbPgS2D6"].Controls["gridS2Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS3"].Controls["tbSeason3"];
            DataGridView gridS3Day1 = (DataGridView)subGroup.Controls["tbPgS3D1"].Controls["gridS3Day1"];
            DataGridView gridS3Day2 = (DataGridView)subGroup.Controls["tbPgS3D2"].Controls["gridS3Day2"];
            DataGridView gridS3Day3 = (DataGridView)subGroup.Controls["tbPgS3D3"].Controls["gridS3Day3"];
            DataGridView gridS3Day4 = (DataGridView)subGroup.Controls["tbPgS3D4"].Controls["gridS3Day4"];
            DataGridView gridS3Day5 = (DataGridView)subGroup.Controls["tbPgS3D5"].Controls["gridS3Day5"];
            DataGridView gridS3Day6 = (DataGridView)subGroup.Controls["tbPgS3D6"].Controls["gridS3Day6"];

            subGroup = new Control();
            subGroup = group.Controls["tbPgS4"].Controls["tbSeason4"];
            DataGridView gridS4Day1 = (DataGridView)subGroup.Controls["tbPgS4D1"].Controls["gridS4Day1"];
            DataGridView gridS4Day2 = (DataGridView)subGroup.Controls["tbPgS4D2"].Controls["gridS4Day2"];
            DataGridView gridS4Day3 = (DataGridView)subGroup.Controls["tbPgS4D3"].Controls["gridS4Day3"];
            DataGridView gridS4Day4 = (DataGridView)subGroup.Controls["tbPgS4D4"].Controls["gridS4Day4"];
            DataGridView gridS4Day5 = (DataGridView)subGroup.Controls["tbPgS4D5"].Controls["gridS4Day5"];
            DataGridView gridS4Day6 = (DataGridView)subGroup.Controls["tbPgS4D6"].Controls["gridS4Day6"];

            DataGridView[] seasonGrids = new DataGridView[] 
            {
                gridS1Day1, gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6,
                gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, 
                gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6,
                gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6
            };
            return seasonGrids;
        }

        private DataGridView[] GetHolidayGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgHoliday"].Controls["tbHoliday"];
            DataGridView gridHoliday1 = (DataGridView)subGroup.Controls["tbPgHDay1"].Controls["gridHoliday1"];
            DataGridView gridHoliday2 = (DataGridView)subGroup.Controls["tbPgHDay2"].Controls["gridHoliday2"];
            DataGridView gridHoliday3 = (DataGridView)subGroup.Controls["tbPgHDay3"].Controls["gridHoliday3"];
            DataGridView gridHoliday4 = (DataGridView)subGroup.Controls["tbPgHDay4"].Controls["gridHoliday4"];
            DataGridView gridHoliday5 = (DataGridView)subGroup.Controls["tbPgHDay5"].Controls["gridHoliday5"];
            DataGridView gridHoliday6 = (DataGridView)subGroup.Controls["tbPgHDay6"].Controls["gridHoliday6"];
            DataGridView gridHoliday7 = (DataGridView)subGroup.Controls["tbPgHDay7"].Controls["gridHoliday7"];
            DataGridView gridHoliday8 = (DataGridView)subGroup.Controls["tbPgHDay8"].Controls["gridHoliday8"];
            DataGridView gridHoliday9 = (DataGridView)subGroup.Controls["tbPgHDay9"].Controls["gridHoliday9"];
            DataGridView gridHoliday10 = (DataGridView)subGroup.Controls["tbPgHDay10"].Controls["gridHoliday10"];

            DataGridView[] holidayGrids = new DataGridView[] 
            {
                gridHoliday1,gridHoliday2, gridHoliday3, gridHoliday4, gridHoliday5,
                gridHoliday6, gridHoliday7, gridHoliday8, gridHoliday9, gridHoliday10
            };
            return holidayGrids;
        }

        private DataGridView[] GetAssignmentGridCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgDayAssignment"].Controls["tbDayAssignment"];
            DataGridView gridAssignmentS1 = (DataGridView)subGroup.Controls["tbPgDayAssignS1"].Controls["gridAssignmentS1"];
            DataGridView gridAssignmentS2 = (DataGridView)subGroup.Controls["tbPgDayAssignS2"].Controls["gridAssignmentS2"];
            DataGridView gridAssignmentS3 = (DataGridView)subGroup.Controls["tbPgDayAssignS3"].Controls["gridAssignmentS3"];
            DataGridView gridAssignmentS4 = (DataGridView)subGroup.Controls["tbPgDayAssignS4"].Controls["gridAssignmentS4"];

            DataGridView[] dayAssignmentGrids = new DataGridView[] 
            {
                gridAssignmentS1, gridAssignmentS2, gridAssignmentS3, gridAssignmentS4
            };
            return dayAssignmentGrids;
        }

        private DateTimePicker[] GetActivationDateCollection()
        {
            group = new Control();
            group = this.touOperation1.Controls["grpTOU"].Controls["tbParent"];
            subGroup = new Control();
            subGroup = group.Controls["tbPgHoliday"].Controls["tbHoliday"];
            DateTimePicker dtPicAcDate1 = (DateTimePicker)subGroup.Controls["tbPgHDay1"].Controls["dtPicAcDate1"];
            DateTimePicker dtPicAcDate2 = (DateTimePicker)subGroup.Controls["tbPgHDay2"].Controls["dtPicAcDate2"];
            DateTimePicker dtPicAcDate3 = (DateTimePicker)subGroup.Controls["tbPgHDay3"].Controls["dtPicAcDate3"];
            DateTimePicker dtPicAcDate4 = (DateTimePicker)subGroup.Controls["tbPgHDay4"].Controls["dtPicAcDate4"];
            DateTimePicker dtPicAcDate5 = (DateTimePicker)subGroup.Controls["tbPgHDay5"].Controls["dtPicAcDate5"];
            DateTimePicker dtPicAcDate6 = (DateTimePicker)subGroup.Controls["tbPgHDay6"].Controls["dtPicAcDate6"];
            DateTimePicker dtPicAcDate7 = (DateTimePicker)subGroup.Controls["tbPgHDay7"].Controls["dtPicAcDate7"];
            DateTimePicker dtPicAcDate8 = (DateTimePicker)subGroup.Controls["tbPgHDay8"].Controls["dtPicAcDate8"];
            DateTimePicker dtPicAcDate9 = (DateTimePicker)subGroup.Controls["tbPgHDay9"].Controls["dtPicAcDate9"];
            DateTimePicker dtPicAcDate10 = (DateTimePicker)subGroup.Controls["tbPgHDay10"].Controls["dtPicAcDate10"];

            DateTimePicker[] holidayActivationDates = new DateTimePicker[]
            {
                dtPicAcDate1, dtPicAcDate2, dtPicAcDate3, dtPicAcDate4, dtPicAcDate5,
                dtPicAcDate6, dtPicAcDate7, dtPicAcDate8, dtPicAcDate9, dtPicAcDate10 
            };
            return holidayActivationDates;
        }


        private void FillList()
        {
            DataSet dataSet = CommonBLL.History();
            lstEnergy.DataSource = dataSet.Tables[0];
            lstEnergy.DisplayMember = "DisplayMember";
            lstEnergy.ValueMember = "ValueMember";

            lstTODMD.DataSource = dataSet.Tables[0];
            lstTODMD.DisplayMember = "DisplayMember";
            lstTODMD.ValueMember = "ValueMember";

            lstTODAveragePowerFactor.DataSource = dataSet.Tables[0];
            lstTODAveragePowerFactor.DisplayMember = "DisplayMember";
            lstTODAveragePowerFactor.ValueMember = "ValueMember";

            lstBillingTamperCounter.DataSource = CommonBLL.Tamper().Tables[0];
            lstBillingTamperCounter.DisplayMember = "DisplayMember";
            lstBillingTamperCounter.ValueMember = "ValueMember";

            lstTODConsumption.DataSource = CommonBLL.TODHistory().Tables[0];
            lstTODConsumption.DisplayMember = "DisplayMember";
            lstTODConsumption.ValueMember = "ValueMember";

            DataSet tamperDataSet = CommonBLL.TamperCounter(Convert.ToInt32(meterDataID));
            lblTamperOccur.Text = CommonBLL.TotalTamperCounter(tamperDataSet).ToString();
            lngTamperSupported.Data = CommonBLL.TamperCounter(tamperDataSet);
            lngTamperSupported.HiddenColumn = lngTamperSupported.ValueColumn = "Data";
            lngTamperSupported.SetWidth("Tamper", 150);
            lngTamperSupported.SetWidth("Counter", 60);
            lngTamperSupported.SetWidth("Status", 65);
            lngTamperSupported.RefreshGrid();
            lngTamperSupported.HSrollBar();
            lngTamperSupported.IsSorting = false;
        }

        private void lstEnergy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(meterDataID))
            {
                if (lstEnergy.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstEnergy.Items[lstEnergy.SelectedIndex])).Row.ItemArray[1].ToString();
                    lngBillingEnergy.Data = tariffBLL.GetMeterData(Convert.ToInt32(meterDataID), Convert.ToInt32(historyId));
                    if (lngBillingEnergy.Data != null)
                    {
                        lngBillingEnergy.SetWidth("Tariff Number", 120);
                        lngBillingEnergy.SetWidth("kWh", 80);
                        lngBillingEnergy.SetWidth("kVAh", 80);
                        lngBillingEnergy.SetWidth("kvarh (Lag)", 110);
                        lngBillingEnergy.SetWidth("kvarh (Lead)", 115);
                    }
                    lngBillingEnergy.HSrollBar();
                    lngBillingEnergy.IsSorting = false;
                }
            }
        }

        private void lstTODMD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(meterDataID))
            {
                if (lstTODMD.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODMD.Items[lstTODMD.SelectedIndex])).Row.ItemArray[1].ToString();
                    lngTODMD.Data = tariffBLL.GetTODMDMeterData(Convert.ToInt32(meterDataID), Convert.ToInt32(historyId));
                    if (lngTODMD.Data != null)
                    {
                        lngTODMD.SetWidth("Tariff Number", 50);
                        lngTODMD.SetWidth("MD1(KW)", 90);
                        lngTODMD.SetWidth("MD1 Time Stamp", 150);
                        lngTODMD.SetWidth("MD2(KVA)", 100);
                        lngTODMD.SetWidth("MD2 Time Stamp", 150);
                        //lngTODMD.SetWidth("MD3", 140);
                        //lngTODMD.SetWidth("MD3 Time Stamp", 220);
                    }
                    lngTODMD.HSrollBar();
                    lngTODMD.IsSorting = false;
                }
            }
        }

        private void lstTODAveragePowerFactor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(meterDataID))
            {
                if (lstTODAveragePowerFactor.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODAveragePowerFactor.Items[lstTODAveragePowerFactor.SelectedIndex])).Row.ItemArray[1].ToString();
                    lngTODAveragePowerFactor.Data = tariffBLL.GetTariffAveragePowerFactor(Convert.ToInt32(meterDataID), Convert.ToInt32(historyId));
                    if (lngTODAveragePowerFactor.Data != null)
                    {
                        lngTODAveragePowerFactor.SetWidth("Tariff Number", 120);
                        lngTODAveragePowerFactor.SetWidth("Value", 90);
                    }
                    lngTODAveragePowerFactor.HSrollBar();
                    lngTODAveragePowerFactor.IsSorting = false;
                }
            }
        }

        private void lstBillingTamperCounter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(meterDataID))
            {
                if (lstBillingTamperCounter.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstBillingTamperCounter.Items[lstBillingTamperCounter.SelectedIndex])).Row.ItemArray[1].ToString();
                    lngBillingTamperCounter.Data = tamperGeneralBLL.GetTamperCounter(Convert.ToInt32(meterDataID), historyId);
                    if (lngBillingTamperCounter.Data != null)
                    {
                        lngBillingTamperCounter.SetWidth("History", 100);
                        lngBillingTamperCounter.SetWidth("Counter", 100);
                    }
                    lngBillingTamperCounter.HSrollBar();
                    lngBillingTamperCounter.IsSorting = false;
                }
            }
        }

        private void lstTODConsumption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(meterDataID))
            {
                if (lstTODConsumption.Items.Count > 0)
                {
                    string historyId = ((System.Data.DataRowView)(lstTODConsumption.Items[lstTODConsumption.SelectedIndex])).Row.ItemArray[1].ToString();
                    lngTODConsumption.Data = tariffBLL.GetTODConsumption(Convert.ToInt32(meterDataID), Convert.ToInt32(historyId));
                    if (lngTODConsumption.Data != null)
                    {
                        lngTODConsumption.SetWidth("Tariff", 50);
                        lngTODConsumption.SetWidth("kWh", 80);
                        lngTODConsumption.SetWidth("kVAh", 80);
                        lngTODConsumption.SetWidth("kvarh (Lag)", 108);
                        lngTODConsumption.SetWidth("kvarh (Lead)", 115);
                    }
                    lngTODConsumption.HSrollBar();
                    lngTODConsumption.IsSorting = false;
                }
            }
        }

        private void AR_btnShowLoadSurvey_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            if (dtpFromDate.Value.Date == dtpToDate.Value.Date)
            {

            }
            if (dtpToDate.Value < FromDate)
            {
                if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
                    this.StatusMessage = "To Date should be greater/equal to " + FromDate.Date.Day.ToString() + "/" + FromDate.Date.Month.ToString() + "/" + FromDate.Date.Year.ToString();
                else
                    this.StatusMessage = "To Date should be greater/equal to " + FromDate.Date.Month.ToString() + "/" + FromDate.Date.Day.ToString() + "/" + FromDate.Date.Year.ToString();
                dtpToDate.Focus();
                Application.DoEvents();
                return;
            }
            if (dtpFromDate.Value > ToDate)
            {
                if (ConfigInfo.DateFormat() == "dd/MM/yyyy")
                    this.StatusMessage = "From Date should be lesser/equal to " + ToDate.Date.Day.ToString() + "/" + ToDate.Date.Month.ToString() + "/" + ToDate.Date.Year.ToString();
                else
                    this.StatusMessage = "From Date should be lesser/equal to " + ToDate.Date.Month.ToString() + "/" + ToDate.Date.Day.ToString() + "/" + ToDate.Date.Year.ToString();
                dtpToDate.Focus();
                Application.DoEvents();
                return;
            }

            long frmDate;
            long toDate;
            if (FromDate.Date > dtpFromDate.Value)

                frmDate = DateUtility.DateTimeToLong(FromDate.Date, true);

            else

                frmDate = DateUtility.DateTimeToLong(dtpFromDate.Value, true);

            if (ToDate.Date < dtpToDate.Value)

                toDate = DateUtility.DateTimeToLong(ToDate.Date, false);
            else
                toDate = DateUtility.DateTimeToLong(dtpToDate.Value, false);


            long diffDays = toDate - frmDate;
            if (diffDays >= 0)
            {
                if (ActivateThisChild("MeterDataLoadSurvey") == false)
                {
                    MeterDataLoadSurvey meterDataLoadSurvey = new MeterDataLoadSurvey();
                    meterDataLoadSurvey.MdiParent = this.MdiParent;
                    meterDataLoadSurvey.FromDate = DateUtility.LongToDateTime(frmDate); // dtpFromDate.Value;
                    meterDataLoadSurvey.ToDate = DateUtility.LongToDateTime(toDate);  //dtpToDate.Value;
                    meterDataLoadSurvey.MeterDataId = Convert.ToInt64(meterDataID);
                    meterDataLoadSurvey.Show();
                }
            }
            else
            {
                this.StatusMessage = "From date should not be greater than To date.";
                dtpFromDate.Focus();
                Application.DoEvents();
            }
        }


        public Boolean ActivateThisChild(String formName)
        {
            this.StatusMessage = string.Empty;
            int i;
            Boolean formSetToMdi = false;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    this.MdiParent.MdiChildren[i].Activate();
                    this.MdiParent.MdiChildren[i].Visible = true;
                    formSetToMdi = true;
                }
            }
            if (i == 0 || formSetToMdi == false)
                return false;
            else
                return true;
        }

        private void lngTamperSupported_OnGridRowChanged(string msg)
        {
            DataSet dataSet = billingBLL.TamperOccurRestore(Convert.ToInt64(meterDataID), Convert.ToInt64(msg));
            lngTamperOccur.Data = dataSet;
            if (dataSet != null)
            {
                lngTamperOccur.SetWidth("Sl.", 50);
                lngTamperOccur.SetWidth("Description", 250);
                lngTamperOccur.SetWidth("Occurrence Time Stamp", 150);
                lngTamperOccur.SetWidth("Restoration Time Stamp", 150);
                lngTamperOccur.SetWidth("Duration (Days:HH:MM)", 100);
                lngTamperOccur.IsSorting = false;
                if (dataSet.Tables[0].Rows.Count <= 0)
                {
                    lblDesc.Visible = false;
                    lngElectricalCondition.Visible = false;
                }
            }

            lngTamperOccur.HiddenColumn = lngTamperOccur.ValueColumn = "TamperSnapShot_ID";
            lngTamperOccur.HSrollBar();
            lngTamperOccur.RefreshGrid();
        }
        private void lngTamperOccur_OnGridRowChanged(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                DataSet dataSet = CommonBLL.GetTamperOccurRestoreDetail(Convert.ToInt64(msg), Convert.ToInt64(meterDataID));
                if (dataSet != null)
                {
                    if (dataSet.Tables[0].Rows[0][3].ToString().Equals("225") || dataSet.Tables[0].Rows[0][3].ToString().Equals("211"))
                    {
                        lblDesc.Visible = false;
                        lngElectricalCondition.Visible = false;
                    }
                    else
                    {
                        lblDesc.Visible = true;
                        lngElectricalCondition.Visible = true;
                        lngElectricalCondition.Data = dataSet;
                        lngElectricalCondition.SetWidth("Description", 200);
                        lngElectricalCondition.SetWidth("Occurrence Value", 150);
                        lngElectricalCondition.SetWidth("Restoration Value", 150);
                        lngElectricalCondition.HiddenColumn = lngElectricalCondition.ValueColumn = "TamperCode";
                        lngElectricalCondition.RefreshGrid();
                        lngElectricalCondition.HSrollBar();
                        lngElectricalCondition.IsSorting = false;
                    }
                }
            }
        }
        private long ParseDate(string dateTime, bool start)
        {
            string val = dateTime.Substring(0, 8);
            if (start)
                val = val + "000000";
            else
                val = val + "235959";
            return long.Parse(val);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            long frmDate = DateUtility.DateTimeToLong(dtpFromDate.Value);
            long toDate = DateUtility.DateTimeToLong(dtpToDate.Value);


            long diffDays = toDate - frmDate;
            if (diffDays >= 0)
            {
                frmLoadsurveyGraph frmloadsurveyGraph = new frmLoadsurveyGraph();
                frmloadsurveyGraph.FromDate = frmDate;
                frmloadsurveyGraph.ToDate = toDate;
                LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
                DataSet LoadSurveyData = loadSurveyBLL.ListDataSet(Convert.ToInt64(meterDataID), ParseDate(frmDate.ToString(), true), ParseDate(toDate.ToString(), false), "Demand");
                if (LoadSurveyData != null)
                {
                    frmloadsurveyGraph.IntervalPeriod = GetIntervalPeriod(LoadSurveyData);
                    if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                        GetNoLoad_PowerOffIPCount(LoadSurveyData, ref frmloadsurveyGraph.noLoadIPCount, ref frmloadsurveyGraph.noPowerIPCount);

                }
                frmloadsurveyGraph.MeterDataId = Convert.ToInt64(MeterDataID);
                frmloadsurveyGraph.Show();
            }
            else
            {
                this.StatusMessage = "From date should not be greater than To date.";
                dtpFromDate.Focus();
                Application.DoEvents();
            }
        }
        private int GetIntervalPeriod(DataSet LoadSurveyData)
        {
            int period = -1;
            for (int i = 0; i < LoadSurveyData.Tables[0].Rows.Count; i++)
            {
                if (Int32.Parse(LoadSurveyData.Tables[0].Rows[i]["MDIntervalPeriod"].ToString()) > 0)
                {
                    return Int32.Parse(LoadSurveyData.Tables[0].Rows[i]["MDIntervalPeriod"].ToString());
                }
            }
            return period;
        }

        private void GetNoLoad_PowerOffIPCount(DataSet ls_ds, ref int noLoadCount, ref int noPowerCount)
        {
            noLoadCount = 0;
            noPowerCount = 0;
            bool PowerOffInIP = false;
            for (int i = 0; i < ls_ds.Tables[0].Rows.Count; i++)
            {
                if (
                    (ls_ds.Tables[0].Rows[i]["Demand kVA"] != null && Convert.ToDouble(ls_ds.Tables[0].Rows[i]["Demand kVA"]) == 0.0)
                    && (ls_ds.Tables[0].Rows[i]["Demand kW"] != null && Convert.ToDouble(ls_ds.Tables[0].Rows[i]["Demand kW"]) == 0.0)
                  )
                    noLoadCount++;

                PowerOffInIP = true;
                for (int j = 1; j < ls_ds.Tables[0].Columns.Count; j++)
                {
                    if ((ls_ds.Tables[0].Rows[i][j] == null || Convert.ToDouble(ls_ds.Tables[0].Rows[i][j]) != -1))
                    {
                        PowerOffInIP = false;
                        break;
                    }
                }
                if (PowerOffInIP) noPowerCount++;
            }
        }

        private void MeterDataList_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
        }

        private void tabControlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StatusMessage = "";
        }

        private void displayParameters_Load(object sender, EventArgs e)
        {

        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpToDate.Value.Date == tmpDTPickToDate.Date)
                dtpToDate.Value = tmpDTPickToDate;
            else if (tmpDTPickToDate.Year == 1)
                return;
            else
                dtpToDate.Value = new DateTime(dtpToDate.Value.Date.Year, dtpToDate.Value.Date.Month, dtpToDate.Value.Date.Day, 23, 30, 0);
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpFromDate.Value.Date == tmpDTPickFromDate.Date)
                dtpFromDate.Value = tmpDTPickFromDate;
            else if (tmpDTPickFromDate.Year == 1)
                return;
            else
                dtpFromDate.Value = dtpFromDate.Value.Date;
        }

        private void MeterDataList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
            this.Cursor = Cursors.Default;
        }

    }
}
