using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using LTCTBLL;
using CAB.Entity;
namespace CAB.UI
{
    public partial class AboutBCS : MdiChildForm
    {
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
            lblCompatibility.Visible = UtilityDetails.UtilityName == UtilityEntity.TNEB;
            lblFirmVersion.Visible = UtilityDetails.UtilityName == UtilityEntity.TNEB;
            lblProductVersion.Text = lblProductVersion.Text + " (" + UtilityDetails.UtilityName + ")";

            //lblCname.Text = Application.CompanyName.ToString() + "\r\n" + "\r\n" + "All Rights Reserved.";
             //string Getres = oblcrypto.EULAVerification();
             //if (Getres != "" && Getres!="FileError") lblliciencedto.Text = "This product is licensed to : " + Getres;
        }

        private void btnsysteminfo_Click(object sender, EventArgs e)
        {
			SystemInfo objsysinfo = new SystemInfo();
			objsysinfo.ShowDialog();
        }

        private void lblProductVersion_Click(object sender, EventArgs e)
        {

        }

        private void lblProductVersion_Click_1(object sender, EventArgs e)
        {

        }

        

    }
}
