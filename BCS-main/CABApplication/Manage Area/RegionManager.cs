using System;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;

namespace CAB.UI
{
    public partial class RegionManager : MdiChildForm
    {
        private const string UPDATEMESSAGE = "Data updated successfully";
        private const string ADDMESSAGE = "Data added successfully";
        private RegionBLL regionBLL = new RegionBLL();
        private RegionEntity regionEntity;
        private bool showPickButton = true;
        public bool ShowPickButton
        {
            get { return showPickButton; }
            set { showPickButton = false; }
        }

        public RegionEntity Region
        {
            get;
            set;
        }
        public RegionManager() { InitializeComponent(); ApplyModernDarkLayout(); }


        private void RegionManager_Load(object sender, EventArgs e)
        {
            this.groupBox.Visible = false;
            LoadGrid();
            regionEntity = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            if (regionEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                regionEntity = null;
            }
            lngbPick.Visible = showPickButton;
            lngBFormClose.Visible = !showPickButton;
        }

        private void LoadGrid()
        {
            lgcRegion.HiddenColumn = "Region_ID";
            lgcRegion.ValueColumn = "Region_ID";
            lgcRegion.Data = regionBLL.ListDataSet();
            lgcRegion.SetWidth("SL. No", 70);
            lgcRegion.SetWidth("Region Name", 387);
            lgcRegion.Visible = true;
            groupBox.Visible = false;
        }

        private void lngbAdd_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            lblStatus.Text = string.Empty;
            this.txtRegion.Text = string.Empty;
            lgcRegion.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "ADD";
            regionEntity = new RegionEntity();
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngBFormClose.Visible = false;
            lngbPick.Visible = false;
            txtRegion.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtRegion.Text.Trim();
            if (regionName.Equals(string.Empty))
            {
                CABMessageBox.ShowFilterMessage("M000023", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //lblStatus.Text=MessageConstant.GetText("M000023"); 
                txtRegion.Focus();
                return;
            }

            if (regionEntity.RegionID == 0)
            {
                if ((regionBLL.ValidateRegion(regionName) as RegionEntity).RegionID != 0)
                {
                    CABMessageBox.ShowFilterMessage("M000024", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000024");
                    txtRegion.Focus();
                }
                else
                {
                    regionEntity.RegionName = regionName;
                    regionBLL.InsertData(regionEntity);
                    // Solved bug 94901
                    this.StatusMessage = ADDMESSAGE;
                    RefreshData();
                }
            }
            else
            {
                RegionEntity rentity = regionBLL.ValidateRegion(regionName) as RegionEntity;
                if (rentity.RegionID != regionEntity.RegionID && rentity.RegionID != 0)
                {
                    CABMessageBox.ShowFilterMessage("M000024", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //lblStatus.Text = MessageConstant.GetText("M000024");
                    txtRegion.Focus();
                }
                else
                {
                    regionEntity.RegionName = regionName;
                    // Solved bug 94901
                    if (regionBLL.UpdateData(regionEntity))
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

        private void txtRegion_TextChanged(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            regionEntity = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            if (regionEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtRegion.Text = regionEntity.RegionName;
            lgcRegion.Visible = false;
            groupBox.Visible = true;
            groupBox.Text = "EDIT";
            lngbAdd.Visible = false;
            lngbEdit.Visible = false;
            lngbDelete.Visible = false;
            lngBFormClose.Visible = false;
            lngbPick.Visible = false;
            txtRegion.Focus();
        }

        private void RefreshData()
        {
            LoadGrid();
            if (lgcRegion.GetPrimaryValue().Equals(string.Empty))
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
                lngbPick.Visible = true;
                lngbAdd.Visible = true;
                lngbEdit.Visible = true;
                lngbDelete.Visible = true;
                lngBFormClose.Visible = true;
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
            }
            lngbPick.Visible = showPickButton;
            regionEntity = null;
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            regionEntity = regionBLL.GetDetailDataConsumerMeter(lgcRegion.GetPrimaryValue()) as RegionEntity;
            //if (regionEntity == null)
            //    return;
            if (regionEntity != null)
            {
                MessageBox.Show("Selected region is already assigned to a consumer so cannot be deleted.", "BCS");
            }
            else
            {
                regionEntity = regionBLL.GetDetailDataCircle(lgcRegion.GetPrimaryValue()) as RegionEntity;
                if (regionEntity != null)
                {
                    MessageBox.Show("Selected region is already assigned to a circle so cannot be deleted.", "BCS");
                }
                else
                {
                    regionEntity = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
                    if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                    {
                        regionBLL.DeleteData(regionEntity);
                        RefreshData();
                    }
                }
            }
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
            Region = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            this.Close();
        }

        private void lgcRegion_OnGridRowChanged(string msg)
        {
            this.lblStatus.Text = string.Empty;
        }

        private void lgcRegion_OnGridMouseDoubleClick(string KeyValue)
        {
            Region = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            this.Close();
        }

        private void lngBFormClose_Click(object sender, EventArgs e)
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
