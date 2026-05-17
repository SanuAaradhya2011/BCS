
namespace CAB.UI
{
    partial class DivisionMaster
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
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.pDivision = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlCircle = new System.Windows.Forms.ComboBox();
            this.ddlRegion = new System.Windows.Forms.ComboBox();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.lnglDivision = new CAB.UI.Controls.CABLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lgcDivision = new CAB.UI.Controls.CABGridControl();
            this.lngBFormClose = new CAB.UI.Controls.CABButton();
            this.pDivision.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(118, 107);
            this.txtDivision.MaxLength = 50;
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(359, 20);
            this.txtDivision.TabIndex = 1;
            this.txtDivision.TextChanged += new System.EventHandler(this.txtDivision_TextChanged);
            // 
            // pDivision
            // 
            this.pDivision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pDivision.Controls.Add(this.label2);
            this.pDivision.Controls.Add(this.label1);
            this.pDivision.Controls.Add(this.ddlCircle);
            this.pDivision.Controls.Add(this.ddlRegion);
            this.pDivision.Controls.Add(this.lngbCancel);
            this.pDivision.Controls.Add(this.lngbSave);
            this.pDivision.Controls.Add(this.txtDivision);
            this.pDivision.Controls.Add(this.lnglDivision);
            this.pDivision.Location = new System.Drawing.Point(15, 41);
            this.pDivision.Name = "pDivision";
            this.pDivision.Size = new System.Drawing.Size(500, 195);
            this.pDivision.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Circle Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Region Name";
            // 
            // ddlCircle
            // 
            this.ddlCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCircle.FormattingEnabled = true;
            this.ddlCircle.Location = new System.Drawing.Point(118, 67);
            this.ddlCircle.Name = "ddlCircle";
            this.ddlCircle.Size = new System.Drawing.Size(359, 21);
            this.ddlCircle.TabIndex = 5;
            // 
            // ddlRegion
            // 
            this.ddlRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRegion.FormattingEnabled = true;
            this.ddlRegion.Location = new System.Drawing.Point(118, 27);
            this.ddlRegion.Name = "ddlRegion";
            this.ddlRegion.Size = new System.Drawing.Size(359, 21);
            this.ddlRegion.TabIndex = 4;
            this.ddlRegion.SelectedIndexChanged += new System.EventHandler(this.ddlRegion_SelectedIndexChanged);
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(402, 147);
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
            this.lngbSave.Location = new System.Drawing.Point(308, 147);
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
            // lnglDivision
            // 
            this.lnglDivision.AutoSize = true;
            this.lnglDivision.Location = new System.Drawing.Point(26, 110);
            this.lnglDivision.Name = "lnglDivision";
            this.lnglDivision.Size = new System.Drawing.Size(75, 13);
            this.lnglDivision.TabIndex = 0;
            this.lnglDivision.Text = "Division Name";
            this.lnglDivision.TranslationKey = "L000043";
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
            // lgcDivision
            // 
            this.lgcDivision.Data = null;
            this.lgcDivision.HiddenColumn = null;
            this.lgcDivision.HiddenColumns = null;
            this.lgcDivision.IsEqual = false;
            this.lgcDivision.IsFullRow = false;
            this.lgcDivision.IsSorting = false;
            this.lgcDivision.Location = new System.Drawing.Point(15, 41);
            this.lgcDivision.Name = "lgcDivision";
            this.lgcDivision.SelectedIndex = 0;
            this.lgcDivision.Size = new System.Drawing.Size(500, 349);
            this.lgcDivision.TabIndex = 0;
            this.lgcDivision.ValueColumn = null;
            this.lgcDivision.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lgcDivision_OnGridRowChanged);
            this.lgcDivision.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lgcDivision_OnGridMouseDoubleClick);
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
            // DivisionMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 431);
            this.Controls.Add(this.lngBFormClose);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pDivision);
            this.Controls.Add(this.lngbPick);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lgcDivision);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DivisionMaster";
            this.StatusMessage = "";
            this.Text = "Division Master";
            this.Load += new System.EventHandler(this.DivisionMaster_Load);
            this.pDivision.ResumeLayout(false);
            this.pDivision.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABButton lngbDelete;
        private CAB.UI.Controls.CABButton lngbEdit;
        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABLabel lnglDivision;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.Panel pDivision;
        private CAB.UI.Controls.CABGridControl lgcDivision;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox ddlRegion;
        private System.Windows.Forms.ComboBox ddlCircle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CAB.UI.Controls.CABButton lngBFormClose;
    }
}
