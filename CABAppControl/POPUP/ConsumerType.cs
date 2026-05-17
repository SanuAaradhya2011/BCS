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
    public partial class ConsumerType : Form
    {
        private ConsumerTypeBLL consumerTypeBLL = new ConsumerTypeBLL();
        private ConsumerTypeMasterEntity consumerTypeMasterEntity;

        public ConsumerType()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("L000052"); 
            this.pDivision.Visible = false;
            LoadGrid();
            consumerTypeMasterEntity = consumerTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as ConsumerTypeMasterEntity;  
            if (consumerTypeMasterEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                consumerTypeMasterEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcCategory.HiddenColumn = "ConsumerType_ID";
            lgcCategory.ValueColumn = "ConsumerType_ID"; 
            lgcCategory.Data = consumerTypeBLL.ListDataSet();
            lgcCategory.SetWidth("SL. No", 70);
            lgcCategory.SetWidth("Consumer Type", 387);
            lgcCategory.Visible = true;
            pDivision.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = string.Empty;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            consumerTypeMasterEntity = new ConsumerTypeMasterEntity();
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
                lblStatus.Text = MessageConstant.GetText("M000074"); 
                txtCategory.Focus();
                return;
            }
            if (consumerTypeMasterEntity.ConsumerType_ID == 0)
            {
                if ((consumerTypeBLL.ValidateType(typeName) as ConsumerTypeMasterEntity).ConsumerType_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000075");
                    txtCategory.Focus();
                }
                else
                {
                    consumerTypeMasterEntity.ConsumerType_Name = typeName;
                    consumerTypeBLL.InsertData(consumerTypeMasterEntity);
                    RefreshData();
                }
            }
            else
            {
                ConsumerTypeMasterEntity type = consumerTypeBLL.ValidateType(typeName) as ConsumerTypeMasterEntity;
                if (type.ConsumerType_ID != consumerTypeMasterEntity.ConsumerType_ID && type.ConsumerType_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000075");
                    txtCategory.Focus();
                }
                else
                {
                    consumerTypeMasterEntity.ConsumerType_Name = typeName;
                    consumerTypeBLL.UpdateData(consumerTypeMasterEntity);
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
            consumerTypeMasterEntity = null;
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
            consumerTypeMasterEntity=consumerTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as ConsumerTypeMasterEntity;
            if (consumerTypeMasterEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = consumerTypeMasterEntity.ConsumerType_Name;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            consumerTypeMasterEntity = consumerTypeBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as ConsumerTypeMasterEntity;
            if (consumerTypeMasterEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                consumerTypeBLL.DeleteData(consumerTypeMasterEntity);
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
