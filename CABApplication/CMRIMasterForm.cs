/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 26/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Data;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CAB.UI
{
    public partial class CMRIMasterForm : MdiChildForm
    {
        private static readonly Color CardBackColor = Color.White;
        private static readonly Color BorderColor = Color.FromArgb(223, 229, 238);
        private static readonly Color PrimaryColor = Color.FromArgb(26, 115, 232);
        private static readonly Color AccentColor = Color.FromArgb(20, 28, 45);
        private static readonly Color MutedTextColor = Color.FromArgb(94, 108, 132);
        private CMRIMasterBLL cmriMasterBLL;
        private CMRIMasterEntity cMRIMasterEntity;
        private AreaBLL areaBLL;
        private MeterDataBLL meterDataBLL;

        public CMRIMasterForm()
        {
            InitializeComponent();
            cmriMasterBLL = new CMRIMasterBLL();
            ucGridControl.ValueColumn = ucGridControl.HiddenColumn = "CMRI_ID";
            InitializePremiumUi();
        }

        private void CMRIMasterForm_Load(object sender, EventArgs e)
        {
            ucSearchControl.SearchRequire = false;
            FillGridWithSearchType();
        }

        private void FillGridWithSearchType()
        {
            ucGridControl.Visible = true;
            ucDetail.Visible = false;
            DataSet dataSet = cmriMasterBLL.ListDataSet();
            ucSearchControl.RefreshControls(dataSet);
            ucGridControl.Data = dataSet;
            ucGridControl.IsEqual = true;
            ucGridControl.SetEqualWidth();
            ucGridControl.RefreshGrid();
        }

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            ucDetail.Visible = true;
            ucDetail.groupBoxMRIDefinition.Text = "New CMRI Definition";
            ucDetail.BringToFront();
            ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            ucDetail.ClearData();
            CenterDetailGroup(); // ensure correct position on first show
        }

        private void ucDetail_OnCancelClick(object sender, EventArgs e)
        {
            CMRIMasterForm_Load(this, null);
            ucSearchControl.Visible = true;
        }

        private void ucDetail_OnSaveClick(object sender, EventArgs e)
        {
            ucSearchControl.Visible = true;
            CMRIMasterForm_Load(this, null);
        }

        private void ucSearchControl_OnEditClick(object sender, EventArgs e)
        {
            ucDetail.Visible = true;
            ucDetail.groupBoxMRIDefinition.Text = "Edit CMRI Definition";
            ucDetail.BringToFront();
            ucSearchControl.EnableAdd = ucGridControl.Visible = false;
            ucSearchControl.Visible = false;
            LoadEntity();
            CenterDetailGroup(); // ensure correct position on first show
        }

        private void LoadEntity()
        {
            string pkValue = ucGridControl.GetPrimaryValue();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(pkValue))
            {
                string[] val = pkValue.Split('|');
                cMRIMasterEntity = (CMRIMasterEntity)cmriMasterBLL.GetDetailData(Int32.Parse(val[0]));
                ucDetail.EditData(cMRIMasterEntity);
            }
        }

        private bool ValidateDelete()
        {
            areaBLL = new AreaBLL();
            meterDataBLL = new MeterDataBLL();
            AreaEntity areaEntity = new AreaEntity();
            MeterDataEntity meterDataEntity = new MeterDataEntity();
            string pkValue = ucGridControl.GetPrimaryValue();
            string cmriID = string.Empty;
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(pkValue))
            {
                string[] val = pkValue.Split('|');
                cMRIMasterEntity = (CMRIMasterEntity)cmriMasterBLL.GetDetailData(Int32.Parse(val[0]));
                pkValue = val[0];
                cmriID = val[1];
            }
            if (!string.IsNullOrEmpty(pkValue))
            {
                //check for manual association
                areaEntity = (AreaEntity)areaBLL.GetDataforCMRI(Int32.Parse(pkValue));
                if (areaEntity == null)
                {
                    //check for auto association
                    meterDataEntity = (MeterDataEntity)meterDataBLL.GetDataforCMRI(cmriID);
                    if (meterDataEntity == null)
                    {
                        return true;
                    }
                    else
                    {
                        this.StatusMessage = "Selected CMRI allocated to a meter , cannot be deleted";
                        return false;
                    }
                }
                else
                {
                    this.StatusMessage = "Selected CMRI allocated to an area, cannot be deleted";
                    return false;
                }
            }
            return false;
        }

        private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
        {
            if (!ValidateDelete())
                return;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            if (CABMessageBox.ShowFilterMessage("M000093", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
            {
                return;
            }
            LoadEntity();
            cmriMasterBLL.DeleteData(cMRIMasterEntity);
            CMRIMasterForm_Load(this, null);
        }

        private void CMRIMasterForm_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void ucDetail_OnControlStatusChanged(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }

        private void InitializePremiumUi()
        {
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.panelToolbarCard.Paint += CardPanel_Paint;
            this.panelContentCard.Paint += CardPanel_Paint;
            StyleSearchControl();
            StyleGridControl();
            StyleDetailControl();
            this.ucDetail.Visible = false;
        }

        private void CardPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null)
            {
                return;
            }

            Rectangle bounds = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
            using (Pen pen = new Pen(BorderColor))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(pen, bounds);
            }
        }

        private void StyleSearchControl()
        {
            this.ucSearchControl.BackColor = CardBackColor;
            foreach (Control control in this.ucSearchControl.Controls)
            {
                control.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

                CABButton button = control as CABButton;
                if (button != null)
                {
                    button.Cursor = Cursors.Hand;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    button.BackColor = Color.FromArgb(241, 245, 249);
                    button.ForeColor = AccentColor;

                    if (button.Name == "lngbAdd")
                    {
                        button.BackColor = PrimaryColor;
                        button.ForeColor = Color.White;
                    }
                    else if (button.Name == "lngbDelete")
                    {
                        button.BackColor = Color.FromArgb(254, 242, 242);
                        button.ForeColor = Color.FromArgb(185, 28, 28);
                    }
                    continue;
                }

                CABLabel label = control as CABLabel;
                if (label != null)
                {
                    label.ForeColor = MutedTextColor;
                }
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
                    grid.DefaultCellStyle.BackColor = CardBackColor;
                    grid.DefaultCellStyle.ForeColor = AccentColor;
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
                    grid.DefaultCellStyle.SelectionForeColor = AccentColor;
                    grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 253);
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
        }

        private void StyleDetailControl()
        {
            this.ucDetail.BackColor = CardBackColor;
            StyleChildControls(this.ucDetail);

            if (this.ucDetail.groupBoxMRIDefinition != null)
            {
                this.ucDetail.groupBoxMRIDefinition.BackColor = CardBackColor;
                this.ucDetail.groupBoxMRIDefinition.ForeColor = AccentColor;
                this.ucDetail.groupBoxMRIDefinition.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                this.ucDetail.groupBoxMRIDefinition.Size = new Size(540, 280);
                // Wire resize on the card panel — ucDetail is DockStyle.Fill so its
                // size always equals panelContentCard; centering must use panelContentCard.
                this.panelContentCard.Resize += delegate
                {
                    CenterDetailGroup();
                };
                CenterDetailGroup();
            }
        }

        private void CenterDetailGroup()
        {
            if (this.ucDetail.groupBoxMRIDefinition == null) return;

            // Center within panelContentCard (ucDetail is DockStyle.Fill inside it)
            int containerW = this.panelContentCard.Width  - this.panelContentCard.Padding.Horizontal;
            int containerH = this.panelContentCard.Height - this.panelContentCard.Padding.Vertical;
            int left = Math.Max(24, (containerW - this.ucDetail.groupBoxMRIDefinition.Width)  / 2);
            int top  = Math.Max(24, (containerH - this.ucDetail.groupBoxMRIDefinition.Height) / 2);
            this.ucDetail.groupBoxMRIDefinition.Location = new Point(left, top);
        }

        private void StyleChildControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    label.ForeColor = AccentColor;
                    label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                }

                TextBox textBox = control as TextBox;
                if (textBox != null)
                {
                    textBox.BackColor = Color.FromArgb(248, 250, 252);
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.ForeColor = AccentColor;
                    textBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                }

                Button button = control as Button;
                if (button != null)
                {
                    button.Cursor = Cursors.Hand;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

                    if (button.Name == "btnSave")
                    {
                        button.BackColor = PrimaryColor;
                        button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(241, 245, 249);
                        button.ForeColor = AccentColor;
                    }
                }

                if (control.HasChildren)
                {
                    StyleChildControls(control);
                }
            }
        }

        private void ucDetail_Load(object sender, EventArgs e)
        {

        }
    }

}
