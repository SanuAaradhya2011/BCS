using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECChannel.Programming;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using CAB.IECChannel.Programming;
using CAB.IECChannel.ReadOut;
using CAB.IECChannel.Formatter;
using CABAppControl;
using CAB.Contracts;
using CAB.Entity;
using CAB.BLL;
using CAB.IECFramework.Entity;
using CABEntity;
using LTCTBLL;
using CHANNEL.Formatter;
using CAB.IECChannel;
using CAB.IECFramework.Utility;



namespace CABApplication
{
    public partial class MeterAccuracyCheck : MdiChildForm   
    {
        Control group = new Control();
        MeterAccuracyCheckEntity MeterAccuracyCheckEntity = new MeterAccuracyCheckEntity();
        bool flag = false;
        DateTime startDatetime;
        private LocalCommunication  communications;
        private System.Resources.ResourceManager resourceMgr;
        bool breakComm = false;
                 
        public MeterAccuracyCheck()
        {
            InitializeComponent();
            // To fill the duration combobox.
            for (int i = 1; i < 61; i++)
            {
                cmbTestduration.Items.Add(i);
            }
            cmbTestduration.SelectedIndex = 0;
            // To create resource file for messages display.
            resourceMgr = new System.Resources.ResourceManager("CABApplication.MeterAccuracyCheck", System.Reflection.Assembly.GetExecutingAssembly());
           
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void MeterAccuracyCheck_Load(object sender, EventArgs e)
        {
            
        }
        private void MeterAccuracyCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            Duration_Timer.Enabled = false;
            this.Cursor = Cursors.Default;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbTestduration_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            
        }
        // Added to refresh the text boxes when start button is clicked. 
        private void Validations()
        {
            if (btnStart.Text == "Start")
            {
                this.StatusMessage = "";
                lblduration.Text = resourceMgr.GetString("Duration") + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero") + ":" + resourceMgr.GetString("Zero");
                lblduration.Visible = true;
                //btnStart.Text = "Stop";
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


            }
            else
            {
                this.StatusMessage = "";
                flag = false;
                ExecuteCommand();
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Call is made to refresh the textboxes and executing the commands for meter communication.
            Validations();
               
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
                    // To execute Handshake command.
                    if (!String.IsNullOrEmpty(readmeteraccuracycheck.HandshakeCommandsMeterAccuracyCheck(true)))
                    {
                        btnStart.Text = resourceMgr.GetString("Stop");
                        string Data = "";
                        string Command = "";
                        Command = ConfigInfo.GetMeterAccuracyCheck();

                        // To get the data after Accuracy check command is sent.
                        if (readmeteraccuracycheck.ReadCommandsMeterAccuracyCheck(Command, ref Data))

                            //this.Cursor = Cursors.Default;
                            startDatetime = DateTime.Now;
                        readmeteraccuracycheck.BreakCommunication();
                        // Timer starts after getting the first response from meter.
                        Duration_Timer.Start();
                        string data = Data;
                        FormatterMeterAccuracyCheck FormatterMeterAccuracyCheck = new FormatterMeterAccuracyCheck();
                        // To split the data received fro meter.
                        FormatterMeterAccuracyCheck.SplitAccuracyCheck(MeterAccuracyCheckEntity, data);
                        // To display intial reading of parameters.
                        DisplayInitialReading();

                    }
                    else
                    {
                        this.StatusMessage = readmeteraccuracycheck.StatusMessage;
                        if (readmeteraccuracycheck.StatusMessage == "Error in opening port")
                        {
                            this.StatusMessage = resourceMgr.GetString("Failure");
                        }

                        this.Cursor = Cursors.Default;
                    }                 
                }
                catch (Exception ex)
                {
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    readmeteraccuracycheck.BreakCommunication();
                    this.StatusMessage = "";
                    Duration_Timer.Enabled = false;
                }
                finally
                 {
                    readmeteraccuracycheck.BreakCommunication();
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
                        if (!String.IsNullOrEmpty(readmeteraccuracycheck.HandshakeCommandsMeterAccuracyCheck(true)))
                        {
                            string Command = "";
                            Command = ConfigInfo.GetMeterAccuracyCheck();
                            string Data = "";
                            // To get the data after Accuracy check command is sent.
                            if (readmeteraccuracycheck.ReadCommandsMeterAccuracyCheck(Command, ref Data))

                                readmeteraccuracycheck.BreakCommunication();
                            string data = Data;
                            
                            Application.DoEvents();
                            FormatterMeterAccuracyCheck FormatterMeterAccuracyCheck = new FormatterMeterAccuracyCheck();
                            // To split the data received from meter.
                            if (data.Length >= 50)//This if condition added on 29 feb 2012 w.r.t. the bug reported
                                FormatterMeterAccuracyCheck.SplitAccuracyCheck(MeterAccuracyCheckEntity, data);
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
                catch (Exception ex)
                {
                    this.StatusMessage = resourceMgr.GetString("Failure");
                    Application.DoEvents();
                    readmeteraccuracycheck.BreakCommunication();
                    Duration_Timer.Enabled = false;
                    this.Cursor = Cursors.Default;
                }
                finally
               {
                    this.Cursor = Cursors.Default;
                    Duration_Timer.Enabled = false;
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
                {
                    txtkVAhInitial.Text = resourceMgr.GetString("Value");
                }
                else
                {
                    txtkVAhInitial.Text = MeterAccuracyCheckEntity.kVAh.ToString();
                }
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
            catch (Exception)
            {
                MessageBox.Show(resourceMgr.GetString("Exception Message"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
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
            catch (Exception)
            {
                MessageBox.Show(resourceMgr.GetString("Exception Message"), resourceMgr.GetString("RubyE250"), MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return false;
            }

        }

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

        private void MeterAccuracyCheck_FormClosed(object sender, FormClosedEventArgs e)
        {
            Duration_Timer.Enabled = false;
            this.StatusMessage = "";
        }
    }
}





          
  

