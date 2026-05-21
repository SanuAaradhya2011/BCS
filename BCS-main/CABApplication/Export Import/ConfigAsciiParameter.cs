using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CABApplication.Export_Import
{
    public class ConfigAsciiParameter
    {
        
        public string ProfileName
        {
            set;
            get;
        }

        public string TableName
        {
            set;
            get;
        }

        public List<String> ParameterName
        {
            set;
            get;
        }

        public List<String> DBname
        {
            set;
            get;
        }

    }
}
