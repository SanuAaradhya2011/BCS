using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using System.Windows.Forms;
using CAB.IECChannel;
using CAB.IECChannel.ReadOut;
using CAB.Contracts;
using CABEntity;
using System.Collections.ObjectModel;
using CAB.IECChannel.Programming;
using System.Text;


namespace CHANNEL.Formatter
{
    public class FormatterMeterAccuracyCheck
    {
        IECMeterAccuracyCheckEntity MeterAccuracyCheckEntity = new IECMeterAccuracyCheckEntity();
        public void SplitAccuracyCheck(IECMeterAccuracyCheckEntity MeterAccuracyCheckEntity, string Data)
        {
            
            string CheckData = Data.Substring(Data.IndexOf("/") + 1, 49);
            if (ReadoutCommon.CalculateBcc(CheckData.Substring(0, CheckData.Length), CheckData.Length, Data.Substring(Data.Length - 1, 1)) == false)
            {
                MeterAccuracyCheckEntity.kWh = 0;
                MeterAccuracyCheckEntity.kVAh = 0; ;
                MeterAccuracyCheckEntity.kvarhLag = 0; ;
                MeterAccuracyCheckEntity.kvarhLead = 0; ;
            }
            else
            {
                string kWh = Convert.ToString(Convert.ToInt32(CheckData.Substring(0, 12), 16), 10);
                string kvarhLag = Convert.ToString(Convert.ToInt32(CheckData.Substring(12, 12), 16), 10);
                string kvarhLead = Convert.ToString(Convert.ToInt32(CheckData.Substring(24, 12), 16), 10);
                string kVAh = Convert.ToString(Convert.ToInt32(CheckData.Substring(36, 12), 16), 10);
                MeterAccuracyCheckEntity.kWh = Convert.ToDecimal(kWh) / 1000000;
                MeterAccuracyCheckEntity.kVAh = Convert.ToDecimal(kVAh) / 1000000;
                MeterAccuracyCheckEntity.kvarhLag = Convert.ToDecimal(kvarhLag) / 1000000;
                MeterAccuracyCheckEntity.kvarhLead = Convert.ToDecimal(kvarhLead) / 1000000;
            }
        }
		/// <summary>
        /// Story - 369686 - Accuracy check for single phase IEC meter
		/// </summary>
		/// <param name="MeterAccuracyCheckEntity"></param>
		/// <param name="Data"></param>
        public void SplitAccuracyCheckForSP(IECMeterAccuracyCheckEntity MeterAccuracyCheckEntity, string Data)
        {
            if (Data.Length < 30)
            {
                MeterAccuracyCheckEntity.kWh = 0;
                MeterAccuracyCheckEntity.kVAh = 0;
                MeterAccuracyCheckEntity.kvarhLag = 0;
            }
            else
            {
                string kWh = Convert.ToString(Convert.ToInt32(ReverseString(Data.Substring(2, 8)), 16), 10);
                string kVAh = Convert.ToString(Convert.ToInt32(ReverseString(Data.Substring(12, 8)), 16), 10);
                string kvarhLag = Convert.ToString(Convert.ToInt32(ReverseString(Data.Substring(22, 8)), 16), 10);
                MeterAccuracyCheckEntity.kWh = (Convert.ToDecimal(kWh) / 1000);
                MeterAccuracyCheckEntity.kVAh = Convert.ToDecimal(kVAh) / 1000;
                MeterAccuracyCheckEntity.kvarhLag = Convert.ToDecimal(kvarhLag) / 1000;
            }
        }

        private string ReverseString(string str)
        {
            int count = str.Length - 2;
            string revString = "";
            for (count = str.Length - 2; count >= 0; count -= 2)
            {
                revString += str.Substring(count, 2);
            }
            return revString;
        }
    }
}
