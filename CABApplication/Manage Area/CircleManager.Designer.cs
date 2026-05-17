namespace CAB.UI
{
    partial class CircleManager
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
            this.label1 = new System.Windows.Forms.Label();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.ddlRegion = new System.Windows.Forms.ComboBox();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.txtCircle = new System.Windows.Forms.TextBox();
            this.lnglCircle = new CAB.UI.Controls.CABLabel();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lgcCircle = new CAB.UI.Controls.CABGridControl();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbFormClose = new CAB.UI.Controls.CABButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "*";
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(22, 28);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(72, 13);
            this.lngLabel1.TabIndex = 5;
            this.lngLabel1.Text = "Region Name";
            this.lngLabel1.TranslationKey = null;
            // 
            // ddlRegion
            // 
            this.ddlRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRegion.FormattingEnabled = true;
            this.ddlRegion.Location = new System.Drawing.Point(100, 25);
            this.ddlRegion.Name = "ddlRegion";
            this.ddlRegion.Size = new System.Drawing.Size(377, 21);
            this.ddlRegion.TabIndex = 4;
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(402, 99);
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
            this.lngbSave.Location = new System.Drawing.Point(308, 99);
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
            // txtCircle
            // 
            this.txtCircle.Location = new System.Drawing.Point(100, 68);
            this.txtCircle.MaxLength = 50;
            this.txtCircle.Name = "txtCircle";
            this.txtCircle.Size = new System.Drawing.Size(377, 20);
            this.txtCircle.TabIndex = 1;
            // 
            // lnglCircle
            // 
            this.lnglCircle.AutoSize = true;
            this.lnglCircle.Location = new System.Drawing.Point(22, 71);
            this.lnglCircle.Name = "lnglCircle";
            this.lnglCircle.Size = new System.Drawing.Size(64, 13);
            this.lnglCircle.TabIndex = 0;
            this.lnglCircle.Text = "Circle Name";
            this.lnglCircle.TranslationKey = "L000042";
            // 
            // lngbAdd
            // 
            this.lngbAdd.Location = new System.Drawing.Point(11, 7);
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
            // lgcCircle
            // 
            this.lgcCircle.Data = null;
            this.lgcCircle.HiddenColumn = null;
            this.lgcCircle.HiddenColumns = null;
            this.lgcCircle.IsEqual = false;
            this.lgcCircle.IsFullRow = false;
            this.lgcCircle.IsSorting = false;
            this.lgcCircle.Location = new System.Drawing.Point(11, 36);
            this.lgcCircle.Name = "lgcCircle";
            this.lgcCircle.SelectedIndex = 0;
            this.lgcCircle.Size = new System.Drawing.Size(500, 349);
            this.lgcCircle.TabIndex = 8;
            this.lgcCircle.ValueColumn = null;
            // 
            // lngbEdit
            // 
            this.lngbEdit.Location = new System.Drawing.Point(92, 7);
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
            // lngbDelete
            // 
            this.lngbDelete.Location = new System.Drawing.Point(173, 7);
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
            // lngbFormClose
            // 
            this.lngbFormClose.Location = new System.Drawing.Point(254, 7);
            this.lngbFormClose.Name = "lngbFormClose";
            this.lngbFormClose.Size = new System.Drawing.Size(75, 23);
            this.lngbFormClose.TabIndex = 15;
            this.lngbFormClose.Text = "Cancel";
            this.lngbFormClose.TranslationKey = null;
            this.lngbFormClose.UseVisualStyleBackColor = false;
            this.lngbFormClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbFormClose.ForeColor = System.Drawing.Color.White;
            this.lngbFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbFormClose.FlatAppearance.BorderSize = 0;
            this.lngbFormClose.Click += new System.EventHandler(this.lngbFormClose_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(80, 401);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 13;
            // 
            // lngbPick
            // 
            this.lngbPick.Location = new System.Drawing.Point(414, 391);
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
            // groupBox
            // 
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.lngLabel1);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.lnglCircle);
            this.groupBox.Controls.Add(this.txtCircle);
            this.groupBox.Controls.Add(this.ddlRegion);
            this.groupBox.Controls.Add(this.lngbSave);
            this.groupBox.Controls.Add(this.lngbCancel);
            this.groupBox.Location = new System.Drawing.Point(12, 36);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(498, 141);
            this.groupBox.TabIndex = 16;
            this.groupBox.TabStop = false;
            // 
            // CircleManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 420);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lgcCircle);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbFormClose);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lngbPick);
            this.Name = "CircleManager";
            this.StatusMessage = "";
            this.Text = "Manage Circle";
            this.Load += new System.EventHandler(this.CircleManager_Load);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.ComboBox ddlRegion;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private System.Windows.Forms.TextBox txtCircle;
        private CAB.UI.Controls.CABLabel lnglCircle;
        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABGridControl lgcCircle;
        private CAB.UI.Controls.CABButton lngbEdit;
        private CAB.UI.Controls.CABButton lngbDelete;
        private CAB.UI.Controls.CABButton lngbFormClose;
        private System.Windows.Forms.Label lblStatus;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox;

    }
}
