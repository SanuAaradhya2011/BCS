namespace CAB.UI 
{
    partial class TestForm
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
            this.SearchControl = new CAB.UI.Controls.CABSearchControl();
            this.SuspendLayout();
            // 
            // SearchControl
            // 
            this.SearchControl.Location = new System.Drawing.Point(12, 12);
            this.SearchControl.Name = "SearchControl";
            this.SearchControl.SearchType = null;
            this.SearchControl.Size = new System.Drawing.Size(749, 32);
            this.SearchControl.TabIndex = 0;
            this.SearchControl.OnAddClick += new CAB.UI.Controls.CABSearchControl.AddClickHandler(this.SearchControl_OnAddClick);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 314);
            this.Controls.Add(this.SearchControl);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABSearchControl SearchControl;
 
    }
}