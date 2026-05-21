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
    public partial class ApplicationLogDetails : MdiChildForm
    {
        private static readonly Color SearchAccentColor = Color.FromArgb(26, 115, 232);
        private static readonly Color SearchSurfaceColor = Color.FromArgb(232, 241, 255);
        private static readonly Color SearchTextColor = Color.FromArgb(20, 28, 45);
		UserLogActivityBLL userLogActivityBLL = null;

        public ApplicationLogDetails()
        {
			userLogActivityBLL = new UserLogActivityBLL();
            InitializeComponent();
            ApplyUiTheme();
        }

        private void ApplyUiTheme()
        {
            this.lngscApplicationLogDetails.BackColor = SearchSurfaceColor;

            foreach (Control control in this.lngscApplicationLogDetails.Controls)
            {
                ComboBox comboBox = control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.BackColor = Color.White;
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.ForeColor = SearchTextColor;
                    comboBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    continue;
                }

                DateTimePicker datePicker = control as DateTimePicker;
                if (datePicker != null)
                {
                    datePicker.CalendarForeColor = SearchTextColor;
                    datePicker.CalendarMonthBackground = Color.White;
                    datePicker.CalendarTitleBackColor = SearchAccentColor;
                    datePicker.CalendarTitleForeColor = Color.White;
                    datePicker.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    continue;
                }

                CABButton button = control as CABButton;
                if (button != null && button.Name == "lngbFindNow")
                {
                    button.BackColor = SearchAccentColor;
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    continue;
                }

                CABLabel label = control as CABLabel;
                if (label != null)
                {
                    label.ForeColor = SearchTextColor;
                    label.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                }
            }

            this.grdActivityLogDetails.BackgroundColor = Color.White;
            this.grdActivityLogDetails.DefaultCellStyle.BackColor = Color.White;
            this.grdActivityLogDetails.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 241, 255);
            this.grdActivityLogDetails.DefaultCellStyle.SelectionForeColor = SearchTextColor;
            this.grdActivityLogDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            this.grdActivityLogDetails.ColumnHeadersDefaultCellStyle.ForeColor = SearchTextColor;
            this.grdActivityLogDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.grdActivityLogDetails.DefaultCellStyle.Font = new Font("Segoe UI", 9.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.grdActivityLogDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 253);
            this.grdActivityLogDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

			if (fromDate > toDate)
			{
				this.StatusMessage ="To date can't be less than From Date";
                Application.DoEvents();
				return;
			}
			if (searchType.Equals("All"))
			{
				dataSet = userLogActivityBLL.GetAllLogActivity();
			}
			else if (searchType.Equals("Date Specific"))
			{
				dataSet = userLogActivityBLL.GetDateWiseLogActivity(DateUtility.ConvertSearchDateTimeToLong(fromDate,"000000"), DateUtility.ConvertSearchDateTimeToLong(toDate,"235959"));
			}
			grdActivityLogDetails.DataSource = dataSet.Tables[0];
		}

        private void ApplicationLogDetails_Activated(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
        }

        private void lngscApplicationLogDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
