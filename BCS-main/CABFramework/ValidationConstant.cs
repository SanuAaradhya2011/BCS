using System;
using System.Collections.Generic; 
using System.Text;

namespace CAB.Framework.Utility
{
    public class ValidationConstant
    {

        /// <summary>
        /// allow a-z A-Z 0-9 and special character ( space, - hyphen and _ underscore)
        /// </summary>
        public static string RegEx1 = @"^(\w[a-zA-Z0-9 _-]*)$";

        /// <summary>
        /// For UserName with Space
        /// </summary>
        public static string UserName = @"^(\w[A-Za-z ]*)$";

        /// <summary>
        /// Expression to validate A-Z, a-z, 1-9, !,@,#,$ 
        /// </summary>
        public static string Password = @"^(\w[A-Za-z1-9!$#@]*)$";

        /// <summary>
        /// Expression for validating the Search
        /// </summary>
        public static string Search = @"^(\w[A-Za-z1-9]*)$";

        /// <summary>
        /// Expression for validating the Consumer ID
        /// </summary>
        public static string consumerID = @"^([A-Za-z0-9]*)$";

		/// <summary>
		/// Expression for characters,Numbers and space
		/// </summary>
		public static string groupName = @"^([A-Za-z0-9 ]*)$";

        ///// <summary>
        ///// matches whole number between 1 and 100
        ///// </summary>
        //public static string NumberValidation = @"^([1-9]|[1-9]\d|100)$";

        /// <summary>
        /// matches the telephone number
        /// </summary>
        public static string NumberValidation = @"^[+]?\d*$";

        /// <summary>
        /// match two digit number with two decimal places
        /// </summary>
        public static string ContractDemand = @"^\d{1,2}(\x2E\d{1,2}){0,1}$";

        /// <summary>
        /// Expression to validate A-Z, a-z, 0-9, /,#
        /// </summary>
        public static string houseNumber = @"^([A-Za-z0-9 #/,-]*)$";

        /// <summary>
        /// Expression to validate A-Z, a-z, 0-9, space , - , 
        /// </summary>
        public static string city = @"^([A-Za-z0-9 -]*)$";

        /// <summary>
        /// Expression to validate Email
        /// </summary>
        public static string email = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        /// <summary>
        ///Regular expression for searching tamper data
        ///SOH - ascii 0x01
        ///[TM]- for searching 'TM' in tamper
        ///(.*?)- for lazy greedy serach so that it will match the first EOT
        /// </summary>
        public static readonly string TamperExpression = @"(\x01(TM)(.*?)\x03[\w\W]\x04)";

        public static readonly string LCommandResponseExpression = @"\x2F(.*?)\x2F|\x2F(.*?\x03[\w\W]\x04)";
        //regular expression for searching load survey Data
        //SOH - ascii 0x01
        //[Ll]- for searching 'L' or 'l' 
        //(.*?)- for lazy greedy search so that it will match the first EOT
        //\EOT - ascii 0x04

        public static readonly string LoadSurveyExpression = @"(\x01[Ll](.*?)\x03[\w\W]\x04)";
        ////allow A-Z, a-z, 0-9, special characters (_ underscore)
        //    RegEx2 = @"(\w(\s)?)+";

        public static readonly string PhasorExpression = @"(\x01[Pp](.*?)\x03[\w\W]\x04)";

        public static readonly string GeneralDateExpression= @"(\x2FXXX(.*){9}\x0D\x0A)";

        public static readonly string DTMLoadSurveyExpression = @"(\x2FXXX(.*){9}\x0D\x0A)";

        public static readonly string FraudEnergyExpression = @"(\x01(MI)(.*?)\x03[\w\W]\x04)";

        public static readonly string DTMDailyProfileDataExpression = @"(\x01(SD)(.*?)\x03[\w\W]\x04)";

        public static readonly string TOUExpression = @"(\(([\w\W]*?)\))";

        public static readonly string Timeout = @"^([0-9]*)$";
    }
}