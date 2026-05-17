namespace CAB.UI
{
    partial class SimSelectForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtBoxMeterSIM = new System.Windows.Forms.TextBox();
            this.lblNoData = new System.Windows.Forms.Label();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.groupBoxSelectSim = new System.Windows.Forms.GroupBox();
            this.dgvMeterIdAndSim = new System.Windows.Forms.DataGridView();
            this.groupBoxSelectSim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(280, 311);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtBoxMeterSIM
            // 
            this.txtBoxMeterSIM.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxMeterSIM.Location = new System.Drawing.Point(137, 315);
            this.txtBoxMeterSIM.MaxLength = 10;
            this.txtBoxMeterSIM.Name = "txtBoxMeterSIM";
            this.txtBoxMeterSIM.Size = new System.Drawing.Size(122, 20);
            this.txtBoxMeterSIM.TabIndex = 29;
            // 
            // lblNoData
            // 
            this.lblNoData.AutoSize = true;
            this.lblNoData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoData.ForeColor = System.Drawing.Color.Red;
            this.lblNoData.Location = new System.Drawing.Point(106, 107);
            this.lblNoData.Name = "lblNoData";
            this.lblNoData.Size = new System.Drawing.Size(116, 16);
            this.lblNoData.TabIndex = 30;
            this.lblNoData.Text = "No Data Found ";
            this.lblNoData.Visible = false;
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(19, 318);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(112, 13);
            this.lngLabel1.TabIndex = 6;
            this.lngLabel1.Text = "Meter SIM No.      +91";
            this.lngLabel1.TranslationKey = "";
            // 
            // groupBoxSelectSim
            // 
            this.groupBoxSelectSim.Controls.Add(this.dgvMeterIdAndSim);
            this.groupBoxSelectSim.Controls.Add(this.lblNoData);
            this.groupBoxSelectSim.Location = new System.Drawing.Point(3, 12);
            this.groupBoxSelectSim.Name = "groupBoxSelectSim";
            this.groupBoxSelectSim.Size = new System.Drawing.Size(354, 287);
            this.groupBoxSelectSim.TabIndex = 31;
            this.groupBoxSelectSim.TabStop = false;
            this.groupBoxSelectSim.Text = "Select SIM Number";
            // 
            // dgvMeterIdAndSim
            // 
            this.dgvMeterIdAndSim.AllowUserToAddRows = false;
            this.dgvMeterIdAndSim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeterIdAndSim.Location = new System.Drawing.Point(8, 19);
            this.dgvMeterIdAndSim.Name = "dgvMeterIdAndSim";
            this.dgvMeterIdAndSim.Size = new System.Drawing.Size(346, 262);
            this.dgvMeterIdAndSim.TabIndex = 1;
            this.dgvMeterIdAndSim.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeterIdAndSim_CellContentClick);
            // 
            // SimSelectForm
            // 
            this.ClientSize = new System.Drawing.Size(369, 369);
            this.Controls.Add(this.groupBoxSelectSim);
            this.Controls.Add(this.txtBoxMeterSIM);
            this.Controls.Add(this.lngLabel1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SimSelectForm";
            this.Text = "Select Sim Number";
            this.groupBoxSelectSim.ResumeLayout(false);
            this.groupBoxSelectSim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeterIdAndSim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.TextBox txtBoxMeterSIM;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.GroupBox groupBoxSelectSim;
        private System.Windows.Forms.DataGridView dgvMeterIdAndSim;

    }
}
