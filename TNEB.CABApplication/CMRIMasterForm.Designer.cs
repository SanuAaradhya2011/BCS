namespace CAB.UI
{
    partial class CMRIMasterForm
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
            this.ucGridControl = new CAB.UI.Controls.CABGridControl();
            this.ucDetail = new CAB.UI.CMRIMasterControl();
            this.SuspendLayout();
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.HideMainSearch = false;
            this.ucSearchControl.Location = new System.Drawing.Point(19, 16);
            this.ucSearchControl.Margin = new System.Windows.Forms.Padding(4);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimaryNonComboData = null;
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = false;
            this.ucSearchControl.SecondaryNonComboData = null;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(749, 32);
            this.ucSearchControl.TabIndex = 0;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // ucGridControl
            // 
            this.ucGridControl.AutoScroll = true;
            this.ucGridControl.Data = null;
            this.ucGridControl.HiddenColumn = null;
            this.ucGridControl.HiddenColumn2 = null;
            this.ucGridControl.HiddenColumn3 = null;
            this.ucGridControl.HiddenColumn4 = null;
            this.ucGridControl.HiddenColumns = null;
            this.ucGridControl.IsEqual = true;
            this.ucGridControl.IsFullRow = false;
            this.ucGridControl.IsSorting = false;
            this.ucGridControl.Location = new System.Drawing.Point(23, 58);
            this.ucGridControl.Margin = new System.Windows.Forms.Padding(4);
            this.ucGridControl.Name = "ucGridControl";
            this.ucGridControl.SelectedIndex = 0;
            this.ucGridControl.Size = new System.Drawing.Size(532, 248);
            this.ucGridControl.TabIndex = 1;
            this.ucGridControl.ValueColumn = null;
            this.ucGridControl.ValueColumn2 = null;
            // 
            // ucDetail
            // 
            this.ucDetail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDetail.Location = new System.Drawing.Point(23, 58);
            this.ucDetail.Margin = new System.Windows.Forms.Padding(4);
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.Size = new System.Drawing.Size(421, 248);
            this.ucDetail.StatusMessage = "";
            this.ucDetail.TabIndex = 2;
            this.ucDetail.Load += new System.EventHandler(this.ucDetail_Load);
            this.ucDetail.OnCancelClick += new CAB.UI.CMRIMasterControl.CancelClickHandler(this.ucDetail_OnCancelClick);
            this.ucDetail.OnControlStatusChanged += new CAB.UI.CMRIMasterControl.ControlStatusChanged(this.ucDetail_OnControlStatusChanged);
            this.ucDetail.OnSaveClick += new CAB.UI.CMRIMasterControl.SaveClickHandler(this.ucDetail_OnSaveClick);
            // 
            // CMRIMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(776, 482);
            this.Controls.Add(this.ucSearchControl);
            this.Controls.Add(this.ucDetail);
            this.Controls.Add(this.ucGridControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CMRIMasterForm";
            this.StatusMessage = "";
            this.Text = "CMRI Definition";
            this.Load += new System.EventHandler(this.CMRIMasterForm_Load);
            this.Activated += new System.EventHandler(this.CMRIMasterForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMRIMasterForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABSearchControl ucSearchControl;
        private CAB.UI.Controls.CABGridControl ucGridControl;
        private CMRIMasterControl ucDetail;
 
    }
}