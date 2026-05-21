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
    public partial class CircleMaster : Form
    {
       private CircleBLL circleBLL = new CircleBLL();
       private CircleEntity circleEntity;
        public CircleEntity Circle
        {
            get;
            set;
        }
        public CircleMaster()
        {
            InitializeComponent();
        }

        private void CircleMaster_Load(object sender, EventArgs e)
        {
            this.Text = MessageConstant.GetText("M000029"); 
            this.pCircle.Visible = false;
            LoadGrid();
            circleEntity = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;  
            if (circleEntity == null)
            {
                lngbEdit.Enabled = false;
                lngbDelete.Enabled = false;
                lngbPick.Visible = true;
                circleEntity = null;
            }
        }
        private void LoadGrid()
        {
            lgcCircle.HiddenColumn = "Circle_ID";
            lgcCircle.ValueColumn = "Circle_ID"; 
            lgcCircle.Data = circleBLL.ListDataSet();
            lgcCircle.SetWidth("SL. No", 70);
            lgcCircle.SetWidth("Circle Name", 387);
            lgcCircle.IsSorting = false;
            lgcCircle.Visible = true;
            pCircle.Visible = false;
        }
        private void lngbAdd_Click(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
            this.txtCircle.Text = string.Empty;
            lgcCircle.Visible = false;
            pCircle.Visible = true;
            circleEntity = new CircleEntity();
            lngbEdit.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCircle.Focus();
        }

        private void lngbSave_Click(object sender, EventArgs e)
        {
            string regionName = this.txtCircle.Text.Trim();
            if (regionName.Equals(string.Empty))
            {
                lblStatus.Text = MessageConstant.GetText("M000030");
                txtCircle.Focus();
                return;
            }
            if (circleEntity.CircleID == 0)
            {
                if ((circleBLL.ValidateCircle(regionName) as CircleEntity).CircleID != 0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000031");
                    txtCircle.Focus();
                }
                else
                {
                    circleEntity.CircleName = regionName;
                    circleBLL.InsertData(circleEntity);
                    RefreshData();
                }
            }
            else
            {
                CircleEntity rentity=circleBLL.ValidateCircle(regionName) as CircleEntity;
                if (rentity.CircleID != circleEntity.CircleID && rentity.CircleID !=0)
                {
                    lblStatus.Text = MessageConstant.GetText("M000031");
                    txtCircle.Focus();
                }
                else
                {
                    circleEntity.CircleName = regionName;
                    circleBLL.UpdateData(circleEntity);
                    RefreshData();
                }
            }
        }
       
        private void lngbCancel_Click(object sender, EventArgs e)
        {
             RefreshData();
             this.lblStatus.Text = string.Empty;
        }

        private void txtCircle_TextChanged(object sender, EventArgs e)
       {
            lblStatus.Text = string.Empty;
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        { 
            circleEntity=circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            if (circleEntity == null)
                return;
            lblStatus.Text = string.Empty;
            this.txtCircle.Text = circleEntity.CircleName;
            lgcCircle.Visible = false;
            pCircle.Visible = true;
            lngbAdd.Enabled = false;
            lngbDelete.Enabled = false;
            lngbPick.Visible = false;
            txtCircle.Focus(); 
        }
        private void RefreshData()
        {
            LoadGrid();
            if (lgcCircle.GetPrimaryValue().Equals(string.Empty))
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
            circleEntity = null;
        } 
        private void lngbDelete_Click(object sender, EventArgs e)
        {
            circleEntity = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            if (circleEntity == null)
                return;
            if (CABMessageBox.ShowFilterMessage("M000025", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                circleBLL.DeleteData(circleEntity);
                RefreshData();
            } 
        }

        private void lngbPick_Click(object sender, EventArgs e)
        {
            Circle = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
           this.Close();
        }

        private void lgcCircle_OnGridRowChanged(string msg)
        { 
            this.lblStatus.Text = string.Empty;
        }

        private void lgcCircle_OnGridMouseDoubleClick(string KeyValue)
        {
            Circle = circleBLL.GetDetailData(lgcCircle.GetPrimaryValue()) as CircleEntity;
            this.Close();
        }
    }
}
