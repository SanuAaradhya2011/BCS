using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.IECChannel.ReadOut
{
    public class ReadoutConstant
    {
        public static string NOCARRIER = "NO CARRIER";
        public static string METERPASSWORD = "00000000";//Meter Id changed from "11111111" to "00000000"
        public static string PASSWORD = "Pass";
        public static string TAMPER = "TR";
        public static string REGISTER = "RU";
        public static string TRANSACTION = "Sep";
        public static string BCC = "Bcc";
        public static string CRETURNENTER = "\r\n";
        public static string PHASOR = "P";
        public static string MAGNETICINFLUENCE = "MI";
        public static string DTMDAILYPROFILE = "SD";
        public static string METERCONFIGURATION = "TU";
        public static string DTMPROFILE = "SA";
        public static string DTMPROFILENUMBER = "3030";
        public static string METERPASSWORDASCII = "3030303030303030";//Meter Id changed from "11111111" to "00000000"
        public static string DATA = "Data";
        public static string BAUDRATE = "BaudRate";
    }
}
