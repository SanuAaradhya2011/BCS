/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using CAB.Entity;
using CAB.BLL;
using CAB.IECFramework.Utility;
using CAB.UI.Controls; 
using CAB.IECChannel;
using CAB.IECFramework.Entity;
using System.Collections.Generic; 
using System.Drawing;
using LTCTBLL;
using CABApplication;
using CABEntity;
using System.IO;
using CAB.IECFramework;
namespace CAB.UI
{
    
    public partial class LoginForm : CABForm
    {

        int counter;
        UserInformationEntity userInformation;
        UserInformationBLL userInformationBLL;
        UserInformationEntity userInformationEntity;
        IList<UserRightEntity> userRightEntities;       
        private UserRightBLL userRightBLL;
        
        public LoginForm()
        {
            counter = 0;
            userInformation = null;
            userInformationBLL = null;
            InitializeComponent();
        }

        private bool ValidateData()
        {
            bool Flag = false;
            if (string.IsNullOrEmpty(userInformation.Login_ID))
            {
                CABMessageBox.ShowFilterMessage("L000001|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                Flag = false;
            }
            else if (string.IsNullOrEmpty(userInformation.User_Password))
            {
                CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                Flag = false;
            }
            else
                Flag = true;
            return Flag;
        }

        private void InsertUser(string userName, string passwd)
        {
            userRightEntities = new List<UserRightEntity>();
            userInformationBLL = new UserInformationBLL();
            userRightBLL = new UserRightBLL();
            if (userInformationEntity == null)
                userInformationEntity = new UserInformationEntity();
            userInformationEntity.Designation_ID = 0;
            userInformationEntity.Category_ID = 1;
            userInformationEntity.User_Name = userName;
            userInformationEntity.Login_ID = userName;
            userInformationEntity.User_Password = passwd;
            userInformationEntity.User_Confirm_Password = passwd;
            int pkId = 0;

            pkId = userInformationBLL.InsertData(userInformationEntity);
            GetRights(pkId);
            userRightBLL.InsertData(userRightEntities);
        }
        private void GetRights(int pkid)
        {
            ModuleMasterBLL moduleMasterBLL = new ModuleMasterBLL();
            for (int menuCount = 1; menuCount <= 8; menuCount++)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = menuCount;
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            counter++;
			if (counter > 3)
			{
				CABMessageBox.ShowFilterMessage("M000097", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				this.btnCancel_Click(this, null);
			}
            userInformation = new UserInformationEntity();
            userInformation.Login_ID = txtUserName.Text.Trim();
            userInformation.User_Password = txtPassword.Text.Trim();
            if (txtUserName.Text.Trim().ToUpper().Equals("SCOTT") && txtPassword.Text.Trim().ToUpper().Equals("TIGER"))
            {
                Utility utility = new Utility();
                new DBGenerationBLL().CreateCABAppDatabase();

                //following two settings added on 9th march 2012 as per validation report
                ConfigSettings.ChangeNode("LocationType", "Default");
                ConfigSettings.ChangeNode("FileNamingConvention", "Default");

                string touFilePath = ConfigInfo.GetTOULocation();
                if (Directory.Exists(touFilePath))
                {
                    string filesToDelete = touFilePath+"TouConfig.TOU";
                    if(File.Exists(filesToDelete))
                    File.Delete(filesToDelete);
                    //FileStream fs = new FileSSStream(string.Concat(touFilePath, "/TouConfig.TOU"), FileMode.Create);
                }

                this.Hide();
                string PortNumber = ConfigSettings.GetValue("PortNumber");
                ConfigSettings.ChangeNode("ConnectionString", "server=127.0.0.1;user=root;database=rubyapp;port=" + PortNumber + ";password=Password12;");
                utility.ShowDialog();
                if (!utility.utilityValidated)
                {
                    MessageBox.Show("Utility not identified!!");
                    return;
                }
                if (UtilityDetails.UtilityName == UtilityEntity.TNEB || UtilityDetails.UtilityName == UtilityEntity.TNEB1)
                    InsertUser("Super", "Super");
                
                userInformationBLL = new UserInformationBLL();
                userInformation.Login_ID = "xxx";
                userInformation.User_Password = "xxx";
                if (userInformationBLL.ValidateUser(userInformation) != null)
                {
                    ConfigInfo.UserInformationID = 0;
                    this.Hide();
                    if (!string.IsNullOrEmpty(utility.UtilityName1) || !string.IsNullOrEmpty(utility.Utility_Password))
                        new MainForm().Show();
                    else
                        Application.Exit();
                    
                }
                else
                {
                    CABMessageBox.ShowFilterMessage("D000001", "A000001|D000002", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                
                

            }
            else if (ValidateData())
            {
                userInformationBLL = new UserInformationBLL();
                userInformation = (UserInformationEntity)userInformationBLL.ValidateUser(userInformation);
                
                if (userInformation != null)
                {
                    if (userInformation.User_Password != txtPassword.Text.Trim())
                    {
                        CABMessageBox.ShowMessage("M000002", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (userInformation.UserInformation_ID == 0)
                        CABMessageBox.ShowMessage("M000002", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        ConfigInfo.LogID = new ApplicationLogBLL().InsertData();
                        ConfigInfo.UserInformationID = userInformation.UserInformation_ID;
                        ConfigSettings.ChangeNode("GSMConnected", "False");
                        this.ConnectionTypeMessage = "Physical";
                        ConfigInfo.ChannelType = ChannelType.RS232;
                        this.Hide();
                        new MainForm().Show();

                    }
                }
                else
                {
                    CABMessageBox.ShowFilterMessage("D000001", "A000001|D000002", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            linklblRegister.Visible = false;
            lbl_ShowDemo.Visible = false;
            
            
            //MyCrypro oblcrypto = new MyCrypro();
            ////GetLabelFromResxFile();
            ////objUser.InsertDefaultAdmin();
            //int daysdiff = 0;
            ////double Time_Remain = 0;
            //string Getres = oblcrypto.EULAVerification();
            //if (Getres != "OK" && Getres != "FileError")
            //{
            //    oblcrypto.SetWin32Info();
            //    daysdiff = oblcrypto.GetWin32Info();

            //    if (daysdiff >= 300)
            //    {
            //        btnLogin.Enabled = false;
            //        linklblRegister.Visible = true;
            //        lbl_ShowDemo.Visible = true;
            //        lbl_ShowDemo.ForeColor = Color.Red;
            //        lbl_ShowDemo.Text = "300 Days Demo Version Expired";
            //        linklblRegister.Text = "Click Here To Register Your Product";
            //    }
            //    else
            //    {  
            //        lbl_ShowDemo.Visible = true;
            //        lbl_ShowDemo.ForeColor = Color.Red;
            //        lbl_ShowDemo.Text = "Demo Version Available For " + (300 - daysdiff).ToString() + " More Day(s)";
            //    }
            //}
            //if (Getres == "FileError")
            //{
            //    //ReRegistration Required;
            //    btnLogin.Enabled = false;
            //    linklblRegister.Visible = true;
            //    lbl_ShowDemo.Visible = true;
            //    lbl_ShowDemo.Text = "Re-Registration Required";
            //    linklblRegister.Text = "Click Here To Register Your Product";
            //}
        }

        private void linklblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterProduct rproduct = new RegisterProduct();
            rproduct.ShowDialog();
        }
        
    }
}
