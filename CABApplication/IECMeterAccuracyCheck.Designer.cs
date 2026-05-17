namespace CABApplication
{
    partial class IECMeterAccuracyCheck
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblduration = new System.Windows.Forms.Label();
            this.txtkvarhLagInitial = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadDelta = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadFinal = new System.Windows.Forms.TextBox();
            this.txtkvarhLeadInitial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTestduration = new System.Windows.Forms.ComboBox();
            this.lblTestDuration = new System.Windows.Forms.Label();
            this.txtkvarhLagDelta = new System.Windows.Forms.TextBox();
            this.txtkVAhDelta = new System.Windows.Forms.TextBox();
            this.txtkWhDelta = new System.Windows.Forms.TextBox();
            this.txtkvarhLagFinal = new System.Windows.Forms.TextBox();
            this.txtkVAhFinal = new System.Windows.Forms.TextBox();
            this.txtkWhFinal = new System.Windows.Forms.TextBox();
            this.txtkVAhInitial = new System.Windows.Forms.TextBox();
            this.txtkWhInitial = new System.Windows.Forms.TextBox();
            this.lblDelta = new System.Windows.Forms.Label();
            this.lblFinalReading = new System.Windows.Forms.Label();
            this.lblInitialReading = new System.Windows.Forms.Label();
            this.lblkvarh = new System.Windows.Forms.Label();
            this.lblkVAh = new System.Windows.Forms.Label();
            this.lblkWh = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Duration_Timer = new System.Windows.Forms.Timer(this.components);
            this.lblReactiveLeadUnit = new System.Windows.Forms.Label();
            this.lblReactiveLagUnit = new System.Windows.Forms.Label();
            this.lblApparentEnergyUnit = new System.Windows.Forms.Label();
            this.lblActiveEnergyUnit = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.lblReactiveLeadUnit);
            this.groupBox1.Controls.Add(this.lblReactiveLagUnit);
            this.groupBox1.Controls.Add(this.lblApparentEnergyUnit);
            this.groupBox1.Controls.Add(this.lblActiveEnergyUnit);
            this.groupBox1.Controls.Add(this.lblduration);
            this.groupBox1.Controls.Add(this.txtkvarhLagInitial);
            this.groupBox1.Controls.Add(this.txtkvarhLeadDelta);
            this.groupBox1.Controls.Add(this.txtkvarhLeadFinal);
            this.groupBox1.Controls.Add(this.txtkvarhLeadInitial);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbTestduration);
            this.groupBox1.Controls.Add(this.lblTestDuration);
            this.groupBox1.Controls.Add(this.txtkvarhLagDelta);
            this.groupBox1.Controls.Add(this.txtkVAhDelta);
            this.groupBox1.Controls.Add(this.txtkWhDelta);
            this.groupBox1.Controls.Add(this.txtkvarhLagFinal);
            this.groupBox1.Controls.Add(this.txtkVAhFinal);
            this.groupBox1.Controls.Add(this.txtkWhFinal);
            this.groupBox1.Controls.Add(this.txtkVAhInitial);
            this.groupBox1.Controls.Add(this.txtkWhInitial);
            this.groupBox1.Controls.Add(this.lblDelta);
            this.groupBox1.Controls.Add(this.lblFinalReading);
            this.groupBox1.Controls.Add(this.lblInitialReading);
            this.groupBox1.Controls.Add(this.lblkvarh);
            this.groupBox1.Controls.Add(this.lblkVAh);
            this.groupBox1.Controls.Add(this.lblkWh);
            this.groupBox1.Location = new System.Drawing.Point(26, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 277);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Meter Accuracy Check";
            // 
            // lblduration
            // 
            this.lblduration.AutoSize = true;
            this.lblduration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblduration.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblduration.Location = new System.Drawing.Point(41, 250);
            this.lblduration.Name = "lblduration";
            this.lblduration.Size = new System.Drawing.Size(41, 13);
            this.lblduration.TabIndex = 21;
            this.lblduration.Text = "label2";
            this.lblduration.Visible = false;
            // 
            // txtkvarhLagInitial
            // 
            this.txtkvarhLagInitial.Location = new System.Drawing.Point(159, 135);
            this.txtkvarhLagInitial.Name = "txtkvarhLagInitial";
            this.txtkvarhLagInitial.ReadOnly = true;
            this.txtkvarhLagInitial.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLagInitial.TabIndex = 8;
            // 
            // txtkvarhLeadDelta
            // 
            this.txtkvarhLeadDelta.Location = new System.Drawing.Point(379, 175);
            this.txtkvarhLeadDelta.Name = "txtkvarhLeadDelta";
            this.txtkvarhLeadDelta.ReadOnly = true;
            this.txtkvarhLeadDelta.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLeadDelta.TabIndex = 20;
            // 
            // txtkvarhLeadFinal
            // 
            this.txtkvarhLeadFinal.Location = new System.Drawing.Point(269, 175);
            this.txtkvarhLeadFinal.Name = "txtkvarhLeadFinal";
            this.txtkvarhLeadFinal.ReadOnly = true;
            this.txtkvarhLeadFinal.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLeadFinal.TabIndex = 19;
            // 
            // txtkvarhLeadInitial
            // 
            this.txtkvarhLeadInitial.Location = new System.Drawing.Point(159, 175);
            this.txtkvarhLeadInitial.Name = "txtkvarhLeadInitial";
            this.txtkvarhLeadInitial.ReadOnly = true;
            this.txtkvarhLeadInitial.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLeadInitial.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Reactive Energy(Lead)";
            // 
            // cmbTestduration
            // 
            this.cmbTestduration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTestduration.FormattingEnabled = true;
            this.cmbTestduration.Location = new System.Drawing.Point(187, 215);
            this.cmbTestduration.Name = "cmbTestduration";
            this.cmbTestduration.Size = new System.Drawing.Size(41, 21);
            this.cmbTestduration.TabIndex = 16;
            // 
            // lblTestDuration
            // 
            this.lblTestDuration.AutoSize = true;
            this.lblTestDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestDuration.Location = new System.Drawing.Point(41, 218);
            this.lblTestDuration.Name = "lblTestDuration";
            this.lblTestDuration.Size = new System.Drawing.Size(117, 13);
            this.lblTestDuration.TabIndex = 15;
            this.lblTestDuration.Text = "Test Duration (Minutes)";
            // 
            // txtkvarhLagDelta
            // 
            this.txtkvarhLagDelta.Location = new System.Drawing.Point(379, 135);
            this.txtkvarhLagDelta.Name = "txtkvarhLagDelta";
            this.txtkvarhLagDelta.ReadOnly = true;
            this.txtkvarhLagDelta.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLagDelta.TabIndex = 14;
            // 
            // txtkVAhDelta
            // 
            this.txtkVAhDelta.Location = new System.Drawing.Point(379, 93);
            this.txtkVAhDelta.Name = "txtkVAhDelta";
            this.txtkVAhDelta.ReadOnly = true;
            this.txtkVAhDelta.Size = new System.Drawing.Size(104, 20);
            this.txtkVAhDelta.TabIndex = 13;
            // 
            // txtkWhDelta
            // 
            this.txtkWhDelta.Location = new System.Drawing.Point(379, 50);
            this.txtkWhDelta.Name = "txtkWhDelta";
            this.txtkWhDelta.ReadOnly = true;
            this.txtkWhDelta.Size = new System.Drawing.Size(104, 20);
            this.txtkWhDelta.TabIndex = 12;
            // 
            // txtkvarhLagFinal
            // 
            this.txtkvarhLagFinal.Location = new System.Drawing.Point(269, 135);
            this.txtkvarhLagFinal.Name = "txtkvarhLagFinal";
            this.txtkvarhLagFinal.ReadOnly = true;
            this.txtkvarhLagFinal.Size = new System.Drawing.Size(104, 20);
            this.txtkvarhLagFinal.TabIndex = 11;
            // 
            // txtkVAhFinal
            // 
            this.txtkVAhFinal.Location = new System.Drawing.Point(269, 93);
            this.txtkVAhFinal.Name = "txtkVAhFinal";
            this.txtkVAhFinal.ReadOnly = true;
            this.txtkVAhFinal.Size = new System.Drawing.Size(104, 20);
            this.txtkVAhFinal.TabIndex = 10;
            // 
            // txtkWhFinal
            // 
            this.txtkWhFinal.Location = new System.Drawing.Point(269, 50);
            this.txtkWhFinal.Name = "txtkWhFinal";
            this.txtkWhFinal.ReadOnly = true;
            this.txtkWhFinal.Size = new System.Drawing.Size(104, 20);
            this.txtkWhFinal.TabIndex = 9;
            // 
            // txtkVAhInitial
            // 
            this.txtkVAhInitial.Location = new System.Drawing.Point(159, 93);
            this.txtkVAhInitial.Name = "txtkVAhInitial";
            this.txtkVAhInitial.ReadOnly = true;
            this.txtkVAhInitial.Size = new System.Drawing.Size(104, 20);
            this.txtkVAhInitial.TabIndex = 7;
            // 
            // txtkWhInitial
            // 
            this.txtkWhInitial.Location = new System.Drawing.Point(159, 50);
            this.txtkWhInitial.Name = "txtkWhInitial";
            this.txtkWhInitial.ReadOnly = true;
            this.txtkWhInitial.Size = new System.Drawing.Size(104, 20);
            this.txtkWhInitial.TabIndex = 6;
            // 
            // lblDelta
            // 
            this.lblDelta.AutoSize = true;
            this.lblDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelta.Location = new System.Drawing.Point(393, 27);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(32, 13);
            this.lblDelta.TabIndex = 5;
            this.lblDelta.Text = "Delta";
            // 
            // lblFinalReading
            // 
            this.lblFinalReading.AutoSize = true;
            this.lblFinalReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFinalReading.Location = new System.Drawing.Point(278, 27);
            this.lblFinalReading.Name = "lblFinalReading";
            this.lblFinalReading.Size = new System.Drawing.Size(72, 13);
            this.lblFinalReading.TabIndex = 4;
            this.lblFinalReading.Text = "Final Reading";
            // 
            // lblInitialReading
            // 
            this.lblInitialReading.AutoSize = true;
            this.lblInitialReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInitialReading.Location = new System.Drawing.Point(166, 27);
            this.lblInitialReading.Name = "lblInitialReading";
            this.lblInitialReading.Size = new System.Drawing.Size(74, 13);
            this.lblInitialReading.TabIndex = 3;
            this.lblInitialReading.Text = "Initial Reading";
            // 
            // lblkvarh
            // 
            this.lblkvarh.AutoSize = true;
            this.lblkvarh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkvarh.Location = new System.Drawing.Point(41, 138);
            this.lblkvarh.Name = "lblkvarh";
            this.lblkvarh.Size = new System.Drawing.Size(110, 13);
            this.lblkvarh.TabIndex = 2;
            this.lblkvarh.Text = "Reactive Energy(Lag)";
            // 
            // lblkVAh
            // 
            this.lblkVAh.AutoSize = true;
            this.lblkVAh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkVAh.Location = new System.Drawing.Point(41, 97);
            this.lblkVAh.Name = "lblkVAh";
            this.lblkVAh.Size = new System.Drawing.Size(86, 13);
            this.lblkVAh.TabIndex = 1;
            this.lblkVAh.Text = "Apparent Energy";
            // 
            // lblkWh
            // 
            this.lblkWh.AutoSize = true;
            this.lblkWh.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblkWh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblkWh.Location = new System.Drawing.Point(41, 54);
            this.lblkWh.Name = "lblkWh";
            this.lblkWh.Size = new System.Drawing.Size(73, 13);
            this.lblkWh.TabIndex = 0;
            this.lblkWh.Text = "Active Energy";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(333, 317);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(428, 317);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Duration_Timer
            // 
            this.Duration_Timer.Tick += new System.EventHandler(this.Duration_Timer_Tick);
            // 
            // lblReactiveLeadUnit
            // 
            this.lblReactiveLeadUnit.AutoSize = true;
            this.lblReactiveLeadUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactiveLeadUnit.Location = new System.Drawing.Point(503, 179);
            this.lblReactiveLeadUnit.Name = "lblReactiveLeadUnit";
            this.lblReactiveLeadUnit.Size = new System.Drawing.Size(36, 13);
            this.lblReactiveLeadUnit.TabIndex = 29;
            this.lblReactiveLeadUnit.Text = "kVArh";
            this.lblReactiveLeadUnit.Visible = false;
            // 
            // lblReactiveLagUnit
            // 
            this.lblReactiveLagUnit.AutoSize = true;
            this.lblReactiveLagUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReactiveLagUnit.Location = new System.Drawing.Point(503, 139);
            this.lblReactiveLagUnit.Name = "lblReactiveLagUnit";
            this.lblReactiveLagUnit.Size = new System.Drawing.Size(36, 13);
            this.lblReactiveLagUnit.TabIndex = 28;
            this.lblReactiveLagUnit.Text = "kVArh";
            this.lblReactiveLagUnit.Visible = false;
            // 
            // lblApparentEnergyUnit
            // 
            this.lblApparentEnergyUnit.AutoSize = true;
            this.lblApparentEnergyUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApparentEnergyUnit.Location = new System.Drawing.Point(503, 98);
            this.lblApparentEnergyUnit.Name = "lblApparentEnergyUnit";
            this.lblApparentEnergyUnit.Size = new System.Drawing.Size(33, 13);
            this.lblApparentEnergyUnit.TabIndex = 27;
            this.lblApparentEnergyUnit.Text = "kVAh";
            this.lblApparentEnergyUnit.Visible = false;
            // 
            // lblActiveEnergyUnit
            // 
            this.lblActiveEnergyUnit.AutoSize = true;
            this.lblActiveEnergyUnit.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblActiveEnergyUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveEnergyUnit.Location = new System.Drawing.Point(503, 55);
            this.lblActiveEnergyUnit.Name = "lblActiveEnergyUnit";
            this.lblActiveEnergyUnit.Size = new System.Drawing.Size(30, 13);
            this.lblActiveEnergyUnit.TabIndex = 26;
            this.lblActiveEnergyUnit.Text = "kWh";
            this.lblActiveEnergyUnit.Visible = false;
            // 
            // IECMeterAccuracyCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(655, 383);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Name = "IECMeterAccuracyCheck";
            this.StatusMessage = "";
            this.Text = "Meter Accuracy Check";
            this.Load += new System.EventHandler(this.IECMeterAccuracyCheck_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblduration;
        private System.Windows.Forms.TextBox txtkvarhLagInitial;
        private System.Windows.Forms.TextBox txtkvarhLeadDelta;
        private System.Windows.Forms.TextBox txtkvarhLeadFinal;
        private System.Windows.Forms.TextBox txtkvarhLeadInitial;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTestduration;
        private System.Windows.Forms.Label lblTestDuration;
        private System.Windows.Forms.TextBox txtkvarhLagDelta;
        private System.Windows.Forms.TextBox txtkVAhDelta;
        private System.Windows.Forms.TextBox txtkWhDelta;
        private System.Windows.Forms.TextBox txtkvarhLagFinal;
        private System.Windows.Forms.TextBox txtkVAhFinal;
        private System.Windows.Forms.TextBox txtkWhFinal;
        private System.Windows.Forms.TextBox txtkVAhInitial;
        private System.Windows.Forms.TextBox txtkWhInitial;
        private System.Windows.Forms.Label lblDelta;
        private System.Windows.Forms.Label lblFinalReading;
        private System.Windows.Forms.Label lblInitialReading;
        private System.Windows.Forms.Label lblkvarh;
        private System.Windows.Forms.Label lblkVAh;
        private System.Windows.Forms.Label lblkWh;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Timer Duration_Timer;
        private System.Windows.Forms.Label lblReactiveLeadUnit;
        private System.Windows.Forms.Label lblReactiveLagUnit;
        private System.Windows.Forms.Label lblApparentEnergyUnit;
        private System.Windows.Forms.Label lblActiveEnergyUnit;
    }
}
