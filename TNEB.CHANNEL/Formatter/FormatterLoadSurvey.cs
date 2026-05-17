/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CAB.Entity;
using CAB.IECChannel.ReadOut;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
namespace CAB.IECChannel.Formatter
{
    public class LoadSurveyTable
    {
        public int srno;
        public string strtablename;
        public int decfactor;
        public string strformatter;
        public int paramlength;

        public LoadSurveyTable(int nsrno, string ntablename, int nfactor, string nformatter, int nparamlength)
        {
            srno = nsrno;
            strtablename = ntablename;
            decfactor = nfactor;
            strformatter = nformatter;
            paramlength = nparamlength;
        }
    }
    public class FormatterLoadSurvey
    {
        private bool RPhaseVoltage = false;
        private bool YPhaseVoltage = false;
        private bool BPhaseVoltage = false;
        private bool RPhaseCurrent = false;
        private bool YPhaseCurrent = false;
        private bool BPhaseCurrent = false;
        private bool AverageCurrent = false;
        private bool AverageVoltage = false;
        private bool DemandKVARLead = false;
        private bool DemandKVARLag = false;
        private bool DemandKVA = false;
        private bool DemandKW = false;
        private bool PowerFactor = false;
        private bool TamperStatus = false;
        private string Parameters = "";
        private IList<LoadSurveyTable> LslookUpTable;

        public FormatterLoadSurvey()
        {
            SetupLookUpTable();
        }

        private void SetupLookUpTable()
        {
            LslookUpTable = new List<LoadSurveyTable>
                (
                  new[]
                    {
                                new LoadSurveyTable(1,            "kWh",                                  1000,        "0.000",          4),
                                new LoadSurveyTable(2,            "kVAh",                                 1000,        "0.000",          4),
                                //new LoadSurveyTable(0,            "TimeStamp",                            0,              null,             8),
                                new LoadSurveyTable(3,            "kVArh/kVArh(Lag)",                     1000,        "0.00",          4),
                                new LoadSurveyTable(4,            "kVArh/kVArh(Lead)",                    1000,        "0.00",          4),
                                new LoadSurveyTable(5,            "Voltage Min",                          1000,        "0.00",          4),
                                new LoadSurveyTable(6,            "Voltage Max",                          1000,        "0.00",          4),
                                new LoadSurveyTable(7,            "Voltage Avg",                          100,         "0.00",          4),
                                new LoadSurveyTable(8,            "Avg Phase Current/ Current",           1000,        "0.00",          4),
                                new LoadSurveyTable(9,            "Avg Neutral Current/ Current",         1000,        "0.00",          4),
                                new LoadSurveyTable(10,           "Avg PF",                               1000,        "0.00",          4),
                                new LoadSurveyTable(11,           "Avg Current",                          100,         "0.00",          4),
                                //new LoadSurveyTable(12,           "Reserve",                              1000.0,        "000.00",          4),
                                //new LoadSurveyTable(13,           "Reserve",                              1000.0,        "000.00",          4),
                    }
              );
        }

        public int[] GetData(string data, ref string[,] loadSurvey)
        {
            try
            {
                int counter = 0;
                data = data.Replace(FormatterConstant.ENTERCARRAGE, string.Empty);
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.LOADSURVEYEXPRESSSION);
                string[] programmingData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    programmingData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateData(programmingData);
                counter = 0;
                int MaxLength = 0;
                int[] loadSurveyDaysCnt = new int[] { };
                if (availableData.Length > 0)
                    loadSurveyDaysCnt = new int[availableData.GetUpperBound(0) + 1];
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    loadSurveyDaysCnt[counter] = tempData.Length;
                    for (int z = tempData.Length - 1; z >= 0; z--)
                    {
                        if (!string.IsNullOrEmpty(tempData[z]))
                            break;
                        loadSurveyDaysCnt[counter]--;
                    }

                    if (tempData.Length > MaxLength)
                    {
                        MaxLength = tempData.Length;
                        if (string.IsNullOrEmpty(tempData[MaxLength - 1]))
                            MaxLength--;
                    }
                    counter++;
                }
                loadSurvey = new string[counter, MaxLength];
                counter = 0;
                //  int s = 0, e = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSION).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                    {
                        if (!string.IsNullOrEmpty(tempData[MaxLength]))
                            loadSurvey[counter, MaxLength] = tempData[MaxLength];

                    }
                    counter++;
                }
                //  if(loadSurveyDaysCnt.Length >0)
                return loadSurveyDaysCnt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CheckParameter(int val1, int val2, int val)
        {
            try
            {
                InitializeLoadSurveyParameters();
                Parameters = string.Empty;
                if ((val1 & (int)Math.Pow(2, 0)) != 0) { RPhaseVoltage = true; Parameters = Parameters + "," + "RPhaseVoltage as 'Voltage R Phase'"; }
                if ((val1 & (int)Math.Pow(2, 1)) != 0) { YPhaseVoltage = true; Parameters = Parameters + "," + "YPhaseVoltage as 'Voltage Y Phase'"; }
                if ((val1 & (int)Math.Pow(2, 2)) != 0) { BPhaseVoltage = true; Parameters = Parameters + "," + "BPhaseVoltage as 'Voltage B Phase'"; }
                if ((val1 & (int)Math.Pow(2, 3)) != 0) { RPhaseCurrent = true; Parameters = Parameters + "," + "RPhaseCurrent as 'Current R Phase'"; }
                if ((val1 & (int)Math.Pow(2, 4)) != 0) { YPhaseCurrent = true; Parameters = Parameters + "," + "YPhaseCurrent as 'Current Y Phase'"; }
                if ((val1 & (int)Math.Pow(2, 5)) != 0) { BPhaseCurrent = true; Parameters = Parameters + "," + "BPhaseCurrent as 'Current B Phase'"; }
                if ((val1 & (int)Math.Pow(2, 6)) != 0) { AverageVoltage = true; Parameters = Parameters + "," + "AvgVoltage as 'Average Voltage'"; }
                if ((val1 & (int)Math.Pow(2, 7)) != 0) { AverageCurrent = true; Parameters = Parameters + "," + "AvgCurrent as 'Average Current'"; }
                if ((val2 & (int)Math.Pow(2, 0)) != 0) { DemandKVARLead = true; Parameters = Parameters + "," + "DemandKVARLead as 'Demand kvar (lead)'"; }
                if ((val2 & (int)Math.Pow(2, 1)) != 0) { DemandKVA = true; Parameters = Parameters + "," + "DemandKVA as 'Demand kVA'"; }
                if ((val2 & (int)Math.Pow(2, 2)) != 0) { DemandKW = true; Parameters = Parameters + "," + "DemandKW as 'Demand kW'"; }
                if ((val2 & (int)Math.Pow(2, 3)) != 0) { DemandKVARLag = true; Parameters = Parameters + "," + "DemandKVARLag as 'Demand kvar (lag)'"; }
                if ((val2 & (int)Math.Pow(2, 4)) != 0) { PowerFactor = true; Parameters = Parameters + "," + "PowerFactor"; }
                if (val == 1) TamperStatus = true;
            }
            catch (Exception) { }
        }

        private void InitializeLoadSurveyParameters()
        {
            RPhaseVoltage = false;
            YPhaseVoltage = false;
            BPhaseVoltage = false;
            RPhaseCurrent = false;
            YPhaseCurrent = false;
            BPhaseCurrent = false;
            AverageVoltage = false;
            AverageCurrent = false;
            DemandKVARLead = false;
            DemandKVA = false;
            DemandKW = false;
            DemandKVARLag = false;
            PowerFactor = false;
        }

        public void SplitData(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity, int loadSurveyDaysCnt)
        {
            try
            {
                if (tempData == null)
                    return;
                bool Flag = true;
                int index = 0;
                int interval = 0;
                long meterReadingDatetime = 0;

                LoadSurveyData loadSurveyData = new LoadSurveyData();
                loadSurveyData.LoadSurvey = new List<IECLoadSurveyEntity>();
                loadSurveyData.LoadSurveyMeterData = new IECMeterDataEntity();


                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter > loadSurveyDaysCnt - 1)
                        continue;

                    if (counter == 1)
                    {
                        //billingGeneralNFEntity.LoadSurveyMeterData.MeterID = tempData[counter].Substring(4).Trim();
                        loadSurveyData.LoadSurveyMeterData.MeterID = tempData[counter].Substring(4).Trim();
                        continue;
                    }
                    else if (counter == 2)
                    {
                        //billingGeneralNFEntity.LoadSurveyMeterData.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        loadSurveyData.LoadSurveyMeterData.ReadingDateTime = Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    //else if (counter == 3)
                    //{
                    //    billingGeneralNFEntity.LoadSurveyMeterData.CMRIID = Convert.ToString(tempData[counter]).Substring(1, 8);
                    //    string cmriType = Convert.ToString(tempData[counter]).Substring(0, 1);
                    //    if (cmriType.Trim().ToUpper().Equals("A"))
                    //        billingGeneralNFEntity.LoadSurveyMeterData.CMRIType = "Analogic";
                    //    else if (cmriType.Trim().ToUpper().Equals("S"))
                    //        billingGeneralNFEntity.LoadSurveyMeterData.CMRIType = "Sands";
                    //    else
                    //        billingGeneralNFEntity.LoadSurveyMeterData.CMRIType = "BCS";
                    //}
                    else if (counter == 3)
                    {
                        byte b = Convert.ToByte(tempData[counter].Substring(0, 2));
                        if (b == 1)
                            interval = 15;
                        else if (b == 2)
                            interval = 30;
                        else if (b == 3)
                            interval = 60;
                        meterReadingDatetime = FormatterCommon.LongDateTime(tempData[counter], 2);
                        int val1 = Convert.ToInt32(tempData[counter].Substring(14, 2), 16);
                        int val2 = Convert.ToInt32(tempData[counter].Substring(16, 2), 16);
                        int val3 = Convert.ToInt32(tempData[counter].Substring(18, 2), 16);
                        CheckParameter(val1, val2, val3);
                    }
                    else
                    {
                        if (tempData[counter] == null)
                            return;
                        if (counter < tempData.GetUpperBound(0))
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 2, tempData[counter].Substring(tempData[counter].Length - 1, 1));
                        if (!Flag)
                            Flag = ReadoutCommon.CalculateBcc(tempData[counter], tempData[counter].Length - 3, tempData[counter].Substring(tempData[counter].Length - 2, 1));
                        if (!Flag)
                            return;
                        index = 0;
                        do
                        {
                            IECLoadSurveyEntity loadSurvey = new IECLoadSurveyEntity();
                            if (RPhaseVoltage) { loadSurvey.RPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 8, 100, "{0:0.00}"); index += 8; }
                            if (YPhaseVoltage) { loadSurvey.YPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 8, 100, "{0:0.00}"); index += 8; }
                            if (BPhaseVoltage) { loadSurvey.BPhaseVoltage = FormatterCommon.ParseData(tempData[counter], index, 8, 100, "{0:0.00}"); index += 8; }
                            if (RPhaseCurrent) { loadSurvey.RPhaseCurrent = FormatterCommon.ParseData(tempData[counter], index, 8, 1000, "{0:0.000}"); index += 8; }
                            if (YPhaseCurrent) { loadSurvey.YPhaseCurrent = FormatterCommon.ParseData(tempData[counter], index, 8, 1000, "{0:0.000}"); index += 8; }
                            if (BPhaseCurrent) { loadSurvey.BPhaseCurrent = FormatterCommon.ParseData(tempData[counter], index, 8, 1000, "{0:0.000}"); index += 8; }
                            if (AverageVoltage) { loadSurvey.AvgVoltage = FormatterCommon.ParseData(tempData[counter], index, 8, 100, "{0:0.00}"); index += 8; }
                            if (AverageCurrent) { loadSurvey.AvgCurrent = FormatterCommon.ParseData(tempData[counter], index, 8, 1000, "{0:0.000}"); index += 8; }
                            if (DemandKVARLead) { loadSurvey.DemandKVARLead = FormatterCommon.ParseData(tempData[counter], index, 8, 1000000, "{0:0.000}"); index += 8; }
                            if (DemandKVA) { loadSurvey.DemandKVA = FormatterCommon.ParseData(tempData[counter], index, 8, 1000000, "{0:0.000}"); index += 8; }
                            if (DemandKW) { loadSurvey.DemandKW = FormatterCommon.ParseData(tempData[counter], index, 8, 1000000, "{0:0.000}"); index += 8; }
                            if (DemandKVARLag) { loadSurvey.DemandKVARLag = FormatterCommon.ParseData(tempData[counter], index, 8, 1000000, "{0:0.000}"); index += 8; }
                            if (PowerFactor) { loadSurvey.PowerFactor = FormatterCommon.ParseData(tempData[counter], index, 8, 10000, "{0:0.000}"); index += 8; }
                            if (TamperStatus) { loadSurvey.TamperStatus = tempData[counter].Substring(index, 6); index += 6; }
                            loadSurvey.MeterReadingDatetime = meterReadingDatetime;
                            loadSurvey.LoadSurveyDateTime = FormatterCommon.LongDateTime(tempData[counter], index);
                            index += 10;
                            loadSurvey.MDIntervalPeriod = interval;
                            loadSurvey.Parameters = Parameters;

                            loadSurveyData.LoadSurvey.Add(loadSurvey);
                            //billingGeneralNFEntity.LoadSurvey.Add(loadSurvey);

                        } while (Convert.ToChar(tempData[counter].Substring(index, 1)) != Convert.ToChar(0x3));
                    }
                }
                billingGeneralNFEntity.listLoadSurveyData.Add(loadSurveyData);

            }
            catch (Exception ex)
            {
                //   MessageBox.Show("Corrupted Load Survey data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GetDataForSPhase(string data, ref  string[,] dtmLoadSurvey)
        {
            try
            {
                int counter = 0;
                MatchCollection matches = FormatterCommon.ValidateData(data, FormatterConstant.DTMLOADSURVEYEXPRESSSIONFORSPHASE);
                string[] dtmLoadSurveyData = new string[matches.Count];
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    dtmLoadSurveyData[counter++] = groups[0].Value;
                }
                string[] availableData = FormatterCommon.RemoveDuplicateDataForSPhase(dtmLoadSurveyData);
                counter = 0;
                int MaxLength = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    if (tempData.Length > MaxLength)
                        MaxLength = tempData.Length;
                    counter++;
                }
                dtmLoadSurvey = new string[counter, MaxLength];
                counter = 0;
                while (counter <= availableData.GetUpperBound(0))
                {
                    string[] tempData = FormatterCommon.GetExpression(FormatterConstant.LCOMMANDEXPRESSIONFORSPHASE).Split(availableData[counter]);
                    for (MaxLength = 0; MaxLength < tempData.Length; MaxLength++)
                        dtmLoadSurvey[counter, MaxLength] = tempData[MaxLength];
                    counter++;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SplitDataSPhase(string[] tempData, ref IECBillingGeneralNFEntity billingGeneralNFEntity, int loadSurveyDaysCnt)
        {
            try
            {
                if (tempData == null)
                    return;

                int interval = 0;
                LoadSurveyData loadSurveyData = new LoadSurveyData();
                loadSurveyData.LoadSurvey = new List<IECLoadSurveyEntity>();
                loadSurveyData.LoadSurveyMeterData = new IECMeterDataEntity();


                for (int counter = 1; counter < tempData.Length; counter++)
                {
                    if (counter > loadSurveyDaysCnt - 1)
                        continue;
                    if (counter == 1)
                    {
                        loadSurveyData.LoadSurveyMeterData.MeterID = tempData[counter].Substring(13, 16).Trim();
                        continue;
                    }
                    else if (counter == 2)
                    {
                        loadSurveyData.LoadSurveyMeterData.ReadingDateTime = 0;// Convert.ToInt64(tempData[counter]);
                        continue;
                    }
                    else if (counter == 3)
                    {   // Remove multiple data packets 
                        string[] dailyLoadData = tempData[counter].Split('(', ')');
                        List<string> dailyLoadDataList = dailyLoadData.ToList<string>();
                        List<string> RemoveDuplicatePacket = dailyLoadDataList.Distinct().ToList();
                        dailyLoadData = RemoveDuplicatePacket.ToArray();
                        // Remove multiple data packets 
                        string paarameter = dailyLoadData[1];
                        if (paarameter != "")
                        {
                            string tempIP = paarameter.Substring(0, 2);
                            string meterIP = Convert.ToString(Convert.ToInt32(tempIP, 16), 2);
                            while (meterIP.Length < 8) { meterIP = "0" + meterIP; }

                            meterIP = meterIP.Substring(2, 2);
                            interval = Convert.ToInt32(meterIP, 2);

                            if (interval == 0)
                                interval = 15;
                            else if (interval == 2)
                                interval = 30;
                            else if (interval == 3)
                                interval = 60;

                            if (paarameter.Length > 2)
                            {
                                string byte1 = Convert.ToString(Convert.ToInt32(paarameter.Substring(2, 2), 16), 2);
                                string byte2 = Convert.ToString(Convert.ToInt32(paarameter.Substring(4, 2), 16), 2);
                                while (byte1.Length < 8) { byte1 = "0" + byte1; }
                                while (byte2.Length < 8) { byte2 = "0" + byte2; }
                                byte1 = ReverseMyString(byte1);
                                byte2 = ReverseMyString(byte2);
                                paarameter = byte1 + byte2;
                            }
                            else
                            {
                                // Default Configuration for StarlightMeters
                                paarameter = "1110000000000000";
                            }

                            //string byte1 = Convert.ToString(Convert.ToInt32(paarameter.Substring(2, 2), 16), 2);
                            //string byte2 = Convert.ToString(Convert.ToInt32(paarameter.Substring(4, 2), 16), 2);
                            //while (byte1.Length < 8) { byte1 = "0" + byte1; }
                            //while (byte2.Length < 8) { byte2 = "0" + byte2; }
                            //byte1 = ReverseMyString(byte1);
                            //byte2 = ReverseMyString(byte2);
                            //paarameter = byte1 + byte2;

                            string ParaConfiguration = dailyLoadData[1];
                            if (ParaConfiguration.Length == 32)
                            {
                                /*****************************************************************************************************
                                                        |              |             |             |		  	  |
                                           (---)(---)(--|-)(---)(---)(-|--)(---)(---)|(---)(---)(--|-)(---)(---)(-|--)(---)(---)
                                                        |              |             |             |		  	  |
                                Scalar       Byte 10 	|    Byte 9	   |	Byte 8	 | 	 Byte 7	   | 	Byte 6	  |    	Byte 5
                                Size         Byte 16 	|    Byte 15   |	Byte 14	 | 	 Byte 13   | 	Byte 12   |    	Byte 11
                                ******************************************************************************************************
                                                        Default decimal		Default Bytes	
                                ---------------         ----------			-------------	
                                KWH 		-			3 Decimal	 		2 Byte		
                                KVAH 		-			3                	2		
                                KVARH LAG	-			3                	2		
                                KVARH LEAD	-			3                	2		
                                MIN VOLT	-			3                	2		
                                MAX VOLT	-			3                	2		
                                AVG VOLT	-			2                	2		
                                PHASE CURRENT	-		3                	2		
                                NEUTRAL CURRENT	-		3                	2		
                                AVG PF		-			3                	2		
                                ACTIVE CURRENT	-		2                	2		
                                RESERVE 3	-			3                	2		
                                RESERVE 2	-			3                	2		
                                RESERVE 1	-			0                	0		
                                RESERVE 0	-			0                	0		
                                RESERVE DUMMY	-		0                	0		
                                ******************************************************************************************************/
                                int i = 0, j = 0;
                                //6 Bytes after LS days Byte represents scalar for each LS item consisting of 3 bits
                                string scalarBits = string.Empty, scalarByte, scalarString; 
                                scalarString = ParaConfiguration.Substring(8, 12);
                                for (i = 0; i < scalarString.Length; i += 2)
                                {
                                    scalarByte = scalarString.Substring(i, 2);
                                    scalarBits = Convert.ToString(Convert.ToInt32(scalarByte, 16), 2).PadLeft(8, '0') + scalarBits;
                                }

                                //6 Bytes after Scalar Bytes represents size in Bytes for each LS item consisting of 3 bits
                                string sizeBits = string.Empty, sizeByte, sizeString;
                                sizeString = ParaConfiguration.Substring(8 + 12, 12);
                                for (i = 0; i < sizeString.Length; i += 2)
                                {
                                    sizeByte = sizeString.Substring(i, 2);
                                    sizeBits = Convert.ToString(Convert.ToInt32(sizeByte, 16), 2).PadLeft(8, '0') + sizeBits;
                                }


                                i = j = 47;
                                int idx = 0;
                                string strDec = string.Empty, strSize = string.Empty;
                                while (i >= 0)
                                {

                                    strDec = scalarBits[i--].ToString();
                                    strDec = scalarBits[i--] + strDec;
                                    strDec = scalarBits[i--] + strDec;

                                    strSize = sizeBits[j--].ToString();
                                    strSize = sizeBits[j--] + strSize;
                                    strSize = sizeBits[j--] + strSize;

                                    SetLSParamsDetails(ref idx, strSize, strDec);

                                    idx++;
                                }
                            }

                        }

                        bool isEnergy = paarameter.Substring(0, 1) == "1" ? true : false;
                        bool isEnergykvah = paarameter.Substring(1, 1) == "1" ? true : false;
                        bool isLSTimeStamp = paarameter.Substring(2, 1) == "1" ? true : false;
                        bool iskVArhLag = paarameter.Substring(3, 1) == "1" ? true : false;
                        bool iskVArhLead = paarameter.Substring(4, 1) == "1" ? true : false;
                        bool isPhaseVolatgeMin = paarameter.Substring(5, 1) == "1" ? true : false;
                        bool isPhaseVolatgeMax = paarameter.Substring(6, 1) == "1" ? true : false;
                        bool isPhaseVolatgeAvg = paarameter.Substring(7, 1) == "1" ? true : false;
                        bool isAveragePhaseCurrent = paarameter.Substring(8, 1) == "1" ? true : false;
                        bool isAverageNeutralCurrent = paarameter.Substring(9, 1) == "1" ? true : false;
                        bool isAveragePF = paarameter.Substring(10, 1) == "1" ? true : false;
                        bool isAverageCurrent = paarameter.Substring(11, 1) == "1" ? true : false;
                        int count = 2; 
                        while ( count < dailyLoadData.Length - 1 )
                        {
                            count++;
                            Parameters = string.Empty;
                            string data = ReverseString(dailyLoadData[count]);
                            int startIndex = 0;
                            while (startIndex < data.Length)
                            {
                                IECLoadSurveyEntity loadSurvey = new IECLoadSurveyEntity();
                                if (isAverageCurrent)
                                {
                                    //loadSurvey.AvgCurrent = FormatterCommon.FilterData(data, startIndex, 4, 100, "0.00");
                                    loadSurvey.AvgCurrent = FormatterCommon.FilterData(data, startIndex, LslookUpTable[10].paramlength, LslookUpTable[10].decfactor, LslookUpTable[10].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[10].paramlength;
                                    if (!Parameters.Contains("AvgCurrent as 'Average Current'"))
                                        Parameters = Parameters + "AvgCurrent as 'Average Current'" + ",";
                                }
                                if (isAveragePF)
                                {
                                    //loadSurvey.PowerFactor = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.PowerFactor = FormatterCommon.FilterData(data, startIndex, LslookUpTable[9].paramlength, LslookUpTable[9].decfactor, LslookUpTable[9].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[9].paramlength;
                                    Parameters = Parameters + "PowerFactor" + ",";
                                }
                                if (isAverageNeutralCurrent)
                                {
                                    //loadSurvey.AvgCurrent = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");

                                    loadSurvey.AvgNeutralCurrent = FormatterCommon.FilterData(data, startIndex, LslookUpTable[8].paramlength, LslookUpTable[8].decfactor, LslookUpTable[8].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[8].paramlength;
                                    if (!Parameters.Contains("AvgNeutralCurrent as 'Average Neutral Current'"))
                                        Parameters = Parameters + "AvgNeutralCurrent as 'Average Neutral Current'" + ",";
                                }
                                if (isAveragePhaseCurrent)
                                {
                                    //loadSurvey.AvgCurrent = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.AvgCurrent = FormatterCommon.FilterData(data, startIndex, LslookUpTable[7].paramlength, LslookUpTable[7].decfactor, LslookUpTable[7].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[7].paramlength;
                                    if (!Parameters.Contains("AvgCurrent as 'Average Current'"))
                                        Parameters = Parameters + "AvgCurrent as 'Average Current'" + ",";
                                }
                                if (isPhaseVolatgeAvg)
                                {
                                    //loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, 4, 100, "0.00");
                                    loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, LslookUpTable[6].paramlength, LslookUpTable[6].decfactor, LslookUpTable[6].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[6].paramlength;
                                    if (!Parameters.Contains("AvgVoltage as 'Average Voltage'"))
                                        Parameters = Parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }
                                if (isPhaseVolatgeMax)
                                {
                                    //loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, LslookUpTable[5].paramlength, LslookUpTable[5].decfactor, LslookUpTable[5].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[5].paramlength;
                                    if (!paarameter.Contains("AvgVoltage as 'Average Voltage'"))
                                        Parameters = Parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }
                                if (isPhaseVolatgeMin)
                                {
                                    //loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.AvgVoltage = FormatterCommon.FilterData(data, startIndex, LslookUpTable[4].paramlength, LslookUpTable[4].decfactor, LslookUpTable[4].strformatter);
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[4].paramlength;
                                    if (!Parameters.Contains("AvgVoltage as 'Average Voltage'"))
                                        Parameters = Parameters + "AvgVoltage as 'Average Voltage'" + ",";
                                }
                                if (iskVArhLead)
                                {
                                    //loadSurvey.DemandKVARLead = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.DemandKVARLead = FormatterCommon.FilterData(data, startIndex, LslookUpTable[3].paramlength, LslookUpTable[3].decfactor, LslookUpTable[3].strformatter);
                                    if (loadSurvey.DemandKVARLead.ToString() == "65.278" || loadSurvey.DemandKVARLead.IndexOf("25.4", StringComparison.OrdinalIgnoreCase) >=0 ) // when meter is power off then default value is 65.278, it should be 0.000 //added case of 1Byte value of 25.4
                                        loadSurvey.DemandKVARLead = "0.000";
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[3].paramlength;
                                    Parameters = Parameters + "DemandKVARLead as 'Demand kvar (lead)'" + ",";
                                }
                                if (iskVArhLag)
                                {
                                    //loadSurvey.DemandKVARLag = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.00");
                                    loadSurvey.DemandKVARLag = FormatterCommon.FilterData(data, startIndex, LslookUpTable[2].paramlength, LslookUpTable[2].decfactor, LslookUpTable[2].strformatter);
                                    if (loadSurvey.DemandKVARLag.ToString() == "65.278" || loadSurvey.DemandKVARLag.IndexOf("25.4", StringComparison.OrdinalIgnoreCase) >= 0) // when meter is power off then default value is 65.278, it should be 0.000 //added case of 1Byte value of 25.4
                                        loadSurvey.DemandKVARLag = "0.000";
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[2].paramlength;
                                    Parameters = Parameters + "DemandKVARLag as 'Demand kvar (lag)'" + ",";
                                }
                                if (isLSTimeStamp)
                                {
                                    loadSurvey.LoadSurveyDateTime = Convert.ToInt64(FormatterCommon.DTMDailyProfileDateSP(ReverseString(data.Substring(startIndex, 8))));
                                    startIndex = startIndex + 8;
                                }
                                if (isEnergykvah)
                                {
                                    //loadSurvey.DemandKVA = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.000");
                                    loadSurvey.DemandKVA = FormatterCommon.FilterData(data, startIndex, LslookUpTable[1].paramlength, LslookUpTable[1].decfactor, LslookUpTable[1].strformatter);
                                    if (loadSurvey.DemandKVA.ToString() == "65.278" || loadSurvey.DemandKVA.IndexOf("25.4", StringComparison.OrdinalIgnoreCase) >= 0) // when meter is power off then default value is 65.278, it should be 0.000 //added case of 1Byte value of 25.4
                                        loadSurvey.DemandKVA = "0.000";
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[1].paramlength;
                                    Parameters = Parameters + "DemandKVA as 'Demand kVA'" + ",";
                                }
                                if (isEnergy)
                                {
                                    //loadSurvey.DemandKW = FormatterCommon.FilterData(data, startIndex, 4, 1000, "0.000");
                                    loadSurvey.DemandKW = FormatterCommon.FilterData(data, startIndex, LslookUpTable[0].paramlength, LslookUpTable[0].decfactor, LslookUpTable[0].strformatter);
                                    if (loadSurvey.DemandKW.ToString() == "65.278" || loadSurvey.DemandKW.IndexOf("25.4", StringComparison.OrdinalIgnoreCase) >= 0) // when meter is power off then default value is 65.278, it should be 0.000 //added case of 1Byte value of 25.4
                                        loadSurvey.DemandKW = "0.000";
                                    //startIndex = startIndex + 4;
                                    startIndex = startIndex + LslookUpTable[0].paramlength;
                                    Parameters = Parameters + "DemandKW as 'Demand kW'" + ",";
                                }

                                //loadSurvey.MeterReadingDatetime = meterReadingDatetime;
                                //loadSurvey.LoadSurveyDateTime = FormatterCommon.LongDateTime(tempData[counter], index);
                                //index += 10;
                                if (!String.IsNullOrEmpty(Parameters))
                                    Parameters = Parameters.Substring(0, Parameters.Length - 1);

                                loadSurvey.MDIntervalPeriod = interval;
                                loadSurvey.Parameters = Parameters;
                                if (loadSurvey.LoadSurveyDateTime != 0)
                                    loadSurveyData.LoadSurvey.Add(loadSurvey);   
                            }
                        }
                    }
                }
                billingGeneralNFEntity.listLoadSurveyData.Add(loadSurveyData);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Corrupted Load Survey data available in file.", "E-250 BCS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetLSParamsDetails(ref int idx, string strSize, string strDec)
        {
            if (idx > LslookUpTable.Count - 1) return;
            //if (LslookUpTable[idx].strtablename.Contains("TimeStamp")) idx++;

            int size = Convert.ToInt32(strSize, 2);
            int scalar = Convert.ToInt32(strDec, 2);

            LslookUpTable[idx].paramlength = 2 * size;
            LslookUpTable[idx].decfactor = (int)Math.Pow(10, scalar);
        }

        public static string ReverseMyString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
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
