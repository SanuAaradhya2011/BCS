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
			this.lngGCGroupDefinition = new CAB.UI.Controls.CABGridControl();
			this.ucSearchControl = new CAB.UI.Controls.CABSearchControl();
			this.tvGroup = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// lngGCGroupDefinition
			// 
			this.lngGCGroupDefinition.Data = null;
			this.lngGCGroupDefinition.HiddenColumn = null;
			this.lngGCGroupDefinition.HiddenColumns = null;
			this.lngGCGroupDefinition.IsEqual = false;
			this.lngGCGroupDefinition.IsSorting = false;
			this.lngGCGroupDefinition.Location = new System.Drawing.Point(183, 45);
			this.lngGCGroupDefinition.Name = "lngGCGroupDefinition";
			this.lngGCGroupDefinition.SelectedIndex = 0;
			this.lngGCGroupDefinition.Size = new System.Drawing.Size(594, 382);
			this.lngGCGroupDefinition.TabIndex = 0;
			this.lngGCGroupDefinition.ValueColumn = null;
			// 
			// ucSearchControl
			// 
			this.ucSearchControl.HideMainSearch = false;
			this.ucSearchControl.Location = new System.Drawing.Point(12, 7);
			this.ucSearchControl.Name = "ucSearchControl";
			this.ucSearchControl.PrimarySearchTypeComboData = "";
			this.ucSearchControl.PrimarySearchTypeData = null;
			this.ucSearchControl.SearchRequire = false;
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
			this.tvGroup.Location = new System.Drawing.Point(12, 45);
			this.tvGroup.Name = "tvGroup";
			this.tvGroup.Size = new System.Drawing.Size(165, 382);
			this.tvGroup.TabIndex = 8;
			this.tvGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvGroup_AfterSelect);
			//this.tvGroup.Click += new System.EventHandler(this.tvGroup_Click);
			// 
			// GroupDefinition
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(789, 439);
			this.Controls.Add(this.tvGroup);
			this.Controls.Add(this.ucSearchControl);
			this.Controls.Add(this.lngGCGroupDefinition);
			this.Name = "GroupDefinition";
			this.Text = "Group Definition";
			this.Load += new System.EventHandler(this.GroupDefinition_Load);
			this.Activated += new System.EventHandler(this.GroupDefinition_Activated);
			this.ResumeLayout(false);

		}

		#endregion

		private CAB.UI.Controls.CABGridControl lngGCGroupDefinition;
		private CAB.UI.Controls.CABSearchControl ucSearchControl;
		private System.Windows.Forms.TreeView tvGroup;
	}
}