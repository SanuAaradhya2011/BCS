using System;
using System.Data;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.IECFramework.Utility;
using System.Windows.Forms;
using System.Drawing;

namespace CAB.UI
{
	public partial class UserInformations : MdiChildForm
	{
		private UserInformationBLL userInformationBLL;
		UserInformationEntity userInformationEntity;
        UserInformation ucDetail = null; 
		public UserInformations()
		{
			InitializeComponent();
            this.Text = MessageConstant.GetText("C000003");
			userInformationBLL = new UserInformationBLL();
            ucGridControl.ValueColumn = ucGridControl.ValueColumn = "UserInformation_ID";
            ucDetail = new UserInformation();
            this.ucDetail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDetail.Location = new System.Drawing.Point(13, 51);
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.Size = new System.Drawing.Size(790, 447);
            this.ucDetail.TabIndex = 3;
            this.ucDetail.OnCancelClick += new CAB.UI.UserInformation.CancelClickHandler(this.ucDetail_OnCancelClick);
            this.ucDetail.OnSaveClick += new CAB.UI.UserInformation.SaveClickHandler(this.ucDetail_OnSaveClick);
            this.Controls.Add(ucDetail);
		}

        private void UserInformations_Load(object sender, EventArgs e)
        {
            LoadGrid();
            ucSearchControl_OnFindNowClick(this, null);
        }

		private void LoadGrid()
		{
            ucSearchControl.PrimarySearchTypeData = this.GetSearchData();         
			ucSearchControl.RefreshList();
		}
       
        private void FillGridWithSearchType()
        {
            DataSet dataSet = null;
            string SearchType = ucSearchControl.PrimarySearchTypeComboData;
            string SearchData = ucSearchControl.SecondarySearchTypeComboData;
            string SearchText = ucSearchControl.SecondarySearchTypeTextData;
            if (SearchType.Equals("All"))
                dataSet = userInformationBLL.GetSearchData();
            else if (SearchType.Equals("User Name"))
            {
                if (SearchText == string.Empty)
                    dataSet = userInformationBLL.GetSearchData();
                else
                    dataSet = userInformationBLL.GetSearchData("Users_Name", SearchText);
            }
            else if (SearchType.Equals("Login ID"))
            {
                if (SearchText == string.Empty)
                    dataSet = userInformationBLL.GetSearchData();
                else
                    dataSet = userInformationBLL.GetSearchData("Login_ID", SearchText);
            }
            else if (SearchType.Equals("Category"))
            {
                if (SearchData == "-")
                    dataSet = userInformationBLL.GetSearchData();
                else
                    dataSet = userInformationBLL.GetSearchData("Category_ID", SearchData);
            }
            else if (SearchType.Equals("Designation"))
            {
                if (SearchData == "-")
                    dataSet = userInformationBLL.GetSearchData();
                else
                    dataSet = userInformationBLL.GetSearchData("Designation_ID", SearchData);  
            }
            ucSearchControl.RefreshControls(dataSet);
            ucGridControl.Data = dataSet;
            ucGridControl.HiddenColumn = "UserInformation_ID";
            ucGridControl.ValueColumn = "UserInformation_ID";
            ucGridControl.IsSorting = false;
            ucGridControl.RefreshGrid();
        }

        private void ucSearchControl_OnFindNowClick(object sender, EventArgs e)
        {
            ucDetail.Visible = false;
            ucGridControl.Visible = true;
            FillGridWithSearchType();
            this.StatusMessage = String.Empty;
        }

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            this.StatusMessage = String.Empty;
            ucSearchControl.ShowAddOption();
            ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            ucDetail.Visible = true;
            ucDetail.Location = new Point(19, 16);
            ucDetail.ClearData();
        }

        private void ucDetail_OnCancelClick(object sender, EventArgs e)
        { 
            UserInformations_Load(this, null);
            ucSearchControl.Visible = true;
            this.StatusMessage = String.Empty;
        }

        private void ucDetail_OnSaveClick(object sender, EventArgs e)
        {
            ucSearchControl.Visible = true;
            UserInformations_Load(this, null);
            this.StatusMessage = MessageConstant.GetText("M000021");
        }

		private void ucSearchControl_OnSearchClick(object sender, EventArgs e)
		{
            this.StatusMessage = String.Empty;
			ucSearchControl.EnableControls();
            ucSearchControl.HideMainSearch = true;
			this.LoadGrid();
		}

        private void ucSearchControl_OnEditClick(object sender, EventArgs e)
        {
            this.StatusMessage = String.Empty;
            ucDetail.Visible = true;
            ucSearchControl.ShowEditOption();
            Application.DoEvents();
            ucSearchControl.Visible = false;
            ucDetail.Location = new Point(19, 16);
            ucGridControl.Visible = false;
            LoadEntity();
        }

        private void LoadEntity()
        {
            string pkValue = ucGridControl.GetPrimaryValue();
            if (!string.IsNullOrEmpty(pkValue))
            {
                userInformationEntity = (UserInformationEntity)userInformationBLL.GetDetailData(Int32.Parse(pkValue));
                ucDetail.EditData(userInformationEntity);
            }
        }

		private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
		{
            LoadEntity();
			userInformationEntity.IsActive = 1;
            if (CABMessageBox.ShowFilterMessage("M000072", "A000001", MessageBoxButtons.YesNo,MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                if (userInformationEntity.UserInformation_ID == ConfigInfo.UserInformationID)
                {
                    CABMessageBox.ShowFilterMessage("M000073", "A000001");
                    return;
                }
                userInformationBLL.DeleteData(userInformationEntity);
                this.StatusMessage = MessageConstant.GetText("M000020");
                UserInformations_Load(this, null);
            }
		}

        private void UserInformations_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            DataSet dataset = userInformationBLL.GetSearchData();
            if (dataset.Tables.Count==0 || dataset.Tables[0].Rows.Count == 0)
            {
                ucSearchControl.ShowAddOption();
            } 
        }
	}
}
