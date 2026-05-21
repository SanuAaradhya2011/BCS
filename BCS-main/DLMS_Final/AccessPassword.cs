using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DLMS_Final
{
    public partial class AccessPassword : Form
    {
        private string _password ; 

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public AccessPassword()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Password = txtBoxPassword.Text;
            
            //Validate the password entered.
            if (!validateScreen())
            {
                return;
            }

            this.Close();
        }

        private void AccessPassword_Load(object sender, EventArgs e)
        {

        }

        private void txtBoxPassword_Enter(object sender, EventArgs e)
        {
           
        }

        private void txtBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter )
            {
                //Validate the password entered.
                if (!validateScreen())
                {
                    return;
                }
                this.Password = txtBoxPassword.Text;
                this.Close();
            }
        }
        /// <summary>
        /// Performs validation on screen and returns true if validation is passed. 
        /// </summary>
        /// <returns></returns>
        private bool validateScreen()
        {
            if (string.IsNullOrEmpty(txtBoxPassword.Text))
            {
                MessageBox.Show("Password can not be empty. Please enter correct Password.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            if (txtBoxPassword.Text != "lng123#")
            {
                MessageBox.Show("Incorrect Password. Please enter correct Password.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
