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
    public partial class CategoryMaster : Form
    {
        private CategoryMasterBLL categoryMasterBLL = new CategoryMasterBLL();
        private CategoryMasterEntity categoryMasterEntity;
        
        public CategoryMaster()
        {
            InitializeComponent();
        }

        private void DivisionMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("M000032"); 
            this.pDivision.Visible = false;
            LoadGrid();
            categoryMasterEntity = categoryMasterBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as CategoryMasterEntity;  
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
            lgcCategory.HiddenColumn = "Category_ID";
            lgcCategory.ValueColumn = "Category_ID"; 
            lgcCategory.Data = categoryMasterBLL.ListDataSet();
            lgcCategory.SetWidth("SL. No", 70);
            lgcCategory.SetWidth("Category Name", 387);
            lgcCategory.Visible = true;
            pDivision.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = string.Empty;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            categoryMasterEntity = new CategoryMasterEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string categoryName = this.txtCategory.Text.Trim();
            if (categoryName.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000034"); 
                txtCategory.Focus();
                return;
            }
            if (categoryMasterEntity.Category_ID == 0)
            {
                if ((categoryMasterBLL.ValidateCategory(categoryName) as CategoryMasterEntity).Category_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000033");
                    txtCategory.Focus();
                }
                else
                {
                    categoryMasterEntity.Category_Name = categoryName;
                    categoryMasterBLL.InsertData(categoryMasterEntity);
                    RefreshData();
                }
            }
            else
            {
                CategoryMasterEntity category=categoryMasterBLL.ValidateCategory(categoryName) as CategoryMasterEntity;
                if (category.Category_ID != categoryMasterEntity.Category_ID && category.Category_ID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000033");
                    txtCategory.Focus();
                }
                else
                {
                    categoryMasterEntity.Category_Name = categoryName;
                    categoryMasterBLL.UpdateData(categoryMasterEntity);
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
            categoryMasterEntity=categoryMasterBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as CategoryMasterEntity;
            if (categoryMasterEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCategory.Text = categoryMasterEntity.Category_Name;
            lgcCategory.Visible = false;
            pDivision.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCategory.Focus();
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            categoryMasterEntity = categoryMasterBLL.GetDetailData(lgcCategory.GetPrimaryValue()) as CategoryMasterEntity;
            if (categoryMasterEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                categoryMasterBLL.DeleteData(categoryMasterEntity);
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
