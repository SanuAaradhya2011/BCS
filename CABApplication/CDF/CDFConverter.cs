using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CAB.Channel.Formatter;
using CAB.Entity;
using LNG.Framework.Utility;
using LNG.EntityToMIOS;
using LNG.MIOS.Common;
using LNG.MIOS.Common.Enumerations;
using CAB.UI.Controls;
using log4net;
using CAB.MeterData.Upload;
using Hunt.EPIC.Logging;
using System.Globalization;
using CAB.BLL;
using CAB.Framework;
using System.Collections.Generic;
using System.Linq;
using CAB.IECChannel.Formatter;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Common.EntityMapper;
using System.Xml;
using System.Text;
namespace CAB.UI
{
    /// <summary>
    /// CDF converter form, contains the funcitonality of converting .2NG files to MIOS .xml according to the configuration file.
    /// </summary>
    public partial class CDFConverter : MdiChildForm
    {
        private OpenFileDialog openFileDialog = null;
        private string[] lstFileNames = null;
        private CABSerializer cabSerializer = null;
        private string configurationFilePath = string.Empty;
        private const string ConfigurationFileName = "CFCConfigFile.xml";
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CDFConverter));
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CDFConverter).ToString());
        UploadFile uploadFile = null;
        CFCAPI cfcAPI = null;
        private Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>> readOuts = null;
        /// <summary>
        /// 
        /// </summary>
        public CDFConverter()
        {
            configurationFilePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationFileName;
            cabSerializer = new CABSerializer();
            uploadFile = new UploadFile();
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog sourceFolderBrowseDialog = new FolderBrowserDialog();
            sourceFolderBrowseDialog.SelectedPath = txtSource.Text;
            if (sourceFolderBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.Text = sourceFolderBrowseDialog.SelectedPath;
                cfcAPI.SOURCEPATH = txtSource.Text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseConvDone_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog convDoneBrowseDialog = new FolderBrowserDialog();
            convDoneBrowseDialog.SelectedPath = txtConversionDone.Text;
            if (convDoneBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                txtConversionDone.Text = convDoneBrowseDialog.SelectedPath;
                cfcAPI.DONEPATH = txtConversionDone.Text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseError_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog errorBrowseDialog = new FolderBrowserDialog();
            errorBrowseDialog.SelectedPath = txtError.Text;
            if (errorBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                txtError.Text = errorBrowseDialog.SelectedPath;
                cfcAPI.ERRORPATH = txtError.Text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseDestination_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog destBrowseDialog = new FolderBrowserDialog();
            destBrowseDialog.SelectedPath = txtDestination.Text;
            if (destBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                txtDestination.Text = destBrowseDialog.SelectedPath;
                cfcAPI.DESTPATH = txtDestination.Text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseResult_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = txtResult.Text;
            openFileDialog.DefaultExt = "XML";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Result(*.XML)|*.XML";
            openFileDialog.InitialDirectory = txtResult.Text;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                cfcAPI.RESULTFILE = txtResult.Text = openFileDialog.FileName;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdSpecific_CheckedChanged(object sender, EventArgs e)
        {
            if (rdSpecific.Checked)
            {

                string fileNames = string.Empty;
                openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = string.Concat(AppDomain.CurrentDomain.BaseDirectory, @"CAB Readout\");
                openFileDialog.Multiselect = true;
                openFileDialog.DefaultExt = "2NG";
                openFileDialog.Filter = "Readout(*.2NG)|*.2NG";
                if (UtilityDetails.PrimaryUtlityName == UtilityEntity.Generic.ToString())
                {
                    // Story - 349654 - Single Phase Non DLMS file extension
                    openFileDialog.Filter = "Readout(*.2NG;*.CAB;*.SLG)|*.2NG;*.CAB;*.SLG";
                }

                DialogResult result = openFileDialog.ShowDialog();
                if (cfcAPI != null)
                {
                    cfcAPI.SCOPE.CONVERTFILE = "SPECIFIC";
                    if (result == DialogResult.OK)
                    {
                        string fileExtension = Path.GetExtension(openFileDialog.FileName);
                        if (UtilityDetails.PrimaryUtlityName == UtilityEntity.Generic.ToString())
                        {
                            if (fileExtension.ToUpper() != ".2NG" && fileExtension.ToUpper() != ".CAB" && fileExtension.ToUpper() != ".SLG") // Story - 349654 - Single Phase Non DLMS file extension
                            {
                                MessageBox.Show("Invalid File.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                rdSpecific.Checked = false;
                                rdSpecific.Checked = true;
                            }
                        }
                        else
                        {
                            if (fileExtension != ".2NG")
                            {
                                MessageBox.Show("Invalid File.", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                rdSpecific.Checked = false;
                                rdSpecific.Checked = true;
                            }
                        }
                        cfcAPI.SCOPE.FILENAME = openFileDialog.FileNames;
                    }
                    else
                    {
                        cfcAPI.SCOPE.FILENAME = null;
                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdConvertAll_CheckedChanged(object sender, EventArgs e)
        {

            if (cfcAPI != null)
            {
                cfcAPI.SCOPE.CONVERTFILE = "ALL";
            }
        }
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CDFConverter_Load(object sender, EventArgs e)
        {
            rdConvertAll.Checked = true;
            if (File.Exists(configurationFilePath))
            {
                cfcAPI = (CFCAPI)cabSerializer.DeserializeToObject(configurationFilePath, typeof(CFCAPI));
                if (cfcAPI != null)
                {
                    FillControlsOnLoad();
                }
            }
            else
            {
                this.StatusMessage = "Configuration file not found";
                Application.DoEvents();
            }
        }
        ///BhardwajG
        /// <summary>
        /// Fill controls on load
        /// </summary>
        private void FillControlsOnLoad()
        {
            txtSource.Text = cfcAPI.SOURCEPATH;
            txtError.Text = cfcAPI.ERRORPATH;
            txtResult.Text = cfcAPI.RESULTFILE;
            txtConversionDone.Text = cfcAPI.DONEPATH;
            txtDestination.Text = cfcAPI.DESTPATH;
        }
        /// <summary>
        /// Cancel click handler, closes the form and empty the status lable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }
        /// <summary>
        /// Get all the files need to be converted according to the configuration file
        /// </summary>
        /// <param name="cfcAPI"></param>
        /// <returns></returns>
        private string[] GetAllFileNames(CFCAPI cfcAPI)
        {
            string[] fileNames = null;
            if (cfcAPI.SCOPE.CONVERTFILE.ToUpper() == "ALL")
            {
                try
                {
                    var lisOfSupportedFileExtension = new List<string> { ".2NG", ".CAB", ".SLG" }; // Story - 349865 - CDF Conversion for SLG files

                    fileNames = Directory.GetFiles(cfcAPI.SOURCEPATH, "*.*", SearchOption.TopDirectoryOnly).Where(s => lisOfSupportedFileExtension.Any(e => s.EndsWith(e))).ToArray();
                }
                catch (Exception ex)    //Exception log for catch block
                {
                    log.Debug("Exception in retrieving files: " + ex.Message);
                    logger.Log(LOGLEVELS.Error, "GetAllFileNames(CFCAPI cfcAPI)", ex);
                }
            }
            else if (cfcAPI.SCOPE.CONVERTFILE.ToUpper() == "SPECIFIC")
            {
                fileNames = cfcAPI.SCOPE.FILENAME;
            }
            return fileNames;
        }
        /// <summary>
        /// Convert click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvert_Click(object sender, EventArgs e)
        {
            this.StatusMessage = "Checking path in configuration ";
            Application.DoEvents();
            CheckPath(cfcAPI);
            this.StatusMessage = "Saving Settings ";
            Application.DoEvents();
            int counter = 0;
            try
            {
                grpCDF.Enabled = false;
                btnConvert.Enabled = false;
                btnCancel.Enabled = false;
                if (rdConvertAll.Checked)
                {
                    cfcAPI.SCOPE.CONVERTFILE = "ALL";
                }
                else
                {
                    cfcAPI.SCOPE.CONVERTFILE = "SPECIFIC";
                }
                string currentDateTime = DateUtility.LongToMIOSStringDateTimeWithSecFormat(DateUtility.DateTimeToLong(DateTime.Now));
                //BhardwajG : Include file name as well to stop the exception.
                cabSerializer.SerializeObjectToFile(AppDomain.CurrentDomain.BaseDirectory, ConfigurationFileName, cfcAPI);
                MIIProtocolEntity miipProtocolEntity = new MIIProtocolEntity();
                miipProtocolEntity.InstanceID = string.Empty;
                miipProtocolEntity.MIIPAdditionalQualifier = MIIProtocolAdditionalQualifier.ConvertToCommonFormatWithAuditTrail;
                miipProtocolEntity.AdditionalQualifier = EnumDescription.GetDescription(MIIProtocolAdditionalQualifier.ConvertToCommonFormatWithAuditTrail);

                lstFileNames = GetAllFileNames(cfcAPI);
                if (lstFileNames != null && lstFileNames.Length > 0)
                {
                    CFCRESULTINSTANCECONVERT[] convert = new CFCRESULTINSTANCECONVERT[lstFileNames.Length];
                    EntityToMIOSConverter entityToMIOS = new EntityToMIOSConverter(cfcAPI, miipProtocolEntity, null, new LNG.API.ProcessMessage.APIProcessMessage());
                    entityToMIOS.OnSayToServerEvent += new LNG.EntityToMIOS.EntityToMIOS.SayToServerEventHandler(entityToMIOS_OnSayToServerEvent);
                    foreach (string fileName in lstFileNames)
                    {
                        this.StatusMessage = "Converting " + fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        Application.DoEvents();
                        ConvertFile(fileName, entityToMIOS, counter, convert);

                        Application.DoEvents();
                        Thread.Sleep(1000);
                        counter++;
                    }
                    this.StatusMessage = "Conversion completed";
                    Application.DoEvents();
                    cabSerializer.SerializeObjectToFile(string.Empty, cfcAPI.RESULTFILE, entityToMIOS.CreateResultFileObject(cfcAPI.INSTANCEID, convert, currentDateTime));
                }
                else
                {
                    this.StatusMessage = "No files to convert";
                    Application.DoEvents();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnConvert_Click(object sender, EventArgs e)", ex);
            }
            finally
            {
                grpCDF.Enabled = true;
                btnConvert.Enabled = true;
                btnCancel.Enabled = true;
                //BhardwajG : forget file names that are converted on the last click. The fix is for specific option.
                cfcAPI.SCOPE.FILENAME = null;
            }
        }
        private bool ConvertFile(string fileName, EntityToMIOSConverter entityToMIOS, int counter, CFCRESULTINSTANCECONVERT[] convert)
        {
            bool success = false;
            string filePath = string.Empty;
            try
            {
                string fileExtension = Path.GetExtension(fileName.ToUpper()); // Story - 349865 - CDF Conversion for SLG files
                List<string> individualFileContent = new List<string>();
                convert[counter] = new CFCRESULTINSTANCECONVERT();
                //BillingGeneralNFDLMSEntity master = null;
                LNG.Entity.BillingGeneralNFDLMSEntity master = null;
                DLMS650FormatterBilling formatterBilling = new DLMS650FormatterBilling();
                //formatterBilling.OnChannelStatusChanged += new CAB.Channel.ReadBase.ChannelStatusChanged(formatterBilling_OnChannelStatusChanged);   
                if (fileExtension == ".2NG")
                {
                    individualFileContent.AddRange(GetContent(fileName).Split('$'));
                }
                else if (fileExtension.ToUpper() == ".CAB" || fileExtension.ToUpper() == ".SLG") // Story - 349865 - CDF Conversion for SLG files
                {
                    individualFileContent.AddRange((uploadFile.GetIECFileContent(fileName)).Split(new string[] { "NP" }, StringSplitOptions.RemoveEmptyEntries));
                    //individualFileContent.Add(uploadFile.GetIECFileContent(fileName));
                }
                foreach (string content in individualFileContent)
                {
                    if (!string.IsNullOrEmpty(content) && content != "\r\n" && fileExtension == ".2NG") //DLMS File
                    {
                        master = uploadFile.GetMasterEntityCAB(content.Substring(27), false);
                    }
                    else if (!string.IsNullOrEmpty(content) && content != "\r\n" && fileExtension.ToUpper() == ".CAB") //IEC File
                    {
                        //master = GetMasterEntityFromCABFile(uploadFile.GetIECFileContent(fileName), fileName);
                        master = GetMasterEntityFromCABFile(content, fileName); // returns CAB.Entity.BillingGeneralNFDLMSEntity
                        //To Stop overwritting of same meter IEC files.
                        Application.DoEvents();
                        Thread.Sleep(1000);
                    }
                    else if (!string.IsNullOrEmpty(content) && content != "\r\n" && fileExtension == ".SLG") // Story - 349865 - CDF Conversion for SLG files
                    {
                        master = GetMasterEntityFromSLGFile(content, fileName);
                        // Story - 354382 - For a day, Time 240000 is coming which would be treated as 000000 for next day for Single phase meter
                        if (master != null && master.MidnightData != null)
                        {
                            for (int counterMidNight = 0; counterMidNight < master.MidnightData.Count; counterMidNight++)
                            {
                                if (!string.IsNullOrEmpty(master.MidnightData[counterMidNight].RealTimeClockDateandTime.ToString()))
                                {
                                    if (master.MidnightData[counterMidNight].RealTimeClockDateandTime.ToString().Substring(8, 2) == "24" && master.MidnightData[counterMidNight].RealTimeClockDateandTime.ToString().Substring(10, 2) == "00" && master.MidnightData[counterMidNight].RealTimeClockDateandTime.ToString().Substring(12, 2) == "00")
                                    {
                                        DateTime realTimeClockDateTime = DateUtility.LongToDateTime(Convert.ToInt64(master.MidnightData[counterMidNight].RealTimeClockDateandTime.ToString()));
                                        master.MidnightData[counterMidNight].RealTimeClockDateandTime = DateUtility.DateTimeToLong(realTimeClockDateTime);
                                    }
                                }

                            }
                        }
                        // Story - 354382 - For a day Time 240000 is coming which would be treated as 000000 for next day for Single phase meter
                        if (master != null && master.LoadSurvey != null)
                        {
                            for (int counterLoadSurvey = 0; counterLoadSurvey < master.LoadSurvey.Count; counterLoadSurvey++)
                            {
                                if (!string.IsNullOrEmpty(master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime.ToString()))
                                {
                                    if (master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime.ToString().Substring(8, 2) == "24" && master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime.ToString().Substring(10, 2) == "00" && master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime.ToString().Substring(12, 2) == "00")
                                    {
                                        DateTime realTimeClockDateTime = DateUtility.LongToDateTime(Convert.ToInt64(master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime.ToString()));
                                        master.LoadSurvey[counterLoadSurvey].RealTimeClockDateandTime = DateUtility.DateTimeToLong(realTimeClockDateTime);
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(master.LoadSurvey[0].RealTimeClockDateandTime.ToString()))
                            {
                                master.LoadSurvey = master.LoadSurvey.OrderBy(l => Convert.ToInt64(l.RealTimeClockDateandTime)).ToList(); // Story - 358810 - To arrange the data in ascending order for single phase file
                            }
                        }
                        //To Stop overwritting of same meter IEC files.
                        Application.DoEvents();
                        Thread.Sleep(1000);
                    }

                    if (master != null && master.MeterData != null && master.General != null)
                    {
                        master.FileType = fileExtension;
                        if (fileExtension == ".2NG")
                        {
                            master.MeterData.ReadingDateTime = DateUtility.DateTimeToLong(DateTime.ParseExact(content.Substring(4, 22).Trim('\r').Trim('\n').Replace('-', '/'), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                        }
                        else if (fileExtension.ToUpper() == ".CAB" || fileExtension.ToUpper() == ".SLG") // Story - 349865 - CDF Conversion for SLG files
                        {   //As CT ratio in case of IEc files is ----- .
                            master.General.InternalCTratio = "1";
                            if (master.LoadSurvey != null && master.LoadSurvey[0].MDIntervalPeriod != 0)
                                master.DemandIntegrationPeriod = master.LoadSurvey[0].MDIntervalPeriod;
                        }
                        convert[counter].READTIME = DateUtility.LongToMIOSStringDateTimeWithSecFormat(master.MeterData.ReadingDateTime);
                        convert[counter].SERIAL = master.General.MeterSerialNumber;
                        // CDF converter specific parameter alignment open*****
                        if (master.General.BasicCurrentRating.ToUpper().Contains("3 X"))
                        master.General.BasicCurrentRating = master.General.CurrentRating;
                        // CDF converter specific parameter alignment close*****
                        filePath = entityToMIOS.ConvertEntityToMIOS(cfcAPI, master);
                        //String newath=cfcAPI.DESTPATH+'/'+filePath;
                       
                        removewhitspaces(cfcAPI.DESTPATH+"/"+filePath);
                        
                       
                        //cabSerializer.SerializeObjectToFile(cfcAPI.DESTPATH, filePath, master);

                        if (filePath == string.Empty)
                        {
                            success = false;
                        }
                        else
                        {
                            success = true;
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                return success;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                this.StatusMessage = "Exception occured while converting " + fileName;
                logger.Log(LOGLEVELS.Error, "Exception occured while converting " + fileName, ex);
                success = false;
                return success;
            }
            finally
            {
                if (success)
                {
                    convert[counter].RESULT = "0";
                    entityToMIOS.CopyFileToFolderAndDelete(fileName, false);
                    this.StatusMessage = fileName.Substring(fileName.LastIndexOf("\\") + 1) + " converted to CDF";
                }
                else
                {
                    convert[counter].RESULT = "1";
                    entityToMIOS.CopyFileToFolderAndDelete(fileName, true);
                    this.StatusMessage = fileName.Substring(fileName.LastIndexOf("\\") + 1) + " not converted to CDF, moved to error folder";
                }
                convert[counter].OUTFILENAME = filePath;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void removewhitspaces(String filename)
        {   XmlWriterSettings settings=new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.None;
            settings.Encoding = Encoding.Default;
            settings.OmitXmlDeclaration = true;
            XmlDocument doc = new XmlDocument();    
            doc.Load(filename);
            doc.PreserveWhitespace = true;
            XmlWriter writer = XmlWriter.Create(filename, settings);
            doc.WriteTo(writer);
            writer.Close();    

        }
        private LNG.Entity.BillingGeneralNFDLMSEntity GetMasterEntityFromCABFile(string fileText, string fileName)
        {
            LNG.Entity.BillingGeneralNFDLMSEntity masterEntity = null;
            Dictionary<string, string> dicOBISandData = new Dictionary<string, string>();// Story - 349865 - CDF Conversion for SLG files
            try
            {
                List<IECBillingGeneralNFEntity> iecMasterEntity = new List<IECBillingGeneralNFEntity>();
                IECMeterDataEntity iecMeterDataEntity = new IECMeterDataEntity();
                FormatterBilling formatterBilling = new FormatterBilling();
                ParseReadOuts(fileText, iecMeterDataEntity, dicOBISandData); // Story - 349865 - CDF Conversion for SLG files
                formatterBilling.GetData(fileText, iecMasterEntity, readOuts);
                if (iecMasterEntity != null && iecMasterEntity.Count > 0)
                {
                    iecMasterEntity[0].listGeneralData[0].MeterData = iecMeterDataEntity;
                    masterEntity = new IECToDLMSMapper(true).ConvertIECEntityToDLMSEntityForCAB(iecMasterEntity[0], false);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Exception occured while converting " + fileName, ex);
            }

            return masterEntity;
        }
        /// <summary>
        /// This function would be used to get the master entiity from SLG file content. Story - 349865
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private LNG.Entity.BillingGeneralNFDLMSEntity GetMasterEntityFromSLGFile(string fileText, string fileName)
        {
            Dictionary<string, string> dicOBISandData = null;
            List<IECBillingGeneralNFEntity> iecMasterEntity = null;
            LNG.Entity.BillingGeneralNFDLMSEntity masterEntity = null;

            IECMeterDataEntity iecMeterdataEntity = null;

            try
            {
                // OBIS code dictionary Creation for single phase data
                iecMasterEntity = new List<IECBillingGeneralNFEntity>();
                iecMeterdataEntity = new IECMeterDataEntity();

                fileText = fileText.Replace("", "");
                fileText = fileText.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                fileText = fileText.Replace(FormatterConstant.NEWLINE, string.Empty);

                string[] arrDataWithOBIS = fileText.Split('/')[3].Split(')');
                dicOBISandData = new Dictionary<string, string>();
                for (int v = 0; v < arrDataWithOBIS.Length - 1; v++)
                {
                    string[] arrData = arrDataWithOBIS[v].Split('(');
                    dicOBISandData[arrData[0]] = arrData[1];
                }
                dicOBISandData["DateTime"] = fileText.Split('/')[2];

                ParseReadOuts(fileText, iecMeterdataEntity, dicOBISandData);

                FormatterBilling formatterBilling = new FormatterBilling();
                formatterBilling.GetDataForSPhase(fileText, iecMasterEntity, readOuts, dicOBISandData);

                if (iecMasterEntity != null && iecMasterEntity.Count > 0)
                {
                    // To calculate Meter Reading Date Time
					//string tmpMtrID = string.Empty;
                    string[] meterArr=fileText.Split('/');
                    string tmpreadingDateTime = string.Empty;

                    if (meterArr.Length > 2)
                    {
                        tmpreadingDateTime = meterArr[2].ToString();
                    }
					else
					{
						tmpreadingDateTime = "0";
					}

                    //tmpMtrID = fileText.Substring(fileText.IndexOf("/") + 1);
                    //tmpMtrID = tmpMtrID.Substring(13, 16);

                    //tmpreadingDateTime = fileText.Substring(fileText.IndexOf(tmpMtrID) + 1 + tmpMtrID.Length);
                    //tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));

                    //string data = FormatterCommon.ParseDataFor1Phase(dicOBISandData, "0.9.1");

                    //string dateAndTime = DateUtility.GetFormatedDateTme(data);
                    //iecMeterdataEntity.ReadingDateTime = Convert.ToInt64(dateAndTime);
                    iecMeterdataEntity.ReadingDateTime = Convert.ToInt64(tmpreadingDateTime);
                    iecMasterEntity[0].listGeneralData[0].MeterData = iecMeterdataEntity;

                    masterEntity = new IECToDLMSMapper(false).ConvertIECEntityToDLMSEntityForSPhaseCAB(iecMasterEntity[0], true);
                    masterEntity.ReadoutCounter = Convert.ToInt64(iecMasterEntity[0].listGeneralData[0].CurrentInstant.ReadoutCounter);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Exception occured while converting " + fileName, ex);
                dicOBISandData = null;
                iecMasterEntity = null;
                iecMeterdataEntity = null;
            }

            finally
            {
                dicOBISandData = null;
                iecMasterEntity = null;
                iecMeterdataEntity = null;
            }
            return masterEntity;
        }
        /// <summary>
        /// Split the File text into the readouts.
        /// Get MeterData ID for each read out.
        /// <History> Signature changed to use the same method for SLG and CAB - Story - 349865</History>
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="mtrDataEntity"></param>
        private void ParseReadOuts(string fileText, IECMeterDataEntity mtrDataEntity, Dictionary<string, string> dicOBISandData)
        {

            readOuts = new Dictionary<ReadOutItem, Collection<ReadOutCounterEntity>>();
            string strSplitTxt = "NP";

            bool isOldFormatFile = false;
            string[] readOutsList = null;
            readOutsList = fileText.Split(new string[] { strSplitTxt }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0;

            for (; i < readOutsList.Length; i++)
            {
                if (readOutsList[i].Trim().Length == 0) continue;
                string tmpMtrID = string.Empty;
                /*GKG 03/04/2013 137643*/
                if (isOldFormatFile)
                {
                    if (i == 0) i++; //to skip the first blank space 
                }
                /*GKG 03/04/2013 137643*/
                string tmpreadingDateTime = string.Empty;
                if (readOutsList[i].ToUpper().Contains("LGC")) // Story - 349865 - This would be called for single phase Non DLMS
                {
                    // To calculate Meter ID
                    tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("/") + 1);
                    tmpMtrID = tmpMtrID.Substring(13, 16);
                    tmpreadingDateTime = "0";
                }
                else
                {
                    // To calculate Meter ID
                    tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("/") + 1);
                    tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);

                    tmpreadingDateTime = readOutsList[i].Substring(readOutsList[i].IndexOf(tmpMtrID) + 1 + tmpMtrID.Length);
                    tmpreadingDateTime = tmpreadingDateTime.Substring(0, tmpreadingDateTime.IndexOf("/"));
                }

                //tmpMtrID = readOutsList[i].Substring(readOutsList[i].IndexOf("/") + 1);
                //tmpMtrID = tmpMtrID.Substring(4, tmpMtrID.IndexOf("/") - 4);

                //Insert meter details specific to this read out in DB and 
                //Get Meter Data ID for this readout.
                mtrDataEntity.MeterID = tmpMtrID.Trim();
                mtrDataEntity.ReadingDateTime = Convert.ToInt64(tmpreadingDateTime);
                mtrDataEntity.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                if (readOutsList[i].ToUpper().Contains("LGC")) // Story - 349865 - This would be called for single phase Non DLMS
                {
                    ParseReadOutItemsForSPhase(readOutsList[i], dicOBISandData);
                }
                else
                {
                    ParseReadOutItems(readOutsList[i]);
                }
            }
        }

        /// <summary>
        /// Parse each readout for Read item tag.
        /// </summary>
        /// <param name="readOut"></param>
        private void ParseReadOutItems(string readOut)
        {
            string data = readOut.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);
            string[] generalData = new string[matches.Count];
            int counter = 0;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateData(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            //Create General Data Mapping
            if (counter > 0 && GeneralData.Length > 0)
                CreateReadOutItemDictionary("RD", readOut);
            //Create Fraud Energy Data Mapping
            if (readOut.Contains("MI"))
                CreateReadOutItemDictionary("MI", readOut);
            //Create Transactions Data Mapping
            if (readOut.Contains("TR"))
                CreateReadOutItemDictionary("TR", readOut);
            //Create RTC Update Data Mapping
            if (readOut.Contains("RU"))
                CreateReadOutItemDictionary("RU", readOut);
            //Create Meter Configurations Data Mapping
            if (readOut.Contains("CR"))
            {
                CreateReadOutItemDictionary("CR", readOut);
                readOut = readOut.Replace("DP", string.Empty);
                readOut = readOut.Replace("DL", string.Empty);
            }
            //Create Load Survey Data Mapping
            if (readOut.Contains("L/"))
                CreateReadOutItemDictionary("L", readOut);
            //Create Tamper Data Mapping
            if (readOut.Contains("TM"))
                CreateReadOutItemDictionary("TM", readOut);
            //Create DTM Load Survey Data Mapping
            if (readOut.Contains("SD/"))
                CreateReadOutItemDictionary("SD", readOut);
            //Create Header Data Mapping
            if (readOut.Contains("HD/"))
                CreateReadOutItemDictionary("HD", readOut);
            //Create Name plate Data Mapping
            if (readOut.Contains("NP"))
            {
                CreateReadOutItemDictionary("NP", readOut);
                readOut = readOut.Replace("NP", string.Empty);
            }
            //Create Phasor Data Mapping
            if (readOut.Contains("P/"))
                CreateReadOutItemDictionary("P", readOut);
        }

        /// <summary>
        /// Parse each readout for Read item tag.
        /// Story - 349865 - Parse Readout items for single phase
        /// </summary>
        /// <param name="readOut"></param>
        private void ParseReadOutItemsForSPhase(string readOut, Dictionary<string, string> dicOBISandData)
        {
            string data = readOut.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
            MatchCollection matches = FormatterCommon.ValidateData(data);//To be changed
            string[] generalData = new string[matches.Count];
            int counter = 0;
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                generalData[counter++] = groups[0].Value;
            }
            string[] GeneralData = FormatterCommon.RemoveDuplicateDataForSPhase(generalData);
            counter = 0;
            while (counter <= GeneralData.GetUpperBound(0)) counter++;
            //Create General Data Mapping
            if (counter > 0 && GeneralData.Length > 0)
                CreateReadOutItemDictionaryForSPhase("RD", readOut, dicOBISandData);
            //Create Fraud Energy Data Mapping
            if (readOut.Contains("MI"))
                CreateReadOutItemDictionary("MI", readOut);
            //Create Transactions Data Mapping
            if (readOut.Contains("TR"))
                CreateReadOutItemDictionary("TR", readOut);
            //Create RTC Update Data Mapping
            if (readOut.Contains("RU"))
                CreateReadOutItemDictionary("RU", readOut);
            //Create Meter Configurations Data Mapping
            if (readOut.Contains("CR"))
            {
                CreateReadOutItemDictionary("CR", readOut);
                readOut = readOut.Replace("DP", string.Empty);
                readOut = readOut.Replace("DL", string.Empty);
            }
            //Create Load Survey Data Mapping
            if (readOut.Contains("L/"))
                CreateReadOutItemDictionaryForSPhase("L", readOut, dicOBISandData);
            if (readOut.Contains("SA/"))
                CreateReadOutItemDictionaryForSPhase("L", readOut, dicOBISandData);
            //Create Tamper Data Mapping
            // Story - 354382 - Mapping has been done for tamper also
            if (readOut.Contains("TM"))
            {
                CreateReadOutItemDictionaryForSPhase("TM", readOut, dicOBISandData);
                CreateReadOutItemDictionaryForSPhase("TR", readOut, dicOBISandData);
                CreateReadOutItemDictionaryForSPhase("RU", readOut, dicOBISandData);
            }
            //Create DTM Load Survey Data Mapping
            if (readOut.Contains("SD/"))
                CreateReadOutItemDictionaryForSPhase("SD", readOut, dicOBISandData);
            //Create Header Data Mapping
            if (readOut.Contains("HD/"))
                CreateReadOutItemDictionary("HD", readOut);
            //Create Name plate Data Mapping
            if (readOut.Contains("NP"))
            {
                CreateReadOutItemDictionary("NP", readOut);
                readOut = readOut.Replace("NP", string.Empty);
            }
            //Create Phasor Data Mapping
            if (readOut.Contains("P/"))
                CreateReadOutItemDictionary("P", readOut);
        }

        /// <summary>
        /// Create mapping of readout item with meter data id for single phase non DLMS. Story - 349865
        /// </summary>
        /// <param name="itemTag"></param>
        /// <param name="readOut"></param>
        /// <param name="meterDataID"></param>
        public void CreateReadOutItemDictionaryForSPhase(string itemTag, string readOut, Dictionary<string, string> dicOBISandData)
        {
            ReadOutItem readOutItem = ReadOutItem.None;
            Collection<ReadOutCounterEntity> collectionItemReadOuts;
            switch (itemTag)
            {
                case "RD"://General
                    readOutItem = ReadOutItem.General;
                    break;
                case "MI":
                    readOutItem = ReadOutItem.FraudEnergy;
                    break;
                case "TR":
                    readOutItem = ReadOutItem.Transaction;
                    break;
                case "RU":
                    readOutItem = ReadOutItem.RTCUpdate;
                    break;
                case "P":
                    readOutItem = ReadOutItem.Phasor;
                    break;
                case "L":
                    readOutItem = ReadOutItem.LoadSurvey;
                    break;
                case "TM":
                    readOutItem = ReadOutItem.Tamper;
                    break;
                case "SD":
                    readOutItem = ReadOutItem.DailyProfile;
                    break;
                case "CR":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
                case "HD":
                    readOutItem = ReadOutItem.HeaderDetails;
                    break;
                case "NP":
                    readOutItem = ReadOutItem.NamePlate;
                    break;
            }
            if (!readOuts.ContainsKey(readOutItem))
            {
                collectionItemReadOuts = new Collection<ReadOutCounterEntity>();
                readOuts.Add(readOutItem, collectionItemReadOuts);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemTag"></param>
        /// <param name="readOut"></param>
        /// <param name="meterDataID"></param>
        private void CreateReadOutItemDictionary(string itemTag, string readOut)
        {
            ReadOutItem readOutItem = ReadOutItem.None;
            Collection<ReadOutCounterEntity> collectionItemReadOuts;
            switch (itemTag)
            {
                case "RD"://General
                    readOutItem = ReadOutItem.General;
                    break;
                case "MI":
                    readOutItem = ReadOutItem.FraudEnergy;
                    break;
                case "TR":
                    readOutItem = ReadOutItem.Transaction;
                    break;
                case "RU":
                    readOutItem = ReadOutItem.RTCUpdate;
                    break;
                case "P":
                    readOutItem = ReadOutItem.Phasor;
                    break;
                case "L":
                    readOutItem = ReadOutItem.LoadSurvey;
                    break;
                case "TM":
                    readOutItem = ReadOutItem.Tamper;
                    break;
                case "SD":
                    readOutItem = ReadOutItem.DailyProfile;
                    break;
                case "CR":
                    readOutItem = ReadOutItem.MeterConfigurations;
                    break;
                case "HD":
                    readOutItem = ReadOutItem.HeaderDetails;
                    break;
                case "NP":
                    readOutItem = ReadOutItem.NamePlate;
                    break;
            }
            if (!readOuts.ContainsKey(readOutItem))
            {
                collectionItemReadOuts = new Collection<ReadOutCounterEntity>();
                readOuts.Add(readOutItem, collectionItemReadOuts);
            }
        }

        public override string GetContent(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string fileContent = streamReader.ReadToEnd();
            streamReader.Close();
            return fileContent;
        }
        private void entityToMIOS_OnSayToServerEvent(string message)
        {
            //this.StatusMessage = message;
            //Application.DoEvents();
        }
        private void formatterBilling_OnChannelStatusChanged(string msg)
        {
            //this.StatusMessage = msg;
            //Application.DoEvents();
        }
        /// <summary>
        /// Checks the different path specified in configuration file are present , if not create them
        /// </summary>
        /// <param name="cfcAPI"></param>
        /// <returns></returns>
        private bool CheckPath(CFCAPI cfcAPI)
        {
            bool isValid = true;
            try
            {
                if (!string.IsNullOrEmpty(cfcAPI.ERRORPATH) && !string.IsNullOrEmpty(cfcAPI.DESTPATH) && !string.IsNullOrEmpty(cfcAPI.DONEPATH))
                {
                    if (!Directory.Exists(cfcAPI.ERRORPATH))
                    {
                        Directory.CreateDirectory(cfcAPI.ERRORPATH);
                    }
                    if (!Directory.Exists(cfcAPI.DESTPATH))
                    {
                        Directory.CreateDirectory(cfcAPI.DESTPATH);
                    }
                    if (!Directory.Exists(cfcAPI.DONEPATH))
                    {
                        Directory.CreateDirectory(cfcAPI.DONEPATH);
                    }
                    if (!Directory.Exists(cfcAPI.RESULTFILE.Substring(0, cfcAPI.RESULTFILE.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(cfcAPI.RESULTFILE.Substring(0, cfcAPI.RESULTFILE.LastIndexOf("\\")));
                    }
                }
                else
                {
                    isValid = false;
                }
                return isValid;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "CheckPath(CFCAPI cfcAPI)", ex);
                isValid = false;
                return isValid;
                
            }

        }
    }
}
