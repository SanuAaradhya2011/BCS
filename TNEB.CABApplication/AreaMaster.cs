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
using CAB.IECFramework.Entity; 

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
       private CircleBLL circleBLL = new CircleBLL();
       private AreaMeterBLL areaMeterBLL = new AreaMeterBLL();
        public AreaMaster()
        {
            InitializeComponent();
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            RegionMaster regionMaster = new RegionMaster();
			regionMaster.StartPosition = FormStartPosition.CenterScreen;
            regionMaster.ShowDialog();
            regionEntity = regionMaster.Region;
            if (regionEntity != null)
                txtRegion.Text = regionEntity.RegionName;
            else
                txtRegion.Text = string.Empty;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            CircleMaster circleMaster = new CircleMaster();
			circleMaster.StartPosition = FormStartPosition.CenterScreen;
            circleMaster.ShowDialog();
            circleEntity = circleMaster.Circle;
            if (circleEntity != null)
                txtCircle.Text = circleEntity.CircleName;
            else
                txtCircle.Text = string.Empty;
        }

        private void btnDivision_Click(object sender, EventArgs e)
        {
            DivisionMaster divisionMaster = new DivisionMaster();
			divisionMaster.StartPosition = FormStartPosition.CenterScreen;
            divisionMaster.ShowDialog();
            divisionEntity = divisionMaster.Division;
            if (divisionEntity != null)
                txtDivision.Text = divisionEntity.DivisionName;
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
            gridAreaMaster.RefreshGrid();
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
                if(cboMRI.Items.Count>0)
                id=((System.Data.DataRowView)(cboMRI.Items[cboMRI.SelectedIndex])).Row.ItemArray[0].ToString();
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
                            areameterEntity.Area_ID = areaEntity.Area_ID;
                        areaMeterBLL.InsertData(areaMeterEntities);
                        gridAreaMaster.Refresh();
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
            this.pnNewAreaDefinition.Location = new Point(19,16);
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
            areaMeterBLL=new AreaMeterBLL();
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
                    gridAreaMaster.Refresh();
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
                txtRegion.Enabled = false;
                txtCircle.Enabled = false; 
                txtDivision.Enabled = false;
                if(cboMRI.Items.Count>0)
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
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
        }
    }
}
