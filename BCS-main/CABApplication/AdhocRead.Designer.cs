namespace CABApplication
{
    partial class AdhocRead
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
            this.components = new System.ComponentModel.Container();
            this.tabControlInstant = new CAB.UI.Controls.PremiumTabControl();
            this.tabPageReading = new System.Windows.Forms.TabPage();
            this.btnReadAdhoc = new System.Windows.Forms.Button();
            this.btnCancelSnap = new System.Windows.Forms.Button();
            this.dgvAdhoc = new CAB.UI.Controls.CABGridControl();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControlInstant.SuspendLayout();
            this.tabPageReading.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlInstant
            // 
            this.tabControlInstant.Controls.Add(this.tabPageReading);
            this.tabControlInstant.Location = new System.Drawing.Point(18, 18);
            this.tabControlInstant.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlInstant.Name = "tabControlInstant";
            this.tabControlInstant.SelectedIndex = 0;
            this.tabControlInstant.Size = new System.Drawing.Size(1449, 637);
            this.tabControlInstant.TabIndex = 1;
            // 
            // tabPageReading
            // 
            this.tabPageReading.AutoScroll = true;
            this.tabPageReading.Controls.Add(this.btnReadAdhoc);
            this.tabPageReading.Controls.Add(this.btnCancelSnap);
            this.tabPageReading.Controls.Add(this.dgvAdhoc);
            this.tabPageReading.Location = new System.Drawing.Point(4, 29);
            this.tabPageReading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageReading.Name = "tabPageReading";
            this.tabPageReading.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageReading.Size = new System.Drawing.Size(1441, 604);
            this.tabPageReading.TabIndex = 0;
            this.tabPageReading.Text = "Adhoc Read";
            // 
            // btnReadAdhoc
            // 
            this.btnReadAdhoc.Location = new System.Drawing.Point(1148, 543);
            this.btnReadAdhoc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReadAdhoc.Name = "btnReadAdhoc";
            this.btnReadAdhoc.Size = new System.Drawing.Size(124, 45);
            this.btnReadAdhoc.TabIndex = 17;
            this.btnReadAdhoc.Text = "Read Data";
            this.btnReadAdhoc.UseVisualStyleBackColor = false;
            this.btnReadAdhoc.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnReadAdhoc.ForeColor = System.Drawing.Color.White;
            this.btnReadAdhoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadAdhoc.FlatAppearance.BorderSize = 0;
            this.btnReadAdhoc.Click += new System.EventHandler(this.btnReadAdhoc_Click);
            // 
            // btnCancelSnap
            // 
            this.btnCancelSnap.Location = new System.Drawing.Point(1281, 543);
            this.btnCancelSnap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelSnap.Name = "btnCancelSnap";
            this.btnCancelSnap.Size = new System.Drawing.Size(124, 45);
            this.btnCancelSnap.TabIndex = 18;
            this.btnCancelSnap.Text = "Cancel";
            this.btnCancelSnap.UseVisualStyleBackColor = false;
            this.btnCancelSnap.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancelSnap.ForeColor = System.Drawing.Color.White;
            this.btnCancelSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSnap.FlatAppearance.BorderSize = 0;
            this.btnCancelSnap.Click += new System.EventHandler(this.btnCancelSnapRead_Click);
            // 
            // dgvAdhoc
            // 
            this.dgvAdhoc.AutoScroll = true;
            this.dgvAdhoc.BackgroundImage = global::CABApplication.Properties.Resources.bakgroundmain;
            this.dgvAdhoc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dgvAdhoc.Data = null;
            this.dgvAdhoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAdhoc.HiddenColumn = null;
            this.dgvAdhoc.HiddenColumns = null;
            this.dgvAdhoc.IsEqual = false;
            this.dgvAdhoc.IsFullRow = false;
            this.dgvAdhoc.IsSorting = false;
            this.dgvAdhoc.Location = new System.Drawing.Point(10, 11);
            this.dgvAdhoc.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.dgvAdhoc.Name = "dgvAdhoc";
            this.dgvAdhoc.SelectedIndex = 0;
            this.dgvAdhoc.SelectedRowId = "";
            this.dgvAdhoc.Size = new System.Drawing.Size(1418, 523);
            this.dgvAdhoc.TabIndex = 0;
            this.dgvAdhoc.ValueColumn = null;
            this.dgvAdhoc.Load += new System.EventHandler(this.dgvAdhoc_Load);
            // 
            // statusStrip
            // 
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 942);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(1470, 34);
            this.statusStrip.TabIndex = 58;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(150, 26);
            // 
            // AdhocRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImage = global::CABApplication.Properties.Resources.Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1470, 655);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControlInstant);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "AdhocRead";
            this.StatusMessage = "";
            this.Text = "Adhoc Read";
            this.Load += new System.EventHandler(this.SnapRead_Load);
            this.tabControlInstant.ResumeLayout(false);
            this.tabPageReading.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabControlInstant;
        private System.Windows.Forms.Button btnCancelSnap;
        private System.Windows.Forms.Button btnReadAdhoc;
        private System.Windows.Forms.TabPage tabPageReading;
        private CAB.UI.Controls.CABGridControl dgvAdhoc;
        private System.Windows.Forms.Timer progressBarTimer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;

    }
}

