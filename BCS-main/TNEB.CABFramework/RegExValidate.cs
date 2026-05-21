using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegExValidation
{
    public class RegExValidate
    {
       
        //class constructor
        public RegExValidate()
        {
            //allow a-z A-Z 0-9 and special character ( space, - hyphen and _ underscore)
            RegEx1 = @"^(\w[a-zA-Z0-9 _-]*)$";

            //allow A-Z, a-z, 0-9, special characters (_ underscore)
            RegEx2 = @"(\w(\s)?)+";

            //matches whole number between 1 and 100
            RegEx3 = @"^([1-9]|[1-9]\d|100)$";

            //matches for all decimal number
            RegEx4 = @"^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$";

            //taken a seven digit number from 0 to 9999999
            RegEx5 = @"^\d{1,7}$";

            //allow A-Z a-z ,space ,'(single quote), -(hyphen)
            RegEx6 = @"^([a-zA-Z '-]+)$";

            //allow only a-z A-Z
            RegEx7 = @"^([a-zA-Z]{3,5})$";

            RegEx8 = @"(\x2FXXX(.*){9}\x0D\x0A)";

            RegEx9 = "";
            RegEx10 = "";

        }

        //properties
        private static string _regEx1 = string.Empty;
        private static string _regEx2 = string.Empty;
        private static string _regEx3 = string.Empty;
        private static string _regEx4 = string.Empty;
        private static string _regEx5 = string.Empty;
        private static string _regEx6 = string.Empty;
        private static string _regEx7 = string.Empty;
        private static string _regEx8 = string.Empty;
        private static string _regEx9 = string.Empty;
        private static string _regEx10 = string.Empty;

        public string RegEx1
        {
            get { return _regEx1; }
            set { _regEx1 = value; }
        }

        public string RegEx2
        {
            get { return _regEx2; }
            set { _regEx2 = value; }
        }

        public string RegEx3
        {
            get { return _regEx3; }
            set { _regEx3 = value; }
        }

        public string RegEx4
        {
            get { return _regEx4; }
            set { _regEx4 = value; }
        }


        public string RegEx5
        {
            get { return _regEx5; }
            set { _regEx5 = value; }
        }


        public string RegEx6
        {
            get { return _regEx6; }
            set { _regEx6 = value; }
        }

        public string RegEx7
        {
            get { return _regEx7; }
            set { _regEx7 = value; }
        }

        public string RegEx8
        {
            get { return _regEx8; }
            set { _regEx8 = value; }
        }
        public string RegEx9
        {
            get { return _regEx9; }
            set { _regEx9 = value; }
        }

        public string RegEx10
        {
            get { return _regEx10; }
            set { _regEx10 = value; }
        }



        //methods
        public bool ValidateRegEx(TextBox t , string regEx)
        {
            if (Regex.Match(t.Text.Trim(), regEx).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateRegEx(string t, string regEx)
        {
            if (Regex.Match(t, regEx).Success == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    
    }
}
