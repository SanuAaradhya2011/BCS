using System.Collections.Generic;
using System.IO.Ports;
using System;
namespace SerialCommunication
{
    /// <summary>
    /// The class contains information about a serial port and a fucntion which will return the next available free port.
    /// </summary>
    public class CABSerialPort
    {
        private bool isBusy;
        private string portName;
        private bool isWaiting;
        private bool isResponding;
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
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        //Yatin 30-Dec-2011
        public bool IsResponding
        {
            get { return isResponding; }
            set { isResponding = value; }
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
        public static IList<CABSerialPort> ListOfSerialPorts
        {
            get { return listOfSerialPorts; }
            set { listOfSerialPorts = value; }
        }

        static CABSerialPorts()
        {
            listOfSerialPorts = GetPorts();
        }
        public static bool GetAvailableModemCOMPort(out string pPortName)
        {
            foreach (CABSerialPort lsp in CABSerialPorts.ListOfSerialPorts)
            {
                if (!lsp.IsWaiting)
                {
                    //serialPort = new SerialPort(lsp.PortName);
                    //if (CheckExistingModemOnComPort(lsp.PortName))
                    //{
                    pPortName = lsp.PortName;
                    return (true);
                    //}
                }
            }

            pPortName = string.Empty;
            return (false);
        }

        private static IList<CABSerialPort> GetPorts()
        {
            //getting PortNames
            string[] PortNames = SerialPort.GetPortNames();
            Array.Sort(PortNames);
            for (int i = 0; i < PortNames.Length; i++)
            {
                CABSerialPort lngComPort = new CABSerialPort();
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
        public static void SetPortToWait(string pPortName, bool pIsWaiting)
        {
            foreach (CABSerialPort objSerialPort in listOfSerialPorts)
            {
                if (objSerialPort.PortName.Equals(pPortName))
                {
                    objSerialPort.IsWaiting = pIsWaiting;
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