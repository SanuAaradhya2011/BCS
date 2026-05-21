using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.IECFramework
{
    public class BCSConstants
    {
        public const string BCS = "BCS";
    }
    public class Symbols
    {
        public const string COMMA = ",";
        public const string HYPHEN = "-";
        public const string SPACE = " ";
        public const string COLON = ":";
        public const string SEMICOLON = ";";
        public const string AMPERSAND = "&";
        public const string LEFTBRACKET = "(";
        public const string RIGHTBRACKET = ")";
        public const string NEWLINE = "\n";
    }
    public class COMMessages
    {
        public const string COSEMCONNECTIONFAILED = "Cosem Connection Failed.";
        public const string CMRICONNECTIONFAILED = "CMRI Connection Failed.";
        public const string ACCESSDENIED = "Access Denied.";
        public const string ERROR = "Error";
        public const string HDLCCONNECTIONFAILED = "HDLC Connection Failed.";
    }
    //BhardwajG : Constants used in Nameplate profile
    public class NamePlateConstants
    {
        public const string InvalidModel = "Invalid Meter Model";
        public const string RubyE250 = "E250 - WCM";
        public const string PumaLTE650 = "E650 - LT";
        public const string PumaHTE650 = "E650 - HT";
        public const string CTRatioObisCode = "1.0.0.4.2.255";
        public const string PTRatioObisCode = "1.0.0.4.3.255";
        public const string MeterModelNoObisCode = "0.0.96.0.166.255";
        public const int RubyE250Value = 1;
        public const int PumaLTE650Value = 2;
        public const int PumaHTE650Value = 3;

    }
    public enum ActivityCalender
    { 
        ActiveSeasonProfile = 3,
        ActiveWeekProfile,
        ActiveDayProfile,
        PassiveSeasonProfile=7,
        PassiveWeekProfile,
        PassiveDayProfile,
        ActivationDate

    }
   
    public class TOUConstants
    {
        public const byte Array = 1;
        public const byte Structure = 2;
        public const byte MaxSeason = 4;
        public const byte MaxWeek = 4;
        public const byte MaxDay = 24;
        public const byte MaxWeekCol = 7;
        public const byte MaxDayRow = 10;

    }
}
