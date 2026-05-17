using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using System.Text.RegularExpressions;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class ConsumerMeterDetails : MdiChildForm
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ConsumerMeterDetails).ToString());
        private ConsumerMasterBLL consumerMasterBLL = new ConsumerMasterBLL();
        private ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
        private MeterMasterBLL meterMasterBLL = new MeterMasterBLL();
        private DLMS650GeneralBLL dlm650generalBLL = new DLMS650GeneralBLL();
        private SuspectedConsumerBLL suspectedConsumerBLL = new SuspectedConsumerBLL();
        private ConsumerMasterEntity consumerMasterEntity = new ConsumerMasterEntity();
        private ConsumerMeterEntity consumerMeterEntity = new ConsumerMeterEntity();
        private MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
        private MeterMasterEntity oldMeterMasterEntity = new MeterMasterEntity();
        private SuspectedConsumerEntity suspectedConsumerEntity = new SuspectedConsumerEntity();
        private GSMGroupBLL gsmGroupBll = new GSMGroupBLL();

        MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
        double contractDemand = 0;
        int installedCTPrimary, installedCTSecondary;
        int installedPTPrimary, installedPTSecondary;
        int installedCTRatio;
        int installedPTRatio;
        string phone = "";
        string defaultValue = "1";
        bool isUseEMFInCalculation = true;

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
        public string GPRSIMEI { get; set; }

        public ConsumerMeterDetails()
        {
            InitializeComponent();
            //DLMS check removed because of single login
            //if (UtilityDetails.PrimaryUtlityName == CAB.Framework.UtilityEntity.DLMS.ToString())
            //{
                //ddlCommunicationtype.Items.Add(CommunicationType.GPRS.ToString());
            //}
            //// GPRS support
            //if (UtilityDetails.ShowGPRSCommunication)
            //{
            //    ddlCommunicationtype.Items.Add(CommunicationType.GPRS.ToString());
            //}
        }

        private void ConsumerMeterDetails_Load(object sender, EventArgs e)
        {
            mainForm.ClearGrid();
            if (UtilityDetails.GetUtilityDetails() == CAB.Framework.UtilityEntity.Generic)
            {
                chkUseEMFInCalculation.Checked = false;
            }
            if (ConfigInfo.GetTenderType() != TenderType.JUSCO)
            {
                gbAreaDetails.Visible = false;
                groupBox2.Height = groupBox2.Height - 80;
                lblindication.Location = new System.Drawing.Point(lblindication.Location.X, lblindication.Location.Y - 80);
                lblNote.Location = new System.Drawing.Point(lblNote.Location.X, lblNote.Location.Y - 80);
                btnSave.Location = new System.Drawing.Point(btnSave.Location.X, btnSave.Location.Y - 80);
                btnCancel.Location = new System.Drawing.Point(btnCancel.Location.X, btnCancel.Location.Y - 80);
            }
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
                // to remove Duplicate E250 - WCM for Ruby6.0 , WB and Legacy Ruby meters
                modelDataSet = new DLMS650CommonBLL().RemoveDuplicateRows(modelDataSet, "DisplayMember");
                // to remove Duplicate E250 - WCM for Ruby6.0 , WB and Legacy Ruby meters

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
                if (gsmGroupBll.GetMeterExistance(MeterID) == 1)
                {
                    ddlCommunicationtype.Enabled = false;
                }
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
            /*GKG 13/02/2013 JDVVNL Change Patch work As suggested by Aditya*/
            if (UtilityDetails.PrimaryUtlityName == "JDVVNL")
            {
                this.txtInstalledPTPrimary.MaxLength = 6;
                this.lblInstalledPTPrimary.Text = "PT Primary (KV)";
                this.txtInstalledPTSecondary.MaxLength = 6;
                this.lblInstalledPTSecondary.Text = "PT Secondary (KV)";

            }
            else
            {
                this.txtInstalledPTPrimary.MaxLength = 3;
                this.lblInstalledPTPrimary.Text = "PT Primary (V)";
                this.txtInstalledPTSecondary.MaxLength = 3;
                this.lblInstalledPTSecondary.Text = "PT Secondary (V)";
            }
            /*GKG 13/02/2013 JDVVNL Change Patch work As suggested by Aditya*/
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
            meterMasterEntity.Meter_Phone = "0" + txtBoxMeterSIM.Text.Trim();

            if (double.TryParse(txtContractDemand.Text, out contractDemand))
                meterMasterEntity.Meter_ContractDemand = contractDemand;
            else
                meterMasterEntity.Meter_ContractDemand = Convert.ToDouble("0");

            meterMasterEntity.MeterUnit_ID = Convert.ToInt16(cboUnits.SelectedValue);

            // following two properties filled in to export Meter CT and PT Ratio; 12 April 2012
            meterMasterEntity.Meter_CTPrimary = Convert.ToInt16(txtMeterCTRatio.Text);
            meterMasterEntity.Meter_PTPrimary = Convert.ToInt16(txtMeterPTRatio.Text);

            //meterMasterEntity.Meter_CTPrimary = Convert.ToInt32(txtMeterCTPrimary.Text.Trim());
            //meterMasterEntity.Meter_CTSecondary = Convert.ToInt32(txtMeterCTSecondary.Text.Trim());
            //meterMasterEntity.Meter_PTPrimary = Convert.ToInt32(txtMeterPTPrimary.Text.Trim());
            //meterMasterEntity.Meter_PTSecondary = Convert.ToInt32(txtMeterPTSecondary.Text.Trim());
            if (int.TryParse(txtInstalledCTPrimary.Text.Trim(), out installedCTPrimary))
                meterMasterEntity.Meter_InstalledCTPrimary = installedCTPrimary;
            else
                meterMasterEntity.Meter_InstalledCTPrimary = 0;
            if (int.TryParse(txtInstalledCTSecondary.Text.Trim(), out installedCTSecondary))
                meterMasterEntity.Meter_InstalledCTSecondary = installedCTSecondary;
            else
                meterMasterEntity.Meter_InstalledCTSecondary = 0;
            meterMasterEntity.MeterInstalledCTRatio = Convert.ToInt32(txtInstalledCTRatio.Text.Trim());
            if (int.TryParse(txtInstalledPTPrimary.Text.Trim(), out installedPTPrimary))
                meterMasterEntity.Meter_InstalledPTPrimary = installedPTPrimary;
            else
                meterMasterEntity.Meter_InstalledPTPrimary = 0;
            meterMasterEntity.MeterInstalledPTRatio = Convert.ToInt32(txtInstalledPTRatio.Text.Trim());
            if (int.TryParse(txtInstalledPTSecondary.Text.Trim(), out installedPTSecondary))
                meterMasterEntity.Meter_InstalledPTSecondary = installedPTSecondary;
            else
                meterMasterEntity.Meter_InstalledPTSecondary = 0;
            // Assign value to use emf in calculations - DHBVNL June 2011
            meterMasterEntity.UseEMFInCalculations = chkUseEMFInCalculation.Checked == true ? 1 : 0;

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
            consumerMeterEntity.Region_ID = Convert.ToInt32(ddlRegion.SelectedValue.ToString());
            consumerMeterEntity.Circle_ID = Convert.ToInt32(ddlCircle.SelectedValue.ToString());
            consumerMeterEntity.Division_ID = Convert.ToInt32(ddlDivision.SelectedValue.ToString());
            if (ddlCommunicationtype.SelectedIndex > -1)
                consumerMeterEntity.Communcation_Type = ddlCommunicationtype.SelectedItem.ToString();
            if (string.IsNullOrEmpty(consumerMeterEntity.Meter_Location))
                consumerMeterEntity.Meter_Location = "-";

            // to enable syncing of endpoints changes to GPRS Adapter
            LandisGyr.AMI.Layers.Endpoint[] endpoints = new LandisGyr.AMI.Layers.Endpoint[1];
            bool isIMEIAlreadyExists = false;
            //Fix defect #165662
            if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.GPRS.ToString())
            {
                meterMasterEntity.MeterGPRSModemIMEI = textBoxModemIMEI.Text.Trim();
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                // to sync GPRS Adapter layer with the endpoint

                endpoints[0] = new LandisGyr.AMI.Layers.Endpoint();
                endpoints[0].Model = new LandisGyr.AMI.Layers.EndpointModel();
                endpoints[0].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                endpoints[0].SerialNumber = meterMasterEntity.MeterGPRSModemIMEI;
                endpoints[0].IsUsable = true;
                endpoints[0].AssociatedMeters = new LandisGyr.AMI.Layers.Meter[1];
                endpoints[0].AssociatedMeters[0] = new LandisGyr.AMI.Layers.Meter();
                endpoints[0].AssociatedMeters[0].MeterNumber = meterMasterEntity.Meter_ID;
                endpoints[0].AssociatedMeters[0].Model = new LandisGyr.AMI.Layers.MeterModel();
                endpoints[0].AssociatedMeters[0].Model.Type = LandisGyr.AMI.Layers.MeterType.Electric;


                isIMEIAlreadyExists = meterMasterBLL.IsIMEIAlreadyExists(meterMasterEntity.MeterGPRSModemIMEI);
            }
            else if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.TCP.ToString())
            {
                //TCP case handelled

                meterMasterEntity.MeterGPRSModemIMEI = textBoxModemIMEI.Text.Trim();
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                isIMEIAlreadyExists = meterMasterBLL.IsIMEIAlreadyExists(meterMasterEntity.MeterGPRSModemIMEI);
            }
            else if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.FTP.ToString())
            {
                //FTP case handelled

                meterMasterEntity.MeterGPRSModemIMEI = textBoxModemIMEI.Text.Trim();
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                isIMEIAlreadyExists = meterMasterBLL.IsIMEIAlreadyExists(meterMasterEntity.MeterGPRSModemIMEI);
            }
            else
            {
                meterMasterEntity.MeterGPRSModemIMEI = null;
                meterMasterEntity.GPRSModemIpType = null;
                meterMasterEntity.GPRSModemConnectionType = null;

            }

            suspectedConsumerEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;


            bool isMeterAlreadyExists = meterMasterBLL.ValidateMeterNumber(meterMasterEntity);

            if (Mode.Equals("Edit"))//OK
            {

                if (isEnterNewMeterDetails)
                {
                    if (!isMeterAlreadyExists)
                    {
                        consumerMasterBLL.UpdateData(consumerMasterEntity);
                        meterMasterBLL.InsertData(meterMasterEntity);
                        InsertIntoMeterMasterLog(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                        SaveSuspectedConsumer();


                        //   MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    else
                    {
                        consumerMasterBLL.UpdateData(consumerMasterEntity);
                        meterMasterBLL.UpdateData(meterMasterEntity);
                        InsertIntoMeterMasterLog(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                        SaveSuspectedConsumer();

                        //   MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    InsertIntoMeterMasterLog(meterMasterEntity);
                    consumerMasterBLL.UpdateData(consumerMasterEntity);
                    if (meterMasterBLL.ValidateMeterNumber(meterMasterEntity))
                        meterMasterBLL.UpdateData(meterMasterEntity);
                    else
                        meterMasterBLL.InsertData(meterMasterEntity);

                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                    SaveSuspectedConsumer();

                }

                //If selected communication type is GPRS. Then either add or update endpoint in M2M. 
                if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.GPRS.ToString().ToUpper())
                {
                    // to enable syncing of endpoints changes to GPRS Adapter
                    if (isIMEIAlreadyExists || isMeterAlreadyExists)
                    {
                        GPRSCommunication.EndPointOperations.UpdateEndpoints(endpoints);
                    }
                    else
                    {
                        GPRSCommunication.EndPointOperations.AddEndpoints(endpoints);
                    }
                }
                //If communication type for meter is changed from GPRS to M2M .
                else if (!string.IsNullOrEmpty(GPRSIMEI.Trim()))
                {
                    LandisGyr.AMI.Layers.Endpoint[] endPoints = new LandisGyr.AMI.Layers.Endpoint[1];
                    endPoints[0] = new LandisGyr.AMI.Layers.Endpoint();
                    endPoints[0].Model = new LandisGyr.AMI.Layers.EndpointModel();
                    endPoints[0].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                    endPoints[0].SerialNumber = GPRSIMEI.Trim();
                    GPRSCommunication.EndPointOperations.RemoveEndpoints(endPoints);

                }

                MessageBox.Show("Data Updated Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearMeterDetails();
                cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
                cboMeterNumber.Text = "";
                groupBox2.Enabled = false;
                this.Close();
            }
            if (Mode.Equals("Add"))
            {
                if (isEnterNewMeterDetails && chkShowMeterDeactive.Checked)//OK
                {
                    consumerMasterBLL.InsertData(consumerMasterEntity);
                    if (isMeterAlreadyExists)
                        meterMasterBLL.UpdateData(meterMasterEntity);
                    else
                        meterMasterBLL.InsertData(meterMasterEntity);
                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    consumerMeterBLL.InsertUpdateData(consumerMeterEntity);
                    SaveSuspectedConsumer();

                }
                else if (isEnterNewMeterDetails && !chkShowMeterDeactive.Checked)//OK
                {
                    if (!isMeterAlreadyExists)
                    {
                        if (((!string.IsNullOrEmpty(txtInstalledCTPrimary.Text) && !string.IsNullOrEmpty(txtInstalledCTSecondary.Text)) || (!string.IsNullOrEmpty(txtInstalledPTPrimary.Text) && !string.IsNullOrEmpty(txtInstalledPTSecondary.Text))))
                        {
                            InsertIntoMeterMasterLog(meterMasterEntity);
                        }

                        consumerMasterBLL.InsertData(consumerMasterEntity);
                        meterMasterBLL.InsertData(meterMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        consumerMeterBLL.InsertUpdateData(consumerMeterEntity);//InsertData
                        SaveSuspectedConsumer();
                    }
                    else
                    {
                        mainForm.toolStripStatusLabel.Text = "Meter already available.";
                        return;
                    }
                }

                // to enable syncing of endpoints changes to GPRS Adapter
                if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.GPRS.ToString())
                {
                    if (isIMEIAlreadyExists || isMeterAlreadyExists)
                    {
                        GPRSCommunication.EndPointOperations.UpdateEndpoints(endpoints);
                    }
                    else
                    {
                        GPRSCommunication.EndPointOperations.AddEndpoints(endpoints);
                    }
                }
                MessageBox.Show("Data saved Successfully", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearMeterDetails();
                cboMeterNumber.DropDownStyle = ComboBoxStyle.Simple;
                cboMeterNumber.Text = "";
                groupBox2.Enabled = false;
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
                    //   mainForm.toolStripStatusLabel.Text = "Data Updated Successfully";
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

                    }
                    else
                    {
                        mainForm.toolStripStatusLabel.Text = "MeterID Already Available";
                        return;
                    }
                }

                if (ddlCommunicationtype.SelectedItem != null && ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.GPRS.ToString())
                {
                    if (isIMEIAlreadyExists || isMeterAlreadyExists)
                    {
                        GPRSCommunication.EndPointOperations.UpdateEndpoints(endpoints);
                    }
                    else
                    {
                        GPRSCommunication.EndPointOperations.AddEndpoints(endpoints);
                    }

                    if (ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.GPRS.ToString())
                    {
                        if (isIMEIAlreadyExists || isMeterAlreadyExists)
                        {
                            GPRSCommunication.EndPointOperations.UpdateEndpoints(endpoints);
                        }
                        else
                        {
                            GPRSCommunication.EndPointOperations.AddEndpoints(endpoints);
                        }
                    }
                }
            }
            mainForm.toolStripStatusLabel.Text = "Data Updated Successfully";
            ClearMeterDetails();
            groupBox2.Enabled = false;

            if (this.Text.Contains("New") && (this.txtConsumerNumber.Text != ""))
            {
                this.txtConsumerNumber.ReadOnly = true;
            }
        }


        private void InsertIntoMeterMasterLog(MeterMasterEntity meterMasterEntity)
        {
            //Check if Installed CT Primary/Secondary values are changed or not

            oldMeterMasterEntity = meterMasterBLL.GetDetailData(MeterID, 1) as MeterMasterEntity;

            if (oldMeterMasterEntity != null)
            {
                if (meterMasterEntity.Meter_InstalledCTPrimary != oldMeterMasterEntity.Meter_InstalledCTPrimary || meterMasterEntity.Meter_InstalledPTPrimary != oldMeterMasterEntity.Meter_InstalledPTPrimary || meterMasterEntity.Meter_InstalledCTSecondary != oldMeterMasterEntity.Meter_InstalledCTSecondary || meterMasterEntity.Meter_InstalledPTSecondary != oldMeterMasterEntity.Meter_InstalledPTSecondary)
                {
                    meterMasterBLL.InsertDataIntoLog(meterMasterEntity);
                }
            }
            else
            {
                meterMasterBLL.InsertDataIntoLog(meterMasterEntity);
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
            string defaultValue = "1";
            mainForm.toolStripStatusLabel.Text = "";
            ClearMeterDetails();
            // Enter the default values for EMF calculation 
            txtMeterPTRatio.Text = defaultValue;
            txtMeterCTRatio.Text = defaultValue;

            //txtInstalledPTPrimary.Text = defaultValue;
            //txtInstalledPTSecondary.Text = defaultValue;
            //txtInstalledCTPrimary.Text = defaultValue;
            //txtInstalledCTSecondary.Text = defaultValue;
            txtInstalledCTRatio.Text = defaultValue;
            txtInstalledPTRatio.Text = defaultValue;
            txtEMF.Text = defaultValue;
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
                ////VBM - Meter model number changes
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                cboMeterModel.Enabled = false;
                cboMeterModel.SelectedIndex = -1;
                //}
                //BhardwajG : If the checkbox is true, then disable it and remove the value
                cboMeterType.Enabled = false;
                cboMeterType.SelectedIndex = -1;
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

                ////VBM - Meter model number changes
                //if (UtilityDetails.ShowMeterModelNo)
                //{
                cboMeterModel.Enabled = true;
                //}
                //BhardwajG : Enable meter type list if deactivate meter is unchecked
                cboMeterType.Enabled = true;
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
                        foreach (DataRow dr in dataSet.Tables[0].Rows)
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

        //private void cboMeterNumber_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        private void DisplayEntityValues(MeterMasterEntity meterMasterEntity)
        {
            if (meterMasterEntity.Meter_ID == null)
            {
                txtMeterCTRatio.Text = meterMasterEntity.MeterCTRatio <= 0 ? "1" : meterMasterEntity.MeterCTRatio.ToString();
                txtMeterPTRatio.Text = meterMasterEntity.MeterPTRatio <= 0 ? "1" : meterMasterEntity.MeterPTRatio.ToString();
                txtInstalledPTRatio.Text = "1";
                txtInstalledCTRatio.Text = "1";
                txtEMF.Text = "1";
                return;
            }
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

                txtMeterPTRatio.Text = meterMasterEntity.MeterPTRatio > 0 ? Convert.ToString(meterMasterEntity.MeterPTRatio) : "1";
                txtMeterCTRatio.Text = meterMasterEntity.MeterCTRatio > 0 ? Convert.ToString(meterMasterEntity.MeterCTRatio) : "1";
                txtEMF.Text = meterMasterEntity.Meter_EMF.ToString();
                if (!string.IsNullOrEmpty(meterMasterEntity.Meter_Phone))
                    if (meterMasterEntity.Meter_Phone.Length > 10)
                        txtBoxMeterSIM.Text = meterMasterEntity.Meter_Phone.Substring(1, 10);
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

                //txtMeterCTPrimary.Text = meterMasterEntity.Meter_CTPrimary.ToString();
                //txtMeterCTSecondary.Text = meterMasterEntity.Meter_CTSecondary.ToString();
                //txtMeterCTRatio.Text = Convert.ToString(meterMasterEntity.Meter_CTPrimary / meterMasterEntity.Meter_CTSecondary);
                //txtMeterPTPrimary.Text = meterMasterEntity.Meter_PTPrimary.ToString();
                //txtMeterPTSecondary.Text = meterMasterEntity.Meter_PTSecondary.ToString();
                //txtMeterPTRatio.Text = Convert.ToString(meterMasterEntity.Meter_PTPrimary / meterMasterEntity.Meter_PTSecondary);
                if (meterMasterEntity.Meter_InstalledCTPrimary > 0)
                    txtInstalledCTPrimary.Text = meterMasterEntity.Meter_InstalledCTPrimary.ToString();
                if (meterMasterEntity.Meter_InstalledCTSecondary > 0)
                    txtInstalledCTSecondary.Text = meterMasterEntity.Meter_InstalledCTSecondary.ToString();
                // Check that both CT primary secondary have values.
                if (meterMasterEntity.MeterInstalledCTRatio > 0)
                {
                    txtInstalledCTRatio.Text = meterMasterEntity.MeterInstalledCTRatio.ToString();
                }
                //else
                //{
                //    //if (meterMasterEntity.MeterCTRatio > 0)
                //    //    txtInstalledCTRatio.Text = meterMasterEntity.MeterCTRatio.ToString();
                //    //else
                //    //    txtInstalledCTRatio.Text = defaultValue;
                //}
                ////if (meterMasRatio = dterEntity.MeterInstalledCTRatio > 0
                ////    txtInstalledCTRatio.Text = meterMasterEntity.MeterInstalledCTRatio.ToString();
                ////else
                ////    txtInstalledCTefaultValue;
                if (meterMasterEntity.Meter_InstalledPTPrimary > 0)
                    txtInstalledPTPrimary.Text = meterMasterEntity.Meter_InstalledPTPrimary.ToString();
                if (meterMasterEntity.Meter_InstalledPTSecondary > 0)
                    txtInstalledPTSecondary.Text = meterMasterEntity.Meter_InstalledPTSecondary.ToString();
                // Check that both PT Primary secondary have values.
                //if (meterMasterEntity.MeterInstalledPTRatio > 0)
                //{
                //    txtInstalledPTRatio.Text = meterMasterEntity.MeterInstalledPTRatio.ToString();
                //}
                //else
                //{
                //    if (meterMasterEntity.MeterPTRatio > 0)
                //        txtInstalledPTRatio.Text = meterMasterEntity.MeterPTRatio.ToString();
                //    else
                //        txtInstalledPTRatio.Text = defaultValue;
                //}
                //txtInstalledPTRatio.Text = Convert.ToString(meterMasterEntity.Meter_InstalledPTPrimary / meterMasterEntity.Meter_InstalledPTSecondary);
                //if (meterMasterEntity.MeterInstalledPTRatio > 0)
                //    txtInstalledPTRatio.Text = meterMasterEntity.MeterInstalledPTRatio;
                //else
                //    txtInstalledPTRatio.Text = defaultValue;
                if (meterMasterEntity.Meter_Status == 0)
                    chkActivateMeter.Checked = false;
                else
                    chkActivateMeter.Checked = true;

                this.chkUseEMFInCalculation.CheckedChanged -= new System.EventHandler(this.chkUseEMFInCalculation_CheckedChanged);

                // Assign value to check box - DHBVNL June 2011
                chkUseEMFInCalculation.Checked = meterMasterEntity.UseEMFInCalculations == 1 ? true : false;
                this.chkUseEMFInCalculation.CheckedChanged += new System.EventHandler(this.chkUseEMFInCalculation_CheckedChanged);


            }
        }

        private void DisplayEntityValues(ConsumerMeterEntity consumerMeterEntity)
        {
            if (consumerMeterEntity == null)
                return;
            if (consumerMeterEntity.Meter_AllocationDate > 0)
                DtpInstalled.Value = DateUtility.LongToDateTime(consumerMeterEntity.Meter_AllocationDate);
            txtMeterLocation.Text = consumerMeterEntity.Meter_Location;
            if (consumerMeterEntity.Meter_Location == "-")
                txtMeterLocation.Text = "";

            //ddlRegion.SelectedValue = consumerMeterEntity.Region_ID;
            //ddlCircle.SelectedValue = consumerMeterEntity.Circle_ID;
            //ddlDivision.SelectedValue = consumerMeterEntity.Division_ID;
            if (consumerMeterEntity.Communcation_Type != null)
            {
                ddlCommunicationtype.SelectedItem = consumerMeterEntity.Communcation_Type;
                textBoxModemIMEI.Text = meterMasterEntity.MeterGPRSModemIMEI;

            }
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

            InitializeRegion();
            ddlRegion.SelectedValue = consumerMeterEntity.Region_ID;
            InitializeCircle(consumerMeterEntity.Region_ID);
            ddlCircle.SelectedValue = consumerMeterEntity.Circle_ID;
            InitializeDivision(consumerMeterEntity.Circle_ID);
            ddlDivision.SelectedValue = consumerMeterEntity.Division_ID;

            txtConsumerTelephone.Text = consumerMasterEntity.Consumer_Phone;
            txtConsumerHouseNo.Text = consumerMasterEntity.Consumer_HNumber;
            txtConsumerStreet.Text = consumerMasterEntity.Consumer_Street;
            txtConsumerCity.Text = consumerMasterEntity.Consumer_City;
            txtConsumerEmail.Text = consumerMasterEntity.Consumer_Email;

        }

        private bool ValidateData()
        {
            float consumerID;
            bool Flag = false;
            string zeroValue = "0";
            string val = "";
            installedCTPrimary = 0;
            installedCTSecondary = 0;
            installedPTPrimary = 0;
            installedPTSecondary = 0;
            int emf = 1;
            int outEMF;
            if (int.TryParse(txtEMF.Text.Trim(), out outEMF))
                emf = outEMF;
            int.TryParse(txtInstalledCTPrimary.Text.Trim(), out installedCTPrimary);
            int.TryParse(txtInstalledCTSecondary.Text.Trim(), out installedCTSecondary);
            int.TryParse(txtInstalledPTPrimary.Text.Trim(), out installedPTPrimary);
            int.TryParse(txtInstalledPTSecondary.Text.Trim(), out installedPTSecondary);

            if (string.IsNullOrEmpty(txtConsumerNumber.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000014|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConsumerNumber.Focus();
                return Flag;
            }
            else if (!float.TryParse(txtConsumerNumber.Text, out consumerID))
            {
                MessageBox.Show("Consumer ID field should be numeric only", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //else if (!ValidationProvider.ValidateData(txtConsumerName.Text.Trim(), ValidationConstant.UserName))
            //{
            //    CABMessageBox.ShowFilterMessage("M000007|L000015|M000006", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtConsumerName.Focus();
            //    return Flag;
            //}
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
            else if (ddlRegion.SelectedIndex <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000112", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ddlRegion.Focus();
                return Flag;
            }
            else if (ddlCircle.SelectedIndex <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000113", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ddlCircle.Focus();
                return Flag;
            }
            else if (ddlDivision.SelectedIndex <= -1)
            {
                CABMessageBox.ShowFilterMessage("M000114", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ddlDivision.Focus();
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
            else if (cboMeterNumber.Text.Trim().Length < 6)
            {
                MessageBox.Show("Meter Number should contain a minimum of 6 characters.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboMeterNumber.Focus();
                return Flag;
            }
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
            else if (emf < 1)
            {
                MessageBox.Show("Installed MF can't be less than 1.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEMF.Focus();
                return Flag;
            }

            else if (string.IsNullOrEmpty(txtContractDemand.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000028|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContractDemand.Focus();
                return Flag;
            }
            else if ((cboUnits.SelectedValue == null || cboUnits.SelectedIndex == 0))
            {
                CABMessageBox.ShowFilterMessage("L000028|L000038|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboUnits.Focus();
                return Flag;
            }
            //else if(string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim()))
            /// <summary>
            /// if either of the two installed ctprimary and ctsecondary are empty then raise validation
            /// </summary>
            else if ((string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim()) && !string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim())) || (string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim()) && !string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000104", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (string.IsNullOrEmpty(txtInstalledCTSecondary.Text))
                    txtInstalledCTSecondary.Focus();
                else
                    txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if ((string.IsNullOrEmpty(txtInstalledPTPrimary.Text.Trim()) && !string.IsNullOrEmpty(txtInstalledPTSecondary.Text.Trim())) || (string.IsNullOrEmpty(txtInstalledPTSecondary.Text.Trim()) && !string.IsNullOrEmpty(txtInstalledPTPrimary.Text.Trim())))
            {
                CABMessageBox.ShowFilterMessage("M000103", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (string.IsNullOrEmpty(txtInstalledPTPrimary.Text))
                    txtInstalledPTPrimary.Focus();
                else
                    txtInstalledPTSecondary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledCTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || (!string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim()) && installedCTPrimary <= 0))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000030|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledCTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || (!string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim()) && installedCTSecondary <= 0))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000031|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTSecondary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledCTRatio.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("M000105", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledPTPrimary.Text.Trim(), ValidationConstant.NumberValidation) || (!string.IsNullOrEmpty(txtInstalledPTPrimary.Text.Trim()) && installedPTPrimary <= 0))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000033|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTPrimary.Focus();
                return Flag;
            }
            else if (!ValidationProvider.ValidateData(txtInstalledPTSecondary.Text.Trim(), ValidationConstant.NumberValidation) || (!string.IsNullOrEmpty(txtInstalledPTSecondary.Text.Trim()) && installedPTSecondary <= 0))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000036|L000034|M000014", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledPTSecondary.Focus();
                return Flag;
            }
            else if (string.IsNullOrEmpty(txtInstalledPTRatio.Text.Trim()))
            {
                CABMessageBox.ShowFilterMessage("M000106", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInstalledCTPrimary.Focus();
                return Flag;
            }
            // if the communication type is of GSM
            else if (ddlCommunicationtype.SelectedIndex > -1)
            {
                if (ddlCommunicationtype.SelectedItem.ToString() == "GSM" || ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.GPRS.ToString())
                {
                    long simNumber = 0;
                    // raise error if the Meter sim number is empty..
                    if (txtBoxMeterSIM.Text.Trim() == string.Empty)
                    {
                        CABMessageBox.ShowFilterMessage("M000115", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxMeterSIM.Focus();
                        return false;
                    }
                    // raise error if the number can not be parsed to a 64 bit signed integer
                    if (!long.TryParse(txtBoxMeterSIM.Text, out simNumber))
                    {
                        CABMessageBox.ShowFilterMessage("M000007|L000058|M000101", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxMeterSIM.Focus();
                        return Flag;
                    }
                    // raise error if the number is not of 10 length
                    if (txtBoxMeterSIM.Text.Trim().Length != 10)
                    {
                        CABMessageBox.ShowFilterMessage("M000100", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtBoxMeterSIM.Focus();
                        return Flag;
                    }

                    //The following if condition added to check if the sim no. is already allocated to any other consumer.
                    consumerMeterBLL = new ConsumerMeterBLL();
                    meterMasterBLL = new MeterMasterBLL();
                    string consumerNo = consumerMeterBLL.GetConsumerNumber(Int64.Parse(txtBoxMeterSIM.Text));
                    if (consumerNo != string.Empty)
                    {
                        if (consumerNo != txtConsumerNumber.Text)
                        {
                            MessageBox.Show("SIM already allocated to a different consumer.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtBoxMeterSIM.Focus();
                            return false;
                        }
                        else
                        {
                            string meterNo = meterMasterBLL.GetMeterNumber(Int64.Parse(txtBoxMeterSIM.Text));
                            if (meterNo != cboMeterNumber.Text)
                            {
                                int counter = new GSMConfigBLL().GetCount(Int64.Parse(txtBoxMeterSIM.Text));
                                if (counter > 0 && txtBoxMeterSIM.Text.Trim() != phone)
                                {
                                    MessageBox.Show("SIM already associated with a different meter.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    txtBoxMeterSIM.Focus();
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }

                    if (ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.GPRS.ToString())
                    {

                        if (!new Regex("^[0-9]{15}$", RegexOptions.Compiled).Match(textBoxModemIMEI.Text.Trim()).Success)
                        {
                            MessageBox.Show("GPRS Modem IMEI number is invalid.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxModemIMEI.Focus();
                            return false;
                        }


                        string meterno = meterMasterBLL.GetMeterNumber(textBoxModemIMEI.Text.Trim());

                        if (!string.IsNullOrEmpty(meterno) && meterno != cboMeterNumber.Text)
                        {
                            MessageBox.Show("GPRS Modem IMEI  is already associated with " + meterno, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxModemIMEI.Focus();
                            return false;
                        }
                    }

                }

            }

            return Flag = true;
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
            cboMeterType.SelectedIndex = -1;
            cboMeterModel.SelectedItem = null;
            txtMeterLocation.Text = "";
            //DtpInstalled.MaxDate = DateTime.Now;
            txtEMF.Text = defaultValue;
            txtBoxMeterSIM.Text = string.Empty;
            txtContractDemand.Text = "";
            cboUnits.SelectedItem = null;
            //txtMeterCTPrimary.Text = "";
            //txtMeterCTSecondary.Text = "";
            txtMeterCTRatio.Text = defaultValue;
            ////txtMeterPTPrimary.Text = "";
            ////txtMeterPTSecondary.Text = "";
            txtMeterPTRatio.Text = defaultValue;
            txtInstalledCTPrimary.Text = "";
            txtInstalledCTSecondary.Text = "";
            txtInstalledCTRatio.Text = defaultValue;
            txtInstalledPTPrimary.Text = "";
            txtInstalledPTSecondary.Text = "";
            txtInstalledPTRatio.Text = defaultValue;

        }


        void ClearMeterDetails_2()
        {
            cboMeterType.SelectedItem = null;
            cboMeterModel.SelectedItem = null;
            txtMeterLocation.Text = "";
            //DtpInstalled.MaxDate = DateTime.Now;
            txtEMF.Text = "";
            txtContractDemand.Text = "";
            cboUnits.SelectedItem = null;
            //txtMeterCTPrimary.Text = "";
            //txtMeterCTSecondary.Text = "";
            txtMeterCTRatio.Text = "";
            ////txtMeterPTPrimary.Text = "";
            ////txtMeterPTSecondary.Text = "";
            txtMeterPTRatio.Text = "";
            txtInstalledCTPrimary.Text = "";
            txtInstalledCTSecondary.Text = "";
            txtInstalledCTRatio.Text = "";
            txtInstalledPTPrimary.Text = "";
            txtInstalledPTSecondary.Text = "";
            /*VBM - Assign Default value to PTRation tetx box */
            txtInstalledPTRatio.Text = defaultValue;
            /*VBM - Assign Default value to PTRation tetx box */

            //BhardwajG : TFS ID : 117997 EMF Enhancement
            // lables to default if a new inactive meter is selected.
            lblExternalCTRValue.Text = "1";
            lblExternalEMFValue.Text = "1";
            lblExtrenalPTRValue.Text = "1";
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

        private void txtInstalledCTPrimary_TextChanged(object sender, EventArgs e)
        {
            if ((txtInstalledCTPrimary.Text != "") && (txtInstalledCTSecondary.Text != ""))
            {
                if (int.TryParse(txtInstalledCTPrimary.Text, out installedCTPrimary))
                    installedCTPrimary = Convert.ToInt32(txtInstalledCTPrimary.Text);
                if (int.TryParse(txtInstalledCTSecondary.Text, out installedCTSecondary))
                    installedCTSecondary = Convert.ToInt32(txtInstalledCTSecondary.Text);
                if ((installedCTPrimary != 0) && (installedCTSecondary != 0))
                    txtInstalledCTRatio.Text = ValidationProvider.GetDivisionValue(installedCTPrimary, installedCTSecondary).ToString();
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
                if (int.TryParse(txtInstalledCTPrimary.Text, out installedCTPrimary))
                    installedCTPrimary = Convert.ToInt32(txtInstalledCTPrimary.Text);
                if (int.TryParse(txtInstalledCTSecondary.Text, out installedCTSecondary))
                    installedCTSecondary = Convert.ToInt32(txtInstalledCTSecondary.Text);
                if ((installedCTPrimary != 0) && (installedCTSecondary != 0))
                    txtInstalledCTRatio.Text = ValidationProvider.GetDivisionValue(installedCTPrimary, installedCTSecondary).ToString();
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
                if (int.TryParse(txtInstalledPTPrimary.Text, out installedPTPrimary))
                    installedPTPrimary = Convert.ToInt32(txtInstalledPTPrimary.Text);
                if (int.TryParse(txtInstalledPTSecondary.Text, out installedPTSecondary))
                    installedPTSecondary = Convert.ToInt32(txtInstalledPTSecondary.Text);
                if ((installedPTPrimary != 0) && (installedPTSecondary != 0))
                    txtInstalledPTRatio.Text = ValidationProvider.GetDivisionValue(installedPTPrimary, installedPTSecondary).ToString();
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
                if (int.TryParse(txtInstalledPTPrimary.Text, out installedCTPrimary))
                    installedCTPrimary = Convert.ToInt32(txtInstalledPTPrimary.Text);
                if (int.TryParse(txtInstalledPTSecondary.Text, out installedCTSecondary))
                    installedCTSecondary = Convert.ToInt32(txtInstalledPTSecondary.Text);
                if ((installedCTPrimary != 0) && (installedCTSecondary != 0))
                    txtInstalledPTRatio.Text = ValidationProvider.GetDivisionValue(installedCTPrimary, installedCTSecondary).ToString();
                else
                    txtInstalledPTRatio.Text = "";
            }
            else
                txtInstalledPTRatio.Text = "";
        }

        private void ConsumerMeterDetails_Activated(object sender, EventArgs e)
        {
            if (!Mode.Equals("Edit"))
                InitializeRegion();

            mainForm.toolStripStatusLabel.Text = "";
            Application.DoEvents();
            cboMeterNumber.MaxLength = 16;
        }

        private void cboMeterNumber_TextChanged(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void txtInstalledCTRatio_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInstalledCTRatio.Text.Trim()))
            {
                //if(!(string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim)&&!string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim()))&&(string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim)&&!string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim())))
                txtInstalledCTRatio.Text = defaultValue;
            }

            CalculateandSetEMF();
        }

        private void txtInstalledPTRatio_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInstalledPTRatio.Text.Trim()))
            {
                //if(!(string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim)&&!string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim()))&&(string.IsNullOrEmpty(txtInstalledCTSecondary.Text.Trim)&&!string.IsNullOrEmpty(txtInstalledCTPrimary.Text.Trim())))
                txtInstalledPTRatio.Text = defaultValue;
            }
            CalculateandSetEMF();
        }
        private void CalculateandSetEMF()
        {
            decimal internalCTRatioValue = 0;
            decimal internalPTRatioValue = 0;
            if (int.TryParse(txtInstalledCTRatio.Text, out installedCTRatio))
            {
                if (decimal.TryParse(txtMeterCTRatio.Text, out internalCTRatioValue) && internalCTRatioValue > 0 && installedCTRatio > 0)
                {
                    internalCTRatioValue = installedCTRatio / internalCTRatioValue;
                    lblExternalCTRValue.Text = internalCTRatioValue.ToString();
                }
                if (int.TryParse(txtInstalledPTRatio.Text, out installedPTRatio))
                {
                    if (decimal.TryParse(txtMeterPTRatio.Text, out internalPTRatioValue) && internalPTRatioValue > 0 && installedPTRatio > 0)
                    {
                        internalPTRatioValue = installedPTRatio / internalPTRatioValue;
                        lblExtrenalPTRValue.Text = internalPTRatioValue.ToString();
                    }
                    txtEMF.Text = (installedCTRatio * installedPTRatio).ToString();
                }
            }
        }

        private void ddlCommunicationtype_SelectedIndexChanged(object sender, EventArgs e)
        {


            lnglabelIMEI.Visible = false;
            textBoxModemIMEI.Text = String.Empty;
            textBoxModemIMEI.Visible = false;


            //BhardwajG : IF communication type is equal to GSM or PSTN then enable text box
            if (ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.GSM.ToString() || ddlCommunicationtype.SelectedItem.ToString() == CommunicationType.PSTN.ToString())
            {
                txtBoxMeterSIM.Enabled = true;
            }
            else if (ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.GPRS.ToString())
            {
                MessageBox.Show("GPRS is applicable for DLMS meters only !!!");
                lnglabelIMEI.Visible = true;
                textBoxModemIMEI.Visible = true;
                txtBoxMeterSIM.Enabled = true;
            }
            else if (ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.TCP.ToString())
            {
                MessageBox.Show("TCP is applicable for DLMS meters only !!!");
                lnglabelIMEI.Visible = true;
                textBoxModemIMEI.Visible = true;
                txtBoxMeterSIM.Enabled = false;
            }
            else if (ddlCommunicationtype.SelectedItem.ToString().ToUpper() == CommunicationType.FTP.ToString())
            {                
                lnglabelIMEI.Visible = true;
                textBoxModemIMEI.Visible = true;
                txtBoxMeterSIM.Enabled = false;
            }
            else
            {
                txtBoxMeterSIM.Text = string.Empty;
                txtBoxMeterSIM.Enabled = false;

            }
        }

        private void InitializeRegion()
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
                ddlRegion.DataSource = table;
                ddlRegion.DisplayMember = "Region Name";
                ddlRegion.ValueMember = "Region_ID";
            }
        }

        private void InitializeCircle(int regionID)
        {
            CircleBLL circleBLL = new CircleBLL();
            DataSet dataSet = circleBLL.GetCircleDetailData(regionID);
            if (dataSet != null)
            {
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    DataRow row = table.NewRow();
                    row["Circle_Name"] = "";
                    row["Circle_ID"] = -1;
                    table.Rows.InsertAt(row, 0);
                    ddlCircle.DataSource = table;
                    ddlCircle.DisplayMember = "Circle_Name";
                    ddlCircle.ValueMember = "Circle_ID";
                }
                else
                    ddlCircle.DataSource = null;
            }
            else
                ddlCircle.DataSource = null;
        }

        private void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regionID = 0;
            if (ddlRegion.SelectedValue != null)
            {
                if (int.TryParse(ddlRegion.SelectedValue.ToString(), out regionID))
                    InitializeCircle(regionID);
            }
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
                    ddlDivision.DataSource = table;
                    ddlDivision.DisplayMember = "Division_Name";
                    ddlDivision.ValueMember = "Division_ID";
                }
                else
                    ddlDivision.DataSource = null;
            }
            else
                ddlDivision.DataSource = null;
        }

        private void SetCircle(int regionID, int circleID)
        {
            InitializeCircle(regionID);
            ddlCircle.SelectedValue = circleID;
        }

        private void ddlCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            int circleID = 0;
            if (ddlCircle.SelectedValue != null)
            {
                if (int.TryParse(ddlCircle.SelectedValue.ToString(), out circleID))
                {
                    InitializeDivision(circleID);
                }
            }
        }

        private void cboMeterNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            int internalCTratio = 1;
            int internalPTratio = 1;
            int meterModelNumber;
            string meterType = string.Empty;
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
                    if (meterMasterEntity == null)
                    {
                        meterMasterEntity = new MeterMasterEntity();
                        //BhardwajG : Get meter type along with ct and pt ratio
                        meterMasterBLL.GetLatestInternalCTPTRatio(meterId, out internalCTratio, out internalPTratio, out meterType);
                        meterMasterEntity.MeterCTRatio = internalCTratio;
                        meterMasterEntity.MeterPTRatio = internalPTratio;
                        meterMasterEntity.MeterType = meterType;
                    }
                    // Added to fix DLMS_0068
                    else if (meterMasterEntity.MeterCTRatio <= 0 || meterMasterEntity.MeterPTRatio <= 0)
                    {
                        //BhardwajG : Get meter type along with ct and pt ratio
                        meterMasterBLL.GetLatestInternalCTPTRatio(meterId, out internalCTratio, out internalPTratio, out meterType);
                        meterMasterEntity.MeterCTRatio = internalCTratio;
                        meterMasterEntity.MeterPTRatio = internalPTratio;
                        meterMasterEntity.MeterType = meterType;
                    }
                    consumerMeterEntity = consumerMeterBLL.GetDetailData(consumerNumber, meterId) as ConsumerMeterEntity;
                    if (meterMasterEntity == null)
                        meterMasterEntity = new MeterMasterEntity();
                    if (consumerMeterEntity == null)
                        consumerMeterEntity = new ConsumerMeterEntity();
                    ClearMeterDetails_2();
                    DisplayEntityValues(meterMasterEntity);
                    DisplayEntityValues(consumerMeterEntity);
                    //VBM - select Meter model based on meterid automatically. 
                    //if (UtilityDetails.ShowMeterModelNo )
                    //{
                    meterModelNumber = dlm650generalBLL.GetMeterModelNoByMeterID(meterId);


                    //Ruby 6, WB and Sapphire [SC] case     
                    if (meterModelNumber == 6 || meterModelNumber == 9 || meterModelNumber == 11 || meterModelNumber == 17 || meterModelNumber == 18 || meterModelNumber == 24 || meterModelNumber == 25 || meterModelNumber == 12 || meterModelNumber == 13)
                    {
                        meterModelNumber = 1;
                    }
                    if (meterModelNumber == 10)
                    {
                        meterModelNumber = 3;
                    }
                    if (meterModelNumber == 14 || meterModelNumber == 15)
                    {
                        meterModelNumber = 2;
                    }
                    // As WB and Ruby 6.0 is removed as all are WCM 

                    if (meterModelNumber > 6)
                    {
                        meterModelNumber = meterModelNumber - 1;
                    }
                    // As WB and Ruby 6.0 is removed as all are WCM 
                    if (meterMasterEntity != null)
                    {
                        //Ruby 6 case
                        if (meterMasterEntity.MeterModel_ID == 6)
                        {
                            meterMasterEntity.MeterModel_ID = 1;
                        }
                        // Single Phase Csae
                        if (meterMasterEntity.MeterModel_ID > 6)
                        {
                            meterMasterEntity.MeterModel_ID = meterMasterEntity.MeterModel_ID - 1;
                        }
                        cboMeterModel.SelectedIndex = meterMasterEntity.MeterModel_ID == 0 ? meterModelNumber : meterMasterEntity.MeterModel_ID;
                    }
                    else
                    {
                        cboMeterModel.SelectedIndex = meterModelNumber;
                    }
                    cboMeterModel.Enabled = false;
                    //}
                    //BhardwajG : Get meter type from database
                    if (!string.IsNullOrEmpty(meterMasterEntity.MeterType))
                    {
                        if (meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.ThreePhaseThreeWireHTPTCT))
                        {
                            cboMeterType.SelectedIndex = 1;
                        }
                        else if (meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.ThreePhaseFourWire) ||
                            meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.ThreePhaseFourWireHTPTCT) ||
                            meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.ThreePhaseFourWireLTCT) ||
                            meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.ThreePhaseFourWireWCM))
                        {
                            cboMeterType.SelectedIndex = 2;
                        }
                        else if (meterMasterEntity.MeterType.Equals(CAB.Framework.MeterType.OnePhaseTwoWire))
                        {
                            cboMeterType.SelectedIndex = 3;
                        }
                        cboMeterType.Enabled = false;
                    }

                    if (chkShowMeterDeactive.Checked)
                    {
                        chkActivateMeter.Checked = true;
                        chkActivateMeter.Enabled = false;
                    }

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "cboMeterNumber_SelectedIndexChanged(object sender, EventArgs e)", ex);
            }
        }

        private void chkUseEMFInCalculation_CheckedChanged(object sender, EventArgs e)
        {
            if (UtilityDetails.GetUtilityDetails() == CAB.Framework.UtilityEntity.Generic)
            {
                if (chkUseEMFInCalculation.Checked)
                {
                    string strMessage = "By checking ‘Multiply EMF’, installed CT/PT ratio will be multiplied. Are you sure you want to continue?";
                    if (MessageBox.Show(strMessage, "BCS", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        chkUseEMFInCalculation.Checked = false;
                        return;
                    }
                }
            }
        }

        private void txtEMF_TextChanged(object sender, EventArgs e)
        {

            decimal internalEMF = 0;
            decimal internalCTRatio = 0;
            decimal internalPTRatio = 0;
            if (decimal.TryParse(txtMeterCTRatio.Text, out internalCTRatio) && decimal.TryParse(txtMeterPTRatio.Text, out internalPTRatio) && decimal.TryParse(txtEMF.Text, out internalEMF))
            {
                internalCTRatio = internalCTRatio > 0 ? internalCTRatio : 1;
                internalPTRatio = internalPTRatio > 0 ? internalPTRatio : 1;
                internalEMF = internalEMF / (internalCTRatio * internalPTRatio);
                lblExternalEMFValue.Text = internalEMF.ToString();
            }
        }

        private void txtMeterPTRatio_TextChanged(object sender, EventArgs e)
        {
            int internalCT = 1;
            int internalPT = 1;
            if (int.TryParse(txtMeterCTRatio.Text, out internalCT) && (int.TryParse(txtMeterPTRatio.Text, out internalPT)) && internalCT > 0 && internalPT > 0)
            {
                txtInternEMF.Text = (internalPT * internalCT).ToString();
            }
        }

        private void txtMeterCTRatio_TextChanged(object sender, EventArgs e)
        {
            int internalCT = 1;
            int internalPT = 1;
            if (int.TryParse(txtMeterCTRatio.Text, out internalCT) && (int.TryParse(txtMeterPTRatio.Text, out internalPT)) && internalCT > 0 && internalPT > 0)
            {
                txtInternEMF.Text = (internalPT * internalCT).ToString();
            }
        }

    }
}
