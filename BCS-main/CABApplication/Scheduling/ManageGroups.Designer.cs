namespace CAB.UI
{
    partial class ManageGroups
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.lngAddButton = new CAB.UI.Controls.CABButton();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new CAB.UI.Controls.CABButton();
            this.grdManageGroups = new CAB.UI.Controls.CABGridControl();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.grdManageGroups);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 416);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Groups";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.lngAddButton);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Location = new System.Drawing.Point(14, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(555, 54);
            this.panel1.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(227, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TranslationKey = null;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lngAddButton
            // 
            this.lngAddButton.Location = new System.Drawing.Point(9, 15);
            this.lngAddButton.Name = "lngAddButton";
            this.lngAddButton.Size = new System.Drawing.Size(75, 23);
            this.lngAddButton.TabIndex = 2;
            this.lngAddButton.Text = "Add";
            this.lngAddButton.TranslationKey = null;
            this.lngAddButton.UseVisualStyleBackColor = false;
            this.lngAddButton.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngAddButton.ForeColor = System.Drawing.Color.White;
            this.lngAddButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngAddButton.FlatAppearance.BorderSize = 0;
            this.lngAddButton.Click += new System.EventHandler(this.lngAddButton_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(81, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(155, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TranslationKey = null;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grdManageGroups
            // 
            this.grdManageGroups.AutoScroll = true;
            this.grdManageGroups.Data = null;
            this.grdManageGroups.HiddenColumn = null;
            this.grdManageGroups.HiddenColumns = null;
            this.grdManageGroups.IsEqual = false;
            this.grdManageGroups.IsFullRow = false;
            this.grdManageGroups.IsSorting = false;
            this.grdManageGroups.Location = new System.Drawing.Point(14, 84);
            this.grdManageGroups.Name = "grdManageGroups";
            this.grdManageGroups.SelectedIndex = 0;
            this.grdManageGroups.SelectedRowId = "";
            this.grdManageGroups.Size = new System.Drawing.Size(555, 320);
            this.grdManageGroups.TabIndex = 4;
            this.grdManageGroups.ValueColumn = null;
            // 
            // ManageGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 439);
            this.Controls.Add(this.groupBox1);
            this.Name = "ManageGroups";
            this.StatusMessage = "";
            this.Text = "Group Manager";
            this.Load += new System.EventHandler(this.ManageGroups_Load);
            this.Activated += new System.EventHandler(this.ManageGroups_Activated);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private CAB.UI.Controls.CABButton lngAddButton;
        private System.Windows.Forms.Button btnEdit;
        private CAB.UI.Controls.CABButton btnDelete;
        private CAB.UI.Controls.CABGridControl grdManageGroups;
        private CAB.UI.Controls.CABButton btnCancel;

    }
}
