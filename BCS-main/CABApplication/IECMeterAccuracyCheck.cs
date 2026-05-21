#region NameSpaces
using System;
using System.Windows.Forms;
using CHANNEL.Formatter;
using CAB.Channel;
using CAB.Framework.Utility;
using CAB.IECChannel.ReadOut;
using CAB.UI.Controls;
using CABEntity;
using Hunt.EPIC.Logging;
#endregion

namespace CABApplication
{
    public partial class IECMeterAccuracyCheck : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        Control group = new Control();
        IECMeterAccuracyCheckEntity MeterAccuracyCheckEntity = new IECMeterAccuracyCheckEntity();
        bool flag = false;
        DateTime startDatetime;
        private System.Resources.ResourceManager resourceMgr;
        bool breakComm = false;
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
        private int isMeterType = 0; // Story - 369686 - To set the variable based on meter type is single phase DLMS

        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(IECMeterAccuracyCheck).ToString());
        #endregion

        public int IsMeterType  // 0 = 1P and 3P dlms ; 1 = SP NonDLMS ; 2 = SP NONDLMS 9600 ; 3 = 3P NON DLMS
        {
            get
            {
                return isMeterType;
            }
            set
            {
                isMeterType = value;
            }
        }
        
        #region Constructor
        public IECMeterAccuracyCheck()
        {
            InitializeComponent();
            // To fill the duration combobox.
            for (int i = 1; i < 61; i++)
            {
                cmbTestduration.Items.Add(i);
            }
            cmbTestduration.SelectedIndex = 0;
            // To create resource file for messages display.
            resourceMgr = new System.Resources.ResourceManager("CABApplication.IECMeterAccuracyCheck", System.Reflection.Assembly.GetExecutingAssembly());

        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        private void IECMeterAccuracyCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            Duration_Timer.Enabled = false;
            this.Cursor = Cursors.Default;
        }

        private void IECMeterAccuracyCheck_FormClosed(object sender, FormClosedEventArgs e)
        {
            Duration_Timer.Enabled = false;
            this.StatusMessage = "";
            SetConnectionDetail(false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Call is made to refresh the textboxes and executing the commands for meter communication.
            Validations();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == resourceMgr.GetString("Stop"))
            {
                int msgres = Convert.ToInt16(MessageBox.Show(resourceMgr.GetString("TestRunning"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
                if (msgres != 6) return;
            }
            this.StatusMessage = "";
            SetConnectionDetail(false);
            this.Close();

        }
        #endregion

        #region Private Methods
        // Added to refresh the text boxes when start button is clicked. 

        /// <summary>
        /// VBM - Used to Display unit for HTCT and LTCT meters
        /// </summary>
        /// <param name="isHTCT"></param>
        private void DisplayUnit(bool isHTCT)
        {
            lblActiveEnergyUnit.Visible = true;
            lblApparentEnergyUnit.Visible = true;
            lblReactiveLagUnit.Visible = true;
            if (IsMeterType == 3)
                lblReactiveLeadUnit.Visible = true; // Story - 369686 - Acccuracy check for single phase IEC

            //if (isHTCT)
            //{
            //    lblActiveEnergyUnit.Text = MegaActiveEnergy;
            //    lblApparentEnergyUnit.Text = MegaApparentEnergy;
            //    lblReactiveLagUnit.Text = MegaReactiveEnergyLagLead;
            //    lblReactiveLeadUnit.Text = MegaReactiveEnergyLagLead;
            //}
        }
        private void Validations()
        {
            if (btnStart.Text == "Start")
            {
                btnStart.Enabled = false;
                this.StatusMessage = "";
                lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                lblduration.Location = new System.Drawing.Point(300, 220);
                lblduration.Visible = true;
                btnStart.Text = "Stop";
                lblduration.Visible = true;
                txtkVAhDelta.Text = "";
                txtkvarhLagDelta.Text = "";
                txtkvarhLeadDelta.Text = "";
                txtkWhDelta.Text = "";
                txtkVAhFinal.Text = "";
                txtkVAhInitial.Text = "";
                txtkvarhLagFinal.Text = "";
                txtkvarhLagInitial.Text = "";
                txtkvarhLeadFinal.Text = "";
                txtkvarhLeadInitial.Text = "";
                txtkWhFinal.Text = "";
                txtkWhInitial.Text = "";
                flag = true;
                ExecuteCommand();
                btnStart.Enabled = true;


            }
            else
            {
                btnStart.Enabled = false;
                btnStart.Text = "Start";
                this.StatusMessage = "";
                flag = false;
                ExecuteCommand();
                btnStart.Enabled = true;
            }
        }
       
        // Added for meter communication. Handshake commands and Meter Accuracy Check commands are sent.
        private void ExecuteCommand()
        {
            ReadMeterAccuracyCheck readmeteraccuracycheck = new ReadMeterAccuracyCheck();
            // When the start is clicked. To get the Intial Readings.
            if (flag)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnStart.Text = "Stop";
                    // To execute Handshake command.
					// Story - 369686 - Acccuracy check for single phase IEC
                    if (!String.IsNullOrEmpty(readmeteraccuracycheck.HandshakeCommandsMeterAccuracyCheck(true,IsMeterType)))
                    {                        
                        string Data = "";
                        string Command = "";

                        if (IsMeterType==1 || IsMeterType==2)
                            Command = "01523102463030422831312903Bcc";
                            //Command = "0152310246303042283131290367";
                        else
                            Command = ConfigInfo.GetMeterAccuracyCheck();

                        // To get the data after Accuracy check command is sent.
                        if (readmeteraccuracycheck.ReadCommandsMeterAccuracyCheck(Command, ref Data, IsMeterType))

                            //this.Cursor = Cursors.Default;
                            startDatetime = DateTime.Now;
                        //if (readmeteraccuracycheck.ReadCommandsMeterAccuracyCheck(Command, ref Data))

                        //    //this.Cursor = Cursors.Default;
                        //    startDatetime = DateTime.Now;
                        readmeteraccuracycheck.BreakCommunication();
                        // Timer starts after getting the first response from meter.
                        Duration_Timer.Start();
                        string data = Data;
                        FormatterMeterAccuracyCheck FormatterMeterAccuracyCheck = new FormatterMeterAccuracyCheck();
                        // To split the data received fro meter.
                        if (IsMeterType == 1 || IsMeterType == 2)
                            FormatterMeterAccuracyCheck.SplitAccuracyCheckForSP(MeterAccuracyCheckEntity, data);
                        else
                            FormatterMeterAccuracyCheck.SplitAccuracyCheck(MeterAccuracyCheckEntity, data);
                        //FormatterMeterAccuracyCheck.SplitAccuracyCheckForSP(MeterAccuracyCheckEntity, data);
                        SetConnectionDetail(true);
                        // To display intial reading of parameters.
                        DisplayInitialReading();
                        DisplayUnit(false);

                    }
                    else
                    {
                        this.StatusMessage = readmeteraccuracycheck.StatusMessage;
                        if (readmeteraccuracycheck.StatusMessage == "Error in opening port")
                        {
                            this.StatusMessage = resourceMgr.GetString("Failure");
                            SetConnectionDetail(false);
                        }

                        this.Cursor = Cursors.Default;
                    }
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    readmeteraccuracycheck.BreakCommunication();
                    this.StatusMessage = "";
                    Duration_Timer.Enabled = false;
                    SetConnectionDetail(false);
                    logger.Log(LOGLEVELS.Error, "ExecuteCommand()", ex);
                }
                finally
                {
                    readmeteraccuracycheck.BreakCommunication();
                    SetConnectionDetail(false);
                    //this.StatusMessage = "";
                }
            }
            // When the Stop is clicked. To get the Final Readings.
            else
            {
                try
                {
                    Application.DoEvents();
                    // To execute Handshake command.
                    if (!breakComm)//This variable declared and condition added on 29 feb 2012 w.r.t. the bug reported
                    {
                        if (!String.IsNullOrEmpty(readmeteraccuracycheck.HandshakeCommandsMeterAccuracyCheck(true,IsMeterType)))
                        {
                            string Command = "";
							//Command = CAB.IECFramework.Utility.ConfigInfo.GetMeterAccuracyCheck();
                            string Data = "";

                            if (IsMeterType==1 ||IsMeterType==2)
                                Command = "01523102463030422831312903Bcc";
                            else
                                Command = CAB.IECFramework.Utility.ConfigInfo.GetMeterAccuracyCheck();
                               
                            
                            // To get the data after Accuracy check command is sent.
                            if (readmeteraccuracycheck.ReadCommandsMeterAccuracyCheck(Command, ref Data,IsMeterType))

                                readmeteraccuracycheck.BreakCommunication();
                            string data = Data;

                            Application.DoEvents();
                            FormatterMeterAccuracyCheck FormatterMeterAccuracyCheck = new FormatterMeterAccuracyCheck();
                            // To split the data received from meter.
                            if (IsMeterType == 1 || IsMeterType == 2) // data length would not be more than 50 in case of single phase non DLMS as it is having only 3 parameters to display
                                FormatterMeterAccuracyCheck.SplitAccuracyCheckForSP(MeterAccuracyCheckEntity, data);
                            else
                            {
                                if (data.Length >= 50)//This if condition added on 29 feb 2012 w.r.t. the bug reported
                                {
                                    //FormatterMeterAccuracyCheck.SplitAccuracyCheck(MeterAccuracyCheckEntity, data);
                                    FormatterMeterAccuracyCheck.SplitAccuracyCheck(MeterAccuracyCheckEntity, data);
                                }
                            }
                            SetConnectionDetail(true);
                            // To display Final reading of parameters.
                            DisplayFinalReading();

                            btnStart.Text = resourceMgr.GetString("Start");
                        }
                        else
                        {
                            this.StatusMessage = readmeteraccuracycheck.StatusMessage;
                            if (readmeteraccuracycheck.StatusMessage == "Error in opening port")
                            {
                                this.StatusMessage = resourceMgr.GetString("Failure");
                                breakComm = true;
                                SetConnectionDetail(false);
                            }
                            btnStart.Text = resourceMgr.GetString("Start");
                        }
                        Application.DoEvents();
                    }

                    // To Calculate Delta readings(Final - Initial). 
                    if (!String.IsNullOrEmpty(txtkVAhFinal.Text) && !String.IsNullOrEmpty(txtkVAhInitial.Text))//The && condition added on 29 feb 2012 w.r.t. the bug reported
                    {
                        txtkVAhDelta.Text = (Convert.ToDecimal(txtkVAhFinal.Text) - Convert.ToDecimal(txtkVAhInitial.Text)).ToString();
                        txtkvarhLagDelta.Text = (Convert.ToDecimal(txtkvarhLagFinal.Text) - Convert.ToDecimal(txtkvarhLagInitial.Text)).ToString();
                        txtkvarhLeadDelta.Text = (Convert.ToDecimal(txtkvarhLeadFinal.Text) - Convert.ToDecimal(txtkvarhLeadInitial.Text)).ToString();
                        txtkWhDelta.Text = (Convert.ToDecimal(txtkWhFinal.Text) - Convert.ToDecimal(txtkWhInitial.Text)).ToString();
                    }
                    Duration_Timer.Enabled = false;

                }
                catch (Exception ex)    //Exception log for catch block
                {
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    Application.DoEvents();
                    readmeteraccuracycheck.BreakCommunication();
                    Duration_Timer.Enabled = false;
                    this.Cursor = Cursors.Default;
                    SetConnectionDetail(false);
                    logger.Log(LOGLEVELS.Error, "ExecuteCommand()", ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    Duration_Timer.Enabled = false;
                    SetConnectionDetail(false);
                    readmeteraccuracycheck.BreakCommunication();
                    //this.StatusMessage = "";
                }
            }
        }

        // To display Initial Readings.
        private bool DisplayInitialReading()
        {
            try
            {

                if (MeterAccuracyCheckEntity.kVAh == 0)         
                    txtkVAhInitial.Text = resourceMgr.GetString("Value");                
                else                
                    txtkVAhInitial.Text = MeterAccuracyCheckEntity.kVAh.ToString();                
                if (MeterAccuracyCheckEntity.kvarhLag == 0)
                    txtkvarhLagInitial.Text = resourceMgr.GetString("Value");
                else
                    txtkvarhLagInitial.Text = MeterAccuracyCheckEntity.kvarhLag.ToString();
                if (MeterAccuracyCheckEntity.kvarhLead == 0)
                    txtkvarhLeadInitial.Text = resourceMgr.GetString("Value");
                else
                    txtkvarhLeadInitial.Text = MeterAccuracyCheckEntity.kvarhLead.ToString();
                if (MeterAccuracyCheckEntity.kWh == 0)
                    txtkWhInitial.Text = resourceMgr.GetString("Value");
                else
                    txtkWhInitial.Text = MeterAccuracyCheckEntity.kWh.ToString();
                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                SetConnectionDetail(false);
                MessageBox.Show(resourceMgr.GetString("Exception Message"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DisplayInitialReading()", ex);
                return false;
            }
        }
        // To display Final Readings.
        private bool DisplayFinalReading()
        {
            try
            {
                if (MeterAccuracyCheckEntity.kVAh == 0)
                    txtkVAhFinal.Text = resourceMgr.GetString("Value");
                else
                    txtkVAhFinal.Text = MeterAccuracyCheckEntity.kVAh.ToString();
                if (MeterAccuracyCheckEntity.kvarhLag == 0)
                    txtkvarhLagFinal.Text = resourceMgr.GetString("Value");
                else
                    txtkvarhLagFinal.Text = MeterAccuracyCheckEntity.kvarhLag.ToString();
                if (MeterAccuracyCheckEntity.kvarhLead == 0)
                    txtkvarhLeadFinal.Text = resourceMgr.GetString("Value");
                else
                    txtkvarhLeadFinal.Text = MeterAccuracyCheckEntity.kvarhLead.ToString();
                if (MeterAccuracyCheckEntity.kWh == 0)
                    txtkWhFinal.Text = resourceMgr.GetString("Value");
                else
                    txtkWhFinal.Text = MeterAccuracyCheckEntity.kWh.ToString();
                return true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                SetConnectionDetail(false);
                MessageBox.Show(resourceMgr.GetString("Exception Message"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "DisplayFinalReading()", ex);
                return false;
            }

        }

        // Added for calculating the elasped time after the timer is enabled.  
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
        /// <summary>
        /// updates protocol , mode and connected/disconnected the right side in status bar  
        /// </summary>
        /// <param name="isConnected"></param>
        private void SetConnectionDetail(bool connected)
        {

            string channelType = ConfigSettings.GetValue("ChannelType");
            string mode;
            if (connected)
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + channelType + "(" + "DLMS" + ")" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;

                Application.DoEvents();
            }
            else
            {

                mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
                this.ConnectionDetailStatusMessageAsync = "Connection: " + "Not Connected" + ", Port: " + ConfigSettings.GetValue("PortName") + ", Mode: " + mode;


            }
        }
        #endregion
		// Story - 369686 - Acccuracy check for single phase IEC
        private void IECMeterAccuracyCheck_Load(object sender, EventArgs e)
        {
            if (IsMeterType == 1 || IsMeterType==2)
            {
                this.label1.Hide();
                this.txtkvarhLeadInitial.Hide();
                this.txtkvarhLeadFinal.Hide();
                this.txtkvarhLeadDelta.Hide();
                this.lblReactiveLeadUnit.Hide();
                //this.lblkWh.Text = "kWh";
				//this.lblkVAh.Text = "kVAh";
				this.lblkvarh.Text = "Reverse Energy";// As per PDM comment these lables should be like below
                this.lblReactiveLagUnit.Text = "kvarh";
            }
        }
    }
}
