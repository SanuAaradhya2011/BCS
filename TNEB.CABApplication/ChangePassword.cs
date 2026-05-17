/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.IECFramework.Utility;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class ChangePassword : MdiChildForm 
    {
		private UserInformationBLL userInformationBLL;
		UserInformationEntity userInformationEntity;
        
        public ChangePassword()
        {
			userInformationBLL = new UserInformationBLL();
            InitializeComponent();
        }

        // check for the validation on the change password
		private bool ValidateData(UserInformationEntity entity)
        {
			bool Flag = false;
			if (string.IsNullOrEmpty(entity.User_Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000011") +" " +  MessageConstant.GetText("M000001");
				Application.DoEvents();
				txtNewPassword.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(entity.User_Password, ValidationConstant.Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000011") + " " + MessageConstant.GetText("M000004");
				txtNewPassword.Focus();
				return Flag;
			}
			else if (string.IsNullOrEmpty(entity.User_Confirm_Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000007") + " " + MessageConstant.GetText("M000001");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}
			else if (!ValidationProvider.ValidateData(entity.User_Confirm_Password, ValidationConstant.Password))
			{
				this.StatusMessage = MessageConstant.GetText("L000007") + " " + MessageConstant.GetText("M000004");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}
			else if (entity.User_Password != entity.User_Confirm_Password)
			{
				this.StatusMessage = MessageConstant.GetText("M000005");
				Application.DoEvents();
				txtConfirmPassword.Focus();
				return Flag;
			}

			return Flag = true;
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
			userInformationEntity = new UserInformationEntity();
			userInformationEntity = (UserInformationEntity)userInformationBLL.GetDetailData(ConfigInfo.UserInformationID);
			txtCurrentPassword.Text = userInformationEntity.User_Password;
        }

		private void btnSave_Click(object sender, EventArgs e)
		{
			userInformationEntity = new UserInformationEntity();
			userInformationEntity.UserInformation_ID = ConfigInfo.UserInformationID;
			userInformationEntity.User_Password = this.txtNewPassword.Text.Trim();
			userInformationEntity.User_Confirm_Password = this.txtConfirmPassword.Text.Trim();
			if (ValidateData(userInformationEntity))
			{
				userInformationBLL.UpdatePassword(userInformationEntity);
                this.StatusMessage = MessageConstant.GetText("M000008");
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
            this.StatusMessage = string.Empty;
			this.Close();
		}

        private void ChangePassword_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
    }
}
