namespace CAB.UI
{
    partial class DivisionManager
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
            this.label2 = new System.Windows.Forms.Label();
            this.lgcDivision = new CAB.UI.Controls.CABGridControl();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.ddlCircle = new System.Windows.Forms.ComboBox();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.ddlRegion = new System.Windows.Forms.ComboBox();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.lngBFormClose = new CAB.UI.Controls.CABButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.lnglDivision = new CAB.UI.Controls.CABLabel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Circle Name";
            // 
            // lgcDivision
            // 
            this.lgcDivision.Data = null;
            this.lgcDivision.HiddenColumn = null;
            this.lgcDivision.HiddenColumns = null;
            this.lgcDivision.IsEqual = false;
            this.lgcDivision.IsFullRow = false;
            this.lgcDivision.IsSorting = false;
            this.lgcDivision.Location = new System.Drawing.Point(16, 58);
            this.lgcDivision.Name = "lgcDivision";
            this.lgcDivision.SelectedIndex = 0;
            this.lgcDivision.Size = new System.Drawing.Size(500, 329);
            this.lgcDivision.TabIndex = 8;
            this.lgcDivision.ValueColumn = null;
            // 
            // lngbDelete
            // 
            this.lngbDelete.Location = new System.Drawing.Point(178, 13);
            this.lngbDelete.Name = "lngbDelete";
            this.lngbDelete.Size = new System.Drawing.Size(75, 23);
            this.lngbDelete.TabIndex = 11;
            this.lngbDelete.Text = "&Delete";
            this.lngbDelete.TranslationKey = "B000005";
            this.lngbDelete.UseVisualStyleBackColor = false;
            this.lngbDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbDelete.ForeColor = System.Drawing.Color.White;
            this.lngbDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbDelete.FlatAppearance.BorderSize = 0;
            this.lngbDelete.Click += new System.EventHandler(this.lngbDelete_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Region Name";
            // 
            // lngbEdit
            // 
            this.lngbEdit.Location = new System.Drawing.Point(97, 13);
            this.lngbEdit.Name = "lngbEdit";
            this.lngbEdit.Size = new System.Drawing.Size(75, 23);
            this.lngbEdit.TabIndex = 10;
            this.lngbEdit.Text = "&Edit";
            this.lngbEdit.TranslationKey = "B000004";
            this.lngbEdit.UseVisualStyleBackColor = false;
            this.lngbEdit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbEdit.ForeColor = System.Drawing.Color.White;
            this.lngbEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbEdit.FlatAppearance.BorderSize = 0;
            this.lngbEdit.Click += new System.EventHandler(this.lngbEdit_Click);
            // 
            // ddlCircle
            // 
            this.ddlCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCircle.FormattingEnabled = true;
            this.ddlCircle.Location = new System.Drawing.Point(127, 62);
            this.ddlCircle.Name = "ddlCircle";
            this.ddlCircle.Size = new System.Drawing.Size(359, 21);
            this.ddlCircle.TabIndex = 5;
            // 
            // lngbAdd
            // 
            this.lngbAdd.Location = new System.Drawing.Point(16, 13);
            this.lngbAdd.Name = "lngbAdd";
            this.lngbAdd.Size = new System.Drawing.Size(75, 23);
            this.lngbAdd.TabIndex = 9;
            this.lngbAdd.Text = "&Add";
            this.lngbAdd.TranslationKey = "B000003";
            this.lngbAdd.UseVisualStyleBackColor = false;
            this.lngbAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbAdd.ForeColor = System.Drawing.Color.White;
            this.lngbAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbAdd.FlatAppearance.BorderSize = 0;
            this.lngbAdd.Click += new System.EventHandler(this.lngbAdd_Click);
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(411, 141);
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
            // lngbPick
            // 
            this.lngbPick.Location = new System.Drawing.Point(419, 397);
            this.lngbPick.Name = "lngbPick";
            this.lngbPick.Size = new System.Drawing.Size(75, 23);
            this.lngbPick.TabIndex = 12;
            this.lngbPick.Text = "&Pick";
            this.lngbPick.TranslationKey = "B000013";
            this.lngbPick.UseVisualStyleBackColor = false;
            this.lngbPick.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbPick.ForeColor = System.Drawing.Color.White;
            this.lngbPick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbPick.FlatAppearance.BorderSize = 0;
            this.lngbPick.Click += new System.EventHandler(this.lngbPick_Click);
            // 
            // ddlRegion
            // 
            this.ddlRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRegion.FormattingEnabled = true;
            this.ddlRegion.Location = new System.Drawing.Point(127, 22);
            this.ddlRegion.Name = "ddlRegion";
            this.ddlRegion.Size = new System.Drawing.Size(359, 21);
            this.ddlRegion.TabIndex = 4;
            this.ddlRegion.SelectedIndexChanged += new System.EventHandler(this.ddlRegion_SelectedIndexChanged);
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(317, 141);
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
            // lngBFormClose
            // 
            this.lngBFormClose.Location = new System.Drawing.Point(259, 13);
            this.lngBFormClose.Name = "lngBFormClose";
            this.lngBFormClose.Size = new System.Drawing.Size(75, 23);
            this.lngBFormClose.TabIndex = 15;
            this.lngBFormClose.Text = "Cancel";
            this.lngBFormClose.TranslationKey = null;
            this.lngBFormClose.UseVisualStyleBackColor = false;
            this.lngBFormClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngBFormClose.ForeColor = System.Drawing.Color.White;
            this.lngBFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngBFormClose.FlatAppearance.BorderSize = 0;
            this.lngBFormClose.Click += new System.EventHandler(this.lngBFormClose_Click_1);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(85, 407);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(27, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "*";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(26, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(27, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "*";
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(127, 104);
            this.txtDivision.MaxLength = 50;
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(359, 20);
            this.txtDivision.TabIndex = 1;
            // 
            // lnglDivision
            // 
            this.lnglDivision.AutoSize = true;
            this.lnglDivision.Location = new System.Drawing.Point(35, 107);
            this.lnglDivision.Name = "lnglDivision";
            this.lnglDivision.Size = new System.Drawing.Size(75, 13);
            this.lnglDivision.TabIndex = 0;
            this.lnglDivision.Text = "Division Name";
            this.lnglDivision.TranslationKey = "L000043";
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label4);
            this.groupBox.Controls.Add(this.label5);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Controls.Add(this.lngbCancel);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.lngbSave);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.txtDivision);
            this.groupBox.Controls.Add(this.ddlCircle);
            this.groupBox.Controls.Add(this.ddlRegion);
            this.groupBox.Controls.Add(this.lnglDivision);
            this.groupBox.Location = new System.Drawing.Point(16, 58);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(500, 180);
            this.groupBox.TabIndex = 16;
            this.groupBox.TabStop = false;
            // 
            // DivisionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 429);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.lgcDivision);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lngbPick);
            this.Controls.Add(this.lngBFormClose);
            this.Controls.Add(this.lblStatus);
            this.Name = "DivisionManager";
            this.StatusMessage = "";
            this.Text = "Manage Division";
            this.Load += new System.EventHandler(this.DivisionManager_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private CAB.UI.Controls.CABGridControl lgcDivision;
        private CAB.UI.Controls.CABButton lngbDelete;
        private System.Windows.Forms.Label label1;
        private CAB.UI.Controls.CABButton lngbEdit;
        private System.Windows.Forms.ComboBox ddlCircle;
        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.ComboBox ddlRegion;
        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngBFormClose;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtDivision;
        private CAB.UI.Controls.CABLabel lnglDivision;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox;
    }
}
