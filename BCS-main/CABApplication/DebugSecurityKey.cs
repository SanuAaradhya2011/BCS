using CAB.Framework.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CABApplication
{
    public partial class DebugSecurityKey : Form
    {
        public DebugSecurityKey()
        {
            InitializeComponent();
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtPlainLLS.Text = "";
                txtPlainEncryption.Text = "";
                txtPlainHLS.Text = "";
                List<string> CipherKeys = new List<string>();
                CipherKeys.Add("12345678");//Default MeterID
                CipherKeys.Add(txtCipherLLS.Text.Trim());
                CipherKeys.Add(txtCipherEncryption.Text.Trim());
                CipherKeys.Add(txtCipherHLS.Text.Trim());
                string RSAprivateKey = ConfigSettings.GetValue("PrivateKey");
                if (RSAprivateKey.Length <=0)
                {
                    MessageBox.Show("Unable To Decrypt, Private Key Can't be Blank !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                List<string> plainKeys = Security_Key.SecurityKeyManager.GetDecryptedSecurityKeys(CipherKeys, RSAprivateKey);
                if (plainKeys == null)
                {
                    MessageBox.Show("Unable To Decrypt, Inputs may not correct !", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                 
                    if (plainKeys.Count > 1) txtPlainLLS.Text = plainKeys[1];
                    if (plainKeys.Count > 2) txtPlainEncryption.Text = plainKeys[2];
                    if (plainKeys.Count > 3) txtPlainHLS.Text = plainKeys[3]; //--No Implemented yet
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
           
        }
    }
}
