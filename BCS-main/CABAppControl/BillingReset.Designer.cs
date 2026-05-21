namespace CABAppControl
{
    partial class BillingReset
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
            this.rbtnManual = new System.Windows.Forms.RadioButton();
            this.rbtnAuto = new System.Windows.Forms.RadioButton();
            this.lblResetLockOutDays = new System.Windows.Forms.Label();
            this.gbManual = new System.Windows.Forms.GroupBox();
            this.cmbResetLockoutdays = new System.Windows.Forms.ComboBox();
            this.lblBillingMode = new System.Windows.Forms.Label();
            this.lblSelectDay = new System.Windows.Forms.Label();
            this.lblSelectHour = new System.Windows.Forms.Label();
            this.lblSelectMinutes = new System.Windows.Forms.Label();
            this.gbAutoMode = new System.Windows.Forms.GroupBox();
            this.cmbSelectMinutes = new System.Windows.Forms.ComboBox();
            this.cmbSelectHour = new System.Windows.Forms.ComboBox();
            this.cmbSelectDay = new System.Windows.Forms.ComboBox();
            this.rbtnMonthly = new System.Windows.Forms.RadioButton();
            this.rbtnEvenMonth = new System.Windows.Forms.RadioButton();
            this.rbtnOddMonth = new System.Windows.Forms.RadioButton();
            this.cmbModeofBilling = new System.Windows.Forms.ComboBox();
            this.lblBillingPeriod = new System.Windows.Forms.Label();
            this.lblBillingType = new System.Windows.Forms.Label();
            this.gbManual.SuspendLayout();
            this.gbAutoMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtnManual
            // 
            this.rbtnManual.AutoSize = true;
            this.rbtnManual.Location = new System.Drawing.Point(272, 12);
            this.rbtnManual.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbtnManual.Name = "rbtnManual";
            this.rbtnManual.Size = new System.Drawing.Size(86, 24);
            this.rbtnManual.TabIndex = 2;
            this.rbtnManual.TabStop = true;
            this.rbtnManual.Text = "Manual";
            this.rbtnManual.UseVisualStyleBackColor = true;
            this.rbtnManual.CheckedChanged += new System.EventHandler(this.rbtnManual_CheckedChanged);
            // 
            // rbtnAuto
            // 
            this.rbtnAuto.AutoSize = true;
            this.rbtnAuto.Location = new System.Drawing.Point(184, 12);
            this.rbtnAuto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbtnAuto.Name = "rbtnAuto";
            this.rbtnAuto.Size = new System.Drawing.Size(68, 24);
            this.rbtnAuto.TabIndex = 1;
            this.rbtnAuto.TabStop = true;
            this.rbtnAuto.Text = "Auto";
            this.rbtnAuto.UseVisualStyleBackColor = true;
            this.rbtnAuto.CheckedChanged += new System.EventHandler(this.rbtnAuto_CheckedChanged);
            // 
            // lblResetLockOutDays
            // 
            this.lblResetLockOutDays.AutoSize = true;
            this.lblResetLockOutDays.Location = new System.Drawing.Point(24, 46);
            this.lblResetLockOutDays.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResetLockOutDays.Name = "lblResetLockOutDays";
            this.lblResetLockOutDays.Size = new System.Drawing.Size(157, 20);
            this.lblResetLockOutDays.TabIndex = 0;
            this.lblResetLockOutDays.Text = "Reset Lock out Days";
            // 
            // gbManual
            // 
            this.gbManual.Controls.Add(this.cmbResetLockoutdays);
            this.gbManual.Controls.Add(this.lblResetLockOutDays);
            this.gbManual.Location = new System.Drawing.Point(24, 378);
            this.gbManual.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbManual.Name = "gbManual";
            this.gbManual.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbManual.Size = new System.Drawing.Size(640, 97);
            this.gbManual.TabIndex = 4;
            this.gbManual.TabStop = false;
            this.gbManual.Text = "Manual";
            this.gbManual.Enter += new System.EventHandler(this.gbManual_Enter);
            // 
            // cmbResetLockoutdays
            // 
            this.cmbResetLockoutdays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResetLockoutdays.FormattingEnabled = true;
            this.cmbResetLockoutdays.Location = new System.Drawing.Point(207, 34);
            this.cmbResetLockoutdays.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbResetLockoutdays.Name = "cmbResetLockoutdays";
            this.cmbResetLockoutdays.Size = new System.Drawing.Size(180, 28);
            this.cmbResetLockoutdays.TabIndex = 1;
            // 
            // lblBillingMode
            // 
            this.lblBillingMode.AutoSize = true;
            this.lblBillingMode.Location = new System.Drawing.Point(24, 42);
            this.lblBillingMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBillingMode.Name = "lblBillingMode";
            this.lblBillingMode.Size = new System.Drawing.Size(112, 20);
            this.lblBillingMode.TabIndex = 0;
            this.lblBillingMode.Text = "Mode of Billing";
            // 
            // lblSelectDay
            // 
            this.lblSelectDay.AutoSize = true;
            this.lblSelectDay.Location = new System.Drawing.Point(24, 145);
            this.lblSelectDay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectDay.Name = "lblSelectDay";
            this.lblSelectDay.Size = new System.Drawing.Size(86, 20);
            this.lblSelectDay.TabIndex = 2;
            this.lblSelectDay.Text = "Select Day";
            // 
            // lblSelectHour
            // 
            this.lblSelectHour.AutoSize = true;
            this.lblSelectHour.Location = new System.Drawing.Point(26, 198);
            this.lblSelectHour.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectHour.Name = "lblSelectHour";
            this.lblSelectHour.Size = new System.Drawing.Size(93, 20);
            this.lblSelectHour.TabIndex = 3;
            this.lblSelectHour.Text = "Select Hour";
            // 
            // lblSelectMinutes
            // 
            this.lblSelectMinutes.AutoSize = true;
            this.lblSelectMinutes.Location = new System.Drawing.Point(24, 249);
            this.lblSelectMinutes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectMinutes.Name = "lblSelectMinutes";
            this.lblSelectMinutes.Size = new System.Drawing.Size(114, 20);
            this.lblSelectMinutes.TabIndex = 4;
            this.lblSelectMinutes.Text = "Select Minutes";
            // 
            // gbAutoMode
            // 
            this.gbAutoMode.Controls.Add(this.cmbSelectMinutes);
            this.gbAutoMode.Controls.Add(this.cmbSelectHour);
            this.gbAutoMode.Controls.Add(this.cmbSelectDay);
            this.gbAutoMode.Controls.Add(this.rbtnMonthly);
            this.gbAutoMode.Controls.Add(this.rbtnEvenMonth);
            this.gbAutoMode.Controls.Add(this.rbtnOddMonth);
            this.gbAutoMode.Controls.Add(this.cmbModeofBilling);
            this.gbAutoMode.Controls.Add(this.lblSelectMinutes);
            this.gbAutoMode.Controls.Add(this.lblSelectHour);
            this.gbAutoMode.Controls.Add(this.lblSelectDay);
            this.gbAutoMode.Controls.Add(this.lblBillingPeriod);
            this.gbAutoMode.Controls.Add(this.lblBillingMode);
            this.gbAutoMode.Location = new System.Drawing.Point(24, 55);
            this.gbAutoMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbAutoMode.Name = "gbAutoMode";
            this.gbAutoMode.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbAutoMode.Size = new System.Drawing.Size(640, 305);
            this.gbAutoMode.TabIndex = 3;
            this.gbAutoMode.TabStop = false;
            this.gbAutoMode.Text = "AutoMode";
            // 
            // cmbSelectMinutes
            // 
            this.cmbSelectMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectMinutes.FormattingEnabled = true;
            this.cmbSelectMinutes.Location = new System.Drawing.Point(207, 237);
            this.cmbSelectMinutes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSelectMinutes.Name = "cmbSelectMinutes";
            this.cmbSelectMinutes.Size = new System.Drawing.Size(180, 28);
            this.cmbSelectMinutes.TabIndex = 11;
            this.cmbSelectMinutes.SelectedIndexChanged += new System.EventHandler(this.cmbSelectMinutes_SelectedIndexChanged);
            // 
            // cmbSelectHour
            // 
            this.cmbSelectHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectHour.FormattingEnabled = true;
            this.cmbSelectHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbSelectHour.Location = new System.Drawing.Point(207, 186);
            this.cmbSelectHour.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSelectHour.Name = "cmbSelectHour";
            this.cmbSelectHour.Size = new System.Drawing.Size(180, 28);
            this.cmbSelectHour.TabIndex = 10;
            // 
            // cmbSelectDay
            // 
            this.cmbSelectDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectDay.FormattingEnabled = true;
            this.cmbSelectDay.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28"});
            this.cmbSelectDay.Location = new System.Drawing.Point(207, 132);
            this.cmbSelectDay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSelectDay.Name = "cmbSelectDay";
            this.cmbSelectDay.Size = new System.Drawing.Size(180, 28);
            this.cmbSelectDay.TabIndex = 9;
            // 
            // rbtnMonthly
            // 
            this.rbtnMonthly.AutoSize = true;
            this.rbtnMonthly.Location = new System.Drawing.Point(477, 85);
            this.rbtnMonthly.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbtnMonthly.Name = "rbtnMonthly";
            this.rbtnMonthly.Size = new System.Drawing.Size(89, 24);
            this.rbtnMonthly.TabIndex = 8;
            this.rbtnMonthly.Text = "Monthly";
            this.rbtnMonthly.UseVisualStyleBackColor = true;
            // 
            // rbtnEvenMonth
            // 
            this.rbtnEvenMonth.AutoSize = true;
            this.rbtnEvenMonth.Location = new System.Drawing.Point(338, 85);
            this.rbtnEvenMonth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbtnEvenMonth.Name = "rbtnEvenMonth";
            this.rbtnEvenMonth.Size = new System.Drawing.Size(119, 24);
            this.rbtnEvenMonth.TabIndex = 7;
            this.rbtnEvenMonth.Text = "Even Month";
            this.rbtnEvenMonth.UseVisualStyleBackColor = true;
            // 
            // rbtnOddMonth
            // 
            this.rbtnOddMonth.AutoSize = true;
            this.rbtnOddMonth.Location = new System.Drawing.Point(207, 85);
            this.rbtnOddMonth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbtnOddMonth.Name = "rbtnOddMonth";
            this.rbtnOddMonth.Size = new System.Drawing.Size(113, 24);
            this.rbtnOddMonth.TabIndex = 6;
            this.rbtnOddMonth.Text = "Odd Month";
            this.rbtnOddMonth.UseVisualStyleBackColor = true;
            // 
            // cmbModeofBilling
            // 
            this.cmbModeofBilling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModeofBilling.FormattingEnabled = true;
            this.cmbModeofBilling.Items.AddRange(new object[] {
            "End of Month",
            "User Defined"});
            this.cmbModeofBilling.Location = new System.Drawing.Point(206, 29);
            this.cmbModeofBilling.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbModeofBilling.Name = "cmbModeofBilling";
            this.cmbModeofBilling.Size = new System.Drawing.Size(180, 28);
            this.cmbModeofBilling.TabIndex = 5;
            this.cmbModeofBilling.SelectedIndexChanged += new System.EventHandler(this.cmbModeofBilling_SelectedIndexChanged);
            // 
            // lblBillingPeriod
            // 
            this.lblBillingPeriod.AutoSize = true;
            this.lblBillingPeriod.Location = new System.Drawing.Point(24, 88);
            this.lblBillingPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBillingPeriod.Name = "lblBillingPeriod";
            this.lblBillingPeriod.Size = new System.Drawing.Size(99, 20);
            this.lblBillingPeriod.TabIndex = 1;
            this.lblBillingPeriod.Text = "Billing Period";
            // 
            // lblBillingType
            // 
            this.lblBillingType.AutoSize = true;
            this.lblBillingType.Location = new System.Drawing.Point(48, 15);
            this.lblBillingType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBillingType.Name = "lblBillingType";
            this.lblBillingType.Size = new System.Drawing.Size(88, 20);
            this.lblBillingType.TabIndex = 0;
            this.lblBillingType.Text = "Billing Type";
            // 
            // BillingReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbManual);
            this.Controls.Add(this.gbAutoMode);
            this.Controls.Add(this.rbtnManual);
            this.Controls.Add(this.rbtnAuto);
            this.Controls.Add(this.lblBillingType);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BillingReset";
            this.Size = new System.Drawing.Size(836, 620);
            this.Load += new System.EventHandler(this.BillingReset_Load);
            this.gbManual.ResumeLayout(false);
            this.gbManual.PerformLayout();
            this.gbAutoMode.ResumeLayout(false);
            this.gbAutoMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnManual;
        private System.Windows.Forms.RadioButton rbtnAuto;
        private System.Windows.Forms.Label lblResetLockOutDays;
        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.Label lblBillingMode;
        private System.Windows.Forms.Label lblSelectDay;
        private System.Windows.Forms.Label lblSelectHour;
        private System.Windows.Forms.Label lblSelectMinutes;
        private System.Windows.Forms.GroupBox gbAutoMode;
        private System.Windows.Forms.RadioButton rbtnMonthly;
        private System.Windows.Forms.RadioButton rbtnEvenMonth;
        private System.Windows.Forms.RadioButton rbtnOddMonth;
        private System.Windows.Forms.ComboBox cmbModeofBilling;
        private System.Windows.Forms.Label lblBillingType;
        private System.Windows.Forms.ComboBox cmbSelectMinutes;
        private System.Windows.Forms.ComboBox cmbSelectHour;
        private System.Windows.Forms.ComboBox cmbSelectDay;
        private System.Windows.Forms.ComboBox cmbResetLockoutdays;
        private System.Windows.Forms.Label lblBillingPeriod;


    }
}
