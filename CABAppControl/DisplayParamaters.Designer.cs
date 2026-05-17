namespace CAB.UI.Controls
{
    partial class DisplayParamaters
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnUpScroll = new System.Windows.Forms.Button();
            this.chkboxSelectAll = new System.Windows.Forms.CheckBox();
            this.btnDownScroll = new System.Windows.Forms.Button();
            this.tabControlDisplayParams = new System.Windows.Forms.TabControl();
            this.tabPushButton = new System.Windows.Forms.TabPage();
            this.dgridPushDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabScrollButton = new System.Windows.Forms.TabPage();
            this.dgridScrollDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabHighResolution = new System.Windows.Forms.TabPage();
            this.dgridHighResolution = new System.Windows.Forms.DataGridView();
            this.tabDisplayTimeouts = new System.Windows.Forms.TabPage();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.chkBoxAutoScrollTime = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtAutoScrollTime = new System.Windows.Forms.TextBox();
            this.txtPushTimeout = new System.Windows.Forms.TextBox();
            this.txtScrollTime = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2.SuspendLayout();
            this.tabControlDisplayParams.SuspendLayout();
            this.tabPushButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgridPushDisplayParams)).BeginInit();
            this.tabScrollButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgridScrollDisplayParams)).BeginInit();
            this.tabHighResolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgridHighResolution)).BeginInit();
            this.tabDisplayTimeouts.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnUpScroll);
            this.panel2.Controls.Add(this.chkboxSelectAll);
            this.panel2.Controls.Add(this.btnDownScroll);
            this.panel2.Location = new System.Drawing.Point(647, 23);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(92, 383);
            this.panel2.TabIndex = 17;
            // 
            // btnUpScroll
            // 
            this.btnUpScroll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpScroll.Location = new System.Drawing.Point(23, 172);
            this.btnUpScroll.Name = "btnUpScroll";
            this.btnUpScroll.Size = new System.Drawing.Size(43, 30);
            this.btnUpScroll.TabIndex = 3;
            this.btnUpScroll.Text = "^";
            this.btnUpScroll.UseVisualStyleBackColor = true;
            this.btnUpScroll.Click += new System.EventHandler(this.btnUpScroll_Click);
            // 
            // chkboxSelectAll
            // 
            this.chkboxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkboxSelectAll.AutoSize = true;
            this.chkboxSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxSelectAll.Location = new System.Drawing.Point(7, 76);
            this.chkboxSelectAll.Name = "chkboxSelectAll";
            this.chkboxSelectAll.Size = new System.Drawing.Size(80, 17);
            this.chkboxSelectAll.TabIndex = 3;
            this.chkboxSelectAll.Text = "Select All";
            this.chkboxSelectAll.UseVisualStyleBackColor = true;
            this.chkboxSelectAll.CheckedChanged += new System.EventHandler(this.chkboxSelectAll_CheckedChanged);
            // 
            // btnDownScroll
            // 
            this.btnDownScroll.Location = new System.Drawing.Point(23, 223);
            this.btnDownScroll.Name = "btnDownScroll";
            this.btnDownScroll.Size = new System.Drawing.Size(43, 31);
            this.btnDownScroll.TabIndex = 2;
            this.btnDownScroll.Text = "v";
            this.btnDownScroll.UseVisualStyleBackColor = true;
            this.btnDownScroll.Click += new System.EventHandler(this.btnDownScroll_Click);
            // 
            // tabControlDisplayParams
            // 
            this.tabControlDisplayParams.Controls.Add(this.tabPushButton);
            this.tabControlDisplayParams.Controls.Add(this.tabScrollButton);
            this.tabControlDisplayParams.Controls.Add(this.tabHighResolution);
            this.tabControlDisplayParams.Controls.Add(this.tabDisplayTimeouts);
            this.tabControlDisplayParams.Location = new System.Drawing.Point(20, 20);
            this.tabControlDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlDisplayParams.Name = "tabControlDisplayParams";
            this.tableLayoutPanel13.SetRowSpan(this.tabControlDisplayParams, 3);
            this.tabControlDisplayParams.SelectedIndex = 0;
            this.tabControlDisplayParams.Size = new System.Drawing.Size(570, 386);
            this.tabControlDisplayParams.TabIndex = 2;
            // 
            // tabPushButton
            // 
            this.tabPushButton.Controls.Add(this.dgridPushDisplayParams);
            this.tabPushButton.Location = new System.Drawing.Point(4, 22);
            this.tabPushButton.Name = "tabPushButton";
            this.tabPushButton.Size = new System.Drawing.Size(562, 360);
            this.tabPushButton.TabIndex = 0;
            this.tabPushButton.Text = "Push Button";
            this.tabPushButton.UseVisualStyleBackColor = true;
            this.tabPushButton.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
            // 
            // dgridPushDisplayParams
            // 
            this.dgridPushDisplayParams.AllowUserToAddRows = false;
            this.dgridPushDisplayParams.AllowUserToDeleteRows = false;
            this.dgridPushDisplayParams.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgridPushDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgridPushDisplayParams.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgridPushDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dgridPushDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dgridPushDisplayParams.MultiSelect = false;
            this.dgridPushDisplayParams.Name = "dgridPushDisplayParams";
            this.dgridPushDisplayParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgridPushDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgridPushDisplayParams.Size = new System.Drawing.Size(555, 350);
            this.dgridPushDisplayParams.TabIndex = 0;
            
            this.dgridPushDisplayParams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CheckBoxChange);
            // 
            // tabScrollButton
            // 
            this.tabScrollButton.Controls.Add(this.dgridScrollDisplayParams);
            this.tabScrollButton.Location = new System.Drawing.Point(4, 22);
            this.tabScrollButton.Name = "tabScrollButton";
            this.tabScrollButton.Size = new System.Drawing.Size(562, 360);
            this.tabScrollButton.TabIndex = 1;
            this.tabScrollButton.Text = "Scroll Button";
            this.tabScrollButton.UseVisualStyleBackColor = true;
            this.tabScrollButton.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
            // 
            // dgridScrollDisplayParams
            // 
            this.dgridScrollDisplayParams.AllowUserToAddRows = false;
            this.dgridScrollDisplayParams.AllowUserToDeleteRows = false;
            this.dgridScrollDisplayParams.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgridScrollDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgridScrollDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dgridScrollDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dgridScrollDisplayParams.MultiSelect = false;
            this.dgridScrollDisplayParams.Name = "dgridScrollDisplayParams";
            this.dgridScrollDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgridScrollDisplayParams.Size = new System.Drawing.Size(555, 350);
            this.dgridScrollDisplayParams.TabIndex = 1;
            this.dgridScrollDisplayParams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CheckBoxChange);
            // 
            // tabHighResolution
            // 
            this.tabHighResolution.Controls.Add(this.dgridHighResolution);
            this.tabHighResolution.Location = new System.Drawing.Point(4, 22);
            this.tabHighResolution.Name = "tabHighResolution";
            this.tabHighResolution.Size = new System.Drawing.Size(562, 360);
            this.tabHighResolution.TabIndex = 2;
            this.tabHighResolution.Text = "High Resolution";
            this.tabHighResolution.UseVisualStyleBackColor = true;
            this.tabHighResolution.Enter += new System.EventHandler(this.SetSelectAllCheckBoxStatus);
            // 
            // dgridHighResolution
            // 
            this.dgridHighResolution.AllowUserToAddRows = false;
            this.dgridHighResolution.AllowUserToDeleteRows = false;
            this.dgridHighResolution.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgridHighResolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgridHighResolution.Location = new System.Drawing.Point(0, 0);
            this.dgridHighResolution.Margin = new System.Windows.Forms.Padding(0);
            this.dgridHighResolution.MultiSelect = false;
            this.dgridHighResolution.Name = "dgridHighResolution";
            this.dgridHighResolution.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgridHighResolution.Size = new System.Drawing.Size(555, 350);
            this.dgridHighResolution.TabIndex = 1;
            this.dgridHighResolution.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_CheckBoxChange);
            // 
            // tabDisplayTimeouts
            // 
            this.tabDisplayTimeouts.Controls.Add(this.groupBox19);
            this.tabDisplayTimeouts.Location = new System.Drawing.Point(4, 22);
            this.tabDisplayTimeouts.Name = "tabDisplayTimeouts";
            this.tabDisplayTimeouts.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayTimeouts.Size = new System.Drawing.Size(562, 360);
            this.tabDisplayTimeouts.TabIndex = 3;
            this.tabDisplayTimeouts.Text = "Display Timeouts";
            this.tabDisplayTimeouts.UseVisualStyleBackColor = true;
            this.tabDisplayTimeouts.Enter += new System.EventHandler(this.tabDisplayTimeouts_Enter);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.label43);
            this.groupBox19.Controls.Add(this.label42);
            this.groupBox19.Controls.Add(this.label41);
            this.groupBox19.Controls.Add(this.chkBoxAutoScrollTime);
            this.groupBox19.Controls.Add(this.label22);
            this.groupBox19.Controls.Add(this.label21);
            this.groupBox19.Controls.Add(this.txtAutoScrollTime);
            this.groupBox19.Controls.Add(this.txtPushTimeout);
            this.groupBox19.Controls.Add(this.txtScrollTime);
            this.groupBox19.Location = new System.Drawing.Point(3, 3);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(610, 350);
            this.groupBox19.TabIndex = 2;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Display Timeouts";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(326, 126);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(137, 13);
            this.label43.TabIndex = 9;
            this.label43.Text = "(sec)  * Valid Range (3-300)";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(326, 90);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(137, 13);
            this.label42.TabIndex = 9;
            this.label42.Text = "(sec)  * Valid Range (1-600)";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(326, 55);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(137, 13);
            this.label41.TabIndex = 9;
            this.label41.Text = "(sec)  * Valid Range (1-300)";
            // 
            // chkBoxAutoScrollTime
            // 
            this.chkBoxAutoScrollTime.AutoSize = true;
            this.chkBoxAutoScrollTime.Location = new System.Drawing.Point(67, 127);
            this.chkBoxAutoScrollTime.Name = "chkBoxAutoScrollTime";
            this.chkBoxAutoScrollTime.Size = new System.Drawing.Size(145, 17);
            this.chkBoxAutoScrollTime.TabIndex = 2;
            this.chkBoxAutoScrollTime.Text = "Auto Scroll Resume Time";
            this.chkBoxAutoScrollTime.UseVisualStyleBackColor = true;
            this.chkBoxAutoScrollTime.CheckedChanged += new System.EventHandler(this.chkBoxAutoScrollTime_CheckedChanged);
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(64, 89);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(106, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "Push Button Timeout";
            // 
            // label21
            // 
            this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(64, 54);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(101, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Scroll Time Per Item";
            // 
            // txtAutoScrollTime
            // 
            this.txtAutoScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAutoScrollTime.Enabled = false;
            this.txtAutoScrollTime.Location = new System.Drawing.Point(245, 119);
            this.txtAutoScrollTime.MaxLength = 3;
            this.txtAutoScrollTime.Name = "txtAutoScrollTime";
            this.txtAutoScrollTime.Size = new System.Drawing.Size(75, 20);
            this.txtAutoScrollTime.TabIndex = 3;
            // 
            // txtPushTimeout
            // 
            this.txtPushTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPushTimeout.Location = new System.Drawing.Point(245, 86);
            this.txtPushTimeout.MaxLength = 3;
            this.txtPushTimeout.Name = "txtPushTimeout";
            this.txtPushTimeout.Size = new System.Drawing.Size(75, 20);
            this.txtPushTimeout.TabIndex = 1;
            // 
            // txtScrollTime
            // 
            this.txtScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtScrollTime.Location = new System.Drawing.Point(245, 51);
            this.txtScrollTime.MaxLength = 3;
            this.txtScrollTime.Name = "txtScrollTime";
            this.txtScrollTime.Size = new System.Drawing.Size(75, 20);
            this.txtScrollTime.TabIndex = 0;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel13.ColumnCount = 3;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 624F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel13.Controls.Add(this.tabControlDisplayParams, 1, 1);
            this.tableLayoutPanel13.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 4;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 449F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(733, 406);
            this.tableLayoutPanel13.TabIndex = 1;
          
            // 
            // DisplayParamaters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel13);
            this.Name = "DisplayParamaters";
            this.Size = new System.Drawing.Size(742, 432);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControlDisplayParams.ResumeLayout(false);
            this.tabPushButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgridPushDisplayParams)).EndInit();
            this.tabScrollButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgridScrollDisplayParams)).EndInit();
            this.tabHighResolution.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgridHighResolution)).EndInit();
            this.tabDisplayTimeouts.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.tableLayoutPanel13.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnUpScroll;
        private System.Windows.Forms.CheckBox chkboxSelectAll;
        private System.Windows.Forms.Button btnDownScroll;
        private System.Windows.Forms.TabControl tabControlDisplayParams;
        private System.Windows.Forms.TabPage tabPushButton;
        private System.Windows.Forms.DataGridView dgridPushDisplayParams;
        private System.Windows.Forms.TabPage tabScrollButton;
        private System.Windows.Forms.DataGridView dgridScrollDisplayParams;
        private System.Windows.Forms.TabPage tabHighResolution;
        private System.Windows.Forms.DataGridView dgridHighResolution;
        private System.Windows.Forms.TabPage tabDisplayTimeouts;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.CheckBox chkBoxAutoScrollTime;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtAutoScrollTime;
        private System.Windows.Forms.TextBox txtPushTimeout;
        private System.Windows.Forms.TextBox txtScrollTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;



    }
}
