using System;
using System.Collections.Generic;
using System.IO.Ports;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using SerialCommunication;
using Utilities;
using System.Threading;
using CABCommunication.PhysicalLayer;
using CABCommunication.Common;
using CAB.Parser;
using CAB.Serialization;
using CAB.Framework.Utility;

namespace DLMSGSMCommunication
{

    public class CABWorkerGSM
    {
        GSMTaskBLL gsmTaskBll = new GSMTaskBLL();
        GSMGroupBLL groupBll = new GSMGroupBLL();
        public RemoteCommunication com = null;
        GSMLoggingBLL logBLL = new GSMLoggingBLL();
        bool isPortAvailable;
        string portName;
        string meterID = null;
        //BhardwajG declare variable for holding meter master entity
        private MeterMasterEntity meterEntity = null;

        // boolean to return status of ports free in PC
        public bool IsPortAvailable
        {
            get { return isPortAvailable; }
            set { isPortAvailable = value; }
        }
        public string MeterID
        {
            get
            {
                return meterID;
            }
            set
            {
                meterID = value;
            }
        }
        static int instanceCounter = 0;
        //CABWorker Conmstructor -- 
        //public CABWorkerGSM()
        //{

        //}
        //static CABWorkerGSM()
        //{


        //}

        /// <summary>
        /// Gets the channel details
        /// </summary>
        /// <param name="isDLMS"></param>
        /// <param name="portName"></param>
        /// <returns></returns>
        public ChannelDetail GetChannelDetails(string portName, CABCommunication.PhysicalLayer.ChannelType channelType)
        {
            ChannelDetail channelDetails = new ChannelDetail();
            channelDetails.BaudRate = "9600";
            channelDetails.InitialBaudRate = "9600";
            channelDetails.ResponseTimeout = 6000;
            channelDetails.InterCharacterDelay = 5000;
            channelDetails.NumberOfRetry = 3;
            channelDetails.ChannelType = channelType;
            channelDetails.ComPort = portName;
            channelDetails.Parity = "None"; ;
            channelDetails.StopBits = "1";
            channelDetails.DataBits = "8";
            return channelDetails;
        }
        /// <summary>
        /// For GPRS Communication
        /// </summary>
        /// <param name="meterMasterEntity"></param>
        /// <param name="gsmtask"></param>
        /// <param name="workerMeterID"></param>
        /// <param name="modemConfig"></param>
        public CABWorkerGSM(MeterMasterEntity meterMasterEntity, GSMTaskEntity gsmtask, string workerMeterID)
        {
            meterID = workerMeterID;
            GSMLoggingEntity gsmLoggingEntity = new GSMLoggingEntity();
            gsmLoggingEntity.Task_ID = gsmtask.taskId;
            gsmLoggingEntity.Group_ID = gsmtask.groupId;
            gsmLoggingEntity.Meter_ID = meterMasterEntity.Meter_ID;
            gsmLoggingEntity.Status = "IP";
            gsmLoggingEntity.Retries = 1;
            gsmLoggingEntity.CreationDateTime = DateTime.Now;
            gsmLoggingEntity.ErrorMessage = "Connecting remote Modem IP: " + meterMasterEntity.MeterGPRSModemIMEI + " Meter ID " + meterMasterEntity.Meter_ID;
            logBLL.InsertData(gsmLoggingEntity, false);
            ChannelInformation channelInfo = new ChannelInformation();
            channelInfo.CommunicationMode = meterMasterEntity.CommunicationType;
            if (ConfigSettings.GetValue("PortName").Contains(","))
                channelInfo.ComPort = ConfigSettings.GetValue("PortName").Split(',')[0];
            else
                channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.ModemInfo = meterMasterEntity.MeterGPRSModemIMEI;
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;            
            channelInfo.NoOfRetries = (byte)gsmtask.taskRetries;
            channelInfo.TcpPort = ConfigSettings.GetValue("TCPPORT");
            this.meterEntity = meterMasterEntity;
            if (ConfigSettings.GetValue("ApplicationContext") == "03")
                channelInfo.SecurityMechanism = 0x00;//---PC Mode read Invo counter for smart meter
            else
                channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS"; //UtilityDetails.PrimaryUtlityName;
            channelInfo.NoOfRetries = Convert.ToByte(ConfigSettings.GetValue("NoOfRetries"));
            Result result = new Result();
            com = new RemoteCommunication(channelInfo, gsmLoggingEntity.Log_ID, meterMasterEntity.MeterGPRSModemIMEI);
        }
        public CABWorkerGSM(MeterMasterEntity meterMasterEntity, GSMTaskEntity gsmtask, string tempSimNumber, string portName, string workerMeterID, ModemConfig modemConfig)
        {
            ChannelDetail channelDetails = null;
            instanceCounter++;
            //if (instanceCounter == 1)
            //    portName = "COM1";
            //else
            //    portName = "COM3";
            meterID = workerMeterID;

            GSMLoggingEntity gsmLoggingEntity = new GSMLoggingEntity();
            gsmLoggingEntity.Task_ID = gsmtask.taskId;
            gsmLoggingEntity.Group_ID = gsmtask.groupId;
            gsmLoggingEntity.Meter_ID = meterMasterEntity.Meter_ID;
            //gsmLoggingEntity.IsGeneralCompleted = gsmtask.isGeneralRequired;
            //gsmLoggingEntity.IsInstantCompleted = gsmtask.isInstantaneousRequired;
            //gsmLoggingEntity.IsBillingCompleted = gsmtask.isBillingRequired;
            gsmLoggingEntity.Status = "IP";
            gsmLoggingEntity.Retries = 1;
            gsmLoggingEntity.CreationDateTime = DateTime.Now;
            gsmLoggingEntity.ErrorMessage = "communication started with sim number " + meterMasterEntity.Meter_Phone + " Meter ID " + meterMasterEntity.Meter_ID;
            logBLL.InsertData(gsmLoggingEntity, false);
            //com.LogID = gsmLoggingEntity.Log_ID;

            channelDetails = GetChannelDetails(portName,
                    (CABCommunication.PhysicalLayer.ChannelType)System.Enum.Parse(typeof(CABCommunication.PhysicalLayer.ChannelType), meterMasterEntity.CommunicationType));

            Serial objSerialComm = CABSerialPorts.SetPortToWait(portName, true, channelDetails);
            //BhardwajG Initialising meter master entity
            this.meterEntity = meterMasterEntity;
            com = new RemoteCommunication(objSerialComm, tempSimNumber, gsmLoggingEntity.Log_ID);


            //com = new MultiplePortsCommunication(tempSimNumber, meterID, gsmLoggingEntity.Log_ID, objSerialComm);
            //com.LogID = logBLL.UpdateData()
            //com.GSMLogCreating += new MultiplePortsGSMLogEventHandler(com_GSMLogCreating);


        }
        //private void com_GSMLogCreating(object sender, GSMLogEventArgs e)
        //{

        //    lock (test)
        //    {
        //        GSMLoggingEntity gsmLogEntity = new GSMLoggingEntity();
        //        gsmLogEntity.Status = e.GSMLog.Status;
        //        gsmLogEntity.Task_ID = e.GSMLog.Task_ID;
        //        gsmLogEntity.Group_ID = e.GSMLog.Group_ID;
        //        gsmLogEntity.Meter_ID = meterID;
        //        gsmLogEntity.Log_ID = e.Log_ID;
        //        gsmLogEntity.IsGeneralCompleted = e.IsGeneralCompleted;
        //        gsmLogEntity.IsInstantCompleted = e.IsInstantCompleted;
        //        gsmLogEntity.IsBillingCompleted = e.IsBillingCompleted;
        //        gsmLogEntity.Retries = e.GSMLog.Retries;

        //        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
        //        gsmLogEntity.CreationDateTime = DateTime.Now;
        //        //EventLogging.CallLogDetails( com.comPortName + "CreationDateTime3 " + gsmLogEntity.CreationDateTime);
        //        //IFormatProvider provider = new System.Globalization.CultureInfo("en-GB", true);
        //        //gsmLogEntity.CreationDateTime = DateTime.Parse(DateTime.Now.ToString(), provider, System.Globalization.DateTimeStyles.NoCurrentDateDefault);

        //        gsmLogEntity.ErrorMessage = e.GSMLog.ErrorMessage;
        //        GSMLoggingEntity gsmLog = ((GSMLoggingEntity)logBLL.InsertorUpdateData(gsmLogEntity, false)) as GSMLoggingEntity;
        //        {
        //            if (gsmLog != null)
        //                e.Log_ID = gsmLog.Log_ID;
        //        }
        //    }
        //}
        /// <summary>
        /// Gets the selected profile list according to the task.
        /// </summary>
        /// <param name="gsmTask"></param>
        /// <returns></returns>
        private IList<ProfileId> GetSelectedProfiles(GSMTaskEntity gsmTask)
        {
            IList<ProfileId> selectedProfiles = new List<ProfileId>();
            if (gsmTask.isGeneralRequired)
            {
                selectedProfiles.Add(ProfileId.NamePlate);
            }
            if (gsmTask.isInstantaneousRequired)
            {
                selectedProfiles.Add(ProfileId.Anomaly);//added by ravi
                selectedProfiles.Add(ProfileId.Instant);
              
            }
            if (gsmTask.isBillingRequired)
            {
                selectedProfiles.Add(ProfileId.Billing);//Billing
                               
                ////TOU
                //selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                //selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                //selectedProfiles.Add(ProfileId.PassiveDayProfile);
                //selectedProfiles.Add(ProfileId.ActiveSeasonProfile);
                //selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                //selectedProfiles.Add(ProfileId.ActiveDayProfile);
                //selectedProfiles.Add(ProfileId.ActivationDate);
            }
            if (gsmTask.IsLoadSurveyRequired)
            {
                selectedProfiles.Add(ProfileId.LoadSurvey);//LoadSurvey
            }
            if (gsmTask.IsTamperRequired)
            {
                selectedProfiles.Add(ProfileId.Tamper);//Tamper
            }
            if (gsmTask.IsMidnightRequired)
            {
               selectedProfiles.Add(ProfileId.Midnight);//Midnight
            }
            if (gsmTask.IsMeterConfigRequired)
            {
                selectedProfiles.Add(ProfileId.MeterConfiguration);//Meter Configuration
                ////TOU
                selectedProfiles.Add(ProfileId.PassiveSeasonProfile);
                selectedProfiles.Add(ProfileId.PassiveWeekProfile);
                selectedProfiles.Add(ProfileId.PassiveDayProfile);
                selectedProfiles.Add(ProfileId.ActiveSeasonProfile);
                selectedProfiles.Add(ProfileId.ActiveWeekProfile);
                selectedProfiles.Add(ProfileId.ActiveDayProfile);
                selectedProfiles.Add(ProfileId.ActivationDate);
                selectedProfiles.Add(ProfileId.RTC);
                selectedProfiles.Add(ProfileId.SIP);
                selectedProfiles.Add(ProfileId.DIP);
                selectedProfiles.Add(ProfileId.BillingType);
                                      
            }


            return selectedProfiles;
        }
        public void ExecuteTask(GSMTaskEntity gsmtask, DateTime currentDate)
        {
            bool finalOutput = false;
            bool isIEC1P = false;            //bool Flag;
            List<GSMTaskEntity> gsmTaskList;
            List<GSMTaskEntity> gsmTaskComleteList;

            GSMLoggingEntity logEntity = new GSMLoggingEntity();

            try
            {
                string message = string.Empty;
                EventLogging.CallLogDetails(com.ComPortName + ":" + "Communication start with sim number : " + com.SimNumber);
                gsmTaskList = new List<GSMTaskEntity>();
                for (int retryCount = 1; retryCount <= gsmtask.taskRetries; retryCount++)
                {
                    logEntity.Retries = retryCount;
                    if (retryCount > 1)
                    {
                        EventLogging.CallLogDetails(com.ComPortName + ":" + "Trying to connect again..");
                    }
                    //BhardwajG : If local modem is initialized, only then do further processing      
                    EventLogging.CallLogDetails(com.ComPortName + ":" + "Opening session with port..");
                    Result result = com.OpenSession();
                    EventLogging.CallLogDetails(com.ComPortName + ":" + CommonBLL.GetEnumDescription(result.ErrorCode));
                    //Make an database entry
                    logEntity.Status = "IP";
                    logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                    com.GSMLogCreating(logEntity);

                    if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS)
                    {
                        result = com.Send(gsmtask, GetSelectedProfiles(gsmtask), logEntity);
                    }
                    else if (result.ErrorCode == CommunicationErrorType.ConnectedNonDLMS || result.ErrorCode == CommunicationErrorType.SuccessForIECSP)
                    {
                        if (result.ErrorCode == CommunicationErrorType.SuccessForIECSP) isIEC1P = true;
                        result = com.Send(gsmtask, logEntity, isIEC1P);
                    }
                    else
                    {
                        if (logEntity.Retries == retryCount)
                        {
                            logEntity.Status = "NC";
                        }
                        else
                        {
                            logEntity.Status = "IP";
                        }
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        finalOutput = true;
                    }

                    EventLogging.CallLogDetails(com.ComPortName + ":" + "Status of the Process : " + finalOutput);

                    if (finalOutput)
                    {
                        gsmTaskComleteList = new List<GSMTaskEntity>();
                        //BhardwajG Remove phone number and dot from the message as it is next phone number.
                        EventLogging.CallLogDetails(com.ComPortName + ":" + "Communication finished");
                        break;
                    }
                    else
                    {
                        //com.Disconnect();
                        continue;
                    }

                }


            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails(com.ComPortName + ":" + "Error Occured during transaction: " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
            }

            finally
            {

                //Make file from file List
                //com.MakeFileFromList();
                gsmtask.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                CABSerialPorts.SetPortToWait(com.ComPortName, false, null);

            }
        }

        public void ExecuteGPRSTask(GSMTaskEntity gsmtask, DateTime currentDate)
        {
            bool finalOutput = false;
            //bool Flag;
            List<GSMTaskEntity> gsmTaskList;
            List<GSMTaskEntity> gsmTaskComleteList;

            GSMLoggingEntity logEntity = new GSMLoggingEntity();

            try
            {
                string message = string.Empty;
                EventLogging.CallLogDetails("Connecting with Modem IMEI Number : " + meterEntity.MeterGPRSModemIMEI);
                gsmTaskList = new List<GSMTaskEntity>();
                for (int retryCount = 1; retryCount <= gsmtask.taskRetries; retryCount++)
                {
                    logEntity.Retries = retryCount;
                    if (retryCount > 1)
                    {
                        EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Trying to connect again..");
                    }

                    Result result = com.OpenSession();
                    if (result.ErrorCode == CommunicationErrorType.ConnectedDLMS && result.RecieveDataBuffer != null)
                    {
                        logEntity.Status = "IP";
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);
                        result = com.Send(gsmtask, GetSelectedProfiles(gsmtask), logEntity);
                    }
                    else
                    {
                        if (logEntity.Retries == retryCount)
                        {
                            logEntity.Status = "NC";
                        }
                        else
                        {
                            logEntity.Status = "IP";
                        }
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        finalOutput = true;
                    }

                    EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Status of the Process : " + finalOutput);

                    if (finalOutput)
                    {
                        gsmTaskComleteList = new List<GSMTaskEntity>();
                        //BhardwajG Remove phone number and dot from the message as it is next phone number.
                        // EventLogging.CallLogDetails(com.ComPortName + ":" + "Communication finished");
                        break;
                    }
                    else
                    {
                        //com.Disconnect();
                        continue;
                    }

                }


            }
            catch (Exception ex)
            {
                // EventLogging.CallLogDetails(com.ComPortName + ":" + "Error Occured during transaction: " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
            }

            finally
            {

                //Make file from file List
                //com.MakeFileFromList();
                gsmtask.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                //  CABSerialPorts.SetPortToWait(com.ComPortName, false, null);

            }
        }


        public void ExecuteTCPTask(GSMTaskEntity gsmtask, DateTime currentDate)
        {
            bool finalOutput = false;
            //bool Flag;
            List<GSMTaskEntity> gsmTaskList;
            List<GSMTaskEntity> gsmTaskComleteList;

            GSMLoggingEntity logEntity = new GSMLoggingEntity();

            try
            {
                string message = string.Empty;
                EventLogging.CallLogDetails("Connecting with remote Modem IP : " + meterEntity.MeterGPRSModemIMEI);
                gsmTaskList = new List<GSMTaskEntity>();
                for (int retryCount = 1; retryCount <= gsmtask.taskRetries; retryCount++)
                {
                    logEntity.Retries = retryCount;
                    if (retryCount > 1)
                    {
                        EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Trying to connect TCP again..");
                       // gt.taskStatus = EnumUtil.stringValueOf(TaskStatus.Tryingtoconnectmodem);
                    }

                    Result result = com.OpenSessionTCP();
                    if (result.ErrorCode == CommunicationErrorType.Success && result.RecieveDataBuffer != null)
                    {
                        logEntity.Status = "IP";
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(TaskStatus.Remotemodemconnected);
                        com.GSMLogCreating(logEntity);
                        result = com.Send(gsmtask, GetSelectedProfiles(gsmtask), logEntity);
                    }
                    else
                    {
                        if (logEntity.Retries == retryCount)
                        {
                            logEntity.Status = "NC";
                        }
                        else
                        {
                            logEntity.Status = "IP";
                        }
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        finalOutput = true;
                    }

                    EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Status of the Process : " + finalOutput);

                    if (finalOutput)
                    {
                        gsmTaskComleteList = new List<GSMTaskEntity>();
                        //BhardwajG Remove phone number and dot from the message as it is next phone number.
                        // EventLogging.CallLogDetails(com.ComPortName + ":" + "Communication finished");
                        break;
                    }
                    else
                    {
                        //com.Disconnect();
                        continue;
                    }

                }


            }
            catch (Exception ex)
            {
                // EventLogging.CallLogDetails(com.ComPortName + ":" + "Error Occured during transaction: " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
            }

            finally
            {

                //Make file from file List
                //com.MakeFileFromList();
                gsmtask.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                //  CABSerialPorts.SetPortToWait(com.ComPortName, false, null);

            }
        }



        public void ExecuteFTPTask(GSMTaskEntity gsmtask, DateTime currentDate)
        {
            bool finalOutput = false;
            //bool Flag;
            List<GSMTaskEntity> gsmTaskList;
            List<GSMTaskEntity> gsmTaskComleteList;

            GSMLoggingEntity logEntity = new GSMLoggingEntity();

            try
            {
                string message = string.Empty;
                EventLogging.CallLogDetails("Connecting with FTP Server  : " + meterEntity.MeterGPRSModemIMEI);
                gsmTaskList = new List<GSMTaskEntity>();
                
                for (int retryCount = 1; retryCount <= gsmtask.taskRetries; retryCount++)
                {
                    logEntity.Retries = retryCount;
                    if (retryCount > 1)
                    {
                        EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Trying to connect again..");
                    }
                    //RFTP
                    Result result = com.DownloadFTP(gsmtask, meterEntity.MeterGPRSModemIMEI);
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        logEntity.Status = "C";
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);                       
                    }
                    else
                    {
                        if (logEntity.Retries == retryCount)
                        {
                            logEntity.Status = "NC";
                        }
                        else
                        {
                            logEntity.Status = "IP";
                        }
                        logEntity.ErrorMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                        com.GSMLogCreating(logEntity);
                    }
                    if (result.ErrorCode == CommunicationErrorType.Success)
                    {
                        finalOutput = true;
                    }

                    EventLogging.CallLogDetails(meterEntity.MeterGPRSModemIMEI + ":" + "Status of the Process : " + finalOutput);

                    if (finalOutput)
                    {
                        gsmTaskComleteList = new List<GSMTaskEntity>();                     
                        //BhardwajG Remove phone number and dot from the message as it is next phone number.
                        // EventLogging.CallLogDetails(com.ComPortName + ":" + "Communication finished");
                        break;
                    }
                    else
                    {
                        //com.Disconnect();
                        continue;
                    }

                }


            }
            catch (Exception ex)
            {
                // EventLogging.CallLogDetails(com.ComPortName + ":" + "Error Occured during transaction: " + ex.Message.ToString() + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.InnerException);
            }

            finally
            {

                //Make file from file List
                //com.MakeFileFromList();
                gsmtask.taskStatus = EnumUtil.stringValueOf(TaskStatus.Active);
                //  CABSerialPorts.SetPortToWait(com.ComPortName, false, null);

            }
        }
    }
}
