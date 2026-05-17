using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.BLL;
using System.IO;

namespace CAB.UI
{
	public partial class ConsumerExportData : MdiChildForm
	{
		ExportImportBLL exportImportBLL; 	
		public ConsumerExportData()
		{
			InitializeComponent();
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			DataSet dSet = new DataSet();
			List<string> columnList = new List<string>();
			StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "ConsumerExport.CSV");
			dSet = exportImportBLL.GetConsumerDetails();

			if (dSet != null)
			{
				if (dSet.Tables[0].Rows.Count > 0)
				{
					WriteDataSetContents(dSet,writer);
				}
			}
			writer.Close();
			MessageBox.Show("Consumer Export Completed Successfully");
		}

		bool WriteDataSetContents(DataSet dataSet, StreamWriter file)
		{
			if (dataSet == null)
			{
				return false;
			}
			else if (dataSet.Tables[0].Rows.Count <= 0)
			{
				return false;
			}
			//Writing the column Contents
			for (int dcount = 0; dcount < dataSet.Tables[0].Columns.Count; dcount++)
			{
				file.Write(dataSet.Tables[0].Columns[dcount].ColumnName);
				if (dcount != dataSet.Tables[0].Columns.Count - 1)
				{
					file.Write(",");
				}
			}
			file.Write('\n');
			file.Write('\n');
			//Writing the values
			foreach (DataRow drow in dataSet.Tables[0].Rows)
			{
				for (int rowCount = 0; rowCount < dataSet.Tables[0].Columns.Count; rowCount++)
				{
					file.Write(drow[rowCount].ToString());
					if (rowCount != dataSet.Tables[0].Columns.Count - 1)
					{
						file.Write(",");
					}
				}
				file.Write('\n');
			}
			file.Write('\n');
			file.Write('\n');
			return true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
