using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LandisGyr.AMI.Layers;
using System.IO;
using DLMSGSMCommunication;
using CAB.DALC.Data;
using CAB.Entity;
using Hunt.EPIC.Logging;
using CAB.Framework;
using GPRSComService.DLMSLIB;

namespace GPRSComService.Tasks
{
    public delegate void TaskStatusChanged();

    public abstract class TaskBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Tasks.TaskBase).ToString());

        private event TaskStatusChanged statusChangedEvent;

        #region "Members"
        private TaskStatus status = TaskStatus.None;
        protected ProfileUtility Profile = null;
        protected int currentStep = 0;
        protected byte[] responseBytes;
        protected List<string> responseList = new List<string>();
        private string responseString;
        protected COSEMLIB objCOSEMLIB = new COSEMLIB();
        protected HDLCLIB objHDLCLIB = new HDLCLIB();
        private int RetryCompleted = 0;
        protected string fileSeparator = string.Empty;
        private string firmWareNumber = string.Empty;
        #endregion

        #region "Constructor"
        public TaskBase()
        {
            statusChangedEvent += new TaskStatusChanged(UpdateTaskStatus);
        }
        #endregion

        #region "Properties"
        public string CommandId { get; set; }
        public int RetryCount { get; set; }

        public TaskStatus Status 
        { 
            get 
            { 
                return status; 
            }
            set
            {
                status = value;
                statusChangedEvent();
            }
        }

        public string StatusMessage { get; set; }
        
        public Int32 TaskId { get; set; }

        public Int32 GroupId { get; set; }

        public string JobName { get; set; }

        public string MeterId { get; set; }

        public string IMEINumber { get; set; }
        
        public string UtilityName { get; set; }
        
        public DateTime? TaskExpiryDate { get; set; }

        public string TaskName { get; set; }

        public int RetryExecuted { get { return RetryCompleted; } set { RetryCompleted = value; } }

        public MeterModels MeterModel { get; set; }

        private string CommandMethodName
        {
            get { return Profile.CommandMethod[currentStep].CommandName; }
        }
        
        private string CommandParameter
        {
            get
            {
                if (!string.IsNullOrEmpty(Profile.CommandMethod[currentStep].CommandParameter))
                {
                    return Profile.CommandMethod[currentStep].CommandParameter;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string ValidatorParameter
        {
            get { return Profile.CommandMethod[currentStep].ValidatorParameter; }
        }

        public bool IsExecutionComplete
        {
            get
            {
                if (Status == TaskStatus.Failed || Status == TaskStatus.Complete)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string GetValidatiorName
        {
            get { return Profile.CommandMethod[currentStep].ValidatorName; }
        }

        private bool isLastStep
        {
            get
            {
                if (currentStep == Profile.CommandMethod.Length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    
        private bool CommandExists
        {
            get
            {
                if (Profile.CommandMethod.Length > currentStep)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsRecursiveCommand
        {
            get { return (currentStep >=Profile.CommandMethod.Length)? false: Profile.CommandMethod[currentStep].IsRecursive; }
        }

        private string RecursiveCommandType
        {
            get { return Profile.CommandMethod[currentStep].RecursiveType; }
        }

        public string FileName
        {
            get;
            set;
        }

        public string FirmwareVersion
        {
            get{ return  string.IsNullOrEmpty(firmWareNumber)?"DEFAULT":firmWareNumber;}
            set { firmWareNumber = value; }
        }
        #endregion

        #region "Abstract Methods"
        protected Profile xmlProfile = null;

        /// <summary>
        /// Initialize the Task Base by populating the CommandMethod Array.
        /// </summary>
        public virtual void init()
        {
            xmlProfile = CommonTaskMethods.GetProfileXML(methodType);
            
            //Filter the Profile for  utility
            IEnumerable<ProfileUtility> utility = xmlProfile.Utility.Where(
                                                name => name.UtilityName.ToUpper().Contains(UtilityName.ToUpper()) 
                                                && name.Firmware.ToUpper().Contains(FirmwareVersion.ToUpper())
                                                );

            if (utility.Count() > 0)
            {
                //Get the Command Array for passed utility
                Profile = utility.ElementAt(0);

                //Filter the Commands based on MeterModel Type
                IEnumerable<ProfileUtilityCommandMethod> commandMethod = utility.ElementAt(0).CommandMethod.Where(metertype => metertype.MeterType.Contains(MeterModel.ToString()));

                //Assign the Filtered Command Array to Profile object
                Profile.CommandMethod = commandMethod.ToArray();
            }
            else
            {
                logger.Log(LOGLEVELS.Error, string.Format("No command structue available for {0} Task, Utility Name: {1}, FirmwareVersion: {2}, MeterModel: {3}",
                    JobName, UtilityName, FirmwareVersion, MeterModel));
            }
        }

        public abstract void AddResponseToOutputString(string response);

        #endregion

        #region "Protected Methods"

        /// <summary>
        /// Increase task expiry timeout to 60 seconds
        /// </summary>
        protected void IncreaseTimeout()
        {
            TaskExpiryDate = DateTime.Now.AddSeconds(Convert.ToInt16(Constants.GetConfigValue(Constants.constCommandExpiryTimeOut)));
        
        }
       /// <summary>
       /// Prepare command object for passed command bytes.
       /// </summary>
       /// <param name="commandByte"></param>
       /// <returns></returns>
        protected RequestOfbase64Binary PrepareCommand(byte[] commandByte)
        {
            RequestOfbase64Binary request = new RequestOfbase64Binary();
            CommandId = request.MessageID = Guid.NewGuid().ToString();

            DeviceInfo[] devices = new DeviceInfo[1];
            DeviceInfo device = new DeviceInfo();
            device.SerialNumber = IMEINumber;
            devices[0] = device;

            request.Devices = devices;
            request.Command = commandByte;

            SLAParameters slaParam = new SLAParameters();
            slaParam.RetryCount = this.RetryCount;
            slaParam.TimeToLive = this.TaskExpiryDate;
            request.SLA = slaParam;

            return request;
        }

        public void WriteToFile()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                try
                {
                    bool flag = false;
                    if (methodType == typeof(GeneralTask))
                    {
                       // flag = true;
                        if (File.Exists(FileName))
                        {
                            try
                            {
                                File.Delete(FileName);
                            }
                            catch (Exception ex)
                            {
                                logger.Log(LOGLEVELS.Error, "Not able to delete existing file.", ex);
                            }
                        }
                    }

                    FileStream file1 = new FileStream(FileName, FileMode.Append);
                    StreamWriter wr1 = new StreamWriter(file1);

                    foreach (string response in responseList)
                    {
                        wr1.WriteLine(response);
                    }
                    //Hard coding: Try to avoid it. It's Hack
                    if (methodType == typeof(BillingTask))
                    {
                        wr1.WriteLine("02" + string.Format("{0:X2}", Convert.ToByte(13)));
                    }

                    if (methodType == typeof(BillingTask) || methodType == typeof(InstantaneousTask) || methodType==typeof(TamperTask) || methodType == typeof(LoadSurveyTask))
                    {
                        wr1.WriteLine("99");
                    }
                    wr1.Close();
                 
                    //UploadFile fileUpload = new UploadFile();
                    //fileUpload.SaveMeterData(FileName,flag);
                    FileUploader fileUpload = new FileUploader();
                    fileUpload.UploadGPRSFile(FileName);
 
                    this.StatusMessage = string.Format(Constants.msgTaskCompletedSuccessfully, JobName);
                    this.Status = TaskStatus.Complete;
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, string.Format("Error in uploading for Taskid:{0}, TaskName: {1}, modemIMEI: {2}, ProfileName:{3} ", TaskId.ToString(), TaskName, IMEINumber, JobName),ex);
                    this.status = TaskStatus.Failed;
                    this.StatusMessage = string.Format(Constants.msgTaskFailedDuringFileUploading, JobName);
                }
            }
        }

        protected Type methodType;
        #endregion

        #region "Private Methods"

        protected T ValidateResponse <T> (byte[] response)
        { 
            object retValue =null;
            try
            {
                MethodInfo info = methodType.GetMethod(GetValidatiorName);
                ParameterInfo[] paramInfo = info.GetParameters();
                retValue = info.Invoke(this, GetParameters(ValidatorParameter, paramInfo));
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, string.Format("Error while in validate method execution for task {0}", IMEINumber), ex);
            }
            return (T)Convert.ChangeType(retValue, typeof(T));
        }

        /// <summary>
        /// Get the next command and put it to RequestCommandQueue
        /// </summary>
        protected void PushCommand()
        {
            try
            {
                //Get the commnad bytes to send.
                byte[] commandByte = GetCommand();

                if (commandByte == null || commandByte.Length == 0)
                {
                    commandByte = GetCommand();

//                    throw new  Exception("Command object is null or command length is 0.");
                }
                //Prepare command object in GND format
                RequestOfbase64Binary command = PrepareCommand(commandByte);

                //Push command to RequestCommandProcessing Queue
                RequestCommandQueue.PushCommand(command);
                logger.Log(LOGLEVELS.Debug,string.Format("Command added for meter{0}, Modem:{1}, Job: {2}, ",MeterId,IMEINumber,JobName));
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, string.Format("Error in Push command for Task id:{0}, TaskName: {1}, modemIMEI: {2}, ProfileName:{3}, StepCount: {4} ", TaskId.ToString(), TaskName, IMEINumber, JobName, currentStep.ToString()),ex);
            }
        }

        /// <summary>
        /// Returns the list of command parameters.
        /// </summary>
        /// <param name="paramString"></param>
        /// <param name="paramInfo"></param>
        /// <returns></returns>
        private object[] GetParameters(string paramString, ParameterInfo[] paramInfo)
        {
            List<object> paramObj = null;
            if (!string.IsNullOrEmpty(paramString))
            {
                paramObj = new List<object>();

                string[] paramList = paramString.Split(',');

                for (int i = 0; i < paramInfo.Length; i++)
                {
                    if (paramInfo[i].ParameterType == typeof(int))
                    {
                        object obj = Convert.ToInt32(paramList[i]);
                        paramObj.Add(obj);
                    }
                    else if (paramInfo[i].ParameterType == typeof(byte))
                    {
                        object obj = Convert.ToByte(paramList[i]);
                        paramObj.Add(obj);
                    }
                    else
                    {
                        object obj = Convert.ToString(paramList[i]);
                        paramObj.Add(obj);
                    }
                }

            }
            return (paramObj == null) ? null : paramObj.ToArray();
        }

        /// <summary>
        /// Return the command bytes
        /// </summary>
        /// <returns></returns>
        private byte[] GetCommand()
        {
            object retValue = null;
            string methodName = string.Empty;
            try
            {
                methodName= CommandMethodName;
                MethodInfo info = methodType.GetMethod(methodName);
                ParameterInfo[] paramInfo = info.GetParameters();
                retValue = info.Invoke(this, GetParameters(CommandParameter, paramInfo));
                logger.Log(LOGLEVELS.Debug,string.Format("Command pushed for method: {0}, meter:{1}, Imei:{2}",methodName,MeterId,IMEINumber)); 
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, string.Format("Error in Get Command for Task id:{0}, TaskName: {1}, modemIMEI: {2}, ProfileName:{3}, StepCount: {4}, methodName: {5}  ", TaskId.ToString(), TaskName, IMEINumber, JobName, currentStep.ToString(),methodName), ex);
            }
            return (byte[])retValue;
        }
        
        #endregion

        #region "Public Method"
        public string OutputString
        {
            get { return responseString; }
            set { responseString = value; }
        }

        /// <summary>
        /// Update the task in database. 
        /// Get the Task entity detail and if it is not null i.e. it is last task then
        /// Update the parent meter details in meter.
        /// </summary>
        public virtual void UpdateTaskStatus()
        {
            if (status == TaskStatus.Complete || status == TaskStatus.Failed || status == TaskStatus.InProgress)
            {

                GSMTaskDAL gsmTaskDAL = new GSMTaskDAL();

                GSMTaskEntity gsmTaskEntity = gsmTaskDAL.UpdateGPRSTask(MeterId, TaskId, GroupId, Status.ToString(), RetryExecuted, StatusMessage, JobName);

                logger.Log(LOGLEVELS.Info, string.Format("updating task id : {0},group id:{1},meter id:{2},status:{3},JobName:{4}", TaskId, GroupId, MeterId, status, JobName));

                if (gsmTaskEntity != null)
                {
                    CAB.BLL.GSMTaskBLL gprsTaskBll = new CAB.BLL.GSMTaskBLL();

                    logger.Log(LOGLEVELS.Info, string.Format("deleting task id : {0},group id:{1},taskstatus:{2}", gsmTaskEntity.taskId, gsmTaskEntity.groupId, gsmTaskEntity.taskStatus));
                    //Delete row from gsm_tasks
                    List<GSMTaskEntity> lstTask = new List<GSMTaskEntity>();
                    lstTask.Add(gsmTaskEntity);

                    gprsTaskBll.deleteGSMTasks(lstTask);

                    // if task is one time task ,don't update new start time
                    if (gsmTaskEntity.taskType != CAB.Framework.EnumUtil.stringValueOf(CAB.Framework.GSMTasksType.OneTimeOnly))
                    {
                        gprsTaskBll.UpdateStartTimeGPRS(gsmTaskEntity);
                        gsmTaskEntity.taskStatus = TaskStatus.Inqueue.ToString();
                        //Insert row in gsm_tasks
                        logger.Log(LOGLEVELS.Info, string.Format("inserting task id : {0},group id:{1},taskstatus:{2},startDate:{3}", gsmTaskEntity.taskId, gsmTaskEntity.groupId, gsmTaskEntity.taskStatus, gsmTaskEntity.startDate));

                        gprsTaskBll.InsertGSMTask(gsmTaskEntity);

                    }
                }
                logger.Log(LOGLEVELS.Info, string.Format("DB Updation completed for for Task: {0}, Meter: {1}, Modem{2}", JobName, MeterId, IMEINumber));

            }
        }

        /// <summary>
        /// Push Response for the task
        /// Method will validate the command and based on nature of response
        /// will push next command.
        /// </summary>
        /// <param name="response"></param>
        public void PushResponse(byte[] response)
        {
    
            responseBytes = response;

            bool isValidated = false;
            bool sendCommandAgain = false;


            //If recursive command
            if (IsRecursiveCommand)
            {
                isValidated = ValidateRecursiveResponse(response, RecursiveCommandType,out sendCommandAgain);
            }
            else
            {
                isValidated = ValidateResponse<bool>(response);
            }

           
            //If command is validated and passed.
            if (isValidated)
            {
                RequestCommandQueue.RemoveCommandFromNewCommandList(CommandId);
                CommandId = string.Empty;

                //Increase the timeout property 
                IncreaseTimeout();

                //If command is not recursive then increment the current step number.
                if (!sendCommandAgain)
                {
                    IncrementStepCount();
                }

                //If ther is next command available for execution. then execute it.
                //else mark the task as DataUploading and sent for data uploadation task.
                if (CommandExists)
                {
                    PushCommand();
                }
                else
                {
                    this.StatusMessage = string.Format(Constants.msgTaskFileUploading, JobName);
                    //Mark the file status as DataUploading
                    this.status = TaskStatus.DataUploading;
                    TaskManager.AddTaskToFileUploadQueue(this);
                }
                
            }   
            else
            {
                //Add commanad id again so that Response can be fetched again by Response processing thread.
                RequestCommandQueue.AddCommandIdToNewCommandList(CommandId);
            }
        }


        /// <summary>
        /// Validates the response for recursive commands.  Returns true if validation is passed else return false.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="recursiveCommandType"></param>
        /// <param name="pushCommandAgain"></param>
        /// <returns></returns>
        private bool ValidateRecursiveResponse(byte[] response, string recursiveCommandType, out bool pushCommandAgain)
        {
            bool retValue = false;
            pushCommandAgain = false;

            //If recursive command type (defined in xml) is "COSEM"
            if (recursiveCommandType.ToUpper() == "COSEM")
            {
                  int validationResponse = ValidateResponse<int>(response);

                  if (validationResponse == 0x01 || validationResponse == 0x07)
                  {
                      retValue = true;
                  }
                  else if (validationResponse == 0x02)
                  {
                      pushCommandAgain = true;
                      retValue = true;
                  }

            }
            //Add if any other Recursive Command Type occurs
            return retValue;
        }


        /// <summary>
        /// Starts the communication on Task by calling Pushcommand method 
        /// also change the status of command to InProgress.
        /// </summary>
        public void StartCommunication()
        {
            //Reset the counter to 0. so that for Retry commands it should start from begining.
            ResetTask();

            this.TaskExpiryDate = DateTime.Now.AddSeconds(Convert.ToInt16(Constants.GetConfigValue(Constants.constCommandExpiryTimeOut)));
            PushCommand();

            this.StatusMessage = string.Format(Constants.msgTaskStatusChangedToInProgress,JobName);
            this.Status = TaskStatus.InProgress;
            logger.Log(LOGLEVELS.Info, string.Format("Communication Started for Task: {0}, Meter: {1}, Modem{2}", JobName, MeterId, IMEINumber)); 
        }

        /// <summary>
        /// Resets the task before starting the communication.
        /// </summary>
        private void ResetTask()
        {
            this.status = TaskStatus.None;
            currentStep = 0;
            responseList = new List<string>();
        }
        protected void IncrementStepCount()
        {
            currentStep++;
        }

        #endregion

        #region "Validators"

        public bool HDLCValidator()
        {
            try
            {
                if (!objHDLCLIB.fCheckStartEndTag(responseBytes))
                {
                    return false;
                }

                if (!objHDLCLIB.fCheckFCS(responseBytes))
                {
                    return false;
                }

                if (!objHDLCLIB.fCheckServerSAP(responseBytes, Constants.nClientSAP))
                {
                    return false;
                }

                if (!objHDLCLIB.fCheckCommand(responseBytes, objHDLCLIB.nCMDByte))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validator method for commands who have block Transfer Response.
        /// </summary>
        /// <returns></returns>
        public int ValidateScalarProfile()
        {
            int retValue = 0;
            objHDLCLIB.fIncRecieve();

            if (HDLCValidator())
            {
                retValue = objCOSEMLIB.fCheckCOSEMResponse(responseBytes);


                if (retValue == 0x01)
                {
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    string strTemp = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    responseList.Add(string.Concat(fileSeparator, strTemp));

                    //Validation for command is Passed. Increment the step count so that next command can be picked.
                    IncrementStepCount();
                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }
            }

            return retValue;
        }

        public byte[] ReadScalarProfileRecursive()
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;
           
            try
            {
                HDLCIndex = PrepareCommandBeforeOBISCode(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fGetBlockTransferPacket(HDLCCommand, HDLCIndex);
                HDLCIndex = PrepareCommandAfterOBISCode(HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
           
        }

      

        /// <summary>
        /// Prepare command byte for SNRM command
        /// </summary>
        /// <returns></returns>
        public byte[] SNRMCommand()
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, Constants.nServerSAP, Constants.nServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, Constants.nClientSAP);
                objHDLCLIB.fSetSNRM();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                objHDLCLIB.fSetUA();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        /// <summary>
        /// Prepare command structure for AARQ command
        /// </summary>
        /// <returns></returns>
        public byte[] AARQCommand()
        {
            try
            {
                byte[] cnfBlock = new byte[3];
                cnfBlock[0] = 0x00;
                cnfBlock[1] = 0x18;
                cnfBlock[2] = 0x1D;
                byte HDLCIndex = 0;
                
                byte[] HDLCCommand = new byte[200];

                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, Constants.nServerSAP, Constants.nServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, Constants.nClientSAP);
                objHDLCLIB.fSetInitialI();

                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (Constants.nSecurityMechanism == 0x01)
                {
                    HDLCIndex = objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x36);
                }
                else
                {
                    HDLCIndex = objCOSEMLIB.fAddAARQTAG(HDLCCommand, HDLCIndex, 0x3E);
                }
                byte nApplicationContext = 0x01;
                HDLCIndex = objCOSEMLIB.fAddContext(HDLCCommand, HDLCIndex, nApplicationContext);
                if (Constants.nSecurityMechanism == 0x01)
                {
                    HDLCIndex = objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, Constants.nSecurityMechanism);
                    HDLCIndex = objCOSEMLIB.fAddPassword(HDLCCommand, HDLCIndex, Constants.nPassword);
                }
                else if (Constants.nSecurityMechanism == 0x02)
                {
                    HDLCIndex = objCOSEMLIB.fAddSecMechanism(HDLCCommand, HDLCIndex, Constants.nSecurityMechanism);
                    HDLCIndex = objCOSEMLIB.fAddRandomKey(HDLCCommand, HDLCIndex, Constants.nPassword);
                }
                HDLCIndex = objCOSEMLIB.fAddUserInf(HDLCCommand, HDLCIndex);
                HDLCIndex = objCOSEMLIB.fAddCnfBlock(HDLCCommand, HDLCIndex, cnfBlock);
                // Changed to support segmentation on behalf of Gopal
                int nPDUSize = 100;// 65535;
                // Changed to support segmentation on behalf of Gopal
                HDLCIndex = objCOSEMLIB.fAddPDUSize(HDLCCommand, HDLCIndex, nPDUSize);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                objHDLCLIB.fIncRecieve();

                Array.Resize(ref HDLCCommand, HDLCIndex );
                return HDLCCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Executes the validation logic for AARQ command
        /// </summary>
        /// <returns></returns>
        public bool AARQCommandValidator()
        {
            if (!HDLCValidator())
            {
                return false;
            }
            if (!objCOSEMLIB.fCheckAARQResponse(responseBytes))
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Executes validation logic for Recursive commands
        /// </summary>
        /// <returns></returns>
        public int ValidateScalarProfileRecursive()
        {
            int retValue = 0;
            if (HDLCValidator())
            {
                retValue = objCOSEMLIB.fCheckCOSEMResponse(responseBytes);
                if (retValue == 0x01)
                {
                    int length = objCOSEMLIB.nBlockTotalByteCount;
                    string strTemp = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        strTemp = strTemp + String.Format("{0:X2}", objCOSEMLIB.BlockBuffer[i]);
                    }
                    responseList.Add(string.Concat(fileSeparator, strTemp));

                }
                else if (retValue == 0x07)
                {
                    responseList.Add(fileSeparator);
                }

            }
            return retValue;
        }

        /// <summary>
        /// Create the packat structure for Disconnect command.
        /// </summary>
        /// <returns></returns>
        public byte[] DISCCommand()
        {
            byte HDLCIndex = 0;
            byte[] HDLCCommand = new byte[200];
            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, Constants.nServerSAP, Constants.nServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, Constants.nClientSAP);
                objHDLCLIB.fSetDISC();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                objHDLCLIB.fSetUA();//Setting Response Command type

            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        /// <summary>
        /// Prepare command structure befor the obis code assignment
        /// </summary>
        /// <param name="HDLCCommand"></param>
        /// <param name="HDLCIndex"></param>
        /// <returns></returns>
        protected byte PrepareCommandAfterOBISCode(byte[] HDLCCommand, byte HDLCIndex)
        {
            // added for Accuracy Check
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
            objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
            objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
            objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);

            return HDLCIndex;

        }

        /// <summary>
        /// Prepare command structure after the obis code assignment.
        /// </summary>
        /// <param name="HDLCCommand"></param>
        /// <param name="HDLCIndex"></param>
        /// <returns></returns>
        protected byte PrepareCommandBeforeOBISCode(byte[] HDLCCommand, byte HDLCIndex)
        {
            HDLCIndex = 0;
            HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, Constants.nServerSAP, Constants.nServerLowerMacAddress);
            HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, Constants.nClientSAP);
            objHDLCLIB.fIncSend();
            HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
            HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
            HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);
            return HDLCIndex;

        }
        #endregion
    }

    public enum TaskStatus
    {
        None,
        Inqueue,
        Failed,
        InProgress,
        Complete,
        DataUploading,
        StartComm,
        UploadInProcess
    }
}
