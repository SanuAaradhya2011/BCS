#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.Framework.Utility;
using CAB.BLL;
using CAB.Framework;
using CABCommunication.Common;
using CAB.Serialization;
using CAB.Parser;
using System.Threading;
using CABCommunication.PhysicalLayer;
using CABCommunication.WrapperLayer;
using Hunt.EPIC.Logging;
#endregion

namespace CAB.UI
{
    /// <summary>
    /// This form is used for selecting readout/communication mode form BCS .
    /// </summary>
    public partial class E650MeterPassword : MdiChildForm
    {

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(E650Settings).ToString());
        private const string ReaderMode = "Reader(MR)";
        private const string MasterMode = "Master(US)";
       private Communication communication;
        #endregion      

        #region Constructor

        public E650MeterPassword()
        {
            InitializeComponent();
        }
        #endregion       

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.StatusMessage = string.Empty;
            Application.DoEvents();
            this.Close();
        }

        private void btnWriteMeterPassword_Click(object sender, EventArgs e)
        {
            try
            {                
            this.StatusMessage = "";
            Application.DoEvents();
            Thread.Sleep(2000);
            Result result = new Result();
            String MRPass = "3131313131313131";
            ProfileCommand selectedCommand;    
            ChannelInformation channelInfo = new ChannelInformation();
            DisableControls();
            string mode = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism")) == 1 ? ReaderMode : MasterMode;
            if (mode == ReaderMode)
            {
                btnWriteMeterPassword.Enabled = false;
            }
            channelInfo.CommunicationMode = ConfigSettings.GetValue("ChannelType");
            channelInfo.ComPort = ConfigSettings.GetValue("PortName");
            channelInfo.SecurityMechanism = Convert.ToByte(ConfigSettings.GetValue("SecurityMechanism"));
            channelInfo.Password = ConfigSettings.GetValue("ModePassword");
            channelInfo.ProtocolType = "DLMS";
            communication = new Communication(channelInfo);
            result = communication.OpenSession();
            result.ErrorCode = CommunicationErrorType.Success;
              
            if (result.ErrorCode == CommunicationErrorType.Success)
            {  
                List<ProfileCommand> lstProfileCommands;
                lstProfileCommands = GetProfileCommandEntity();
                List<System.Enum> selectedProfiles= new List<System.Enum>();
                selectedProfiles.Add(ProfileId.MRPasswordWrite);   
                foreach (ProfileId selectedConfigId in selectedProfiles)
                {
                    List<ProfileCommand> profileCommand = lstProfileCommands.FindAll(delegate(ProfileCommand profileCommandEntity)
                    {
                        return profileCommandEntity.TagNumber == (int)selectedConfigId
                        && (Convert.ToString(profileCommandEntity.MeterModelNumber) == ConfigInfo.MeterModel ||
                        profileCommandEntity.MeterModelNumber == 0);
                    });

                    if (selectedConfigId == ProfileId.MRPasswordWrite)
                    {
                        ProfileCommand mrCommand = new ProfileCommand(15, "00.00.28.00.02.FF", 7);
                        mrCommand.ClassName = "CAB.E650MeterConfiguration.MRMode,E650MeterConfiguration";
                        profileCommand.Add(mrCommand);
                    }
                    if (profileCommand.Count > 0)
                    {
                        selectedCommand = profileCommand[0];
                        selectedCommand.Action = ActionType.WRITE;
                        this.StatusMessage = "Writing MR Password" + " ...";
                        result.ErrorCode = CommunicationErrorType.Nothing;
                        profileCommand[0].WriteDataBuffer = GetPasswordBytes(MRPass);
                        result = communication.Send(profileCommand[0]);
                        
                    }
                }

                if (result != null && result.ErrorCode == CommunicationErrorType.Success)
                {
                    if (result.RecieveDataBuffer == null )
                        this.StatusMessage = "Writing MR Password Failed.";
                    else if (result.RecieveDataBuffer[3] == 0x00)
                    {                                      
                        this.StatusMessage = "MR Password Written Successfully.";
                        Application.DoEvents();
                    }
                    else
                        this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);       
                }
                else
                    this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                                           
                         
            }
            else
            {
                this.StatusMessage = CommonBLL.GetEnumDescription(result.ErrorCode);
                Application.DoEvents();
            }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "btnWriteMeterPassword", ex);
            }
            finally
            {
                
                communication.CloseSession();
                EnableControls();            
            }
        }

        private void DisableControls()
        {

            btnWriteMeterPassword.Enabled = false;
            
           
        }
        private void EnableControls()
        {

            btnWriteMeterPassword.Enabled = true;
            

        }
        private List<ProfileCommand> GetProfileCommandEntity()
        {
            DLMS profileCommands = (DLMS)new Serializer().DeserializeToObject("CommandRepository.xml", typeof(DLMS));
            List<ProfileCommand> lstProfileCommands = new List<ProfileCommand>();
            ProfileCommand profileCommandEntity;
            foreach (DLMSCOMMAND dlmsCommand in profileCommands.Items)
            {
                profileCommandEntity = new ProfileCommand();
                profileCommandEntity.TagNumber = Convert.ToInt32(dlmsCommand.TAGNO);
                profileCommandEntity.Attribute = Convert.ToByte(dlmsCommand.ATTRIBUTE);
                profileCommandEntity.ClassId = Convert.ToByte(dlmsCommand.CLASS);
                profileCommandEntity.ObisCode = dlmsCommand.OBISCODE;
                profileCommandEntity.ClassName = dlmsCommand.CLASSNAME;
                lstProfileCommands.Add(profileCommandEntity);

            }
            return lstProfileCommands;
        }
        private List<byte> GetPasswordBytes(string passwordstr)
        {
            //string passwordstr = txtpwd.Text;
            byte[] MasterKeyArr = new byte[16];
            byte[] SecuritykeyArr = new byte[16];
            byte[] Wraptext = new byte[24];
            string KeyCipher = string.Empty;
            List<byte> pwdbyte = new List<byte>();
            int countlen = 0;
            while (countlen < passwordstr.Length)
            {
                pwdbyte.Add(Convert.ToByte(passwordstr.Substring(countlen, 2), 16));
                countlen += 2;
            }          

            return pwdbyte;
        }

       


       

     

      


    }
}
