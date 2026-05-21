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
using CAB.Framework.Utility;
using CAB.Framework;
using CAB.MeterData.Upload;
using CABFramework;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class RestoreFile : MdiChildForm
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RestoreFile).ToString());
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
                StreamReader streamReader = new StreamReader(textBox1.Text);
                string content = streamReader.ReadToEnd();
                if (content == "")
                {
                    this.StatusMessage = "File does not contain any data";
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                try
                {
                    ds.ReadXml(textBox1.Text);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    this.StatusMessage = "File corrupted";
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    logger.Log(LOGLEVELS.Error, "btn_restore_Click(object sender, EventArgs e)", ex);
                    return;
                }
                if (ds == null)
                {
                    this.StatusMessage = "File does not contain any data";
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (ds.Tables.Count == 0)
                {
                    this.StatusMessage = "File does not contain any data";
                    //MessageBox.Show("File does not contain any data", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;
                    return;
                }

                UploadFile uploadFile = new UploadFile();
                ApplicationType types = ConfigInfo.GetApplicationType();
                MeterDataBLL meterDataBLL = new MeterDataBLL();
                RestoreDialogBox restoreBox = new RestoreDialogBox();
                bool Flag = false;
                //if (types.Equals(ApplicationType.IEC_LTCT_650))
                //{
                //    uploadFile = new IEC650UploadFile();
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
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        string fileName = dr["FileName"].ToString();
                //        FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(fileName) as FileUploadMasterEntity;
                //        if (fileUploadMasterEntity == null)
                //            continue;
                //        bool flag = false;
                //        if (!string.IsNullOrEmpty(fileUploadMasterEntity.FileName))
                //        {
                //            if (buttonValue == 0 || buttonValue == 1 || buttonValue == 3)
                //            {
                //                string message = "The database already contains a file named " + fileName + ".\r\n\n Would you like to replace the existing file?\r\n";
                //                RestoreDialogBoxResult result = restoreBox.ShowMemoryDialog(message, "Confirm File Replace");
                //                Application.DoEvents();
                //                if (result == RestoreDialogBoxResult.Yes) { buttonValue = 1; flag = true; }
                //                else if (result == RestoreDialogBoxResult.YesToAll) { buttonValue = 2; flag = true; }
                //                else if (result == RestoreDialogBoxResult.No) { buttonValue = 3; continue; }
                //                else if (result == RestoreDialogBoxResult.NoToAll) buttonValue = 4;
                //                else if (result == RestoreDialogBoxResult.Cancel) { this.Cursor = Cursors.Default; return; }
                //            }
                //            else if (buttonValue == 4) continue;
                //        }
                //        if (flag)
                //        {
                //            long meterDataId = meterDataBLL.GetMeterDataID(fileUploadMasterEntity.FileUpload_ID);
                //            fileUploadMasterBLL.DeleteData(fileUploadMasterEntity);
                //            meterDataBLL.DeleteData(meterDataId);
                //            instantPowerBLL.DeleteData(meterDataId);
                //            generalBLL.DeleteData(meterDataId);
                //            billingBLL.DeleteData(meterDataId);
                //            tariffBLL.DeleteData(meterDataId);
                //            tamperGeneralBLL.DeleteData(meterDataId);
                //            fraudEnergyBLL.DeleteData(meterDataId);
                //            programmingBLL.DeleteData(meterDataId);
                //            rTCUpdateBLL.DeleteData(meterDataId);
                //            phasorBLL.DeleteData(meterDataId);
                //            loadSurveyBLL.DeleteData(meterDataId);
                //            loadSurveyParameterBLL.DeleteData(meterDataId);
                //            tamperSnapShotBLL.DeleteData(meterDataId);
                //            tamperCounterBLL.DeleteData(meterDataId);
                //            dTMLoadSurveyBLL.DeleteData(meterDataId);
                //            dTMDailyProfileBLL.DeleteData(meterDataId);
                //            dTMDailyProfileParameterBLL.DeleteData(meterDataId);
                //        }
                //        string fileContent = dr["FileContent"].ToString();
                //        if (uploadFile.Upload(fileName, ConfigInfo.DecryptFile(fileContent), false))
                //            Flag = true;
                //    }
                //}
                //else 
                if (types.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    // uploadFile = new DLMS650UploadFile();
                    DLMS650BillingBLL dlms650BillingBLL = new DLMS650BillingBLL();
                    DLMS650GeneralBLL dlms650GeneralBLL = new DLMS650GeneralBLL();
                    DLMS650InstantaneousBLL dlms650InstantaneousBLL = new DLMS650InstantaneousBLL();
                    DLMS650LoadSurveyBLL dlms650LoadSurveyBLL = new DLMS650LoadSurveyBLL();
                    DLMS650TamperMasterBLL dlms650TamperMasterBLL = new DLMS650TamperMasterBLL();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string fileName = dr["FileName"].ToString();
                        FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(fileName) as FileUploadMasterEntity;
                        if (fileUploadMasterEntity == null)
                            continue;
                        bool flag = false;
                        if (!string.IsNullOrEmpty(fileUploadMasterEntity.FileName))
                        {
                            if (buttonValue == 0 || buttonValue == 1 || buttonValue == 3)
                            {
                                string message = "The database already contains a file named " + fileName + ".\r\n\n Would you like to replace the existing file?\r\n";
                                RestoreDialogBoxResult result = restoreBox.ShowMemoryDialog(message, "Confirm File Replace");
                                Application.DoEvents();
                                if (result == RestoreDialogBoxResult.Yes) { buttonValue = 1; flag = true; }
                                else if (result == RestoreDialogBoxResult.YesToAll) { buttonValue = 2; flag = true; }
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
                            dlms650BillingBLL.DeleteData(meterDataId);
                            dlms650GeneralBLL.DeleteData(meterDataId);
                            dlms650InstantaneousBLL.DeleteData(meterDataId);
                            dlms650LoadSurveyBLL.DeleteData(meterDataId);
                            dlms650TamperMasterBLL.DeleteData(meterDataId);
                        }
                        string fileContent = dr["FileContent"].ToString();
                        string[] data = fileContent.Split('~');
                        string resultMessaeg = string.Empty;
                        ConfigSettings.ChangeNode("SourceOfFile", ((int)CommTypes.Restore).ToString());
                        foreach (string fileData in data)
                        {
                            if (string.IsNullOrEmpty(fileData))
                                continue;
                            if (fileData.Contains("$"))
                            {
                                if (uploadFile.Upload2NGFile(fileName, fileData, false, out resultMessaeg,null))
                                    Flag = true;
                            }
                            else if (ConfigInfo.DecryptFile(fileData).ToUpper().Contains("LGC")) // Story - 0427028 - single phase case was not handled during restoration
                            {
                                if (uploadFile.UploadSLGFile(fileName, ConfigInfo.DecryptFile(fileData), false, out resultMessaeg,null))
                                    Flag = true;
                            }
                            else
                            {
                                if (uploadFile.UploadCABFile(fileName, ConfigInfo.DecryptFile(fileData), false, out resultMessaeg, null))
                                    Flag = true;
                            }
                        }
                    }
                   
                    if (Flag)
                    {

                        this.StatusMessage = "File Restored successfully.";
                        Application.DoEvents();
                    }
                    else
                    {
                        if (buttonValue != 3 && buttonValue != 4)
                        {
                            this.StatusMessage = "File corrupted";
                            Application.DoEvents();
                        }
                    }

                    this.Cursor = Cursors.Default;
                    this.ListRefresh = true;
                }
            }
        }

        private void RestoreFile_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void RestoreFile_FormClosing(object sender, FormClosingEventArgs e)
        {                   
            this.Cursor = Cursors.Default;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
    }
}
