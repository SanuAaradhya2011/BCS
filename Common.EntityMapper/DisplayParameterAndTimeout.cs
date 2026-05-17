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
using System.Xml;
using System.Data;
using CAB.BLL;
using System.Collections.ObjectModel;
using Hunt.EPIC.Logging;
#endregion
namespace Common.EntityMapper
{
    /// <summary>
    /// Fills Display parameters and siplay timepot parameters 
    /// </summary>
    public class DisplayParameterAndTimeout
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        private DataTable pushDisplayParameterRepository = null;
        private DataTable scrollDisplayParameterRepository = null;
        private DataTable highResolutionDisplayParameterRepository = null;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(DisplayParameterAndTimeout).ToString());
        string MType = string.Empty;
        #endregion

        #region Properties
        #endregion

        #region Constructor


        public DisplayParameterAndTimeout(string MeterType)
        {
            MType = MeterType;
            XmlDataDocument xmlDatadoc = null;
            DataSet displayParameterRepository;
            int MeterModelNumber = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
            try
            {
                //Read the Parameters from the XML file.
                xmlDatadoc = new XmlDataDocument();
                if (ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.TwoByte)
                {
                    xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "DisplayParameters_Extended.xml"));
                }
                else if (MeterType == "ST")
                {
                    xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "LTCT_DisplayParameters.xml"));
                }
                else
                {
                    if (MeterType == "SM0405" || MeterType == "SM0310")
                    {
                        xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "DisplayParameters_Falcon3PH.xml"));


                    }
                    else if (MeterType == "SM0110")
                    {
                        xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "DisplayParameters_Falcon1PH.xml"));


                    }
                    else
                    {
                        xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "DisplayParameters.xml"));
                    }
                }
                displayParameterRepository = xmlDatadoc.DataSet;
                if (displayParameterRepository.Tables.Count == 3)
                {
                    pushDisplayParameterRepository = displayParameterRepository.Tables["PushDisplayParams"];
                    scrollDisplayParameterRepository = displayParameterRepository.Tables["ScrollDisplayParams"];
                    highResolutionDisplayParameterRepository = displayParameterRepository.Tables["HighResolution"];
                }                                
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DisplayParameterAndTimeout(string MeterType)", ex);
                throw ex;
            }
        }


        /*public DisplayParameterAndTimeout()
        {
            XmlDataDocument xmlDatadoc = null;
            DataSet displayParameterRepository;
            try
            {
                //Read the Parameters from the XML file.
                xmlDatadoc = new XmlDataDocument();
                xmlDatadoc.DataSet.ReadXml(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\", "DisplayParameters.xml"));
                displayParameterRepository = xmlDatadoc.DataSet;
                if (displayParameterRepository.Tables.Count == 3)
                {
                    pushDisplayParameterRepository = displayParameterRepository.Tables["PushDisplayParams"];
                    scrollDisplayParameterRepository = displayParameterRepository.Tables["ScrollDisplayParams"];
                    highResolutionDisplayParameterRepository = displayParameterRepository.Tables["HighResolution"];                    
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }*/
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill display parameter data 
        /// </summary>
        /// <param name="displayParameterData"></param>
        /// <param name="displayParameterType"></param>
        /// <returns></returns>
        public Collection<DisplayParamatersDBEntity> GetData(List<ProfileData> displayParameterData, DisplayParameterType displayParameterType)
        {
            Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            DisplayParamatersDBEntity displayParametet=null ;            
            try
            {
                if (displayParameterData[0].ListMeterDataPacket.Count > 0 && displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    if (displayParameterType != DisplayParameterType.DisplayTimeouts && ConfigInfo.DisplayProgrammingVariant == DisplayProgrammingTypes.OneByte)
                    {
                        displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.Reverse();
                    }
                    foreach (DataElement dataElement in displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue)
                    {
                      
                        displayParametet = new DisplayParamatersDBEntity();
                        if (displayParameterType == DisplayParameterType.DisplayTimeouts)
                        {
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 1)
                            {
                                displayParametet.paramaterName = "Push Time Out";
                                displayParametet.displayParamaterType = DisplayParameterType.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 0)
                            {
                                displayParametet.paramaterName = "Scroll Time Out";
                                displayParametet.displayParamaterType = DisplayParameterType.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 2)
                            {
                                displayParametet.paramaterName = "Auto Scroll Resume Time";
                                displayParametet.displayParamaterType = DisplayParameterType.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                        }
                        else
                        {

                            string id = dataElement.Value;
                          
                                displayParametet.paramaterValue = 1;
                                displayParametet.displayParamaterType = displayParameterType;
                                displayParametet.paramaterName = GetParameterNameFromId(Convert.ToInt32(id), displayParameterType);
                            if (id == "0" && displayParametet.paramaterName == "")
                            {
                                  displayParametet.paramaterValue = 0;
                                 displayParametet.paramaterName=null;
                                 displayParametet.displayParamaterType =0;
                            }
                        }

                       // if (dataElement.Value != "0" && MType == "SM0110")
                        collDisplayParamatersDBEntity.Add(displayParametet);
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetData(List<ProfileData> displayParameterData, DisplayParameterType displayParameterType)", ex);
            }
            return collDisplayParamatersDBEntity;
        }


        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        /// <summary>
        ///  Used to get parameter Name from parameter Id 
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        private string GetParameterNameFromId(int parameterId, DisplayParameterType parameterType)
        {
            DataTable masterTable = new DataTable();
            string parameterName = string.Empty;
            if (parameterType == DisplayParameterType.PushMode)
            {
                masterTable = pushDisplayParameterRepository;
            }
            else if (parameterType == DisplayParameterType.ScrollMode)
            {
                masterTable = scrollDisplayParameterRepository;
            }
            else if (parameterType == DisplayParameterType.HighResolutionMode)
            {
                masterTable = highResolutionDisplayParameterRepository;
            }

            foreach (DataRow row in masterTable.Rows)
            {
                if(Convert.ToInt32(row["ID"]) == parameterId)
                {
                    parameterName = row["Description"].ToString();
                    break;
                }
            }
            return parameterName;
        }

        #endregion

    }
}
