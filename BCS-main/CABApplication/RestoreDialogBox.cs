using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CAB.UI
{
    public enum RestoreDialogBoxResult { Yes, YesToAll, No, NoToAll, Cancel }

    public partial class RestoreDialogBox : Form
    {
        // Internal values
        RestoreDialogBoxResult lastResult = RestoreDialogBoxResult.Cancel;
        RestoreDialogBoxResult result = RestoreDialogBoxResult.Cancel;

        // Enums
        // Results

        /// <summary>
        /// The default constructor for RestoreDialogBox.
        /// </summary>
        public RestoreDialogBox()
        {
            InitializeComponent();
        }

        #region Properties
        public String LabelText
        {
            get { return this.labelBody.Text; }
            set
            {
                this.labelBody.Text = value;
            }
        }

        public RestoreDialogBoxResult Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Call this function instead of ShowDialog, to check for remembered
        /// result.
        /// </summary>
        /// <returns></returns>
        public RestoreDialogBoxResult ShowMemoryDialog()
        {
            result = RestoreDialogBoxResult.Cancel;
            if (lastResult == RestoreDialogBoxResult.NoToAll)
            {
                result = RestoreDialogBoxResult.No;
            }
            else if (lastResult == RestoreDialogBoxResult.YesToAll)
            {
                result = RestoreDialogBoxResult.Yes;
            }
            else
            {
                base.ShowDialog();
            }
            return result;
        }

        public RestoreDialogBoxResult ShowMemoryDialog(String label, string title)
        {
            this.Text = title;
            LabelText = label;
            return ShowMemoryDialog();
        }
        #endregion


        private void buttonYes_Click(object sender, EventArgs e)
        {
            result = RestoreDialogBoxResult.Yes;
            lastResult = RestoreDialogBoxResult.Yes;
            DialogResult = DialogResult.Yes;
        }

        private void buttonYestoAll_Click(object sender, EventArgs e)
        {
            result = RestoreDialogBoxResult.Yes;
            lastResult = RestoreDialogBoxResult.YesToAll;
            DialogResult = DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            result = RestoreDialogBoxResult.No;
            lastResult = RestoreDialogBoxResult.No;
            DialogResult = DialogResult.No;
        }

        private void buttonNotoAll_Click(object sender, EventArgs e)
        {
            result = RestoreDialogBoxResult.No;
            lastResult = RestoreDialogBoxResult.NoToAll;
            DialogResult = DialogResult.No;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            result = RestoreDialogBoxResult.Cancel;
            lastResult = RestoreDialogBoxResult.Cancel;
            DialogResult = DialogResult.Cancel;
        }
    }
}