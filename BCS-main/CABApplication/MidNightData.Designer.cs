namespace CAB.UI
{
    partial class MidNightData
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
            this.pnlMD = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblToDateCaption = new System.Windows.Forms.Label();
            this.lblMeterIdValue = new System.Windows.Forms.Label();
            this.lblToDateValue = new System.Windows.Forms.Label();
            this.lblMeterIdCaption = new System.Windows.Forms.Label();
            this.lblFromDateCaption = new System.Windows.Forms.Label();
            this.lblFromDateValue = new System.Windows.Forms.Label();
            this.lngMidNightData = new CAB.UI.Controls.CABGridControl();
            this.pnlMD.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMD
            // 
            this.pnlMD.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlMD.BackColor = System.Drawing.Color.White;
            this.pnlMD.Controls.Add(this.label1);
            this.pnlMD.Controls.Add(this.lblToDateCaption);
            this.pnlMD.Controls.Add(this.lblMeterIdValue);
            this.pnlMD.Controls.Add(this.lblToDateValue);
            this.pnlMD.Controls.Add(this.lblMeterIdCaption);
            this.pnlMD.Controls.Add(this.lblFromDateCaption);
            this.pnlMD.Controls.Add(this.lblFromDateValue);
            this.pnlMD.Location = new System.Drawing.Point(220, 4);
            this.pnlMD.Name = "pnlMD";
            this.pnlMD.Size = new System.Drawing.Size(535, 39);
            this.pnlMD.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(448, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "ND : No Data";
            // 
            // lblToDateCaption
            // 
            this.lblToDateCaption.AutoSize = true;
            this.lblToDateCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDateCaption.Location = new System.Drawing.Point(295, 11);
            this.lblToDateCaption.Name = "lblToDateCaption";
            this.lblToDateCaption.Size = new System.Drawing.Size(61, 13);
            this.lblToDateCaption.TabIndex = 10;
            this.lblToDateCaption.Text = "To Date :";
            // 
            // lblMeterIdValue
            // 
            this.lblMeterIdValue.AutoSize = true;
            this.lblMeterIdValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeterIdValue.Location = new System.Drawing.Point(73, 11);
            this.lblMeterIdValue.Name = "lblMeterIdValue";
            this.lblMeterIdValue.Size = new System.Drawing.Size(0, 13);
            this.lblMeterIdValue.TabIndex = 12;
            // 
            // lblToDateValue
            // 
            this.lblToDateValue.AutoSize = true;
            this.lblToDateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDateValue.Location = new System.Drawing.Point(353, 11);
            this.lblToDateValue.Name = "lblToDateValue";
            this.lblToDateValue.Size = new System.Drawing.Size(23, 13);
            this.lblToDateValue.TabIndex = 7;
            this.lblToDateValue.Text = "----";
            // 
            // lblMeterIdCaption
            // 
            this.lblMeterIdCaption.AutoSize = true;
            this.lblMeterIdCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMeterIdCaption.Location = new System.Drawing.Point(11, 11);
            this.lblMeterIdCaption.Name = "lblMeterIdCaption";
            this.lblMeterIdCaption.Size = new System.Drawing.Size(68, 13);
            this.lblMeterIdCaption.TabIndex = 11;
            this.lblMeterIdCaption.Text = "Meter ID : ";
            // 
            // lblFromDateCaption
            // 
            this.lblFromDateCaption.AutoSize = true;
            this.lblFromDateCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDateCaption.Location = new System.Drawing.Point(151, 11);
            this.lblFromDateCaption.Name = "lblFromDateCaption";
            this.lblFromDateCaption.Size = new System.Drawing.Size(73, 13);
            this.lblFromDateCaption.TabIndex = 8;
            this.lblFromDateCaption.Text = "From Date :";
            // 
            // lblFromDateValue
            // 
            this.lblFromDateValue.AutoSize = true;
            this.lblFromDateValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDateValue.Location = new System.Drawing.Point(222, 11);
            this.lblFromDateValue.Name = "lblFromDateValue";
            this.lblFromDateValue.Size = new System.Drawing.Size(27, 13);
            this.lblFromDateValue.TabIndex = 9;
            this.lblFromDateValue.Text = "-----";
            // 
            // lngMidNightData
            // 
            this.lngMidNightData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lngMidNightData.AutoScroll = true;
            this.lngMidNightData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lngMidNightData.Data = null;
            this.lngMidNightData.HiddenColumn = null;
            this.lngMidNightData.HiddenColumns = null;
            this.lngMidNightData.IsEqual = false;
            this.lngMidNightData.IsFullRow = false;
            this.lngMidNightData.IsSorting = false;
            this.lngMidNightData.Location = new System.Drawing.Point(11, 44);
            this.lngMidNightData.Margin = new System.Windows.Forms.Padding(4);
            this.lngMidNightData.Name = "lngMidNightData";
            this.lngMidNightData.SelectedIndex = 0;
            this.lngMidNightData.Size = new System.Drawing.Size(1024, 503);
            this.lngMidNightData.TabIndex = 1;
            this.lngMidNightData.ValueColumn = null;
            // 
            // MidNightData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1048, 575);
            this.Controls.Add(this.lngMidNightData);
            this.Controls.Add(this.pnlMD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MidNightData";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StatusMessage = "";
            this.Text = "MidNightData";
            this.Load += new System.EventHandler(this.MidNightData_Load);
            this.pnlMD.ResumeLayout(false);
            this.pnlMD.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblToDateCaption;
        private System.Windows.Forms.Label lblMeterIdValue;
        private System.Windows.Forms.Label lblToDateValue;
        private System.Windows.Forms.Label lblMeterIdCaption;
        private System.Windows.Forms.Label lblFromDateCaption;
        private System.Windows.Forms.Label lblFromDateValue;
        private CAB.UI.Controls.CABGridControl lngMidNightData;
    }
}