namespace CAB.UI
{
    partial class CABFileWise
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
            this.groupBoxTamperCategory = new System.Windows.Forms.GroupBox();
            this.lngGridTamper = new CAB.UI.Controls.CABGridControl();
            this.btnTamperCategoryNext = new System.Windows.Forms.Button();
            this.btnTamperCategoryPrevious = new System.Windows.Forms.Button();
            this.btnTamperCategoryCancel = new System.Windows.Forms.Button();
            this.g4_lblSelectParameters = new System.Windows.Forms.Label();
            this.groupBoxParameters = new System.Windows.Forms.GroupBox();
            this.btnParametersPrevious = new System.Windows.Forms.Button();
            this.chkListSelectParameters = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnShowParameters = new System.Windows.Forms.Button();
            this.btnParametersCancel = new System.Windows.Forms.Button();
            this.g3_lblSelectParameters = new System.Windows.Forms.Label();
            this.groupBoxParameterCategory = new System.Windows.Forms.GroupBox();
            this.groupBoxParameterContainer = new System.Windows.Forms.GroupBox();
            this.radioBtnTamperParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnInstantParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnLoadSurveyParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnGeneralParameter = new System.Windows.Forms.RadioButton();
            this.radioBtnBillingParameter = new System.Windows.Forms.RadioButton();
            this.btnParameterCategoryPrevious = new System.Windows.Forms.Button();
            this.btnParameterCategoryCancel = new System.Windows.Forms.Button();
            this.btnParameterCategoryNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxSelectFile = new System.Windows.Forms.GroupBox();
            this.lngGridAvailableMeters = new CAB.UI.Controls.CABGridControl();
            this.btnFileWiseCancel = new System.Windows.Forms.Button();
            this.btnFileWiseNext = new System.Windows.Forms.Button();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.txtBoxSelectFile = new System.Windows.Forms.TextBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.lblAvailableParams = new System.Windows.Forms.Label();
            this.groupBoxTamperCategory.SuspendLayout();
            this.groupBoxParameters.SuspendLayout();
            this.groupBoxParameterCategory.SuspendLayout();
            this.groupBoxParameterContainer.SuspendLayout();
            this.groupBoxSelectFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTamperCategory
            // 
            this.groupBoxTamperCategory.Controls.Add(this.lngGridTamper);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryNext);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryPrevious);
            this.groupBoxTamperCategory.Controls.Add(this.btnTamperCategoryCancel);
            this.groupBoxTamperCategory.Controls.Add(this.g4_lblSelectParameters);
            this.groupBoxTamperCategory.Location = new System.Drawing.Point(367, 2);
            this.groupBoxTamperCategory.Name = "groupBoxTamperCategory";
            this.groupBoxTamperCategory.Size = new System.Drawing.Size(348, 350);
            this.groupBoxTamperCategory.TabIndex = 28;
            this.groupBoxTamperCategory.TabStop = false;
            this.groupBoxTamperCategory.Visible = false;
            // 
            // lngGridTamper
            // 
            this.lngGridTamper.Data = null;
            this.lngGridTamper.HiddenColumn = null;
            this.lngGridTamper.HiddenColumn2 = null;
            this.lngGridTamper.HiddenColumns = null;
            this.lngGridTamper.IsEqual = false;
            this.lngGridTamper.IsFullRow = false;
            this.lngGridTamper.IsSorting = false;
            this.lngGridTamper.Location = new System.Drawing.Point(16, 32);
            this.lngGridTamper.Name = "lngGridTamper";
            this.lngGridTamper.SelectedIndex = 0;
            this.lngGridTamper.Size = new System.Drawing.Size(323, 258);
            this.lngGridTamper.TabIndex = 25;
            this.lngGridTamper.ValueColumn = null;
            this.lngGridTamper.ValueColumn2 = null;
            // 
            // btnTamperCategoryNext
            // 
            this.btnTamperCategoryNext.Location = new System.Drawing.Point(183, 302);
            this.btnTamperCategoryNext.Name = "btnTamperCategoryNext";
            this.btnTamperCategoryNext.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryNext.TabIndex = 24;
            this.btnTamperCategoryNext.Text = "Next >>";
            this.btnTamperCategoryNext.UseVisualStyleBackColor = true;
            this.btnTamperCategoryNext.Click += new System.EventHandler(this.btnTamperCategoryNext_Click);
            // 
            // btnTamperCategoryPrevious
            // 
            this.btnTamperCategoryPrevious.Location = new System.Drawing.Point(102, 302);
            this.btnTamperCategoryPrevious.Name = "btnTamperCategoryPrevious";
            this.btnTamperCategoryPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryPrevious.TabIndex = 23;
            this.btnTamperCategoryPrevious.Text = "<< Previous";
            this.btnTamperCategoryPrevious.UseVisualStyleBackColor = true;
            this.btnTamperCategoryPrevious.Click += new System.EventHandler(this.btnTamperCategoryPrevious_Click);
            // 
            // btnTamperCategoryCancel
            // 
            this.btnTamperCategoryCancel.Location = new System.Drawing.Point(264, 302);
            this.btnTamperCategoryCancel.Name = "btnTamperCategoryCancel";
            this.btnTamperCategoryCancel.Size = new System.Drawing.Size(75, 23);
            this.btnTamperCategoryCancel.TabIndex = 17;
            this.btnTamperCategoryCancel.Text = "Cancel";
            this.btnTamperCategoryCancel.UseVisualStyleBackColor = true;
            this.btnTamperCategoryCancel.Click += new System.EventHandler(this.btnTamperCategoryCancel_Click);
            // 
            // g4_lblSelectParameters
            // 
            this.g4_lblSelectParameters.AutoSize = true;
            this.g4_lblSelectParameters.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g4_lblSelectParameters.Location = new System.Drawing.Point(13, 13);
            this.g4_lblSelectParameters.Name = "g4_lblSelectParameters";
            this.g4_lblSelectParameters.Size = new System.Drawing.Size(148, 14);
            this.g4_lblSelectParameters.TabIndex = 13;
            this.g4_lblSelectParameters.Text = "Select Tamper Category : ";
            // 
            // groupBoxParameters
            // 
            this.groupBoxParameters.Controls.Add(this.btnParametersPrevious);
            this.groupBoxParameters.Controls.Add(this.chkListSelectParameters);
            this.groupBoxParameters.Controls.Add(this.label2);
            this.groupBoxParameters.Controls.Add(this.btnShowParameters);
            this.groupBoxParameters.Controls.Add(this.btnParametersCancel);
            this.groupBoxParameters.Controls.Add(this.g3_lblSelectParameters);
            this.groupBoxParameters.Location = new System.Drawing.Point(728, 2);
            this.groupBoxParameters.Name = "groupBoxParameters";
            this.groupBoxParameters.Size = new System.Drawing.Size(348, 350);
            this.groupBoxParameters.TabIndex = 27;
            this.groupBoxParameters.TabStop = false;
            this.groupBoxParameters.Visible = false;
            // 
            // btnParametersPrevious
            // 
            this.btnParametersPrevious.Location = new System.Drawing.Point(102, 302);
            this.btnParametersPrevious.Name = "btnParametersPrevious";
            this.btnParametersPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnParametersPrevious.TabIndex = 23;
            this.btnParametersPrevious.Text = "<< Previous";
            this.btnParametersPrevious.UseVisualStyleBackColor = true;
            this.btnParametersPrevious.Click += new System.EventHandler(this.btnParametersPrevious_Click);
            // 
            // chkListSelectParameters
            // 
            this.chkListSelectParameters.CheckOnClick = true;
            this.chkListSelectParameters.FormattingEnabled = true;
            this.chkListSelectParameters.Location = new System.Drawing.Point(12, 29);
            this.chkListSelectParameters.Name = "chkListSelectParameters";
            this.chkListSelectParameters.Size = new System.Drawing.Size(327, 229);
            this.chkListSelectParameters.TabIndex = 22;
            this.chkListSelectParameters.SelectedIndexChanged += new System.EventHandler(this.chkListSelectParameters_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Note : Maximum of 8 Parameters can be selected";
            // 
            // btnShowParameters
            // 
            this.btnShowParameters.Location = new System.Drawing.Point(183, 302);
            this.btnShowParameters.Name = "btnShowParameters";
            this.btnShowParameters.Size = new System.Drawing.Size(75, 23);
            this.btnShowParameters.TabIndex = 18;
            this.btnShowParameters.Text = "Show";
            this.btnShowParameters.UseVisualStyleBackColor = true;
            this.btnShowParameters.Click += new System.EventHandler(this.btnShowParameters_Click);
            // 
            // btnParametersCancel
            // 
            this.btnParametersCancel.Location = new System.Drawing.Point(264, 302);
            this.btnParametersCancel.Name = "btnParametersCancel";
            this.btnParametersCancel.Size = new System.Drawing.Size(75, 23);
            this.btnParametersCancel.TabIndex = 17;
            this.btnParametersCancel.Text = "Cancel";
            this.btnParametersCancel.UseVisualStyleBackColor = true;
            this.btnParametersCancel.Click += new System.EventHandler(this.btnParametersCancel_Click);
            // 
            // g3_lblSelectParameters
            // 
            this.g3_lblSelectParameters.AutoSize = true;
            this.g3_lblSelectParameters.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g3_lblSelectParameters.Location = new System.Drawing.Point(11, 13);
            this.g3_lblSelectParameters.Name = "g3_lblSelectParameters";
            this.g3_lblSelectParameters.Size = new System.Drawing.Size(118, 14);
            this.g3_lblSelectParameters.TabIndex = 13;
            this.g3_lblSelectParameters.Text = "Select Parameters : ";
            // 
            // groupBoxParameterCategory
            // 
            this.groupBoxParameterCategory.Controls.Add(this.groupBoxParameterContainer);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryPrevious);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryCancel);
            this.groupBoxParameterCategory.Controls.Add(this.btnParameterCategoryNext);
            this.groupBoxParameterCategory.Controls.Add(this.label1);
            this.groupBoxParameterCategory.Location = new System.Drawing.Point(7, 369);
            this.groupBoxParameterCategory.Name = "groupBoxParameterCategory";
            this.groupBoxParameterCategory.Size = new System.Drawing.Size(350, 333);
            this.groupBoxParameterCategory.TabIndex = 26;
            this.groupBoxParameterCategory.TabStop = false;
            this.groupBoxParameterCategory.Visible = false;
            // 
            // groupBoxParameterContainer
            // 
            this.groupBoxParameterContainer.Controls.Add(this.radioBtnTamperParameter);
            this.groupBoxParameterContainer.Controls.Add(this.radioBtnInstantParameter);
            this.groupBoxParameterContainer.Controls.Add(this.radioBtnLoadSurveyParameter);
            this.groupBoxParameterContainer.Controls.Add(this.radioBtnGeneralParameter);
            this.groupBoxParameterContainer.Controls.Add(this.radioBtnBillingParameter);
            this.groupBoxParameterContainer.Location = new System.Drawing.Point(12, 33);
            this.groupBoxParameterContainer.Name = "groupBoxParameterContainer";
            this.groupBoxParameterContainer.Size = new System.Drawing.Size(327, 262);
            this.groupBoxParameterContainer.TabIndex = 11;
            this.groupBoxParameterContainer.TabStop = false;
            // 
            // radioBtnTamperParameter
            // 
            this.radioBtnTamperParameter.AutoSize = true;
            this.radioBtnTamperParameter.Location = new System.Drawing.Point(95, 179);
            this.radioBtnTamperParameter.Name = "radioBtnTamperParameter";
            this.radioBtnTamperParameter.Size = new System.Drawing.Size(112, 17);
            this.radioBtnTamperParameter.TabIndex = 11;
            this.radioBtnTamperParameter.Text = "Tamper Parameter";
            this.radioBtnTamperParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnInstantParameter
            // 
            this.radioBtnInstantParameter.AutoSize = true;
            this.radioBtnInstantParameter.Location = new System.Drawing.Point(95, 101);
            this.radioBtnInstantParameter.Name = "radioBtnInstantParameter";
            this.radioBtnInstantParameter.Size = new System.Drawing.Size(108, 17);
            this.radioBtnInstantParameter.TabIndex = 7;
            this.radioBtnInstantParameter.Text = "Instant Parameter";
            this.radioBtnInstantParameter.UseVisualStyleBackColor = true;
            this.radioBtnInstantParameter.CheckedChanged += new System.EventHandler(this.radioBtnInstantParameter_CheckedChanged);
            // 
            // radioBtnLoadSurveyParameter
            // 
            this.radioBtnLoadSurveyParameter.AutoSize = true;
            this.radioBtnLoadSurveyParameter.Location = new System.Drawing.Point(95, 153);
            this.radioBtnLoadSurveyParameter.Name = "radioBtnLoadSurveyParameter";
            this.radioBtnLoadSurveyParameter.Size = new System.Drawing.Size(136, 17);
            this.radioBtnLoadSurveyParameter.TabIndex = 10;
            this.radioBtnLoadSurveyParameter.Text = "Load Survey Parameter";
            this.radioBtnLoadSurveyParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnGeneralParameter
            // 
            this.radioBtnGeneralParameter.AutoSize = true;
            this.radioBtnGeneralParameter.Checked = true;
            this.radioBtnGeneralParameter.Location = new System.Drawing.Point(95, 75);
            this.radioBtnGeneralParameter.Name = "radioBtnGeneralParameter";
            this.radioBtnGeneralParameter.Size = new System.Drawing.Size(113, 17);
            this.radioBtnGeneralParameter.TabIndex = 8;
            this.radioBtnGeneralParameter.TabStop = true;
            this.radioBtnGeneralParameter.Text = "General Parameter";
            this.radioBtnGeneralParameter.UseVisualStyleBackColor = true;
            // 
            // radioBtnBillingParameter
            // 
            this.radioBtnBillingParameter.AutoSize = true;
            this.radioBtnBillingParameter.Location = new System.Drawing.Point(95, 127);
            this.radioBtnBillingParameter.Name = "radioBtnBillingParameter";
            this.radioBtnBillingParameter.Size = new System.Drawing.Size(103, 17);
            this.radioBtnBillingParameter.TabIndex = 9;
            this.radioBtnBillingParameter.Text = "Billing Parameter";
            this.radioBtnBillingParameter.UseVisualStyleBackColor = true;
            // 
            // btnParameterCategoryPrevious
            // 
            this.btnParameterCategoryPrevious.Location = new System.Drawing.Point(102, 301);
            this.btnParameterCategoryPrevious.Name = "btnParameterCategoryPrevious";
            this.btnParameterCategoryPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryPrevious.TabIndex = 10;
            this.btnParameterCategoryPrevious.Text = "<< Previous";
            this.btnParameterCategoryPrevious.UseVisualStyleBackColor = true;
            this.btnParameterCategoryPrevious.Click += new System.EventHandler(this.btnParameterCategoryPrevious_Click);
            // 
            // btnParameterCategoryCancel
            // 
            this.btnParameterCategoryCancel.Location = new System.Drawing.Point(264, 301);
            this.btnParameterCategoryCancel.Name = "btnParameterCategoryCancel";
            this.btnParameterCategoryCancel.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryCancel.TabIndex = 9;
            this.btnParameterCategoryCancel.Text = "Cancel";
            this.btnParameterCategoryCancel.UseVisualStyleBackColor = true;
            this.btnParameterCategoryCancel.Click += new System.EventHandler(this.btnParameterCategoryCancel_Click);
            // 
            // btnParameterCategoryNext
            // 
            this.btnParameterCategoryNext.Location = new System.Drawing.Point(183, 301);
            this.btnParameterCategoryNext.Name = "btnParameterCategoryNext";
            this.btnParameterCategoryNext.Size = new System.Drawing.Size(75, 23);
            this.btnParameterCategoryNext.TabIndex = 8;
            this.btnParameterCategoryNext.Text = "Next >>";
            this.btnParameterCategoryNext.UseVisualStyleBackColor = true;
            this.btnParameterCategoryNext.Click += new System.EventHandler(this.btnParameterCategoryNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Parameter Category : ";
            // 
            // groupBoxSelectFile
            // 
            this.groupBoxSelectFile.AutoSize = true;
            this.groupBoxSelectFile.Controls.Add(this.lngGridAvailableMeters);
            this.groupBoxSelectFile.Controls.Add(this.btnFileWiseCancel);
            this.groupBoxSelectFile.Controls.Add(this.btnFileWiseNext);
            this.groupBoxSelectFile.Controls.Add(this.btnBrowseFile);
            this.groupBoxSelectFile.Controls.Add(this.txtBoxSelectFile);
            this.groupBoxSelectFile.Controls.Add(this.lblSelect);
            this.groupBoxSelectFile.Controls.Add(this.lblAvailableParams);
            this.groupBoxSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSelectFile.Location = new System.Drawing.Point(7, 2);
            this.groupBoxSelectFile.Name = "groupBoxSelectFile";
            this.groupBoxSelectFile.Size = new System.Drawing.Size(350, 350);
            this.groupBoxSelectFile.TabIndex = 25;
            this.groupBoxSelectFile.TabStop = false;
            // 
            // lngGridAvailableMeters
            // 
            this.lngGridAvailableMeters.Data = null;
            this.lngGridAvailableMeters.HiddenColumn = null;
            this.lngGridAvailableMeters.HiddenColumn2 = null;
            this.lngGridAvailableMeters.HiddenColumns = null;
            this.lngGridAvailableMeters.IsEqual = false;
            this.lngGridAvailableMeters.IsFullRow = false;
            this.lngGridAvailableMeters.IsSorting = false;
            this.lngGridAvailableMeters.Location = new System.Drawing.Point(12, 78);
            this.lngGridAvailableMeters.Name = "lngGridAvailableMeters";
            this.lngGridAvailableMeters.SelectedIndex = 0;
            this.lngGridAvailableMeters.Size = new System.Drawing.Size(327, 218);
            this.lngGridAvailableMeters.TabIndex = 7;
            this.lngGridAvailableMeters.ValueColumn = null;
            this.lngGridAvailableMeters.ValueColumn2 = null;
            // 
            // btnFileWiseCancel
            // 
            this.btnFileWiseCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileWiseCancel.Location = new System.Drawing.Point(264, 302);
            this.btnFileWiseCancel.Name = "btnFileWiseCancel";
            this.btnFileWiseCancel.Size = new System.Drawing.Size(75, 23);
            this.btnFileWiseCancel.TabIndex = 6;
            this.btnFileWiseCancel.Text = "Cancel";
            this.btnFileWiseCancel.UseVisualStyleBackColor = true;
            this.btnFileWiseCancel.Click += new System.EventHandler(this.btnFileWiseCancel_Click);
            // 
            // btnFileWiseNext
            // 
            this.btnFileWiseNext.Enabled = false;
            this.btnFileWiseNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileWiseNext.Location = new System.Drawing.Point(183, 302);
            this.btnFileWiseNext.Name = "btnFileWiseNext";
            this.btnFileWiseNext.Size = new System.Drawing.Size(75, 23);
            this.btnFileWiseNext.TabIndex = 5;
            this.btnFileWiseNext.Text = "Next >>";
            this.btnFileWiseNext.UseVisualStyleBackColor = true;
            this.btnFileWiseNext.Click += new System.EventHandler(this.btnFileWiseNext_Click);
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFile.Location = new System.Drawing.Point(310, 31);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(29, 20);
            this.btnBrowseFile.TabIndex = 5;
            this.btnBrowseFile.Text = "...";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            // 
            // txtBoxSelectFile
            // 
            this.txtBoxSelectFile.BackColor = System.Drawing.Color.White;
            this.txtBoxSelectFile.Location = new System.Drawing.Point(12, 32);
            this.txtBoxSelectFile.MaxLength = 40;
            this.txtBoxSelectFile.Name = "txtBoxSelectFile";
            this.txtBoxSelectFile.ReadOnly = true;
            this.txtBoxSelectFile.Size = new System.Drawing.Size(287, 20);
            this.txtBoxSelectFile.TabIndex = 4;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelect.Location = new System.Drawing.Point(9, 13);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(72, 14);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Text = "Select File : ";
            // 
            // lblAvailableParams
            // 
            this.lblAvailableParams.AutoSize = true;
            this.lblAvailableParams.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableParams.Location = new System.Drawing.Point(9, 60);
            this.lblAvailableParams.Name = "lblAvailableParams";
            this.lblAvailableParams.Size = new System.Drawing.Size(108, 14);
            this.lblAvailableParams.TabIndex = 1;
            this.lblAvailableParams.Text = "Meters Available : ";
            // 
            // CABFileWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1102, 722);
            this.Controls.Add(this.groupBoxTamperCategory);
            this.Controls.Add(this.groupBoxParameters);
            this.Controls.Add(this.groupBoxParameterCategory);
            this.Controls.Add(this.groupBoxSelectFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CABFileWise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CAB File Wise";
            this.Load += new System.EventHandler(this.CABFileWise_Load);
            this.groupBoxTamperCategory.ResumeLayout(false);
            this.groupBoxTamperCategory.PerformLayout();
            this.groupBoxParameters.ResumeLayout(false);
            this.groupBoxParameters.PerformLayout();
            this.groupBoxParameterCategory.ResumeLayout(false);
            this.groupBoxParameterCategory.PerformLayout();
            this.groupBoxParameterContainer.ResumeLayout(false);
            this.groupBoxParameterContainer.PerformLayout();
            this.groupBoxSelectFile.ResumeLayout(false);
            this.groupBoxSelectFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTamperCategory;
        private System.Windows.Forms.Button btnTamperCategoryNext;
        private System.Windows.Forms.Button btnTamperCategoryPrevious;
        private System.Windows.Forms.Button btnTamperCategoryCancel;
        private System.Windows.Forms.Label g4_lblSelectParameters;
        private System.Windows.Forms.GroupBox groupBoxParameters;
        private System.Windows.Forms.Button btnParametersPrevious;
        private System.Windows.Forms.CheckedListBox chkListSelectParameters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnShowParameters;
        private System.Windows.Forms.Button btnParametersCancel;
        private System.Windows.Forms.Label g3_lblSelectParameters;
        private System.Windows.Forms.GroupBox groupBoxParameterCategory;
        private System.Windows.Forms.GroupBox groupBoxParameterContainer;
        private System.Windows.Forms.RadioButton radioBtnTamperParameter;
        private System.Windows.Forms.RadioButton radioBtnInstantParameter;
        private System.Windows.Forms.RadioButton radioBtnLoadSurveyParameter;
        private System.Windows.Forms.RadioButton radioBtnGeneralParameter;
        private System.Windows.Forms.RadioButton radioBtnBillingParameter;
        private System.Windows.Forms.Button btnParameterCategoryPrevious;
        private System.Windows.Forms.Button btnParameterCategoryCancel;
        private System.Windows.Forms.Button btnParameterCategoryNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxSelectFile;
        private System.Windows.Forms.Button btnFileWiseCancel;
        private System.Windows.Forms.Button btnFileWiseNext;
        private System.Windows.Forms.Button btnBrowseFile;
        public System.Windows.Forms.TextBox txtBoxSelectFile;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.Label lblAvailableParams;
        private CAB.UI.Controls.CABGridControl lngGridTamper;
        private CAB.UI.Controls.CABGridControl lngGridAvailableMeters;

    }
}