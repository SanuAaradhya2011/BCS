using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using CABEntity;
using CAB.Channel.Programming;

namespace CHANNEL.Formatter
{
   public class FormatterDisplayParameter
    {
       public string[] SplitDisplayParamaterCount(string dispParamOutputString)
       {
           string[] displayParmaterCountByType = new string[3];
           displayParmaterCountByType[0] = dispParamOutputString.Substring(2, 2);// ConvertHexToAscii(dispParamOutputString.Substring(2, 2));
           displayParmaterCountByType[1] = dispParamOutputString.Substring(4, 2);
           displayParmaterCountByType[2] = dispParamOutputString.Substring(6, 2);

           return displayParmaterCountByType;
       }

       #region ParsePushModeParameters
       /// <summary>
       /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
       /// Purpose : Parsing of output of Push Paramater Read Command to 
       /// get the paramaters to be selected on grod.
       /// </summary>
       /// <param name="pushParamOutputString"></param>
       /// <param name="paramCount"></param>
       /// <returns></returns>
       public Collection<string> ParsePushModeParameters(string pushParamOutputString, int paramCount)
       {
           Collection<string> colSelectedPushModeParam = new Collection<string>();
           int upperParseLimit = 2 + ((paramCount - 1) * 2);
           string strParamCode = string.Empty;
           PushModeParameters pushModeParamater;
           for (int i = 2; i <= upperParseLimit; i += 2)
           {
               strParamCode = pushParamOutputString.Substring(i, 2);
               //strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
               pushModeParamater = (PushModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(PushModeParameters));
               switch (pushModeParamater)
               {
                   case PushModeParameters.BillpointAvgPowerFactor:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillpointAvgPowerFactor));
                       break;
                   case PushModeParameters.BillpointKvahr:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillpointKvahr));
                       break;
                   case PushModeParameters.BillpointKwhr:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.BillpointKwhr));
                       break;
                   case PushModeParameters.CumulativeKvarhrLag:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKvarhrLag));
                       break;
                   case PushModeParameters.CumulativeKvarhrLead:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeKvarhrLead));
                       break;
                   case PushModeParameters.CumulativeMD:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativeMD));
                       break;
                   case PushModeParameters.CumulativePowerOffHrs_RPhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHrs_RPhase));
                       break;
                   case PushModeParameters.CumulativePowerOffHrs_YPhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHrs_YPhase));
                       break;
                   case PushModeParameters.CumulativePowerOffHrs_BPhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.CumulativePowerOffHrs_BPhase));
                       break;
                   case PushModeParameters.Frequency:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.Frequency));
                       break;
                   case PushModeParameters.LBP_APFPriorLastReset:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBP_APFPriorLastReset));
                       break;
                   case PushModeParameters.LBP_MDinKW_PriorLastReset:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBP_MDinKW_PriorLastReset));
                       break;
                   case PushModeParameters.LBPCumulativeKvahrPriorLastReset:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPCumulativeKvahrPriorLastReset));
                       break;
                   case PushModeParameters.LBPCumulativeKwhrPriorLastReset:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.LBPCumulativeKwhrPriorLastReset));
                       break;
                   case PushModeParameters.MagneticInterferenceIndication:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.MagneticInterferenceIndication));
                       break;
                   case PushModeParameters.PhaseSequenceCheck:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PhaseSequenceCheck));
                       break;
                   case PushModeParameters.PowerOffHrs:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PowerOffHrs));
                       break;
                   case PushModeParameters.PowerOFFHrsforLastBillingPeriod:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.PowerOFFHrsforLastBillingPeriod));
                       break;
                   case PushModeParameters.TamperStatus:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.TamperStatus));
                       break;
                   case PushModeParameters.THDinPercentFor_RPhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.THDinPercentFor_RPhase));
                       break;
                   case PushModeParameters.THDinPercentFor_YPhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.THDinPercentFor_YPhase));
                       break;
                   case PushModeParameters.THDinPercentFor_Bhase:
                       colSelectedPushModeParam.Add(EnumUtil.StringValue(PushModeParameters.THDinPercentFor_Bhase));
                       break;
               }
           }
           return colSelectedPushModeParam;
       }
       #endregion

       #region ParseScrollModeParameters
       /// <summary>
       /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
       /// Purpose : Parsing of output of scroll Paramater Read Command to 
       /// get the paramaters to be selected on grod.
       /// </summary>
       /// <param name="pushParamOutputString"></param>
       /// <param name="paramCount"></param>
       /// <returns></returns>
       public Collection<string> ParseScrollModeParameters(string scrollParamOutputString, int paramCount)
       {
           Collection<string> colSelectedScrollModeParam = new Collection<string>();
           int upperParseLimit = 2 + ((paramCount - 1) * 2);
           string strParamCode = string.Empty;
           ScrollModeParameters scrollModeParamater;
           for (int i = 2; i <= upperParseLimit; i += 2)
           {
               strParamCode = scrollParamOutputString.Substring(i, 2);
              // strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
               scrollModeParamater = (ScrollModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(ScrollModeParameters));
               #region
               switch (scrollModeParamater)
               {
                   case ScrollModeParameters.AveragePowerFactorSinceLastReset:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.AveragePowerFactorSinceLastReset));
                       break;
                   case ScrollModeParameters.DateTimeOfLastReset:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.DateTimeOfLastReset));
                       break;

                   case ScrollModeParameters.InstantaneousSignedPFfor_RPhasewithLGandLDsign:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPFfor_RPhasewithLGandLDsign));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPFfor_YPhasewithLGandLDsign:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPFfor_YPhasewithLGandLDsign));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPFfor_BPhasewithLGandLDsign:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPFfor_BPhasewithLGandLDsign));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPFfor_SystemPF:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPFfor_SystemPF));
                       break;

                   case ScrollModeParameters.InstantaneousSignedPowerInKWfor_RPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerInKWfor_RPhase));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPowerInKWfor_YPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerInKWfor_YPhase));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPowerInKWfor_BPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerInKWfor_BPhase));
                       break;
                   case ScrollModeParameters.InstantaneousSignedPowerInKWfor_Total:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousSignedPowerInKWfor_Total));
                       break;

                   case ScrollModeParameters.InstantaneousVfor_RPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousVfor_RPhase));
                       break;
                   case ScrollModeParameters.InstantaneousVfor_YPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousVfor_YPhase));
                       break;
                   case ScrollModeParameters.InstantaneousVfor_BPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousVfor_BPhase));
                       break;
                   case ScrollModeParameters.InstantaneousIfor_RPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousIfor_RPhase));
                       break;
                   case ScrollModeParameters.InstantaneousIfor_YPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousIfor_YPhase));
                       break;
                   case ScrollModeParameters.InstantaneousIfor_BPhase:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.InstantaneousIfor_BPhase));
                       break;

                   case ScrollModeParameters.KvahrCumulative:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.KvahrCumulative));
                       break;
                   case ScrollModeParameters.KwhrCumulative:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.KwhrCumulative));
                       break;
                   case ScrollModeParameters.LCDCheck:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.LCDCheck));
                       break;
                   case ScrollModeParameters.MaximumDemandinKWforLastReset:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MaximumDemandinKWforLastReset));
                       break;
                   case ScrollModeParameters.MaximumDemandinKWSinceLastReset:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MaximumDemandinKWSinceLastReset));
                       break;
                   case ScrollModeParameters.MDresetCountCumulative:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MDresetCountCumulative));
                       break;

                   case ScrollModeParameters.MeterCoversOpenTamperWithDate:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MeterCoversOpenTamperWithDate));
                       break;
                   case ScrollModeParameters.MeterCoversOpenTamperWithTime:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.MeterCoversOpenTamperWithTime));
                       break;

                   case ScrollModeParameters.RealDate:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RealDate));
                       break;
                   case ScrollModeParameters.RealTime:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RealTime));
                       break;

                   case ScrollModeParameters.RisingDemandInKW_KvaWithRisingTime:
                       colSelectedScrollModeParam.Add(EnumUtil.StringValue(ScrollModeParameters.RisingDemandInKW_KvaWithRisingTime));
                       break;
               }
               #endregion
           }
           return colSelectedScrollModeParam;
       }
       #endregion

       #region Parse High Resolution Mode Paramaters
       /// <summary>
       /// Code Region Added by Vivek on 12 August 2011 (TNEB Project).
       /// Purpose : Parsing of output of High Resolution paramater Read Command to 
       /// get the paramaters to be selected on grod.
       /// </summary>
       /// <param name="pushParamOutputString"></param>
       /// <param name="paramCount"></param>
       /// <returns></returns>
       public Collection<string> ParseHighResolutionModeParameters(string hrParamOutputString, int paramCount)
       {
           Collection<string> colSelectedHRModeParam = new Collection<string>();
           int upperParseLimit = 2 + ((paramCount - 1) * 2);
           string strParamCode = string.Empty;
           HighResolutionModeParameters hrModeParamater;
           for (int i = 2; i <= upperParseLimit; i += 2)
           {
               strParamCode = hrParamOutputString.Substring(i, 2);
               //strParamCode = (Convert.ToInt32(strParamCode)).ToString("X2");
               hrModeParamater = (HighResolutionModeParameters)EnumUtil.GetValueByParamCode(ProgrammingCommon.GetASCIIValue(strParamCode), typeof(HighResolutionModeParameters));
               switch (hrModeParamater)
               {
                   case HighResolutionModeParameters.kVAh:
                       colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVAh));
                       break;
                   case HighResolutionModeParameters.kVArhlag:
                       colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVArhlag));
                       break;
                   case HighResolutionModeParameters.kVArhlead:
                       colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kVArhlead));
                       break;
                   case HighResolutionModeParameters.kWh:
                       colSelectedHRModeParam.Add(EnumUtil.StringValue(HighResolutionModeParameters.kWh));
                       break;
               }
           }
           return colSelectedHRModeParam;
       }
       #endregion

       public string ConvertHexToAscii(string hexvalue)
       {
           StringBuilder sb = new StringBuilder();
           for (int i = 0; i <= hexvalue.Length - 2; i += 2)
           {
               sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexvalue.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
           }
           return sb.ToString();
       }
    }
}
