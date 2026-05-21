using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Entity;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.DALC.Data.DataServices;
using LTCTBLL;
using CABApplication;
using CABEntity;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
	public partial class DeleteFile : MdiChildForm
	{
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DeleteFile).ToString());
		FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
		MeterDataBLL meterDataBLL = new MeterDataBLL();
        TabNameBLL tabNameBll = new TabNameBLL();
		public DeleteFile()
		{
			InitializeComponent();
		}

		private void DeleteFile_Load(object sender, EventArgs e)
		{
			this.Text = "Delete Data";
			DataSet dataSet = fileUploadMasterBLL.ComboList();
			chkFileUploadList.Items.Clear();
			if (dataSet != null)
			{
				foreach (DataRow Drow in dataSet.Tables[0].Rows)
				{
					chkFileUploadList.Items.Add(Drow["FileName"].ToString());
				}
			}
		}
		private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
		{
            this.StatusMessage = string.Empty;
			CheckedAll(chkSelectAll.Checked);
		}

		private void CheckedAll(bool status)
		{
			for (int i = 0; i < chkFileUploadList.Items.Count; i++)
			{
				chkFileUploadList.SetItemChecked(i, status);
			}
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.StatusMessage = "";
			Application.DoEvents();
			this.Close();
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
            this.StatusMessage = string.Empty;
            try
            {
                if (chkFileUploadList.CheckedItems.Count <= 0)
                {
                    this.StatusMessage = "File not selected.";
                    Application.DoEvents();
                    return;
                }
                else if (chkFileUploadList.Items.Count <= 0)
                {
                    this.StatusMessage = "File not exist";
                    Application.DoEvents();
                }
                else if (MessageBox.Show("Are you sure to delete this file?", "Delete Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;
                else
                {
                    ApplicationType types = ConfigInfo.GetApplicationType(); 
                    if (chkFileUploadList.CheckedItems.Count > 0)
                    {
                        //if (types.Equals(ApplicationType.IEC_LTCT_650))
                        //{
                        //    MeterDataBLL meterDataBLL = new MeterDataBLL();
                        //    InstantPowerBLL instantPowerBLL = new InstantPowerBLL();
                        //    GeneralBLL generalBLL = new GeneralBLL();
                        //    BillingBLL billingBLL = new BillingBLL();
                        //    TariffBLL tariffBLL = new TariffBLL();
                        //    TamperGeneralBLL tamperGeneralBLL = new TamperGeneralBLL();
                        //    FraudEnergyBLL fraudEnergyBLL = new FraudEnergyBLL();
                        //    ProgrammingBLL programmingBLL = new ProgrammingBLL();
                        //    RTCUpdateBLL rTCUpdateBLL = new RTCUpdateBLL();
                        //    PhasorBLL phasorBLL = new PhasorBLL();
                        //    LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
                        //    LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                        //    TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
                        //    TamperCounterBLL tamperCounterBLL = new TamperCounterBLL();
                        //    DTMLoadSurveyBLL dTMLoadSurveyBLL = new DTMLoadSurveyBLL();
                        //    DTMDailyProfileBLL dTMDailyProfileBLL = new DTMDailyProfileBLL();
                        //    DTMDailyProfileParameterBLL dTMDailyProfileParameterBLL = new DTMDailyProfileParameterBLL();
                        //    foreach (object item in chkFileUploadList.CheckedItems)
                        //    {
                        //        FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(item.ToString()) as FileUploadMasterEntity;
                        //        long meterDataId = meterDataBLL.GetMeterDataID(fileUploadMasterEntity.FileUpload_ID);
                        //        fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                        //        meterDataBLL.DeleteData(meterDataId);
                        //        instantPowerBLL.DeleteData(meterDataId);
                        //        generalBLL.DeleteData(meterDataId);
                        //        billingBLL.DeleteData(meterDataId);
                        //        tariffBLL.DeleteData(meterDataId);
                        //        tamperGeneralBLL.DeleteData(meterDataId);
                        //        fraudEnergyBLL.DeleteData(meterDataId);
                        //        programmingBLL.DeleteData(meterDataId);
                        //        rTCUpdateBLL.DeleteData(meterDataId);
                        //        phasorBLL.DeleteData(meterDataId);
                        //        loadSurveyBLL.DeleteData(meterDataId);
                        //        loadSurveyParameterBLL.DeleteData(meterDataId);
                        //        tamperSnapShotBLL.DeleteData(meterDataId);
                        //        tamperCounterBLL.DeleteData(meterDataId);
                        //        dTMLoadSurveyBLL.DeleteData(meterDataId);
                        //        dTMDailyProfileBLL.DeleteData(meterDataId);
                        //        dTMDailyProfileParameterBLL.DeleteData(meterDataId);
                        //        meterDataBLL.DeleteDataBasedOnFileID(fileUploadMasterEntity.FileUpload_ID);

                        //    }
                        //}
                        //else 
                        if (types.Equals(ApplicationType.DLMS_LTCT_650))
                        {
                            DLMS650BillingBLL dlms650BillingBLL = new DLMS650BillingBLL();
                            FraudEnergyBLL dlms650fraudEnergyBLL = new FraudEnergyBLL();
                            DLMS650GeneralBLL dlms650GeneralBLL = new DLMS650GeneralBLL();
                            DLMS650NamePlateBLL dlms650NamePlateBLL = new DLMS650NamePlateBLL();
                            DLMS650InstantaneousBLL dlms650InstantaneousBLL = new DLMS650InstantaneousBLL();
                            DLMS650LoadSurveyBLL dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                            DLMS650TamperMasterBLL dlms650TamperMasterBLL = new DLMS650TamperMasterBLL();
                            DLMS650PhasorBLL dlms650PhasorBLL = new DLMS650PhasorBLL();
                            LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                            MidnightParameterBLL midnightParameterBLL = new MidnightParameterBLL();
                            DLMS650MidnightDataBLL dlms650MidnightBLL = new DLMS650MidnightDataBLL();
                            TamperParameterBLL tamperParameterBLL = new TamperParameterBLL();
                            MDWithIPBLL mdWithIPBLL = new MDWithIPBLL();
                            kvarSelectionBLL kvarSelectBLL = new kvarSelectionBLL();
                            RS232BLL rs232BLL = new RS232BLL();
                            BillingTypeBLL billingTypeBLL = new BillingTypeBLL();
                            AutoLockBLL autoLockBLL = new AutoLockBLL();
                            RTCBLL rtcBLL = new RTCBLL();
                            DailyLogBLL dailyLogBLL = new DailyLogBLL();
                            TodBLL todBLL = new TodBLL();
                            DisplayParameterBLL displayParameterBLL = new DisplayParameterBLL();
                            LSIPBLL lsipBLL = new LSIPBLL();
                            DIPBLL dipBLL = new DIPBLL();
                            AnomalyBLL anomalyBLL = new AnomalyBLL();
                            ManualBillingBLL manualBillingBLL = new ManualBillingBLL();
                            SoftwareBillingBLL softwareBillingBLL = new SoftwareBillingBLL();
                            PulseEnergyBLL pulseEnergyBLL = new PulseEnergyBLL();
                            long meterDataId = 0;
                            foreach (object item in chkFileUploadList.CheckedItems)
                            {
                                FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(item.ToString()) as FileUploadMasterEntity;
                                DataSet meterDataIds = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                                if (meterDataIds.Tables[0].Rows.Count != 0 || meterDataIds.Tables[0].Rows == null)
                                {
                                    foreach (DataRow meterDaatIdRow in meterDataIds.Tables[0].Rows)
                                    {
                                        meterDataId = Convert.ToInt64(meterDaatIdRow["meterData_id"]);
                                        fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                                        meterDataBLL.DeleteData(meterDataId);
                                        dlms650BillingBLL.DeleteData(meterDataId);
                                        dlms650GeneralBLL.DeleteData(meterDataId);
                                        dlms650NamePlateBLL.DeleteData(meterDataId);
                                        dlms650InstantaneousBLL.DeleteData(meterDataId);
                                        dlms650LoadSurveyBLL.DeleteData(meterDataId);
                                        loadSurveyParameterBLL.DeleteData(meterDataId);
                                        dlms650TamperMasterBLL.DeleteData(meterDataId);
                                        tabNameBll.DeleteTabnameData(meterDataId);
                                        midnightParameterBLL.DeleteData(meterDataId);
                                        dlms650MidnightBLL.DeleteData(meterDataId);
                                        dlms650PhasorBLL.DeleteData(meterDataId);
                                        dlms650fraudEnergyBLL.DeleteData(meterDataId);
                                        tamperParameterBLL.DeleteData(meterDataId);
                                        mdWithIPBLL.DeleteData(meterDataId);
                                        kvarSelectBLL.DeleteData(meterDataId);
                                        rs232BLL.DeleteData(meterDataId);
                                        billingTypeBLL.DeleteData(meterDataId);
                                        autoLockBLL.DeleteData(meterDataId);
                                        rtcBLL.DeleteData(meterDataId);
                                        dailyLogBLL.DeleteData(meterDataId);
                                        todBLL.DeleteData(meterDataId);
                                        displayParameterBLL.DeleteData(meterDataId);
                                        lsipBLL.DeleteData(meterDataId);
                                        dipBLL.DeleteData(meterDataId);
                                        anomalyBLL.DeleteData(meterDataId);
                                        manualBillingBLL.DeleteData(meterDataId);
                                        softwareBillingBLL.DeleteData(meterDataId);
                                        pulseEnergyBLL.DeleteData(meterDataId);
                                    }
                                }
                                else
                                {
                                    fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                                }
                            }
                        }
                       
                        this.StatusMessage = "Data deleted successfully.";
                        Application.DoEvents();
                        DeleteFile_Load(this, null);
                        this.ListRefresh = true;
                    }
                    else
                    {
                        this.StatusMessage = "No file selected.";
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "Data deleted successfully.";
                Application.DoEvents();
                logger.Log(LOGLEVELS.Error, "btnDelete_Click(object sender, EventArgs e)", ex);
            }
		}

		private void DeleteFile_Activated(object sender, EventArgs e)
		{
			this.StatusMessage = string.Empty;
			Application.DoEvents();
		}

        private void chkFileUploadList_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
        }

        private void DeleteFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
	}
}
