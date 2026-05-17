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
using System.IO;
using System.Xml;
using CAB.IECFramework.Utility;

namespace CAB.UI
{
    public partial class RestoreFile : MdiChildForm
    {
        public RestoreFile()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            Application.DoEvents();
            this.Close();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "Bak";
            openFile.Filter = "Backup files (*.Bak)|*.Bak";
            if (textBox1.SelectedText.Trim().IndexOf(".Bak") > -1)
                openFile.FilterIndex = 0;
            else
                openFile.FilterIndex = 1;
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
                textBox1.Text = openFile.FileName;
        }

        private void btn_restore_Click(object sender, EventArgs e)
        {
			this.StatusMessage = "";
			Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;
            FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
            int buttonValue = 0;

			if (textBox1.Text.Trim() == "")
			{
				//this.StatusMessage = "Please select the backup file";
				MessageBox.Show("Please select the backup file", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Cursor = Cursors.Default;
                return;
			}
            else if (!File.Exists(textBox1.Text))
            {
                //this.StatusMessage = "File is not selected";
				MessageBox.Show("File is not selected", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.DoEvents();
                this.Cursor = Cursors.Default;
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                try
                {
                    ds.ReadXml(textBox1.Text);
                }
                catch (Exception)
                {
                    this.StatusMessage = "File does not contain any data";
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (ds == null)
                {
                    this.StatusMessage = "File does not contain any data";
					//MessageBox.Show("File does not contain any data","BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (ds.Tables.Count==0)
                {
                    this.StatusMessage = "File does not contain any data";
					//MessageBox.Show("File does not contain any data", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                bool Flag = false;
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
                RestoreDialogBox restoreBox = new RestoreDialogBox();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string fileName = dr["FileName"].ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(fileName) as FileUploadMasterEntity;
                    if (fileUploadMasterEntity == null)
                        continue;
                    bool flag = false;
                    if (!string.IsNullOrEmpty( fileUploadMasterEntity.FileName))
                    {  
                        if (buttonValue == 0 || buttonValue == 1 || buttonValue == 3)
                        {
                            string message = "The database already contains a file named " + fileName + ".\r\n\n Would you like to replace the existing file?\r\n";
                            RestoreDialogBoxResult result = restoreBox.ShowMemoryDialog(message, "Confirm File Replace");
                            Application.DoEvents();
                            if (result == RestoreDialogBoxResult.Yes){ buttonValue = 1;flag=true;}
                            else if (result == RestoreDialogBoxResult.YesToAll) {buttonValue = 2;flag=true;}
                            else if (result == RestoreDialogBoxResult.No) { buttonValue = 3; continue; }
                            else if (result == RestoreDialogBoxResult.NoToAll) buttonValue = 4;
                            else if (result == RestoreDialogBoxResult.Cancel) { this.Cursor = Cursors.Default; return; }
                        }
                        else if (buttonValue == 4) continue;
                    }
                    if (flag)
                    {
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
                    }
                    string fileContent = dr["FileContent"].ToString();
                    UploadFile uploadFile = new UploadFile("BCS");
                    if(uploadFile.Upload(fileName, ConfigInfo.DecryptFile(fileContent), false))
                    Flag = true; 
                }               
                if (Flag)
                {
                    this.ListRefresh = true;   
					this.StatusMessage = "File Restored successfully.";
                    Application.DoEvents();
                }
                else
                {
                    if (buttonValue != 3 && buttonValue != 4)
                    {
                        this.StatusMessage = "File does not contain any data";
                        Application.DoEvents();
                    }
                    this.ListRefresh = true;
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void RestoreFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void RestoreFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = "";
        }
    }
}
