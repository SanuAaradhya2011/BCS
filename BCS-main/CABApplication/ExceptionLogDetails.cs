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
    public partial class ExceptionLogDetails : MdiChildForm 
    {
        ExceptionLogDetailsBLL exceptionLogDetailsBLL = null;
        public ExceptionLogDetails()
        {
            exceptionLogDetailsBLL = new ExceptionLogDetailsBLL();
            InitializeComponent();
        }

        private void ExceptionLogDetails_Load(object sender, EventArgs e)
        {
            lngscApplicationLogDetails.PrimarySearchTypeData = GetSearchData();
            lngscApplicationLogDetails.ShowSearch();
            lngscApplicationLogDetails.RefreshList();
            lngscApplicationLogDetails.ShowSearchText();
            lngscApplicationLogDetails_OnFindNowClick(this, null);
        }

        public DataSet GetSearchData()
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

            if (fromDate > toDate)
            {
                //CABMessageBox.ShowFilterMessage("M000003", "A000001", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return;
                this.StatusMessage = "To date can't be less than From Date";
                Application.DoEvents();
                return;
            }
            if (searchType.Equals("All"))
            {
                dataSet = exceptionLogDetailsBLL.GetAllLogActivity();
            }
            else if (searchType.Equals("Date Specific"))
            {
                dataSet = exceptionLogDetailsBLL.GetDateWiseLogActivity(DateUtility.ConvertSearchDateTimeToLong(fromDate, "000000"), DateUtility.ConvertSearchDateTimeToLong(toDate, "235959"));
            }
            grdExceptionLogDetails.DataSource= dataSet.Tables[0];
            grdExceptionLogDetails.Columns[0].Width = 30;
            grdExceptionLogDetails.Columns[0].HeaderText = "SL.";
            grdExceptionLogDetails.Columns[0].ReadOnly = true;

            grdExceptionLogDetails.Columns[1].Width = 120;
            grdExceptionLogDetails.Columns[1].HeaderText = "Date";
            grdExceptionLogDetails.Columns[1].ReadOnly = true;

            grdExceptionLogDetails.Columns[2].Width = 300;
            grdExceptionLogDetails.Columns[2].HeaderText = "Source";
            grdExceptionLogDetails.Columns[2].ReadOnly = true;

            grdExceptionLogDetails.Columns[3].Width = 350;
            grdExceptionLogDetails.Columns[3].HeaderText = "Message";
            grdExceptionLogDetails.Columns[3].ReadOnly = true;

        }

        private void ExceptionLogDetails_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }
    }
}
