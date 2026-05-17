namespace CAB.UI.Graphs
{
	partial class GraphFormControl
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphFormControl));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelChart = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panelUserOptions = new System.Windows.Forms.Panel();
			this.grpBoxParameterList = new System.Windows.Forms.GroupBox();
			this.chkShowGridLines = new System.Windows.Forms.CheckBox();
			this.chkLBParameterDisplay = new System.Windows.Forms.CheckedListBox();
			this.cboChartType = new System.Windows.Forms.ComboBox();
			this.rbtnComposite = new System.Windows.Forms.RadioButton();
			this.rbtnSeparate = new System.Windows.Forms.RadioButton();
			this.cboViewType = new System.Windows.Forms.ComboBox();
			this.cboParameters = new System.Windows.Forms.ComboBox();
			this.grpBoxParameterValues = new System.Windows.Forms.GroupBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lblBPhaseValue = new System.Windows.Forms.Label();
			this.lblxPhaseValue = new System.Windows.Forms.Label();
			this.lblYPhaseValue = new System.Windows.Forms.Label();
			this.label_YAxis = new System.Windows.Forms.Label();
			this.label_XAxis = new System.Windows.Forms.Label();
			this.lblRphaseValue = new System.Windows.Forms.Label();
			this.lblDateTime = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.lblChartType = new CAB.UI.Controls.CABLabel();
			this.lblParameterView = new CAB.UI.Controls.CABLabel();
			this.lblViewType = new CAB.UI.Controls.CABLabel();
			this.lblParameter = new CAB.UI.Controls.CABLabel();
			this.lblParameterValues = new CAB.UI.Controls.CABLabel();
			this.btnNext = new CAB.UI.Controls.CABButton();
			this.btnPrevious = new CAB.UI.Controls.CABButton();
			this.btnPrintPreview = new CAB.UI.Controls.CABButton();
			this.btnPrint = new CAB.UI.Controls.CABButton();
			this.tableLayoutPanel1.SuspendLayout();
			this.panelUserOptions.SuspendLayout();
			this.grpBoxParameterList.SuspendLayout();
			this.grpBoxParameterValues.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.27726F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.72274F));
			this.tableLayoutPanel1.Controls.Add(this.panelChart, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.panelUserOptions, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.53933F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.460674F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1284, 712);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panelChart
			// 
			this.panelChart.AutoScroll = true;
			this.panelChart.BackColor = System.Drawing.Color.Lavender;
			this.panelChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelChart.Location = new System.Drawing.Point(209, 43);
			this.panelChart.Margin = new System.Windows.Forms.Padding(0);
			this.panelChart.Name = "panelChart";
			this.panelChart.Size = new System.Drawing.Size(1075, 625);
			this.panelChart.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Lavender;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(212, 671);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1069, 38);
			this.panel1.TabIndex = 1;
			// 
			// panelUserOptions
			// 
			this.panelUserOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.panelUserOptions.BackColor = System.Drawing.Color.Lavender;
			this.panelUserOptions.Controls.Add(this.grpBoxParameterList);
			this.panelUserOptions.Controls.Add(this.grpBoxParameterValues);
			this.panelUserOptions.Location = new System.Drawing.Point(4, 47);
			this.panelUserOptions.Name = "panelUserOptions";
			this.tableLayoutPanel1.SetRowSpan(this.panelUserOptions, 2);
			this.panelUserOptions.Size = new System.Drawing.Size(202, 662);
			this.panelUserOptions.TabIndex = 0;
			// 
			// grpBoxParameterList
			// 
			this.grpBoxParameterList.BackColor = System.Drawing.Color.Lavender;
			this.grpBoxParameterList.Controls.Add(this.chkShowGridLines);
			this.grpBoxParameterList.Controls.Add(this.chkLBParameterDisplay);
			this.grpBoxParameterList.Controls.Add(this.cboChartType);
			this.grpBoxParameterList.Controls.Add(this.lblChartType);
			this.grpBoxParameterList.Controls.Add(this.rbtnComposite);
			this.grpBoxParameterList.Controls.Add(this.rbtnSeparate);
			this.grpBoxParameterList.Controls.Add(this.lblParameterView);
			this.grpBoxParameterList.Controls.Add(this.cboViewType);
			this.grpBoxParameterList.Controls.Add(this.lblViewType);
			this.grpBoxParameterList.Controls.Add(this.lblParameter);
			this.grpBoxParameterList.Controls.Add(this.cboParameters);
			this.grpBoxParameterList.Location = new System.Drawing.Point(2, 279);
			this.grpBoxParameterList.Name = "grpBoxParameterList";
			this.grpBoxParameterList.Size = new System.Drawing.Size(196, 380);
			this.grpBoxParameterList.TabIndex = 3;
			this.grpBoxParameterList.TabStop = false;
			this.grpBoxParameterList.Text = "Parameter List";
			// 
			// chkShowGridLines
			// 
			this.chkShowGridLines.AutoSize = true;
			this.chkShowGridLines.Checked = true;
			this.chkShowGridLines.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowGridLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShowGridLines.Location = new System.Drawing.Point(6, 499);
			this.chkShowGridLines.Name = "chkShowGridLines";
			this.chkShowGridLines.Size = new System.Drawing.Size(118, 17);
			this.chkShowGridLines.TabIndex = 12;
			this.chkShowGridLines.Text = "Show Grid Lines";
            this.chkShowGridLines.UseVisualStyleBackColor = true;
			this.chkShowGridLines.CheckedChanged += new System.EventHandler(this.chkShowGridLines_CheckedChanged);
			// 
			// chkLBParameterDisplay
			// 
			this.chkLBParameterDisplay.CheckOnClick = true;
			this.chkLBParameterDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkLBParameterDisplay.FormattingEnabled = true;
			this.chkLBParameterDisplay.Location = new System.Drawing.Point(6, 154);
			this.chkLBParameterDisplay.Name = "chkLBParameterDisplay";
			this.chkLBParameterDisplay.Size = new System.Drawing.Size(172, 79);
			this.chkLBParameterDisplay.TabIndex = 11;
			this.chkLBParameterDisplay.SelectedIndexChanged += new System.EventHandler(this.chkLBParameterDisplay_SelectedIndexChanged);
			// 
			// cboChartType
			// 
			this.cboChartType.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.cboChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChartType.FormattingEnabled = true;
			this.cboChartType.Items.AddRange(new object[] {
            "Line Chart",
            "Bar Chart"});
			this.cboChartType.Location = new System.Drawing.Point(81, 84);
			this.cboChartType.Name = "cboChartType";
			this.cboChartType.Size = new System.Drawing.Size(97, 21);
			this.cboChartType.TabIndex = 10;
			this.cboChartType.SelectedIndexChanged += new System.EventHandler(this.cboChartType_SelectedIndexChanged);
			// 
			// rbtnComposite
			// 
			this.rbtnComposite.AutoSize = true;
			this.rbtnComposite.Checked = true;
			this.rbtnComposite.Location = new System.Drawing.Point(79, 334);
			this.rbtnComposite.Name = "rbtnComposite";
			this.rbtnComposite.Size = new System.Drawing.Size(74, 17);
			this.rbtnComposite.TabIndex = 8;
			this.rbtnComposite.TabStop = true;
			this.rbtnComposite.Text = "Composite";
            this.rbtnComposite.UseVisualStyleBackColor = true;
			this.rbtnComposite.Visible = false;
			// 
			// rbtnSeparate
			// 
			this.rbtnSeparate.AutoSize = true;
			this.rbtnSeparate.Location = new System.Drawing.Point(79, 311);
			this.rbtnSeparate.Name = "rbtnSeparate";
			this.rbtnSeparate.Size = new System.Drawing.Size(68, 17);
			this.rbtnSeparate.TabIndex = 7;
			this.rbtnSeparate.Text = "Separate";
            this.rbtnSeparate.UseVisualStyleBackColor = true;
			this.rbtnSeparate.Visible = false;
			// 
			// cboViewType
			// 
			this.cboViewType.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.cboViewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboViewType.FormattingEnabled = true;
			this.cboViewType.Items.AddRange(new object[] {
            "Daily",
            "Weekly",
            "Monthly"});
			this.cboViewType.Location = new System.Drawing.Point(81, 47);
			this.cboViewType.Name = "cboViewType";
			this.cboViewType.Size = new System.Drawing.Size(97, 21);
			this.cboViewType.TabIndex = 5;
			this.cboViewType.SelectedIndexChanged += new System.EventHandler(this.cboViewType_SelectedIndexChanged);
			// 
			// cboParameters
			// 
			this.cboParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboParameters.FormattingEnabled = true;
			this.cboParameters.Items.AddRange(new object[] {
            "Voltage",
            "Current",
            "Demand"});
			this.cboParameters.Location = new System.Drawing.Point(81, 121);
			this.cboParameters.Name = "cboParameters";
			this.cboParameters.Size = new System.Drawing.Size(97, 21);
			this.cboParameters.TabIndex = 2;
			this.cboParameters.SelectedIndexChanged += new System.EventHandler(this.cboParameters_SelectedIndexChanged);
			// 
			// grpBoxParameterValues
			// 
			this.grpBoxParameterValues.BackColor = System.Drawing.Color.Lavender;
			this.grpBoxParameterValues.Controls.Add(this.panel3);
			this.grpBoxParameterValues.Controls.Add(this.lblParameterValues);
			this.grpBoxParameterValues.Location = new System.Drawing.Point(3, 19);
			this.grpBoxParameterValues.Name = "grpBoxParameterValues";
			this.grpBoxParameterValues.Size = new System.Drawing.Size(196, 254);
			this.grpBoxParameterValues.TabIndex = 5;
			this.grpBoxParameterValues.TabStop = false;
			this.grpBoxParameterValues.Text = "Parameter Values";
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.lblBPhaseValue);
			this.panel3.Controls.Add(this.lblxPhaseValue);
			this.panel3.Controls.Add(this.lblYPhaseValue);
			this.panel3.Controls.Add(this.label_YAxis);
			this.panel3.Controls.Add(this.label_XAxis);
			this.panel3.Controls.Add(this.lblRphaseValue);
			this.panel3.Controls.Add(this.lblDateTime);
			this.panel3.Location = new System.Drawing.Point(0, 46);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(195, 202);
			this.panel3.TabIndex = 12;
			// 
			// lblBPhaseValue
			// 
			this.lblBPhaseValue.AutoSize = true;
			this.lblBPhaseValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBPhaseValue.Location = new System.Drawing.Point(8, 122);
			this.lblBPhaseValue.Name = "lblBPhaseValue";
			this.lblBPhaseValue.Size = new System.Drawing.Size(19, 13);
			this.lblBPhaseValue.TabIndex = 14;
			this.lblBPhaseValue.Text = "   ";
			// 
			// lblxPhaseValue
			// 
			this.lblxPhaseValue.AutoSize = true;
			this.lblxPhaseValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblxPhaseValue.Location = new System.Drawing.Point(8, 147);
			this.lblxPhaseValue.Name = "lblxPhaseValue";
			this.lblxPhaseValue.Size = new System.Drawing.Size(19, 13);
			this.lblxPhaseValue.TabIndex = 13;
			this.lblxPhaseValue.Text = "   ";
			// 
			// lblYPhaseValue
			// 
			this.lblYPhaseValue.AutoSize = true;
			this.lblYPhaseValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblYPhaseValue.Location = new System.Drawing.Point(8, 97);
			this.lblYPhaseValue.Name = "lblYPhaseValue";
			this.lblYPhaseValue.Size = new System.Drawing.Size(19, 13);
			this.lblYPhaseValue.TabIndex = 12;
			this.lblYPhaseValue.Text = "   ";
			// 
			// label_YAxis
			// 
			this.label_YAxis.AutoSize = true;
			this.label_YAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_YAxis.Location = new System.Drawing.Point(6, 41);
			this.label_YAxis.Name = "label_YAxis";
			this.label_YAxis.Size = new System.Drawing.Size(51, 13);
			this.label_YAxis.TabIndex = 11;
			this.label_YAxis.Text = "Value : ";
			// 
			// label_XAxis
			// 
			this.label_XAxis.AutoSize = true;
			this.label_XAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label_XAxis.Location = new System.Drawing.Point(6, 10);
			this.label_XAxis.Name = "label_XAxis";
			this.label_XAxis.Size = new System.Drawing.Size(73, 13);
			this.label_XAxis.TabIndex = 10;
			this.label_XAxis.Text = "DateTime : ";
			// 
			// lblRphaseValue
			// 
			this.lblRphaseValue.AutoSize = true;
			this.lblRphaseValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblRphaseValue.ForeColor = System.Drawing.Color.Navy;
			this.lblRphaseValue.Location = new System.Drawing.Point(8, 72);
			this.lblRphaseValue.Name = "lblRphaseValue";
			this.lblRphaseValue.Size = new System.Drawing.Size(19, 13);
			this.lblRphaseValue.TabIndex = 9;
			this.lblRphaseValue.Text = "   ";
			// 
			// lblDateTime
			// 
			this.lblDateTime.AutoSize = true;
			this.lblDateTime.BackColor = System.Drawing.SystemColors.Control;
			this.lblDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDateTime.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lblDateTime.Location = new System.Drawing.Point(82, 10);
			this.lblDateTime.Name = "lblDateTime";
			this.lblDateTime.Size = new System.Drawing.Size(41, 13);
			this.lblDateTime.TabIndex = 8;
			this.lblDateTime.Text = "label1";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.Lavender;
			this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.btnNext);
			this.panel2.Controls.Add(this.btnPrevious);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.btnPrintPreview);
			this.panel2.Controls.Add(this.btnPrint);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.ForeColor = System.Drawing.Color.MediumBlue;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1278, 37);
			this.panel2.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(899, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(260, 31);
			this.label4.TabIndex = 5;
			this.label4.Text = "label4";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(639, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(260, 31);
			this.label3.TabIndex = 4;
			this.label3.Text = "label3";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(379, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(260, 31);
			this.label2.TabIndex = 3;
			this.label2.Text = "label2";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(119, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(260, 31);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblChartType
			// 
			this.lblChartType.AutoSize = true;
			this.lblChartType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblChartType.Location = new System.Drawing.Point(8, 87);
			this.lblChartType.Name = "lblChartType";
			this.lblChartType.Size = new System.Drawing.Size(69, 13);
			this.lblChartType.TabIndex = 9;
			this.lblChartType.Text = "Chart Type";
			this.lblChartType.TranslationKey = null;
			// 
			// lblParameterView
			// 
			this.lblParameterView.AutoSize = true;
			this.lblParameterView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblParameterView.Location = new System.Drawing.Point(6, 292);
			this.lblParameterView.Name = "lblParameterView";
			this.lblParameterView.Size = new System.Drawing.Size(101, 13);
			this.lblParameterView.TabIndex = 6;
			this.lblParameterView.Text = "Parameters View";
			this.lblParameterView.TranslationKey = null;
			this.lblParameterView.Visible = false;
			// 
			// lblViewType
			// 
			this.lblViewType.AutoSize = true;
			this.lblViewType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblViewType.Location = new System.Drawing.Point(8, 50);
			this.lblViewType.Name = "lblViewType";
			this.lblViewType.Size = new System.Drawing.Size(66, 13);
			this.lblViewType.TabIndex = 4;
			this.lblViewType.Text = "View Type";
			this.lblViewType.TranslationKey = null;
			// 
			// lblParameter
			// 
			this.lblParameter.AutoSize = true;
			this.lblParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblParameter.Location = new System.Drawing.Point(8, 124);
			this.lblParameter.Name = "lblParameter";
			this.lblParameter.Size = new System.Drawing.Size(70, 13);
			this.lblParameter.TabIndex = 3;
			this.lblParameter.Text = "Parameters";
			this.lblParameter.TranslationKey = null;
			// 
			// lblParameterValues
			// 
			this.lblParameterValues.AutoSize = true;
			this.lblParameterValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblParameterValues.Location = new System.Drawing.Point(8, 27);
			this.lblParameterValues.Name = "lblParameterValues";
			this.lblParameterValues.Size = new System.Drawing.Size(132, 16);
			this.lblParameterValues.TabIndex = 4;
			this.lblParameterValues.Text = "Parameter Values";
			this.lblParameterValues.TranslationKey = null;
			// 
			// btnNext
			// 
			this.btnNext.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNext.ForeColor = System.Drawing.Color.Navy;
			this.btnNext.Location = new System.Drawing.Point(1223, 3);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(52, 31);
			this.btnNext.TabIndex = 3;
			this.btnNext.Text = ">>";
			this.btnNext.TranslationKey = null;
			this.btnNext.UseVisualStyleBackColor = false;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPrevious.ForeColor = System.Drawing.Color.Navy;
			this.btnPrevious.Location = new System.Drawing.Point(1171, 3);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(52, 31);
			this.btnPrevious.TabIndex = 2;
			this.btnPrevious.Text = "<<";
			this.btnPrevious.TranslationKey = null;
			this.btnPrevious.UseVisualStyleBackColor = false;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnPrintPreview
			// 
			this.btnPrintPreview.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.btnPrintPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrintPreview.BackgroundImage")));
			this.btnPrintPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrintPreview.Location = new System.Drawing.Point(58, 4);
			this.btnPrintPreview.Name = "btnPrintPreview";
			this.btnPrintPreview.Size = new System.Drawing.Size(52, 30);
			this.btnPrintPreview.TabIndex = 1;
			this.toolTip1.SetToolTip(this.btnPrintPreview, "Print Preview");
			this.btnPrintPreview.TranslationKey = null;
			this.btnPrintPreview.UseVisualStyleBackColor = false;
			this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.btnPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrint.BackgroundImage")));
			this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrint.Location = new System.Drawing.Point(3, 4);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(52, 30);
			this.btnPrint.TabIndex = 0;
			this.toolTip1.SetToolTip(this.btnPrint, "Print");
			this.btnPrint.TranslationKey = null;
			this.btnPrint.UseVisualStyleBackColor = false;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// GraphFormControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "GraphFormControl";
			this.Size = new System.Drawing.Size(1287, 712);
			this.Load += new System.EventHandler(this.GraphFormControl_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panelUserOptions.ResumeLayout(false);
			this.grpBoxParameterList.ResumeLayout(false);
			this.grpBoxParameterList.PerformLayout();
			this.grpBoxParameterValues.ResumeLayout(false);
			this.grpBoxParameterValues.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panelUserOptions;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panelChart;
		private System.Windows.Forms.ComboBox cboParameters;
		private System.Windows.Forms.GroupBox grpBoxParameterList;
		private CAB.UI.Controls.CABLabel lblParameter;
		private CAB.UI.Controls.CABLabel lblParameterValues;
		private System.Windows.Forms.GroupBox grpBoxParameterValues;
		private System.Windows.Forms.ComboBox cboViewType;
		private CAB.UI.Controls.CABLabel lblViewType;
		private System.Windows.Forms.RadioButton rbtnComposite;
		private System.Windows.Forms.RadioButton rbtnSeparate;
		private CAB.UI.Controls.CABLabel lblParameterView;
		private System.Windows.Forms.Label label_YAxis;
		private System.Windows.Forms.Label label_XAxis;
		private CAB.UI.Controls.CABButton btnNext;
		private CAB.UI.Controls.CABButton btnPrevious;
		private CAB.UI.Controls.CABButton btnPrintPreview;
		private CAB.UI.Controls.CABButton btnPrint;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ComboBox cboChartType;
		private CAB.UI.Controls.CABLabel lblChartType;
		private System.Windows.Forms.CheckedListBox chkLBParameterDisplay;
		public System.Windows.Forms.Label lblDateTime;
		public System.Windows.Forms.Label lblRphaseValue;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkShowGridLines;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lblYPhaseValue;
		private System.Windows.Forms.Label lblxPhaseValue;
		private System.Windows.Forms.Label lblBPhaseValue;
	}
}


