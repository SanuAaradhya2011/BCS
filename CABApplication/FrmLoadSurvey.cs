using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;

namespace CAB.UI
{
	public partial class FrmLoadSurvey : MdiChildForm
	{
		public FrmLoadSurvey()
		{
			InitializeComponent();
		}

		private void FrmLoadSurvey_Load(object sender, EventArgs e)
		{
			graphFormControl1.Location = new Point(0, 0);
		}

		
	}
}
