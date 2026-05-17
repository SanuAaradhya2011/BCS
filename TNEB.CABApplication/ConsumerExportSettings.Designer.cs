namespace CAB.UI
{
    partial class ConsumerExportSettings
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
            this.grpAdd = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.btnMoveBackAll = new System.Windows.Forms.Button();
            this.btnMoveBack = new System.Windows.Forms.Button();
            this.btnMoveNextAll = new System.Windows.Forms.Button();
            this.btnMoveNext = new System.Windows.Forms.Button();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grpList = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lstParameter = new System.Windows.Forms.ListBox();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpAdd.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpList.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAdd
            // 
            this.grpAdd.Controls.Add(this.label2);
            this.grpAdd.Controls.Add(this.txtFileName);
            this.grpAdd.Controls.Add(this.label1);
            this.grpAdd.Controls.Add(this.btnSave);
            this.grpAdd.Controls.Add(this.btnCancel);
            this.grpAdd.Controls.Add(this.groupBox2);
            this.grpAdd.Location = new System.Drawing.Point(10, 11);
            this.grpAdd.Name = "grpAdd";
            this.grpAdd.Size = new System.Drawing.Size(589, 404);
            this.grpAdd.TabIndex = 3;
            this.grpAdd.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 359);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Note : All fields are mandatory.";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(90, 32);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFileName.MaxLength = 50;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(150, 20);
            this.txtFileName.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "File Name";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(421, 359);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 32);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(502, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 32);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.listBoxSelectedMeters);
            this.groupBox2.Controls.Add(this.btnMoveBackAll);
            this.groupBox2.Controls.Add(this.btnMoveBack);
            this.groupBox2.Controls.Add(this.btnMoveNextAll);
            this.groupBox2.Controls.Add(this.btnMoveNext);
            this.groupBox2.Controls.Add(this.listBoxAvailableMeters);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Location = new System.Drawing.Point(22, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 275);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select parameters for exporting";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(326, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Selected Parameters";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Available Parameters";
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.HorizontalScrollbar = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(323, 46);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(204, 225);
            this.listBoxSelectedMeters.TabIndex = 5;
            // 
            // btnMoveBackAll
            // 
            this.btnMoveBackAll.Location = new System.Drawing.Point(255, 180);
            this.btnMoveBackAll.Name = "btnMoveBackAll";
            this.btnMoveBackAll.Size = new System.Drawing.Size(44, 29);
            this.btnMoveBackAll.TabIndex = 4;
            this.btnMoveBackAll.Text = "<<";
            this.btnMoveBackAll.UseVisualStyleBackColor = true;
            this.btnMoveBackAll.Click += new System.EventHandler(this.btnMoveBackAll_Click);
            // 
            // btnMoveBack
            // 
            this.btnMoveBack.Location = new System.Drawing.Point(255, 150);
            this.btnMoveBack.Name = "btnMoveBack";
            this.btnMoveBack.Size = new System.Drawing.Size(44, 29);
            this.btnMoveBack.TabIndex = 3;
            this.btnMoveBack.Text = "<";
            this.btnMoveBack.UseVisualStyleBackColor = true;
            this.btnMoveBack.Click += new System.EventHandler(this.btnMoveBack_Click);
            // 
            // btnMoveNextAll
            // 
            this.btnMoveNextAll.Location = new System.Drawing.Point(255, 120);
            this.btnMoveNextAll.Name = "btnMoveNextAll";
            this.btnMoveNextAll.Size = new System.Drawing.Size(44, 29);
            this.btnMoveNextAll.TabIndex = 2;
            this.btnMoveNextAll.Text = ">>";
            this.btnMoveNextAll.UseVisualStyleBackColor = true;
            this.btnMoveNextAll.Click += new System.EventHandler(this.btnMoveNextAll_Click);
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.Location = new System.Drawing.Point(255, 90);
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.Size = new System.Drawing.Size(44, 29);
            this.btnMoveNext.TabIndex = 1;
            this.btnMoveNext.Text = ">";
            this.btnMoveNext.UseVisualStyleBackColor = true;
            this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.HorizontalScrollbar = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(29, 46);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(204, 225);
            this.listBoxAvailableMeters.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel4.Location = new System.Drawing.Point(322, 25);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(204, 20);
            this.panel4.TabIndex = 16;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel3.Location = new System.Drawing.Point(28, 25);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(204, 20);
            this.panel3.TabIndex = 15;
            // 
            // grpList
            // 
            this.grpList.Controls.Add(this.btnEdit);
            this.grpList.Controls.Add(this.btnClose);
            this.grpList.Controls.Add(this.btnDelete);
            this.grpList.Controls.Add(this.label5);
            this.grpList.Controls.Add(this.label6);
            this.grpList.Controls.Add(this.lstParameter);
            this.grpList.Controls.Add(this.lstFile);
            this.grpList.Controls.Add(this.btnAdd);
            this.grpList.Controls.Add(this.panel1);
            this.grpList.Controls.Add(this.panel2);
            this.grpList.Location = new System.Drawing.Point(10, 11);
            this.grpList.Name = "grpList";
            this.grpList.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.grpList.Size = new System.Drawing.Size(589, 404);
            this.grpList.TabIndex = 4;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(96, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 32);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(261, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(179, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 32);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(259, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Parameter List";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "File List";
            // 
            // lstParameter
            // 
            this.lstParameter.FormattingEnabled = true;
            this.lstParameter.HorizontalScrollbar = true;
            this.lstParameter.Location = new System.Drawing.Point(252, 73);
            this.lstParameter.Name = "lstParameter";
            this.lstParameter.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstParameter.Size = new System.Drawing.Size(326, 316);
            this.lstParameter.TabIndex = 9;
            this.lstParameter.SelectedIndexChanged += new System.EventHandler(this.lstParameter_SelectedIndexChanged);
            this.lstParameter.Enter += new System.EventHandler(this.lstParameter_Enter);
            // 
            // lstFile
            // 
            this.lstFile.FormattingEnabled = true;
            this.lstFile.HorizontalScrollbar = true;
            this.lstFile.Location = new System.Drawing.Point(15, 73);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(204, 316);
            this.lstFile.TabIndex = 8;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            this.lstFile.Click += new System.EventHandler(this.lstFile_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 32);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Location = new System.Drawing.Point(15, 53);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 20);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Location = new System.Drawing.Point(252, 53);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(326, 20);
            this.panel2.TabIndex = 15;
            // 
            // ConsumerExportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 447);
            this.Controls.Add(this.grpAdd);
            this.Controls.Add(this.grpList);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ConsumerExportSettings";
            this.StatusMessage = "";
            this.Text = "ConsumerExportSettings";
            this.Load += new System.EventHandler(this.ConsumerExportSettings_Load);
            this.grpAdd.ResumeLayout(false);
            this.grpAdd.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpList.ResumeLayout(false);
            this.grpList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxSelectedMeters;
        private System.Windows.Forms.Button btnMoveBackAll;
        private System.Windows.Forms.Button btnMoveBack;
        private System.Windows.Forms.Button btnMoveNextAll;
        private System.Windows.Forms.Button btnMoveNext;
        private System.Windows.Forms.ListBox listBoxAvailableMeters;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel grpList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lstParameter;
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
    }
}