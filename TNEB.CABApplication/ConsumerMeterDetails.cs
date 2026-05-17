/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
	public partial class ConsumerMeterDetails : MdiChildForm
	{

		private ConsumerMasterBLL consumerMasterBLL=new ConsumerMasterBLL();
		private ConsumerMeterBLL consumerMeterBLL=new ConsumerMeterBLL();
		private MeterMasterBLL meterMasterBLL=new MeterMasterBLL();
		private SuspectedConsumerBLL suspectedConsumerBLL=new SuspectedConsumerBLL(); 
		private ConsumerMasterEntity consumerMasterEntity=new ConsumerMasterEntity();
		private ConsumerMeterEntity consumerMeterEntity=new ConsumerMeterEntity();
		private MeterMasterEntity meterMasterEntity=new MeterMasterEntity();
		private SuspectedConsumerEntity suspectedConsumerEntity=new SuspectedConsumerEntity(); 
		MainForm mainForm = (MainForm)Application.OpenForms["MainForm"]; 
		double contractDemand = 0; 
		int ctPrimary, ctSecondary;
        string phone = "";
		/// <summary>
		/// variable to check whether the enter new meter details button is clicked or not.
		/// </summary>
		private bool isEnterNewMeterDetails = false; 
		private bool isNewMode = false;
		private bool isEditMode = false;
		private bool isFreeConsumerMode = false;

        public string MeterID { get; set; }
        public string ConsumerID { get; set; }
        public string Mode { get; set; }
		public ConsumerMeterDetails()
		{ 
			InitializeComponent(); 
		}  
        private void ConsumerMeterDetails_Load(object sender, EventArgs e)
        {
            DataSet consumerTypeDataSet = new SearchControlBLL().GetFilterData("consumertype_master|ConsumerType_Name|ConsumerType_ID");
            if (consumerTypeDataSet.Tables[0].Rows.Count > 0)
            {
                cboConsumerType.DisplayMember = "DisplayMember";
                cboConsumerType.ValueMember = "ValueMember";
                cboConsumerType.DataSource = consumerTypeDataSet.Tables[0];
            }

            DataSet typeDataSet = new SearchControlBLL().GetFilterData("metertype_master|MeterType_Name|MeterType_ID");
            if (typeDataSet.Tables[0].Rows.Count > 0)
            {
                cboMeterType.DisplayMember = "DisplayMember";
                cboMeterType.ValueMember = "ValueMember";
                cboMeterType.DataSource = typeDataSet.Tables[0];
            }

            DataSet modelDataSet = new SearchControlBLL().GetFilterData("metermodel_master|MeterModel_Name|MeterModel_ID");
            if (modelDataSet.Tables[0].Rows.Count > 0)
            {
                cboMeterModel.DisplayMember = "DisplayMember";
                cboMeterModel.ValueMember = "ValueMember";
                cboMeterModel.DataSource = modelDataSet.Tables[0];
            }

            DataSet unitDataSet = new SearchControlBLL().GetFilterData("meterunit_master|MeterUnit_Type|MeterUnit_ID");
            if (unitDataSet.Tables[0].Rows.Count > 0)
            {
                cboUnits.DisplayMember = "DisplayMember";
                cboUnits.ValueMember = "ValueMember";
                cboUnits.DataSource = unitDataSet.Tables[0];
            }
            if (Mode.Equals("Add"))
            {
                groupBox2.Enabled = false;
                consumerMasterEntity = new ConsumerMasterEntity();
                consumerMeterEntity = new ConsumerMeterEntity();
                meterMasterEntity = new MeterMasterEntity();
                suspectedConsumerEntity = new SuspectedConsumerEntity();
            }
            if (Mode.Equals("Edit"))
            {
                btnSave.Text = "Update"; 
                cboMeterNumber.DropDownStyle = ComboBoxStyle.DropDownList;
                txtConsumerNumber.Enabled = false;
                DataSet meterNumberDset = consumerMeterBLL.GetActiveMeterID(ConsumerID);
                if ((meterNumberDset != null) && (meterNumberDset.Tables.Count != 0) && (meterNumberDset.Tables[0].Rows.Count != 0))
                {
                    foreach (DataRow drow in meterNumberDset.Tables[0].Rows)
                    {
                        cboMeterNumber.Items.Add(drow[0].ToString());
                    }
                }
                consumerMasterEntity = consumerMasterBLL.GetDetailData(ConsumerID) as ConsumerMasterEntity;
                meterMasterEntity = meterMasterBLL.GetDetailData(MeterID, 1) as MeterMasterEntity;
                if (meterMasterEntity == null)
                    meterMasterEntity = new MeterMasterEntity();
                txtConsumerNumber.Text = ConsumerID;
                consumerMeterEntity = consumerMeterBLL.GetDetailData(ConsumerID, MeterID) as ConsumerMeterEntity;
                bool isSuspected = suspectedConsumerBLL.IsConsumerSuspected(ConsumerID);
                suspectedConsumerEntity = new SuspectedConsumerEntity();
                DisplayEntityValues(meterMasterEntity);
                DisplayEntityValues(consumerMasterEntity);
                DisplayEntityValues(consumerMeterEntity);
                DisplayEntityValues(isSuspected); 
            }
            if (Mode.Equals("FreeConsumer"))
            { 
                groupBox2.Enabled = false;
                this.txtConsumerNumber.ReadOnly = true;
                consumerMasterEntity = consumerMasterBLL.GetDetailData(ConsumerID) as ConsumerMasterEntity;
                bool isSuspectedConsumer = suspectedConsumerBLL.IsConsumerSuspected(ConsumerID);
                DisplayEntityValues(consumerMasterEntity);
                DisplayEntityValues(isSuspectedConsumer); 
            } 
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!(ValidateData()))
            {
                return;
            } 
            consumerMasterEntity.Consumer_Number = this.txtConsumerNumber.Text.Trim();
            consumerMasterEntity.Consumer_Name = this.txtConsumerName.Text.Trim();
            consumerMasterEntity.ConsumerType_ID = Convert.ToInt16(cboConsumerType.SelectedValue);
            consumerMasterEntity.Consumer_Phone = txtConsumerTelephone.Text.Trim();
            consumerMasterEntity.Consumer_HNumber = txtConsumerHouseNo.Text.Trim();
            consumerMasterEntity.Consumer_Street = txtConsumerStreet.Text.Trim();
            consumerMasterEntity.Consumer_City = txtConsumerCity.Text.Trim();
            consumerMasterEntity.Consumer_Email = txtConsumerEmail.Text.Trim();

            suspectedConsumerEntity.Consumer_Number = this.txtConsumerNumber.Text.Trim();
            suspectedConsumerEntity.SuspectionStartDate = DateUtility.DateTimeToLong(DateTime.Now);
            suspectedConsumerEntity.SuspectionEndDate = DateUtility.DateTimeToLong(DateTime.Now);
 
                meterMasterEntity.Meter_ID = cboMeterNumber.Text.Trim(); 
            meterMasterEntity.MeterType_ID = Convert.ToInt16(cboMeterType.SelectedValue);
            meterMasterEntity.MeterModel_ID = Convert.ToInt16(cboMeterModel.SelectedValue);
            meterMasterEntity.Meter_EMF = Convert.ToInt32(txtEMF.Text.Trim());
            meterMasterEntity.Meter_Phone = txtBoxMeterSIM.Text.Trim();
           
            if (double.TryParse(txtContractDemand.Text, out contractDemand))
                meterMasterEntity.Meter_ContractDemand = contractDemand;
            else
                meterMasterEntity.Meter_ContractDemand = Convert.ToDouble("0");

            meterMasterEntity.MeterUnit_ID = Convert.ToInt16(cboUnits.SelectedValue);
            meterMasterEntity.Meter_CTPrimary = Convert.ToInt32(txtMeterCTPrimary.Text.Trim());
            meterMasterEntity.Meter_CTSecondary = Convert.ToInt32(txtMeterCTSecondary.Text.Trim());
            meterMasterEntity.Meter_PTPrimary = Convert.ToInt32(txtMeterPTPrimary.Text.Trim());
            meterMasterEntity.Meter_PTSecondary = Convert.ToInt32(txtMeterPTSecondary.Text.Trim());
            meterMasterEntity.Meter_InstalledCTPrimary = Convert.ToInt32(txtInstalledCTPrimary.Text.Trim());
            meterMasterEntity.Meter_InstalledCTSecondary = Convert.ToInt32(txtInstalledCTSecondary.Text.Trim());
            meterMasterEntity.Meter_InstalledPTPrimary = Convert.ToInt32(txtInstalledPTPrimary.Text.Trim());
            meterMasterEntity.Meter_InstalledPTSecondary = Convert.ToInt32(txtInstalledPTSecondary.Text.Trim());

            if (chkActivateMeter.Checked == true)
            {
                meterMasterEntity.Meter_Status = 1;
                consumerMeterEntity.Status = 1;
            }
            else
            {
                meterMasterEntity.Meter_Status = 0;
                consumerMeterEntity.Status = 0;
            }

            consumerMeterEntity.Consumer_Number = this.txtConsumerNumber.Text.Trim();
            consumerMeterEntity.Meter_ID = this.cboMeterNumber.Text.Trim();
            consumerMeterEntity.Meter_AllocationDate = DateUtility.DateTimeToLong(DtpInstalled.Value);
            consumerMeterEntity.Meter_Location = this.txtMeterLocation.Text.Trim();
            //if (string.IsNullOrEmpty(consumerMeterEntity.Meter_Location))
            //    consumerMeterEntity.Meter_Location = "-";

            suspectedConsumerEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
            if (Mode.Equals("Edit"))//OK
            {

                if (isEnterNewMeterDetails)
                {
                    if (!(meterMasterBLL.ValidateMeterNumber(meterMasterEntity)))
                    {
                        consumerMasterBLL.UpdateData(consumerMasterEntity);
                        meterMasterBLL.InsertData(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                        SaveSuspectedConsumer();
                        MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        consumerMasterBLL.UpdateData(consumerMasterEntity);
                        meterMasterBLL.UpdateData(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                        SaveSuspectedConsumer();
                        MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    consumerMasterBLL.UpdateData(consumerMasterEntity);
                    if (meterMasterBLL.ValidateMeterNumber(meterMasterEntity))
                        meterMasterBLL.UpdateData(meterMasterEntity);
                    else
                        meterMasterBLL.InsertData(meterMasterEntity);
                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                    SaveSuspectedConsumer();
                    MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearMeterDetails();
                    cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
                    cboMeterNumber.Text = "";
                    groupBox2.Enabled = false;
                }
                this.Close();
            }
            if (Mode.Equals("Add"))
            {
                if (isEnterNewMeterDetails && chkShowMeterDeactive.Checked)//OK
                {
                    consumerMasterBLL.InsertData(consumerMasterEntity);
                    if (meterMasterBLL.ValidateMeterNumber(meterMasterEntity))
                        meterMasterBLL.UpdateData(meterMasterEntity);
                    else
                        meterMasterBLL.InsertData(meterMasterEntity);
                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                    SaveSuspectedConsumer();
                    MessageBox.Show("Data saved Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearMeterDetails();
                    cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
                    cboMeterNumber.Text = "";
                    groupBox2.Enabled = false;
                }
                else if (isEnterNewMeterDetails && !chkShowMeterDeactive.Checked)//OK
                {
                    if (!(meterMasterBLL.ValidateMeterNumber(meterMasterEntity)))
                    {
                        consumerMasterBLL.InsertData(consumerMasterEntity);
                        meterMasterBLL.InsertData(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);//InsertData
                        SaveSuspectedConsumer();
                        MessageBox.Show("Data saved Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearMeterDetails();
                        groupBox2.Enabled = false;
                    }
                    else
                    {
                        mainForm.toolStripStatusLabel.Text = "Meter already available.";
                        return;
                    }
                }
                this.Close();
            }
            if (Mode.Equals("FreeConsumer"))
            {
                if (isEnterNewMeterDetails && chkShowMeterDeactive.Checked)
                {
                    consumerMasterBLL.UpdateData(consumerMasterEntity);
                    if (meterMasterBLL.ValidateMeterNumber(meterMasterEntity))
                        meterMasterBLL.UpdateData(meterMasterEntity);
                    else
                        meterMasterBLL.InsertData(meterMasterEntity);
                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    consumerMeterBLL.InsertUpdateData(consumerMeterEntity);//InsertData
                    SaveSuspectedConsumer();
                    mainForm.toolStripStatusLabel.Text = "Data Updated Successfully";
                }
                else if (isEnterNewMeterDetails && !chkShowMeterDeactive.Checked)
                {
                    if (!(meterMasterBLL.ValidateMeterNumber(meterMasterEntity)))
                    {
                        consumerMasterBLL.UpdateData(consumerMasterEntity);
                        meterMasterBLL.InsertData(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        if (consumerMeterBLL.GetConsumerAvailable(consumerMeterEntity.Consumer_Number))
                         consumerMeterBLL.DeleteData(consumerMeterEntity.Consumer_Number);
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                        SaveSuspectedConsumer();
                        mainForm.toolStripStatusLabel.Text = "Data Updated Successfully";
                        ClearMeterDetails();
                        groupBox2.Enabled = false;
                    }
                    else
                    {
                        mainForm.toolStripStatusLabel.Text = "MeterID Already Available";
                        return;
                    }
                }
            } 
            if (this.Text.Contains("New") && (this.txtConsumerNumber.Text != ""))
            {
                this.txtConsumerNumber.ReadOnly = true;
            } 
        }

        private void SaveSuspectedConsumer()
        { 
            bool suspected = suspectedConsumerBLL.IsConsumerSuspected(suspectedConsumerEntity.Consumer_Number);
            if (chkSuspectedConsumer.Checked == true)
            {
                if (suspected)
                    suspectedConsumerBLL.DeleteData(suspectedConsumerEntity);
                else
                    suspectedConsumerBLL.InsertData(suspectedConsumerEntity);
            }
            else
            {
                if (suspected)
                    suspectedConsumerBLL.DeleteData(suspectedConsumerEntity);
            } 
        } 
		private void btnEnterNewMeterDetails_Click(object sender, EventArgs e)
		{ 
			mainForm.toolStripStatusLabel.Text = "";
			ClearMeterDetails();
			isEnterNewMeterDetails = true;
			groupBox2.Enabled = true; 
			cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
			if (isEditMode == true) 
				cboMeterNumber.Enabled = true; 
			if (chkActivateMeter.Checked == true)
			{
				chkActivateMeter.Enabled = false;
				chkActivateMeter.Checked = true;
			}
			else
			{
				chkActivateMeter.Enabled = true;
				chkActivateMeter.Checked = false;
			}
			cboMeterNumber.Text = "";
		}

        private void chkShowMeterDeactive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowMeterDeactive.Checked)
            {
                DisplayDeactiveMeters();
                if (cboMeterNumber.Items.Count > 0)
                {
                    chkActivateMeter.Checked = true;
                    chkActivateMeter.Enabled = false;
                }
            }
            else
            {
                ClearMeterDetails();
                string consumer_Number = txtConsumerNumber.Text.Trim();
                DisplayActiveMeters(consumer_Number);
                if (cboMeterNumber.Items.Count <= 0)
                {
                    cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
                    chkActivateMeter.Checked = true;
                    chkActivateMeter.Enabled = false;
                }
                else
                    chkActivateMeter.Enabled = true;
            }
        }
 
		void DisplayActiveMeters(string consumer_Number)
		{
		 DataSet dataSet = new DataSet();
			if (consumer_Number != "")
			{
				dataSet = consumerMeterBLL.GetActiveMeterID(consumer_Number);
				if (dataSet != null)
				{
					if (dataSet.Tables.Count > 0)
					{
                        cboMeterNumber.Items.Clear();
                        cboMeterNumber.DropDownStyle = ComboBoxStyle.DropDownList;
						foreach(DataRow dr in dataSet.Tables[0].Rows) 
                            cboMeterNumber.Items.Add(Convert.ToString(dr[0])); 
					}
					else
					{
						ClearMeterDetails();
						cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
						cboMeterNumber.DataSource = null;
					}
				}
			}
			else
			{
				cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
				cboMeterNumber.Text = string.Empty;
			}
		}
 
		void DisplayDeactiveMeters()
		{
            DataSet dataSet = consumerMeterBLL.GetInactiveMeterID();
            if (dataSet == null)
                return;
            if (dataSet.Tables.Count == 0)
                return; 
            cboMeterNumber.Items.Clear();
            cboMeterNumber.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (DataRow dr in dataSet.Tables[0].Rows)
                cboMeterNumber.Items.Add(Convert.ToString(dr[0]));  
		}
 
		private void cboMeterNumber_SelectedIndexChanged(object sender, EventArgs e)
		{
            try
            {
                mainForm.toolStripStatusLabel.Text = "";
                if (cboMeterNumber.Text.Trim() != "")
                {
                    string meterId = cboMeterNumber.Text.Trim();
                    string consumerNumber = txtConsumerNumber.Text.Trim();
                    meterMasterEntity = null;
                    consumerMeterEntity = null;
                    if (chkShowMeterDeactive.Checked)
                        meterMasterEntity = meterMasterBLL.GetDetailData(meterId, 0) as MeterMasterEntity;
                    else
                        meterMasterEntity = meterMasterBLL.GetDetailData(meterId, 1) as MeterMasterEntity;
                    consumerMeterEntity = consumerMeterBLL.GetDetailData(consumerNumber, meterId) as ConsumerMeterEntity;
                    if (meterMasterEntity == null)
                        meterMasterEntity = new MeterMasterEntity();
                    if (consumerMeterEntity == null)
                        consumerMeterEntity = new ConsumerMeterEntity();
                    ClearMeterDetails_2();
                    DisplayEntityValues(meterMasterEntity);
                    DisplayEntityValues(consumerMeterEntity);
                    if (chkShowMeterDeactive.Checked)
                    {
                        chkActivateMeter.Checked = true;
                        chkActivateMeter.Enabled = false;
                    }
                }
            }
            catch (Exception)
            {

            }
		}
 
		private void DisplayEntityValues(MeterMasterEntity meterMasterEntity)
		{
            if (meterMasterEntity.Meter_ID == null)
                return;
			cboMeterNumber.Text = meterMasterEntity.Meter_ID;

			if (meterMasterEntity != null)
			{
				for (int index = 0; index < cboMeterType.Items.Count; index++)
				{
					if (((System.Data.DataRowView)(cboMeterType.Items[index])).Row.ItemArray[1].ToString().Equals(meterMasterEntity.MeterType_ID.ToString()))
					{
						cboMeterType.SelectedIndex = index;
						break;
					}
				}

				for (int index = 0; index < cboMeterModel.Items.Count; index++)
				{
					if (((System.Data.DataRowView)(cboMeterModel.Items[index])).Row.ItemArray[1].ToString().Equals(meterMasterEntity.MeterModel_ID.ToString()))
					{
						cboMeterModel.SelectedIndex = index;
						break;
					}
				}


				txtEMF.Text = meterMasterEntity.Meter_EMF.ToString();
				txtBoxMeterSIM.Text = meterMasterEntity.Meter_Phone;
                phone = meterMasterEntity.Meter_Phone;
				if (Convert.ToDouble((meterMasterEntity.Meter_ContractDemand.ToString())) < 1)
					txtContractDemand.Text = "0" + meterMasterEntity.Meter_ContractDemand.ToString();
				else if (Convert.ToDouble((meterMasterEntity.Meter_ContractDemand.ToString())) < 10)
					txtContractDemand.Text = "0" + meterMasterEntity.Meter_ContractDemand.ToString();
				else
					txtContractDemand.Text = meterMasterEntity.Meter_ContractDemand.ToString();
				for (int index = 0; index < cboUnits.Items.Count; index++)
				{
					if (((System.Data.DataRowView)(cboUnits.Items[index])).Row.ItemArray[1].ToString().Equals(meterMasterEntity.MeterUnit_ID.ToString()))
					{
						cboUnits.SelectedIndex = index;
						break;
					}
				}

				txtMeterCTPrimary.Text = meterMasterEntity.Meter_CTPrimary.ToString();
				txtMeterCTSecondary.Text = meterMasterEntity.Meter_CTSecondary.ToString();
				txtMeterCTRatio.Text = Convert.ToString(meterMasterEntity.Meter_CTPrimary / meterMasterEntity.Meter_CTSecondary);
				txtMeterPTPrimary.Text = meterMasterEntity.Meter_PTPrimary.ToString();
				txtMeterPTSecondary.Text = meterMasterEntity.Meter_PTSecondary.ToString();
				txtMeterPTRatio.Text = Convert.ToString(meterMasterEntity.Meter_PTPrimary / meterMasterEntity.Meter_PTSecondary);
				txtInstalledCTPrimary.Text = meterMasterEntity.Meter_InstalledCTPrimary.ToString();
				txtInstalledCTSecondary.Text = meterMasterEntity.Meter_InstalledCTSecondary.ToString();
				txtInstalledCTRatio.Text = Convert.ToString(meterMasterEntity.Meter_InstalledCTPrimary / meterMasterEntity.Meter_InstalledCTSecondary);
				txtInstalledPTPrimary.Text = meterMasterEntity.Meter_InstalledPTPrimary.ToString();
				txtInstalledPTSecondary.Text = meterMasterEntity.Meter_InstalledPTSecondary.ToString();
				txtInstalledPTRatio.Text = Convert.ToString(meterMasterEntity.Meter_InstalledPTPrimary / meterMasterEntity.Meter_InstalledPTSecondary);

				if (meterMasterEntity.Meter_Status == 0)
					chkActivateMeter.Checked = false;
				else
					chkActivateMeter.Checked = true;
			}
		}

        private void DisplayEntityValues(ConsumerMeterEntity consumerMeterEntity)
        {
            if (consumerMeterEntity == null)
                return;
            if(consumerMeterEntity.Meter_AllocationDate>0)
            DtpInstalled.Value = DateUtility.LongToDateTime(consumerMeterEntity.Meter_AllocationDate);
            txtMeterLocation.Text = consumerMeterEntity.Meter_Location;
            if (consumerMeterEntity.Meter_Location == "-")
                txtMeterLocation.Text = "";
        }
 
		private void DisplayEntityValues(bool isSuspected) 
		{
			if (isSuspected == true)
				chkSuspectedConsumer.Checked = true;
			else
				chkSuspectedConsumer.Checked = false;
		}
 
        private void DisplayEntityValues(ConsumerMasterEntity consumerMasterEntity)
        {
            if (consumerMasterEntity == null)
                return;
            if (Mode.Equals("Edit"))
            {
                txtConsumerNumber.Enabled = false;
                txtConsumerNumber.Text = consumerMasterEntity.Consumer_Number;
            }
            else
                txtConsumerNumber.Text = consumerMasterEntity.Consumer_Number;
            txtConsumerName.Text = consumerMasterEntity.Consumer_Name;
            for (int index = 0; index < cboConsumerType.Items.Count; index++)
            {
                if (((System.Data.DataRowView)(cboConsumerType.Items[index])).Row.ItemArray[1].ToString().Equals(consumerMasterEntity.ConsumerType_ID.ToString()))
                {
                    cboConsumerType.SelectedIndex = index; 
                    break;
                }
            }
            txtConsumerTelephone.Text = consumerMasterEntity.Consumer_Phone;
            txtConsumerHouseNo.Text = consumerMasterEntity.Consumer_HNumber;
            txtConsumerStreet.Text = consumerMasterEntity.Consumer_Street;
            txtConsumerCity.Text = consumerMasterEntity.Consumer_City;
            txtConsumerEmail.Text = consumerMasterEntity.Consumer_Email;
        }
 
		private bool ValidateData()
		{
			bool Flag = false;
			string zeroValue = "0";
			string val = "";
            int result;
            int emf = 1;
           
			if (string.IsNullOrEmpty(txtConsumerNumber.Text.Trim()))
			{
				CABMessageBox.ShowFilterMessage("L000014|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerNumber.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(txtConsumerNumber.Text.Trim(), ValidationConstant.consumerID))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000014|M000012", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerNumber.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(txtConsumerName.Text.Trim()))
			{
				CABMessageBox.ShowFilterMessage("L000015|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerName.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(txtConsumerName.Text.Trim(), ValidationConstant.UserName))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000015|M000006", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerName.Focus();
				return Flag;
			}
			else if (cboConsumerType.SelectedValue.ToString() == "-")
			{
				CABMessageBox.ShowFilterMessage("L000016|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				cboConsumerType.Focus();
				return Flag;
			}
			else if ((txtConsumerTelephone.Text.Trim() != "") && (!ValidationProvider.ValidateData(txtConsumerTelephone.Text.Trim(), ValidationConstant.NumberValidation)))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000017|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerTelephone.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(txtConsumerHouseNo.Text.Trim()))
			{
				CABMessageBox.ShowFilterMessage("L000018|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerHouseNo.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(txtConsumerHouseNo.Text.Trim(), ValidationConstant.houseNumber))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000018|M000015", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerHouseNo.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(txtConsumerStreet.Text.Trim()))
			{
				CABMessageBox.ShowFilterMessage("L000019|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerStreet.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(txtConsumerStreet.Text.Trim(), ValidationConstant.houseNumber))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000019|M000015", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerStreet.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(txtConsumerCity.Text.Trim()))
			{
				CABMessageBox.ShowFilterMessage("L000020|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerCity.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(txtConsumerCity.Text.Trim(), ValidationConstant.city))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000020|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerCity.Focus();
				return Flag;
			}
			else if ((txtConsumerEmail.Text.Trim() != "") && (!ValidationProvider.ValidateData(txtConsumerEmail.Text.Trim(), ValidationConstant.email)))
			{
				CABMessageBox.ShowFilterMessage("M000007|L000021|M000013", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
				txtConsumerEmail.Focus();
				return Flag;
			}
            else if (cboMeterNumber.Text.Trim() == "")
            { 
                    CABMessageBox.ShowFilterMessage("L000022|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMeterNumber.Focus();
                    return Flag;  
            }
            //else if (cboMeterNumber.Text.Trim().Length != 7)
            //{
            //    MessageBox.Show("Meter Number can't be less or gereater then 7 characher long.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cboMeterNumber.Focus();
            //    return Flag;
            //}
            else if (!ValidationProvider.ValidateData(cboMeterNumber.Text.Trim(), ValidationConstant.consumerID))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000022|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterNumber.Focus();
                return Flag;
            }
            else if (cboMeterType.SelectedValue == null)
            {
                CABMessageBox.ShowFilterMessage("L000023|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterType.Focus();
                return Flag;
            }
            else if (cboMeterType.SelectedValue.ToString() == "-")
            {
                CABMessageBox.ShowFilterMessage("L000023|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterType.Focus();
                return Flag;
            }

            else if (cboMeterModel.SelectedValue == null)
            {
                CABMessageBox.ShowFilterMessage("L000024|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterModel.Focus();
                return Flag;
            }
            else if (cboMeterModel.SelectedValue.ToString() == "-")
            {
                CABMessageBox.ShowFilterMessage("L000024|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterModel.Focus();
                return Flag;
            }
            else if ((txtMeterLocation.Text.Trim() != "") && (!ValidationProvider.ValidateData(txtMeterLocation.Text.Trim(), ValidationConstant.city)))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000025|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterLocation.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtEMF.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000027|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEMF.Focus();
                return Flag;
            }
            else if (emf<1)
            {
                 MessageBox.Show("EMF can't be less than 1.","BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEMF.Focus();
                return Flag;
            }
            else if (!Int32.TryParse(txtEMF.Text.Trim(), out result))
            {
                MessageBox.Show("Invalid EMF Number.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEMF.Focus();
                return false;
            }
            else if ((txtBoxMeterSIM.Text.Trim() != "") && (!ValidationProvider.ValidateData(txtBoxMeterSIM.Text.Trim(), ValidationConstant.NumberValidation)))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return Flag;
            }
            else if ((txtBoxMeterSIM.Text.Trim() != "") && (txtBoxMeterSIM.Text.Trim().Length != 10))
            {
                CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxMeterSIM.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtContractDemand.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000028|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContractDemand.Focus();
                return Flag;
            }
            else if ((cboUnits.SelectedValue == null))
            {
                CABMessageBox.ShowFilterMessage("L000028|L000038|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboUnits.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtMeterCTPrimary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000030|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterCTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtMeterCTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || txtMeterCTPrimary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000030|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterCTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtMeterCTSecondary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000031|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterCTSecondary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtMeterCTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || txtMeterCTSecondary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000031|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterCTSecondary.Focus();
                return Flag;
            }
            else if (Convert.ToInt32(txtMeterCTPrimary.Text.Trim()) < (Convert.ToInt32(txtMeterCTSecondary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000016", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterCTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtMeterPTPrimary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000033|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterPTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtMeterPTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || txtMeterPTPrimary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000033|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterPTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtMeterPTSecondary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000033|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterPTSecondary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtMeterPTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || txtMeterPTSecondary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000033|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterPTSecondary.Focus();
                return Flag;
            }
            else if (Convert.ToInt32(txtMeterPTPrimary.Text.Trim()) < (Convert.ToInt32(txtMeterPTSecondary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000017", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMeterPTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000036|L000030|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledCTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || txtInstalledCTPrimary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000030|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000036|L000031|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTSecondary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledCTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || txtInstalledCTSecondary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000031|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTSecondary.Focus();
                return Flag;
            }
            else if (Convert.ToInt32(txtInstalledCTPrimary.Text.Trim()) < (Convert.ToInt32(txtInstalledCTSecondary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000018", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledPTPrimary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000036|L000033|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledPTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || txtInstalledPTPrimary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000033|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTPrimary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledPTSecondary.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000036|L000034|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTSecondary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledPTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || txtInstalledPTSecondary.Text.Trim() == zeroValue)
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000034|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTSecondary.Focus();
                return Flag;
            }
            else if (Convert.ToInt32(txtInstalledPTPrimary.Text.Trim()) < (Convert.ToInt32(txtInstalledPTSecondary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000019", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTPrimary.Focus();
                return Flag;
            }
            else if (txtBoxMeterSIM.Text.Trim() != "")
            {
                int counter = new GSMConfigBLL().GetCount(Int64.Parse(txtBoxMeterSIM.Text));
                if (counter > 0 && txtBoxMeterSIM.Text.Trim() != phone)
                {
                    MessageBox.Show("SIM already allocated.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    return false;
                }

                if (txtBoxMeterSIM.Text.Substring(0, 1) == "0")
                {
                    MessageBox.Show("Meter Sim Number cannot start with a Zero.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBoxMeterSIM.Focus();
                    return false;
                }
            }
           
            return Flag = true;
            //if (txtBoxMeterSIM.Text.Trim() != "")
            //{
            //    int counter = new GSMConfigBLL().GetCount(Int64.Parse(txtBoxMeterSIM.Text));
            //    if (counter > 0 && txtBoxMeterSIM.Text.Trim() != phone)
            //    {
            //        MessageBox.Show("SIM already allocated.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        txtBoxMeterSIM.Focus();
            //        return false;
            //    }
            //}

            //if (txtBoxMeterSIM.Text.Substring(0,1)=="0")
            //{
            //    MessageBox.Show("Meter Sim Number cannot start with a Zero.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtBoxMeterSIM.Focus();
            //    return false;
            //}
            //return Flag = true;
		} 

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	  
		  
		void ClearMeterDetails()
		{ 
			if (cboMeterNumber.DropDownStyle == ComboBoxStyle.DropDownList) 
                cboMeterNumber.Items.Clear(); 
			chkShowMeterDeactive.Checked = false;
			cboMeterNumber.Text = "";
			cboMeterType.SelectedItem = null;
			cboMeterModel.SelectedItem = null;
			txtMeterLocation.Text = ""; 
			DtpInstalled.MaxDate = DateTime.Now;
			txtEMF.Text = "";
			txtBoxMeterSIM.Text = string.Empty;
			txtContractDemand.Text = "";
			cboUnits.SelectedItem = null;
			txtMeterCTPrimary.Text = "";
			txtMeterCTSecondary.Text = "";
			txtMeterCTRatio.Text = "";
			txtMeterPTPrimary.Text = "";
			txtMeterPTSecondary.Text = "";
			txtMeterPTRatio.Text = "";
			txtInstalledCTPrimary.Text = "";
			txtInstalledCTSecondary.Text = "";
			txtInstalledCTRatio.Text = "";
			txtInstalledPTPrimary.Text = "";
			txtInstalledPTSecondary.Text = "";
			txtInstalledPTRatio.Text = "";
		}

		void ClearMeterDetails_2()
		{
			cboMeterType.SelectedItem = null;
			cboMeterModel.SelectedItem = null;
			txtMeterLocation.Text = ""; 
			DtpInstalled.MaxDate = DateTime.Now;
			txtEMF.Text = "";
			txtContractDemand.Text = "";
			cboUnits.SelectedItem = null;
			txtMeterCTPrimary.Text = "";
			txtMeterCTSecondary.Text = "";
			txtMeterCTRatio.Text = "";
			txtMeterPTPrimary.Text = "";
			txtMeterPTSecondary.Text = "";
			txtMeterPTRatio.Text = "";
			txtInstalledCTPrimary.Text = "";
			txtInstalledCTSecondary.Text = "";
			txtInstalledCTRatio.Text = "";
			txtInstalledPTPrimary.Text = "";
			txtInstalledPTSecondary.Text = "";
			txtInstalledPTRatio.Text = "";
		}
 
		void DisableFunction()
		{
			txtConsumerNumber.Enabled = false;
			txtConsumerName.Enabled = false;
			cboConsumerType.Enabled = false;
			txtConsumerTelephone.Enabled = false;
			txtConsumerHouseNo.Enabled = false;
			txtConsumerStreet.Enabled = false;
			txtConsumerCity.Enabled = false;
			txtConsumerEmail.Enabled = false;
			chkSuspectedConsumer.Enabled = false;
		}
 
		
		private void txtMeterCTPrimary_TextChanged(object sender, EventArgs e)
		{
			if ((txtMeterCTPrimary.Text != "") && (txtMeterCTSecondary.Text != ""))
			{
				if (int.TryParse(txtMeterCTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtMeterCTPrimary.Text);
				if (int.TryParse(txtMeterCTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtMeterCTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtMeterCTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtMeterCTRatio.Text = "";
			}
			else
				txtMeterCTRatio.Text = "";
		}

		private void txtMeterCTSecondary_TextChanged(object sender, EventArgs e)
		{
			if ((txtMeterCTPrimary.Text != "") && (txtMeterCTSecondary.Text != ""))
			{
				if (int.TryParse(txtMeterCTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtMeterCTPrimary.Text);
				if (int.TryParse(txtMeterCTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtMeterCTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtMeterCTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtMeterCTRatio.Text = "";
			}
			else
				txtMeterCTRatio.Text = "";
		}

		private void txtMeterPTPrimary_TextChanged(object sender, EventArgs e)
		{
			if ((txtMeterPTPrimary.Text != "") && (txtMeterPTSecondary.Text != ""))
			{
				if (int.TryParse(txtMeterPTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtMeterPTPrimary.Text);
				if (int.TryParse(txtMeterPTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtMeterPTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtMeterPTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtMeterPTRatio.Text = "";
			}
			else
				txtMeterPTRatio.Text = "";
		}

		private void txtMeterPTSecondary_TextChanged(object sender, EventArgs e)
		{
			if ((txtMeterPTPrimary.Text != "") && (txtMeterPTSecondary.Text != ""))
			{
				if (int.TryParse(txtMeterPTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtMeterPTPrimary.Text);
				if (int.TryParse(txtMeterPTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtMeterPTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtMeterPTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtMeterPTRatio.Text = "";
			}
			else
				txtMeterPTRatio.Text = "";
		}

		private void txtInstalledCTPrimary_TextChanged(object sender, EventArgs e)
		{
			if ((txtInstalledCTPrimary.Text != "") && (txtInstalledCTSecondary.Text != ""))
			{
				if (int.TryParse(txtInstalledCTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtInstalledCTPrimary.Text);
				if (int.TryParse(txtInstalledCTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtInstalledCTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtInstalledCTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtInstalledCTRatio.Text = "";
			}
			else
				txtInstalledCTRatio.Text = "";
		}

		private void txtInstalledCTSecondary_TextChanged(object sender, EventArgs e)
		{
			if ((txtInstalledCTPrimary.Text != "") && (txtInstalledCTSecondary.Text != ""))
			{
				if (int.TryParse(txtInstalledCTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtInstalledCTPrimary.Text);
				if (int.TryParse(txtInstalledCTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtInstalledCTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtInstalledCTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtInstalledCTRatio.Text = "";
			}
			else
				txtInstalledCTRatio.Text = "";
		}

		private void txtInstalledPTPrimary_TextChanged(object sender, EventArgs e)
		{
			if ((txtInstalledPTPrimary.Text != "") && (txtInstalledPTSecondary.Text != ""))
			{
				if (int.TryParse(txtInstalledPTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtInstalledPTPrimary.Text);
				if (int.TryParse(txtInstalledPTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtInstalledPTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtInstalledPTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtInstalledPTRatio.Text = "";
			}
			else
				txtInstalledPTRatio.Text = "";
		}

		private void txtInstalledPTSecondary_TextChanged(object sender, EventArgs e)
		{
			if ((txtInstalledPTPrimary.Text != "") && (txtInstalledPTSecondary.Text != ""))
			{
				if (int.TryParse(txtInstalledPTPrimary.Text, out ctPrimary))
					ctPrimary = Convert.ToInt32(txtInstalledPTPrimary.Text);
				if (int.TryParse(txtInstalledPTSecondary.Text, out ctSecondary))
					ctSecondary = Convert.ToInt32(txtInstalledPTSecondary.Text);
				if ((ctPrimary != 0) && (ctSecondary != 0))
					txtInstalledPTRatio.Text = ValidationProvider.GetDivisionValue(ctPrimary, ctSecondary).ToString();
				else
					txtInstalledPTRatio.Text = "";
			}
			else
				txtInstalledPTRatio.Text = "";
		}

		private void ConsumerMeterDetails_Activated(object sender, EventArgs e)
		{
			mainForm.toolStripStatusLabel.Text = "";
			Application.DoEvents();
		}

        private void cboMeterNumber_TextChanged(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

	}
}


