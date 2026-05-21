using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAB.BLL;
using CAB.UI.Controls;
using CAB.Entity;
using CAB.Framework.Utility;
using System.Reflection;
using CAB.Framework.Entity;

namespace CAB.UI
{
	public partial class ExportStandardData : MdiChildForm
	{
		ExportImportBLL exportImportBLL;
		List<DataGridViewRow> exportListRow = new List<DataGridViewRow>();
		List<string> columnList = new List<string>();
		Dictionary<string, object> entityDictionary = new Dictionary<string, object>();
		bool isASCIISelected = false;

		GeneralEntity generalEntity;
		InstantPowerEntity instantPowerEntity;
		DataSet exportDataSet;
		TamperCounterEntity tamperCounterEntity;
		TamperCounterGeneralEntity tamperCounterGeneralEntity;
	

		public ExportStandardData()
		{
			InitializeComponent();
			exportImportBLL = new ExportImportBLL();
			generalEntity = new GeneralEntity();
			instantPowerEntity = new InstantPowerEntity();
			tamperCounterEntity = new TamperCounterEntity();
			tamperCounterGeneralEntity = new TamperCounterGeneralEntity();
		}
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ExportStandardData_Load(object sender, EventArgs e)
		{
			//Making the readonly property for the rest of the columns except for the bool column
			dGVCABMeterDetails.DataSource = GetFileList();
			for (int colCount = 0; colCount < dGVCABMeterDetails.Columns.Count - 1; colCount++)
			{
				dGVCABMeterDetails.Columns[colCount].ReadOnly = true;
			}
			dGVCABMeterDetails.Columns["Meterdata_ID"].Visible = false;
			dGVCABMeterDetails.Columns["isSelected"].ReadOnly = false;
		}

		/// <summary>
		/// Get the meter and filename from the fileuploaded and the meterdata table
		/// </summary>
		/// <returns></returns>

		private DataTable GetFileList()
		{
			return exportImportBLL.GetMeterIDFileNameList();
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow drow in dGVCABMeterDetails.Rows)
			{
				if (Convert.ToBoolean(drow.Cells["isSelected"].Value) == true)
				{
					exportListRow.Add(drow);
				}
			}
			if (ExportValues(exportListRow))
			{
				MessageBox.Show("Export Completed Successfully");
			}
		}

		bool ExportValues(List<DataGridViewRow> listEntity)
		{
			//Stream Writer for writing the File
			StreamWriter file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "ExportFile.csv");
			for (int listCount = 0; listCount < listEntity.Count; listCount++)
			{
				//Setting the Property for the meterDataID
				exportImportBLL.MeterDataID = Convert.ToInt32(listEntity[listCount].Cells["MeterData_ID"].Value.ToString());

				generalEntity = exportImportBLL.GetGeneralData() as GeneralEntity;
				WriteEntityContents(generalEntity, "meterdata_general", file);

				instantPowerEntity = exportImportBLL.GetInstantPowerEntity() as InstantPowerEntity;
				WriteEntityContents(instantPowerEntity, "meterdata_instantpower", file);

				exportDataSet = new DataSet();
				exportDataSet = exportImportBLL.GetBillingData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet, file);
				}

				exportDataSet = new DataSet();
				//exportDataSet = exportImportBLL.GetLoadSurveyData();
				//if (exportDataSet != null)
				//{
				//    WriteDataSetContents(exportDataSet, file);
				//}

				exportDataSet= new DataSet();
				exportDataSet = exportImportBLL.GetFraudEnergyData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet, file);
				}

				exportDataSet = new DataSet();
				exportDataSet = exportImportBLL.GetTamperCounterData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet,file);
				}

				exportDataSet = new DataSet();
				exportDataSet = exportImportBLL.GetTamperCounterGeneralData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet, file);
				}

				exportDataSet = new DataSet();
				exportDataSet = exportImportBLL.GetTamperSnapShotsData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet, file);
				}

				exportDataSet = new DataSet();
				exportDataSet = exportImportBLL.GetTariffInformationData();
				if (exportDataSet != null)
				{
					WriteDataSetContents(exportDataSet, file);
				}
			}
			file.Close();
			return true;
		}

		bool WriteEntityContents(IEntity entity, string tableName, StreamWriter file)
		{
			try
			{
				//Get the General ColumnList for the exact Column Names
				columnList = exportImportBLL.GetColumnList(tableName);
				for (int count = 0; count < columnList.Count; count++)
				{
					file.Write(columnList[count].ToString());
					if (count != columnList.Count - 1)
					{
						file.Write(",");
					}
				}
				//Inserting a new line character after the column list has been filled
				file.Write('\n');
				file.Write('\n');
				//Getting the Entity Type
				Type t = entity.GetType();
				//Getting the properties for the Property Info 
				PropertyInfo[] props = t.GetProperties();

				//First Write the Headings for the file columns then write the values
				for (int Countlist = 0; Countlist < columnList.Count; Countlist++)
				{
					foreach (PropertyInfo pInfo in props)
					{
						if (pInfo.Name.Contains(columnList[Countlist].ToString()))
						{
							file.Write(pInfo.GetValue(entity, new object[] { }));
							if (Countlist != columnList.Count - 1)
							{
								file.Write(",");
							}
						}
					}
				}
				file.Write('\n');
				file.Write('\n');
				return true;
			}
			catch (Exception)
			{
				return false;
			}
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
	}
}
