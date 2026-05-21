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
	public partial class ExportConsumerData : MdiChildForm
	{
		ExportImportBLL exportImportBLL = null;
		bool isASCIISelected = false;
		public ExportConsumerData()
		{
			InitializeComponent();
			exportImportBLL = new ExportImportBLL();
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			//Saving the File at a specified location

			string fileLocation = string.Empty;

            //open Save dialog box as per the file extension filter
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Default file extension
            saveFileDialog .DefaultExt = "CSV";

            // Available file extensions
            saveFileDialog .Filter = "Export file (*.CSV)|*.CSV";//|All files (*.*)|*.*";

            // Adds a extension if the user does not
            saveFileDialog .AddExtension = true;

            // Restores the selected directory, next time
            saveFileDialog .RestoreDirectory = true;

            //To Set the Default FileName as the MeterID
            saveFileDialog .FileName = System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".CSV";

            // Dialog title
            saveFileDialog .Title = "Where do you want to save the file?";

            // Startup directory
            //DialogSave.InitialDirectory = System.Windows.Forms.Application.StartupPath;

            saveFileDialog .RestoreDirectory = true;

            // Show the dialog and process the result
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (saveFileDialog.FileName == "")
				{
					CABMessageBox.ShowFilterMessage("M000084", "A000001", MessageBoxButtons.YesNo).Equals(DialogResult.Yes);
					return;
				}
				else
                {
                    fileLocation = saveFileDialog.FileName.Trim();
                }
			}
            else
            {
                return;
            }

			if (fileLocation == "")
			{
				return;
			}

			DataSet dSet = new DataSet();
			StreamWriter writer = new StreamWriter(fileLocation);
			if (rbtnASCII.Checked)
			{
				isASCIISelected = true;
			}
			else
			{
				isASCIISelected = false;
			}
			dSet = exportImportBLL.GetConsumerDetails();

			if (dSet != null)
			{
				if (dSet.Tables[0].Rows.Count > 0)
				{
					WriteDataSetContents(dSet,writer);
				}
			}
			writer.Close();

			CABMessageBox.ShowFilterMessage("M000085", "A000001", MessageBoxButtons.OK,MessageBoxIcon.Information);
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
					if (isASCIISelected == true)
					{
						file.Write(GetASCIIValue(drow[rowCount].ToString()));
					}
					else
					{
						file.Write(drow[rowCount].ToString());
					}
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

		/// <summary>
		/// This function returns the ASCII equivalent of the string passes as paarmeter.
		/// </summary>
		/// <param name="strTemp"></param>
		/// <param name="tempDataSeparator"></param>
		/// <returns>The ASII string corresponding to the string passed as the parameter.</returns>
		private string GetASCIIValue(string strTemp)
		{
			StringBuilder sbTemp = new StringBuilder();
			if ((strTemp == null) || (strTemp.Length == 0))
			{
				return sbTemp.ToString();
			}
			else
			{
				foreach (char ch in strTemp)
				{
					sbTemp.Append(Convert.ToInt32(ch).ToString() + " ");
				}
				return sbTemp.ToString().Substring(0, sbTemp.Length - 1);
			}
		}
	}
}
