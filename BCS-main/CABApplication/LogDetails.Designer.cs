namespace CAB.UI
{
    partial class LogDetails
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdActivityLogDetails = new CAB.UI.Controls.CABGridControl();
            this.lngscApplicationLogDetails = new CAB.UI.Controls.CABSearchControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.grdActivityLogDetails);
            this.panel1.Controls.Add(this.lngscApplicationLogDetails);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1249, 471);
            this.panel1.TabIndex = 0;
            // 
            // grdActivityLogDetails
            // 
            this.grdActivityLogDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdActivityLogDetails.AutoScroll = true;
            this.grdActivityLogDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grdActivityLogDetails.Data = null;
            this.grdActivityLogDetails.HiddenColumn = null;
            this.grdActivityLogDetails.HiddenColumns = null;
            this.grdActivityLogDetails.IsEqual = false;
            this.grdActivityLogDetails.IsFullRow = false;
            this.grdActivityLogDetails.IsSorting = true;
            this.grdActivityLogDetails.Location = new System.Drawing.Point(4, 60);
            this.grdActivityLogDetails.Margin = new System.Windows.Forms.Padding(4);
            this.grdActivityLogDetails.Name = "grdActivityLogDetails";
            this.grdActivityLogDetails.SelectedIndex = 0;
            this.grdActivityLogDetails.SelectedRowId = "";
            this.grdActivityLogDetails.Size = new System.Drawing.Size(944, 324);
            this.grdActivityLogDetails.TabIndex = 10;
            this.grdActivityLogDetails.ValueColumn = null;
            // 
            // lngscApplicationLogDetails
            // 
            this.lngscApplicationLogDetails.HideMainSearch = true;
            this.lngscApplicationLogDetails.Location = new System.Drawing.Point(3, 3);
            this.lngscApplicationLogDetails.Margin = new System.Windows.Forms.Padding(4);
            this.lngscApplicationLogDetails.Name = "lngscApplicationLogDetails";
            this.lngscApplicationLogDetails.PrimaryNonComboData = null;
            this.lngscApplicationLogDetails.PrimarySearchTypeComboData = "";
            this.lngscApplicationLogDetails.PrimarySearchTypeData = null;
            this.lngscApplicationLogDetails.SearchRequire = true;
            this.lngscApplicationLogDetails.SecondaryNonComboData = null;
            this.lngscApplicationLogDetails.SecondarySearchTypeComboData = "";
            this.lngscApplicationLogDetails.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.lngscApplicationLogDetails.SecondarySearchTypeTextData = "";
            this.lngscApplicationLogDetails.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.lngscApplicationLogDetails.Size = new System.Drawing.Size(1233, 49);
            this.lngscApplicationLogDetails.TabIndex = 4;
            this.lngscApplicationLogDetails.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.lngscApplicationLogDetails_OnFindNowClick);
            // 
            // LogDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1249, 471);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LogDetails";
            this.StatusMessage = "";
            this.Text = "Application Log Details";
            this.Activated += new System.EventHandler(this.ApplicationLogDetails_Activated);
            this.Load += new System.EventHandler(this.ApplicationLogDetails_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CAB.UI.Controls.CABSearchControl lngscApplicationLogDetails;
        private CAB.UI.Controls.CABGridControl grdActivityLogDetails;


    }
}