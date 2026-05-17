using System;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Framework;
using System.Configuration;
using Microsoft.Win32;
using System.Xml;
using Hunt.EPIC.Logging;
namespace CAB.UI
{
    public partial class AboutBCS : MdiChildForm
    {
        private string productName = string.Empty;
        private const string DLMSBACKWARDCOMPATIBLE = "DLMS Backward Compatible";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(AboutBCS).ToString());
        public AboutBCS()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutBCS_Load(object sender, EventArgs e)
        {


            //MyCrypro oblcrypto = new MyCrypro();
            //lblProductVersion.Text = "Initial Release 1.0";  //Application.ProductVersion.ToString();
            lblproductname.Text = Application.ProductName.ToString();
            //lblCname.Text = Application.CompanyName.ToString() + "\r\n" + "\r\n" + "All Rights Reserved.";
            //string Getres = oblcrypto.EULAVerification();
            //if (Getres != "" && Getres!="FileError") lblliciencedto.Text = "This product is licensed to : " + Getres;
            productName = GetProductName();
            if (!string.IsNullOrEmpty(productName))
            {
                //lblProductVersion.Text = ProductVersion() + " (" + productName + ")";
                lblProductVersion.Text = ProductVersion() + " (" + productName + ")";
            }
        }
        /// <summary>
        /// Get Product version to display.
        /// </summary>
        /// <returns></returns>
        private string ProductVersion()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Version.xml");
                XmlNode node = xmlDoc.SelectSingleNode("Versions/Desktop");
                return node.InnerText;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                return string.Empty;
                logger.Log(LOGLEVELS.Error, "ProductVersion()", ex);
            }
        }
        private void btnsysteminfo_Click(object sender, EventArgs e)
        {
            SystemInfo objsysinfo = new SystemInfo();
            objsysinfo.ShowDialog();
        }
        private string GetProductName()
        {
            UtilityEntity utilityEntity = UtilityDetails.GetPrimaryUtilityDetails();
            if (utilityEntity == UtilityEntity.JUSCO)
            {
                productName = DLMSBACKWARDCOMPATIBLE;
            }
            else
            {
                productName = utilityEntity.ToString();
            }

            return productName;
        }

        private void lblproductname_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
