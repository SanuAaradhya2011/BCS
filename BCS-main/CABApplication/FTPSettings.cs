#region NameSpaces
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using CABCommunication.Common;
using CABCommunication.PhysicalLayer;
using SerialCommunication;
using System.Text;
using System.Net;
using Hunt.EPIC.Logging;
//using CABApplication;
#endregion


namespace CABApplication
{
    public partial class FTPSettings : MdiChildForm
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(FTPSettings).ToString());
        public FTPSettings()
        {
            InitializeComponent();
        }


        private bool ValidateControl()
        {
            bool result = false;
            try
            {

                IPAddress address;
                if (IPAddress.TryParse(txtFtpIP.Text.Trim(), out address))
                {
                    //Valid IP, with address containing the IP
                    result = true;
                }
                else
                {
                    MessageBox.Show("Enter Valid IP Address!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "ValidateControl()", ex);
            }
            return result;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {


                if (ValidateControl())
                {
                    ConfigSettings.ChangeNode("FTPIP", txtFtpIP.Text.Trim());
                    ConfigSettings.ChangeNode("LoginID", txtuserID.Text.Trim());
                    ConfigSettings.ChangeNode("LoginPassword", txtftpPassword.Text.Trim());
                    ConfigSettings.ChangeNode("ServerDirectory", txtftpDirectoryName.Text.Trim());
                    ConfigSettings.ChangeNode("IsAutoUpload", chkAutoUpload.Checked.ToString());


                    MessageBox.Show("Settings Saved Succesfully!", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    this.Close();

                }



            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                logger.Log(LOGLEVELS.Error, "btnSave_Click(object sender, EventArgs e)", Ex);
            }

        }

        private void FTPSettings_Load(object sender, EventArgs e)
        {
            try
            {
                txtFtpIP.Text = ConfigSettings.GetValue("FTPIP");
                txtuserID.Text = ConfigSettings.GetValue("LoginID");
                txtftpPassword.Text = ConfigSettings.GetValue("LoginPassword");
                txtftpDirectoryName.Text = ConfigSettings.GetValue("ServerDirectory");
                chkAutoUpload.Checked = Convert.ToBoolean(ConfigSettings.GetValue("IsAutoUpload"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FTPSettings_Load(object sender, EventArgs e)", ex);
                return;
            }
        }
    }
}
