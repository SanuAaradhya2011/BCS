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
using LTCTBLL;

namespace CAB.UI
{
	public partial class DeleteFile : MdiChildForm
	{
		FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
		MeterDataBLL meterDataBLL = new MeterDataBLL();
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
            this.StatusMessage = "";
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
            try
            {
                this.StatusMessage = "";
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
                    if (chkFileUploadList.CheckedItems.Count > 0)
                    {
                        MeterDataBLL meterDataBLL = new MeterDataBLL();
                        InstantPowerBLL instantPowerBLL = new InstantPowerBLL();
                        GeneralBLL generalBLL = new GeneralBLL();
                        BillingBLL billingBLL = new BillingBLL();
                        TariffBLL tariffBLL = new TariffBLL();
                        TamperGeneralBLL tamperGeneralBLL = new TamperGeneralBLL();
                        FraudEnergyBLL fraudEnergyBLL = new FraudEnergyBLL();
                        ProgrammingBLL programmingBLL = new ProgrammingBLL();
                        RTCUpdateBLL rTCUpdateBLL = new RTCUpdateBLL();
                        PhasorBLL phasorBLL = new PhasorBLL();
                        LoadSurveyBLL loadSurveyBLL = new LoadSurveyBLL();
                        LoadSurveyParameterBLL loadSurveyParameterBLL = new LoadSurveyParameterBLL();
                        TamperSnapShotBLL tamperSnapShotBLL = new TamperSnapShotBLL();
                        TamperCounterBLL tamperCounterBLL = new TamperCounterBLL();
                        DTMLoadSurveyBLL dTMLoadSurveyBLL = new DTMLoadSurveyBLL();
                        DTMDailyProfileBLL dTMDailyProfileBLL = new DTMDailyProfileBLL();
                        DTMDailyProfileParameterBLL dTMDailyProfileParameterBLL = new DTMDailyProfileParameterBLL();
                        MeterDataHeaderInfoBLL meterDataHeaderInfoBLL = new MeterDataHeaderInfoBLL();
                        AutoLockBLL autoLockBLL = new AutoLockBLL();
                        
                        foreach (object item in chkFileUploadList.CheckedItems)
                        {
                            FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(item.ToString()) as FileUploadMasterEntity;
                            long meterDataId = meterDataBLL.GetMeterDataID(fileUploadMasterEntity.FileUpload_ID);
                            fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                            meterDataBLL.DeleteData(meterDataId);
                            instantPowerBLL.DeleteData(meterDataId);
                            generalBLL.DeleteData(meterDataId);
                            billingBLL.DeleteData(meterDataId);
                            tariffBLL.DeleteData(meterDataId);
                            tamperGeneralBLL.DeleteData(meterDataId);
                            fraudEnergyBLL.DeleteData(meterDataId);
                            programmingBLL.DeleteData(meterDataId);
                            rTCUpdateBLL.DeleteData(meterDataId);
                            phasorBLL.DeleteData(meterDataId);
                            loadSurveyBLL.DeleteData(meterDataId);
                            loadSurveyParameterBLL.DeleteData(meterDataId);
                            tamperSnapShotBLL.DeleteData(meterDataId);
                            tamperCounterBLL.DeleteData(meterDataId);
                            dTMLoadSurveyBLL.DeleteData(meterDataId);
                            dTMDailyProfileBLL.DeleteData(meterDataId);
                            dTMDailyProfileParameterBLL.DeleteData(meterDataId);
                            meterDataHeaderInfoBLL.DeleteData(Convert.ToString(meterDataId));
                            meterDataBLL.DeleteDataBasedOnFileID(fileUploadMasterEntity.FileUpload_ID);
                            autoLockBLL.DeleteData(meterDataId);
                        }
                        this.ListRefresh = true;
                        this.StatusMessage = "Data deleted successfully.";
                        Application.DoEvents();
                        DeleteFile_Load(this, null);
                    }
                    else
                    {
                        this.StatusMessage = "No file selected.";
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception)
            {
                this.StatusMessage = "Unable to delete the data.";
                Application.DoEvents();
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
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }
	}
}
