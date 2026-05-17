#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Hunt.EPIC.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GPRSCommunication;
using CABCommunication.Common;
using System.Net.NetworkInformation;
using CAB.Framework.Utility;

#endregion

namespace CABCommunication.PhysicalLayer
{
    public class TCP : Channel, IPhysicalChannel
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private ChannelInformation channelInfo = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger("TCP Communication");
        public int TCPReceiveSleep = 0;
        public int TCPReceiveTimeOut = 15000;
        public int intercharacterDelay = 2000;
        public int ConnectTimeOut = 25000;
        public int noOfRetry = 2;
        private DateTime timeStamp = DateTime.Now;
        private byte[] receiveBuffer;
        private const int MaxRecieveLength = 5000000;
        private bool readFlag = false;
        private bool dataRecievedFlag;
        private long elapsedMilliseconds;
        private long timeout;

        private int bufferIndex = 0;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Modem IMEI number
        /// </summary>
        private string StaticIP { get; set; }
        private string Port { get; set; }
        TcpClient client = null;
        NetworkStream stream = null;
        IPAddress IPAddress = null;
        public static Result result = new Result();
        //result.ErrorCode = CommunicationErrorType.Nothing;
        private static Dictionary<string, TcpClient> dicIPScoket = new Dictionary<string, TcpClient>();

        //private CABForm lf;
        //public CABForm LF
        //{
        //    get
        //    {
        //        return new CABForm();
        //    }
        //    set
        //    {
        //        lf = value;
        //    }
        //}


        #endregion

        #region Constructors     
        /// <summary>
        /// Default Constructor
        /// </summary>     

        /// <summary>
        /// This operation will be invoked by User to assign channel properties.
        /// </summary>
        /// <param name="channelDetail"></param>
        public TCP(ChannelInformation channelInfo)
        {
            try
            {
                // LF.StatusMessageAsync = new object();
                this.StaticIP = channelInfo.ModemInfo;
                this.Port = channelInfo.TcpPort;

                this.channelInfo = channelInfo;
                this.TCPReceiveTimeOut = Convert.ToInt32(ConfigSettings.GetValue("TCPReceiveTimeout"));
                this.TCPReceiveSleep = Convert.ToInt32(ConfigSettings.GetValue("TCPReceiveSleep"));
                this.ConnectTimeOut = Convert.ToInt32(ConfigSettings.GetValue("ConnectTimeOut"));

            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Debug, ex);
            }
        }
        #endregion

        #region Public Methods

        public override void SetBaud(byte baud)
        {

        }

        private bool GetConnectionState(TcpClient client)
        {
            bool response = false;
            try
            {
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnection = ipProperties.GetActiveTcpConnections();//.Where(x => x.LocalEndPoint.Equals(client.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(client.Client.RemoteEndPoint)).ToArray();
                List<TcpConnectionInformation> tcpConnectionsSelect = new List<TcpConnectionInformation>();
                try
                {
                    foreach (TcpConnectionInformation item in tcpConnection)
                    {
                        if (item.LocalEndPoint.Equals(client.Client.LocalEndPoint) && item.RemoteEndPoint.Equals(client.Client.RemoteEndPoint))
                        {
                            tcpConnectionsSelect.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Debug, ex);
                }

                TcpConnectionInformation[] tcpConnections = tcpConnectionsSelect.ToArray();
                if (tcpConnections != null && tcpConnections.Length > 0)
                {
                    TcpState stateOfConnection = tcpConnections[0].State;
                    if (stateOfConnection == TcpState.Established)
                    {
                        //result.ErrorCode = CommunicationErrorType.TCPModemConnected;

                        // Connection is OK
                        response = true;
                    }
                    else
                    {
                        // No active tcp Connection to hostName:port
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Debug, ex);
            }
            return response;
        }



        private CommunicationErrorType Timeout()
        {
            CommunicationErrorType errorType = CommunicationErrorType.Nothing;
            long elapsedTime = DateTime.Now.Ticks - timeStamp.Ticks;

            TimeSpan objTimeSpan = new TimeSpan(elapsedTime);

            elapsedMilliseconds = Convert.ToInt64(objTimeSpan.TotalMilliseconds);

            if (dataRecievedFlag == true)
            {
                if (elapsedMilliseconds > timeout)
                {
                    errorType = CommunicationErrorType.InterFrameTimeout;
                }
            }
            else
            {
                if (elapsedMilliseconds > timeout)
                {
                    errorType = CommunicationErrorType.ResponseTimeout;
                }

            }
            return errorType;
        }



        private void SocketSend(Socket TCPClient, byte[] bytesToSend)
        {
            try
            {
                TCPClient.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None, new AsyncCallback(this.OnSend), TCPClient);
            }
            catch (SocketException seex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP Send", seex);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP Send", ex);
            }
        }




        private void OnSend(IAsyncResult ar)
        {

            Socket asyncState = (Socket)ar.AsyncState;
            try
            {
                int num = asyncState.EndSend(ar);

            }
            catch (SocketException seex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP OnSend", seex);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP OnSend", ex);
            }
        }


        private void OnReceive(IAsyncResult ar)
        {
            Socket TCPClient = (Socket)ar.AsyncState;
            try
            {
                dataRecievedFlag = true;
                timeout = this.intercharacterDelay;
                timeStamp = DateTime.Now;
                int num = TCPClient.EndReceive(ar);
                if (num > 0)
                {
                    bufferIndex = bufferIndex + num;
                    int nHDLCPktLength = ((receiveBuffer[1] & 0x07) << 8) | (int)receiveBuffer[2];
                    if (receiveBuffer[0] == 0x7E && receiveBuffer[nHDLCPktLength + 1] == 0x7E)
                    {
                        readFlag = true;
                    }
                    //TCPClient.BeginReceive(receiveBuffer, bufferIndex, MaxRecieveLength, SocketFlags.None, new AsyncCallback(this.OnReceive), TCPClient);
                }
            }
            catch (SocketException seex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP OnReceive", seex);
            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Error, "Error while TCP OnReceive", ex);
            }
        }
        public override Result Send(byte[] Buffer, int dataLength)
        {
            Byte[] ReceivedData;
            Result commResult = new Result();
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                commResult.RecieveDataBuffer = new List<byte>();
                if (dicIPScoket.ContainsKey(this.StaticIP))
                {
                    client = dicIPScoket[this.StaticIP];                  
                }
                else
                {
                    dicIPScoket.Add(this.StaticIP, client);
                }
                if (!GetConnectionState(client))
                 {
                //IPAddress address = IPAddress.Parse(this.StaticIP).MapToIPv6();
                //client = new TcpClient(address.AddressFamily.ToString(), Convert.ToInt32(Port));
                 client = new TcpClient(this.StaticIP, Convert.ToInt32(Port));
                 dicIPScoket[StaticIP] = client;
                }
                client.SendTimeout = 7000;                
                stream = client.GetStream();               
                Byte[] SendData = Buffer; // System.Text.Encoding.ASCII.GetBytes(message);
                System.Threading.Thread.Sleep(400);
                stream.Write(SendData, 0, dataLength);
                // Read the TcpServer response bytes.
                ReceivedData = new Byte[999];
                System.Threading.Thread.Sleep(3000);
                client.ReceiveTimeout = 15000;
                Int32 byteLength = stream.Read(ReceivedData, 0, ReceivedData.Length);//Communication receive logic change
                if (byteLength > 0)
                {
                    byte[] response = ReceivedData;
                    commResult.RecieveDataBuffer.AddRange(response);
                    commResult.ErrorCode = CommunicationErrorType.Success;
                    commResult.RecieveDataLength = byteLength;
                }
                else
                {
                    commResult.ErrorCode = CommunicationErrorType.ModemORMeterNotConnected;

                }
            }
            catch (SocketException e)
            {
                commResult.ErrorCode = CommunicationErrorType.RemoteModemNotConnected;
                logger.Log(LOGLEVELS.Debug, e);
                ShutdownSocket(client, stream);
            }
            catch (ArgumentNullException e)
            {
                commResult.ErrorCode = CommunicationErrorType.InvalidCommand;
                logger.Log(LOGLEVELS.Debug, e);
                ShutdownSocket(client, stream);
            }
            catch (Exception e)
            {
                commResult.ErrorCode = CommunicationErrorType.ResponseTimeout;
                logger.Log(LOGLEVELS.Debug, e);
                ShutdownSocket(client, stream);
            }
            return commResult;

        }

        //public override Result Send(byte[] Buffer, int dataLength)
        //{
        //    Result result = new Result();
        //    //TcpClient client = null;
        //    //NetworkStream stream = null;
        //    try
        //    {
        //        //Initial Values
        //        result.ErrorCode = CommunicationErrorType.ResponseTimeout;
        //        result.RecieveDataBuffer = new List<byte>();
        //        byte retries = 1;
        //        readFlag = false;
        //        bufferIndex = 0;
        //        dataRecievedFlag = false;
        //        //Check for already existing client socket
        //        if (dicIPScoket.ContainsKey(this.StaticIP))
        //        {
        //            client = dicIPScoket[this.StaticIP];
        //        }
        //        else
        //        {
        //            dicIPScoket.Add(this.StaticIP, client);
        //        }

        //        //Check client socket connection state
        //        if (!GetConnectionState(client))
        //        {
        //            //Sync Connect
        //            client = new TcpClient(StaticIP, Convert.ToInt32(Port));
        //            logger.Log(LOGLEVELS.Debug, StaticIP + " Waiting for response...");                  
        //            //update client socket in dictionary
        //            dicIPScoket[StaticIP] = client;
        //        }
        //        //write request on socket stream
        //        client.SendTimeout = 5000;
        //        stream = client.GetStream();
        //        //logger.Log(LOGLEVELS.Debug, StaticIP + " Connected...");
        //        Byte[] SendData = Buffer; // System.Text.Encoding.ASCII.GetBytes(message);
        //        System.Threading.Thread.Sleep(300);

        //        stream.Write(SendData, 0, dataLength);
        //        //SocketSend(client.Client, SendData); //for async sending
        //        receiveBuffer = new byte[MaxRecieveLength];
        //        // Read the TcpServer response bytes.                

        //        System.Threading.Thread.Sleep(6000);
        //        client.ReceiveTimeout=6000;
        //        client.Client.BeginReceive(receiveBuffer, bufferIndex, MaxRecieveLength, SocketFlags.None, new AsyncCallback(this.OnReceive), client.Client);

        //        timeStamp = DateTime.Now;
        //        timeout = this.TCPReceiveTimeOut;

        //        do
        //        {
        //            System.Threading.Thread.Sleep(300);
        //            CommunicationErrorType errorType = Timeout();
        //            if (errorType == CommunicationErrorType.ResponseTimeout)
        //            {
        //                if (retries > this.noOfRetry)
        //                {
        //                    break;
        //                }
        //                else
        //                {
        //                    dataRecievedFlag = false;
        //                    retries++;
        //                    bufferIndex = 0;
        //                    readFlag = false;                                         
        //                    System.Threading.Thread.Sleep(300);
        //                    stream.Write(SendData, 0, dataLength);
        //                    receiveBuffer = new byte[MaxRecieveLength];
        //                    System.Threading.Thread.Sleep(this.TCPReceiveSleep);
        //                    timeStamp = DateTime.Now;
        //                    timeout = this.TCPReceiveTimeOut;

        //                }
        //            }
        //            else if (errorType == CommunicationErrorType.InterFrameTimeout)
        //            {
        //                if (readFlag)
        //                {
        //                    result.ErrorCode = CommunicationErrorType.Success;
        //                    for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
        //                    {
        //                        result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
        //                    }
        //                    result.RecieveDataLength = bufferIndex;
        //                    bufferIndex = 0;
        //                    readFlag = false;
        //                }
        //                else
        //                {
        //                    if (bufferIndex > 1)
        //                    {
        //                        for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
        //                        {
        //                            result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
        //                        }
        //                        result.RecieveDataLength = bufferIndex;
        //                        bufferIndex = 0;
        //                        readFlag = false;
        //                        result.ErrorCode = CommunicationErrorType.Success;
        //                    }
        //                    else
        //                    {
        //                        bufferIndex = 0;
        //                        result.ErrorCode = CommunicationErrorType.InterFrameTimeout;
        //                    }

        //                }
        //                break;
        //            }
        //            else
        //            {
        //                if (readFlag == true)
        //                {
        //                    result.ErrorCode = CommunicationErrorType.Success;
        //                    for (int byteIndex = 0; byteIndex < bufferIndex; byteIndex++)
        //                    {
        //                        result.RecieveDataBuffer.Add(receiveBuffer[byteIndex]);
        //                    }
        //                    result.RecieveDataLength = bufferIndex;
        //                    bufferIndex = 0;
        //                    break;

        //                }
        //            }
        //        } while (true);
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorCode = CommunicationErrorType.ResponseTimeout;
        //        logger.Log(LOGLEVELS.Debug, e);
        //        ShutdownSocket(client, stream);
        //        logger.Log(LOGLEVELS.Debug, StaticIP + " Exception, Socket Shutdown...");
        //    }
        //    return result;

        //}

        private void ShutdownSocket(TcpClient client, NetworkStream stream)
        {
            try
            {
                if (client != null)
                {
                    client.Client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    stream.Close();
                }

            }
            catch (Exception ex)
            {
                logger.Log(LOGLEVELS.Debug, ex);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool OpenSession()
        {

            return true;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CloseSession()
        {
            ShutdownSocket(client, stream);
            logger.Log(LOGLEVELS.Debug, StaticIP + " CloseSession, Socket Shutdown...");
            return true;
        }
        #endregion

        #region Private Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

    }
}
