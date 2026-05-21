using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;
using CAB.UI.Controls;
using CAB.Entity;
using CAB.Framework.Utility;

namespace CAB.UI
{
    public partial class LogDetails : MdiChildForm
    {
        ApplicationLogBLL applicationLogBLL = null;

        public LogDetails()
        {
            applicationLogBLL = new ApplicationLogBLL();
            InitializeComponent();
        }

        private void ApplicationLogDetails_Load(object sender, EventArgs e)
        {
            lngscApplicationLogDetails.PrimarySearchTypeData = GetSearchData();
            lngscApplicationLogDetails.ShowSearch();
            lngscApplicationLogDetails.RefreshList();
            lngscApplicationLogDetails.ShowSearchText();
            lngscApplicationLogDetails_OnFindNowClick(this, null);
        }

        public override DataSet GetSearchData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            AddNewRow(dataTable, "All", "-");
            AddNewRow(dataTable, "Date Specific", "DATE");
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        private void lngscApplicationLogDetails_OnFindNowClick(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            DataSet dataSet = new DataSet();
            string searchType = lngscApplicationLogDetails.PrimarySearchTypeComboData;
            long fromDate = lngscApplicationLogDetails.SecondarySearchTypeFromDateData;
            long toDate = lngscApplicationLogDetails.SecondarySearchTypeToDateData; 
            if (searchType.Equals("All"))
            {
                dataSet = applicationLogBLL.GetAllLogActivity();
            }
            else if (searchType.Equals("Date Specific"))
            {
                if (fromDate > toDate)
                {
                    this.StatusMessage = "To date can't be less than From Date";
                    Application.DoEvents();
                    return;
                }
                else
                    dataSet = applicationLogBLL.GetDateWiseLogActivity(DateUtility.ConvertSearchDateTimeToLong(fromDate, "000000"), DateUtility.ConvertSearchDateTimeToLong(toDate, "235959"));
            }
            grdActivityLogDetails.Data = dataSet;
            if (dataSet != null)
            {
                grdActivityLogDetails.SetWidth("SL. Number", 90);
                grdActivityLogDetails.SetWidth("Login ID", 140);
                grdActivityLogDetails.SetWidth("Login DateTime", 140);
                grdActivityLogDetails.SetWidth("Logout DateTime", 140);
                grdActivityLogDetails.SetWidth("Duration (DD:HH:MM:SS)", 140); 
            }
            grdActivityLogDetails.RefreshGrid();
        }

        private void ApplicationLogDetails_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
    }
}
