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
using CAB.Framework;
using CAB.Framework.Utility;
namespace CAB.UI
{
    public partial class ASCIIExportFileWise : MdiChildForm
    {
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL();
        private ASCIIExportSettingsEntity entity = null;
        private FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
        private bool isPUMA = false;
        public ASCIIExportFileWise()
        {
            InitializeComponent();
            if (UtilityDetails.GetUtilityDetails() == CAB.Framework.UtilityEntity.Generic)
            {
                isPUMA = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }
        //private string GetValuesOf(string index,string[] data)
        //{
        //    string value = string.Empty;
        //    string[] indices = index.Split(",);
        //    for (int i = 0; indices.Length - 1; i++)
        //    {
        //       value = string.Concat(value,data[Convert.ToInt32(indices[i])] + ",");

        //    }
        //        return value.Substring(0,value.Length-1);
        //}
        private void btnExportData_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            bool dataAvailable = false;
            if (cboSettings.SelectedIndex == -1)
            {
                MessageBox.Show("Please select file Name", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSettings.Focus();
                return;
            }
            string dlmsData = string.Empty;
            string fileId = ((System.Data.DataRowView)(cboSettings.Items[cboSettings.SelectedIndex])).Row.ItemArray[0].ToString();
            entity = asciiExportSettingsBLL.DetailData(fileId) as ASCIIExportSettingsEntity;

            string[] meterData_ID = new string[500];
            int counter = 0;
            bool result;
            bool flag = false;
            this.Cursor = Cursors.WaitCursor;
            if (rbtnFileWise.Checked)
            {
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    else
                    {
                        if (Convert.ToBoolean(val))
                        {
                            flag = true;
                            break;
                        }
                    }

                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    else
                    {
                        if (Convert.ToBoolean(val))
                        {
                            flag = true;
                            break;
                        }
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

                # region Billing
                //Billing
                bool IsHeader = true;
                setUnitForMeterType(entity);
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.BillingColumn))
                    //    {
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                    //    }
                    //    IsHeader = false;
                    //}
                    if (Convert.ToBoolean(val))
                    {
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
                        qry = qry.Substring(0, qry.Length - 1);
                        //entity.BillingDBColumn = entity.BillingDBColumn.Substring(0, entity.BillingDBColumn.Length - 1);
                        qry = string.Concat(entity.BillingDBColumn.Substring(0, entity.BillingDBColumn.Length - 1), " in(", qry, ")");
                        DataSet BillingDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (BillingDataSet == null)
                            continue;
                        if (BillingDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Billing data.";
                        Application.DoEvents();
                        DLMS650CommonBLL commonBLL = new DLMS650CommonBLL();
                       
                        //BillingDataSet = commonBLL.ApplyBillingEMF(BillingDataSet);  //newly added
                        // To solve bug 88662.
                        file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                        for (int i = 0; i < BillingDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < BillingDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region General
                //General
                IsHeader = true;
                counter = 0;
                //file.WriteLine(" ");
                //file.WriteLine(" ");
                //file.WriteLine(" "); 
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                   
                    if (Convert.ToBoolean(val))
                    {
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
                        qry = qry.Substring(0, qry.Length - 1);
                        //entity.GeneralDBColumn = entity.GeneralDBColumn.Substring(0, entity.GeneralDBColumn.Length - 1);
                        qry = string.Concat(entity.GeneralDBColumn.Substring(0, entity.GeneralDBColumn.Length - 1), " in(", qry, ")");
                        DataSet GeneralDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (GeneralDataSet == null)
                            continue;
                        if (GeneralDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting General data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.GeneralColumn.Trim(','));
                        for (int i = 0; i < GeneralDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < GeneralDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Instant
                //Instant
                IsHeader = true;
                counter = 0;
              
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
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
                        qry = qry.Substring(0, qry.Length - 1);
                        //entity.InstantDBColum = entity.InstantDBColum.Substring(0, entity.InstantDBColum.Length - 1);
                        qry = string.Concat(entity.InstantDBColum.Substring(0, entity.InstantDBColum.Length - 1), " in(", qry, ")");
                        DataSet InstantDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (InstantDataSet == null)
                            continue;
                        if (InstantDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Instant data.";
                        Application.DoEvents();
                        for (int i = 0; i < InstantDataSet.Tables[0].Rows.Count; i++)
                        {
                            if (ApplicationType.IEC_LTCT_650 == ConfigInfo.GetApplicationType())
                            {
                                string data = string.Empty;
                                for (int col = 0; col < InstantDataSet.Tables[0].Columns.Count; col++)
                                    data = string.Concat(data, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                                //data = string.Concat(data, Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                                if (string.IsNullOrEmpty(data))
                                    continue;
                                data = data.Substring(0, data.Length - 1);
                                file.WriteLine(data);
                            }
                            else if (ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType())
                            {
                                //condition changed from  if (i == InstantDataSet.Tables[0].Rows.Count - 1) to i==0 to bring get the meterID as the first record instead of the last; 26th April 2012
                                if (i == 0)
                                    dlmsData = string.Concat(dlmsData, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["MeterID"]), entity.Delimeter));
                                dlmsData = string.Concat(dlmsData, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["InstantPowerColumnValue"]), entity.Delimeter));
                                //dlmsData = string.Concat(dlmsData,Convert.ToString(InstantDataSet.Tables[0].Rows[i]["InstantPowerColumnValue"]), entity.Delimeter );
                                //return ApplyMultiplyFactor(meterDataId, dataSet, "Descriptions", "Value");
                                //dlmsData = string.Concat(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["InstantPowerColumnValue"]), entity.Delimeter, dlmsData);
                                //dlmsData = string.Concat(dlmsData,Convert.ToString(InstantDataSet.Tables[0].Rows[i]["MeterID"]), entity.Delimeter);
                            }


                        }
                        if (!string.IsNullOrEmpty(dlmsData))
                        {
                            dlmsData = dlmsData.Substring(0, dlmsData.Length - 1);
                            if (dataAvailable)
                            {
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                            }
                            file.WriteLine("MeterSerialNumber" + entity.InstantColumn);
                            file.WriteLine(dlmsData);
                            dlmsData = string.Empty;
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Tamper
                //Tamper
                IsHeader = true;
                counter = 0;
                //file.WriteLine(" ");
                //file.WriteLine(" ");
                //file.WriteLine(" ");
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (ApplicationType.IEC_LTCT_650 == ConfigInfo.GetApplicationType() && !string.IsNullOrEmpty(entity.TamperColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.TamperColumn);
                    //    }
                    //    else if ((ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType()) && !string.IsNullOrEmpty(entity.TamperColumn))
                    //    {
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine("MeterSerialNumber,EventCode,DateTimeEvent,CurrentIR,CurrentIY,CurrentIB,VoltageVRN,VoltageVYN,VoltageVBN,PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh");
                    //    }
                    //    IsHeader = false;
                    //}
                    if (Convert.ToBoolean(val))
                    {
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
                        qry = qry.Substring(0, qry.Length - 1);
                        //entity.TamberDBColumn = entity.TamberDBColumn.Substring(0, entity.TamberDBColumn.Length - 1);
                        qry = string.Concat(entity.TamberDBColumn.Substring(0, entity.TamberDBColumn.Length - 1), " in(", qry, ")");
                        DataSet TamperDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (TamperDataSet == null)
                            continue;
                        if (TamperDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Tamper data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber,EventCode,DateTimeEvent,CurrentIR,CurrentIY,CurrentIB,PhaseCurrent,VoltageVRN,VoltageVYN,VoltageVBN,PhaseVoltage,PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh,CumulativeEnergykVAh,TotalPowerFactor");
                        for (int i = 0; i < TamperDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < TamperDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter);

                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber,EventCode,DateTimeEvent,CurrentIR,CurrentIY,CurrentIB,VoltageVRN,VoltageVYN,VoltageVBN,PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh"); 
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Load Survey
                //Loadsurvey
                IsHeader = true;
                counter = 0;
                //file.WriteLine(" ");
                //file.WriteLine(" ");
                //file.WriteLine(" ");
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.LoadSurveyColumn))
                    //    {
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine(" ");
                    //        //file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                    //    }
                    //    IsHeader = false;
                    //}
                    if (Convert.ToBoolean(val))
                    {
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
                        qry = qry.Substring(0, qry.Length - 1);
                        //entity.LoadSurveyDBColumn = entity.LoadSurveyDBColumn.Substring(0, entity.LoadSurveyDBColumn.Length - 1);
                        qry = string.Concat(entity.LoadSurveyDBColumn.Substring(0, entity.LoadSurveyDBColumn.Length - 1), " in(", qry, ")");
                        DataSet LoadSurveyDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (LoadSurveyDataSet == null)
                            continue;
                        if (LoadSurveyDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Loadsurvey data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                        for (int i = 0; i < LoadSurveyDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < LoadSurveyDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter);

                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                                //    file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Midnight Energies - MVVNL
                //added for MVVNL
                //Midnight Energies

                counter = 0;
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
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
                        if (string.IsNullOrEmpty(entity.MidnightEnergiesColumn))
                            continue;
                        qry = qry.Substring(0, qry.Length - 1);
                        qry = string.Concat(entity.MidnightEnergiesDBColumn.Substring(0, entity.MidnightEnergiesDBColumn.Length - 1), " in(", qry, ")");
                        DataSet MidnightEnergiesDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (MidnightEnergiesDataSet == null)
                            continue;
                        if (MidnightEnergiesDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Midnight Energies.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.MidnightEnergiesColumn);
                        for (int i = 0; i < MidnightEnergiesDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < MidnightEnergiesDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(MidnightEnergiesDataSet.Tables[0].Rows[i][col]), entity.Delimeter));

                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);

                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.MidnightEnergiesColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                //added for MVVNL
                # endregion

                # region "SelfDiagnostics"
                //Instant
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                {
                    string val = Convert.ToString(dvgFileWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                    {
                        continue;
                    }
                    if (Convert.ToBoolean(val))
                    {
                        string filesName = dvgFileWiseExport.Rows[counter - 1].Cells["File Name"].Value.ToString();
                        FileUploadMasterEntity fileUploadMasterEntity = fileUploadMasterBLL.ValidateFile(filesName) as FileUploadMasterEntity;
                        DataSet dataSet = meterDataBLL.GetMeterDataSetID(fileUploadMasterEntity.FileUpload_ID);
                        if (dataSet == null)
                        {
                            continue;
                        }
                        if (dataSet.Tables.Count == 0)
                        {
                            continue;
                        }
                        string qry = "";
                        foreach (DataRow dataIdRow in dataSet.Tables[0].Rows)
                        {
                            qry = string.Concat(qry, Convert.ToString(dataIdRow[0]), ",");
                        }
                        if (string.IsNullOrEmpty(qry))
                        {
                            continue;
                        }
                        if (string.IsNullOrEmpty(entity.SelfDiagnosisDBColumn))
                        {
                            continue;
                        }
                        qry = qry.Substring(0, qry.Length - 1);
                        qry = string.Concat(entity.SelfDiagnosisDBColumn.Substring(0, entity.SelfDiagnosisDBColumn.Length - 1), " in(", qry, ")");
                        DataSet SelfDiagData = asciiExportSettingsBLL.GetParameterData(qry);
                        if (SelfDiagData == null)
                        {
                            continue;
                        }
                        if (SelfDiagData.Tables.Count == 0)
                        {
                            continue;
                        }
                        this.StatusMessage = "Exporting Instant data.";
                        Application.DoEvents();

                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.SelfDiagnosisColumn);
                        for (int i = 0; i < SelfDiagData.Tables[0].Rows.Count; i++)
                        {
                                string data = string.Empty;
                                for (int col = 0; col < SelfDiagData.Tables[0].Columns.Count; col++)
                                {
                                    data = string.Concat(data, GetASCIIValue(Convert.ToString(SelfDiagData.Tables[0].Rows[i][col]), entity.Delimeter));
                                }

                                if (string.IsNullOrEmpty(data))
                                {
                                    continue;
                                }
                                data = data.Substring(0, data.Length - 1);
                                file.WriteLine(data);
                                if (!string.IsNullOrEmpty(data))
                                {
                                    dataAvailable = true;
                                }
                        }
                    }
                }
                # endregion
                #endregion
            }
            else
            {
                #region Export Meter ID Wise
                counter = 0;

                # region Billing
                //Billing
                bool IsHeader = true;
                this.StatusMessage = "Exporting Billing data.";
                Application.DoEvents();
                setUnitForMeterType(entity);
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.BillingColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                    //    }
                    //    IsHeader = false;
                    //}
                    if (string.IsNullOrEmpty(entity.BillingDBColumn))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.BillingDBColumn, meterDataID);
                        DataSet BillingDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (BillingDataSet == null)
                            continue;
                        if (BillingDataSet.Tables.Count == 0)
                            continue;

                        // To solve bug 88662.
                        file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                        for (int i = 0; i < BillingDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < BillingDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(BillingDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.BillingColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region General
                //General
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.GeneralColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                    //    }
                    //    IsHeader = false;
                    //}

                    if (string.IsNullOrEmpty(entity.GeneralDBColumn))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.GeneralDBColumn, meterDataID);
                        DataSet GeneralDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (GeneralDataSet == null)
                            continue;
                        if (GeneralDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting General data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                        for (int i = 0; i < GeneralDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < GeneralDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(GeneralDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.GeneralColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Instant
                //Instant
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;

                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.InstantColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.InstantColumn);
                    //    }
                    //    IsHeader = false;
                    //}


                    if (string.IsNullOrEmpty(entity.InstantDBColum))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.InstantDBColum, meterDataID);
                        DataSet InstantDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (InstantDataSet == null)
                            continue;
                        if (InstantDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Instant data.";
                        Application.DoEvents();
                        for (int i = 0; i < InstantDataSet.Tables[0].Rows.Count; i++)
                        {
                            if (ApplicationType.IEC_LTCT_650 == ConfigInfo.GetApplicationType())
                            {
                                string data = string.Empty;
                                for (int col = 0; col < InstantDataSet.Tables[0].Columns.Count; col++)
                                    data = string.Concat(data, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                                //data = string.Concat(data, Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                                if (string.IsNullOrEmpty(data))
                                    continue;
                                data = data.Substring(0, data.Length - 1);
                                if (dataAvailable)
                                {
                                    file.WriteLine(" ");
                                    file.WriteLine(" ");
                                    file.WriteLine(" ");
                                }
                                file.WriteLine("MeterSerialNumber," + entity.InstantColumn);
                                file.WriteLine(data);
                            }
                            else if (ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType())
                            {

                                dlmsData = string.Concat(dlmsData, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["InstantPowerColumnValue"]), entity.Delimeter));
                                //dlmsData = string.Concat(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["InstantPowerColumnValue"]), entity.Delimeter, dlmsData);
                                if (i == InstantDataSet.Tables[0].Rows.Count - 1)
                                    //dlmsData = string.Concat(dlmsData, GetASCIIValue(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["MeterID"]), entity.Delimeter));
                                    // Fix by Swati 23/02/2012 for duplicate instant columns values.
                                    dlmsData = string.Concat(GetASCIIValue(InstantDataSet.Tables[0].Rows[i]["MeterID"].ToString(), entity.Delimeter), dlmsData);
                                //, dlmsData);
                                //dlmsData = string.Concat(Convert.ToString(InstantDataSet.Tables[0].Rows[i]["MeterID"]), entity.Delimeter, dlmsData);
                            }

                        }
                        if (!string.IsNullOrEmpty(dlmsData))
                        {
                            dlmsData = dlmsData.Substring(0, dlmsData.Length - 1);
                            //file.WriteLine(" ");
                            //file.WriteLine(" ");
                            //file.WriteLine(" ");
                            file.WriteLine("MeterSerialNumber," + entity.InstantColumn);
                            file.WriteLine(dlmsData);
                            dlmsData = string.Empty;
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Tamper
                //Tamper
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (ApplicationType.IEC_LTCT_650 == ConfigInfo.GetApplicationType() && !string.IsNullOrEmpty(entity.TamperColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.TamperColumn);
                    //    }
                    //    else if ((ApplicationType.DLMS_LTCT_650 == ConfigInfo.GetApplicationType()) && !string.IsNullOrEmpty(entity.TamperColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber,EventCode,DateTimeEvent,CurrentIR,CurrentIY,CurrentIB,VoltageVRN,VoltageVYN,VoltageVBN,PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh");
                    //    }

                    //    IsHeader = false;
                    //}
                    if (string.IsNullOrEmpty(entity.TamberDBColumn))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.TamberDBColumn, meterDataID);
                        DataSet TamperDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (TamperDataSet == null)
                            continue;
                        if (TamperDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Tamper data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber,EventCode,DateTimeEvent,CurrentIR,CurrentIY,CurrentIB,PhaseCurrent,VoltageVRN,VoltageVYN,VoltageVBN,PhaseVoltage,PowerFactorRphase,PowerFactorYphase,PowerFactorBphase,CumulativeEnergykWh,CumulativeEnergykVAh,TotalPowerFactor");
                        for (int i = 0; i < TamperDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < TamperDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(TamperDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.TamperColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Load Survey
                //Loadsurvey
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;
                    //if (IsHeader)
                    //{
                    //    if (!string.IsNullOrEmpty(entity.LoadSurveyColumn))
                    //    {
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine(" ");
                    //        file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                    //    }
                    //    IsHeader = false;
                    //}
                    if (string.IsNullOrEmpty(entity.LoadSurveyDBColumn))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.LoadSurveyDBColumn, meterDataID);
                        DataSet LoadSurveyDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (LoadSurveyDataSet == null)
                            continue;
                        if (LoadSurveyDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Loadsurvey data.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                        for (int i = 0; i < LoadSurveyDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < LoadSurveyDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            //data = string.Concat(data, Convert.ToString(LoadSurveyDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.LoadSurveyColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region Midnight Energies - MVVNL
                //Midnight Energies - MVVNL
                counter = 0;
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;
                    if (string.IsNullOrEmpty(val))
                        continue;

                    if (string.IsNullOrEmpty(entity.MidnightEnergiesDBColumn))
                        continue;
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.MidnightEnergiesDBColumn, meterDataID);
                        DataSet MidnightEnergiesDataSet = asciiExportSettingsBLL.GetParameterData(qry);
                        if (MidnightEnergiesDataSet == null)
                            continue;
                        if (MidnightEnergiesDataSet.Tables.Count == 0)
                            continue;
                        this.StatusMessage = "Exporting Midnight Energies.";
                        Application.DoEvents();
                        // To solve bug 88662.
                        file.WriteLine(" ");
                        file.WriteLine("MeterSerialNumber," + entity.MidnightEnergiesColumn);
                        for (int i = 0; i < MidnightEnergiesDataSet.Tables[0].Rows.Count; i++)
                        {
                            string data = string.Empty;
                            for (int col = 0; col < MidnightEnergiesDataSet.Tables[0].Columns.Count; col++)
                                data = string.Concat(data, GetASCIIValue(Convert.ToString(MidnightEnergiesDataSet.Tables[0].Rows[i][col]), entity.Delimeter));
                            if (string.IsNullOrEmpty(data))
                                continue;
                            data = data.Substring(0, data.Length - 1);
                            if (dataAvailable)
                            {
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                                //file.WriteLine(" ");
                            }
                            //file.WriteLine("MeterSerialNumber," + entity.MidnightEnergiesColumn);
                            file.WriteLine(data);
                            dataAvailable = true;
                        }
                    }
                }
                # endregion

                # region "Self Dignostics"
                //Instant
                IsHeader = true;
                counter = 0;

                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                {
                    string val = Convert.ToString(dgvMeterWiseExport.Rows[counter].Cells["Include"].Value);
                    counter++;

                    if (string.IsNullOrEmpty(val))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(entity.SelfDiagnosisDBColumn))
                    {
                        continue;
                    }
                    if (Convert.ToBoolean(val))
                    {
                        string meterDataID = dgvMeterWiseExport.Rows[counter - 1].Cells["MeterData_ID"].Value.ToString();
                        string qry = string.Concat(entity.SelfDiagnosisDBColumn, meterDataID);
                        DataSet SelfDignoData = asciiExportSettingsBLL.GetParameterData(qry);
                        if (SelfDignoData == null)
                        {
                            continue;
                        }
                        if (SelfDignoData.Tables.Count == 0)
                        {
                            continue;
                        }
                        this.StatusMessage = "Exporting Self Diagnostics data.";
                        Application.DoEvents();
                        for (int i = 0; i < SelfDignoData.Tables[0].Rows.Count; i++)
                        {
                                string data = string.Empty;
                                for (int col = 0; col < SelfDignoData.Tables[0].Columns.Count; col++)
                                {
                                    data = string.Concat(data, GetASCIIValue(Convert.ToString(SelfDignoData.Tables[0].Rows[i][col]), entity.Delimeter));
                                }
                                //data = string.Concat(data, Convert.ToString(InstantDataSet.Tables[0].Rows[i][col]), entity.Delimeter);
                                if (string.IsNullOrEmpty(data))
                                {
                                    continue;
                                }
                                data = data.Substring(0, data.Length - 1);
                                if (dataAvailable)
                                {
                                    file.WriteLine(" ");
                                    file.WriteLine(" ");
                                    file.WriteLine(" ");
                                }
                                file.WriteLine("MeterSerialNumber," + entity.SelfDiagnosisColumn);
                                file.WriteLine(data);
                                dataAvailable = true;
                        }
                    }
                }
                # endregion

                #endregion
            }
            file.Close();
            stream.Close();
            if (dataAvailable == false)
            {
                File.Delete(filename);
                this.StatusMessage = "No data available";
            }
            else
            {
                this.StatusMessage = "File Exported Successfully.";
            }
            this.Cursor = Cursors.Default;
        }

        private void setUnitForMeterType(ASCIIExportSettingsEntity entity)
        {
            entity.BillingColumn = ApplyUnit(entity.BillingColumn);
            entity.InstantColumn = ApplyUnit(entity.InstantColumn);
            entity.LoadSurveyColumn = ApplyUnit(entity.LoadSurveyColumn);
            entity.MidnightEnergiesColumn = ApplyUnit(entity.MidnightEnergiesColumn);
            entity.TamperColumn = ApplyUnit(entity.TamperColumn);
            entity.GeneralColumn = ApplyUnit(entity.GeneralColumn);
        }

        private string ApplyUnit(string paramterColumns)
        {
            string unit = "k";
            if (rdHTCT.Checked)
            {
                unit = "M";
            }
            if (string.IsNullOrEmpty(paramterColumns))
            {
                return string.Empty;
            }
            paramterColumns = paramterColumns.Replace("Block Active Energy", string.Format("Block Energy {0}Wh", unit));
            paramterColumns = paramterColumns.Replace("Block Reactive Energy", string.Format("Block Energy {0}varh", unit));
            paramterColumns = paramterColumns.Replace("Block Active Energy", string.Format("Block Active Energy {0}VAh", unit));

            paramterColumns = paramterColumns.Replace("Active Energy", string.Format("Active Energy {0}Wh", unit));
            paramterColumns = paramterColumns.Replace("Reactive Energy", string.Format("Reactive Energy {0}varh", unit));
            paramterColumns = paramterColumns.Replace("Apparent Energy", string.Format("Apparent Energy {0}vah", unit));
            paramterColumns = paramterColumns.Replace("Reactive Demand", string.Format("Reactive Demand {0}var", unit));
            paramterColumns = paramterColumns.Replace("Active Demand", string.Format("Active Demand {0}W", unit));
            paramterColumns = paramterColumns.Replace("MD Active", string.Format("MD {0}VA", unit));

            paramterColumns = paramterColumns.Replace("Reactive Demand (Lead)", string.Format("Reactive Demand {0}VAR (Lead)", unit));
            paramterColumns = paramterColumns.Replace("Apparent Demand", string.Format("Apparent Demand {0}VA", unit));
            paramterColumns = paramterColumns.Replace("Reative Demand (Lag)", string.Format("Reative Demand {0}VAR(Lag)", unit));

            paramterColumns = paramterColumns.Replace("Active Power", string.Format("Active Power {0}W", unit));
            paramterColumns = paramterColumns.Replace("Apparent Power", string.Format("Power {0}VA", unit));
            paramterColumns = paramterColumns.Replace("Reactive Power", string.Format("Reactive Power {0}var", unit));
            
            return paramterColumns;
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
        private void FillFileWiseGrid()
        {
            string meterType;

            DataSet fileWiseDataSet;
            if (isPUMA)
            {
                if (rdLTCT.Checked)
                {
                    meterType = "LTCT";
                }
                else
                {
                    meterType = "HTCT";
                }
                fileWiseDataSet = meterDataBLL.AsciiPUMAFileExportListDataSet(meterType);
            }
            else
            {
                fileWiseDataSet = meterDataBLL.FileExportListDataSet(false);
            }
            if (dvgFileWiseExport.Columns["Include"] != null)
            {
                dvgFileWiseExport.Columns.Remove("Include");
            }

            if (fileWiseDataSet == null)
            {
                dvgFileWiseExport.DataSource = null;
                return;
            }
          

            if (fileWiseDataSet.Tables.Count == 0)
                return;
            dvgFileWiseExport.DataSource = null;
           
            dvgFileWiseExport.DataSource = fileWiseDataSet.Tables[0].DefaultView;
            dvgFileWiseExport.Columns[0].Width = 50;
            dvgFileWiseExport.Columns[0].HeaderText = "S.No.";
            dvgFileWiseExport.Columns[0].ReadOnly = true;

            dvgFileWiseExport.Columns[1].Width = 250;
            dvgFileWiseExport.Columns[1].HeaderText = "File Name";
            dvgFileWiseExport.Columns[1].ReadOnly = true;

            dvgFileWiseExport.Columns[2].Width = 150;
            dvgFileWiseExport.Columns[2].HeaderText = "Uploading DateTime";
            dvgFileWiseExport.Columns[2].ReadOnly = true;

            if (dvgFileWiseExport.Columns.Count <= 3)
            {

                DataGridViewCheckBoxColumn objdvgFileWiseExport = new DataGridViewCheckBoxColumn();
                objdvgFileWiseExport.Name = "Include";
                dvgFileWiseExport.Columns.Add(objdvgFileWiseExport);
                dvgFileWiseExport.Columns[3].HeaderText = "Include";
                dvgFileWiseExport.Columns[3].Width = 80;
                dvgFileWiseExport.Columns[3].ReadOnly = false;
            }
            foreach (DataGridViewColumn column in dvgFileWiseExport.Columns)
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

        }
        private void FillMeterWiseGrid()
        {
            string meterType;
            DataSet dataSet;

            if (isPUMA)
            {
                if (rdLTCT.Checked)
                {
                    meterType = "LTCT";
                }
                else
                {
                    meterType = "HTCT";
                }
                dataSet = meterDataBLL.AsciiPUMAListExportDataSet(meterType);
            }
            else
            {
                dataSet = meterDataBLL.ListExportDataSet();
            }

            if (dgvMeterWiseExport.Columns["Include"] != null)
            {
                dgvMeterWiseExport.Columns.Remove("Include");
            }
            if (dataSet == null)
            {
                dgvMeterWiseExport.DataSource = null;
                return;
            }
            
            if (dataSet.Tables.Count == 0)
                return;
            dgvMeterWiseExport.DataSource = null;
            
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
            if (dgvMeterWiseExport.Columns["Include"] == null)
            {

                DataGridViewCheckBoxColumn objCmb = new DataGridViewCheckBoxColumn();
                objCmb.Name = "Include";
                dgvMeterWiseExport.Columns.Add(objCmb);
                dgvMeterWiseExport.Columns[5].HeaderText = "Include";

                dgvMeterWiseExport.Columns[5].Width = 80;
                dgvMeterWiseExport.Columns[5].ReadOnly = false;
            }
            foreach (DataGridViewColumn column in dgvMeterWiseExport.Columns)
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
        }
        private void ASCIIExportFileWise_Load(object sender, EventArgs e)
        {

            this.Text = "ASCII Export";
            this.StatusMessage = string.Empty;
            DataSet dataSet1 = asciiExportSettingsBLL.ListDataSet();
            cboSettings.DataSource = dataSet1.Tables[0];
            cboSettings.DisplayMember = "FileName";
            cboSettings.ValueMember = "Asciiexportsettings_ID";
            if (isPUMA)
            {
                gbMeterType.Visible = true;
            }
            else
            {
                gbMeterType.Visible = false;
            }
            FillFileWiseGrid();
            FillMeterWiseGrid();
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
            FillFileWiseGrid();
            chkAll.Checked = false;
            if (dvgFileWiseExport.Rows.Count == 0)
                return;
            LoadData();

            foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                row.Cells["Include"].Value = false;
        }

        private void rbtnMeterWise_CheckedChanged(object sender, EventArgs e)
        {
            FillMeterWiseGrid();
            chkAll.Checked = false;
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
            if (rbtnMeterWise.Checked)
            {
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                    row.Cells["Include"].Value = chkAll.Checked;
            }
            else
            {
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                    row.Cells["Include"].Value = chkAll.Checked;
            }
        }
        // Fix by swati
        private void dvgFileWiseExport_CheckBoxChange(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    chkAll.CheckedChanged -= chkAll_CheckedChanged;

                    if ((bool)dvgFileWiseExport.CurrentCell.EditedFormattedValue == false)
                        chkAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < dvgFileWiseExport.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = dvgFileWiseExport[0, i] as DataGridViewCheckBoxCell;
                            if (cell.EditedFormattedValue == null || (bool)cell.EditedFormattedValue == false)
                            { IfAllRowsSelected = false; break; }
                        }
                        chkAll.Checked = IfAllRowsSelected;
                    }
                    chkAll.CheckedChanged += chkAll_CheckedChanged;
                }
            }
        }
        // Fix by swati
        private void dgvMeterWiseExport_CheckBoxChange(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 0)
                {
                    chkAll.CheckedChanged -= chkAll_CheckedChanged;

                    if ((bool)dgvMeterWiseExport.CurrentCell.EditedFormattedValue == false)
                        chkAll.Checked = false;
                    else
                    {
                        bool IfAllRowsSelected = true;
                        for (int i = 0; i < dgvMeterWiseExport.Rows.Count; i++)
                        {
                            DataGridViewCheckBoxCell cell = dgvMeterWiseExport[0, i] as DataGridViewCheckBoxCell;
                            if (cell.EditedFormattedValue == null || (bool)cell.EditedFormattedValue == false)
                            { IfAllRowsSelected = false; break; }
                        }
                        chkAll.Checked = IfAllRowsSelected;
                    }
                    chkAll.CheckedChanged += chkAll_CheckedChanged;
                }
            }
        }

        private void rdLTCT_CheckedChanged(object sender, EventArgs e)
        {
            chkAll.Checked = false;
            if (rbtnFileWise.Checked)
            {
                FillFileWiseGrid();
                if (dvgFileWiseExport.Rows.Count == 0)
                    return;
                LoadData();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                    row.Cells["Include"].Value = false;
            }
            else
            {
                FillMeterWiseGrid();
                if (dgvMeterWiseExport.Rows.Count == 0)
                    return;
                LoadData();
                dgvMeterWiseExport.Columns["MeterData_ID"].Visible = false;
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                    row.Cells["Include"].Value = false;
            }

        }

        private void rdHTCT_CheckedChanged(object sender, EventArgs e)
        {
            chkAll.Checked = false;

            if (rbtnFileWise.Checked)
            {
                FillFileWiseGrid();
                if (dvgFileWiseExport.Rows.Count == 0)
                    return;
                LoadData();
                foreach (DataGridViewRow row in dvgFileWiseExport.Rows)
                    row.Cells["Include"].Value = false;
            }
            else
            {
                FillMeterWiseGrid();
                if (dgvMeterWiseExport.Rows.Count == 0)
                    return;
                LoadData();
                dgvMeterWiseExport.Columns["MeterData_ID"].Visible = false;
                foreach (DataGridViewRow row in dgvMeterWiseExport.Rows)
                    row.Cells["Include"].Value = false;
            }
        }
    }
}
