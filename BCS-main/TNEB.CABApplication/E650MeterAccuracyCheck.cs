#region NameSpaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABEntity;
using CAB.EntityGenerator;
using CAB.Mapper;
using CABCommunication.WrapperLayer;
using CAB.IECFramework.Utility;
using CAB.Parser;
using CAB.IECFramework;
using CAB.Serialization;
using CAB.BLL;
#endregion
namespace CABApplication
{
    /// <summary>
    /// Meter Accuracy Check 
    /// </summary>
    public partial class E650MeterAccuracyCheck : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private System.Resources.ResourceManager resourceMgr;
        private const string START = "Start";
        private const string STOP = "Stop";
        DateTime startDatetime;
        private List<byte> meterId = null;
        private MeterAccuracyCheckEntity meterAccuracyCheckEntity;
        private Communication communication;       
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public E650MeterAccuracyCheck()
        {
            InitializeComponent();

            communication = new Communication(ConfigSettings.GetValue("PortName"),
                                             Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")),
                                             ConfigSettings.GetValue("ModePassword"));

            // To fill the duration combobox.
            for (int i = 1; i < 61; i++)
            {
                cmbTestduration.Items.Add(i);
            }
            cmbTestduration.SelectedIndex = 0;
            // To create resource file for messages display.
            resourceMgr = new System.Resources.ResourceManager("CABApplication.MeterAccuracyCheck", System.Reflection.Assembly.GetExecutingAssembly());
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == resourceMgr.GetString("Stop"))
            {
                int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("TestRunning"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (msgres != 6) return;
            }
            this.StatusMessage = "";
            this.Close();
        }
        /// <summary>
        /// Meter Accuracy check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            ValidateParameters();
        }
        /// <summary>
        /// Closing form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E650MeterAccuracyCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = string.Empty;
        }
        /// <summary>
        /// Added for calculating the elasped time after the timer is enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Duration_Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan dtDuration = DateTime.Now - startDatetime;

            if (cmbTestduration.Text != "")
            {
                if (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) + 3 == Convert.ToInt32(cmbTestduration.Text) * 60)
                {
                    //Duration_Timer.Stop();
                    btnStart_Click(this, e);
                    Duration_Timer.Stop();
                    dtDuration = DateTime.Now - startDatetime;

                    if ((((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) > Convert.ToInt32(cmbTestduration.Text) * 60) || (((dtDuration.Seconds) + (dtDuration.Minutes * 60) + (dtDuration.Hours * 3600)) < Convert.ToInt32(cmbTestduration.Text) * 60))
                    {
                        if (Convert.ToInt32(cmbTestduration.Text) < 60)
                        {
                            lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + Convert.ToInt32(cmbTestduration.Text).ToString(resourceMgr.GetString("Zero")) + ":" + resourceMgr.GetString("Zero");
                        }
                        else
                        {
                            lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("One") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                        }
                    }
                    else
                    {
                        lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
                    }
                }
                else
                {
                    dtDuration = DateTime.Now - startDatetime;
                    lblduration.Text = resourceMgr.GetString("Duration") + dtDuration.Hours.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Minutes.ToString(resourceMgr.GetString("Zero")) + ":" + dtDuration.Seconds.ToString(resourceMgr.GetString("Zero"));
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This method is used for clear all the controls when accuracy check is started.
        /// </summary>
        private void ValidateParameters()
        {
            if (btnStart.Text.Equals(START))
            {
                this.StatusMessage = string.Empty;
                btnStart.Text = STOP;
                cmbTestduration.Enabled = false;
                Application.DoEvents();
                lblduration.Visible = true;
                lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                txtkVAhDelta.Text = string.Empty;
                txtkvarhLagDelta.Text = string.Empty;
                txtkvarhLeadDelta.Text = string.Empty;
                txtkWhDelta.Text = string.Empty;
                txtkVAhFinal.Text = string.Empty;
                txtkVAhInitial.Text = string.Empty;
                txtkvarhLagFinal.Text = string.Empty;
                txtkvarhLagInitial.Text = string.Empty;
                txtkvarhLeadFinal.Text = string.Empty;
                txtkvarhLeadInitial.Text = string.Empty;
                txtkWhFinal.Text = string.Empty;
                txtkWhInitial.Text = string.Empty;

                txtReversekVAhDelta.Text = string.Empty;
                txtReversekVAhInitial.Text = string.Empty;
                txtReversekVAhFinal.Text = string.Empty;

                txtReversekvarhLagInitial.Text = string.Empty;
                txtReversekvarhLagDelta.Text = string.Empty;                
                txtReversekvarhLagFinal.Text = string.Empty;

                txtReversekWhDelta.Text = string.Empty;
                txtReversekWhFinal.Text = string.Empty;
                txtReversekWhInitial.Text = string.Empty;                  
               
                txtReversekvarhLeadFinal.Text = string.Empty;
                txtReversekvarhLeadInitial.Text = string.Empty;
                txtReversekvarhLeadDelta.Text = string.Empty;                            
                Application.DoEvents();
                // To start reading parameters on start of accuracy check.
                StartMeterAccuracyCheck();
            }
            else
            {
                this.StatusMessage = string.Empty;               
                Application.DoEvents();
                // To start reading parameters on stop of accuracy check.
                StopAccuracyCheck();
                cmbTestduration.Enabled = true;
            }

        }
        /// <summary>
        /// Final reading
        /// </summary>
        private void StopAccuracyCheck()
        {
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            bool isConnected = false;
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            FraudEnergy mapperFraudEnergy = new FraudEnergy();
            try
            {
                btnStart.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                Result result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        string idLength = result.RecieveDataBuffer[1].ToString("00");
                        int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        meterId = new List<byte>();
                        meterId = result.RecieveDataBuffer.GetRange(2, index);
                        lstProfileCommands = GetProfileCommandEntity();
                        List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                        {
                            return profileCommandEntity.TagNumber == (byte)ProfileId.FraudEnergy
                            && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                            profileCommandEntity.MeterModelNumber == 0);
                        });

                        profileReadCommands[0].Action = ActionType.READ;
                        profileReadCommands[0].MeterID = meterId;

                        try
                        {

                            result = communication.Send(profileReadCommands[0]);
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                resultData.Append(String.Format("{0:X2}", profileReadCommands[0].ClassId)
                                       + profileReadCommands[0].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                              + String.Format("{0:X2}", profileReadCommands[0].Attribute));
                                for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                {
                                    resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                }
                                CAB.Mapper.MeterAccuracyCheck mapperMeterAccuracy = new CAB.Mapper.MeterAccuracyCheck();
                                List<ProfileData> meterAccuracyData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), true);
                                List<MeterAccuracyCheckEntity> meterAccuracyCheckEntity = mapperMeterAccuracy.GetData(meterAccuracyData);
                                Application.DoEvents();
                                if (meterAccuracyCheckEntity.Count > 0)
                                {
                                    DisplayFinalReading(meterAccuracyCheckEntity[0]);
                                    cmbTestduration.Enabled = true;
                                    DisplayDeltaValues();
                                    btnStart.Text = START;
                                    Duration_Timer.Stop();
                                    this.Cursor = Cursors.Default;
                                }
                            }                            
                        }
                        catch (Exception)
                        {
                            btnStart.Text = START;
                            cmbTestduration.Enabled = true;
                            this.Cursor = Cursors.Default;
                            Duration_Timer.Stop();
                            MessageBox.Show("Error in Reading Fraud Energy", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }
                    
                }
                if(result.ErrorCode != CommunicationErrorType.Success)
                {
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    btnStart.Text = START;
                    cmbTestduration.Enabled = true;
                    this.Cursor = Cursors.Default;
                    Duration_Timer.Stop();
                    Application.DoEvents();
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Reading Fraud Energy", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (isConnected)
                {
                    communication.CloseSession();
                }
                this.Cursor = Cursors.Default;
                btnStart.Enabled = true;
                Application.DoEvents();

            }
        }        
        /// <summary>
        /// Meter Acccuracy check read
        /// </summary>
        private void StartMeterAccuracyCheck()
        {
            string meterID = string.Empty;
            string lngFileName = string.Empty;
            string downloadedData = string.Empty;
            bool isConnected = false;
            List<ProfileCommand> lstProfileCommands;
            StringBuilder resultData = new StringBuilder();
            GenerateEntity entityGenerator = new GenerateEntity();
            FraudEnergy mapperFraudEnergy = new FraudEnergy();
            try
            {
               // btnStart.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                ProfileCommand profileCommand = new ProfileCommand(01, "00.00.60.01.00.FF", 02);
                startDatetime = DateTime.Now;                
                Duration_Timer.Start();
                Result result = communication.OpenSession();
                if (result.ErrorCode == CommunicationErrorType.Success)
                {
                    isConnected = true;
                    result = communication.Send(profileCommand);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        string idLength = result.RecieveDataBuffer[1].ToString("00");
                        int index = Convert.ToInt16(result.RecieveDataBuffer[1]);
                        meterId = new List<byte>();
                        meterId = result.RecieveDataBuffer.GetRange(2, index);
                        lstProfileCommands = GetProfileCommandEntity();
                        List<ProfileCommand> profileReadCommands = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                        {
                            return profileCommandEntity.TagNumber == (byte)ProfileId.FraudEnergy
                            && (profileCommandEntity.MeterModelNumber == NamePlateConstants.PumaLTE650Value ||
                            profileCommandEntity.MeterModelNumber == 0);
                        });

                        profileReadCommands[0].Action = ActionType.READ;
                        profileReadCommands[0].MeterID = meterId;

                        try
                        {

                            result = communication.Send(profileReadCommands[0]);
                            if (result.ErrorCode == CommunicationErrorType.Success)
                            {
                                resultData.Append(String.Format("{0:X2}", profileReadCommands[0].ClassId)
                                       + profileReadCommands[0].ObisCode.Replace(".", "").ToUpper().Replace("METERID", "FF")
                                                              + String.Format("{0:X2}", profileReadCommands[0].Attribute));
                                for (int counter = 0; counter < result.RecieveDataLength; counter++)
                                {
                                    resultData.Append(String.Format("{0:X2}", result.RecieveDataBuffer[counter]));
                                }
                                CAB.Mapper.MeterAccuracyCheck mapperMeterAccuracy = new CAB.Mapper.MeterAccuracyCheck();
                                List<ProfileData> meterAccuracyData = entityGenerator.GetProfileWiseEntityList(resultData.ToString(), true);
                                List<MeterAccuracyCheckEntity> meterAccuracyCheckEntity = mapperMeterAccuracy.GetData(meterAccuracyData);
                                Application.DoEvents();
                                if (meterAccuracyCheckEntity.Count > 0)
                                {
                                    DisplayInitialReading(meterAccuracyCheckEntity[0]);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Error in Reading Fraud Energy", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }                    
                }
                if(result.ErrorCode != CommunicationErrorType.Success)
                {
                    
                    btnStart.Text = START;
                    Duration_Timer.Stop();
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(CommonBLL.GetEnumDescription(result.ErrorCode), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                this.StatusMessage = resourceMgr.GetString("Failure");
                MessageBox.Show("Error in Reading Fraud Energy", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (isConnected)
                {
                    communication.CloseSession();
                }
                this.Cursor = Cursors.Default;
                btnStart.Enabled = true;
                Application.DoEvents();
            }
        }
        /// <summary>
        /// Filll initial Data to coiltrols 
        /// </summary>
        /// <param name="meterAccuracyCheckEntity"></param>
        private void DisplayInitialReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            //Normal Energies 
            txtkVAhInitial.Text = meterAccuracyCheckEntity.kVAh.ToString();          
            txtkvarhLagInitial.Text = meterAccuracyCheckEntity.kvarhLag.ToString();           
            txtkvarhLeadInitial.Text = meterAccuracyCheckEntity.kvarhLead.ToString();           
            txtkWhInitial.Text = meterAccuracyCheckEntity.kWh.ToString();

            //Reverse Energies 
            txtReversekVAhInitial.Text = meterAccuracyCheckEntity.ReversekVAh.ToString();
            txtReversekWhInitial.Text = meterAccuracyCheckEntity.ReversekWh.ToString();
            txtReversekvarhLagInitial.Text = meterAccuracyCheckEntity.ReversekvarhLag.ToString();
            txtReversekvarhLeadInitial.Text = meterAccuracyCheckEntity.ReversekvarhLead.ToString();            
            
            
        }
        /// <summary>
        /// Filll final data to controls
        /// </summary>
        /// <param name="meterAccuracyCheckEntity"></param>
        private void DisplayFinalReading(MeterAccuracyCheckEntity meterAccuracyCheckEntity)
        {
            //Normal Energies 
            txtkVAhFinal.Text = meterAccuracyCheckEntity.kVAh.ToString();
            txtkvarhLagFinal.Text = meterAccuracyCheckEntity.kvarhLag.ToString();
            txtkvarhLeadFinal.Text = meterAccuracyCheckEntity.kvarhLead.ToString();
            txtkWhFinal.Text = meterAccuracyCheckEntity.kWh.ToString();

            //Reverse Energies 
            txtReversekVAhFinal.Text = meterAccuracyCheckEntity.ReversekVAh.ToString();
            txtReversekWhFinal.Text = meterAccuracyCheckEntity.ReversekWh.ToString();
            txtReversekvarhLagFinal.Text = meterAccuracyCheckEntity.ReversekvarhLag.ToString();
            txtReversekvarhLeadFinal.Text = meterAccuracyCheckEntity.ReversekvarhLead.ToString();


        }
        /// <summary>
        /// Fill Delta data to controls 
        /// </summary>
        private void DisplayDeltaValues()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtkVAhFinal.Text) && !String.IsNullOrEmpty(txtkVAhInitial.Text))
                {
                    txtkVAhDelta.Text = (Convert.ToDecimal(txtkVAhFinal.Text) - Convert.ToDecimal(txtkVAhInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLagFinal.Text) && !String.IsNullOrEmpty(txtkvarhLagInitial.Text))
                {
                    txtkvarhLagDelta.Text = (Convert.ToDecimal(txtkvarhLagFinal.Text) - Convert.ToDecimal(txtkvarhLagInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkvarhLeadFinal.Text) && !String.IsNullOrEmpty(txtkvarhLeadInitial.Text))
                {
                    txtkvarhLeadDelta.Text = (Convert.ToDecimal(txtkvarhLeadFinal.Text) - Convert.ToDecimal(txtkvarhLeadInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtkWhFinal.Text) && !String.IsNullOrEmpty(txtkWhInitial.Text))
                {                    
                    txtkWhDelta.Text = (Convert.ToDecimal(txtkWhFinal.Text) - Convert.ToDecimal(txtkWhInitial.Text)).ToString();
                }


                if (!String.IsNullOrEmpty(txtReversekVAhFinal.Text) && !String.IsNullOrEmpty(txtReversekVAhInitial.Text))
                {
                    txtReversekVAhDelta.Text = (Convert.ToDecimal(txtReversekVAhFinal.Text) - Convert.ToDecimal(txtReversekVAhInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtReversekvarhLagFinal.Text) && !String.IsNullOrEmpty(txtReversekvarhLagInitial.Text))
                {
                    txtReversekvarhLagDelta.Text = (Convert.ToDecimal(txtReversekvarhLagFinal.Text) - Convert.ToDecimal(txtReversekvarhLagInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtReversekvarhLeadFinal.Text) && !String.IsNullOrEmpty(txtReversekvarhLeadInitial.Text))
                {
                    txtReversekvarhLeadDelta.Text = (Convert.ToDecimal(txtReversekvarhLeadFinal.Text) - Convert.ToDecimal(txtReversekvarhLeadInitial.Text)).ToString();
                }
                if (!String.IsNullOrEmpty(txtReversekWhFinal.Text) && !String.IsNullOrEmpty(txtReversekWhInitial.Text))
                {
                    txtReversekWhDelta.Text = (Convert.ToDecimal(txtReversekWhFinal.Text) - Convert.ToDecimal(txtReversekWhInitial.Text)).ToString();
                }
            }
            catch
            {

            }
        }              
        /// <summary>
        /// Used to Get commands for reading profiles from xml file and deserialize 
        /// that into list of ProFileCommand as return value.
        /// </summary>
        /// <returns></returns>
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                profileCommandEntity.MeterModelNumber = Convert.ToByte(dlmsCommand.METERMODEL);
                lstProfileCommands.Add(profileCommandEntity);
            }
            return lstProfileCommands;
        }        
        #endregion        

    }
}
