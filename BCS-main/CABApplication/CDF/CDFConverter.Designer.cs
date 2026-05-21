using CAB.UI.Controls;
namespace CAB.UI
{
    partial class CDFConverter
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
            this.grpCDF = new System.Windows.Forms.GroupBox();
            this.lblNoOfFiles = new System.Windows.Forms.Label();
            this.rdSpecific = new System.Windows.Forms.RadioButton();
            this.rdConvertAll = new System.Windows.Forms.RadioButton();
            this.btnBrowseResult = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.lblConversionError = new System.Windows.Forms.Label();
            this.btnBrowseError = new System.Windows.Forms.Button();
            this.txtError = new System.Windows.Forms.TextBox();
            this.btnBrowseConvDone = new System.Windows.Forms.Button();
            this.txtConversionDone = new System.Windows.Forms.TextBox();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.lblConversionDone = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.grpAction = new System.Windows.Forms.GroupBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpCDF.SuspendLayout();
            this.grpAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCDF
            // 
            this.grpCDF.Controls.Add(this.lblNoOfFiles);
            this.grpCDF.Controls.Add(this.rdSpecific);
            this.grpCDF.Controls.Add(this.rdConvertAll);
            this.grpCDF.Controls.Add(this.btnBrowseResult);
            this.grpCDF.Controls.Add(this.txtResult);
            this.grpCDF.Controls.Add(this.lblResult);
            this.grpCDF.Controls.Add(this.btnBrowseDestination);
            this.grpCDF.Controls.Add(this.txtDestination);
            this.grpCDF.Controls.Add(this.lblDestination);
            this.grpCDF.Controls.Add(this.lblConversionError);
            this.grpCDF.Controls.Add(this.btnBrowseError);
            this.grpCDF.Controls.Add(this.txtError);
            this.grpCDF.Controls.Add(this.btnBrowseConvDone);
            this.grpCDF.Controls.Add(this.txtConversionDone);
            this.grpCDF.Controls.Add(this.btnSourceBrowse);
            this.grpCDF.Controls.Add(this.lblConversionDone);
            this.grpCDF.Controls.Add(this.lblSource);
            this.grpCDF.Controls.Add(this.txtSource);
            this.grpCDF.Location = new System.Drawing.Point(12, 8);
            this.grpCDF.Name = "grpCDF";
            this.grpCDF.Size = new System.Drawing.Size(463, 267);
            this.grpCDF.TabIndex = 0;
            this.grpCDF.TabStop = false;
            this.grpCDF.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCDF.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.grpCDF.Text = "CDF Settings";
            // 
            // lblNoOfFiles
            // 
            this.lblNoOfFiles.AutoSize = true;
            this.lblNoOfFiles.Location = new System.Drawing.Point(377, 231);
            this.lblNoOfFiles.Name = "lblNoOfFiles";
            this.lblNoOfFiles.Size = new System.Drawing.Size(0, 13);
            this.lblNoOfFiles.TabIndex = 17;
            // 
            // rdSpecific
            // 
            this.rdSpecific.AutoSize = true;
            this.rdSpecific.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdSpecific.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.rdSpecific.Location = new System.Drawing.Point(192, 231);
            this.rdSpecific.Name = "rdSpecific";
            this.rdSpecific.Size = new System.Drawing.Size(63, 17);
            this.rdSpecific.TabIndex = 16;
            this.rdSpecific.TabStop = true;
            this.rdSpecific.Text = "Specific";
            this.rdSpecific.UseVisualStyleBackColor = true;
            this.rdSpecific.CheckedChanged += new System.EventHandler(this.rdSpecific_CheckedChanged);
            // 
            // rdConvertAll
            // 
            this.rdConvertAll.AutoSize = true;
            this.rdConvertAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdConvertAll.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.rdConvertAll.Location = new System.Drawing.Point(91, 231);
            this.rdConvertAll.Name = "rdConvertAll";
            this.rdConvertAll.Size = new System.Drawing.Size(76, 17);
            this.rdConvertAll.TabIndex = 15;
            this.rdConvertAll.TabStop = true;
            this.rdConvertAll.Text = "Convert All";
            this.rdConvertAll.UseVisualStyleBackColor = true;
            this.rdConvertAll.CheckedChanged += new System.EventHandler(this.rdConvertAll_CheckedChanged);
            // 
            // btnBrowseResult
            // 
            this.btnBrowseResult.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowseResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseResult.FlatAppearance.BorderSize = 0;
            this.btnBrowseResult.ForeColor = System.Drawing.Color.White;
            this.btnBrowseResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseResult.Location = new System.Drawing.Point(371, 192);
            this.btnBrowseResult.Name = "btnBrowseResult";
            this.btnBrowseResult.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseResult.TabIndex = 14;
            this.btnBrowseResult.Text = "Browse";
            this.btnBrowseResult.UseVisualStyleBackColor = false;
            this.btnBrowseResult.Click += new System.EventHandler(this.btnBrowseResult_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(132, 193);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(233, 20);
            this.txtResult.TabIndex = 13;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblResult.Location = new System.Drawing.Point(84, 197);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(43, 13);
            this.lblResult.TabIndex = 12;
            this.lblResult.Text = "Result :";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowseDestination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseDestination.FlatAppearance.BorderSize = 0;
            this.btnBrowseDestination.ForeColor = System.Drawing.Color.White;
            this.btnBrowseDestination.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseDestination.Location = new System.Drawing.Point(371, 152);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDestination.TabIndex = 11;
            this.btnBrowseDestination.Text = "Browse";
            this.btnBrowseDestination.UseVisualStyleBackColor = false;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(132, 153);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(233, 20);
            this.txtDestination.TabIndex = 10;
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestination.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblDestination.Location = new System.Drawing.Point(62, 157);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(69, 13);
            this.lblDestination.TabIndex = 9;
            this.lblDestination.Text = "Destination : ";
            // 
            // lblConversionError
            // 
            this.lblConversionError.AutoSize = true;
            this.lblConversionError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConversionError.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblConversionError.Location = new System.Drawing.Point(92, 117);
            this.lblConversionError.Name = "lblConversionError";
            this.lblConversionError.Size = new System.Drawing.Size(38, 13);
            this.lblConversionError.TabIndex = 8;
            this.lblConversionError.Text = "Error : ";
            // 
            // btnBrowseError
            // 
            this.btnBrowseError.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowseError.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseError.FlatAppearance.BorderSize = 0;
            this.btnBrowseError.ForeColor = System.Drawing.Color.White;
            this.btnBrowseError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseError.Location = new System.Drawing.Point(371, 113);
            this.btnBrowseError.Name = "btnBrowseError";
            this.btnBrowseError.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseError.TabIndex = 7;
            this.btnBrowseError.Text = "Browse";
            this.btnBrowseError.UseVisualStyleBackColor = false;
            this.btnBrowseError.Click += new System.EventHandler(this.btnBrowseError_Click);
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(132, 114);
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.Size = new System.Drawing.Size(233, 20);
            this.txtError.TabIndex = 6;
            // 
            // btnBrowseConvDone
            // 
            this.btnBrowseConvDone.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowseConvDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseConvDone.FlatAppearance.BorderSize = 0;
            this.btnBrowseConvDone.ForeColor = System.Drawing.Color.White;
            this.btnBrowseConvDone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseConvDone.Location = new System.Drawing.Point(371, 73);
            this.btnBrowseConvDone.Name = "btnBrowseConvDone";
            this.btnBrowseConvDone.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseConvDone.TabIndex = 5;
            this.btnBrowseConvDone.Text = "Browse";
            this.btnBrowseConvDone.UseVisualStyleBackColor = false;
            this.btnBrowseConvDone.Click += new System.EventHandler(this.btnBrowseConvDone_Click);
            // 
            // txtConversionDone
            // 
            this.txtConversionDone.Location = new System.Drawing.Point(132, 74);
            this.txtConversionDone.Name = "txtConversionDone";
            this.txtConversionDone.ReadOnly = true;
            this.txtConversionDone.Size = new System.Drawing.Size(233, 20);
            this.txtConversionDone.TabIndex = 4;
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSourceBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSourceBrowse.FlatAppearance.BorderSize = 0;
            this.btnSourceBrowse.ForeColor = System.Drawing.Color.White;
            this.btnSourceBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSourceBrowse.Location = new System.Drawing.Point(371, 33);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceBrowse.TabIndex = 3;
            this.btnSourceBrowse.Text = "Browse";
            this.btnSourceBrowse.UseVisualStyleBackColor = false;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // lblConversionDone
            // 
            this.lblConversionDone.AutoSize = true;
            this.lblConversionDone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConversionDone.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblConversionDone.Location = new System.Drawing.Point(32, 77);
            this.lblConversionDone.Name = "lblConversionDone";
            this.lblConversionDone.Size = new System.Drawing.Size(98, 13);
            this.lblConversionDone.TabIndex = 2;
            this.lblConversionDone.Text = "Conversion Done : ";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.lblSource.Location = new System.Drawing.Point(79, 37);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(50, 13);
            this.lblSource.TabIndex = 1;
            this.lblSource.Text = "Source : ";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(132, 34);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(233, 20);
            this.txtSource.TabIndex = 0;
            // 
            // grpAction
            // 
            this.grpAction.Controls.Add(this.btnConvert);
            this.grpAction.Controls.Add(this.btnCancel);
            this.grpAction.Location = new System.Drawing.Point(12, 281);
            this.grpAction.Name = "grpAction";
            this.grpAction.Size = new System.Drawing.Size(463, 64);
            this.grpAction.TabIndex = 1;
            this.grpAction.TabStop = false;
            this.grpAction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpAction.ForeColor = System.Drawing.Color.FromArgb(32, 32, 32);
            // 
            // btnConvert
            // 
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnConvert.Location = new System.Drawing.Point(301, 22);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(96, 96, 96);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(382, 22);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CDFConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClientSize = new System.Drawing.Size(490, 353);
            this.Controls.Add(this.grpAction);
            this.Controls.Add(this.grpCDF);
            this.Name = "CDFConverter";
            this.StatusMessage = "";
            this.Text = "CDFConverter";
            this.Load += new System.EventHandler(this.CDFConverter_Load);
            this.grpCDF.ResumeLayout(false);
            this.grpCDF.PerformLayout();
            this.grpAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCDF;
        private System.Windows.Forms.Button btnSourceBrowse;
        private System.Windows.Forms.Label lblConversionDone;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtConversionDone;
        private System.Windows.Forms.Button btnBrowseConvDone;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblConversionError;
        private System.Windows.Forms.Button btnBrowseError;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnBrowseResult;
        private System.Windows.Forms.GroupBox grpAction;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdSpecific;
        private System.Windows.Forms.RadioButton rdConvertAll;
        private System.Windows.Forms.Label lblNoOfFiles;
    }
}

