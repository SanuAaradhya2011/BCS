using System;
using CAB.UI.Controls;
using System.Data;
using CAB.BLL;
using CAB.IECFramework.Utility;
using System.Windows.Forms;

namespace CAB.UI
{
    public partial class MeterFileList : MdiChildForm
    {
        private string listData = null;
        private MeterDataList meterDataList = null;
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
        public MeterFileList()
        {
            InitializeComponent();
        }
        private DataSet ParseData(DataSet ds, string types)
        {
            if (ds == null)
                return null;

            DataSet dataSet = new MeterDataBLL().ComboList(false);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Serial Number", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("MeterData_ID", typeof(System.String)));
            table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
            if (types.Equals("CABF"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading Date Time", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Uploading Date Time", typeof(System.String)));
            }
            if (types.Equals("MSN"))
            {
                table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading Date Time", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Uploading Date Time", typeof(System.String)));
            }
            if (types.Equals("RD"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading Date Time", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Uploading Date Time", typeof(System.String)));
            }
            if (types.Equals("CMRIID")) 
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                //table.Columns.Add(new DataColumn("Meter Location", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading Date Time", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Uploading Date Time", typeof(System.String)));
            }
            //MeterLocation removed 20 march 2012
            if(types.Equals("DR")||types.Equals("DRMID")||types.Equals("WN")||types.Equals("MN"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                //table.Columns.Add(new DataColumn("Meter Location", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading Date Time", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Uploading Date Time", typeof(System.String)));
            }

            DataRow row;
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                row["Serial Number"] =Convert.ToInt32( drow[0]);
                row["MeterData_ID"] = Convert.ToString(drow[1]);
                row["FileUpload_ID"] = Convert.ToString(drow[5]);
                if (types.Equals("CABF"))
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    row["Meter Reading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["File Uploading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                }
                if (types.Equals("MSN"))
                {
                    row["File Name"] = Convert.ToString(drow[2]);
                    row["Meter Reading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["File Uploading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                }
                if (types.Equals("RD"))
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    row["File Name"] = Convert.ToString(drow[3]);
                    row["Meter Reading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                    row["File Uploading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[5]));
                }
                if (types.Equals("CMRIID")) 
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    //row["Meter Location"] = Convert.ToString(drow[3]);
                    row["Meter Reading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["File Uploading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                }
                //MeterLocation removed and indices for the next two rows decremented by 1: 20 march 2012
                if (types.Equals("DR") || types.Equals("DRMID") || types.Equals("WN") || types.Equals("MN"))
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    //row["Meter Location"] = Convert.ToString(drow[3]);
                    row["Meter Reading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["File Uploading Date Time"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                }

              
                table.Rows.Add(row);
            }
            DataSet dst = new DataSet();
            dst.Tables.Add(table);
            return dst;
        }
        private void MeterFileList_Load(object sender, EventArgs e)
        {
            //string cmriType;
            this.Text = "Search List";
            if (comboData.Equals("CABF"))
                lblText.Text = "File Name :" + listData; 
            if (comboData.Equals("MSN")) 
                lblText.Text = "Meter ID :" + listData; 
            if (comboData.Equals("RD")) 
                lblText.Text = "Meter Reading Date :" + listData;
            if (comboData.Equals("CMRIID"))
            {
                lblText.Text = "CMRI Number :" + listData;
                //lblText.Text = "CMRI Number :" + listData.Replace("(", "  ").Replace(")", "");
                //cmriType = listData.Substring(listData.LastIndexOf("(") + 1, listData.LastIndexOf(")") - listData.LastIndexOf("(")-1); 
                //listData = listData.Substring(0, listData.LastIndexOf("("));
               
                
            }
            if (comboData.Equals("DR") || comboData.Equals("DRMID") || comboData.Equals("WN") || comboData.Equals("MN"))
                lblText.Text = "Meter Reading Date :" + listData;
         

            DataSet dataSet = ParseData(new MeterDataBLL().ListDataSet(comboData, listData), comboData);
            if (dataSet == null)
                return;
            if (dataSet.Tables.Count == 0)
                return;
            if (dataSet.Tables[0].Rows.Count == 0)
                return;
            this.lngFileLists.Data = dataSet;
            this.lngFileLists.HiddenColumn = "MeterData_ID";
            this.lngFileLists.ValueColumn = "MeterData_ID";

            this.lngFileLists.HiddenColumn2 = "FileUpload_ID";
            this.lngFileLists.ValueColumn2 = "FileUpload_ID";
            this.lngFileLists.HiddenColumns = "MeterData_ID,FileUpload_ID";
            this.lngFileLists.SetWidth("Serial Number", 80);
            if (dataSet.Tables[0].Columns.IndexOf("File Name") > 0)
                this.lngFileLists.SetWidth("File Name", 230);
            if (dataSet.Tables[0].Columns.IndexOf("Meter Number") > 0)
                this.lngFileLists.SetWidth("Meter Number", 125);
            if (dataSet.Tables[0].Columns.IndexOf("Meter Reading Date Time") > 0)
                this.lngFileLists.SetWidth("Meter Reading Date Time", 170);
            if (dataSet.Tables[0].Columns.IndexOf("File Uploading Date Time") > 0)
                this.lngFileLists.SetWidth("File Uploading Date Time",170);
            lngFileLists.HSrollBar();
            this.lngFileLists.RefreshGrid();
        }


        private Boolean ActivateThisChild(String formName)
        {
            int i;
            Boolean formSetToMdi = false;
            for (i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == formName)
                {
                    if (formName == "MeterDataList")
                    { 
                        this.MdiParent.MdiChildren[i].Visible = false;
                        this.MdiParent.MdiChildren[i].Close();
                        formSetToMdi = false;
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
            try
            {
                MeterDataBLL meterDataBLL;
                if (ActivateThisChild("MeterDataList") == false)
                {
                    if (KeyValue.Trim() != "")
                    {
                        this.Cursor = Cursors.WaitCursor;
                        meterDataList = new MeterDataList();
                        meterDataList.MdiParent = this.MdiParent;
                        meterDataList.On_StatusChanged += new IsStatusChanged(OnStatus_Changed);
                        meterDataList.On_RightStatusChanged += new IsRightStatusChanged(OnRightStatus_Changed);
                        meterDataList.MeterDataID = KeyValue;                                           
                        meterDataList.fileUploadID = Convert.ToInt64(lngFileLists.GetHiddenColumnValue());                        
                        //Following if condition addded on 29 feb 2012 as per the bug report                        
                        meterDataBLL = new MeterDataBLL();
                        meterDataList.FileType = meterDataBLL.GetFileType(meterDataList.fileUploadID);  
                        int recordNo = meterDataBLL.FetchRecords(Convert.ToInt64(meterDataList.MeterDataID));
                        if (recordNo <= 0)
                        {
                            this.StatusMessage = "File not available";
                            this.RightStatusMessage = "";
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        meterDataList.Show();
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch(Exception ex)
            {
               // MessageBox.Show(ex.StackTrace);
            }
        }
        private void OnStatus_Changed(string msg)
        {
            this.StatusMessage = msg;
        }
        private void OnRightStatus_Changed(string msg)
        {
            this.RightStatusMessage = msg;
        }
        private void lngFileLists_OnGridRowChanged(string msg)
        {
            ConfigInfo.ActiveMeterDataId = msg;
            
        }
        private void lngFileLists_OnGridRowChanged1(string msg)
        {
            ConfigInfo.FileUpload_ID = msg;

        }
      
      
        private void lngFileLists_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void MeterFileList_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ConfigInfo.ActiveMeterDataId = string.Empty;
            //addded on 29 feb 2012 as per the bug report
            this.StatusMessage = string.Empty;
            this.RightStatusMessage = string.Empty;
            this.Cursor = Cursors.Default;

        }

        private void lblText_Click(object sender, EventArgs e)
        {

        }
    }
}
