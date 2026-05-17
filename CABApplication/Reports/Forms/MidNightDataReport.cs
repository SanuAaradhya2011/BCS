using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.DALC.Data;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABApplication.Reports.Forms;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class MidNightDataReport : CABForm
    {
        DLMS650LoadSurveyBLL loadSurveyBLL;
        DLMS650LoadSurveyDAL loadSurveyDAL;
        MeterDataBLL meterDataBll;
        MeterDataEntity meterDataEntity = null;
        private FileReportDataSet reportXSD = null;
        private DataSet MidNightDataSet;
        private DataSet MeterDetailsDataSet;
        string dataUnavailable = "--------";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MidNightDataReport).ToString());
        public MidNightDataReport()
        {
            InitializeComponent();

            //initializing objects
            loadSurveyBLL = new DLMS650LoadSurveyBLL();
            meterDataBll = new MeterDataBLL();
            reportXSD = new FileReportDataSet();
            DataSet detailsDS = new DataSet();
            DataSet meterIDDS = new DataSet();
            this.Cursor = Cursors.WaitCursor;
            
            // To get consumer meter details.
            detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
            if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                FillConsumerMeterDetails(detailsDS);
            else
            {
                // To get meter id if consumer details not present.
                meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    FillMeterID(meterIDDS);
            }
            //FillMidNightDataDetails will fetch data from DB
            FillMidNightDataDetails();

            //from will close
            this.Close();
        }
        private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
        {
            return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
        }

        private DataSet ListConsumerMeterDetails(long activeMeterDataId)
        {
            return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
        }
        // Fill meter id.
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
                        reportRow["MeterNo"] = dataUnavailable;
                }
                reportRow["ActiveMeter"] = "No";
                reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
            }
        }
        // Fill consumer details.
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
                        reportRow["MeterNo"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Consumer_Number"].ToString()))
                        reportRow["ConsumerNo"] = CommonBLL.GetFormattedData(row["Consumer_Number"].ToString());
                    else
                        reportRow["ConsumerNo"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_Location"].ToString()))
                        reportRow["Location"] = CommonBLL.GetFormattedData(row["Meter_Location"].ToString());
                    else
                        reportRow["Location"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_AllocationDate"].ToString()))
                        reportRow["InstallationDate"] = DateUtility.LongToDateTime(Convert.ToInt64(row["Meter_AllocationDate"].ToString())).ToString(ConfigInfo.DateFormat());
                    else
                        reportRow["InstallationDate"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterType_Name"].ToString()))
                        reportRow["MeterType"] = CommonBLL.GetFormattedData(row["MeterType_Name"].ToString());
                    else
                        reportRow["MeterType"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["MeterModel_Name"].ToString()))
                        reportRow["MeterModel"] = CommonBLL.GetFormattedData(row["MeterModel_Name"].ToString());
                    else
                        reportRow["MeterModel"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Meter_EMF"].ToString()))
                        reportRow["EMF"] = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                    else
                        reportRow["EMF"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Region_Name"].ToString()))
                        reportRow["Region"] = CommonBLL.GetFormattedData(row["Region_Name"].ToString());
                    else
                        reportRow["Region"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Circle_Name"].ToString()))
                        reportRow["Circle"] = CommonBLL.GetFormattedData(row["Circle_Name"].ToString());
                    else
                        reportRow["Circle"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Division_Name"].ToString()))
                        reportRow["Division"] = CommonBLL.GetFormattedData(row["Division_Name"].ToString());
                    else
                        reportRow["Division"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["CMRI_Number"].ToString()))
                        reportRow["CMRINumber"] = CommonBLL.GetFormattedData(row["CMRI_Number"].ToString());
                    else
                        reportRow["CMRINumber"] = dataUnavailable;

                    if (row["Status"].ToString().Equals("0"))
                        reportRow["ActiveMeter"] = "No";
                    else
                        reportRow["ActiveMeter"] = "Yes";

                    if (!string.IsNullOrEmpty(row["Meter_ContractDemand"].ToString()))
                        reportRow["ContractDemand"] = CommonBLL.GetFormattedData(row["Meter_ContractDemand"].ToString());
                    else
                        reportRow["ContractDemand"] = dataUnavailable;

                    reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
                    reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                }
            }
        }
        // Added to solve midnight data difference in fast download and direct read out issue 
        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }
        private void FillMidNightDataDetails()
        {
            try
            {
                string fileName = string.Empty;
                bool isKWParsed = false;
                bool isKVAParsed = false;
                decimal KWValue =0;
                decimal KVAValue = 0;
                //call to DAL to get From and To date
                long lsFromDateMD = loadSurveyBLL.GetFromDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                long lsToDateMD = loadSurveyBLL.GetToDate(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

                //call to DAL Meter Details
                meterDataEntity = new MeterDataBLL().GetDetailData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId)) as MeterDataEntity;
                // Added to solve midnight data difference in fast download and direct read out issue 
                DataSet fileDataset = new DataSet();
                fileDataset = ListFileName(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (fileDataset != null)
                {
                    if (fileDataset.Tables[0].Rows.Count > 0)
                    {
                        fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                    }
                }
                //call to DAL to get MidNight Data
                MidNightDataSet = loadSurveyBLL.GetMidNightData(fileName,Convert.ToInt64(ConfigInfo.ActiveMeterDataId), lsFromDateMD, lsToDateMD);
                // Added to solve bug 83732. Demand calculation on basis of integration period.
                int div = 1;
                DataSet dSet = loadSurveyBLL.ListDataSet(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), lsFromDateMD, lsToDateMD);
                int MDInterval = 0;
                if (dSet != null)
                {
                    if (dSet.Tables[0].Rows.Count > 1)
                    {
                        TimeSpan ts = DateUtility.LongToDateTime(Int64.Parse(dSet.Tables[0].Rows[1][0].ToString())) - DateUtility.LongToDateTime(Int64.Parse(dSet.Tables[0].Rows[0][0].ToString()));
                        MDInterval = (int)ts.TotalMinutes;
                        if (MDInterval == 15)
                            div = 4;
                        if (MDInterval == 30)
                            div = 2;
                    }
                    else
                    {
                        div = 1;
                    }
                }
                //check for null condition
                if (MidNightDataSet != null)
                {
                    if (MidNightDataSet.Tables.Count != 0 || MidNightDataSet.Tables[0].Rows.Count != 0)
                    {
                        // Added to calculate demand from energy values.
                        for (int i = 0; i < MidNightDataSet.Tables[0].Rows.Count; i++)
                        {
                            isKWParsed = decimal.TryParse(MidNightDataSet.Tables[0].Rows[i][17].ToString(),out KWValue);
                            isKVAParsed = decimal.TryParse(MidNightDataSet.Tables[0].Rows[i][18].ToString(), out KVAValue);
                            if (isKWParsed)
                            {
                                MidNightDataSet.Tables[0].Rows[i][17] = Convert.ToDecimal(KWValue * div);
                            }
                            else
                            {
                                MidNightDataSet.Tables[0].Rows[i][17] = MidNightDataSet.Tables[0].Rows[i][17].ToString();
                            }
                            if (isKVAParsed)
                            {
                                MidNightDataSet.Tables[0].Rows[i][18] = Convert.ToDecimal(KVAValue * div);
                            }
                            else
                            {
                                MidNightDataSet.Tables[0].Rows[i][18] = MidNightDataSet.Tables[0].Rows[i][18].ToString();
                            }
                        }
                        //call FillMidNightDataDetails to fill schema
                        FillMidNightDataSchema(MidNightDataSet);
                        //call ShowReport to create report
                        ShowReport();
                    }
                }
                else
                    // For bug 72651.Message changed.
                    MessageBox.Show("No data available.");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "FillMidNightDataDetails()", ex);
            }
        }

        private void FillMidNightDataSchema(DataSet dsMidNightData)
        {
            meterDataEntity = (MeterDataEntity)meterDataBll.GetDetailData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));

            //filling the FileReportDataSet
            DataRow reportRow;
            
            if (dsMidNightData.Tables[0].Rows.Count > 0)
            {
                //reportRow = reportXSD.Tables["MidNightDataDetails"].NewRow();
                
                foreach (DataRow row in dsMidNightData.Tables[0].Rows)
                {
                    reportRow = reportXSD.Tables["MidNightDataDetails"].NewRow();

                    if (meterDataEntity.MeterID != null)
                    {
                        reportRow["MeterId"] = meterDataEntity.MeterID;
                    }

                    reportRow["ReadingDate"] = DateTime.Now.ToString(ConfigInfo.DateFormat() + " HH:mm");

                    if (!string.IsNullOrEmpty(row["Date"].ToString()))
                        reportRow["MeterDate"] = row["Date"].ToString();
                    else
                        reportRow["MeterDate"] = dataUnavailable;
                    // Changed Cu to Daily to solve DLMS_110.
                    if (!string.IsNullOrEmpty(row["Daily-kWh (1.0.1.29.0.255;3;2)"].ToString()))
                        reportRow["Cu-kWh"] = row["Daily-kWh (1.0.1.29.0.255;3;2)"].ToString();
                    else
                        reportRow["Cu-kWh"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Daily-kVAh (1.0.9.29.0.255;3;2)"].ToString()))
                        reportRow["Cu-kVAh"] = row["Daily-kVAh (1.0.9.29.0.255;3;2)"].ToString();
                    else
                        reportRow["Cu-kVAh"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Daily-kvarh lag (1.0.5.29.0.255;3;2)"].ToString()))
                        reportRow["Cu-kvarh-lag"] = row["Daily-kvarh lag (1.0.5.29.0.255;3;2)"].ToString();
                    else
                        reportRow["Cu-kvarh-lag"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Daily-kvarh lead (1.0.8.29.0.255;3;2)"].ToString()))
                        reportRow["Cu-kvarh-lead"] = row["Daily-kvarh lead (1.0.8.29.0.255;3;2)"].ToString();
                    else
                        reportRow["Cu-kvarh-lead"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-VRVoltage (1.0.32.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-rVoltage"] = row["Max-VRVoltage (1.0.32.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-rVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-VRVoltage (1.0.32.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-rVoltage"] = row["Min-VRVoltage (1.0.32.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-rVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-IRCurrent (1.0.31.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-rCurrent"] = row["Max-IRCurrent (1.0.31.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-rCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-IRCurrent (1.0.31.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-rCurrent"] = row["Min-IRCurrent (1.0.31.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-rCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-VYVoltage (1.0.52.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-yVoltage"] = row["Max-VYVoltage (1.0.52.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-yVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-VYVoltage (1.0.52.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-yVoltage"] = row["Min-VYVoltage (1.0.52.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-yVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-IYCurrent (1.0.51.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-yCurrent"] = row["Max-IYCurrent (1.0.51.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-yCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-IYCurrent (1.0.51.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-yCurrent"] = row["Min-IYCurrent (1.0.51.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-yCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-VBVoltage (1.0.72.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-bVoltage"] = row["Max-VBVoltage (1.0.72.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-bVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-VBVoltage (1.0.72.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-bVoltage"] = row["Min-VBVoltage (1.0.72.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-bVoltage"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Max-IBCurrent (1.0.71.27.0.255;3;2)"].ToString()))
                        reportRow["Max Avg-bCurrent"] = row["Max-IBCurrent (1.0.71.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Max Avg-bCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["Min-IBCurrent (1.0.71.27.0.255;3;2)"].ToString()))
                        reportRow["Min Avg-bCurrent"] = row["Min-IBCurrent (1.0.71.27.0.255;3;2)"].ToString();
                    else
                        reportRow["Min Avg-bCurrent"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["MD - kW (1.0.1.29.0.255;3;2)"].ToString()))
                        reportRow["MD-kW"] = row["MD - kW (1.0.1.29.0.255;3;2)"].ToString();
                    else
                        reportRow["MD-kW"] = dataUnavailable;

                    if (!string.IsNullOrEmpty(row["MD - kVA (1.0.9.29.0.255;3;2)"].ToString()))
                        reportRow["MD-kVA"] = row["MD - kVA (1.0.9.29.0.255;3;2)"].ToString();
                    else
                        reportRow["MD-kVA"] = dataUnavailable;

                    reportXSD.Tables["MidNightDataDetails"].Rows.Add(reportRow);
                }
            }
        }

        private void ShowReport()
        {
            //creating object of ReportForm and MidNightReport
            ReportForm objReportForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.MidNightReport midNightReport = new CABApplication.Reports.DLMS_Detailed_Reports.MidNightReport();

            // Apply modern blue theme and custom logo before rendering
            ReportThemeHelper.Apply(midNightReport);

            //assinging data source to MidNightReport
            midNightReport.SetDataSource(reportXSD);

            //assinging data source to ReportForm
            objReportForm.rptViewer.ReportSource = midNightReport;

            Cursor.Current = Cursors.Default;
            objReportForm.rptViewer.Zoom(1);
            this.Hide();
            objReportForm.ShowDialog();
            reportXSD.Clear();
            this.Show();
            Cursor.Current = Cursors.Default;
        }
    }
}
