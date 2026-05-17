using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using System.IO;
using CAB.BLL;
using CAB.Entity;

namespace CAB.UI
{
	public partial class ImportConsumerData : MdiChildForm 
	{
		ConsumerMasterEntity consumerMasterEntity;
		ConsumerMeterEntity consumerMeterEntity;
		MeterMasterEntity meterMasterEntity;
		ExportImportBLL exportImportBLL; 

		public ImportConsumerData()
		{
			InitializeComponent();
			consumerMasterEntity = new ConsumerMasterEntity();
			consumerMeterEntity = new ConsumerMeterEntity();
			meterMasterEntity = new MeterMasterEntity();
			exportImportBLL = new ExportImportBLL();
		}

		string fileName = string.Empty;

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.DefaultExt = "csv";
			// The Filter property requires a search string after the pipe ( | )
			openFile.Filter = "Export files (*.CSV)|*.CSV"; //|*.csv|*.csv";

			DialogResult result = openFile.ShowDialog();
			if (result == DialogResult.OK)
			{
				txtFile.Text = openFile.FileName;
				fileName = txtFile.Text.Trim();
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			string input = string.Empty;
			List<string> strSplitValue = new List<string>(); 
			StreamReader sr = new StreamReader(fileName);
			
			while ((input = sr.ReadLine()) != null)
			{
				strSplitValue.Add(input);
			}
			
			for (int i = 2; i < strSplitValue.Count; i++)
			{
				if (strSplitValue[i] != "")
				{
					SplitAndInsertValues(strSplitValue[i].ToString());
				}
			}
			CABMessageBox.ShowFilterMessage("M000086", "A000001", MessageBoxButtons.OK);
		}

		/// <summary>
		/// Split the values in from Comma Separated Values and then assign the entities 
		/// with the values and if the values are already present then it is updated
		/// else it is Inserted
		/// </summary>
		/// <param name="meterConsumerData"></param>
		/// <returns></returns>
		bool SplitAndInsertValues(string meterConsumerData)
		{
			bool isConsumerMaster = false;
			bool isMeterMaster = false;
			bool isConsumerMeter = false;

			List<string> entityValues = new List<string>();
			entityValues.AddRange(meterConsumerData.Split(','));

			consumerMasterEntity.Consumer_Number = Convert.ToString(entityValues[0]);
			consumerMasterEntity.Consumer_Name = Convert.ToString(entityValues[1]);
			consumerMasterEntity.ConsumerType_ID = Convert.ToInt32(entityValues[2]);
			consumerMasterEntity.Consumer_Phone = Convert.ToString(entityValues[3]);
			consumerMasterEntity.Consumer_HNumber = Convert.ToString(entityValues[4]);
			consumerMasterEntity.Consumer_Street = Convert.ToString(entityValues[5]);
			consumerMasterEntity.Consumer_City = Convert.ToString(entityValues[6]);
			consumerMasterEntity.Consumer_Email = Convert.ToString(entityValues[7]);

			meterMasterEntity.Meter_ID = Convert.ToString(entityValues[8]);
			meterMasterEntity.MeterType_ID = Convert.ToInt32(entityValues[9]);
			meterMasterEntity.MeterModel_ID = Convert.ToInt32(entityValues[10]);
			meterMasterEntity.Meter_EMF = Convert.ToInt32(entityValues[11]);
			meterMasterEntity.Meter_ContractDemand = Convert.ToInt64(entityValues[12]);
			meterMasterEntity.MeterUnit_ID = Convert.ToInt32(entityValues[13]);
			meterMasterEntity.Meter_CTPrimary = Convert.ToInt32(entityValues[14]);
			meterMasterEntity.Meter_CTSecondary = Convert.ToInt32(entityValues[15]);
			meterMasterEntity.Meter_PTPrimary = Convert.ToInt32(entityValues[16]);
			meterMasterEntity.Meter_PTSecondary = Convert.ToInt32(entityValues[17]);
			meterMasterEntity.Meter_InstalledCTPrimary = Convert.ToInt32(entityValues[18]);
			meterMasterEntity.Meter_InstalledCTSecondary = Convert.ToInt32(entityValues[19]);
			meterMasterEntity.Meter_InstalledPTPrimary = Convert.ToInt32(entityValues[20]);
			meterMasterEntity.Meter_InstalledPTSecondary = Convert.ToInt32(entityValues[21]);
			meterMasterEntity.Meter_Phone = Convert.ToString(entityValues[22]);
			meterMasterEntity.Meter_Status = Convert.ToInt32(entityValues[23]);

			consumerMeterEntity.Meter_ID = Convert.ToString(entityValues[8]);
			consumerMeterEntity.Consumer_Number = Convert.ToString(entityValues[0]);
			consumerMeterEntity.Meter_AllocationDate = Convert.ToInt64(entityValues[24]);
			consumerMeterEntity.Meter_Location = Convert.ToString(entityValues[25]);
			consumerMeterEntity.Status = Convert.ToInt32(entityValues[26]);

			if (exportImportBLL.CheckConsumerAvailable(consumerMasterEntity))
			{
				if (exportImportBLL.UpdateConsumerMasterValues(consumerMasterEntity))
					isConsumerMaster = true;
			}
			else
			{
				if (exportImportBLL.InsertConsumerMasterValues(consumerMasterEntity))
					isConsumerMaster = true;
			}

			if (exportImportBLL.CheckMeterIDAvailable(meterMasterEntity))
			{
				if (exportImportBLL.UpdateMeterValues(meterMasterEntity))
					isMeterMaster = true;
			}
			else
			{
				if (exportImportBLL.InsertMeterValues(meterMasterEntity))
					isMeterMaster = true;
			}

			if (exportImportBLL.CheckConsumerMeterAvailable(consumerMeterEntity))
			{
				if (exportImportBLL.UpdateConsumerMeterValues(consumerMeterEntity))
					isConsumerMeter = true;
			}
			else
			{
				if (exportImportBLL.InsertConsumerMeterValues(consumerMeterEntity))
					isConsumerMeter = true;
			}

			if ((isConsumerMaster) && (isConsumerMeter) && (isMeterMaster))
				return true;

			return false;
		}
	}
}
