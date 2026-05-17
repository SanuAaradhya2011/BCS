namespace CAB.UI
{
    partial class AreaMaster
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnAddArea = new System.Windows.Forms.Panel();
            this.pnAreaMaster = new System.Windows.Forms.Panel();
            this.gridAreaMaster = new CAB.UI.Controls.CABGridControl();
            this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
            this.pnNewAreaDefinition = new System.Windows.Forms.Panel();
            this.GrpAddNew = new System.Windows.Forms.GroupBox();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.lblSelectedMeters = new CAB.UI.Controls.CABLabel();
            this.lblAvailableMeters = new CAB.UI.Controls.CABLabel();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.btnSave = new CAB.UI.Controls.CABButton();
            this.btnMoveBackAll = new CAB.UI.Controls.CABButton();
            this.btnMoveBack = new CAB.UI.Controls.CABButton();
            this.btnMoveNextAll = new CAB.UI.Controls.CABButton();
            this.btnMoveNext = new CAB.UI.Controls.CABButton();
            this.gbCMRI = new System.Windows.Forms.GroupBox();
            this.cboMRI = new System.Windows.Forms.ComboBox();
            this.lblMRI = new CAB.UI.Controls.CABLabel();
            this.gbArea = new System.Windows.Forms.GroupBox();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.txtCircle = new System.Windows.Forms.TextBox();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.btnDivision = new CAB.UI.Controls.CABButton();
            this.btnCircle = new CAB.UI.Controls.CABButton();
            this.btnRegion = new CAB.UI.Controls.CABButton();
            this.lblDivision = new CAB.UI.Controls.CABLabel();
            this.lblCircle = new CAB.UI.Controls.CABLabel();
            this.lblRegion = new CAB.UI.Controls.CABLabel();
            this.pnAddArea.SuspendLayout();
            this.pnAreaMaster.SuspendLayout();
            this.pnNewAreaDefinition.SuspendLayout();
            this.GrpAddNew.SuspendLayout();
            this.gbCMRI.SuspendLayout();
            this.gbArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnAddArea
            // 
            this.pnAddArea.AutoScroll = true;
            this.pnAddArea.Controls.Add(this.pnAreaMaster);
            this.pnAddArea.Controls.Add(this.pnNewAreaDefinition);
            this.pnAddArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAddArea.Location = new System.Drawing.Point(0, 0);
            this.pnAddArea.Name = "pnAddArea";
            this.pnAddArea.Size = new System.Drawing.Size(1284, 507);
            this.pnAddArea.TabIndex = 1;
            // 
            // pnAreaMaster
            // 
            this.pnAreaMaster.AutoScroll = true;
            this.pnAreaMaster.Controls.Add(this.gridAreaMaster);
            this.pnAreaMaster.Controls.Add(this.ucSearchControl);
            this.pnAreaMaster.Location = new System.Drawing.Point(1, 1);
            this.pnAreaMaster.Name = "pnAreaMaster";
            this.pnAreaMaster.Size = new System.Drawing.Size(561, 451);
            this.pnAreaMaster.TabIndex = 7;
            // 
            // gridAreaMaster
            // 
            this.gridAreaMaster.Data = null;
            this.gridAreaMaster.HiddenColumn = null;
            this.gridAreaMaster.HiddenColumn2 = null;
            this.gridAreaMaster.HiddenColumn3 = null;
            this.gridAreaMaster.HiddenColumn4 = null;
            this.gridAreaMaster.HiddenColumns = null;
            this.gridAreaMaster.IsEqual = false;
            this.gridAreaMaster.IsFullRow = false;
            this.gridAreaMaster.IsSorting = false;
            this.gridAreaMaster.Location = new System.Drawing.Point(23, 58);
            this.gridAreaMaster.Name = "gridAreaMaster";
            this.gridAreaMaster.SelectedIndex = 0;
            this.gridAreaMaster.Size = new System.Drawing.Size(460, 263);
            this.gridAreaMaster.TabIndex = 0;
            this.gridAreaMaster.ValueColumn = null;
            this.gridAreaMaster.ValueColumn2 = null;
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.HideMainSearch = false;
            this.ucSearchControl.Location = new System.Drawing.Point(19, 16);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = false;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(540, 32);
            this.ucSearchControl.TabIndex = 6;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // pnNewAreaDefinition
            // 
            this.pnNewAreaDefinition.Controls.Add(this.GrpAddNew);
            this.pnNewAreaDefinition.Location = new System.Drawing.Point(575, 1);
            this.pnNewAreaDefinition.Name = "pnNewAreaDefinition";
            this.pnNewAreaDefinition.Size = new System.Drawing.Size(543, 451);
            this.pnNewAreaDefinition.TabIndex = 6;
            // 
            // GrpAddNew
            // 
            this.GrpAddNew.Controls.Add(this.listBoxSelectedMeters);
            this.GrpAddNew.Controls.Add(this.listBoxAvailableMeters);
            this.GrpAddNew.Controls.Add(this.lblSelectedMeters);
            this.GrpAddNew.Controls.Add(this.lblAvailableMeters);
            this.GrpAddNew.Controls.Add(this.btnCancel);
            this.GrpAddNew.Controls.Add(this.btnSave);
            this.GrpAddNew.Controls.Add(this.btnMoveBackAll);
            this.GrpAddNew.Controls.Add(this.btnMoveBack);
            this.GrpAddNew.Controls.Add(this.btnMoveNextAll);
            this.GrpAddNew.Controls.Add(this.btnMoveNext);
            this.GrpAddNew.Controls.Add(this.gbCMRI);
            this.GrpAddNew.Controls.Add(this.gbArea);
            this.GrpAddNew.Location = new System.Drawing.Point(5, 6);
            this.GrpAddNew.Name = "GrpAddNew";
            this.GrpAddNew.Size = new System.Drawing.Size(528, 433);
            this.GrpAddNew.TabIndex = 4;
            this.GrpAddNew.TabStop = false;
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.HorizontalScrollbar = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(299, 197);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(200, 186);
            this.listBoxSelectedMeters.TabIndex = 5;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.HorizontalScrollbar = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(27, 197);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(200, 186);
            this.listBoxAvailableMeters.TabIndex = 0;
            // 
            // lblSelectedMeters
            // 
            this.lblSelectedMeters.AutoSize = true;
            this.lblSelectedMeters.Location = new System.Drawing.Point(302, 181);
            this.lblSelectedMeters.Name = "lblSelectedMeters";
            this.lblSelectedMeters.Size = new System.Drawing.Size(79, 13);
            this.lblSelectedMeters.TabIndex = 37;
            this.lblSelectedMeters.Text = "Selected Meter";
            this.lblSelectedMeters.TranslationKey = "L000048";
            // 
            // lblAvailableMeters
            // 
            this.lblAvailableMeters.AutoSize = true;
            this.lblAvailableMeters.Location = new System.Drawing.Point(29, 181);
            this.lblAvailableMeters.Name = "lblAvailableMeters";
            this.lblAvailableMeters.Size = new System.Drawing.Size(80, 13);
            this.lblAvailableMeters.TabIndex = 36;
            this.lblAvailableMeters.Text = "Available Meter";
            this.lblAvailableMeters.TranslationKey = "L000047";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(419, 393);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TranslationKey = null;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(335, 393);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 28);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.TranslationKey = null;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnMoveBackAll
            // 
            this.btnMoveBackAll.Location = new System.Drawing.Point(240, 335);
            this.btnMoveBackAll.Name = "btnMoveBackAll";
            this.btnMoveBackAll.Size = new System.Drawing.Size(46, 29);
            this.btnMoveBackAll.TabIndex = 4;
            this.btnMoveBackAll.Text = "<<";
            this.btnMoveBackAll.TranslationKey = null;
            this.btnMoveBackAll.UseVisualStyleBackColor = true;
            this.btnMoveBackAll.Click += new System.EventHandler(this.btnMoveBackAll_Click);
            // 
            // btnMoveBack
            // 
            this.btnMoveBack.Location = new System.Drawing.Point(240, 300);
            this.btnMoveBack.Name = "btnMoveBack";
            this.btnMoveBack.Size = new System.Drawing.Size(46, 29);
            this.btnMoveBack.TabIndex = 3;
            this.btnMoveBack.Text = "<";
            this.btnMoveBack.TranslationKey = null;
            this.btnMoveBack.UseVisualStyleBackColor = true;
            this.btnMoveBack.Click += new System.EventHandler(this.btnMoveBack_Click);
            // 
            // btnMoveNextAll
            // 
            this.btnMoveNextAll.Location = new System.Drawing.Point(240, 265);
            this.btnMoveNextAll.Name = "btnMoveNextAll";
            this.btnMoveNextAll.Size = new System.Drawing.Size(46, 29);
            this.btnMoveNextAll.TabIndex = 2;
            this.btnMoveNextAll.Text = ">>";
            this.btnMoveNextAll.TranslationKey = null;
            this.btnMoveNextAll.UseVisualStyleBackColor = true;
            this.btnMoveNextAll.Click += new System.EventHandler(this.btnMoveNextAll_Click);
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.Location = new System.Drawing.Point(240, 230);
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.Size = new System.Drawing.Size(46, 29);
            this.btnMoveNext.TabIndex = 1;
            this.btnMoveNext.Text = ">";
            this.btnMoveNext.TranslationKey = null;
            this.btnMoveNext.UseVisualStyleBackColor = true;
            this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
            // 
            // gbCMRI
            // 
            this.gbCMRI.Controls.Add(this.cboMRI);
            this.gbCMRI.Controls.Add(this.lblMRI);
            this.gbCMRI.Location = new System.Drawing.Point(27, 125);
            this.gbCMRI.Name = "gbCMRI";
            this.gbCMRI.Size = new System.Drawing.Size(472, 47);
            this.gbCMRI.TabIndex = 27;
            this.gbCMRI.TabStop = false;
            // 
            // cboMRI
            // 
            this.cboMRI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMRI.FormattingEnabled = true;
            this.cboMRI.Location = new System.Drawing.Point(186, 16);
            this.cboMRI.Name = "cboMRI";
            this.cboMRI.Size = new System.Drawing.Size(165, 21);
            this.cboMRI.TabIndex = 0;
            // 
            // lblMRI
            // 
            this.lblMRI.AutoSize = true;
            this.lblMRI.Location = new System.Drawing.Point(122, 22);
            this.lblMRI.Name = "lblMRI";
            this.lblMRI.Size = new System.Drawing.Size(34, 13);
            this.lblMRI.TabIndex = 19;
            this.lblMRI.Text = "CMRI";
            this.lblMRI.TranslationKey = "L000046";
            // 
            // gbArea
            // 
            this.gbArea.Controls.Add(this.txtDivision);
            this.gbArea.Controls.Add(this.txtCircle);
            this.gbArea.Controls.Add(this.txtRegion);
            this.gbArea.Controls.Add(this.btnDivision);
            this.gbArea.Controls.Add(this.btnCircle);
            this.gbArea.Controls.Add(this.btnRegion);
            this.gbArea.Controls.Add(this.lblDivision);
            this.gbArea.Controls.Add(this.lblCircle);
            this.gbArea.Controls.Add(this.lblRegion);
            this.gbArea.Location = new System.Drawing.Point(27, 20);
            this.gbArea.Name = "gbArea";
            this.gbArea.Size = new System.Drawing.Size(472, 101);
            this.gbArea.TabIndex = 26;
            this.gbArea.TabStop = false;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(114, 71);
            this.txtDivision.MaxLength = 40;
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(251, 20);
            this.txtDivision.TabIndex = 4;
            // 
            // txtCircle
            // 
            this.txtCircle.Location = new System.Drawing.Point(114, 45);
            this.txtCircle.MaxLength = 40;
            this.txtCircle.Name = "txtCircle";
            this.txtCircle.Size = new System.Drawing.Size(251, 20);
            this.txtCircle.TabIndex = 2;
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(114, 17);
            this.txtRegion.MaxLength = 40;
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(251, 20);
            this.txtRegion.TabIndex = 0;
            // 
            // btnDivision
            // 
            this.btnDivision.Location = new System.Drawing.Point(371, 70);
            this.btnDivision.Name = "btnDivision";
            this.btnDivision.Size = new System.Drawing.Size(89, 23);
            this.btnDivision.TabIndex = 5;
            this.btnDivision.Text = "&Pick";
            this.btnDivision.TranslationKey = "B000013";
            this.btnDivision.UseVisualStyleBackColor = true;
            this.btnDivision.Click += new System.EventHandler(this.btnDivision_Click);
            // 
            // btnCircle
            // 
            this.btnCircle.Location = new System.Drawing.Point(371, 43);
            this.btnCircle.Name = "btnCircle";
            this.btnCircle.Size = new System.Drawing.Size(89, 23);
            this.btnCircle.TabIndex = 3;
            this.btnCircle.Text = "&Pick";
            this.btnCircle.TranslationKey = "B000013";
            this.btnCircle.UseVisualStyleBackColor = true;
            this.btnCircle.Click += new System.EventHandler(this.btnCircle_Click);
            // 
            // btnRegion
            // 
            this.btnRegion.Location = new System.Drawing.Point(371, 16);
            this.btnRegion.Name = "btnRegion";
            this.btnRegion.Size = new System.Drawing.Size(89, 23);
            this.btnRegion.TabIndex = 1;
            this.btnRegion.Text = "&Pick";
            this.btnRegion.TranslationKey = "B000013";
            this.btnRegion.UseVisualStyleBackColor = true;
            this.btnRegion.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // lblDivision
            // 
            this.lblDivision.AutoSize = true;
            this.lblDivision.Location = new System.Drawing.Point(30, 74);
            this.lblDivision.Name = "lblDivision";
            this.lblDivision.Size = new System.Drawing.Size(75, 13);
            this.lblDivision.TabIndex = 11;
            this.lblDivision.Text = "Division Name";
            this.lblDivision.TranslationKey = "L000043";
            // 
            // lblCircle
            // 
            this.lblCircle.AutoSize = true;
            this.lblCircle.Location = new System.Drawing.Point(30, 47);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(64, 13);
            this.lblCircle.TabIndex = 10;
            this.lblCircle.Text = "Circle Name";
            this.lblCircle.TranslationKey = "L000042";
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(30, 20);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(72, 13);
            this.lblRegion.TabIndex = 9;
            this.lblRegion.Text = "Region Name";
            this.lblRegion.TranslationKey = "L000041";
            // 
            // AreaMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 507);
            this.Controls.Add(this.pnAddArea);
            this.Name = "AreaMaster";
            this.StatusMessage = "";
            this.Text = "Area Definition";
            this.Load += new System.EventHandler(this.AreaMaster_Load);
            this.Activated += new System.EventHandler(this.AreaMaster_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AreaMaster_FormClosing);
            this.pnAddArea.ResumeLayout(false);
            this.pnAreaMaster.ResumeLayout(false);
            this.pnNewAreaDefinition.ResumeLayout(false);
            this.GrpAddNew.ResumeLayout(false);
            this.GrpAddNew.PerformLayout();
            this.gbCMRI.ResumeLayout(false);
            this.gbCMRI.PerformLayout();
            this.gbArea.ResumeLayout(false);
            this.gbArea.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnAddArea;
        private System.Windows.Forms.Panel pnAreaMaster;
        private System.Windows.Forms.Panel pnNewAreaDefinition;
        private System.Windows.Forms.GroupBox GrpAddNew;
        private System.Windows.Forms.ListBox listBoxSelectedMeters;
        private System.Windows.Forms.ListBox listBoxAvailableMeters;
        private CAB.UI.Controls.CABLabel lblSelectedMeters;
        private CAB.UI.Controls.CABLabel lblAvailableMeters;
        private CAB.UI.Controls.CABButton btnCancel;
        private CAB.UI.Controls.CABButton btnSave;
        private CAB.UI.Controls.CABButton btnMoveBackAll;
        private CAB.UI.Controls.CABButton btnMoveBack;
        private CAB.UI.Controls.CABButton btnMoveNextAll;
        private CAB.UI.Controls.CABButton btnMoveNext;
        private System.Windows.Forms.GroupBox gbCMRI;
        private System.Windows.Forms.ComboBox cboMRI;
        private CAB.UI.Controls.CABLabel lblMRI;
        private System.Windows.Forms.GroupBox gbArea;
        public System.Windows.Forms.TextBox txtDivision;
        public System.Windows.Forms.TextBox txtCircle;
        public System.Windows.Forms.TextBox txtRegion;
        private CAB.UI.Controls.CABButton btnDivision;
        private CAB.UI.Controls.CABButton btnCircle;
        private CAB.UI.Controls.CABButton btnRegion;
        private CAB.UI.Controls.CABLabel lblDivision;
        private CAB.UI.Controls.CABLabel lblCircle;
        private CAB.UI.Controls.CABLabel lblRegion;
        private CAB.UI.Controls.CABGridControl gridAreaMaster;
        private CAB.UI.Controls.CABSearchControl ucSearchControl;
    }
}