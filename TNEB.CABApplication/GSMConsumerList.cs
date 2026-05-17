using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.BLL;

namespace CAB.UI
{
    public partial class GSMConsumerList : Form
    {
        private GSMScheduleBLL sgmScheduleBLL = new GSMScheduleBLL();
        public string Data { get; set; }
        public GSMConsumerList()
        {
            InitializeComponent();
        }

        private void GSMConsumerList_Load(object sender, EventArgs e)
        {
            lngConsumer.Data = sgmScheduleBLL.GetCustomerMeterInformationList();
            lngConsumer.IsFullRow = true;
            lngConsumer.ValueColumn = "Meter Number";
            lngConsumer.SetEqualWidth();
            lngConsumer.RefreshGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Data = lngConsumer.GetPrimaryValue();
            this.Close();
        }
    }
}
