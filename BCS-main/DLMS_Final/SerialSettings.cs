///****************************************************************************
//'*
//'*  Projet       : DLMS With LTCT
//'*
//'*  Component    : MMP
//'*
//'*  Module       : SerialSettings
//'*
//'*  Environment  : Visual Studio 2008 - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 10/08/09 | Singh, Dhirendra Singh : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialCommunication;

namespace DLMS_Final
{
    public partial class SerialSettings : Form
    {
        SerialComm objSerialComm = new SerialComm();

        public SerialSettings()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SerialSettings_Load(object sender, EventArgs e)
        {
            string[] PortNames = objSerialComm.GetAvailablePorts();
            //Mahadevan has changed on April 30 2009 
            Array.Reverse(PortNames);
            //The following loop is used to display the items in the ComboBox
            foreach (string Port in PortNames)
            {
               cmbAvailableSerialPort.Items.Add(Port);
            }

            // CmbPortName.SelectedItem = "COM1";                 //To select the default COM1 Port

            //AddBaudRates();                                    //Adding Baud Rate to the CmbBaudRate 

            //select the Port settings Value from the File
            //The function reads the PortSettings.XML File and gets the values to the ComboBox as the 
            //Configured ones

            GetExistingPortSettings();

        }
      
        //'******************************************************************************
        //'
        //'  NAME     : GetExistingPortSettings
        //'
        //'  INPUT    : None
        //'
        //'  OUTPUT   : None
        //'
        //'  PURPOSE  : It Reads the PortSettings file and gets the values to the ComboBox
        //'
        //'*******************************************************************************

        private void GetExistingPortSettings()
        {
            cmbAvailableSerialPort.Text = SerialPortSettings.Default.SerialPort;
            //cmbBaudRate.Text = SerialPortSettings.Default.CommandBaudRate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            SerialPortSettings.Default.SerialPort = cmbAvailableSerialPort.Text;
            //SerialPortSettings.Default.CommandBaudRate = "9600";
            

            if (rdBtnRJPort.Checked == true)
            {
                SerialPortSettings.Default.CommunicationPort = rdBtnRJPort.Text;
            }
            else if (rdBtnOpticalPort.Checked == true)
            {
                SerialPortSettings.Default.CommunicationPort = rdBtnOpticalPort.Text;
            }

            if (rdBtnModeE.Checked == true)
            {
                SerialPortSettings.Default.CommunicationMode = rdBtnModeE.Text;
            }
            else if (rdBtnDirectHDLC.Checked == true)
            {
                SerialPortSettings.Default.CommunicationMode = rdBtnDirectHDLC.Text;
            }
            
            SerialPortSettings.Default.Save();
            MessageBox.Show("Settings saved sucessfully !", "Cabcon", MessageBoxButtons.OK,MessageBoxIcon.Information);
            this.Close();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void cmbAvailableSerialPort_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rdBtnOpticalPort_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdBtnRJPort_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
