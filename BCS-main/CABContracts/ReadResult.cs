using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CAB.Contracts
{
    public class ReadResult
    {
        public string CommandName
        {
            get;
            set;
        }
        public string Result
        {
            get;
            set;
        }

        public MeterConfigurationConfigSection section { get; set; }
    }
}
