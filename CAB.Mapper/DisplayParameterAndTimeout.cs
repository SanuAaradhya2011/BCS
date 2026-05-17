#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Parser;
using CABEntity;
using System.Collections.ObjectModel;
using CAB.Parser.Entity;
using System.Xml;
using System.Data;
using System.IO;
#endregion
namespace CAB.Mapper
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
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DisplayParameterAndTimeout()
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
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Used to fill display parameter data 
        /// </summary>
        /// <param name="displayParameterData"></param>
        /// <param name="displayParameterType"></param>
        /// <returns></returns>
        public Collection<DisplayParamatersDBEntity> GetData(List<ProfileData> displayParameterData,DisplayParameter displayParameterType)
        {
            Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity = new Collection<DisplayParamatersDBEntity>();
            DisplayParamatersDBEntity displayParametet=null ;
            try
            {
                if (displayParameterData[0].ListMeterDataPacket.Count > 0 && displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.Count > 0)
                {
                    if (displayParameterType != DisplayParameter.DisplayTimeouts)
                    {
                        displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.Reverse();
                    }
                    foreach (DataElement dataElement in displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue)
                    {
                      
                        displayParametet = new DisplayParamatersDBEntity();
                        if (displayParameterType == DisplayParameter.DisplayTimeouts)
                        {
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 1)
                            {
                                displayParametet.paramaterName = "Push Time Out";
                                displayParametet.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 0)
                            {
                                displayParametet.paramaterName = "Scroll Time Out";
                                displayParametet.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                            if (displayParameterData[0].ListMeterDataPacket[0].ListDataElementValue.IndexOf(dataElement) == 2)
                            {
                                displayParametet.paramaterName = "Auto Scroll Resume Time";
                                displayParametet.displayParamaterType = DisplayParameter.DisplayTimeouts;
                                displayParametet.paramaterValue = Convert.ToInt32(dataElement.Value);
                            }
                        }
                        else
                        {
                            string id = dataElement.Value;
                            displayParametet.paramaterValue = 1;
                            displayParametet.displayParamaterType = displayParameterType;
                            displayParametet.paramaterName = GetParameterNameFromId(Convert.ToInt32(id), displayParameterType);
                        }

                        collDisplayParamatersDBEntity.Add(displayParametet);
                    }
                }
            }
            catch (Exception)
            {

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
        private string GetParameterNameFromId(int parameterId, DisplayParameter parameterType)
        {
            DataTable masterTable = new DataTable();
            string parameterName = string.Empty;
            if (parameterType == DisplayParameter.PushMode)
            {
                masterTable = pushDisplayParameterRepository;
            }
            else if (parameterType == DisplayParameter.ScrollMode)
            {
                masterTable = scrollDisplayParameterRepository;
            }
            else if (parameterType == DisplayParameter.HighResolutionMode)
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
