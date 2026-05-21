using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Entity;
namespace CAB.UI
{
    public partial class ConsumerExport : MdiChildForm
    { 
        private ConsumerExportSettingsBLL consumerExportSettingsBLL = new ConsumerExportSettingsBLL();
        private ConsumerExportSettingsEntity detailEntity = null;
        public ConsumerExport()
        {
            InitializeComponent();
        }
 
        private void ConsumerExport_Load(object sender, EventArgs e)
        {
            this.Text = "Export Consumer Data";
            this.StatusMessage = string.Empty;
            DataSet dataSet = consumerExportSettingsBLL.ListDataSet();
            cboFileFormate.DataSource = dataSet.Tables[0];
            cboFileFormate.DisplayMember = "FileName";
            cboFileFormate.ValueMember = "ConsumerExportSettings_ID";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cboFileFormate.Items.Count > 0)
            {
                string fileId = ((System.Data.DataRowView)(cboFileFormate.Items[cboFileFormate.SelectedIndex])).Row.ItemArray[0].ToString();
                detailEntity = consumerExportSettingsBLL.DetailData(fileId) as ConsumerExportSettingsEntity;
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "Text files (*.txt)|*.txt|*.csv|*.csv";
                savefile.RestoreDirectory = true;
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    if (ExportToCSVFormate(savefile.FileName))
                        this.StatusMessage = "File Exported Successfully.";
                    else
                        File.Delete(savefile.FileName);
                }
            }
            else
            {
                this.StatusMessage = "Please select file.";
                detailEntity = null;
            }
        }
        private bool ExportToCSVFormate(string filename)
        {
            StreamWriter file = new StreamWriter(filename);
            if (string.IsNullOrEmpty(detailEntity.ParameterColumn))
            {
                this.StatusMessage = "File does not cantain any parameter.";
                file.Close();
                return false;
            }
            DataSet dataSet = consumerExportSettingsBLL.GetParameterData(detailEntity.ParameterColumn);
            int endIndex = dataSet.Tables[0].Rows.Count - 2;
            for (int i = 0; i <= endIndex; i++)
            {
                if (Convert.ToString( dataSet.Tables[0].Rows[i]["Meter ID"] )== Convert.ToString( dataSet.Tables[0].Rows[i+1]["Meter ID"]))
                {
                    dataSet.Tables[0].Rows[i + 1].Delete();
                    dataSet.AcceptChanges();
                    i--;
                    endIndex--;
                }
            }

            if (dataSet == null)
            {
                this.StatusMessage = "No Data Found.";
                file.Close();
                return false;
            }
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                this.StatusMessage = "No Data Found.";
                file.Close();
                return false;
            }
            string data = string.Empty;
            int counter = 0;
            for (; counter < dataSet.Tables[0].Columns.Count; counter++)
                data = string.Concat(data , ",", dataSet.Tables[0].Columns[counter].ColumnName);
            
            if(data.Length>0)
                data = data.Substring(1);
                //data = data.Substring(0, data.Length - 1);
            file.WriteLine(data);
            long length = data.Length;
            for (counter = 0; counter < dataSet.Tables[0].Rows.Count; counter++)
            {
                data = string.Empty;
                for (int col = 0; col < dataSet.Tables[0].Columns.Count; col++)
                    data = string.Concat(data, ",", CheckData(dataSet.Tables[0].Rows[counter][col]));
                if (data.Length > 0)
                    data = data.Substring(1);
                    //data = data.Substring(0, data.Length - 1);
                file.WriteLine(data);
                // Solved DLMS_0094.
                length =length+ data.Length;
            }
            file.WriteLine(length.ToString());
            file.Close();
            return true;
        }

        public string CheckData(object obj)
        {
            string data=Convert.ToString(obj);
            if (string.IsNullOrEmpty(data))
                return string.Empty;
            else if (data.IndexOf(",") == -1)
                return data;
            else
                return "\"" + data + "\"";

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.StatusMessage = string.Empty;
        }  
    }
}
