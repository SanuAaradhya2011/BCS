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

namespace CAB.UI.Controls
{
    public partial class UnitMaster : Form
    {
        private UnitBLL unitBLL = new UnitBLL();
        private UnitEntity categoryMasterEntity;

        public UnitMaster()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("M000080"); 
            this.pDivision.Visible = false;
            LoadGrid();
            categoryMasterEntity = unitBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as UnitEntity;  
            if (categoryMasterEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                categoryMasterEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcCategory.HiddenColumn = "MeterUnit_ID";
            lgcCategory.ValueColumn = "MeterUnit_ID"; 
            lgcCategory.Data = unitBLL.ListDataSet();
            lgcCategory.SetWidth("SL. No", 70);
            lgcCategory.SetWidth("Unit Name", 387);
            lgcCategory.Visible = true;
            pDivision.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = string.Empty;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            categoryMasterEntity = new UnitEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string unitName = this.txtCategory.Text.Trim();
            if (unitName.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000078"); 
                txtCategory.Focus();
                return;
            }
            if (categoryMasterEntity.MeterUnit_ID == 0)
            {
                if ((unitBLL.ValidateUnit(unitName) as UnitEntity).MeterUnit_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000079");
                    txtCategory.Focus();
                }
                else
                {
                    categoryMasterEntity.MeterUnit_Type = unitName;
                    unitBLL.InsertData(categoryMasterEntity);
                    RefreshData();
                }
            }
            else
            {
                UnitEntity unit=unitBLL.ValidateUnit(unitName) as UnitEntity;
                if (unit.MeterUnit_ID != categoryMasterEntity.MeterUnit_ID && unit.MeterUnit_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000079");
                    txtCategory.Focus();
                }
                else
                {
                    categoryMasterEntity.MeterUnit_Type = unitName;
                    unitBLL.UpdateData(categoryMasterEntity);
                    RefreshData();
                }
            }
        }
        private void RefreshData()
        {
            LoadGrid();
            if (lgcCategory.GetPrimaryValue().Equals(string.Empty))
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
            categoryMasterEntity = null;
        }
        private void lngbCancel_Click(object sender, EventArgs e)
        {
             RefreshData();
             this.lblStatus.Text = string.Empty;
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
       {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        { 
            categoryMasterEntity=unitBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as UnitEntity;
            if (categoryMasterEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = categoryMasterEntity.MeterUnit_Type;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            categoryMasterEntity = unitBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as UnitEntity;
            if (categoryMasterEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                unitBLL.DeleteData(categoryMasterEntity);
                RefreshData();
            }
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void lgcCategory_OnGridRowChanged(string msg)
        { 
            this.lblStatus.Text = string.Empty;
        }
    }
}
