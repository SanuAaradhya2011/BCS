using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CABEntity;
using System.Collections.ObjectModel;
using LTCTDAL;


namespace LTCTBLL
{
    
    #region Class - PushModeParameterBLL
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :PushModeParameterBLL class is responsible to create Read/Write Commands for Pushmode display parameter type
    /// 
    /// </summary>
    public class PushModeParameterBLL : DisplayParametersBLL
    {
        public override string GetWriteCommand(Collection<String> collectionSelectedParameters,int startIndex,int lastIndex)
        {
            string strParameters = string.Empty;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                foreach (PushModeParameters parameter in Enum.GetValues(typeof(PushModeParameters)))
                {
                    if (EnumUtil.StringValue(parameter) == collectionSelectedParameters[i])
                        strParameters += EnumUtil.ParameterCode(parameter).ToString();
                }
            }
            return strParameters;
        }
    }
    #endregion

    #region Class - ScrollModeParameterBLL
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :ScrollModeParameterBLL class is responsible to create Read/Write Commands for ScrollMode display parameter type
    /// </summary>
    public class ScrollModeParameterBLL : DisplayParametersBLL
    {
        public override string GetWriteCommand(Collection<String> collectionSelectedParameters,int startIndex,int lastIndex)
        {
            string strParameters = string.Empty;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                foreach (ScrollModeParameters parameter in Enum.GetValues(typeof(ScrollModeParameters)))
                {
                    if (EnumUtil.StringValue(parameter) == collectionSelectedParameters[i])
                        strParameters += EnumUtil.ParameterCode(parameter).ToString();
               }
            }
            return strParameters;
        }
    }
    #endregion

    #region Class - HighResolutionModeParameterBLL
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :HighResolutionModeParameterBLL class is responsible to create Read/Write Commands for highresolution mode display parameter type
    /// </summary>
    public class HighResolutionModeParameterBLL : DisplayParametersBLL
    {
        public override string GetWriteCommand(Collection<String> collectionSelectedParameters, int startIndex, int lastIndex)
        {
            string strParameters = string.Empty;
            for (int i = 0; i <= lastIndex; i++)
            {
                foreach (HighResolutionModeParameters parameter in Enum.GetValues(typeof(HighResolutionModeParameters)))
                {
                    if (EnumUtil.StringValue(parameter) == collectionSelectedParameters[i])
                        strParameters += EnumUtil.ParameterCode(parameter).ToString();
                }
            }
            return strParameters;
        }
    }
    #endregion

    #region Class - DisplayTimeOutsParameterBLL
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// Purpose :DisplayTimeOutsParameterBLL class is responsible to create Read/Write Commands for displaytimeouts display parameter type
    /// </summary>
    public class DisplayTimeOutsParameterBLL : DisplayParametersBLL
    {
        public override string GetWriteCommand(Collection<String> collectionSelectedParameters, int startIndex, int lastIndex)
        {
            string strParameters = string.Empty;
            for (int i = 0; i < collectionSelectedParameters.Count; i++)
            {
                if (collectionSelectedParameters[i] == DisplayTimeOutsParameters.ScrollTimePerItem.ToString())
                    strParameters += ((int)DisplayTimeOutsParameters.ScrollTimePerItem);
                else if (collectionSelectedParameters[i] == DisplayTimeOutsParameters.PushButtonTimeOut.ToString())
                    strParameters += ((int)DisplayTimeOutsParameters.ScrollTimePerItem);
                if (collectionSelectedParameters[i] == DisplayTimeOutsParameters.AutoScrollResumeTime.ToString())
                {
                    strParameters += ((int)DisplayTimeOutsParameters.ScrollTimePerItem);
                }

            }
            return strParameters;
        }
    }
    #endregion

    #region Class - DisplayParametersBLL
    /// <summary>
    /// Code Region Added by Vivek on 10 August 2011 (TNEB Project)
    /// 
    /// </summary>
    public class DisplayParametersBLL
    {
        public string GetWriteCommand(Collection<Collection<String>> collectionSelectedParameters)
        {
            string strParameters = string.Empty;
            string paramCount = string.Empty;

            for (int i = 0; i < collectionSelectedParameters.Count-1; i++)
            {
                paramCount = collectionSelectedParameters[i].Count.ToString("X2");
                if (paramCount.Length == 1)
                    paramCount = "0" + paramCount;
                foreach (char ch in paramCount)
                {
                    //ProgrammingCommon.GetASCIIValue(touMemoryAddress[gridIndex++].ToString("X4"));
                    strParameters += String.Format("{0:x2}", Convert.ToInt32(ch));
                }
            }
            return strParameters;
        }
        public virtual string GetWriteCommand(Collection<String> collectionSelectedParameters, int startIndex, int lastIndex)
        {
            return null;
        }
        public string GetReadCommand()
        {
            return "3033";
        }

        public bool InsertData(Collection<DisplayParamatersDBEntity> collDisplayParamatersDBEntity, string meterID, Int64 fileUploadID,Int64 MeterData_ID)
        {
            DisplayParamaterDAL displayParamaterDAL = new DisplayParamaterDAL();
            displayParamaterDAL.DeleteAllData(meterID, fileUploadID, MeterData_ID);
            return displayParamaterDAL.InsertData(collDisplayParamatersDBEntity, meterID, fileUploadID, MeterData_ID);
        }
        public Collection<Collection<string>> GetData(string meterID, Int64 fileUploadID,Int64 MeterData_ID)
        {
            return new DisplayParamaterDAL().GetData(meterID, fileUploadID, MeterData_ID);
        }
    }
    #endregion
}
