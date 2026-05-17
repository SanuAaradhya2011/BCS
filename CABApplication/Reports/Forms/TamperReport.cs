using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI;
using CAB.UI.Controls;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;

namespace CABApplication.Reports.Forms
{
    public partial class TamperReport : CABForm
    {
        /// <summary>
        /// Add Sorting Modes for Tamper Report
        /// AS - 20191009
        /// </summary>
        private enum SortModes
        {
            [System.ComponentModel.Description("Default")]
            Default=0,
            [System.ComponentModel.Description("By Occurence DateTime Ascending")]
            ByOccurenceDateTimeAscending=1,
            [System.ComponentModel.Description("By Occurence DateTime Descending")]
            ByOccurenceDateTimeDescending = 2,
            [System.ComponentModel.Description("By Restoration DateTime Ascending")]
            ByRestorationDateTimeAscending = 3,
            [System.ComponentModel.Description("By Restoration DateTime Descending")]
            ByRestorationDateTimeDescending=4
        }

        #region Private Variables
        private const string DateUnavailable = "----";
        private const int CountColumnWidth = 50;
        private const int DescColumnWidth = 200;
        private const string NotApplied = "Not Applied";
        private const string Applied = "Applied";
        private const string ShortDateFromat = "dd/MM/yyyy";
        private static readonly Hunt.EPIC.Logging.IGeneralLog logger = Hunt.EPIC.Logging.LogFactory.CreateGeneralLogger(typeof(TamperReport).ToString());
        private string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        //GKG  Added Tamper for kvahselection
        //List<int> lstTamperIdsWithNoRestoration = new List<int> { 151, 152, 153, 154, 155, 156, 157, 251 };
        List<int> lstTamperIdsWithNoRestoration = new List<int> { 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 167, 168, 251, 190, 191, 192, 189, 184, 185, 186, 187, 188, 194, 200, 256 };
        //GKG  Added Tamper for kvahselection
        public static Dictionary<string, string> dictTamperCodeAndAbbreviation = null;
        private DLMS650TamperMasterBLL dlmsTamperMaster = null;
        private DLMS650CommonBLL dlms650CommonBLL = null;
        private MeterDataBLL meterDataBll = null;
        private FileReportDataSet reportXSD = null;
        private DataSet detailTamperData;
        private long activeMeterDataId = 0;
        private bool hasData = false;
        private int meterModelNumber = 0;
        List<string> tamperHeadings = null;
        private long changedFromDate;
        private long changedToDate;
        private static string[] sortValues = 
                {
                    EnumUtil.stringValueOf(SortModes.Default),
                    EnumUtil.stringValueOf(SortModes.ByOccurenceDateTimeAscending),
                    EnumUtil.stringValueOf(SortModes.ByOccurenceDateTimeDescending),
                    EnumUtil.stringValueOf(SortModes.ByRestorationDateTimeAscending),
                    EnumUtil.stringValueOf(SortModes.ByRestorationDateTimeDescending)
                };
        #endregion
        #region Constructors
        public TamperReport()
        {
            InitializeComponent();
            reportXSD = new FileReportDataSet();
            meterDataBll = new MeterDataBLL();
            dlmsTamperMaster = new DLMS650TamperMasterBLL();
            dlms650CommonBLL = new DLMS650CommonBLL();
            activeMeterDataId = Convert.ToInt64(ConfigInfo.ActiveMeterDataId);
            meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(activeMeterDataId.ToString());
            if (ConfigInfo.ActiveMeterType == "1P-2W")
            {
                rdbComp1.Enabled = false;
            }
            CreateDetailTamperDataSet();
            FillTamperSatrtEndDates(activeMeterDataId);
            FillTamperCountDetail(activeMeterDataId);
            GetTamperAbbreviation();
            SetSortValues();
        }

        //Add Sort Values on control initialization //AS - 20191009
        private void SetSortValues()
        {
            cmbSorting.Items.AddRange(sortValues);
            cmbSorting.SelectedIndex = 0;
        }

        #endregion
        // SB code change Start - 20180629 - Multiple Analysis View
        public void Refresh()
        {
            reportXSD = new FileReportDataSet();
            meterDataBll = new MeterDataBLL();
            dlmsTamperMaster = new DLMS650TamperMasterBLL();
            dlms650CommonBLL = new DLMS650CommonBLL();
            activeMeterDataId = Convert.ToInt64(ConfigInfo.ActiveMeterDataId);
            meterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(activeMeterDataId.ToString());
            if (ConfigInfo.ActiveMeterType == "1P-2W")
            {
                rdbComp1.Enabled = false;
            }
            CreateDetailTamperDataSet();
            FillTamperSatrtEndDates(activeMeterDataId);
            FillTamperCountDetail(activeMeterDataId);
            GetTamperAbbreviation();
        }
        // SB code change End - 20180629 - Multiple Analysis View
        public bool HasData
        {
            get
            {
                return hasData;
            }
            set
            {
                hasData = value;
            }
        }
        #region Methods

        /// <summary>
        /// Used to get type and Count of tampers
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        private void FillTamperCountDetail(long activeMeterDataId)
        {
            DataRow tamperTypeRow;
            DataSet dataSet = dlmsTamperMaster.GetTamperCountDetail(activeMeterDataId);
            #region PSPCL Specific Check
            string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
            if (firmwareVersion == "7.01")
            {
                for (int count = 0; count < dataSet.Tables[0].Rows.Count; count++)
                {
                    tamperTypeRow = dataSet.Tables[0].Rows[count];
                    if (tamperTypeRow != null && tamperTypeRow["Description"].ToString().Contains("Voltage Unbalance"))
                    {
                        tamperTypeRow["Description"] = "Invalid Voltage";
                    }

                }
            }
            #endregion
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                // Removing MD Reset, Over Voltage and Low Voltage for WB meters.
                if (meterModelNumber == 9)
                {
                    //DataRow[] rowPowFailCount = dataSet.Tables[0].Select("TamperId = '159' or TamperId = '7' or TamperId = '8' or TamperId = '9' or TamperId = '10'");
                    // MD Reset now get visisble /* This check is removed as per WB specific OPF demand */
                    DataRow[] rowPowFailCount = dataSet.Tables[0].Select("TamperId = '7' or TamperId = '8' or TamperId = '9' or TamperId = '10'");
                    if (rowPowFailCount != null && rowPowFailCount.Length > 0)
                    {
                        for (int rowCount = 0; rowCount < rowPowFailCount.Length; rowCount++)
                        {
                            dataSet.Tables[0].Rows.Remove(rowPowFailCount[rowCount]);
                            dataSet.AcceptChanges();
                        }
                    }
                }
                hasData = true;
                lblNoData.Visible = false;
                dgvTamperOccurence.Show();
                dgvTamperOccurence.AutoGenerateColumns = true;
                dgvTamperOccurence.DataSource = dataSet.Tables[0];
                DataGridViewColumn dgvColumn = new DataGridViewCheckBoxColumn();
                dgvColumn.Name = "Select";
                dgvColumn.HeaderText = "Select";
                if (!dgvTamperOccurence.Columns.Contains("Select"))
                {
                    dgvTamperOccurence.Columns.Insert(dgvTamperOccurence.Columns.Count, dgvColumn);
                    dgvTamperOccurence.Columns["Count"].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["Description"].Width = DescColumnWidth;
                    dgvTamperOccurence.Columns["Select"].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["S.No."].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["TamperId"].Visible = false;
                    /*VBM - Make columns read only */
                    dgvTamperOccurence.Columns["Count"].ReadOnly = true;
                    dgvTamperOccurence.Columns["Description"].ReadOnly = true;
                    dgvTamperOccurence.Columns["S.No."].ReadOnly = true;
                    /*VBM - Make columns read only */
                }
            }
            else
            {
                lblNoData.Visible = true;
                MessageBox.Show("No Data Available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvTamperOccurence.Hide();
                hasData = false;

            }

        }

        /// <summary>
        /// Used to get type and Count of tampers
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        private void FillTamperCountDetailByDateRange(int activeMeterDataId, long frmDate, long toDate)
        {
            DataSet dataSet = dlmsTamperMaster.GetTamperDetailByDateRange(activeMeterDataId, frmDate, toDate);
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                HasData = true;
                lblNoData.Visible = false;
                dgvTamperOccurence.Show();
                dgvTamperOccurence.AutoGenerateColumns = true;
                dgvTamperOccurence.DataSource = dataSet.Tables[0];
                DataGridViewColumn dgvColumn = new DataGridViewCheckBoxColumn();
                dgvColumn.Name = "Select";
                dgvColumn.HeaderText = "Select";
                if (!dgvTamperOccurence.Columns.Contains("Select"))
                {
                    dgvTamperOccurence.Columns.Insert(dgvTamperOccurence.Columns.Count, dgvColumn);
                    dgvTamperOccurence.Columns["Count"].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["Description"].Width = DescColumnWidth;
                    dgvTamperOccurence.Columns["Select"].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["S.No."].Width = CountColumnWidth;
                    dgvTamperOccurence.Columns["TamperId"].Visible = false;
                }
            }
            else
            {
                lblNoData.Visible = true;
                dgvTamperOccurence.Hide();
                HasData = false;
            }
        }
        /// <summary>
        /// Used to get type and Count of tampers based on compartment ID
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        private void FillTamperCountDetailByDateRangeWithCompartmentID(int activeMeterDataId, long frmDate, long toDate, string compartmentNo)
        {
            DataSet dataSet = dlmsTamperMaster.GetTamperDetailByDateRangeWithCompartmentID(activeMeterDataId, frmDate, toDate, compartmentNo);
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                HasData = true;
                lblNoData.Visible = false;
                dgvTamperOccurence.Show();
                dgvTamperOccurence.AutoGenerateColumns = true;
                dgvTamperOccurence.DataSource = dataSet.Tables[0];
            }
            else
            {
                lblNoData.Visible = true;
                dgvTamperOccurence.Hide();
                HasData = false;
            }

        }


        /// <summary>
        /// Used to Get Tamper start and End dates 
        /// and Fill Tamper occurence dates on form
        /// </summary>
        /// <param name="activeMeterDataId"></param>
        private void FillTamperSatrtEndDates(long activeMeterDataId)
        {
            DataSet dataSet = dlmsTamperMaster.GetTamperStartEndDate(activeMeterDataId);
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
            {
                string startDate = dataSet.Tables[0].Rows[0]["TamperStartDate"].ToString();
                string endDate = dataSet.Tables[0].Rows[0]["TamperEndDate"].ToString();
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && startDate!="0")
                {
                    dtPickerStartDate.Value = DateUtility.LongToDateTime(Convert.ToInt64(startDate));
                    dtPickerEndDate.Value = DateUtility.LongToDateTime(Convert.ToInt64(endDate));
                    dtPickerStartDate.MinDate = DateUtility.LongToDateTime(Convert.ToInt64(startDate));
                    dtPickerStartDate.MaxDate = DateUtility.LongToDateTime(Convert.ToInt64(endDate));
                    dtPickerEndDate.MinDate = DateUtility.LongToDateTime(Convert.ToInt64(startDate));
                    dtPickerEndDate.MaxDate = DateUtility.LongToDateTime(Convert.ToInt64(endDate));
                }
            }
        }
        private void CheckAndUpdateSelectAll(DataGridView dGVTamper)
        {
            bool isSelected = false;
            foreach (DataGridViewRow row in dGVTamper.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) != true)
                {
                    isSelected = false;
                    break;
                }
                else if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    isSelected = true;
                }
            }
            chkSelectAll.CheckedChanged -= new EventHandler(chkSelectAll_CheckedChanged);
            if (isSelected == false)
            {
                chkSelectAll.Checked = false;
            }
            else
            {
                chkSelectAll.Checked = true;
            }
            chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);
        }
        private bool Validate(DataGridView dGVTamper)
        {
            bool isValidated = false;
            foreach (DataGridViewRow row in dGVTamper.Rows)
            {
                if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value) == true)
                {
                    isValidated = true;
                    break;
                }
            }
            if (!isValidated)
            {
                MessageBox.Show("Please select atleast one Tamper/Transaction to view the report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return isValidated;
        }
        /// <summary>
        /// Used to display tamper report  and cumulative tamper counter report .         
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowReport_Click(object sender, EventArgs e)
        {
            int errCount = 0;
            int showReport = 0;
            string errMsg = String.Empty;
            DataTable dtTable = new DataTable();
            if(detailTamperData.Tables[0].Columns.Contains("OCCDATE")) detailTamperData.Tables[0].Columns.Remove("OCCDATE");
            if (detailTamperData.Tables[0].Columns.Contains("RESDATE")) detailTamperData.Tables[0].Columns.Remove("RESDATE");
            if (Validate(dgvTamperOccurence))
            {

                Cursor.Current = Cursors.WaitCursor;
                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerStartDate.Value.ToShortDateString() + " 00:00:00"));
                long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerEndDate.Value.ToShortDateString() + " 23:59:59"));
                detailsDS = meterDataBll.GetConsumerMeterDetails(activeMeterDataId);

                GetDetailTamperData();

                detailsDS = meterDataBll.GetConsumerMeterDetails(activeMeterDataId);

                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                {
                    FillConsumerMeterDetails(detailsDS);
                }
                else
                {
                    meterIDDS = meterDataBll.GetMeterIDFromMeterDataID(activeMeterDataId);
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    {
                        FillMeterID(meterIDDS);
                    }
                }
                // code copied from
                DataSet tamperCounterDataSet = dlmsTamperMaster.GetTamperDetailByDateRange(Convert.ToInt32(activeMeterDataId), frmDate, toDate); //dlmsTamperMaster.GetTamperCountDetail(activeMeterDataId);
                if (tamperCounterDataSet != null && tamperCounterDataSet.Tables.Count > 0 && tamperCounterDataSet.Tables[0].Rows.Count > 0)
                {
                    #region WB Specific to hide Tamper Parameters
                    // Removing MD Reset, Over Voltage and Low Voltage for WB meters.
                    if (meterModelNumber == 9)
                    {
                        //DataRow[] rowPowFailCount = tamperCounterDataSet.Tables[0].Select("TamperId = '159' or TamperId = '7' or TamperId = '8' or TamperId = '9' or TamperId = '10'");
                        // MD Reset now get visisble /* This check is removed as per WB specific OPF demand */
                        DataRow[] rowPowFailCount = tamperCounterDataSet.Tables[0].Select("TamperId = '7' or TamperId = '8' or TamperId = '9' or TamperId = '10'");
                        if (rowPowFailCount != null && rowPowFailCount.Length > 0)
                        {
                            for (int rowCount = 0; rowCount < rowPowFailCount.Length; rowCount++)
                            {
                                tamperCounterDataSet.Tables[0].Rows.Remove(rowPowFailCount[rowCount]);
                                tamperCounterDataSet.AcceptChanges();
                            }
                        }
                    }
                    #endregion
                    #region PSPCL specific check
                    DataRow tamperTypeRow;
                    string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
                    if (firmwareVersion == "7.01")
                    {
                        for (int count = 0; count < tamperCounterDataSet.Tables[0].Rows.Count; count++)
                        {
                            tamperTypeRow = tamperCounterDataSet.Tables[0].Rows[count];
                            if (tamperTypeRow != null && tamperTypeRow["Description"].ToString().Contains("Voltage Unbalance"))
                            {
                                tamperTypeRow["Description"] = "Invalid Voltage";
                            }

                        }
                    }
                    #endregion
                    FillTamperCounterXSD(tamperCounterDataSet);
                    showReport++;
                }
                else
                {
                    errCount++;
                    errMsg = "Cumulative tamper count data not available.";
                }
                if (detailTamperData != null && detailTamperData.Tables.Count > 0 && detailTamperData.Tables[0].Rows.Count > 0)
                {
                    //Add Sorting to Tamper Report  //AS - 20191009
                    DataView dv = detailTamperData.Tables[0].DefaultView;
                    dv.Table.Columns.Add("OCCDATE", typeof(DateTime));
                    dv.Table.Columns.Add("RESDATE", typeof(DateTime));
                    dv.Table.Columns["OCCDATE"].ColumnMapping = MappingType.Hidden;
                    dv.Table.Columns["RESDATE"].ColumnMapping = MappingType.Hidden;
                    DateTime occ, res;
                    foreach(DataRow row in dv.Table.Rows)
                    {
                        if(DateTime.TryParseExact(row["OccDateTime"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out occ))
                        {
                            row["OCCDATE"] = occ;
                        }
                        if (DateTime.TryParseExact(row["ResDateTime"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out res))
                        {
                            row["RESDATE"] = res;
                        }
                    }
                    switch ((SortModes)EnumUtil.enumValueOf(cmbSorting.SelectedItem.ToString(), typeof(SortModes)))       
                    {
                        case SortModes.ByOccurenceDateTimeDescending:
                            dv.Sort = "OCCDATE" + " desc";
                            break;
                        case SortModes.ByRestorationDateTimeDescending:
                            dv.Sort = "RESDATE" + " desc";
                            break;
                        case SortModes.ByOccurenceDateTimeAscending:
                            dv.Sort = "OCCDATE" + " asc";
                            break;
                        case SortModes.ByRestorationDateTimeAscending:
                            dv.Sort = "RESDATE" + " asc";
                            break;
                        case SortModes.Default:
                        default:
                            break;
                    }
                    detailTamperData.Tables.Remove("Table1");
                    detailTamperData.Tables.Add(dv.ToTable());
                    detailTamperData.AcceptChanges();

                    FillDetailedTamperXSD(detailTamperData);
                    showReport++;
                }
                else
                {
                    errCount++;
                    errMsg = "Detailed tamper data not available";
                }
                if (errCount == 0)
                {
                    ShowReport();
                }
                else
                {
                    if (string.IsNullOrEmpty(errMsg))
                    { MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    else
                    {
                        errCount++;
                        errMsg = "Detailed tamper data not available";
                    }
                    if (errCount == 0)
                    {
                        ShowReport();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(errMsg))
                        { MessageBox.Show("No data available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        else
                        {
                            MessageBox.Show(errMsg, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (showReport > 0)
                            ShowReport();
                    }
                }

            }
        }
        /// <summary>
        /// VBM - Use to get TamperIds for all checked checkboxes
        /// </summary>
        /// <returns></returns>
        private List<int> GetCheckedTamperIds()
        {
            StringBuilder strTamperIds = new StringBuilder();
            List<int> lstSelectedIds = new List<int>();
            int intTamperIdOccurence;
            if (dgvTamperOccurence != null)
            {
                foreach (DataGridViewRow row in dgvTamperOccurence.Rows)
                {
                    if (row.Cells["Select"].Value != null && Convert.ToBoolean(row.Cells["Select"].Value))
                    {
                        intTamperIdOccurence = Convert.ToInt32(row.Cells["TamperId"].Value);
                        lstSelectedIds.Add(intTamperIdOccurence);
                    }
                }
            }

            return lstSelectedIds;
        }
        /// <summary>
        /// Gets tamper data from database and fills it into a data set that is used to bind report table.
        /// </summary>
        private void GetDetailTamperData()
        {
            string OccRVoltage = string.Empty;
            string ResRVoltage = string.Empty;
            string OccYVoltage = string.Empty;
            string ResYVoltage = string.Empty;
            string OccBVoltage = string.Empty;
            string ResBVoltage = string.Empty;
            string OccPhaseVoltage = string.Empty;
            string ResPhaseVoltage = string.Empty;
            string OccRCurrent = string.Empty;
            string ResRCurrent = string.Empty;
            string OccYCurrent = string.Empty;
            string ResYCurrent = string.Empty;
            string OccBCurrent = string.Empty;
            string ResBCurrent = string.Empty;
            string OccPhaseCurrent = string.Empty;
            string ResPhaseCurrent = string.Empty;
            string OccNeutralCurrent = string.Empty; // Story - 349654 - Neutral current in Tamper

            string OccHighNeutralCurrent = string.Empty; // Pradipta_neu
            string ResHighNeutralCurrent = string.Empty;

            string OccByPassCurrent = string.Empty; 
            string ResByPassCurrent = string.Empty;

            string OcckWr = string.Empty; // Pradipta_neu
            string ReskWr = string.Empty;

            string OcckWy = string.Empty; // Pradipta_neu
            string ReskWy = string.Empty;

            string OcckWb = string.Empty; // Pradipta_neu
            string ReskWb = string.Empty;

            string OcckVAr = string.Empty; // Pradipta_neu
            string ReskVAr = string.Empty;

            string OcckVAy = string.Empty; // Pradipta_neu
            string ReskVAy = string.Empty;

            string OcckVAb = string.Empty; // Pradipta_neu
            string ReskVAb = string.Empty;

            string OccCumulTampCount = string.Empty; // smart meter
            string ResCumulTampCount = string.Empty;

            string ResNeutralCurrent = string.Empty;
            string OccRPF = string.Empty;
            string ResRPF = string.Empty;
            string OccYPF = string.Empty;
            string ResYPF = string.Empty;
            string OccBPF = string.Empty;
            string ResBPF = string.Empty;
            string OccPF = string.Empty;
            string ResPF = string.Empty;
            string OccKwh = string.Empty;
            string ResKwh = string.Empty;
            string OccKVAh = string.Empty;
            string ResKVAh = string.Empty;
            string OccDateTime = string.Empty;
            string ResDateTime = string.Empty;
            string OcckWhImport = string.Empty;
            string OcckWhExport = string.Empty;
            string ReskWhImport = string.Empty;
            string ReskWhExport = string.Empty;
            string OcckVAhImport = string.Empty;
            string OcckVAhExport = string.Empty;
            string ReskVAhImport = string.Empty;
            string ReskVAhExport = string.Empty;
            string OcckvarhLag = string.Empty;
            string ReskvarhLag = string.Empty;
            string OcckvarhLead = string.Empty;
            string ReskvarhLead = string.Empty;
            // SB Code Change Start 20171120
            string OccActCurR = string.Empty;
            string ResActCurR = string.Empty;
            string OccActCurY = string.Empty;
            string ResActCurY = string.Empty;
            string OccActCurB = string.Empty;
            string ResActCurB = string.Empty;
            // SB Code Change End 20171120

            //SarkarA code change start 20180330 // add phase current instant, frequency/end
            string OccPhaseCurrentInstant = string.Empty;
            string ResPhaseCurrentInstant = string.Empty;
            string OccFrequency = string.Empty;
            string ResFrequency = string.Empty;
            //SarkarA code change end 20180330
            string OccTemprature = string.Empty;
            string ResTemprature = string.Empty;
            string OccTHDVR = string.Empty;
            string ResTHDVR = string.Empty;
            string OccTHDVY = string.Empty;
            string ResTHDVY = string.Empty;
            string OccTHDVB = string.Empty;
            string ResTHDVB = string.Empty;
            string OccTHDIR = string.Empty;
            string ResTHDIR = string.Empty;
            string OccTHDIY = string.Empty;
            string ResTHDIY = string.Empty;
            string OccTHDIB = string.Empty;
            string ResTHDIB = string.Empty;
           

            detailTamperData.Clear();
            DataSet detailedTamperOccData = new DataSet();
            DataSet detailedTamperResData = new DataSet();
            long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerStartDate.Value.ToShortDateString() + " 00:00:00"));
            long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerEndDate.Value.ToShortDateString() + " 23:59:59"));
            List<int> lstSelectedTamperIds = GetCheckedTamperIds();
            foreach (int intTamperIds in lstSelectedTamperIds)
            {
                detailedTamperOccData.Clear();
                detailedTamperResData.Clear();
                if (lstTamperIdsWithNoRestoration.Contains(intTamperIds))
                {
                    detailedTamperOccData = dlmsTamperMaster.GetTamperDetailByTamperId(activeMeterDataId, intTamperIds, frmDate, toDate);
                     
                }
                else
                {
                    detailedTamperOccData = dlmsTamperMaster.GetTamperDetailByTamperId(activeMeterDataId, intTamperIds, frmDate, toDate);
                    detailedTamperResData = dlmsTamperMaster.GetTamperDetailByTamperId(activeMeterDataId, intTamperIds + 1, frmDate, toDate);
                }

                if (detailedTamperOccData != null && detailedTamperOccData.Tables.Count > 0 && detailedTamperResData != null && detailedTamperResData.Tables.Count > 0)
                {
                    int intOccRowCount = detailedTamperOccData.Tables[0].Rows.Count;
                    int intResRowCount = detailedTamperResData.Tables[0].Rows.Count;
                    int intMaxRows = intOccRowCount;// (intOccRowCount > intResRowCount) ? intOccRowCount : intResRowCount;
                    string strTamperCode = string.Empty;
                    //for (int i = 0; i < intMaxRows; i++)
                    int occCounter = 0;
                    int resCounter = 0;

                    //This condition applies when Occurance count is less than restoration count
                    if (intOccRowCount < intResRowCount)
                    {
                        //To prevent display of data where restoration is earlier that occurence : RollOver case .
                        if (Convert.ToInt64(detailedTamperResData.Tables[0].Rows[resCounter]["Time Stamp"])
                            < Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]))
                        {
                            /* User Story: 447861 Tamper Occurrance and Restoration issue in Tamper Report
                             * Remove the first Restoration from the tamper restoration dataset */ 
                            detailedTamperResData.Tables[0].Rows.RemoveAt(resCounter);
                        }
                    }

                    while (occCounter < intMaxRows)
                    {

                        DataRow detailTamperRow = detailTamperData.Tables[0].NewRow();
                        strTamperCode = detailedTamperOccData.Tables[0].Rows[occCounter]["Event Code"].ToString();
                        if (dictTamperCodeAndAbbreviation.ContainsKey(strTamperCode))
                        {
                            detailTamperRow["Description"] = dictTamperCodeAndAbbreviation[strTamperCode];
                        }
                        else
                        {
                            detailTamperRow["Description"] = DateUnavailable;
                        }

                       
                        if (occCounter <= intResRowCount - 1 && resCounter <= intOccRowCount - 1 && detailedTamperResData.Tables[0].Rows.Count - 1 >= resCounter
                            && detailedTamperOccData.Tables[0].Rows.Count - 1 >= occCounter)
                        {
                            //To prevent display of data where restoration is earlier that occurence : RollOver case.
                            if (Convert.ToInt64(detailedTamperResData.Tables[0].Rows[resCounter]["Time Stamp"])
                                < Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]))
                            {
                                resCounter++;
                                continue;
                            }
                            //Filter only those tampers where corresponding phase current is greater than 0.
                            //if (chkVoltageTamperHavingCurrent.Checked)
                            //{
                            //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIR"]) == 0)
                            //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIY"]) == 0)
                            //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIB"]) == 0))
                            //    {
                            //        occCounter++;
                            //        resCounter++;
                            //        continue;
                            //    }

                            //}

                            // case for occurence and restoration both starts

                            OccRVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVRN"].ToString());
                            ResRVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVRN"].ToString());
                            OccYVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVYN"].ToString());
                            ResYVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVYN"].ToString());
                            OccBVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVBN"].ToString());
                            ResBVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVBN"].ToString());
                            OccPhaseVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseVoltage"].ToString());
                            ResPhaseVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseVoltage"].ToString());
                            OccRCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIR"].ToString());
                            ResRCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIR"].ToString());
                            OccYCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIY"].ToString());
                            ResYCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIY"].ToString());
                            OccBCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIB"].ToString());
                            ResBCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIB"].ToString());
                            OccPhaseCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseCurrent"].ToString());
                            ResPhaseCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseCurrent"].ToString());
                            OccNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["NeutralCurrent"].ToString()); // Story - 349654 - Neutral current in Tamper
                            ResNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["NeutralCurrent"].ToString());

                            OccByPassCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ByPassCurrent"].ToString()); 
                            ResByPassCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["ByPassCurrent"].ToString());

                            OccHighNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["HighNeutralCurrent"].ToString()); // pradipta_neu
                            ResHighNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["HighNeutralCurrent"].ToString());


                            OcckWr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWr"].ToString()); // pradipta_neu
                            ReskWr = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWr"].ToString());

                            OcckWy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWy"].ToString()); // pradipta_neu
                            ReskWy = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWy"].ToString());

                            OcckWb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWb"].ToString()); // pradipta_neu
                            ReskWb = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWb"].ToString());

                            OcckVAr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAr"].ToString()); // pradipta_neu
                            ReskVAr = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAr"].ToString());

                            OcckVAy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAy"].ToString()); // pradipta_neu
                            ReskVAy = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAy"].ToString());

                            OcckVAb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAb"].ToString()); // pradipta_neu
                            ReskVAb = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAb"].ToString());

                            OccCumulTampCount = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeTamperCount"].ToString()); // smart meter
                            ResCumulTampCount = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeTamperCount"].ToString());
                                                      

                            OccRPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorRphase"].ToString();
                            ResRPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorRphase"].ToString();
                            OccYPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorYphase"].ToString();
                            ResYPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorYphase"].ToString();
                            OccBPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorBphase"].ToString();
                            ResBPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorBphase"].ToString();
                            OccPF = detailedTamperOccData.Tables[0].Rows[occCounter]["TotalPowerFactor"].ToString();
                            ResPF = detailedTamperResData.Tables[0].Rows[resCounter]["TotalPowerFactor"].ToString();
                            OccKwh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWh"].ToString());
                            ResKwh = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykWh"].ToString());
                            OccKVAh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAh"].ToString());
                            ResKVAh = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykVAh"].ToString());

                            OcckWhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWhImport"].ToString());

                            OcckWhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWhExport"].ToString());

                            ReskWhImport = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykWhImport"].ToString());

                            ReskWhExport = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykWhExport"].ToString());

                            OcckVAhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAhImport"].ToString());

                            OcckVAhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAhExport"].ToString());

                            ReskVAhImport = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykVAhImport"].ToString());

                            ReskVAhExport = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykVAhExport"].ToString());

                            OcckvarhLag = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykvarhLag"].ToString());
                            ReskvarhLag = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykvarhLag"].ToString());

                            OcckvarhLead = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykvarhLead"].ToString());
                            ReskvarhLead = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykvarhLead"].ToString());

                            // SB Code Change Start 20171120
                            OccActCurR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentR"].ToString());
                            ResActCurR = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["ActiveCurrentR"].ToString());
                            OccActCurY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentY"].ToString());
                            ResActCurY = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["ActiveCurrentY"].ToString());
                            OccActCurB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentB"].ToString());
                            ResActCurB = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["ActiveCurrentB"].ToString());
                            // SB Code Change End 20171120

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            OccPhaseCurrentInstant = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseCurrentInstant"].ToString());
                            ResPhaseCurrentInstant = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseCurrentInstant"].ToString());
                            OccFrequency = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["Frequency"].ToString());
                            ResFrequency = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["Frequency"].ToString());
                            OccTemprature = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["Temprature"].ToString());
                            ResTemprature = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["Temprature"].ToString());

                            OccTHDVR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVR"].ToString());
                            ResTHDVR = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVR"].ToString());
                            OccTHDVY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVY"].ToString());
                            ResTHDVY = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVY"].ToString());
                            OccTHDVB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVB"].ToString());
                            ResTHDVB = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVB"].ToString());
                            OccTHDIR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIR"].ToString());
                            ResTHDIR = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIR"].ToString());
                            OccTHDIY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIY"].ToString());
                            ResTHDIY = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIY"].ToString());
                            OccTHDIB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIB"].ToString());
                            ResTHDIB = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIB"].ToString());

                            //SarkarA code change end 20180330

                            // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                            //if (strTamperCode == "101")
                            //{
                            //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            //}
                            //else
                            //{
                            //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            //}
                            OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            ResDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperResData.Tables[0].Rows[resCounter]["Time Stamp"]));

                            detailTamperRow["OccEvent"] = detailedTamperOccData.Tables[0].Rows[occCounter]["Event Code"].ToString(); ;
                            detailTamperRow["ResEvent"] = detailedTamperResData.Tables[0].Rows[resCounter]["Event Code"].ToString(); ;
                            detailTamperRow["OccDateTime"] = OccDateTime;
                            detailTamperRow["ResDateTime"] = ResDateTime;
                            detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DateUnavailable : OccRVoltage;
                            detailTamperRow["ResRVoltage"] = ResRVoltage == string.Empty ? DateUnavailable : ResRVoltage;
                            detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DateUnavailable : OccYVoltage;
                            detailTamperRow["ResYVoltage"] = ResYVoltage == string.Empty ? DateUnavailable : ResYVoltage;
                            detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DateUnavailable : OccBVoltage;
                            detailTamperRow["ResBVoltage"] = ResBVoltage == string.Empty ? DateUnavailable : ResBVoltage;

                            detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DateUnavailable : OccPhaseVoltage;
                            detailTamperRow["ResPhaseVoltage"] = ResPhaseVoltage == string.Empty ? DateUnavailable : ResPhaseVoltage;   //SarkarA code change 20180308 // fix Restoration Voltage/end

                            detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DateUnavailable : OccRCurrent;
                            detailTamperRow["ResRCurrent"] = ResRCurrent == string.Empty ? DateUnavailable : ResRCurrent;
                            detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DateUnavailable : OccYCurrent;
                            detailTamperRow["ResYCurrent"] = ResYCurrent == string.Empty ? DateUnavailable : ResYCurrent;
                            detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DateUnavailable : OccBCurrent;
                            detailTamperRow["ResBCurrent"] = ResBCurrent == string.Empty ? DateUnavailable : ResBCurrent;

                            detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DateUnavailable : OccPhaseCurrent;
                            detailTamperRow["ResPhaseCurrent"] = ResPhaseCurrent == string.Empty ? DateUnavailable : ResPhaseCurrent;

                            detailTamperRow["OccNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OccNeutralCurrent; // Story - 349654 - Neutral current in Tamper
                            detailTamperRow["ResNeutralCurrent"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ResNeutralCurrent;

                            detailTamperRow["OccByPassCurrent"] = OccByPassCurrent == string.Empty ? DateUnavailable : OccByPassCurrent;
                            detailTamperRow["ResByPassCurrent"] = ResByPassCurrent == string.Empty ? DateUnavailable : ResByPassCurrent;

                            detailTamperRow["OccHighNeutralCurrent"] = OccHighNeutralCurrent == string.Empty ? DateUnavailable : OccHighNeutralCurrent; //pradipta_neu
                            detailTamperRow["ResHighNeutralCurrent"] = ResHighNeutralCurrent == string.Empty ? DateUnavailable : ResHighNeutralCurrent;



                            detailTamperRow["OcckWr"] = OcckWr == string.Empty ? DateUnavailable : OcckWr; //pradipta_neu
                            detailTamperRow["ReskWr"] = ReskWr == string.Empty ? DateUnavailable : ReskWr;

                            detailTamperRow["OcckWy"] = OcckWy == string.Empty ? DateUnavailable : OcckWy; //pradipta_neu
                            detailTamperRow["ReskWy"] = ReskWy == string.Empty ? DateUnavailable : ReskWy;

                            detailTamperRow["OcckWb"] = OcckWb == string.Empty ? DateUnavailable : OcckWb; //pradipta_neu
                            detailTamperRow["ReskWb"] = ReskWb == string.Empty ? DateUnavailable : ReskWb;

                            detailTamperRow["OcckVAr"] = OcckVAr == string.Empty ? DateUnavailable : OcckVAr; //pradipta_neu
                            detailTamperRow["ReskVAr"] = ReskVAr == string.Empty ? DateUnavailable : ReskVAr;

                            detailTamperRow["OcckVAy"] = OcckVAy == string.Empty ? DateUnavailable : OcckVAy; //pradipta_neu
                            detailTamperRow["ReskVAy"] = ReskVAy == string.Empty ? DateUnavailable : ReskVAy;

                            detailTamperRow["OcckVAb"] = OcckVAb == string.Empty ? DateUnavailable : OcckVAb; //pradipta_neu
                            detailTamperRow["ReskVAb"] = ReskVAb == string.Empty ? DateUnavailable : ReskVAb;

                            detailTamperRow["OccCumulTampCount"] = OccCumulTampCount == string.Empty ? DateUnavailable : OccCumulTampCount; //smart meter
                            detailTamperRow["ResCumulTampCount"] = ResCumulTampCount == string.Empty ? DateUnavailable : ResCumulTampCount;


                            detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DateUnavailable : OccRPF;
                            detailTamperRow["ResRPF"] = ResRPF == string.Empty ? DateUnavailable : ResRPF;
                            detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DateUnavailable : OccYPF;
                            detailTamperRow["ResYPF"] = ResYPF == string.Empty ? DateUnavailable : ResYPF;
                            detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DateUnavailable : OccBPF;
                            detailTamperRow["ResBPF"] = ResBPF == string.Empty ? DateUnavailable : ResBPF;
                            detailTamperRow["OccPF"] = OccPF == string.Empty ? DateUnavailable : OccPF;
                            detailTamperRow["ResPF"] = ResPF == string.Empty ? DateUnavailable : ResPF;

                            detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DateUnavailable : OccKwh;
                            detailTamperRow["ResKwh"] = ResKwh == string.Empty ? DateUnavailable : ResKwh;
                            detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DateUnavailable : OccKVAh;
                            detailTamperRow["ResKVAh"] = ResKVAh == string.Empty ? DateUnavailable : ResKVAh;

                            detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DateUnavailable : OcckWhImport;
                            detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DateUnavailable : OcckWhExport;
                            detailTamperRow["ReskWhImport"] = ReskWhImport == string.Empty ? DateUnavailable : ReskWhImport;
                            detailTamperRow["ReskWhExport"] = ReskWhExport == string.Empty ? DateUnavailable : ReskWhExport;
                            detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DateUnavailable : OcckVAhImport;
                            detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DateUnavailable : OcckVAhExport;
                            detailTamperRow["ReskVAhImport"] = ReskVAhImport == string.Empty ? DateUnavailable : ReskVAhImport;
                            detailTamperRow["ReskVAhExport"] = ReskVAhExport == string.Empty ? DateUnavailable : ReskVAhExport;

                            detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DateUnavailable : OcckvarhLag;
                            detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DateUnavailable : OcckvarhLead;
                            detailTamperRow["ReskvarhLag"] = ReskvarhLag == string.Empty ? DateUnavailable : ReskvarhLag;
                            detailTamperRow["ReskvarhLead"] = ReskvarhLead == string.Empty ? DateUnavailable : ReskvarhLead;

                            // SB code change start 20171120
                            detailTamperRow["OccActCurR"] = OccActCurR == string.Empty ? DateUnavailable : OccActCurR;
                            detailTamperRow["ResActCurR"] = ResActCurR == string.Empty ? DateUnavailable : ResActCurR;
                            detailTamperRow["OccActCurY"] = OccActCurY == string.Empty ? DateUnavailable : OccActCurY;
                            detailTamperRow["ResActCurY"] = ResActCurY == string.Empty ? DateUnavailable : ResActCurY;
                            detailTamperRow["OccActCurB"] = OccActCurB == string.Empty ? DateUnavailable : OccActCurB;
                            detailTamperRow["ResActCurB"] = ResActCurB == string.Empty ? DateUnavailable : ResActCurB;
                            // SB code change end 20171120

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            detailTamperRow["OccPhaseCurrentInstant"] = OccPhaseCurrentInstant == string.Empty ? DateUnavailable : OccPhaseCurrentInstant;
                            detailTamperRow["ResPhaseCurrentInstant"] = ResPhaseCurrentInstant == string.Empty ? DateUnavailable : ResPhaseCurrentInstant;
                            detailTamperRow["OccFrequency"] = OccFrequency == string.Empty ? DateUnavailable : OccFrequency;
                            detailTamperRow["ResFrequency"] = ResFrequency == string.Empty ? DateUnavailable : ResFrequency;
                            //SarkarA code change start 20180330
                            detailTamperRow["OccTemprature"] = OccTemprature == string.Empty ? DateUnavailable : OccTemprature;
                            detailTamperRow["ResTemprature"] = ResTemprature == string.Empty ? DateUnavailable : ResTemprature;

                            detailTamperRow["OccTHDVR"] = OccTHDVR == string.Empty ? DateUnavailable : OccTHDVR;
                            detailTamperRow["ResTHDVR"] = ResTHDVR == string.Empty ? DateUnavailable : ResTHDVR;
                            detailTamperRow["OccTHDVY"] = OccTHDVY == string.Empty ? DateUnavailable : OccTHDVY;
                            detailTamperRow["ResTHDVY"] = ResTHDVY == string.Empty ? DateUnavailable : ResTHDVY;
                            detailTamperRow["OccTHDVB"] = OccTHDVB == string.Empty ? DateUnavailable : OccTHDVB;
                            detailTamperRow["ResTHDVB"] = ResTHDVB == string.Empty ? DateUnavailable : ResTHDVB;
                            detailTamperRow["OccTHDIR"] = OccTHDIR == string.Empty ? DateUnavailable : OccTHDIR;
                            detailTamperRow["ResTHDIR"] = ResTHDIR == string.Empty ? DateUnavailable : ResTHDIR;
                            detailTamperRow["OccTHDIY"] = OccTHDIY == string.Empty ? DateUnavailable : OccTHDIY;
                            detailTamperRow["ResTHDIY"] = ResTHDIY == string.Empty ? DateUnavailable : ResTHDIY;
                            detailTamperRow["OccTHDIB"] = OccTHDIB == string.Empty ? DateUnavailable : OccTHDIB;
                            detailTamperRow["ResTHDIB"] = ResTHDIB == string.Empty ? DateUnavailable : ResTHDIB;
                           
                            // case for occurence and restoration both ends
                        }
                        else if (occCounter <= intOccRowCount)
                        {
                            //Filter only those tampers where corresponding phase current is greater than 0.
                            //if (chkVoltageTamperHavingCurrent.Checked)
                            //{
                            //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIR"]) == 0)
                            //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIY"]) == 0)
                            //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIB"]) == 0))
                            //    {
                            //        occCounter++;
                            //        resCounter++;
                            //        continue;
                            //    }

                            //}

                            // case for occurence only starts

                            OccRVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVRN"].ToString());
                            OccYVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVYN"].ToString());
                            OccBVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["VoltageVBN"].ToString());
                            OccPhaseVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseVoltage"].ToString());
                            OccRCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIR"].ToString());
                            OccYCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIY"].ToString());
                            OccBCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CurrentIB"].ToString());
                            OccPhaseCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseCurrent"].ToString());
                            OccNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["NeutralCurrent"].ToString()); // Story - 349654 - Neutral current in Tamper

                            OccHighNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["HighNeutralCurrent"].ToString()); // pradipta_neu

                            OccByPassCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ByPassCurrent"].ToString()); // Story - 349654 - Neutral current in Tamper
                            //SarkarA code change start 20180226 // fix TamperReport for new params
                            OcckWr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWr"].ToString()); // pradipta_neu
                            OcckWy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWy"].ToString()); // pradipta_neu
                            OcckWb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kWb"].ToString()); // pradipta_neu
                            OcckVAr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAr"].ToString()); // pradipta_neu
                            OcckVAy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAy"].ToString()); // pradipta_neu
                            OcckVAb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["kVAb"].ToString()); // pradipta_neu
 
                            OccActCurR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentR"].ToString());
                            OccActCurY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentY"].ToString());
                            OccActCurB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["ActiveCurrentB"].ToString());
                            //SarkarA code change end 20180226

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            OccPhaseCurrentInstant = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["PhaseCurrentInstant"].ToString());
                            OccFrequency = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["Frequency"].ToString());
                            //SarkarA code change end 20180330
                            OccTemprature = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["Temprature"].ToString());

                            OccTHDVR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVR"].ToString());
                            OccTHDVY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVY"].ToString());
                            OccTHDVB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDVB"].ToString());
                            OccTHDIR = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIR"].ToString());
                            OccTHDIY = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIY"].ToString());
                            OccTHDIB = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["THDIB"].ToString());

                            detailTamperRow["OcckWr"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckWr;// pradipta_neu
                            detailTamperRow["ReskWr"] = DateUnavailable;

                            detailTamperRow["OcckWy"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckWy;// pradipta_neu
                            detailTamperRow["ReskWy"] = DateUnavailable;

                            detailTamperRow["OcckWb"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckWb;// pradipta_neu
                            detailTamperRow["ReskWb"] = DateUnavailable;

                            detailTamperRow["OcckVAr"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckVAr;// pradipta_neu
                            detailTamperRow["ReskVAr"] = DateUnavailable;

                            detailTamperRow["OcckVAy"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckVAy;// pradipta_neu
                            detailTamperRow["ReskVAy"] = DateUnavailable;

                            detailTamperRow["OcckVAb"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OcckVAb;// pradipta_neu
                            detailTamperRow["ReskVAb"] = DateUnavailable;

                            detailTamperRow["OccCumulTampCount"] = OccCumulTampCount == string.Empty ? DateUnavailable : OccCumulTampCount;//smart meter
                            detailTamperRow["ResCumulTampCount"] = DateUnavailable;

                            OccRPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorRphase"].ToString();
                            OccYPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorYphase"].ToString();
                            OccBPF = detailedTamperOccData.Tables[0].Rows[occCounter]["PowerFactorBphase"].ToString();
                            OccPF = detailedTamperOccData.Tables[0].Rows[occCounter]["TotalPowerFactor"].ToString();
                            OccKwh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWh"].ToString());
                            OccKVAh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAh"].ToString());
                            OcckWhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWhImport"].ToString());
                            OcckWhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykWhExport"].ToString());
                            OcckVAhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAhImport"].ToString());
                            OcckVAhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykVAhExport"].ToString());
                            OcckvarhLag = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykvarhLag"].ToString());
                            OcckvarhLead = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[occCounter]["CumulativeEnergykvarhLead"].ToString());

                            // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                            //if (strTamperCode == "101")
                            //{
                            //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            //}
                            //else
                            //{
                            //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            //}
                            OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[occCounter]["Time Stamp"]));
                            detailTamperRow["OccEvent"] = detailedTamperOccData.Tables[0].Rows[occCounter]["Event Code"];
                            detailTamperRow["ResEvent"] = DateUnavailable;
                            detailTamperRow["OccDateTime"] = OccDateTime;
                            detailTamperRow["ResDateTime"] = DateUnavailable;
                            detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DateUnavailable : OccRVoltage;
                            detailTamperRow["ResRVoltage"] = DateUnavailable;
                            detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DateUnavailable : OccYVoltage;
                            detailTamperRow["ResYVoltage"] = DateUnavailable;
                            detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DateUnavailable : OccBVoltage;
                            detailTamperRow["ResBVoltage"] = DateUnavailable;
                            detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DateUnavailable : OccPhaseVoltage;
                            detailTamperRow["ResPhaseVoltage"] = DateUnavailable;

                            detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DateUnavailable : OccRCurrent;
                            detailTamperRow["ResRCurrent"] = DateUnavailable;
                            detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DateUnavailable : OccYCurrent;
                            detailTamperRow["ResYCurrent"] = DateUnavailable;
                            detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DateUnavailable : OccBCurrent;
                            detailTamperRow["ResBCurrent"] = DateUnavailable;
                            detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DateUnavailable : OccPhaseCurrent;
                            detailTamperRow["ResPhaseCurrent"] = DateUnavailable;
                            detailTamperRow["OccNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OccNeutralCurrent;// Story - 349654 - Neutral current in Tamper
                            detailTamperRow["ResNeutralCurrent"] = DateUnavailable;

                             
                            detailTamperRow["OccHighNeutralCurrent"] = OccHighNeutralCurrent == string.Empty ? DateUnavailable : OccHighNeutralCurrent; //pradipta_neu
                            detailTamperRow["ResHighNeutralCurrent"] = DateUnavailable;

                            detailTamperRow["OccByPassCurrent"] = OccByPassCurrent == string.Empty ? DateUnavailable : OccByPassCurrent; //pradipta_neu
                            detailTamperRow["ResByPassCurrent"] = DateUnavailable;

                            detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DateUnavailable : OccRPF;
                            detailTamperRow["ResRPF"] = DateUnavailable;
                            detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DateUnavailable : OccYPF;
                            detailTamperRow["ResYPF"] = DateUnavailable;
                            detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DateUnavailable : OccBPF;
                            detailTamperRow["ResBPF"] = DateUnavailable;
                            detailTamperRow["OccPF"] = OccPF == string.Empty ? DateUnavailable : OccPF;
                            detailTamperRow["ResPF"] = DateUnavailable;
                            detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DateUnavailable : OccKwh;
                            detailTamperRow["ResKwh"] = DateUnavailable;
                            detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DateUnavailable : OccKVAh;
                            detailTamperRow["ResKVah"] = DateUnavailable;


                            detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DateUnavailable : OcckWhImport;
                            detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DateUnavailable : OcckWhExport;
                            detailTamperRow["ReskWhImport"] = DateUnavailable;
                            detailTamperRow["ReskWhExport"] = DateUnavailable;
                            detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DateUnavailable : OcckVAhImport;
                            detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DateUnavailable : OcckVAhExport;
                            detailTamperRow["ReskVAhImport"] = DateUnavailable;
                            detailTamperRow["ReskVAhExport"] = DateUnavailable;
                            detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DateUnavailable : OcckvarhLag;
                            detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DateUnavailable : OcckvarhLead;
                            detailTamperRow["ReskvarhLag"] = DateUnavailable;
                            detailTamperRow["ReskvarhLead"] = DateUnavailable;

                            // SB code change start 20171120
                            detailTamperRow["OccActCurR"] = OccActCurR == string.Empty ? DateUnavailable : OccActCurR;
                            detailTamperRow["ResActCurR"] = DateUnavailable;
                            detailTamperRow["OccActCurY"] = OccActCurY == string.Empty ? DateUnavailable : OccActCurY;
                            detailTamperRow["ResActCurY"] = DateUnavailable;
                            detailTamperRow["OccActCurB"] = OccActCurB == string.Empty ? DateUnavailable : OccActCurB;
                            detailTamperRow["ResActCurB"] = DateUnavailable;
                            // SB code change end 20171120

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            detailTamperRow["OccPhaseCurrentInstant"] = OccPhaseCurrentInstant == string.Empty ? DateUnavailable : OccPhaseCurrentInstant;
                            detailTamperRow["ResPhaseCurrentInstant"] = DateUnavailable;
                            detailTamperRow["OccFrequency"] = OccFrequency == string.Empty ? DateUnavailable : OccFrequency;
                            detailTamperRow["ResFrequency"] =  DateUnavailable;
                            //SarkarA code change start 20180330
                            detailTamperRow["OccTemprature"] = OccTemprature == string.Empty ? DateUnavailable : OccTemprature;
                            detailTamperRow["ResTemprature"] = DateUnavailable;

                            detailTamperRow["OccTHDVR"] = OccTHDVR == string.Empty ? DateUnavailable : OccTHDVR;
                            detailTamperRow["ResTHDVR"] = DateUnavailable;
                            detailTamperRow["OccTHDVY"] = OccTHDVY == string.Empty ? DateUnavailable : OccTHDVY;
                            detailTamperRow["ResTHDVY"] = DateUnavailable;
                            detailTamperRow["OccTHDVB"] = OccTHDVB == string.Empty ? DateUnavailable : OccTHDVB;
                            detailTamperRow["ResTHDVB"] = DateUnavailable;
                            detailTamperRow["OccTHDIR"] = OccTHDIR == string.Empty ? DateUnavailable : OccTHDIR;
                            detailTamperRow["ResTHDIR"] = DateUnavailable;
                            detailTamperRow["OccTHDIY"] = OccTHDIY == string.Empty ? DateUnavailable : OccTHDIY;
                            detailTamperRow["ResTHDIY"] = DateUnavailable;
                            detailTamperRow["OccTHDIB"] = OccTHDIB == string.Empty ? DateUnavailable : OccTHDIB;
                            detailTamperRow["ResTHDIB"] = DateUnavailable;
                            
                            // case for occurence only ends
                        }
                        else
                        {
                            // case for Restoration only starts

                            ResRVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVRN"].ToString());
                            ResYVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVYN"].ToString());
                            ResBVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["VoltageVBN"].ToString());
                            ResPhaseVoltage = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseVoltage"].ToString());
                            ResRCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIR"].ToString());
                            ResYCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIY"].ToString());
                            ResBCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CurrentIB"].ToString());
                            ResPhaseCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseCurrent"].ToString());
                            ResNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["NeutralCurrent"].ToString()); // Story - 349654 - Neutral current in Tamper
                            ResByPassCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["ByPassCurrent"].ToString()); // pradipta_neu
                            ResHighNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["HighNeutralCurrent"].ToString()); // pradipta_neu


                            ReskWr = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWr"].ToString()); // pradipta_neu
                            ReskWy = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWy"].ToString()); // pradipta_neu
                            ReskWb = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kWb"].ToString()); // pradipta_neu

                            ReskVAr = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAr"].ToString()); // pradipta_neu
                            ReskVAy = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAy"].ToString()); // pradipta_neu
                            ReskVAb = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["kVAb"].ToString()); // pradipta_neu

                            ResCumulTampCount = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeTamperCount"].ToString()); // smart meter


                            ResRPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorRphase"].ToString();
                            ResYPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorYphase"].ToString();
                            ResBPF = detailedTamperResData.Tables[0].Rows[resCounter]["PowerFactorBphase"].ToString();
                            ResPF = detailedTamperResData.Tables[0].Rows[resCounter]["TotalPowerFactor"].ToString();
                            ResKwh = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykWh"].ToString());
                            ResKVAh = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykVAh"].ToString());
                            ResDateTime = ResDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperResData.Tables[0].Rows[resCounter]["Time Stamp"]));
                            ReskvarhLag = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykvarhLag"].ToString());
                            ReskvarhLead = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["CumulativeEnergykvarhLead"].ToString());

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            ResPhaseCurrentInstant = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["PhaseCurrentInstant"].ToString());
                            ResFrequency = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["Frequency"].ToString());
                            //SarkarA code change end 20180330
                            ResTemprature = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["Temprature"].ToString());

                            ResTHDVR = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVR"].ToString());
                            ResTHDVY = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVY"].ToString());
                            ResTHDVB = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDVB"].ToString());
                            ResTHDIR = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIR"].ToString());
                            ResTHDIY = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIY"].ToString());
                            ResTHDIB = CommonBLL.RemoveUnit(detailedTamperResData.Tables[0].Rows[resCounter]["THDIB"].ToString());



                            detailTamperRow["OccEvent"] = DateUnavailable;
                            detailTamperRow["ResEvent"] = detailedTamperResData.Tables[0].Rows[resCounter]["EventCode"];
                            detailTamperRow["OccDateTime"] = DateUnavailable;
                            detailTamperRow["ResDateTime"] = ResDateTime;
                            detailTamperRow["OccRVoltage"] = DateUnavailable;
                            detailTamperRow["ResRVoltage"] = ResRVoltage == string.Empty ? DateUnavailable : ResRVoltage;
                            detailTamperRow["OccYVoltage"] = DateUnavailable;
                            detailTamperRow["ResYVoltage"] = ResYVoltage == string.Empty ? DateUnavailable : ResYVoltage;
                            detailTamperRow["OccBVoltage"] = DateUnavailable;
                            detailTamperRow["ResBVoltage"] = ResBVoltage == string.Empty ? DateUnavailable : ResBVoltage;
                            detailTamperRow["OccPhaseVoltage"] = DateUnavailable;
                            detailTamperRow["ResPhaseVoltage"] = ResPhaseVoltage == string.Empty ? DateUnavailable : ResPhaseVoltage;
                            detailTamperRow["OccRCurrent"] = DateUnavailable;
                            detailTamperRow["ResRCurrent"] = ResRCurrent == string.Empty ? DateUnavailable : ResRCurrent;
                            detailTamperRow["OccYCurrent"] = DateUnavailable;
                            detailTamperRow["ResYCurrent"] = ResYCurrent == string.Empty ? DateUnavailable : ResYCurrent;
                            detailTamperRow["OccBCurrent"] = DateUnavailable;
                            detailTamperRow["ResBCurrent"] = ResBCurrent == string.Empty ? DateUnavailable : ResBCurrent;
                            detailTamperRow["OccPhaseCurrent"] = DateUnavailable;
                            detailTamperRow["ResPhaseCurrent"] = ResPhaseCurrent == string.Empty ? DateUnavailable : ResPhaseCurrent;
                            detailTamperRow["OccNeutralCurrent"] = DateUnavailable;// Story - 349654 - Neutral current in Tamper
                            detailTamperRow["ResNeutralCurrent"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ResNeutralCurrent;

							detailTamperRow["OccByPassCurrent"] = DateUnavailable;
                            detailTamperRow["ResByPassCurrent"] = ResByPassCurrent == string.Empty ? DateUnavailable : ResByPassCurrent;
                            detailTamperRow["OccHighNeutralCurrent"] = DateUnavailable;// Story - 349654 - Neutral current in Tamper
                            detailTamperRow["ResByPassCurrent"] = ResByPassCurrent == string.Empty ? DateUnavailable : ResByPassCurrent;//add pradipta_neu
                            detailTamperRow["ResHighNeutralCurrent"] = ResHighNeutralCurrent == string.Empty ? DateUnavailable : ResHighNeutralCurrent;//add pradipta_neu

                            detailTamperRow["OcckWr"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskWr"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskWr;

                            detailTamperRow["OcckWy"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskWy"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskWy;

                            detailTamperRow["OcckWb"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskWb"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskWb;


                            detailTamperRow["OcckVAr"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskVAr"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskVAr;

                            detailTamperRow["OcckVAy"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskVAy"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskVAy;

                            detailTamperRow["OcckVAb"] = DateUnavailable;// pradipta_neu
                            detailTamperRow["ReskVAb"] = ResNeutralCurrent == string.Empty ? DateUnavailable : ReskVAb;

                            detailTamperRow["OccCumulTampCount"] = DateUnavailable;// smart meter
                            detailTamperRow["ResCumulTampCount"] = ResCumulTampCount == string.Empty ? DateUnavailable : ResCumulTampCount;


                            detailTamperRow["OccRPF"] = DateUnavailable;
                            detailTamperRow["ResRPF"] = ResRPF == string.Empty ? DateUnavailable : ResRPF;
                            detailTamperRow["OccYPF"] = DateUnavailable;
                            detailTamperRow["ResYPF"] = ResYPF == string.Empty ? DateUnavailable : ResYPF;
                            detailTamperRow["OccBPF"] = DateUnavailable;
                            detailTamperRow["ResBPF"] = ResBPF == string.Empty ? DateUnavailable : ResBPF;
                            detailTamperRow["OccPF"] = DateUnavailable;
                            detailTamperRow["ResPF"] = ResPF == string.Empty ? DateUnavailable : ResPF;
                            detailTamperRow["OccKwh"] = DateUnavailable;
                            detailTamperRow["ResKwh"] = ResKwh == string.Empty ? DateUnavailable : ResKwh;
                            detailTamperRow["OccKVAh"] = DateUnavailable;
                            detailTamperRow["ResKVAh"] = ResKVAh == string.Empty ? DateUnavailable : ResKVAh;


                            detailTamperRow["OcckWhImport"] = DateUnavailable;
                            detailTamperRow["OcckWhExport"] = DateUnavailable; 
                            detailTamperRow["ReskWhImport"] = ReskWhImport == string.Empty ? DateUnavailable : ReskWhImport;
                            detailTamperRow["ReskWhExport"] = ReskWhExport == string.Empty ? DateUnavailable : ReskWhExport;
                            detailTamperRow["OcckVAhImport"] = DateUnavailable;
                            detailTamperRow["OcckVAhExport"] = DateUnavailable; 
                            detailTamperRow["ReskVAhImport"] = ReskVAhImport == string.Empty ? DateUnavailable : ReskVAhImport;
                            detailTamperRow["ReskVAhExport"] = ReskVAhExport == string.Empty ? DateUnavailable : ReskVAhExport;

                            detailTamperRow["OcckvarhLag"] = DateUnavailable;
                            detailTamperRow["OcckvarhLead"] = DateUnavailable;
                            detailTamperRow["ReskvarhLag"] = ReskvarhLag == string.Empty ? DateUnavailable : ReskvarhLag;
                            detailTamperRow["ReskvarhLead"] = ReskvarhLead == string.Empty ? DateUnavailable : ReskvarhLead;

                            // SB code change start 20171120
                            detailTamperRow["OccActCurR"] = DateUnavailable;
                            detailTamperRow["ResActCurR"] = ResActCurR == string.Empty ? DateUnavailable : ResActCurR;
                            detailTamperRow["OccActCurY"] = DateUnavailable;
                            detailTamperRow["ResActCurY"] = ResActCurY == string.Empty ? DateUnavailable : ResActCurY;
                            detailTamperRow["OccActCurB"] = DateUnavailable;
                            detailTamperRow["ResActCurB"] = ResActCurB == string.Empty ? DateUnavailable : ResActCurB;
                            // SB code change end 20171120

                            //SarkarA code change start 20180330 // add phase current instant, frequency/end
                            detailTamperRow["OccPhaseCurrentInstant"] = DateUnavailable;
                            detailTamperRow["ResPhaseCurrentInstant"] = ResPhaseCurrentInstant == string.Empty ? DateUnavailable : ResPhaseCurrentInstant;
                            detailTamperRow["OccFrequency"] = DateUnavailable;
                            detailTamperRow["ResFrequency"] = ResFrequency == string.Empty ? DateUnavailable : ResFrequency;
                            //SarkarA code change start 20180330
                            detailTamperRow["OccTemprature"] = DateUnavailable;
                            detailTamperRow["ResTemprature"] = ResTemprature == string.Empty ? DateUnavailable : ResTemprature;

                            detailTamperRow["OccTHDVR"] = DateUnavailable;
                            detailTamperRow["ResTHDVR"] = ResTHDVR == string.Empty ? DateUnavailable : ResTHDVR;
                            detailTamperRow["OccTHDVY"] = DateUnavailable;
                            detailTamperRow["ResTHDVY"] = ResTHDVY == string.Empty ? DateUnavailable : ResTHDVY;
                            detailTamperRow["OccTHDVB"] = DateUnavailable;
                            detailTamperRow["ResTHDVB"] = ResTHDVB == string.Empty ? DateUnavailable : ResTHDVB;
                            detailTamperRow["OccTHDIR"] = DateUnavailable;
                            detailTamperRow["ResTHDIR"] = ResTHDIR == string.Empty ? DateUnavailable : ResTHDIR;
                            detailTamperRow["OccTHDIY"] = DateUnavailable;
                            detailTamperRow["ResTHDIY"] = ResTHDIY == string.Empty ? DateUnavailable : ResTHDIY;
                            detailTamperRow["OccTHDIB"] = DateUnavailable;
                            detailTamperRow["ResTHDIB"] = ResTHDIB == string.Empty ? DateUnavailable : ResTHDIB;
                            // case for Restoration only ends
                        }

                        detailTamperData.Tables[0].Rows.Add(detailTamperRow);
                        occCounter++;
                        resCounter++;
                    }

                }
                else if (detailedTamperOccData != null && detailedTamperOccData.Tables.Count > 0)
                {
                    int intOccRowCount = detailedTamperOccData.Tables[0].Rows.Count;
                    string strTamperCode = string.Empty;
                    for (int i = 0; i < intOccRowCount; i++)
                    {
                        DataRow detailTamperRow = detailTamperData.Tables[0].NewRow();
                        strTamperCode = detailedTamperOccData.Tables[0].Rows[i]["Event Code"].ToString();
                        if (dictTamperCodeAndAbbreviation.ContainsKey(strTamperCode))
                        {
                            detailTamperRow["Description"] = dictTamperCodeAndAbbreviation[strTamperCode];
                        }
                        else
                        {
                            detailTamperRow["Description"] = DateUnavailable;
                        }
                        //Filter only those tampers where corresponding phase current is greater than 0.
                        //if (chkVoltageTamperHavingCurrent.Checked)
                        //{
                        //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[i]["CurrentIR"]) == 0)
                        //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[i]["CurrentIY"]) == 0)
                        //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData.Tables[0].Rows[i]["CurrentIB"]) == 0))
                        //    {
                        //        continue;
                        //    }

                        //}
                        OccRVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["VoltageVRN"].ToString());
                        OccYVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["VoltageVYN"].ToString());
                        OccBVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["VoltageVBN"].ToString());
                        OccPhaseVoltage = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["PhaseVoltage"].ToString());
                        OccRCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CurrentIR"].ToString());
                        OccYCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CurrentIY"].ToString());
                        OccBCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CurrentIB"].ToString());
                        OccPhaseCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["PhaseCurrent"].ToString());
                        OccNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["NeutralCurrent"].ToString());// Story - 349654 - Neutral current in Tamper

                        OccHighNeutralCurrent = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["HighNeutralCurrent"].ToString());// pradipta_neu

                        OcckWr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kWr"].ToString()); // pradipta_neu
                       

                        OcckWy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kWy"].ToString()); // pradipta_neu
                       

                        OcckWb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kWb"].ToString()); // pradipta_neu
                      

                        OcckVAr = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kVAr"].ToString()); // pradipta_neu


                        OcckVAy = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kVAy"].ToString()); // pradipta_neu
                       

                        OcckVAb = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["kVAb"].ToString()); // pradipta_neu

                        OccCumulTampCount = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeTamperCount"].ToString()); // smart meter

                        OccRPF = detailedTamperOccData.Tables[0].Rows[i]["PowerFactorRphase"].ToString();
                        OccYPF = detailedTamperOccData.Tables[0].Rows[i]["PowerFactorYphase"].ToString();
                        OccBPF = detailedTamperOccData.Tables[0].Rows[i]["PowerFactorBphase"].ToString();
                        OccPF = detailedTamperOccData.Tables[0].Rows[i]["TotalPowerFactor"].ToString();
                        OccKwh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykWh"].ToString());
                        OccKVAh = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykVAh"].ToString());

                        OcckWhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykWhImport"].ToString());
                        OcckVAhImport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykVAhImport"].ToString());
                        OcckWhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykWhExport"].ToString());
                        OcckVAhExport = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykVAhExport"].ToString());

                        OcckvarhLag = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykvarhLag"].ToString());
                        OcckvarhLead = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["CumulativeEnergykvarhLead"].ToString());

                        //SarkarA code change start 20180330 // add phase current instant, frequency/end
                        OccPhaseCurrentInstant = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["PhaseCurrentInstant"].ToString());
                        OccFrequency = CommonBLL.RemoveUnit(detailedTamperOccData.Tables[0].Rows[i]["Frequency"].ToString());
                        //SarkarA code change end 20180330

                        // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                        //if (strTamperCode == "101")
                        //{
                        //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[i]["Time Stamp"]));
                        //}
                        //else
                        //{
                        //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[i]["Time Stamp"]));
                        //}
                        OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData.Tables[0].Rows[i]["Time Stamp"]));

                        detailTamperRow["OccEvent"] = detailedTamperOccData.Tables[0].Rows[i]["Event Code"];
                        detailTamperRow["ResEvent"] = DateUnavailable;
                        detailTamperRow["OccDateTime"] = OccDateTime;
                        detailTamperRow["ResDateTime"] = DateUnavailable;
                        detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DateUnavailable : OccRVoltage;
                        detailTamperRow["ResRVoltage"] = DateUnavailable;
                        detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DateUnavailable : OccYVoltage;
                        detailTamperRow["ResYVoltage"] = DateUnavailable;
                        detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DateUnavailable : OccBVoltage;
                        detailTamperRow["ResBVoltage"] = DateUnavailable;
                        detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DateUnavailable : OccPhaseVoltage;
                        detailTamperRow["ResPhaseVoltage"] = DateUnavailable;
                        detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DateUnavailable : OccRCurrent;
                        detailTamperRow["ResRCurrent"] = DateUnavailable;
                        detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DateUnavailable : OccYCurrent;
                        detailTamperRow["ResYCurrent"] = DateUnavailable;
                        detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DateUnavailable : OccBCurrent;
                        detailTamperRow["ResBCurrent"] = DateUnavailable;
                        detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DateUnavailable : OccPhaseCurrent;
                        detailTamperRow["ResPhaseCurrent"] = DateUnavailable;
                     
                        detailTamperRow["OccByPassCurrent"] = OccNeutralCurrent == string.Empty ? DateUnavailable : OccNeutralCurrent; 
                        detailTamperRow["ResByPassCurrent"] = DateUnavailable;

                        detailTamperRow["OccHighNeutralCurrent"] = OccHighNeutralCurrent == string.Empty ? DateUnavailable : OccHighNeutralCurrent; 
                        detailTamperRow["ResHighNeutralCurrent"] = DateUnavailable;


                        detailTamperRow["OcckWr"] = OcckWr == string.Empty ? DateUnavailable : OcckWr; 
                        detailTamperRow["ReskWr"] = ReskWr == string.Empty ? DateUnavailable : ReskWr;

                        detailTamperRow["OcckWy"] = OcckWy == string.Empty ? DateUnavailable : OcckWy; 
                        detailTamperRow["ReskWy"] = ReskWy == string.Empty ? DateUnavailable : ReskWy;

                        detailTamperRow["OcckWb"] = OcckWb == string.Empty ? DateUnavailable : OcckWb; 
                        detailTamperRow["ReskWb"] = ReskWb == string.Empty ? DateUnavailable : ReskWb;

                        detailTamperRow["OcckVAr"] = OcckVAr == string.Empty ? DateUnavailable : OcckVAr; 
                        detailTamperRow["ReskVAr"] = ReskVAr == string.Empty ? DateUnavailable : ReskVAr;

                        detailTamperRow["OcckVAy"] = OcckVAy == string.Empty ? DateUnavailable : OcckVAy; 
                        detailTamperRow["ReskVAy"] = ReskVAy == string.Empty ? DateUnavailable : ReskVAy;

                        detailTamperRow["OcckVAb"] = OcckVAb == string.Empty ? DateUnavailable : OcckVAb; 
                        detailTamperRow["ReskVAb"] = ReskVAb == string.Empty ? DateUnavailable : ReskVAb;
                        detailTamperRow["OccCumulTampCount"] = OccCumulTampCount == string.Empty ? DateUnavailable : OccCumulTampCount; //smart meter
                        detailTamperRow["ResCumulTampCount"] = ResCumulTampCount == string.Empty ? DateUnavailable : ResCumulTampCount;


                        detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DateUnavailable : OccRPF;
                        detailTamperRow["ResRPF"] = DateUnavailable;
                        detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DateUnavailable : OccYPF;
                        detailTamperRow["ResYPF"] = DateUnavailable;
                        detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DateUnavailable : OccBPF;
                        detailTamperRow["ResBPF"] = DateUnavailable;
                        detailTamperRow["OccPF"] = OccPF == string.Empty ? DateUnavailable : OccPF;
                        detailTamperRow["ResPF"] = DateUnavailable;
                        detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DateUnavailable : OccKwh;
                        detailTamperRow["ResKwh"] = DateUnavailable;
                        detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DateUnavailable : OccKVAh;
                        detailTamperRow["ResKVAh"] = DateUnavailable;

                        detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DateUnavailable : OcckWhImport;
                        detailTamperRow["ReskWhImport"] = DateUnavailable;
                        detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DateUnavailable : OcckVAhImport;
                        detailTamperRow["ReskVAhImport"] = DateUnavailable;

                        detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DateUnavailable : OcckWhExport;
                        detailTamperRow["ReskWhExport"] = DateUnavailable;
                        detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DateUnavailable : OcckVAhExport;
                        detailTamperRow["ReskVAhExport"] = DateUnavailable;

                        detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DateUnavailable : OcckvarhLag;
                        detailTamperRow["ReskvarhLag"] = DateUnavailable;
                        detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DateUnavailable : OcckvarhLead;
                        detailTamperRow["ReskvarhLead"] = DateUnavailable;

                        // SB code change start 20171120
                        detailTamperRow["OccActCurR"] = OccActCurR == string.Empty ? DateUnavailable : OccActCurR;
                        detailTamperRow["ResActCurR"] = ResActCurR == string.Empty ? DateUnavailable : ResActCurR;
                        detailTamperRow["OccActCurY"] = OccActCurY == string.Empty ? DateUnavailable : OccActCurY;
                        detailTamperRow["ResActCurY"] = ResActCurY == string.Empty ? DateUnavailable : ResActCurY;
                        detailTamperRow["OccActCurB"] = OccActCurB == string.Empty ? DateUnavailable : OccActCurB;
                        detailTamperRow["ResActCurB"] = ResActCurB == string.Empty ? DateUnavailable : ResActCurB;
                        // SB code change end 20171120

                        //SarkarA code change start 20180330 // add phase current instant, frequency/end
                        detailTamperRow["OccPhaseCurrentInstant"] = OccPhaseCurrentInstant == string.Empty ? DateUnavailable : OccPhaseCurrentInstant;
                        detailTamperRow["ResPhaseCurrentInstant"] = DateUnavailable;
                        detailTamperRow["OccFrequency"] = OccFrequency == string.Empty ? DateUnavailable : OccFrequency;
                        detailTamperRow["ResFrequency"] = DateUnavailable;
                        //SarkarA code change start 20180330
                        detailTamperRow["OccTemprature"] = OccTemprature == string.Empty ? DateUnavailable : OccTemprature;
                        detailTamperRow["ResTemprature"] = ResTemprature == string.Empty ? DateUnavailable : ResTemprature;
                        //Deep 25-02-19
                        detailTamperRow["OccTHDVR"] = OccTHDVR == string.Empty ? DateUnavailable : OccTHDVR;
                        detailTamperRow["ResTHDVR"] = ResTHDVR == string.Empty ? DateUnavailable : ResTHDVR;
                        detailTamperRow["OccTHDVY"] = OccTHDVY == string.Empty ? DateUnavailable : OccTHDVY;
                        detailTamperRow["ResTHDVY"] = ResTHDVY == string.Empty ? DateUnavailable : ResTHDVY;
                        detailTamperRow["OccTHDVB"] = OccTHDVB == string.Empty ? DateUnavailable : OccTHDVB;
                        detailTamperRow["ResTHDVB"] = ResTHDVB == string.Empty ? DateUnavailable : ResTHDVB;
                        detailTamperRow["OccTHDIR"] = OccTHDIR == string.Empty ? DateUnavailable : OccTHDIR;
                        detailTamperRow["ResTHDIR"] = ResTHDIR == string.Empty ? DateUnavailable : ResTHDIR;
                        detailTamperRow["OccTHDIY"] = OccTHDIY == string.Empty ? DateUnavailable : OccTHDIY;
                        detailTamperRow["ResTHDIY"] = ResTHDIY == string.Empty ? DateUnavailable : ResTHDIY;
                        detailTamperRow["OccTHDIB"] = OccTHDIB == string.Empty ? DateUnavailable : OccTHDIB;
                        detailTamperRow["ResTHDIB"] = ResTHDIB == string.Empty ? DateUnavailable : ResTHDIB;
                        
                        
                        detailTamperData.Tables[0].Rows.Add(detailTamperRow);
                    }
                }
            }
        }
        /// <summary>
        /// Creates detail tamper data set
        /// </summary>
        private void CreateDetailTamperDataSet()
        {
            detailTamperData = new DataSet();
            DataTable tamperData = new DataTable();
            tamperData.Columns.Add("Description", typeof(string));
            tamperData.Columns.Add("OccEvent", typeof(string));
            tamperData.Columns.Add("ResEvent", typeof(string));
            tamperData.Columns.Add("OccDateTime", typeof(string));
            tamperData.Columns.Add("ResDateTime", typeof(string));
            tamperData.Columns.Add("OccRVoltage", typeof(string));
            tamperData.Columns.Add("ResRVoltage", typeof(string));
            tamperData.Columns.Add("OccYVoltage", typeof(string));
            tamperData.Columns.Add("ResYVoltage", typeof(string));
            tamperData.Columns.Add("OccBVoltage", typeof(string));
            tamperData.Columns.Add("ResBVoltage", typeof(string));
            tamperData.Columns.Add("OccPhaseVoltage", typeof(string));
            tamperData.Columns.Add("ResPhaseVoltage", typeof(string));
            tamperData.Columns.Add("OccRCurrent", typeof(string));
            tamperData.Columns.Add("ResRCurrent", typeof(string));
            tamperData.Columns.Add("OccYCurrent", typeof(string));
            tamperData.Columns.Add("ResYCurrent", typeof(string));
            tamperData.Columns.Add("OccBCurrent", typeof(string));
            tamperData.Columns.Add("ResBCurrent", typeof(string));
            tamperData.Columns.Add("OccPhaseCurrent", typeof(string));
            tamperData.Columns.Add("ResPhaseCurrent", typeof(string));
            tamperData.Columns.Add("OccNeutralCurrent", typeof(string));// Story - 349654 - Neutral current in Tamper
            tamperData.Columns.Add("ResNeutralCurrent", typeof(string));

            tamperData.Columns.Add("OccByPassCurrent", typeof(string));
            tamperData.Columns.Add("ResByPassCurrent", typeof(string));

            tamperData.Columns.Add("OccHighNeutralCurrent", typeof(string));//pradipta_neu
            tamperData.Columns.Add("ResHighNeutralCurrent", typeof(string));

            tamperData.Columns.Add("OcckWr", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWr", typeof(string));

            tamperData.Columns.Add("OcckWy", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWy", typeof(string));

            tamperData.Columns.Add("OcckWb", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWb", typeof(string));

            tamperData.Columns.Add("OcckVAr", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAr", typeof(string));

            tamperData.Columns.Add("OcckVAy", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAy", typeof(string));

            tamperData.Columns.Add("OcckVAb", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAb", typeof(string));

            tamperData.Columns.Add("OccCumulTampCount", typeof(string));//smart meter
            tamperData.Columns.Add("ResCumulTampCount", typeof(string));


            tamperData.Columns.Add("OccRPF", typeof(string));
            tamperData.Columns.Add("ResRPF", typeof(string));
            tamperData.Columns.Add("OccYPF", typeof(string));
            tamperData.Columns.Add("ResYPF", typeof(string));
            tamperData.Columns.Add("OccBPF", typeof(string));
            tamperData.Columns.Add("ResBPF", typeof(string));
            tamperData.Columns.Add("OccPF", typeof(string));
            tamperData.Columns.Add("ResPF", typeof(string));
            tamperData.Columns.Add("OccKwh", typeof(string));
            tamperData.Columns.Add("ResKwh", typeof(string));
            tamperData.Columns.Add("OccKVAh", typeof(string));
            tamperData.Columns.Add("ResKVAh", typeof(string));
            tamperData.Columns.Add("OcckWhImport", typeof(string));
            tamperData.Columns.Add("OcckWhExport", typeof(string));
            tamperData.Columns.Add("ReskWhImport", typeof(string));
            tamperData.Columns.Add("ReskWhExport", typeof(string));
            tamperData.Columns.Add("OcckVAhImport", typeof(string));
            tamperData.Columns.Add("OcckVAhExport", typeof(string));
            tamperData.Columns.Add("ReskVAhImport", typeof(string));
            tamperData.Columns.Add("ReskVAhExport", typeof(string));

            tamperData.Columns.Add("OcckvarhLag", typeof(string));
            tamperData.Columns.Add("OcckvarhLead", typeof(string));
            tamperData.Columns.Add("ReskvarhLag", typeof(string));
            tamperData.Columns.Add("ReskvarhLead", typeof(string));

            // SB code change start 20171116
            tamperData.Columns.Add("OccActCurR", typeof(string));
            tamperData.Columns.Add("ResActCurR", typeof(string));
            tamperData.Columns.Add("OccActCurY", typeof(string));
            tamperData.Columns.Add("ResActCurY", typeof(string));
            tamperData.Columns.Add("OccActCurB", typeof(string));
            tamperData.Columns.Add("ResActCurB", typeof(string));
            // SB code change end 20171116

            //SarkarA code change start 20180330 // add phase current instant, frequency/end
            tamperData.Columns.Add("OccFrequency", typeof(string));
            tamperData.Columns.Add("ResFrequency", typeof(string));
            tamperData.Columns.Add("OccPhaseCurrentInstant", typeof(string));
            tamperData.Columns.Add("ResPhaseCurrentInstant", typeof(string));
            //SarkarA code change end 20180330
            //Deep 25-02-19
            tamperData.Columns.Add("OccTemprature", typeof(string));
            tamperData.Columns.Add("ResTemprature", typeof(string));
            tamperData.Columns.Add("OccTHDVR", typeof(string));
            tamperData.Columns.Add("ResTHDVR", typeof(string));
            tamperData.Columns.Add("OccTHDVY", typeof(string));
            tamperData.Columns.Add("ResTHDVY", typeof(string));
            tamperData.Columns.Add("OccTHDVB", typeof(string));
            tamperData.Columns.Add("ResTHDVB", typeof(string));
            tamperData.Columns.Add("OccTHDIR", typeof(string));
            tamperData.Columns.Add("ResTHDIR", typeof(string));
            tamperData.Columns.Add("OccTHDIY", typeof(string));
            tamperData.Columns.Add("ResTHDIY", typeof(string));
            tamperData.Columns.Add("OccTHDIB", typeof(string));
            tamperData.Columns.Add("ResTHDIB", typeof(string));
          

            detailTamperData.Tables.Add(tamperData);
        }
        /// <summary>
        /// Fill consumer meterdetail table for report header
        /// </summary>
        /// <param name="detailsDS"></param>
        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            DataRow reportRow;
            DataTable table = new DataTable();
            if (detailsDS.Tables[0].Rows.Count > 0)
            {
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                foreach (DataRow row in detailsDS.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                        reportRow["MeterNo"] = row["MeterID"].ToString();
                    else
                        reportRow["MeterNo"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                        reportRow["ConsumerNo"] = CommonBLL.GetFormattedData(row["Consumer_Number"].ToString());
                    else
                        reportRow["ConsumerNo"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_Location"].ToString()))
                        reportRow["Location"] = CommonBLL.GetFormattedData(row["Meter_Location"].ToString());
                    else
                        reportRow["Location"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_AllocationDate"].ToString()))
                        reportRow["InstallationDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
                    else
                        reportRow["InstallationDate"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterType_Name"].ToString()))
                        reportRow["MeterType"] = CommonBLL.GetFormattedData(row["MeterType_Name"].ToString());
                    else
                        reportRow["MeterType"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterModel_Name"].ToString()))
                        reportRow["MeterModel"] = CommonBLL.GetFormattedData(row["MeterModel_Name"].ToString());
                    else
                        reportRow["MeterModel"] = DateUnavailable;

                    /* GKG 13/02/2013 JDVVNL Utility Addition */
                    //if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                    //    reportRow["EMF"] = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                    //else
                    //    reportRow["EMF"] = dateUnavailable;
                    //decimal actualEMF = 0;
                    //int internalCTRatio = 0;
                    //int internalPTRatio = 0;

                    //String meterEMF = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());

                    //if (int.TryParse(CommonBLL.GetFormattedData(row["internalCTRatio"].ToString()), out internalCTRatio)
                    //    && int.TryParse(CommonBLL.GetFormattedData(row["internalPTRatio"].ToString()), out internalPTRatio))
                    //{
                    //    //if (internalCTRatio <= 0)
                    //    //{
                    //    //    internalCTRatio = 1;
                    //    //}
                    //    //if (internalPTRatio <= 0)
                    //    //{
                    //    //    internalPTRatio = 1;
                    //    //}
                    //}
                    //if (internalCTRatio <= 0)
                    //{
                    //    internalCTRatio = 1;
                    //}
                    //if (internalPTRatio <= 0)
                    //{
                    //    internalPTRatio = 1;
                    //}
                    //actualEMF = Convert.ToDecimal(meterEMF) / (internalPTRatio * internalCTRatio);
                    /* VBM - EMF Bug Fixed */
                    string meterEMF = CommonBLL.CalculateActualEMF(Convert.ToDecimal(row["Meter_EMF"].ToString()),
                                                                      row["internalCTRatio"].ToString(),
                                                                      row["internalPTRatio"].ToString());
                    /* VBM - EMF Bug Fixed */
                    //meterEMF = actualEMF.ToString();

                    string emfApplied = CommonBLL.GetFormattedData(row["UseEMFInCalculations"].ToString());
                    if (emfApplied == "1")
                    {
                        emfApplied = Applied;
                    }
                    else
                    {
                        emfApplied = NotApplied;
                    }
                    meterEMF = meterEMF + " (" + emfApplied + ")";

                    if (!string.IsNullOrEmpty(meterEMF))
                        reportRow["EMF"] = meterEMF;
                    else
                        reportRow["EMF"] = DateUnavailable;
                    /* GKG 13/02/2013 JDVVNL Utility Addition */

                    if (!string.IsNullOrEmpty(row["Region_Name"].ToString()))
                        reportRow["Region"] = CommonBLL.GetFormattedData(row["Region_Name"].ToString());
                    else
                        reportRow["Region"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["Circle_Name"].ToString()))
                        reportRow["Circle"] = CommonBLL.GetFormattedData(row["Circle_Name"].ToString());
                    else
                        reportRow["Circle"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["Division_Name"].ToString()))
                        reportRow["Division"] = CommonBLL.GetFormattedData(row["Division_Name"].ToString());
                    else
                        reportRow["Division"] = DateUnavailable;

                    if (!string.IsNullOrEmpty(row["CMRI_Number"].ToString()))
                        reportRow["CMRINumber"] = CommonBLL.GetFormattedData(row["CMRI_Number"].ToString());
                    else
                        reportRow["CMRINumber"] = DateUnavailable;

                    if (row["Status"].ToString().Equals("0"))
                        reportRow["ActiveMeter"] = "Inactive";
                    else
                        reportRow["ActiveMeter"] = "Active";

                    if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                        reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                    else
                        reportRow["ContractDemand"] = DateUnavailable;

                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                }
            }
        }
        /// <summary>
        /// Used to split values and units
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string SplitWithOutUnit(string data)
        {
            string value = data;
            if (data.IndexOf('*') > 0)
            {
                string[] val = data.Split('*');
                value = val[0] + " " + val[1];
            }
            return value;
        }

        /// <summary>
        /// To close tamper report window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handle checked change of checkbox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckAllTheElementsInGrid(chkSelectAll.Checked);
        }
        /// <summary>
        /// Select/ unselect all elements in grid view
        /// </summary>
        /// <param name="dgvTemp"></param>
        private void CheckAllTheElementsInGrid(bool isChecked)
        {
            // dgvTamperOccurence.CellValueChanged-=new DataGridViewCellEventHandler(dgvTamperOccurence_CellValueChanged);
            if (isChecked)
            {
                foreach (DataGridViewRow row in dgvTamperOccurence.Rows)
                {
                    row.Cells["Select"].Value = true;
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvTamperOccurence.Rows)
                {
                    row.Cells["Select"].Value = false;
                }
            }
            // dgvTamperOccurence.CellValueChanged += new DataGridViewCellEventHandler(dgvTamperOccurence_CellValueChanged);
        }
        /// <summary>
        /// Used to fill Coumlative tamper counter XSD
        /// </summary>
        /// <param name="loadSurveyData"></param>
        private void FillTamperCounterXSD(DataSet tamperCounterData)
        {

            try
            {
                Dictionary<string, TimeSpan> durationDict = new Dictionary<string, TimeSpan>();
                string idTamper = string.Empty;
                DateTime occDate, resDate;
                foreach (DataRow row in detailTamperData.Tables[0].Rows)
                {
                    idTamper = row["OccEvent"].ToString();
                    if (!durationDict.ContainsKey(idTamper))
                    {
                        durationDict.Add(idTamper, TimeSpan.Zero);
                    }

                    if (DateTime.TryParse(row["ResDateTime"].ToString(), out resDate) && DateTime.TryParse(row["OccDateTime"].ToString(), out occDate))
                    {
                        durationDict[idTamper] += resDate - occDate;
                    }
                }

                DataRow reportRow;
                foreach (DataRow row in tamperCounterData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["CoumulativeTamperCounter"].NewRow();
                    reportRow["S. No."] = row["S.No."].ToString();
                    reportRow["Tamper Id"] = row["TamperId"].ToString();
                    reportRow["Description"] = row["Description"].ToString();
                    reportRow["Count"] = row["Count"].ToString();

                    reportRow["Duration"] = CommonBLL.TimeSpanToReadableString(durationDict[row["TamperId"].ToString()]);

                    reportXSD.Tables["CoumulativeTamperCounter"].Rows.Add(reportRow);
                }    
            }
            catch (Exception ex)
            {
                logger.Log(Hunt.EPIC.Logging.LOGLEVELS.Error, "FillTamperCounterXSD(DataSet tamperCounterData)", ex);
            }
        }
        /// <summary>
        /// Used to fill Coumlative tamper counter XSD
        /// </summary>
        /// <param name="loadSurveyData"></param>
        
        private void FillDetailedTamperXSD(DataSet detailedTamperData)
        {
            try
            {
                //DataRow reportRow;
                //foreach (DataRow row in detailedTamperData.Tables[0].Rows)
                //{
                //    reportRow = reportXSD.Tables["DetailedTamperTable"].NewRow();
                //    reportRow["Description"] = row["Description"].ToString();
                //    reportRow["OccEvent"] = row["OccEvent"].ToString();
                //    reportRow["ResEvent"] = row["ResEvent"].ToString();
                //    reportRow["OccDateTime"] = row["OccDateTime"];
                //    reportRow["ResDateTime"] = row["ResDateTime"];
                //    reportRow["Duration"] = GetTamperDuration(row["OccDateTime"].ToString(), row["ResDateTime"].ToString());
                //    reportRow["OccRVoltage"] = row["OccRVoltage"].ToString();
                //    reportRow["ResRVoltage"] = row["ResRVoltage"].ToString();
                //    reportRow["OccYVoltage"] = row["OccYVoltage"].ToString();
                //    reportRow["ResYVoltage"] = row["ResYVoltage"].ToString();
                //    reportRow["OccBVoltage"] = row["OccBVoltage"].ToString();
                //    reportRow["ResBVoltage"] = row["ResBVoltage"].ToString();
                //    reportRow["OccRCurrent"] = row["OccRCurrent"].ToString();
                //    reportRow["ResRCurrent"] = row["ResRCurrent"].ToString();
                //    reportRow["OccYCurrent"] = row["OccYCurrent"].ToString();
                //    reportRow["ResYCurrent"] = row["ResYCurrent"].ToString();
                //    reportRow["OccBCurrent"] = row["OccBCurrent"].ToString();
                //    reportRow["ResBCurrent"] = row["ResBCurrent"].ToString();
                //    reportRow["OccRPF"] = row["OccRPF"].ToString();
                //    reportRow["ResRPF"] = row["ResRPF"].ToString();
                //    reportRow["OccYPF"] = row["OccYPF"].ToString();
                //    reportRow["ResYPF"] = row["ResYPF"].ToString();
                //    reportRow["OccBPF"] = row["OccBPF"].ToString();
                //    reportRow["ResBPF"] = row["ResBPF"].ToString();
                //    reportRow["OccKwh"] = row["OccKwh"].ToString() == DateUnavailable ? row["OccKwh"].ToString() : decimal.Parse(row["OccKwh"].ToString()).ToString();
                //    reportRow["ResKwh"] = row["ResKwh"].ToString() == DateUnavailable ? row["ResKwh"].ToString() : decimal.Parse(row["ResKwh"].ToString()).ToString();
                //    reportRow["OccKVAh"] = row["OccKVAh"].ToString() == DateUnavailable ? row["OccKVAh"].ToString() : decimal.Parse(row["OccKVAh"].ToString()).ToString();
                //    reportRow["ResKVAh"] = row["ResKVAh"].ToString() == DateUnavailable ? row["ResKVAh"].ToString() : decimal.Parse(row["ResKVAh"].ToString()).ToString();
                //    reportXSD.Tables["DetailedTamperTable"].Rows.Add(reportRow);
                //}

                TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                DataRow newReportRow;
                DataSet dataSet = new DataSet();
                tamperHeadings = new List<string>();
                dataSet = tamperParameterBLL.GetColumnNames(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 && dataSet.Tables[0].Columns.Count > 0 && Convert.ToString(dataSet.Tables[0].Rows[0][0]) != string.Empty)
                {
                    
                    List<string> lstClmNamesOrig = new List<string>(dataSet.Tables[0].Rows[0][0].ToString().Split(','));
                    List<string> lstClmNames = new List<string>();
                    foreach (string item in lstClmNamesOrig)
                    {
                        switch (item)
                        {       case "CumulativeEnergykWhImport":
                                lstClmNames.Add("CumulativeEnergykWhForward");
                                break;
                                case "CumulativeEnergykVAhImport":
                                lstClmNames.Add("CumulativeEnergykVAhForward");
                                break;
                                default:
                                lstClmNames.Add(item);
                                break;
                        }
                    }
                    if (meterModelNumber == 28 || meterModelNumber == 29)
                    {
                        string temp = dataSet.Tables[0].Rows[0][0].ToString().Replace("CumulativeEnergykWh", "CumulativeEnergyMWh").Replace("CumulativeEnergykVAh", "CumulativeEnergyMVAh");
                        lstClmNames = new List<string>(temp.Split(','));

                    }
                    
                    NetReportCheckListPopUp netReportCheckListPopUp = new NetReportCheckListPopUp(lstClmNames, "Tamper Check List", 11);
                    netReportCheckListPopUp.ShowDialog();
                    netReportCheckListPopUp.Dispose();
                    DataTable dtTableWithDesiredColumnSelected = netReportCheckListPopUp.GetSelectedDataTable();
                    if (dtTableWithDesiredColumnSelected != null)
                    {
                        foreach (DataColumn item in dtTableWithDesiredColumnSelected.Columns)
                        {
                            tamperHeadings.Add(item.ColumnName);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                List<string> newTamperList = new List<string>();
                newTamperList.Add("Description");
                newTamperList.Add("OccEvent");
                newTamperList.Add("ResEvent");
                newTamperList.Add("OccDateTime");
                newTamperList.Add("ResDateTime");
                if ( tamperHeadings.Count > 0 &&   !string.IsNullOrEmpty(tamperHeadings[0])) // Story - 354382 - If Tamper data is not avialable for meters or not in proper format
                {
                    for (int selectedItemCount = 0; selectedItemCount < tamperHeadings.Count; selectedItemCount++)
                    {
                        if (tamperHeadings[selectedItemCount].ToString() == "CurrentIR")
                        {
                            newTamperList.Add("OccRCurrent");
                            newTamperList.Add("ResRCurrent");
                        }
                        else
                            if (tamperHeadings[selectedItemCount].ToString() == "CurrentIY")
                            {
                                newTamperList.Add("OccYCurrent");
                                newTamperList.Add("ResYCurrent");
                            }
                            else
                                if (tamperHeadings[selectedItemCount].ToString() == "CurrentIB")
                                {
                                    newTamperList.Add("OccBCurrent");
                                    newTamperList.Add("ResBCurrent");
                                }
                                else
                                    if (tamperHeadings[selectedItemCount].ToString() == "PhaseCurrent")
                                    {
                                        newTamperList.Add("OccPhaseCurrent");
                                        newTamperList.Add("ResPhaseCurrent");
                                    }
                                    else
                                        if (tamperHeadings[selectedItemCount].ToString() == "NeutralCurrent") // Story - 349654 - Neutral current in Tamper
                                        {
                                            newTamperList.Add("OccNeutralCurrent");
                                            newTamperList.Add("ResNeutralCurrent");
                                        }
                                       else
                                       if (tamperHeadings[selectedItemCount].ToString() == "ByPassCurrent")
                                            {
                                                newTamperList.Add("OccByPassCurrent");
                                                newTamperList.Add("ResByPassCurrent");
                                            }
                                            else
                                            if (tamperHeadings[selectedItemCount].ToString() == "HighNeutralCurrent") 
                                            {
                                                newTamperList.Add("OccHighNeutralCurrent");
                                                newTamperList.Add("ResHighNeutralCurrent");
                                            }
                                            else
                                                if (tamperHeadings[selectedItemCount].ToString() == "kWr")
                                                {
                                                    newTamperList.Add("OcckWr");
                                                    newTamperList.Add("ReskWr");
                                                }
                                                else
                                                    if (tamperHeadings[selectedItemCount].ToString() == "kWy") 
                                                    {
                                                        newTamperList.Add("OcckWy");
                                                        newTamperList.Add("ReskWy");
                                                    }
                                                    else
                                                        if (tamperHeadings[selectedItemCount].ToString() == "kWb")
                                                        {
                                                            newTamperList.Add("OcckWb");
                                                            newTamperList.Add("ReskWb");
                                                        }
                                                        else
                                                            if (tamperHeadings[selectedItemCount].ToString() == "kVAr") 
                                                            {
                                                                newTamperList.Add("OcckVAr");
                                                                newTamperList.Add("ReskVAr");
                                                            }
                else
                    if (tamperHeadings[selectedItemCount].ToString() == "kVAy") 
                    {
                        newTamperList.Add("OcckVAy");
                        newTamperList.Add("ReskVAy");
                    }
                    else
                        if (tamperHeadings[selectedItemCount].ToString() == "kVAb") 
                        {
                            newTamperList.Add("OcckVAb");
                            newTamperList.Add("ReskVAb");
                        }
                        else
                            if (tamperHeadings[selectedItemCount].ToString() == "CumulativeTamperCount")
                            {
                                newTamperList.Add("OccCumulTampCount");
                                newTamperList.Add("ResCumulTampCount");
                            }
                                        else
                                            if (tamperHeadings[selectedItemCount].ToString() == "VoltageVRN")
                                            {
                                                newTamperList.Add("OccRVoltage");
                                                newTamperList.Add("ResRVoltage");
                                            }
                                            else
                                                if (tamperHeadings[selectedItemCount].ToString() == "VoltageVYN")
                                                {
                                                    newTamperList.Add("OccYVoltage");
                                                    newTamperList.Add("ResYVoltage");
                                                }
                                                else
                                                    if (tamperHeadings[selectedItemCount].ToString() == "VoltageVBN")
                                                    {
                                                        newTamperList.Add("OccBVoltage");
                                                        newTamperList.Add("ResBVoltage");
                                                    }
                                                    else
                                                        if (tamperHeadings[selectedItemCount].ToString() == "PhaseVoltage")
                                                        {
                                                            newTamperList.Add("OccPhaseVoltage");
                                                            newTamperList.Add("ResPhaseVoltage");
                                                        }
                                                        else
                                                            if (tamperHeadings[selectedItemCount].ToString() == "PowerFactorRphase")
                                                            {
                                                                newTamperList.Add("OccRPF");
                                                                newTamperList.Add("ResRPF");
                                                            }
                                                            else
                                                                if (tamperHeadings[selectedItemCount].ToString() == "PowerFactorYphase")
                                                                {
                                                                    newTamperList.Add("OccYPF");
                                                                    newTamperList.Add("ResYPF");
                                                                }
                                                                else
                                                                    if (tamperHeadings[selectedItemCount].ToString() == "PowerFactorBphase")
                                                                    {
                                                                        newTamperList.Add("OccBPF");
                                                                        newTamperList.Add("ResBPF");
                                                                    }
                                                                    else
                                                                        if (tamperHeadings[selectedItemCount].ToString() == "TotalPowerFactor")
                                                                        {
                                                                            newTamperList.Add("OccPF");
                                                                            newTamperList.Add("ResPF");
                                                                        }
                                                                        else
                                                                            if (meterModelNumber == 28 || meterModelNumber == 29)
                                                                            {
                                                                          if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergyMWh")
                                                                                {
                                                                                    newTamperList.Add("OccKWh");
                                                                                    newTamperList.Add("ResKWh");
                                                                                }
                                                                            }
                                                                          if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykWh")
                                                                            {

                                                                                newTamperList.Add("OccKWh");
                                                                                newTamperList.Add("ResKWh");
                                                                            }
                                                                            else
                                                                                if (meterModelNumber == 28 || meterModelNumber == 29)
                                                                                {
                                                                           if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergyMVAh")
                                                                                    {
                                                                                        newTamperList.Add("OccKVAh");
                                                                                        newTamperList.Add("ResKVAh");
                                                                                    }
                                                                                }
                                                                                if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykVAh")
                                                                                {
                                                                                    newTamperList.Add("OccKVAh");
                                                                                    newTamperList.Add("ResKVAh");
                                                                                }
                                                                                else
                                                                                    if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykWhExport")
                                                                                    {
                                                                                        newTamperList.Add("OccKWhExport");
                                                                                        newTamperList.Add("ResKWhExport");
                                                                                    }
                                                                                    else
                                                                                        if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykVAhExport")
                                                                                        {
                                                                                            newTamperList.Add("OccKVAhExport");
                                                                                            newTamperList.Add("ResKVAhExport");
                                                                                        }
                                                                                        else
                                                                                            if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykWhForward")
                                                                                            {
                                                                                                newTamperList.Add("OccKWhImport");
                                                                                                newTamperList.Add("ResKWhImport");
                                                                                            }
                                                                                            else
                                                                                                if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykVAhForward")
                                                                                                {
                                                                                                    newTamperList.Add("OccKVAhImport");
                                                                                                    newTamperList.Add("ResKVAhImport");
                                                                                                }
                                                                                                else
                                                                                                    if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykvarhLag")
                                                                                                    {
                                                                                                        newTamperList.Add("OcckvarhLag");
                                                                                                        newTamperList.Add("ReskvarhLag");
                                                                                                    }
                                                                                                    else
                                                                                                        if (tamperHeadings[selectedItemCount].ToString() == "CumulativeEnergykvarhLead")
                                                                                                        {
                                                                                                            newTamperList.Add("OcckvarhLead");
                                                                                                            newTamperList.Add("ReskvarhLead");
                                                                                                        }
                                                                                                        // SB Code Change Start 20171116
                                                                                                        else
                                                                                                            if (tamperHeadings[selectedItemCount].ToString() == "ActiveCurrentR")
                                                                                                            {
                                                                                                                newTamperList.Add("OccActCurR");
                                                                                                                newTamperList.Add("ResActCurR");
                                                                                                            }
                                                                                                            else
                                                                                                                if (tamperHeadings[selectedItemCount].ToString() == "ActiveCurrentY")
                                                                                                                {
                                                                                                                    newTamperList.Add("OccActCurY");
                                                                                                                    newTamperList.Add("ResActCurY");
                                                                                                                }
                                                                                                                else
                                                                                                                    if (tamperHeadings[selectedItemCount].ToString() == "ActiveCurrentB")
                                                                                                                    {
                                                                                                                        newTamperList.Add("OccActCurB");
                                                                                                                        newTamperList.Add("ResActCurB");
                                                                                                                    }
                                                                                                                    //SarkarA code change start 20180330 // add phase current instant, frequency/end 
                                                                                                                    else
                                                                                                                        if (tamperHeadings[selectedItemCount].ToString() == "PhaseCurrentInstant")
                                                                                                                        {
                                                                                                                            newTamperList.Add("OccPhaseCurrentInstant");
                                                                                                                            newTamperList.Add("ResPhaseCurrentInstant");
                                                                                                                        }
                    else
                        if (tamperHeadings[selectedItemCount].ToString() == "Frequency")
                        {
                            newTamperList.Add("OccFrequency");
                            newTamperList.Add("ResFrequency");
                        }
                        //SarkarA code change end 20180330
    else
        if (tamperHeadings[selectedItemCount].ToString() == "Temprature")
        {
            newTamperList.Add("OccTemprature");
            newTamperList.Add("ResTemprature");
        }
    else
        if (tamperHeadings[selectedItemCount].ToString() == "THDVR")
        {
            newTamperList.Add("OccTHDVR");
            newTamperList.Add("ResTHDVR");
        }
        else
            if (tamperHeadings[selectedItemCount].ToString() == "THDVY")
            {
                newTamperList.Add("OccTHDVY");
                newTamperList.Add("ResTHDVY");
            }
        else
            if (tamperHeadings[selectedItemCount].ToString() == "THDVB")
            {
                newTamperList.Add("OccTHDVB");
                newTamperList.Add("ResTHDVB");
            }
            else
            if (tamperHeadings[selectedItemCount].ToString() == "THDIR")
            {
                newTamperList.Add("OccTHDIR");
                newTamperList.Add("ResTHDIR");
            }
            else
                if (tamperHeadings[selectedItemCount].ToString() == "THDIY")
                {
                    newTamperList.Add("OccTHDIY");
                    newTamperList.Add("ResTHDIY");
                }
            else
                if (tamperHeadings[selectedItemCount].ToString() == "THDIB")
                {
                    newTamperList.Add("OccTHDIB");
                    newTamperList.Add("ResTHDIB");
                }
                
                       
            }
            }
                DataView dtView = new DataView(detailedTamperData.Tables[0]);
                DataTable dtTableWithDesiredColumn = dtView.ToTable(false, newTamperList.ToArray());
                foreach (DataRow row in dtTableWithDesiredColumn.Rows)
                {
                    int columnIndex = 0;
                    newReportRow = reportXSD.Tables["DetailedTamperDynamic"].NewRow();
                    try
                    {
                        foreach (DataColumn desiredColumns in dtTableWithDesiredColumn.Columns)
                        {
                            if (desiredColumns.ColumnName.ToLower().Contains("description") || desiredColumns.ColumnName.ToLower().Contains("occevent")
                                || desiredColumns.ColumnName.ToLower().Contains("resevent") || desiredColumns.ColumnName.ToLower().Contains("occdatetime")
                                || desiredColumns.ColumnName.ToLower().Contains("resdatetime"))
                            {
                                continue;
                            }
                            else if (desiredColumns.ColumnName.Contains("OccTemprature"))
                            {
                                newReportRow["Parameter" + columnIndex.ToString()] = CommonBLL.GetFormattedData_Temperature(row[desiredColumns].ToString());
                                columnIndex++;
                            }
                            else
                            {
                                newReportRow["Parameter" + columnIndex.ToString()] = CommonBLL.GetFormattedData(row[desiredColumns].ToString());
                                columnIndex++;
                            }
                        }
                    }
                    catch
                    { }
                    newReportRow["Description"] = row["Description"].ToString();
                    newReportRow["OccEvent"] = row["OccEvent"].ToString();
                    newReportRow["ResEvent"] = row["ResEvent"].ToString();
                    newReportRow["OccDateTime"] = row["OccDateTime"].ToString();
                    newReportRow["ResDateTime"] = row["ResDateTime"].ToString();
                    newReportRow["Duration"] = GetTamperDuration(row["OccDateTime"].ToString(), row["ResDateTime"].ToString());
                    reportXSD.Tables["DetailedTamperDynamic"].Rows.Add(newReportRow);
                }

            }
            catch (Exception ex)
            {
              
            }
        }
        /// <summary>
        /// To get Tamper duration from occurence and restoration time in DD:HH :MM format.
        /// </summary>
        /// <param name="occTime"></param>
        /// <param name="resTime"></param>
        /// <returns></returns>
        private string GetTamperDuration(string occTime, string resTime)
        {
            string tamperDuration = DateUnavailable;
           
            int clockDeviations = 0;
            if (occTime != DateUnavailable && resTime != DateUnavailable)
            {
                //-----Conditions to handle meter time format 24:00 with system time format, as system datetime dosn't support 24:00
                if (occTime.Contains("24 : 00") && resTime.Contains("24 : 00")) clockDeviations = 0;
                else if (occTime.Contains("24 : 00")) clockDeviations = -1;
                else if (resTime.Contains("24 : 00")) clockDeviations = 1; 
                if (occTime.Contains("24 : 00")) occTime = occTime.Replace("24 : 00", "23 : 59");
                if (resTime.Contains("24 : 00")) resTime = resTime.Replace("24 : 00", "23 : 59");
                //--------------------------------------------------------------------------------------------------------------------
                TimeSpan timeSpan = Convert.ToDateTime(resTime, new CultureInfo("hi-IN")).AddMinutes(clockDeviations) - Convert.ToDateTime(occTime, new CultureInfo("hi-IN"));
                tamperDuration = dlms650CommonBLL.ConvertTimSpanToDDHHMM(timeSpan);
            }
            return tamperDuration;
        }
        /// <summary>
        /// Maps tamper Event code with tamper abbreviation used in reports.
        /// Creates dictionay that contains tamper event codes and corresponding abbreviations.
        /// </summary>
        public static void GetTamperAbbreviation()
        {
            dictTamperCodeAndAbbreviation = new Dictionary<string, string>();

            dictTamperCodeAndAbbreviation.Add("1", "R Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("2", "R Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("3", "Y Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("4", "Y Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("5", "B Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("6", "B Ph. Missing Potential");
            dictTamperCodeAndAbbreviation.Add("7", "Over Voltage / Phase Split");
            dictTamperCodeAndAbbreviation.Add("8", "Over Voltage / Phase Split");
            dictTamperCodeAndAbbreviation.Add("9", "Under Voltage");
            dictTamperCodeAndAbbreviation.Add("10", "Under Voltage");
            #region check for PSPCL Tender
            string firmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
            if (firmwareVersion == "7.01")
            {
                dictTamperCodeAndAbbreviation.Add("11", "Invalid Voltage");
                dictTamperCodeAndAbbreviation.Add("12", "Invalid Voltage");
            }
            else
            {
                dictTamperCodeAndAbbreviation.Add("11", "Voltage Unbalance");
                dictTamperCodeAndAbbreviation.Add("12", "Voltage Unbalance");
            }
            #endregion
            dictTamperCodeAndAbbreviation.Add("13", "Voltage Ph. Seq. Reversal");
            dictTamperCodeAndAbbreviation.Add("14", "Voltage Ph. Seq. Reversal");
            dictTamperCodeAndAbbreviation.Add("47", "Invalid Phase Association ");
            dictTamperCodeAndAbbreviation.Add("48", "Invalid Phase Association ");
            dictTamperCodeAndAbbreviation.Add("49", "Invalid Voltage");
            dictTamperCodeAndAbbreviation.Add("50", "Invalid Voltage");
            dictTamperCodeAndAbbreviation.Add("51", "R Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("52", "R Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("53", "Y Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("54", "Y Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("55", "B Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("56", "B Ph. CT Reverse");
            dictTamperCodeAndAbbreviation.Add("57", "R Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("58", "R Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("59", "Y Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("60", "Y Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("61", "B Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("62", "B Ph. CT Open");
            dictTamperCodeAndAbbreviation.Add("63", "Current Unbalance");
            dictTamperCodeAndAbbreviation.Add("64", "Current Unbalance");
            dictTamperCodeAndAbbreviation.Add("65", "CT Bypass/ High Neutral Current");
            dictTamperCodeAndAbbreviation.Add("66", "CT Bypass/ High Neutral Current");
            dictTamperCodeAndAbbreviation.Add("67", "Over Current in any Phase");
            dictTamperCodeAndAbbreviation.Add("68", "Over Current in any Phase");
            dictTamperCodeAndAbbreviation.Add("69", "Earth Loading / Current Bypass");
            dictTamperCodeAndAbbreviation.Add("70", "Earth Loading / Current Bypass");
            dictTamperCodeAndAbbreviation.Add("101", "Power Failure");
            dictTamperCodeAndAbbreviation.Add("102", "Power Failure");
            dictTamperCodeAndAbbreviation.Add("151", "Real Time Clock Config");
            dictTamperCodeAndAbbreviation.Add("152", "Demand IP Config");
            dictTamperCodeAndAbbreviation.Add("153", "Load Survey IP Config");
            dictTamperCodeAndAbbreviation.Add("154", "Billing Date Config");
            dictTamperCodeAndAbbreviation.Add("194", "Billing Period Config"); // Billing Period (cycle)
            dictTamperCodeAndAbbreviation.Add("155", "TOU Config");
            dictTamperCodeAndAbbreviation.Add("156", "RS 485"); //@Deep: RS 485 tamper event id: 156 updated over previous CT ratio Config id as per latest ammendment of DLMS
            dictTamperCodeAndAbbreviation.Add("157", "PT Ratio Config");
            dictTamperCodeAndAbbreviation.Add("190", "CT Ratio Config");
            dictTamperCodeAndAbbreviation.Add("191", "PT Ratio Config");
            dictTamperCodeAndAbbreviation.Add("158", "kVAh Selection Changed");
            dictTamperCodeAndAbbreviation.Add("188", "kVAh Selection Changed"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("159", "MD Reset");
            dictTamperCodeAndAbbreviation.Add("189", "MD Reset");
            dictTamperCodeAndAbbreviation.Add("160", "Display - Push Mode Config");
            dictTamperCodeAndAbbreviation.Add("161", "Display - Scroll Mode Config");
            dictTamperCodeAndAbbreviation.Add("162", "Display - HR Mode Config");
            dictTamperCodeAndAbbreviation.Add("192", "Display Parameter"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("163", "Display - Scroll Time Config");
            dictTamperCodeAndAbbreviation.Add("164", "Auto Billing Lock");
            dictTamperCodeAndAbbreviation.Add("187", "Auto Billing Lock"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("165", "RS232 Lock / Unlock");
            dictTamperCodeAndAbbreviation.Add("186", "RS232 Lock / Unlock"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("167", "Software Billing Lock");
            dictTamperCodeAndAbbreviation.Add("185", "Software Billing Lock"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("168", "Manaul Billing Lock");
            dictTamperCodeAndAbbreviation.Add("184", "Manaul Billing Lock"); // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            dictTamperCodeAndAbbreviation.Add("91", "Over Load"); // Story - 428915 - Tamper Changes
            dictTamperCodeAndAbbreviation.Add("92", "Over Load");
            dictTamperCodeAndAbbreviation.Add("93", "Low Load"); // Low load Occurrence
            dictTamperCodeAndAbbreviation.Add("94", "Low Load");// Low Load restoration
            // Story - 472408 - Tamper Configuration support           
            dictTamperCodeAndAbbreviation.Add("71", "HighNeutralCurrent"); // pradipta_neu
            dictTamperCodeAndAbbreviation.Add("72", "HighNeutralCurrent"); // pradipta_neu
            dictTamperCodeAndAbbreviation.Add("200", "Tamper Reset"); // Task ID: 569567: Tamper Reset Option in 3P DLMS Sapphire Two TOU "sc" model
            dictTamperCodeAndAbbreviation.Add("201", "Magnetic Influence");
            dictTamperCodeAndAbbreviation.Add("202", "Magnetic Influence");
            dictTamperCodeAndAbbreviation.Add("203", "Neutral Disturbance");
            dictTamperCodeAndAbbreviation.Add("204", "Neutral Disturbance");
            dictTamperCodeAndAbbreviation.Add("205", "Very Low P.F.");
            dictTamperCodeAndAbbreviation.Add("206", "Very Low P.F.");
            dictTamperCodeAndAbbreviation.Add("207", "Single Wire Operation");
            dictTamperCodeAndAbbreviation.Add("208", "Single Wire Operation");
            dictTamperCodeAndAbbreviation.Add("209", "Plugin Module Removal Occurrence");
            dictTamperCodeAndAbbreviation.Add("210", "Plugin Module Removal Restoration");
            dictTamperCodeAndAbbreviation.Add("211", "Configuration changed to postpaid mode");
            dictTamperCodeAndAbbreviation.Add("213", "Configuration changed to forward only mode");
            dictTamperCodeAndAbbreviation.Add("214", "Configuration changed to import and export mode");
            dictTamperCodeAndAbbreviation.Add("215", "Overload Occurrence");
            dictTamperCodeAndAbbreviation.Add("216", "Overload Restoration");
            dictTamperCodeAndAbbreviation.Add("243", "Low Supply Voltage");
            dictTamperCodeAndAbbreviation.Add("244", "Low Supply Voltage");
            dictTamperCodeAndAbbreviation.Add("245", "Phase in Neutral");
            dictTamperCodeAndAbbreviation.Add("246", "Phase in Neutral");
            dictTamperCodeAndAbbreviation.Add("247", "2 Phase Without Neutral (2PN)");//User story 433253- Updated the name (2PN) 
            dictTamperCodeAndAbbreviation.Add("248", "2 Phase Without Neutral (2PN)");//User story 433253- Updated the name (2PN)
            dictTamperCodeAndAbbreviation.Add("249", "ESD");
            dictTamperCodeAndAbbreviation.Add("250", "ESD");
            dictTamperCodeAndAbbreviation.Add("251", "Front Cover Open");
            //Temper Reset
            dictTamperCodeAndAbbreviation.Add("256", "Tamper Reset"); // User Story 456437, 455259 1P IEC meter
            dictTamperCodeAndAbbreviation.Add("297", "COMS Card Removal Occurence"); // Single phase smart meter
            dictTamperCodeAndAbbreviation.Add("301", "Meter Disconnected");
            dictTamperCodeAndAbbreviation.Add("302", "Meter Connected");
            dictTamperCodeAndAbbreviation.Add("701", "High Neutral Current Occurence");
            dictTamperCodeAndAbbreviation.Add("702", "High Neutral Current Restoration");
            //SarkarA code change start 20180308 // add Current Mismatch Tamper
            dictTamperCodeAndAbbreviation.Add("703", "Current Mismatch");
            dictTamperCodeAndAbbreviation.Add("704", "Current Mismatch"); 
            dictTamperCodeAndAbbreviation.Add("751", "Last Token Recharge Amount prepaid");           
             dictTamperCodeAndAbbreviation.Add("752","Last Token Recharge Time prepaid");
             dictTamperCodeAndAbbreviation.Add("753","Total Amount Last Recharge prepaid");
             dictTamperCodeAndAbbreviation.Add("754","Current Balance Amount prepaid");
             dictTamperCodeAndAbbreviation.Add("755","Current Balance Time prepaid");
             dictTamperCodeAndAbbreviation.Add("756","Digital output Operation");
             dictTamperCodeAndAbbreviation.Add("757","Sliding Demand Period Change");
             dictTamperCodeAndAbbreviation.Add("758","Event Threshold Config Change");
             dictTamperCodeAndAbbreviation.Add("759","Event Threshold Persistence time Change");
             dictTamperCodeAndAbbreviation.Add("760","Event Display Parameters Change");
             dictTamperCodeAndAbbreviation.Add("761","LS Parameter Store ID");
             dictTamperCodeAndAbbreviation.Add("762","Optical port Lock");
             dictTamperCodeAndAbbreviation.Add("763","Optical port Unlock");
             dictTamperCodeAndAbbreviation.Add("764","RJ port Lock");
             dictTamperCodeAndAbbreviation.Add("765","RJ port Unlock");
             dictTamperCodeAndAbbreviation.Add("766","Special Day");
             dictTamperCodeAndAbbreviation.Add("767","Event Enable/Disable Config");
             dictTamperCodeAndAbbreviation.Add("768","Load Control Paramter");
             dictTamperCodeAndAbbreviation.Add("769","ARM button Enable");
             dictTamperCodeAndAbbreviation.Add("770","ARM button Disable");
             dictTamperCodeAndAbbreviation.Add("771","FS Mode Lock");
             dictTamperCodeAndAbbreviation.Add("772", "FS Mode Unlock");
             dictTamperCodeAndAbbreviation.Add("801", "ESD");
             dictTamperCodeAndAbbreviation.Add("802", "ESD");
             dictTamperCodeAndAbbreviation.Add("803", "Abnormal Power Off");//pradipta abnor
             dictTamperCodeAndAbbreviation.Add("804", "Abnormal Power Off");//pradipta abnor
             dictTamperCodeAndAbbreviation.Add("805", "Invalid Phase Association Occurrence");
             dictTamperCodeAndAbbreviation.Add("806", "Invalid Phase Association Restoration");
             dictTamperCodeAndAbbreviation.Add("239", "Temperature  Occurrence");//pradipta
             dictTamperCodeAndAbbreviation.Add("951", "Temperature  Occurrence");
             dictTamperCodeAndAbbreviation.Add("952", "Temperature  Restoration");
             dictTamperCodeAndAbbreviation.Add("959", "%THDV R- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("960", "%THDV R- Phase Tamper Restoration");
             dictTamperCodeAndAbbreviation.Add("961", "%THDV Y- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("962", "%THDV Y- Phase Tamper Restoration");
             dictTamperCodeAndAbbreviation.Add("963", "%THDV B- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("964", "%THDV B- Phase Tamper Restoration");
             dictTamperCodeAndAbbreviation.Add("965", "%THDI R- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("966", "%THDI R- Phase Tamper Restoration");
             dictTamperCodeAndAbbreviation.Add("967", "%THDI Y- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("968", "%THDI Y- Phase Tamper Restoration");
             dictTamperCodeAndAbbreviation.Add("969", "%THDI B- Phase Tamper Occurance");
             dictTamperCodeAndAbbreviation.Add("970", "%THDI B- Phase Tamper Restoration");
             //SarkarA code change end 20180308 
             dictTamperCodeAndAbbreviation.Add("1001", "Digital Input 2 Occurrence");
             dictTamperCodeAndAbbreviation.Add("1002", "Digital Input 2 Restoration");
             dictTamperCodeAndAbbreviation.Add("1003", "Digital Input 3 Occurrence");
             dictTamperCodeAndAbbreviation.Add("1004", "Digital Input 3 Restoration");
             dictTamperCodeAndAbbreviation.Add("1005", "Digital Input 4 Occurrence");
             dictTamperCodeAndAbbreviation.Add("1006", "Digital Input 4 Restoration");
             dictTamperCodeAndAbbreviation.Add("1007", "Digital Input 5 Occurrence");
             dictTamperCodeAndAbbreviation.Add("1008", "Digital Input 5 Restoration");
             dictTamperCodeAndAbbreviation.Add("1009", "Digital Input 6 Occurrence");
             dictTamperCodeAndAbbreviation.Add("1010", "Digital Input 6 Restoration");
            
        }
        /// <summary>
        /// Fill Meter Id in XSD table - BillingDetailsTable 
        /// 
        /// </summary>
        /// <param name="meterIdDS"></param>
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
                        reportRow["MeterNo"] = DateUnavailable;
                }
                reportRow["ActiveMeter"] = "Inactive";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }
        /// <summary>
        /// Display tamper report
        /// </summary>
        private void ShowReport()
        {
            CommonMethods.MeterDataType = new DLMS650GeneralBLL().GetMeterDataType(ConfigInfo.ActiveMeterDataId);
            ReportForm ObjRptForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.NDPLTamperReport tamperReport = new NDPLTamperReport();
            /* Add BCS Version in Report header */
            CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)tamperReport.ReportDefinition.ReportObjects["txtBCSVersion"];
            txtBCSVersion.Text = CAB.UI.Common.GetBCSVersion();
            /* Add BCS Version in Report header */
            if (reportXSD.Tables["CoumulativeTamperCounter"].Rows.Count == 0)
            {
                tamperReport.DetailSection1.SectionFormat.EnableSuppress = true;
            }
            /* VBM - Display Start End Date on Cumulative counter report */
            else
            {
                CrystalDecisions.CrystalReports.Engine.ReportObjects reportObjectDetail = tamperReport.DetailSection1.ReportObjects;
                foreach (ReportObject reportObject in reportObjectDetail)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        TextObject objTextFromDate = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TextFromDate"];
                        TextObject objTextToDate = (TextObject)subReportDocument.ReportDefinition.ReportObjects["TextToDate"];
                        objTextFromDate.Text = dtPickerStartDate.Value.ToString(ShortDateFromat);
                        objTextToDate.Text = dtPickerEndDate.Value.ToString(ShortDateFromat);

                    }
                }

            }
            /* VBM - Display Start End Date on Cumulative counter report */
            if (reportXSD.Tables["DetailedTamperDynamic"].Rows.Count == 0)
            {
                tamperReport.SecBillingGeneral.SectionFormat.EnableSuppress = true;
            }
            else
            {
                TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                DataSet dataSet = new DataSet();              
                int tamperHeadingCount = 0;
                //Temp code till next solution
                //tamperHeadingCount = ConfigInfo.ActiveFileType == "NONDLMS" ? tamperHeadings.Length - 1 : tamperHeadings.Length; 
                tamperHeadingCount = tamperHeadings.Count; // Story - 349654 - Neutral current in Tamper

                CrystalDecisions.CrystalReports.Engine.ReportObjects rebObjCol = tamperReport.SecBillingGeneral.ReportObjects;
                // CrystalDecisions.CrystalReports.Engine.SubreportObject repSubReport = (CrystalDecisions.CrystalReports.Engine.SubreportObject)rebObjCol[0];
                foreach (ReportObject reportObject in rebObjCol)
                {
                    if (reportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject subreportObject = (SubreportObject)reportObject;
                        ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
                        if (tamperHeadings.Count > 0 && !string.IsNullOrEmpty(tamperHeadings[0])) // Story - 354382 - If Tamper data is not avialable for meters or not in proper format
                        {
                            for (int selectedItemCount = 0; selectedItemCount < tamperHeadingCount; selectedItemCount++)
                            {
                                if (selectedItemCount > 10) // Story - 354382 - Only 10 parameters are allowed on Tamper Report
                                    break;
                                TextObject groupHeaderTextObject = (TextObject)subReportDocument.ReportDefinition.ReportObjects["GroupHeader" + (selectedItemCount).ToString()] as TextObject;
                                
                                //SarkarA code change start 20180226 // commented out Top and EnableCanGrow properties due to column mismatch in Excel Data Export
                                //short nLoactionTop = 30;

                                if (groupHeaderTextObject != null)
                                {
                                    switch (tamperHeadings[selectedItemCount].ToString())
                                    {
                                        case "CurrentIR":
                                            groupHeaderTextObject.Text = "Ir (A)";
                                            break;
                                        case "CurrentIY":
                                            groupHeaderTextObject.Text = "Iy (A)";
                                            break;
                                        case "CurrentIB":
                                            groupHeaderTextObject.Text = "Ib (A)";
                                            break;
                                        case "PhaseCurrent":
                                            groupHeaderTextObject.Text = "I (A)";
                                            break;
                                        case "NeutralCurrent": // Story - 349654 - Neutral current in Tamper
                                            groupHeaderTextObject.Text = "In (A)";
                                            break;
                                        case "ByPassCurrent":
                                            groupHeaderTextObject.Text = "ByPassCurrent";
                                            break;
                                        //SarkarA code change start 20170118 // neutral current
                                        case "HighNeutralCurrent": // pradipta_neu
                                            groupHeaderTextObject.Text = "Ihn (A)";
                                            break;
                                        //SarkarA code change end 20170118

                                        case "kWr": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active Power R";// "ActP R";//add  pradipta_neu
                                            break;
                                        case "kWy": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active Power Y";// "ActP Y";//add  pradipta_neu
                                            break;
                                        case "kWb": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active Power B";// "ActP B";//add  pradipta_neu
                                            break;
                                        case "kVAr": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Apparent Power R";// "ApprP R";//add  pradipta_neu
                                            break;
                                        case "kVAy": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Apparent Power Y";//"ApprP Y";//add  pradipta_neu
                                            break;
                                        case "kVAb": // 
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Apparent Power B";//"ApprP B";//add  pradipta_neu
                                            break;

                                        case "CumulativeTamperCount":
                                            groupHeaderTextObject.Text = "Temp.Count";
                                            break;
                                        case "VoltageVRN":
                                            groupHeaderTextObject.Text = "Vr (V)";
                                            break;
                                        case "VoltageVYN":
                                            groupHeaderTextObject.Text = "Vy (V)";
                                            break;
                                        case "VoltageVBN":
                                            groupHeaderTextObject.Text = "Vb (V)";
                                            break;
                                        case "PhaseVoltage":
                                            groupHeaderTextObject.Text = "V (V)";
                                            break;
                                        case "PowerFactorRphase":
                                            groupHeaderTextObject.Text = "PFr";
                                            break;
                                        case "PowerFactorYphase":
                                            groupHeaderTextObject.Text = "PFy";
                                            break;
                                        case "PowerFactorBphase":
                                            groupHeaderTextObject.Text = "PFb";
                                            break;
                                        case "TotalPowerFactor":
                                            groupHeaderTextObject.Text = "PF";
                                            break;
                                        case "CumulativeEnergykWh":
                                            groupHeaderTextObject.Text =  CommonMethods.getDisplayHeaderText("{0}Wh");
                                            break;
                                        case "CumulativeEnergykVAh":
                                            groupHeaderTextObject.Text =  CommonMethods.getDisplayHeaderText("{0}VAh");
                                            break;
                                        case "CumulativeEnergyMWh":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}Wh");// For HTCT Mega Varient
                                            break;
                                        case "CumulativeEnergyMVAh":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}VAh"); // For HTCT Mega Varient
                                            break;
                                        case "CumulativeEnergykWhForward":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}Wh Fwd");
                                            break;
                                        case "CumulativeEnergykWhExport":
                                            groupHeaderTextObject.Text =  CommonMethods.getDisplayHeaderText("{0}Wh Exp");
                                            break;
                                        case "CumulativeEnergykVAhForward":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}VAh Fwd");
                                            break;  
                                        case "CumulativeEnergykVAhExport":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}VAh Exp");
                                            break;
                                        case "CumulativeEnergykvarhLag":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}varh Lag");
                                            break;
                                        case "CumulativeEnergykvarhLead":
                                            groupHeaderTextObject.Text = CommonMethods.getDisplayHeaderText("{0}varh Lead");
                                            break;
                                        // SB Code Change Start 20171116 //corrected nomenclature 20180122
                                        case "ActiveCurrentR":
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active (i) R";// "Act(i) R";
                                            break;
                                        case "ActiveCurrentY":
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active (i) Y";//"Act(i) Y";
                                            break;
                                        case "ActiveCurrentB":
                                            //groupHeaderTextObject.ObjectFormat.EnableCanGrow = true;
                                            //groupHeaderTextObject.Top = nLoactionTop;
                                            groupHeaderTextObject.Text = "Active (i) B";//"Act(i) B";
                                            break;
                                        //SarkarA code change start 20180330 // add phase current instant, frequency/end      
                                        case "Frequency":
                                            groupHeaderTextObject.Text = "Freq. (Hz)";
                                            break;
                                        //SarkarA code change end 20180330
                                        case "Temprature":                                    
                                            groupHeaderTextObject.Text = "Temprature "+"("+ Convert.ToChar(0176)+ "C"+")";//pradipta
                                            break;
                                        case "PhaseCurrentInstant":
                                            groupHeaderTextObject.Text = "I (A)";
                                            break;
                                        case "THDVR":
                                            groupHeaderTextObject.Text = "THDVR";
                                            break;
                                        case "THDVY":
                                            groupHeaderTextObject.Text = "THDVY";
                                            break;
                                        case "THDVB":
                                            groupHeaderTextObject.Text = "THDVB";
                                            break;
                                        case "THDIR":
                                            groupHeaderTextObject.Text = "THDIR";
                                            break;
                                        case "THDIY":
                                            groupHeaderTextObject.Text = "THDIY";
                                            break;
                                        case "THDIB":
                                            groupHeaderTextObject.Text = "THDIB";
                                            break;

                                        
// SB Code Change End 20171116      
//SarkarA code change end 20180226 
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ///* Delete kvah column for few utilities */
            //if (!UtilityDetails.ShowApparantEnergyInTamper)
            //{
            //    CrystalDecisions.CrystalReports.Engine.ReportObjects reportObjectDetailedTamper = tamperReport.SecBillingGeneral.ReportObjects;
            //    foreach (ReportObject reportObject in reportObjectDetailedTamper)
            //    {
            //        if (reportObject.Kind == ReportObjectKind.SubreportObject)
            //        {
            //            SubreportObject subreportObject = (SubreportObject)reportObject;
            //            ReportDocument subReportDocument = subreportObject.OpenSubreport(subreportObject.SubreportName);
            //            //TextObject objKVAhColumn = (TextObject)subReportDocument.ReportDefinition.ReportObjects["Text21"];
            //            //objKVAhColumn.Width = 0;

            //        }
            //    }
            //    if (reportXSD.Tables["DetailedTamperTable"].Columns.Contains("OccKVAh"))
            //    {
            //        reportXSD.Tables["DetailedTamperTable"].Columns.Remove("OccKVAh");
            //    }
            //    if (reportXSD.Tables["DetailedTamperTable"].Columns.Contains("ResKVAh"))
            //    {
            //        reportXSD.Tables["DetailedTamperTable"].Columns.Remove("ResKVAh");
            //    }
            //}
            /* Delete kvah column for utilities */

            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(tamperReport);
            tamperReport.SetDataSource(reportXSD);
            ObjRptForm.rptViewer.ReportSource = tamperReport;
            Cursor.Current = Cursors.Default;
            ObjRptForm.rptViewer.Zoom(1);
            this.Hide();
            // SB code change Start - 20180629 - Multiple Analysis View
            //ObjRptForm.ShowDialog();
            ObjRptForm.Show();
            // SB code change End - 20180629 - Multiple Analysis View
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;

        }

        private void dgvTamperOccurence_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CheckAndUpdateSelectAll(dgvTamperOccurence);
        }

        private void ViewTamperCountDetails()
        {
            long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerStartDate.Value.ToShortDateString() + " 00:00:00"));
            long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerEndDate.Value.ToShortDateString() + " 23:59:59"));
            if (frmDate > toDate)
            {
                MessageBox.Show("Start Date should not be greater than End date", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int compartmentId = 0;
                if (rdbAllTamper.Checked)
                {
                    FillTamperCountDetailByDateRange(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), frmDate, toDate);
                }
                else
                {
                    if (rdbComp1.Checked)
                    {
                        compartmentId = 1;
                    }
                    else if (rdbComp2.Checked)
                    {
                        compartmentId = 2;
                    }
                    else if (rdbComp3.Checked)
                    {
                        compartmentId = 3;
                    }
                    else if (rdbComp5.Checked)
                    {
                        compartmentId = 5;
                    }
                    FillTamperCountDetailByDateRangeWithCompartmentID(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), frmDate, toDate, Convert.ToString(compartmentId));
                }
            }
        }
        private void dtPickerStartDate_DropDown(object sender, EventArgs e)
        {
            changedFromDate = DateUtility.DateTimeToLong(dtPickerStartDate.Value);
            changedToDate = DateUtility.DateTimeToLong(dtPickerEndDate.Value);
        }

        private void dtPickerStartDate_CloseUp(object sender, EventArgs e)
        {
            ViewTamperCountDetails();
        }

        private void dtPickerEndDate_CloseUp(object sender, EventArgs e)
        {
            ViewTamperCountDetails();
        }

        private void dtPickerEndDate_DropDown(object sender, EventArgs e)
        {
            changedFromDate = DateUtility.DateTimeToLong(dtPickerStartDate.Value);
            changedToDate = DateUtility.DateTimeToLong(dtPickerEndDate.Value);
        }

        private void rdbAllTamper_CheckedChanged(object sender, EventArgs e)
        {
            long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerStartDate.Value.ToShortDateString() + " 00:00:00"));
            long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dtPickerEndDate.Value.ToShortDateString() + " 23:59:59"));
            if (rdbAllTamper.Checked)
            {
                if (sender.Equals(rdbAllTamper))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FillTamperCountDetailByDateRange(Convert.ToInt32(activeMeterDataId), frmDate, toDate);
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                int compartmentId = 0;
                if (rdbComp1.Checked)
                {
                    compartmentId = 1;
                }
                else if (rdbComp2.Checked)
                {
                    compartmentId = 2;
                }
                else if (rdbComp3.Checked)
                {
                    compartmentId = 3;
                }
                else if (rdbComp5.Checked)
                {
                    compartmentId = 5;
                }
                //Fill Tamper summary grid according to compartment id.
                if ((compartmentId == 1 && sender.Equals(rdbComp1)) || (compartmentId == 2 && sender.Equals(rdbComp2))
                    || (compartmentId == 3 && sender.Equals(rdbComp3)) || (compartmentId == 5 && sender.Equals(rdbComp5)))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FillTamperCountDetailByDateRangeWithCompartmentID(Convert.ToInt32(activeMeterDataId), frmDate, toDate, Convert.ToString(compartmentId));
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void rdbComp1_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }

        private void rdbComp2_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }

        private void rdbComp3_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }

        private void rdbComp5_CheckedChanged(object sender, EventArgs e)
        {
            rdbAllTamper_CheckedChanged(sender, e);
        }
        #endregion
              

         }
}
