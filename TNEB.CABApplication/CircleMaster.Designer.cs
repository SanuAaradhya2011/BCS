namespace CAB.UI
{
    partial class CircleMaster
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
            this.pCircle = new System.Windows.Forms.Panel();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.txtCircle = new System.Windows.Forms.TextBox();
            this.lnglCircle = new CAB.UI.Controls.CABLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lgcCircle = new CAB.UI.Controls.CABGridControl();
            this.pCircle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pCircle
            // 
            this.pCircle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pCircle.Controls.Add(this.lngbCancel);
            this.pCircle.Controls.Add(this.lngbSave);
            this.pCircle.Controls.Add(this.txtCircle);
            this.pCircle.Controls.Add(this.lnglCircle);
            this.pCircle.Location = new System.Drawing.Point(15, 41);
            this.pCircle.Name = "pCircle";
            this.pCircle.Size = new System.Drawing.Size(500, 126);
            this.pCircle.TabIndex = 6;
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(402, 81);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(75, 23);
            this.lngbCancel.TabIndex = 3;
            this.lngbCancel.Text = "&Cancel";
            this.lngbCancel.TranslationKey = "B000009";
            this.lngbCancel.UseVisualStyleBackColor = true;
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
            this.lngbSave.UseVisualStyleBackColor = true;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // txtCircle
            // 
            this.txtCircle.Location = new System.Drawing.Point(100, 23);
            this.txtCircle.MaxLength = 50;
            this.txtCircle.Name = "txtCircle";
            this.txtCircle.Size = new System.Drawing.Size(377, 20);
            this.txtCircle.TabIndex = 1;
            this.txtCircle.TextChanged += new System.EventHandler(this.txtCircle_TextChanged);
            // 
            // lnglCircle
            // 
            this.lnglCircle.AutoSize = true;
            this.lnglCircle.Location = new System.Drawing.Point(21, 23);
            this.lnglCircle.Name = "lnglCircle";
            this.lnglCircle.Size = new System.Drawing.Size(64, 13);
            this.lnglCircle.TabIndex = 0;
            this.lnglCircle.Text = "Circle Name";
            this.lnglCircle.TranslationKey = "L000042";
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
            this.lngbPick.Location = new System.Drawing.Point(418, 396);
            this.lngbPick.Name = "lngbPick";
            this.lngbPick.Size = new System.Drawing.Size(75, 23);
            this.lngbPick.TabIndex = 4;
            this.lngbPick.Text = "&Pick";
            this.lngbPick.TranslationKey = "B000013";
            this.lngbPick.UseVisualStyleBackColor = true;
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
            this.lngbDelete.UseVisualStyleBackColor = true;
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
            this.lngbEdit.UseVisualStyleBackColor = true;
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
            this.lngbAdd.UseVisualStyleBackColor = true;
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
            this.lgcCircle.Location = new System.Drawing.Point(15, 41);
            this.lgcCircle.Name = "lgcCircle";
            this.lgcCircle.SelectedIndex = 0;
            this.lgcCircle.Size = new System.Drawing.Size(500, 349);
            this.lgcCircle.TabIndex = 0;
            this.lgcCircle.ValueColumn = null;
            this.lgcCircle.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lgcCircle_OnGridRowChanged);
            this.lgcCircle.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lgcCircle_OnGridMouseDoubleClick);
            // 
            // CircleMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 431);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pCircle);
            this.Controls.Add(this.lngbPick);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lgcCircle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CircleMaster";
            this.Text = "Region Master";
            this.Load += new System.EventHandler(this.CircleMaster_Load);
            this.pCircle.ResumeLayout(false);
            this.pCircle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABButton lngbDelete;
        private CAB.UI.Controls.CABButton lngbEdit;
        private CAB.UI.Controls.CABButton lngbAdd;
        private CAB.UI.Controls.CABLabel lnglCircle;
        private System.Windows.Forms.Panel pCircle;
        private CAB.UI.Controls.CABGridControl lgcCircle;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtCircle;
    }
}