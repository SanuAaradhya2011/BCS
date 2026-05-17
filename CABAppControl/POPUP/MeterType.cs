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
using CAB.Framework.Utility;
using System.Threading;

namespace CAB.UI.Controls
{
    public partial class MeterType : Form
    {
        private MeterTypeBLL meterTypeBLL = new MeterTypeBLL();
        private MeterTypeMasterEntity meterTypeMasterEntity;

        public MeterType()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("L000053"); 
            this.pDivision.Visible = false;
            LoadGrid();
            meterTypeMasterEntity = meterTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as MeterTypeMasterEntity;  
            if (meterTypeMasterEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                meterTypeMasterEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcCategory.HiddenColumn = "MeterType_ID";
            lgcCategory.ValueColumn = "MeterType_ID"; 
            lgcCategory.Data = meterTypeBLL.ListDataSet();
            lgcCategory.SetWidth("SL. No", 70);
            lgcCategory.SetWidth("Meter Type", 387);
            lgcCategory.Visible = true;
            pDivision.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = string.Empty;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            meterTypeMasterEntity = new MeterTypeMasterEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string typeName = this.txtCategory.Text.Trim();
            if (typeName.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000076"); 
                txtCategory.Focus();
                return;
            }
            if (meterTypeMasterEntity.MeterType_ID == 0)
            {
                if ((meterTypeBLL.ValidateType(typeName) as MeterTypeMasterEntity).MeterType_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000077");
                    txtCategory.Focus();
                }
                else
                {
                    meterTypeMasterEntity.MeterType_Name = typeName;
                    meterTypeBLL.InsertData(meterTypeMasterEntity);
                    RefreshData();
                }
            }
            else
            {
                MeterTypeMasterEntity metertype = meterTypeBLL.ValidateType(typeName) as MeterTypeMasterEntity;
                if (metertype.MeterType_ID != meterTypeMasterEntity.MeterType_ID && metertype.MeterType_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000077");
                    txtCategory.Focus();
                }
                else
                {
                    meterTypeMasterEntity.MeterType_Name = typeName;
                    meterTypeBLL.UpdateData(meterTypeMasterEntity);
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
            meterTypeMasterEntity = null;
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
            meterTypeMasterEntity = meterTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as MeterTypeMasterEntity;
            if (meterTypeMasterEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = meterTypeMasterEntity.MeterType_Name;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            meterTypeMasterEntity = meterTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as MeterTypeMasterEntity;
            if (meterTypeMasterEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                meterTypeBLL.DeleteData(meterTypeMasterEntity);
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
