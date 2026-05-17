using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Generic3PhaseCommunication
{
   public class SaveReadoutFile
    {
        public bool SaveRaoutfilePath(string filepath)
        {
            SerialPortSettings.Default.ReadOut = filepath;
            SerialPortSettings.Default.Save();
            return true;
        }

    }
}
