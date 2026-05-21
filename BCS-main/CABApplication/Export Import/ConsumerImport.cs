using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using CAB.BLL;
using CAB.Entity;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.Framework;
using System.Text;
using System.Linq;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using CABFramework;
using CAB.Framework.Entity;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
    public partial class ConsumerImport : MdiChildForm
    {
        private List<ConsumerImportEntity> entities = null;
        private const string DefaultRegion = "Test";
        private const string Defaultvalue = "---";
        private const int DefaultConsumerType = 5;
        private const int DefaultCTPTRatio = 1;
        private const int DefaultComsumerMeterStatus = 1;
        private const int DefaultMeterUnitId = 1;
        private const int DefaultEMF = 1;
        private const int DefaultMeterContractDemand = 1;
        MeterMasterBLL metermasterBLL;
        MeterMasterEntity meterMasterEntity;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ConsumerImport).ToString());
        public ConsumerImport()
        {
            InitializeComponent();

            metermasterBLL = new MeterMasterBLL();
            meterMasterEntity = new MeterMasterEntity();
            //// if gprs is enabled for utility 
            //if (UtilityDetails.ShowExcelImportInConsumerImport)
            //{
            downloadXlsxLink.Visible = true;
            saveFileDialog.DefaultExt = "xls";
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = "Consumer";
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls*";
            //}
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
            //VBM - Excel Import for NDPL.
            //if (UtilityDetails.ShowExcelImportInConsumerImport)
            //{
            openFile.Filter = "Text files (*.txt)|*.txt|*.csv|*.csv|Excel files (*.xls)|*.xls*";
            //}
            //else
            //{                
            //    openFile.Filter = "Text files (*.txt)|*.txt|*.csv|*.csv";
            //}
            if (openFile.ShowDialog().Equals(DialogResult.OK))
                txtFile.Text = openFile.FileName;
            else
                txtFile.Text = string.Empty;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                this.StatusMessage = string.Empty;
                float consumer_Number;
                DateTime dt;
                this.Cursor = Cursors.WaitCursor;
                string fileName = txtFile.Text.Trim();

                if (string.IsNullOrEmpty(fileName))
                {
                    this.StatusMessage = "File name can't be empty.";
                    return;
                }
                else if (!File.Exists(fileName))
                {
                    this.StatusMessage = "File does not exist.";
                    return;
                }
                else
                {
                    string extension = fileName.Substring(fileName.LastIndexOf("."), 4).ToUpper();
                    if (extension != ".XLS" && ImportData(fileName))
                    {
                        ConsumerMasterBLL consumerMasterBLL = new ConsumerMasterBLL();
                        ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
                        MeterMasterBLL meterMasterBLL = new MeterMasterBLL();
                        RegionBLL regionBLL = new RegionBLL();
                        CircleBLL circleBLL = new CircleBLL();
                        DivisionBLL divisionBLL = new DivisionBLL();
                        AreaBLL areaBLL = new AreaBLL();
                        AreaMeterBLL areaMeterBLL = new AreaMeterBLL();
                        ConsumerMasterEntity oldEntity = null;
                        bool flag = false;
                        foreach (ConsumerImportEntity entity in entities)
                        {
                            ConsumerMasterEntity consumerMasterEntity = entity.ConsumerMaster as ConsumerMasterEntity;
                            flag = consumerMasterBLL.ValidateConsumerNumber(consumerMasterEntity);
                            if (flag)
                            {
                                MessageBox.Show("Data already available. Please delete data from consumer meter definition and manage area.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                            {
                                if (!float.TryParse(consumerMasterEntity.Consumer_Number, out consumer_Number))
                                {
                                    MessageBox.Show("Consumer ID field should be numeric.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            MeterMasterEntity meterMasterEntity = entity.MeterMaster as MeterMasterEntity;
                            flag = meterMasterBLL.ValidateMeterNumber(meterMasterEntity);
                            if (flag)
                            {

                                MessageBox.Show("Data already available. Please delete data from consumer meter definition and manage area.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            //20th april 2012
                            RegionEntity regionEntity = entity.Region as RegionEntity;
                            flag = regionBLL.ValidateRegion(regionEntity);
                            if (flag)
                            {

                                MessageBox.Show("Data already available. Please delete data from consumer meter definition and manage area.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            CircleEntity circleEntity = entity.Circle as CircleEntity;
                            flag = circleBLL.ValidateCircle(circleEntity);
                            if (flag)
                            {

                                MessageBox.Show("Data already available. Please delete data from consumer meter definition and manage area.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            DivisionEntity divisionEntity = entity.Division as DivisionEntity;
                            flag = divisionBLL.ValidateDivision(divisionEntity);
                            if (flag)
                            {

                                MessageBox.Show("Data already available. Please delete data from consumer meter definition and manage area.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //20th april 2012

                            ConsumerMeterEntity consumerMeterEntity = entity.ConsumerMeter as ConsumerMeterEntity;
                            dt = DateUtility.LongToDateTime(consumerMeterEntity.Meter_AllocationDate);
                        }
                        foreach (ConsumerImportEntity entity in entities)
                        {
                            //20th April 2012
                            RegionEntity regionEntity = entity.Region as RegionEntity;
                            CircleEntity circleEntity = entity.Circle as CircleEntity; ;
                            DivisionEntity divisionEntity = entity.Division as DivisionEntity;
                            AreaEntity areaEntity = new AreaEntity();
                            AreaMeterEntity areaMeterEntity = new AreaMeterEntity();

                            regionBLL.InsertData(regionEntity);
                            regionEntity = regionBLL.ValidateRegion(regionEntity.RegionName) as RegionEntity;

                            circleEntity.RegionID = regionEntity.RegionID;
                            circleBLL.InsertData(circleEntity);

                            circleEntity = circleBLL.ValidateCircle(circleEntity.CircleName) as CircleEntity;
                            divisionEntity.RegionID = regionEntity.RegionID;
                            divisionEntity.CircleID = circleEntity.CircleID;
                            divisionBLL.InsertData(divisionEntity);
                            //20th April 2012

                            divisionEntity = divisionBLL.ValidateDivision(divisionEntity.DivisionName) as DivisionEntity;

                            areaEntity.Circle_ID = circleEntity.CircleID;
                            areaEntity.Region_ID = regionEntity.RegionID;
                            areaEntity.Divsion_ID = regionEntity.RegionID;
                            areaBLL.InsertData(areaEntity);

                            
                            ConsumerMasterEntity consumerMasterEntity = entity.ConsumerMaster as ConsumerMasterEntity;
                            MeterMasterEntity meterMasterEntity = entity.MeterMaster as MeterMasterEntity;
                            ConsumerMeterEntity consumerMeterEntity = entity.ConsumerMeter as ConsumerMeterEntity;
                            bool innerFlag = consumerMasterBLL.ValidateConsumerNumber(consumerMasterEntity);
                            consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                            consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;

                            areaEntity = areaBLL.ValidateData(areaEntity.Divsion_ID, areaEntity.Circle_ID, areaEntity.Region_ID) as AreaEntity;
                            areaMeterEntity.Area_ID = areaEntity.Area_ID;
                            areaMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                            areaMeterBLL.InsertData(areaMeterEntity);

                            //20th April 2012
                            regionEntity = regionBLL.ValidateRegion(regionEntity.RegionName) as RegionEntity;
                            consumerMeterEntity.Region_ID = regionEntity.RegionID;
                            circleEntity = circleBLL.ValidateCircle(circleEntity.CircleName) as CircleEntity;
                            consumerMeterEntity.Circle_ID = circleEntity.CircleID;
                            divisionEntity = divisionBLL.ValidateDivision(divisionEntity.DivisionName) as DivisionEntity;
                            consumerMeterEntity.Division_ID = divisionEntity.DivisionID;
                            //20th April 2012
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
                    else if (extension == ".XLS") // For Excel Import 
                    {
                        bool ImportFlag = false;
                        if (Convert.ToBoolean(ConfigSettings.GetValue("Is64BitConsumerImport")))
                        {
                            ImportFlag = ValidateandImportExcelData(fileName);
                        }
                        else
                        {
                            ImportFlag = ValidateAndImportExcelData(fileName);
                        }
                        //if ()
                        if (ImportFlag)
                        {
                            InsertConsumerMeterData();
                            this.StatusMessage = "File Imported successfully.";
                        }
                        else
                            this.StatusMessage = "File corrupted.";
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "btnImport_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                System.Windows.Forms.Application.DoEvents();
                this.Cursor = Cursors.Default;
            }
        }

        private bool ValidateandImportExcelData(string fileName)
        {
             bool retValue = false;
             Microsoft.Office.Interop.Excel.Application xlApp = null;
             Microsoft.Office.Interop.Excel.Workbook xlWorkbook = null;
             DataSet ds = new DataSet();
             StringBuilder errorMessage = null;
             try
             {
                 string FilePath = fileName;
                 xlApp = new Microsoft.Office.Interop.Excel.Application();
                 xlApp.DisplayAlerts = false;
                 xlWorkbook = xlApp.Workbooks.Open(@FilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                 int maxSheetCount = xlWorkbook.Sheets.Count;
                 for (int i = 0; i < maxSheetCount; i++)
                 {
                    _Worksheet xlWorksheet;// = (_Worksheet)xlWorkbook.Sheets[i + 1];
                    // Range xlRange = xlWorksheet.UsedRange;
                    // System.Data.DataTable dt = GetDataTableFromSheet(xlRange);
                   //  dt.TableName = "Sheet" + (i + 1).ToString();
                    // ds.Tables.Add(dt);
                 }

                 // GPRS:made changes in column count to make it work for GPRS
                 if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count >= 5)
                 {
                     entities = new List<ConsumerImportEntity>();
                     ConsumerMasterBLL consumerMaster = new ConsumerMasterBLL();
                     DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
                     long consumerNumber = 0;
                     long tempconsumerNumber = consumerNumber;
                     int tempconsumertype = 0;
                     int excelRowNumber = 0;
                     string modemIMEI = string.Empty;
                     //Check for duplicate values of meterDi and Sim number in Excel rows.
                     string errDupMessage = CheckForDuplicates(ds.Tables[0]);
                     // No duplicate ,its fine Move on. 
                     if (errDupMessage.Length == 0)
                     {
                         DataSet dsMeterIdSimNumber = metermasterBLL.GetMeterDetails();
                         DataSet allMeterTypeDS = generalBLL.GetAllMeterType();
                         DataSet allMeterModelDS = generalBLL.GetAllMeterModel();

                         retValue = ValidationConsumerRecord(dsMeterIdSimNumber, ds);

                         if (retValue)
                         {
                             foreach (DataRow consumerRow in ds.Tables[0].Rows)
                             {
                                 string ConsumerID = "";
                                 string ConsumerType = "";

                                 //make sure that even if excel does not have modem IMEI column it exports data as this not mandatory field.
                                 if (ds.Tables[0].Columns.Count > 5)
                                 {
                                     modemIMEI = consumerRow["GPRS Modem IMEI"].ToString().Trim();
                                 }
                                 excelRowNumber++;
                                 
                                 ConsumerMasterEntity consumerMasterEntity = new ConsumerMasterEntity();
                                 MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
                                 ConsumerMeterEntity consumerMeterEntity = new ConsumerMeterEntity();
                                 ConsumerImportEntity entity = new ConsumerImportEntity();
                                 RegionEntity regionEntity = new RegionEntity();
                                 CircleEntity circleEntity = new CircleEntity();
                                 DivisionEntity divisionEntity = new DivisionEntity();


                                 string meterId = consumerRow[0].ToString().Trim();
                                 string meterType = consumerRow[2].ToString().Trim();
                                 string meterModel = consumerRow[3].ToString().Trim();
                                 string simNumber = consumerRow[1].ToString().Trim();
                                 string commType = consumerRow[4].ToString().Trim();
                                 if (consumerRow.ItemArray.Length > 6) ConsumerType = consumerRow[6].ToString().Trim();
                                 if (consumerRow.ItemArray.Length > 7) ConsumerID = consumerRow[7].ToString().Trim();

                                 ////Validate Excel Data
                                 //string errMessage = ValidateData(meterId, meterType, meterModel, simNumber, commType, modemIMEI, ds);



                                 ////There are some errors , Say to user.
                                 //if (!string.IsNullOrEmpty(errorMessage.ToString()))
                                 //{
                                 //    errorMessage.Append("Error At Excel Row " + excelRowNumber.ToString() + " : " + errorMessage.ToString() + ".\n");
                                 //    retValue = false;
                                 //}
                                 //Data is fine Go ahead to create entity.
                                 if (retValue)
                                 {
                                     // Need to change for PED
                                     //if (!long.TryParse(ConsumerID, System.Globalization.NumberStyles.AllowHexSpecifier, null, out tempconsumerNumber )) { }
                                     //if (!long.TryParse(ConsumerID, System.Globalization.NumberStyles.AllowHexSpecifier, null, out tempconsumerNumber
                                     
//                                     if (tempconsumerNumber == 0) consumerNumber = consumerNumber + 1;

  //                                   else consumerNumber = tempconsumerNumber;
                                     meterMasterEntity.Meter_ID = meterId;
                                     meterMasterEntity.MeterType_ID = GetMeterTypeId(meterType, meterId, commType);
                                     meterMasterEntity.MeterModel_ID = GetMeterModelId(meterModel, meterId, commType);
                                     if (simNumber != "") meterMasterEntity.Meter_Phone = "0" + simNumber;
                                     else meterMasterEntity.Meter_Phone = simNumber;


                                     //consumerMasterEntity.Consumer_Number = consumerNumber.ToString("X12");
                                     consumerMasterEntity.Consumer_Number = ConsumerID.PadLeft(12, ' ');// consumerNumber.ToString("X12");
                                     consumerMasterEntity.Consumer_Name = Defaultvalue;
                                     if (ConsumerType == "") consumerMasterEntity.ConsumerType_ID = DefaultConsumerType; //others
                                     else consumerMasterEntity.ConsumerType_ID = GetConsumerTypeId(ConsumerType);
                                     consumerMasterEntity.Consumer_Phone = "";
                                     consumerMasterEntity.Consumer_HNumber = Defaultvalue;
                                     consumerMasterEntity.Consumer_Street = Defaultvalue;
                                     consumerMasterEntity.Consumer_City = Defaultvalue;
                                     consumerMasterEntity.Consumer_Email = "";

                                     consumerMeterEntity.Meter_ID = consumerRow[0].ToString().Trim();
                                     consumerMeterEntity.Meter_Location = Defaultvalue;
                                     consumerMeterEntity.Meter_AllocationDate = DateUtility.DateTimeToLong(DateTime.Now);
                                     consumerMeterEntity.Communcation_Type = commType;
                                     consumerMeterEntity.Status = DefaultComsumerMeterStatus;
                                     consumerMeterEntity.Consumer_Number = consumerNumber.ToString();


                                     meterMasterEntity.Meter_EMF = DefaultEMF;
                                     meterMasterEntity.Meter_ContractDemand = DefaultMeterContractDemand;
                                     meterMasterEntity.Meter_CTSecondary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_CTPrimary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_PTPrimary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_PTSecondary = DefaultCTPTRatio;
                                     meterMasterEntity.MeterUnit_ID = DefaultMeterUnitId;
                                     regionEntity.RegionName = DefaultRegion;
                                     circleEntity.CircleName = DefaultRegion;
                                     divisionEntity.DivisionName = DefaultRegion;
                                     meterMasterEntity.Meter_InstalledCTPrimary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_InstalledCTSecondary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_InstalledPTPrimary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_InstalledPTSecondary = DefaultCTPTRatio;
                                     meterMasterEntity.Meter_Status = DefaultComsumerMeterStatus;
                                     meterMasterEntity.MeterGPRSModemIMEI = modemIMEI;

                                     entity.ConsumerMaster = consumerMasterEntity;
                                     entity.MeterMaster = meterMasterEntity;
                                     entity.ConsumerMeter = consumerMeterEntity;
                                     entity.Region = regionEntity;
                                     entity.Circle = circleEntity;
                                     entity.Division = divisionEntity;
                                     entities.Add(entity);
                                 }
                             }
                             // Adding endpoint to M2M database.
                             SyncGPRSOnAdd(ds);
                         }
                     }
                     // Duplicate values ,Display message to user.
                     else
                     {
                         errorMessage = new StringBuilder(errDupMessage);
                         MessageBox.Show(errorMessage.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         retValue = false;
                     }

                 }
                 // Excel File format is invalid.
                 else
                 {
                     errorMessage = new StringBuilder("Excel File Structure is not correct or it has no data");
                     retValue = false; 
                 }
                 ////Display Combined Error message to user.
                 //if (!retValue)
                 //{
                 //    MessageBox.Show(errorMessage.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 //}
                 return retValue;  
                 //SaveXls(@FileName);0
             }
             catch (Exception ex)    //Exception log for catch block
             {
                 MessageBox.Show(ex.Message);
                 logger.Log(LOGLEVELS.Error, "ValidateandImportExcelData(string fileName)", ex);
             }
            finally
            {
                if (xlWorkbook != null)
                {
                    xlWorkbook.Saved = true;
                    xlWorkbook.Close(true, Type.Missing, Type.Missing);
                    xlApp.Quit();
                    Marshal.FinalReleaseComObject(xlWorkbook);
                    Marshal.FinalReleaseComObject(xlApp);
                    xlWorkbook = null;
                    xlApp = null;
                    GC.Collect();
                }
            }
            return retValue;
        }

        private System.Data.DataTable GetDataTableFromSheet(Range xlRange)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                int rowCount = xlRange.Rows.Count;
                int columnCount = xlRange.Columns.Count;
                if (rowCount > 0)
                {
                    for (int i = 1; i <= columnCount; i++)
                    {
                        string Temp = string.Empty;
                       // Temp = Convert.ToString(((Range)xlRange.Cells[1, i]).Value2);
                        dt.Columns.Add(Temp, typeof(string));
                    }

                    for (int i = 2; i <= rowCount; i++)
                    {
                        //if (Convert.ToString(((Range)xlRange.Cells[i, 1]).Value2) == string.Empty)
                        //{
                        //    break;
                        //}
                        DataRow drRow = dt.NewRow();
                        for (int j = 1; j <= columnCount; j++)
                        {
                            string Temp = string.Empty;
                            //Temp = Convert.ToString(((Range)xlRange.Cells[i, j]).Value2);
                            drRow[j-1] = Temp;
                        }
                        dt.Rows.Add(drRow);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show(ex.Message);
                logger.Log(LOGLEVELS.Error, "GetDataTableFromSheet(Range xlRange)", ex);
            }
            return dt;
        }


       

        



        /// <summary>
        /// VBM - Validates and imports Excel Data into DataSet
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool ValidateAndImportExcelData(string fileName)
        {
           
            try
            {
                #region Excel Import into DataSet
                DataSet ds = new DataSet();
                bool retValue = true;
                StringBuilder errorMessage = new StringBuilder();
                try
                {
                    string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 12.0;"; ;
                    using (OleDbConnection dbConnection = new OleDbConnection(strConn))
                    {
                        dbConnection.Open();
                        string sheetName = dbConnection.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
                        using (OleDbDataAdapter dbAdapter = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", sheetName), dbConnection))
                        {
                            dbAdapter.Fill(ds);
                        }
                    }

                    // GPRS changes,to remove empty rows

                    IEnumerable<DataRow> clearedDataRow = ds.Tables[0].Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare(field.ToString().Trim(), string.Empty) == 0));

                    if (clearedDataRow.Count<DataRow>() > 0)
                    {
                        ds.Tables.Clear();
                        ds.Tables.Add(clearedDataRow.CopyToDataTable());
                    }
                    else
                    {
                        MessageBox.Show("No entries found.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    // end 
                }
                catch (Exception message)    //Exception log for catch block
                {
                    MessageBox.Show(message.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    logger.Log(LOGLEVELS.Error, "ValidateAndImportExcelData(string fileName)", message);
                    return false;
                }
            #endregion
                // GPRS:made changes in column count to make it work for GPRS
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count >= 5)
                {
                    entities = new List<ConsumerImportEntity>();
                    ConsumerMasterBLL consumerMaster = new ConsumerMasterBLL();
                    DLMS650GeneralBLL generalBLL = new DLMS650GeneralBLL();
                    long consumerNumber = 0;
                    long tempconsumerNumber = consumerNumber;
                    int tempconsumertype = 0;
                    int excelRowNumber = 0;
                    string modemIMEI = string.Empty;
                    //Check for duplicate values of meterDi and Sim number in Excel rows.
                    string errDupMessage = CheckForDuplicates(ds.Tables[0]);
                    // No duplicate ,its fine Move on. 
                    if (errDupMessage.Length == 0)
                    {
                        DataSet dsMeterIdSimNumber = metermasterBLL.GetMeterDetails();
                        DataSet allMeterTypeDS = generalBLL.GetAllMeterType();
                        DataSet allMeterModelDS = generalBLL.GetAllMeterModel();

                        retValue = ValidationConsumerRecord(dsMeterIdSimNumber, ds);

                        if (retValue)
                        {
                            foreach (DataRow consumerRow in ds.Tables[0].Rows)
                            {
                                string ConsumerID = "";
                                string ConsumerType = "";

                                //make sure that even if excel does not have modem IMEI column it exports data as this not mandatory field.
                                if (ds.Tables[0].Columns.Count > 5)
                                {
                                    modemIMEI = consumerRow["GPRS Modem IMEI"].ToString().Trim();
                                }
                                excelRowNumber++;


                                ConsumerMasterEntity consumerMasterEntity = new ConsumerMasterEntity();
                                MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
                                ConsumerMeterEntity consumerMeterEntity = new ConsumerMeterEntity();
                                ConsumerImportEntity entity = new ConsumerImportEntity();
                                RegionEntity regionEntity = new RegionEntity();
                                CircleEntity circleEntity = new CircleEntity();
                                DivisionEntity divisionEntity = new DivisionEntity();


                                string meterId = consumerRow[0].ToString().Trim();
                                string meterType = consumerRow[2].ToString().Trim();
                                string meterModel = consumerRow[3].ToString().Trim();
                                string simNumber = consumerRow[1].ToString().Trim();
                                string commType = consumerRow[4].ToString().Trim();
                                if (consumerRow.ItemArray.Length > 6) ConsumerType = consumerRow[6].ToString().Trim();
                                if (consumerRow.ItemArray.Length > 7) ConsumerID = consumerRow[7].ToString().Trim();

                                ////Validate Excel Data
                                //string errMessage = ValidateData(meterId, meterType, meterModel, simNumber, commType, modemIMEI, ds);



                                ////There are some errors , Say to user.
                                //if (!string.IsNullOrEmpty(errorMessage.ToString()))
                                //{
                                //    errorMessage.Append("Error At Excel Row " + excelRowNumber.ToString() + " : " + errorMessage.ToString() + ".\n");
                                //    retValue = false;
                                //}
                                //Data is fine Go ahead to create entity.
                                if (retValue)
                                {
                                    if (!long.TryParse(ConsumerID, out tempconsumerNumber)) { }
                                    
                                    if (tempconsumerNumber == 0) consumerNumber = consumerNumber + 1;
                                    else consumerNumber = tempconsumerNumber;
                                    meterMasterEntity.Meter_ID = meterId;
                                    meterMasterEntity.MeterType_ID = GetMeterTypeId(meterType, meterId, commType);
                                    meterMasterEntity.MeterModel_ID = GetMeterModelId(meterModel, meterId, commType);
                                    if (simNumber != "") meterMasterEntity.Meter_Phone = "0" + simNumber;
                                    else meterMasterEntity.Meter_Phone = simNumber;


                                    consumerMasterEntity.Consumer_Number = consumerNumber.ToString();
                                    consumerMasterEntity.Consumer_Name = Defaultvalue;
                                    if (ConsumerType == "") consumerMasterEntity.ConsumerType_ID = DefaultConsumerType; //others
                                    else consumerMasterEntity.ConsumerType_ID = GetConsumerTypeId(ConsumerType);
                                    consumerMasterEntity.Consumer_Phone = "";
                                    consumerMasterEntity.Consumer_HNumber = Defaultvalue;
                                    consumerMasterEntity.Consumer_Street = Defaultvalue;
                                    consumerMasterEntity.Consumer_City = Defaultvalue;
                                    consumerMasterEntity.Consumer_Email = "";

                                    consumerMeterEntity.Meter_ID = consumerRow[0].ToString().Trim();
                                    consumerMeterEntity.Meter_Location = Defaultvalue;
                                    consumerMeterEntity.Meter_AllocationDate = DateUtility.DateTimeToLong(DateTime.Now);
                                    consumerMeterEntity.Communcation_Type = commType;
                                    consumerMeterEntity.Status = DefaultComsumerMeterStatus;
                                    consumerMeterEntity.Consumer_Number = consumerNumber.ToString();


                                    meterMasterEntity.Meter_EMF = DefaultEMF;
                                    meterMasterEntity.Meter_ContractDemand = DefaultMeterContractDemand;
                                    meterMasterEntity.Meter_CTSecondary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_CTPrimary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_PTPrimary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_PTSecondary = DefaultCTPTRatio;
                                    meterMasterEntity.MeterUnit_ID = DefaultMeterUnitId;
                                    regionEntity.RegionName = DefaultRegion;
                                    circleEntity.CircleName = DefaultRegion;
                                    divisionEntity.DivisionName = DefaultRegion;
                                    meterMasterEntity.Meter_InstalledCTPrimary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_InstalledCTSecondary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_InstalledPTPrimary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_InstalledPTSecondary = DefaultCTPTRatio;
                                    meterMasterEntity.Meter_Status = DefaultComsumerMeterStatus;
                                    meterMasterEntity.MeterGPRSModemIMEI = modemIMEI;

                                    entity.ConsumerMaster = consumerMasterEntity;
                                    entity.MeterMaster = meterMasterEntity;
                                    entity.ConsumerMeter = consumerMeterEntity;
                                    entity.Region = regionEntity;
                                    entity.Circle = circleEntity;
                                    entity.Division = divisionEntity;
                                    entities.Add(entity);
                                }
                            }
                            // Adding endpoint to M2M database.
                            SyncGPRSOnAdd(ds);
                        }
                    }
                    // Duplicate values ,Display message to user.
                    else
                    {
                        errorMessage = new StringBuilder(errDupMessage);
                        MessageBox.Show(errorMessage.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        retValue = false;
                    }

                }
                // Excel File format is invalid.
                else
                {
                    errorMessage = new StringBuilder("Excel File Structure is not correct or it has no data");
                    retValue = false;
                }
                ////Display Combined Error message to user.
                //if (!retValue)
                //{
                //    MessageBox.Show(errorMessage.ToString(), "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                return retValue;
            }
            catch (Exception exMessage)
            {
                MessageBox.Show(exMessage.Message, "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                logger.Log(LOGLEVELS.Error, "ValidateAndImportExcelData(string fileName)", exMessage);
                return false;
            }
        }

        /// <summary>
        /// valiadtes customer records ,and returns true if data is fine.
        /// </summary>
        /// <param name="dsMeterIdSimNumberGSM"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private bool ValidationConsumerRecord(DataSet dsMeterIdSimNumber, DataSet excelImportDS)
        {
            string errorMessage = string.Empty;
            bool isValidated = true;
            int excelRowNumber = 0;
            foreach (DataRow consumerRow in excelImportDS.Tables[0].Rows)
            {
                excelRowNumber++;
                string errMsg = ValidateData(consumerRow, dsMeterIdSimNumber);

                if (!string.IsNullOrEmpty(errMsg))
                {
                    errorMessage = errorMessage + "Error At Excel Row " + excelRowNumber.ToString() + " : " + errMsg + ".\n";

                }
            }
            if (errorMessage.ToString() != string.Empty)
            {
                MessageBox.Show(errorMessage.ToString());
                isValidated = false;
            }

            return isValidated;
        }
        /// <summary>
        ///  valiadtes customer records for each row and returns error msg , if error found.
        /// </summary>
        /// <param name="consumerRow"></param>
        /// <param name="dsMeterIdSimNumberGSM"></param>
        /// <returns></returns>
        private string ValidateData(DataRow consumerRow, DataSet dsMeterIdSimNumber)
        {
            string meterId = consumerRow["MeterId"].ToString().Trim();
            string meterType = consumerRow["MeterType"].ToString().Trim();
            string meterModel = consumerRow["MeterModel"].ToString().Trim();
            string simNumber = consumerRow["Sim Number"].ToString().Trim();
            string commType = consumerRow["CommType"].ToString().Trim();
            long intSimNumber;
            StringBuilder errorMessage = new StringBuilder();
            MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
            meterMasterEntity.Meter_ID = meterId;

            // these are non gprs specific validaition
            if (commType.ToUpper() == CommunicationType.FTP.ToString())
            {
                # region TCP Specific Validation
                // added to support GPRS communication
                string imeiNumber = consumerRow[5].ToString().Trim();
                if (string.IsNullOrEmpty(meterId))
                {
                    errorMessage.Append("Meter Id is invalid,");
                }
                if (!new Regex("^[A-Za-z0-9]{7,16}$", RegexOptions.Compiled).Match(meterId).Success)
                {
                    errorMessage.Append("Meter Id can only contain alphanumeric characters and length should be greater than 7 and less than 16,");
                }
                if (!new Regex("^[0-9]{10}$", RegexOptions.Compiled).Match(simNumber).Success)
                {
                    errorMessage.Append("SIM number is invalid,");
                }
                Boolean isValidType = false;
                Array meterModelTypes = Enum.GetValues(typeof(MeterTypes));
                foreach (MeterTypes val in meterModelTypes)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterType.ToUpper())
                    {
                        isValidType = true;
                    }
                }
                if (!isValidType)
                {
                    errorMessage.Append("Meter Type is invalid,");
                }
                Boolean isValidModel = false;
                Array meterModelvalues = Enum.GetValues(typeof(MeterModels));

                foreach (MeterModels val in meterModelvalues)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterModel.ToUpper())
                    {
                        isValidModel = true;
                    }
                }
                if (!isValidModel)
                {
                    errorMessage.Append("Meter Model is invalid,");
                }
                //if (new Regex("((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))(?![\\d])", RegexOptions.Compiled).Match(imeiNumber).Success)
                //{
                //    meterMasterEntity.MeterGPRSModemIMEI = imeiNumber;
                //}
                //else
                //{
                //    errorMessage.Append("GPRS Modem IP number is invalid/missing,");
                //}
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                // end of GPRS specific Information
                # endregion
            }
            else if (commType.ToUpper() == CommunicationType.TCP.ToString())
            {
                # region TCP Specific Validation
                // added to support GPRS communication
                string imeiNumber = consumerRow[5].ToString().Trim();
                if (string.IsNullOrEmpty(meterId))
                {
                    errorMessage.Append("Meter Id is invalid,");
                }
                if (!new Regex("^[A-Za-z0-9]{7,16}$", RegexOptions.Compiled).Match(meterId).Success)
                {
                    errorMessage.Append("Meter Id can only contain alphanumeric characters and length should be greater than 7 and less than 16,");
                }
                //if (!new Regex("^[0-9]{10}$", RegexOptions.Compiled).Match(simNumber).Success)
                //{
                //    errorMessage.Append("SIM number is invalid,");
                //}
                Boolean isValidType = false;
                Array meterModelTypes = Enum.GetValues(typeof(MeterTypes));
                foreach (MeterTypes val in meterModelTypes)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterType.ToUpper())
                    {
                        isValidType = true;
                    }
                }
                if (!isValidType)
                {
                    errorMessage.Append("Meter Type is invalid,");
                }
                Boolean isValidModel = false;
                Array meterModelvalues = Enum.GetValues(typeof(MeterModels));

                foreach (MeterModels val in meterModelvalues)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterModel.ToUpper())
                    {
                        isValidModel = true;
                    }
                }
                if (!isValidModel)
                {
                    errorMessage.Append("Meter Model is invalid,");
                }
                //if (new Regex("((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))(?![\\d])", RegexOptions.Compiled).Match(imeiNumber).Success)
                //{
                //    meterMasterEntity.MeterGPRSModemIMEI = imeiNumber;
                //}
                //else
                //{
                //    errorMessage.Append("GPRS Modem IP number is invalid/missing,");
                //}
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                // end of GPRS specific Information
                # endregion
            }
            else if (commType.ToUpper() == CommunicationType.GPRS.ToString())
            {
                # region GPRS Specific Validation
                // added to support GPRS communication
                string imeiNumber = consumerRow[5].ToString().Trim();
                if (string.IsNullOrEmpty(meterId))
                {
                    errorMessage.Append("Meter Id is invalid,");
                }
                if (!new Regex("^[A-Za-z0-9]{7,16}$", RegexOptions.Compiled).Match(meterId).Success)
                {
                    errorMessage.Append("Meter Id can only contain alphanumeric characters and length should be greater than 7 and less than 16,");
                }
                if (!new Regex("^[0-9]{10}$", RegexOptions.Compiled).Match(simNumber).Success)
                {
                    errorMessage.Append("SIM number is invalid,");
                }
                Boolean isValidType = false;
                Array meterModelTypes = Enum.GetValues(typeof(MeterTypes));
                foreach (MeterTypes val in meterModelTypes)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterType.ToUpper())
                    {
                        isValidType = true;
                    }
                }
                if (!isValidType)
                {
                    errorMessage.Append("Meter Type is invalid,");
                }
                Boolean isValidModel = false;
                Array meterModelvalues = Enum.GetValues(typeof(MeterModels));

                foreach (MeterModels val in meterModelvalues)
                {
                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterModel.ToUpper())
                    {
                        isValidModel = true;
                    }
                }
                if (!isValidModel)
                {
                    errorMessage.Append("Meter Model is invalid,");
                }
                if (new Regex("^[0-9]{15}$", RegexOptions.Compiled).Match(imeiNumber).Success)
                {
                    meterMasterEntity.MeterGPRSModemIMEI = imeiNumber;
                }
                else
                {
                    errorMessage.Append("GPRS Modem IMEI number is invalid/missing,");
                }
                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                // end of GPRS specific Information
                # endregion
            }
            else
            {
                if (commType.ToUpper() != CommunicationType.DIRECT.ToString())
                {
                    if (simNumber == string.Empty)
                    {
                        if (errorMessage.Length == 0)
                        {
                            errorMessage.Append("Meter SIM Number field is blank");
                        }
                        else
                        {
                            errorMessage.Append(" ,Meter SIM Number field is blank");
                        }
                    }
                    else if (!long.TryParse(simNumber, out intSimNumber))
                    {
                        if (errorMessage.Length == 0)
                        {
                            errorMessage.Append("Invalid SIM Number,");
                        }
                        else
                        {
                            errorMessage.Append(" ,Invalid SIM Number,");
                        }
                    }
                    else if (simNumber.Length != 10)
                    {
                        if (errorMessage.Length == 0)
                        {
                            errorMessage.Append("Meter SIM number should be 10 digits long");
                        }
                        else
                        {
                            errorMessage.Append(" ,Meter SIM number should be 10 digits long");
                        }
                    }
                }

                if (meterType.ToUpper() != "3P-3W" && meterType.ToUpper() != "3P-4W" && meterType.ToUpper() != "1P-2W" && commType.ToUpper() != CommunicationType.DIRECT.ToString())
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Meter Type");
                    }
                    else
                    {
                        errorMessage.Append(" ,Invalid Meter Type");
                    }
                }

                if (meterModel.ToUpper() != NamePlateConstants.PumaHTE650 && meterModel.ToUpper() != NamePlateConstants.PumaLTE650 && meterModel.ToUpper() != NamePlateConstants.RubyE250
                    && meterModel.ToUpper() != NamePlateConstants.Ruby6Val && meterModel.ToUpper() != NamePlateConstants.E350Val && meterModel.ToUpper() != NamePlateConstants.E150Val
                    && meterModel.ToUpper() != NamePlateConstants.LTCTCortex && meterModel.ToUpper() != NamePlateConstants.HTCTCortex && meterModel.ToUpper() != NamePlateConstants.WBVal 
                    && meterModel.ToUpper() != NamePlateConstants.PumaHTE650MW && commType.ToUpper() != CommunicationType.DIRECT.ToString())
                if (meterModel.ToUpper() != NamePlateConstants.PumaHTE650 && meterModel.ToUpper() != NamePlateConstants.PumaLTE650
                    && meterModel.ToUpper() != NamePlateConstants.RubyE250 && meterModel.ToUpper() != NamePlateConstants.Ruby6Val
                    && meterModel.ToUpper() != NamePlateConstants.E350Val && meterModel.ToUpper() != NamePlateConstants.E150Val
                    && meterModel.ToUpper() != NamePlateConstants.LTCTCortex && meterModel.ToUpper() != NamePlateConstants.HTCTCortex
                    && meterModel.ToUpper() != NamePlateConstants.WBVal && meterModel.ToUpper() != NamePlateConstants.Sapphire
                    && meterModel.ToUpper() != NamePlateConstants.TwoTOUltModel && meterModel.ToUpper() != NamePlateConstants.Ruby6ukModel
                    && meterModel.ToUpper() != NamePlateConstants.WBLTVal && meterModel.ToUpper() != NamePlateConstants.TNModel 
                    && commType.ToUpper() != CommunicationType.DIRECT.ToString())
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Meter Model");
                    }
                    else
                    {
                        errorMessage.Append(" ,Invalid Meter Model");
                    }
                }

                if (meterId.Length < 6 || meterId.Length > 16)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid MeterId");
                    }
                    else
                    {
                        errorMessage.Append(" ,Invalid MeterId");
                    }


                }
                if (commType.ToUpper() != "GSM" && commType.ToUpper() != "PSTN" && commType.ToUpper() != CommunicationType.DIRECT.ToString())
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Communication Type");
                    }
                    else
                    {
                        errorMessage.Append(" ,Invalid Communication Type");
                    }
                }
                if (ValidateMeterNumber(meterId, dsMeterIdSimNumber))
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Meter Id already available");
                    }
                    else
                    {
                        errorMessage.Append(" ,Meter Id already available");
                    }
                }
                string meterNo = ValidateSimNumber(simNumber, dsMeterIdSimNumber);
                if (!string.IsNullOrEmpty(meterNo) && meterNo == meterId)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("SIM - " + simNumber + " already associated with meter id - " + meterNo);
                    }
                    else
                    {
                        errorMessage.Append(" ,SIM - " + simNumber + " already associated with meter id - " + meterNo);
                    }
                }
            }
           
            return errorMessage.ToString();
        }
        /// <summary>
        /// checks if new phone number , doesnt previously exists in the database.
        /// </summary>
        /// <param name="simNumber"></param>
        /// <param name="dsMeterIdSimNumberGSM"></param>
        /// <returns></returns>
        private string ValidateSimNumber(string simNumber, DataSet dsMeterIdSimNumber)
        {
            string meterNumber = string.Empty;
            foreach (DataRow row in dsMeterIdSimNumber.Tables[0].Rows)
            {
                if (row["Meter_Phone"].ToString() == String.Concat("0", simNumber))
                {
                    meterNumber = row["Meter_ID"].ToString();
                    break;
                }
            }
            return meterNumber;
        }
        /// <summary>
        /// checks if new meter number , doesnt previously exists in the database.
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="dsMeterIdSimNumberGSM"></param>
        /// <returns></returns>
        private bool ValidateMeterNumber(string meterId, DataSet dsMeterIdSimNumber)
        {
            bool isMeterIDAvaialble = false;
            foreach (DataRow row in dsMeterIdSimNumber.Tables[0].Rows)
            {
                if (row["Meter_ID"].ToString() == meterId)
                {
                    isMeterIDAvaialble = true;
                    break;
                }
            }
            return isMeterIDAvaialble;
        }
        /// <summary>
        /// validate region data
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="regionData"></param>
        /// <returns></returns>
        private RegionEntity ValidateRegionData(string regionName, DataSet regionData)
        {
            RegionEntity regionEntity = new RegionEntity();
            foreach (DataRow row in regionData.Tables[0].Rows)
            {
                if (row["Region_Name"].ToString() == regionName)
                {
                    regionEntity.RegionName = row["Region_Name"].ToString();
                    regionEntity.RegionID = Convert.ToInt32(row["Region_ID"]);
                    break;
                }
            }
            return regionEntity;

        }
        /// <summary>
        /// validate circle data
        /// </summary>
        /// <param name="circleName"></param>
        /// <param name="circleData"></param>
        /// <returns></returns>
        private CircleEntity ValidateCircleData(string circleName, DataSet circleData)
        {
            CircleEntity circleEntity = new CircleEntity();
            foreach (DataRow row in circleData.Tables[0].Rows)
            {
                if (row["Circle_Name"].ToString() == circleName)
                {
                    circleEntity.CircleID = Convert.ToInt32(row["Circle_ID"]);
                    circleEntity.CircleName = row["Circle_Name"].ToString();
                    break;
                }
            }
            return circleEntity;

        }
        /// <summary>
        /// validate division data
        /// </summary>
        /// <param name="divisionName"></param>
        /// <param name="divisionData"></param>
        /// <returns></returns>
        private DivisionEntity ValidateDivisionData(string divisionName, DataSet divisionData)
        {
            DivisionEntity divisionEntity = new DivisionEntity();
            foreach (DataRow row in divisionData.Tables[0].Rows)
            {
                if (row["Division_Name"].ToString() == divisionName)
                {
                    divisionEntity.DivisionName = row["Division_Name"].ToString();
                    divisionEntity.DivisionID = Convert.ToInt32(row["Division_ID"]);
                    break;
                }
            }
            return divisionEntity;

        }

        /// <summary>
        /// validate area data
        /// </summary>
        /// <param name="regionID"></param>
        /// <param name="circleID"></param>
        /// <param name="divisionID"></param>
        /// <param name="areaData"></param>
        /// <returns></returns>
        private AreaEntity ValidateAreaData(string regionID, string circleID, string divisionID, DataSet areaData)
        {
            AreaEntity areaEntity = new AreaEntity();

            foreach (DataRow row in areaData.Tables[0].Rows)
            {
                if (row["Region_ID"].ToString() == regionID && row["Circle_ID"].ToString() == circleID && row["Divsion_ID"].ToString() == divisionID)
                {
                    areaEntity.Area_ID = Convert.ToInt32(row["Area_ID"]);
                    areaEntity.Circle_ID = Convert.ToInt32(row["Circle_ID"]);
                    areaEntity.Region_ID = Convert.ToInt32(row["Region_ID"]);
                    areaEntity.Divsion_ID = Convert.ToInt32(row["Divsion_ID"]);
                    break;
                }
            }
            return areaEntity;

        }

        /// <summary>
        /// Adds GPRS endpoints to M2M database
        /// </summary>
        /// <param name="dataSet"></param>
        private void SyncGPRSOnAdd(DataSet dataSet)
        {
            // to enable syncing of endpoints changes to GPRS Adapter
            LandisGyr.AMI.Layers.Endpoint[] endpoints;
            bool isIMEIAlreadyExists = false;

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                if (dataRow["CommType"].ToString() == CommunicationType.GPRS.ToString())
                {
                    endpoints = new LandisGyr.AMI.Layers.Endpoint[1];
                    meterMasterEntity.MeterGPRSModemIMEI = dataRow["GPRS Modem IMEI"].ToString();
                    meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;
                    meterMasterEntity.Meter_ID = dataRow["MeterId"].ToString();
                    meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;
                    // to sync GPRS Adapter layer with the endpoint
                    endpoints[0] = new LandisGyr.AMI.Layers.Endpoint();
                    endpoints[0].Model = new LandisGyr.AMI.Layers.EndpointModel();
                    endpoints[0].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                    endpoints[0].SerialNumber = meterMasterEntity.MeterGPRSModemIMEI;
                    endpoints[0].IsUsable = true;
                    endpoints[0].AssociatedMeters = new LandisGyr.AMI.Layers.Meter[1];
                    endpoints[0].AssociatedMeters[0] = new LandisGyr.AMI.Layers.Meter();
                    endpoints[0].AssociatedMeters[0].MeterNumber = meterMasterEntity.Meter_ID;
                    endpoints[0].AssociatedMeters[0].Model = new LandisGyr.AMI.Layers.MeterModel();
                    endpoints[0].AssociatedMeters[0].Model.Type = LandisGyr.AMI.Layers.MeterType.Electric;
                    isIMEIAlreadyExists = metermasterBLL.IsIMEIAlreadyExists(meterMasterEntity.MeterGPRSModemIMEI);
                    if (isIMEIAlreadyExists)
                    {
                        GPRSCommunication.EndPointOperations.UpdateEndpoints(endpoints);
                    }
                    else
                    {
                        GPRSCommunication.EndPointOperations.AddEndpoints(endpoints);
                    }
                }
                else
                {
                    meterMasterEntity.MeterGPRSModemIMEI = null;
                    meterMasterEntity.GPRSModemIpType = null;
                    meterMasterEntity.GPRSModemConnectionType = null;
                }
            }
        }

        /// <summary>
        /// VBM - Inserts Excel data into database.
        /// </summary>
        private void InsertConsumerMeterData()
        {
            ConsumerMasterBLL consumerMasterBLL = new ConsumerMasterBLL();
            ConsumerMeterBLL consumerMeterBLL = new ConsumerMeterBLL();
            MeterMasterBLL meterMasterBLL = new MeterMasterBLL();
            RegionBLL regionBLL = new RegionBLL();
            CircleBLL circleBLL = new CircleBLL();
            DivisionBLL divisionBLL = new DivisionBLL();
            AreaBLL areaBLL = new AreaBLL();
            AreaMeterBLL areaMeterBLL = new AreaMeterBLL();
            StringBuilder errorMessage = new StringBuilder();

            DataSet regionData = regionBLL.GetRegionData();
            DataSet circleData = circleBLL.GetCircleData();
            DataSet divisionData = divisionBLL.GetDivisionData();
            DataSet areaData = areaBLL.GetAreaData();

            List<IEntity> areaMeterEntities = new List<IEntity>();
            List<IEntity> consumerMeterEntities = new List<IEntity>();
            List<IEntity> meterMasterEntities = new List<IEntity>();
            List<IEntity> consumerMasterEntities = new List<IEntity>();
            try
            {
                foreach (ConsumerImportEntity entity in entities)
                {
                    RegionEntity regionEntity = entity.Region as RegionEntity;
                    CircleEntity circleEntity = entity.Circle as CircleEntity;
                    DivisionEntity divisionEntity = entity.Division as DivisionEntity;
                    AreaEntity areaEntity = new AreaEntity();
                    AreaMeterEntity areaMeterEntity = new AreaMeterEntity();
                    #region ValidateInsert Region
                    //Validate default value of region whether present in database or not.
                    //regionEntity = regionBLL.ValidateRegion(regionEntity.RegionName) as RegionEntity;
                    //if not present then insert the default value
                    regionEntity = ValidateRegionData(regionEntity.RegionName, regionData);
                    if (regionEntity.RegionName == null)
                    {
                        regionData = regionBLL.GetRegionData();
                        regionBLL.InsertData(entity.Region);
                        //Reload Inserted value in Entity
                        regionEntity = regionBLL.ValidateRegion(entity.Region.RegionName) as RegionEntity;
                    }

                    #endregion
                    #region ValidateInsert Circle

                    //Validate default value of circle whether present in database or not.
                    circleEntity = ValidateCircleData(circleEntity.CircleName, circleData);
                    //if not present then insert the default value

                    if (circleEntity.CircleName == null)
                    {
                        circleData = circleBLL.GetCircleData();
                        circleEntity.RegionID = regionEntity.RegionID;
                        entity.Circle.RegionID = regionEntity.RegionID;
                        circleBLL.InsertData(entity.Circle);
                        //Reload Inserted value in Entity
                        circleEntity = circleBLL.ValidateCircle(entity.Circle.CircleName) as CircleEntity;
                    }
                    #endregion
                    #region ValidateInsert Division
                    //Validate default value of circle whether present in database or not
                    divisionEntity = ValidateDivisionData(divisionEntity.DivisionName, divisionData);
                    //if not present then insert the default value
                    if (divisionEntity.DivisionName == null)
                    {
                        divisionData = divisionBLL.GetDivisionData();
                        divisionEntity.RegionID = regionEntity.RegionID;
                        divisionEntity.CircleID = circleEntity.CircleID;
                        entity.Division.RegionID = regionEntity.RegionID;
                        entity.Division.CircleID = circleEntity.CircleID;
                        divisionBLL.InsertData(entity.Division);
                        //Reload Inserted value in Entity
                        divisionEntity = divisionBLL.ValidateDivision(entity.Division.DivisionName) as DivisionEntity;
                    }
                    #endregion
                    #region ValidateInsert Area
                    //areaEntity.Circle_ID = circleEntity.CircleID;
                    //areaEntity.Region_ID = regionEntity.RegionID;
                    //areaEntity.Divsion_ID = divisionEntity.DivisionID;
                    areaEntity = ValidateAreaData(regionEntity.RegionID.ToString(), circleEntity.CircleID.ToString(), divisionEntity.DivisionID.ToString(), areaData);
                    if (areaEntity.Area_ID == 0)
                    {

                        areaEntity.Circle_ID = circleEntity.CircleID;
                        areaEntity.Region_ID = regionEntity.RegionID;
                        areaEntity.Divsion_ID = divisionEntity.DivisionID;

                        areaBLL.InsertData(areaEntity);
                        areaEntity = areaBLL.ValidateData(divisionEntity.DivisionID, circleEntity.CircleID, regionEntity.RegionID) as AreaEntity;
                        areaData = areaBLL.GetAreaData();
                    }


                    #endregion
                    ConsumerMasterEntity consumerMasterEntity = entity.ConsumerMaster as ConsumerMasterEntity;
                    MeterMasterEntity meterMasterEntity = entity.MeterMaster as MeterMasterEntity;
                    ConsumerMeterEntity consumerMeterEntity = entity.ConsumerMeter as ConsumerMeterEntity;
                    // the line below is used no-where ,hence commented.
                    // bool innerFlag = consumerMasterBLL.ValidateConsumerNumber(consumerMasterEntity);
                    consumerMeterEntity.Consumer_Number = consumerMasterEntity.Consumer_Number;
                    consumerMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    //Insert data to areameter_master table
                    //areaMeterEntity.Area_ID = areaEntity.Area_ID;
                    //areaMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    //areaMeterBLL.InsertData(areaMeterEntity);
                    areaMeterEntity.Area_ID = areaEntity.Area_ID;
                    areaMeterEntity.Meter_ID = meterMasterEntity.Meter_ID;
                    areaMeterEntities.Add(areaMeterEntity);
                    //Insert data to consumermeter table
                    consumerMeterEntity.Region_ID = regionEntity.RegionID;
                    consumerMeterEntity.Circle_ID = circleEntity.CircleID;
                    consumerMeterEntity.Division_ID = divisionEntity.DivisionID;
                    //Insert Data to metermaster table
                    // consumerMasterBLL.InsertData(consumerMasterEntity);
                    meterMasterEntities.Add(meterMasterEntity);
                    consumerMasterEntities.Add(consumerMasterEntity);
                    consumerMeterEntities.Add(consumerMeterEntity);
                    // meterMasterBLL.InsertData(meterMasterEntity);
                    // consumerMeterBLL.InsertData(consumerMeterEntity, true);
                }
                // bulk insertion in database
                areaMeterBLL.BatchInsert(areaMeterEntities);
                consumerMasterBLL.BatchInsert(consumerMasterEntities);
                meterMasterBLL.BatchInsert(meterMasterEntities);
                consumerMeterBLL.BatchInsert(consumerMeterEntities, true);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertConsumerMeterData()", ex);
            }
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
                    System.Windows.Forms.Application.DoEvents();
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
                        if (lineData.IndexOf("Consumer ID") < 0 || lineData.IndexOf("Meter ID") < 0)
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
                        RegionEntity regionEntity = new RegionEntity();
                        CircleEntity circleEntity = new CircleEntity();
                        DivisionEntity divisionEntity = new DivisionEntity();
                        string[] columnData = lineData.Split(',');
                        for (int counter = 0; counter < columnNames.Length; counter++)
                        {
                            // Commented the below lines to solve bug DLMS_0094.

                            //for (int i = 0; i < columnNames.Length; i++)
                            //{
                            //    if (columnNames[counter].Trim().Equals("Meter ID"))
                            //    {
                            //        if(columnData[counter] == columnData[i])
                            //            return false;
                            //    }
                            //    if (columnNames[counter].Trim().Equals("Meter Sim Number"))
                            //    {
                            //        if (columnData[counter] == columnData[i])
                            //            return false;
                            //    }
                            //}
                            if (columnNames[counter].Trim().Equals("Consumer ID"))
                                consumerMasterEntity.Consumer_Number = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer Name"))
                                consumerMasterEntity.Consumer_Name = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Consumer Type ID"))
                                consumerMasterEntity.ConsumerType_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Consumer Phone"))
                                consumerMasterEntity.Consumer_Phone = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("House Number"))
                                consumerMasterEntity.Consumer_HNumber = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Street"))
                                consumerMasterEntity.Consumer_Street = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("City"))
                                consumerMasterEntity.Consumer_City = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Email"))
                                consumerMasterEntity.Consumer_Email = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter ID"))
                                meterMasterEntity.Meter_ID = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter Type ID"))
                                meterMasterEntity.MeterType_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter Model ID"))
                                meterMasterEntity.MeterModel_ID = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter Location"))
                                consumerMeterEntity.Meter_Location = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter Allocation Date"))
                                consumerMeterEntity.Meter_AllocationDate = ConvertToLong(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("EMF"))
                                meterMasterEntity.Meter_EMF = ConvertToDecimal(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Contract Demand"))
                                meterMasterEntity.Meter_ContractDemand = ConvertToDouble(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Internal CT Ratio"))
                                meterMasterEntity.Meter_CTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Internal PT Ratio"))
                                meterMasterEntity.Meter_PTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Meter Unit ID"))
                                meterMasterEntity.MeterUnit_ID = ConvertToInt(columnData[counter]);

                            // following three properties added to resolve bug 73549; 12th April 2012 
                            else if (columnNames[counter].Trim().Equals("Region Name"))
                                regionEntity.RegionName = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Circle Name"))
                                circleEntity.CircleName = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Division Name"))
                                divisionEntity.DivisionName = columnData[counter];


                            else if (columnNames[counter].Trim().Equals("Installed CT Primary"))
                                meterMasterEntity.Meter_InstalledCTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Installed CT Secondary"))
                                meterMasterEntity.Meter_InstalledCTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Installed PT Primary"))
                                meterMasterEntity.Meter_InstalledPTPrimary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Installed PT Secondary"))
                                meterMasterEntity.Meter_InstalledPTSecondary = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Communication Type"))
                                consumerMeterEntity.Communcation_Type = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter Sim Number"))
                                meterMasterEntity.Meter_Phone = columnData[counter];
                            else if (columnNames[counter].Trim().Equals("Meter Status"))
                                meterMasterEntity.Meter_Status = ConvertToInt(columnData[counter]);
                            else if (columnNames[counter].Trim().Equals("Consumer Status"))
                                consumerMeterEntity.Status = ConvertToInt(columnData[counter]);
                        }

                        // following code for region ID, circle ID and division ID added to resolve bug 73549; 12th April 2012 
                        RegionBLL regionBLL = new RegionBLL();
                        ////regionEntity=regionBLL.ValidateRegion(regionEntity.RegionName) as RegionEntity;
                        //regionEntity = new RegionEntity();
                        //consumerMeterEntity.Region_ID = regionEntity.RegionID;
                        //CircleBLL circleBLL = new CircleBLL();
                        //circleEntity = circleBLL.ValidateCircle(circleEntity.CircleName) as CircleEntity;
                        //consumerMeterEntity.Circle_ID = circleEntity.CircleID;
                        //DivisionBLL divisionBLL = new DivisionBLL();
                        //divisionEntity = divisionBLL.ValidateDivision(divisionEntity.DivisionName) as DivisionEntity;
                        //consumerMeterEntity.Division_ID = divisionEntity.DivisionID;

                        entity.ConsumerMaster = consumerMasterEntity;
                        entity.MeterMaster = meterMasterEntity;
                        entity.ConsumerMeter = consumerMeterEntity;
                        entity.Region = regionEntity;
                        entity.Circle = circleEntity;
                        entity.Division = divisionEntity;
                        entities.Add(entity);
                    }
                }
                sr.Close();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ImportData(string fileName)", ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Used to find duplicate values of MeterId and Sim number column in Excel.
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        private string CheckForDuplicates(System.Data.DataTable inputTable)
        {
            StringBuilder errorMessage = new StringBuilder();
            ArrayList arrMeterIds = new ArrayList();
            ArrayList arrSimNumbers = new ArrayList();
            // Added for PED for Consumer Number
            ArrayList arrCunsumerNumbers = new ArrayList();

            for (int j = 0; j < inputTable.Rows.Count; j++)
            {
                for (int i = j + 1; i < inputTable.Rows.Count; i++)
                {
                    if (inputTable.Rows[j][0].ToString() == inputTable.Rows[i][0].ToString())
                    {
                        arrMeterIds.Add(inputTable.Rows[j][0].ToString());
                    }
                    if (inputTable.Rows[j][1].ToString() == inputTable.Rows[i][1].ToString())
                    {
                        arrSimNumbers.Add(inputTable.Rows[j][1].ToString());
                    }
                    // Added for PED Consumer Number change to string 
                    if (inputTable.Rows[j][7].ToString() == inputTable.Rows[i][7].ToString())
                    {
                        arrCunsumerNumbers.Add(inputTable.Rows[j][7].ToString());
                    }
                }
            }

            arrSimNumbers = new ArrayList(arrSimNumbers.ToArray().Distinct().ToArray());
            arrMeterIds = new ArrayList(arrMeterIds.ToArray().Distinct().ToArray());
            arrCunsumerNumbers = new ArrayList(arrCunsumerNumbers.ToArray().Distinct().ToArray());

            if (arrMeterIds.Count > 0)
            {
                string meterId = string.Empty;
                foreach (string meterIds in arrMeterIds)
                {
                    meterId += meterId == string.Empty ? meterIds : "," + meterIds;
                }
                errorMessage.Append("File has duplicate meter id(s) - " + meterId + "\n");
            }
            if (arrSimNumbers.Count > 0 && arrSimNumbers[0] != string.Empty)
            {
                string simNumber = string.Empty;
                foreach (string simNumbers in arrSimNumbers)
                {
                    simNumber += simNumber == string.Empty ? simNumbers : "," + simNumbers;
                }
                errorMessage.Append("File has duplicate sim number(s) - " + simNumber + "\n");
            }
            if (arrCunsumerNumbers.Count > 0)
            {
                string consumerid = string.Empty;
                foreach (string consumerids in arrCunsumerNumbers)
                {
                    consumerid += consumerid == string.Empty ? consumerids : "," + consumerids;
                }
                errorMessage.Append("File has duplicate consumer id(s) - " + consumerid + "\n");
            }

            return errorMessage.ToString();
        }


        /// <summary>
        /// Validate the parameters passed
        /// </summary>
        /// <param name="meterId"></param>
        /// <param name="meterType"></param>
        /// <param name="meterModel"></param>
        /// <param name="simNumber"></param>
        /// <param name="CommunicationType"></param>
        /// <param name="IMEI"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string ValidateData(string meterId, string meterType, string meterModel, string simNumber, string communicationType, string IMEI, DataSet ds)
        {
            long intSimNumber;
            StringBuilder errorMessage = new StringBuilder();
            MeterMasterEntity meterMasterEntity = new MeterMasterEntity();
            meterMasterEntity.Meter_ID = meterId;

            // these are non gprs specific validaition
            if (communicationType.ToUpper() != CommunicationType.GPRS.ToString())
            {
                if (simNumber == string.Empty)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Meter SIM Number field is blank");
                    }
                    else
                    {
                        errorMessage.Append(",Meter SIM Number field is blank");
                    }
                }
                else if (!long.TryParse(simNumber, out intSimNumber))
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid SIM Number,");
                    }
                    else
                    {
                        errorMessage.Append(",Invalid SIM Number,");
                    }
                }
                else if (simNumber.Length != 10)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Meter SIM number should be 10 digits long");
                    }
                    else
                    {
                        errorMessage.Append(",Meter SIM number should be 10 digits long");
                    }
                }

                if (meterType.ToUpper() != "3P-3W" && meterType.ToUpper() != "3P-4W")
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Meter Type");
                    }
                    else
                    {
                        errorMessage.Append(",Invalid Meter Type");
                    }
                }

                if (meterModel.ToUpper() != NamePlateConstants.PumaHTE650 && meterModel.ToUpper() != NamePlateConstants.PumaLTE650 && meterModel.ToUpper() != NamePlateConstants.RubyE250
                    && meterModel.ToUpper() != NamePlateConstants.Ruby6Val)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Meter Model");
                    }
                    else
                    {
                        errorMessage.Append(",Invalid Meter Model");
                    }
                }

                if (meterId.Length < 7 || meterId.Length > 16)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid MeterId");
                    }
                    else
                    {
                        errorMessage.Append(",Invalid MeterId");
                    }


                }
                if (communicationType.ToUpper() != "GSM" && communicationType.ToUpper() != "PSTN")
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Invalid Communication Type");
                    }
                    else
                    {
                        errorMessage.Append(",Invalid Communication Type");
                    }
                }
                if (metermasterBLL.ValidateMeterNumber(meterMasterEntity))
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("Meter Id already available");
                    }
                    else
                    {
                        errorMessage.Append(",Meter Id already available");
                    }
                }
                string meterNo = metermasterBLL.GetMeterNumber(Int64.Parse("0" + simNumber));
                if (!string.IsNullOrEmpty(meterNo) && meterNo != meterId)
                {
                    if (errorMessage.Length == 0)
                    {
                        errorMessage.Append("SIM - " + simNumber + " already associated with meter id - " + meterNo);
                    }
                    else
                    {
                        errorMessage.Append(",SIM - " + simNumber + " already associated with meter id - " + meterNo);
                    }
                }
            }

            # region GPRS Specific Validation

            else if (UtilityDetails.ShowGPRSCommunication && communicationType.ToUpper() == CommunicationType.GPRS.ToString())
            {
                // added to support GPRS communication
                if (string.IsNullOrEmpty(meterId))
                {
                    errorMessage.Append("Meter Id is invalid,");

                }


                if (!new Regex("^[A-Za-z0-9]{7,16}$", RegexOptions.Compiled).Match(meterId).Success)
                {
                    errorMessage.Append("Meter Id can only contain alphanumeric characters and length should be greater than 7 and less than 16,");


                }

                if (!new Regex("^[0-9]{10}$", RegexOptions.Compiled).Match(simNumber).Success)
                {
                    errorMessage.Append("SIM number is invalid,");


                }

                Boolean isValidType = false;
                Array meterModelTypes = Enum.GetValues(typeof(MeterTypes));

                foreach (MeterTypes val in meterModelTypes)
                {

                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterType.ToUpper())
                    {
                        isValidType = true;
                        //  meterMasterEntity.MeterType_ID = Convert.ToInt32(val);
                    }
                }

                if (!isValidType)
                {
                    errorMessage.Append("Meter Type is invalid,");

                }


                Boolean isValidModel = false;
                Array meterModelvalues = Enum.GetValues(typeof(MeterModels));

                foreach (MeterModels val in meterModelvalues)
                {

                    if (!string.IsNullOrEmpty(val.GetDisplayName()) && val.GetDisplayName().ToUpper() == meterModel.ToUpper())
                    {
                        isValidModel = true;
                        //   meterMasterEntity.MeterModel_ID = Convert.ToInt32(val);
                    }
                }

                if (!isValidModel)
                {
                    errorMessage.Append("Meter Model is invalid,");

                }


                if (new Regex("^[0-9]{15}$", RegexOptions.Compiled).Match(IMEI).Success)
                {
                    meterMasterEntity.MeterGPRSModemIMEI = IMEI;

                }
                else
                {
                    errorMessage.Append("GPRS Modem IMEI number is invalid,");

                }


                meterMasterEntity.GPRSModemIpType = GPRSIPType.Dynamic;


                meterMasterEntity.GPRSModemConnectionType = GPRSConnectionMode.AlwaysOn;

                // DataTable dataTable = ds.Tables[0].Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

                var groupMeterId = (from table in ds.Tables[0].AsEnumerable()
                                    group table by
                                    table[ds.Tables[0].Columns[0].ColumnName] into newTable
                                    where newTable.Count() > 1
                                    select newTable).ToList();

                if (groupMeterId.Count > 0)
                {
                    errorMessage.Append(string.Format("Meter Id {0} have duplicate entries,", groupMeterId[0].Key));

                }

                MeterMasterBLL meterMasterBLL = new MeterMasterBLL();

                if (meterMasterBLL.ValidateMeterNumber(meterMasterEntity))
                {
                    errorMessage.Append("Meter Id is already available,");

                }

                var groupSimNumber = (from table in ds.Tables[0].AsEnumerable()
                                      group table by
                                      table[ds.Tables[0].Columns[1].ColumnName] into newTable
                                      where newTable.Count() > 1
                                      select newTable).ToList();


                if (groupSimNumber.Count > 0)
                {
                    errorMessage.Append(string.Format("Sim Number {0} have duplicate entries,", groupSimNumber[0].Key));

                }


                var groupIMEIId = (from table in ds.Tables[0].AsEnumerable()
                                   where table[ds.Tables[0].Columns[4].ColumnName].ToString().ToUpper() == "GPRS"
                                   group table by
                                   table[ds.Tables[0].Columns[5].ColumnName] into newTable
                                   where newTable.Count() > 1
                                   select newTable).ToList();


                if (groupIMEIId.Count > 0)
                {
                    errorMessage.Append(string.Format("GPRS Modem IMEI number {0} have duplicate entries,", groupIMEIId[0].Key));

                }


                if (meterMasterBLL.IsIMEIAlreadyExists(meterMasterEntity.MeterGPRSModemIMEI))
                {
                    errorMessage.Append("GPRS Modem IMEI is already associated with some other meter,");

                }


                // end of GPRS specific Information

            }
            # endregion

            return errorMessage.ToString().TrimEnd(',');
        }
        /// <summary>
        /// Get MeterModelId from Metermodel
        /// </summary>
        /// <param name="meterModel"></param>
        /// <param name="meterId"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        private int GetMeterModelId(string meterModel, string meterId, string commType)
        {
            int meterModelId = 0;
            if (meterModel.ToUpper() == NamePlateConstants.PumaLTE650.ToUpper())
                meterModelId = NamePlateConstants.PumaLTE650Value;
            else if (meterModel.ToUpper() == NamePlateConstants.RubyE250.ToUpper())
                meterModelId = NamePlateConstants.RubyE250Value;
            else if (meterModel.ToUpper() == NamePlateConstants.PumaHTE650.ToUpper())
                meterModelId = NamePlateConstants.PumaHTE650Value;
            else if (meterModel.ToUpper() == NamePlateConstants.LTCTCortex.ToUpper())
                meterModelId = NamePlateConstants.LTCTCortexValue;
            else if (meterModel.ToUpper() == NamePlateConstants.HTCTCortex.ToUpper())
                meterModelId = NamePlateConstants.HTCTCortexValue;
            else if (meterModel.ToUpper() == NamePlateConstants.Ruby6Val.ToUpper())
                meterModelId = NamePlateConstants.Ruby6Value;
            else if (meterModel.ToUpper() == NamePlateConstants.E350Val.ToUpper())
                meterModelId = NamePlateConstants.RubyE350Value;
            else if (meterModel.ToUpper() == NamePlateConstants.E150Val.ToUpper())
                meterModelId = NamePlateConstants.RubyE150Value;
            else if (meterModel.ToUpper() == NamePlateConstants.SFSPVal.ToUpper())
                meterModelId = NamePlateConstants.SFSP;
            else if (meterModel.ToUpper() == NamePlateConstants.Sapphire.ToUpper())
                meterModelId = NamePlateConstants.SapphireValue;
            else if (meterModel.ToUpper() == NamePlateConstants.Sapphire.ToUpper())
                meterModelId = NamePlateConstants.Sapphire_Netmeter_WCM;
            else if (meterModel.ToUpper() == NamePlateConstants.TNModel.ToUpper())
                meterModelId = NamePlateConstants.TNValue;

            else if (commType.ToUpper() == CommunicationType.DIRECT.ToString() && meterModel == string.Empty)
            {
                meterModelId = NamePlateConstants.RubyE250Value;
            }
            // Below lines are commented because , even if meterModelId is wrong , the meter reads and shows the correct data only.
            //int actualModelId = new DLMS650GeneralBLL().GetMeterModelNoByMeterID(meterId);
            //meterModelId = (actualModelId == 0 || actualModelId == -1) ? meterModelId : actualModelId;
            return meterModelId;
        }


        /// <summary>
        /// Get meter type id from meter type 
        /// </summary>
        /// <param name="meterType"></param>
        /// <param name="meterId"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        private int GetMeterTypeId(string meterType, string meterId, string commType)
        {
            int meterTypeId = 0;
            if (meterType.ToUpper() == "3P-3W")
                meterTypeId = 1;
            else if (meterType.ToUpper() == "3P-4W")
                meterTypeId = 2;
            // actual metertypeId is 5 for single phase, but 3 is primary key in table metertype_master. 
            else if (meterType.ToUpper() == "1P-2W")
                meterTypeId = 3;

            else if (commType.ToUpper() == CommunicationType.DIRECT.ToString() && meterType == string.Empty)
            {
                meterTypeId = 2;
            }
            // Below lines are commented because , even if meterTypeId is wrong , the meter reads and shows the correct data only.
            // int actualMeterType = new DLMS650GeneralBLL().GetMeterTypeByMeterID(meterId);
            //meterTypeId = actualMeterType == 0 ? meterTypeId : actualMeterType;
            return meterTypeId;
        }

        /// <summary>
        /// Get Consumer type id from Consumer type 
        /// </summary>
        /// <param name="meterType"></param>
        /// <param name="meterId"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        private int GetConsumerTypeId(string consumerType)
        {
            
            int consumerTypeId = 0;
            if (consumerType.ToUpperInvariant() == "Feeder".ToUpperInvariant())
                consumerTypeId = 1;
            else if (consumerType.ToUpperInvariant() == "Substation".ToUpperInvariant())
                consumerTypeId = 2;
            else if (consumerType.ToUpperInvariant() == "Industrial".ToUpperInvariant())
                consumerTypeId = 3;
            else if (consumerType.ToUpperInvariant() == "DT".ToUpperInvariant())
                consumerTypeId = 4;
            else if (consumerType.ToUpperInvariant() == "Others".ToUpperInvariant())
                consumerTypeId = 5;
            else if (consumerType.ToUpperInvariant() == "LT Bulk Customer".ToUpperInvariant())
                consumerTypeId = 6;


            return consumerTypeId;
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
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertToDouble(string data)", ex);
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
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertToInt(string data)", ex);
                    return 0;
                }
            }
        }

        private Decimal ConvertToDecimal(string data)
        {
            if (string.IsNullOrEmpty(data))
                return 0;
            else
            {
                try
                {
                    return Convert.ToDecimal(data);
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertToDecimal(string data)", ex);
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
                catch (Exception ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ConvertToLong(string data)", ex);
                    return 0;
                }
            }
        }

        private void ConsumerImport_Load(object sender, EventArgs e)
        {
            this.Text = "Consumer Import";
            this.StatusMessage = string.Empty;
        }

        // gprs changes ,event to handle download sample file 
        private void downloadXlsxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //Fix Defect #218728
                saveFileDialog.CreatePrompt = false;
                saveFileDialog.FileName = "Consumer";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WebClient myWebClient = new WebClient();
                    //Fix Defect 
                    myWebClient.DownloadFile(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Consumer.xls"), saveFileDialog.FileName);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                MessageBox.Show("BCS cannot overwrite to the file. It is already opened exclusively by some other application.", "BCS");
                logger.Log(LOGLEVELS.Error, "downloadXlsxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)", ex);
            }


        }
                
    }
}
