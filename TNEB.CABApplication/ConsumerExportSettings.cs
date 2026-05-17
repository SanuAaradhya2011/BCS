using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.UI.Controls;
using CAB.Entity;

namespace CAB.UI
{
    public partial class ConsumerExportSettings : MdiChildForm
    {
        private ConsumerExportSettingsBLL consumerExportSettingsBLL = new ConsumerExportSettingsBLL();

        private ConsumerExportSettingsEntity detailEntity = null;

        public ConsumerExportSettings()
        {
            InitializeComponent();
        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            int idx = listBoxAvailableMeters.SelectedIndex;
            foreach (object item in listBoxAvailableMeters.SelectedItems)
                listBoxSelectedMeters.Items.Add(item);
            for (int counter = 0; counter < listBoxSelectedMeters.Items.Count; counter++)
            { 
                int i = 0;
                bool Flag=false;
                for (; i < listBoxAvailableMeters.Items.Count; i++)
                {
                    if (listBoxAvailableMeters.Items[i].ToString().Equals(listBoxSelectedMeters.Items[counter].ToString()))
                    {
                        Flag = true;
                        break;
                    }
                }
                if (Flag)
                listBoxAvailableMeters.Items.RemoveAt(i);
            }
            if(idx!=0)
            idx -= 1;
            if (listBoxAvailableMeters.Items.Count >= idx && listBoxAvailableMeters.Items.Count != 0 && idx > 0)
                listBoxAvailableMeters.SelectedIndex = idx;
            else
            {
                if (listBoxAvailableMeters.Items.Count > 0)
                    listBoxAvailableMeters.SelectedIndex = 0;
            }
        }

        private void btnMoveNextAll_Click(object sender, EventArgs e)
        {
            if (listBoxAvailableMeters.Items.Count == 0)
                return;
            foreach (object item in listBoxAvailableMeters.Items)
                listBoxSelectedMeters.Items.Add(item);
            listBoxAvailableMeters.Items.Clear();
        }

        private void btnMoveBack_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxSelectedMeters.SelectedItems) 
                listBoxAvailableMeters.Items.Add(item); 
            for (int SelectedIndex = listBoxSelectedMeters.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--) 
                listBoxSelectedMeters.Items.RemoveAt(listBoxSelectedMeters.SelectedIndex); 
            if (listBoxSelectedMeters.Items.Count != 0)
            {
                listBoxSelectedMeters.SelectedIndex = 0;
                listBoxAvailableMeters.SelectedIndex = -1;
            }
        }

        private void btnMoveBackAll_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedMeters.Items.Count == 0) 
                return; 
            foreach (object item in listBoxSelectedMeters.Items) 
                listBoxAvailableMeters.Items.Add(item); 
            listBoxSelectedMeters.Items.Clear();
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
            if(detailEntity==null)
            detailEntity = new ConsumerExportSettingsEntity();
            string fileName=txtFileName.Text.Trim();
            if(string.IsNullOrEmpty(fileName))
            {
                this.StatusMessage = "File name can not be empty";
                txtFileName.Focus();
                return;
            }
            if (listBoxSelectedMeters.Items.Count == 0)
            {
                this.StatusMessage = "Please select Parameter";
                listBoxAvailableMeters.Focus();
                return;
            }
            if (consumerExportSettingsBLL.IsValidFile(fileName) && detailEntity.ConsumerExportSettings_ID==0)
            {
                this.StatusMessage = "File already exist.";
                txtFileName.Focus();
                return;
            }
            string paramName = string.Empty;
            string paramColumnName = string.Empty;
            foreach (object item in listBoxSelectedMeters.Items)
            {
                paramName = string.Concat(item.ToString(), ",", paramName);
                paramColumnName = string.Concat(consumerExportSettingsBLL.GetDBColumn(item.ToString()), paramColumnName);
            }
            if (listBoxAvailableMeters.Items.Count != 0 && listBoxSelectedMeters.Items.Count!=27)
            {
                this.StatusMessage = "Please select all parameter.";
                listBoxSelectedMeters.Focus();
                return;
            }
            //if (paramName.IndexOf("Meter ID") < 0)
            //{
            //    this.StatusMessage = "Please add Meter ID.";
            //    listBoxSelectedMeters.Focus();
            //    return;
            //}
            //if (paramName.IndexOf("Consumer ID") < 0)
            //{
            //    this.StatusMessage = "Please add Consumer ID.";
            //    listBoxSelectedMeters.Focus();
            //    return;
            //}
            paramColumnName = string.Concat("Select ", paramColumnName.Substring(0, paramColumnName.Length - 1)," from consumer_master A,meter_master B,consumermeter C where A.Consumer_Number=C.Consumer_Number and C.Meter_ID =B.Meter_ID");
            detailEntity.FileName = fileName;
            paramName = paramName.Substring(0, paramName.Length - 1);
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
            listBoxSelectedMeters.Items.Clear();
            this.grpAdd.Visible =true ; 
            this.grpList.Visible = false;
            txtFileName.Enabled = true; 
            grpAdd.Text= "New Consumer Export Settings";
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            grpAdd.Text = "Edit Consumer Export Settings";
            LoadAllParameter();
            if (lstFile.SelectedIndex != -1)
            {
                string fileId = ((System.Data.DataRowView)(lstFile.Items[lstFile.SelectedIndex])).Row.ItemArray[0].ToString();
                detailEntity = consumerExportSettingsBLL.DetailData(fileId) as ConsumerExportSettingsEntity;
                if (detailEntity != null)
                {
                    listBoxSelectedMeters.Items.Clear();
                    txtFileName.Text = detailEntity.FileName;
                    this.grpAdd.Visible = true;
                    this.grpList.Visible = false;
                    string[] data = null;
                    if (!string.IsNullOrEmpty(detailEntity.ParametersName))
                        data = detailEntity.ParametersName.Split(',');
                    for (int counter = 0; counter < data.Length; counter++)
                    {
                        listBoxSelectedMeters.Items.Add(data[counter]);
                        int i = 0;
                        for (; i < listBoxAvailableMeters.Items.Count; i++)
                        {
                            if (listBoxAvailableMeters.Items[i].ToString().Equals(data[counter]))
                                break;
                        }
                        listBoxAvailableMeters.Items.RemoveAt(i);
                    }
                    txtFileName.Enabled = false;
                }
            }
            else
            {
                detailEntity = null;
                MessageBox.Show("No record to be edited", "Edit Export Settings", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
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
