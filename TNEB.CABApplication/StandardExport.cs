using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.IECChannel.ReadOut;
using CAB.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class StandardExport : MdiChildForm
    {
        private string fileName = string.Empty;
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private ASCIIExportSettingsBLL asciiExportSettingsBLL = new ASCIIExportSettingsBLL(); 
        public StandardExport()
        {
            InitializeComponent();
        }

        private void StandardExport_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Text = "Standard Export";
            DataSet mainListDataSet = meterDataBLL.FileExportListDataSet();
            if (mainListDataSet == null)
                return;
            if (mainListDataSet.Tables.Count == 0)
                return; 
            grdFileList.DataSource = mainListDataSet.Tables[0].DefaultView;  
            foreach (DataGridViewColumn column in grdFileList.Columns)
            {
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
            SetTopGridEqualWidth(mainListDataSet); 
            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "Include";
            grdFileList.Columns.Add(checkBoxCol);
            grdFileList.Columns[3].HeaderText = "Include";
            grdFileList.Columns[3].ReadOnly = false; 
        }

        public void SetTopGridEqualWidth(DataSet dataSet)
        {
            int width = grdFileList.Width - 50;
            int totCol = grdFileList.Columns.Count;
            if (totCol == 0)
                return;
            int ExactWidth = width / totCol;
            int i = 1;
            foreach (DataColumn col in dataSet.Tables[0].Columns)
            {
                if (i == 1 )
                   ExactWidth= ExactWidth / 3;
                else if(  i == 3)
                    ExactWidth = (width / totCol)-50;
                else
                    ExactWidth=width / totCol;
                this.grdFileList.Columns[col.ColumnName].Width = ExactWidth;
                i++;
            }
        } 
 
         
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            this.StatusMessage = string.Empty;
            bool flag = false;
            int count = 0;
            foreach (DataGridViewRow row in grdFileList.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    flag = (bool)row.Cells[0].Value;
                    if (flag)
                        count++;
                }
                
            }

            /* GKG 11/02/2013 135724 File Export Issue*/
            //if (count > 1)
            //{
            //    this.StatusMessage = "Please select only one file to export";               
            //    return;

            //}
            /* GKG 11/02/2013 135724 File Export Issue*/
            if (count==0)
            {
                this.StatusMessage = "Please select file";
                return;
            }
            bool IsSaved=FindAndWriteContents();
            this.StatusMessage = "Exporting the file.";
            Application.DoEvents();
            if (IsSaved)
                this.StatusMessage = "File Exported Successfully."; 
            else
                this.StatusMessage = "";
            Application.DoEvents();
        }
        private bool FindAndWriteContents()
        {
            string fileLocation = string.Empty; 
            SaveFileDialog DialogSave = new SaveFileDialog(); 
            DialogSave.DefaultExt = "EXP"; 
            DialogSave.Filter = "Export file (*.EXP)|*.EXP"; 
            DialogSave.AddExtension = true; 
            DialogSave.RestoreDirectory = true; 
            DialogSave.FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".EXP";
            DialogSave.Title = "Where do you want to save the file?"; 
            DialogSave.RestoreDirectory = true; 
            if (DialogSave.ShowDialog() == DialogResult.OK)
            {
                if (DialogSave.FileName == "") 
                    return false; 
                else
                {
                    fileLocation = DialogSave.FileName.Trim();
                    return WriteContents(fileLocation);
                } 
            }
            else 
                return false;  
        }
        private bool WriteContents(string fileLocation)
        {    
            StreamWriter file = new StreamWriter(fileLocation); 
            StringBuilder strBuild = new StringBuilder();
            /* GKG 11/02/2013 135724 File Export Issue*/
            //int cols = 0; 
            //foreach (DataGridViewRow row in grdFileList.Rows)
            //{
                //string val=Convert.ToString( grdFileList.Rows[cols].Cells["Include"].Value);
                //  cols++;
                //  if (string.IsNullOrEmpty(val))
                //    continue;
                //fileName = grdFileList.Rows[cols-1].Cells["File Name"].Value.ToString(); 
                //FileUploadMasterBLL filebll = new FileUploadMasterBLL();
                //FileUploadMasterEntity entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                //byte[] CABFile = entity.FileContent;
                //string fileContent = Encoding.ASCII.GetString(CABFile);
                //fileContent = ConfigInfo.DecryptFile(fileContent);
                //strBuild.Append(fileContent);

         
            //} 

            int rowCounter = 0;
            FileUploadMasterBLL filebll = null;
            FileUploadMasterEntity entity = null;
            string fileContent = String.Empty;
            foreach (DataGridViewRow row in grdFileList.Rows)
            {
                if (Convert.ToBoolean(grdFileList.Rows[rowCounter].Cells["Include"].Value))
                {
                    fileName = grdFileList.Rows[rowCounter].Cells["File Name"].Value.ToString();
                    filebll = new FileUploadMasterBLL();
                    entity = filebll.ValidateFile(fileName) as FileUploadMasterEntity;
                    if (Path.GetExtension(fileName) == ".2NG")
                    {
                        fileContent = Encoding.ASCII.GetString(entity.FileContent);
                    }
                    else
                    {
                        fileContent = ConfigInfo.DecryptFile(Encoding.ASCII.GetString(entity.FileContent));
                    }
                    strBuild.Append(fileContent);
                }
                rowCounter++;
            }
            /* GKG 11/02/2013 135724 File Export Issue*/

            if (strBuild.ToString().Length == 0)
            {
                this.StatusMessage = "No data available in file.";
                return false;
            }
            if (Path.GetExtension(fileName) == ".2NG")
            {
                file.Write(strBuild.ToString());
            }
            else
            {
                string calculatedBCC = ReadoutCommon.CalculateFileBcc(strBuild.ToString());
                string bcc = Convert.ToChar(calculatedBCC).ToString();
                file.Write(ConfigInfo.EncryptFile(strBuild.ToString() + bcc));
            }
            file.Close();
            return true;
        }

        private void StandardExport_Deactivate(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in grdFileList.Rows)
                row.Cells["Include"].Value = checkBox1.Checked;
        }

    }
}
