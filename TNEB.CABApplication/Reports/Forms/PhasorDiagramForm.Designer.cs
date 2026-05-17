namespace CAB.UI
{
    partial class PhasorDiagramForm
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
            this.lngPhasorData = new CAB.UI.Controls.CABGridControl();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btnPrint = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblPhasorNotShown = new System.Windows.Forms.Label();
            this.lngPhasorDiagram = new CAB.UI.Controls.PhasorDiagram();
            this.SuspendLayout();
            // 
            // lngPhasorData
            // 
            this.lngPhasorData.AutoScroll = true;
            this.lngPhasorData.BackColor = System.Drawing.Color.Transparent;
            this.lngPhasorData.Data = null;
            this.lngPhasorData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngPhasorData.HiddenColumn = null;
            this.lngPhasorData.HiddenColumn2 = null;
            this.lngPhasorData.HiddenColumn3 = null;
            this.lngPhasorData.HiddenColumn4 = null;
            this.lngPhasorData.HiddenColumns = null;
            this.lngPhasorData.IsEqual = false;
            this.lngPhasorData.IsFullRow = false;
            this.lngPhasorData.IsSorting = false;
            this.lngPhasorData.Location = new System.Drawing.Point(532, 2);
            this.lngPhasorData.Margin = new System.Windows.Forms.Padding(4);
            this.lngPhasorData.Name = "lngPhasorData";
            this.lngPhasorData.SelectedIndex = 0;
            this.lngPhasorData.Size = new System.Drawing.Size(286, 391);
            this.lngPhasorData.TabIndex = 5;
            this.lngPhasorData.ValueColumn = null;
            this.lngPhasorData.ValueColumn2 = null;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(286, 342);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblPhasorNotShown
            // 
            this.lblPhasorNotShown.AutoSize = true;
            this.lblPhasorNotShown.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhasorNotShown.Location = new System.Drawing.Point(45, 64);
            this.lblPhasorNotShown.Name = "lblPhasorNotShown";
            this.lblPhasorNotShown.Size = new System.Drawing.Size(0, 18);
            this.lblPhasorNotShown.TabIndex = 10;
            // 
            // lngPhasorDiagram
            // 
            this.lngPhasorDiagram.BackColor = System.Drawing.Color.Transparent;
            this.lngPhasorDiagram.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lngPhasorDiagram.Location = new System.Drawing.Point(15, 15);
            this.lngPhasorDiagram.Name = "lngPhasorDiagram";
            this.lngPhasorDiagram.PhasorData = null;
            this.lngPhasorDiagram.PhasorDataset = null;
            this.lngPhasorDiagram.Size = new System.Drawing.Size(525, 400);
            this.lngPhasorDiagram.TabIndex = 0;
            this.lngPhasorDiagram.Load += new System.EventHandler(this.lngPhasorDiagram_Load);
            // 
            // PhasorDiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(825, 412);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.lngPhasorData);
            this.Controls.Add(this.lngPhasorDiagram);
            this.Controls.Add(this.lblPhasorNotShown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PhasorDiagramForm";
            this.Opacity = 0;
            this.Text = "Phasor Report";
            this.Shown += new System.EventHandler(this.PhasorDiagramForm_Shown);
            this.Load += new System.EventHandler(this.PhasorDiagramForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.CABGridControl lngPhasorData;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblPhasorNotShown;
        private CAB.UI.Controls.PhasorDiagram lngPhasorDiagram;

    }
}