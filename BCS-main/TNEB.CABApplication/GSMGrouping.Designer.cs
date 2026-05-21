namespace CAB.UI
{
    partial class GSMGrouping
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
            this.SuspendLayout();
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.HideMainSearch = true;
            this.ucSearchControl.Location = new System.Drawing.Point(5, 6);
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
            this.ucSearchControl.Size = new System.Drawing.Size(749, 32);
            this.ucSearchControl.TabIndex = 4;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.ucSearchControl_OnFindNowClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            this.ucSearchControl.OnSearchClick += new CAB.UI.Controls.CABSearchControl.SearchClickHandler(this.ucSearchControl_OnSearchClick);
            // 
            // ucGridControl
            // 
            this.ucGridControl.AutoScroll = true;
            this.ucGridControl.Data = null;
            this.ucGridControl.HiddenColumn = null;
            this.ucGridControl.HiddenColumns = null;
            this.ucGridControl.IsEqual = false;
            this.ucGridControl.IsFullRow = false;
            this.ucGridControl.IsSorting = false;
            this.ucGridControl.Location = new System.Drawing.Point(6, 45);
            this.ucGridControl.Name = "ucGridControl";
            this.ucGridControl.SelectedIndex = 0;
            this.ucGridControl.Size = new System.Drawing.Size(782, 345);
            this.ucGridControl.TabIndex = 5;
            this.ucGridControl.ValueColumn = null;
            // 
            // GSMGrouping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(796, 399);
            this.Controls.Add(this.ucSearchControl);
            this.Controls.Add(this.ucGridControl);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "GSMGrouping";
            this.StatusMessage = "";
            this.Text = "GSMGrouping";
            this.Load += new System.EventHandler(this.GSMScheduling_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABSearchControl ucSearchControl;
        private CAB.UI.Controls.CABGridControl ucGridControl;


    }
}