using System;
using System.Data;
using System.Windows.Forms;
using CAB.BLL;
using CAB.Framework.Utility;
using System.ComponentModel;

namespace CAB.UI.Controls
{
    public partial class CABSearchControl : UserControl
    {
        public delegate void AddClickHandler(object sender, EventArgs e);
        public event AddClickHandler OnAddClick;
        public delegate void EditClickHandler(object sender, EventArgs e);
        public event EditClickHandler OnEditClick;
        public delegate void DeleteClickHandler(object sender, EventArgs e);
        public event DeleteClickHandler OnDeleteClick;
        public delegate void FindNowClickHandler(object sender, EventArgs e);
        public event FindNowClickHandler OnFindNowClick;
        public delegate void SearchClickHandler(object sender, EventArgs e);
        public event SearchClickHandler OnSearchClick;
        public delegate void DeleteAllClickHandler(object sender, EventArgs e);
        public event DeleteAllClickHandler OnDeleteAllClick;
        private DataSet primarySearchTypeData = null;
        private bool searchRequire;
        private bool IsSearchClicked=false;
        [Browsable(false)]
        public bool SearchRequire
        {
            get { return searchRequire; }
            set
            {
                searchRequire = value;
                this.lngbSearch.Visible = searchRequire; 
                this.cboFilerType.Visible = searchRequire;
                this.lngbFindNow.Visible = searchRequire;
                this.dtpFromDate.Visible = searchRequire;
                this.dtpToDate.Visible = searchRequire;
                this.cboData.Visible = searchRequire;
                this.cboText.Visible = searchRequire;
            }
        }

        /// <summary>
        /// This property is used to get & set the Search type creteria.
        /// </summary>
        [Browsable(false)]
        public DataSet PrimarySearchTypeData
        {
            get
            {
                return primarySearchTypeData;
            }
            set
            {
                primarySearchTypeData = value;
            }
        }
        [Browsable(false)]
        public string PrimarySearchTypeComboData
        {
            get
            {
                int selectedIndex = cboFilerType.SelectedIndex;
                if (selectedIndex > -1)
                    return ((System.Data.DataRowView)(cboFilerType.Items[selectedIndex])).Row.ItemArray[0].ToString();
                else
                    return string.Empty;
            }
            set
            {
                for (int counter = 0; counter < cboFilerType.Items.Count; counter++)
                {
                    cboFilerType.SelectedIndex = counter + 1;
                    if ((((System.Data.DataRowView)(cboFilerType.Items[counter])).Row.ItemArray[0].ToString()).Equals(value))
                        break;
                }
            }
        }
        [Browsable(false)]
        public string SecondarySearchTypeComboData
        {
            get
            {
                if (cboData.SelectedIndex > -1)
                    return cboData.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set
            {
                for (int counter = 0; counter < cboData.Items.Count; counter++)
                {
                    cboData.SelectedIndex = counter + 1;
                    if (cboData.SelectedValue.ToString().Equals(value))
                        break;
                }
            }
        }
        [Browsable(false)]
        public string SecondarySearchTypeTextData
        {
            get
            {
                string val = this.cboText.Text;
                if (string.IsNullOrEmpty(val))
                    return string.Empty;
                else
                    return val.Trim();
            }
            set
            {
                cboText.Text = value;
            }
        }

        /// <summary>
        /// This property is used to get & set the Form Date.
        /// </summary>
        [Browsable(false)]
        public long SecondarySearchTypeFromDateData
        {
            get
            {
                return DateUtility.DateTimeToLong(dtpFromDate.Value);
            }
            set
            {
                dtpFromDate.Value = DateUtility.LongToDateTime(value);
            }
        }

        /// <summary>
        /// This property is used to get and set the To Date.
        /// </summary>
        [Browsable(false)]
        public long SecondarySearchTypeToDateData
        {
            get
            {
                return DateUtility.DateTimeToLong(dtpToDate.Value);
            }
            set
            {
                dtpToDate.Value = DateUtility.LongToDateTime(value);
            }
        }
        [Browsable(false)]
        public void RefreshList()
        {
            cboFilerType.DisplayMember = "DisplayMember";
            cboFilerType.ValueMember = "ValueMember";
            if (primarySearchTypeData.Tables[0].Rows.Count > 0)
                cboFilerType.DataSource = primarySearchTypeData.Tables[0];
        }
        [Browsable(false)]
        public void RefreshList(DataSet dataSet)
        {
            primarySearchTypeData = dataSet;
            RefreshList();
        }
        [Browsable(false)]
        public void RefreshControls(DataSet dataSet)
        { 
            if (dataSet == null)
                goto notExist;
            else if (dataSet.Tables.Count == 0)
                goto notExist;
            else if (dataSet.Tables[0].Rows.Count == 0)
                goto notExist;
            else
                goto exist;
        notExist:
            this.lngbAdd.Enabled = true;
            this.lngbEdit.Enabled = false;
            this.lngbDelete.Enabled = false;
            this.lngbSearch.Enabled = false;
            this.HideMainSearch = IsSearchClicked;
            return;
        exist:
            this.lngbAdd.Enabled = true;
            this.lngbEdit.Enabled = true;
            this.lngbDelete.Enabled = true;
            this.lngbSearch.Enabled = true;
            this.HideMainSearch = IsSearchClicked;
        }
        [Browsable(false)]
        public CABSearchControl()
        {
            InitializeComponent();
            this.lngbDelete.Enabled = true;
            this.lngbEdit.Enabled = true;
            this.lnglSearchType.Visible = false;
            this.cboFilerType.Visible = false;
            this.cboData.Visible = false;
            this.cboText.Visible = false;
            this.dtpFromDate.Visible = false;
            this.dtpToDate.Visible = false;
            this.lngbFindNow.Visible = false;
        }
        [Browsable(false)]
        private void lngbSearch_Click(object sender, EventArgs e)
        {
            if (OnSearchClick != null)
            {
                OnSearchClick(sender, e);
            }
            IsSearchClicked = true;
			EnableControls();
            this.lnglSearchType.Visible = true;
            this.lngbSearch.Visible = false; 
                    UpdateFindNowLocation();
        }
        [Browsable(false)]
        public bool EnableAdd
        {
            set { this.lngbAdd.Enabled = value; }
        }
        [Browsable(false)]
        public bool EnableEdit
        {
            set { this.lngbEdit.Enabled = value; }
        }
        [Browsable(false)]
        public bool EnableDelete
        {
            set { this.lngbDelete.Enabled = value; }
        }
        [Browsable(false)]
        public bool EnableSearch
        {
            set { this.lngbSearch.Enabled = value; }
        }
        [Browsable(false)]
        public bool EnableDeleteAll
        {
            set { this.btnDeleteAll.Enabled = value; }
        }

        [Browsable(false)]
        public void ShowSearch()
        {
            this.lngbAdd.Visible = false;
            this.lngbEdit.Visible = false;
            this.lngbDelete.Visible = false;
            this.lngbSearch.Visible = false;
        }

        [Browsable(false)]
        public void ShowSearchandDelete()
        {
            this.lngbAdd.Visible = false;
            this.lngbEdit.Visible = false;
            this.lngbSearch.Visible = true;
        }

        public void ShowSearchText()
        {
            IsSearchClicked = true;
            this.lnglSearchType.Visible = true;
            this.lngbSearch.Visible = false;
        }
        [Browsable(false)]
        public bool HideMainSearch
        {
            get
            {
                return cboFilerType.Visible;
            }
            set
            {
                cboFilerType.Visible = value;
                lngbFindNow.Visible = value;
                if (value == true)
                {
                    this.lnglSearchType.Visible = this.lnglSearchType.Enabled = true;
                    this.lngbSearch.Visible = false;
                } 
            }
        }

		[Browsable(false)]
		public void ShowEditOption()
		{
			this.lngbDelete.Enabled = false;
			this.lngbAdd.Enabled = false;
			this.cboFilerType.Visible = false;
			this.cboText.Visible = false;
			this.dtpFromDate.Visible = false;
			this.dtpToDate.Visible = false;
			this.cboData.Visible = false;
			this.lngbFindNow.Visible = false;
			this.lngbSearch.Enabled = false;
			if (IsSearchClicked)
			{
				this.lnglSearchType.Visible = false;
				this.lngbSearch.Visible = false;
				this.lnglSearchType.Enabled = false;
			}
		}
		[Browsable(false)]
		public void HideEditOption()
		{
			this.lngbDelete.Enabled = true;
			this.lngbAdd.Enabled = true;
			this.lngbEdit.Enabled = false;
			this.cboFilerType.Visible = false;
			this.cboText.Visible = false;
			this.dtpFromDate.Visible = false;
			this.dtpToDate.Visible = false;
			this.cboData.Visible = false;
			this.lngbFindNow.Visible = false;
			this.lngbSearch.Enabled = false;
			if (IsSearchClicked)
			{
				this.lnglSearchType.Visible = false;
				this.lngbSearch.Visible = false;
				this.lnglSearchType.Enabled = false;
			}
		}
		

		[Browsable(false)]
		public void ShowDeleteOption()
		{
			this.lngbDelete.Enabled = true;
			this.lngbAdd.Enabled = true;
			this.cboFilerType.Visible = false;
			this.cboText.Visible = false;
			this.dtpFromDate.Visible = false;
			this.dtpToDate.Visible = false;
			this.cboData.Visible = false;
			this.lngbFindNow.Visible = false;
			this.lngbSearch.Enabled = false;
			if (IsSearchClicked)
			{
				this.lnglSearchType.Visible = false;
				this.lngbSearch.Visible = false;
				this.lnglSearchType.Enabled = false;
			}
		}

        [Browsable(false)]
        public void ShowAddOption()
        {
            this.lngbDelete.Enabled = false;
            this.lngbEdit.Enabled = false;
            this.cboFilerType.Visible = false;
            this.cboText.Visible = false;
            this.dtpFromDate.Visible = false;
            this.dtpToDate.Visible = false;
            this.cboData.Visible = false;
            this.lngbFindNow.Visible = false;
            this.lngbSearch.Enabled = false;
            if (IsSearchClicked)
            {
                this.lnglSearchType.Visible = false;
                this.lngbSearch.Visible = false;
                this.lnglSearchType.Enabled = false;
            }
        }

        [Browsable(false)]
        public void EnableControls()
        {
			this.lngbDelete.Enabled = true;
			this.lngbEdit.Enabled = true;
			this.cboFilerType.Visible = true;
			this.cboText.Visible = false;
			this.dtpFromDate.Visible = false;
			this.dtpToDate.Visible = false;
			this.cboData.Visible = false;
			this.lngbFindNow.Visible = true; 
                    UpdateFindNowLocation();
			if (IsSearchClicked)
			{
				this.lnglSearchType.Visible = false;
				this.lngbSearch.Visible = false;
				this.lnglSearchType.Enabled = false;
			}
        }

		[Browsable(false)]
		public void EnableMainControls()
		{
			this.lngbDelete.Enabled = true;
			this.lngbEdit.Enabled = true;
			this.cboFilerType.Visible = false;
			this.cboText.Visible = false;
			this.dtpFromDate.Visible = false;
			this.dtpToDate.Visible = false;
			this.cboData.Visible = false;
			this.lngbFindNow.Visible = false;	
                    UpdateFindNowLocation();
			if (IsSearchClicked)
			{
				this.lnglSearchType.Visible = false;
				this.lngbSearch.Visible = false;
				this.lnglSearchType.Enabled = false;
			}
		}

        public DataSet PrimaryNonComboData { get; set; }
        public DataSet SecondaryNonComboData { get; set; }
        private void cboFilerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilerType.SelectedIndex > -1)
            {
                string filerType = cboFilerType.SelectedValue.ToString();
                if (filerType.IndexOf('|') > 0)
                {
                    this.cboData.Visible = true;
                    this.dtpFromDate.Visible = false;
                    this.dtpToDate.Visible = false;
                    this.cboText.Visible = false;
                    DataSet dataSet = new SearchControlBLL().GetFilterData(filerType, true);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        cboData.DisplayMember = "DisplayMember";
                        cboData.ValueMember = "ValueMember";
                        cboData.DataSource = dataSet.Tables[0];
                    }
                    UpdateFindNowLocation();
                }
                else if (filerType.ToUpper().Equals("NONCBO1"))
                {
                    this.cboData.Visible = true;
                    this.dtpFromDate.Visible = false;
                    this.dtpToDate.Visible = false;
                    this.cboText.Visible = false;
                    DataSet dataSet = PrimaryNonComboData;
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        cboData.DisplayMember = "DisplayMember";
                        cboData.ValueMember = "ValueMember";
                        cboData.DataSource = dataSet.Tables[0];
                    }
                    UpdateFindNowLocation();
                }
                else if (filerType.ToUpper().Equals("NONCBO2"))
                {
                    this.cboData.Visible = true;
                    this.dtpFromDate.Visible = false;
                    this.dtpToDate.Visible = false;
                    this.cboText.Visible = false;
                    DataSet dataSet = SecondaryNonComboData;
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        cboData.DisplayMember = "DisplayMember";
                        cboData.ValueMember = "ValueMember";
                        cboData.DataSource = dataSet.Tables[0];
                    }
                    UpdateFindNowLocation();
                }
                else if (filerType.ToUpper().Equals("DATE"))
                {   this.dtpFromDate.Value = DateTime.Now;
                    this.dtpToDate.Value = DateTime.Now;
                    this.dtpFromDate.CustomFormat = ConfigInfo.DateFormat();
                    this.dtpToDate.CustomFormat = ConfigInfo.DateFormat();
                    this.dtpFromDate.Format = DateTimePickerFormat.Custom;
                    this.dtpToDate.Format = DateTimePickerFormat.Custom;
                    this.dtpFromDate.Visible = true;
                    this.dtpToDate.Visible = true;
                    this.cboData.Visible = false;
                    this.cboText.Visible = false;
                    UpdateFindNowLocation();
                    this.dtpFromDate.Focus();
                }
                else if (filerType.ToUpper().Equals("TEXT"))
                {
                    this.cboText.Visible = true;
                    this.dtpFromDate.Visible = false;
                    this.dtpToDate.Visible = false;
                    this.cboData.Visible = false;
                    this.cboText.Text = string.Empty;
                    UpdateFindNowLocation();
                    this.cboText.Focus();
                }
                else
                {
                    this.cboText.Visible = false;
                    this.dtpFromDate.Visible = false;
                    this.dtpToDate.Visible = false;
                    this.cboData.Visible = false;
                    UpdateFindNowLocation();
                }
            }
        }

        private void lngbAdd_Click(object sender, EventArgs e)
        {
            if (OnAddClick != null)
            {
                OnAddClick(sender, e);
            }
        }

        private void lngbEdit_Click(object sender, EventArgs e)
        {
            if (OnEditClick != null)
            {
                OnEditClick(sender, e);
            }
        }

        private void lngbDelete_Click(object sender, EventArgs e)
        {
            if (OnDeleteClick != null)
            {
                OnDeleteClick(sender, e);
            }
        }
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (OnDeleteAllClick != null)
            {
                OnDeleteAllClick(sender, e);
            }
        }

        private void lngbFindNow_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;
            if (OnFindNowClick != null)
            {
                OnFindNowClick(sender, e);
            }
        }

        private bool ValidateData()
        {
            string filerType = cboFilerType.SelectedValue.ToString();
            if (filerType.ToUpper().Equals("TEXT"))
            {
                string val = cboText.Text;
                if (string.IsNullOrEmpty(val))
                {
                    CABMessageBox.ShowMessage("M000009", "C000002");
                    cboText.Focus();
                    return false;
                }
                //if (!ValidationProvider.ValidateData(val, ValidationConstant.Search))
                //{
                //    CABMessageBox.ShowMessage("M000010", "C000002");
                //    cboText.Focus();
                //    return false;
                //}
            }
            return true;
        }

        private void btnDeleteAll_Click_1(object sender, EventArgs e)
        {

        }
        private void UpdateFindNowLocation()
        {
            int findNowX = this.cboFilerType.Location.X + this.cboFilerType.Width + 5;
            if (this.cboData.Visible) findNowX = this.cboData.Location.X + this.cboData.Width + 5;
            if (this.dtpToDate.Visible) findNowX = this.dtpToDate.Location.X + this.dtpToDate.Width + 5;
            if (this.cboText.Visible) findNowX = this.cboText.Location.X + this.cboText.Width + 5;
            this.lngbFindNow.Location = new System.Drawing.Point(findNowX, 4);
        }
    }
}
