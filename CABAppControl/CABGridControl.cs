using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CAB.UI.Controls
{
    public partial class CABGridControl : UserControl
    {
        public delegate void GridRowChanged(string msg);
        public event GridRowChanged OnGridRowChanged;
        public delegate void GridRowChanged1(string msg);
        public event GridRowChanged1 OnGridRowChanged1;
        public delegate void CABGridMouseDoubleClick(string KeyValue);
        public event CABGridMouseDoubleClick OnGridMouseDoubleClick;
        private string valueColumn;
        private string hiddenColumn;                
        private string colVal2;
        private string colVal;
        private string rowId= string.Empty;
        private DataSet dataSet;
        private bool isEqual = false;
        private bool isFullRow = false;
        private int selectedIndex = 0;
        
        [Browsable(false)]
        public string HiddenColumn
        {
            get
            {
                return hiddenColumn;
            }
            set
            {
                hiddenColumn = value;
            }
        }
        [Browsable(true)]
        public string SetLabelText
        {           
            set
            {
                lngLabel1.Text = value;
            }
        }
        


        [Browsable(false)]
        public string HiddenColumns
        {
            get
            {
                return hiddenColumn;
            }
            set
            {
                hiddenColumn = value;
            }
        }        
        public bool IsFullRow
        {
            get
            {
                return isFullRow;
            }
            set
            {
                isFullRow = value;
            }
        }
        public bool IsEqual
        {
            get
            {
                return isEqual;
            }
            set
            {
                isEqual = value;
                if (isEqual)
                {
                    this.panelNoData.Width = this.grdData.Width;
                    this.panelNoData.Height = this.grdData.Height;
                }
            }
        }
        [Browsable(false)]
        public string ValueColumn
        {
            get
            {
                return valueColumn;
            }
            set
            {
                valueColumn = value;
            }
        }        
        [Browsable(false)]
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
            }
        }
        
            [Browsable(false)]
        public void SetWidth(string columnName,int width)
        {
            try
            {
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            if (this.grdData.Columns.Contains(columnName))
                            {
                                this.grdData.Columns[columnName].Width = width;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

            [Browsable(false)]
            public void SetWidth(int columnId, int width)
            {
                try
                {
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables[0]!=null)
                    {
                        if (dataSet.Tables[0].Rows.Count > 0 && dataSet.Tables[0].Columns.Count > columnId)
                        {
                            this.grdData.Columns[columnId].Width = width;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            [Browsable(false)]
            public void SetHeaderText(int columnId, string text)
            {
                try
                {
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables[0] != null)
                    {
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            this.grdData.Columns[columnId].HeaderText = text;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            [Browsable(false)]
            public void ResizeColumn(bool allowResize)
            {
                try
                {
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables[0] != null)
                    {
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            this.grdData.AllowUserToResizeColumns = allowResize;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        [Browsable(false)]
        public void SetVisibility(string columnName, bool visible)
        {
            try
            {
                if (dataSet != null)
                {
                    if (dataSet.Tables.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            if (this.grdData.Columns.Contains(columnName))
                            {
                                this.grdData.Columns[columnName].Visible = visible;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        //[Browsable(false)]
        //public void SetRowVisibility(int rowIndex, bool visible)
        //{
        //    try
        //    {
        //        if (dataSet != null)
        //        {
        //            if (dataSet.Tables.Count > 0)
        //            {
        //                if (dataSet.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataGridViewRow dataRow in this.grdData.Rows)
        //                    {
        //                        if (grdData.Rows.IndexOf(dataRow) == rowIndex)
        //                        {
        //                            dataRow.Visible = visible;
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        [Browsable(false)]
        public string GetPrimaryValue()
        {
            this.grdData_SelectionChanged(this, null);
            return colVal;
        }
        [Browsable(false)]
        public string GetFileUploadId()
        {
            return colVal2;
        }
        [Browsable(false)]
        public void SetEqualWidth()
        {
            int width = grdData.Width - 50;
            int totCol = grdData.Columns.Count;
            if (totCol == 0)
                return;
            int ExactWidth = width / totCol;
            if (dataSet != null)
                if (dataSet.Tables.Count > 0)
                    foreach (DataColumn col in dataSet.Tables[0].Columns)
                    {
                        this.grdData.Columns[col.ColumnName].Width = ExactWidth;
                    }
        }
        [Browsable(false)]
        public void RefreshGrid()
        {
            this.Data = dataSet;
            foreach (DataGridViewTextBoxColumn column in grdData.Columns)
            {
                //column.Frozen = true;
                column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            }
        }
        [Browsable(false)]
        public DataSet Data
        {
            get 
            { 
                return dataSet; 
            }
            set 
            { 
                dataSet = value;
                if (dataSet == null)
                {
                    ShowControl();
                    return;
                }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        this.grdData.Visible = true;
                        //grdData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                        //grdData.AllowUserToResizeColumns = false;
                        //grdData.AllowUserToResizeRows = false;
                        this.panelNoData.Visible = false;
                        this.grdData.DataSource = dataSet.Tables[0];
                        if (!string.IsNullOrEmpty(hiddenColumn))
                        {
                            if (hiddenColumn.IndexOf(",") > 0)
                            {
                                string[] columnHidden = hiddenColumn.Split(',');
                                for(int i=0;i<columnHidden.Length;i++)
                                    this.grdData.Columns[columnHidden[i]].Visible = false;
                            }
                            else
                                this.grdData.Columns[hiddenColumn].Visible = false;
                        }
                    }
                    else
                    {
                        this.grdData.DataSource = dataSet.Tables[0];
                        ShowControl();
                    }
                }
                else
                    ShowControl();
                IsSorting=true;  
            } 
        }
        private bool isSort = false;
        public bool IsSorting
        {
            get { return isSort; }
            set
            {
                isSort = value;
                //if (!isSort)
                {
                    foreach (DataGridViewTextBoxColumn column in grdData.Columns)
                    {
                        //column.Frozen = true;
                        column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
        }

        /// <summary>
        /// GET/SET ROW ID .
        /// </summary>
        public string SelectedRowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        private void ShowControl()
        {
            this.grdData.Visible = false;
            this.panelNoData.Visible = true; 
        }
        public CABGridControl()
        {
            InitializeComponent();
            ApplyPremiumGridStyle();
        }

        private void ApplyPremiumGridStyle()
        {
            if (this.grdData != null)
            {
                this.grdData.BackgroundColor = System.Drawing.Color.White;
                this.grdData.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.grdData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
                this.grdData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
                
                System.Windows.Forms.DataGridViewCellStyle headerStyle = new System.Windows.Forms.DataGridViewCellStyle();
                headerStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                headerStyle.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
                headerStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
                headerStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
                headerStyle.SelectionBackColor = System.Drawing.Color.FromArgb(240, 242, 245);
                headerStyle.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
                this.grdData.ColumnHeadersDefaultCellStyle = headerStyle;
                this.grdData.EnableHeadersVisualStyles = false;

                System.Windows.Forms.DataGridViewCellStyle rowStyle = new System.Windows.Forms.DataGridViewCellStyle();
                rowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                rowStyle.BackColor = System.Drawing.Color.White;
                rowStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
                rowStyle.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
                rowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(0, 120, 215);
                rowStyle.SelectionForeColor = System.Drawing.Color.White;
                this.grdData.DefaultCellStyle = rowStyle;

                this.grdData.RowHeadersVisible = false;
                this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
                this.grdData.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
                this.grdData.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
            }
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0 && (!string.IsNullOrEmpty(valueColumn)))
            {
                colVal = grdData.SelectedRows[0].Cells[valueColumn].Value.ToString();
                if (grdData.Columns.Contains("FileUpload_ID"))
                {
                    colVal2 = grdData.SelectedRows[0].Cells["FileUpload_ID"].Value.ToString();
                }

                for (selectedIndex = 0; selectedIndex < dataSet.Tables[0].Rows.Count; selectedIndex++)
                {
                    if (Convert.ToString(dataSet.Tables[0].Rows[selectedIndex][valueColumn]).Equals(colVal))
                        break;
                }
                if (isFullRow)
                {
                    colVal = string.Empty;
                    for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                        colVal = string.Concat(colVal, Convert.ToString(dataSet.Tables[0].Rows[selectedIndex][i]), "|");
                    if (colVal.Length > 0)
                        colVal = colVal.Substring(0, colVal.Length - 1);
                }

                //Set unique value to row id field.
                this.SelectedRowId = colVal;

                if (OnGridRowChanged != null)
                    OnGridRowChanged(colVal);
                if (OnGridRowChanged1 != null)
                    OnGridRowChanged1(colVal2);
            }
            else
            {
                colVal = string.Empty;
                colVal2 = string.Empty;
            }

            //Set unique value to row id field.
            this.SelectedRowId = colVal;
           
        }

        private void grdData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnGridMouseDoubleClick != null)
                OnGridMouseDoubleClick(colVal);
        }

        public void Addrows(int rowindex,string colname,string val)
        {
            grdData.Rows[rowindex].Cells[colname].Value = val;
        }

        public void ClearData(string Value)
        {
            int rcnt = 0;
            while (rcnt < grdData.RowCount)
            {
                grdData.Rows[rcnt++].Cells[Value].Value = null;
            }
        }

    }
}
