namespace CAB.UI
{
    partial class DateWiseBetween
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
            this.groupBoxSelectFile = new System.Windows.Forms.GroupBox();
            this.lngGridSelectFiles = new CAB.UI.Controls.CABGridControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.DWB_lblDateFrom = new System.Windows.Forms.Label();
            this.dtPickerToDate = new System.Windows.Forms.DateTimePicker();
            this.dtPickerFromDate = new System.Windows.Forms.DateTimePicker();
            this.DWB_lblDateTo = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnSelectDateCancel = new System.Windows.Forms.Button();
            this.btnSelectDateNext = new System.Windows.Forms.Button();
            this.DWB_lblSelectFiles = new System.Windows.Forms.Label();
            this.groupBoxAvailableMeters = new System.Windows.Forms.GroupBox();
            this.lngGridAvailableMeters = new CAB.UI.Controls.CABGridControl();
            this.btnAvailableMeterPrevious = new System.Windows.Forms.Button();
            this.btnAvailableMeterCancel = new System.Windows.Forms.Button();
            this.btnAvailableMeterNext = new System.Windows.Forms.Button();
            this.g2_lblSelectedFiles = new System.Windows.Forms.Label();
            this.groupBoxTamperCategory = new System.Windows.Forms.GroupBox();
            this.lngGridTamper = new CAB.UI.Controls.CABGridControl();
            this.btnTamperCategoryPrevious = new System.Windows.Forms.Button();
            this.btnTamperCategoryCancel = new System.Windows.Forms.Button();
            this.btnTamperCategoryNext = new System.Windows.Forms.Button();
            this.DWB_lblSelectTamper = new System.Windows.Forms.Label();
            this.groupBoxParameterCategory = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radioBtnMidnightEnergy = new System.Windows.Forms.RadioButton();
            this.radioBtnTransactionParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnTamperParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnLoadSurveyParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnBillingParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnGeneralParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnInstantParameter = new System.Windows.Forms.RadioButton();
            this.btnParameterCategoryPrevious = new System.Windows.Forms.Button();
            this.btnParameterCategoryCancel = new System.Windows.Forms.Button();
            this.btnParameterCategoryNext = new System.Windows.Forms.Button();
            this.DWB_lblSelectParameter = new System.Windows.Forms.Label();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.lblNotification = new System.Windows.Forms.Label();
            this.btnParametersPrevious = new System.Windows.Forms.Button();
            this.btnParametersCancel = new System.Windows.Forms.Button();
            this.btnShowParameters = new System.Windows.Forms.Button();
            this.DWB_lblSelectParameters = new System.Windows.Forms.Label();
            this.chkListSelectParameters = new System.Windows.Forms.CheckedListBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdbSelfDiagnosis = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectFile.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBoxAvailableMeters.SuspendLayout();
            this.groupBoxTamperCategory.SuspendLayout();
            this.groupBoxParameterCategory.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSelectFile
            // 
            this.groupBoxSelectFile.Controls.Add(this.lngGridSelectFiles);
            this.groupBoxSelectFile.Controls.Add(this.groupBox6);
            this.groupBoxSelectFile.Controls.Add(this.btnSelectDateCancel);
            this.groupBoxSelectFile.Controls.Add(this.btnSelectDateNext);
            this.groupBoxSelectFile.Controls.Add(this.DWB_lblSelectFiles);
            this.groupBoxSelectFile.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSelectFile.Name = "groupBoxSelectFile";
            this.groupBoxSelectFile.Size = new System.Drawing.Size(350, 350);
            this.groupBoxSelectFile.TabIndex = 4;
            this.groupBoxSelectFile.TabStop = false;
            // 
            // lngGridSelectFiles
            // 
            this.lngGridSelectFiles.Data = null;
            this.lngGridSelectFiles.HiddenColumn = null;
            this.lngGridSelectFiles.HiddenColumns = null;
            this.lngGridSelectFiles.IsEqual = false;
            this.lngGridSelectFiles.IsFullRow = false;
            this.lngGridSelectFiles.IsSorting = false;
            this.lngGridSelectFiles.Location = new System.Drawing.Point(6, 96);
            this.lngGridSelectFiles.Name = "lngGridSelectFiles";
            this.lngGridSelectFiles.SelectedIndex = 0;
            this.lngGridSelectFiles.SelectedRowId = "";
            this.lngGridSelectFiles.Size = new System.Drawing.Size(338, 217);
            this.lngGridSelectFiles.TabIndex = 7;
            this.lngGridSelectFiles.ValueColumn = null;
            this.lngGridSelectFiles.OnGridRowChanged += new CAB.UI.Controls.CABGridControl.GridRowChanged(this.lngGridSelectFiles_OnGridRowChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.DWB_lblDateFrom);
            this.groupBox6.Controls.Add(this.dtPickerToDate);
            this.groupBox6.Controls.Add(this.dtPickerFromDate);
            this.groupBox6.Controls.Add(this.DWB_lblDateTo);
            this.groupBox6.Controls.Add(this.btnView);
            this.groupBox6.Location = new System.Drawing.Point(6, 11);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(338, 64);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Select Uploading Dates : ";
            // 
            // DWB_lblDateFrom
            // 
            this.DWB_lblDateFrom.AutoSize = true;
            this.DWB_lblDateFrom.Location = new System.Drawing.Point(25, 22);
            this.DWB_lblDateFrom.Name = "DWB_lblDateFrom";
            this.DWB_lblDateFrom.Size = new System.Drawing.Size(56, 13);
            this.DWB_lblDateFrom.TabIndex = 7;
            this.DWB_lblDateFrom.Text = "From Date";
            // 
            // dtPickerToDate
            // 
            this.dtPickerToDate.CustomFormat = "dd/MM/yyyy";
            this.dtPickerToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerToDate.Location = new System.Drawing.Point(140, 37);
            this.dtPickerToDate.Name = "dtPickerToDate";
            this.dtPickerToDate.Size = new System.Drawing.Size(98, 20);
            this.dtPickerToDate.TabIndex = 2;
            this.dtPickerToDate.ValueChanged += new System.EventHandler(this.dtPickerToDate_ValueChanged);
            // 
            // dtPickerFromDate
            // 
            this.dtPickerFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtPickerFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerFromDate.Location = new System.Drawing.Point(27, 37);
            this.dtPickerFromDate.Name = "dtPickerFromDate";
            this.dtPickerFromDate.Size = new System.Drawing.Size(98, 20);
            this.dtPickerFromDate.TabIndex = 1;
            this.dtPickerFromDate.ValueChanged += new System.EventHandler(this.dtPickerFromDate_ValueChanged);
            // 
            // DWB_lblDateTo
            // 
            this.DWB_lblDateTo.AutoSize = true;
            this.DWB_lblDateTo.Location = new System.Drawing.Point(137, 22);
            this.DWB_lblDateTo.Name = "DWB_lblDateTo";
            this.DWB_lblDateTo.Size = new System.Drawing.Size(46, 13);
            this.DWB_lblDateTo.TabIndex = 8;
            this.DWB_lblDateTo.Text = "To Date";
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(253, 34);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(55, 23);
            this.btnView.TabIndex = 3;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnView.ForeColor = System.Drawing.Color.White;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnSelectDateCancel
            // 
            this.btnSelectDateCancel.Location = new System.Drawing.Point(269, 319);
            this.btnSelectDateCancel.Name = "btnSelectDateCancel";
            this.btnSelectDateCancel.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDateCancel.TabIndex = 6;
            this.btnSelectDateCancel.Text = "Cancel";
            this.btnSelectDateCancel.UseVisualStyleBackColor = false;
            this.btnSelectDateCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSelectDateCancel.ForeColor = System.Drawing.Color.White;
            this.btnSelectDateCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectDateCancel.FlatAppearance.BorderSize = 0;
            this.btnSelectDateCancel.Click += new System.EventHandler(this.btnSelectDateCancel_Click);
            // 
            // btnSelectDateNext
            // 
            this.btnSelectDateNext.Enabled = false;
            this.btnSelectDateNext.Location = new System.Drawing.Point(188, 319);
            this.btnSelectDateNext.Name = "btnSelectDateNext";
            this.btnSelectDateNext.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDateNext.TabIndex = 5;
            this.btnSelectDateNext.Text = "Next >>";
            this.btnSelectDateNext.UseVisualStyleBackColor = false;
            this.btnSelectDateNext.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSelectDateNext.ForeColor = System.Drawing.Color.White;
            this.btnSelectDateNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectDateNext.FlatAppearance.BorderSize = 0;
            this.btnSelectDateNext.Click += new System.EventHandler(this.btnSelectDateNext_Click);
            // 
            // DWB_lblSelectFiles
            // 
            this.DWB_lblSelectFiles.AutoSize = true;
            this.DWB_lblSelectFiles.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DWB_lblSelectFiles.Location = new System.Drawing.Point(6, 78);
            this.DWB_lblSelectFiles.Name = "DWB_lblSelectFiles";
            this.DWB_lblSelectFiles.Size = new System.Drawing.Size(79, 14);
            this.DWB_lblSelectFiles.TabIndex = 6;
            this.DWB_lblSelectFiles.Text = "Select Files : ";
            // 
            // groupBoxAvailableMeters
            // 
            this.groupBoxAvailableMeters.Controls.Add(this.lngGridAvailableMeters);
            this.groupBoxAvailableMeters.Controls.Add(this.btnAvailableMeterPrevious);
            this.groupBoxAvailableMeters.Controls.Add(this.btnAvailableMeterCancel);
            this.groupBoxAvailableMeters.Controls.Add(this.btnAvailableMeterNext);
            this.groupBoxAvailableMeters.Controls.Add(this.g2_lblSelectedFiles);
            this.groupBoxAvailableMeters.Location = new System.Drawing.Point(377, 12);
            this.groupBoxAvailableMeters.Name = "groupBoxAvailableMeters";
            this.groupBoxAvailableMeters.Size = new System.Drawing.Size(350, 350);
            this.groupBoxAvailableMeters.TabIndex = 7;
            this.groupBoxAvailableMeters.TabStop = false;
            this.groupBoxAvailableMeters.Visible = false;
            // 
            // lngGridAvailableMeters
            // 
            this.lngGridAvailableMeters.Data = null;
            this.lngGridAvailableMeters.HiddenColumn = null;
            this.lngGridAvailableMeters.HiddenColumns = null;
            this.lngGridAvailableMeters.IsEqual = false;
            this.lngGridAvailableMeters.IsFullRow = false;
            this.lngGridAvailableMeters.IsSorting = false;
            this.lngGridAvailableMeters.Location = new System.Drawing.Point(6, 33);
            this.lngGridAvailableMeters.Name = "lngGridAvailableMeters";
            this.lngGridAvailableMeters.SelectedIndex = 0;
            this.lngGridAvailableMeters.SelectedRowId = "";
            this.lngGridAvailableMeters.Size = new System.Drawing.Size(338, 280);
            this.lngGridAvailableMeters.TabIndex = 13;
            this.lngGridAvailableMeters.ValueColumn = null;
            // 
            // btnAvailableMeterPrevious
            // 
            this.btnAvailableMeterPrevious.Location = new System.Drawing.Point(107, 319);
            this.btnAvailableMeterPrevious.Name = "btnAvailableMeterPrevious";
            this.btnAvailableMeterPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnAvailableMeterPrevious.TabIndex = 10;
            this.btnAvailableMeterPrevious.Text = "<< Previous";
            this.btnAvailableMeterPrevious.UseVisualStyleBackColor = false;
            this.btnAvailableMeterPrevious.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAvailableMeterPrevious.ForeColor = System.Drawing.Color.White;
            this.btnAvailableMeterPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvailableMeterPrevious.FlatAppearance.BorderSize = 0;
            this.btnAvailableMeterPrevious.Click += new System.EventHandler(this.btnAvailableMeterPrevious_Click);
            // 
            // btnAvailableMeterCancel
            // 
            this.btnAvailableMeterCancel.Location = new System.Drawing.Point(269, 319);
            this.btnAvailableMeterCancel.Name = "btnAvailableMeterCancel";
            this.btnAvailableMeterCancel.Size = new System.Drawing.Size(75, 23);
            this.btnAvailableMeterCancel.TabIndex = 9;
            this.btnAvailableMeterCancel.Text = "Cancel";
            this.btnAvailableMeterCancel.UseVisualStyleBackColor = false;
            this.btnAvailableMeterCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAvailableMeterCancel.ForeColor = System.Drawing.Color.White;
            this.btnAvailableMeterCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvailableMeterCancel.FlatAppearance.BorderSize = 0;
            this.btnAvailableMeterCancel.Click += new System.EventHandler(this.btnAvailableMeterCancel_Click);
            // 
            // btnAvailableMeterNext
            // 
            this.btnAvailableMeterNext.Location = new System.Drawing.Point(188, 319);
            this.btnAvailableMeterNext.Name = "btnAvailableMeterNext";
            this.btnAvailableMeterNext.Size = new System.Drawing.Size(75, 23);
            this.btnAvailableMeterNext.TabIndex = 8;
            this.btnAvailableMeterNext.Text = "Next >>";
            this.btnAvailableMeterNext.UseVisualStyleBackColor = false;
            this.btnAvailableMeterNext.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAvailableMeterNext.ForeColor = System.Drawing.Color.White;
            this.btnAvailableMeterNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAvailableMeterNext.FlatAppearance.BorderSize = 0;
            this.btnAvailableMeterNext.Click += new System.EventHandler(this.btnAvailableMeterNext_Click);
            // 
            // g2_lblSelectedFiles
            // 
            this.g2_lblSelectedFiles.AutoSize = true;
            this.g2_lblSelectedFiles.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g2_lblSelectedFiles.Location = new System.Drawing.Point(3, 11);
            this.g2_lblSelectedFiles.Name = "g2_lblSelectedFiles";
            this.g2_lblSelectedFiles.Size = new System.Drawing.Size(210, 14);
            this.g2_lblSelectedFiles.TabIndex = 12;
            this.g2_lblSelectedFiles.Text = "Meters Available for Selected Files :  ";
            // 
            // groupBoxTamperCategory
            // 
            this.groupBoxTamperCategory.Controls.Add(this.lngGridTamper);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryPrevious);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryCancel);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryNext);
            this.groupBoxTamperCategory.Controls.Add(this.DWB_lblSelectTamper);
            this.groupBoxTamperCategory.Location = new System.Drawing.Point(745, 12);
            this.groupBoxTamperCategory.Name = "groupBoxTamperCategory";
            this.groupBoxTamperCategory.Size = new System.Drawing.Size(350, 350);
            this.groupBoxTamperCategory.TabIndex = 13;
            this.groupBoxTamperCategory.TabStop = false;
            this.groupBoxTamperCategory.Visible = false;
            // 
            // lngGridTamper
            // 
            this.lngGridTamper.Data = null;
            this.lngGridTamper.HiddenColumn = null;
            this.lngGridTamper.HiddenColumns = null;
            this.lngGridTamper.IsEqual = false;
            this.lngGridTamper.IsFullRow = false;
            this.lngGridTamper.IsSorting = false;
            this.lngGridTamper.Location = new System.Drawing.Point(9, 33);
            this.lngGridTamper.Name = "lngGridTamper";
            this.lngGridTamper.SelectedIndex = 0;
            this.lngGridTamper.SelectedRowId = "";
            this.lngGridTamper.Size = new System.Drawing.Size(335, 280);
            this.lngGridTamper.TabIndex = 19;
            this.lngGridTamper.ValueColumn = null;
            // 
            // btnTamperCategoryPrevious
            // 
            this.btnTamperCategoryPrevious.Location = new System.Drawing.Point(107, 319);
            this.btnTamperCategoryPrevious.Name = "btnTamperCategoryPrevious";
            this.btnTamperCategoryPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryPrevious.TabIndex = 16;
            this.btnTamperCategoryPrevious.Text = "<< Previous";
            this.btnTamperCategoryPrevious.UseVisualStyleBackColor = false;
            this.btnTamperCategoryPrevious.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnTamperCategoryPrevious.ForeColor = System.Drawing.Color.White;
            this.btnTamperCategoryPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTamperCategoryPrevious.FlatAppearance.BorderSize = 0;
            this.btnTamperCategoryPrevious.Click += new System.EventHandler(this.btnTamperCategoryPrevious_Click);
            // 
            // btnTamperCategoryCancel
            // 
            this.btnTamperCategoryCancel.Location = new System.Drawing.Point(269, 319);
            this.btnTamperCategoryCancel.Name = "btnTamperCategoryCancel";
            this.btnTamperCategoryCancel.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryCancel.TabIndex = 15;
            this.btnTamperCategoryCancel.Text = "Cancel";
            this.btnTamperCategoryCancel.UseVisualStyleBackColor = false;
            this.btnTamperCategoryCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnTamperCategoryCancel.ForeColor = System.Drawing.Color.White;
            this.btnTamperCategoryCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTamperCategoryCancel.FlatAppearance.BorderSize = 0;
            this.btnTamperCategoryCancel.Click += new System.EventHandler(this.btnTamperCategoryCancel_Click);
            // 
            // btnTamperCategoryNext
            // 
            this.btnTamperCategoryNext.Location = new System.Drawing.Point(188, 319);
            this.btnTamperCategoryNext.Name = "btnTamperCategoryNext";
            this.btnTamperCategoryNext.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryNext.TabIndex = 14;
            this.btnTamperCategoryNext.Text = "Next >>";
            this.btnTamperCategoryNext.UseVisualStyleBackColor = false;
            this.btnTamperCategoryNext.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnTamperCategoryNext.ForeColor = System.Drawing.Color.White;
            this.btnTamperCategoryNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTamperCategoryNext.FlatAppearance.BorderSize = 0;
            this.btnTamperCategoryNext.Click += new System.EventHandler(this.btnTamperCategoryNext_Click);
            // 
            // DWB_lblSelectTamper
            // 
            this.DWB_lblSelectTamper.AutoSize = true;
            this.DWB_lblSelectTamper.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DWB_lblSelectTamper.Location = new System.Drawing.Point(6, 11);
            this.DWB_lblSelectTamper.Name = "DWB_lblSelectTamper";
            this.DWB_lblSelectTamper.Size = new System.Drawing.Size(148, 14);
            this.DWB_lblSelectTamper.TabIndex = 18;
            this.DWB_lblSelectTamper.Text = "Select Tamper Category : ";
            // 
            // groupBoxParameterCategory
            // 
            this.groupBoxParameterCategory.Controls.Add(this.groupBox7);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryPrevious);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryCancel);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryNext);
            this.groupBoxParameterCategory.Controls.Add(this.DWB_lblSelectParameter);
            this.groupBoxParameterCategory.Location = new System.Drawing.Point(12, 373);
            this.groupBoxParameterCategory.Name = "groupBoxParameterCategory";
            this.groupBoxParameterCategory.Size = new System.Drawing.Size(350, 350);
            this.groupBoxParameterCategory.TabIndex = 19;
            this.groupBoxParameterCategory.TabStop = false;
            this.groupBoxParameterCategory.Visible = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rdbSelfDiagnosis);
            this.groupBox7.Controls.Add(this.radioBtnMidnightEnergy);
            this.groupBox7.Controls.Add(this.radioBtnTransactionParameter);
            this.groupBox7.Controls.Add(this.radioBtnTamperParameter);
            this.groupBox7.Controls.Add(this.radioBtnLoadSurveyParameter);
            this.groupBox7.Controls.Add(this.radioBtnBillingParameter);
            this.groupBox7.Controls.Add(this.radioBtnGeneralParameter);
            this.groupBox7.Controls.Add(this.radioBtnInstantParameter);
            this.groupBox7.Location = new System.Drawing.Point(37, 36);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(280, 247);
            this.groupBox7.TabIndex = 29;
            this.groupBox7.TabStop = false;
            // 
            // radioBtnMidnightEnergy
            // 
            this.radioBtnMidnightEnergy.AutoSize = true;
            this.radioBtnMidnightEnergy.Location = new System.Drawing.Point(72, 173);
            this.radioBtnMidnightEnergy.Name = "radioBtnMidnightEnergy";
            this.radioBtnMidnightEnergy.Size = new System.Drawing.Size(109, 17);
            this.radioBtnMidnightEnergy.TabIndex = 31;
            this.radioBtnMidnightEnergy.Text = "Midnight Energies";
            this.radioBtnMidnightEnergy.UseVisualStyleBackColor = true;
            // 
            // radioBtnTransactionParameter
            // 
            this.radioBtnTransactionParameter.AutoSize = true;
            this.radioBtnTransactionParameter.Location = new System.Drawing.Point(72, 150);
            this.radioBtnTransactionParameter.Name = "radioBtnTransactionParameter";
            this.radioBtnTransactionParameter.Size = new System.Drawing.Size(132, 17);
            this.radioBtnTransactionParameter.TabIndex = 30;
            this.radioBtnTransactionParameter.Text = "Transaction Parameter";
            this.radioBtnTransactionParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnTamperParameter
            // 
            this.radioBtnTamperParameter.AutoSize = true;
            this.radioBtnTamperParameter.Location = new System.Drawing.Point(73, 127);
            this.radioBtnTamperParameter.Name = "radioBtnTamperParameter";
            this.radioBtnTamperParameter.Size = new System.Drawing.Size(112, 17);
            this.radioBtnTamperParameter.TabIndex = 29;
            this.radioBtnTamperParameter.Text = "Tamper Parameter";
            this.radioBtnTamperParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnLoadSurveyParameter
            // 
            this.radioBtnLoadSurveyParameter.AutoSize = true;
            this.radioBtnLoadSurveyParameter.Location = new System.Drawing.Point(73, 104);
            this.radioBtnLoadSurveyParameter.Name = "radioBtnLoadSurveyParameter";
            this.radioBtnLoadSurveyParameter.Size = new System.Drawing.Size(136, 17);
            this.radioBtnLoadSurveyParameter.TabIndex = 28;
            this.radioBtnLoadSurveyParameter.Text = "Load Survey Parameter";
            this.radioBtnLoadSurveyParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnBillingParameter
            // 
            this.radioBtnBillingParameter.AutoSize = true;
            this.radioBtnBillingParameter.Location = new System.Drawing.Point(73, 81);
            this.radioBtnBillingParameter.Name = "radioBtnBillingParameter";
            this.radioBtnBillingParameter.Size = new System.Drawing.Size(103, 17);
            this.radioBtnBillingParameter.TabIndex = 27;
            this.radioBtnBillingParameter.Text = "Billing Parameter";
            this.radioBtnBillingParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnGeneralParameter
            // 
            this.radioBtnGeneralParameter.AutoSize = true;
            this.radioBtnGeneralParameter.Checked = true;
            this.radioBtnGeneralParameter.Location = new System.Drawing.Point(72, 35);
            this.radioBtnGeneralParameter.Name = "radioBtnGeneralParameter";
            this.radioBtnGeneralParameter.Size = new System.Drawing.Size(113, 17);
            this.radioBtnGeneralParameter.TabIndex = 26;
            this.radioBtnGeneralParameter.TabStop = true;
            this.radioBtnGeneralParameter.Text = "General Parameter";
            this.radioBtnGeneralParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnInstantParameter
            // 
            this.radioBtnInstantParameter.AutoSize = true;
            this.radioBtnInstantParameter.Location = new System.Drawing.Point(73, 58);
            this.radioBtnInstantParameter.Name = "radioBtnInstantParameter";
            this.radioBtnInstantParameter.Size = new System.Drawing.Size(108, 17);
            this.radioBtnInstantParameter.TabIndex = 25;
            this.radioBtnInstantParameter.Text = "Instant Parameter";
            this.radioBtnInstantParameter.UseVisualStyleBackColor = true;
            // 
            // btnParameterCategoryPrevious
            // 
            this.btnParameterCategoryPrevious.Location = new System.Drawing.Point(107, 317);
            this.btnParameterCategoryPrevious.Name = "btnParameterCategoryPrevious";
            this.btnParameterCategoryPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryPrevious.TabIndex = 27;
            this.btnParameterCategoryPrevious.Text = "<< Previous";
            this.btnParameterCategoryPrevious.UseVisualStyleBackColor = false;
            this.btnParameterCategoryPrevious.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnParameterCategoryPrevious.ForeColor = System.Drawing.Color.White;
            this.btnParameterCategoryPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParameterCategoryPrevious.FlatAppearance.BorderSize = 0;
            this.btnParameterCategoryPrevious.Click += new System.EventHandler(this.btnParameterCategoryPrevious_Click);
            // 
            // btnParameterCategoryCancel
            // 
            this.btnParameterCategoryCancel.Location = new System.Drawing.Point(269, 317);
            this.btnParameterCategoryCancel.Name = "btnParameterCategoryCancel";
            this.btnParameterCategoryCancel.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryCancel.TabIndex = 26;
            this.btnParameterCategoryCancel.Text = "Cancel";
            this.btnParameterCategoryCancel.UseVisualStyleBackColor = false;
            this.btnParameterCategoryCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnParameterCategoryCancel.ForeColor = System.Drawing.Color.White;
            this.btnParameterCategoryCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParameterCategoryCancel.FlatAppearance.BorderSize = 0;
            this.btnParameterCategoryCancel.Click += new System.EventHandler(this.btnParameterCategoryCancel_Click);
            // 
            // btnParameterCategoryNext
            // 
            this.btnParameterCategoryNext.Location = new System.Drawing.Point(188, 317);
            this.btnParameterCategoryNext.Name = "btnParameterCategoryNext";
            this.btnParameterCategoryNext.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryNext.TabIndex = 25;
            this.btnParameterCategoryNext.Text = "Next >>";
            this.btnParameterCategoryNext.UseVisualStyleBackColor = false;
            this.btnParameterCategoryNext.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnParameterCategoryNext.ForeColor = System.Drawing.Color.White;
            this.btnParameterCategoryNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParameterCategoryNext.FlatAppearance.BorderSize = 0;
            this.btnParameterCategoryNext.Click += new System.EventHandler(this.btnParameterCategoryNext_Click);
            // 
            // DWB_lblSelectParameter
            // 
            this.DWB_lblSelectParameter.AutoSize = true;
            this.DWB_lblSelectParameter.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DWB_lblSelectParameter.Location = new System.Drawing.Point(6, 16);
            this.DWB_lblSelectParameter.Name = "DWB_lblSelectParameter";
            this.DWB_lblSelectParameter.Size = new System.Drawing.Size(164, 14);
            this.DWB_lblSelectParameter.TabIndex = 28;
            this.DWB_lblSelectParameter.Text = "Select Parameter Category : ";
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.lblNotification);
            this.groupBoxParameters.Controls.Add(this.btnParametersPrevious);
            this.groupBoxParameters.Controls.Add(this.btnParametersCancel);
            this.groupBoxParameters.Controls.Add(this.btnShowParameters);
            this.groupBoxParameters.Controls.Add(this.DWB_lblSelectParameters);
            this.groupBoxParameters.Controls.Add(this.chkListSelectParameters);
            this.groupBoxParameters.Location = new System.Drawing.Point(377, 373);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(350, 350);
            this.groupBoxParameters.TabIndex = 29;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Visible = false;
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Location = new System.Drawing.Point(3, 296);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(239, 13);
            this.lblNotification.TabIndex = 35;
            this.lblNotification.Text = "Note : Maximum of 8 Parameters can be selected";
            // 
            // btnParametersPrevious
            // 
            this.btnParametersPrevious.Location = new System.Drawing.Point(107, 317);
            this.btnParametersPrevious.Name = "btnParametersPrevious";
            this.btnParametersPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnParametersPrevious.TabIndex = 32;
            this.btnParametersPrevious.Text = "<< Previous";
            this.btnParametersPrevious.UseVisualStyleBackColor = false;
            this.btnParametersPrevious.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnParametersPrevious.ForeColor = System.Drawing.Color.White;
            this.btnParametersPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParametersPrevious.FlatAppearance.BorderSize = 0;
            this.btnParametersPrevious.Click += new System.EventHandler(this.btnParametersPrevious_Click);
            // 
            // btnParametersCancel
            // 
            this.btnParametersCancel.Location = new System.Drawing.Point(269, 317);
            this.btnParametersCancel.Name = "btnParametersCancel";
            this.btnParametersCancel.Size = new System.Drawing.Size(75, 23);
            this.btnParametersCancel.TabIndex = 31;
            this.btnParametersCancel.Text = "Cancel";
            this.btnParametersCancel.UseVisualStyleBackColor = false;
            this.btnParametersCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnParametersCancel.ForeColor = System.Drawing.Color.White;
            this.btnParametersCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParametersCancel.FlatAppearance.BorderSize = 0;
            this.btnParametersCancel.Click += new System.EventHandler(this.btnParametersCancel_Click);
            // 
            // btnShowParameters
            // 
            this.btnShowParameters.Location = new System.Drawing.Point(188, 317);
            this.btnShowParameters.Name = "btnShowParameters";
            this.btnShowParameters.Size = new System.Drawing.Size(75, 23);
            this.btnShowParameters.TabIndex = 30;
            this.btnShowParameters.Text = "Show";
            this.btnShowParameters.UseVisualStyleBackColor = false;
            this.btnShowParameters.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShowParameters.ForeColor = System.Drawing.Color.White;
            this.btnShowParameters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowParameters.FlatAppearance.BorderSize = 0;
            this.btnShowParameters.Click += new System.EventHandler(this.btnShowParameters_Click);
            // 
            // DWB_lblSelectParameters
            // 
            this.DWB_lblSelectParameters.AutoSize = true;
            this.DWB_lblSelectParameters.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DWB_lblSelectParameters.Location = new System.Drawing.Point(6, 16);
            this.DWB_lblSelectParameters.Name = "DWB_lblSelectParameters";
            this.DWB_lblSelectParameters.Size = new System.Drawing.Size(118, 14);
            this.DWB_lblSelectParameters.TabIndex = 34;
            this.DWB_lblSelectParameters.Text = "Select Parameters : ";
            // 
            // chkListSelectParameters
            // 
            this.chkListSelectParameters.CheckOnClick = true;
            this.chkListSelectParameters.FormattingEnabled = true;
            this.chkListSelectParameters.Location = new System.Drawing.Point(6, 36);
            this.chkListSelectParameters.Name = "chkListSelectParameters";
            this.chkListSelectParameters.Size = new System.Drawing.Size(338, 259);
            this.chkListSelectParameters.TabIndex = 33;
            this.chkListSelectParameters.SelectedIndexChanged += new System.EventHandler(this.chkListSelectParameters_SelectedIndexChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "S.No";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.Frozen = true;
            this.dataGridViewTextBoxColumn2.HeaderText = "File Names";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 174;
            // 
            // rdbSelfDiagnosis
            // 
            this.rdbSelfDiagnosis.AutoSize = true;
            this.rdbSelfDiagnosis.Location = new System.Drawing.Point(73, 196);
            this.rdbSelfDiagnosis.Name = "rdbSelfDiagnosis";
            this.rdbSelfDiagnosis.Size = new System.Drawing.Size(101, 17);
            this.rdbSelfDiagnosis.TabIndex = 32;
            this.rdbSelfDiagnosis.TabStop = true;
            this.rdbSelfDiagnosis.Text = "Self Diagnostics";
            this.rdbSelfDiagnosis.UseVisualStyleBackColor = true;
            // 
            // DateWiseBetween
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1103, 735);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxAvailableMeters);
            this.Controls.Add(this.groupBoxTamperCategory);
            this.Controls.Add(this.groupBoxParameterCategory);
            this.Controls.Add(this.groupBoxSelectFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DateWiseBetween";
            this.Text = "Date Wise";
            this.Load += new System.EventHandler(this.DateWiseBetween_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DateWiseBetween_FormClosing);
            this.groupBoxSelectFile.ResumeLayout(false);
            this.groupBoxSelectFile.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBoxAvailableMeters.ResumeLayout(false);
            this.groupBoxAvailableMeters.PerformLayout();
            this.groupBoxTamperCategory.ResumeLayout(false);
            this.groupBoxTamperCategory.PerformLayout();
            this.groupBoxParameterCategory.ResumeLayout(false);
            this.groupBoxParameterCategory.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSelectFile;
        private System.Windows.Forms.GroupBox groupBoxAvailableMeters;
        private System.Windows.Forms.GroupBox groupBoxTamperCategory;
        private System.Windows.Forms.GroupBox groupBoxParameterCategory;
        private System.Windows.Forms.Label DWB_lblDateFrom;
        private System.Windows.Forms.Label DWB_lblDateTo;
        private System.Windows.Forms.Label DWB_lblSelectFiles;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnSelectDateCancel;
        private System.Windows.Forms.Button btnSelectDateNext;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DateTimePicker dtPickerFromDate;
        private System.Windows.Forms.DateTimePicker dtPickerToDate;
        private System.Windows.Forms.Label g2_lblSelectedFiles;
        private System.Windows.Forms.Button btnAvailableMeterCancel;
        private System.Windows.Forms.Button btnAvailableMeterNext;
        private System.Windows.Forms.Button btnAvailableMeterPrevious;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Label DWB_lblSelectTamper;
        private System.Windows.Forms.Button btnTamperCategoryPrevious;
        private System.Windows.Forms.Button btnTamperCategoryCancel;
        private System.Windows.Forms.Button btnTamperCategoryNext;
        private System.Windows.Forms.Label DWB_lblSelectParameter;
        private System.Windows.Forms.Button btnParameterCategoryPrevious;
        private System.Windows.Forms.Button btnParameterCategoryCancel;
        private System.Windows.Forms.Button btnParameterCategoryNext;
        private System.Windows.Forms.CheckedListBox chkListSelectParameters;
        private System.Windows.Forms.Label DWB_lblSelectParameters;
        private System.Windows.Forms.Button btnParametersPrevious;
        private System.Windows.Forms.Button btnParametersCancel;
        private System.Windows.Forms.Button btnShowParameters;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton radioBtnTamperParameter;
        private System.Windows.Forms.RadioButton radioBtnLoadSurveyParameter;
        private System.Windows.Forms.RadioButton radioBtnBillingParameter;
        private System.Windows.Forms.RadioButton radioBtnGeneralParameter;
        private System.Windows.Forms.RadioButton radioBtnInstantParameter;
        private System.Windows.Forms.Label lblNotification;
        private CAB.UI.Controls.CABGridControl lngGridSelectFiles;
        private CAB.UI.Controls.CABGridControl lngGridAvailableMeters;
        private CAB.UI.Controls.CABGridControl lngGridTamper;
        private System.Windows.Forms.RadioButton radioBtnTransactionParameter;
        private System.Windows.Forms.RadioButton radioBtnMidnightEnergy;
        private System.Windows.Forms.RadioButton rdbSelfDiagnosis;
    }
}

