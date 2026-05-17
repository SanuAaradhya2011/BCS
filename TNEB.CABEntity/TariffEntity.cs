/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class TariffEntity : EntityBase
    {
        private long tariff_ID;
        public long Tariff_ID
        {
            get { return tariff_ID; }
            set { tariff_ID = value; }
        }
        private int historyID;
        public int HistoryID
        {
            get { return historyID; }
            set { historyID = value; }
        }
        private string tariff1_kWh;
        public string Tariff1_kWh
        {
            get { return tariff1_kWh; }
            set { tariff1_kWh = value; }
        }
        private string tariff1_kVAh;
        public string Tariff1_kVAh
        {
            get { return tariff1_kVAh; }
            set { tariff1_kVAh = value; }
        }
        private string tariff1_kVARh_lag;
        public string Tariff1_kVARh_lag
        {
            get { return tariff1_kVARh_lag; }
            set { tariff1_kVARh_lag = value; }
        }
        private string tariff1_kVARh_lead;
        public string Tariff1_kVARh_lead
        {
            get { return tariff1_kVARh_lead; }
            set { tariff1_kVARh_lead = value; }
        }
        private string tariff1_MD1;
        public string Tariff1_MD1
        {
            get { return tariff1_MD1; }
            set { tariff1_MD1 = value; }
        }
        private long tariff1_MD1_TimeStamp;
        public long Tariff1_MD1_TimeStamp
        {
            get { return tariff1_MD1_TimeStamp; }
            set { tariff1_MD1_TimeStamp = value; }
        }

        private string tariff1_MD2;
        public string Tariff1_MD2
        {
            get { return tariff1_MD2; }
            set { tariff1_MD2 = value; }
        }
        private long tariff1_MD2_TimeStamp;
        public long Tariff1_MD2_TimeStamp
        {
            get { return tariff1_MD2_TimeStamp; }
            set { tariff1_MD2_TimeStamp = value; }
        }
        private string tariff1_Aver_PF;
        public string Tariff1_Aver_PF
        {
            get { return tariff1_Aver_PF; }
            set { tariff1_Aver_PF = value; }
        }
        private string tariff2_kWh;
        public string Tariff2_kWh
        {
            get { return tariff2_kWh; }
            set { tariff2_kWh = value; }
        }
        private string tariff2_kVAh;
        public string Tariff2_kVAh
        {
            get { return tariff2_kVAh; }
            set { tariff2_kVAh = value; }
        }
        private string tariff2_kVARh_lag;
        public string Tariff2_kVARh_lag
        {
            get { return tariff2_kVARh_lag; }
            set { tariff2_kVARh_lag = value; }
        }
        private string tariff2_kVARh_lead;
        public string Tariff2_kVARh_lead
        {
            get { return tariff2_kVARh_lead; }
            set { tariff2_kVARh_lead = value; }
        }
        private string tariff2_MD1;
        public string Tariff2_MD1
        {
            get { return tariff2_MD1; }
            set { tariff2_MD1 = value; }
        }
        private long tariff2_MD1_TimeStamp;
        public long Tariff2_MD1_TimeStamp
        {
            get { return tariff2_MD1_TimeStamp; }
            set { tariff2_MD1_TimeStamp = value; }
        }
        private string tariff2_MD2;
        public string Tariff2_MD2
        {
            get { return tariff2_MD2; }
            set { tariff2_MD2 = value; }
        }
        private long tariff2_MD2_TimeStamp;
        public long Tariff2_MD2_TimeStamp
        {
            get { return tariff2_MD2_TimeStamp; }
            set { tariff2_MD2_TimeStamp = value; }
        }

        private string tariff2_Aver_PF;
        public string Tariff2_Aver_PF
        {
            get { return tariff2_Aver_PF; }
            set { tariff2_Aver_PF = value; }
        }
        private string tariff3_kWh;
        public string Tariff3_kWh
        {
            get { return tariff3_kWh; }
            set { tariff3_kWh = value; }
        }
        private string tariff3_kVAh;
        public string Tariff3_kVAh
        {
            get { return tariff3_kVAh; }
            set { tariff3_kVAh = value; }
        }
        private string tariff3_kVARh_lag;
        public string Tariff3_kVARh_lag
        {
            get { return tariff3_kVARh_lag; }
            set { tariff3_kVARh_lag = value; }
        }
        private string tariff3_kVARh_lead;
        public string Tariff3_kVARh_lead
        {
            get { return tariff3_kVARh_lead; }
            set { tariff3_kVARh_lead = value; }
        }
        private string tariff3_MD1;
        public string Tariff3_MD1
        {
            get { return tariff3_MD1; }
            set { tariff3_MD1 = value; }
        }
        private long tariff3_MD1_TimeStamp;
        public long Tariff3_MD1_TimeStamp
        {
            get { return tariff3_MD1_TimeStamp; }
            set { tariff3_MD1_TimeStamp = value; }
        }
        private string tariff3_MD2;
        public string Tariff3_MD2
        {
            get { return tariff3_MD2; }
            set { tariff3_MD2 = value; }
        }
        private long tariff3_MD2_TimeStamp;
        public long Tariff3_MD2_TimeStamp
        {
            get { return tariff3_MD2_TimeStamp; }
            set { tariff3_MD2_TimeStamp = value; }
        }

        private string tariff3_Aver_PF;
        public string Tariff3_Aver_PF
        {
            get { return tariff3_Aver_PF; }
            set { tariff3_Aver_PF = value; }
        }
        private string tariff4_kWh;
        public string Tariff4_kWh
        {
            get { return tariff4_kWh; }
            set { tariff4_kWh = value; }
        }
        private string tariff4_kVAh;
        public string Tariff4_kVAh
        {
            get { return tariff4_kVAh; }
            set { tariff4_kVAh = value; }
        }
        private string tariff4_kVARh_lag;
        public string Tariff4_kVARh_lag
        {
            get { return tariff4_kVARh_lag; }
            set { tariff4_kVARh_lag = value; }
        }
        private string tariff4_kVARh_lead;
        public string Tariff4_kVARh_lead
        {
            get { return tariff4_kVARh_lead; }
            set { tariff4_kVARh_lead = value; }
        }
        private string tariff4_MD1;
        public string Tariff4_MD1
        {
            get { return tariff4_MD1; }
            set { tariff4_MD1 = value; }
        }
        private long tariff4_MD1_TimeStamp;
        public long Tariff4_MD1_TimeStamp
        {
            get { return tariff4_MD1_TimeStamp; }
            set { tariff4_MD1_TimeStamp = value; }
        }
        private string tariff4_MD2;
        public string Tariff4_MD2
        {
            get { return tariff4_MD2; }
            set { tariff4_MD2 = value; }
        }
        private long tariff4_MD2_TimeStamp;
        public long Tariff4_MD2_TimeStamp
        {
            get { return tariff4_MD2_TimeStamp; }
            set { tariff4_MD2_TimeStamp = value; }
        }

        private string tariff4_Aver_PF;
        public string Tariff4_Aver_PF
        {
            get { return tariff4_Aver_PF; }
            set { tariff4_Aver_PF = value; }
        }
        private string tariff5_kWh;
        public string Tariff5_kWh
        {
            get { return tariff5_kWh; }
            set { tariff5_kWh = value; }
        }
        private string tariff5_kVAh;
        public string Tariff5_kVAh
        {
            get { return tariff5_kVAh; }
            set { tariff5_kVAh = value; }
        }
        private string tariff5_kVARh_lag;
        public string Tariff5_kVARh_lag
        {
            get { return tariff5_kVARh_lag; }
            set { tariff5_kVARh_lag = value; }
        }
        private string tariff5_kVARh_lead;
        public string Tariff5_kVARh_lead
        {
            get { return tariff5_kVARh_lead; }
            set { tariff5_kVARh_lead = value; }
        }
        private string tariff5_MD1;
        public string Tariff5_MD1
        {
            get { return tariff5_MD1; }
            set { tariff5_MD1 = value; }
        }
        private long tariff5_MD1_TimeStamp;
        public long Tariff5_MD1_TimeStamp
        {
            get { return tariff5_MD1_TimeStamp; }
            set { tariff5_MD1_TimeStamp = value; }
        }
        private string tariff5_MD2;
        public string Tariff5_MD2
        {
            get { return tariff5_MD2; }
            set { tariff5_MD2 = value; }
        }
        private long tariff5_MD2_TimeStamp;
        public long Tariff5_MD2_TimeStamp
        {
            get { return tariff5_MD2_TimeStamp; }
            set { tariff5_MD2_TimeStamp = value; }
        }

        private string tariff5_Aver_PF;
        public string Tariff5_Aver_PF
        {
            get { return tariff5_Aver_PF; }
            set { tariff5_Aver_PF = value; }
        }
        private string tariff6_kWh;
        public string Tariff6_kWh
        {
            get { return tariff6_kWh; }
            set { tariff6_kWh = value; }
        }
        private string tariff6_kVAh;
        public string Tariff6_kVAh
        {
            get { return tariff6_kVAh; }
            set { tariff6_kVAh = value; }
        }
        private string tariff6_kVARh_lag;
        public string Tariff6_kVARh_lag
        {
            get { return tariff6_kVARh_lag; }
            set { tariff6_kVARh_lag = value; }
        }
        private string tariff6_kVARh_lead;
        public string Tariff6_kVARh_lead
        {
            get { return tariff6_kVARh_lead; }
            set { tariff6_kVARh_lead = value; }
        }
        private string tariff6_MD1;
        public string Tariff6_MD1
        {
            get { return tariff6_MD1; }
            set { tariff6_MD1 = value; }
        }
        private long tariff6_MD1_TimeStamp;
        public long Tariff6_MD1_TimeStamp
        {
            get { return tariff6_MD1_TimeStamp; }
            set { tariff6_MD1_TimeStamp = value; }
        }
        private string tariff6_MD2;
        public string Tariff6_MD2
        {
            get { return tariff6_MD2; }
            set { tariff6_MD2 = value; }
        }
        private long tariff6_MD2_TimeStamp;
        public long Tariff6_MD2_TimeStamp
        {
            get { return tariff6_MD2_TimeStamp; }
            set { tariff6_MD2_TimeStamp = value; }
        }

        private string tariff6_Aver_PF;
        public string Tariff6_Aver_PF
        {
            get { return tariff6_Aver_PF; }
            set { tariff6_Aver_PF = value; }
        }
        private string tariff7_kWh;
        public string Tariff7_kWh
        {
            get { return tariff7_kWh; }
            set { tariff7_kWh = value; }
        }
        private string tariff7_kVAh;
        public string Tariff7_kVAh
        {
            get { return tariff7_kVAh; }
            set { tariff7_kVAh = value; }
        }
        private string tariff7_kVARh_lag;
        public string Tariff7_kVARh_lag
        {
            get { return tariff7_kVARh_lag; }
            set { tariff7_kVARh_lag = value; }
        }
        private string tariff7_kVARh_lead;
        public string Tariff7_kVARh_lead
        {
            get { return tariff7_kVARh_lead; }
            set { tariff7_kVARh_lead = value; }
        }
        private string tariff7_MD1;
        public string Tariff7_MD1
        {
            get { return tariff7_MD1; }
            set { tariff7_MD1 = value; }
        }
        private long tariff7_MD1_TimeStamp;
        public long Tariff7_MD1_TimeStamp
        {
            get { return tariff7_MD1_TimeStamp; }
            set { tariff7_MD1_TimeStamp = value; }
        }
        private string tariff7_MD2;
        public string Tariff7_MD2
        {
            get { return tariff7_MD2; }
            set { tariff7_MD2 = value; }
        }
        private long tariff7_MD2_TimeStamp;
        public long Tariff7_MD2_TimeStamp
        {
            get { return tariff7_MD2_TimeStamp; }
            set { tariff7_MD2_TimeStamp = value; }
        }

        private string tariff7_Aver_PF;
        public string Tariff7_Aver_PF
        {
            get { return tariff7_Aver_PF; }
            set { tariff7_Aver_PF = value; }
        }
        private string tariff8_kWh;
        public string Tariff8_kWh
        {
            get { return tariff8_kWh; }
            set { tariff8_kWh = value; }
        }
        private string tariff8_kVAh;
        public string Tariff8_kVAh
        {
            get { return tariff8_kVAh; }
            set { tariff8_kVAh = value; }
        }
        private string tariff8_kVARh_lag;
        public string Tariff8_kVARh_lag
        {
            get { return tariff8_kVARh_lag; }
            set { tariff8_kVARh_lag = value; }
        }
        private string tariff8_kVARh_lead;
        public string Tariff8_kVARh_lead
        {
            get { return tariff8_kVARh_lead; }
            set { tariff8_kVARh_lead = value; }
        }
        private string tariff8_MD1;
        public string Tariff8_MD1
        {
            get { return tariff8_MD1; }
            set { tariff8_MD1 = value; }
        }
        private long tariff8_MD1_TimeStamp;
        public long Tariff8_MD1_TimeStamp
        {
            get { return tariff8_MD1_TimeStamp; }
            set { tariff8_MD1_TimeStamp = value; }
        }
        private string tariff8_MD2;
        public string Tariff8_MD2
        {
            get { return tariff8_MD2; }
            set { tariff8_MD2 = value; }
        }
        private long tariff8_MD2_TimeStamp;
        public long Tariff8_MD2_TimeStamp
        {
            get { return tariff8_MD2_TimeStamp; }
            set { tariff8_MD2_TimeStamp = value; }
        }

        private string tariff8_Aver_PF;
        public string Tariff8_Aver_PF
        {
            get { return tariff8_Aver_PF; }
            set { tariff8_Aver_PF = value; }
        }
        private long meterData_ID;
        public long MeterData_ID
        {
            get { return meterData_ID; }
            set { meterData_ID = value; }
        }
        private string cumulativeExportEnergyKWH;
        public string CumulativeExportEnergyKWH
        {
            get { return cumulativeExportEnergyKWH; }
            set { cumulativeExportEnergyKWH = value; }
        }
        private string cumulativeExportEnergyKVAH;
        public string CumulativeExportEnergyKVAH
        {
            get { return cumulativeExportEnergyKWH; }
            set { cumulativeExportEnergyKWH = value; }
        }
        public long BillingTimeStamp { get; set; }
    }
}
