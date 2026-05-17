using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using System.IO;
using System.Threading;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.Parser;
using System.Globalization;
using Common.EntityMapper;
using CAB.DALC.Data;
using CABApplication.Export_Import;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class HVDSExport : MdiChildForm
    {
        private OpenFileDialog openFileDialog = null;
        List<Thread> thUploadList = new List<Thread>();
        Dictionary<string, HVDS_Readout_File> dicHVDS = null;
        string Seperator = ",";
        int iteratorGrid = 0;
        int ExportMaxCount = 100;
        string HVDS_Export_Directory =  string.Concat(ConfigInfo.GetLocation(), @"\HVDS Export");
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(HVDSExport).ToString());
        public HVDSExport()
        {
            try
            {
                InitializeComponent();     
                ExportMaxCount =  Convert.ToInt32(ConfigSettings.GetValue("ExportMaxCount"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "HVDSExport()", ex);
             
            }
                             
        }

        
        private void EnableDisableControl(Control cnt, bool IsEnable)
        {
            try
            {
                Action a = () =>
                {
                    cnt.Enabled = IsEnable;                   
                };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                //MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("BCS Export Error...", Color.Red);
                logger.Log(LOGLEVELS.Error, "EnableDisableControl(Control cnt, bool IsEnable)", ex);
            }
        }

        private void ChangeCursor(Cursor cur)
        {
            try
            {
                Action a = () =>
                {
                    this.Cursor = cur;
                };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                //MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);          
                UpdateStatusMessage("BCS Export Error...", Color.Red);
                logger.Log(LOGLEVELS.Error, "ChangeCursor(Cursor cur)", ex);
            }
        }

        private void EnableDisableFormControls(bool flag)
        {
            try
            {
               
                if (!flag)
                {
                    ChangeCursor(Cursors.WaitCursor);
                }
                else
                {
                    ChangeCursor(Cursors.Default);
                }
                EnableDisableControl(btnUpload, flag);
                EnableDisableControl(btnAbort, !flag);               
                EnableDisableControl(btnBrowse, flag);
                EnableDisableExportControl(flag);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                //MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);          
                UpdateStatusMessage("BCS Export Error...", Color.Red);
                logger.Log(LOGLEVELS.Error, "EnableDisableFormControls(bool flag)", ex);
            }
        }

        private void ReadOnlyGrid(bool IsReadOnly)
        {
            try
            {
                Action a = () =>
                {
                    dgvList.ReadOnly = IsReadOnly;
                };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                //MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                UpdateStatusMessage("BCS Export Error...", Color.Red);
                logger.Log(LOGLEVELS.Error, "ReadOnlyGrid(bool IsReadOnly)", ex);
            }
        }


        private void EnableDisableExportControl(bool flag)
        {
            try
            {
                if (flag && (dgvList.Rows.Count > 0))
                {
                    EnableDisableControl(btnExport, true);
                    EnableDisableControl(chkSelectAll, true);
                    ReadOnlyGrid(false);
                }
                else
                {
                    EnableDisableControl(btnExport, false);
                    EnableDisableControl(chkSelectAll, false);
                    ReadOnlyGrid(true);
                    
                }
            }
            catch (Exception ex)    //Exception log for catch block 
            {

                logger.Log(LOGLEVELS.Error, "EnableDisableExportControl(bool flag)", ex);
            }           
        }      

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (txtBoxFileName.Text.Trim() == "" || txtBoxFileName.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Select .2nG File to Upload", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int FileCount = 0;
                
                foreach (string filePath in openFileDialog.FileNames)
                {
                    if (Path.GetFileNameWithoutExtension(filePath).Length > 40)
                    {
                        MessageBox.Show("File name should not exceed 40 characters." + filePath, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxFileName.Focus();
                        return;
                    }
                    FileCount++;
                }

                if (FileCount == 0)
                {
                    MessageBox.Show("Please Select .2nG File to Upload", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxFileName.Text = "";
                    return;
                }
                if (FileCount > ExportMaxCount)
                {
                    MessageBox.Show("Max " + ExportMaxCount + " .2nG File allowed to Upload, Try Again", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxFileName.Text = "";
                    return;
                }

                EnableDisableFormControls(false);    
                bool IsUploaded = false;
                dicHVDS = new Dictionary<string, HVDS_Readout_File>();
                InstantiateGrid();
                chkSelectAll.Checked = false;
                foreach (string filePath in openFileDialog.FileNames)
                {
                    UpdateStatusMessage("Uploading " + Path.GetFileName(filePath) + "..", Color.Green);
                    Application.DoEvents();
                    if (Path.GetExtension(filePath).ToUpper() == ".2NG")
                    {
                        IsUploaded = Upload2nGFile(Path.GetFileNameWithoutExtension(filePath), filePath, GetFileContent(filePath));
                    }
                }
                if (IsUploaded)
                {
                    PopulateGrid(dicHVDS);
                    UpdateStatusMessage("Files uploaded successfully...", Color.Green);
                }
                else
                {
                    UpdateStatusMessage("Files not uploaded...", Color.Red);
                }               



            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "btnUpload_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                EnableDisableFormControls(true);
            }
        }

       

        public string GetFileContent(string filePath)
        {
            string fileContent = string.Empty;
            try
            {
                StreamReader streamReader = new StreamReader(filePath);
                fileContent = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, " GetFileContent(string filePath)", ex);
            }
            return fileContent;
        }

      
               
                
           

        private bool Upload2nGFile(string fileName,string filePath, string fileText)
        {
            bool ResFlag = false;
            try
            {               
                if (!Directory.Exists(HVDS_Export_Directory))
                {
                    Directory.CreateDirectory(HVDS_Export_Directory);
                }
                if (string.IsNullOrEmpty(fileText))
                {
                    UpdateStatusMessage("File Corrupted..." + "..", Color.Red);
                    Application.DoEvents();
                    return ResFlag;
                }

                
                string[] individualFileContent = fileText.Split('$');
                int iterator = 0;
                foreach (string itemFileContent in individualFileContent)
                {
                    if (itemFileContent.Trim().Length > 5)
                    {
                        iterator++;
                        HVDS_Readout_File objHVDS_Readout_File = new HVDS_Readout_File(HVDS_Export_Directory);

                        objHVDS_Readout_File.FileText = itemFileContent.Substring(27);
                        objHVDS_Readout_File.SourceFilePath = filePath;
                        int meterIdLength = Convert.ToInt32(itemFileContent.Substring(2, 2).Trim('\r').Trim('\n'));
                        objHVDS_Readout_File.MeterIdLength = meterIdLength;
                        objHVDS_Readout_File.ReadingDateTime = DateTime.ParseExact(itemFileContent.Substring(4, 22).Trim('\r').Trim('\n').Replace('-', '/'), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        objHVDS_Readout_File.MeterID = GetMeterID(itemFileContent, meterIdLength);              
                        string itemFile_name = fileName + "_" + iterator;
                        dicHVDS.Add(itemFile_name, objHVDS_Readout_File);
                    }
                }
                ResFlag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, "Upload2nGFile(string fileName,string filePath, string fileText)", ex);
            }
            return ResFlag;
        }

        string GetMeterID(string itemFileContent, int meterIdLength)
        {
            string MeterId = string.Empty;
            try
            {
                int index = itemFileContent.IndexOf("0000600100FF", 0);
                int mIDLenght = FromHex(itemFileContent.Substring(index + 16, 2))[0];
                MeterId = ASCIIEncoding.ASCII.GetString(FromHex(itemFileContent.Substring(index + 18, (meterIdLength * 2))));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterID(string itemFileContent, int meterIdLength)", ex);           
            }
            return MeterId;
        }


      


        public byte[] FromHex(string hex)
        {
            //***********Convert from Hex Byte String to equivalent Byte Array*****//
            byte[] raw = new byte[hex.Length / 2];
            try
            {
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FromHex(string hex)", ex);
            }
            return raw;
        }

        private void InstantiateGrid()
        {
            try
            {
                Action a = () =>
                    {
                        iteratorGrid = 0;
                        if (dgvList.Columns == null || dgvList.Columns.Count == 0)
                        {
                            DataGridViewColumn dgvc0 = new DataGridViewColumn(new DataGridViewTextBoxCell());
                            dgvc0.Name = "S.No.";
                            //dgvc1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc0.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc0.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc0);


                            DataGridViewColumn dgvc1 = new DataGridViewColumn(new DataGridViewTextBoxCell());
                            dgvc1.Name = "FileSerialNameIndex";
                            dgvc1.Visible = false;
                            //dgvc1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc1.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc1.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc1);



                            DataGridViewColumn dgvc1_ = new DataGridViewColumn(new DataGridViewTextBoxCell());
                            dgvc1_.Name = "FileSerialName";
                            dgvc1_.Visible = true;
                            //dgvc1.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc1_.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc1_.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc1_);


                            DataGridViewColumn dgvc2 = new DataGridViewColumn();
                            dgvc2.Name = "MeterSerialNumber";
                            //dgvc2.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc2.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc2.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc2);


                            DataGridViewColumn dgvc3 = new DataGridViewColumn();
                            dgvc3.Name = "ReadingDateTime";
                            //dgvc3.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc3.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc3.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc3);


                            DataGridViewCheckBoxColumn dgvc = new DataGridViewCheckBoxColumn();
                            dgvc.Name = "Export";
                            //dgvc.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc.FlatStyle = FlatStyle.Standard;
                            dgvc.ThreeState = false;
                            dgvc.CellTemplate = new DataGridViewCheckBoxCell();
                            dgvc.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc);

                            DataGridViewColumn dgvc4 = new DataGridViewColumn();
                            dgvc4.Name = "Status";
                            //dgvc3.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            dgvc4.CellTemplate = new DataGridViewTextBoxCell();
                            dgvc4.CellTemplate.Style.BackColor = Color.Beige;
                            dgvList.Columns.Add(dgvc4);

                        }


                        if (dgvList.Rows != null && dgvList.Rows.Count > 0)
                        {
                            dgvList.Rows.Clear();
                        }
                    };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, "InstantiateGrid()", ex);
            }
        }


        private void PopulateGrid(Dictionary<string, HVDS_Readout_File> dicHVDS)
        {
            try
            {                          
                foreach (KeyValuePair<string, HVDS_Readout_File> item in dicHVDS)
                {
                    Action a = () =>
                        {
                            dgvList.Rows.Add();
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["S.No."].Value = ++iteratorGrid;
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["FileSerialNameIndex"].Value = item.Key;
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["FileSerialName"].Value = Path.GetFileNameWithoutExtension(item.Value.SourceFilePath);
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["MeterSerialNumber"].Value = item.Value.MeterID;
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["ReadingDateTime"].Value = item.Value.ReadingDateTime;
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["Export"].Value = false;
                            dgvList.Rows[dgvList.Rows.Count - 1].Cells["Status"].Value = string.Empty;
                        };
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(a);
                    }
                    else
                    {
                        a();
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, "PopulateGrid(Dictionary<string, HVDS_Readout_File> dicHVDS)", ex);
            }           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {               
                EnableDisableFormControls(true);
                Clear_Abort_ThreadList();
                UpdateStatusMessage("Export Aborted", Color.Red);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "btnCancel_Click(object sender, EventArgs e)", ex);
            }
        }

        private void Clear_Abort_ThreadList()
        {
            try
            { 
                foreach (Thread item in thUploadList)
                {
                    if (item != null && item.IsAlive)
                    {
                        item.Abort();
                    }
                }
                thUploadList.Clear();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Clear_Abort_ThreadList()", ex);         
            }
        }



        private void UpdateStatusMessage(string Message, Color colr)
        {
            try
            {
                Action a = () =>
                {
                    stsmsg.ForeColor = colr;
                    stsmsg.Text = Message;
                };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "UpdateStatusMessage(string Message, Color colr)", ex);
            }
        }



        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableFormControls(false);
                UpdateStatusMessage(string.Empty, Color.Green);
                Application.DoEvents();
                InstantiateGrid();
                chkSelectAll.Checked = false;
                txtBoxFileName.Text = string.Empty;
                openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = ConfigInfo.GetLocation();
                openFileDialog.Multiselect = true;
                openFileDialog.DefaultExt = "2NG";
                openFileDialog.Filter = "Readout(*.2NG)|*.2NG";
                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    int counter = 0;
                    foreach (string fileName in openFileDialog.SafeFileNames)
                    {
                        counter++;
                        txtBoxFileName.Text += fileName;
                        if (counter < openFileDialog.SafeFileNames.Length)
                        {
                            txtBoxFileName.Text += Seperator;
                        }


                        //break;
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "btnBrowse_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                EnableDisableFormControls(true);
            }
        }

        private void StartExport()
        {
            try
            {
                EnableDisableFormControls(false);
                foreach (DataGridViewRow item in dgvList.Rows)
                {
                    if (Convert.ToBoolean(item.Cells["Export"].Value) == true)
                    {
                        UpdateStatusMessage("Exporting File...", Color.Green);
                        HVDS_Readout_File objHVDS_Readout_File = dicHVDS[Convert.ToString(item.Cells["FileSerialNameIndex"].Value)];
                        objHVDS_Readout_File.dgvStatuscell = item.Cells["Status"];
                        Thread th = new Thread(objHVDS_Readout_File.ExportFile);
                        th.Name = "Child";
                        th.Start();
                        thUploadList.Add(th);
                    }
                }
                foreach (Thread item in thUploadList)
                {
                    if (item.Name == "Child")
                    {
                        item.Join();
                    }
                }
                UpdateStatusMessage("", Color.Green);               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, "StartExport()", ex);
            }
            finally
            {
                EnableDisableFormControls(true);
            }           
        }


        private bool ValidateExportDatagridViewList()
        {
            bool flag = false;
            try
            {
                foreach (DataGridViewRow item in dgvList.Rows)
                {
                    item.Cells["Status"].Value = string.Empty;
                    ((DataGridViewCell)item.Cells["Status"]).Style.BackColor = Color.Beige;
                    if (Convert.ToBoolean(item.Cells["Export"].Value) == true)
                    {
                        flag = true;                        
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValidateExportDatagridViewList()", ex);
              
            }
            return flag;
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvList != null && ValidateExportDatagridViewList())
                {
                    Clear_Abort_ThreadList();
                    Thread thExport = new Thread(StartExport);
                    thUploadList.Add(thExport);
                    thExport.Start();

                }
                else
                {
                    MessageBox.Show("Please Select File to Export from DataGridView", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                UpdateStatusMessage("BCS Export Error..." + "..", Color.Red);
                logger.Log(LOGLEVELS.Error, "btnExport_Click(object sender, EventArgs e)", ex);
            }
        }

        private void HVDSExport_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                dgvList.Height = this.Height - grpUpload.Height - 50;
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "HVDSExport_SizeChanged(object sender, EventArgs e)", ex);
            }
        }

        private void HVDSExport_Load(object sender, EventArgs e)
        {
            try
            {
                EnableDisableFormControls(true);                
                InstantiateGrid();
                txtOutput.Text = HVDS_Export_Directory.Replace("\\\\","\\");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "HVDSExport_Load(object sender, EventArgs e)", ex);
            }
        }

        private void HVDSExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Clear_Abort_ThreadList();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                //MessageBox.Show(ex.ToString() + MethodInfo.GetCurrentMethod().Name);
                MessageBox.Show("BCS Export Error...", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                logger.Log(LOGLEVELS.Error, "HVDSExport_FormClosing(object sender, FormClosingEventArgs e)", ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {               
                this.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "btnClose_Click(object sender, EventArgs e)", ex);
            }
        }


        private void CheckUncheckGrid(bool flag)
        {
            try
            {
                Action a = () =>
                {
                    foreach (DataGridViewRow dgvr in dgvList.Rows)
                    {
                        dgvr.Cells["Export"].Value = flag;
                    }
                };
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckUncheckGrid(bool flag)", ex);

            }
        }
        


        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckUncheckGrid(chkSelectAll.Checked);
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "chkSelectAll_CheckedChanged(object sender, EventArgs e)", ex);
            }
        }       
    }


    
}
