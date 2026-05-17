
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
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.lnglDivision = new CAB.UI.Controls.CABLabel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lngbPick = new CAB.UI.Controls.CABButton();
            this.lngbDelete = new CAB.UI.Controls.CABButton();
            this.lngbEdit = new CAB.UI.Controls.CABButton();
            this.lngbAdd = new CAB.UI.Controls.CABButton();
            this.lgcDivision = new CAB.UI.Controls.CABGridControl();
            this.pDivision.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(118, 27);
            this.txtDivision.MaxLength = 50;
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(359, 20);
            this.txtDivision.TabIndex = 1;
            this.txtDivision.TextChanged += new System.EventHandler(this.txtDivision_TextChanged);
            // 
            // pDivision
            // 
            this.pDivision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pDivision.Controls.Add(this.lngbCancel);
            this.pDivision.Controls.Add(this.lngbSave);
            this.pDivision.Controls.Add(this.txtDivision);
            this.pDivision.Controls.Add(this.lnglDivision);
            this.pDivision.Location = new System.Drawing.Point(15, 41);
            this.pDivision.Name = "pDivision";
            this.pDivision.Size = new System.Drawing.Size(500, 126);
            this.pDivision.TabIndex = 6;
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
            // lnglDivision
            // 
            this.lnglDivision.AutoSize = true;
            this.lnglDivision.Location = new System.Drawing.Point(26, 27);
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
            // DivisionMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 431);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pDivision);
            this.Controls.Add(this.lngbPick);
            this.Controls.Add(this.lngbDelete);
            this.Controls.Add(this.lngbEdit);
            this.Controls.Add(this.lngbAdd);
            this.Controls.Add(this.lgcDivision);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DivisionMaster";
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
    }
}