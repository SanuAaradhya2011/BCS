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

namespace CAB.UI
{
    public partial class FileSelectForm : Form
    {
        public FileSelectForm()
        {
            InitializeComponent();
        }

        public delegate void GetValueColumn(string gridValue);
        public event GetValueColumn OnGridValue_Selection;
        public string FileName { get; set; }

        private void FileSelectForm_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            lngGridFileDisplay.Data = new FileUploadMasterBLL().GetCABFileNames();
            if (lngGridFileDisplay.Data.Tables[0].Rows.Count == 0)
                btnOK.Enabled = false;
            else
                btnOK.Enabled = true;
            lngGridFileDisplay.SetWidth("S.No", 60);
            lngGridFileDisplay.SetWidth("FileName", 215);
            lngGridFileDisplay.IsSorting = false;
            lngGridFileDisplay.HiddenColumn = "FileUpload_ID";
            lngGridFileDisplay.ValueColumn = "FileName";
            lngGridFileDisplay.RefreshGrid();
        }
        public void GridValueSelection(string gridValue)
        {
            if (OnGridValue_Selection != null)
            {
                OnGridValue_Selection(gridValue);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            FileName = lngGridFileDisplay.Data.Tables[0].Rows[lngGridFileDisplay.SelectedIndex]["FileName"].ToString();
            if (!string.IsNullOrEmpty(FileName))
            {
                this.GridValueSelection(FileName);
                this.Close();
            }
        }
        private void lngGridFileDisplay_OnGridRowChanged(string fileName)
        {
            this.FileName = fileName;
        }
        private void lngGridFileDisplay_DoubleClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                this.GridValueSelection(FileName);
                this.Close();
            }
        }
    }
}