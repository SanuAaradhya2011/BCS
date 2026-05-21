#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
using CAB.Framework;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Maps Tamper  data to Load Tamper Entity.
    /// </summary>
    public class Tamper
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Tamper).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets Tamper  Entity from tamper Data.
        /// </summary>
        /// <param name="dailyProfileData"></param>
        /// <returns></returns>
        /// 
        double pf_r, pf_y, pf_b, pf_tot, pf_tot1; // add pradipta
        double pf_r_sin, pf_y_sin, pf_b_sin, pf_tot_sin, pf_tot_sin1, Neu_curr, Neu_curr1; // add pradipta

        double pf_r_cos, pf_y_cos, pf_b_cos;

        string _pf_r_ph, _pf_y_ph, _pf_b_ph;
        string _i_r_ph, _i_y_ph, _i_b_ph, _pf_r, _pf_y, _pf_b;

        //SarkarA code change start 20180321 // remove parameter for transaction events
        string _i_r, _i_y, _i_b;//current
        double? _r_ph, _y_ph, _b_ph;

        string _v_r, _v_y, _v_b;//voltage
        double? v_r_ph, v_y_ph, v_b_ph;

        string _p_r, _p_y, _p_b;//power factor
        double? p_r_ph, p_y_ph, p_b_ph;
        //SarkarA code change end 20180321

        public BillingGeneralNFDLMSEntity GetMappedEntity(List<ProfileData> inputData)
        {
            List<DLMS650TamperEntity> resultEntity = new List<DLMS650TamperEntity>();
            DLMS650TamperEntity tamperEntity = null;
            BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
            ConfigurationParser meterConfigParser = new ConfigurationParser(true);
            TamperParameterEntity tamperColumns = new TamperParameterEntity();
            string tamperColumnNames = string.Empty;
            DataElement dataElement = new DataElement();
            bool isTamperColumnsFetched = false;
            int tamperColMaxCount = 0;
            //string value = string.Empty;
            //string defaultValue = "----";
            try
            {
                foreach (ProfileData tamperData in inputData)
                {
                    foreach (MeterDataPacket meterDataPacket in tamperData.ListMeterDataPacket)
                    {
                        //SarkarA code change start 20180321 // remove parameter for transaction events
                        _r_ph = null; v_r_ph = null; p_r_ph = null;
                        _y_ph = null; v_y_ph = null; p_y_ph = null;
                        _b_ph = null; v_b_ph = null; p_b_ph = null;
                        //SarkarA code change end 20180321 

                        tamperEntity = new DLMS650TamperEntity();

                        //search for date time as date time would be present if the file is read from fast download
                        dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 30);
                        //match the element value with 0 as 0 is the default value from GetDataElementByDataDefId                        
                        if (!string.IsNullOrEmpty(dataElement.Value) && dataElement.Value != "0")
                        {
                            tamperEntity.DateTimeEvent = CommonMapper.StringToLongDateTimeFormat(dataElement.Value);
                        }
                        else
                        {
                            tamperEntity.DateTimeEvent = CommonMapper.StringToLongDateTimeFormat(meterDataPacket.ReadingDate.ToString("dd/MM/yyyy HH:mm:ss"));
                        }

                        dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 76);
                        tamperEntity.EventCode = Convert.ToInt32(dataElement.Value);

                        if (dataElement.DataDefinitionID <= 0)
                            dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2178);

                        if (dataElement.DataDefinitionID <= 0)
                            dataElement = CommonMapper.GetDataElementByDataDefId(meterDataPacket.ListDataElementValue, 2179);

                        tamperEntity.EventCode = Convert.ToInt32(dataElement.Value);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 187);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PhaseCurrent = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2142);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.Temprature = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2133);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDVR = CommonMapper.FormatData(dataElement);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2134);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDVY = CommonMapper.FormatData(dataElement);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2135);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDVB = CommonMapper.FormatData(dataElement);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2136);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDIR = CommonMapper.FormatData(dataElement);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2137);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDIY = CommonMapper.FormatData(dataElement);
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2138);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.THDIB  = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 10);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CurrentIR = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 11);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CurrentIY = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 12);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CurrentIB = CommonMapper.FormatData(dataElement);



                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 172);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PhaseVoltage = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 13);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.VoltageVRN = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 14);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.VoltageVYN = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 15);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.VoltageVBN = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 16);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PowerFactorRphase = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 17);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PowerFactorYphase = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 18);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PowerFactorBphase = CommonMapper.FormatData(dataElement);
                        //single phase - pf
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 19);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.TotalPowerFactor = CommonMapper.FormatData(dataElement);
                        // for smart meter cumul. tamper count

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 27);
                        if (dataElement.DataDefinitionID != 0)
                           tamperEntity.CumulativeTampercount = CommonMapper.FormatData(dataElement);
                        
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 31);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykWh = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 34);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykVAh = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 32);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykvarhLag = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 33);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykvarhLead = CommonMapper.FormatData(dataElement);

                        //SarkarA code change start 20180330 // add phase current instant, frequency
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 173);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.PhaseCurrentInstant = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 20);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.Frequency = CommonMapper.FormatData(dataElement);


                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2145);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.ActiveCurrentR = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2146);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.ActiveCurrentY = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2147);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.ActiveCurrentB = CommonMapper.FormatData(dataElement);
                        //SarkarA code change end 20180330 

                        #region "Net Metering New Parameters"

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2051);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykWhImport = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2052);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykVAhImport = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1079);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykWhExport = CommonMapper.FormatData(dataElement);

                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 1080);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.CumulativeEnergykVAhExport = CommonMapper.FormatData(dataElement);

                        //SarkarA code change start 20170118 // neutral current
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 174);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.NeutralCurrent = CommonMapper.FormatData(dataElement);
                        //SarkarA code change end 20170118
                        
                        dataElement = CommonMapper.GetDataElementOrNullByDataDefId(meterDataPacket.ListDataElementValue, 2209);
                        if (dataElement.DataDefinitionID != 0)
                            tamperEntity.ByPassCurrent = CommonMapper.FormatData(dataElement);
                       

                        if (tamperEntity.CurrentIR != null || tamperEntity.VoltageVRN != null || tamperEntity.PowerFactorRphase != null)//add  pradipta
                        {
                            _i_r = tamperEntity.CurrentIR.Replace("*", "");
                            _i_r = tamperEntity.CurrentIR.Replace("*A", "");
                            _r_ph = Convert.ToDouble(_i_r);

                            //for voltage 

                            _v_r = tamperEntity.VoltageVRN.Replace("*", "");
                            _v_r = tamperEntity.VoltageVRN.Replace("*V", "");
                            v_r_ph = Convert.ToDouble(_v_r);

                            //POWER FACTOR
                            _p_r = tamperEntity.PowerFactorRphase.Replace("*", "");
                            p_r_ph = Convert.ToDouble(_p_r);
                        }
                        if (tamperEntity.CurrentIY != null || tamperEntity.VoltageVYN != null || tamperEntity.PowerFactorYphase != null)//add  pradipta
                        {
                            _i_y = tamperEntity.CurrentIY.Replace("*", "");
                            _i_y = tamperEntity.CurrentIY.Replace("*A", "");
                            _y_ph = Convert.ToDouble(_i_y);

                            //for voltage 

                            _v_y = tamperEntity.VoltageVYN.Replace("*", "");
                            _v_y = tamperEntity.VoltageVYN.Replace("*V", "");
                            v_y_ph = Convert.ToDouble(_v_y);

                            //POWER FACTOR
                            _p_y = tamperEntity.PowerFactorYphase.Replace("*", "");
                            p_y_ph = Convert.ToDouble(_p_y);
                        }
                        if (tamperEntity.CurrentIB != null || tamperEntity.VoltageVBN != null || tamperEntity.PowerFactorBphase != null)//add  pradipta
                        {
                            _i_b = tamperEntity.CurrentIB.Replace("*", "");
                            _i_b = tamperEntity.CurrentIB.Replace("*A", "");
                            _b_ph = Convert.ToDouble(_i_b);

                            //for voltage 

                            _v_b = tamperEntity.VoltageVBN.Replace("*", "");
                            _v_b = tamperEntity.VoltageVBN.Replace("*V", "");
                            v_b_ph = Convert.ToDouble(_v_b);

                            //POWER FACTOR
                            _p_b = tamperEntity.PowerFactorBphase.Replace("*", "");
                            p_b_ph = Convert.ToDouble(_p_b);
                        }
                       
                        //SarkarA code change start 20180321 // remove parameter for transaction events

                        //tamperEntity.kWr = ((_r_ph * v_r_ph * p_r_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);
                        //tamperEntity.kWy = ((_y_ph * v_y_ph * p_y_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);
                        //tamperEntity.kWb = ((_b_ph * v_b_ph * p_b_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);

                        //tamperEntity.kVAr = ((_r_ph * v_r_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);
                        //tamperEntity.kVAy = ((_y_ph * v_y_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);
                        //tamperEntity.kVAb = ((_b_ph * v_b_ph) / 1000).ToString("0.00");// "33.99"; // Convert.ToString ( kwr / 1000);

                        
                            tamperEntity.kWr = string.Format("{0:0.000}",((_r_ph * v_r_ph * p_r_ph) / 1000));
                            tamperEntity.kVAr = string.Format("{0:0.000}",((_r_ph * v_r_ph) / 1000));
                        
                            tamperEntity.kWy = string.Format("{0:0.000}",((_y_ph * v_y_ph * p_y_ph) / 1000));
                            tamperEntity.kVAy = string.Format("{0:0.000}",((_y_ph * v_y_ph) / 1000));
                        
                            tamperEntity.kWb = string.Format("{0:0.000}", ((_b_ph * v_b_ph * p_b_ph) / 1000));
                            tamperEntity.kVAb = string.Format("{0:0.000}", ((_b_ph * v_b_ph) / 1000));
                            
                        //SarkarA code change end 20180321

                        #endregion

                        //only insert record with valid RTC as for fast download empty RTC are present.
                        if (!string.IsNullOrEmpty(tamperEntity.DateTimeEvent.ToString()) && !(tamperEntity.DateTimeEvent == 0))
                        {
                            resultEntity.Add(tamperEntity);
                        }

                        #region FetchTamperSnapShotParameterNames
                        //---To Get Tamper column again if parameter/colun counts increased, like incase of temperatute login, Temperature parameter is coming as extra parameter.
                       if (meterDataPacket.ListDataElementValue.Count > tamperColMaxCount) { tamperColMaxCount = meterDataPacket.ListDataElementValue.Count; isTamperColumnsFetched = false; }
                        // Get Tamper columns only for one time .
                        if (!isTamperColumnsFetched )
                        {
                            Dictionary<string, string> OBISTamperColumns = CommonMapper.GetOBISCodeColumnNamesTamper();
                            foreach (DataElement data in meterDataPacket.ListDataElementValue)
                            {
                                DLMSCOMMAND obisInfo = meterConfigParser.GetObisInfoFromRepository(data.DataDefinitionID);
                                foreach (KeyValuePair<string, string> pair in OBISTamperColumns)
                                {
                                    if (obisInfo.OBISCODE == pair.Key)
                                    {
                                        if (tamperColumnNames.Split(',').ToList<string>().IndexOf(pair.Value) == -1)
                                        {
                                            tamperColumnNames += pair.Value + ",";
                                            isTamperColumnsFetched = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(tamperColumnNames))
                            {
                                 tamperColumns.ColumnsNames = tamperColumnNames.Remove(tamperColumnNames.LastIndexOf(","));
                            }
                        }
                        #endregion

                    }
                }

                masterEntity.Tamper = resultEntity;
                masterEntity.TamperParameterColumns = tamperColumns;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMappedEntity(List<ProfileData> inputData)", ex);
            }
            return masterEntity;
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
