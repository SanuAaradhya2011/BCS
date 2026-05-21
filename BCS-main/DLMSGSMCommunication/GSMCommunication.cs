using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using Utilities;
using CAB.License.DataStore;
using CAB.License;
using CAB.Framework.Utility;
namespace DLMSGSMCommunication
{
    public partial class GSMCommunication : ServiceBase
    {        
        GSMTaskBLL gsmTaskBll = new GSMTaskBLL();
        GSMGroupBLL groupBll = new GSMGroupBLL();
        GSMLoggingBLL logBLL = new GSMLoggingBLL();
        SystemSettingsBLL systemSettingsBLL = new SystemSettingsBLL();
        private System.Timers.Timer _timer;
        private System.Timers.Timer licensingTimer;
        bool isServiceStarted = false;
        IDataStoreManager dataStoreManager = null;
        DataStoreInfo dataStoreInfo = null;
        ILicenseManager licenseManager = null;
        private CABSerializer lngSerializer = null;
        private const string MaxLoadSurveyDays = "MaxLoadSurveyDays";
        private ModemConfig modemConfig = null;
        #region constructor
        public GSMCommunication()
        {
            //com.GSMLogCreating += new GSMLogEventHandler(com_GSMLogCreating);
            InitializeComponent();
            dataStoreManager = new DataStoreManager();
            licenseManager = new LicenseManager();
            this.CanHandlePowerEvent = true;
            lngSerializer = new CABSerializer();
            
        }
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            UpdateRegistryOnStop();
            return base.OnPowerEvent(powerStatus);
        }
        private void com_GSMLogCreating(object sender, GSMLogEventArgs e)
        {
            GSMLoggingEntity gsmLogEntity = new GSMLoggingEntity();
            gsmLogEntity.Status = e.GSMLog.Status;
            gsmLogEntity.Task_ID = e.GSMLog.Task_ID;
            gsmLogEntity.Group_ID = e.GSMLog.Group_ID;
            gsmLogEntity.Meter_ID = e.GSMLog.Meter_ID;
            gsmLogEntity.Log_ID = e.Log_ID;
            gsmLogEntity.IsGeneralCompleted = e.IsGeneralCompleted;
            gsmLogEntity.IsInstantCompleted = e.IsInstantCompleted;
            gsmLogEntity.IsBillingCompleted = e.IsBillingCompleted;
            gsmLogEntity.Retries = e.GSMLog.Retries;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            gsmLogEntity.CreationDateTime = DateTime.Now;
            //EventLogging.CallLogDetails("CreationDateTime3 " + gsmLogEntity.CreationDateTime);
            //IFormatProvider provider = new System.Globalization.CultureInfo("en-GB", true);
            //gsmLogEntity.CreationDateTime = DateTime.Parse(DateTime.Now.ToString(), provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

            gsmLogEntity.ErrorMessage = e.GSMLog.ErrorMessage;
            GSMLoggingEntity gsmLog = ((GSMLoggingEntity)logBLL.InsertorUpdateData(gsmLogEntity, false)) as GSMLoggingEntity;
            {
                if (gsmLog != null)
                    e.Log_ID = gsmLog.Log_ID;
            }
        }
        #endregion

        private CABSerialPort GetAvailableModemCOMPort()
        {
            CABSerialPort serialPort = null;
            foreach (CABSerialPort lsp in CABSerialPorts.ListOfSerialPorts)
            {
                if (!lsp.IsWaiting)
                {
                    serialPort = lsp;
                    
                }
            }          
            return serialPort;
        }
        /// <summary>
        /// Gets the maximum load survey days.
        /// </summary>
        /// <returns></returns>
        private int GetMaxLoadSurveyDays()
        {
            int maxLoadSurveyDays = 0;
            try
            {
                maxLoadSurveyDays = Convert.ToInt32(ConfigSettings.GetValue(MaxLoadSurveyDays));
            }
            catch
            {
                maxLoadSurveyDays = 30;
            }
            return maxLoadSurveyDays;
        }

        /// <summary>
        /// This method is used for getting all active scheduled tasks for executing.
        /// </summary>
        public void GetScheduledTask()
        {

            //EventLogging.CallLogDetails(Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData));
            List<GSMTaskEntity> GSMTask = new List<GSMTaskEntity>();
            List<MeterMasterEntity> meterNumbers = new List<MeterMasterEntity>();
             int totalCurrentMinute = 0;
             bool isGPRSCommunication = false;
            DateTime validDate = DateTime.MinValue;
            try
            {
                GSMTask = gsmTaskBll.GetFilteredScheduledTasks("Inqueue");
                if (GSMTask != null)
                {
                    this.modemConfig = (ModemConfig)lngSerializer.DeserializeToObject(AppDomain.CurrentDomain.BaseDirectory + "Modem.xml", typeof(ModemConfig));
                    //EventLogging.fileDelete();
                    List<Thread> threads = new List<Thread>();
                    //foreach Schedule
                    foreach (GSMTaskEntity gt in GSMTask)
                    {
                        threads.Clear();
                        isGPRSCommunication = false; 
                        // Set max load survey days before as the task begins
                        MultipleSerialPortSettings.Default.MaxLoadSurveyDays = GetMaxLoadSurveyDays();
                        
                        int countMeters = 0;
                        System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
                        dateInfo.ShortDatePattern = "dd/MM/yyyy";

                        DateTime checkDate = Convert.ToDateTime(gt.startDate, dateInfo);
                        validDate = new DateTime(checkDate.Year, checkDate.Month, checkDate.Day, Convert.ToInt32(gt.StartHour), Convert.ToInt32(gt.StartMinute), 0);

                        if (validDate <= DateTime.Now)
                        {
                            meterNumbers = groupBll.ListMeterSimNumbers(gt.groupId, gt.taskId, gt.taskRetries);
                            if (meterNumbers.Count > 0)
                            {
                                EventLogging.CallLogDetails("Schedule " + gt.taskName + " started.");
                                EventLogging.CallLogDetails("Total Meters to be read in this Schedule :" + meterNumbers.Count);

                               
                               

                                //Here getting time and date when the task start for first scheduled.
                                totalCurrentMinute = (DateTime.Now.Hour * 60) + DateTime.Now.Minute;                               
                                //Execute the task if it is inqueue
                                if (gt.taskStatus != EnumUtil.stringValueOf(TaskStatus.InProgress))
                                {
                                    //Change the status of the Schedule to Inprogress
                                    gt.taskStatus = EnumUtil.stringValueOf(TaskStatus.InProgress);
                                    gsmTaskBll.UpdateGSMTask(gt);
                                    //foreach Meter in running Schedule                                  
                                    foreach (MeterMasterEntity meterNumber in meterNumbers)
                                    {
                                        //if (!systemSettingsBLL.UseMultiplePorts())
                                        //{
                                        //    ExecuteTask(meterNumber, gt, DateTime.Now, totalCurrentMinute);
                                        //}
                                        ////if it is configured for multiple ports.
                                        //else
                                        //{
                                        Thread lngThread = null;
                                        CABWorkerGSM workerGSM = null;
                                        if (meterNumber.CommunicationType == CommunicationType.GPRS.ToString())
                                        {
                                            isGPRSCommunication = true;
                                            workerGSM = new CABWorkerGSM(meterNumber, gt,meterNumber.Meter_ID);
                                            lngThread = new Thread(new ThreadStart(delegate { workerGSM.ExecuteGPRSTask(gt, DateTime.Now); }));
                                            lngThread.IsBackground = true;
                                            lngThread.Name = meterNumber.Meter_Phone + ":" + meterNumber.Meter_ID;
                                            lngThread.Start();
                                            countMeters++;
                                            threads.Add(lngThread);
                                        }
                                        if (meterNumber.CommunicationType == CommunicationType.TCP.ToString())
                                        {
                                            isGPRSCommunication = true;
                                            workerGSM = new CABWorkerGSM(meterNumber, gt, meterNumber.Meter_ID);
                                            lngThread = new Thread(new ThreadStart(delegate { workerGSM.ExecuteTCPTask(gt, DateTime.Now); }));
                                            lngThread.IsBackground = true;
                                            lngThread.Name = meterNumber.Meter_Phone + ":" + meterNumber.Meter_ID;
                                            lngThread.Start();
                                            countMeters++;
                                            threads.Add(lngThread);
                                        }
                                        if (meterNumber.CommunicationType == CommunicationType.FTP.ToString())
                                        {
                                            isGPRSCommunication = true;
                                            workerGSM = new CABWorkerGSM(meterNumber, gt, meterNumber.Meter_ID);
                                            lngThread = new Thread(new ThreadStart(delegate { workerGSM.ExecuteFTPTask(gt, DateTime.Now); }));
                                            lngThread.IsBackground = true;
                                            lngThread.Name = meterNumber.Meter_Phone + ":" + meterNumber.Meter_ID;
                                            lngThread.Start();
                                            countMeters++;
                                            threads.Add(lngThread);
                                        }

                                        //else
                                        if (meterNumber.CommunicationType == CommunicationType.GSM.ToString())
                                        {
                                            CABSerialPort serialPort = GetAvailableModemCOMPort();
                                            if (serialPort != null)
                                            {
                                                EventLogging.CallLogDetails(serialPort.PortName + ":" + serialPort.PortName + " available");

                                            }
                                            else
                                            {

                                                // if all the com ports are busy
                                                //EventLogging.CallLogDetails("waiting for port to get free");
                                                //int index = WaitHandle.WaitAny(resetEvents, 5000 * 60 * gt.taskRetries);
                                                //if (index == WaitHandle.WaitTimeout)
                                                //{
                                                //    EventLogging.CallLogDetails(portName + ":" + "Meter time out");
                                                //    ReleaseResources();
                                                //}
                                                int timeOut = 0;
                                                while (serialPort == null)
                                                {
                                                    Thread.Sleep(5000);
                                                    timeOut++;
                                                    //if it takes more than 60 minutes per retry
                                                    if (timeOut > 24 * 30 * gt.taskRetries)
                                                    {
                                                        EventLogging.CallLogDetails(serialPort.PortName + ":" + "Meter time out. Releasing resources..");
                                                        CABSerialPorts.ReleaseResources();
                                                    }
                                                    //WaitHandle.WaitAny(CABSerialPorts.GetAllEvents());
                                                    serialPort = GetAvailableModemCOMPort();
                                                }


                                            }
                                            if (serialPort != null)
                                            {
                                                workerGSM = new CABWorkerGSM(meterNumber, gt, meterNumber.Meter_Phone, serialPort.PortName
                                                    , meterNumber.Meter_ID, modemConfig);
                                                //BhardwajG There is no need of sending meter master entity in execute task as the worker object is
                                                //          already initialised with meter master entity
                                                lngThread = new Thread(new ThreadStart(delegate { workerGSM.ExecuteTask(gt, DateTime.Now); }));
                                                lngThread.IsBackground = true;
                                                lngThread.Name = meterNumber.Meter_Phone + ":" + meterNumber.Meter_ID;
                                                lngThread.Start();
                                                countMeters++;
                                            }
                                            else
                                            {
                                                EventLogging.CallLogDetails("Com Port not available");
                                                Thread.Sleep(300000);
                                            }
                                        }
                                        //}
                                    }
                                }
                                if (isGPRSCommunication)
                                {
                                    //wait for meter reads to completed here 3600 seconds = 1hour
                                    foreach (var thread in threads)
                                    {
                                        thread.Join(60 * 60 * 1000 * gt.taskRetries);
                                    }
                                }
                                else
                                {
                                    //wait for meter reads to completed here 3600 seconds = 1hour
                                    bool ok = WaitHandle.WaitAll(CABSerialPorts.GetAllEvents(), 60 * 60 * 1000 * gt.taskRetries);
                                    //bool ok = false;
                                    //Thread.Sleep(600000);
                                    if (!ok)
                                    {
                                        //EventLogging.CallLogDetails("Meter timed out.");
                                        CABSerialPorts.ReleaseResources();
                                    }
                                }
                            }

                            if (gt.taskType == EnumUtil.stringValueOf(GSMTasksType.OneTimeOnly))
                            {

                                gt.taskStatus = EnumUtil.stringValueOf(TaskStatus.Inactive);
                            }
                            else
                            {

                                gt.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                            }

                            //Insert row in gsm_tasks_completed
                            gsmTaskBll.InsertCompleteTask(gt);

                            //Update next run time when going out of the funtion
                            gsmTaskBll.UpdateGSMTask(gt, true);

                            //Delete row from gsm_tasks
                            List<GSMTaskEntity> lstTask = new List<GSMTaskEntity>();
                            lstTask.Add(gt);
                            gsmTaskBll.deleteGSMTasks(lstTask);

                            //Insert row in gsm_tasks
                            gsmTaskBll.InsertGSMTask(gt);
                        }
                        else
                        {
                            //do not update next run time when going out of the funtion..
                            gsmTaskBll.UpdateGSMTask(gt, false);
                        }
                    }
                }
                else
                {
                    UpdateInprogresstoInqueue();
                    EventLogging.CallLogDetails("Currently there is no active Schedule.");
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("Error occured during running Schedule : " + ex.Message.ToString() + " Trace :" + ex.StackTrace + " Source : " + ex.Source);
            }
        }

        /// <summary>
        /// This method is used for executing task according to sim number.
        /// </summary>
        /// <param name="simNumber">Meter modem sim number to be dialed.</param>
        /// <param name="gsmtask">GSM task entitiy object</param>
        /// <param name="currentDate">current data to execute task</param>
        /// <param name="currentExecuteMinute">curent total execute minute for starting task</param>
        //private void ExecuteTask(MeterMasterEntity meterMasterEntity, GSMTaskEntity gsmtask, DateTime currentDate, int currentExecuteMinute)
        //{
        //    bool bSuccess = false;
        //    bool isModemConnected = false;
        //    try
        //    {
        //        string message = string.Empty;
        //        bool newMeter = true;

        //        EventLogging.CallLogDetails("Communication start with Meter number : " + meterMasterEntity.Meter_ID);

        //        for (int i = 1; i <= gsmtask.taskRetries; i++)
        //        {
        //            if (i > 1)
        //                EventLogging.CallLogDetails("Trying to connect again to Meter number " + meterMasterEntity.Meter_ID);

        //            isModemConnected = com.Connect(meterMasterEntity.Meter_Phone, out message);

        //            EventLogging.CallLogDetails("Status of the connection : " + message);
        //            bSuccess = com.GetMeterData(meterMasterEntity, gsmtask, i, gsmtask.taskRetries, message, newMeter, isModemConnected);
        //            EventLogging.CallLogDetails("Status of the Process : " + bSuccess);
        //            newMeter = false;
        //            if (isModemConnected)
        //            {
        //                com.DLMSDisconnect();

        //                EventLogging.CallLogDetails("Disconnecting Modem..");

        //                com.Disconnect();
        //            }
        //            EventLogging.CallLogDetails("Disconnected.");
        //            if (bSuccess)
        //            {
        //                EventLogging.CallLogDetails("Communication with Meter number " + meterMasterEntity.Meter_ID + " finished.");
        //                break;
        //            }
        //            else
        //            {
        //                continue;
        //            }

        //        }
        //        //if (!bSuccess)
        //        //{
        //        //    EventLogging.CallLogDetails("Rebooting local modem..");
        //        //    com.RebootModem();
        //        //    EventLogging.CallLogDetails("Waiting for 10 seconds");
        //        //    Thread.Sleep(10000);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        if (isModemConnected)
        //        {
        //            com.DLMSDisconnect();
        //            EventLogging.CallLogDetails("Disconnecting Modem..");
        //            com.Disconnect();
        //            EventLogging.CallLogDetails("Disconnected.");
        //        }
        //        EventLogging.CallLogDetails("Error Occured during transaction: " + ex.Message.ToString());

        //        //EventLogging.CallLogDetails("Rebooting local modem..");
        //        //com.SendCommandToModem("AT+cfun=1");
        //        //EventLogging.CallLogDetails("Waiting for 10 seconds");
        //        //Thread.Sleep(10000);

        //    }
        //    finally
        //    {
        //        GlobalObjects.objSerialComm.ClosePort();
        //    }
        //}
        private void ReleaseResources()
        {
            foreach (CABSerialPort port in CABSerialPorts.ListOfSerialPorts)
            {
                //try
                //{
                //    if (port.CABWorkerGSM != null)
                //    {
                //        if (port.CABWorkerGSM.com != null)
                //        {
                //            port.CABWorkerGSM.com.DLMSDisconnect();
                //            port.CABWorkerGSM.com.Disconnect();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    EventLogging.CallLogDetails("Exception occured while disconnecting timed out meter " + ex.Message);
                //}
                port.IsWaiting = false;
            }


        }
        # region Service Communication
        /// <summary>
        /// This method is used for the starting the windows service and it will auto execute while GSM service started..
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            OnServiceStart();
        }
        private void UpdateRegistryTime()
        {
            TimeSpan timeSpan;
            dataStoreInfo = dataStoreManager.ReadDataFromRegistry();
            if (dataStoreInfo != null)
            {
                EventLogging.CallLogDetails(dataStoreInfo.LastRunDate.ToString());
                if (dataStoreInfo.LastRunDate > DateTime.MinValue)
                {
                    if (DateTime.Now > dataStoreInfo.LastRunDate)
                    {
                        timeSpan = DateTime.Now - dataStoreInfo.LastRunDate;
                        dataStoreInfo.NumberOfDaysElapsed = dataStoreInfo.NumberOfDaysElapsed + timeSpan.Hours + timeSpan.Days * 24;
                        EventLogging.CallLogDetails(timeSpan.ToString());
                    }
                    else
                    {
                        dataStoreInfo.NumberOfDaysElapsed = 30 * 24 + 1;
                    }
                }
                dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
            }

        }
        public void OnServiceStart()
        {
            try
            {
                UpdateRegistryTime();
                if (LicenseStatus.Activated != licenseManager.GetLicenseStatus())
                {
                    licensingTimer = new System.Timers.Timer(60 * 60 * 1000);
                    licensingTimer.Enabled = true;
                    licensingTimer.Elapsed += new System.Timers.ElapsedEventHandler(licensingTimer_Elapsed);
                }

                //After every two minute the service will call "GetScheduledTask" method and will check if any new task created.
                //BhardwajG : After every 10 seconds the service will call "GetScheduledTask" method and will check if any new task created.
                _timer = new System.Timers.Timer(10 * 1000);
                _timer.Enabled = true;
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);




                //ashish
                //if (EventLog.SourceExists("DLMS GSM Communication"))
                //{
                //    EventLog ev = new EventLog("Application", System.Environment.MachineName);
                //    ev.Clear();
                //}
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
            }
        }

        private void licensingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                dataStoreInfo = dataStoreManager.ReadDataFromRegistry();
                if (dataStoreInfo != null)
                {
                    dataStoreInfo.LastRunDate = DateTime.Now;
                    dataStoreInfo.NumberOfDaysElapsed = dataStoreInfo.NumberOfDaysElapsed + 1;
                    dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(ex.Message);
            }
        }
        private void UpdateInprogresstoInqueue()
        {
            List<GSMTaskEntity> lstGSMTaskEntity = new List<GSMTaskEntity>();
            try
            {
                lstGSMTaskEntity = gsmTaskBll.GetFilteredScheduledTasks(EnumUtil.stringValueOf(TaskStatus.InProgress));
                foreach (GSMTaskEntity gsmtask in lstGSMTaskEntity)
                {
                    gsmtask.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                }
                gsmTaskBll.updateGSMTasksStatus(lstGSMTaskEntity);
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("error in service:" + ex.Message);
            }
        }
        private void UpdateRegistryOnStop()
        {
            dataStoreInfo = dataStoreManager.ReadDataFromRegistry();
            if (dataStoreInfo != null)
            {
                dataStoreInfo.LastRunDate = DateTime.Now;
                dataStoreManager.WriteDatatoRegistry(dataStoreInfo);
            }
        }
        /// <summary>
        /// This method is used for the stopping the windows service..
        /// </summary>
        protected override void OnStop()
        {
            UpdateRegistryOnStop();
            _timer.Enabled = false;
            CABSerialPorts.ReleaseResources();
            //com.DLMSConnect();
            //com.Disconnect();
            EventLogging.CallLogDetails("DLMS GSM communication service stop successfully");
        }

        /// <summary>
        /// This method is used running scheduled task on every specified min.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (!isServiceStarted)
            {
                UpdateInprogresstoInqueue();
                isServiceStarted = true;
            }
            _timer.Stop();
            GetScheduledTask();
            _timer.Start();
        }
        #endregion
    }
}