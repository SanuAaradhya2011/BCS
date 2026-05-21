using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.UI.Controls;
using System.Windows.Forms;

namespace CAB.IECChannel.ReadOut
{
     public enum GSMCommunicationStatus
        {
            ModemConnected,
            ModemDisconnected,
            PortOpenError,
            SendCommandError,
            TimeOut,
            ModemNotResponding,
            NoCarrier,
            ErrorInOpeningPort
      }

   public class ReadoutGSM : ReadBase
    { 
       private Command command;
       private IECChannelBase communications;
       public IECChannelBase Channel
       {
           get { return communications; }
           set { communications = value; }
       } 
       private string ATCommand;
       private string CommandText2;
       private string CommandText3;
       private string CommandText4;
       private string CommandText5;
       private string simNumber;
       public string SIMNumber
       {
           get { return simNumber; }
           set { simNumber = value; CommandText3 = "415444" + GetASCIICode(value) + "0D"; }
       }
       public ReadoutGSM()
       {
           ATCommand = "415445300D";
           CommandText2 = "41540D";
           CommandText4 = "2B2B2B";
           CommandText5 = "4154480D";
           command = Command.GetInstance();
       }
       private string GetASCIICode(string SimNumber)
       {
           string commandText = string.Empty;
           foreach (char ch in SimNumber)
               commandText += Convert.ToInt32(ch).ToString("X2");
           return commandText;
       }

       public GSMCommunicationStatus ConnectToModem()
       {
           GSMCommunicationStatus status = GSMCommunicationStatus.ModemNotResponding;
           try
           {
               communications.BaudRate =  9600;
               if (!communications.OpenPort())
               {
                   this.StatusMessage = MessageConstant.GetText("M000038");
                   Application.DoEvents();
                   return GSMCommunicationStatus.ErrorInOpeningPort;
               }
               
               communications.ReadFlag = false;
               communications.OutBuffer = string.Empty;
               communications.CurrentTime = DateTime.Now;
               communications.IsDataReceived = false;
               communications.CommandID = 0;
               communications.Command = ATCommand;
               communications.CommandTimeout = command.GSMATCommandTimeout;
               communications.InterChatracterDelay = command.GSMATInterCharacterTimeout;
               if (!communications.SendCommand())
               {
                   return GSMCommunicationStatus.SendCommandError;
               }
               else
               {
                   communications.DelayExecution();
                   do
                   { 
                       if (communications.OutBuffer.ToUpper().IndexOf("OK") > 0)
                           break;
                   } while (communications.Timeout() != true);
               }

               if (!communications.IsDataReceived)
               {
                   return GSMCommunicationStatus.TimeOut;
               }
               communications.ReadFlag = false;
               communications.OutBuffer = string.Empty;
               communications.CurrentTime = DateTime.Now;
               communications.IsDataReceived = false;
               communications.CommandID = 0;
               communications.Command = CommandText2;
               communications.CommandTimeout = command.GSMATCommandTimeout;
               communications.InterChatracterDelay = command.GSMATInterCharacterTimeout;
               if (!communications.SendCommand())
               {
                   return GSMCommunicationStatus.SendCommandError;
               }
               else
               {
                   communications.DelayExecution();
                   do
                   {
                       if (communications.OutBuffer.ToUpper().IndexOf("OK") > 0)
                           break;
                   } while (communications.Timeout() != true);
               }

               if (!communications.IsDataReceived)
               {
                   return GSMCommunicationStatus.TimeOut;
               }
               communications.ReadFlag = false;
               communications.OutBuffer = string.Empty;
               communications.CurrentTime = DateTime.Now;
               communications.IsDataReceived = false;
               communications.CommandID = 1;
               communications.Command = CommandText3;
               communications.CommandTimeout = 70000;// command.GSMConnectTimeout;
               communications.InterChatracterDelay = command.GSMConnectInterCharacterTimeout;

               if (!communications.SendCommand())
               {
                   return GSMCommunicationStatus.SendCommandError;
               }
               else
               {
                   communications.DelayExecution();
                   do
                   {
                       if (communications.OutBuffer.ToUpper().IndexOf("CONNEC") > 0)
                           break;
                       if (communications.OutBuffer.ToUpper().IndexOf("NO CARRIER") > 0)
                           break;
                   } while (communications.Timeout() != true);
               }

               if (!communications.IsDataReceived)
                   status = GSMCommunicationStatus.TimeOut;
               else if (communications.OutBuffer.Equals("\r\nCONNECT 9600\r\n") || (communications.OutBuffer.ToUpper().IndexOf("CONNEC") > 0))
                   status = GSMCommunicationStatus.ModemConnected;
               else if (communications.OutBuffer == "\r\nNO CARRIER\r\n" || ("\r\nNO CARRIER\r\n").Contains(communications.OutBuffer))
                   status = GSMCommunicationStatus.NoCarrier;
               else
                   status = GSMCommunicationStatus.ModemNotResponding;
           }
           catch (Exception ex)
           { 
               status = GSMCommunicationStatus.ModemNotResponding;
           }
           finally
           {
               if (communications != null)
               {
                   communications.DelayExecution();
                   communications.ClosePort();
               }
           } 
           return status;
       }

       public GSMCommunicationStatus DisconnectModem()
       {
           GSMCommunicationStatus status = GSMCommunicationStatus.PortOpenError;
           try
           {
               if (!communications.OpenPort())
               {
                   this.StatusMessage = MessageConstant.GetText("M000038");
                   Application.DoEvents();
               }
               communications.BaudRate =9600;
               communications.ReadFlag = false;
               communications.OutBuffer = string.Empty;
               communications.CurrentTime = DateTime.Now;
               communications.IsDataReceived = false;
               communications.CommandID = 0;
               communications.Command = CommandText4;
               communications.CommandTimeout = command.GSMATCommandTimeout;
               communications.InterChatracterDelay = command.GSMATInterCharacterTimeout;
               if (!communications.SendCommand())
                   return GSMCommunicationStatus.SendCommandError;
               else
               {
                   do
                   {
                       if (communications.OutBuffer.ToUpper().IndexOf("OK") > 0)
                           break;
                   } while (communications.Timeout() != true);
               }

               communications.BaudRate =9600;
               communications.ReadFlag = false;
               communications.OutBuffer = string.Empty;
               communications.CurrentTime = DateTime.Now;
               communications.IsDataReceived = false;
               communications.CommandID = 0;
               communications.Command = CommandText5;
               communications.CommandTimeout = command.GSMATCommandTimeout;
               communications.InterChatracterDelay = command.GSMATInterCharacterTimeout;
               if (!communications.SendCommand())
                   return GSMCommunicationStatus.SendCommandError;
               else
               {
                   do
                   {
                       if (communications.OutBuffer.ToUpper().IndexOf("OK") > 0)
                           break;
                   } while (communications.Timeout() != true);
               }

               if (!communications.IsDataReceived)
                   status = GSMCommunicationStatus.TimeOut;
               else
                   status = GSMCommunicationStatus.ModemDisconnected;
           }
           catch (Exception)
           {
               status = GSMCommunicationStatus.ModemNotResponding;
           }
           finally
           {
               communications.DelayExecution();
               communications.ClosePort();
           }
           return status;
       }
    }
}
