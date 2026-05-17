namespace CAB.UI.Controls
{
    partial class CABGridControl
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
            this.panelNoData = new System.Windows.Forms.Panel();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.panelNoData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // panelNoData
            // 
            this.panelNoData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNoData.AutoScroll = true;
            this.panelNoData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNoData.Controls.Add(this.lngLabel1);
            this.panelNoData.Location = new System.Drawing.Point(0, 0);
            this.panelNoData.Name = "panelNoData";
            this.panelNoData.Size = new System.Drawing.Size(445, 270);
            this.panelNoData.TabIndex = 0;
            // 
            // lngLabel1
            // 
            this.lngLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngLabel1.ForeColor = System.Drawing.Color.Red;
            this.lngLabel1.Location = new System.Drawing.Point(154, 46);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(122, 17);
            this.lngLabel1.TabIndex = 0;
            this.lngLabel1.Text = "No Data Found.";
            this.lngLabel1.TranslationKey = null;
            // 
            // grdData
            // 
            this.grdData.AllowUserToAddRows = false;
            this.grdData.AllowUserToDeleteRows = false;
            this.grdData.AllowUserToOrderColumns = true;
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.MultiSelect = false;
            this.grdData.Name = "grdData";
            this.grdData.ReadOnly = true;
            this.grdData.RowTemplate.Height = 24;
            this.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdData.Size = new System.Drawing.Size(445, 273);
            this.grdData.TabIndex = 1;
            this.grdData.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseDoubleClick);
            this.grdData.SelectionChanged += new System.EventHandler(this.grdData_SelectionChanged);
            // 
            // CABGridControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.panelNoData);
            this.Name = "CABGridControl";
            this.Size = new System.Drawing.Size(445, 273);
            this.panelNoData.ResumeLayout(false);
            this.panelNoData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelNoData;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.DataGridView grdData;
    }
}
