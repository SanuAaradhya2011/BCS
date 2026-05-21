using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CAB.Framework;

namespace CAB.UI
{
    public partial class frmGSMGrouping : MdiChildForm
    {
        public string Mode { get; set; }
        public GSMGroupEntity GSMGroup { get; set; }
        GSMGroupBLL gsmGroupBLL = null;
        public frmGSMGrouping()
        {
            gsmGroupBLL = new GSMGroupBLL();
            InitializeComponent();

            //if (UtilityDetails.PrimaryUtlityName == CAB.Framework.UtilityEntity.DLMS.ToString())
            //{
            //comboBoxCommType.Items.Add(CommunicationType.GPRS.ToString());
            //}

            //// gprs :Communication type ddl will have GPRS only when it is enabled for the utiltity

            //if (UtilityDetails.ShowGPRSCommunication)
            //{
            //    comboBoxCommType.Items.Add(CommunicationType.GPRS.ToString());
            //}

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GSMGroupEntity gsmGroupEntity = new GSMGroupEntity();

            if (ValidateData())
            {
                gsmGroupEntity.GroupName = txtGroupName.Text;
                gsmGroupEntity.SelectedMeterList = new List<string>();
                gsmGroupEntity.CommunicationType = comboBoxCommType.SelectedItem.ToString();

                foreach (object item in lstSelected.Items)
                    gsmGroupEntity.SelectedMeterList.Add(item.ToString());

                if (lstAvailable.Items.Count > 0)
                {
                    gsmGroupEntity.AvailableMeterList = new List<string>();
                    foreach (object item in lstAvailable.Items)
                        gsmGroupEntity.AvailableMeterList.Add(item.ToString());
                }

                if (rdMeterWise.Checked)
                    gsmGroupEntity.GroupType = "M";
                else if (rdAreaWise.Checked)
                {
                    gsmGroupEntity.GroupType = "A";
                    gsmGroupEntity.RegionID = Convert.ToInt32(cmbRegion.SelectedValue);
                    gsmGroupEntity.CircleID = Convert.ToInt32(cmbCircle.SelectedValue);
                    if (cmbDivision.SelectedIndex > -1)
                        gsmGroupEntity.DivisionID = Convert.ToInt32(cmbDivision.SelectedValue);
                }
                else
                    gsmGroupEntity.GroupType = "S";

                // do processing
                if (Mode == "Add")
                {
                    if (gsmGroupBLL.ValidateGroupName(gsmGroupEntity.GroupName))
                        CABMessageBox.ShowFilterMessage("M000107|M000011", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        GSMGroupEntity entity = (GSMGroupEntity)gsmGroupBLL.InsertData(gsmGroupEntity);
                        this.StatusMessage = "Data Saved SuccessFully.";
                        this.Close();
                    }
                }
                else if (Mode == "Edit" || Mode == string.Empty)
                {
                    if (GSMGroup != null)
                    {
                        gsmGroupEntity.GroupID = GSMGroup.GroupID;
                        gsmGroupBLL.UpdateData(gsmGroupEntity);
                        this.StatusMessage = "Data Saved SuccessFully.";
                        this.Close();
                    }
                }
            }
        }

        private bool ValidateData()
        {
            //check whether group name is empty,if yes raise error
            if (String.IsNullOrEmpty(txtGroupName.Text))
            {
                CABMessageBox.ShowFilterMessage("M000107|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGroupName.Focus();
                return false;
            }
            //check that group name shouldn't have special characters
            else if (!ValidationProvider.ValidateData(txtGroupName.Text, ValidationConstant.groupName))
            {
                CABMessageBox.ShowFilterMessage("M000007|M000107|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGroupName.Focus();
                return false;
            }
            //Check that meter number textbox shouldn't have special characters
            else if (!ValidationProvider.ValidateData(txtMeterNumber.Text, ValidationConstant.groupName))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000022|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterNumber.Focus();
                return false;
            }
            //Check that if Group Type is Area then Region and circle should be selected, if not raise error
            else if (rdAreaWise.Checked && (cmbRegion.SelectedIndex <= 0 || cmbCircle.SelectedIndex <= 0))
            {
                CABMessageBox.ShowFilterMessage("M000109", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (cmbRegion.SelectedIndex <= 0)
                {
                    cmbRegion.Focus();
                    return false;
                }
                if (cmbCircle.SelectedIndex <= 0)
                {
                    cmbCircle.Focus();
                    return false;
                }
            }
            //Check that if user has selected in meter in selected listbox , if havnt selected any raise error
            else if (lstSelected.Items.Count <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000110", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstAvailable.Focus();
                return false;
            }
            return true;
        }

        private void rdMeterwise_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMeterWise.Checked)
            {
                pnlArea.Enabled = false;
                pnlMeter.Enabled = true;
            }
            else
            {
                pnlArea.Enabled = true;
                pnlMeter.Enabled = false;
            }
        }

        private void rdAreaWise_CheckedChanged(object sender, EventArgs e)
        {
            if (rdAreaWise.Checked)
            {
                pnlArea.Enabled = true;
                pnlMeter.Enabled = false;
                InitializeConsumerArea();
            }
            else
            {
                pnlArea.Enabled = false;
                pnlMeter.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateMeterNumber()
        {
            ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();

            if (rdMeterWise.Checked && txtMeterNumber.Text.Length < 4)
            {
                CABMessageBox.ShowFilterMessage("M000111", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterNumber.Focus();
                return false;
            }

            else if (!ValidationProvider.ValidateData(txtMeterNumber.Text, ValidationConstant.groupName))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000022|M000012", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterNumber.Focus();
                return false;
            }

            //MangtanH : to solve bug : 208624 , the following lines have been commented. 
            //what is wanted is first four letters of meter id and all meter id's with similar begining will be added.
            //but this check was checking for complete meter id , so partial searching was not working.

            //else if (!consumerMeterBLL.IsMeterExists(txtMeterNumber.Text.Trim(),comboBoxCommType.SelectedItem.ToString()))
            //{
            //    CABMessageBox.ShowFilterMessage("M000117", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtMeterNumber.Focus();
            //    return false;

            //}

            return true;
        }
        // <-<-
        private void button1_Click(object sender, EventArgs e)
        {
            // Fix by Swati
            bool flagMeter = true;
            string items = string.Empty;
            if (lstSelected.Items.Count == 0)
                return;

            foreach (object item in lstSelected.Items)
            {
                for (int i = 0; i < lstAvailable.Items.Count; i++)
                {
                    //Check for any duplicate meter in lstAvailable
                    if (item.ToString() == lstAvailable.Items[i].ToString())
                    {
                        items += item + "\n";
                    }
                }
            }
            string[] itemsToRemove = items.Split(new string[] { "\n" }, StringSplitOptions.None);
            foreach (string str in itemsToRemove)
            {
                if (!string.IsNullOrEmpty(str))
                    lstAvailable.Items.Remove(str);
            }
            foreach (object item in lstSelected.Items)
            {
                lstAvailable.Items.Add(item);
            }
            lstSelected.Items.Clear();

        }
        // <-
        private void button2_Click(object sender, EventArgs e)
        {
            // Fix by Swati
            //// Added to remove duplicacy.
            bool flagMeter = true;

            if (lstSelected.Items.Count == 0)
                return;

            foreach (object item in lstSelected.SelectedItems)
            {
                for (int i = 0; i < lstAvailable.Items.Count; i++)
                {
                    //Check for any duplicate meter in lstAvailable
                    if (item.ToString() == lstAvailable.Items[i].ToString())
                    {
                        lstAvailable.Items.Remove(item);
                        flagMeter = true;
                        break;
                    }
                    else
                        flagMeter = true;
                }
                //add meter in available list
                if (flagMeter)
                {
                    lstAvailable.Items.Add(item);
                }
            }

            //if (flagMeter)
            //    lstSelected.Items.Clear();

            if (flagMeter)
            {
                for (int SelectedIndex = lstSelected.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--)
                    lstSelected.Items.RemoveAt(lstSelected.SelectedIndex);
                if (lstSelected.Items.Count != 0)
                {
                    lstSelected.SelectedIndex = 0;
                    lstAvailable.SelectedIndex = -1;
                }
            }

        }
        // ->
        private void button4_Click(object sender, EventArgs e)
        {
            bool flagMeter = true;

            foreach (object item in lstAvailable.SelectedItems)
            {
                for (int i = 0; i < lstSelected.Items.Count; i++)
                {
                    //Check for any duplicate meter in lstSelected
                    if (item.ToString() == lstSelected.Items[i].ToString())
                    {
                        //MessageBox.Show("Meter already exist in the Selected List.");
                        flagMeter = false;
                        break;
                    }
                    else
                        flagMeter = true;
                }
                //add meter in selected list
                if (flagMeter)
                    lstSelected.Items.Add(item);
            }

            if (flagMeter)
            {
                for (int SelectedIndex = lstAvailable.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--)
                    lstAvailable.Items.RemoveAt(lstAvailable.SelectedIndex);

                if (lstAvailable.Items.Count != 0)
                {
                    lstAvailable.SelectedIndex = 0;
                    lstSelected.SelectedIndex = -1;
                }
            }
        }
        // ->->
        private void button3_Click(object sender, EventArgs e)
        {
            bool flagMeter = true;

            if (lstAvailable.Items.Count == 0)
                return;

            foreach (object item in lstAvailable.Items)
            {
                for (int i = 0; i < lstSelected.Items.Count; i++)
                {
                    //Check for any duplicate meter in lstSelected
                    if (item.ToString() == lstSelected.Items[i].ToString())
                    {
                        //MessageBox.Show("Meter already exist in the Selected List.");
                        flagMeter = false;
                        break;
                    }
                    else
                        flagMeter = true;
                }
                //add meter in selected list
                if (flagMeter)
                    lstSelected.Items.Add(item);
            }

            if (flagMeter)
                lstAvailable.Items.Clear();
        }

        private void frmGSMGrouping_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Mode))
                if (Mode == "Edit")
                {
                    if (GSMGroup != null)
                    {
                        lstAvailable.Items.Clear();
                        lstSelected.Items.Clear();
                        txtGroupName.Text = GSMGroup.GroupName;
                        txtGroupName.Enabled = false;

                        if (GSMGroup.AvailableMeterList.Count > 0)
                            foreach (string avameterID in GSMGroup.AvailableMeterList)
                            {
                                lstAvailable.Items.Add(avameterID);
                            }
                        if (GSMGroup.SelectedMeterList.Count > 0)
                            foreach (string selmeterID in GSMGroup.SelectedMeterList)
                            {
                                lstSelected.Items.Add(selmeterID);
                            }
                        // Set mode to empty as it will prevent circle,division selected item index changed
                        // to populate the available meter list.
                        Mode = string.Empty;
                        if (GSMGroup.GroupType == "A")
                        {
                            rdAreaWise.Checked = true;
                            InitializeAreaDropdowns();
                        }
                        else if (GSMGroup.GroupType == "M")
                        {
                            rdMeterWise.Checked = true;
                        }
                        else if (GSMGroup.GroupType == "S")
                        {
                            rdSelectAll.Checked = true;
                        }
                        // set mode to its original value
                        Mode = "Edit";
                    }
                }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string meterIDsPresent = string.Empty;
            if (ValidateMeterNumber())
            {
                //lstAvailable.Items.Add(txtMeterNumber.SelectedItem);                       
                DataSet dataSet = null;
                GSMGroupBLL groupBLL = new GSMGroupBLL();
                //BhardwajG Pass communication type to the function
                dataSet = groupBLL.ListMeterSimNumbers(txtMeterNumber.Text, comboBoxCommType.SelectedItem.ToString());
                if (dataSet != null)
                    if (dataSet.Tables.Count > 0)
                    {
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            if ((!lstAvailable.Items.Contains(row["Meter_ID"].ToString())) && (!lstSelected.Items.Contains(row["Meter_ID"].ToString())))
                                lstAvailable.Items.Add(row["Meter_ID"].ToString());
                            else
                                meterIDsPresent = meterIDsPresent + row["Meter_ID"].ToString() + ",";
                        }
                        if (!string.IsNullOrEmpty(meterIDsPresent))
                            MessageBox.Show(meterIDsPresent.Substring(0, meterIDsPresent.Length - 1) + " already exists in either available or selected meters", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        meterIDsPresent = string.Empty;
                    }
            }
        }

        private void rdSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdSelectAll.Checked)
            {
                pnlMeter.Enabled = false;
                pnlArea.Enabled = false;
                ConsumerMeterBLL consumerBLL = new ConsumerMeterBLL();
                DataSet dataSet = consumerBLL.GetActiveMeterList();
                if (dataSet != null)
                    if (dataSet.Tables.Count > 0)
                    {
                        lstAvailable.Items.Clear();
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            lstAvailable.Items.Add(row["Meter_ID"].ToString());
                        }
                    }
            }
            else
            {
                pnlMeter.Enabled = true;
                pnlArea.Enabled = true;
                lstAvailable.Items.Clear();
            }
        }

        private void lstSelected_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void InitializeConsumerArea()
        {
            DataSet regionData = null;
            RegionBLL regionBLL = new RegionBLL();
            regionData = regionBLL.ListDataSet();
            if (regionData != null && regionData.Tables.Count > 0)
            {
                DataTable table = regionData.Tables[0];
                DataRow row = table.NewRow();
                row["Region Name"] = "";
                row["Region_ID"] = -1;
                table.Rows.InsertAt(row, 0);
                cmbRegion.DataSource = table;
                cmbRegion.DisplayMember = "Region Name";
                cmbRegion.ValueMember = "Region_ID";
            }
        }

        private void InitializeAreaDropdowns()
        {
            if (GSMGroup != null)
            {
                InitializeConsumerArea();
                cmbRegion.SelectedValue = GSMGroup.RegionID;
                InitializeCircle(GSMGroup.RegionID);
                cmbCircle.SelectedValue = GSMGroup.CircleID;
                InitializeDivision(GSMGroup.CircleID);
                cmbDivision.SelectedValue = GSMGroup.DivisionID;
            }
        }

        private void InitializeCircle(int region_ID)
        {
            CircleBLL circleBLL = new CircleBLL();
            DataSet dataSet = circleBLL.GetCircleDetailData(region_ID);

            if (dataSet != null)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    DataRow row = table.NewRow();
                    row["Circle_Name"] = "";
                    row["Circle_ID"] = -1;
                    table.Rows.InsertAt(row, 0);
                    cmbCircle.DataSource = table;
                    cmbCircle.DisplayMember = "Circle_Name";
                    cmbCircle.ValueMember = "Circle_ID";
                }
                else
                    cmbCircle.DataSource = null;
            }
            else
                cmbCircle.DataSource = null;
        }

        private void InitializeDivision(int circleID)
        {
            DivisionBLL divisionBLL = new DivisionBLL();
            DataSet dataSet = divisionBLL.GetDivisionDataByCircleID(circleID);
            if (dataSet != null)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    DataRow row = table.NewRow();
                    row["Division_Name"] = "";
                    row["Division_ID"] = -1;
                    table.Rows.InsertAt(row, 0);
                    cmbDivision.DataSource = table;
                    cmbDivision.DisplayMember = "Division_Name";
                    cmbDivision.ValueMember = "Division_ID";
                }
                else
                    cmbDivision.DataSource = null;
            }
            else
                cmbDivision.DataSource = null;
        }

        private void cmbRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regionID = 0;
            if (cmbRegion.SelectedValue != null)
            {
                if (int.TryParse(cmbRegion.SelectedValue.ToString(), out regionID))
                    InitializeCircle(regionID);
            }
        }

        private void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int circleID = 0;
            if (Mode != string.Empty)
            {
                if (cmbCircle.SelectedValue != null)
                {
                    if (int.TryParse(cmbCircle.SelectedValue.ToString(), out circleID))
                    {
                        InitializeDivision(circleID);
                    }
                }
            }
        }

        private void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Mode != string.Empty && cmbDivision.SelectedIndex > -1)
            {
                int divisionID = 0;
                if (cmbDivision.SelectedValue != null)
                {
                    if (int.TryParse(cmbDivision.SelectedValue.ToString(), out divisionID))
                    {
                        ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
                        //BhardwajG Pass communication type to the function so that meters can be fetched communication type wise 
                        DataSet dataSet = consumerMeterBLL.GetMetersbyDivisionID(divisionID, comboBoxCommType.SelectedItem.ToString());
                        if (dataSet != null)
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                lstAvailable.Items.Clear();
                                foreach (DataRow row in dataSet.Tables[0].Rows)
                                    lstAvailable.Items.Add(row["Meter_ID"].ToString());
                            }
                        }
                    }
                }
            }
        }

        private void frmGSMGrouping_Activated(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// event to take care of available meter population on change of communication type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCommType_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (comboBoxCommType.SelectedItem.ToString().ToUpper() == CommunicationType.GPRS.ToString())
            {
                MessageBox.Show("GPRS is applicable for DLMS meters only !!!");                
            }
            if (Mode != "Edit")
            {
                lstAvailable.Items.Clear();
                lstSelected.Items.Clear();

                if (rdAreaWise.Checked && cmbDivision.SelectedIndex >= 0)
                {
                    int divisionID = 0;
                    if (cmbDivision.SelectedValue != null)
                    {
                        if (int.TryParse(cmbDivision.SelectedValue.ToString(), out divisionID))
                        {
                            ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
                            DataSet dataSet = null;
                            if (Mode.Equals("Edit") && GSMGroup != null)
                            {
                                dataSet = consumerMeterBLL.GetMetersbyDivisionID(GSMGroup.GroupID, divisionID, GSMGroup.CommunicationType);
                            }
                            else
                            {
                                dataSet = consumerMeterBLL.GetMetersbyDivisionID(divisionID, comboBoxCommType.SelectedItem.ToString());
                            }
                            if (dataSet != null)
                            {
                                if (dataSet.Tables[0].Rows.Count > 0)
                                {
                                    lstAvailable.Items.Clear();
                                    foreach (DataRow row in dataSet.Tables[0].Rows)
                                        lstAvailable.Items.Add(row["Meter_ID"].ToString());
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// // gprs :shadowed the show method of base class 
        /// </summary>
        public new void Show()
        {
            base.Show();
            if (Mode == "Edit")
            {
                comboBoxCommType.SelectedItem = GSMGroup.CommunicationType.ToString();
                comboBoxCommType.Enabled = false;
            }
            else
            {
                comboBoxCommType.SelectedItem = CommunicationType.GSM.ToString();
            }
        }

    }
}