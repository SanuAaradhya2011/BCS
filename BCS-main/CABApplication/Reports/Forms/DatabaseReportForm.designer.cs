namespace CAB.UI
{
    partial class DatabaseReportForm
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
            this.drptViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // drptViewer
            // 
            this.drptViewer.ActiveViewIndex = -1;
            this.drptViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.drptViewer.DisplayGroupTree = false;
            this.drptViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drptViewer.Location = new System.Drawing.Point(0, 0);
            this.drptViewer.Name = "drptViewer";
            this.drptViewer.SelectionFormula = "";
            this.drptViewer.ShowGroupTreeButton = false;
            this.drptViewer.Size = new System.Drawing.Size(1074, 674);
            this.drptViewer.TabIndex = 0;
            this.drptViewer.ViewTimeSelectionFormula = "";
            // 
            // DatabaseReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 674);
            this.Controls.Add(this.drptViewer);
            this.Name = "DatabaseReportForm";
            this.Text = "Report Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer drptViewer;

    }
}