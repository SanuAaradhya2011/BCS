namespace CAB.UI
{
	partial class GroupDefinition
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
            this.tvGroup = new System.Windows.Forms.TreeView();
            this.lngGCGroupDefinition = new CAB.UI.Controls.CABGridControl();
            this.SuspendLayout();
            // 
            // ucSearchControl
            // 
            this.ucSearchControl.HideMainSearch = false;
            this.ucSearchControl.Location = new System.Drawing.Point(12, 7);
            this.ucSearchControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.ucSearchControl.Size = new System.Drawing.Size(765, 32);
            this.ucSearchControl.TabIndex = 7;
            this.ucSearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.ucSearchControl_OnAddClick);
            this.ucSearchControl.OnEditClick += new CAB.UI.Controls.CABSearchControl.EditClickHandler(this.ucSearchControl_OnEditClick);
            this.ucSearchControl.OnDeleteClick += new CAB.UI.Controls.CABSearchControl.DeleteClickHandler(this.ucSearchControl_OnDeleteClick);
            // 
            // tvGroup
            // 
            this.tvGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvGroup.Location = new System.Drawing.Point(12, 45);
            this.tvGroup.Name = "tvGroup";
            this.tvGroup.Size = new System.Drawing.Size(165, 382);
            this.tvGroup.TabIndex = 8;
            this.tvGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroup_AfterSelect);
            // 
            // lngGCGroupDefinition
            // 
            this.lngGCGroupDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lngGCGroupDefinition.AutoScroll = true;
            this.lngGCGroupDefinition.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lngGCGroupDefinition.Data = null;
            this.lngGCGroupDefinition.HiddenColumn = null;
            this.lngGCGroupDefinition.HiddenColumns = null;
            this.lngGCGroupDefinition.IsEqual = false;
            this.lngGCGroupDefinition.IsFullRow = false;
            this.lngGCGroupDefinition.IsSorting = true;
            this.lngGCGroupDefinition.Location = new System.Drawing.Point(184, 46);
            this.lngGCGroupDefinition.Margin = new System.Windows.Forms.Padding(4);
            this.lngGCGroupDefinition.Name = "lngGCGroupDefinition";
            this.lngGCGroupDefinition.SelectedIndex = 0;
            this.lngGCGroupDefinition.SelectedRowId = "";
            this.lngGCGroupDefinition.Size = new System.Drawing.Size(592, 381);
            this.lngGCGroupDefinition.TabIndex = 9;
            this.lngGCGroupDefinition.ValueColumn = null;
            this.lngGCGroupDefinition.Load += new System.EventHandler(this.lngGCGroupDefinition_Load);
            // 
            // GroupDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(789, 439);
            this.Controls.Add(this.lngGCGroupDefinition);
            this.Controls.Add(this.tvGroup);
            this.Controls.Add(this.ucSearchControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "GroupDefinition";
            this.StatusMessage = "";
            this.Text = "Group Definition";
            this.Activated += new System.EventHandler(this.GroupDefinition_Activated);
            this.Load += new System.EventHandler(this.GroupDefinition_Load);
            this.ResumeLayout(false);

		}

		#endregion

        private CAB.UI.Controls.CABSearchControl ucSearchControl;
		private System.Windows.Forms.TreeView tvGroup;
        private CAB.UI.Controls.CABGridControl lngGCGroupDefinition;
	}
}