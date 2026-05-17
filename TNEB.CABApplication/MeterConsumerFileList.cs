using System;
using CAB.UI.Controls;
using System.Data;
using CAB.BLL;
using CAB.IECFramework.Utility;
using System.Windows.Forms;

namespace CAB.UI
{
    public partial class MeterConsumerFileList : MdiChildForm
    { 
        private string listData = null;
        public string ListData
        {
            get { return listData; }
            set { listData = value; }
        }
        private string comboData = null;
        public string ComboData
        {
            get { return comboData; }
            set { comboData = value; }
        }
        public MeterConsumerFileList()
        {
            InitializeComponent();
        }
        private DataSet ParseData(DataSet ds, string types)
        {
            if (ds == null)
                return null;

            DataSet dataSet = new MeterDataBLL().ComboList(false);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Serial Number", typeof(System.String)));
            table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
            if (types.Equals("CN"))
            {
                table.Columns.Add(new DataColumn("Meter Location", typeof(System.String))); 
            }
            if (types.Equals("L"))
            {
                table.Columns.Add(new DataColumn("Consumer Number", typeof(System.String)));
            }
            table.Columns.Add(new DataColumn("Installation DateTime", typeof(System.String)));
            DataRow row;
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                row["Serial Number"] = Convert.ToString(drow[0]);
                row["Meter Number"] = Convert.ToString(drow[1]);
                if (types.Equals("CN"))
                {
                    row["Meter Location"] = Convert.ToString(drow[2]);
                }
                if (types.Equals("L"))
                {
                    row["Consumer Number"] = Convert.ToString(drow[2]); 
                }
                row["Installation DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                table.Rows.Add(row);
            }
            DataSet dst = new DataSet();
            dst.Tables.Add(table);
            return dst;
        }
        private void MeterFileList_Load(object sender, EventArgs e)
        {
            if (comboData.Equals("CN"))
            {
                lblText.Text = "Consumer Number :" + listData;
            }
            if (comboData.Equals("L"))
            {
                lblText.Text = "Location :" + listData;
            }
            this.Text = "Search List";
            DataSet dataSet = ParseData(new ConsumerMeterBLL().ListDataSet(comboData, listData), comboData);
            this.lngFileLists.Data = dataSet;
            this.lngFileLists.ValueColumn = "Meter Number";
            this.lngFileLists.RefreshGrid();
            this.lngFileLists.SetWidth("Serial Number", 125);
            this.lngFileLists.SetWidth("Meter Number", 200);
            if (dataSet.Tables[0].Columns.IndexOf("Meter Location") > 0)
                this.lngFileLists.SetWidth("Meter Location", 150);
            if (dataSet.Tables[0].Columns.IndexOf("Consumer Number") > 0)
                this.lngFileLists.SetWidth("Consumer Number", 150);
            this.lngFileLists.SetWidth("Installation DateTime", 200);
        }

        private Boolean ActivateThisChild(String formName)
        {
            int i;
            Boolean formSetToMdi = false;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    if (formName == "MeterFileList")
                    {
                        this.MdiParent.MdiChildren[i].Activate();
                        this.MdiParent.MdiChildren[i].Visible = true;
                        formSetToMdi = true;
                    }
                }
            }
            if (i == 0 || formSetToMdi == false)
                return false;
            else
                return true;
        }

        private void lngFileLists_OnGridMouseDoubleClick(string KeyValue)
        {
            if (ActivateThisChild("MeterFileList") == false)
            {
                if (KeyValue.Trim() != "")
                {
					this.Cursor = Cursors.WaitCursor;
                    MeterFileList meterFileList = new MeterFileList();
                    meterFileList.MdiParent = this.MdiParent;
                    meterFileList.ListData = KeyValue;
                    meterFileList.ComboData = "MSN";
                    meterFileList.Show();
					this.Cursor = Cursors.Default;
                }
            }
        }
    }
}
