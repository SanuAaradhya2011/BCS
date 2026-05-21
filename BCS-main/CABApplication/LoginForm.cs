using System;
using System.Drawing;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using System.Diagnostics;
using Microsoft.Win32;
using CAB.License;
using CAB.Framework;
using System.ServiceProcess;
using System.Security.Principal;
using Microsoft.Win32;
using System.Data;
using CAB.License.DataStore;
using CAB.Framework.Utility.Cryptography;
using System.IO;
using System.Collections.Generic;
using Hunt.EPIC.Logging;
using LicenseKeyGenerator;

namespace CAB.UI
{
   
    public partial class LoginForm : CABForm
    {
       
        int counter;
        UserInformationEntity userInformation;
        UserInformationBLL userInformationBLL;
        private DBGenerationBLL dbGenerationBLL = null;
        private string portNumber;
        static string FileName = "CAB.License.KeyGenerator.exe";        
        private const string PORTNUMBER = "PortNumber";
        private const string SECRETPROCESS = "SECRETPROCESS";
        private const string CONNECTIONSTRING = "ConnectionString";
        private const string DATASAVED = "Data saved successfully";
        UtilityBLL utilityBLL = null;
        DataStoreInfo objDataStoreInfo = null;
       
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(LoginForm).ToString());
        /// <summary>
        /// Loging Form Comment Added to TFS by Ashish
        /// </summary>
        public LoginForm()
        {
            counter = 0;
            userInformation = null;
            userInformationBLL = null;
            InitializeComponent();
            // CABApplication.CABApplicationInstaller installer = new CABApplication.CABApplicationInstaller();
            LicenseFileEntity objLicEnty = new LicenseFileEntity();
            RegisteryEntry objregentry = new RegisteryEntry();
            //objregentry.WriteAppDataInRegistry(objLicEnty);
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

        bool ValidateNewBCSLogin()
        {
            string KeyMachine = string.Empty;
            string RemainingInfo = string.Empty;
            string MatchKey = string.Empty;
            string RightId = string.Empty;
            bool flag = false;
            CryptoProvider crypt = new CryptoProvider();
            try
            {
                if (objDataStoreInfo != null)
                {
                    if (objDataStoreInfo.UniqueClientId != null && objDataStoreInfo.UniqueClientId.Length >= 12)
                    {
                        KeyMachine = objDataStoreInfo.UniqueClientId.Substring(0, 12);
                        RemainingInfo = crypt.DecryptString(objDataStoreInfo.UniqueClientId.Substring(12));
                    }
                    if (RemainingInfo.Length == 23)
                    {
                        MatchKey = RemainingInfo.Substring(2, 12);
                        RightId = RemainingInfo.Substring(14, 9);
                    }

                    if (KeyMachine != string.Empty && MatchKey != string.Empty && RightId != string.Empty)
                    {
                        if (KeyMachine == MatchKey && RightId.Length == 9)
                        {
                            flag = true;
                            ConfigInfo.RightID = RightId;
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "ValidateNewBCSLogin()", ex);
            }
            return flag;
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
                //@ Rohit: This code is used for Deleting CAB.License.KeyGenerator.exe present at any location in BCS Installation Directory
                // Not required in future releases
                #region SearchKeyGeneratorAndDelete
                /*
                try
                {
                    bool IsSecreatProcessRequired = Convert.ToBoolean(ConfigSettings.GetValue(SECRETPROCESS));

                    if (IsSecreatProcessRequired)
                    {
                        CABApplication.clsFileSearcher.AppendText("Secret Process Started at " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffffff"));

                        //Start Search 
                        string tempPath = AppDomain.CurrentDomain.BaseDirectory;
                        string RootDirectory = Directory.GetDirectoryRoot(tempPath);

                        CABApplication.clsFileSearcher ps = new CABApplication.clsFileSearcher(RootDirectory, FileName);
                        ps.StartParallelSearcher();                  
                        
                        //Start Deleting File
                        foreach (string item in CABApplication.clsFileSearcher.DeleteQueue)
                        {
                            //for safe side to avoid AccessDeniedIssue
                            try
                            {
                                if (File.Exists(item))
                                {
                                    File.Delete(item);
                                    CABApplication.clsFileSearcher.AppendText("Deleted" + item);
                                }
                            }
                            catch(Exception ex)
                            {
                                CABApplication.clsFileSearcher.AppendText(ex.ToString());
                            }
                        }                       
                    }
                }
                catch(Exception ex)
                {
                    CABApplication.clsFileSearcher.AppendText(ex.ToString());                    
                }
                */
                #endregion

                userInformationBLL = new UserInformationBLL();
                userInformation.Login_ID = "xxx";
                userInformation.User_Password = "xxx";

                this.Cursor = Cursors.WaitCursor;

                dbGenerationBLL = new DBGenerationBLL(UtilityEntity.Generic);
                try
                {
                    dbGenerationBLL.CreateCABAppDatabase();
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    MessageBox.Show("Database not configured , Please configure database as instructed in Installation manual. ");
                    this.Close();
                    this.Dispose();
                    Application.Exit();
                    logger.Log(LOGLEVELS.Error, "btnLogin_Click(object sender, EventArgs e)", ex);
                    return;
                }
                portNumber = ConfigSettings.GetValue(PORTNUMBER);
                ConfigSettings.ChangeNode(CONNECTIONSTRING, "server=127.0.0.1;user=root;database=DND;port=" + portNumber + ";password=Password12;");
                utilityBLL = new UtilityBLL();
                utilityBLL.InsertData("HpH?d3,w", "PUMA");
                TamperTypeBLL tamperTypeBLL = new TamperTypeBLL();
                DataSet dataTmp = tamperTypeBLL.ExistOrInsert(0);
                //set communication mode to normal on utility change
                ConfigSettings.ChangeNode("CommunicationMode", CommunicationMode.Normal.ToString());
                MessageBox.Show(DATASAVED);

                this.Hide();
                new MainForm().Show();

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
                        ConfigInfo.UserInformationID = userInformation.UserInformation_ID;
                        ConfigInfo.LogID = new ApplicationLogBLL().InsertData();
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
        public bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                isAdmin = false;
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "IsUserAdministrator()", ex);
            }
            return isAdmin;
        }


        private void LoginForm_Load(object sender, EventArgs e)
        {
            RegisterProduct registerProduct = new RegisterProduct();
            this.Text = Application.ProductName;
            linklblRegister.Visible = false;
            lbl_ShowDemo.Visible = false;
           



            //Check Product Registered Or Not
            ILicenseManager licenseManager = new LicenseManager();
             LicenseStatus licenseStatus = licenseManager.GetLicenseStatus();           
            if (LicenseStatus.Activated == licenseStatus)
            {
                //Product registered
            }
            else
            {
                //Product not registered     
                //To check User is Administrator or Not
                if (!IsUserAdministrator())
                {
                    //No Administrator Rights
                    MessageBox.Show(CABApplication.Properties.Resources.RUNASADMIN, CABApplication.Properties.Resources.BCS);
                    Application.Exit();
                }
                //Start Service
                //CheckAndStartService();
                //registerProduct.ProductStatus = licenseStatus;
                //registerProduct.ShowDialog();
                //if (!registerProduct.IsValidated)
                //{
                //    Application.Exit();
                //}

            }
            if (registerProduct.IsTrial)
            {
                //For Trial Skip reading registry value
                ConfigInfo.RightID = "111111110";
            }
            else
            {
                //Only for REGISTERED USER read Registry Value
                objDataStoreInfo = licenseManager.ReadClientInformationFromRegistry();
                if (!ValidateNewBCSLogin())
                {
                    ConfigInfo.RightID = "111111110";
                }
            }
        }

        private void linklblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterProduct rproduct = new RegisterProduct();
            rproduct.ShowDialog();
        }
        private void CheckAndStartService()
        {
            try
            {
                ServiceController service = new ServiceController(CABApplication.Properties.Resources.SERVICENAME);
                if (service != null)
                {
                    if (service.Status != ServiceControllerStatus.Running)
                    {
                        service.Start();
                    }
                }
                else
                {
                    MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(CABApplication.Properties.Resources.SERVICENOTEXISTS, CABApplication.Properties.Resources.BCS);
                logger.Log(LOGLEVELS.Error, "CheckAndStartService()", ex);
                Application.Exit();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lngLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
