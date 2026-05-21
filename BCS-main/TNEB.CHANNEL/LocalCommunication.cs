using System;
using System.IO.Ports;
using CAB.IECFramework.Utility;
namespace CAB.IECChannel
{
    public class IECLocalCommunication : IECChannelBase
    {
        //static int i = 0;
        public IECLocalCommunication()
        {            
            this.InterChatracterDelay = CAB.IECFramework.Command.GetInstance().LocalCommunicationInterChatracterDelay;
            this.CommandTimeout = CAB.IECFramework.Command.GetInstance().LocalCommunicationCommandTimeout;
            this.InterCommandDelay = CAB.IECFramework.Command.GetInstance().LocalCommunicationInterCommandDelay;
        }
        //public override void OnPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        //{

            //System.Diagnostics.Debug.Print((i++).ToString());
            //try
            //{
            //    IsDataReceived = true;
            //    ExistingBuffer = string.Empty;
            //    int bufferIndex;

            //    if (CommandID == 0)
            //    {
                   
            //        ReadBytes();
            //        if (TotalReadBytes >= 27)
            //        {
            //            ReadFlag = true;
            //            FlagBCC = false;
            //            return;
            //        }
            //    }
            //    if (CommandID == 2)
            //    {
            //        CurrentTime = DateTime.Now;
            //        TotalReadBytes = TotalReadBytes + ComPort.BytesToRead;
            //        ExistingBuffer = ComPort.ReadExisting();
            //        OutBuffer = string.Concat(OutBuffer, ExistingBuffer);
            //        IsDataReceived = true;
            //        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //        Byte[] totalByte = encoding.GetBytes(ExistingBuffer);
            //        int index = 0;
            //        if (!FlagBCC)
            //        {
            //            foreach (byte bytes in totalByte)
            //            {
            //                if (bytes == 3)
            //                {
            //                    if (totalByte.Length > index + 1)
            //                    {
            //                        ReadFlag = true;
            //                        FlagBCC = false;
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        FlagBCC = true;
            //                        return;
            //                    }
            //                }
            //                index++;
            //            }
            //        }
            //        else
            //        {
            //            ReadFlag = true;
            //            FlagBCC = false;
            //            return;
            //        }
            //    }
            //    if (CommandID == 3)
            //    {
            //        ReadBytes();
            //        if (TotalReadBytes >= 1 && OutBuffer.ToString().Equals(Convert.ToChar(6).ToString()))
            //            this.ReadFlag = true;
            //        TotalReadBytes = 0;
            //    }
            //    if (CommandID == 4)
            //    {
            //        ReadBytes();
            //        if (TotalReadBytes >= 80)
            //            ReadFlag = true;
            //    }
            //    if (CommandID == 9)
            //    {
            //        ReadBytes();
            //        bufferIndex = ExistingBuffer.IndexOf("t");
            //        if (bufferIndex > 0)
            //            bufferIndex = ExistingBuffer.IndexOf("kt");
            //        if (bufferIndex >= 0)
            //        {
            //            PacketCount = 1;
            //            Command = "06";
            //            SendCommand();
            //        }
            //        bufferIndex = ExistingBuffer.IndexOf("PktEnd");
            //        if (bufferIndex >= 0) ReadFlag = true;
            //    }
            //}
            //catch (Exception)
            //{
            //    //new CABException( ex);
            //}
        //}
    }
}
