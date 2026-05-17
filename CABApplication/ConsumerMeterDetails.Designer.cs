using System;
using CAB.Framework;
namespace CAB.UI
{
    partial class ConsumerMeterDetails
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtMeterLocation = new System.Windows.Forms.TextBox();
            this.txtEMF = new System.Windows.Forms.TextBox();
            this.btnEnterNewMeterDetails = new CAB.UI.Controls.CABButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ddlDivision = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDivision = new CAB.UI.Controls.CABLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlCircle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlRegion = new System.Windows.Forms.ComboBox();
            this.lngLabel7 = new CAB.UI.Controls.CABLabel();
            this.lblCircle = new CAB.UI.Controls.CABLabel();
            this.lblCityTown = new CAB.UI.Controls.CABLabel();
            this.lblStreet = new CAB.UI.Controls.CABLabel();
            this.lblTelephoneNumber = new CAB.UI.Controls.CABLabel();
            this.lblHouseNo = new CAB.UI.Controls.CABLabel();
            this.txtConsumerHouseNo = new System.Windows.Forms.TextBox();
            this.lblRegion = new CAB.UI.Controls.CABLabel();
            this.lblConsumerType = new CAB.UI.Controls.CABLabel();
            this.lblConsumerName = new CAB.UI.Controls.CABLabel();
            this.lblConsumerID = new CAB.UI.Controls.CABLabel();
            this.txtConsumerEmail = new System.Windows.Forms.TextBox();
            this.txtConsumerCity = new System.Windows.Forms.TextBox();
            this.txtConsumerStreet = new System.Windows.Forms.TextBox();
            this.txtConsumerNumber = new System.Windows.Forms.TextBox();
            this.txtConsumerName = new System.Windows.Forms.TextBox();
            this.cboConsumerType = new System.Windows.Forms.ComboBox();
            this.chkSuspectedConsumer = new System.Windows.Forms.CheckBox();
            this.txtConsumerTelephone = new System.Windows.Forms.TextBox();
            this.btnSave = new CAB.UI.Controls.CABButton();
            this.btnCancel = new CAB.UI.Controls.CABButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNote = new CAB.UI.Controls.CABLabel();
            this.lblindication = new CAB.UI.Controls.CABLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbAreaDetails = new System.Windows.Forms.GroupBox();
            this.lngLabel6 = new CAB.UI.Controls.CABLabel();
            this.textBoxModemIMEI = new System.Windows.Forms.TextBox();
            this.lnglabelIMEI = new CAB.UI.Controls.CABLabel();
            this.ddlCommunicationtype = new System.Windows.Forms.ComboBox();
            this.lngCommunicationType = new CAB.UI.Controls.CABLabel();
            this.txtBoxMeterSIM = new System.Windows.Forms.TextBox();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblExternalEMFValue = new System.Windows.Forms.Label();
            this.lblExtrenalPTRValue = new System.Windows.Forms.Label();
            this.lblExternalCTRValue = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblExternalCTR = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.chkUseEMFInCalculation = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContractDemand = new System.Windows.Forms.MaskedTextBox();
            this.cboMeterModel = new System.Windows.Forms.ComboBox();
            this.chkActivateMeter = new System.Windows.Forms.CheckBox();
            this.lblInstalledPTRatio = new CAB.UI.Controls.CABLabel();
            this.lblInstalledPTSecondary = new CAB.UI.Controls.CABLabel();
            this.lblInstalledPTPrimary = new CAB.UI.Controls.CABLabel();
            this.lblInstalledCTRatio = new CAB.UI.Controls.CABLabel();
            this.lblInstalledCTSecondary = new CAB.UI.Controls.CABLabel();
            this.lblInstalledCTPrimary = new CAB.UI.Controls.CABLabel();
            this.lblContractDemand = new CAB.UI.Controls.CABLabel();
            this.lblInstEMF = new CAB.UI.Controls.CABLabel();
            this.lblInstallationDate = new CAB.UI.Controls.CABLabel();
            this.lblLocation = new CAB.UI.Controls.CABLabel();
            this.lblMeterModel = new CAB.UI.Controls.CABLabel();
            this.lblMeterType = new CAB.UI.Controls.CABLabel();
            this.lblMeterNumber = new CAB.UI.Controls.CABLabel();
            this.CMD_lblInstalledCTPT = new System.Windows.Forms.Label();
            this.chkShowMeterDeactive = new System.Windows.Forms.CheckBox();
            this.CMD_lblMeterCTPT = new System.Windows.Forms.Label();
            this.txtInstalledCTPrimary = new System.Windows.Forms.TextBox();
            this.txtInstalledCTSecondary = new System.Windows.Forms.TextBox();
            this.txtInstalledCTRatio = new System.Windows.Forms.TextBox();
            this.txtInstalledPTPrimary = new System.Windows.Forms.TextBox();
            this.txtInstalledPTSecondary = new System.Windows.Forms.TextBox();
            this.txtInstalledPTRatio = new System.Windows.Forms.TextBox();
            this.cboUnits = new System.Windows.Forms.ComboBox();
            this.DtpInstalled = new System.Windows.Forms.DateTimePicker();
            this.cboMeterType = new System.Windows.Forms.ComboBox();
            this.cboMeterNumber = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtInternEMF = new System.Windows.Forms.TextBox();
            this.lblInternEMF = new System.Windows.Forms.Label();
            this.lblCTRatio = new CAB.UI.Controls.CABLabel();
            this.lblPTRatio = new CAB.UI.Controls.CABLabel();
            this.txtMeterCTRatio = new System.Windows.Forms.TextBox();
            this.txtMeterPTRatio = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbAreaDetails.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMeterLocation
            // 
            this.txtMeterLocation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMeterLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txtMeterLocation.Location = new System.Drawing.Point(170, 208);
            this.txtMeterLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMeterLocation.MaxLength = 20;
            this.txtMeterLocation.Name = "txtMeterLocation";
            this.txtMeterLocation.Size = new System.Drawing.Size(190, 26);
            this.txtMeterLocation.TabIndex = 17;
            this.toolTip1.SetToolTip(this.txtMeterLocation, " Location identifies the DT/ Transformer meter is connetced to.");
            // 
            // txtEMF
            // 
            this.txtEMF.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtEMF.BackColor = System.Drawing.SystemColors.Control;
            this.txtEMF.Enabled = false;
            this.txtEMF.HideSelection = false;
            this.txtEMF.Location = new System.Drawing.Point(906, 305);
            this.txtEMF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtEMF.MaxLength = 6;
            this.txtEMF.Name = "txtEMF";
            this.txtEMF.Size = new System.Drawing.Size(148, 26);
            this.txtEMF.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtEMF, "External Multiplication Factor");
            this.txtEMF.TextChanged += new System.EventHandler(this.txtEMF_TextChanged);
            // 
            // btnEnterNewMeterDetails
            // 
            this.btnEnterNewMeterDetails.Location = new System.Drawing.Point(434, 232);
            this.btnEnterNewMeterDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEnterNewMeterDetails.Name = "btnEnterNewMeterDetails";
            this.btnEnterNewMeterDetails.Size = new System.Drawing.Size(255, 46);
            this.btnEnterNewMeterDetails.TabIndex = 0;
            this.btnEnterNewMeterDetails.Text = "Enter &New Meter Details";
            this.btnEnterNewMeterDetails.TranslationKey = "B000010";
            this.btnEnterNewMeterDetails.UseVisualStyleBackColor = false;
            this.btnEnterNewMeterDetails.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnEnterNewMeterDetails.ForeColor = System.Drawing.Color.White;
            this.btnEnterNewMeterDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnterNewMeterDetails.FlatAppearance.BorderSize = 0;
            this.btnEnterNewMeterDetails.Click += new System.EventHandler(this.btnEnterNewMeterDetails_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.ddlDivision);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lblDivision);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ddlCircle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ddlRegion);
            this.groupBox1.Controls.Add(this.lngLabel7);
            this.groupBox1.Controls.Add(this.lblCircle);
            this.groupBox1.Controls.Add(this.lblCityTown);
            this.groupBox1.Controls.Add(this.lblStreet);
            this.groupBox1.Controls.Add(this.lblTelephoneNumber);
            this.groupBox1.Controls.Add(this.lblHouseNo);
            this.groupBox1.Controls.Add(this.txtConsumerHouseNo);
            this.groupBox1.Controls.Add(this.lblRegion);
            this.groupBox1.Controls.Add(this.lblConsumerType);
            this.groupBox1.Controls.Add(this.lblConsumerName);
            this.groupBox1.Controls.Add(this.lblConsumerID);
            this.groupBox1.Controls.Add(this.txtConsumerEmail);
            this.groupBox1.Controls.Add(this.txtConsumerCity);
            this.groupBox1.Controls.Add(this.txtConsumerStreet);
            this.groupBox1.Controls.Add(this.txtConsumerNumber);
            this.groupBox1.Controls.Add(this.txtConsumerName);
            this.groupBox1.Controls.Add(this.cboConsumerType);
            this.groupBox1.Controls.Add(this.chkSuspectedConsumer);
            this.groupBox1.Controls.Add(this.txtConsumerTelephone);
            this.groupBox1.Location = new System.Drawing.Point(21, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1113, 208);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consumer Information";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(741, 120);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 20);
            this.label10.TabIndex = 38;
            this.label10.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(740, 35);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(16, 20);
            this.label14.TabIndex = 22;
            this.label14.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(393, 118);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 20);
            this.label9.TabIndex = 37;
            this.label9.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(394, 38);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 20);
            this.label13.TabIndex = 21;
            this.label13.Text = "*";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(33, 120);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 20);
            this.label8.TabIndex = 36;
            this.label8.Text = "*";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(740, 75);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 20);
            this.label12.TabIndex = 20;
            this.label12.Text = "*";
            // 
            // ddlDivision
            // 
            this.ddlDivision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDivision.FormattingEnabled = true;
            this.ddlDivision.Location = new System.Drawing.Point(908, 118);
            this.ddlDivision.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlDivision.Name = "ddlDivision";
            this.ddlDivision.Size = new System.Drawing.Size(139, 28);
            this.ddlDivision.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(396, 80);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 20);
            this.label11.TabIndex = 19;
            this.label11.Text = "*";
            // 
            // lblDivision
            // 
            this.lblDivision.AutoSize = true;
            this.lblDivision.Location = new System.Drawing.Point(753, 120);
            this.lblDivision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDivision.Name = "lblDivision";
            this.lblDivision.Size = new System.Drawing.Size(109, 20);
            this.lblDivision.TabIndex = 20;
            this.lblDivision.Text = "Division Name";
            this.lblDivision.TranslationKey = "L000043";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(32, 35);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "*";
            // 
            // ddlCircle
            // 
            this.ddlCircle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCircle.FormattingEnabled = true;
            this.ddlCircle.Location = new System.Drawing.Point(562, 117);
            this.ddlCircle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlCircle.Name = "ddlCircle";
            this.ddlCircle.Size = new System.Drawing.Size(139, 28);
            this.ddlCircle.TabIndex = 8;
            this.ddlCircle.SelectedIndexChanged += new System.EventHandler(this.ddlCircle_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(32, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "*";
            // 
            // ddlRegion
            // 
            this.ddlRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRegion.FormattingEnabled = true;
            this.ddlRegion.Location = new System.Drawing.Point(200, 118);
            this.ddlRegion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlRegion.Name = "ddlRegion";
            this.ddlRegion.Size = new System.Drawing.Size(139, 28);
            this.ddlRegion.TabIndex = 7;
            this.ddlRegion.SelectedIndexChanged += new System.EventHandler(this.ddlRegion_SelectedIndexChanged);
            // 
            // lngLabel7
            // 
            this.lngLabel7.AutoSize = true;
            this.lngLabel7.Location = new System.Drawing.Point(408, 160);
            this.lngLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel7.Name = "lngLabel7";
            this.lngLabel7.Size = new System.Drawing.Size(48, 20);
            this.lngLabel7.TabIndex = 7;
            this.lngLabel7.Text = "Email";
            this.lngLabel7.TranslationKey = "L000021";
            // 
            // lblCircle
            // 
            this.lblCircle.AutoSize = true;
            this.lblCircle.Location = new System.Drawing.Point(408, 120);
            this.lblCircle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCircle.Name = "lblCircle";
            this.lblCircle.Size = new System.Drawing.Size(94, 20);
            this.lblCircle.TabIndex = 19;
            this.lblCircle.Text = "Circle Name";
            this.lblCircle.TranslationKey = "L000042";
            // 
            // lblCityTown
            // 
            this.lblCityTown.AutoSize = true;
            this.lblCityTown.Location = new System.Drawing.Point(753, 80);
            this.lblCityTown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCityTown.Name = "lblCityTown";
            this.lblCityTown.Size = new System.Drawing.Size(77, 20);
            this.lblCityTown.TabIndex = 6;
            this.lblCityTown.Text = "City/Town";
            this.lblCityTown.TranslationKey = "L000020";
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(408, 80);
            this.lblStreet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(53, 20);
            this.lblStreet.TabIndex = 5;
            this.lblStreet.Text = "Street";
            this.lblStreet.TranslationKey = "L000019";
            // 
            // lblTelephoneNumber
            // 
            this.lblTelephoneNumber.AutoSize = true;
            this.lblTelephoneNumber.Location = new System.Drawing.Point(44, 160);
            this.lblTelephoneNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTelephoneNumber.Name = "lblTelephoneNumber";
            this.lblTelephoneNumber.Size = new System.Drawing.Size(144, 20);
            this.lblTelephoneNumber.TabIndex = 3;
            this.lblTelephoneNumber.Text = "Telephone Number";
            this.lblTelephoneNumber.TranslationKey = "L000017";
            // 
            // lblHouseNo
            // 
            this.lblHouseNo.AutoSize = true;
            this.lblHouseNo.Location = new System.Drawing.Point(45, 80);
            this.lblHouseNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHouseNo.Name = "lblHouseNo";
            this.lblHouseNo.Size = new System.Drawing.Size(116, 20);
            this.lblHouseNo.TabIndex = 4;
            this.lblHouseNo.Text = "House Number";
            this.lblHouseNo.TranslationKey = "L000018";
            // 
            // txtConsumerHouseNo
            // 
            this.txtConsumerHouseNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerHouseNo.Location = new System.Drawing.Point(200, 74);
            this.txtConsumerHouseNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerHouseNo.MaxLength = 20;
            this.txtConsumerHouseNo.Name = "txtConsumerHouseNo";
            this.txtConsumerHouseNo.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerHouseNo.TabIndex = 4;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(45, 120);
            this.lblRegion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(106, 20);
            this.lblRegion.TabIndex = 18;
            this.lblRegion.Text = "Region Name";
            this.lblRegion.TranslationKey = "L000041";
            // 
            // lblConsumerType
            // 
            this.lblConsumerType.AutoSize = true;
            this.lblConsumerType.Location = new System.Drawing.Point(753, 40);
            this.lblConsumerType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConsumerType.Name = "lblConsumerType";
            this.lblConsumerType.Size = new System.Drawing.Size(120, 20);
            this.lblConsumerType.TabIndex = 2;
            this.lblConsumerType.Text = "Consumer Type";
            this.lblConsumerType.TranslationKey = "L000016";
            // 
            // lblConsumerName
            // 
            this.lblConsumerName.AutoSize = true;
            this.lblConsumerName.Location = new System.Drawing.Point(408, 40);
            this.lblConsumerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConsumerName.Name = "lblConsumerName";
            this.lblConsumerName.Size = new System.Drawing.Size(132, 20);
            this.lblConsumerName.TabIndex = 1;
            this.lblConsumerName.Text = "Consumer Name.";
            this.lblConsumerName.TranslationKey = "L000015";
            // 
            // lblConsumerID
            // 
            this.lblConsumerID.AutoSize = true;
            this.lblConsumerID.Location = new System.Drawing.Point(45, 40);
            this.lblConsumerID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConsumerID.Name = "lblConsumerID";
            this.lblConsumerID.Size = new System.Drawing.Size(107, 20);
            this.lblConsumerID.TabIndex = 0;
            this.lblConsumerID.Text = "Consumer ID.";
            this.lblConsumerID.TranslationKey = "L000014";
            // 
            // txtConsumerEmail
            // 
            this.txtConsumerEmail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerEmail.Location = new System.Drawing.Point(562, 155);
            this.txtConsumerEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerEmail.MaxLength = 50;
            this.txtConsumerEmail.Name = "txtConsumerEmail";
            this.txtConsumerEmail.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerEmail.TabIndex = 11;
            // 
            // txtConsumerCity
            // 
            this.txtConsumerCity.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerCity.Location = new System.Drawing.Point(908, 74);
            this.txtConsumerCity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerCity.MaxLength = 20;
            this.txtConsumerCity.Name = "txtConsumerCity";
            this.txtConsumerCity.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerCity.TabIndex = 6;
            // 
            // txtConsumerStreet
            // 
            this.txtConsumerStreet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerStreet.Location = new System.Drawing.Point(562, 74);
            this.txtConsumerStreet.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerStreet.MaxLength = 20;
            this.txtConsumerStreet.Name = "txtConsumerStreet";
            this.txtConsumerStreet.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerStreet.TabIndex = 5;
            // 
            // txtConsumerNumber
            // 
            this.txtConsumerNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerNumber.Location = new System.Drawing.Point(200, 32);
            this.txtConsumerNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerNumber.MaxLength = 20;
            this.txtConsumerNumber.Name = "txtConsumerNumber";
            this.txtConsumerNumber.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerNumber.TabIndex = 0;
            // 
            // txtConsumerName
            // 
            this.txtConsumerName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConsumerName.Location = new System.Drawing.Point(562, 32);
            this.txtConsumerName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerName.MaxLength = 40;
            this.txtConsumerName.Name = "txtConsumerName";
            this.txtConsumerName.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerName.TabIndex = 1;
            // 
            // cboConsumerType
            // 
            this.cboConsumerType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboConsumerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConsumerType.FormattingEnabled = true;
            this.cboConsumerType.Items.AddRange(new object[] {
            "Industrial",
            "Substation",
            "Feeder",
            "DT",
            "LT Bulk Customer",
            "Others"});
            this.cboConsumerType.Location = new System.Drawing.Point(908, 32);
            this.cboConsumerType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboConsumerType.Name = "cboConsumerType";
            this.cboConsumerType.Size = new System.Drawing.Size(148, 28);
            this.cboConsumerType.TabIndex = 2;
            // 
            // chkSuspectedConsumer
            // 
            this.chkSuspectedConsumer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkSuspectedConsumer.AutoSize = true;
            this.chkSuspectedConsumer.Location = new System.Drawing.Point(758, 157);
            this.chkSuspectedConsumer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSuspectedConsumer.Name = "chkSuspectedConsumer";
            this.chkSuspectedConsumer.Size = new System.Drawing.Size(186, 24);
            this.chkSuspectedConsumer.TabIndex = 12;
            this.chkSuspectedConsumer.Text = "Suspected consumer";
            this.chkSuspectedConsumer.UseVisualStyleBackColor = true;
            // 
            // txtConsumerTelephone
            // 
            this.txtConsumerTelephone.Location = new System.Drawing.Point(200, 160);
            this.txtConsumerTelephone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConsumerTelephone.MaxLength = 15;
            this.txtConsumerTelephone.Name = "txtConsumerTelephone";
            this.txtConsumerTelephone.Size = new System.Drawing.Size(148, 26);
            this.txtConsumerTelephone.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(940, 802);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 48);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "&Save";
            this.btnSave.TranslationKey = "B000008";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1044, 802);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 48);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.TranslationKey = "B000009";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(75, 802);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "*";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(16, 802);
            this.lblNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(47, 20);
            this.lblNote.TabIndex = 19;
            this.lblNote.Text = "Note:";
            this.lblNote.TranslationKey = "L000008";
            // 
            // lblindication
            // 
            this.lblindication.AutoSize = true;
            this.lblindication.Location = new System.Drawing.Point(86, 802);
            this.lblindication.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblindication.Name = "lblindication";
            this.lblindication.Size = new System.Drawing.Size(192, 20);
            this.lblindication.TabIndex = 20;
            this.lblindication.Text = "indicates mandatory fields";
            this.lblindication.TranslationKey = "L000040";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gbAreaDetails);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(21, 285);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1113, 506);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meter";
            // 
            // gbAreaDetails
            // 
            this.gbAreaDetails.Controls.Add(this.lngLabel6);
            this.gbAreaDetails.Controls.Add(this.textBoxModemIMEI);
            this.gbAreaDetails.Controls.Add(this.lnglabelIMEI);
            this.gbAreaDetails.Controls.Add(this.ddlCommunicationtype);
            this.gbAreaDetails.Controls.Add(this.lngCommunicationType);
            this.gbAreaDetails.Controls.Add(this.txtBoxMeterSIM);
            this.gbAreaDetails.Controls.Add(this.lngLabel1);
            this.gbAreaDetails.Location = new System.Drawing.Point(16, 388);
            this.gbAreaDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbAreaDetails.Name = "gbAreaDetails";
            this.gbAreaDetails.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbAreaDetails.Size = new System.Drawing.Size(1080, 111);
            this.gbAreaDetails.TabIndex = 23;
            this.gbAreaDetails.TabStop = false;
            this.gbAreaDetails.Text = "Communication Information";
            // 
            // lngLabel6
            // 
            this.lngLabel6.AutoSize = true;
            this.lngLabel6.Location = new System.Drawing.Point(28, 49);
            this.lngLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel6.Name = "lngLabel6";
            this.lngLabel6.Size = new System.Drawing.Size(43, 20);
            this.lngLabel6.TabIndex = 42;
            this.lngLabel6.Text = "Type";
            this.lngLabel6.TranslationKey = "";
            // 
            // textBoxModemIMEI
            // 
            this.textBoxModemIMEI.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxModemIMEI.Location = new System.Drawing.Point(906, 34);
            this.textBoxModemIMEI.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxModemIMEI.MaxLength = 15;
            this.textBoxModemIMEI.Name = "textBoxModemIMEI";
            this.textBoxModemIMEI.Size = new System.Drawing.Size(148, 26);
            this.textBoxModemIMEI.TabIndex = 36;
            this.textBoxModemIMEI.Visible = false;
            // 
            // lnglabelIMEI
            // 
            this.lnglabelIMEI.AutoSize = true;
            this.lnglabelIMEI.Location = new System.Drawing.Point(736, 38);
            this.lnglabelIMEI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnglabelIMEI.Name = "lnglabelIMEI";
            this.lnglabelIMEI.Size = new System.Drawing.Size(127, 20);
            this.lnglabelIMEI.TabIndex = 35;
            this.lnglabelIMEI.Text = "Modem IMEI / IP";
            this.lnglabelIMEI.TranslationKey = "";
            this.lnglabelIMEI.Visible = false;
            // 
            // ddlCommunicationtype
            // 
            this.ddlCommunicationtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCommunicationtype.FormattingEnabled = true;
            this.ddlCommunicationtype.Items.AddRange(new object[] {
            "DIRECT",
            "GSM",
            "PSTN",
            "GPRS",
            "TCP",
            "FTP"});
            this.ddlCommunicationtype.Location = new System.Drawing.Point(170, 29);
            this.ddlCommunicationtype.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ddlCommunicationtype.Name = "ddlCommunicationtype";
            this.ddlCommunicationtype.Size = new System.Drawing.Size(190, 28);
            this.ddlCommunicationtype.TabIndex = 27;
            this.ddlCommunicationtype.SelectedIndexChanged += new System.EventHandler(this.ddlCommunicationtype_SelectedIndexChanged);
            // 
            // lngCommunicationType
            // 
            this.lngCommunicationType.AutoSize = true;
            this.lngCommunicationType.Location = new System.Drawing.Point(28, 29);
            this.lngCommunicationType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngCommunicationType.Name = "lngCommunicationType";
            this.lngCommunicationType.Size = new System.Drawing.Size(119, 20);
            this.lngCommunicationType.TabIndex = 32;
            this.lngCommunicationType.Text = "Communication";
            this.lngCommunicationType.TranslationKey = "";
            // 
            // txtBoxMeterSIM
            // 
            this.txtBoxMeterSIM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxMeterSIM.Enabled = false;
            this.txtBoxMeterSIM.Location = new System.Drawing.Point(570, 34);
            this.txtBoxMeterSIM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBoxMeterSIM.MaxLength = 10;
            this.txtBoxMeterSIM.Name = "txtBoxMeterSIM";
            this.txtBoxMeterSIM.Size = new System.Drawing.Size(148, 26);
            this.txtBoxMeterSIM.TabIndex = 28;
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(406, 38);
            this.lngLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(146, 20);
            this.lngLabel1.TabIndex = 5;
            this.lngLabel1.Text = "Meter SIM No.  +91";
            this.lngLabel1.TranslationKey = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.chkUseEMFInCalculation);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtContractDemand);
            this.groupBox3.Controls.Add(this.cboMeterModel);
            this.groupBox3.Controls.Add(this.chkActivateMeter);
            this.groupBox3.Controls.Add(this.lblInstalledPTRatio);
            this.groupBox3.Controls.Add(this.lblInstalledPTSecondary);
            this.groupBox3.Controls.Add(this.lblInstalledPTPrimary);
            this.groupBox3.Controls.Add(this.lblInstalledCTRatio);
            this.groupBox3.Controls.Add(this.lblInstalledCTSecondary);
            this.groupBox3.Controls.Add(this.lblInstalledCTPrimary);
            this.groupBox3.Controls.Add(this.lblContractDemand);
            this.groupBox3.Controls.Add(this.lblInstEMF);
            this.groupBox3.Controls.Add(this.lblInstallationDate);
            this.groupBox3.Controls.Add(this.lblLocation);
            this.groupBox3.Controls.Add(this.lblMeterModel);
            this.groupBox3.Controls.Add(this.lblMeterType);
            this.groupBox3.Controls.Add(this.lblMeterNumber);
            this.groupBox3.Controls.Add(this.CMD_lblInstalledCTPT);
            this.groupBox3.Controls.Add(this.chkShowMeterDeactive);
            this.groupBox3.Controls.Add(this.txtMeterLocation);
            this.groupBox3.Controls.Add(this.CMD_lblMeterCTPT);
            this.groupBox3.Controls.Add(this.txtInstalledCTPrimary);
            this.groupBox3.Controls.Add(this.txtInstalledCTSecondary);
            this.groupBox3.Controls.Add(this.txtInstalledCTRatio);
            this.groupBox3.Controls.Add(this.txtInstalledPTPrimary);
            this.groupBox3.Controls.Add(this.txtInstalledPTSecondary);
            this.groupBox3.Controls.Add(this.txtInstalledPTRatio);
            this.groupBox3.Controls.Add(this.txtEMF);
            this.groupBox3.Controls.Add(this.cboUnits);
            this.groupBox3.Controls.Add(this.DtpInstalled);
            this.groupBox3.Controls.Add(this.cboMeterType);
            this.groupBox3.Controls.Add(this.cboMeterNumber);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox7);
            this.groupBox3.Location = new System.Drawing.Point(16, 25);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1080, 360);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Meter Information";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblExternalEMFValue);
            this.groupBox4.Controls.Add(this.lblExtrenalPTRValue);
            this.groupBox4.Controls.Add(this.lblExternalCTRValue);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.lblExternalCTR);
            this.groupBox4.Location = new System.Drawing.Point(396, 220);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(324, 134);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "MF";
            // 
            // lblExternalEMFValue
            // 
            this.lblExternalEMFValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExternalEMFValue.Location = new System.Drawing.Point(66, 94);
            this.lblExternalEMFValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalEMFValue.Name = "lblExternalEMFValue";
            this.lblExternalEMFValue.Size = new System.Drawing.Size(250, 23);
            this.lblExternalEMFValue.TabIndex = 5;
            this.lblExternalEMFValue.Text = "1";
            // 
            // lblExtrenalPTRValue
            // 
            this.lblExtrenalPTRValue.Location = new System.Drawing.Point(66, 62);
            this.lblExtrenalPTRValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExtrenalPTRValue.Name = "lblExtrenalPTRValue";
            this.lblExtrenalPTRValue.Size = new System.Drawing.Size(297, 20);
            this.lblExtrenalPTRValue.TabIndex = 4;
            this.lblExtrenalPTRValue.Text = "1";
            // 
            // lblExternalCTRValue
            // 
            this.lblExternalCTRValue.Location = new System.Drawing.Point(66, 28);
            this.lblExternalCTRValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalCTRValue.Name = "lblExternalCTRValue";
            this.lblExternalCTRValue.Size = new System.Drawing.Size(298, 20);
            this.lblExternalCTRValue.TabIndex = 3;
            this.lblExternalCTRValue.Text = "1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 92);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(48, 20);
            this.label17.TabIndex = 2;
            this.label17.Text = "MF   :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 62);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "PTR :";
            // 
            // lblExternalCTR
            // 
            this.lblExternalCTR.AutoSize = true;
            this.lblExternalCTR.Location = new System.Drawing.Point(10, 31);
            this.lblExternalCTR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalCTR.Name = "lblExternalCTR";
            this.lblExternalCTR.Size = new System.Drawing.Size(49, 20);
            this.lblExternalCTR.TabIndex = 0;
            this.lblExternalCTR.Text = "CTR :";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(16, 295);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(16, 20);
            this.label18.TabIndex = 31;
            this.label18.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(16, 91);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 20);
            this.label16.TabIndex = 29;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(16, 131);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 20);
            this.label15.TabIndex = 28;
            this.label15.Text = "*";
            // 
            // chkUseEMFInCalculation
            // 
            this.chkUseEMFInCalculation.AutoSize = true;
            this.chkUseEMFInCalculation.Checked = true;
            this.chkUseEMFInCalculation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseEMFInCalculation.Location = new System.Drawing.Point(402, 189);
            this.chkUseEMFInCalculation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkUseEMFInCalculation.Name = "chkUseEMFInCalculation";
            this.chkUseEMFInCalculation.Size = new System.Drawing.Size(125, 24);
            this.chkUseEMFInCalculation.TabIndex = 21;
            this.chkUseEMFInCalculation.Text = "Multiply EMF";
            this.chkUseEMFInCalculation.UseVisualStyleBackColor = true;
            this.chkUseEMFInCalculation.CheckedChanged += new System.EventHandler(this.chkUseEMFInCalculation_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(16, 255);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 20);
            this.label6.TabIndex = 22;
            this.label6.Text = "*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(16, 174);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 20);
            this.label3.TabIndex = 18;
            this.label3.Text = "*";
            // 
            // txtContractDemand
            // 
            this.txtContractDemand.Location = new System.Drawing.Point(170, 292);
            this.txtContractDemand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtContractDemand.Mask = "00.00";
            this.txtContractDemand.Name = "txtContractDemand";
            this.txtContractDemand.PromptChar = '0';
            this.txtContractDemand.Size = new System.Drawing.Size(61, 26);
            this.txtContractDemand.TabIndex = 19;
            this.txtContractDemand.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // cboMeterModel
            // 
            this.cboMeterModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMeterModel.FormattingEnabled = true;
            this.cboMeterModel.Items.AddRange(new object[] {
            "1P2W",
            "3P3W",
            "3P4W"});
            this.cboMeterModel.Location = new System.Drawing.Point(170, 169);
            this.cboMeterModel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboMeterModel.Name = "cboMeterModel";
            this.cboMeterModel.Size = new System.Drawing.Size(190, 28);
            this.cboMeterModel.TabIndex = 16;
            // 
            // chkActivateMeter
            // 
            this.chkActivateMeter.AutoSize = true;
            this.chkActivateMeter.Checked = true;
            this.chkActivateMeter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivateMeter.Location = new System.Drawing.Point(561, 189);
            this.chkActivateMeter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkActivateMeter.Name = "chkActivateMeter";
            this.chkActivateMeter.Size = new System.Drawing.Size(137, 24);
            this.chkActivateMeter.TabIndex = 26;
            this.chkActivateMeter.Text = "Activate Meter";
            this.chkActivateMeter.UseVisualStyleBackColor = true;
            // 
            // lblInstalledPTRatio
            // 
            this.lblInstalledPTRatio.AutoSize = true;
            this.lblInstalledPTRatio.Location = new System.Drawing.Point(736, 265);
            this.lblInstalledPTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledPTRatio.Name = "lblInstalledPTRatio";
            this.lblInstalledPTRatio.Size = new System.Drawing.Size(70, 20);
            this.lblInstalledPTRatio.TabIndex = 20;
            this.lblInstalledPTRatio.Text = "PT Ratio";
            this.lblInstalledPTRatio.TranslationKey = "L000035";
            // 
            // lblInstalledPTSecondary
            // 
            this.lblInstalledPTSecondary.AutoSize = true;
            this.lblInstalledPTSecondary.Location = new System.Drawing.Point(736, 222);
            this.lblInstalledPTSecondary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledPTSecondary.Name = "lblInstalledPTSecondary";
            this.lblInstalledPTSecondary.Size = new System.Drawing.Size(133, 20);
            this.lblInstalledPTSecondary.TabIndex = 19;
            this.lblInstalledPTSecondary.Text = "PT Secondary (V)";
            this.lblInstalledPTSecondary.TranslationKey = "L000034";
            // 
            // lblInstalledPTPrimary
            // 
            this.lblInstalledPTPrimary.AutoSize = true;
            this.lblInstalledPTPrimary.Location = new System.Drawing.Point(736, 178);
            this.lblInstalledPTPrimary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledPTPrimary.Name = "lblInstalledPTPrimary";
            this.lblInstalledPTPrimary.Size = new System.Drawing.Size(109, 20);
            this.lblInstalledPTPrimary.TabIndex = 18;
            this.lblInstalledPTPrimary.Text = "PT Primary (V)";
            this.lblInstalledPTPrimary.TranslationKey = "L000033";
            // 
            // lblInstalledCTRatio
            // 
            this.lblInstalledCTRatio.AutoSize = true;
            this.lblInstalledCTRatio.Location = new System.Drawing.Point(736, 135);
            this.lblInstalledCTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledCTRatio.Name = "lblInstalledCTRatio";
            this.lblInstalledCTRatio.Size = new System.Drawing.Size(71, 20);
            this.lblInstalledCTRatio.TabIndex = 17;
            this.lblInstalledCTRatio.Text = "CT Ratio";
            this.lblInstalledCTRatio.TranslationKey = "L000032";
            // 
            // lblInstalledCTSecondary
            // 
            this.lblInstalledCTSecondary.AutoSize = true;
            this.lblInstalledCTSecondary.Location = new System.Drawing.Point(736, 92);
            this.lblInstalledCTSecondary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledCTSecondary.Name = "lblInstalledCTSecondary";
            this.lblInstalledCTSecondary.Size = new System.Drawing.Size(156, 20);
            this.lblInstalledCTSecondary.TabIndex = 16;
            this.lblInstalledCTSecondary.Text = "CT Secondary (Amp)";
            this.lblInstalledCTSecondary.TranslationKey = "L000031";
            // 
            // lblInstalledCTPrimary
            // 
            this.lblInstalledCTPrimary.AutoSize = true;
            this.lblInstalledCTPrimary.Location = new System.Drawing.Point(736, 49);
            this.lblInstalledCTPrimary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstalledCTPrimary.Name = "lblInstalledCTPrimary";
            this.lblInstalledCTPrimary.Size = new System.Drawing.Size(132, 20);
            this.lblInstalledCTPrimary.TabIndex = 15;
            this.lblInstalledCTPrimary.Text = "CT Primary (Amp)";
            this.lblInstalledCTPrimary.TranslationKey = "L000030";
            // 
            // lblContractDemand
            // 
            this.lblContractDemand.AutoSize = true;
            this.lblContractDemand.Location = new System.Drawing.Point(28, 297);
            this.lblContractDemand.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContractDemand.Name = "lblContractDemand";
            this.lblContractDemand.Size = new System.Drawing.Size(135, 20);
            this.lblContractDemand.TabIndex = 6;
            this.lblContractDemand.Text = "Contract Demand";
            this.lblContractDemand.TranslationKey = "L000028";
            // 
            // lblInstEMF
            // 
            this.lblInstEMF.AutoSize = true;
            this.lblInstEMF.Location = new System.Drawing.Point(736, 308);
            this.lblInstEMF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstEMF.Name = "lblInstEMF";
            this.lblInstEMF.Size = new System.Drawing.Size(96, 20);
            this.lblInstEMF.TabIndex = 5;
            this.lblInstEMF.Text = "Installed MF";
            this.lblInstEMF.TranslationKey = "";
            // 
            // lblInstallationDate
            // 
            this.lblInstallationDate.AutoSize = true;
            this.lblInstallationDate.Location = new System.Drawing.Point(28, 257);
            this.lblInstallationDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstallationDate.Name = "lblInstallationDate";
            this.lblInstallationDate.Size = new System.Drawing.Size(125, 20);
            this.lblInstallationDate.TabIndex = 4;
            this.lblInstallationDate.Text = "Installation Date";
            this.lblInstallationDate.TranslationKey = "L000026";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(28, 215);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(70, 20);
            this.lblLocation.TabIndex = 3;
            this.lblLocation.Text = "Location";
            this.lblLocation.TranslationKey = "L000025";
            // 
            // lblMeterModel
            // 
            this.lblMeterModel.AutoSize = true;
            this.lblMeterModel.Location = new System.Drawing.Point(28, 174);
            this.lblMeterModel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeterModel.Name = "lblMeterModel";
            this.lblMeterModel.Size = new System.Drawing.Size(97, 20);
            this.lblMeterModel.TabIndex = 2;
            this.lblMeterModel.Text = "Meter Model";
            this.lblMeterModel.TranslationKey = "L000024";
            // 
            // lblMeterType
            // 
            this.lblMeterType.AutoSize = true;
            this.lblMeterType.Location = new System.Drawing.Point(28, 132);
            this.lblMeterType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeterType.Name = "lblMeterType";
            this.lblMeterType.Size = new System.Drawing.Size(88, 20);
            this.lblMeterType.TabIndex = 1;
            this.lblMeterType.Text = "Meter Type";
            this.lblMeterType.TranslationKey = "L000023";
            // 
            // lblMeterNumber
            // 
            this.lblMeterNumber.AutoSize = true;
            this.lblMeterNumber.Location = new System.Drawing.Point(28, 91);
            this.lblMeterNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMeterNumber.Name = "lblMeterNumber";
            this.lblMeterNumber.Size = new System.Drawing.Size(110, 20);
            this.lblMeterNumber.TabIndex = 0;
            this.lblMeterNumber.Text = "Meter Number";
            this.lblMeterNumber.TranslationKey = "L000022";
            // 
            // CMD_lblInstalledCTPT
            // 
            this.CMD_lblInstalledCTPT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.CMD_lblInstalledCTPT.AutoSize = true;
            this.CMD_lblInstalledCTPT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CMD_lblInstalledCTPT.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.CMD_lblInstalledCTPT.Location = new System.Drawing.Point(736, 28);
            this.CMD_lblInstalledCTPT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CMD_lblInstalledCTPT.Name = "CMD_lblInstalledCTPT";
            this.CMD_lblInstalledCTPT.Size = new System.Drawing.Size(0, 20);
            this.CMD_lblInstalledCTPT.TabIndex = 14;
            // 
            // chkShowMeterDeactive
            // 
            this.chkShowMeterDeactive.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkShowMeterDeactive.AutoSize = true;
            this.chkShowMeterDeactive.Location = new System.Drawing.Point(33, 49);
            this.chkShowMeterDeactive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkShowMeterDeactive.Name = "chkShowMeterDeactive";
            this.chkShowMeterDeactive.Size = new System.Drawing.Size(219, 24);
            this.chkShowMeterDeactive.TabIndex = 13;
            this.chkShowMeterDeactive.Text = "Show Inactive meters only";
            this.chkShowMeterDeactive.UseVisualStyleBackColor = true;
            this.chkShowMeterDeactive.CheckedChanged += new System.EventHandler(this.chkShowMeterDeactive_CheckedChanged);
            // 
            // CMD_lblMeterCTPT
            // 
            this.CMD_lblMeterCTPT.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CMD_lblMeterCTPT.AutoSize = true;
            this.CMD_lblMeterCTPT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CMD_lblMeterCTPT.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.CMD_lblMeterCTPT.Location = new System.Drawing.Point(375, 42);
            this.CMD_lblMeterCTPT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CMD_lblMeterCTPT.Name = "CMD_lblMeterCTPT";
            this.CMD_lblMeterCTPT.Size = new System.Drawing.Size(0, 20);
            this.CMD_lblMeterCTPT.TabIndex = 7;
            // 
            // txtInstalledCTPrimary
            // 
            this.txtInstalledCTPrimary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledCTPrimary.Location = new System.Drawing.Point(906, 38);
            this.txtInstalledCTPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledCTPrimary.MaxLength = 3;
            this.txtInstalledCTPrimary.Name = "txtInstalledCTPrimary";
            this.txtInstalledCTPrimary.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledCTPrimary.TabIndex = 22;
            this.txtInstalledCTPrimary.TextChanged += new System.EventHandler(this.txtInstalledCTPrimary_TextChanged);
            // 
            // txtInstalledCTSecondary
            // 
            this.txtInstalledCTSecondary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledCTSecondary.Location = new System.Drawing.Point(906, 82);
            this.txtInstalledCTSecondary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledCTSecondary.MaxLength = 3;
            this.txtInstalledCTSecondary.Name = "txtInstalledCTSecondary";
            this.txtInstalledCTSecondary.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledCTSecondary.TabIndex = 23;
            this.txtInstalledCTSecondary.TextChanged += new System.EventHandler(this.txtInstalledCTSecondary_TextChanged);
            // 
            // txtInstalledCTRatio
            // 
            this.txtInstalledCTRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledCTRatio.Cursor = System.Windows.Forms.Cursors.No;
            this.txtInstalledCTRatio.Enabled = false;
            this.txtInstalledCTRatio.Location = new System.Drawing.Point(906, 126);
            this.txtInstalledCTRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledCTRatio.MaxLength = 3;
            this.txtInstalledCTRatio.Name = "txtInstalledCTRatio";
            this.txtInstalledCTRatio.ReadOnly = true;
            this.txtInstalledCTRatio.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledCTRatio.TabIndex = 17;
            this.txtInstalledCTRatio.TextChanged += new System.EventHandler(this.txtInstalledCTRatio_TextChanged);
            // 
            // txtInstalledPTPrimary
            // 
            this.txtInstalledPTPrimary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledPTPrimary.Location = new System.Drawing.Point(906, 171);
            this.txtInstalledPTPrimary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledPTPrimary.MaxLength = 3;
            this.txtInstalledPTPrimary.Name = "txtInstalledPTPrimary";
            this.txtInstalledPTPrimary.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledPTPrimary.TabIndex = 24;
            this.txtInstalledPTPrimary.TextChanged += new System.EventHandler(this.txtInstalledPTPrimary_TextChanged);
            // 
            // txtInstalledPTSecondary
            // 
            this.txtInstalledPTSecondary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledPTSecondary.Location = new System.Drawing.Point(906, 215);
            this.txtInstalledPTSecondary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledPTSecondary.MaxLength = 3;
            this.txtInstalledPTSecondary.Name = "txtInstalledPTSecondary";
            this.txtInstalledPTSecondary.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledPTSecondary.TabIndex = 25;
            this.txtInstalledPTSecondary.TextChanged += new System.EventHandler(this.txtInstalledPTSecondary_TextChanged);
            // 
            // txtInstalledPTRatio
            // 
            this.txtInstalledPTRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtInstalledPTRatio.Cursor = System.Windows.Forms.Cursors.No;
            this.txtInstalledPTRatio.Enabled = false;
            this.txtInstalledPTRatio.Location = new System.Drawing.Point(906, 260);
            this.txtInstalledPTRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstalledPTRatio.MaxLength = 3;
            this.txtInstalledPTRatio.Name = "txtInstalledPTRatio";
            this.txtInstalledPTRatio.ReadOnly = true;
            this.txtInstalledPTRatio.Size = new System.Drawing.Size(148, 26);
            this.txtInstalledPTRatio.TabIndex = 20;
            this.txtInstalledPTRatio.TextChanged += new System.EventHandler(this.txtInstalledPTRatio_TextChanged);
            // 
            // cboUnits
            // 
            this.cboUnits.AccessibleDescription = "";
            this.cboUnits.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnits.FormattingEnabled = true;
            this.cboUnits.Items.AddRange(new object[] {
            "kW",
            "kVA",
            "MW",
            "MVA"});
            this.cboUnits.Location = new System.Drawing.Point(236, 291);
            this.cboUnits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboUnits.Name = "cboUnits";
            this.cboUnits.Size = new System.Drawing.Size(124, 28);
            this.cboUnits.TabIndex = 20;
            this.cboUnits.Tag = "";
            // 
            // DtpInstalled
            // 
            this.DtpInstalled.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DtpInstalled.CustomFormat = "dd-MMM-yyyy";
            this.DtpInstalled.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpInstalled.Location = new System.Drawing.Point(170, 248);
            this.DtpInstalled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DtpInstalled.Name = "DtpInstalled";
            this.DtpInstalled.Size = new System.Drawing.Size(190, 26);
            this.DtpInstalled.TabIndex = 18;
            // 
            // cboMeterType
            // 
            this.cboMeterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMeterType.FormattingEnabled = true;
            this.cboMeterType.Items.AddRange(new object[] {
            "1P2W",
            "3P-3W",
            "3P-4W"});
            this.cboMeterType.Location = new System.Drawing.Point(170, 128);
            this.cboMeterType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboMeterType.Name = "cboMeterType";
            this.cboMeterType.Size = new System.Drawing.Size(190, 28);
            this.cboMeterType.TabIndex = 15;
            // 
            // cboMeterNumber
            // 
            this.cboMeterNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboMeterNumber.FormattingEnabled = true;
            this.cboMeterNumber.Location = new System.Drawing.Point(170, 86);
            this.cboMeterNumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboMeterNumber.Name = "cboMeterNumber";
            this.cboMeterNumber.Size = new System.Drawing.Size(190, 30);
            this.cboMeterNumber.TabIndex = 14;
            this.cboMeterNumber.SelectedIndexChanged += new System.EventHandler(this.cboMeterNumber_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtInternEMF);
            this.groupBox5.Controls.Add(this.lblInternEMF);
            this.groupBox5.Controls.Add(this.lblCTRatio);
            this.groupBox5.Controls.Add(this.lblPTRatio);
            this.groupBox5.Controls.Add(this.txtMeterCTRatio);
            this.groupBox5.Controls.Add(this.txtMeterPTRatio);
            this.groupBox5.Location = new System.Drawing.Point(396, 17);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox5.Size = new System.Drawing.Size(324, 157);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Internal CT/PT";
            // 
            // txtInternEMF
            // 
            this.txtInternEMF.BackColor = System.Drawing.SystemColors.Control;
            this.txtInternEMF.Enabled = false;
            this.txtInternEMF.Location = new System.Drawing.Point(166, 111);
            this.txtInternEMF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInternEMF.Name = "txtInternEMF";
            this.txtInternEMF.Size = new System.Drawing.Size(148, 26);
            this.txtInternEMF.TabIndex = 1;
            // 
            // lblInternEMF
            // 
            this.lblInternEMF.AutoSize = true;
            this.lblInternEMF.Location = new System.Drawing.Point(12, 112);
            this.lblInternEMF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInternEMF.Name = "lblInternEMF";
            this.lblInternEMF.Size = new System.Drawing.Size(90, 20);
            this.lblInternEMF.TabIndex = 0;
            this.lblInternEMF.Text = "Internal MF";
            // 
            // lblCTRatio
            // 
            this.lblCTRatio.AutoSize = true;
            this.lblCTRatio.Location = new System.Drawing.Point(12, 31);
            this.lblCTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTRatio.Name = "lblCTRatio";
            this.lblCTRatio.Size = new System.Drawing.Size(71, 20);
            this.lblCTRatio.TabIndex = 10;
            this.lblCTRatio.Text = "CT Ratio";
            this.lblCTRatio.TranslationKey = "L000032";
            // 
            // lblPTRatio
            // 
            this.lblPTRatio.AutoSize = true;
            this.lblPTRatio.Location = new System.Drawing.Point(12, 71);
            this.lblPTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPTRatio.Name = "lblPTRatio";
            this.lblPTRatio.Size = new System.Drawing.Size(70, 20);
            this.lblPTRatio.TabIndex = 13;
            this.lblPTRatio.Text = "PT Ratio";
            this.lblPTRatio.TranslationKey = "L000035";
            // 
            // txtMeterCTRatio
            // 
            this.txtMeterCTRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMeterCTRatio.Cursor = System.Windows.Forms.Cursors.No;
            this.txtMeterCTRatio.Enabled = false;
            this.txtMeterCTRatio.Location = new System.Drawing.Point(166, 23);
            this.txtMeterCTRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMeterCTRatio.Name = "txtMeterCTRatio";
            this.txtMeterCTRatio.ReadOnly = true;
            this.txtMeterCTRatio.Size = new System.Drawing.Size(148, 26);
            this.txtMeterCTRatio.TabIndex = 9;
            this.txtMeterCTRatio.TextChanged += new System.EventHandler(this.txtMeterCTRatio_TextChanged);
            // 
            // txtMeterPTRatio
            // 
            this.txtMeterPTRatio.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtMeterPTRatio.Cursor = System.Windows.Forms.Cursors.No;
            this.txtMeterPTRatio.Enabled = false;
            this.txtMeterPTRatio.Location = new System.Drawing.Point(166, 68);
            this.txtMeterPTRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMeterPTRatio.Name = "txtMeterPTRatio";
            this.txtMeterPTRatio.ReadOnly = true;
            this.txtMeterPTRatio.Size = new System.Drawing.Size(148, 26);
            this.txtMeterPTRatio.TabIndex = 12;
            this.txtMeterPTRatio.TextChanged += new System.EventHandler(this.txtMeterPTRatio_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(728, 17);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox6.Size = new System.Drawing.Size(344, 335);
            this.groupBox6.TabIndex = 32;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Installed CT/PT";
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(9, 18);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox7.Size = new System.Drawing.Size(378, 335);
            this.groupBox7.TabIndex = 33;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "General";
            // 
            // ConsumerMeterDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BackgroundImage = global::CABApplication.Properties.Resources.bakgroundmain;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1162, 874);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblindication);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnEnterNewMeterDetails);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "ConsumerMeterDetails";
            this.StatusMessage = "";
            this.Text = "Consumer Meter Details";
            this.Activated += new System.EventHandler(this.ConsumerMeterDetails_Activated);
            this.Load += new System.EventHandler(this.ConsumerMeterDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbAreaDetails.ResumeLayout(false);
            this.gbAreaDetails.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
		private CAB.UI.Controls.CABButton btnEnterNewMeterDetails;
		private System.Windows.Forms.GroupBox groupBox1;
		private CAB.UI.Controls.CABLabel lngLabel7;
		private CAB.UI.Controls.CABLabel lblCityTown;
		private CAB.UI.Controls.CABLabel lblStreet;
		private CAB.UI.Controls.CABLabel lblHouseNo;
		private CAB.UI.Controls.CABLabel lblTelephoneNumber;
		private CAB.UI.Controls.CABLabel lblConsumerType;
		private CAB.UI.Controls.CABLabel lblConsumerName;
		private CAB.UI.Controls.CABLabel lblConsumerID;
		private System.Windows.Forms.TextBox txtConsumerEmail;
		private System.Windows.Forms.TextBox txtConsumerCity;
		private System.Windows.Forms.TextBox txtConsumerStreet;
		private System.Windows.Forms.TextBox txtConsumerHouseNo;
		private System.Windows.Forms.TextBox txtConsumerNumber;
		private System.Windows.Forms.TextBox txtConsumerName;
		private System.Windows.Forms.ComboBox cboConsumerType;
		private System.Windows.Forms.CheckBox chkSuspectedConsumer;
        private System.Windows.Forms.TextBox txtConsumerTelephone;
		private CAB.UI.Controls.CABButton btnSave;
        private CAB.UI.Controls.CABButton btnCancel;
        private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private CAB.UI.Controls.CABLabel lblNote;
        private CAB.UI.Controls.CABLabel lblindication;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox gbAreaDetails;
        private System.Windows.Forms.ComboBox ddlCommunicationtype;
        private System.Windows.Forms.ComboBox ddlRegion;
        private CAB.UI.Controls.CABLabel lngCommunicationType;
        private System.Windows.Forms.ComboBox ddlCircle;
        private System.Windows.Forms.ComboBox ddlDivision;
        private CAB.UI.Controls.CABLabel lblDivision;
        private CAB.UI.Controls.CABLabel lblCircle;
        private CAB.UI.Controls.CABLabel lblRegion;
        private System.Windows.Forms.TextBox txtBoxMeterSIM;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox chkUseEMFInCalculation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox txtContractDemand;
        private System.Windows.Forms.ComboBox cboMeterModel;
        private CAB.UI.Controls.CABLabel lblInstalledPTRatio;
        private CAB.UI.Controls.CABLabel lblInstalledPTSecondary;
        private CAB.UI.Controls.CABLabel lblInstalledPTPrimary;
        private CAB.UI.Controls.CABLabel lblInstalledCTRatio;
        private CAB.UI.Controls.CABLabel lblInstalledCTSecondary;
        private CAB.UI.Controls.CABLabel lblInstalledCTPrimary;
        private CAB.UI.Controls.CABLabel lblPTRatio;
        private CAB.UI.Controls.CABLabel lblCTRatio;
        private CAB.UI.Controls.CABLabel lblContractDemand;
        private CAB.UI.Controls.CABLabel lblInstEMF;
        private CAB.UI.Controls.CABLabel lblInstallationDate;
        private CAB.UI.Controls.CABLabel lblLocation;
        private CAB.UI.Controls.CABLabel lblMeterModel;
        private CAB.UI.Controls.CABLabel lblMeterType;
        private CAB.UI.Controls.CABLabel lblMeterNumber;
        private System.Windows.Forms.Label CMD_lblInstalledCTPT;
        private System.Windows.Forms.TextBox txtMeterCTRatio;
        private System.Windows.Forms.CheckBox chkShowMeterDeactive;
        private System.Windows.Forms.TextBox txtMeterLocation;
        private System.Windows.Forms.Label CMD_lblMeterCTPT;
        private System.Windows.Forms.TextBox txtMeterPTRatio;
        private System.Windows.Forms.TextBox txtInstalledCTPrimary;
        private System.Windows.Forms.TextBox txtInstalledCTSecondary;
        private System.Windows.Forms.TextBox txtInstalledCTRatio;
        private System.Windows.Forms.TextBox txtInstalledPTPrimary;
        private System.Windows.Forms.TextBox txtInstalledPTSecondary;
        private System.Windows.Forms.TextBox txtInstalledPTRatio;
        private System.Windows.Forms.TextBox txtEMF;
        private System.Windows.Forms.ComboBox cboUnits;
        private System.Windows.Forms.DateTimePicker DtpInstalled;
        private System.Windows.Forms.ComboBox cboMeterType;
        private System.Windows.Forms.ComboBox cboMeterNumber;
        private System.Windows.Forms.CheckBox chkActivateMeter;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblExternalEMFValue;
        private System.Windows.Forms.Label lblExtrenalPTRValue;
        private System.Windows.Forms.Label lblExternalCTRValue;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblExternalCTR;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblInternEMF;
        private System.Windows.Forms.TextBox txtInternEMF;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox textBoxModemIMEI;
        private CAB.UI.Controls.CABLabel lnglabelIMEI;
        private CAB.UI.Controls.CABLabel lngLabel6;
    }
}

