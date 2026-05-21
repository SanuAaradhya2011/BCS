using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LTCTBLL;
using CAB.Entity;
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

        public delegate void CABGridCellFormat(DataGridViewCellFormattingEventArgs value);
        public event CABGridCellFormat OnCABGridCellFormat;


        private string valueColumn;
        private string valueColumn2;
        private string hiddenColumn;
        private string hiddenColumn2;
        private string hiddenColumn3;
        [Browsable(false)]
        public string HiddenColumn3
        {
            get { return hiddenColumn3; }
            set { hiddenColumn3 = value; }
        }
        private string hiddenColumn4;
        [Browsable(false)]
        public string HiddenColumn4
        {
            get { return hiddenColumn4; }
            set { hiddenColumn4 = value; }
        }
        private string colVal;
        private string colVal2;
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
        [Browsable(false)]
        public string HiddenColumn2
        {
            get
            {
                return hiddenColumn2;
            }
            set
            {
                hiddenColumn2 = value;
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
        public string ValueColumn2
        {
            get
            {
                return valueColumn2;
            }
            set
            {
                valueColumn2 = value;
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
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        this.grdData.Columns[columnName].Width = width;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        [Browsable(false)]
        public string GetPrimaryValue()
        {
            this.grdData_SelectionChanged(this, null);
            return colVal;
        }
        [Browsable(false)]
        public void SetEqualWidth()
        {
            int width = grdData.Width-50;
            int totCol = grdData.Columns.Count;
            if (totCol == 0)
                return;
            int ExactWidth = width / totCol;
            foreach(DataColumn col in dataSet.Tables[0].Columns)
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
                column.Frozen = true;
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
                        column.Frozen = false;
                        column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
        }
        private void ShowControl()
        {
            this.grdData.Visible = false;
            this.panelNoData.Visible = true; 
        }
        public CABGridControl()
        {
            InitializeComponent();
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (grdData.SelectedRows.Count > 0 && (!string.IsNullOrEmpty(valueColumn)))
            {
               
                colVal = grdData.SelectedRows[0].Cells[valueColumn].Value.ToString();
                if (grdData.Columns.Contains("FileUpload_ID"))
                    if (UtilityDetails.UtilityName == IECUtilityEntity.TNEB || UtilityDetails.UtilityName == IECUtilityEntity.TNEB1)
                    {
                        colVal2 = grdData.SelectedRows[0].Cells[valueColumn2].Value.ToString();
                    }
                    else
                        colVal2 = "-1";
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
        }

        private void grdData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OnGridMouseDoubleClick != null)
                OnGridMouseDoubleClick(colVal);
        }
        public string GetHiddenColumnValue()
        {
            return colVal2;
        }
        public void HSrollBar()
        {
            this.Data = dataSet;
            foreach (DataGridViewTextBoxColumn column in grdData.Columns)
            {
                column.Frozen = false;
            }
            grdData.AllowUserToResizeColumns = false;
            grdData.AllowUserToResizeRows = false;
        }

        private void grdData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.Value=="NP")
            //{ 
            //    e.Graphics.Color.Aquamarine, e.CellBounds); 
            //} 
            //else if (e.Value =="NL")
            //{ 
            //    e.Graphics.FillRectangle(Color.Grey, e.CellBounds); 
            //} 
            
        }

        private void grdData_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (OnCABGridCellFormat != null)
                OnCABGridCellFormat(e);

        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
