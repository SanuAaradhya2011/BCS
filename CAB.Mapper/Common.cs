#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser.Entity;
#endregion

namespace CAB.Mapper
{
    public class Common
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
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
        /// Used to format data within data element 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatData(DataElement dataElement)
        {
            //dataElement.Value = dataElement.Value.Replace("-","");
                 
            if (!dataElement.Value.Contains("*") && (!string.IsNullOrEmpty(dataElement.Value)))
            {
                if (dataElement.Unit.ToUpper() == "V")
                {
                    dataElement.Value = String.Format("{0:00000000.00}", Convert.ToDecimal(dataElement.Value)) + "*" + dataElement.Unit;
                }
                else if (dataElement.Unit.ToUpper() == "A")
                {
                    dataElement.Value = String.Format("{0:00000000.000}", Convert.ToDecimal(dataElement.Value)) + "*" + dataElement.Unit;
                }
                else if (dataElement.Unit.ToUpper() == "W" || dataElement.Unit.ToUpper() == "VA" || dataElement.Unit.ToUpper() == "VAR")
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString();
                    if (dataElement.Unit.ToUpper() == "VAR")
                    {
                        dataElement.Unit = "VAr";
                    }
                    else if (dataElement.Unit.ToUpper() == "W")
                    {
                        dataElement.Unit = dataElement.Unit.ToUpper();
                    }

                    dataElement.Value = String.Format("{0:000000.0000}", Convert.ToDecimal(dataElement.Value)) + "*K" + dataElement.Unit;
                }
                else if (dataElement.Unit.ToUpper() == "HZ")
                {
                    dataElement.Value = String.Format("{0:00000000.00}", Convert.ToDecimal(dataElement.Value)) + "*" + dataElement.Unit;
                }
                else if (dataElement.Unit.ToUpper() == "WH" || dataElement.Unit.ToUpper() == "VAH" || dataElement.Unit.ToUpper() == "VARH")
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString(); // convert Wh to kWh
                    dataElement.Value = String.Format("{0:0000000.0}", Convert.ToDecimal(dataElement.Value)) + "*K" + dataElement.Unit.ToUpper();
                }
                else if (dataElement.Unit.ToUpper() == "KWH" || dataElement.Unit.ToUpper() == "KVAH" || dataElement.Unit.ToUpper() == "KVARH")
                {                    
                    dataElement.Value = String.Format("{0:0000000.0}", Convert.ToDecimal(dataElement.Value)) +"*" +dataElement.Unit.ToUpper();
                }
                else if (dataElement.Unit.ToUpper() == "KW" || dataElement.Unit.ToUpper() == "KVA")
                {
                    dataElement.Value = String.Format("{0:0000.00}", Convert.ToDecimal(dataElement.Value)) +"*" +dataElement.Unit.ToUpper();
                }
                else if (dataElement.Unit.ToUpper() == "KVAR" )
                {
                    dataElement.Value = String.Format("{0:000000.0000}", Convert.ToDecimal(dataElement.Value)) + "*" + dataElement.Unit;
                }
            }
            return dataElement.Value;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatCurrentMonthMD(DataElement dataElement)
        {
            dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString();
            if (dataElement.Unit.ToUpper() == "W" || dataElement.Unit.ToUpper() == "VA")
            {
                dataElement.Value = String.Format("{0:0000.00}", Convert.ToDecimal(dataElement.Value)) +"*K" +dataElement.Unit.ToUpper();
            }
            return dataElement.Value;
        }
        /// <summary>
        /// Used to format Cumulative energy data 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatCumulativeEnergy(DataElement dataElement)
        {
            if (!string.IsNullOrEmpty(dataElement.Value))
            {
                dataElement.Value = dataElement.Value.Replace("-", "");
                dataElement.Value = dataElement.Value.Split('*')[0];
                if (dataElement.Unit.ToUpper().Contains("WH")) // Value is coming in KWH
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) * 1000).ToString();
                }
                dataElement.Value = String.Format("{0:000000.0000}", Convert.ToDecimal(dataElement.Value));
                
            }
            return dataElement.Value;

        }

         /// <summary>
        /// Used to format data within data element 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatRisingDemand(DataElement dataElement)
        {
            dataElement.Value = dataElement.Value.Replace("-","");

            if (!dataElement.Value.Contains("*") && (!string.IsNullOrEmpty(dataElement.Value)))
            {
                dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString();
                if (dataElement.Unit.ToUpper() == "VAR")
                {
                    dataElement.Unit = "VAr";
                }
                else if (dataElement.Unit.ToUpper() == "W")
                {
                    dataElement.Unit = dataElement.Unit.ToUpper();
                }

                dataElement.Value = String.Format("{0:000000.000}", Convert.ToDecimal(dataElement.Value)) + "*K" + dataElement.Unit;
            }
            return dataElement.Value;

        }


        /// <summary>
        /// Used to format Load survey data 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatLoadSurveyData(DataElement dataElement)
        {
            if (!dataElement.Value.Contains("*") && (!string.IsNullOrEmpty(dataElement.Value)))
            {

                if (dataElement.Unit.ToUpper() == "WH" || dataElement.Unit.ToUpper() == "VARH" || dataElement.Unit.ToUpper() == "VAH")
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString("0.000");
                }                
                else if (dataElement.Unit.ToUpper() == "V")
                {
                    dataElement.Value = Convert.ToDecimal(dataElement.Value).ToString("0.00");
                }
                else if (dataElement.Unit.ToUpper() == "A")
                {
                    dataElement.Value = Convert.ToDecimal(dataElement.Value).ToString("0.000");
                }

            }

            return dataElement.Value;
        }

        /// <summary>
        /// Used to format phasor data 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatPhasorData(DataElement dataElement)
        {
            if (!dataElement.Value.Contains("*") && (!string.IsNullOrEmpty(dataElement.Value)))
            {
                if (dataElement.Unit.ToUpper() == "V" || dataElement.Unit.ToUpper() == "HZ")
                {
                    dataElement.Value = Convert.ToDecimal(dataElement.Value).ToString("0.00");
                }
                else if (dataElement.Unit.ToUpper() == "A" || dataElement.Unit.ToUpper() == "KWH" || dataElement.Unit.ToUpper() == "KW"
                          || dataElement.Unit.ToUpper() == "KVAR" || dataElement.Unit.ToUpper() == "KVARH" || dataElement.Unit.ToUpper() == "KVA")
                {
                    dataElement.Value = Convert.ToDecimal(dataElement.Value).ToString("0.000");
                }
                else
                {
                    dataElement.Value = Convert.ToDecimal(dataElement.Value).ToString("0.0");
                }

            }

            return dataElement.Value;
        }

        /// <summary>
        /// Used to format Midnight data 
        /// </summary>
        /// <param name="dataElement"></param>
        /// <returns></returns>
        public static string FormatMidNightData(DataElement dataElement)
        {
            if (!dataElement.Value.Contains("*") && (!string.IsNullOrEmpty(dataElement.Value)))
            {

                if (dataElement.Unit.ToUpper() == "WH" || dataElement.Unit.ToUpper() == "VARH" || dataElement.Unit.ToUpper() == "VAH")
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString("0.0000");
                }
                else if (dataElement.Unit.ToUpper() == "W" || dataElement.Unit.ToUpper() == "VA")
                {
                    dataElement.Value = (Convert.ToDecimal(dataElement.Value) / 1000).ToString("0.0000");
                }

            }

            return dataElement.Value;
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
                char separator = '/' ;
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
       
        #endregion

        #region Protected Methods
            #endregion

        #region Event Handlers

            #endregion

        #region Private Methods
            #endregion


    }
}
