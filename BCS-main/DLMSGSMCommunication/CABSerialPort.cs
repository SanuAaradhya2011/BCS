using System.Collections.Generic;
using System.IO.Ports;
using System;
using CAB.BLL;
using CAB.Entity;
using CAB.Framework;
using Utilities;
using CABCommunication.PhysicalLayer;
using System.Threading;
namespace DLMSGSMCommunication
{
    /// <summary>
    /// The class contains information about a serial port and a fucntion which will return the next available free port.
    /// </summary>
    public class CABSerialPort : Serial
    {
        private bool isBusy;
        private string portName;
        private bool isWaiting;
        private bool isResponding;
        private string simNumber;
        private CABWorkerGSM lngWorkerGSM = null;
        private AutoResetEvent waitEvent = null;
        /// <summary>
        /// Added constructor for intializing base class.
        /// </summary>
        /// <param name="comPort"></param>
        public CABSerialPort(string comPort)
            : base(comPort)
        {
            PortName = comPort;
            waitEvent = new AutoResetEvent(false);
        }
        public string SimNumber
        {
            get { return simNumber; }
            set { simNumber = value; }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }
        public bool IsWaiting
        {
            get { return isWaiting; }
            set { isWaiting = value; }
        }
        //Yatin 30-Dec-2011
        public bool IsResponding
        {
            get { return isResponding; }
            set { isResponding = value; }
        }
        public CABWorkerGSM CABWorkerGSM
        {
            get { return lngWorkerGSM; }
            set { lngWorkerGSM = value; }

        }
        /// <summary>
        /// wait event for waiting in the main thread until the current thread completes
        /// </summary>
        public AutoResetEvent WaitEvent
        {
            get { return waitEvent; }
            set { waitEvent = value; }
        }
        //public static bool GetAvailableFreePort(out string portName)
        //{
        //    // Serial Port object which opens the port will return isOpen = true
        //    // not the object you have made anonymousloy and assigneing the port name.

        //    SerialPort lngComPort = null;
        //    bool isAvailable = false;
        //    portName = string.Empty;

        //    //getting PortNames
        //    string[] PortNames = SerialPort.GetPortNames();

        //    for (int i = 0; i < PortNames.Length; i++)
        //    {
        //        lngComPort = new SerialPort();
        //        lngComPort.PortName = PortNames[i].ToString();
        //        if (!lngComPort.IsOpen)
        //        {
        //            portName = lngComPort.PortName;
        //            isAvailable = true;
        //            break;
        //        }
        //    }
        //    return isAvailable;
        //}

        public static bool GetAvailableFreePort(out string portName)
        {
            bool isAvailable = false;
            portName = string.Empty;

            IList<CABSerialPort> listOfSerialPort = CABSerialPorts.ListOfSerialPorts;
            foreach (CABSerialPort lngComPort in listOfSerialPort)
            {
                if (!lngComPort.isBusy)
                {
                    portName = lngComPort.portName;                    
                    isAvailable = true;
                    break;
                }
            }
            return isAvailable;
        }


    }

    /// <summary>
    /// The class will hold functions related to available free ports and will keep the
    /// ports udpated in a collection.i.e if port is busy currectly or free
    /// </summary>
    public static class CABSerialPorts
    {
        private static IList<CABSerialPort> listOfSerialPorts = new List<CABSerialPort>();
        static SystemSettingsBLL systemSystemSettingsBLL = new SystemSettingsBLL();
        public static IList<CABSerialPort> ListOfSerialPorts
        {
            get { return listOfSerialPorts; }
            set { listOfSerialPorts = value; }
        }

        static CABSerialPorts()
        {
            //pick port from the system rather than database
            listOfSerialPorts = GetBCSPorts();
        }
        public static bool AllPortsFree()
        {
            foreach (CABSerialPort lsp in CABSerialPorts.ListOfSerialPorts)
            {
                if (lsp.IsWaiting)
                {
                    return false;
                    //}
                }
            }
            return true;
        }
        /// <summary>
        /// Gets an event array associated with the cuurent serila port object
        /// </summary>
        /// <returns></returns>
        public static AutoResetEvent[] GetAllEvents()
        {
            AutoResetEvent[] events = new AutoResetEvent[listOfSerialPorts.Count];
            for (int counter = 0; counter < listOfSerialPorts.Count; counter++)
            {
                events[counter] = listOfSerialPorts[counter].WaitEvent;
            }
            return events;
        }
        public static bool GetAvailableModemCOMPort(out string pPortName)
        {
            foreach (CABSerialPort lsp in CABSerialPorts.ListOfSerialPorts)
            {
                if (!lsp.IsWaiting)
                {
                    pPortName = lsp.PortName;
                    return (true);
                    //}
                }
            }

            pPortName = string.Empty;
            return (false);
        }
        public static CABSerialPort GetCABSerialPort(string pPortName)
        {
            foreach (CABSerialPort lsp in CABSerialPorts.ListOfSerialPorts)
            {
                if (lsp.PortName == pPortName)
                {
                    return lsp;
                    //}
                }
            }


            return null;
        }
        private static IList<CABSerialPort> GetPorts()
        {
            //getting PortNames
            string[] PortNames = SerialPort.GetPortNames();
            Array.Sort(PortNames);
            
            ////if more than one ports are present leave the first port
            //if (PortNames.Length > 1)
            //{
            //    i = 1;
            //}
            for (int i = 0; i < PortNames.Length; i++)
            {
                CABSerialPort lngComPort = new CABSerialPort(PortNames[i]);
                lngComPort.PortName = PortNames[i];
                lngComPort.IsBusy = false;
                listOfSerialPorts.Add(lngComPort);
            }
            return listOfSerialPorts;
        }
        private static IList<CABSerialPort> GetBCSPorts()
        {
            //getting PortNames
            string isMultiplePort = systemSystemSettingsBLL.GetSettingValue(SystemSettings.USE_MULTIPLE_PORTS);
            string[] PortNames = null;
            if (!string.IsNullOrEmpty(isMultiplePort))
            {
                if (isMultiplePort == "1")
                {
                    PortNames = systemSystemSettingsBLL.GetSettingValue(SystemSettings.GSM_COM_PORTS).Split(',');
                }
                else
                {
                    PortNames = systemSystemSettingsBLL.GetSettingValue(SystemSettings.COM_PORT).Split(',');
                }
            }
            //string[] PortNames = systemSystemSettingsBLL.GetSettingValue(SystemSettings.GSM_COM_PORTS).Split(',');
            Array.Sort(PortNames);
            for (int i = 0; i < PortNames.Length; i++)
            {
                CABSerialPort lngComPort = new CABSerialPort(PortNames[i]);
                lngComPort.PortName = PortNames[i];
                lngComPort.IsBusy = false;
                listOfSerialPorts.Add(lngComPort);
            }
            return listOfSerialPorts;
        }
        public static void UpdateSerialPortList(string portName, bool isBusy)
        {
            foreach (CABSerialPort serialPort in listOfSerialPorts)
            {
                if (serialPort.PortName.Equals(portName))
                {
                    serialPort.IsBusy = isBusy;
                }
            }
        }

        //Yatin 14-Dec-2011
        public static CABSerialPort SetPortToWait(string pPortName, bool pIsWaiting, ChannelDetail channel)
        {
            try
            {
                for (int counter = 0; counter < listOfSerialPorts.Count; counter++)
                {
                    if (listOfSerialPorts[counter].PortName.Equals(pPortName))
                    {
                        if (pIsWaiting)
                        {                            
                            if (channel != null)
                            {
                                listOfSerialPorts[counter] = new CABSerialPort(pPortName);
                                listOfSerialPorts[counter].SetChannel(channel);
                                if (channel.Parity.ToLower().Equals("none"))
                                {
                                    listOfSerialPorts[counter].ReadBufferSize = 4096;
                                }
                            }
                            //objSerialPort.SetSerialPortSettings("9600", "None", "8", "1", MultipleSerialPortSettings.Default.CommandTimeOut, MultipleSerialPortSettings.Default.IntercharacterDelay);
                            if (listOfSerialPorts[counter].OpenSessionWithDelay())
                            {
                                listOfSerialPorts[counter].IsWaiting = true;
                            }

                        }
                        else
                        {
                            if (listOfSerialPorts[counter].CloseSessionWithDelay())
                            {
                                listOfSerialPorts[counter].IsWaiting = false;
                            }
                            listOfSerialPorts[counter].WaitEvent.Set();
                        }
                        return listOfSerialPorts[counter];
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogging.CallLogDetails("exception occured While setting port " + ex.Message + "Stack Trace" + ex.StackTrace);
            }
            return null;
        }
        public static void ReleaseResources()
        {
            foreach (CABSerialPort objSerialPort in listOfSerialPorts)
            {
                //// if the port is busy and service is closed, then close remote session and then close the port. 
                //if (objSerialPort.IsWaiting)
                //{
                //    objSerialPort.CloseRemoteSession();
                //}
                if (objSerialPort.CloseSessionWithDelay())
                    objSerialPort.IsWaiting = false;
            }
        }
        public static void SetPortToWait(string pPortName, bool pIsWaiting, string simNumber, CABWorkerGSM worker)
        {
            foreach (CABSerialPort objSerialPort in listOfSerialPorts)
            {
                if (objSerialPort.PortName.Equals(pPortName))
                {
                    objSerialPort.IsWaiting = pIsWaiting;
                    objSerialPort.SimNumber = simNumber;
                    objSerialPort.CABWorkerGSM = worker;
                }
            }
        }
        //Yatin 30-Dec-2011
        public static void SetPortRespondingStatus(string pPortName, bool pIsResponding)
        {
            foreach (CABSerialPort objSerialPort in listOfSerialPorts)
            {
                if (objSerialPort.PortName.Equals(pPortName))
                {
                    objSerialPort.IsResponding = pIsResponding;
                }
            }
        }
    }
}