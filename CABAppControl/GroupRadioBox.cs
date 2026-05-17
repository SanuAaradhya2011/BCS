using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CABAppControl
{
    public partial class GroupRadioBox : UserControl
    {
        private List<string> MeterID;
        private RadioButton radioButton;
        private static RadioButton _radioButtonChecked;
        public RadioButton radioButtonChecked
        {
            get
            {
                return _radioButtonChecked;
            }
            private set
            {
                _radioButtonChecked = value;
            }
        }
        public int SelectedRadioButtonCount { set; get; }

        public GroupRadioBox()
        {
            InitializeComponent();
            radioGroupBox.Paint +=new PaintEventHandler(groupBox1_Paint);


}

void  groupBox1_Paint(object sender, PaintEventArgs e)
{
    //ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
}
        


        public void AddRadioButtons(int MeterIdCount, List<string> MeterID)
        {
            for (int idCounter = 0; idCounter < MeterIdCount; idCounter++)
            {
                radioButton = new RadioButton();
                radioButton.Name = MeterID[idCounter];
                radioButton.Text = MeterID[idCounter];
                radioButton.Location = new Point(5, 15 + 22 * idCounter);
                radioGroupBox.Controls.Add(radioButton);
            }
        }


        public void SetRadioButtonChecked(int index, bool check)
        {
            RadioButton radioBtn = (RadioButton)radioGroupBox.Controls[index];
            radioBtn.Checked = check;
        }

       

        
    }
}
