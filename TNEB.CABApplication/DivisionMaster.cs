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
    public partial class DivisionMaster : Form
    {
        private DivisionBLL divisionBLL = new DivisionBLL();
        private DivisionEntity divisionEntity;
        public DivisionEntity Division
        {
            get;
            set;
        }

        public DivisionMaster()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("M000026"); 
            this.pDivision.Visible = false;
            LoadGrid();
            divisionEntity = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;  
            if (divisionEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                divisionEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcDivision.HiddenColumn = "Division_ID";
            lgcDivision.ValueColumn = "Division_ID"; 
            lgcDivision.Data = divisionBLL.ListDataSet();
            lgcDivision.SetWidth("SL. No", 70);
            lgcDivision.SetWidth("Division Name", 387);
            lgcDivision.IsSorting = false;
            lgcDivision.Visible = true;
            pDivision.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtDivision.Text = string.Empty;
            lgcDivision.Visible = false;
            pDivision.Visible = true;
            divisionEntity = new DivisionEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtDivision.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtDivision.Text.Trim();
            if (regionName.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000027"); 
                txtDivision.Focus();
                return;
            }
            if (divisionEntity.DivisionID == 0)
            {
                if ((divisionBLL.ValidateDivision(regionName) as DivisionEntity).DivisionID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000028");
                    txtDivision.Focus();
                }
                else
                {
                    divisionEntity.DivisionName = regionName;
                    divisionBLL.InsertData(divisionEntity);
                    RefreshData();
                }
            }
            else
            {
                DivisionEntity rentity=divisionBLL.ValidateDivision(regionName) as DivisionEntity;
                if (rentity.DivisionID != divisionEntity.DivisionID && rentity.DivisionID !=0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000028");
                    txtDivision.Focus();
                }
                else
                {
                    divisionEntity.DivisionName = regionName;
                    divisionBLL.UpdateData(divisionEntity);
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
            }
            else
            {
                lngbEdit.Enabled = true;
                lngbDelete.Enabled = true;
                lngbPick.Visible = true;
                lngbAdd.Enabled = true;
            }
            divisionEntity = null;
        }
        private void lngbCancel_Click(object sender, EventArgs e)
        {
             RefreshData();
             this.lblStatus.Text = string.Empty;
        }

        private void txtDivision_TextChanged(object sender, EventArgs e)
       {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        { 
            divisionEntity=divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            if (divisionEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtDivision.Text = divisionEntity.DivisionName;
            lgcDivision.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtDivision.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            divisionEntity = divisionBLL.GetDetailData(lgcDivision.GetPrimaryValue()) as DivisionEntity;
            if (divisionEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                divisionBLL.DeleteData(divisionEntity);
                RefreshData();
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
    }
}
