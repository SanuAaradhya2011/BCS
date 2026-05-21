using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class ConsumerExportSettings : MdiChildForm
    {
        private ConsumerExportSettingsBLL consumerExportSettingsBLL = new ConsumerExportSettingsBLL();
        private ConsumerExportSettingsEntity detailEntity = null;
        const string METERID = "Meter ID";
        public ConsumerExportSettings()
        {
            InitializeComponent();
        }

        private void ConsumerExportSettings_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Text = "Consumer Export Settings";
            this.grpAdd.Visible = false;
            grpAdd.Location = new System.Drawing.Point(5, 5);
            this.grpList.Visible = true;
            grpList.Location = new System.Drawing.Point(5, 5);
            LoadAllParameter();
            LoadList();
        }

        private void LoadAllParameter()
        {
            listBoxAvailableMeters.Items.Clear();
            string[] data = consumerExportSettingsBLL.GetDisplayColumnName();
            for (int counter = 0; counter < data.Length; counter++)
                listBoxAvailableMeters.Items.Add(data[counter]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (detailEntity == null)
                detailEntity = new ConsumerExportSettingsEntity();
            string fileName = txtFileName.Text.Trim();

            if (string.IsNullOrEmpty(fileName))
            {
                this.StatusMessage = "File name can not be empty";
                txtFileName.Focus();
                return;
            }

            if (consumerExportSettingsBLL.IsValidFile(fileName) && detailEntity.ConsumerExportSettings_ID == 0)
            {
                this.StatusMessage = "File already exist.";
                txtFileName.Focus();
                return;
            }
            string paramName = string.Empty;
            string paramColumnName = string.Empty;
            // Added to solve the consumer info coming duplicate
            paramColumnName = string.Concat(paramColumnName, "B.Meter_ID as 'Meter ID', ");
            paramName = string.Concat(paramName, ",", "Meter ID");
            foreach (object item in listBoxAvailableMeters.Items)
            {
                // Added to solve the consumer info coming duplicate
                if (item.ToString().Trim() != METERID)
                {
                    paramName = string.Concat(paramName, ",", item.ToString());
                    paramColumnName = string.Concat(paramColumnName, consumerExportSettingsBLL.GetDBColumn(item.ToString()));
                }
            }
            paramName = paramName.Substring(1);
            // Added "distinct' to query to solve the consumer info coming duplicate
            //following query changed to resolve bug 73549; 12 April 2012
            paramColumnName = string.Concat("Select distinct ", paramColumnName.Substring(0, paramColumnName.Length - 1), " from consumer_master A inner join consumermeter C on A.Consumer_Number = C.Consumer_Number inner join meter_master B on C.Meter_ID = B.Meter_ID left outer join meterdata MD on MD.MeterID = B.Meter_ID left outer join meterdata_general MG on MG.MeterData_ID = MD.MeterData_ID left outer join region_master R on R.Region_ID = C.Region_ID left outer join circle_master CR on CR.Circle_ID = C.Circle_ID left outer join division_master D on D.Division_ID = C.Division_ID order by MD.UploadingDateTime desc");

            detailEntity.FileName = fileName;
            detailEntity.ParametersName = paramName;
            detailEntity.ParameterColumn = paramColumnName;

            if (detailEntity.ConsumerExportSettings_ID == 0)
            {
                consumerExportSettingsBLL.InsertData(detailEntity);
                this.StatusMessage = "File saved successfully.";
            }
            else
            {
                consumerExportSettingsBLL.UpdateData(detailEntity);
                this.StatusMessage = "File modified successfully.";
            }

            DataSet dataSet = consumerExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "ConsumerExportSettings_ID";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are you sure to delete this file", "Delete Export Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
                consumerExportSettingsBLL.DeleteSettings(Convert.ToInt32(fileId));
                lstParameter.Items.Clear();
                LoadList();
            }
            else
            {
                MessageBox.Show("No record to delete", "Delete Export Settings", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LoadAllParameter();
            detailEntity = null;
            txtFileName.Text = string.Empty;
            this.grpAdd.Visible = true;
            this.grpList.Visible = false;
            txtFileName.Enabled = true;
            grpAdd.Text = "New Consumer Export Settings";
            txtFileName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ConsumerExportSettings_Load(this, null);
        }

        private void lstFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstParameter.Items.Clear();
            int index = lstFile.SelectedIndex;
            string fileId = ((System.Data.DataRowView)(lstFile.Items[index])).Row.ItemArray[0].ToString();
            string[] data = consumerExportSettingsBLL.DetailData(Convert.ToInt32(fileId));
            for (int counter = 0; counter < data.Length; counter++)
                lstParameter.Items.Add(data[counter]);
        }

        private void lstParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void LoadList()
        {
            DataSet dataSet = consumerExportSettingsBLL.ListDataSet();
            lstFile.DataSource = dataSet.Tables[0];
            lstFile.DisplayMember = "FileName";
            lstFile.ValueMember = "ConsumerExportSettings_ID";
        }

        private void lstParameter_Enter(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void lstFile_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
    }
}