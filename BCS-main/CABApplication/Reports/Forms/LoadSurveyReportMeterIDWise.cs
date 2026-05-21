using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using System.Drawing;
using System.Linq;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    /// <summary>
    /// generates report for Load Survey 
    /// </summary>
    public partial class LoadSurveyReportMeterIDWise : CABForm
    {

     
        #region Nested Types
        #endregion

        #region Constants and Variables

      
       
       private long changedFromDate;
        private long changedToDate;
         private DLMS650LoadSurveyBLL loadSurveyBll ;
         private LoadSurveyParameterBLL loadSurveyParameterBLL;
         
      

        private FileReportDataSet reportXSD = null;
        static List<string> lsHeadings;
        ApplicationType types;
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        string dateUnavailable = "--------";
        private const string FREQUENCY = "Frequency - Hz (1.0.14.27.0.255;3;2)";
        private const string TAMPERSTATUS = "Tamper Status (0.0.96.1.152.255;1;2)";
        private const string POWERFACTOR = "Power Factor";
        private const string POWERFACTORLITERAL = "Power Factor";
        private const string EMPTYPARAMETER = "Empty Parameter";
        bool isPUMA = false;
        string utility = string.Empty;
        DataSet loadSurveyDS = null;
        private const string DEMAND = "demand";
        private const string ENERGY = "energy";
        private const string CURRENT = "current";
        private const string VOLTAGE = "voltage";
        private const string AMPERE = "A";
        private const string VOLT = "V";
        private const string HZ = "HZ";
        private const string KVAR = "kvar";
        private const string ENERGYKVARH = "kvarh";
        private const string DEMANDKVAR = "kvar";
        private const string KVA = "kva";
        private const string ENERGYKVAH = "kVAh";
        private const string DEMANDKVA = "kVA";
        private const string KW = "kw";
        private const string ENERGYKWH = "kWh";
        private const string DEMANDKW = "kW";
        private const string HERTZ = "hz";
        private const string DEMANDHEADERTEXT = "Load Survey(Demand)";
        private const string ENERGYHEADERTEXT = "Load Survey(Energy)";
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoadSurveyReportMeterIDWise).ToString());
        #endregion

        #region Properties
        private static string ActiveMeterIdLoadSurvey
        {
            get;
            set;
        }

       
        #endregion


        #region Constructor

        public LoadSurveyReportMeterIDWise()
        {
           
            reportXSD = new FileReportDataSet();
            loadSurveyBll = new DLMS650LoadSurveyBLL();
            InitializeComponent();
            
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Public Methods
        #endregion


        #region Private Methords

        private void FillMeterID(DataSet meterIdDS)
        {
            try
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
                    reportRow["ActiveMeter"] = "No";
                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillMeterID(DataSet meterIdDS)", ex);
                
            }
           
        }

        private void FillConsumerMeterDetails(DataSet detailsDS)
        {
            try
            {
                DataRow reportRow;

                if (detailsDS.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in detailsDS.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();

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
                        {
                            ///* VBM - EMF Bug Fixed #146611 */
                            // decimal actualEMF = 0;
                            // int internalCTRatio = 0;
                            // int internalPTRatio = 0;

                            // String meterEMF = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                            // //BhardwajG : EMF Bug
                            // if (int.TryParse(CommonBLL.GetFormattedData(row["internalCTRatio"].ToString()), out internalCTRatio)
                            //     && int.TryParse(CommonBLL.GetFormattedData(row["internalPTRatio"].ToString()), out internalPTRatio))
                            // {

                            // }
                            // if (internalCTRatio <= 0)
                            // {
                            //     internalCTRatio = 1;
                            // }
                            // if (internalPTRatio <= 0)
                            // {
                            //     internalPTRatio = 1;
                            // }
                            // actualEMF = Convert.ToDecimal(meterEMF) / (internalPTRatio * internalCTRatio);

                            // /* GKG 146126 EMF resolution issue */
                            // //meterEMF = actualEMF.ToString();
                            // meterEMF = string.Format("{0:F3}", actualEMF);                        
                            // /* GKG 146126 EMF resolution issue */

                            /* VBM - EMF Bug Fixed */
                            string meterEMF = CommonBLL.CalculateActualEMF(Convert.ToDecimal(row["Meter_EMF"].ToString()),
                                                                              row["internalCTRatio"].ToString(),
                                                                              row["internalPTRatio"].ToString());
                            string emfApplied = CommonBLL.GetFormattedData(row["UseEMFInCalculations"].ToString());
                            if (emfApplied == "1")
                            {
                                emfApplied = APPLIED;
                            }
                            else
                            {
                                emfApplied = NOTAPPLIED;
                            }
                            meterEMF = meterEMF + " (" + emfApplied + ")";
                            reportRow["EMF"] = meterEMF;
                            /* VBM - EMF Bug Fixed */
                        }
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
                            reportRow["ActiveMeter"] = "No";
                        else
                            reportRow["ActiveMeter"] = "Yes";

                        if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                            reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                        else
                            reportRow["ContractDemand"] = dateUnavailable;

                        reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                        reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillConsumerMeterDetails(DataSet detailsDS)", ex);
                
            }          
        }

        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetailForMeterID(string activeMeterId)
        {
            return new MeterDataBLL().GetConsumerMeterDetailsForMeterID(activeMeterId);
        }

        //private DataSet FilterData(DataSet dataSet)
        //{
        //    if (dataSet != null)
        //    {
        //        if (dataSet.Tables != null)
        //        {
        //            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataColumn column in dataSet.Tables[0].Columns)
        //                { 
        //                   chkListLoadSurveyParameters.
        //                }
        //            }
        //        }
        //    }
        //    return dataSet;
        //}
        private bool FillLoadSurveyXSD(DataSet loadSurveyData)
        {
            lsHeadings = new List<string>();
            DataRow reportRow;
            DateTime PreviousDate = DateTime.Now;

            try
            {
                reportXSD.Tables["LoadSurveyTable"].Rows.Clear();
                if (loadSurveyData == null || loadSurveyData.Tables[0].Rows.Count == 0)
                    return false;

                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {

                    int maxColumns = 13;
                    if (chkListLoadSurveyParameters.CheckedItems.Count <= 0)
                    {
                        MessageBox.Show("Please select atleast one parameter for report.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (chkListLoadSurveyParameters.CheckedItems.Count > maxColumns)
                    {
                        MessageBox.Show("Selected parameters can not exceed maxLimit (" + maxColumns + ")", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }


                    reportRow = reportXSD.Tables["LoadSurveyTable"].NewRow();
                    //foreach (DataColumn col in loadSurveyData.Tables[0].Columns)
                    //{
                    //    // Added to solve the DLMS_0102.
                    //    if (col.ColumnName.Trim() == FREQUENCY)
                    //    {
                    //        col.ColumnName = "Frequency-HZ(1.0.14.27.0.255;3;2)";
                    //    }
                    //    if (col.ColumnName.Trim() == TAMPERSTATUS)
                    //    {
                    //        col.ColumnName = "Tamper Status(0.0.96.1.152.255;1;2)";
                    //    }
                    //    if (col.ColumnName.Trim() == POWERFACTOR)
                    //    {
                    //        col.ColumnName = "Power Factor";
                    //    }

                    //    lsHeadings.Add(col.ColumnName);
                    //}





                    foreach (string columnName in chkListLoadSurveyParameters.CheckedItems)
                    {
                        if (columnName == POWERFACTORLITERAL)
                        {
                            lsHeadings.Add(POWERFACTOR);
                        }
                        else
                        {
                            lsHeadings.Add(columnName);
                        }
                    }
                    foreach (DataRow row in loadSurveyData.Tables[0].Rows)
                    {
                        DataTable LoadsurveyTable = new DataTable();
                        //if (SMD_rbtnLoadSurveyDemand.Checked)
                        //{ LoadsurveyTable = reportXSD.Tables["DLMS650LoadSurvey"]; }
                        //else
                        //{
                        LoadsurveyTable = reportXSD.Tables["LoadSurveyTable"];

                        reportRow = LoadsurveyTable.NewRow();
                        //To Fill -- when values are not available
                        //for (int colCount = 0; colCount < LoadsurveyTable.Columns.Count; colCount++)
                        //{
                        //    reportRow[colCount] = "----";
                        //}
                        string dateTimes = Convert.ToString(row[0]);
                        if (dateTimes.Length > 10)
                            dateTimes = dateTimes.Substring(0, 10);
                        reportRow["GroupDateTime"] = dateTimes;
                        for (int colCount = 0; colCount < lsHeadings.Count; colCount++)
                        {
                            if (colCount == 0)
                            { reportRow["DateTime"] = Convert.ToString(row[0]).Substring(11, Convert.ToString(row[0]).Length - 11); }
                            else
                            {

                                reportRow["Parameter" + colCount.ToString()] = CommonBLL.RemoveUnit(CommonBLL.GetFormattedData(row[lsHeadings[colCount]].ToString()));
                            }
                        }
                        LoadsurveyTable.Rows.Add(reportRow);
                    }
                    // To Get the Min and Max of Energy and Demand per Day Start
                    GetMaxMinDemandEnergy();
                    // To Get the Min and Max of Energy and Demand per Day End
                }
                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillLoadSurveyXSD(DataSet loadSurveyData)", ex);
                return false;
            }
            
        }

        /// <summary>
        /// This function is used to get the Max and Min of Demand and Energy per Day
        /// </summary>
        private void GetMaxMinDemandEnergy()
        {
            DataTable LoadsurveyMaxTable = null;
            DataTable LoadsurveyMinTable = null;
            DataRow maxReportRow = null;
            DataRow minReportRow = null;
            string max = string.Empty;
            string min = string.Empty;
            string columnName = string.Empty;

            try
            {
                LoadsurveyMaxTable = new DataTable();
                LoadsurveyMinTable = new DataTable();

                LoadsurveyMaxTable = reportXSD.Tables["LoadSurveyMaxTable"];
                LoadsurveyMinTable = reportXSD.Tables["LoadSurveyMinTable"];

                if (reportXSD.Tables["LoadSurveyTable"] != null && reportXSD.Tables["LoadSurveyTable"].Rows.Count != 0 && !string.IsNullOrEmpty(reportXSD.Tables["LoadSurveyTable"].Rows[0][0].ToString()))
                {
                    // Step 1: Linq to get the unique Dates
                    var uniqueDatesTable = (from UniqueDates in reportXSD.Tables["LoadSurveyTable"].AsEnumerable()
                                            group UniqueDates by UniqueDates.Field<string>("GroupDateTime") into gUniqueDates
                                            select new
                                            {
                                                groupDateTime = gUniqueDates.Key
                                            });

                    // Step 2: Loop over unique dates
                    foreach (var uniqueDateVar in uniqueDatesTable)
                    {
                        maxReportRow = LoadsurveyMaxTable.NewRow();
                        minReportRow = LoadsurveyMinTable.NewRow();

                        // Step 3: Loop over LoadSurveyMaxTable per column
                        for (int i = 1; i <= reportXSD.Tables["LoadSurveyTable"].Columns.Count - 3; i++)
                        {
                            columnName = "Parameter" + i;

                            // Step 4: Linq to get per Date per Column data
                            var perDateDataTable = (from perDate in reportXSD.Tables["LoadSurveyTable"].AsEnumerable()
                                                    where perDate.Field<string>("GroupDateTime") == uniqueDateVar.groupDateTime
                                                    select new
                                                    {
                                                        parameterValue = perDate.Field<string>(columnName)
                                                    });

                            // Step 5: Check the nullability of per Column record
                            if (perDateDataTable != null)
                            {
                                // Step 6: Loop over per Column record
                                foreach (var perDateDataVar in perDateDataTable)
                                {
                                    if (string.IsNullOrEmpty(perDateDataVar.parameterValue))
                                    {
                                        max = EMPTYPARAMETER;
                                        min = EMPTYPARAMETER;
                                        break;
                                    }
                                    if (ConvertStringToDouble(perDateDataVar.parameterValue))
                                    {
                                        if (string.IsNullOrEmpty(max) && string.IsNullOrEmpty(min))
                                        {
                                            max = perDateDataVar.parameterValue;
                                            min = perDateDataVar.parameterValue;
                                        }
                                        if (Convert.ToDecimal(max) < Convert.ToDecimal(perDateDataVar.parameterValue))
                                        {
                                            max = perDateDataVar.parameterValue;
                                        }
                                        if (Convert.ToDecimal(min) > Convert.ToDecimal(perDateDataVar.parameterValue))
                                        {
                                            min = perDateDataVar.parameterValue;
                                        }
                                    }
                                }
                                // If all the records in column are non double then set ----
                                if (string.IsNullOrEmpty(max) && string.IsNullOrEmpty(min))
                                {
                                    max = "----";
                                    min = "----";
                                }
                                else if (max.ToString() == EMPTYPARAMETER && min.ToString() == EMPTYPARAMETER)
                                {
                                    max = string.Empty;
                                    min = string.Empty;
                                }

                                maxReportRow[columnName] = max;
                                minReportRow[columnName] = min;

                                // Again set these variables to empty as need to check for another column records
                                max = string.Empty;
                                min = string.Empty;
                            }
                        }

                        maxReportRow["GroupDateTime"] = uniqueDateVar.groupDateTime;
                        minReportRow["GroupDateTime"] = uniqueDateVar.groupDateTime;

                        LoadsurveyMaxTable.Rows.Add(maxReportRow);
                        LoadsurveyMinTable.Rows.Add(minReportRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "GetMaxMinDemandEnergy()", ex);
            }
            finally
            {
                maxReportRow = null;
                minReportRow = null;
                LoadsurveyMaxTable = null;
                LoadsurveyMinTable = null;
            }
        }

        /// <summary>
        /// New function written to handle string conversion to Double
        /// </summary>
        /// <param name="value"> This is the string value need to convert to Double</param>
        /// <returns>If conversion is Successful then return True</returns>
        private bool ConvertStringToDouble(string value)
        {
            bool isDouble = false;

            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Convert.ToDecimal(value);
                    isDouble = true;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                isDouble = false;
                logger.Log(LOGLEVELS.Error, "ConvertStringToDouble(string value)", ex);
            }
            return isDouble;
        }
       
        /// <summary>
        /// check if start date and end date are same ,if yes , then add one day to end date 
        /// </summary>
        private void SetupDateTime()
        {
            
            long leastDate = loadSurveyBll.GetMinDateForMeterID(ActiveMeterIdLoadSurvey);
            long maxDate = loadSurveyBll.GetMaxDateForMeterID(ActiveMeterIdLoadSurvey);
            try
            {
                dateTimeStart.MinDate = DateTimePicker.MinimumDateTime;
                dateTimeStart.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimeEnd.MinDate = DateTimePicker.MinimumDateTime;
                dateTimeEnd.MaxDate = DateTimePicker.MaximumDateTime;
                dateTimeStart.Value = DateUtility.LongToDateTime(leastDate);
                dateTimeEnd.Value = DateUtility.LongToDateTime(maxDate);

                if (dateTimeStart.Value != dateTimeEnd.Value)
                {
                    dateTimeStart.MinDate = dateTimeStart.Value;
                    dateTimeStart.MaxDate = dateTimeEnd.Value;
                    dateTimeEnd.MinDate = dateTimeStart.Value;
                    dateTimeEnd.MaxDate = dateTimeEnd.Value;

                }
                else if (dateTimeStart.Value == dateTimeEnd.Value)
                {
                    dateTimeStart.MinDate = dateTimeStart.Value;
                    dateTimeStart.MaxDate = dateTimeEnd.Value.AddDays(1);
                    dateTimeEnd.MinDate = dateTimeStart.Value;
                    dateTimeEnd.MaxDate = dateTimeEnd.Value.AddDays(1);
                }
                dateTimeStart.Value = DateUtility.LongToDateTime(leastDate);
                dateTimeEnd.Value = DateUtility.LongToDateTime(maxDate);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "SetupDateTime()", ex);
                ex.GetBaseException();
                
            }
        }
        /// <summary>
        /// gets meterID from database and stores it in a list 
        /// </summary>
        /// <returns></returns>
        private List<string> SetRadioButton()
        {
            List<string> radioBoxMeterID = new List<string>();
            try
            {
                
                DataSet meterIdData = new MeterDataBLL().GetMeterIDLoadSurvey(true);
                if (meterIdData != null && meterIdData.Tables != null && meterIdData.Tables.Count > 0)
                {
                    foreach (DataRow row in meterIdData.Tables[0].Rows)
                    {
                        radioBoxMeterID.Add(Convert.ToString(row[0]));
                    }
                }
                radioBoxMeterID.Sort();
                
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "List<string> SetRadioButton()", ex);
            }
            return radioBoxMeterID;       
        }

/// <summary>
/// adds meterID's to groupRadioBox user control. 
/// </summary>
        private void GroupRadioBoxAdd()
        {
            try
            {
                dateTimeStart.Enabled = false;
                dateTimeEnd.Enabled = false;

                List<string> radioButtonMeterID = SetRadioButton();
                if (radioButtonMeterID.Count > 0)
                    AddRadioButtons(radioButtonMeterID.Count, radioButtonMeterID);
                else
                {
                    this.Close();
                    MessageBox.Show("LoadSurvey data not available.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }

                for (int i = 0; i < radioBoxPanel.Controls.Count; i++)
                {
                    RadioButton radioBox = (RadioButton)radioBoxPanel.Controls[i];

                    radioBox.CheckedChanged += new EventHandler(radioBox_CheckedChanged);
                }
                FillLoadSurveyParameters();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GroupRadioBoxAdd()", ex);
                
            }           
           
        }

        /// <summary>
        /// dynamically adds radio buttons to the panel
        /// </summary>
        /// <param name="MeterIdCount"></param>
        /// <param name="MeterID"></param>
        public void AddRadioButtons(int MeterIdCount, List<string> MeterID)
        {
            try
            {
                RadioButton radioButton;
                for (int idCounter = 0; idCounter < MeterIdCount; idCounter++)
                {
                    radioButton = new RadioButton();
                    radioButton.Width = 300;
                    radioButton.Name = MeterID[idCounter];
                    radioButton.Text = MeterID[idCounter];
                    radioButton.Location = new Point(5, 22 * idCounter);
                    radioBoxPanel.Controls.Add(radioButton);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "AddRadioButtons(int MeterIdCount, List<string> MeterID)", ex);
                
            }
          
        }

        private void FillLoadSurveyParameters()
        {
            try
            {
                long id = 0;
                string MeterID = null;
                DataSet dSet = null;
                loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                if (ActiveMeterIdLoadSurvey != null)
                {
                    MeterID = ActiveMeterIdLoadSurvey;
                    ConfigInfo.ActiveFileType = new MeterDataBLL().GetFileTypeByMeterId(MeterID);
                }


                string type = "Energy";
                if (SMD_rbtnLoadSurveyDemand.Checked)
                    type = "Demand";
                bool isPadding = false;
                if (rdbWithPadding.Checked)
                    isPadding = true;
                types = ConfigInfo.GetApplicationType();
                //long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                //if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //{
                //    loadSurveyDS = new DLMS650LoadSurveyBLL().ListDataSetColumnWise(id, new DLMS650LoadSurveyBLL().GetFromDate(id), new DLMS650LoadSurveyBLL().GetToDate(id), type);
                //}
                //else if (types.Equals(ApplicationType.IEC_LTCT_650))
                //{
                //    loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                //}
                if (MeterID != null)
                    dSet = loadSurveyParameterBLL.GetColumnNamesForMeterID(MeterID);

                if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                }
                else if (types.Equals(ApplicationType.DLMS_LTCT_650) && MeterID != null)
                {
                    if (dSet.Tables[0].Rows.Count == 1)
                    {
                        long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeStart.Value.ToShortDateString() + " 00:00:00"));
                        long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeEnd.Value.ToShortDateString() + " 23:59:59"));
                        loadSurveyDS = loadSurveyBll.ListDataSetColumnWiseForMeterID(MeterID, frmDate, toDate, type, isPadding);
                    }
                    else if (dSet.Tables[0].Rows.Count > 1)
                    {
                        MessageBox.Show("Parameter mismatch in readout files.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        btnShow.Enabled = false;
                    }
                }



                if (loadSurveyDS != null)
                {
                    if (loadSurveyDS.Tables != null && loadSurveyDS.Tables.Count > 0)
                    {
                        if (loadSurveyDS.Tables[0].Rows.Count == 1 && type == "Demand" && ConfigInfo.ActiveFileType == "DLMS")
                        {
                            chkListLoadSurveyParameters.Items.Clear();
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "Report can not be shown";
                            return;
                        }
                        if (loadSurveyDS.Tables[0].Columns.Count > 0)
                        {
                            lblNoDataFound.Visible = false;
                            chkListLoadSurveyParameters.Items.Clear();
                            foreach (DataColumn column in loadSurveyDS.Tables[0].Columns)
                            {
                                if (column.ColumnName == POWERFACTOR)
                                {
                                    chkListLoadSurveyParameters.Items.Add(POWERFACTORLITERAL);
                                }
                                else
                                {
                                    chkListLoadSurveyParameters.Items.Add(column.ColumnName);
                                }
                            }
                        }
                        else
                        {
                            lblNoDataFound.Visible = true;
                            lblNoDataFound.Text = "No Data Found";
                        }
                    }
                    else
                    {
                        lblNoDataFound.Visible = true;
                        lblNoDataFound.Text = "No Data Found";
                    }
                }
                else
                {
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = "No Data Found";
                }
                for (int itemCounter = 0; itemCounter < chkListLoadSurveyParameters.Items.Count; itemCounter++)
                {
                    chkListLoadSurveyParameters.SetItemCheckState(itemCounter, CheckState.Checked);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FillLoadSurveyParameters()", ex);
                
            }           
        }

        private void ShowReport()
        {
            if (chkListLoadSurveyParameters.CheckedItems.Count <= 0)
            {
                return;
            }
            ReportForm ObjRptForm = new ReportForm();
            JUSCOLoadSurveyEnergyNew juscoLoadSurveyEnergyNew = new JUSCOLoadSurveyEnergyNew();
            try
            {
                /* Add BCS Version in Report header */
                CrystalDecisions.CrystalReports.Engine.TextObject txtBCSVersion = (CrystalDecisions.CrystalReports.Engine.TextObject)juscoLoadSurveyEnergyNew.ReportDefinition.ReportObjects["txtBCSVersion"];
                txtBCSVersion.Text = Common.GetBCSVersion();
                /* Add BCS Version in Report header */
                if (!isPUMA)
                {
                    juscoLoadSurveyEnergyNew.Section4.SectionFormat.EnableSuppress = true;
                }
                // Added to solve bug 92274. 
                if (!chkListLoadSurveyParameters.CheckedItems.Contains(TAMPERSTATUS))
                {
                    juscoLoadSurveyEnergyNew.Section4.SectionFormat.EnableSuppress = true;
                    juscoLoadSurveyEnergyNew.ReportFooterSection2.SectionFormat.EnableSuppress = true;
                }
                else
                {
                    if (ConfigInfo.ActiveMeterType == "1P-2W")
                    {
                        juscoLoadSurveyEnergyNew.Section4.SectionFormat.EnableSuppress = true;
                    }
                    else
                    {
                        juscoLoadSurveyEnergyNew.ReportFooterSection2.SectionFormat.EnableSuppress = true;
                    }
                }
                TextObject headerText = (TextObject)juscoLoadSurveyEnergyNew.GroupHeaderSection1.ReportObjects["Text100"] as TextObject;
                if (headerText != null)
                {
                    if (SMD_rbtnLoadSurveyDemand.Checked)
                    {
                        headerText.Text = DEMANDHEADERTEXT;
                    }
                    else
                    {
                        headerText.Text = ENERGYHEADERTEXT;
                    }
                }
                if (chkListLoadSurveyParameters.CheckedItems.Count > 0)
                {
                    for (int selectedItemCount = 0; selectedItemCount < chkListLoadSurveyParameters.CheckedItems.Count; selectedItemCount++)
                    {
                        if (selectedItemCount != 0)
                        {
                            TextObject groupHeaderTextObject = (TextObject)juscoLoadSurveyEnergyNew.GroupHeaderSection1.ReportObjects["GroupHeader" + (selectedItemCount - 1).ToString()] as TextObject;
                            TextObject groupUnitTextObject = (TextObject)juscoLoadSurveyEnergyNew.GroupHeaderSection1.ReportObjects["GroupUnit" + (selectedItemCount - 1).ToString()] as TextObject;
                            if (groupHeaderTextObject != null)
                            {
                                groupHeaderTextObject.Text = chkListLoadSurveyParameters.CheckedItems[selectedItemCount].ToString();
                                if (groupUnitTextObject != null)
                                {
                                    groupUnitTextObject.Text = GetUnit(groupHeaderTextObject.Text);
                                    if (!string.IsNullOrEmpty(groupUnitTextObject.Text))
                                    {
                                        groupUnitTextObject.Text = "(" + groupUnitTextObject.Text + ")";
                                    }
                                }

                            }
                        }
                    }
                    // Apply modern blue theme and custom logo before rendering
                    ReportThemeHelper.Apply(juscoLoadSurveyEnergyNew);
                    juscoLoadSurveyEnergyNew.SetDataSource(reportXSD);
                    ObjRptForm.rptViewer.ReportSource = juscoLoadSurveyEnergyNew;
                    Cursor.Current = Cursors.Default;
                    ObjRptForm.rptViewer.Zoom(1);
                    this.Hide();
                    // SB code change Start - 20180629 - Multiple Analysis View
                    //ObjRptForm.ShowDialog();
                    ObjRptForm.Show();
                    // SB code change End - 20180629 - Multiple Analysis View
                    reportXSD.Clear();
                    this.Show();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "ShowReport()", ex);
            }
        }

        private string GetUnit(string columnName)
        {
            if (columnName.ToLower().Contains(CURRENT))
            {
                return AMPERE;
            }
            else if (columnName.ToLower().Contains(VOLTAGE))
            {
                return VOLT;
            }
            else if (columnName.ToLower().Contains(KVAR) && columnName.ToLower().Contains(ENERGY))
            {
                return ENERGYKVARH;
            }
            else if (columnName.ToLower().Contains(KVAR) && columnName.ToLower().Contains(DEMAND))
            {
                return DEMANDKVAR;
            }
            else if (columnName.ToLower().Contains(KVA) && columnName.ToLower().Contains(ENERGY))
            {
                return ENERGYKVAH;
            }
            else if (columnName.ToLower().Contains(KW) && columnName.ToLower().Contains(ENERGY))
            {
                return ENERGYKWH;
            }
            else if (columnName.ToLower().Contains(KVA) && columnName.ToLower().Contains(DEMAND))
            {
                return DEMANDKVA;
            }
            else if (columnName.ToLower().Contains(KW) && columnName.ToLower().Contains(DEMAND))
            {
                return DEMANDKW;
            }
            else if (columnName.ToLower().Contains(HERTZ))
            {
                return HZ;
            }
            else
            {
                return string.Empty;
            }
        }


        
        #endregion


        #region Event Handlers

        //private void SMD_rbtnLoadSurveyDemand_CheckedChanged(object sender, EventArgs e)
        //{
        //    FillLoadSurveyParameters();
        //}

        /// <summary>
        ///generates a Report, OnClick of show button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                long id = 0;
                if (ActiveMeterIdLoadSurvey == null)
                {
                    MessageBox.Show("No MeterID Selected.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //if (chkListLoadSurveyParameters.CheckedItems.Count <= 0)
                //{
                //    return;
                //}
                ConfigInfo.ActiveMeterType = loadSurveyBll.GetActiveMeterTypeByMeterId(ActiveMeterIdLoadSurvey.Trim());
                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();
                this.Cursor = Cursors.WaitCursor;
                detailsDS = ListConsumerMeterDetailForMeterID(ActiveMeterIdLoadSurvey);
                //detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                    FillConsumerMeterDetails(detailsDS);
                else
                {
                    DataTable meterIDDT = new DataTable();
                    meterIDDT.Columns.Add("MeterID", typeof(string));
                    meterIDDT.Rows.Add(ActiveMeterIdLoadSurvey);
                    meterIDDS.Tables.Add(meterIDDT);

                    //  meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                        FillMeterID(meterIDDS);
                }
                string type = "Energy";
                if (SMD_rbtnLoadSurveyDemand.Checked)
                    type = "Demand";
                bool isPadding = false;
                if (rdbWithPadding.Checked)
                    isPadding = true;
                // long id = Int64.Parse(ConfigInfo.ActiveMeterDataId);
                loadSurveyDS = new DataSet();
                types = ConfigInfo.GetApplicationType();
                //if (types.Equals(ApplicationType.DLMS_LTCT_650))
                //{
                //    loadSurveyDS = new DLMS650LoadSurveyBLL().ListDataSetColumnWise(id, new DLMS650LoadSurveyBLL().GetFromDate(id), new DLMS650LoadSurveyBLL().GetToDate(id), type);
                //}
                //else if (types.Equals(ApplicationType.IEC_LTCT_650))
                //{
                //    loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                //}



                if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    loadSurveyDS = new LoadSurveyBLL().ListDataSet(id, new LoadSurveyBLL().GetFromDate(id), new LoadSurveyBLL().GetToDate(id), type);
                }
                else if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeStart.Value.ToShortDateString() + " 00:00:00"));
                    long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(dateTimeEnd.Value.ToShortDateString() + " 23:59:59"));
                    loadSurveyDS = loadSurveyBll.ListDataSetColumnWiseForMeterID(ActiveMeterIdLoadSurvey, frmDate, toDate, type, isPadding);
                }

                if (loadSurveyDS != null && loadSurveyDS.Tables[0].Rows.Count > 0)
                {
                    if (type == "Demand" && loadSurveyDS.Tables[0].Rows.Count == 1 && ConfigInfo.ActiveFileType == "DLMS")
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    if (FillLoadSurveyXSD(loadSurveyDS))
                        ShowReport();
                    this.Cursor = Cursors.Default;
                }
                else
                {

                    if (loadSurveyBll.IntegrationPeriodStatus)
                    {
                        MessageBox.Show("Data Corrupt", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No data available", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                
            }          
        }

        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSurveyReport_Load(object sender, EventArgs e)
        {
            try
            {
                //// Check utility
                ActiveMeterIdLoadSurvey = null;
                if (UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
                {
                    isPUMA = true;
                }
                SMD_rbtnLoadSurveyEnergy.Checked = true;
                rdbNoPadding.Checked = true;
                if (chkListLoadSurveyParameters.Items.Count > 0)
                {
                    chkListLoadSurveyParameters.SetItemCheckState(0, CheckState.Checked);
                    chkListLoadSurveyParameters.ItemCheck += new ItemCheckEventHandler(chkListLoadSurveyParameters_ItemCheck);
                }

                GroupRadioBoxAdd();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "LoadSurveyReport_Load(object sender, EventArgs e)", ex);
                
            }           
        }

        void chkListLoadSurveyParameters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            e.NewValue = CheckState.Checked;
        }

        /// <summary>
        /// radio box On Load Survey report form , to select for energy 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SMD_rbtnLoadSurveyEnergy_CheckedChanged(object sender, EventArgs e)
        {
            FillLoadSurveyParameters();
        }
        /// <summary>
        /// radio box On Load Survey report form , to select for energy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SMD_rbtnLoadSurveyEnergy_CheckedChanged_1(object sender, EventArgs e)
        {
            FillLoadSurveyParameters();
        }


        /// <summary>
        /// event handler for radioGroupBox , when a radiobutton is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton radioButtonCheck = sender as RadioButton;
                if (radioButtonCheck != null)
                {
                    if (radioButtonCheck.Checked)
                    {
                        dateTimeStart.Enabled = true;
                        dateTimeEnd.Enabled = true;

                        btnShow.Enabled = true;
                        //selectedRadioButtonCount = groupBox.Controls.GetChildIndex(radioButtonCheck) + 1;
                        // ConfigInfo.ActiveMeterDataIdLoadSurvey = selectedRadioButtonCount;

                        //ActiveMeterIdLoadSurvey = mainForm.MeterId[selectedRadioButtonCount-1];
                        ActiveMeterIdLoadSurvey = radioButtonCheck.Text;
                        SetupDateTime();
                        //resetting mindate and maxvalue

                        FillLoadSurveyParameters();
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "radioBox_CheckedChanged(object sender, EventArgs e)", ex);
                
            }
        }
        /// <summary>
        /// to check if the user selects ToDate less than FromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeStart_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (dateTimeStart.Value > dateTimeEnd.Value)
                {

                    MessageBox.Show("StartDate cannot be greater than EndDate.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dateTimeStart.Value = DateUtility.LongToDateTime(changedFromDate);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dateTimeStart_CloseUp(object sender, EventArgs e)", ex);
                
            }
          
        }
        /// <summary>
        /// to check if the user selects ToDate less than FromDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeEnd_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (dateTimeStart.Value > dateTimeEnd.Value)
                {
                    MessageBox.Show("StartDate cannot be greater than EndDate.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dateTimeEnd.Value = DateUtility.LongToDateTime(changedToDate);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "dateTimeEnd_CloseUp(object sender, EventArgs e)", ex);
            }
       
        }
        /// <summary>
        /// to check if end date is not greater than end date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeEnd_DropDown(object sender, EventArgs e)
        {
            try
            {
                changedFromDate = DateUtility.DateTimeToLong(dateTimeStart.Value);
                changedToDate = DateUtility.DateTimeToLong(dateTimeEnd.Value);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "dateTimeEnd_DropDown(object sender, EventArgs e)", ex);
                
            }
            
        }

        /// <summary>
        /// to check if start date is not greater than end date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimeStart_DropDown(object sender, EventArgs e)
        {
            try
            {
                changedFromDate = DateUtility.DateTimeToLong(dateTimeStart.Value);
                changedToDate = DateUtility.DateTimeToLong(dateTimeEnd.Value);
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "dateTimeStart_DropDown(object sender, EventArgs e)", ex);
            }
        

        }


        #endregion

       
        

      
    }
}
