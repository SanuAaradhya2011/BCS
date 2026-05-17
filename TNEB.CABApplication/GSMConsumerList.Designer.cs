namespace CAB.UI
{
    partial class GSMConsumerList
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
            this.lngConsumer = new CAB.UI.Controls.CABGridControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lngConsumer
            // 
            this.lngConsumer.AutoScroll = true;
            this.lngConsumer.Data = null;
            this.lngConsumer.HiddenColumn = null;
            this.lngConsumer.HiddenColumns = null;
            this.lngConsumer.IsEqual = true;
            this.lngConsumer.IsSorting = false;
            this.lngConsumer.Location = new System.Drawing.Point(13, 13);
            this.lngConsumer.Margin = new System.Windows.Forms.Padding(4);
            this.lngConsumer.Name = "lngConsumer";
            this.lngConsumer.SelectedIndex = 0;
            this.lngConsumer.Size = new System.Drawing.Size(837, 391);
            this.lngConsumer.TabIndex = 0;
            this.lngConsumer.ValueColumn = null;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(726, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Pick && Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GSMConsumerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 451);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lngConsumer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GSMConsumerList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Consumer Meter List";
            this.Load += new System.EventHandler(this.GSMConsumerList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABGridControl lngConsumer;
        private System.Windows.Forms.Button button1;
    }
}