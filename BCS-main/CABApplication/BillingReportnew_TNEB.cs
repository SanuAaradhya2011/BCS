using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CAB.UI;
using CABApplication;
using CABApplication.Reports.DLMS_Detailed_Reports;
using CABApplication.Reports.Forms;
using LTCTBLL;
using System.ComponentModel;
using CABEntity;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Channel.Formatter;
using Hunt.EPIC.Logging;

namespace CABApplication.Reports.Forms
{
    public partial class BillingReportnew_TNEB : Form
    {
        private FileReportDataSet reportXSD = null;
        DataSet BillingDS = new DataSet();
        DataSet InstantNewDS = new DataSet(); 
        bool isPUMA = false;
        ApplicationType types;
        private int meterModelNumber = 0;
        private const string dateUnavailable = "--------";
        private const string HISTORY = "History";
        private const string FILEEXTENSION = ".FDL";
        string dateFormat = ConfigInfo.DateFormat() + " HH:mm";
        private const string NOTAPPLIED = "Not Applied";
        private const string APPLIED = "Applied";
        DataRow reportRow;
        private static int powerFactorCount = 0;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(BillingReportnew_TNEB).ToString());
        public BillingReportnew_TNEB() 
        {
            InitializeComponent();
        }

        private void BillingReport_TNEB_Load(object sender, EventArgs e)
        {
            // Check utility
            if (UtilityEntity.Generic == UtilityDetails.GetUtilityDetails())
            {
                isPUMA = true;
            }
                   
        }
       
        private void SMD_btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DataSet ListFileName(long activeMeterDataId)
        {
            return new FileUploadMasterBLL().GetCABFileNameWithMeterDataId(activeMeterDataId);
        }

        private DataSet ListPowerFactorData(long activeMeterDataId)
        {
            return new ReportBLL().GetPowerFactorReportData(activeMeterDataId);
        }

        private DataSet ListMainEnergyData(long activeMeterDataId)
        {
            return new ReportBLL().GetMainEnergyReportData(activeMeterDataId);
        }

        private void FillBilling_TNEB(DataSet mainBillingData)
        {
            DataRow reportRow;

            if (mainBillingData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in mainBillingData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["BillingReport_TNEB"].NewRow();

                        for (int colCount = 0; colCount < mainBillingData.Tables[0].Columns.Count; colCount++)
                        {
                            reportRow[colCount] = row[colCount].ToString();
                            
                        }
                        reportXSD.Tables["BillingReport_TNEB"].Rows.Add(reportRow);

                    }
                }
              }
        }

        private void FillNewInstant_TNEB(DataSet NewInstantData)
        {
            DataRow reportRow;

            if (NewInstantData.Tables[0].Rows.Count > 0)
            {
                types = ConfigInfo.GetApplicationType();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    foreach (DataRow row in NewInstantData.Tables[0].Rows)
                    {
                        reportRow = reportXSD.Tables["Instantaneous_TNEB"].NewRow();

                        for (int colCount = 0; colCount < NewInstantData.Tables[0].Columns.Count; colCount++)
                        {
                            reportRow[colCount] = row[colCount].ToString();

                        }
                        reportXSD.Tables["Instantaneous_TNEB"].Rows.Add(reportRow);

                    }
                }
            }
        }

        private void ShowReport()
        {
            ReportForm ObjRptForm = new ReportForm();
            CABApplication.Reports.DLMS_Detailed_Reports.MainReport generalReport = new MainReport();
            if (reportXSD.Tables["EnergyTableNET"].Rows.Count == 0)
            {
                generalReport.secMainEnergyNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PowerFactorNET"].Rows.Count == 0)
            {
                generalReport.secPowerFactorNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TODAveragePF"].Rows.Count == 0)
            {
                generalReport.secTODAveragePF.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TODEnergyTableNET"].Rows.Count == 0)
            {
                generalReport.secTODEnergyNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TODEnergyConsumptionTableNET"].Rows.Count == 0)
            {
                generalReport.secTODEnergyConsumptionNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["MaximumDemandTableNET"].Rows.Count == 0)
            {
                generalReport.secMaximumDemandNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TODDemandTableNET"].Rows.Count == 0)
            {
                generalReport.secTODDemandNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["EnergyConsumptionTableNET"].Rows.Count == 0)
            {
                generalReport.secMainEnergyConsumptionNET.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["NameplateDetails"].Rows.Count == 0)
            {
                generalReport.SecBillingGeneral.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650InstantTable"].Rows.Count == 0)
            {
                generalReport.SecMeterInstant.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["BillingReport_TNEB"].Rows.Count == 0)
            {
                generalReport.secBillingReportTNEB.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["Instantaneous_TNEB"].Rows.Count == 0)
            {
                generalReport.secInstantReportTNEB.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["RS485"].Rows.Count == 0)
            {
                generalReport.RS485Report.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["AbcCode1"].Rows.Count == 0)
            {
                generalReport.ABCCodeReport.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["LoadFactorDT"].Rows.Count == 0)
            {
                generalReport.secLoadFactor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PowerOnOffDuration"].Rows.Count == 0)
            {
                generalReport.secPowerOnOffDuration.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["AnomalyDynamic"].Rows.Count == 0)
            {
                generalReport.AnomalySection.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TouConfiguration"].Rows.Count == 0)
            {
                generalReport.TouConfiguration.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650MainEnergyTable"].Rows.Count == 0)
            {
                generalReport.SecMainEnergy.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650EnergyConsumptionTable"].Rows.Count == 0)
            {
                generalReport.SecMainEnergyConsumption.SectionFormat.EnableSuppress = true;
            }
                     
            if ((reportXSD.Tables["DLMS650TODEnergyTable"].Rows.Count == 0))
            {
                generalReport.SecTODEnergy.SectionFormat.EnableSuppress = true;
            }
           
            if ((reportXSD.Tables["TODKWhTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhTable"].Rows.Count == 0))
            {
                generalReport.SecTODKVAhEnergy.SectionFormat.EnableSuppress = true;
                generalReport.SecTODKWhEnergy.SectionFormat.EnableSuppress = true;
            }

            if ((reportXSD.Tables["DLMS650TODEnergyConsumptionTable"].Rows.Count == 0))
            {
                generalReport.SecTODConsumption.SectionFormat.EnableSuppress = true;
            }
            if ((reportXSD.Tables["TODKWhConsumptionTable"].Rows.Count == 0) || (reportXSD.Tables["TODKVAhConsumptionTable"].Rows.Count == 0))
            {
                generalReport.SecTODKVAhConsumption.SectionFormat.EnableSuppress = true;
                generalReport.SecTODKWhConsumption.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650MaximumDemandTable"].Rows.Count == 0)
            {
                generalReport.SecMaximumDemand.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650TODDemandTable"].Rows.Count == 0)
            {
                generalReport.SecTODDemand.SectionFormat.EnableSuppress = true;
            }
                        
            if (reportXSD.Tables["DLMS650PowerFactorTable"].Rows.Count == 0)
            {
                generalReport.SecPowerFactor.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650LoadSurvey"].Rows.Count == 0)
            {
                generalReport.SecLoadSurveyDemand.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650LoadSurveyEnergy"].Rows.Count == 0)
            {
                generalReport.SecLoadSurveyEnergy.SectionFormat.EnableSuppress = true;
            }
            
            if (reportXSD.Tables["TamperTableDynamic"].Rows.Count == 0)
            {
                generalReport.SecTamper.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PowerFailureTable"].Rows.Count == 0)
            {
                generalReport.SecPowerFailure.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TransactionDynamic"].Rows.Count == 0)
            {
                generalReport.SecTransactions.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PhasorDiagramTable"].Rows.Count == 0)
            {
                generalReport.PhasorReportSection.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PowerOffDuration"].Rows.Count == 0)
            {
                generalReport.DetailSection1.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["Miscelleneous"].Rows.Count == 0)
            {
                generalReport.Miscelleneous.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["FraudEnergyTable"].Rows.Count == 0)
            {
                generalReport.DetailSection2.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TouWeekTable"].Rows.Count == 0)
            {
                generalReport.DetailSection5.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TouSeasonTable"].Rows.Count == 0)
            {
                generalReport.DetailSection6.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["TouDayProfileTable"].Rows.Count == 0)
            {
                generalReport.DetailSection7.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["TouSpecialDayTable"].Rows.Count == 0)
            {
                generalReport.DetailSection12.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["DisconnectControl"].Rows.Count == 0)
            {
                generalReport.DetailSection15.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["LoadControl"].Rows.Count == 0)
            {
                generalReport.DetailSection16.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["LoadControl1Psmartmeter"].Rows.Count == 0)
            {
                generalReport.DetailSection17.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["OtherConfigurations"].Rows.Count == 0)
            {
                generalReport.DetailSection4.SectionFormat.EnableSuppress = true;
                generalReport.DetailSection3.SectionFormat.EnableSuppress = true;
                generalReport.DetailSection8.SectionFormat.EnableSuppress = true;
                generalReport.DetailSection9.SectionFormat.EnableSuppress = true;
            
            }
            if (reportXSD.Tables["TODFutureActivationDate"].Rows.Count == 0)
            {
                generalReport.DetailSection10.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["AverageLoadDT"].Rows.Count == 0)
            {
                generalReport.AverageLoad.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PaymentMode"].Rows.Count == 0)
            {
                generalReport.PaymentMode.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["MeteringMode"].Rows.Count == 0)
            {
                generalReport.MeteringMode.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["LoadlimitMode"].Rows.Count == 0)
            {
                generalReport.LoadLimit.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["SlidingdemandMode"].Rows.Count == 0)
            {
                generalReport.SlidingDemand.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["OpticalRJportMode"].Rows.Count == 0)
            {
                generalReport.PortConfiguration.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["DLMS650CumulativeMDTable"].Rows.Count == 0)
            {
                generalReport.CumulativeMD.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["DIPforSmartmeter"].Rows.Count == 0)
            {
                generalReport.CumulativeDIP.SectionFormat.EnableSuppress = true;
            }
            if (reportXSD.Tables["PulseEnergyType"].Rows.Count == 0)
            {
                generalReport.PulseEnergyType.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["SoftwareBillingTable"].Rows.Count == 0)
            {
                generalReport.SoftwareBilling.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["AutoLockTable"].Rows.Count == 0)
            {
                generalReport.AutoLock.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["KvahSelectionTable"].Rows.Count == 0)
            {
                generalReport.KVAHSelection.SectionFormat.EnableSuppress = true;
            }

            if (reportXSD.Tables["ManualMDResetTable"].Rows.Count == 0)
            {
                generalReport.ManualButtonMDReset.SectionFormat.EnableSuppress = true;
            }


            try
            {
                if (!isPUMA)
                {
                    generalReport.Section4.SectionFormat.EnableSuppress = true;
                }
                                
                generalReport.SetDataSource(reportXSD);
                ObjRptForm.rptViewer.ReportSource = generalReport;
                    Cursor.Current = Cursors.Default;
                    ObjRptForm.rptViewer.Zoom(1);
                    this.Hide();
                    // SB code change Start - 20180629 - Multiple Analysis View
                    //ObjRptForm.ShowDialog();
                    ObjRptForm.Show();
                    // SB code change Start - 20180629 - Multiple Analysis View
                    reportXSD.Clear();
                    this.Show();
                }

            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "ShowReport()", ex);
            }
        }
              

       private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       private void FillMeterID(DataSet meterIdDS)
       {
           
           if (meterIdDS != null && meterIdDS.Tables[0].Rows.Count > 0)
           {
               //reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
               foreach (DataRow row in meterIdDS.Tables[0].Rows)
               {
                   if (!string.IsNullOrEmpty(row["MeterID"].ToString()))
                       reportRow["MeterNo"] = row["MeterID"].ToString();
                   else
                       reportRow["MeterNo"] = dateUnavailable;

                   reportRow["ReadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                   if (MeterFileList.filesource == "CMRI")
                   {
                       string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                       reportRow["CMRITime"] = CMRITime.Substring(11);
                   }
                   else
                       reportRow["CMRITime"] = "";
                   //string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                   //reportRow["CMRITime"] = CMRITime.Substring(11); 
                   reportRow["UploadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                string FileCreationTime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                reportRow["FileCreationTime"] = FileCreationTime.Substring(11);
                if (!DBNull.Value.Equals(row["NoLoadDataTime"]))
                {
                    reportRow["NoLoadDataTime"] = row["NoLoadDataTime"];
                }
                if (!DBNull.Value.Equals(row["NoPowerSupplyTime"]))
                {
                    reportRow["NoPowerSupplyTime"] = row["NoPowerSupplyTime"];
                }
               }
               reportRow["ActiveMeter"] = "Inactive";
               reportRow["ReadingDate"] = DateTime.Now.ToString(dateFormat);
             //  reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
           }
       }
       private void FillConsumerMeterDetails(DataSet detailsDS)
       {
          // DataRow reportRow;
           DataTable table = new DataTable();
           if (detailsDS.Tables[0].Rows.Count > 0)
           {
             //  reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
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
                  
                   decimal actualEMF = 0;
                   int internalCTRatio = 0;
                   int internalPTRatio = 0;

                   String meterEMF = CommonBLL.GetFormattedData(row["Meter_EMF"].ToString());
                   if (int.TryParse(CommonBLL.GetFormattedData(row["internalCTRatio"].ToString()), out internalCTRatio)
                       && int.TryParse(CommonBLL.GetFormattedData(row["internalPTRatio"].ToString()), out internalPTRatio))
                   {

                   }
                   if (internalCTRatio <= 0)
                   {
                       internalCTRatio = 1;
                   }
                   if (internalPTRatio <= 0)
                   {
                       internalPTRatio = 1;
                   }
                   actualEMF = Convert.ToDecimal(meterEMF) / (internalPTRatio * internalCTRatio);
                 
                   meterEMF = string.Format("{0:F3}", actualEMF);
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

                   if (!string.IsNullOrEmpty(meterEMF))
                       reportRow["EMF"] = meterEMF;
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
                   reportRow["ReadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                   if (MeterFileList.filesource == "CMRI")
                   {
                   string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                   reportRow["CMRITime"] = CMRITime.Substring(11); 
                   }
                   else
                       reportRow["CMRITime"] = "";
                   //string CMRITime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["ReadingDateTime"]));
                   //reportRow["CMRITime"] = CMRITime.Substring(11); 
                   reportRow["UploadingDateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                   string FileCreationTime = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(row["UploadingDateTime"]));
                   reportRow["FileCreationTime"] = FileCreationTime.Substring(11);

                   if (!DBNull.Value.Equals(row["NoLoadDataTime"]))
                   {
                       reportRow["NoLoadDataTime"] = row["NoLoadDataTime"];
                   }
                   if (!DBNull.Value.Equals(row["NoPowerSupplyTime"]))
                   {
                       reportRow["NoPowerSupplyTime"] = row["NoPowerSupplyTime"];
                   }

                 //  reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);
                   break; // VBM - Take only first Row as a meter willl have one customer. .
               }
           }
       }
       private DataSet ListConsumerMeterDetails(long activeMeterDataId)
       {
           return new MeterDataBLL().GetConsumerMeterDetails(activeMeterDataId);
       }
       private DataSet GetMeterIDFromMeterDataID(long activeMeterDataId)
       {
           return new MeterDataBLL().GetMeterIDFromMeterDataID(activeMeterDataId);
       }
       private DataSet ListGeneralData(long activeMeterDataId)
       {
           return new ReportBLL().GetGeneralReportData(activeMeterDataId);
       }
       private int ListDIPData(long activeMeterDataId)
       {
           return new DIPBLL().GetData(activeMeterDataId);
       }
       private BillingTypeEntity ListBillingTypeData(long activeMeterDataId)
       {
           return new BillingTypeBLL().GetData(activeMeterDataId);
       }
       private DataSet ListInstantData(long activeMeterDataId)
       {
           return new ReportBLL().GetInstantReportData(activeMeterDataId);
       }
       private DataSet ListNoPowerSupplyLoadTime(long activeMeterDataId, DataSet NoSupplyLoadData)
       {
           return new MeterDataBLL().ListNoPowerSupplyLoadTime(activeMeterDataId, NoSupplyLoadData);
       }


        private void btnShow_Click(object sender, EventArgs e)
        {
            string resetcount = "";
            string val="";
            int errCount = 0;
            int dip = 0;
            int showReport = 0;
            BillingTypeEntity billingTypeEntity = null;
            types = ConfigInfo.GetApplicationType();
            reportXSD = new FileReportDataSet();
            string errMsg = String.Empty;
            string fileName = string.Empty;
            DataTable BillingTNEB = new DataTable();
            BillingTNEB.Columns.Add("ResetNo");
            BillingTNEB.Columns.Add("AvgPF");
            BillingTNEB.Columns.Add("ResetMode");
            BillingTNEB.Columns.Add("ResetDateTime");
            BillingTNEB.Columns.Add("CumKwh");
            BillingTNEB.Columns.Add("KWMDOCC");
            BillingTNEB.Columns.Add("MDOccDateTime");
            DataTable InstantTNEB = new DataTable();
           // InstantTNEB.Columns.Add("Phase");
            InstantTNEB.Columns.Add("Voltage");
            InstantTNEB.Columns.Add("Current");
            InstantTNEB.Columns.Add("NeutralCurrent");
            InstantTNEB.Columns.Add("kW");
            InstantTNEB.Columns.Add("PF");
                             
            try
            {
                
                // Added to get the filename.
                DataSet fileDataset = new DataSet();
                fileDataset = ListFileName(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (fileDataset != null)
                {
                    if (fileDataset.Tables[0].Rows.Count > 0)
                    {
                        fileName = fileDataset.Tables[0].Rows[0][1].ToString();
                    }
                }
                //****************For General Header Report  ********************
                Cursor.Current = Cursors.WaitCursor;
                DataSet detailsDS = new DataSet();
                DataSet meterIDDS = new DataSet();

                detailsDS = ListConsumerMeterDetails(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                reportRow = reportXSD.Tables["BillingDetailsTable"].NewRow();
                if (detailsDS != null && detailsDS.Tables[0].Rows.Count > 0)
                {
                    //This function calling is required to add NoPower Supply and No Load Data column in the dataset
                    detailsDS = ListNoPowerSupplyLoadTime(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), detailsDS);
                    FillConsumerMeterDetails(detailsDS);
                }
                else
                {
                    meterIDDS = GetMeterIDFromMeterDataID(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    if (meterIDDS != null && meterIDDS.Tables[0].Rows.Count > 0)
                    {
                        //This function calling is required to add NoPower Supply and No Load Data column in the dataset
                        meterIDDS = ListNoPowerSupplyLoadTime(Convert.ToInt64(ConfigInfo.ActiveMeterDataId), meterIDDS);
                        FillMeterID(meterIDDS);
                    }
                }
                //**************** General Detail Volt & Current Rating ********************
                DataSet generalDS = new DataSet();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    generalDS = new DLMS650GeneralBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    string Voltrating = generalDS.Tables[0].Rows[10]["Value"].ToString();
                    string Currentrating = generalDS.Tables[0].Rows[11]["Value"].ToString();
                    reportRow["VoltageRating"] = Voltrating;
                    reportRow["CurrentRating"] = Currentrating;
                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    generalDS = ListGeneralData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    string Voltrating = generalDS.Tables[0].Rows[10]["Value"].ToString();
                    string Currentrating = generalDS.Tables[0].Rows[11]["Value"].ToString();
                    reportRow["VoltageRating"] = Voltrating;
                    reportRow["CurrentRating"] = Currentrating;
                }
                if (generalDS != null && generalDS.Tables[0].Rows.Count > 0)
                {
                    
                }
                else
                {
                    errCount++;
                    if (errMsg == "")
                    { errMsg = "General data not available."; }
                    else
                    { errMsg = errMsg + "\n" + "General data not available."; }
                    
                }
                showReport++;
                //**************** Instant data for billing count ********************
                DataSet instantDS = new DataSet();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    instantDS = new DLMS650InstantaneousBLL().GetMeterData(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                    if (instantDS != null && instantDS.Tables != null && instantDS.Tables.Count > 0 && instantDS.Tables[0].Rows.Count > 0 )
                    {
                        string metertime = instantDS.Tables[0].Rows[0]["Value"].ToString();
                        reportRow["MeterTime"] = metertime.Substring(11);
                    }

                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    instantDS = ListInstantData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                    string metertime = instantDS.Tables[0].Rows[0]["Value"].ToString();
                    reportRow["MeterTime"] = metertime.Substring(11);

                 }
                try
                {
                    DataRow[] Voltage = instantDS.Tables[0].Select("[OBIS Code] = '1.0.12.7.0.255'");
                    DataRow[] Current = instantDS.Tables[0].Select("[OBIS Code] = '1.0.11.7.0.255'");
                    DataRow[] NeutralCurrent = instantDS.Tables[0].Select("[OBIS Code] = '1.0.91.7.0.255'");
                    DataRow[] InstkW = instantDS.Tables[0].Select("[OBIS Code] = '1.0.1.6.0.255'");
                    DataRow[] PF = instantDS.Tables[0].Select("[OBIS Code] = '1.0.13.7.0.255'");
                    
                    //This check is implement for Instant Billing Report. This report is visible only when all the value of above parameter OBIS code comes from meter.
                    if (Voltage.Length > 0 && Current.Length > 0 && NeutralCurrent.Length > 0 && InstkW.Length > 0 && PF.Length > 0)
                    {
                        ////********** New Instant report Add single phase  ***************
                        DataRow Instdr = InstantTNEB.NewRow();
                        if (Voltage != null && Voltage.Length > 0 && Voltage != null && Voltage.Length > 0)
                            Instdr[0] = Voltage[0].ItemArray[4].ToString().Trim();
                        if (Current != null && Current.Length > 0 && Current != null && Current.Length > 0)
                            Instdr[1] = Current[0].ItemArray[4].ToString().Trim();
                        if (NeutralCurrent != null && NeutralCurrent.Length > 0 && NeutralCurrent != null && NeutralCurrent.Length > 0)
                            Instdr[2] = NeutralCurrent[0].ItemArray[4].ToString().Trim();
                        if (InstkW != null && InstkW.Length > 0 && InstkW != null && InstkW.Length > 0)
                            Instdr[3] = InstkW[0].ItemArray[4].ToString().Trim();
                        if (PF != null && PF.Length > 0 && PF != null && PF.Length > 0)
                            Instdr[4] = PF[0].ItemArray[4].ToString().Trim();

                        InstantTNEB.Rows.Add((Instdr));
                        InstantNewDS.Tables.Add(InstantTNEB);
                        FillNewInstant_TNEB(InstantNewDS);
                    }
                }

                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                }        
                                
                if (instantDS != null && instantDS.Tables[0].Rows.Count > 0)
                {
                    
                    foreach (DataRow row in instantDS.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Descriptions"].ToString()))
                            val = row["Descriptions"].ToString();
                        if (val == "Cumulative Billing Count")
                            resetcount = row["Value"].ToString() ;
                   }          
                }
                else
                {
                    errCount++;
                    if (errMsg == "") errMsg = "Instantaneous data not available.";
                    else errMsg = errMsg + "\n" + "Instantaneous data not available.";
                }
                showReport++;
                //**************** DIP ********************
                try
                {
               
                dip = ListDIPData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                if (dip > 0)
                {
                    if (dip == 0x00000384 || dip == 0x00001384)
                    {
                        reportRow["DIP"] = "15 Min";
                    }
                    else if (dip == 0x00000708 || dip == 0x00001708 || dip == 0x00002708)
                    {
                        reportRow["DIP"] = "30 Min";
                    }
                }
                showReport++;
                }

                catch (Exception ex)    //Exception log for catch block
                {
                    errCount++;
                    if (errMsg == "")
                    { errMsg = "DIP not available."; }
                    else
                    { errMsg = errMsg + "\n" + "DIP not available."; }
                    logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                }
               
                //**************** Auto Reset Date ********************
                try
                {
                billingTypeEntity = ListBillingTypeData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                int MeterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
               if (MeterModelNumber == NamePlateConstants.RubyE150Value || MeterModelNumber == NamePlateConstants.SFSP)
               {
                   if (billingTypeEntity.BillingType == "02")
                   {
                       reportRow["AutoResetDate"] = "Monthly";// rdbMonthly.Checked = true;
                   }
                   else if (billingTypeEntity.BillingType == "01")
                   {
                       reportRow["AutoResetDate"] = "Odd Month";
                   }
                   else if (billingTypeEntity.BillingType == "00")
                   {
                       reportRow["AutoResetDate"] = "Even Month";
                   }
               }
               else
               {

                   if (billingTypeEntity.BillingType == "00")
                   {
                       reportRow["AutoResetDate"] = "Monthly";
                   }
                   else if (billingTypeEntity.BillingType == "01")
                   {
                       reportRow["AutoResetDate"] = "Odd Month";
                   }
                   else if (billingTypeEntity.BillingType == "02")
                   {
                       reportRow["AutoResetDate"] = "Even Month";
                   }
               }
              //  reportRow["AutoResetDate"] = billingTypeEntity.ModeOfBilling;
                reportXSD.Tables["BillingDetailsTable"].Rows.Add(reportRow);// odd /even or monthly 
                }

                catch (Exception ex)    //Exception log for catch block
                 {
                     ex.ToString();
                     logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                 }
                //**************** Power Factor ********************
                               
                DataSet powerFactorDS = new DataSet();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    powerFactorDS = new DLMS650BillingBLL().GetAveragePowerFactor(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    powerFactorDS = ListPowerFactorData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                }
                if (powerFactorDS != null && powerFactorDS.Tables[0].Rows.Count > 0)
                {
                   
                }
                else
                {
                    errCount++;
                    if (errMsg == "")
                    { errMsg = "Power Factor not available."; }
                    else
                    { errMsg = errMsg + "\n" + "Power Factor not available."; }

                }
                showReport++;
                  //**************** Main Energy ********************
                             
                DataSet mainEnergyDS = new DataSet();
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    // if (meterModelNumber == 9)
                    //{
                    //    mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false, meterModelNumber);
                    //}
                    //else
                    //{
                        mainEnergyDS = new DLMS650BillingBLL().GetCumulativeEnergy(Convert.ToInt32(ConfigInfo.ActiveMeterDataId), false);
                    //}

                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    mainEnergyDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                }
                if (mainEnergyDS != null && mainEnergyDS.Tables[0].Rows.Count > 0)
                {
                  
                }
                else
                {
                    errCount++;
                    if (errMsg == "") errMsg = "Main Energy data not available.";
                    else errMsg = errMsg + "\n" + "Main Energy data not available.";
                }
                showReport++;

                //**************** Demand ********************

               DataSet maximumDemandDS = new DataSet();

                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    maximumDemandDS = new DLMS650BillingBLL().GetMaximumDemand(Convert.ToInt32(ConfigInfo.ActiveMeterDataId));
                }
                else if (types.Equals(ApplicationType.IEC_LTCT_650))
                {
                    maximumDemandDS = ListMainEnergyData(Convert.ToInt64(ConfigInfo.ActiveMeterDataId));
                }
                if (maximumDemandDS != null && maximumDemandDS.Tables[0].Rows.Count > 0)
                {
                  
                }
                else
                {
                    errCount++;
                    if (errMsg == "")
                        errMsg = "Demand data not available.";
                    else errMsg = errMsg + "\n" + "Demand data not available.";
                }
                showReport++;
                try
                {
                    if (powerFactorDS != null)
                    {
                        if (powerFactorDS.Tables[0].Rows.Count > 0)
                        {
                            for (int colCount = 0; colCount < powerFactorDS.Tables[0].Rows.Count; colCount++)
                            {
                                DataRow dr = BillingTNEB.NewRow();
                                if (Convert.ToInt32(resetcount) > 0)
                                {
                                    if (colCount == 0)
                                    {
                                        dr[0] = "--";
                                    }
                                    else
                                    {
                                        dr[0] =Convert.ToInt32(resetcount) - colCount+1; // Reset No
                                    }
                                }
                                else
                                    dr[0] = Convert.ToInt32(resetcount); // Reset No
                                dr[1] = powerFactorDS.Tables[0].Rows[colCount][2].ToString();// Avg. PF
                                if (mainEnergyDS.Tables[0].Columns.Contains(GlobalConstants.conCumulativeEnergyBILLINGTYPE))
                                {
                                    if (mainEnergyDS.Tables[0].Rows[colCount][GlobalConstants.conCumulativeEnergyBILLINGTYPE].ToString() == "255" || mainEnergyDS.Tables[0].Rows[colCount][GlobalConstants.conCumulativeEnergyBILLINGTYPE].ToString() == "")
                                        dr[2] = "---";
                                    else
                                        //dr[2] = mainEnergyDS.Tables[0].Rows[colCount][6].ToString();// Reset Mode
                                        dr[2] = mainEnergyDS.Tables[0].Rows[colCount][GlobalConstants.conCumulativeEnergyBILLINGTYPE].ToString();// Reset Mode
                                }
                                //dr[3] = mainEnergyDS.Tables[0].Rows[colCount][1].ToString();// Reset Date & Time
                                //dr[4] = mainEnergyDS.Tables[0].Rows[colCount][2].ToString();// Cum kwh
                                //dr[5] = maximumDemandDS.Tables[0].Rows[colCount][2].ToString();// Kw MD OCC
                                //dr[6] = maximumDemandDS.Tables[0].Rows[colCount][3].ToString();//Date & Time

                                dr[3] = mainEnergyDS.Tables[0].Rows[colCount]["Billing DateTime (0.0.0.1.2.255;3;2)"].ToString();// Reset Date & Time
                                dr[4] = mainEnergyDS.Tables[0].Rows[colCount][CommonMethods.getDisplayHeaderText(GlobalConstants.conCumulativeEnergyColWH)].ToString();// Cum kwh
                                dr[5] = maximumDemandDS.Tables[0].Rows[colCount][CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKW)].ToString();// Kw MD OCC
                                dr[6] = maximumDemandDS.Tables[0].Rows[colCount][CommonMethods.getDisplayHeaderText(GlobalConstants.conMaximumDemandKWTIMESTAMP)].ToString();//Date & Time
                                BillingTNEB.Rows.Add(dr);
                            }
                            BillingDS.Tables.Add(BillingTNEB);
                            FillBilling_TNEB(BillingDS);

                        }
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    ex.ToString();
                    logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
                }
                showReport++;

                if (errCount == 0 && showReport > 0)
                    ShowReport();
                else
                {
                    if (string.IsNullOrEmpty(errMsg))
                    { 
                        MessageBox.Show("No data available/Meter not Supported.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    }
                    else
                    {
                        MessageBox.Show("No data available/Meter not Supported.","BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //MessageBox.Show(errMsg, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //if (showReport > 0)
                    //    ShowReport();
                }

            }

            catch (Exception ex)    //Exception log for catch block
            {
                ex.ToString();
                logger.Log(LOGLEVELS.Error, "btnShow_Click(object sender, EventArgs e)", ex);
            }
        }
                
             
    }
}
