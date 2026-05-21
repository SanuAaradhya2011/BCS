using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework.Entity;
using CAB.Framework.Utility;
using CAB.UI.Controls;
using System.ComponentModel;
using System.Drawing;

namespace CAB.UI
{
    public partial class UserInformation : UserControl
    {
        private static readonly Color DetailAccentColor = Color.FromArgb(15, 23, 42);
        private static readonly Color DetailMutedColor = Color.FromArgb(94, 108, 132);
        private static readonly Color DetailPrimaryColor = Color.FromArgb(26, 115, 232);

        public delegate void CancelClickHandler(object sender, EventArgs e);
        public event CancelClickHandler OnCancelClick;
        public delegate void SaveClickHandler(object sender, EventArgs e);
        public event SaveClickHandler OnSaveClick;
        private int userInformationId = 0;
        private UserInformationBLL userInformationBLL;
        private CategoryMasterBLL categoryMasterBLL;
        private CategoryMasterEntity categoryMasterEntity;
        private DesignationMasterBLL designationMasterBLL;
        private DesignationMasterEntity designationMasterEntity;
        private UserRightBLL userRightBLL;
        private UserInformationEntity userInformationEntity;
        ModuleMasterBLL moduleMasterBLL = null;
        IList<UserRightEntity> userRightEntities;
        private string category = string.Empty;
        public UserInformation()
        {
            userInformationBLL = new UserInformationBLL();
            userRightBLL = new UserRightBLL();
            InitializeComponent();
            ApplyPremiumTheme();
            this.Resize += UserInformation_Resize;
            EnableCheckBoxUsingRights();
            AssociateCheckChangeEnable();
        }

        private void ApplyPremiumTheme()
        {
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.AutoScroll = true;

            StyleContainerGroup(this.groupBox1, 11F);
            StyleContainerGroup(this.groupBox2, 10F);

            StyleFieldLabel(this.lblUserName);
            StyleFieldLabel(this.lngLabel_Category);
            StyleFieldLabel(this.lngLabel_Designation);
            StyleFieldLabel(this.lngLabel_LoginID);
            StyleFieldLabel(this.lngLabel_Password);
            StyleFieldLabel(this.lngLabel_ConfirmPassword);

            StyleRequiredMark(this.label1);
            StyleRequiredMark(this.label2);
            StyleRequiredMark(this.label3);
            StyleRequiredMark(this.label4);
            StyleRequiredMark(this.label5);

            StyleInput(this.txtUserName);
            StyleInput(this.txtLoginID);
            StyleInput(this.txtPassword);
            StyleInput(this.txtConfirmPassword);
            StyleCategoryCombo(this.cboCategory);
            StyleCategoryCombo(this.cboDesignation);

            StyleActionButton(this.lngButton1, true);
            StyleActionButton(this.lngButton2, false);
            StyleActionButton(this.lngBtnAddDesignation, false);
            this.lngBtnAddDesignation.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

            StyleAccessCheck(this.chkUserAdministrator);
            StyleAccessCheck(this.chkDefinition);
            StyleAccessCheck(this.chkProgramming);
            StyleAccessCheck(this.chkReportsView);
            StyleAccessCheck(this.chkCMRIDownload);
            StyleAccessCheck(this.chkDataReadout);
            StyleAccessCheck(this.chkDataArchive);
            StyleAccessCheck(this.chkDataExportImport);

            this.lngLabel4.ForeColor = DetailMutedColor;
            this.lngLabel4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel2.ForeColor = DetailMutedColor;
            this.lngLabel1.ForeColor = Color.FromArgb(220, 38, 38);
            ArrangeEditorLayout();
        }

        private void StyleContainerGroup(GroupBox groupBox, float fontSize)
        {
            groupBox.BackColor = Color.White;
            groupBox.ForeColor = DetailAccentColor;
            groupBox.Font = new Font("Segoe UI Semibold", fontSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleFieldLabel(Label label)
        {
            label.ForeColor = DetailAccentColor;
            label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleRequiredMark(Label label)
        {
            label.ForeColor = Color.FromArgb(220, 38, 38);
            label.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleInput(TextBox textBox)
        {
            textBox.BackColor = Color.FromArgb(248, 250, 252);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.ForeColor = DetailAccentColor;
            textBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleCombo(ComboBox comboBox)
        {
            comboBox.BackColor = Color.White;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.ForeColor = DetailAccentColor;
            comboBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleCategoryCombo(ComboBox comboBox)
        {
            comboBox.BackColor = Color.White;
            comboBox.FlatStyle = FlatStyle.Standard;
            comboBox.ForeColor = DetailAccentColor;
            comboBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        private void StyleActionButton(CABButton button, bool primary)
        {
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

            if (primary)
            {
                button.BackColor = DetailPrimaryColor;
                button.ForeColor = Color.White;
                return;
            }

            button.BackColor = Color.FromArgb(241, 245, 249);
            button.ForeColor = DetailAccentColor;
        }

        private void StyleAccessCheck(CheckBox checkBox)
        {
            checkBox.ForeColor = DetailAccentColor;
            checkBox.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            checkBox.UseVisualStyleBackColor = true;
        }

        private void UserInformation_Resize(object sender, EventArgs e)
        {
            ArrangeEditorLayout();
        }

        private void ArrangeEditorLayout()
        {
            int groupWidth = Math.Min(760, Math.Max(720, this.ClientSize.Width - 40));
            int groupHeight = Math.Max(580, this.ClientSize.Height - 40);
            int left = Math.Max(20, (this.ClientSize.Width - groupWidth) / 2);

            this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.groupBox1.Location = new Point(left, 20);
            this.groupBox1.Size = new Size(groupWidth, groupHeight);

            int fieldWidth = Math.Min(320, Math.Max(260, groupWidth - 430));

            this.txtUserName.Width = fieldWidth;
            this.cboCategory.Width = fieldWidth;
            this.txtLoginID.Width = fieldWidth;
            this.txtPassword.Width = fieldWidth;
            this.txtConfirmPassword.Width = fieldWidth;

            this.cboDesignation.Width = fieldWidth;
            this.lngBtnAddDesignation.Left = this.cboDesignation.Right + 12;

            this.groupBox2.Width = groupWidth - 90;
            ArrangeRightsLayout();

            this.lngButton2.Top = groupHeight - this.lngButton2.Height - 21;
            this.lngButton1.Top = this.lngButton2.Top;
            this.lngButton2.Left = groupWidth - this.lngButton2.Width - 42;
            this.lngButton1.Left = this.lngButton2.Left - this.lngButton1.Width - 12;

            int noteTop = this.lngButton1.Top + 11;
            this.lngLabel4.Top = noteTop;
            this.lngLabel1.Top = noteTop - 2;
            this.lngLabel2.Top = noteTop;
        }

        private void ArrangeRightsLayout()
        {
            if (this.groupBox2.Width >= 620)
            {
                this.groupBox2.Height = 168;
                this.chkUserAdministrator.Location = new Point(21, 32);
                this.chkProgramming.Location = new Point(21, 76);
                this.chkReportsView.Location = new Point(21, 120);

                this.chkDefinition.Location = new Point(246, 32);
                this.chkCMRIDownload.Location = new Point(246, 76);
                this.chkDataExportImport.Location = new Point(246, 120);

                this.chkDataReadout.Location = new Point(468, 32);
                this.chkDataArchive.Location = new Point(468, 76);
                return;
            }

            this.groupBox2.Height = 204;
            this.chkUserAdministrator.Location = new Point(21, 32);
            this.chkProgramming.Location = new Point(21, 72);
            this.chkReportsView.Location = new Point(21, 112);
            this.chkDataExportImport.Location = new Point(21, 152);

            this.chkDefinition.Location = new Point(246, 32);
            this.chkCMRIDownload.Location = new Point(246, 72);
            this.chkDataReadout.Location = new Point(246, 112);
            this.chkDataArchive.Location = new Point(246, 152);
        }

        private void AssociateCheckChangeEnable()
        {
            this.chkUserAdministrator.CheckedChanged += new System.EventHandler(chk_CheckedChanged);
            this.chkDefinition.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkDataReadout.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkCMRIDownload.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkDataArchive.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkProgramming.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkReportsView.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            this.chkDataExportImport.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
        }

        private void EnableCheckbox(string CheckList)
        {
            int startIndex = 0;
            chkUserAdministrator.Enabled = (CheckList[startIndex] == '1') ? true:false;
            chkDefinition.Enabled = (CheckList[startIndex + 1] == '1') ? true : false;
            chkDataReadout.Enabled = (CheckList[startIndex + 2] == '1') ? true : false;
            chkCMRIDownload.Enabled = (CheckList[startIndex + 3] == '1') ? true : false;
            chkDataArchive.Enabled = (CheckList[startIndex + 4] == '1') ? true : false;
            chkProgramming.Enabled = (CheckList[startIndex + 5] == '1') ? true : false;
            chkReportsView.Enabled = (CheckList[startIndex + 6] == '1') ? true : false;
            chkDataExportImport.Enabled = (CheckList[startIndex + 7] == '1') ? true : false;
        }

        private void EnableCheckBoxUsingRights()
        {
            EnableCheckbox(ConfigInfo.RightID);
        }

        private bool ValidateData(UserInformationEntity entity)
        {
            //bool Flag = false;
            if (string.IsNullOrEmpty(entity.User_Name.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000006|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                //Flag = false;
                return false;
            }

            if (entity.Category_ID==0)
            {
                CABMessageBox.ShowFilterMessage("L000057|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                //Flag = false;
                return false;
            }

            else if (!ValidationProvider.ValidateData(entity.User_Name.Trim(), ValidationConstant.UserName))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000006|M000006", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                //Flag = false;
                return false;
            }
            else if (string.IsNullOrEmpty(entity.Login_ID))
            {
                CABMessageBox.ShowFilterMessage("L000005|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLoginID.Focus();
                //Flag = false;
                return false;
            }
            else if (!ValidationProvider.ValidateData(entity.Login_ID.Trim(), ValidationConstant.Password))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000005|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLoginID.Focus();
                //Flag = false;
                return false;
            }
            else if (string.IsNullOrEmpty(entity.User_Password.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000002|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                //Flag = false;
                return false;
            }
            else if (!ValidationProvider.ValidateData(entity.User_Password.Trim(), ValidationConstant.Password))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000002|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                //Flag = false;
                return false;
            }
            else if (string.IsNullOrEmpty(entity.User_Confirm_Password.Trim()))
            {
                CABMessageBox.ShowFilterMessage("L000007|M000001", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConfirmPassword.Focus();
                //Flag = false;
                return false;
            }
            else if (!ValidationProvider.ValidateData(entity.User_Confirm_Password.Trim(), ValidationConstant.Password))
            {
                CABMessageBox.ShowFilterMessage("M000007|L000007|M000004", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConfirmPassword.Focus();
                //Flag = false;
                return false;
            }
            else if (entity.User_Confirm_Password.Trim() != entity.User_Password.Trim())
            {
                CABMessageBox.ShowFilterMessage("M000005", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtConfirmPassword.Focus();
                //Flag = false;
                return false;
            }
            else
            {

                //Flag = true;
                return true;
            }
        }



        private void lngButton2_Click(object sender, EventArgs e)
        {
            if (OnCancelClick != null)
            {
                OnCancelClick(sender, e);
            }
        }
        [Browsable(false)]
        public void ClearData()
        {
            userInformationEntity = null;
            this.txtUserName.Text = string.Empty;
            this.cboCategory.SelectedIndex = 0;
            this.cboDesignation.SelectedIndex = 0;
            this.txtLoginID.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
            this.txtConfirmPassword.Text = string.Empty;
            this.txtLoginID.Enabled = true;
            CheckBoxStateChanged();
            this.txtUserName.Focus();

            chkUserAdministrator.Checked = false;
            chkProgramming.Checked = false;
            chkCMRIDownload.Checked = false;
            chkDataExportImport.Checked = false;
            chkDataArchive.Checked = false;
            chkDataReadout.Checked = false;
            chkDefinition.Checked = false;
            chkReportsView.Checked = false;
            groupBox1.Text = "Create User";
        }

        private void CheckBoxStateChanged()
        {
            chkUserAdministrator.Checked = false;
            chkProgramming.Checked = false;
            chkCMRIDownload.Checked = false;
            chkDataExportImport.Checked = false;
            chkDataArchive.Checked = false;
            chkDataReadout.Checked = false;
            chkDefinition.Checked = false;
            chkReportsView.Checked = false;
        }
        [Browsable(false)]
        public void EditData(IEntity entity)
        {
            CheckBoxStateChanged();
            if (entity == null)
                return;
            userInformationEntity = entity as UserInformationEntity;

            categoryMasterBLL = new CategoryMasterBLL();
            categoryMasterEntity = categoryMasterBLL.GetDetailData(userInformationEntity.Category_ID) as CategoryMasterEntity;
            if (categoryMasterEntity != null)
                this.cboCategory.Text = categoryMasterEntity.Category_Name;
            else
                this.cboCategory.Text = "";

            designationMasterBLL = new DesignationMasterBLL();
            designationMasterEntity = designationMasterBLL.GetDetailData(userInformationEntity.Designation_ID) as DesignationMasterEntity;
            if (designationMasterEntity != null)
                this.cboDesignation.Text = designationMasterEntity.Designation_Name;
            else
                this.cboDesignation.Text = "";

            userInformationId = userInformationEntity.UserInformation_ID;
            this.txtUserName.Text = userInformationEntity.User_Name;
            this.txtLoginID.Enabled = false;
            this.txtLoginID.Text = userInformationEntity.Login_ID;
            this.txtPassword.Text = userInformationEntity.User_Password;
            this.txtConfirmPassword.Text = userInformationEntity.User_Confirm_Password;
            IList<UserRightEntity> rightEntity = userRightBLL.ListData(userInformationEntity.UserInformation_ID);
            foreach (UserRightEntity usrRightEntity in rightEntity)
            {
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("User Administrator"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkUserAdministrator.Checked = true;
                    else
                        chkUserAdministrator.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Programming"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkProgramming.Checked = true;
                    else
                        chkProgramming.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Schedule"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkCMRIDownload.Checked = true;
                    else
                        chkCMRIDownload.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Data Export/Import"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkDataExportImport.Checked = true;
                    else
                        chkDataExportImport.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Data Readout"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkDataReadout.Checked = true;
                    else
                        chkDataReadout.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Data Archive"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkDataArchive.Checked = true;
                    else
                        chkDataArchive.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Definition"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkDefinition.Checked = true;
                    else
                        chkDefinition.Checked = false;
                }
                if (usrRightEntity.Module_ID == moduleMasterBLL.GetModuleIdByName("Reports View"))
                {
                    if (usrRightEntity.Permission == 1)
                        chkReportsView.Checked = true;
                    else
                        chkReportsView.Checked = false;
                }

            }
            groupBox1.Text = "Edit User";
            this.txtUserName.Focus();
        }

        private void lngButton1_Click(object sender, EventArgs e)
        {

            if (userInformationEntity == null)
                userInformationEntity = new UserInformationEntity();
            userInformationEntity.Designation_ID = 0;
            userInformationEntity.Category_ID = 0;

            userInformationEntity.User_Name = this.txtUserName.Text;
            if (string.IsNullOrEmpty(category) || category.Equals("-"))
                category = "0";
            userInformationEntity.Category_ID = Convert.ToInt16(category);
            string designation = ((System.Data.DataRowView)(cboDesignation.Items[cboDesignation.SelectedIndex])).Row.ItemArray[1].ToString().Trim();

            if (string.IsNullOrEmpty(designation) || designation.Equals("-"))
                designation = "0";
            userInformationEntity.Designation_ID = Convert.ToInt16(designation);

            userInformationEntity.Login_ID = this.txtLoginID.Text;
            userInformationEntity.User_Password = this.txtPassword.Text;
            userInformationEntity.User_Confirm_Password = this.txtConfirmPassword.Text;
            int pkId=0;

            if (ValidateData(userInformationEntity))
            {
                if (userInformationEntity.UserInformation_ID == 0)
                {
                    if (!userInformationBLL.ValidateLogin(userInformationEntity.Login_ID))
                    {
                        pkId = userInformationBLL.InsertData(userInformationEntity);
                        GetRights(pkId);
                        userRightBLL.InsertData(userRightEntities);
                    }
                    else
                    {
                        CABMessageBox.ShowFilterMessage("M000071", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtLoginID.Focus();
                        return;
                    }
                }
                else
                {
                    userInformationBLL.UpdateData(userInformationEntity);
                    GetRights(userInformationEntity.UserInformation_ID);
                    userRightBLL.UpdateData(userRightEntities);
                }

                ModuleMasterBLL moduleMasterBLL = new ModuleMasterBLL();
                DataSet dSet = moduleMasterBLL.GetAllData();
                if (OnSaveClick != null)
                {
                    OnSaveClick(sender, e);
                }
            }
        }
        private void GetRights(int pkid)
        {
            moduleMasterBLL = new ModuleMasterBLL();
            userRightEntities = new List<UserRightEntity>();
            if (chkUserAdministrator.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("User Administrator");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("User Administrator");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkProgramming.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Programming");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Programming");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkCMRIDownload.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Schedule");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Schedule");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkDataExportImport.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Export/Import");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Export/Import");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkDataArchive.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Archive");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Archive");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkDataReadout.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Readout");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Data Readout");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkDefinition.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Definition");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Definition");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            if (chkReportsView.Checked == true)
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Reports View");
                entity.Permission = 1;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
            else
            {
                UserRightEntity entity = new UserRightEntity();
                entity.Module_ID = moduleMasterBLL.GetModuleIdByName("Reports View");
                entity.Permission = 0;
                entity.UserInformation_ID = pkid;
                userRightEntities.Add(entity);
            }
        }

        private void UserInformation_Load(object sender, EventArgs e)
        {
            DataSet dataSet = new SearchControlBLL().GetFilterData("designation_master|Designation_Name|Designation_ID");
            if (dataSet != null)
            {
                this.cboDesignation.DataSource = dataSet.Tables[0];
                this.cboDesignation.DisplayMember = "DisplayMember";
                this.cboDesignation.ValueMember = "ValueMember";
            }
            this.cboDesignation.SelectedIndex = 0;
            dataSet = new SearchControlBLL().GetFilterData("Category_Master|Category_Name|Category_ID");
            if (dataSet != null)
            {
                this.cboCategory.DataSource = dataSet.Tables[0];
                this.cboCategory.DisplayMember = "DisplayMember";
                this.cboCategory.ValueMember = "ValueMember";
            }
            this.cboCategory.SelectedIndex = 0;
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            moduleMasterBLL = new ModuleMasterBLL();
            CategoryRightBLL categoryRightBLL = new CategoryRightBLL();
            DataSet dataSet = new DataSet();
            category = ((System.Data.DataRowView)(cboCategory.Items[cboCategory.SelectedIndex])).Row.ItemArray[1].ToString().Trim();

            if (!string.IsNullOrEmpty(category) && category != "-")
            {
                dataSet = categoryRightBLL.GetRight(Convert.ToInt16(category));
                if (dataSet.Tables[0].Rows.Count == 0)
                {
                    categoryRightBLL.InsertDefaultRight();
                    dataSet = categoryRightBLL.GetRight(Convert.ToInt16(category));
                }
                foreach (DataRow drow in dataSet.Tables[0].Rows)
                {
                    if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("User Administrator"))
                        chkUserAdministrator.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Programming"))
                        chkProgramming.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Schedule"))
                        chkCMRIDownload.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Data Export/Import"))
                        chkDataExportImport.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Data Archive"))
                        chkDataArchive.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Data Readout"))
                        chkDataReadout.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Definition"))
                        chkDefinition.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                    else if (Convert.ToInt16(drow["Module_ID"].ToString()) == moduleMasterBLL.GetModuleIdByName("Reports View"))
                        chkReportsView.Checked = (Convert.ToInt16(drow["DefaultRight"].ToString()) == 1) ? true : false;
                }

                //Make sure that when utility is selected then definition is checked bcs for schedule to work its required.
                if (category == "3")
                {
                    chkDefinition.Checked = true;
                }
            }
            else
            {
                chkUserAdministrator.Checked = false;
                chkProgramming.Checked = false;
                chkCMRIDownload.Checked = false;
                chkDataExportImport.Checked = false;
                chkDataArchive.Checked = false;
                chkDataReadout.Checked = false;
                chkDefinition.Checked = false;
                chkReportsView.Checked = false;
            }
        }

        private void lngBtnAddDesignation_Click(object sender, EventArgs e)
        {
            DesignationMaster designationMaster = new DesignationMaster();
            designationMaster.ShowDialog();
            DataSet dataSet = new SearchControlBLL().GetFilterData("designation_master|Designation_Name|Designation_ID");
            if (dataSet != null)
            {
                this.cboDesignation.DataSource = dataSet.Tables[0];
                this.cboDesignation.DisplayMember = "DisplayMember";
                this.cboDesignation.ValueMember = "ValueMember";
            }
            for (int i = 0; i < cboDesignation.Items.Count; i++)
            {
                string desID = ((System.Data.DataRowView)(cboDesignation.Items[i])).Row.ItemArray[1].ToString().Trim();
                if (string.IsNullOrEmpty(desID) || desID.Equals("-"))
                    desID = "0";
                if (designationMaster.designation == null)
                {
                    this.cboDesignation.SelectedIndex = 0;
                    break;
                }
                if (designationMaster.designation.Designation_ID == Convert.ToInt32(desID))
                {
                    this.cboDesignation.SelectedIndex = i;
                    break;
                }
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (!chk.Enabled)
            {
                chk.Checked = false;
            }
        }


    }
}
