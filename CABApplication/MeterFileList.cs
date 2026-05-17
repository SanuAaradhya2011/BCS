using System;
using CAB.UI.Controls;
using System.Data;
using CAB.BLL;
using CAB.Framework.Utility;
using System.Windows.Forms;
using CAB.Framework;
using CABFramework;
using System.Text;
using System.Globalization;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class MeterFileList : MdiChildForm
    {
        public static string filesource = null;
       private string listData = null;
        private CABForm meterDataList = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(MeterFileList).ToString());
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
        DataSet dataSetDatewise = new DataSet();
        private DataSet ParseData(DataSet ds, string types)
        {
            if (ds == null)
                return null;

            DataSet dataSet = new MeterDataBLL().ComboList("ReadingDateTime");
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Serial Number", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("MeterData_ID", typeof(System.String)));
            if (types.Equals("CABF"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("Upload DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Source", typeof(System.String)));
            }
            if (types.Equals("MSN"))
            {
                table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("Upload DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Source", typeof(System.String)));
            }
            if (types.Equals("RD"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("Upload DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Source", typeof(System.String)));
            }
            if (types.Equals("CMRI"))
            {
                table.Columns.Add(new DataColumn("Meter Number", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Name", typeof(System.String)));
                table.Columns.Add(new DataColumn("Meter Reading DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("Upload DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("FileUpload_ID", typeof(System.String)));
                table.Columns.Add(new DataColumn("File Source", typeof(System.String)));
            }

            table.Columns.Add(new DataColumn("File Size", typeof(System.String)));

            DataRow row;
            foreach (DataRow drow in ds.Tables[0].Rows)
            {
                row = table.NewRow();
                row["Serial Number"] = Convert.ToInt32(drow[0]);
                row["MeterData_ID"] = Convert.ToString(drow[1]);
                if (types.Equals("CABF"))
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    row["Meter Reading DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["FileUpload_ID"] = Convert.ToString(drow["FileUpload_ID"]);
                    row["File Source"] = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    filesource = Enum.GetName(typeof(CommTypes), drow["CommType"]);

                    row["Upload DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow["Uploading DateTime"]));
                }
                if (types.Equals("MSN"))
                {
                    row["File Name"] = Convert.ToString(drow[2]);
                    row["Meter Reading DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[3]));
                    row["FileUpload_ID"] = Convert.ToString(drow["FileUpload_ID"]);
                    if (!Convert.IsDBNull(drow["CommType"]))
                    {
                        row["File Source"] = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                        filesource = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    }
                    else
                    {
                        row["File Source"] = CommTypes.PreviousVersionUpload.GetDisplayName();
                        filesource = CommTypes.PreviousVersionUpload.GetDisplayName();
                    }
                    row["Upload DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow["Uploading DateTime"]));
                }
                if (types.Equals("RD"))
                {
                    row["Meter Number"] = Convert.ToString(drow[2]);
                    row["File Name"] = Convert.ToString(drow[3]);
                    row["Meter Reading DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow[4]));
                    row["FileUpload_ID"] = Convert.ToString(drow["FileUpload_ID"]);
                    row["File Source"] = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    filesource = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    row["Upload DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow["Uploading DateTime"]));
                }
                if (types.Equals("CMRI"))
                {
                    row["Meter Number"] = Convert.ToString(drow["Meter Number"]);
                    row["File Name"] = Convert.ToString(drow["FileName"]);
                    row["Meter Reading DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow["Reading DateTime"]));
                    row["FileUpload_ID"] = Convert.ToString(drow["FileUpload_ID"]);
                    row["File Source"] = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    filesource = Enum.GetName(typeof(CommTypes), drow["CommType"]);
                    row["Upload DateTime"] = DateUtility.LongToStringDateTimeWithSecFormat(Convert.ToInt64(drow["Uploading DateTime"]));
                }
                row["File Size"] = Convert.ToString(drow["File Size"]);
                table.Rows.Add(row);
            }
            DataSet dst = new DataSet();
            dst.Tables.Add(table);
            return dst;
        }

        /// <summary>
        /// set billing tariff count from meter type
        /// </summary>
        /// <returns></returns>
        public void SetBillingTariffCount()
        {
            switch (ConfigInfo.ActiveMeterType)
            {
                case "1P-2W":
                    ConfigInfo.BillingTariffCount = 8; // Previous count '6' change to '8' as per new requirement of Torrent Power
                    break;
                case "3P-4W":
                    ConfigInfo.BillingTariffCount = 8;
                    break;
                case "3P-3W":
                    ConfigInfo.BillingTariffCount = 8;
                    break;
                default:
                    ConfigInfo.BillingTariffCount = 8;
                    break;
            }

        }

        private void MeterFileList_Load(object sender, EventArgs e)
        {

            DataSet dataSet = new DataSet();
            this.Text = "Search List";
            if (comboData.Equals("CMRI"))
            {
                lblText.Text = "CMRI ID :" + listData;
                dataSet = ParseData(new CMRIMasterBLL().ListDataSet(listData), comboData);
            }
            else
            {
                if (comboData.Equals("CABF"))
                    lblText.Text = "File Name :" + listData;
                if (comboData.Equals("MSN"))
                    lblText.Text = "Meter Number :" + listData;
                if (comboData.Equals("RD"))
                    lblText.Text = "Meter Reading Date :" + listData;
                dataSet = ParseData(new MeterDataBLL().ListDataSet(comboData, listData, false), comboData);
                dataSetDatewise = dataSet.Copy(); // hold dta to filter dataset
            }
            if (dataSet == null)
                return;
            if (dataSet.Tables.Count == 0)
                return;
            if (dataSet.Tables[0].Rows.Count == 0)
                return;
            this.lngFileLists.Data = dataSet;
            this.lngFileLists.HiddenColumn = "MeterData_ID,FileUpload_ID";
            this.lngFileLists.ValueColumn = "MeterData_ID";
            this.lngFileLists.SetWidth("Serial Number", 115);
            if (dataSet.Tables[0].Columns.IndexOf("File Name") > 0)
                this.lngFileLists.SetWidth("File Name", 230);
            if (dataSet.Tables[0].Columns.IndexOf("Meter Number") > 0)
                this.lngFileLists.SetWidth("Meter Number", 125);
            this.lngFileLists.SetWidth("Meter Reading DateTime", 160);
            this.lngFileLists.SetWidth("Upload DateTime", 160);
            this.lngFileLists.SetWidth("File Source", 160);

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
                    if (formName == "IEC650MeterDataList" || formName == "DLMS650MeterDataList")
                    {
                        // SB code change Start - 20180629 - Multiple Analysis View
                        //this.MdiParent.MdiChildren[i].Visible = false;
                        //this.MdiParent.MdiChildren[i].Close();
                        // SB code change End - 20180629 - Multiple Analysis View
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
            ApplicationType types = ConfigInfo.GetApplicationType();
            //if (types.Equals(ApplicationType.IEC_LTCT_650))
            //{
            //    if (ActivateThisChild("IEC650MeterDataList") == false)
            //    {
            //        if (KeyValue.Trim() != "")
            //        {
            //            this.Cursor = Cursors.WaitCursor;
            //            meterDataList = new IEC650MeterDataList();
            //            meterDataList.MdiParent = this.MdiParent;
            //            meterDataList.On_StatusChanged += new IsStatusChanged(OnStatus_Changed);
            //            meterDataList.On_RightStatusChanged += new IsRightStatusChanged(OnRightStatus_Changed);
            //            meterDataList.MeterDataID = KeyValue;
            //            meterDataList.Show();
            //            this.Cursor = Cursors.Default;
            //        }
            //    }
            //}


            if (types.Equals(ApplicationType.DLMS_LTCT_650))
            {
                if (ActivateThisChild("DLMS650MeterDataList") == false)
                {
                    if (KeyValue.Trim() != "")
                    {
                        this.Cursor = Cursors.WaitCursor;
                        // SB code change Start - 20180629 - Multiple Analysis View
                        DLMS650MeterDataList meterDataListMDIChield = null;
                        bool formOpen = false;

                        // Select the opened for if already exists.
                        if (Application.OpenForms["DLMS650MeterDataList"] != null)
                        {
                            Form[] forms = (Form[])this.MdiParent.MdiChildren;
                            foreach (Form form in forms)
                            {
                                if (form.GetType() == typeof(DLMS650MeterDataList))
                                {
                                    meterDataListMDIChield = (DLMS650MeterDataList)form;
                                    if (meterDataListMDIChield.Tag.ToString().Equals(KeyValue))
                                    {
                                        meterDataListMDIChield.Activate();
                                        formOpen = true;
                                        break;
                                    }
                                }
                            }
                        }
                        
                        if (!formOpen)
                        {
                            DLMS650MeterDataList meterDataList = new DLMS650MeterDataList();
                            meterDataList.MdiParent = this.MdiParent;
                            meterDataList.On_StatusChanged += new IsStatusChanged(OnStatus_Changed);
                            meterDataList.On_RightStatusChanged += new IsRightStatusChanged(OnRightStatus_Changed);
                            meterDataList.MeterDataID = KeyValue;
                            meterDataList.Show();
                        }

                        this.Cursor = Cursors.Default;
                        // SB code change End - 20180629 - Multiple Analysis View
                    }
                }
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
            MeterDataBLL meterDataBLL = new MeterDataBLL();
            ConfigInfo.ActiveFileType = meterDataBLL.GetFileType(Convert.ToInt64(lngFileLists.GetFileUploadId()));
            ConfigInfo.ActiveFirmwareVersion = new DLMS650GeneralBLL().GetFirmwareVersionByMeterDataID(ConfigInfo.ActiveMeterDataId);
            ConfigInfo.ActiveMeterType = new DLMS650GeneralBLL().GetActiveMeterTypeByMeterDataID(ConfigInfo.ActiveMeterDataId);
            SetBillingTariffCount();
        }

        private void lngFileLists_Load(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
        }

        private void MeterFileList_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ConfigInfo.ActiveMeterDataId = string.Empty;
            ConfigInfo.ActiveFirmwareVersion = string.Empty;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
           this.FilterDataset();         
        }

        /// <summary>
        /// Filter Grid data based on selection in datetimepicker
        ///  // userstory 505186
        /// </summary>
        private void FilterDataset()
        {
            try
            {
                if (dataSetDatewise != null)
                {
                    if (dataSetDatewise.Tables.Count > 0)
                    {
                        DataSet FilteredSet = new DataSet();
                        if (dataSetDatewise.Tables[0].Rows.Count > 0)
                        {                                             
                            string format = "dd/MM/yyyy HH:mm:ss";
                            long frmDate = DateUtility.DateTimeToLong(Convert.ToDateTime(DtpickerFrom.Value.ToShortDateString() + " 00:00:00"));
                            long toDate = DateUtility.DateTimeToLong(Convert.ToDateTime(DtpickerTo.Value.ToShortDateString() + " 23:59:59"));

                               
                         long diffDays = toDate - frmDate;
                         if (diffDays >= 0)
                         {
                             this.StatusMessage = "";
                             CultureInfo provider = CultureInfo.InvariantCulture;

                             StringBuilder strfromdateValue = new StringBuilder(DtpickerFrom.Value.ToShortDateTimeCABFormat());
                             string strfromValue = strfromdateValue.Append(" 00:00:00").ToString();

                             StringBuilder strTodateValue = new StringBuilder(DtpickerTo.Value.ToShortDateTimeCABFormat());
                             string strToValue = strTodateValue.Append(" 23:59:59").ToString();

                             DataTable dt = dataSetDatewise.Tables[0].Clone();
                             FilteredSet.Tables.Add(dt);         
                            foreach (DataRow item in dataSetDatewise.Tables[0].Rows)
                            {
                                string Value = item["Meter Reading DateTime"].ToString();
                              
                                DateTime selecteddt = DateTime.ParseExact(Value,format, provider);
                                DateTime dtfromValue = DateTime.ParseExact(strfromValue, format, provider);
                                DateTime dtToValue = DateTime.ParseExact(strToValue, format, provider);
                                if (DateTime.Compare(selecteddt, dtfromValue) >= 0 && DateTime.Compare(selecteddt, dtToValue) <= 0)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dt.Rows.Add(item.ItemArray);
                                    }
                                }
                                this.lngFileLists.Data = FilteredSet;
                                this.lngFileLists.RefreshGrid();
                        }  
                        else
                        {
                            this.StatusMessage = "From date should not be greater than To date.";
                            DtpickerFrom.Focus();
                            Application.DoEvents();
                            return;
                        }                              
                       }                           
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FilterDataset()", ex); 
            }
        }

        private void lblText_Click(object sender, EventArgs e)
        {

        }

        private void lblFromdate_Click(object sender, EventArgs e)
        {

        }
    }
}
