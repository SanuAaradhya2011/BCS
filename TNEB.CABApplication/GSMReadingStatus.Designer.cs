namespace CAB.UI
{
    partial class GSMReadingStatus
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
            this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdGSMReadingStatus = new CAB.UI.Controls.CABGridControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.HideMainSearch = true;
            this.ucSearchControl.Location = new System.Drawing.Point(-14, 1);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = true;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(751, 32);
            this.ucSearchControl.TabIndex = 3;
            this.ucSearchControl.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.ucSearchControl_OnFindNowClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            this.ucSearchControl.OnSearchClick += new CAB.UI.Controls.CABSearchControl.SearchClickHandler(this.ucSearchControl_OnSearchClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdGSMReadingStatus);
            this.panel1.Location = new System.Drawing.Point(6, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(734, 344);
            this.panel1.TabIndex = 4;
            // 
            // grdGSMReadingStatus
            // 
            this.grdGSMReadingStatus.Data = null;
            this.grdGSMReadingStatus.HiddenColumn = null;
            this.grdGSMReadingStatus.HiddenColumns = null;
            this.grdGSMReadingStatus.IsEqual = false;
            this.grdGSMReadingStatus.IsFullRow = false;
            this.grdGSMReadingStatus.IsSorting = false;
            this.grdGSMReadingStatus.Location = new System.Drawing.Point(16, 8);
            this.grdGSMReadingStatus.Name = "grdGSMReadingStatus";
            this.grdGSMReadingStatus.SelectedIndex = 0;
            this.grdGSMReadingStatus.Size = new System.Drawing.Size(702, 330);
            this.grdGSMReadingStatus.TabIndex = 1;
            this.grdGSMReadingStatus.ValueColumn = null;
            // 
            // GSMReadingStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 385);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ucSearchControl);
            this.Name = "GSMReadingStatus";
            this.StatusMessage = "";
            this.Text = "GSM Reading Status";
            this.Load += new System.EventHandler(this.GSMReadingStatus_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABSearchControl ucSearchControl;
        private System.Windows.Forms.Panel panel1;
        private CAB.UI.Controls.CABGridControl grdGSMReadingStatus;
    }
}