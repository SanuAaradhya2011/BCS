using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using CAB.BLL;
using System.Text.RegularExpressions;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.Framework.Entity;

namespace CAB.UI
{
    public partial class ChangePassword : UserControl
    {
        public long userid=0;
        bool passtype = false;
        public ChangePassword()
        {
			InitializeComponent();
        }

        private void tbSave_Click(object sender, EventArgs e)
        {
			////Form1 frmMDI=new Form1();
			//userid = Form1.UserId;

			//if (validateUserInfo())
			//{
			//    UserInfo uinfo = new UserInfo();
			//    uinfo.UserId = userid.ToString();
			//    uinfo.OldPassword = txtoldpwd.Text.Trim();
			//    uinfo.Password = txtpwd1.Text.Trim();
			//    string result=uinfo.ChangePassword();
			//    if (result == "0")
			//    {
			//       // MessageBox.Show("Current password is wrong.");
			//        MessageBox.Show(Global.m_ResourceManager.GetString("ChangePwd_lblCurrentPwd") + " " + Global.m_ResourceManager.GetString("msg_Same"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    }
			//    else if (result == "1")
			//    {
			//        //MessageBox.Show("Password successfully changed.");
			//        MessageBox.Show(Global.m_ResourceManager.GetString("msg_Insert"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
			//    }
			//}

        }
        // check for the validation on the change password
        private bool validateUserInfo()
        {
			return false; 
			//if (txtoldpwd.Text.Trim() == "")
			//{
			//    txtoldpwd.Focus();
			//    //MessageBox.Show("Old password  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    MessageBox.Show(Global.m_ResourceManager.GetString("ChangePwd_lblCurrentPwd") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			//    return false;
			//}
			//if (txtpwd1.Text.Trim() == "")
			//{
			//    txtpwd1.Focus();
			//  //  MessageBox.Show("Password  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    MessageBox.Show(Global.m_ResourceManager.GetString("ChangePwd_lblNewPwd") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			//    return false;
			//}
			//if (txtpwd2.Text.Trim() == "")
			//{
			//    txtpwd2.Focus();
			//    //MessageBox.Show("Confirm password  cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    MessageBox.Show(Global.m_ResourceManager.GetString("ChangePwd_lblCfmPwd") + " " + Global.m_ResourceManager.GetString("msg_Blank"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			//    return false;
			//}
			//if (txtpwd1.Text.Trim() != txtpwd2.Text.Trim())
			//{
			//    txtpwd1.Focus();
			//    //MessageBox.Show("Password and confirm password is not same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    MessageBox.Show(Global.m_ResourceManager.GetString("EU_lblPwd") + " &  " + Global.m_ResourceManager.GetString("EU_lblCfmPwd") + " " + Global.m_ResourceManager.GetString("msg_Same"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    return false;
			//}
			//if (IsAlpha(txtpwd1.Text.Trim().Substring(0, 1))==false)
			//{
			//    txtpwd1.Focus();
			//    //MessageBox.Show("Password should be start from alphabets", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//    MessageBox.Show(Global.m_ResourceManager.GetString("EU_lblPwd") + " " + Global.m_ResourceManager.GetString("msg_CreatePwd"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//    return false;
			//}
			//if (passtype == true)
			//{
			//    if (txtpwd1.Text.Trim().Length < 6 || txtpwd1.Text.Trim().Length > 10)
			//    {
			//        txtpwd1.Focus();
			//       // MessageBox.Show("Password  should be greater than 6 and less than 10", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//        MessageBox.Show(Global.m_ResourceManager.GetString("msg_Strong"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//        return false;
			//    }
			//}
			//else
			//{
			//    if (txtpwd1.Text.Trim().Length < 1 || txtpwd1.Text.Trim().Length > 10)
			//    {
			//        txtpwd1.Focus();
			//       // MessageBox.Show("Password  should not be greater than 10", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//        MessageBox.Show(Global.m_ResourceManager.GetString("msg_Default"), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
			//        return false;
			//    }
			//}
           
			//return true;
        }
        public bool IsAlpha(String strToCheck)
        {
			return false;
			//Regex objAlphaPattern = new Regex("[^a-zA-Z0-9]");
			//return !objAlphaPattern.IsMatch(strToCheck);
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
           
        }

        void GetLabelFromResxFile()
        {
            //    //The text value gets from the Resource file by calling its relevant names.

			//ChangePwd_lblCurrentPwd.Text = Global.m_ResourceManager.GetString("ChangePwd_lblCurrentPwd");
			//ChangePwd_lblNewPwd.Text = Global.m_ResourceManager.GetString("ChangePwd_lblNewPwd");
			//ChangePwd_lblCfmPwd.Text = Global.m_ResourceManager.GetString("ChangePwd_lblCfmPwd");
			//ChangePwd_btnSave.Text = Global.m_ResourceManager.GetString("ChangePwd_btnSave");
			//ChangePwd_btnCancel.Text = Global.m_ResourceManager.GetString("ChangePwd_btnCancel");



            // Login_lblUName.Text = ResourceFile_en.Login_lblUName;


        }

        private void ChangePassword_Activated(object sender, EventArgs e)
        {
			//UserInfo uinfo = new UserInfo();
			//passtype = Convert.ToBoolean(uinfo.GetPasswordType());
			//GetLabelFromResxFile();
        }
        
    }
}
