using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CAB.BLL;
using CAB.Framework.Utility;

namespace CAB.UI
{
    public partial class MeterSelectForm : Form
    {
        public delegate void GetValueColumn(string gridValue);
        public event GetValueColumn OnGridValue_Selection;
        public string MeterID { get; set; }

        public MeterSelectForm()
        {
            InitializeComponent();
        }

        private void MeterSelectForm_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(ConfigInfo.AutoNumberedTable(new MeterDataBLL().ComboList("MeterId").Tables[0]));
            if (ds.Tables[0].Rows.Count == 0)
                btnOK.Enabled = false;
            else
                btnOK.Enabled = true;
            lngGridAvailableMeter.Data = ds;
            lngGridAvailableMeter.SetWidth("SNo", 60);
            lngGridAvailableMeter.SetWidth("MeterID", 225);
            lngGridAvailableMeter.IsSorting = false;
            lngGridAvailableMeter.ValueColumn = "MeterID";
            lngGridAvailableMeter.RefreshGrid();
        }

        public void GridValueSelection(string gridValue)
        {
            if (OnGridValue_Selection != null)
            {
                OnGridValue_Selection(gridValue);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MeterID = lngGridAvailableMeter.Data.Tables[0].Rows[lngGridAvailableMeter.SelectedIndex]["MeterID"].ToString();
            if (!string.IsNullOrEmpty(MeterID))
            {
                this.GridValueSelection(MeterID);
                this.Close();
            }
        }

        private void lngGridAvailableMeter_DoubleClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MeterID))
            {
                this.GridValueSelection(MeterID);
                this.Close();
            }
        }

        private void lngGridAvailableMeter_OnGridRowChanged(string msg)
        {
            this.MeterID = msg;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lngGridAvailableMeter_OnGridMouseDoubleClick(string KeyValue)
        {
            MeterID = lngGridAvailableMeter.Data.Tables[0].Rows[lngGridAvailableMeter.SelectedIndex]["MeterID"].ToString();
            if (!string.IsNullOrEmpty(MeterID))
            {
                this.GridValueSelection(MeterID);
                this.Close();
            } 
        }

       

    }
}
