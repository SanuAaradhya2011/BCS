namespace CAB.UI.Controls
{
    partial class CABSearchControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboFilerType = new System.Windows.Forms.ComboBox();
            this.cboData = new System.Windows.Forms.ComboBox();
            this.cboText = new System.Windows.Forms.TextBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnDeleteAll = new CAB.UI.Controls.CABButton();
            this.lngbFindNow = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lngbSearch = new CAB.UI.Controls.CABButton();
            this.lnglSearchType = new CAB.UI.Controls.CABLabel();
            this.SuspendLayout();
            // 
            // cboFilerType
            // 
            this.cboFilerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilerType.FormattingEnabled = true;
            this.cboFilerType.Location = new System.Drawing.Point(327, 5);
            this.cboFilerType.Name = "cboFilerType";
            this.cboFilerType.Size = new System.Drawing.Size(121, 21);
            this.cboFilerType.TabIndex = 5;
            this.cboFilerType.SelectedIndexChanged += new System.EventHandler(this.cboFilerType_SelectedIndexChanged);
            // 
            // cboData
            // 
            this.cboData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboData.FormattingEnabled = true;
            this.cboData.Location = new System.Drawing.Point(454, 5);
            this.cboData.Name = "cboData";
            this.cboData.Size = new System.Drawing.Size(204, 21);
            this.cboData.TabIndex = 6;
            // 
            // cboText
            // 
            this.cboText.Location = new System.Drawing.Point(454, 5);
            this.cboText.Name = "cboText";
            this.cboText.Size = new System.Drawing.Size(204, 20);
            this.cboText.TabIndex = 7;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(454, 5);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpFromDate.Size = new System.Drawing.Size(99, 20);
            this.dtpFromDate.TabIndex = 10;
            this.dtpFromDate.Value = new System.DateTime(2010, 3, 23, 0, 0, 0, 0);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd/MM/yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(559, 5);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dtpToDate.Size = new System.Drawing.Size(99, 20);
            this.dtpToDate.TabIndex = 11;
            this.dtpToDate.Value = new System.DateTime(2010, 3, 23, 0, 0, 0, 0);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Location = new System.Drawing.Point(246, 3);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAll.TabIndex = 12;
            this.btnDeleteAll.Text = "&Delete All";
            this.btnDeleteAll.TranslationKey = "B000021";
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnDeleteAll.ForeColor = System.Drawing.Color.White;
            this.btnDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAll.FlatAppearance.BorderSize = 0;
            this.btnDeleteAll.Visible = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // lngbFindNow
            // 
            this.lngbFindNow.Location = new System.Drawing.Point(664, 4);
            this.lngbFindNow.Name = "lngbFindNow";
            this.lngbFindNow.Size = new System.Drawing.Size(75, 23);
            this.lngbFindNow.TabIndex = 8;
            this.lngbFindNow.Text = "&Find Now";
            this.lngbFindNow.TranslationKey = "B000007";
            this.lngbFindNow.UseVisualStyleBackColor = false;
            this.lngbFindNow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbFindNow.ForeColor = System.Drawing.Color.White;
            this.lngbFindNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbFindNow.FlatAppearance.BorderSize = 0;
            this.lngbFindNow.Click += new System.EventHandler(this.lngbFindNow_Click);
            // 
            // lngbDelete
            // 
            this.lngbDelete.Location = new System.Drawing.Point(165, 3);
            this.lngbDelete.Name = "lngbDelete";
            this.lngbDelete.Size = new System.Drawing.Size(75, 23);
            this.lngbDelete.TabIndex = 2;
            this.lngbDelete.Text = "&Delete";
            this.lngbDelete.TranslationKey = "B000005";
            this.lngbDelete.UseVisualStyleBackColor = false;
            this.lngbDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbDelete.ForeColor = System.Drawing.Color.White;
            this.lngbDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbDelete.FlatAppearance.BorderSize = 0;
            this.lngbDelete.Click += new System.EventHandler(this.lngbDelete_Click);
            // 
            // lngbEdit
            // 
            this.lngbEdit.Location = new System.Drawing.Point(84, 3);
            this.lngbEdit.Name = "lngbEdit";
            this.lngbEdit.Size = new System.Drawing.Size(75, 23);
            this.lngbEdit.TabIndex = 1;
            this.lngbEdit.Text = "&Edit";
            this.lngbEdit.TranslationKey = "B000004";
            this.lngbEdit.UseVisualStyleBackColor = false;
            this.lngbEdit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbEdit.ForeColor = System.Drawing.Color.White;
            this.lngbEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbEdit.FlatAppearance.BorderSize = 0;
            this.lngbEdit.Click += new System.EventHandler(this.lngbEdit_Click);
            // 
            // lngbAdd
            // 
            this.lngbAdd.Location = new System.Drawing.Point(3, 3);
            this.lngbAdd.Name = "lngbAdd";
            this.lngbAdd.Size = new System.Drawing.Size(75, 23);
            this.lngbAdd.TabIndex = 0;
            this.lngbAdd.Text = "&Add";
            this.lngbAdd.TranslationKey = "B000003";
            this.lngbAdd.UseVisualStyleBackColor = false;
            this.lngbAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbAdd.ForeColor = System.Drawing.Color.White;
            this.lngbAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbAdd.FlatAppearance.BorderSize = 0;
            this.lngbAdd.Click += new System.EventHandler(this.lngbAdd_Click);
            // 
            // lngbSearch
            // 
            this.lngbSearch.Location = new System.Drawing.Point(246, 3);
            this.lngbSearch.Name = "lngbSearch";
            this.lngbSearch.Size = new System.Drawing.Size(75, 23);
            this.lngbSearch.TabIndex = 4;
            this.lngbSearch.Text = "&Search";
            this.lngbSearch.TranslationKey = "B000006";
            this.lngbSearch.UseVisualStyleBackColor = false;
            this.lngbSearch.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbSearch.ForeColor = System.Drawing.Color.White;
            this.lngbSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSearch.FlatAppearance.BorderSize = 0;
            this.lngbSearch.Click += new System.EventHandler(this.lngbSearch_Click);
            // 
            // lnglSearchType
            // 
            this.lnglSearchType.AutoSize = true;
            this.lnglSearchType.Location = new System.Drawing.Point(253, 8);
            this.lnglSearchType.Name = "lnglSearchType";
            this.lnglSearchType.Size = new System.Drawing.Size(68, 13);
            this.lnglSearchType.TabIndex = 3;
            this.lnglSearchType.Text = "Search Type";
            this.lnglSearchType.TranslationKey = "L000003";
            // 
            // CABSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.lngbFindNow);
            this.Controls.Add(this.cboFilerType);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.lngbSearch);
            this.Controls.Add(this.lnglSearchType);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.cboData);
            this.Controls.Add(this.cboText);
            this.Name = "CABSearchControl";
            this.Size = new System.Drawing.Size(749, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABButton lngbEdit;
        private CAB.UI.Controls.CABButton lngbDelete;
        private CAB.UI.Controls.CABLabel lnglSearchType;
        private CAB.UI.Controls.CABButton lngbSearch;
        private System.Windows.Forms.ComboBox cboFilerType;
        private System.Windows.Forms.ComboBox cboData;
        private System.Windows.Forms.TextBox cboText;
        private CAB.UI.Controls.CABButton lngbFindNow;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private CABButton btnDeleteAll;
    }
}

