using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.UI;
using CAB.Framework.Utility;
using System.Runtime.InteropServices;

namespace CAB.UI.Controls
{
    public partial class CABDropDown : UserControl
    {
        [ComVisibleAttribute(false)] 
        public delegate void OnDataChanged(object sender, EventArgs e); 
        public event OnDataChanged DataChanged; 
        public DropDownEnum FormType { get; set; }
        private string comboData;

        private int Index=0;  
        public string Data
        {
            get 
            {
                if (cboData.Items.Count > 0)
                {
                    comboData = ((System.Data.DataRowView)(cboData.Items[cboData.SelectedIndex])).Row.ItemArray[Index].ToString();
                }
                return comboData;
            }
            set
            {
                comboData = value;
                for (int i = 0; i < cboData.Items.Count; i++)
                {
                    cboData.SelectedIndex = i;
                    if (comboData == ((System.Data.DataRowView)(cboData.Items[i])).Row.ItemArray[Index].ToString())
                        break;
                }
            }
        }
        public CABDropDown()
        {
            InitializeComponent();
        }

        private void LoadData()
        { 
            DataSet dataSet=null;
            if(FormType.Equals(DropDownEnum.Category))
            {
                CategoryMasterBLL categoryMasterBLL = new CategoryMasterBLL();
                dataSet=categoryMasterBLL.ListDataSet();
                Index = 1;
                 if(dataSet!=null)
                 {
                     cboData.DataSource = dataSet.Tables[0];
                     cboData.DisplayMember = "Category Name";
                     cboData.ValueMember = "Category_ID";
                 }
            }
            if (FormType.Equals(DropDownEnum.Designation))
            {
                DesignationMasterBLL designationMasterBLL = new DesignationMasterBLL();
                dataSet = designationMasterBLL.ListDataSet();
                Index = 1;
                if (dataSet != null)
                {
                    cboData.DataSource = dataSet.Tables[0];
                    cboData.DisplayMember = "Designation Name";
                    cboData.ValueMember = "Designation_ID";
                }
            }
             if (FormType.Equals(DropDownEnum.ConsumerType))
            {
                ConsumerTypeBLL consumerTypeBLL = new ConsumerTypeBLL();
                dataSet = consumerTypeBLL.ListDataSet();
                Index = 2;
                if (dataSet != null)
                {
                    cboData.DataSource = dataSet.Tables[0];
                    cboData.DisplayMember = "Consumer Type";
                    cboData.ValueMember = "Consumer Type";
                } 
            }
             if (FormType.Equals(DropDownEnum.MeterType))
             {
                 MeterTypeBLL meterTypeBLL = new MeterTypeBLL();
                 dataSet = meterTypeBLL.ListDataSet();
                 Index = 2;
                 if (dataSet != null)
                 {
                     cboData.DataSource = dataSet.Tables[0];
                     cboData.DisplayMember = "Meter Type";
                     cboData.ValueMember = "Meter Type";
                 }
             }
             if (FormType.Equals(DropDownEnum.Unit))
             {
                 UnitBLL unitBLL = new UnitBLL();
                 dataSet = unitBLL.ListDataSet();
                 Index = 2;
                 if (dataSet != null)
                 {
                     cboData.DataSource = dataSet.Tables[0];
                     cboData.DisplayMember = "Unit Name";
                     cboData.ValueMember = "Unit Name";
                 }
             }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (FormType.Equals(DropDownEnum.Category))
            {
                CategoryMaster master = new CategoryMaster();
                master.ShowDialog();
            }
            if (FormType.Equals(DropDownEnum.Designation))
            {
                DesignationMaster master = new DesignationMaster();
                master.ShowDialog();
            }
            if (FormType.Equals(DropDownEnum.ConsumerType))
            {
                ConsumerType master = new ConsumerType();
                master.ShowDialog();
            }
            if (FormType.Equals(DropDownEnum.MeterType))
            {
                MeterType master = new MeterType();
                master.ShowDialog();
            }
            if (FormType.Equals(DropDownEnum.Unit))
            {
                UnitMaster master = new UnitMaster();
                master.ShowDialog();
            }
            LoadData();
        }

        private void CABDropDown_Load(object sender, EventArgs e)
        {
            LoadData();
            if (ConfigInfo.UserInformationID == 0)
            {
                this.btnPlus.Visible = true;
                cboData.Width = this.Width-24;
            }
            else
            {
                this.btnPlus.Visible = false;
                cboData.Width = this.Width;
            }
        }

        private void cboData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(sender, e);
        }
    }
}
