/*=================================================
** $Workfile:   CreateUser.cs  
** PROJECT  : Cabcon Edge Solution
** COPYRIGHT: Cabcon
** AUTHOR   : Sonia Yadav
**
** LAST MOD.: $Author:   ----------------
**            $Modtime:   ----------------  $
** PURPOSE  :--------------------- 
**
** File Contents (For Validation Check): -
**================================================*/


/*------------------------- includes ----------------------------*/



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

using CAB.Entity;
using CAB.BLL;
using System.Text.RegularExpressions;
using CAB.UI.Controls;
using CAB.Framework.Utility;

namespace CAB.UI
{
	public partial class UserInformation : CABForm
	{

		bool passtype = false;
		UserInformationEntity objUserInformationEntity;
		CategoryMasterEntity objCategoryMasterEntity;
		UserInformationBLL userInformationBLL;

		public UserInformation()
		{
			objUserInformationEntity = null;
			userInformationBLL = new UserInformationBLL();
			InitializeComponent();
		}

		private void CreateUser_Load(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Maximized;
			DataSet categoryData = new SearchControlBLL().GetFilterData("Category_Master|Category_Name|Category_ID");//userInformationBLL.GetCategotyList();
			if (categoryData != null)
			{
				cmbCategory.DisplayMember = "DisplayMember";
				cmbCategory.ValueMember = "ValueMember";
				cmbCategory.DataSource = categoryData.Tables[0];
			}
			fillfropdown();
			checkDefaults();
			GetLabelFromResxFile();

		}

		void GetLabelFromResxFile()
		{
			//    //The text value gets from the Resource file by calling its relevant names.

			////lblUserName.Text	 = 

			//////////CU_lblUName.Text = Global.m_ResourceManager.GetString("CU_lblUName");
			//////////CU_lblCategory.Text = Global.m_ResourceManager.GetString("CU_lblCategory");
			//////////CU_lblPwd.Text = Global.m_ResourceManager.GetString("CU_lblPwd");
			//////////CU_lblCfmPwd.Text = Global.m_ResourceManager.GetString("CU_lblCfmPwd");
			//////////CU_lblDsig.Text = Global.m_ResourceManager.GetString("CU_lblDsig");
			//////////CU_chkadmin.Text =Global.m_ResourceManager.GetString("CU_chkadmin");
			//////////CU_chkProgramming.Text = Global.m_ResourceManager.GetString("CU_chkProgramming");
			//////////CU_chkMRI.Text =Global.m_ResourceManager.GetString("CU_chkMRI");
			////////////CU_chkFullAccess.Text =Global.m_ResourceManager.GetString("CU_chkFullAccess");
			//////////CU_chkdatastore.Text = Global.m_ResourceManager.GetString("CU_chkdatastore");
			//////////CU_chkArchival.Text = Global.m_ResourceManager.GetString("CU_chkArchival");
			//////////CU_chkdatacollect.Text = Global.m_ResourceManager.GetString("CU_chkdatacollect");
			//////////CU_chkConMgmt.Text =Global.m_ResourceManager.GetString("CU_chkConMgmt");
			//////////CU_chkview.Text = Global.m_ResourceManager.GetString("CU_chkview");
			//////////CU_btnSave.Text = Global.m_ResourceManager.GetString("CU_btnSave");
			//////////CU_btnCancel.Text = Global.m_ResourceManager.GetString("CU_btnCancel");

			// Login_lblUName.Text = ResourceFile_en.Login_lblUName;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void fillfropdown()
		{
			//UserInfo userInfo = new UserInfo();
			//DataSet ds = userInfo.SelectUserCat();

			//cmbCategory.DataSource = ds.Tables[0];
			//cmbCategory.ValueMember = "category";
			//passtype = Convert.ToBoolean(userInfo.GetPasswordType());
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{

			}
		}
			//////////if (validateUserInfo())
			//////////{
			//////////    UserInfo userInfo = new UserInfo();
			//////////    userInfo.UserName = txtUserName.Text.Trim();
			//////////    userInfo.Password = txtPassword.Text.Trim();
			//////////    userInfo.Category = cmbCategory.Text.Trim();
			//////////    userInfo.Designation = txtDesignation.Text.Trim();

			//////////    userInfo.UserAdmin = (CU_chkadmin.Checked == true ? true : false);
			//////////    //userInfo.Fullaccess = (CU_chkFullAccess.Checked == true ? true : false);
			//////////    //userInfo.Datacollection = (chkatacollect.Checked == true ? true : false);
			//////////    userInfo.Datacollection = (CU_chkdatacollect.Checked == true ? true : false);
			//////////    userInfo.Programming = (CU_chkProgramming.Checked == true ? true : false);
			//////////    userInfo.Datastoreadmin = (CU_chkdatastore.Checked == true ? true : false);
			//////////    userInfo.Consumermgmt = (CU_chkConMgmt.Checked == true ? true : false);
			//////////    userInfo.MRIdownload = (CU_chkMRI.Checked == true ? true : false);
			//////////    userInfo.Archival = (CU_chkArchival.Checked == true ? true : false);
			//////////    userInfo.Views = (CU_chkview.Checked == true ? true : false);

			//////////    if (txtDesignation.Text != "")
			//////////    {
			//////////        //if (txtDesign.Text.Trim() != "" && IsValidDesignation(txtDesign.Text.Trim()) == false)
			//////////        if (IsValidDesignation(txtDesignation.Text.Trim().Substring(0, 1)) == true)
			//////////        {
			//////////           // MessageBox.Show("designation should start with alphabet");
			//////////            MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblDsig") + " " + Global.m_ResourceManager.GetString("msg_RegEx3"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);

			//////////            txtDesignation.Focus();
			//////////             return  ;
			//////////        }

			//////////    }
			//////////    string result = userInfo.SaveUserInfo();

			//////////    if (result == "1")
			//////////    {
			//////////        //MessageBox.Show("Record successfully saved");
			//////////        MessageBox.Show(Global.m_ResourceManager.GetString("msg_Insert"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//////////        this.Close();
			//////////        MainUser.ActiveForm.Show();
			//////////    }
			//////////    else
			//////////    {
			//////////       // MessageBox.Show("user name already exists");
			//////////        DataSet ds = userInfo.SelectUserstatus(txtUserName.Text);
			//////////        if(ds.Tables[0].Rows[0][13].ToString() == "True") 
			//////////        {
			//////////            string result_status = userInfo.DeleteUserInfo_status(txtUserName.Text);
			//////////             result = userInfo.SaveUserInfo();
			//////////             MessageBox.Show(Global.m_ResourceManager.GetString("msg_Insert"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//////////             this.Close(); 
			//////////        }
			//////////        else
			//////////        {
			//////////        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblUName") + " " + Global.m_ResourceManager.GetString("msg_DataExists"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			//////////    }
			//////////    }

			//////////}
	///////////////}

		/*----------------Validation Check--------------*/
		//private bool validateUserInfo()
		//{
		//    if (txtUserName.Text.Trim() == "")
		//    {
		//        // MessageBox.Show("User name  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblUName") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        txtUserName.Focus();
		//        return false;
		//    }
		//    else if (IsvalidUserName(txtUserName.Text.Trim()) == false)
		//    {
		//        // MessageBox.Show("User name starts from albhabets allow only three special characters(@._)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblUName") + " " + Global.m_ResourceManager.GetString("msg_RegEx4"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        return false;
		//    }

		//    //
		//    if (txtPassword.Text.Trim() == "")
		//    {
		//        txtPassword.Focus();
		//        //MessageBox.Show("Password  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblPwd") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        return false;
		//    }
		//    if (txtConfirmPassword.Text.Trim() == "")
		//    {
		//        txtConfirmPassword.Focus();
		//        // MessageBox.Show("Confirm password  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblCfmPwd") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        return false;
		//    }
		//    if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
		//    {
		//        txtPassword.Focus();
		//        //MessageBox.Show("Password and confirm password is not same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblPwd") + " & " + Global.m_ResourceManager.GetString("CU_lblCfmPwd") + " " + Global.m_ResourceManager.GetString("msg_Same"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        return false;
		//    }
		//    if (IsvalidUserName(txtPassword.Text.Trim().Substring(0, 1)) == false)
		//    {
		//        txtPassword.Focus();
		//        // MessageBox.Show("Password should be start from alphabets", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//        MessageBox.Show(Global.m_ResourceManager.GetString("CU_lblPwd") + " " + Global.m_ResourceManager.GetString("msg_CreatePwd"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//        return false;
		//    }

		//    if (passtype == true)
		//    {
		//        if (txtPassword.Text.Trim().Length < 6 || txtPassword.Text.Trim().Length > 10)
		//        {
		//            txtPassword.Focus();
		//            //  MessageBox.Show(" strong Password so it should be greater than 6 and less than 10", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//            MessageBox.Show(Global.m_ResourceManager.GetString("msg_Strong"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//            return false;
		//        }
		//    }
		//    else
		//    {
		//        if (txtPassword.Text.Trim().Length < 1 || txtPassword.Text.Trim().Length > 10)
		//        {
		//            txtPassword.Focus();
		//            // MessageBox.Show(" Default Password so it should be less than 10", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//            MessageBox.Show(Global.m_ResourceManager.GetString("msg_Default"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
		//            return false;
		//        }
		//    }
		//    return true;
		//}

		// Function To test for Alphabets.
		////////////////////public bool IsAlpha(String strToCheck)
		////////////////////{
		////////////////////    Regex objAlphaPattern = new Regex("[^\\w\\@._a-zA-Z]");
		////////////////////    return !objAlphaPattern.IsMatch(strToCheck);
		////////////////////}

		private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
		{
			checkDefaults();
		}

		private void checkDefaults()
		{
			CU_chkadmin.Checked = false;
			//CU_chkFullAccess.Checked = false;

			CU_chkdatacollect.Checked = false;
			CU_chkProgramming.Checked = false;
			CU_chkdatastore.Checked = false;
			CU_chkConMgmt.Checked = false;
			CU_chkMRI.Checked = false;
			CU_chkArchival.Checked = false;
			CU_chkview.Checked = false;

			if (cmbCategory.Text == "Administrator")
			{
				CU_chkadmin.Checked = true;
				//CU_chkFullAccess.Checked = true;
				CU_chkdatacollect.Checked = true;
				CU_chkProgramming.Checked = true;
				CU_chkdatastore.Checked = true;
				CU_chkConMgmt.Checked = true;
				CU_chkMRI.Checked = true;
				CU_chkArchival.Checked = true;
				CU_chkview.Checked = true;


				CU_chkadmin.Enabled = true;
				//CU_chkFullAccess.Enabled = true;
				CU_chkdatacollect.Enabled = true;
				CU_chkProgramming.Enabled = true;
				CU_chkdatastore.Enabled = true;
				CU_chkConMgmt.Enabled = true;
				CU_chkMRI.Enabled = true;
				CU_chkArchival.Enabled = true;
				CU_chkview.Enabled = true;
			}
			else if (cmbCategory.Text == "Master")
			{
				CU_chkdatacollect.Checked = true;
				CU_chkview.Checked = true;
				CU_chkProgramming.Checked = true;
				CU_chkdatastore.Checked = true;
				CU_chkConMgmt.Checked = true;
				CU_chkMRI.Checked = true;
				CU_chkArchival.Checked = true;
				////// CU_chkadmin.Enabled = false;
				//////// CU_chkFullAccess.Enabled = false;
				////// CU_chkArchival.Enabled = false;

				////CU_chkdatacollect.Enabled = true;
				////CU_chkview.Enabled = true;
				////CU_chkProgramming.Enabled = true;
				////CU_chkdatastore.Enabled = true;
				////CU_chkConMgmt.Enabled = true;
				////CU_chkMRI.Enabled = true;
			}
			else if (cmbCategory.Text == "Utility")
			{
				CU_chkdatacollect.Checked = true;
				CU_chkview.Checked = true;
				//
				////// CU_chkadmin.Enabled = false;
				//////// CU_chkFullAccess.Enabled = false;

				////// CU_chkProgramming.Enabled = false;
				////// CU_chkdatastore.Enabled = false;
				////// CU_chkConMgmt.Enabled = false;
				////// CU_chkMRI.Enabled = false;
				////// CU_chkArchival.Enabled = false;

				//////CU_chkdatacollect.Enabled = true;
				//////CU_chkview.Enabled = true;
			}
			else if (cmbCategory.Text == "Reader")
			{
				CU_chkdatacollect.Checked = true;
				CU_chkMRI.Checked = true;
				//
				//////CU_chkadmin.Enabled = false;
				////////CU_chkFullAccess.Enabled = false;


				//////CU_chkProgramming.Enabled = false;
				//////CU_chkdatastore.Enabled = false;
				//////CU_chkConMgmt.Enabled = false;
				//////CU_chkMRI.Enabled = false;
				//////CU_chkArchival.Enabled = false;
				//////CU_chkview.Enabled = false;
				//////CU_chkdatacollect.Enabled = true;
			}
			else if (cmbCategory.Text == "Data Store Administrator")
			{
				CU_chkdatastore.Checked = true;

				//////CU_chkadmin.Enabled = false;
				////////CU_chkFullAccess.Enabled = false;
				//////CU_chkdatacollect.Enabled = false;
				//////CU_chkdatacollect.Enabled = false;
				//////CU_chkProgramming.Enabled = false;

				//////CU_chkConMgmt.Enabled = false;
				//////CU_chkMRI.Enabled = false;
				CU_chkArchival.Checked = true;
				//////CU_chkview.Enabled = false;
				//////CU_chkdatastore.Enabled = true;
			}
		}

		private void CreateUser_FormClosed(object sender, FormClosedEventArgs e)
		{
			////this.Hide();
			////MainUser mn = new MainUser();
			////mn.MdiParent = Form1.ActiveForm;
			////mn.Show();
		}

		private bool ValidateData()
		{
			return false;

			//bool Flag = false;
			//if (string.IsNullOrEmpty(objUserInformationEntity.Users_Name))
			//{
			//    CABMessageBox.ShowFilterMessage("L000001|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    txtUserName.Focus();
			//    Flag = false;
			//}
			//else if (string.IsNullOrEmpty(objUserInformationEntity.User_Category_Name))
			//{
			//    CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    cmbCategory.Focus();
			//    Flag = false;
			//}
			//else if (string.IsNullOrEmpty(objUserInformationEntity.User_Password))
			//{
			//    CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    txtPassword.Focus();
			//    Flag = false;
			//}
			//else if (string.IsNullOrEmpty(objUserInformationEntity.User_Confirm_Password))
			//{
			//    CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    txtConfirmPassword.Focus();
			//    Flag = false;
			//}
			//else if (string.IsNullOrEmpty(objUserInformationEntity.User_Designation))
			//{
			//    CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    txtDesignation.Focus();
			//    Flag = false;
			//}
			//else
			//    Flag = true;
			//return Flag;
		}

		//public bool IsvalidUserName(String strToCheck)
		//{
		//    bool flag = false;
		//    if (strToCheck != "")
		//    {
		//        Regex objAlphaPattern = new Regex("[^a-zA-Z0-9]");
		//        flag = !objAlphaPattern.IsMatch(strToCheck.Substring(0, 1));
		//        if (flag == true)
		//        {
		//            objAlphaPattern = new Regex("[^a-zA-Z0-9@._]");
		//            flag = !objAlphaPattern.IsMatch(strToCheck);
		//        }
		//    }
		//    return flag;
		//}

		//public bool IsValidDesignation(String strToCheck)
		//{
		//    bool flag = false;


		//    Regex objAlphaPattern = new Regex("[^a-zA-Z]");
		//    flag = objAlphaPattern.IsMatch(strToCheck);

		//    return flag;
		//}

		private void CU_btnSave_Click(object sender, EventArgs e)
		{
            //UserInformationEntity objUserInformationEntity = new UserInformationEntity();
            //objUserInformationEntity.Users_Name = txtUserName.Text.Trim();
            //objUserInformationEntity.Category_ID = Convert.ToInt32(cmbCategory.SelectedValue);
            //objUserInformationEntity.Designation_ID = Convert.ToInt32(cmbDesignation.SelectedValue);
            //objUserInformationEntity.Login_ID = txtLoginID.Text.Trim();
            //objUserInformationEntity.User_Password = txtPassword.Text.Trim();
            //objUserInformationEntity.User_Confirm_Password = txtConfirmPassword.Text.Trim();

            //if (ValidateData())
            //{
            //     //ConfigInfo.UserInformationID = ((UserInformationEntity)userInformationBLL.InsertData(objUserInformationEntity)).UserInformation_ID;
            //}
		}

		private void CU_btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}