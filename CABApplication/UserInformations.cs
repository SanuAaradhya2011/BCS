using System;
using System.Data;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CAB.UI
{
	public partial class UserInformations : MdiChildForm
	{
        private static readonly Color PageBackColor = Color.FromArgb(244, 247, 251);
        private static readonly Color CardBackColor = Color.White;
        private static readonly Color BorderColor = Color.FromArgb(223, 229, 238);
        private static readonly Color PrimaryColor = Color.FromArgb(26, 115, 232);
        private static readonly Color AccentColor = Color.FromArgb(15, 23, 42);
        private static readonly Color MutedTextColor = Color.FromArgb(94, 108, 132);
        private bool isDetailMode;

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
            this.ucDetail.BackColor = CardBackColor;
            this.ucDetail.Dock = DockStyle.Fill;
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.Size = this.ucGridControl.Size;
            this.ucDetail.TabIndex = 3;
            this.ucDetail.OnCancelClick += new CAB.UI.UserInformation.CancelClickHandler(this.ucDetail_OnCancelClick);
            this.ucDetail.OnSaveClick += new CAB.UI.UserInformation.SaveClickHandler(this.ucDetail_OnSaveClick);
            this.panelContentCard.Controls.Add(ucDetail);
            InitializePremiumUi();
            this.Resize += UserInformations_Resize;
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
            isDetailMode = false;
            ucDetail.Visible = false;
            ucGridControl.Visible = true;
            ucGridControl.BringToFront();
            FillGridWithSearchType();
            this.StatusMessage = String.Empty;
            SetPageSubtitle("Search, review and maintain user profiles from a cleaner workspace.");
            UpdateLayoutMode();
        }

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            isDetailMode = true;
            this.StatusMessage = String.Empty;
            ucSearchControl.ShowAddOption();
            ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            ucDetail.Visible = true;
            ucDetail.BringToFront();
            ucDetail.ClearData();
            SetPageSubtitle("Create a new user profile and assign access rights without changing any workflow.");
            UpdateLayoutMode();
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
            isDetailMode = true;
            this.StatusMessage = String.Empty;
            ucDetail.Visible = true;
            ucSearchControl.ShowEditOption();
            Application.DoEvents();
            ucSearchControl.Visible = false;
            ucDetail.BringToFront();
            ucGridControl.Visible = false;
            LoadEntity();
            SetPageSubtitle("Update profile details and fine-tune module access with the same existing behavior.");
            UpdateLayoutMode();
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
            if (dataset.Tables[0].Rows.Count == 0)
            {
                ucSearchControl.ShowAddOption();
            } 
        }

        private void InitializePremiumUi()
        {
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.BackColor = PageBackColor;
            this.panelHeader.BackColor = PageBackColor;
            ConfigureCardPanel(this.panelSearchCard);
            ConfigureCardPanel(this.panelContentCard);
            StyleSearchControl();
            StyleGridControl();
            this.labelPageTitle.Text = this.Text;
            SetPageSubtitle("Search, review and maintain user profiles from a cleaner workspace.");
            this.ucDetail.Visible = false;
            UpdateLayoutMode();
        }

        private void ConfigureCardPanel(Panel panel)
        {
            panel.BackColor = CardBackColor;
            panel.Paint += CardPanel_Paint;
        }

        private void CardPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null)
            {
                return;
            }

            Rectangle borderBounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (Pen borderPen = new Pen(BorderColor))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(borderPen, borderBounds);
            }
        }

        private void SetPageSubtitle(string text)
        {
            this.labelPageTitle.Text = this.Text;
            this.labelPageSubtitle.Text = text;
        }

        private void StyleSearchControl()
        {
            this.ucSearchControl.BackColor = CardBackColor;
            foreach (Control control in this.ucSearchControl.Controls)
            {
                control.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

                TextBox textBox = control as TextBox;
                if (textBox != null)
                {
                    textBox.BackColor = Color.FromArgb(248, 250, 252);
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.ForeColor = AccentColor;
                    continue;
                }

                ComboBox comboBox = control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.BackColor = Color.White;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.ForeColor = AccentColor;
                    continue;
                }

                DateTimePicker datePicker = control as DateTimePicker;
                if (datePicker != null)
                {
                    datePicker.CalendarForeColor = AccentColor;
                    datePicker.CalendarMonthBackground = Color.White;
                    datePicker.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    continue;
                }

                Label label = control as Label;
                if (label != null)
                {
                    label.ForeColor = MutedTextColor;
                    continue;
                }

                CABButton button = control as CABButton;
                if (button != null)
                {
                    StyleSearchButton(button);
                }
            }
        }

        private void StyleSearchButton(CABButton button)
        {
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            button.ForeColor = AccentColor;
            button.BackColor = Color.FromArgb(241, 245, 249);

            if (button.Name == "lngbAdd" || button.Name == "lngbFindNow")
            {
                button.BackColor = PrimaryColor;
                button.ForeColor = Color.White;
            }
            else if (button.Name == "lngbDelete" || button.Name == "btnDeleteAll")
            {
                button.BackColor = Color.FromArgb(254, 242, 242);
                button.ForeColor = Color.FromArgb(185, 28, 28);
            }
        }

        private void StyleGridControl()
        {
            this.ucGridControl.BackColor = CardBackColor;
            Control[] gridMatches = this.ucGridControl.Controls.Find("grdData", true);
            if (gridMatches.Length > 0)
            {
                DataGridView grid = gridMatches[0] as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = CardBackColor;
                    grid.BorderStyle = BorderStyle.None;
                    grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    grid.EnableHeadersVisualStyles = false;
                    grid.GridColor = BorderColor;
                    grid.RowHeadersVisible = false;
                    grid.RowTemplate.Height = 34;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 253);
                    grid.DefaultCellStyle.BackColor = CardBackColor;
                    grid.DefaultCellStyle.ForeColor = AccentColor;
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
                    grid.DefaultCellStyle.SelectionForeColor = AccentColor;
                    grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
                    grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = AccentColor;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
                    grid.ColumnHeadersHeight = 38;
                    grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                }
            }

            Control[] noDataMatches = this.ucGridControl.Controls.Find("panelNoData", true);
            if (noDataMatches.Length > 0)
            {
                Panel noDataPanel = noDataMatches[0] as Panel;
                if (noDataPanel != null)
                {
                    noDataPanel.BackColor = CardBackColor;
                    noDataPanel.BorderStyle = BorderStyle.None;
                }
            }

            Control[] noDataLabelMatches = this.ucGridControl.Controls.Find("lngLabel1", true);
            if (noDataLabelMatches.Length > 0)
            {
                Label noDataLabel = noDataLabelMatches[0] as Label;
                if (noDataLabel != null)
                {
                    noDataLabel.ForeColor = MutedTextColor;
                    noDataLabel.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void UserInformations_Resize(object sender, EventArgs e)
        {
            UpdateLayoutMode();
        }

        private void UpdateLayoutMode()
        {
            int margin = 20;
            int contentTop = isDetailMode ? 106 : 190;
            this.panelSearchCard.Visible = !isDetailMode;
            this.panelContentCard.Location = new Point(margin, contentTop);
            this.panelContentCard.Height = Math.Max(360, this.ClientSize.Height - contentTop - margin);
        }

        private void labelPageTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
