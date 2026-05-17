using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;

namespace CAB.IECChannel.Formatter
{
    public class FormatterConstant : IFormatter
    {
        public static int VOLTAGECONVERSIONFACTOR = 100;
        public static int DEMANDCONVERSIONFACTOR = 1000000;
        public static int CURRENTCONVERSIONFACTOR = 1000;
        public static int PFCONVERSIONFACTOR = 10000;
        public static string VOLTAGE = "0.00";
        public static string CURRENT = "0.000";
        public static string POWERFACTOR = "0.00";
        public static string ENERGY = "0.000";
        public static string POWER = "0.000";

        public static string ENTERCARRAGE = "\r\n";
        public static string NEWLINE = "\r\n";
        public static string BILLING = @"(\x01(RD)([\w\W]*?)\x03[\w\W]\x04)";
        public static string HEADERINFO = @"(\x01(HD)([\w\W]*?)\x03[\w\W]\x04)";
        public static string TAMPER = string.Empty;
        public static string METERID = @"(\x2FXXX(.*?)\x2F)";
        public static string METERIDPART1 = @"\x2FXXX5";
        public static string METERIDPART1FOR1PHASE = @"LGC11";
        public static string METERIDPART2 = @"\x2F";
        public static string METERIDPART2FOR1PHASE = @"C11";
        public static string ALLPARAMETER = @"(\(([\w\W]*?)\))";
        public static string MeterIDEXPRESSION = @"(\x2FXXX(.*?)\x2F)";
        //public static string MeterIDEXPRESSIONFOR1PHASE = @"(LGC)(110)(N)(\d)(\d)(\.)(\d)(\d)(.)";
        public static string MeterIDEXPRESSIONFOR1PHASE = @"(LGC)(110)([A-Z])(\d)(\d)(\.)(\d)(\d)(.)"; //for All character support instead of N after LGC110
        public static string VALIDFILECONTENT = @"(\x01[\w\W](.*?)\x03[\w\W]\x04)";
        public static string FRAUDENERGYEXPRESSION = @"(\x01(MI)(.*?)\x03[\w\W]\x04)";
        public static string LCOMMANDEXPRESSION = @"\x2F(.*?)\x2F|\x2F(.*?\x03[\w\W]\x04)";
        public static string LCOMMANDEXPRESSIONFORSPHASE = @"\x2F(.*?)\x2F|\x2F(.*?[\w\W]\x03)";
        public static string PROGRAMMINGEXPRESSSION = @"(\x01(TR)(.*?)\x03[\w\W]\x04)";
        public static string RTCUPDATEEXPRESSSION = @"(\x01(RU)(.*?)\x03[\w\W]\x04)";
        public static string PHASOREXPRESSSION = @"(\x01[Pp](.*?)\x03[\w\W]\x04)";
        public static string LOADSURVEYEXPRESSSION = @"(\x01[Ll](.*?)\x03[\w\W]\x04)";
        public static string TAMPEREXPRESSSION = @"(\x01(TM)(.*?)\x03[\w\W]\x04)";
        public static string TAMPEREXPRESSSIONFORSPHASE = @"(\x01(TM)(.*?)[\w\W]\x03)";
        public static string DTMLOADSURVEYEXPRESSSION = @"(\x01(SA)(.*?)\x03[\w\W]\x04)";
        public static string DTMLOADSURVEYEXPRESSSIONFORSPHASE = @"(\x01(SA)(.*?)[\w\W]\x03)";
        public static string DTMDAILYPROFILEEXPRESSSION = @"(\x01(SD)(.*?)\x03[\w\W]\x04)";        
        public static string DTMDAILYPROFILEEXPRESSSIONFORSPHASE = @"(\x01(SD)(.*?)[\w\W]\x03)";
        public static string NamePlateDetail = @"(\x01(NP)([\w\W]*?)\x03[\w\W]\x04)";
        public static string METERCONFIGURATIONFORSP = @"(\x01(TU)(.*?)[\w\W]\x03)";

    }
}
