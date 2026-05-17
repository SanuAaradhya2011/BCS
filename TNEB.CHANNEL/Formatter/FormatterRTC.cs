using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECChannel.ReadOut;

namespace CHANNEL.Formatter
{
    public class FormatterRTC
    {
        public string ParseRTC(string configData, MeterConfigurationConfigSection configSection)
        {
            string tempData = configData.Substring(configData.IndexOf("|") + 2, configData.Length - configData.IndexOf("|") - 2);

           // bool Bccres = ReadoutCommon.CalculateBcc(tempData.Substring(1), tempData.Length - 3, tempData.Substring(tempData.Length - 1, 1));
            //if (Bccres == true)
            //{
                tempData = tempData.Substring(0, 2) + "/" + tempData.Substring(2, 2) + "/" + tempData.Substring(4, 2) + " " + tempData.Substring(6, 2) + ":" + tempData.Substring(8, 2) + ":" + tempData.Substring(10, 2);
                DateTime meterRTC = new DateTime();
                if (!DateTime.TryParse(tempData, new System.Globalization.CultureInfo("en-GB"), System.Globalization.DateTimeStyles.None, out meterRTC))
                    return null;//this.StatusMessage = "Invalid RTC.";
                else
                    return tempData;
           // }
            //else
              //  return null;

        }
    }
}
