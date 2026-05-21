using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class CircleManager : MdiChildForm
    {
        private const string UPDATEMESSAGE = "Data updated successfully";
        private const string ADDMESSAGE = "Data added successfully";
        public CircleManager() { InitializeComponent(); ApplyModernDarkLayout(); }

        private CircleBLL circleBLL = new CircleBLL();

        private CircleEntity circleEntity;
        public CircleEntity Circle
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

        private void CircleManager_Load(object sender, EventArgs e)
        {
            InitializeRegions();
            this.groupBox.Visible = false;
            LoadGrid();
            circleEntity = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            if (circleEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                circleEntity = null;
            }
            lngbPick.Visible = showPickButton;
            lngbFormClose.Visible = !showPickButton;
        }

        private void InitializeRegions()
        {
            RegionBLL regionBLL = new RegionBLL();
            DataSet dataSet = regionBLL.ListDataSet();
            if (dataSet != null)
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    ddlRegion.DataSource = dataSet.Tables[0];
                    ddlRegion.DisplayMember = "Region Name";
                    ddlRegion.ValueMember = "Region_ID";
                }
        }

        private void LoadGrid()
        {
            lgcCircle.HiddenColumn = "Circle_ID";
            lgcCircle.ValueColumn = "Circle_ID";
            lgcCircle.Data = circleBLL.ListDataSet();
            lgcCircle.SetWidth("SL. No", 70);
            lgcCircle.SetWidth("Circle Name", 193);
            lgcCircle.SetWidth("Region Name", 194);
            lgcCircle.Visible = true;
            groupBox.Visible = false;
        }

        private void lngbAdd_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblStatus.Text = string.Empty;
            this.txtCircle.Text = string.Empty;
            lgcCircle.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "ADD";
            circleEntity = new CircleEntity();
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngbFormClose.Visible = false;
            lngbPick.Visible = false;
            txtCircle.Focus();
            ddlRegion.Enabled = true;
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtCircle.Text.Trim();
            if (ddlRegion.SelectedIndex <= -1)
            {
                CABMessageBox.ShowFilterMessage("M000112", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text = MessageConstant.GetText("M000112");
                ddlRegion.Focus();
                return;
            }
            if (regionName.Equals(string.Empty))
            {
                CABMessageBox.ShowFilterMessage("M000030", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text = MessageConstant.GetText("M000030");
                txtCircle.Focus();
                return;
            }

            if (circleEntity.CircleID == 0)
            {
                if ((circleBLL.ValidateCircle(regionName) as CircleEntity).CircleID != 0)
                {
                    CABMessageBox.ShowFilterMessage("M000031", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000031");
                    txtCircle.Focus();
                }
                else
                {
                    circleEntity.CircleName = regionName;
                    circleEntity.RegionID = Convert.ToInt32(ddlRegion.SelectedValue);
                    circleBLL.InsertData(circleEntity);
                    // Solved bug 94901
                    this.StatusMessage = ADDMESSAGE;
                    RefreshData();
                }
            }
            else
            {
                CircleEntity rentity = circleBLL.ValidateCircle(regionName) as CircleEntity;
                if (rentity.CircleID != circleEntity.CircleID && rentity.CircleID != 0)
                {
                    CABMessageBox.ShowFilterMessage("M000031", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000031");
                    txtCircle.Focus();
                }
                else
                {
                    circleEntity.CircleName = regionName;
                    circleEntity.RegionID = Convert.ToInt32(ddlRegion.SelectedValue);
                    // Solved bug 94901
                    if (circleBLL.UpdateData(circleEntity))
                    {
                        this.StatusMessage = UPDATEMESSAGE;
                    }
                    RefreshData();
                }
            }
        }

        private void lngbCancel_Click(object sender, EventArgs e)
        {
            RefreshData();
            this.lblStatus.Text = string.Empty;
            this.StatusMessage = string.Empty;
        }

        private void txtCircle_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            circleEntity = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            if (circleEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCircle.Text = circleEntity.CircleName;
            lgcCircle.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "EDIT";
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngbFormClose.Visible = false;
            lngbPick.Visible = false;
            txtCircle.Focus();
            ddlRegion.SelectedValue = circleEntity.RegionID;
        }

        private void RefreshData()
        {
            LoadGrid();
            if (lgcCircle.GetPrimaryValue().Equals(string.Empty))
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                lngbAdd.Visible = true;
                lngbEdit.Visible = true;
                lngbDelete.Visible = true;
                lngbFormClose.Visible = true;
            }
            else
            {
                lngbAdd.Visible = true;
                lngbEdit.Visible = true;
                lngbDelete.Visible = true;
                lngbFormClose.Visible = true;
                lngbPick.Visible = true;
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
            }
            lngbPick.Visible = showPickButton;
            circleEntity = null;
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            circleEntity = circleBLL.GetDetailDataConsumer(lgcCircle.GetPrimaryValue()) as CircleEntity;
            if (circleEntity == null)
            {
                circleEntity = circleBLL.GetDetailDataDivision(lgcCircle.GetPrimaryValue()) as CircleEntity;
                if (circleEntity == null)
                {
                    if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    {
                        circleEntity = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
                        circleBLL.DeleteData(circleEntity);
                        RefreshData();
                    }
                }
                else
                {
                    MessageBox.Show("Selected Circle is already assigned to a division so cannot be deleted.", "BCS");
                }
            }
            else
            {
                MessageBox.Show("Selected Circle is already assigned to a consumer so cannot be deleted.", "BCS");
            }
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
            Circle = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            this.Close();
        }

        private void lgcCircle_OnGridRowChanged(string msg)
        {
            this.lblStatus.Text = string.Empty;
        }

        private void lgcCircle_OnGridMouseDoubleClick(string KeyValue)
        {
            Circle = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            this.Close();
        }

        private void lngbFormClose_Click(object sender, EventArgs e)
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
