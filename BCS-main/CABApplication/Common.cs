using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.BLL;
using CAB.Framework;
using System.Xml;
using CABFramework;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.UI
{
   static class Common
    {
       private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(Common).ToString());
       /// <summary>
       /// Return true if Billing Type display is set for Utility
       /// </summary>
       /// <param name="utility"></param>
       /// <returns></returns>
       public static bool showBillingType(UtilityEntity utility)
       {
           switch (utility)
           {
               case UtilityEntity.PGVCL:
                   return true;
               default:
                   return false;
           }
       }
       /// <summary>
       /// VBM - Used to get BCS Version from Version.xml file.
       /// </summary>
       /// <returns></returns>
       public static string GetBCSVersion()
       {
           try
           {
               XmlDocument xmlDoc = new XmlDocument();
               xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Version.xml");
               XmlNode node = xmlDoc.SelectSingleNode("Versions/Desktop");
               return node.InnerText;
           }
           catch (Exception ex)    //Exception log for catch block
           {
               logger.Log(LOGLEVELS.Error, "GetBCSVersion()", ex);
               return string.Empty;
               
           }
       }

       /// <summary>
       /// This method gets channel type through which communication happens.
       /// </summary>
       /// <returns></returns>
       public static string GetChannelType()
       {
           CommTypes commType = CommTypes.Direct;
           string channelType = ConfigSettings.GetValue("ChannelType");
           if (channelType == CABCommunication.PhysicalLayer.ChannelType.GSM.ToString())
           {
               commType = CommTypes.GSM;
           }
           else if (channelType == CABCommunication.PhysicalLayer.ChannelType.PSTN.ToString())
           {
               commType = CommTypes.PSTN;
           }
           else if (channelType == CABCommunication.PhysicalLayer.ChannelType.GPRS.ToString())
           {
               commType = CommTypes.GPRS;
           }
           else if (channelType == CABCommunication.PhysicalLayer.ChannelType.TCP.ToString())
           {
               commType = CommTypes.TCP;
           }
           return ((int)commType).ToString();
       }
       
    }
}
