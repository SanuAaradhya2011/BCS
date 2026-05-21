#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.BLL;
using CABCommunication.PhysicalLayer;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.UI
{
    /// <summary>
    /// This form is used for selecting readout/communication mode form BCS .
    /// </summary>
    public partial class E650Settings : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650Settings).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor

        public E650Settings()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        string Encryptionkey = "000102030405060708090A0B0C0D0E0F";
        string Authenticationkey = "000102030405060708090A0B0C0D0E0F";
        /// <summary>
        /// This is page load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSettings_Load(object sender, EventArgs e)
        {
           // BindCommunicationModeCombo();
            BindDefaultSettings();
            Rs485Default();
            // Added for Socket Programming with HHU for CC integration
            // Date: 12-03-2018
            // Author: Mohsin RAza
            HHUServerDefault();
            CCServerDefault();
            //Enabling FD mode in Generic login also
            //CheckEnabledCommunicationMode();

            if (ConfigSettings.GetValue("OtherManufacture") == "1")
                chkOthersManufacture.Checked = true;
            else
                chkOthersManufacture.Checked = false;

            txtGlobalEncryptionKey.Text = Encryptionkey;
        }
        private void Rs485Default()
        {
            txt485.Text = ConfigSettings.GetValue("RS485DeviceAddress").Trim(); 
        
        }

        // Added for Socket Programming with HHU for CC integration
        // Date: 12-03-2018
        // Author: Mohsin Raza
        private void HHUServerDefault()
        {
            txtservername.Text = ConfigSettings.GetValue("HHUServer").Trim();
            txtportno.Text = ConfigSettings.GetValue("HHUPortNumber").Trim();

        }

        // Added for CC server details to Invoke service
        // Date: 04-04-2018
        // Author: Mohsin Raza
        private void CCServerDefault()
        {
            txtservicename.Text = ConfigSettings.GetValue("EndPointConfigurationName").Trim();
            txtserviceaddress.Text = ConfigSettings.GetValue("RemoteAddress").Trim();
            txtuploadname.Text = ConfigSettings.GetValue("EndPointUploadName").Trim();
            txtuploadaddress.Text = ConfigSettings.GetValue("RemoteUploadAddress").Trim();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String errorMessage = string.Empty; ;
            if (rdNormal.Checked)
            {
                ConfigSettings.ChangeNode("CommunicationMode", CommunicationMode.Normal.ToString());
            }
            else
            {
                ConfigSettings.ChangeNode("CommunicationMode", CommunicationMode.FastDownload.ToString());
            }
            if (rdGSM.Checked)
            {
                ConfigSettings.ChangeNode("ChannelType", ChannelType.GSM.ToString());
            }
            else if (rdPSTN.Checked)
            {
                ConfigSettings.ChangeNode("ChannelType", ChannelType.PSTN.ToString());
            }
            else if (rdGPRS.Checked)
            {
                ConfigSettings.ChangeNode("ChannelType", ChannelType.GPRS.ToString());
            }
            else
            {
                ConfigSettings.ChangeNode("ChannelType", ChannelType.Direct.ToString());
            }

            if (txtPWD.Text.Length == 0)
            {
                errorMessage = "Please enter Password.";

            }


            int RS485DeviceAddressMin = 16;
            int RS485DeviceAddressMax = 16381;
            if (!ValueInBetween(txt485.Text.Trim(), RS485DeviceAddressMin, RS485DeviceAddressMax))
            {
                errorMessage = "Please Enter Valid RS485 Device Address ";
                Application.DoEvents();
                txt485.Focus();

            }
            string AppContext = string.Empty;
            string AuthLevel = string.Empty;
            string securitySuit = string.Empty;
            string dedicatedKey = string.Empty;

            if (rbtnUsermode.Checked == true)// For Normal communication
            {

                AppContext = "01"; //Logical without cipher
                dedicatedKey = "0";// Dedicated key false
                if (cmbMode.Text.Trim() == "Reader (MR)")
                    AuthLevel = "01";  //Low Level 
                else if (cmbMode.Text.Trim() == "Master (US)")
                    AuthLevel = "02";  //High Level 
                else
                    AuthLevel = "00";//No Security
                securitySuit = "10";//Authentication
                if (txtPWD.Text.Length < 16 && cmbMode.Text.Trim() == "Master (US)")
                {
                    errorMessage = "Please enter 16 digit HLS Password";
                }
                // Checking validation first. 2-May-2012
                if (txtGlobalEncryptionKey.Text.Length < 32)
                {
                    errorMessage = "Please enter 16 Byte Global Encryption Key in Hex Format.";
                    txtGlobalEncryptionKey.Focus();
                }
            }

            if (rbtnSecuritymode.Checked == true)// For Cipher communication
            {                
                if (cmbMode.SelectedIndex == 00)
                {
                    AppContext = "03"; //Logical with cipher
                    AuthLevel = "01";  //Low Level 
                    securitySuit = "20";// Encryption
                    dedicatedKey = "0";//Dedicated key false
                }
                else
                {
                    AppContext = "03"; //Logical with cipher
                    AuthLevel = "02";  //High Level 
                    securitySuit = "30";// Encryption
                    dedicatedKey = "0";//Dedicated key false
                }

            }

            if (txtservername.Text == "" || txtservername.TextLength <= 0)
            {
                errorMessage = "Please Enter Valid Host Name";
                Application.DoEvents();
                txtservername.Focus();
            }

            if (txtportno.Text == "" || txtservername.TextLength <= 0)
            {
                errorMessage = "Please Enter Valid Port Number";
                Application.DoEvents();
                txtportno.Focus();
            }
            if (chkOthersManufacture.Checked == true)// For other meter manufacture communication
            {
                ConfigSettings.ChangeNode("OtherManufacture", "TRUE");
            }
            else
            {
                ConfigSettings.ChangeNode("OtherManufacture", "FALSE");
            }


            //____________ For Refrence _______________
            //AppContext = "02"; //Short Name
            //AppContext = "01"; //Logical without cipher
            //AppContext = "03"; //Logical with cipher

            //AuthLevel="00";//No Security
            //AuthLevel="01";  //Low Level   
            //AuthLevel = "02";  //High Level   


            //securitySuit="10";//Authentication
            //securitySuit="20";//Encryption
            //securitySuit="30";//Authentication + Encryption

            //dedicatedKey="1";//Dedicated key true
            //dedicatedKey="0";//Dedicated key false

            if (errorMessage.Length == 0)
            {
                ConfigSettings.ChangeNode("SecurityMechanism", ((KeyValuePair<string, string>)cmbMode.SelectedItem).Key);
                ConfigSettings.ChangeNode("ModePassword", txtPWD.Text);
                ConfigSettings.ChangeNode("DefaultModePassword", txtPWD.Text);
                ConfigSettings.ChangeNode("RS485DeviceAddress", txt485.Text);
                ConfigSettings.ChangeNode("ApplicationContext", AppContext);
                ConfigSettings.ChangeNode("AuthenticationLevel", AuthLevel);
                ConfigSettings.ChangeNode("ClientSystemTitle", "12345678");//Default system title
                ConfigSettings.ChangeNode("SecuritySuit", securitySuit);
                ConfigSettings.ChangeNode("GlobalEncryptionKey", txtGlobalEncryptionKey.Text);
                ConfigSettings.ChangeNode("AuthenticationKey", txtGlobalEncryptionKey.Text);
                ConfigSettings.ChangeNode("DefaultGlobalEncryptionKey", txtGlobalEncryptionKey.Text);
                ConfigSettings.ChangeNode("DefaultAuthenticationKey", txtGlobalEncryptionKey.Text);
                ConfigSettings.ChangeNode("DedicatedKey", dedicatedKey);
                ConfigSettings.ChangeNode("HHUServer", txtservername.Text);
                ConfigSettings.ChangeNode("HHUPortNumber", txtportno.Text);
                ConfigSettings.ChangeNode("EndPointConfigurationName", txtservicename.Text);
                ConfigSettings.ChangeNode("RemoteAddress", txtserviceaddress.Text);
                ConfigSettings.ChangeNode("EndPointUploadName", txtuploadname.Text);
                ConfigSettings.ChangeNode("RemoteUploadAddress", txtuploadaddress.Text);



                MessageBox.Show("Settings Saved Successfully.", "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                this.Close();
            }
            else
            {
                MessageBox.Show(errorMessage, "Cabcon", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

        }

        private bool ValueInBetween(string checkValue, Single minValue, Single maxValue)
        {
            try
            {
                Single temp = 0;

                if (Single.TryParse(checkValue, out temp) == false)
                {
                    return false;
                }

                if (temp >= minValue && temp <= maxValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ValueInBetween(string checkValue, Single minValue, Single maxValue)", ex);
               return false; 
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        #endregion

        /// <summary>
        /// Selction of Mode 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte selectedMode = Convert.ToByte(((KeyValuePair<string, string>)cmbMode.SelectedItem).Key);

            if (selectedMode == 0x02)
            {
                lbllHLS.Text = "HLS Key";                
                if(rbtnSecuritymode.Checked==true)
                 txtPWD.MaxLength = 32;
                else
                 txtPWD.MaxLength = 16;
            }
            else if (selectedMode == 0x01)
            {
                lbllHLS.Text = "Password";
                txtPWD.MaxLength = 16;
            }
            txtPWD.Text = "";
        }


        #region Private Methods

        /// <summary>
        /// If Login is for DLMS utility Then fast download readout support is provided .
        /// </summary>
        private void CheckEnabledCommunicationMode()
        {
            if (UtilityDetails.PrimaryUtlityName == CAB.Framework.UtilityEntity.DLMS.ToString())
            {
                gbComMode.Enabled = true;
                rdGPRS.Visible = true;
            }
            else
            {
                gbComMode.Enabled = false;
            }

        }

        /// <summary>
        /// Used to bind Mode combo box 
        /// </summary>
        private void BindCommunicationModeCombo()
        {
            Dictionary<string, string> comboBoxItems = new Dictionary<string, string>();
            comboBoxItems.Add("01", "Reader (MR)");
            comboBoxItems.Add("02", "Master (US)");
            cmbMode.DataSource = new BindingSource(comboBoxItems, null);
            cmbMode.DisplayMember = "Value";
            cmbMode.ValueMember = "Key";
        }
        private void BindCommunicationModeCipherCombo()
        {
            Dictionary<string, string> comboBoxItems = new Dictionary<string, string>();
            comboBoxItems.Add("01", "Reader (MR)");
            comboBoxItems.Add("02", "Master (US)");
            cmbMode.DataSource = new BindingSource(comboBoxItems, null);
            cmbMode.DisplayMember = "Value";
            cmbMode.ValueMember = "Key";
        }

        private void BindDefaultSettings()
        {
            cmbMode.SelectedValue = ConfigSettings.GetValue("SecurityMechanism");
            string channelType = ConfigSettings.GetValue("ChannelType");
            if (channelType == ChannelType.GSM.ToString())
            {
                rdGSM.Checked = true;
            }
            else if (channelType == ChannelType.PSTN.ToString())
            {
                rdPSTN.Checked = true;
            }
            else if (channelType == ChannelType.GPRS.ToString())
            {
                rdGPRS.Checked = true;
            }
            else
            {
                rdDirect.Checked = true;
            }
            string comMode = ConfigSettings.GetValue("CommunicationMode");
            if (comMode == CommunicationMode.FastDownload.ToString())
            {
                rdFastDownload.Checked = true;
            }
            else
            {
                rdNormal.Checked = true;
            }
            rbtnUsermode.Checked = true;
        }
        #endregion

        private void rbtnSecuritymode_CheckedChanged(object sender, EventArgs e)
        {

            BindCommunicationModeCipherCombo();
            txtPWD.Text = ConfigSettings.GetValue("ModePassword").Trim();
            txtGlobalEncryptionKey.Text = Encryptionkey;
        }

        private void rbtnUsermode_CheckedChanged(object sender, EventArgs e)
        {
            BindCommunicationModeCombo();
            txtPWD.Text = ConfigSettings.GetValue("ModePassword").Trim();
            txtGlobalEncryptionKey.Text = Encryptionkey;
        }

        private void txtportno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }


    }
}
