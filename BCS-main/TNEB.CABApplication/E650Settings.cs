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
using CAB.IECFramework.Utility;
#endregion

namespace CABApplication
{
    /// <summary>
    /// This form is used for selecting readout/communication mode form BCS .
    /// </summary>
    public partial class E650Settings : MdiChildForm
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
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

        /// <summary>
        /// This is page load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSettings_Load(object sender, EventArgs e)
        {
            BindCommunicationModeCombo();
            BindDefaultSettings();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            String errorMessage = string.Empty; ;
            if (txtPWD.Text.Length == 0)
            {                
                errorMessage = "Please enter Password.";               
                
            }
            if (txtPWD.Text.Length < 16 && cmbMode.Text.Trim() == "Reader (US)")
            {
                errorMessage = "Please enter 16 digit random Key in Password Box";
            }
            // Checking validation first. 2-May-2012
            if (txtPWD.Text.Length < 8 && cmbMode.Text.Trim() == "Reader (MR)")
            {
                errorMessage = "Please enter 8 digit password in Password Box.";                 
            }
            if (errorMessage.Length == 0)
            {
                ConfigSettings.ChangeNode("SecurityMechanism", ((KeyValuePair<string, string>)cmbMode.SelectedItem).Key);
                ConfigSettings.ChangeNode("ModePassword", txtPWD.Text);
                MessageBox.Show("Setting's Saved Successfully.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                this.Close();
            }
            else
            {
                MessageBox.Show(errorMessage, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);                
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
                txtPWD.MaxLength = 16;                                            
            }
            else if (selectedMode == 0x01)
            {
                lbllHLS.Text = "Password";                                                        
                txtPWD.MaxLength = 8;                                            
            }
            txtPWD.Text = "";
        }
       

        #region Private Methods

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

        private void BindDefaultSettings()
        {           
           cmbMode.SelectedValue = ConfigSettings.GetValue("SecurityMechanism");
        }
        #endregion
        

        
    }
}
