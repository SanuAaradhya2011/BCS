using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.Entity;
using CAB.Framework;

namespace CAB.Channel.Formatter 
{
    class DLMS650FormatterAnomaly : ReadBase
    {
        private void OnChannelStatusChange(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }
        public void LoadDiagnosisData(string[] data, BillingGeneralNFDLMSEntity master)
        {
       //    "09 0F 01 01 00 00 00 00 01 01 01010100000000\"
               if(string.IsNullOrEmpty(data[0]))
               {
                   return;
               }
            string anomalyData = data[0];
            AnomalyEntity entity = new AnomalyEntity();
            entity.Flash = getAnomaly(anomalyData,4);
            entity.EeProm = getAnomaly(anomalyData, 6);
            /*GKG - BESCOM PUMA anomaly changes */
            //entity.Smps = getAnomaly(anomalyData, 18);
            //entity.Rtc = getAnomaly(anomalyData, 22);
            entity.Smps = getAnomaly(anomalyData, 8);
            entity.Rtc = getAnomaly(anomalyData, 10);
            /*GKG - BESCOM PUMA anomaly changes */
            master.Anomaly = entity;

        }
        public void LoadDiagnosisData(string[] data, BillingGeneralNFDLMSEntity master,int meterModelNumber)
        {
            //    "09 0F 01 01 00 00 00 00 01 01 01010100000000\"
            if (string.IsNullOrEmpty(data[0]))
            {
                return;
            }
            string anomalyData = data[0];
            AnomalyEntity entity = new AnomalyEntity();
            entity.Flash = getAnomaly(anomalyData, 4);
            entity.EeProm = getAnomaly(anomalyData, 6);
            /*GKG - BESCOM PUMA anomaly changes */
            if (meterModelNumber == NamePlateConstants.RubyE250Value)
            {
                entity.Smps = getAnomaly(anomalyData, 18);
                entity.Rtc = getAnomaly(anomalyData, 22);
            }
            else
            {
                entity.Smps = getAnomaly(anomalyData, 8);
                entity.Rtc = getAnomaly(anomalyData, 10);
            }
            /*GKG - BESCOM PUMA anomaly changes */
            master.Anomaly = entity;

        }

        private int getAnomaly(string data, int dataIndex)
        {
            string value = data.Substring(dataIndex, 2);
            int isFlashEnomaly =0;
            int.TryParse(value,out isFlashEnomaly);
            return  isFlashEnomaly;
        }

    }
}
