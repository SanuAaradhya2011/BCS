using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using CAB.UI.Controls;
using CAB.BLL;
using CAB.Entity;
using System.Collections.Generic;
namespace CAB.UI
{
    public partial class ConsumerImport : MdiChildForm
    {
        private List<ConsumerImportEntity> entities = null;
        public ConsumerImport()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "csv"; 
            openFile.Filter = "Text files (*.txt)|*.txt|*.csv|*.csv";
            if (openFile.ShowDialog().Equals(DialogResult.OK))
                txtFile.Text = openFile.FileName;
            else
                txtFile.Text = string.Empty; 
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string fileName = txtFile.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                this.StatusMessage = "Please select an export file.";
                return;
            }
            else if (!File.Exists(fileName))
            {
                this.StatusMessage = "File does not exist.";
                return;
            }
            else
            {
                if (ImportData(fileName))
                {
                    ConsumerMasterBLL consumerMasterBLL = new ConsumerMasterBLL();
                    ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
                    MeterMasterBLL meterMasterBLL = new MeterMasterBLL();
                    ConsumerMasterEntity oldEntity = null;
                    bool flag = false;
                    foreach (ConsumerImportEntity entity in entities)
                    {
                        ConsumerMasterEntity consumerMasterEntity = entity.ConsumerMaster as ConsumerMasterEntity;
                        flag = consumerMasterBLL.ValidateConsumerNumber(consumerMasterEntity);
                        if (flag)
                        {
                            MessageBox.Show("Data Already available. \n Please delete the data first then Import.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //if (flag)
                        //{
                        //    DialogResult dr = MessageBox.Show("Do you want to overwrite the data", "BCS", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                        //    if (dr.Equals(DialogResult.Yes))
                        //    {
                        //        flag = true;
                        //        break;
                        //    }
                        //    else
                        //        flag = false;
                        //}
                        else
                        { 
                            MeterMasterEntity meterMasterEntity=entity.MeterMaster as MeterMasterEntity;
                            flag = meterMasterBLL.ValidateMeterNumber(meterMasterEntity);
                            if(flag)
                            {
                                MessageBox.Show("Data Already available. \n Please delete the data first then Import.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    foreach (ConsumerImportEntity entity in entities)
                    {
                        ConsumerMasterEntity consumerMasterEntity = entity.ConsumerMaster as ConsumerMasterEntity;
                        MeterMasterEntity meterMasterEntity = entity.MeterMaster as MeterMasterEntity;
                        ConsumerMeterEntity consumerMeterEntity = entity.ConsumerMeter as ConsumerMeterEntity;
                        bool innerFlag = consumerMasterBLL.ValidateConsumerNumber(consumerMasterEntity);
                        consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                        consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                        if (oldEntity != consumerMasterEntity)
                        {
                            if (oldEntity == null)
                                oldEntity = consumerMasterEntity;
                            if ((innerFlag && flag))
                            {
                                consumerMasterBLL.DeleteData(consumerMasterEntity);
                                meterMasterBLL.DeleteData(meterMasterEntity);
                                consumerMeterBLL.DeleteData(consumerMeterEntity);
                                consumerMasterBLL.InsertData(consumerMasterEntity);
                                meterMasterBLL.InsertData(meterMasterEntity);
                                consumerMeterBLL.InsertData(consumerMeterEntity);
                            }
                            else if (innerFlag && !flag)
                            {
                                meterMasterBLL.InsertData(meterMasterEntity);
                                consumerMeterBLL.InsertData(consumerMeterEntity);
                            }
                            else
                            {
                                consumerMasterBLL.InsertData(consumerMasterEntity);
                                meterMasterBLL.InsertData(meterMasterEntity);
                                consumerMeterBLL.InsertData(consumerMeterEntity);
                            }
                        }
                    }
                    this.StatusMessage = "File Imported successfully.";
                } 
                else
                    this.StatusMessage = "File corrupted.";
            }
            Application.DoEvents();
        }
     

        private bool ImportData(string fileName)
        {
            try
            {
                string data = "";
                long lengths = 0;
                StreamReader sr = new StreamReader(fileName);
                while (true)
                {
                    string dataTmp = sr.ReadLine();
                    if (string.IsNullOrEmpty(dataTmp))
                        break;
                    data = dataTmp;
                    lengths = lengths + data.Length;
                }
                sr.Close();
                lengths = lengths - data.Length;
                if (lengths != Convert.ToInt64(data))
                {
                    this.StatusMessage = "File corrupted.";
                    Application.DoEvents();
                    return false;
                }
                     sr = new StreamReader(fileName);
                string lineData = string.Empty;
                string[] columnNames = null;
                bool Flag = true;
                while ((lineData = sr.ReadLine()) != null)
                {
                    lineData = lineData.Trim();
                    if (data.Trim().Equals(lineData))
                        break;
                    if (Flag)
                    {
                        Flag = false;
                        entities = new List<ConsumerImportEntity>();
                        columnNames = lineData.Split(',');
                        if (string.IsNullOrEmpty(lineData))
                        {
                            this.StatusMessage = "File does not contain header information";
                            return false;
                        }
                        if (lineData.IndexOf("Consumer_Number") < 0 || lineData.IndexOf("Meter_ID") < 0)
                        {
                            this.StatusMessage = "File does not contain consumer/Meter information";
                            return false;
                        }
                    }
                    else
                    {
                        ConsumerMasterEntity consumerMasterEntity = new ConsumerMasterEntity();
                        MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
                        ConsumerMeterEntity consumerMeterEntity = new ConsumerMeterEntity();
                        ConsumerImportEntity entity = new ConsumerImportEntity();
                        string[] columnData = lineData.Split(',');
                        for (int counter = 0; counter < columnNames.Length; counter++)
                        {
                            if (columnNames[counter].Trim().Equals("Consumer_Number"))
                                consumerMasterEntity.Consumer_Number = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer_Name"))
                                consumerMasterEntity.Consumer_Name = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("ConsumerType_ID"))
                                consumerMasterEntity.ConsumerType_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Consumer_Phone"))
                                consumerMasterEntity.Consumer_Phone = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer_HNumber"))
                                consumerMasterEntity.Consumer_HNumber = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer_Street"))
                                consumerMasterEntity.Consumer_Street = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer_City"))
                                consumerMasterEntity.Consumer_City = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer_Email"))
                                consumerMasterEntity.Consumer_Email = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter_ID"))
                                meterMasterEntity.Meter_ID = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("MeterType_ID"))
                                meterMasterEntity.MeterType_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("MeterModel_ID"))
                                meterMasterEntity.MeterModel_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_Location"))
                                consumerMeterEntity.Meter_Location = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter_AllocationDate"))
                                consumerMeterEntity.Meter_AllocationDate = ConvertToLong(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_EMF"))
                                meterMasterEntity.Meter_EMF = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_ContractDemand"))
                                meterMasterEntity.Meter_ContractDemand = ConvertToDouble(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("MeterUnit_ID"))
                                meterMasterEntity.MeterUnit_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_CTPrimary"))
                                meterMasterEntity.Meter_CTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_CTSecondary"))
                                meterMasterEntity.Meter_CTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_PTPrimary"))
                                meterMasterEntity.Meter_PTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_PTSecondary"))
                                meterMasterEntity.Meter_PTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_InstalledCTPrimary"))
                                meterMasterEntity.Meter_InstalledCTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_InstalledCTSecondary"))
                                meterMasterEntity.Meter_InstalledCTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_InstalledPTPrimary"))
                                meterMasterEntity.Meter_InstalledPTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_InstalledPTSecondary"))
                                meterMasterEntity.Meter_InstalledPTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter_Phone"))
                                meterMasterEntity.Meter_Phone = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter_Status"))
                                meterMasterEntity.Meter_Status = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Status"))
                                consumerMeterEntity.Status = ConvertToInt(columnData[counter]);
                        }
                        entity.ConsumerMaster = consumerMasterEntity;
                        entity.MeterMaster = meterMasterEntity;
                        entity.ConsumerMeter = consumerMeterEntity;
                        entities.Add(entity);
                    }
                }
                sr.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private double ConvertToDouble(string data)
        {
            if (string.IsNullOrEmpty(data))
                return 0.00;
            else
            {
                try
                {
                    return Convert.ToDouble(data);
                }
                catch (Exception)
                {
                    return 0.00;
                }
            }
        }
        private Int32 ConvertToInt(string data)
        {
            if (string.IsNullOrEmpty(data))
                return 0;
            else
            {
                try
                {
                    return Convert.ToInt32(data);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        private Int64 ConvertToLong(string data)
        {
            if (string.IsNullOrEmpty(data))
                return 0;
            else
            {
                try
                {
                    return Convert.ToInt64(data);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        private void ConsumerImport_Load(object sender, EventArgs e)
        {
            this.Text = "Consumer Import";
            this.StatusMessage = string.Empty;
        }
    }
}
