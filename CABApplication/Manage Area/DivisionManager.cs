using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class DivisionManager : MdiChildForm
    {
        private const string UPDATEMESSAGE = "Data updated successfully";
        private const string ADDMESSAGE = "Data added successfully";
        public DivisionManager() { InitializeComponent(); ApplyModernDarkLayout(); }


        private DivisionBLL divisionBLL = new DivisionBLL();
        private RegionBLL regionBLL = new RegionBLL();

        private DivisionEntity divisionEntity;
        public DivisionEntity Division
        {
            get;
            set;
        }

        private bool showPickButton = true;
        public bool ShowPickButton
        {
            get { return showPickButton; }
            set { showPickButton = false; }
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {

        }

        private void LoadGrid()
        {
            lgcDivision.HiddenColumn = "Division_ID";
            lgcDivision.ValueColumn = "Division_ID";
            lgcDivision.Data = divisionBLL.ListDataSet();
            lgcDivision.SetWidth("SL. No", 70);
            lgcDivision.SetWidth("Division Name", 129);
            lgcDivision.SetWidth("Region Name", 129);
            lgcDivision.SetWidth("Circle Name", 129);
            lgcDivision.Visible = true;
            groupBox.Visible = false;
        }

        private void lngbAdd_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblStatus.Text = string.Empty;
            this.txtDivision.Text = string.Empty;
            lgcDivision.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "ADD";
            divisionEntity = new DivisionEntity();
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngBFormClose.Visible = false;
            lngbPick.Visible = false;
            txtDivision.Focus();
            ddlRegion.Enabled = true;
            ddlCircle.Enabled = true;
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtDivision.Text.Trim();

            if (ddlRegion.SelectedIndex <= 0)
            {
                CABMessageBox.ShowFilterMessage("M000112", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text = MessageConstant.GetText("M000112");
                ddlRegion.Focus();
                return;
            }
            if (ddlCircle.SelectedIndex <= -1)
            {
                CABMessageBox.ShowFilterMessage("M000113", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text = MessageConstant.GetText("M000113");
                ddlCircle.Focus();
                return;
            }
            if (regionName.Equals(string.Empty))
            {
                CABMessageBox.ShowFilterMessage("M000027", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text = MessageConstant.GetText("M000027");
                txtDivision.Focus();
                return;
            }
            if (divisionEntity.DivisionID == 0)
            {
                if ((divisionBLL.ValidateDivision(regionName) as DivisionEntity).DivisionID != 0)
                {
                    CABMessageBox.ShowFilterMessage("M000028", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000028");
                    txtDivision.Focus();
                }
                else
                {
                    divisionEntity.DivisionName = regionName;
                    divisionEntity.RegionID = Convert.ToInt32(ddlRegion.SelectedValue.ToString());
                    divisionEntity.CircleID = Convert.ToInt32(ddlCircle.SelectedValue.ToString());
                    divisionBLL.InsertData(divisionEntity);
                    // Solved bug 94901
                    this.StatusMessage = ADDMESSAGE;
                    RefreshData();
                }
            }
            else
            {
                DivisionEntity rentity = divisionBLL.ValidateDivision(regionName) as DivisionEntity;
                if (rentity.DivisionID != divisionEntity.DivisionID && rentity.DivisionID != 0)
                {

                    CABMessageBox.ShowFilterMessage("M000028", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000028");
                    txtDivision.Focus();
                }
                else
                {
                    divisionEntity.DivisionName = regionName;
                    divisionEntity.RegionID = Convert.ToInt32(ddlRegion.SelectedValue);
                    divisionEntity.CircleID = Convert.ToInt32(ddlCircle.SelectedValue);
                    // Solved bug 94901
                    if (divisionBLL.UpdateData(divisionEntity))
                    {
                        this.StatusMessage = UPDATEMESSAGE;
                    }
                    RefreshData();
                }
            }
        }

        private void RefreshData()
        {
            LoadGrid();
            if (lgcDivision.GetPrimaryValue().Equals(string.Empty))
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                lngbAdd.Visible = true;
                lngbEdit.Visible = true;
                lngbDelete.Visible = true;
                lngBFormClose.Visible = true;
            }
            else
            {
                lngbAdd.Visible = true;
                lngbEdit.Visible = true;
                lngbDelete.Visible = true;
                lngBFormClose.Visible = true;
                lngbPick.Visible = true;
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
            }
            divisionEntity = null;
            lngbPick.Visible = showPickButton;
        }

        private void lngbCancel_Click(object sender, EventArgs e)
        {
            RefreshData();
            this.lblStatus.Text = string.Empty;
            this.StatusMessage = string.Empty;
        }

        private void txtDivision_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            divisionEntity = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            if (divisionEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtDivision.Text = divisionEntity.DivisionName;
            lgcDivision.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "EDIT";
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngBFormClose.Visible = false;
            lngbPick.Visible = false;
            txtDivision.Focus();
            ddlRegion.SelectedValue = divisionEntity.RegionID;
            SetCircle(divisionEntity.RegionID, divisionEntity.CircleID);
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            divisionEntity = divisionBLL.GetDetailDataConsumer(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            //if (divisionEntity == null)
            //    return;
            if (divisionEntity != null)
            {
                MessageBox.Show("Selected division is already assigned to a consumer so cannot be deleted.", "BCS");
            }
            else
            {
                if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo,MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    divisionEntity = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
                    divisionBLL.DeleteData(divisionEntity);
                    RefreshData();
                }
            }
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
            Division = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            this.Close();
        }

        private void lgcDivision_OnGridRowChanged(string msg)
        {
            this.lblStatus.Text = string.Empty;
        }

        private void lgcDivision_OnGridMouseDoubleClick(string KeyValue)
        {
            Division = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            this.Close();
        }

        private void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regionID = 0;
            if (ddlRegion.SelectedValue != null)
            {
                if (int.TryParse(ddlRegion.SelectedValue.ToString(), out regionID))
                {
                    InitializeCircle(regionID);
                }
            }
        }

        private void InitializeCircle(int regionID)
        {
            if (regionID <= -1)
            {
                ddlCircle.DataSource = null;
                return;
            }
            CircleBLL circleBLL = new CircleBLL();
            DataSet dataSet = circleBLL.GetCircleDetailData(regionID);
          
            ddlCircle.DataSource = dataSet.Tables[0];
            ddlCircle.DisplayMember = "Circle_Name";
            ddlCircle.ValueMember = "Circle_ID";
              
        }

        private void SetCircle(int regionID, int circleID)
        {
            InitializeCircle(regionID);
            ddlCircle.SelectedValue = circleID;
        }

        private void DivisionManager_Load(object sender, EventArgs e)
        {
            this.groupBox.Visible = false;
            LoadGrid();
            divisionEntity = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            if (divisionEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                divisionEntity = null;
            }
            DataSet dataSet = regionBLL.ListDataSet();
            if (dataSet != null)
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable table = dataSet.Tables[0];
                    DataRow row = table.NewRow();
                    row["Region Name"] = "";
                    row["Region_ID"] = -1;
                    table.Rows.InsertAt(row, 0);
                    ddlRegion.DataSource = table;
                    ddlRegion.DisplayMember = "Region Name";
                    ddlRegion.ValueMember = "Region_ID";
                }
            lngbPick.Visible = showPickButton;
        }

        private void lngBFormClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.StatusMessage = string.Empty;
        }
    
        // --- DARK MODE LAYOUT AND STYLING (Added 2026) ---
        private void ApplyModernDarkLayout()
        {
            System.Drawing.Color darkBg = System.Drawing.Color.FromArgb(30, 35, 45);
            System.Drawing.Color cardBg = System.Drawing.Color.FromArgb(40, 45, 55);
            System.Drawing.Color textLight = System.Drawing.Color.FromArgb(240, 240, 245);
            System.Drawing.Color accentTeal = System.Drawing.Color.FromArgb(32, 114, 126);
            System.Drawing.Color borderDark = System.Drawing.Color.FromArgb(60, 65, 75);

            this.BackColor = darkBg;

            Action<System.Windows.Forms.Control.ControlCollection> styleControls = null;
            styleControls = (controls) =>
            {
                foreach (System.Windows.Forms.Control c in controls)
                {
                    if (c is System.Windows.Forms.TextBox tb)
                    {
                        tb.BackColor = darkBg;
                        tb.ForeColor = textLight;
                        tb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    }
                    else if (c is System.Windows.Forms.ComboBox cb)
                    {
                        cb.BackColor = darkBg;
                        cb.ForeColor = textLight;
                    }
                    else if (c is System.Windows.Forms.ListBox lb)
                    {
                        lb.BackColor = darkBg;
                        lb.ForeColor = textLight;
                        lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    }
                    else if (c is System.Windows.Forms.Label lbl)
                    {
                        lbl.ForeColor = textLight;
                        lbl.BackColor = System.Drawing.Color.Transparent;
                    }
                    else if (c is System.Windows.Forms.Button btn)
                    {
                        btn.BackColor = accentTeal;
                        btn.ForeColor = textLight;
                        btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        if (btn.Text == "Cancel" || btn.Text == "Close") btn.BackColor = System.Drawing.Color.FromArgb(90, 95, 105);
                    }
                    else if (c is System.Windows.Forms.GroupBox gb)
                    {
                        gb.ForeColor = textLight;
                        gb.Paint += (s, e) => {
                            e.Graphics.Clear(cardBg);
                            using (var pen = new System.Drawing.Pen(borderDark, 1))
                            {
                                e.Graphics.DrawRectangle(pen, 0, 0, gb.Width - 1, gb.Height - 1);
                            }
                        };
                        styleControls(c.Controls);
                    }
                    else if (c is System.Windows.Forms.Panel pn)
                    {
                        pn.BackColor = darkBg;
                        styleControls(c.Controls);
                    }
                }
            };
            
            styleControls(this.Controls);
        }
}
}
