namespace CAB.UI
{
    partial class RegisterProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterProduct));
            this.GrpCompanyDetail = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnproclose = new System.Windows.Forms.Button();
            this.btnprofinish = new System.Windows.Forms.Button();
            this.btnproback = new System.Windows.Forms.Button();
            this.txt_ProductCode = new System.Windows.Forms.TextBox();
            this.txtcompantname = new System.Windows.Forms.TextBox();
            this.GrpCompanyDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrpCompanyDetail
            // 
            this.GrpCompanyDetail.Controls.Add(this.label3);
            this.GrpCompanyDetail.Controls.Add(this.label2);
            this.GrpCompanyDetail.Controls.Add(this.label1);
            this.GrpCompanyDetail.Controls.Add(this.btnproclose);
            this.GrpCompanyDetail.Controls.Add(this.btnprofinish);
            this.GrpCompanyDetail.Controls.Add(this.btnproback);
            this.GrpCompanyDetail.Controls.Add(this.txt_ProductCode);
            this.GrpCompanyDetail.Controls.Add(this.txtcompantname);
            this.GrpCompanyDetail.Location = new System.Drawing.Point(12, 12);
            this.GrpCompanyDetail.Name = "GrpCompanyDetail";
            this.GrpCompanyDetail.Size = new System.Drawing.Size(362, 183);
            this.GrpCompanyDetail.TabIndex = 12;
            this.GrpCompanyDetail.TabStop = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(321, 32);
            this.label3.TabIndex = 18;
            this.label3.Text = "Enter the Registration Name and product Key below.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Product Key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Name";
            // 
            // btnproclose
            // 
            this.btnproclose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnproclose.Location = new System.Drawing.Point(292, 134);
            this.btnproclose.Name = "btnproclose";
            this.btnproclose.Size = new System.Drawing.Size(64, 32);
            this.btnproclose.TabIndex = 14;
            this.btnproclose.Text = "Cancel";
            this.btnproclose.UseVisualStyleBackColor = true;
            this.btnproclose.Click += new System.EventHandler(this.btnproclose_Click);
            // 
            // btnprofinish
            // 
            this.btnprofinish.Location = new System.Drawing.Point(160, 134);
            this.btnprofinish.Name = "btnprofinish";
            this.btnprofinish.Size = new System.Drawing.Size(64, 32);
            this.btnprofinish.TabIndex = 13;
            this.btnprofinish.Text = "Buy Now !";
            this.btnprofinish.UseVisualStyleBackColor = true;
            this.btnprofinish.Click += new System.EventHandler(this.btnprofinish_Click);
            // 
            // btnproback
            // 
            this.btnproback.Location = new System.Drawing.Point(226, 134);
            this.btnproback.Name = "btnproback";
            this.btnproback.Size = new System.Drawing.Size(64, 32);
            this.btnproback.TabIndex = 15;
            this.btnproback.Text = "OK";
            this.btnproback.UseVisualStyleBackColor = true;
            this.btnproback.Click += new System.EventHandler(this.btnproback_Click);
            // 
            // txt_ProductCode
            // 
            this.txt_ProductCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_ProductCode.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ProductCode.Location = new System.Drawing.Point(79, 103);
            this.txt_ProductCode.MaxLength = 50;
            this.txt_ProductCode.Name = "txt_ProductCode";
            this.txt_ProductCode.Size = new System.Drawing.Size(277, 21);
            this.txt_ProductCode.TabIndex = 2;
            this.txt_ProductCode.TextChanged += new System.EventHandler(this.txt_ProductCode_TextChanged);
            // 
            // txtcompantname
            // 
            this.txtcompantname.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcompantname.Location = new System.Drawing.Point(79, 66);
            this.txtcompantname.MaxLength = 255;
            this.txtcompantname.Name = "txtcompantname";
            this.txtcompantname.Size = new System.Drawing.Size(277, 21);
            this.txtcompantname.TabIndex = 1;
            this.txtcompantname.TextChanged += new System.EventHandler(this.txtcompantname_TextChanged);
            // 
            // RegisterProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(383, 203);
            this.Controls.Add(this.GrpCompanyDetail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterProduct";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register Product";
            this.Load += new System.EventHandler(this.RegisterProduct_Load);
            this.GrpCompanyDetail.ResumeLayout(false);
            this.GrpCompanyDetail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GrpCompanyDetail;
        private System.Windows.Forms.TextBox txtcompantname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnproclose;
        private System.Windows.Forms.Button btnprofinish;
        private System.Windows.Forms.Button btnproback;
        private System.Windows.Forms.TextBox txt_ProductCode;
        private System.Windows.Forms.Label label3;

    }
}