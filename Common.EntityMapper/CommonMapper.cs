#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser.Entity;
using Hunt.EPIC.Logging;
#endregion

namespace Common.EntityMapper
{
    public class CommonMapper
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(CommonMapper).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to filter data element having input datadefinitionId
        /// </summary>
        /// <param name="dataDefId"></param>
        /// <returns></returns>
        public static DataElement GetDataElementByDataDefId(List<DataElement> inputData, int dataDefId)
        {
            DataElement resultEntity = new DataElement();
            resultEntity.Value = "0";           
            resultEntity.Unit = string.Empty;
            List<DataElement> dataElement = inputData.Where(item => item.DataDefinitionID == dataDefId).ToList() as List<DataElement>;
            if (dataElement != null && dataElement.Count > 0)
            {
                resultEntity = dataElement[0];
            }
            return resultEntity;
        }

        /// <summary>
        /// Used to filter data element for power on/off duration having input datadefinitionId
        /// </summary>
        /// <param name="dataDefId"></param>
        /// <returns></returns>
        public static DataElement GetDataElementOrNullByDataDefId(List<DataElement> inputData, int dataDefId)
        {
            DataElement resultEntity = new DataElement();
            resultEntity.Value = null;
            resultEntity.Unit = string.Empty;
            List<DataElement> dataElement = inputData.Where(item => item.DataDefinitionID == dataDefId).ToList() as List<DataElement>;
            if (dataElement != null && dataElement.Count > 0)
            {
                resultEntity = dataElement[0];
            }
            return resultEntity;
        }

        /// <summary>
        /// Used to format data within data element 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatData(DataElement dataElement)
        {
            //dataElement.Value = dataElement.Value.Replace("-","");
          
                if ((!string.IsNullOrEmpty(dataElement.Value)) && (!dataElement.Value.Contains("*")) && !string.IsNullOrEmpty(dataElement.Unit))
                {

                    dataElement.Value = dataElement.Value + "*" + dataElement.Unit;

                }
                else if (string.IsNullOrEmpty(dataElement.Unit))
                {
                    dataElement.Value = dataElement.Value + "*";
                }    
            return dataElement.Value;
        }

        /// <summary>
        /// Used to format data within data element 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatIECData(string data)
        {
            string output = "----";
            string value = string.Empty;
            string unit = string.Empty;
            if (data.Contains("*") && (!string.IsNullOrEmpty(data)))
            {
                string[] dataUnit = data.Split('*');
                value = dataUnit[0];
                if (dataUnit.Length > 1 && !string.IsNullOrEmpty(dataUnit[0]))
                {
                    unit = dataUnit[1];
                }

                if (unit.ToUpper() == "V")
                {
                    value = String.Format("{0:0.00}", Convert.ToDecimal(value)) + "*" + unit;
                }
                else if (unit.ToUpper() == "A")
                {
                    value = String.Format("{0:0.000}", Convert.ToDecimal(value)) + "*" + unit;
                }
                else if (unit.ToUpper() == "W" || unit.ToUpper() == "VA" || unit.ToUpper() == "VAR")
                {
                    value = (Convert.ToDecimal(value) / 1000).ToString();
                    if (unit.ToUpper() == "VAR")
                    {
                        unit = "VAr";
                    }
                    else if (unit.ToUpper() == "W")
                    {
                        unit = unit.ToUpper();
                    }

                    value = String.Format("{0:000000.00000}", Convert.ToDecimal(value)) + "*k" + unit;
                }
                else if (unit.ToUpper() == "HZ")
                {
                    value = String.Format("{0:0.00}", Convert.ToDecimal(value)) + "*" + "Hz";
                }
                else if (unit.ToUpper() == "WH" || unit.ToUpper() == "VAH" || unit.ToUpper() == "VARH")
                {
                    value = (Convert.ToDecimal(value) / 1000).ToString(); // convert Wh to kWh
                    value = String.Format("{0:0.0}", Convert.ToDecimal(value)) + "*k" + unit;
                }
                else if (unit.ToUpper() == "KWH" || unit.ToUpper() == "KVAH" || unit.ToUpper() == "KVARH")
                {

                    unit = unit.ToUpper() == "KWH" ? "kWh" : unit.ToUpper() == "KVAH" ? "kVAh" : "kvarh";

                    value = Convert.ToDecimal(value) + "*" + unit;

                }
                else if (unit.ToUpper() == "KW" || unit.ToUpper() == "KVA")
                {
                    value = Convert.ToDecimal(value) + "*" + unit.Replace("K", "k");
                }
                else if (unit.ToUpper() == "KVAR")
                {
                    value = String.Format("{0:0.00000}", Convert.ToDecimal(value)) + "*" + unit;
                }
                else if (unit == "min.")
                {
                    value = value + "*" + unit;
                }

            }
            else if (string.IsNullOrEmpty(unit))
            {
                value = value + "*";
            }
            return value;
        }

        /// <summary>
        /// Used to format data within data element 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatIECDataForSinglePhase(string data)
        {
            string value = string.Empty;
            string unit = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    string[] result = data.Split('k');
                    value = result[0];
                    unit = string.Concat("k", result[1]);
                    data = string.Concat(value, "*", unit);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FormatIECDataForSinglePhase(string data)", ex);
            }
            return data;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="dataElement"></param>
        ///// <returns></returns>
        //public static string FormatCurrentMonthMD(DataElement dataElement)
        //{
        //    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString();
        //    if (dataElement.Unit.ToUpper() == "W" || dataElement.Unit.ToUpper() == "VA")
        //    {
        //        dataElement.Value = String.Format("{0:0000.00}", Convert.ToDecimal(dataElement.Value)) +"*k" +dataElement.Unit.ToUpper();
        //    }
        //    return dataElement.Value;
        //}

        public static string FormatIECMD(string data)
        {
            string[] valueUnit = data.Split('*');
            string value = valueUnit[0];
            string unit = valueUnit[1];
            if (unit.ToUpper() == "KW" || unit.ToUpper() == "KVA")
            {
                value = Convert.ToDecimal(value) + "*" + unit.Replace("K", "k");
            }
            return value;
        }
        /// <summary>
        /// Used to apply fix 3 place decimal for Load survey energy values for DLMS .
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatDLMSLoadSurveyEnergy(string data)
        {
            return String.Format("{0:0.000}", (Convert.ToDecimal(data) / 1000));
        }


        /// <summary>
        /// Converts date time to long format
        /// </summary>
        /// <param name="value"></param> input format "12/3/2013 6:18:38 PM"
        /// <returns></returns> 
        public static long StringToLongDateTimeFormat(string value)
        {

            StringBuilder result = new StringBuilder("");
            if (value != "0")
            {

                string[] fullDateTime = value.Split(' ');
                char separator = '/';
                if (fullDateTime.Length > 1)
                {
                    separator = fullDateTime[0].Contains('/') ? '/' : fullDateTime[0].Contains('-') ? '-' : fullDateTime[0].Contains('.') ? '.' : separator;
                    string[] dateComponent = fullDateTime[0].Split(separator);
                    string[] timeComponent = fullDateTime[1].Split(':');
                    string day = Convert.ToInt32(dateComponent[0]).ToString("D2");
                    string month = Convert.ToInt32(dateComponent[1]).ToString("D2");
                    string year = dateComponent[2];
                    string hour = Convert.ToInt32(timeComponent[0]).ToString("D2");
                    string minute = Convert.ToInt32(timeComponent[1]).ToString("D2");
                    string second = Convert.ToInt32(timeComponent[2]).ToString("D2");
                    result.Append(year).Append(month).Append(day).Append(hour).Append(minute).Append(second);
                    // NO billing so need to inser t0 in database 
                    if (result.ToString() == "00010101120000" || result.ToString() == "00010101000000")
                    {
                        result = new StringBuilder("0");
                    }

                }
            }
            else
            {
                result.Append("0");
            }
            return Convert.ToInt64(result.ToString());
        }
        /// <summary>
        /// obis code and database column list for load survey .
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetOBISCodeColumnNamesBilling()
        {
            Dictionary<string, string> OBISBillingColumns = new Dictionary<string, string>();
            try
            {
                OBISBillingColumns.Add("0.0.0.1.2.255", "BillingDate");
                OBISBillingColumns.Add("1.0.13.0.0.255", "SystemPowerFactorforbillingperiod");
                OBISBillingColumns.Add("1.0.140.128.128.255", "SystemPowerFactorImportforBillingPeriod");
                OBISBillingColumns.Add("1.0.84.0.0.255", "SystemPowerFactorExportforBillingPeriod");

                OBISBillingColumns.Add("1.0.21.8.0.255", "CumEnergykWhRPhase");
                OBISBillingColumns.Add("1.0.41.8.0.255", "CumEnergykWhYPhase");
                OBISBillingColumns.Add("1.0.61.8.0.255", "CumEnergykWhBPhase");

                OBISBillingColumns.Add("1.0.1.8.0.255", "CumulativeEnergykWhTZ0");
                OBISBillingColumns.Add("1.0.1.8.1.255", "CumulativeEnergykWhTZ1");
                OBISBillingColumns.Add("1.0.1.8.2.255", "CumulativeEnergykWhTZ2");
                OBISBillingColumns.Add("1.0.1.8.3.255", "CumulativeEnergykWhTZ3");
                OBISBillingColumns.Add("1.0.1.8.4.255", "CumulativeEnergykWhTZ4");
                OBISBillingColumns.Add("1.0.1.8.5.255", "CumulativeEnergykWhTZ5");
                OBISBillingColumns.Add("1.0.1.8.6.255", "CumulativeEnergykWhTZ6");
                OBISBillingColumns.Add("1.0.1.8.7.255", "CumulativeEnergykWhTZ7");
                OBISBillingColumns.Add("1.0.1.8.8.255", "CumulativeEnergykWhTZ8");

                OBISBillingColumns.Add("1.0.143.128.128.255", "CumulativeEnergykWhTZ0Import");
                OBISBillingColumns.Add("1.0.143.128.129.255", "CumulativeEnergykWhTZ1Import");
                OBISBillingColumns.Add("1.0.143.128.130.255", "CumulativeEnergykWhTZ2Import");
                OBISBillingColumns.Add("1.0.143.128.131.255", "CumulativeEnergykWhTZ3Import");
                OBISBillingColumns.Add("1.0.143.128.132.255", "CumulativeEnergykWhTZ4Import");
                OBISBillingColumns.Add("1.0.143.128.133.255", "CumulativeEnergykWhTZ5Import");
                OBISBillingColumns.Add("1.0.143.128.134.255", "CumulativeEnergykWhTZ6Import");
                OBISBillingColumns.Add("1.0.143.128.135.255", "CumulativeEnergykWhTZ7Import");
                OBISBillingColumns.Add("1.0.143.128.136.255", "CumulativeEnergykWhTZ8Import");

                OBISBillingColumns.Add("1.0.2.8.0.255", "CumulativeEnergykWhTZ0Export");
                OBISBillingColumns.Add("1.0.2.8.1.255", "CumulativeEnergykWhTZ1Export");
                OBISBillingColumns.Add("1.0.2.8.2.255", "CumulativeEnergykWhTZ2Export");
                OBISBillingColumns.Add("1.0.2.8.3.255", "CumulativeEnergykWhTZ3Export");
                OBISBillingColumns.Add("1.0.2.8.4.255", "CumulativeEnergykWhTZ4Export");
                OBISBillingColumns.Add("1.0.2.8.5.255", "CumulativeEnergykWhTZ5Export");
                OBISBillingColumns.Add("1.0.2.8.6.255", "CumulativeEnergykWhTZ6Export");
                OBISBillingColumns.Add("1.0.2.8.7.255", "CumulativeEnergykWhTZ7Export");
                OBISBillingColumns.Add("1.0.2.8.8.255", "CumulativeEnergykWhTZ8Export");

                OBISBillingColumns.Add("1.0.8.8.0.255", "CumulativeEnergykvarhLead");
                OBISBillingColumns.Add("1.0.8.8.1.255", "CumulativeEnergykvarhLeadTZ1");
                OBISBillingColumns.Add("1.0.8.8.2.255", "CumulativeEnergykvarhLeadTZ2");
                OBISBillingColumns.Add("1.0.8.8.3.255", "CumulativeEnergykvarhLeadTZ3");
                OBISBillingColumns.Add("1.0.8.8.4.255", "CumulativeEnergykvarhLeadTZ4");
                OBISBillingColumns.Add("1.0.8.8.5.255", "CumulativeEnergykvarhLeadTZ5");
                OBISBillingColumns.Add("1.0.8.8.6.255", "CumulativeEnergykvarhLeadTZ6");
                OBISBillingColumns.Add("1.0.8.8.7.255", "CumulativeEnergykvarhLeadTZ7");
                OBISBillingColumns.Add("1.0.8.8.8.255", "CumulativeEnergykvarhLeadTZ8");

                OBISBillingColumns.Add("1.0.5.8.0.255", "CumulativeEnergykvarhLag");
                OBISBillingColumns.Add("1.0.5.8.1.255", "CumulativeEnergykvarhLagTZ1");
                OBISBillingColumns.Add("1.0.5.8.2.255", "CumulativeEnergykvarhLagTZ2");
                OBISBillingColumns.Add("1.0.5.8.3.255", "CumulativeEnergykvarhLagTZ3");
                OBISBillingColumns.Add("1.0.5.8.4.255", "CumulativeEnergykvarhLagTZ4");
                OBISBillingColumns.Add("1.0.5.8.5.255", "CumulativeEnergykvarhLagTZ5");
                OBISBillingColumns.Add("1.0.5.8.6.255", "CumulativeEnergykvarhLagTZ6");
                OBISBillingColumns.Add("1.0.5.8.7.255", "CumulativeEnergykvarhLagTZ7");
                OBISBillingColumns.Add("1.0.5.8.8.255", "CumulativeEnergykvarhLagTZ8");


                OBISBillingColumns.Add("1.0.145.128.128.255", "CumulativeEnergykvarhLagQ1");
                OBISBillingColumns.Add("1.0.145.128.129.255", "CumulativeEnergykvarhLagTZ1Q1");
                OBISBillingColumns.Add("1.0.145.128.130.255", "CumulativeEnergykvarhLagTZ2Q1");
                OBISBillingColumns.Add("1.0.145.128.131.255", "CumulativeEnergykvarhLagTZ3Q1");
                OBISBillingColumns.Add("1.0.145.128.132.255", "CumulativeEnergykvarhLagTZ4Q1");
                OBISBillingColumns.Add("1.0.145.128.133.255", "CumulativeEnergykvarhLagTZ5Q1");
                OBISBillingColumns.Add("1.0.145.128.134.255", "CumulativeEnergykvarhLagTZ6Q1");
                OBISBillingColumns.Add("1.0.145.128.135.255", "CumulativeEnergykvarhLagTZ7Q1");
                OBISBillingColumns.Add("1.0.145.128.136.255", "CumulativeEnergykvarhLagTZ8Q1");

                OBISBillingColumns.Add("1.0.146.128.128.255", "CumulativeEnergykvarhLeadQ4");
                OBISBillingColumns.Add("1.0.146.128.129.255", "CumulativeEnergykvarhLeadTZ1Q4");
                OBISBillingColumns.Add("1.0.146.128.130.255", "CumulativeEnergykvarhLeadTZ2Q4");
                OBISBillingColumns.Add("1.0.146.128.131.255", "CumulativeEnergykvarhLeadTZ3Q4");
                OBISBillingColumns.Add("1.0.146.128.132.255", "CumulativeEnergykvarhLeadTZ4Q4");
                OBISBillingColumns.Add("1.0.146.128.133.255", "CumulativeEnergykvarhLeadTZ5Q4");
                OBISBillingColumns.Add("1.0.146.128.134.255", "CumulativeEnergykvarhLeadTZ6Q4");
                OBISBillingColumns.Add("1.0.146.128.135.255", "CumulativeEnergykvarhLeadTZ7Q4");
                OBISBillingColumns.Add("1.0.146.128.136.255", "CumulativeEnergykvarhLeadTZ8Q4");

                OBISBillingColumns.Add("1.0.7.8.0.255", "CumulativeEnergykvarhLagQ3");
                OBISBillingColumns.Add("1.0.7.8.1.255", "CumulativeEnergykvarhLagTZ1Q3");
                OBISBillingColumns.Add("1.0.7.8.2.255", "CumulativeEnergykvarhLagTZ2Q3");
                OBISBillingColumns.Add("1.0.7.8.3.255", "CumulativeEnergykvarhLagTZ3Q3");
                OBISBillingColumns.Add("1.0.7.8.4.255", "CumulativeEnergykvarhLagTZ4Q3");
                OBISBillingColumns.Add("1.0.7.8.5.255", "CumulativeEnergykvarhLagTZ5Q3");
                OBISBillingColumns.Add("1.0.7.8.6.255", "CumulativeEnergykvarhLagTZ6Q3");
                OBISBillingColumns.Add("1.0.7.8.7.255", "CumulativeEnergykvarhLagTZ7Q3");
                OBISBillingColumns.Add("1.0.7.8.8.255", "CumulativeEnergykvarhLagTZ8Q3");

                OBISBillingColumns.Add("1.0.6.8.0.255", "CumulativeEnergykvarhLeadQ2");
                OBISBillingColumns.Add("1.0.6.8.1.255", "CumulativeEnergykvarhLeadTZ1Q2");
                OBISBillingColumns.Add("1.0.6.8.2.255", "CumulativeEnergykvarhLeadTZ2Q2");
                OBISBillingColumns.Add("1.0.6.8.3.255", "CumulativeEnergykvarhLeadTZ3Q2");
                OBISBillingColumns.Add("1.0.6.8.4.255", "CumulativeEnergykvarhLeadTZ4Q2");
                OBISBillingColumns.Add("1.0.6.8.5.255", "CumulativeEnergykvarhLeadTZ5Q2");
                OBISBillingColumns.Add("1.0.6.8.6.255", "CumulativeEnergykvarhLeadTZ6Q2");
                OBISBillingColumns.Add("1.0.6.8.7.255", "CumulativeEnergykvarhLeadTZ7Q2");
                OBISBillingColumns.Add("1.0.6.8.8.255", "CumulativeEnergykvarhLeadTZ8Q2");

                OBISBillingColumns.Add("1.0.9.8.0.255", "CumulativeEnergykVAhTZ0");
                OBISBillingColumns.Add("1.0.9.8.1.255", "CumulativeEnergykVAhTZ1");
                OBISBillingColumns.Add("1.0.9.8.2.255", "CumulativeEnergykVAHTZ2");
                OBISBillingColumns.Add("1.0.9.8.3.255", "CumulativeEnergykVAHTZ3");
                OBISBillingColumns.Add("1.0.9.8.4.255", "CumulativeEnergykVAHTZ4");
                OBISBillingColumns.Add("1.0.9.8.5.255", "CumulativeEnergykVAHTZ5");
                OBISBillingColumns.Add("1.0.9.8.6.255", "CumulativeEnergykVAHTZ6");
                OBISBillingColumns.Add("1.0.9.8.7.255", "CumulativeEnergykVAHTZ7");
                OBISBillingColumns.Add("1.0.9.8.8.255", "CumulativeEnergykVAHTZ8");

                OBISBillingColumns.Add("1.0.144.128.128.255", "CumulativeEnergykVAhTZ0Import");
                OBISBillingColumns.Add("1.0.144.128.129.255", "CumulativeEnergykVAhTZ1Import");
                OBISBillingColumns.Add("1.0.144.128.130.255", "CumulativeEnergykVAhTZ2Import");
                OBISBillingColumns.Add("1.0.144.128.131.255", "CumulativeEnergykVAhTZ3Import");
                OBISBillingColumns.Add("1.0.144.128.132.255", "CumulativeEnergykVAhTZ4Import");
                OBISBillingColumns.Add("1.0.144.128.133.255", "CumulativeEnergykVAhTZ5Import");
                OBISBillingColumns.Add("1.0.144.128.134.255", "CumulativeEnergykVAhTZ6Import");
                OBISBillingColumns.Add("1.0.144.128.135.255", "CumulativeEnergykVAhTZ7Import");
                OBISBillingColumns.Add("1.0.144.128.136.255", "CumulativeEnergykVAhTZ8Import");

                //OBISBillingColumns.Add("1.0.2.8.0.255", "CumulativeEnergykWhTZ0Export");
                //OBISBillingColumns.Add("1.0.2.8.1.255", "CumulativeEnergykWhTZ1Export");
                //OBISBillingColumns.Add("1.0.2.8.2.255", "CumulativeEnergykWhTZ2Export");
                //OBISBillingColumns.Add("1.0.2.8.3.255", "CumulativeEnergykWhTZ3Export");
                //OBISBillingColumns.Add("1.0.2.8.4.255", "CumulativeEnergykWhTZ4Export");
                //OBISBillingColumns.Add("1.0.2.8.5.255", "CumulativeEnergykWhTZ5Export");
                //OBISBillingColumns.Add("1.0.2.8.6.255", "CumulativeEnergykWhTZ6Export");
                //OBISBillingColumns.Add("1.0.2.8.7.255", "CumulativeEnergykWhTZ7Export");
                //OBISBillingColumns.Add("1.0.2.8.8.255", "CumulativeEnergykWhTZ8Export");

                OBISBillingColumns.Add("1.0.10.8.0.255", "CumulativeEnergykVAhTZ0Export");
                OBISBillingColumns.Add("1.0.10.8.1.255", "CumulativeEnergykVAhTZ1Export");
                OBISBillingColumns.Add("1.0.10.8.2.255", "CumulativeEnergykVAhTZ2Export");
                OBISBillingColumns.Add("1.0.10.8.3.255", "CumulativeEnergykVAhTZ3Export");
                OBISBillingColumns.Add("1.0.10.8.4.255", "CumulativeEnergykVAhTZ4Export");
                OBISBillingColumns.Add("1.0.10.8.5.255", "CumulativeEnergykVAhTZ5Export");
                OBISBillingColumns.Add("1.0.10.8.6.255", "CumulativeEnergykVAhTZ6Export");
                OBISBillingColumns.Add("1.0.10.8.7.255", "CumulativeEnergykVAhTZ7Export");
                OBISBillingColumns.Add("1.0.10.8.8.255", "CumulativeEnergykVAhTZ8Export");

                OBISBillingColumns.Add("1.0.1.6.0.255", "MDkWTZ0");
                //OBISBillingColumns.Add("1.0.1.6.0.255", "MDkWDateTimeTZ0");
                OBISBillingColumns.Add("1.0.1.6.1.255", "MDkWTZ1");
                //OBISBillingColumns.Add("1.0.1.6.1.255", "MDkWDateTimeTZ1");
                OBISBillingColumns.Add("1.0.1.6.2.255", "MDkWTZ2");
                //OBISBillingColumns.Add("1.0.1.6.2.255", "MDkWDateTimeTZ2");
                OBISBillingColumns.Add("1.0.1.6.3.255", "MDkWTZ3");
                //OBISBillingColumns.Add("1.0.1.6.3.255", "MDkWDateTimeTZ3");
                OBISBillingColumns.Add("1.0.1.6.4.255", "MDkWTZ4");
                //OBISBillingColumns.Add("1.0.1.6.4.255", "MDkWDateTimeTZ4");
                OBISBillingColumns.Add("1.0.1.6.5.255", "MDkWTZ5");
                //OBISBillingColumns.Add("1.0.1.6.5.255", "MDkWDateTimeTZ5");
                OBISBillingColumns.Add("1.0.1.6.6.255", "MDkWTZ6");
                //OBISBillingColumns.Add("1.0.1.6.6.255", "MDkWDateTimeTZ6");
                OBISBillingColumns.Add("1.0.1.6.7.255", "MDkWTZ7");
                //OBISBillingColumns.Add("1.0.1.6.7.255", "MDkWDateTimeTZ7");
                OBISBillingColumns.Add("1.0.1.6.8.255", "MDkWTZ8");
                //OBISBillingColumns.Add("1.0.1.6.8.255", "MDkWDateTimeTZ8");

                OBISBillingColumns.Add("1.0.151.128.128.255", "MDkWTZ0Import");
                //OBISBillingColumns.Add("1.0.151.128.128.255", "MDkWDateTimeTZ0Import");
                OBISBillingColumns.Add("1.0.151.128.129.255", "MDkWTZ1Import");
                //OBISBillingColumns.Add("1.0.151.128.129.255", "MDkWDateTimeTZ1Import");
                OBISBillingColumns.Add("1.0.151.128.130.255", "MDkWTZ2Import");
                //OBISBillingColumns.Add("1.0.151.128.130.255", "MDkWDateTimeTZ2Import");
                OBISBillingColumns.Add("1.0.151.128.131.255", "MDkWTZ3Import");
                //OBISBillingColumns.Add("1.0.151.128.131.255", "MDkWDateTimeTZ3Import");
                OBISBillingColumns.Add("1.0.151.128.132.255", "MDkWTZ4Import");
                //OBISBillingColumns.Add("1.0.151.128.132.255", "MDkWDateTimeTZ4Import");
                OBISBillingColumns.Add("1.0.151.128.133.255", "MDkWTZ5Import");
                //OBISBillingColumns.Add("1.0.151.128.133.255", "MDkWDateTimeTZ5Import");
                OBISBillingColumns.Add("1.0.151.128.134.255", "MDkWTZ6Import");
                //OBISBillingColumns.Add("1.0.151.128.134.255", "MDkWDateTimeTZ6Import");
                OBISBillingColumns.Add("1.0.151.128.135.255", "MDkWTZ7Import");
                //OBISBillingColumns.Add("1.0.151.128.135.255", "MDkWDateTimeTZ7Import");
                OBISBillingColumns.Add("1.0.151.128.136.255", "MDkWTZ8Import");
                //OBISBillingColumns.Add("1.0.151.128.136.255", "MDkWDateTimeTZ8Import");

                OBISBillingColumns.Add("1.0.2.6.0.255", "MDkWTZ0Export");
                //OBISBillingColumns.Add("1.0.2.6.0.255", "MDkWDateTimeTZ0Export");
                OBISBillingColumns.Add("1.0.2.6.1.255", "MDkWTZ1Export");
                //OBISBillingColumns.Add("1.0.2.6.1.255", "MDkWDateTimeTZ1Export");
                OBISBillingColumns.Add("1.0.2.6.2.255", "MDkWTZ2Export");
                //OBISBillingColumns.Add("1.0.2.6.2.255", "MDkWDateTimeTZ2Export");
                OBISBillingColumns.Add("1.0.2.6.3.255", "MDkWTZ3Export");
                //OBISBillingColumns.Add("1.0.2.6.3.255", "MDkWDateTimeTZ3Export");
                OBISBillingColumns.Add("1.0.2.6.4.255", "MDkWTZ4Export");
                //OBISBillingColumns.Add("1.0.2.6.4.255", "MDkWDateTimeTZ4Export");
                OBISBillingColumns.Add("1.0.2.6.5.255", "MDkWTZ5Export");
                //OBISBillingColumns.Add("1.0.2.6.5.255", "MDkWDateTimeTZ5Export");
                OBISBillingColumns.Add("1.0.2.6.6.255", "MDkWTZ6Export");
                //OBISBillingColumns.Add("1.0.2.6.6.255", "MDkWDateTimeTZ6Export");
                OBISBillingColumns.Add("1.0.2.6.7.255", "MDkWTZ7Export");
                //OBISBillingColumns.Add("1.0.2.6.7.255", "MDkWDateTimeTZ7Export");
                OBISBillingColumns.Add("1.0.2.6.8.255", "MDkWTZ8Export");
                //OBISBillingColumns.Add("1.0.2.6.8.255", "MDkWDateTimeTZ8Export");

                OBISBillingColumns.Add("1.0.9.6.0.255", "MDkVATZ0");
                //OBISBillingColumns.Add("1.0.9.6.0.255", "MDkVADateTimeTZ0");
                OBISBillingColumns.Add("1.0.9.6.1.255", "MDkVATZ1");
                //OBISBillingColumns.Add("1.0.9.6.1.255", "MDkVADateTimeTZ1");
                OBISBillingColumns.Add("1.0.9.6.2.255", "MDkVATZ2");
                //OBISBillingColumns.Add("1.0.9.6.2.255", "MDkVADateTimeTZ2");
                OBISBillingColumns.Add("1.0.9.6.3.255", "MDkVATZ3");
                //OBISBillingColumns.Add("1.0.9.6.3.255", "MDkVADateTimeTZ3");
                OBISBillingColumns.Add("1.0.9.6.4.255", "MDkVATZ4");
                //OBISBillingColumns.Add("1.0.9.6.4.255", "MDkVADateTimeTZ4");
                OBISBillingColumns.Add("1.0.9.6.5.255", "MDkVATZ5");
                //OBISBillingColumns.Add("1.0.9.6.5.255", "MDkVADateTimeTZ5");
                OBISBillingColumns.Add("1.0.9.6.6.255", "MDkVATZ6");
                //OBISBillingColumns.Add("1.0.9.6.6.255", "MDkVADateTimeTZ6");
                OBISBillingColumns.Add("1.0.9.6.7.255", "MDkVATZ7");
                //OBISBillingColumns.Add("1.0.9.6.7.255", "MDkVADateTimeTZ7");
                OBISBillingColumns.Add("1.0.9.6.8.255", "MDkVATZ8");
                //OBISBillingColumns.Add("1.0.9.6.8.255", "MDkVADateTimeTZ8");

                OBISBillingColumns.Add("1.0.152.128.128.255", "MDkVATZ0Import");
                //OBISBillingColumns.Add("1.0.152.128.128.255", "MDkVADateTimeTZ0Import");
                OBISBillingColumns.Add("1.0.152.128.129.255", "MDkVATZ1Import");
                //OBISBillingColumns.Add("1.0.152.128.129.255", "MDkVADateTimeTZ1Import");
                OBISBillingColumns.Add("1.0.152.128.130.255", "MDkVATZ2Import");
                //OBISBillingColumns.Add("1.0.152.128.130.255", "MDkVADateTimeTZ2Import");
                OBISBillingColumns.Add("1.0.152.128.131.255", "MDkVATZ3Import");
                //OBISBillingColumns.Add("1.0.152.128.131.255", "MDkVADateTimeTZ3Import");
                OBISBillingColumns.Add("1.0.152.128.132.255", "MDkVATZ4Import");
                //OBISBillingColumns.Add("1.0.152.128.132.255", "MDkVADateTimeTZ4Import");
                OBISBillingColumns.Add("1.0.152.128.133.255", "MDkVATZ5Import");
                //OBISBillingColumns.Add("1.0.152.128.133.255", "MDkVADateTimeTZ5Import");
                OBISBillingColumns.Add("1.0.152.128.134.255", "MDkVATZ6Import");
                //OBISBillingColumns.Add("1.0.152.128.134.255", "MDkVADateTimeTZ6Import");
                OBISBillingColumns.Add("1.0.152.128.135.255", "MDkVATZ7Import");
                //OBISBillingColumns.Add("1.0.152.128.135.255", "MDkVADateTimeTZ7Import");
                OBISBillingColumns.Add("1.0.152.128.136.255", "MDkVATZ8Import");
                //OBISBillingColumns.Add("1.0.152.128.136.255", "MDkVADateTimeTZ8Import");

                OBISBillingColumns.Add("1.0.10.6.0.255", "MDkWTZ0Export");
                //OBISBillingColumns.Add("1.0.10.6.0.255", "MDkWDateTimeTZ0Export");
                OBISBillingColumns.Add("1.0.10.6.1.255", "MDkVATZ1Export");
                //OBISBillingColumns.Add("1.0.10.6.1.255", "MDkWDateTimeTZ1Export");
                OBISBillingColumns.Add("1.0.10.6.2.255", "MDkVATZ2Export");
                //OBISBillingColumns.Add("1.0.10.6.2.255", "MDkWDateTimeTZ2Export");
                OBISBillingColumns.Add("1.0.10.6.3.255", "MDkVATZ3Export");
                //OBISBillingColumns.Add("1.0.10.6.3.255", "MDkWDateTimeTZ3Export");
                OBISBillingColumns.Add("1.0.10.6.4.255", "MDkVATZ4Export");
                //OBISBillingColumns.Add("1.0.10.6.4.255", "MDkWDateTimeTZ4Export");
                OBISBillingColumns.Add("1.0.10.6.5.255", "MDkVATZ5Export");
                //OBISBillingColumns.Add("1.0.10.6.5.255", "MDkWDateTimeTZ5Export");
                OBISBillingColumns.Add("1.0.10.6.6.255", "MDkVATZ6Export");
                //OBISBillingColumns.Add("1.0.10.6.6.255", "MDkWDateTimeTZ6Export");
                OBISBillingColumns.Add("1.0.10.6.7.255", "MDkVATZ7Export");
                //OBISBillingColumns.Add("1.0.10.6.7.255", "MDkWDateTimeTZ7Export");
                OBISBillingColumns.Add("1.0.10.6.8.255", "MDkVATZ8Export");
                //OBISBillingColumns.Add("1.0.10.6.8.255", "MDkWDateTimeTZ8Export");
                OBISBillingColumns.Add("0.0.94.91.13.255", "PowerONHoursDelta");
                OBISBillingColumns.Add("0.0.96.1.165.255", "BillingTypeAutoManual");
                OBISBillingColumns.Add("1.0.96.50.2.255", "BillingResetType");
                //for smart meter                
                OBISBillingColumns.Add("1.0.1.2.0.255", "CumulativeMDkw");
                OBISBillingColumns.Add("1.0.9.2.0.255", "CumulativeMDkva");

                OBISBillingColumns.Add("0.0.96.1.149.255", "CumulativeMDkw");
                OBISBillingColumns.Add("0.0.96.1.150.255", "CumulativeMDkva"); 

                OBISBillingColumns.Add("0.0.96.1.218.255", "CumulativeEnergyFraudkWh");
                OBISBillingColumns.Add("0.0.96.2.189.255", "CumulativeEnergyFraudkVAh");

                OBISBillingColumns.Add("1.0.5.6.0.255", "MDkVArLagTZ0");    //user story 1000867
                OBISBillingColumns.Add("1.0.8.6.0.255", "MDkVArLeadTZ0");

                OBISBillingColumns.Add("1.0.1.8.128.255", "kWh Lag");//for KSEB sapphire s2
                OBISBillingColumns.Add("1.0.1.8.129.255", "kWh Lead");//for KSEB sapphire s2
                OBISBillingColumns.Add("1.0.9.8.128.255", "kVAh Lag");//for KSEB sapphire s2 
                OBISBillingColumns.Add("1.0.9.8.129.255", "kVAh Lead");//for KSEB sapphire s2

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetOBISCodeColumnNamesBilling()", ex);
            }         
            return OBISBillingColumns;
        }


      


        /// <summary>
        /// obis code and database column list for load survey .
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetOBISCodeColumnNames()
        {
            Dictionary<string, string> OBISLoadSurveyColumns = new Dictionary<string, string>();
            OBISLoadSurveyColumns.Add("0.0.1.0.0.255", "realTimeClockDateandTime");
            OBISLoadSurveyColumns.Add("1.0.31.27.0.255", "rPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.51.27.0.255", "yPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.71.27.0.255", "bPhaseCurrent");
            OBISLoadSurveyColumns.Add("1.0.11.27.0.255", "averageCurrent");
            OBISLoadSurveyColumns.Add("1.0.32.27.0.255", "rPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.52.27.0.255", "yPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.72.27.0.255", "bPhaseVoltage");
            OBISLoadSurveyColumns.Add("1.0.12.27.0.255", "averageVoltage");
            OBISLoadSurveyColumns.Add("1.0.1.29.0.255", "blockEnergykWh");
            OBISLoadSurveyColumns.Add("1.0.2.29.0.255", "blockEnergykWhExport");
            OBISLoadSurveyColumns.Add("1.0.5.29.0.255", "blockEnergykvarhlag");
            OBISLoadSurveyColumns.Add("1.0.7.29.0.255", "blockEnergykvarhlagQ3");
            OBISLoadSurveyColumns.Add("1.0.8.29.0.255", "blockEnergykvarhlead");
            OBISLoadSurveyColumns.Add("1.0.6.29.0.255", "blockEnergykvarhleadQ2");
            OBISLoadSurveyColumns.Add("1.0.9.29.0.255", "blockEnergykVAh");
            OBISLoadSurveyColumns.Add("1.0.10.29.0.255", "blockEnergykVAhExport");

            OBISLoadSurveyColumns.Add("1.0.147.128.128.255", "blockEnergykWhImport");
            OBISLoadSurveyColumns.Add("1.0.148.128.128.255", "blockEnergykVAhImport");
            OBISLoadSurveyColumns.Add("1.0.149.128.128.255", "blockEnergykvarhlagQ1");
            OBISLoadSurveyColumns.Add("1.0.150.128.128.255", "blockEnergykvarhleadQ4");
            OBISLoadSurveyColumns.Add("1.0.21.29.0.255", "blockEnergykWhRPhase");
            OBISLoadSurveyColumns.Add("1.0.41.29.0.255", "blockEnergykWhYPhase");
            OBISLoadSurveyColumns.Add("1.0.61.29.0.255", "blockEnergykWhBPhase");

            OBISLoadSurveyColumns.Add("1.0.3.29.0.255", "blockEnergykvarhQ12");
            OBISLoadSurveyColumns.Add("1.0.4.29.0.255", "blockEnergykvarhQ34");
            OBISLoadSurveyColumns.Add("1.0.155.128.128.255", "blockEnergykvarhQ14");
            OBISLoadSurveyColumns.Add("1.0.156.128.128.255", "blockEnergykvarhQ23");
            OBISLoadSurveyColumns.Add("1.0.128.29.1.255", "blockEnergyFundamentalkWhAbsolute");

            OBISLoadSurveyColumns.Add("1.0.14.27.0.255", "frequency");
            OBISLoadSurveyColumns.Add("0.0.96.1.152.255", "tamperStatus");

            OBISLoadSurveyColumns.Add("1.1.24.7.0.255", "activePowerRPhase");
            OBISLoadSurveyColumns.Add("1.1.56.7.0.255", "activePowerYPhase");
            OBISLoadSurveyColumns.Add("1.1.76.7.0.255", "activePowerBPhase");
            OBISLoadSurveyColumns.Add("1.0.29.7.0.255", "apparentPowerRPhase");
            OBISLoadSurveyColumns.Add("1.0.49.7.0.255", "apparentPowerYPhase");
            OBISLoadSurveyColumns.Add("1.0.69.7.0.255", "apparentPowerBPhase");
            OBISLoadSurveyColumns.Add("1.0.23.7.0.255", "reactivePowerRPhase");
            OBISLoadSurveyColumns.Add("1.0.43.7.0.255", "reactivePowerYPhase");
            OBISLoadSurveyColumns.Add("1.0.63.7.0.255", "reactivePowerBPhase");
            OBISLoadSurveyColumns.Add("0.0.94.91.128.255", "powerOffDurationLSIP");
            OBISLoadSurveyColumns.Add("0.0.96.9.129.255", "temperature");
            OBISLoadSurveyColumns.Add("1.0.91.7.0.255", "neutralCurrent"); //add pradipta_load_neu           
            OBISLoadSurveyColumns.Add("0.0.96.10.128.255", "tamperflag");
            OBISLoadSurveyColumns.Add("1.0.12.27.128.255", "Avgvoltageof3phase");
            OBISLoadSurveyColumns.Add("1.0.33.29.0.255", "AvgRphasePF");
            OBISLoadSurveyColumns.Add("1.0.53.29.0.255", "AvgYphasePF");
            OBISLoadSurveyColumns.Add("1.0.73.29.0.255", "AvgBphasePF");
            OBISLoadSurveyColumns.Add("1.0.13.29.0.255", "AvgTotalPF");
            OBISLoadSurveyColumns.Add("1.0.91.29.0.255", "AvgNeutralCurrent");
            OBISLoadSurveyColumns.Add("1.0.128.27.0.255", "AvgPhaseCurrent");//For TPDDL 1 Phase
            OBISLoadSurveyColumns.Add("1.0.32.128.124.255", "THDVR");
            OBISLoadSurveyColumns.Add("1.0.52.128.124.255", "THDVY");
            OBISLoadSurveyColumns.Add("1.0.72.128.124.255", "THDVB");
            OBISLoadSurveyColumns.Add("1.0.31.128.124.255", "THDIR");
            OBISLoadSurveyColumns.Add("1.0.51.128.124.255", "THDIY");
            OBISLoadSurveyColumns.Add("1.0.71.128.124.255", "THDIB"); 
           

            return OBISLoadSurveyColumns;
        }

        /// <summary>
        /// obis code and database column list for Midnight.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetOBISCodeColumnNamesMidnight()
        {
            Dictionary<string, string> OBISMidnightColumns = new Dictionary<string, string>();
            OBISMidnightColumns.Add("0.0.1.0.0.255", "realTimeClockDateandTime");
            OBISMidnightColumns.Add("1.0.1.8.0.255", "cumEnergykWh");
            OBISMidnightColumns.Add("1.0.5.8.0.255", "cumEnergykvarhlag");
            OBISMidnightColumns.Add("1.0.8.8.0.255", "cumEnergykvarhlead");
            OBISMidnightColumns.Add("1.0.9.8.0.255", "cumEnergykVAh");
            OBISMidnightColumns.Add("1.0.1.6.0.255", "mDKW Import");
            OBISMidnightColumns.Add("1.0.9.6.0.255", "mDKVA Import");

            OBISMidnightColumns.Add("1.0.96.0.164.255", "PowerOnDurationThreePhases");
            OBISMidnightColumns.Add("1.0.96.0.165.255", "PowerOnDuration");// OBIS Code changed for APSPDCL : Daily Survey Requirement
            OBISMidnightColumns.Add("0.0.94.91.13.255", "PowerOnDurationGeneric");// OBIS Code changed for JVVNL : Daily Survey Requirement
            OBISMidnightColumns.Add("0.0.94.91.14.255", "PowerOnDurationGeneric1P");// OBIS Code changed for JVVNL : Daily Survey Requirement
            OBISMidnightColumns.Add("0.0.96.1.217.255", "PowerFailureDuration");

            OBISMidnightColumns.Add("1.0.2.8.0.255", "cumEnergykWhExport");
            OBISMidnightColumns.Add("1.0.7.8.0.255", "cumEnergykvarhlagQ3");
            OBISMidnightColumns.Add("1.0.6.8.0.255", "cumEnergykvarhleadQ2");
            OBISMidnightColumns.Add("1.0.10.8.0.255", "cumEnergykVAhExport");

            OBISMidnightColumns.Add("1.0.143.128.128.255", "cumEnergykWhImport");
            OBISMidnightColumns.Add("1.0.144.128.128.255", "cumEnergykVAhImport");
            OBISMidnightColumns.Add("1.0.21.8.0.255", "cumEnergykWhRPhase");
            OBISMidnightColumns.Add("1.0.41.8.0.255", "cumEnergykWhYPhase");
            OBISMidnightColumns.Add("1.0.61.8.0.255", "cumEnergykWhBPhase");
            OBISMidnightColumns.Add("1.0.3.8.0.255", "cumEnergykvarhQ12");
            OBISMidnightColumns.Add("1.0.4.8.0.255", "cumEnergykvarhQ34");
            OBISMidnightColumns.Add("1.0.153.128.128.255", "cumEnergykvarhQ14");
            OBISMidnightColumns.Add("1.0.154.128.128.255", "cumEnergykvarhQ23");
            OBISMidnightColumns.Add("1.0.145.128.128.255", "cumEnergykvarhlagQ1");
            OBISMidnightColumns.Add("1.0.146.128.128.255", "cumEnergykvarhleadQ4");
            OBISMidnightColumns.Add("1.0.128.8.1.255", "fundamentalAbsolutekWH");

            #region JDVVNL

            OBISMidnightColumns.Add("1.0.32.51.128.255", "minVoltageLSIPAcrossDayRPhase");
            OBISMidnightColumns.Add("1.0.52.51.128.255", "minVoltageLSIPAcrossDayYPhase");
            OBISMidnightColumns.Add("1.0.72.51.128.255", "minVoltageLSIPAcrossDayBPhase");
            OBISMidnightColumns.Add("1.0.31.53.128.255", "highestCurrentLSIPAcrossDayRPhase");
            OBISMidnightColumns.Add("1.0.51.53.128.255", "highestCurrentLSIPAcrossDayYPhase");
            OBISMidnightColumns.Add("1.0.71.53.128.255", "highestCurrentLSIPAcrossDayBPhase");
            #endregion
            return OBISMidnightColumns;
        }

        /// <summary>
        /// obis code and database column list for Tamper.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetOBISCodeColumnNamesTamper()
        {
            Dictionary<string, string> OBISTamperColumns = new Dictionary<string, string>();
            OBISTamperColumns.Add("1.0.31.7.0.255", "CurrentIR");
            OBISTamperColumns.Add("1.0.51.7.0.255", "CurrentIY");
            OBISTamperColumns.Add("1.0.71.7.0.255", "CurrentIB");
            OBISTamperColumns.Add("1.0.94.91.14.255", "PhaseCurrent");
            OBISTamperColumns.Add("1.0.32.7.0.255", "VoltageVRN");
            OBISTamperColumns.Add("1.0.52.7.0.255", "VoltageVYN");
            OBISTamperColumns.Add("1.0.72.7.0.255", "VoltageVBN");
            OBISTamperColumns.Add("1.0.12.7.0.255", "PhaseVoltage");
            OBISTamperColumns.Add("1.0.33.7.0.255", "PowerFactorRphase");
            OBISTamperColumns.Add("1.0.53.7.0.255", "PowerFactorYphase");
            OBISTamperColumns.Add("1.0.73.7.0.255", "PowerFactorBphase");
            OBISTamperColumns.Add("1.0.1.8.0.255", "CumulativeEnergykWh");
            OBISTamperColumns.Add("1.0.9.8.0.255", "CumulativeEnergykVAh");
            OBISTamperColumns.Add("0.0.96.9.128.255", "Temprature");//Deep
            OBISTamperColumns.Add("1.0.13.7.0.255", "TotalPowerFactor");
            OBISTamperColumns.Add("1.0.5.8.0.255", "CumulativeEnergykvarhLag");
            OBISTamperColumns.Add("1.0.8.8.0.255", "CumulativeEnergykvarhLead");
            // Net Metering new Parameters

            OBISTamperColumns.Add("1.0.143.128.128.255", "CumulativeEnergykWhImport");
            OBISTamperColumns.Add("1.0.144.128.128.255", "CumulativeEnergykVAhImport"); 
            OBISTamperColumns.Add("1.0.2.8.0.255", "CumulativeEnergykWhExport");
            OBISTamperColumns.Add("1.0.10.8.0.255", "CumulativeEnergykVAhExport");

            //SarkarA code change start 20170118 // neutral current
            //OBISTamperColumns.Add("0.0.96.11.0.255", "HighNeutralCurrent");//add pradipta_neu //SarkarA code change 20180213 // hidden as HNC already exists as an event
            OBISTamperColumns.Add("1.0.91.7.0.255", "NeutralCurrent"); //SarkarA code change end 20170118
            OBISTamperColumns.Add("1.0.11.7.128.255", "ByPassCurrent");
            OBISTamperColumns.Add("0.0.94.91.0.255", "CumulativeTamperCount");//smart meter

            //SarkarA code change start 20180330 // add phase current instant, frequency
            OBISTamperColumns.Add("1.0.11.7.0.255", "PhaseCurrentInstant");
            OBISTamperColumns.Add("1.0.14.7.0.255", "Frequency");            
            //SarkarA code change end 20180330
           //Add utility specific event tamper for normal sapphire meter 20-2-19
            OBISTamperColumns.Add("1.0.32.7.124.255", "THDVR");
            OBISTamperColumns.Add("1.0.52.7.124.255", "THDVY");
            OBISTamperColumns.Add("1.0.72.7.124.255", "THDVB");
            OBISTamperColumns.Add("1.0.31.7.124.255", "THDIR");
            OBISTamperColumns.Add("1.0.51.7.124.255", "THDIY");
            OBISTamperColumns.Add("1.0.71.7.124.255", "THDIB");

            return OBISTamperColumns;
        }
        #endregion

        public static Dictionary<string, string> GetLoadswitchOBISCodeColumnNames()
        {
            Dictionary<string, string> OBISLoadSwitchColumns = new Dictionary<string, string>();
            
            OBISLoadSwitchColumns.Add("0.0.96.11.6.255", "Controleventconnectdisconnect");
            OBISLoadSwitchColumns.Add("0.0.1.0.0.255", "RTC");
            OBISLoadSwitchColumns.Add("0.0.96.50.4.255", "Switchoperationreason");
            OBISLoadSwitchColumns.Add("1.0.1.8.0.255", "Cumulativeenergykwh");
            OBISLoadSwitchColumns.Add("1.0.1.8.1.255", "CumulativeenergykwhTZ1");
            OBISLoadSwitchColumns.Add("1.0.1.8.2.255", "CumulativeenergykwhTZ2");
            OBISLoadSwitchColumns.Add("1.0.1.8.3.255", "CumulativeenergykwhTZ3");
            OBISLoadSwitchColumns.Add("1.0.1.8.4.255", "CumulativeenergykwhTZ4");
            OBISLoadSwitchColumns.Add("1.0.1.8.5.255", "CumulativeenergykwhTZ5");
            OBISLoadSwitchColumns.Add("1.0.1.8.6.255", "CumulativeenergykwhTZ6");
            OBISLoadSwitchColumns.Add("1.0.1.8.7.255", "CumulativeenergykwhTZ7");
            OBISLoadSwitchColumns.Add("1.0.1.8.8.255", "CumulativeenergykwhTZ8");
            OBISLoadSwitchColumns.Add("1.0.9.8.0.255", "Cumulativeenergykvah");
            OBISLoadSwitchColumns.Add("1.0.9.8.1.255", "CumulativeenergykvahTZ1");
            OBISLoadSwitchColumns.Add("1.0.9.8.2.255", "CumulativeenergykvahTZ2");
            OBISLoadSwitchColumns.Add("1.0.9.8.3.255", "CumulativeenergykvahTZ3");
            OBISLoadSwitchColumns.Add("1.0.9.8.4.255", "CumulativeenergykvahTZ4");
            OBISLoadSwitchColumns.Add("1.0.9.8.5.255", "CumulativeenergykvahTZ5");
            OBISLoadSwitchColumns.Add("1.0.9.8.6.255", "CumulativeenergykvahTZ6");
            OBISLoadSwitchColumns.Add("1.0.9.8.7.255", "CumulativeenergykvahTZ7");
            OBISLoadSwitchColumns.Add("1.0.9.8.8.255", "CumulativeenergykvahTZ8");
            return OBISLoadSwitchColumns;
        }

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        #endregion


    }
}
