namespace CAB.UI
{
    partial class MeterWise
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
            this.groupBoxSelectMeter = new System.Windows.Forms.GroupBox();
            this.lngGridAvailableFiles = new CAB.UI.Controls.CABGridControl();
            this.btnMeterCancel = new System.Windows.Forms.Button();
            this.btnMeterNext = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtBoxSelectMeter = new System.Windows.Forms.TextBox();
            this.LFW_lblSelectFile = new System.Windows.Forms.Label();
            this.MW_lblFilesAvailable = new System.Windows.Forms.Label();
            this.groupBoxParameterCategory = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioBtnTamperParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnLoadSurveyParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnBillingParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnGeneralParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnInstantParameter = new System.Windows.Forms.RadioButton();
            this.btnParameterPrevious = new System.Windows.Forms.Button();
            this.btnParameterCancel = new System.Windows.Forms.Button();
            this.btnParameterNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxTamperCategory = new System.Windows.Forms.GroupBox();
            this.lngGridTamper = new CAB.UI.Controls.CABGridControl();
            this.btnTamperCategoryNext = new System.Windows.Forms.Button();
            this.btnTamperPrevious = new System.Windows.Forms.Button();
            this.btnTamperCancel = new System.Windows.Forms.Button();
            this.g4_lblSelectParameters = new System.Windows.Forms.Label();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.btnReportParamsPrevious = new System.Windows.Forms.Button();
            this.chkListSelectParameters = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.btnReportParamsCancel = new System.Windows.Forms.Button();
            this.g3_lblSelectParameters = new System.Windows.Forms.Label();
            this.groupBoxSelectMeter.SuspendLayout();
            this.groupBoxParameterCategory.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBoxTamperCategory.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSelectMeter
            // 
            this.groupBoxSelectMeter.Controls.Add(this.lngGridAvailableFiles);
            this.groupBoxSelectMeter.Controls.Add(this.btnMeterCancel);
            this.groupBoxSelectMeter.Controls.Add(this.btnMeterNext);
            this.groupBoxSelectMeter.Controls.Add(this.btnBrowse);
            this.groupBoxSelectMeter.Controls.Add(this.txtBoxSelectMeter);
            this.groupBoxSelectMeter.Controls.Add(this.LFW_lblSelectFile);
            this.groupBoxSelectMeter.Controls.Add(this.MW_lblFilesAvailable);
            this.groupBoxSelectMeter.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSelectMeter.Name = "groupBoxSelectMeter";
            this.groupBoxSelectMeter.Size = new System.Drawing.Size(350, 350);
            this.groupBoxSelectMeter.TabIndex = 0;
            this.groupBoxSelectMeter.TabStop = false;
            // 
            // lngGridAvailableFiles
            // 
            this.lngGridAvailableFiles.Data = null;
            this.lngGridAvailableFiles.HiddenColumn = null;
            this.lngGridAvailableFiles.HiddenColumns = null;
            this.lngGridAvailableFiles.IsEqual = false;
            this.lngGridAvailableFiles.IsFullRow = false;
            this.lngGridAvailableFiles.IsSorting = false;
            this.lngGridAvailableFiles.Location = new System.Drawing.Point(12, 82);
            this.lngGridAvailableFiles.Name = "lngGridAvailableFiles";
            this.lngGridAvailableFiles.SelectedIndex = 0;
            this.lngGridAvailableFiles.Size = new System.Drawing.Size(327, 197);
            this.lngGridAvailableFiles.TabIndex = 8;
            this.lngGridAvailableFiles.ValueColumn = null;
            // 
            // btnMeterCancel
            // 
            this.btnMeterCancel.Location = new System.Drawing.Point(264, 303);
            this.btnMeterCancel.Name = "btnMeterCancel";
            this.btnMeterCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMeterCancel.TabIndex = 4;
            this.btnMeterCancel.Text = "Cancel";
            this.btnMeterCancel.UseVisualStyleBackColor = true;
            this.btnMeterCancel.Click += new System.EventHandler(this.btnMeterCancel_Click);
            // 
            // btnMeterNext
            // 
            this.btnMeterNext.Enabled = false;
            this.btnMeterNext.Location = new System.Drawing.Point(183, 303);
            this.btnMeterNext.Name = "btnMeterNext";
            this.btnMeterNext.Size = new System.Drawing.Size(75, 23);
            this.btnMeterNext.TabIndex = 3;
            this.btnMeterNext.Text = "Next >>";
            this.btnMeterNext.UseVisualStyleBackColor = true;
            this.btnMeterNext.Click += new System.EventHandler(this.btnMeterNext_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(310, 32);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(29, 20);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtBoxSelectMeter
            // 
            this.txtBoxSelectMeter.BackColor = System.Drawing.Color.White;
            this.txtBoxSelectMeter.Location = new System.Drawing.Point(12, 32);
            this.txtBoxSelectMeter.MaxLength = 40;
            this.txtBoxSelectMeter.Name = "txtBoxSelectMeter";
            this.txtBoxSelectMeter.ReadOnly = true;
            this.txtBoxSelectMeter.Size = new System.Drawing.Size(287, 20);
            this.txtBoxSelectMeter.TabIndex = 1;
            // 
            // LFW_lblSelectFile
            // 
            this.LFW_lblSelectFile.AutoSize = true;
            this.LFW_lblSelectFile.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LFW_lblSelectFile.Location = new System.Drawing.Point(9, 12);
            this.LFW_lblSelectFile.Name = "LFW_lblSelectFile";
            this.LFW_lblSelectFile.Size = new System.Drawing.Size(86, 14);
            this.LFW_lblSelectFile.TabIndex = 7;
            this.LFW_lblSelectFile.Text = "Select Meter : ";
            // 
            // MW_lblFilesAvailable
            // 
            this.MW_lblFilesAvailable.AutoSize = true;
            this.MW_lblFilesAvailable.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MW_lblFilesAvailable.Location = new System.Drawing.Point(9, 64);
            this.MW_lblFilesAvailable.Name = "MW_lblFilesAvailable";
            this.MW_lblFilesAvailable.Size = new System.Drawing.Size(94, 14);
            this.MW_lblFilesAvailable.TabIndex = 6;
            this.MW_lblFilesAvailable.Text = "Files Available : ";
            // 
            // groupBoxParameterCategory
            // 
            this.groupBoxParameterCategory.Controls.Add(this.groupBox5);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterPrevious);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCancel);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterNext);
            this.groupBoxParameterCategory.Controls.Add(this.label1);
            this.groupBoxParameterCategory.Location = new System.Drawing.Point(380, 12);
            this.groupBoxParameterCategory.Name = "groupBoxParameterCategory";
            this.groupBoxParameterCategory.Size = new System.Drawing.Size(350, 350);
            this.groupBoxParameterCategory.TabIndex = 9;
            this.groupBoxParameterCategory.TabStop = false;
            this.groupBoxParameterCategory.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioBtnTamperParameter);
            this.groupBox5.Controls.Add(this.radioBtnLoadSurveyParameter);
            this.groupBox5.Controls.Add(this.radioBtnBillingParameter);
            this.groupBox5.Controls.Add(this.radioBtnGeneralParameter);
            this.groupBox5.Controls.Add(this.radioBtnInstantParameter);
            this.groupBox5.Location = new System.Drawing.Point(12, 32);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(327, 247);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            // 
            // radioBtnTamperParameter
            // 
            this.radioBtnTamperParameter.AutoSize = true;
            this.radioBtnTamperParameter.Location = new System.Drawing.Point(95, 165);
            this.radioBtnTamperParameter.Name = "radioBtnTamperParameter";
            this.radioBtnTamperParameter.Size = new System.Drawing.Size(112, 17);
            this.radioBtnTamperParameter.TabIndex = 4;
            this.radioBtnTamperParameter.Text = "Tamper Parameter";
            this.radioBtnTamperParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnLoadSurveyParameter
            // 
            this.radioBtnLoadSurveyParameter.AutoSize = true;
            this.radioBtnLoadSurveyParameter.Location = new System.Drawing.Point(95, 140);
            this.radioBtnLoadSurveyParameter.Name = "radioBtnLoadSurveyParameter";
            this.radioBtnLoadSurveyParameter.Size = new System.Drawing.Size(136, 17);
            this.radioBtnLoadSurveyParameter.TabIndex = 3;
            this.radioBtnLoadSurveyParameter.Text = "Load Survey Parameter";
            this.radioBtnLoadSurveyParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnBillingParameter
            // 
            this.radioBtnBillingParameter.AutoSize = true;
            this.radioBtnBillingParameter.Location = new System.Drawing.Point(95, 115);
            this.radioBtnBillingParameter.Name = "radioBtnBillingParameter";
            this.radioBtnBillingParameter.Size = new System.Drawing.Size(103, 17);
            this.radioBtnBillingParameter.TabIndex = 2;
            this.radioBtnBillingParameter.Text = "Billing Parameter";
            this.radioBtnBillingParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnGeneralParameter
            // 
            this.radioBtnGeneralParameter.AutoSize = true;
            this.radioBtnGeneralParameter.Checked = true;
            this.radioBtnGeneralParameter.Location = new System.Drawing.Point(95, 65);
            this.radioBtnGeneralParameter.Name = "radioBtnGeneralParameter";
            this.radioBtnGeneralParameter.Size = new System.Drawing.Size(113, 17);
            this.radioBtnGeneralParameter.TabIndex = 0;
            this.radioBtnGeneralParameter.TabStop = true;
            this.radioBtnGeneralParameter.Text = "General Parameter";
            this.radioBtnGeneralParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnInstantParameter
            // 
            this.radioBtnInstantParameter.AutoSize = true;
            this.radioBtnInstantParameter.Location = new System.Drawing.Point(95, 90);
            this.radioBtnInstantParameter.Name = "radioBtnInstantParameter";
            this.radioBtnInstantParameter.Size = new System.Drawing.Size(108, 17);
            this.radioBtnInstantParameter.TabIndex = 1;
            this.radioBtnInstantParameter.Text = "Instant Parameter";
            this.radioBtnInstantParameter.UseVisualStyleBackColor = true;
            // 
            // btnParameterPrevious
            // 
            this.btnParameterPrevious.Location = new System.Drawing.Point(102, 303);
            this.btnParameterPrevious.Name = "btnParameterPrevious";
            this.btnParameterPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnParameterPrevious.TabIndex = 12;
            this.btnParameterPrevious.Text = "<< Previous";
            this.btnParameterPrevious.UseVisualStyleBackColor = true;
            this.btnParameterPrevious.Click += new System.EventHandler(this.btnParameterPrevious_Click);
            // 
            // btnParameterCancel
            // 
            this.btnParameterCancel.Location = new System.Drawing.Point(264, 303);
            this.btnParameterCancel.Name = "btnParameterCancel";
            this.btnParameterCancel.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCancel.TabIndex = 11;
            this.btnParameterCancel.Text = "Cancel";
            this.btnParameterCancel.UseVisualStyleBackColor = true;
            this.btnParameterCancel.Click += new System.EventHandler(this.btnParameterCancel_Click);
            // 
            // btnParameterNext
            // 
            this.btnParameterNext.Location = new System.Drawing.Point(183, 303);
            this.btnParameterNext.Name = "btnParameterNext";
            this.btnParameterNext.Size = new System.Drawing.Size(75, 23);
            this.btnParameterNext.TabIndex = 10;
            this.btnParameterNext.Text = "Next >>";
            this.btnParameterNext.UseVisualStyleBackColor = true;
            this.btnParameterNext.Click += new System.EventHandler(this.btnParameterNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Parameter Category : ";
            // 
            // groupBoxTamperCategory
            // 
            this.groupBoxTamperCategory.Controls.Add(this.lngGridTamper);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryNext);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperPrevious);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCancel);
            this.groupBoxTamperCategory.Controls.Add(this.g4_lblSelectParameters);
            this.groupBoxTamperCategory.Location = new System.Drawing.Point(12, 375);
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
            this.lngGridTamper.Location = new System.Drawing.Point(12, 42);
            this.lngGridTamper.Name = "lngGridTamper";
            this.lngGridTamper.SelectedIndex = 0;
            this.lngGridTamper.Size = new System.Drawing.Size(327, 245);
            this.lngGridTamper.TabIndex = 25;
            this.lngGridTamper.ValueColumn = null;
            // 
            // btnTamperCategoryNext
            // 
            this.btnTamperCategoryNext.Location = new System.Drawing.Point(183, 303);
            this.btnTamperCategoryNext.Name = "btnTamperCategoryNext";
            this.btnTamperCategoryNext.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryNext.TabIndex = 24;
            this.btnTamperCategoryNext.Text = "Next >>";
            this.btnTamperCategoryNext.UseVisualStyleBackColor = true;
            this.btnTamperCategoryNext.Click += new System.EventHandler(this.btnTamperCategoryNext_Click);
            // 
            // btnTamperPrevious
            // 
            this.btnTamperPrevious.Location = new System.Drawing.Point(102, 303);
            this.btnTamperPrevious.Name = "btnTamperPrevious";
            this.btnTamperPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnTamperPrevious.TabIndex = 23;
            this.btnTamperPrevious.Text = "<< Previous";
            this.btnTamperPrevious.UseVisualStyleBackColor = true;
            this.btnTamperPrevious.Click += new System.EventHandler(this.btnTamperPrevious_Click);
            // 
            // btnTamperCancel
            // 
            this.btnTamperCancel.Location = new System.Drawing.Point(264, 303);
            this.btnTamperCancel.Name = "btnTamperCancel";
            this.btnTamperCancel.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCancel.TabIndex = 17;
            this.btnTamperCancel.Text = "Cancel";
            this.btnTamperCancel.UseVisualStyleBackColor = true;
            this.btnTamperCancel.Click += new System.EventHandler(this.btnTamperCancel_Click);
            // 
            // g4_lblSelectParameters
            // 
            this.g4_lblSelectParameters.AutoSize = true;
            this.g4_lblSelectParameters.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g4_lblSelectParameters.Location = new System.Drawing.Point(9, 16);
            this.g4_lblSelectParameters.Name = "g4_lblSelectParameters";
            this.g4_lblSelectParameters.Size = new System.Drawing.Size(148, 14);
            this.g4_lblSelectParameters.TabIndex = 13;
            this.g4_lblSelectParameters.Text = "Select Tamper Category : ";
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.btnReportParamsPrevious);
            this.groupBoxParameters.Controls.Add(this.chkListSelectParameters);
            this.groupBoxParameters.Controls.Add(this.label2);
            this.groupBoxParameters.Controls.Add(this.btnShowReport);
            this.groupBoxParameters.Controls.Add(this.btnReportParamsCancel);
            this.groupBoxParameters.Controls.Add(this.g3_lblSelectParameters);
            this.groupBoxParameters.Location = new System.Drawing.Point(380, 375);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(350, 350);
            this.groupBoxParameters.TabIndex = 25;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Visible = false;
            // 
            // btnReportParamsPrevious
            // 
            this.btnReportParamsPrevious.Location = new System.Drawing.Point(102, 303);
            this.btnReportParamsPrevious.Name = "btnReportParamsPrevious";
            this.btnReportParamsPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnReportParamsPrevious.TabIndex = 23;
            this.btnReportParamsPrevious.Text = "<< Previous";
            this.btnReportParamsPrevious.UseVisualStyleBackColor = true;
            this.btnReportParamsPrevious.Click += new System.EventHandler(this.btnReportParamsPrevious_Click);
            // 
            // chkListSelectParameters
            // 
            this.chkListSelectParameters.CheckOnClick = true;
            this.chkListSelectParameters.FormattingEnabled = true;
            this.chkListSelectParameters.Location = new System.Drawing.Point(12, 42);
            this.chkListSelectParameters.Name = "chkListSelectParameters";
            this.chkListSelectParameters.Size = new System.Drawing.Size(327, 229);
            this.chkListSelectParameters.TabIndex = 22;
            this.chkListSelectParameters.SelectedIndexChanged += new System.EventHandler(this.chklistBoxSelectedParameter_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Note : Maximum of 8 Parameters can be selected";
            // 
            // btnShowReport
            // 
            this.btnShowReport.Location = new System.Drawing.Point(183, 303);
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.Size = new System.Drawing.Size(75, 23);
            this.btnShowReport.TabIndex = 18;
            this.btnShowReport.Text = "Show";
            this.btnShowReport.UseVisualStyleBackColor = true;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);
            // 
            // btnReportParamsCancel
            // 
            this.btnReportParamsCancel.Location = new System.Drawing.Point(264, 303);
            this.btnReportParamsCancel.Name = "btnReportParamsCancel";
            this.btnReportParamsCancel.Size = new System.Drawing.Size(75, 23);
            this.btnReportParamsCancel.TabIndex = 17;
            this.btnReportParamsCancel.Text = "Cancel";
            this.btnReportParamsCancel.UseVisualStyleBackColor = true;
            this.btnReportParamsCancel.Click += new System.EventHandler(this.btnReportParamsCancel_Click);
            // 
            // g3_lblSelectParameters
            // 
            this.g3_lblSelectParameters.AutoSize = true;
            this.g3_lblSelectParameters.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g3_lblSelectParameters.Location = new System.Drawing.Point(9, 16);
            this.g3_lblSelectParameters.Name = "g3_lblSelectParameters";
            this.g3_lblSelectParameters.Size = new System.Drawing.Size(161, 14);
            this.g3_lblSelectParameters.TabIndex = 13;
            this.g3_lblSelectParameters.Text = "Select Report Parameters :  ";
            // 
            // MeterWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(748, 737);
            this.Controls.Add(this.groupBoxTamperCategory);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxParameterCategory);
            this.Controls.Add(this.groupBoxSelectMeter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeterWise";
            this.Text = "Meter Wise";
            this.Load += new System.EventHandler(this.MeterWiseReport_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MeterWise_FormClosed);
            this.groupBoxSelectMeter.ResumeLayout(false);
            this.groupBoxSelectMeter.PerformLayout();
            this.groupBoxParameterCategory.ResumeLayout(false);
            this.groupBoxParameterCategory.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBoxTamperCategory.ResumeLayout(false);
            this.groupBoxTamperCategory.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSelectMeter;
        private System.Windows.Forms.Button btnMeterCancel;
        private System.Windows.Forms.Button btnMeterNext;
        private System.Windows.Forms.Button btnBrowse;
        public System.Windows.Forms.TextBox txtBoxSelectMeter;
        private System.Windows.Forms.Label LFW_lblSelectFile;
        private System.Windows.Forms.Label MW_lblFilesAvailable;
        private System.Windows.Forms.GroupBox groupBoxParameterCategory;
        private System.Windows.Forms.Button btnParameterPrevious;
        private System.Windows.Forms.Button btnParameterCancel;
        private System.Windows.Forms.Button btnParameterNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxTamperCategory;
        private System.Windows.Forms.Button btnTamperCategoryNext;
        private System.Windows.Forms.Button btnTamperPrevious;
        private System.Windows.Forms.Button btnTamperCancel;
        private System.Windows.Forms.Label g4_lblSelectParameters;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Button btnReportParamsPrevious;
        private System.Windows.Forms.CheckedListBox chkListSelectParameters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShowReport;
        private System.Windows.Forms.Button btnReportParamsCancel;
        private System.Windows.Forms.Label g3_lblSelectParameters;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioBtnTamperParameter;
        private System.Windows.Forms.RadioButton radioBtnLoadSurveyParameter;
        private System.Windows.Forms.RadioButton radioBtnBillingParameter;
        private System.Windows.Forms.RadioButton radioBtnGeneralParameter;
        private System.Windows.Forms.RadioButton radioBtnInstantParameter;
        private CAB.UI.Controls.CABGridControl lngGridAvailableFiles;
        private CAB.UI.Controls.CABGridControl lngGridTamper;
    }
}