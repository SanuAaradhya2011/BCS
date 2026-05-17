namespace CAB.UI
{
    partial class RegionMaster
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
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.pRegion = new System.Windows.Forms.Panel();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.lngRegion = new CAB.UI.Controls.CABLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lgcRegion = new CAB.UI.Controls.CABGridControl();
            this.lngBFormClose = new CAB.UI.Controls.CABButton();
            this.pRegion.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(113, 23);
            this.txtRegion.MaxLength = 50;
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(364, 20);
            this.txtRegion.TabIndex = 1;
            this.txtRegion.TextChanged += new System.EventHandler(this.txtRegion_TextChanged);
            // 
            // pRegion
            // 
            this.pRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pRegion.Controls.Add(this.lngbCancel);
            this.pRegion.Controls.Add(this.lngbSave);
            this.pRegion.Controls.Add(this.txtRegion);
            this.pRegion.Controls.Add(this.lngRegion);
            this.pRegion.Location = new System.Drawing.Point(15, 41);
            this.pRegion.Name = "pRegion";
            this.pRegion.Size = new System.Drawing.Size(500, 126);
            this.pRegion.TabIndex = 6;
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(402, 81);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(75, 23);
            this.lngbCancel.TabIndex = 3;
            this.lngbCancel.Text = "&Cancel";
            this.lngbCancel.TranslationKey = "B000009";
            this.lngbCancel.UseVisualStyleBackColor = false;
            this.lngbCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbCancel.ForeColor = System.Drawing.Color.White;
            this.lngbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbCancel.FlatAppearance.BorderSize = 0;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(308, 81);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(75, 23);
            this.lngbSave.TabIndex = 2;
            this.lngbSave.Text = "&Save";
            this.lngbSave.TranslationKey = "B000008";
            this.lngbSave.UseVisualStyleBackColor = false;
            this.lngbSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbSave.ForeColor = System.Drawing.Color.White;
            this.lngbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSave.FlatAppearance.BorderSize = 0;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // lngRegion
            // 
            this.lngRegion.AutoSize = true;
            this.lngRegion.Location = new System.Drawing.Point(23, 30);
            this.lngRegion.Name = "lngRegion";
            this.lngRegion.Size = new System.Drawing.Size(72, 13);
            this.lngRegion.TabIndex = 0;
            this.lngRegion.Text = "Region Name";
            this.lngRegion.TranslationKey = "L000041";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(84, 406);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 5;
            // 
            // lngbPick
            // 
            this.lngbPick.Location = new System.Drawing.Point(359, 396);
            this.lngbPick.Name = "lngbPick";
            this.lngbPick.Size = new System.Drawing.Size(75, 23);
            this.lngbPick.TabIndex = 4;
            this.lngbPick.Text = "&Pick";
            this.lngbPick.TranslationKey = "B000013";
            this.lngbPick.UseVisualStyleBackColor = false;
            this.lngbPick.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbPick.ForeColor = System.Drawing.Color.White;
            this.lngbPick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbPick.FlatAppearance.BorderSize = 0;
            this.lngbPick.Click += new System.EventHandler(this.lngbPick_Click);
            // 
            // lngbDelete
            // 
            this.lngbDelete.Location = new System.Drawing.Point(177, 12);
            this.lngbDelete.Name = "lngbDelete";
            this.lngbDelete.Size = new System.Drawing.Size(75, 23);
            this.lngbDelete.TabIndex = 3;
            this.lngbDelete.Text = "&Delete";
            this.lngbDelete.TranslationKey = "B000005";
            this.lngbDelete.UseVisualStyleBackColor = false;
            this.lngbDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbDelete.ForeColor = System.Drawing.Color.White;
            this.lngbDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbDelete.FlatAppearance.BorderSize = 0;
            this.lngbDelete.Visible = false;
            this.lngbDelete.Click += new System.EventHandler(this.lngbDelete_Click);
            // 
            // lngbEdit
            // 
            this.lngbEdit.Location = new System.Drawing.Point(96, 12);
            this.lngbEdit.Name = "lngbEdit";
            this.lngbEdit.Size = new System.Drawing.Size(75, 23);
            this.lngbEdit.TabIndex = 2;
            this.lngbEdit.Text = "&Edit";
            this.lngbEdit.TranslationKey = "B000004";
            this.lngbEdit.UseVisualStyleBackColor = false;
            this.lngbEdit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbEdit.ForeColor = System.Drawing.Color.White;
            this.lngbEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbEdit.FlatAppearance.BorderSize = 0;
            this.lngbEdit.Visible = false;
            this.lngbEdit.Click += new System.EventHandler(this.lngbEdit_Click);
            // 
            // lngbAdd
            // 
            this.lngbAdd.Location = new System.Drawing.Point(15, 12);
            this.lngbAdd.Name = "lngbAdd";
            this.lngbAdd.Size = new System.Drawing.Size(75, 23);
            this.lngbAdd.TabIndex = 1;
            this.lngbAdd.Text = "&Add";
            this.lngbAdd.TranslationKey = "B000003";
            this.lngbAdd.UseVisualStyleBackColor = false;
            this.lngbAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbAdd.ForeColor = System.Drawing.Color.White;
            this.lngbAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbAdd.FlatAppearance.BorderSize = 0;
            this.lngbAdd.Visible = false;
            this.lngbAdd.Click += new System.EventHandler(this.lngbAdd_Click);
            // 
            // lgcRegion
            // 
            this.lgcRegion.Data = null;
            this.lgcRegion.HiddenColumn = null;
            this.lgcRegion.HiddenColumns = null;
            this.lgcRegion.IsEqual = false;
            this.lgcRegion.IsFullRow = false;
            this.lgcRegion.IsSorting = false;
            this.lgcRegion.Location = new System.Drawing.Point(15, 41);
            this.lgcRegion.Name = "lgcRegion";
            this.lgcRegion.SelectedIndex = 0;
            this.lgcRegion.Size = new System.Drawing.Size(500, 349);
            this.lgcRegion.TabIndex = 0;
            this.lgcRegion.ValueColumn = null;
            this.lgcRegion.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lgcRegion_OnGridRowChanged);
            this.lgcRegion.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lgcRegion_OnGridMouseDoubleClick);
            // 
            // lngBFormClose
            // 
            this.lngBFormClose.Location = new System.Drawing.Point(440, 396);
            this.lngBFormClose.Name = "lngBFormClose";
            this.lngBFormClose.Size = new System.Drawing.Size(75, 23);
            this.lngBFormClose.TabIndex = 7;
            this.lngBFormClose.Text = "Cancel";
            this.lngBFormClose.TranslationKey = null;
            this.lngBFormClose.UseVisualStyleBackColor = false;
            this.lngBFormClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngBFormClose.ForeColor = System.Drawing.Color.White;
            this.lngBFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngBFormClose.FlatAppearance.BorderSize = 0;
            this.lngBFormClose.Click += new System.EventHandler(this.lngBFormClose_Click);
            // 
            // RegionMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 431);
            this.Controls.Add(this.lngBFormClose);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pRegion);
            this.Controls.Add(this.lngbPick);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lgcRegion);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegionMaster";
            this.StatusMessage = "";
            this.Text = "Region Master";
            this.Load += new System.EventHandler(this.RegionMaster_Load);
            this.pRegion.ResumeLayout(false);
            this.pRegion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABButton lngbDelete;
        private CAB.UI.Controls.CABButton lngbEdit;
        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABLabel lngRegion;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Panel pRegion;
        private CAB.UI.Controls.CABGridControl lgcRegion;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.Label lblStatus;
        private CAB.UI.Controls.CABButton lngBFormClose;
    }
}
