using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.License.DataStore;
using CAB.License;
using CAB.Framework;
using CAB.Framework.Utility.Cryptography;
namespace CAB.License.KeyGenerator
{
    /// <summary>
    /// This Form is used for Generating the key.
    /// </summary>
    public partial class KeyGenerator : Form
    {
        #region Local Variables
        private OpenFileDialog openFileDialog;
        private LicenseManager licenseManager = null;
        private const string EXCPETIONOCCURED = "Excpetion occured while reading file !";
        private const string CABCON = "Cabcon";
        private const string VALIDLICENSEFILE = "Please select a valid license file";
        CryptoProvider cryptKey = null;
        #endregion

        #region Constructor
        public KeyGenerator()
        {
            licenseManager = new LicenseManager();
            cryptKey = new CryptoProvider();
            InitializeComponent();
        }
        #endregion
        #region Event Handlers

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = Resource.DEFAULTEXTENSION;
            openFileDialog.Filter = Resource.FILTER;
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)          
            {
                if (openFileDialog.FileName.Contains('.'))
                {
                    if (openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf('.')+1).ToLower() != Resource.DEFAULTEXTENSION)
                    {
                        MessageBox.Show(VALIDLICENSEFILE);
                        return;
                    }
                    textBox1.Text = string.Empty;
                    textBox1.Text = openFileDialog.FileName;
                    // Read the file as one string.
                    System.IO.StreamReader myFile =
                       new System.IO.StreamReader(openFileDialog.FileName);
                    string myString = myFile.ReadToEnd();
                    myFile.Close();
                }
            }
        }

        private void BCS_Existing()
        {
            try
            {
                if (openFileDialog != null)
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(openFileDialog.FileName);
                    string myString = myFile.ReadToEnd();
                    DataStoreInfo dataStoreInfo = licenseManager.ReadClientInformationFromFile(openFileDialog.FileName);
                    textBox2.Text = licenseManager.GenerateKey(dataStoreInfo.UniqueClientId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(EXCPETIONOCCURED,CABCON);
                openFileDialog = null;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
            }
        }
        #endregion


        private string GetCheckListData()
        {
            StringBuilder CheckList = new StringBuilder("000000000");
            int startIndex = 0;
            if (chkUserAdministrator.Checked)
            {
                CheckList[startIndex] = '1';
            }
            if (chkDefinition.Checked)
            {
                CheckList[startIndex + 1] = '1';
            }
            if (chkDataReadout.Checked)
            {
                CheckList[startIndex + 2] = '1';
            }
            if (chkSchedule.Checked)
            {
                CheckList[startIndex + 3] = '1';
            }
            if (chkDataArchive.Checked)
            {
                CheckList[startIndex + 4] = '1';
            }
            if (chkProgramming.Checked)
            {
                CheckList[startIndex + 5] = '1';
            }
            if (chkReportsView.Checked)
            {
                CheckList[startIndex + 6] = '1';
            }
            if (chkDataExportImport.Checked)
            {
                CheckList[startIndex + 7] = '1';
            }
            if (chkAdvanceProgramming.Checked)
            {
                CheckList[startIndex + 8] = '1';
            }           
            return CheckList.ToString();
        }

        private void BCS_Version()
        {
            try
            {
                if (openFileDialog != null)
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(openFileDialog.FileName);
                    

                    string myString = myFile.ReadToEnd();
                    DataStoreInfo dataStoreInfo = licenseManager.ReadClientInformationFromFile(openFileDialog.FileName);
                    Random rnd = new Random();
                    string UniqueRighttID = Convert.ToString(rnd.Next(10, 99))
                        + licenseManager.GenerateKey(dataStoreInfo.UniqueClientId)
                        + GetCheckListData();

                    textBox2.Text = licenseManager.GenerateKey(dataStoreInfo.UniqueClientId) + cryptKey.EncryptString(UniqueRighttID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(EXCPETIONOCCURED, CABCON);
                openFileDialog = null;
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBCSVersion.SelectedIndex == 0)
                {
                    BCS_Existing();
                }
                else
                {
                    BCS_Version();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void KeyGenerator_Load(object sender, EventArgs e)
        {
            cmbBCSVersion.SelectedIndex = 0;
        }

        private void EnableDisableCheckbox(bool flag)
        {
            foreach (Control item in grbUserRights.Controls)
            {
                if (item.GetType() == typeof(CheckBox))
                {
                    CheckBox chk = (CheckBox)item;
                    chk.Checked = false;
                    chk.Enabled = flag;
                }
            }

        }


        private void cmbBCSVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBCSVersion.SelectedIndex == 0)
            {
                EnableDisableCheckbox(false);
            }
            else
            {
                EnableDisableCheckbox(true);
            }
        }
       
    }
}
