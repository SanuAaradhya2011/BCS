namespace CABApplication
{
    partial class IECPhasorReadout
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
            this.tabControl1 = new CAB.UI.Controls.PremiumTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lngPhasorDiagram = new CAB.UI.Controls.IECPhasorDiagram();
            this.lblPhasorData = new System.Windows.Forms.Label();
            this.btnCancelPhasor = new System.Windows.Forms.Button();
            this.btnStopPhasor = new System.Windows.Forms.Button();
            this.btnReadPhasor = new System.Windows.Forms.Button();
            this.lngPgrid = new CAB.UI.Controls.CABGridControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(953, 460);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lngPhasorDiagram);
            this.tabPage1.Controls.Add(this.lblPhasorData);
            this.tabPage1.Controls.Add(this.btnCancelPhasor);
            this.tabPage1.Controls.Add(this.btnStopPhasor);
            this.tabPage1.Controls.Add(this.btnReadPhasor);
            this.tabPage1.Controls.Add(this.lngPgrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(945, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Phasor";
            // 
            // lngPhasorDiagram
            // 
            this.lngPhasorDiagram.BackColor = System.Drawing.Color.Transparent;
            this.lngPhasorDiagram.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lngPhasorDiagram.Location = new System.Drawing.Point(5, 7);
            this.lngPhasorDiagram.Name = "lngPhasorDiagram";
            this.lngPhasorDiagram.PhasorData = null;
            this.lngPhasorDiagram.PhasorDataset = null;
            this.lngPhasorDiagram.Size = new System.Drawing.Size(468, 394);
            this.lngPhasorDiagram.TabIndex = 18;
            // 
            // lblPhasorData
            // 
            this.lblPhasorData.AutoSize = true;
            this.lblPhasorData.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhasorData.ForeColor = System.Drawing.Color.Red;
            this.lblPhasorData.Location = new System.Drawing.Point(36, 17);
            this.lblPhasorData.Name = "lblPhasorData";
            this.lblPhasorData.Size = new System.Drawing.Size(61, 13);
            this.lblPhasorData.TabIndex = 17;
            this.lblPhasorData.Text = "NoPhasor";
            this.lblPhasorData.Visible = false;
            // 
            // btnCancelPhasor
            // 
            this.btnCancelPhasor.Location = new System.Drawing.Point(854, 399);
            this.btnCancelPhasor.Name = "btnCancelPhasor";
            this.btnCancelPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnCancelPhasor.TabIndex = 16;
            this.btnCancelPhasor.Text = "Cancel";
            this.btnCancelPhasor.UseVisualStyleBackColor = false;
            this.btnCancelPhasor.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancelPhasor.ForeColor = System.Drawing.Color.White;
            this.btnCancelPhasor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelPhasor.FlatAppearance.BorderSize = 0;
            this.btnCancelPhasor.Click += new System.EventHandler(this.btnCancelPhasor_Click);
            // 
            // btnStopPhasor
            // 
            this.btnStopPhasor.Enabled = false;
            this.btnStopPhasor.Location = new System.Drawing.Point(765, 399);
            this.btnStopPhasor.Name = "btnStopPhasor";
            this.btnStopPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnStopPhasor.TabIndex = 15;
            this.btnStopPhasor.Text = "Hold";
            this.btnStopPhasor.UseVisualStyleBackColor = false;
            this.btnStopPhasor.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnStopPhasor.ForeColor = System.Drawing.Color.White;
            this.btnStopPhasor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopPhasor.FlatAppearance.BorderSize = 0;
            this.btnStopPhasor.Click += new System.EventHandler(this.btnStopPhasor_Click);
            // 
            // btnReadPhasor
            // 
            this.btnReadPhasor.Location = new System.Drawing.Point(676, 399);
            this.btnReadPhasor.Name = "btnReadPhasor";
            this.btnReadPhasor.Size = new System.Drawing.Size(83, 29);
            this.btnReadPhasor.TabIndex = 14;
            this.btnReadPhasor.Text = "Read Data";
            this.btnReadPhasor.UseVisualStyleBackColor = false;
            this.btnReadPhasor.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnReadPhasor.ForeColor = System.Drawing.Color.White;
            this.btnReadPhasor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadPhasor.FlatAppearance.BorderSize = 0;
            this.btnReadPhasor.Click += new System.EventHandler(this.btnReadPhasor_Click);
            // 
            // lngPgrid
            // 
            this.lngPgrid.AutoScroll = true;
            this.lngPgrid.Data = null;
            this.lngPgrid.HiddenColumn = null;
            this.lngPgrid.HiddenColumns = null;
            this.lngPgrid.IsEqual = true;
            this.lngPgrid.IsFullRow = false;
            this.lngPgrid.IsSorting = false;
            this.lngPgrid.Location = new System.Drawing.Point(476, 4);
            this.lngPgrid.Margin = new System.Windows.Forms.Padding(4);
            this.lngPgrid.Name = "lngPgrid";
            this.lngPgrid.SelectedIndex = 0;
            this.lngPgrid.SelectedRowId = "";
            this.lngPgrid.Size = new System.Drawing.Size(464, 388);
            this.lngPgrid.TabIndex = 13;
            this.lngPgrid.ValueColumn = null;
            // 
            // statusStrip
            // 
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 490);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(972, 22);
            this.statusStrip.TabIndex = 57;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Tick += new System.EventHandler(this.progressBarTimer_Tick);
            // 
            // IECPhasorReadout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(972, 512);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.Name = "IECPhasorReadout";
            this.StatusMessage = "";
            this.Text = "Dynamic Phasor Readout";
            this.Load += new System.EventHandler(this.IECPhasorReadout_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnCancelPhasor;
        private System.Windows.Forms.Button btnStopPhasor;
        private System.Windows.Forms.Button btnReadPhasor;
        private CAB.UI.Controls.CABGridControl lngPgrid;
        private System.Windows.Forms.Label lblPhasorData;
        private CAB.UI.Controls.IECPhasorDiagram lngPhasorDiagram;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Timer progressBarTimer;
    }
}

