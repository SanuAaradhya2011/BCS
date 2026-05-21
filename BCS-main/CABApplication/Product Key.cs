using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using CAB.UI.Controls;
using System.Management;
using CAB.Framework.Utility;
using CAB.License;
using CAB.Framework;
using CABApplication.Properties;
using CAB.License.DataStore;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class ProductKey : Form
    {
        private LicenseStatus productStatus;
        ILicenseManager licenseManager;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ProductKey).ToString());
        public ProductKey()
        {
            InitializeComponent();
            productStatus = LicenseStatus.Trial;
            licenseManager = new LicenseManager();
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
        /* private void btnprofinish_Click(object sender, EventArgs e)
         {
             try
             {
                 if (txt_ProductCode.Text.Trim() == string.Empty)
                 {
                     MessageBox.Show(Resources.KEYCANNOTBEBLANK, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     txt_ProductCode.Focus();
                     return;
                 }
                 string chkalphanum = txt_ProductCode.Text.Replace(Symbols.HYPHEN, string.Empty);
                 if (!IsAlphaNumericchar(chkalphanum))
                 {

                     MessageBox.Show(Resources.INVALIDKEY, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                     txt_ProductCode.Focus();
                     return;
                 }
                 if (licenseManager.MatchKey(txt_ProductCode.Text))
                 {
                     productStatus = LicenseStatus.Activated;
                     this.Close();
                 }
                 else
                 {
                     productStatus = LicenseStatus.Error;
                     MessageBox.Show(Resources.INVALIDKEY, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                     return;
                 }


             }
             catch (Exception Ex)
             {
                 MessageBox.Show(Ex.Message, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);

             }


         }*/
        public bool IsAlphaNumericchar(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex(Resources.ALPHANUMERICCHARREGEX);
            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }

        public bool IsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaPattern = new Regex(Resources.ALPHANUMERICREGEX);
            return !objAlphaPattern.IsMatch(strToCheck);
        }

        private void GrpCompanyDetail_Enter(object sender, EventArgs e)
        {

        }

        private void ProductKey_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
            DataStoreInfo dataStoreInfo = new DataStoreInfo();
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = Resources.LCSFILEFILTER;
            savefile.RestoreDirectory = true;
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                dataStoreInfo.NumberOfDaysElapsed = 0;
                dataStoreInfo.LicenseFileName = savefile.FileName;
                dataStoreInfo.StartDateTime = DateTime.Now;
                licenseManager.WriteClientInformationToFile(savefile.FileName, new CAB.License.DataStore.DataStoreInfo());
                MessageBox.Show(Resources.SENDFILETOVENDOR, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }*/

        // Updated for getting Mac ID for Eathernet
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataStoreInfo dataStoreInfo = new DataStoreInfo();
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = Resources.LCSFILEFILTER;
                savefile.RestoreDirectory = true;
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    dataStoreInfo.NumberOfDaysElapsed = 0;
                    dataStoreInfo.LicenseFileName = savefile.FileName;
                    dataStoreInfo.StartDateTime = DateTime.Now;
                    //licenseManager.WriteClientInformationToFile(savefile.FileName, new CAB.License.DataStore.DataStoreInfo());
                    licenseManager.WriteClientInformationToFile(savefile.FileName, dataStoreInfo);
                    MessageBox.Show(Resources.SENDFILETOVENDOR, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                logger.Log(LOGLEVELS.Error, "button2_Click(object sender, EventArgs e)", ex);
            }
        }

        private void btnValidateNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_ProductCode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show(Resources.KEYCANNOTBEBLANK, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_ProductCode.Focus();
                    return;
                }
                if (licenseManager.MatchKey(txt_ProductCode.Text))
                {
                    productStatus = LicenseStatus.Activated;
                    this.Close();
                }
                else
                {
                    productStatus = LicenseStatus.Error;
                    MessageBox.Show(Resources.INVALIDKEY, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }


            }
            catch (Exception Ex)    //Exception log for catch block
            {
                MessageBox.Show(Ex.Message, Resources.BCS, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                logger.Log(LOGLEVELS.Error, "btnValidateNew_Click(object sender, EventArgs e)", Ex);
            }


        }
    }

}
