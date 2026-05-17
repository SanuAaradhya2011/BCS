
namespace CAB.UI.Controls
{
    partial class DesignationMaster
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
			this.txtCategory = new System.Windows.Forms.TextBox();
			this.pDivision = new System.Windows.Forms.Panel();
			this.lngbCancel = new CAB.UI.Controls.CABButton();
			this.lngbSave = new CAB.UI.Controls.CABButton();
			this.lnglDivision = new CAB.UI.Controls.CABLabel();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lngbPick = new CAB.UI.Controls.CABButton();
			this.lngbDelete = new CAB.UI.Controls.CABButton();
			this.lngbEdit = new CAB.UI.Controls.CABButton();
			this.lngbAdd = new CAB.UI.Controls.CABButton();
			this.lgcDesignation = new CAB.UI.Controls.CABGridControl();
			this.lngBtnPick = new CAB.UI.Controls.CABButton();
			this.pDivision.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtCategory
			// 
			this.txtCategory.Location = new System.Drawing.Point(118, 27);
			this.txtCategory.MaxLength = 50;
			this.txtCategory.Name = "txtCategory";
			this.txtCategory.Size = new System.Drawing.Size(359, 20);
			this.txtCategory.TabIndex = 1;
			this.txtCategory.TextChanged += new System.EventHandler(this.txtCategory_TextChanged);
			// 
			// pDivision
			// 
			this.pDivision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pDivision.Controls.Add(this.lngbCancel);
			this.pDivision.Controls.Add(this.lngbSave);
			this.pDivision.Controls.Add(this.txtCategory);
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
			this.lnglDivision.Location = new System.Drawing.Point(18, 30);
			this.lnglDivision.Name = "lnglDivision";
			this.lnglDivision.Size = new System.Drawing.Size(94, 13);
			this.lnglDivision.TabIndex = 0;
			this.lnglDivision.Text = "Designation Name";
			this.lnglDivision.TranslationKey = "L000050";
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
			this.lngbPick.Location = new System.Drawing.Point(440, 12);
			this.lngbPick.Name = "lngbPick";
			this.lngbPick.Size = new System.Drawing.Size(75, 23);
			this.lngbPick.TabIndex = 4;
			this.lngbPick.Text = "&Close";
			this.lngbPick.TranslationKey = "B000014";
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
			// lgcDesignation
			// 
			this.lgcDesignation.Data = null;
			this.lgcDesignation.HiddenColumn = null;
			this.lgcDesignation.HiddenColumns = null;
			this.lgcDesignation.IsEqual = false;
			this.lgcDesignation.IsFullRow = false;
			this.lgcDesignation.IsSorting = false;
			this.lgcDesignation.Location = new System.Drawing.Point(15, 41);
			this.lgcDesignation.Name = "lgcDesignation";
			this.lgcDesignation.SelectedIndex = 0;
			this.lgcDesignation.Size = new System.Drawing.Size(500, 349);
			this.lgcDesignation.TabIndex = 0;
			this.lgcDesignation.ValueColumn = null;
			this.lgcDesignation.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lgcCategory_OnGridRowChanged);
			this.lgcDesignation.OnGridMouseDoubleClick += new CAB.UI.Controls.CABGridControl.CABGridMouseDoubleClick(this.lgcDesignation_OnGridMouseDoubleClick);
			// 
			// lngBtnPick
			// 
			this.lngBtnPick.Location = new System.Drawing.Point(441, 393);
			this.lngBtnPick.Name = "lngBtnPick";
			this.lngBtnPick.Size = new System.Drawing.Size(75, 23);
			this.lngBtnPick.TabIndex = 7;
			this.lngBtnPick.Text = "&Pick";
			this.lngBtnPick.TranslationKey = "B000013";
			this.lngBtnPick.UseVisualStyleBackColor = true;
			this.lngBtnPick.Click += new System.EventHandler(this.lngBtnPick_Click);
			// 
			// DesignationMaster
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(527, 431);
			this.Controls.Add(this.lngBtnPick);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.pDivision);
			this.Controls.Add(this.lngbPick);
			this.Controls.Add(this.lngbDelete);
			this.Controls.Add(this.lngbEdit);
			this.Controls.Add(this.lngbAdd);
			this.Controls.Add(this.lgcDesignation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DesignationMaster";
			this.Text = "Designation Master";
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
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Panel pDivision;
        private CAB.UI.Controls.CABGridControl lgcDesignation;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngbPick;
        private System.Windows.Forms.Label lblStatus;
        private CABButton lngBtnPick;
    }
}