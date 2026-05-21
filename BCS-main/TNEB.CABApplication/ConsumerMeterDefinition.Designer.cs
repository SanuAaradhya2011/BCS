namespace CAB.UI
{
    partial class ConsumerMeterDefinition
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
            this.tabControlConsumerMeter = new System.Windows.Forms.TabControl();
            this.tabPageActive = new System.Windows.Forms.TabPage();
            this.grdActiveMeter = new System.Windows.Forms.DataGridView();
            this.tabPageDeactive = new System.Windows.Forms.TabPage();
            this.grdInactiveMeter = new System.Windows.Forms.DataGridView();
            this.tabPageFreeConsumer = new System.Windows.Forms.TabPage();
            this.grdFreeConsumer = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.tabControlConsumerMeter.SuspendLayout();
            this.tabPageActive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdActiveMeter)).BeginInit();
            this.tabPageDeactive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInactiveMeter)).BeginInit();
            this.tabPageFreeConsumer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFreeConsumer)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.ucSearchControl);
            this.panel1.Controls.Add(this.tabControlConsumerMeter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(757, 486);
            this.panel1.TabIndex = 0;
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucSearchControl.HideMainSearch = false;
            this.ucSearchControl.Location = new System.Drawing.Point(4, 3);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = false;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(749, 32);
            this.ucSearchControl.TabIndex = 37;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // tabControlConsumerMeter
            // 
            this.tabControlConsumerMeter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlConsumerMeter.Controls.Add(this.tabPageActive);
            this.tabControlConsumerMeter.Controls.Add(this.tabPageDeactive);
            this.tabControlConsumerMeter.Controls.Add(this.tabPageFreeConsumer);
            this.tabControlConsumerMeter.Location = new System.Drawing.Point(3, 41);
            this.tabControlConsumerMeter.Name = "tabControlConsumerMeter";
            this.tabControlConsumerMeter.SelectedIndex = 0;
            this.tabControlConsumerMeter.Size = new System.Drawing.Size(751, 442);
            this.tabControlConsumerMeter.TabIndex = 36;
            // 
            // tabPageActive
            // 
            this.tabPageActive.Controls.Add(this.grdActiveMeter);
            this.tabPageActive.Location = new System.Drawing.Point(4, 22);
            this.tabPageActive.Name = "tabPageActive";
            this.tabPageActive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageActive.Size = new System.Drawing.Size(743, 416);
            this.tabPageActive.TabIndex = 0;
            this.tabPageActive.Text = "Active Meter";
            this.tabPageActive.UseVisualStyleBackColor = true;
            this.tabPageActive.Enter += new System.EventHandler(this.tabPageActive_Enter);
            // 
            // grdActiveMeter
            // 
            this.grdActiveMeter.AllowUserToAddRows = false;
            this.grdActiveMeter.AllowUserToDeleteRows = false;
            this.grdActiveMeter.AllowUserToResizeColumns = false;
            this.grdActiveMeter.AllowUserToResizeRows = false;
            this.grdActiveMeter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdActiveMeter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdActiveMeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdActiveMeter.Location = new System.Drawing.Point(3, 3);
            this.grdActiveMeter.MultiSelect = false;
            this.grdActiveMeter.Name = "grdActiveMeter";
            this.grdActiveMeter.ReadOnly = true;
            this.grdActiveMeter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdActiveMeter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdActiveMeter.Size = new System.Drawing.Size(737, 410);
            this.grdActiveMeter.TabIndex = 36;
            // 
            // tabPageDeactive
            // 
            this.tabPageDeactive.Controls.Add(this.grdInactiveMeter);
            this.tabPageDeactive.Location = new System.Drawing.Point(4, 22);
            this.tabPageDeactive.Name = "tabPageDeactive";
            this.tabPageDeactive.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDeactive.Size = new System.Drawing.Size(743, 416);
            this.tabPageDeactive.TabIndex = 1;
            this.tabPageDeactive.Text = "Inactive Meter";
            this.tabPageDeactive.UseVisualStyleBackColor = true;
            this.tabPageDeactive.Enter += new System.EventHandler(this.tabPageDeactive_Enter);
            // 
            // grdInactiveMeter
            // 
            this.grdInactiveMeter.AllowUserToAddRows = false;
            this.grdInactiveMeter.AllowUserToDeleteRows = false;
            this.grdInactiveMeter.AllowUserToResizeColumns = false;
            this.grdInactiveMeter.AllowUserToResizeRows = false;
            this.grdInactiveMeter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.grdInactiveMeter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdInactiveMeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInactiveMeter.Location = new System.Drawing.Point(3, 3);
            this.grdInactiveMeter.MultiSelect = false;
            this.grdInactiveMeter.Name = "grdInactiveMeter";
            this.grdInactiveMeter.ReadOnly = true;
            this.grdInactiveMeter.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdInactiveMeter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdInactiveMeter.Size = new System.Drawing.Size(737, 410);
            this.grdInactiveMeter.TabIndex = 37;
            // 
            // tabPageFreeConsumer
            // 
            this.tabPageFreeConsumer.Controls.Add(this.grdFreeConsumer);
            this.tabPageFreeConsumer.Location = new System.Drawing.Point(4, 22);
            this.tabPageFreeConsumer.Name = "tabPageFreeConsumer";
            this.tabPageFreeConsumer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFreeConsumer.Size = new System.Drawing.Size(743, 416);
            this.tabPageFreeConsumer.TabIndex = 2;
            this.tabPageFreeConsumer.Text = "Free Consumer";
            this.tabPageFreeConsumer.UseVisualStyleBackColor = true;
            this.tabPageFreeConsumer.Enter += new System.EventHandler(this.tabPageFreeConsumer_Enter);
            // 
            // grdFreeConsumer
            // 
            this.grdFreeConsumer.AllowUserToAddRows = false;
            this.grdFreeConsumer.AllowUserToDeleteRows = false;
            this.grdFreeConsumer.AllowUserToResizeColumns = false;
            this.grdFreeConsumer.AllowUserToResizeRows = false;
            this.grdFreeConsumer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.grdFreeConsumer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdFreeConsumer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFreeConsumer.Location = new System.Drawing.Point(3, 3);
            this.grdFreeConsumer.MultiSelect = false;
            this.grdFreeConsumer.Name = "grdFreeConsumer";
            this.grdFreeConsumer.ReadOnly = true;
            this.grdFreeConsumer.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdFreeConsumer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFreeConsumer.Size = new System.Drawing.Size(737, 410);
            this.grdFreeConsumer.TabIndex = 37;
            // 
            // ConsumerMeterDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(757, 486);
            this.Controls.Add(this.panel1);
            this.Name = "ConsumerMeterDefinition";
            this.StatusMessage = "";
            this.Text = "Consumer Meter Definition";
            this.Activated += new System.EventHandler(this.ConsumerMeterDefinition_Activated);
            this.panel1.ResumeLayout(false);
            this.tabControlConsumerMeter.ResumeLayout(false);
            this.tabPageActive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdActiveMeter)).EndInit();
            this.tabPageDeactive.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInactiveMeter)).EndInit();
            this.tabPageFreeConsumer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFreeConsumer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.TabControl tabControlConsumerMeter;
		private System.Windows.Forms.TabPage tabPageActive;
		private System.Windows.Forms.DataGridView grdActiveMeter;
		private System.Windows.Forms.TabPage tabPageDeactive;
		private System.Windows.Forms.DataGridView grdInactiveMeter;
		private System.Windows.Forms.TabPage tabPageFreeConsumer;
		private System.Windows.Forms.DataGridView grdFreeConsumer;
		private CAB.UI.Controls.CABSearchControl ucSearchControl;

	}
}