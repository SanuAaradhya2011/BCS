using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.IECFramework.Utility;
using System.Threading;

namespace CAB.UI
{
    public partial class RegionMaster : Form
    {
        private RegionBLL regionBLL = new RegionBLL();
        private RegionEntity regionEntity;
        public RegionEntity Region
        {
            get;
            set;
        }
        public RegionMaster()
        {
            InitializeComponent();
        }

        private void RegionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("M000022"); 
            this.pRegion.Visible = false;
            LoadGrid();
            regionEntity = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;  
            if (regionEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                regionEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcRegion.HiddenColumn = "Region_ID";
            lgcRegion.ValueColumn = "Region_ID"; 
            lgcRegion.Data = regionBLL.ListDataSet();
            lgcRegion.SetWidth("SL. No", 70);
            lgcRegion.SetWidth("Region Name", 387);
            lgcRegion.IsSorting = false;
            lgcRegion.Visible = true;
            pRegion.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtRegion.Text = string.Empty;
            lgcRegion.Visible = false;
            pRegion.Visible = true;
            regionEntity = new RegionEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtRegion.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtRegion.Text.Trim();
            if (regionName.Equals(string.Empty))
            {
                lblStatus.Text=MessageConstant.GetText("M000023"); 
                txtRegion.Focus();
                return;
            }
            if (regionEntity.RegionID == 0)
            {
                if ((regionBLL.ValidateRegion(regionName) as RegionEntity).RegionID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000024");
                    txtRegion.Focus();
                }
                else
                {
                    regionEntity.RegionName = regionName;
                    regionBLL.InsertData(regionEntity);
                    RefreshData();
                }
            }
            else
            {
                RegionEntity rentity=regionBLL.ValidateRegion(regionName) as RegionEntity;
                if (rentity.RegionID != regionEntity.RegionID && rentity.RegionID !=0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000024");
                    txtRegion.Focus();
                }
                else
                {
                    regionEntity.RegionName = regionName;
                    regionBLL.UpdateData(regionEntity);
                    RefreshData();
                }
            }
        }
       
        private void lngbCancel_Click(object sender, EventArgs e)
        {
             RefreshData();
             this.lblStatus.Text = string.Empty;
        }

        private void txtRegion_TextChanged(object sender, EventArgs e)
       {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        { 
            regionEntity=regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            if (regionEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtRegion.Text = regionEntity.RegionName;
            lgcRegion.Visible = false;
            pRegion.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
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
            }
            else
            {
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
                lngbPick.Visible = true;
                lngbAdd.Enabled = true;
            }
            regionEntity = null;
        } 
        private void lngbDelete_Click(object sender, EventArgs e)
        {
            regionEntity = regionBLL.GetDetailData(lgcRegion.GetPrimaryValue()) as RegionEntity;
            if (regionEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                regionBLL.DeleteData(regionEntity);
                RefreshData(); 
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
    }
}
