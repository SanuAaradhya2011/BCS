using CAB.UI.Controls;
namespace CAB.UI
{
    partial class UserInformations
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
            this.ucSearchControl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucSearchControl.HideMainSearch = true;
            this.ucSearchControl.Location = new System.Drawing.Point(19, 16);
            this.ucSearchControl.Name = "ucSearchControl";
            this.ucSearchControl.PrimarySearchTypeComboData = "";
            this.ucSearchControl.PrimarySearchTypeData = null;
            this.ucSearchControl.SearchRequire = true;
            this.ucSearchControl.SecondarySearchTypeComboData = "";
            this.ucSearchControl.SecondarySearchTypeFromDateData = ((long)(20100323000000));
            this.ucSearchControl.SecondarySearchTypeTextData = "";
            this.ucSearchControl.SecondarySearchTypeToDateData = ((long)(20100323000000));
            this.ucSearchControl.Size = new System.Drawing.Size(790, 32);
            this.ucSearchControl.TabIndex = 2;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnFindNowClick += new CAB.UI.Controls.CABSearchControl.FindNowClickHandler(this.ucSearchControl_OnFindNowClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // ucGridControl
            // 
            this.ucGridControl.Data = null;
            this.ucGridControl.HiddenColumn = null;
            this.ucGridControl.HiddenColumns = null;
            this.ucGridControl.IsEqual = false;
            this.ucGridControl.IsSorting = true;
            this.ucGridControl.Location = new System.Drawing.Point(23, 58);
            this.ucGridControl.Name = "ucGridControl";
            this.ucGridControl.SelectedIndex = 0;
            this.ucGridControl.Size = new System.Drawing.Size(781, 439);
            this.ucGridControl.TabIndex = 4;
            this.ucGridControl.ValueColumn = "UserInformation_ID";
            // 
            // UserInformations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(815, 502);
            this.Controls.Add(this.ucSearchControl);
            this.Controls.Add(this.ucGridControl);
            this.Name = "UserInformations";
            this.Text = "UserInformations";
            this.Load += new System.EventHandler(this.UserInformations_Load);
            this.Activated += new System.EventHandler(this.UserInformations_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private  CABSearchControl ucSearchControl;
        //private UserInformation ucDetail;
        private CABGridControl ucGridControl;
    }
}