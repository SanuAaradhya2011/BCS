using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.UI.Controls;
using System.Management;
using CAB.Framework.Utility;
using CAB.Framework;
using CAB.License;
using CAB.License.DataStore;
using CABApplication;
using System.Configuration;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class RegisterProduct : Form
    {
        IDataStoreManager dataStoreManager = null;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        LicenseStatus productStatus = LicenseStatus.TrialExpired;
        ILicenseManager licenseManager = null;
        bool isValidated = false;
        bool isTrial = false;
        private const int MAXTRIALPERIODDAY = 30;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(RegisterProduct).ToString());
        public RegisterProduct()
        {
            InitializeComponent();
            licenseManager = new LicenseManager();
            dataStoreManager = new DataStoreManager();
        }

        public bool IsTrial
        {
            get
            {
                return isTrial;
            }
            set
            {
                isTrial = value;
            }
        }

        public bool IsValidated
        {
            get
            {
                return isValidated;
            }
            set
            {
                isValidated = value;
            }
        }
        public LicenseStatus ProductStatus
        {
            get
            {
                return productStatus;
            }
            set
            {
                productStatus = value;
            }
        }
        private void RegisterProduct_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            int numberOfHoursElapsed = 1;
            DataStoreInfo dataStoreInfo = dataStoreManager.ReadDataFromRegistry();
            if (dataStoreInfo != null)
            {
                if (dataStoreInfo.NumberOfDaysElapsed > 0)
                {
                    numberOfHoursElapsed = dataStoreInfo.NumberOfDaysElapsed;
                }
                if (dataStoreInfo.NumberOfDaysElapsed > 24 * MAXTRIALPERIODDAY)
                {
                    lblDaysRemaining.Text = CABApplication.Properties.Resources.TRIALEXPIRED;
                }
                else
                {
                    lblDaysRemaining.Text = string.Format(CABApplication.Properties.Resources.DAYSREMAININGFORMATSTR, (24 * MAXTRIALPERIODDAY - numberOfHoursElapsed) / 24) + CABApplication.Properties.Resources.DAYSREMAINING;
                }
            }

        }
        public bool IsAlphaNumericchar(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex(CABApplication.Properties.Resources.ISALPHANUMERICCHAR);

            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }

        public bool IsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaPattern = new Regex(CABApplication.Properties.Resources.ISALPHANUMERIC);

            return !objAlphaPattern.IsMatch(strToCheck);
        }
        private void btnproclose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void ShowRestartMessageIfMainFormIsOpen()
        {
            try
            {
                foreach (Form item in Application.OpenForms)
                {
                    string FormName = item.Name;
                    if (FormName.ToUpper() == "MAINFORM")
                    {
                        MessageBox.Show("Please Restart Application to complete Registration", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "ShowRestartMessageIfMainFormIsOpen()", ex);

            }
        }

        private void ProceedFunctionality()
        {
            isTrial = false;
            if (rdBuyNow.Checked)
            {
                try
                {
                    ProductKey keyForm = new ProductKey();
                    keyForm.ShowDialog();
                    if (keyForm.ProductStatus == LicenseStatus.Activated)
                    {
                        isValidated = true;
                        productStatus = LicenseStatus.Activated;
                        ShowRestartMessageIfMainFormIsOpen();
                        this.Close();
                    }
                }
                catch (Exception Ex)    //Exception log for catch block
                {
                    MessageBox.Show(Ex.ToString(), CABApplication.Properties.Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    logger.Log(LOGLEVELS.Error, "ProceedFunctionality()", Ex);
                }
            }
            else
            {
                isValidated = true;
                isTrial = true;
                //Advance Programming rights not given in Trial Mode.
                ConfigInfo.RightID = "111111110";
                this.Hide();
            }
        }


        public void ImposeProceedFunctionality()
        {
            rdBuyNow.Checked = true;
            ProceedFunctionality();
        }


        private void btnProceed_Click(object sender, EventArgs e)
        {
            ProceedFunctionality();
        }

        private void RegisterProduct_FormClosing(object sender, FormClosingEventArgs e)
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if ((productStatus == LicenseStatus.TrialExpired))
                {
                    btnProceed.Enabled = false;
                }
            }
            else
            {
                btnProceed.Enabled = true;
            }
        }

        private void rdBuyNow_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
