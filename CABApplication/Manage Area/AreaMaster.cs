using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Framework.Entity;

namespace CAB.UI
{
    public partial class AreaMaster : MdiChildForm
    {
        private RegionEntity regionEntity = new RegionEntity();
        private DivisionEntity divisionEntity = new DivisionEntity();
        private CircleEntity circleEntity = new CircleEntity();
        private AreaEntity areaEntity = new AreaEntity();
        private AreaBLL areaBLL = new AreaBLL();
        private RegionBLL regionBLL = new RegionBLL();
        private DivisionBLL divisionBLL = new DivisionBLL();
        private MeterDataBLL meterDataBLL = new MeterDataBLL();
        private FileUploadMasterBLL fileUploadMasterBLL = new FileUploadMasterBLL();
                private CircleBLL circleBLL = new CircleBLL();
        private AreaMeterBLL areaMeterBLL = new AreaMeterBLL();
        public static int Region_ID;
        public static int Circle_ID;
        public AreaMaster()
        {
            InitializeComponent();
            ApplyModernDarkLayout();
        }

        // --- DARK MODE LAYOUT AND STYLING (Added 2026) ---
        private void ApplyModernDarkLayout()
        {
            // Global Colors
            System.Drawing.Color darkBg = System.Drawing.Color.FromArgb(30, 35, 45);
            System.Drawing.Color cardBg = System.Drawing.Color.FromArgb(40, 45, 55);
            System.Drawing.Color textLight = System.Drawing.Color.FromArgb(240, 240, 245);
            System.Drawing.Color accentTeal = System.Drawing.Color.FromArgb(32, 114, 126);
            System.Drawing.Color borderDark = System.Drawing.Color.FromArgb(60, 65, 75);

            this.BackColor = darkBg;
            this.pnAddArea.BackColor = darkBg;
            this.pnNewAreaDefinition.BackColor = darkBg;
            
            // Restructure Container
            this.pnNewAreaDefinition.Size = new System.Drawing.Size(900, 560);
            this.pnNewAreaDefinition.Location = new System.Drawing.Point(Math.Max(0, (this.pnAddArea.Width - 900) / 2), 20);
            this.GrpAddNew.Text = "";
            this.GrpAddNew.Size = new System.Drawing.Size(880, 540);
            this.GrpAddNew.Location = new System.Drawing.Point(10, 10);
            
            // Area Hierarchy (Left side)
            this.gbArea.Text = ""; // Hidden border via custom paint later
            this.gbArea.ForeColor = textLight;
            this.gbArea.Size = new System.Drawing.Size(380, 240);
            this.gbArea.Location = new System.Drawing.Point(20, 50);
            
            this.lblRegion.Location = new System.Drawing.Point(20, 20);
            this.txtRegion.Location = new System.Drawing.Point(20, 40);
            this.txtRegion.Size = new System.Drawing.Size(200, 25);
            this.btnRegion.Location = new System.Drawing.Point(230, 38);
            this.btnRegion.Text = "Pick";
            this.btnRegion.Size = new System.Drawing.Size(130, 28);
            
            this.lblCircle.Location = new System.Drawing.Point(20, 85);
            this.txtCircle.Location = new System.Drawing.Point(20, 105);
            this.txtCircle.Size = new System.Drawing.Size(200, 25);
            this.btnCircle.Location = new System.Drawing.Point(230, 103);
            this.btnCircle.Text = "Pick";
            this.btnCircle.Size = new System.Drawing.Size(130, 28);
            
            this.lblDivision.Location = new System.Drawing.Point(20, 150);
            this.txtDivision.Location = new System.Drawing.Point(20, 170);
            this.txtDivision.Size = new System.Drawing.Size(200, 25);
            this.btnDivision.Location = new System.Drawing.Point(230, 168);
            this.btnDivision.Text = "Pick";
            this.btnDivision.Size = new System.Drawing.Size(130, 28);
            
            // CMRI Device (Bottom Left)
            this.gbCMRI.Text = "";
            this.gbCMRI.ForeColor = textLight;
            this.gbCMRI.Size = new System.Drawing.Size(380, 80);
            this.gbCMRI.Location = new System.Drawing.Point(20, 310);
            this.lblMRI.Visible = false;
            this.cboMRI.Location = new System.Drawing.Point(20, 35);
            this.cboMRI.Size = new System.Drawing.Size(340, 25);
            
            // Meter Management (Right side)
            this.lblAvailableMeters.Location = new System.Drawing.Point(440, 50);
            this.lblAvailableMeters.ForeColor = textLight; this.lblAvailableMeters.BackColor = cardBg;
            this.lblAvailableMeters.Text = "Available Meters";
            this.lblAvailableMeters.Size = new System.Drawing.Size(150, 30);
            
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(440, 90);
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(180, 280);
            this.listBoxAvailableMeters.BackColor = cardBg;
            this.listBoxAvailableMeters.ForeColor = textLight;
            this.listBoxAvailableMeters.BorderStyle = BorderStyle.FixedSingle;
            
            this.btnMoveNext.Location = new System.Drawing.Point(630, 150);
            this.btnMoveNext.Text = "->";
            this.btnMoveNext.Size = new System.Drawing.Size(35, 30);
            
            this.btnMoveNextAll.Location = new System.Drawing.Point(630, 190);
            this.btnMoveNextAll.Text = "=>";
            this.btnMoveNextAll.Size = new System.Drawing.Size(35, 30);
            
            this.btnMoveBack.Location = new System.Drawing.Point(630, 230);
            this.btnMoveBack.Text = "<-";
            this.btnMoveBack.Size = new System.Drawing.Size(35, 30);
            
            this.btnMoveBackAll.Location = new System.Drawing.Point(630, 270);
            this.btnMoveBackAll.Text = "<=";
            this.btnMoveBackAll.Size = new System.Drawing.Size(35, 30);
            
            this.lblSelectedMeters.Location = new System.Drawing.Point(680, 50);
            this.lblSelectedMeters.ForeColor = textLight; this.lblSelectedMeters.BackColor = cardBg;
            this.lblSelectedMeters.Text = "Selected Meters";
            this.lblSelectedMeters.Size = new System.Drawing.Size(150, 30);
            
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(680, 90);
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(180, 280);
            this.listBoxSelectedMeters.BackColor = cardBg;
            this.listBoxSelectedMeters.ForeColor = textLight;
            this.listBoxSelectedMeters.BorderStyle = BorderStyle.FixedSingle;

            // Bottom Buttons
            this.btnSave.Location = new System.Drawing.Point(640, 460);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.BackColor = accentTeal;
            this.btnSave.ForeColor = textLight;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            
            this.btnCancel.Location = new System.Drawing.Point(750, 460);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(90, 95, 105);
            this.btnCancel.ForeColor = textLight;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            
            // Format existing inputs
            foreach (Control c in this.gbArea.Controls)
            {
                if (c is TextBox tb)
                {
                    tb.BackColor = darkBg;
                    tb.ForeColor = textLight;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
                if (c is Label lbl) { lbl.ForeColor = textLight; lbl.BackColor = cardBg; }
                if (c is CAB.UI.Controls.CABButton btn)
                {
                    btn.BackColor = accentTeal;
                    btn.ForeColor = textLight;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                }
            }
            
            foreach (Control c in this.GrpAddNew.Controls)
            {
                if (c is CAB.UI.Controls.CABButton btn && btn.Text.Length <= 2) // arrows
                {
                    btn.BackColor = accentTeal;
                    btn.ForeColor = textLight;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                }
            }

            // Remove borders of GroupBoxes via Paint
            this.gbArea.Paint += (s, e) => { e.Graphics.Clear(cardBg); };
            this.gbCMRI.Paint += (s, e) => { e.Graphics.Clear(cardBg); };
            
            // Repaint container for cards
            this.GrpAddNew.Paint += (s, e) => {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.Clear(darkBg);
                
                // Draw title
                e.Graphics.DrawString("Define New Area", new Font("Segoe UI", 16, FontStyle.Bold), new SolidBrush(textLight), 10, 0);
                
                using (var brush = new SolidBrush(cardBg))
                using (var pen = new Pen(borderDark, 1))
                {
                    // Draw Area Hierarchy Card
                    e.Graphics.FillRectangle(brush, 10, 40, 400, 360);
                    e.Graphics.DrawRectangle(pen, 10, 40, 400, 360);
                    e.Graphics.DrawString("Area Hierarchy", new Font("Segoe UI", 10, FontStyle.Bold), new SolidBrush(textLight), 20, 50);
                    
                    // Draw Meter Management Card
                    e.Graphics.FillRectangle(brush, 420, 40, 450, 360);
                    e.Graphics.DrawRectangle(pen, 420, 40, 450, 360);
                    e.Graphics.DrawString("Meter Management", new Font("Segoe UI", 10, FontStyle.Bold), new SolidBrush(textLight), 580, 20);
                    
                    // Draw bottom bar
                    e.Graphics.FillRectangle(brush, 10, 440, 860, 70);
                    e.Graphics.DrawRectangle(pen, 10, 440, 860, 70);
                }
            };
            
            // Override Resize to keep it centered
                        this.pnAddArea.Resize += (s, e) => {
                this.pnNewAreaDefinition.Location = new System.Drawing.Point(Math.Max(0, (this.pnAddArea.Width - this.pnNewAreaDefinition.Width) / 2), 40);
            };
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            RegionMaster regionMaster = new RegionMaster();
            regionMaster.StartPosition = FormStartPosition.CenterScreen;
            regionMaster.ShowDialog();
            regionEntity = regionMaster.Region;
            txtCircle.Text = "";
            txtDivision.Text = "";

            if (regionEntity != null)
            {
                txtRegion.Text = regionEntity.RegionName;
                Region_ID = regionEntity.RegionID;
            }
            else
                txtRegion.Text = string.Empty;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (txtRegion.Text == "")
            {
                this.StatusMessage = "Region name cannot be left blank";
                return;
            }

            CircleMaster circleMaster = new CircleMaster();
            circleMaster.StartPosition = FormStartPosition.CenterScreen;
            circleMaster.ShowDialog();
            txtDivision.Text = "";

            circleEntity = circleMaster.Circle;
            if (circleEntity != null)
            {
                txtCircle.Text = circleEntity.CircleName;
                Circle_ID = circleEntity.CircleID;
            }
            else
                txtCircle.Text = string.Empty;
        }

        private void btnDivision_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            if (txtRegion.Text == "")
            {
                this.StatusMessage = "Region name cannot be left blank";
                return;
            }
            if (txtCircle.Text == "")
            {
                this.StatusMessage = "Circle name cannot be left blank";
                return;
            }
            DivisionMaster divisionMaster = new DivisionMaster();
            divisionMaster.StartPosition = FormStartPosition.CenterScreen;
            divisionMaster.ShowDialog();
            divisionEntity = divisionMaster.Division;
            if (divisionEntity != null)
            {
                txtDivision.Text = divisionEntity.DivisionName;
            }
            else
                txtDivision.Text = string.Empty;
        }

        private void ListDetails()
        {
            gridAreaMaster.Data = areaBLL.ListDetails();
            if (gridAreaMaster.Data.Tables[0].Rows.Count == 0)
            {
                ucSearchControl.EnableEdit = false;
                ucSearchControl.EnableDelete = false;
            }
            else
            {
                ucSearchControl.EnableEdit = true;
                ucSearchControl.EnableDelete = true;
            }
        }

        private void AreaMaster_Load(object sender, EventArgs e)
        {
            this.Text = "Area Definition";
            gbArea.Text = MessageConstant.GetText("L000044");
            gbCMRI.Text = MessageConstant.GetText("L000045");
            CMRIMasterBLL cmriBLL = new CMRIMasterBLL();
            cboMRI.DisplayMember = "CMRI Number";
            cboMRI.ValueMember = "CMRI_ID";
            cboMRI.DataSource = cmriBLL.ListDataSet().Tables[0];
            SetControls();
            pnNewAreaDefinition.Hide();
            areaBLL = new AreaBLL();
            ListDetails();
            gridAreaMaster.HiddenColumn = gridAreaMaster.ValueColumn = "Area_ID";
            //gridAreaMaster.RefreshGrid();
            pnAreaMaster.Show();
            this.pnAreaMaster.Location = new Point(19, 16);
        }

        private void GetMeterID()
        {
            MeterMasterBLL metermasterBLL = new MeterMasterBLL();
            DataTable dt = metermasterBLL.ListUnAssignedAreaMeterID().Tables[0]; //metermasterBLL.ListMeterID().Tables[0];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                listBoxAvailableMeters.Items.Add(dt.Rows[i].ItemArray[0]);
            }
        }

        private bool CheckBeforeSave()
        {
            if (txtRegion.Text == "")
            {
                this.StatusMessage = "Region name cannot be left blank";
                return false;
            }
            if (txtCircle.Text == "")
            {
                this.StatusMessage = "Circle name cannot be left blank";
                return false;
            }
            if (txtDivision.Text == "")
            {
                this.StatusMessage = "Division name cannot be left blank";
                return false;
            }
            if (cboMRI.Items.Count == 0)
            {
                this.StatusMessage = "Please select CMRI";
                return false;
            }
            if (listBoxSelectedMeters.Items.Count == 0)
            {
                this.StatusMessage = "No meter selected";
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (!CheckBeforeSave())
                return;
            if (areaEntity.Area_ID == 0)  //Insert
            {
                areaEntity.Region_ID = regionEntity.RegionID;
                areaEntity.Divsion_ID = divisionEntity.DivisionID;
                areaEntity.Circle_ID = circleEntity.CircleID;
                string id = "0";
                string cmriNo = string.Empty;
                if (cboMRI.Items.Count > 0)
                {
                    id = ((System.Data.DataRowView)(cboMRI.Items[cboMRI.SelectedIndex])).Row.ItemArray[0].ToString();
                    cmriNo = ((System.Data.DataRowView)(cboMRI.Items[cboMRI.SelectedIndex])).Row.ItemArray[1].ToString();
                }
                areaEntity.CMRI_ID = Convert.ToInt32(id);
                List<IEntity> areaMeterEntities = new List<IEntity>();
                for (int i = 0; i < listBoxSelectedMeters.Items.Count; i++)
                {
                    AreaMeterEntity areameterEntity = new AreaMeterEntity();
                    areameterEntity.Meter_ID = listBoxSelectedMeters.Items[i].ToString();
                    areaMeterEntities.Add(areameterEntity);

                }

                flag = areaBLL.ValidateData(areaEntity);
                if (!flag)
                {
                    flag = areaMeterBLL.validateMeter(areaMeterEntities);
                    if (flag)
                    {
                        areaEntity = areaBLL.InsertData(areaEntity) as AreaEntity;
                        foreach (AreaMeterEntity areameterEntity in areaMeterEntities)
                        {
                            areameterEntity.Area_ID = areaEntity.Area_ID;

                            //update meter data
                            DataSet fuploadIDs = meterDataBLL.UpdateCMRIID(areameterEntity.Meter_ID, cmriNo);

                            //update file upload 
                            fileUploadMasterBLL.UpdateCMRIID(fuploadIDs, cmriNo);
                        }
                        areaMeterBLL.InsertData(areaMeterEntities);

                        //gridAreaMaster.Refresh();
                        ListDetails();
                        this.StatusMessage = "Data saved successfully";
                        pnAreaMaster.Show();
                        this.pnAreaMaster.Location = new Point(19, 16);
                        pnNewAreaDefinition.Hide();
                    }
                    else
                    {
                        this.StatusMessage = "Meter(s) already allocated.";
                        Application.DoEvents();
                    }
                }
                else
                {
                    this.StatusMessage = "Area already exist.";
                    Application.DoEvents();
                }
            }
            else//Update
            {
                areaEntity.Region_ID = regionEntity.RegionID;
                areaEntity.Divsion_ID = divisionEntity.DivisionID;
                areaEntity.Circle_ID = circleEntity.CircleID;
                string id = ((System.Data.DataRowView)(cboMRI.Items[cboMRI.SelectedIndex])).Row.ItemArray[0].ToString();
                string cmriNo = ((System.Data.DataRowView)(cboMRI.Items[cboMRI.SelectedIndex])).Row.ItemArray[1].ToString();
                areaEntity.CMRI_ID = Convert.ToInt32(id);
                List<IEntity> areaMeterEntities = new List<IEntity>();
                if (areaBLL.UpdateData(areaEntity))
                {
                    AreaMeterEntity areameterEntity = new AreaMeterEntity();
                    DataSet dataset = new DataSet();
                    areaMeterBLL.DeleteMeters(areaEntity.Area_ID);

                    for (int i = 0; i < listBoxSelectedMeters.Items.Count; i++)
                    {
                        areameterEntity = new AreaMeterEntity();
                        areameterEntity.Meter_ID = listBoxSelectedMeters.Items[i].ToString();
                        areameterEntity.Area_ID = areaEntity.Area_ID;
                        areaMeterEntities.Add(areameterEntity);
                    }
                    flag = areaMeterBLL.validateMeter(areaMeterEntities);

                    if (flag)
                    {

                        foreach (AreaMeterEntity ame in areaMeterEntities)
                        {
                            //update meter data
                            DataSet fuploadIDs = meterDataBLL.UpdateCMRIID(ame.Meter_ID, cmriNo);

                            //update file upload 
                            fileUploadMasterBLL.UpdateCMRIID(fuploadIDs, cmriNo);
                        }

                        areaMeterBLL.InsertData(areaMeterEntities);
                        ListDetails();
                        this.StatusMessage = "Data updated successfully";
                        pnAreaMaster.Show();
                        this.pnAreaMaster.Location = new Point(19, 16);
                        pnNewAreaDefinition.Hide();
                    }
                    else
                    {
                        this.StatusMessage = "Meter(s) already allocated.";
                        Application.DoEvents();
                    }
                }
            }
        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxAvailableMeters.SelectedItems)
            {
                listBoxSelectedMeters.Items.Add(item);
            }
            for (int index = listBoxAvailableMeters.SelectedIndices.Count; index > 0; index--)
            {
                listBoxAvailableMeters.Items.RemoveAt(listBoxAvailableMeters.SelectedIndex);
            }
            if (listBoxAvailableMeters.Items.Count != 0)
            {
                listBoxAvailableMeters.SelectedIndex = 0;
            }
        }

        private void btnMoveNextAll_Click(object sender, EventArgs e)
        {
            if (listBoxAvailableMeters.Items.Count == 0)
            {
                return;
            }
            foreach (object item in listBoxAvailableMeters.Items)
            {
                listBoxSelectedMeters.Items.Add(item);
            }
            listBoxAvailableMeters.Items.Clear();
        }

        private void btnMoveBack_Click(object sender, EventArgs e)
        {
            foreach (object item in listBoxSelectedMeters.SelectedItems)
            {
                listBoxAvailableMeters.Items.Add(item);
            }
            for (int SelectedIndex = listBoxSelectedMeters.SelectedIndices.Count; SelectedIndex > 0; SelectedIndex--)
            {
                listBoxSelectedMeters.Items.RemoveAt(listBoxSelectedMeters.SelectedIndex);
            }
            if (listBoxSelectedMeters.Items.Count != 0)
            {
                listBoxSelectedMeters.SelectedIndex = 0;
            }
        }

        private void btnMoveBackAll_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedMeters.Items.Count == 0)
            {
                return;
            }
            foreach (object item in listBoxSelectedMeters.Items)
            {
                listBoxAvailableMeters.Items.Add(item);
            }
            listBoxSelectedMeters.Items.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "";
            pnAreaMaster.Show();
            pnNewAreaDefinition.Hide();
        }

        private void btnMasterCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ucSearchControl_OnAddClick(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            areaEntity = new AreaEntity();
            pnAreaMaster.Hide();
            pnNewAreaDefinition.Show();
            GrpAddNew.Text = "Add New Area";
            this.pnNewAreaDefinition.Location = new Point(19, 16);
            SetControls();
        }

        private void ucSearchControl_OnDeleteClick(object sender, EventArgs e)
        {
            bool flag = false;
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            if (CABMessageBox.ShowFilterMessage("M000093", "A000001", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
            {
                return;
            }
            long areaId = Convert.ToInt64(gridAreaMaster.GetPrimaryValue());
            areaBLL = new AreaBLL();
            areaMeterBLL = new AreaMeterBLL();
            areaEntity = new AreaEntity();
            flag = areaBLL.DeleteData(areaId);
            if (!flag)
            {
                this.StatusMessage = "Unable to delete the record.";
                Application.DoEvents();
            }
            else
            {

                flag = areaMeterBLL.DeleteMeters(areaId);
                if (!flag)
                {
                    this.StatusMessage = "Unable to delete the record.";
                    Application.DoEvents();
                }
                else
                {
                    // gridAreaMaster.Refresh();
                    ListDetails();
                    this.StatusMessage = "Data deleted successfully";
                    Application.DoEvents();
                }
            }
        }

        private void SetControls()
        {
            try
            {
                DataSet ds = new DataSet();
                areaBLL = new AreaBLL();
                ds = areaBLL.ListDataSet();
                txtRegion.Text = "";
                txtCircle.Text = "";
                txtDivision.Text = "";

                if (cboMRI.Items.Count > 0)
                    cboMRI.SelectedIndex = 0;
                listBoxAvailableMeters.Items.Clear();
                listBoxSelectedMeters.Items.Clear();
                GetMeterID();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void btnMoveNextAll_Click()
        {
            throw new NotImplementedException();
        }

        private void ucSearchControl_OnEditClick(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            int areaId = Convert.ToInt32(gridAreaMaster.GetPrimaryValue());
            areaEntity = areaBLL.GetDetailData(areaId) as AreaEntity;
            regionEntity = regionBLL.GetDetailData(areaEntity.Region_ID) as RegionEntity;
            circleEntity = circleBLL.GetDetailData(areaEntity.Circle_ID) as CircleEntity;
            divisionEntity = divisionBLL.GetDetailData(areaEntity.Divsion_ID) as DivisionEntity;
            txtRegion.Text = regionEntity.RegionName;
            txtCircle.Text = circleEntity.CircleName;
            txtDivision.Text = divisionEntity.DivisionName;

            AreaMeterBLL areameterBLL = new AreaMeterBLL();
            AreaMeterEntity areameterEntity = new AreaMeterEntity();
            DataSet areaMeterData = areameterBLL.ListDataSet(areaId);
            listBoxSelectedMeters.Items.Clear();
            if (areaMeterData != null)
            {
                foreach (DataRow dr in areaMeterData.Tables[0].Rows)
                    listBoxSelectedMeters.Items.Add(dr["Meter_ID"]);
            }
            foreach (object item in listBoxSelectedMeters.Items)
                listBoxAvailableMeters.Items.Remove(item);


            cboMRI.SelectedValue = areaEntity.CMRI_ID;




            areaMeterData = new DataSet();

            pnAreaMaster.Hide();
            pnNewAreaDefinition.Show();
            GrpAddNew.Text = "Edit Area";
            this.pnNewAreaDefinition.Location = new Point(19, 16);
        }

        private void AreaMaster_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void AreaMaster_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StatusMessage = "";
            this.RightStatusMessage = "";
        }

        private void lblAvailableMeters_Click(object sender, EventArgs e)
        {

        }

        private void listBoxAvailableMeters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}



