namespace CABAppControl
{
    partial class MultiSelect
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button btnSelectedToAvailableAll;
            System.Windows.Forms.Button btnAvailableToSelectedAll;
            this.btnSelectedToAvailableMany = new System.Windows.Forms.Button();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.listAvailable = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAvailableToSelectedMany = new System.Windows.Forms.Button();
            btnSelectedToAvailableAll = new System.Windows.Forms.Button();
            btnAvailableToSelectedAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelectedToAvailableAll
            // 
            btnSelectedToAvailableAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnSelectedToAvailableAll.Location = new System.Drawing.Point(305, 310);
            btnSelectedToAvailableAll.Name = "btnSelectedToAvailableAll";
            btnSelectedToAvailableAll.Size = new System.Drawing.Size(62, 29);
            btnSelectedToAvailableAll.TabIndex = 36;
            btnSelectedToAvailableAll.Text = "<<";
            btnSelectedToAvailableAll.UseVisualStyleBackColor = true;
            btnSelectedToAvailableAll.Click += new System.EventHandler(this.btnSelectedToAvailableAll_Click);
            // 
            // btnAvailableToSelectedAll
            // 
            btnAvailableToSelectedAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnAvailableToSelectedAll.Location = new System.Drawing.Point(305, 237);
            btnAvailableToSelectedAll.Name = "btnAvailableToSelectedAll";
            btnAvailableToSelectedAll.Size = new System.Drawing.Size(62, 29);
            btnAvailableToSelectedAll.TabIndex = 44;
            btnAvailableToSelectedAll.Text = ">>";
            btnAvailableToSelectedAll.UseVisualStyleBackColor = true;
            btnAvailableToSelectedAll.Click += new System.EventHandler(this.btnAvailableToSelectedAll_Click);
            // 
            // btnSelectedToAvailableMany
            // 
            this.btnSelectedToAvailableMany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectedToAvailableMany.Location = new System.Drawing.Point(305, 275);
            this.btnSelectedToAvailableMany.Name = "btnSelectedToAvailableMany";
            this.btnSelectedToAvailableMany.Size = new System.Drawing.Size(62, 29);
            this.btnSelectedToAvailableMany.TabIndex = 35;
            this.btnSelectedToAvailableMany.Text = "<";
            this.btnSelectedToAvailableMany.UseVisualStyleBackColor = true;
            this.btnSelectedToAvailableMany.Click += new System.EventHandler(this.btnSelectedToAvailableMany_Click);
            // 
            // lstSelected
            // 
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.HorizontalScrollbar = true;
            this.lstSelected.Location = new System.Drawing.Point(382, 36);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(272, 511);
            this.lstSelected.TabIndex = 39;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(579, 558);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 32);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // listAvailable
            // 
            this.listAvailable.FormattingEnabled = true;
            this.listAvailable.Location = new System.Drawing.Point(17, 36);
            this.listAvailable.Name = "listAvailable";
            this.listAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listAvailable.Size = new System.Drawing.Size(272, 511);
            this.listAvailable.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(379, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Selected Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Available Parameters";
            // 
            // btnAvailableToSelectedMany
            // 
            this.btnAvailableToSelectedMany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvailableToSelectedMany.Location = new System.Drawing.Point(305, 201);
            this.btnAvailableToSelectedMany.Name = "btnAvailableToSelectedMany";
            this.btnAvailableToSelectedMany.Size = new System.Drawing.Size(62, 29);
            this.btnAvailableToSelectedMany.TabIndex = 43;
            this.btnAvailableToSelectedMany.Text = ">";
            this.btnAvailableToSelectedMany.UseVisualStyleBackColor = true;
            this.btnAvailableToSelectedMany.Click += new System.EventHandler(this.btnAvailableToSelectedMany_Click);
            // 
            // MultiSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(btnAvailableToSelectedAll);
            this.Controls.Add(this.btnAvailableToSelectedMany);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listAvailable);
            this.Controls.Add(btnSelectedToAvailableAll);
            this.Controls.Add(this.btnSelectedToAvailableMany);
            this.Controls.Add(this.lstSelected);
            this.Controls.Add(this.btnSave);
            this.Name = "MultiSelect";
            this.Size = new System.Drawing.Size(677, 593);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectedToAvailableMany;
        private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox listAvailable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAvailableToSelectedMany;
    }
}
