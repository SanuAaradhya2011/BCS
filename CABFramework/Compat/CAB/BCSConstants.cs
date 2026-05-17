using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LNG.Framework
{
    public class BCSConstants
    {
        public const string BCS = "BCS";
        public const string IEC = "NONDLMS";        
        public const string History = "History";
        public const string LoadFactorColumn = "Load Factor (%) (0.0.96.1.219.255;3;2)";
        public const string LoadFactorColumn_WB = "Load Factor (%) (1.0.13.0.1.255;3;2)"; // WB tender specific check implemented for Average Load factor OBIS code change
        public const string LoadFactorColumnForSLG = "Load Factor (0.0.96.1.219.255;3;2)";// Story - 365960 - Instant parameters for single phase non dlms

        public const string LoadFactorColumnForImportNet = "Import Load Factor (1.0.1.8.0.255;3;2)";
        public const string LoadFactorColumnForExportNet = "Export Load Factor (1.0.2.8.0.255;3;2)";
        //pradipta_start_081018

        public const string LoadFactorColumnForImportkW = "kW Import Load Factor (%) (1.0.1.0.128.255;3;2)";
        public const string LoadFactorColumnForExportkW = "kW Export Load Factor (%) (1.0.2.0.128.255;3;2)";

        public const string LoadFactorColumnForImportkVA = "kVA Import Load Factor (%)(1.0.9.0.128.255;3;2)";
        public const string LoadFactorColumnForExportkVA = "kVA Export Load Factor (%)(1.0.10.0.128.255;3;2)";
        //pradipta_End_081018

        public const string PowerOnColumn = "Power On Duration (dd:hh:mm)";

        public const string PowerOnDuration_WB = "Power On Duration (0.0.94.91.13.255;3;2) dddd:hh";
        public const string PowerOffDuration_WB = "Power Off Duration (0.0.96.1.217.255.255;3;2) dddd:hh";
        public const string ReaderMode = "Reader(MR)";
        public const string MasterMode = "Master(US)";
        public const string DLMS = "DLMS";

        public const string AverageLoadColumn = "Average Load {0}W (0.0.96.1.198.255)";
        public enum SmartHHU
        {
            GetMeterIDList = 0,
            GetSecurityKeyFile = 1
        }
    }

    public class MeterType
    {
        public const string ThreePhaseThreeWireHTPTCT = "3P-3W HT PT/CT";
        public const string ThreePhaseFourWire = "3P-4W";
        public const string ThreePhaseFourWireHTPTCT = "3P-4W HT PT/CT";
        public const string ThreePhaseFourWireLTCT = "3P-4W LTCT";
        public const string ThreePhaseFourWireWCM = "3P-4W WCM";
        public const string OnePhaseTwoWire = "1P-2W";        
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
    public class MeterVariant
    {
        public const string ONE = "1";
        public const string TWO = "2";
        public const string THREE = "3";
        public const string FOUR = "4";        
    }
    //BhardwajG : Constants used in Nameplate profile
    public class NamePlateConstants
    {
        public static string SapphireS2_NeteMeterModel = string.Empty;
        public const string InvalidModel = "Invalid Meter Model";
        public const string RubyE250 = "E250 - WCM";
        public const string PumaLTE650 = "E650 - LT";
        public const string PumaHTE650 = "E650 - HT";
        public const string LTCTCortex = "E650 - LTCX";
        public const string HTCTCortex = "E650 - HTCX";
        public const string Ruby6Val = "E250 - WCM";
        public const string E350Val = "E350";
        public const string E150Val = "E150 - LGC110";
        public const string WBVal = "E250 - WCM";
        public const string PumaHTE650MW = "E650 - HT";
        public const string CTRatioObisCode = "1.0.0.4.2.255";
        public const string PTRatioObisCode = "1.0.0.4.3.255";
        public const string MeterModelNoObisCode = "0.0.96.0.166.255";
        public const string Sapphire = "E250 - WCM";
        public const string Sapphire_WCM = "E250 - WCM";//3TOU
        public const string TNModel = "E250 - WCM";
        public const string Ruby6ukModel = "E250 - WCM";
        public const string TwoTOUltModel = "E650 - LT";
        public const string WBLTVal = "E650 - LT";
        public const string SapphireSTVal = "E650 - LTCT";
        public const string Sapphire_stVal = "E650 - LTCT";        //Net Metering
        public const string SapphireSTValNET = "E650 - LTCT (I/E)";
        public const string Sapphire_stValNET = "E650 - LTCT (I/E)";
        public const string RubyE250NET = "E250 - WCM (I/E)";
        public const string Ruby6ValNET = "E250 - WCM (I/E)";
        public const string WBValNET = "E250 - WCM (I/E)";
        public const string SapphireNET = "E250 - WCM (I/E)";
        public const string TNModelNET = "E250 - WCM (I/E)";
        public const string Ruby6ukModelNET = "E250 - WCM (I/E)";
        public const string E150ValNet = "E150 - LGC110 (I/E)";
        public const string SFSPValNet = "E150 - LGC110 (I/E)";
        public const string VIMS2Net = "E150 - LGC110 (I/E)";
		public const string SM110Val = "E350 - SM110";// Single phase smart meter
        //******* Smart meter 3 phase WCM  ***********//
        public const string SmartWCM670 = "E350-SM310";
        //******* Smart meter 3 phase LTCT  ***********//
        public const string SmartLTCT670 = "E670-SM405";
        //******* Smart meter 3 phase HTCT for added in future  ***********//
        public const string SmartHTCT670 = "E670-HTCT";
        //*********1Phase DLMS Meter SF with Fast Download
        public const string SFSPVal = "E150 - LGC110";
        public const string VIMS2 = "E150 - LGC110";
        public const string SapphireSHVal = "E250- HTCT";
        public const string SapphireSMVal = "E250- HTCT";
        public const string SapphiresmVal = "E250- HTCT";
        public const string SapphireshVal = "E250- HTCT";
        public const string SapphireS2Val = "E250 - SPS202";
        public const string SmartMeterCipherLTCT = "E670 - LTCT";
        public const string SmartMeterCipherWCM = "E350 - WCM";
        public const string SmartMeterCipher1PH = "E350 - SM110";
        public const string NonLandis = "Non Cabcon Technologies  Meter";
        public const string VIM64KFD = "E150";
        public const int InvalidModelValue = 0;
        public const int RubyE250Value = 1;
        public const int PumaLTE650Value = 2;
        public const int PumaHTE650Value = 3;
        public const int LTCTCortexValue = 4;
        public const int HTCTCortexValue = 5;
        public const int Ruby6Value = 6;
        public const int RubyE350Value = 7;
        public const int RubyE150Value = 8;
        public const int WBValue = 9;
        public const int PumaHTE650MWValue = 10;
        public const int SapphireValue = 11;
        public const int Ruby6ukModelValue = 12;
        public const int TNValue = 13;
        public const int TwoTOUltModelValue = 14;
        public const int WBLTValue = 15;
        public const int VBSPNoSeasonNoWeek = 16; 
        public const int TwoTOUSapphireValue = 17;
        public const int ThreeTOUWCMValue = 33;// FOR 3TOU
        public const int SapphireLTCT = 18;
        //******* Meter Model Change Required Here ***********//
        public const int VFSPNoSeasonNoWeek = 19; // For 1P VIM 64K DLMS with FD
       
        //metermodel 20,21 is added for IEC meter in previous versions
       
       //******* Smart meter 3 phase HTCT  ***********//
        public const int Smartmeter_HTCT = 22;
        //******* Smart meter 3 phase WCM refrence meter model 11  ***********//
        public const int Smartmeter_WCM = 24;
        //******* Smart meter 3 phase LTCT refrence meter model 18  ***********//
        public const int Smartmeter_LTCT = 25; 
        public const int SM110value = 23;// Single phase smart meter
        //*********1Phase DLMS Meter SF with Fast Download
        public const int SFSP = 26;
        public const int Sapphire_SH = 27;
        public const int Sapphire_SM = 28;
        public const int Sapphire_sm = 29;
        public const int Sapphire_sh = 30;
        public const int VIM_Series2 = 31;
        public const int SapphireLTCT_st = 32;
        //******* Smart meter Ciphering 3 phase LTCT ***********//
        public const int SmartM_Cipher_LTCT = 34;
        //******* Smart meter Ciphering 3 phase WCM ***********//
        public const int SmartM_Cipher_WCM = 35;
        //******* Smart meter Ciphering 3 phase HTCT reserve ***********//
        public const int SmartM_Cipher_HTCT = 36;//Reserve for HTCT smart meter unused
        //******* Smart meter Ciphering 1 phase ***********//
        public const int SmartM_Cipher_1PH = 37;
    
        public const int NonLandisMeter = 38;
        //******* VIM BRPL/BYPL with 7 slot TOU ***********//
        public const int BYPL_7Slot = 39;
        public const int BRPL_7Slot = 40;
        public const int BYPL_FD = 41;// For 1P VIM 64K DLMS with FD
        public const int SapphireWCM_St = 42;// For UPCL 
        public const int SapphireS2 = 43;// For Low cost meter
        //******* VIM BRPL/BYPL with 7 slot TOU ***********//
        public const int BRPL_CBSP = 44;    // For 1P VIM 64K DLMS wo FD 7 slot TOU
        //******* Amendment 5 changes meter models ***********//
        public const int Sapphire_Netmeter_WCM = 45;    // For 3P sapphire S2 WCM
        //******* Amendment 5 changes meter models ***********//
        public const int Sapphire_Netmeter_LTCT = 46;    // For 3P sapphire S2 LTCT
        ////******* Amendment 5 changes VIM cost down single phase meter models ***********//
        public const int SapphireS2_NetMeter = 47;// For Low cost meter

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
    /// <summary>
    /// Hold Billing Reset Type Data
    /// Description is meant to be used by for CDF converter 
    /// </summary>
    public enum BillingResetType
    {
        [Description("Auto")]
        Auto ,
        [Description("Command")]
        SManual,
        [Description("Push Button")]
        Manual,
        [Description("No Billing")]
        NoBilling

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
    /// <summary>
    /// Enums used to identify various Type of comunications with CMRI
    /// </summary>
    public enum CMRICommunicationType
    {
        DLMSDumpData = 1,
        DLMSPrerpareCMRI,
        DLMSUpdateCMRIRTC,
        DLMSClearCMRI ,
        IECDumpData,        
        IECPrerpareCMRI
    }
    public class RefVoltageSmartMeter
    {
        public const string Refvolt1 = "240";//1
        public const string Refvolt2 = "230";//2
        public const string Refvolt3 = "63.5";//3

    }
    public class RefCurrentSmartMeter
    {
        public const string ThreePSMLTCT = "05-06";//1
        public const string ThreePSMLTCT1 = "05-10";//2
        public const string ThreePSMWCM = "05-30";//3
        public const string ThreePSMWCM1 = "10-60";//4
        public const string ThreePSMWCM2 = "20-100";//5
        public const string ThreePSMHTCT = "05-06";//6
        public const string ThreePSMHTCT1 = "05-10";//7
        public const string SinglePSM = "10-60";//8
     } 

   
    public class SapphireS2Rating
    {       
        public static Dictionary<string, string> CurrentRatingSapphireS2 = new Dictionary<string, string>()

        {
            { "3", "05-30"},
            { "4", "10-60"},
            { "5", "20-100"},
            { "9", "10-40"},
            { "A", "10-100"},
            { "1", "05-06"},
            { "2", "05-10"},
            { "6", "05-06"},
            { "7", "05-10"},
            { "B", "05-06"},
            { "C", "05-10"},
        };

    }

    public class RTC_ISFormat
    {
        public byte yearhighbyte { get; set; }
        public byte yearlowbyte { get; set; }
        public byte month { get; set; }
        public byte dayofmonth { get; set; }
        public byte dayofweek { get; set; }
        public byte hour { get; set; }
        public byte minute { get; set; }
        public byte second { get; set; }        
        public byte Hundreds { get; set; }
        public byte deviationhighbyte { get; set; }
        public byte deviationlowbyte { get; set; }
        public byte clockstatus { get; set; }
    }

    public class GenericRTC
    {
        public static RTC_ISFormat RTCWRITE(int metermodel)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() {clockstatus=0x00 };

            if (metermodel == NamePlateConstants.SapphireS2 || metermodel == NamePlateConstants.SapphireS2_NetMeter || metermodel == NamePlateConstants.Sapphire_Netmeter_LTCT || metermodel == NamePlateConstants.Sapphire_Netmeter_WCM || metermodel == NamePlateConstants.BYPL_FD || metermodel == NamePlateConstants.SmartM_Cipher_LTCT || metermodel == NamePlateConstants.SmartM_Cipher_WCM || metermodel == NamePlateConstants.SmartM_Cipher_HTCT || metermodel == NamePlateConstants.SmartM_Cipher_1PH)
            {               
                rtc.clockstatus = 0xFF;
            }          
            return rtc;
        }

        public static RTC_ISFormat TOUACTIVATIONWRITE(int metermodel,int DayofWeekVal,int HourVal,int MinVal)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() {dayofweek=0xFF, hour = 0xFF, minute = 0xFF, second = 0xFF, Hundreds = 0xFF, clockstatus = 0x00 };
                       
            if (metermodel == NamePlateConstants.SapphireS2 || metermodel == NamePlateConstants.SapphireS2_NetMeter || metermodel == NamePlateConstants.Sapphire_Netmeter_LTCT || metermodel == NamePlateConstants.Sapphire_Netmeter_WCM || metermodel == NamePlateConstants.PumaLTE650Value || metermodel == NamePlateConstants.BYPL_FD || metermodel == NamePlateConstants.BRPL_CBSP)
            {
                rtc.dayofweek = (byte)DayofWeekVal;
                rtc.hour = (byte)HourVal;
                rtc.minute = (byte)MinVal;
                rtc.second = 0x00;
                rtc.Hundreds = 0x00;
                rtc.clockstatus = 0xFF;
            }

            if (metermodel == NamePlateConstants.SmartM_Cipher_LTCT || metermodel == NamePlateConstants.SmartM_Cipher_WCM || metermodel == NamePlateConstants.SmartM_Cipher_HTCT || metermodel == NamePlateConstants.SmartM_Cipher_1PH)
            {
                rtc.dayofweek = 0xFF;
                rtc.hour = (byte)HourVal;
                rtc.minute = (byte)MinVal;
                rtc.second = 0xFF;
                rtc.Hundreds = 0xFF;
                rtc.clockstatus = 0xFF;
            }

            return rtc;
        }

        public static RTC_ISFormat SEASONPROFILEWRITE(int metermodel)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() { clockstatus = 0x00};

            if (metermodel == NamePlateConstants.SapphireS2  || metermodel == NamePlateConstants.SapphireS2_NetMeter || metermodel == NamePlateConstants.Sapphire_Netmeter_LTCT || metermodel == NamePlateConstants.Sapphire_Netmeter_WCM || metermodel == NamePlateConstants.BYPL_FD || metermodel == NamePlateConstants.PumaLTE650Value || metermodel == NamePlateConstants.BRPL_CBSP || metermodel == NamePlateConstants.SmartM_Cipher_LTCT || metermodel == NamePlateConstants.SmartM_Cipher_WCM || metermodel == NamePlateConstants.SmartM_Cipher_HTCT || metermodel == NamePlateConstants.SmartM_Cipher_1PH)
            {
                rtc.clockstatus = 0xFF;                
            }

            return rtc;
        }

        public static RTC_ISFormat DAYPROFILEWRITE(int metermodel)
        {
            RTC_ISFormat rtc = new RTC_ISFormat() {second = 0x00, Hundreds = 0x00};

            //if (metermodel == NamePlateConstants.SapphireS2 || metermodel == NamePlateConstants.Sapphire_Netmeter_LTCT || metermodel == NamePlateConstants.Sapphire_Netmeter_WCM || metermodel == NamePlateConstants.BYPL_FD || metermodel == NamePlateConstants.PumaLTE650Value || metermodel == NamePlateConstants.SapphireValue)
            //{
            //    rtc.second = 0x00;
            //    rtc.Hundreds = 0x00;
            //}
            if (metermodel == NamePlateConstants.SmartM_Cipher_LTCT || metermodel == NamePlateConstants.SmartM_Cipher_WCM || metermodel == NamePlateConstants.SmartM_Cipher_HTCT || metermodel == NamePlateConstants.SmartM_Cipher_1PH)
            {
                rtc.second = 0xFF;
                rtc.Hundreds = 0xFF;
            }

            return rtc;
        }       

    }
    public class Convertstring
    {
       public static string FormatData(byte[] buffer, bool bUnsignFlag)
       {
        StringBuilder sb = new StringBuilder();

        bool bSignFlag = false;
        Int64 tempVal = 0;
         for (int i = 0; i < buffer.Length; i++)
         {

            if (buffer[0] > 127)
            {

                if (buffer.Length > 1)
                {
                    if (bUnsignFlag) bSignFlag = true;

                }
            }
            sb.Append(buffer[i].ToString("X2"));
         }

         if (bSignFlag == true)
         {
            if (buffer.Length == 4)
            {
                tempVal = Convert.ToInt64("FFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                return "-" + tempVal.ToString();
            }
            else if (buffer.Length == 8)
            {
                tempVal = Convert.ToInt64("FFFFFFFFFFFFFFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                return "-" + tempVal.ToString();
            }
            else
            {
                tempVal = Convert.ToInt32("FFFF", 16) - (Convert.ToInt64(sb.ToString(), 16) - 1);
                return "-" + tempVal.ToString();
            }

         }
         else
         {
            return Convert.ToInt64(sb.ToString(), 16).ToString();
         }
       }

    }
}

