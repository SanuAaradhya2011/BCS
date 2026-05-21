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

namespace CAB.UI
{
    public partial class ASCIIExportFileWise : MdiChildForm
    {
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL();
        private ASCIIExportSettingsEntity entity = null;
        private FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
        public ASCIIExportFileWise()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            if (cboSettings.SelectedIndex == -1)
            {
                MessageBox.Show("Please select file Name", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSettings.Focus();
                return;
            }

            string fileId = ((System.Data.DataRowView)(cboSettings.Items[cboSettings.SelectedIndex])).Row.ItemArray[0].ToString();
            entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;

            string[] meterData_ID = new string[500];
            int counter = 0;
            bool flag = false;
            this.Cursor = Cursors.WaitCursor;
            if (rbtnFileWise.Checked)
            {
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val)|| val=="False" )
                        continue;
                    else
                    {
                        flag = true;
                        break;
                    }

                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val) || val == "False")
                        continue;
                    else
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                MessageBox.Show("Please Include a file for export", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Cursor = Cursors.Default;
                return;
            }

            string filename = "";
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text files (*.txt)|*.txt|*.csv|*.csv";
            savefile.RestoreDirectory = true;

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                filename = savefile.FileName;
                if (string.IsNullOrEmpty(filename))
                {
                    MessageBox.Show("Please select file", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            else
            {
                this.StatusMessage = "";
                Application.DoEvents();
                this.Cursor = Cursors.Default;
                return;
            }
            FileStream stream = new FileStream(filename, FileMode.Create);
            StreamWriter file = new StreamWriter(stream);
            if (rbtnFileWise.Checked)
            {
                #region Export File Wise
                counter = 0;
                //Billing
                bool IsHeader = true;
                Application.DoEvents();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.BillingColumn))
                            file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                        IsHeader = false;
                    }
                    string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                    DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                    if (dataSet == null)
                        continue;
                    if (dataSet.Tables.Count == 0)
                        continue;
                    string qry = "";
                    foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                    if (string.IsNullOrEmpty(qry))
                        continue;
                    if (string.IsNullOrEmpty(entity.BillingDBColumn))
                        continue;
                    this.StatusMessage = "Exporting Billing data.";
                    qry = qry.Substring(0, qry.Length - 1);
                    qry = string.Concat(entity.BillingDBColumn.Substring(0, entity.BillingDBColumn.Length - 1), " in(", qry, ")");
                    //entity.BillingDBColumn = entity.BillingDBColumn.Substring(0, entity.BillingDBColumn.Length - 1);
                    //qry = string.Concat(entity.BillingDBColumn, " in(", qry, ")");
                    DataSet BillingDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (BillingDataSet == null)
                        continue;
                    if (BillingDataSet.Tables.Count == 0)
                        continue;
                    for (int i = 0; i < BillingDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < BillingDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");

                    }
                }

                //General
                IsHeader = true;
                counter = 0;

                Application.DoEvents();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.GeneralColumn))
                            file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                        IsHeader = false;
                    }
                    string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                    DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                    if (dataSet == null)
                        continue;
                    if (dataSet.Tables.Count == 0)
                        continue;
                    string qry = "";
                    foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                    if (string.IsNullOrEmpty(qry))
                        continue;
                    if (string.IsNullOrEmpty(entity.GeneralDBColumn))
                        continue;
                    this.StatusMessage = "Exporting General data.";
                    qry = qry.Substring(0, qry.Length - 1);
                    qry = string.Concat(entity.GeneralDBColumn.Substring(0, entity.GeneralDBColumn.Length - 1), " in(", qry, ")");
                    //entity.GeneralDBColumn = entity.GeneralDBColumn.Substring(0, entity.GeneralDBColumn.Length - 1);
                    //qry = string.Concat(entity.GeneralDBColumn, " in(", qry, ")");
                    DataSet GeneralDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (GeneralDataSet == null)
                        continue;
                    if (GeneralDataSet.Tables.Count == 0)
                        continue;

                    for (int i = 0; i < GeneralDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < GeneralDataSet.Tables[0].Columns.Count; col++)
                        {
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        }
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Instant
                IsHeader = true;
                counter = 0;

                Application.DoEvents();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.InstantColumn))
                            file.WriteLine("MeterSerialNumber," + entity.InstantColumn);
                        IsHeader = false;
                    }
                    string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                    DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                    if (dataSet == null)
                        continue;
                    if (dataSet.Tables.Count == 0)
                        continue;
                    string qry = "";
                    foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                    if (string.IsNullOrEmpty(qry))
                        continue;
                    if (string.IsNullOrEmpty(entity.InstantDBColum))
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Instant data.";
                    qry = qry.Substring(0, qry.Length - 1);
                    //entity.InstantDBColum = entity.InstantDBColum.Substring(0, entity.InstantDBColum.Length - 1);
                    qry = string.Concat(entity.InstantDBColum.Substring(0, entity.InstantDBColum.Length - 1), " in(", qry, ")");
                    //qry = string.Concat(entity.InstantDBColum, " in(", qry, ")");
                    DataSet InstantDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (InstantDataSet == null)
                        continue;
                    if (InstantDataSet.Tables.Count == 0)
                        continue;
                    for (int i = 0; i < InstantDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < InstantDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Tamper
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.TamperColumn))
                            file.WriteLine("MeterSerialNumber," + entity.TamperColumn);
                        IsHeader = false;
                    }
                    string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                    DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                    if (dataSet == null)
                        continue;
                    if (dataSet.Tables.Count == 0)
                        continue;
                    string qry = "";
                    foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                    if (string.IsNullOrEmpty(qry))
                        continue;
                    if (string.IsNullOrEmpty(entity.TamberDBColumn))
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Tamper data.";
                    qry = qry.Substring(0, qry.Length - 1);
                    //entity.TamberDBColumn = entity.TamberDBColumn.Substring(0, entity.TamberDBColumn.Length - 1);
                    qry = string.Concat(entity.TamberDBColumn.Substring(0, entity.TamberDBColumn.Length - 1), " in(", qry, ")");
                    //qry = string.Concat(entity.TamberDBColumn, " in(", qry, ")");
                    DataSet TamperDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (TamperDataSet == null)
                        continue;
                    if (TamperDataSet.Tables.Count == 0)
                        continue;
                    for (int i = 0; i < TamperDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < TamperDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Loadsurvey
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.LoadSurveyColumn))
                            file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                        IsHeader = false;
                    }
                    string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                    FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                    DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                    if (dataSet == null)
                        continue;
                    if (dataSet.Tables.Count == 0)
                        continue;
                    string qry = "";
                    foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                    if (string.IsNullOrEmpty(qry))
                        continue;
                    if (string.IsNullOrEmpty(entity.LoadSurveyDBColumn))
                        continue;
                    this.StatusMessage = "Exporting Loadsurvey data.";
                    qry = qry.Substring(0, qry.Length - 1);
                    //entity.LoadSurveyDBColumn = entity.LoadSurveyDBColumn.Substring(0, entity.LoadSurveyDBColumn.Length - 1);
                    qry = string.Concat(entity.LoadSurveyDBColumn.Substring(0, entity.LoadSurveyDBColumn.Length - 1), " in(", qry, ")");
                    DataSet LoadSurveyDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (LoadSurveyDataSet == null)
                        continue;
                    if (LoadSurveyDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    for (int i = 0; i < LoadSurveyDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < LoadSurveyDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                #endregion
            }
            else
            {
                #region Export Meter ID Wise
                counter = 0;
                //Billing
                bool IsHeader = true;
                Application.DoEvents();
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.BillingColumn))
                            file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                        IsHeader = false;
                    }
                    if (string.IsNullOrEmpty(entity.BillingDBColumn))
                        continue;

                    string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                    string qry = string.Concat(entity.BillingDBColumn, meterDataID);
                    DataSet BillingDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (BillingDataSet == null)
                        continue;
                    if (BillingDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Billing data.";
                    for (int i = 0; i < BillingDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < BillingDataSet.Tables[0].Columns.Count; col++)
                            //data = string.Concat(data, Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        data = string.Concat(data, GetASCIIValue(Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //General
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.GeneralColumn))
                            file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                        IsHeader = false;
                    }
                    if (string.IsNullOrEmpty(entity.GeneralDBColumn))
                        continue;
                    string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                    string qry = string.Concat(entity.GeneralDBColumn, meterDataID);
                    DataSet GeneralDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (GeneralDataSet == null)
                        continue;
                    if (GeneralDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting General data.";
                    for (int i = 0; i < GeneralDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < GeneralDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Instant
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.InstantColumn))
                            file.WriteLine("MeterSerialNumber," + entity.InstantColumn);
                        IsHeader = false;
                    }

                    if (string.IsNullOrEmpty(entity.InstantDBColum))
                        continue;
                    string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                    string qry = string.Concat(entity.InstantDBColum, meterDataID);
                    DataSet InstantDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (InstantDataSet == null)
                        continue;
                    if (InstantDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Instant data.";
                    for (int i = 0; i < InstantDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < InstantDataSet.Tables[0].Columns.Count; col++)
                            //data = string.Concat(data, Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        data = string.Concat(data, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Tamper
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.TamperColumn))
                            file.WriteLine("MeterSerialNumber," + entity.TamperColumn);
                        IsHeader = false;
                    }

                    if (string.IsNullOrEmpty(entity.TamberDBColumn))
                        continue;
                    string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                    string qry = string.Concat(entity.TamberDBColumn, meterDataID);
                    DataSet TamperDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (TamperDataSet == null)
                        continue;
                    if (TamperDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Tamper data.";
                    for (int i = 0; i < TamperDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < TamperDataSet.Tables[0].Columns.Count; col++)
                            //data = string.Concat(data, Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        data = string.Concat(GetASCIIValue(Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter), data);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                //Loadsurvey
                IsHeader = true;
                counter = 0;
                Application.DoEvents();
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (IsHeader)
                    {
                        if (!string.IsNullOrEmpty(entity.LoadSurveyColumn))
                            file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                        IsHeader = false;
                    }

                    if (string.IsNullOrEmpty(entity.LoadSurveyDBColumn))
                        continue;
                    string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                    string qry = string.Concat(entity.LoadSurveyDBColumn, meterDataID);
                    DataSet LoadSurveyDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                    if (LoadSurveyDataSet == null)
                        continue;
                    if (LoadSurveyDataSet.Tables.Count == 0)
                        continue;
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    //file.WriteLine(" ");
                    this.StatusMessage = "Exporting Loadsurvey data.";
                    for (int i = 0; i < LoadSurveyDataSet.Tables[0].Rows.Count; i++)
                    {
                        string data = string.Empty;
                        for (int col = 0; col < LoadSurveyDataSet.Tables[0].Columns.Count; col++)
                            data = string.Concat(data, GetASCIIValue(Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                        //data = string.Concat(data, Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                        if (string.IsNullOrEmpty(data))
                            continue;
                        data = data.Substring(0, data.Length - 1);
                        file.WriteLine(data);
                        file.WriteLine(" ");
                    }
                }
                #endregion
            }
            file.Close();
            stream.Close();
            this.StatusMessage = "File Exported Successfully.";
            this.Cursor = Cursors.Default;
        }

        private string GetASCIIValue(string strTemp, string tempDataSeparator)
        {
            StringBuilder sbTemp = new StringBuilder();
            if (string.IsNullOrEmpty(strTemp))
            {
                sbTemp.Append(Convert.ToInt32('0').ToString());
                sbTemp.Append(tempDataSeparator);
                return sbTemp.ToString();
            }

            foreach (char ch in strTemp)
            {
                sbTemp.Append(Convert.ToInt32(ch).ToString());
            }
            sbTemp.Append(tempDataSeparator);
            return sbTemp.ToString();
        }

        private void ASCIIExportFileWise_Load(object sender, EventArgs e)
        {
            this.Text = "ASCII Export";
            this.StatusMessage = string.Empty;
            DataSet dataSet1 = asciiExportSettingsBLL.ListDataSet();
            cboSettings.DataSource = dataSet1.Tables[0];
            cboSettings.DisplayMember = "FileName";
            cboSettings.ValueMember = "Asciiexportsettings_ID";
            DataSet fileWiseDataSet = meterDataBLL.FileExportListDataSet();
            if (fileWiseDataSet == null)
                return;
            if (fileWiseDataSet.Tables.Count == 0)
                return;
            dvgFileWiseExport.DataSource = fileWiseDataSet.Tables[0].DefaultView;
            dvgFileWiseExport.Columns[0].Width = 50;
            dvgFileWiseExport.Columns[0].HeaderText = "S.No.";
            dvgFileWiseExport.Columns[0].ReadOnly = true;

            dvgFileWiseExport.Columns[1].Width = 250;
            dvgFileWiseExport.Columns[1].HeaderText = "File Name";
            dvgFileWiseExport.Columns[1].ReadOnly = true;

            dvgFileWiseExport.Columns[2].Width = 150;
            dvgFileWiseExport.Columns[2].HeaderText = "Reading DateTime";
            dvgFileWiseExport.Columns[2].ReadOnly = true;

            DataGridViewCheckBoxColumn objdvgFileWiseExport = new DataGridViewCheckBoxColumn();
            objdvgFileWiseExport.Name = "Include";
            dvgFileWiseExport.Columns.Add(objdvgFileWiseExport);
            dvgFileWiseExport.Columns[3].HeaderText = "Include";
            dvgFileWiseExport.Columns[3].Width = 80;
            dvgFileWiseExport.Columns[3].ReadOnly = false;
            foreach (DataGridViewColumn column in dvgFileWiseExport.Columns)
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;


            DataSet dataSet = meterDataBLL.ListExportDataSet();
            if (dataSet == null)
                return;
            if (dataSet.Tables.Count == 0)
                return;
            dgvMeterWiseExport.DataSource = dataSet.Tables[0].DefaultView;
            dgvMeterWiseExport.Columns[0].Width = 50;
            dgvMeterWiseExport.Columns[0].HeaderText = "S.No.";
            dgvMeterWiseExport.Columns[0].ReadOnly = true;

            dgvMeterWiseExport.Columns[1].Width = 10;
            dgvMeterWiseExport.Columns[1].HeaderText = "MeterData_ID";
            dgvMeterWiseExport.Columns[1].ReadOnly = true;

            dgvMeterWiseExport.Columns[2].Width = 120;
            dgvMeterWiseExport.Columns[2].HeaderText = "Meter Number";
            dgvMeterWiseExport.Columns[2].ReadOnly = true;

            dgvMeterWiseExport.Columns[3].Width = 150;
            dgvMeterWiseExport.Columns[3].HeaderText = "File Name";
            dgvMeterWiseExport.Columns[3].ReadOnly = true;

            dgvMeterWiseExport.Columns[4].Width = 150;
            dgvMeterWiseExport.Columns[4].HeaderText = "Reading DateTime";
            dgvMeterWiseExport.Columns[4].ReadOnly = true;

            DataGridViewCheckBoxColumn objCmb = new DataGridViewCheckBoxColumn();
            objCmb.Name = "Include";
            dgvMeterWiseExport.Columns.Add(objCmb);
            dgvMeterWiseExport.Columns[5].HeaderText = "Include";

            dgvMeterWiseExport.Columns[5].Width = 80;
            dgvMeterWiseExport.Columns[5].ReadOnly = false;

            foreach (DataGridViewColumn column in dgvMeterWiseExport.Columns)
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            LoadData();
        }

        private void LoadData()
        {
            if (rbtnFileWise.Checked)
            {
                dvgFileWiseExport.Visible = true;
                dgvMeterWiseExport.Visible = false;
            }
            else
            {
                dvgFileWiseExport.Visible = false;
                dgvMeterWiseExport.Visible = true;
            }
        }

        private void rbtnFileWise_CheckedChanged(object sender, EventArgs e)
        {
            if (dvgFileWiseExport.Rows.Count == 0)
                return;
            LoadData();
            foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                row.Cells["Include"].Value = false;
        }

        private void rbtnMeterWise_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvMeterWiseExport.Rows.Count == 0)
                return;
            LoadData();
            dgvMeterWiseExport.Columns["MeterData_ID"].Visible = false;
            foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                row.Cells["Include"].Value = false;
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnFileWise.Checked)
            {
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                    row.Cells["Include"].Value = chkAll.Checked;
            }
            else
            {
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                    row.Cells["Include"].Value = chkAll.Checked;
            }
        }
    }
}
