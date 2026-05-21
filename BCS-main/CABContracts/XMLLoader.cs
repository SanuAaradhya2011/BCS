using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CABEntity;

namespace CAB.Contracts
{
    public class XMLLoader
    {
        // static variable for meter configuration..
        private static MeterConfiguration meterConfiguration = null;
      
        static XMLLoader()
        {
            // create only one object..Dont worry about threads for now..
            if (meterConfiguration == null)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MeterConfiguration));
                    string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
                    TextReader textReader = new StreamReader( appPath +  "config.xml");
                    meterConfiguration = (MeterConfiguration)serializer.Deserialize(textReader) as MeterConfiguration;
                    textReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static MeterConfigurationConfigSection GetConfigSection(ConfigurationParameter parameter)
        {
            
           
            if (meterConfiguration != null)
            {
                foreach (MeterConfigurationConfigSection configSection in meterConfiguration.ConfigSection)//meterConfiguration.ConfigSection
                {
                    if (configSection.Name.ToLower() == parameter.ToString().ToLower()) //configSectionName.ToLower())
                        return configSection;
                }
            }
            return null;
        }

        /// <summary>
        /// Added by Vivek Agrawal
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static MeterConfigurationConfigSection GetConfigurationSection(DisplayParameter parameter)
        {
            MeterConfigurationConfigSection configSection = null;
            if (meterConfiguration != null)
            {
                for (int i = 0; i < meterConfiguration.ConfigSection.Length; i++)
                {
                    if (meterConfiguration.ConfigSection[i].Name.ToLower() == parameter.ToString().ToLower()) //configSectionName.ToLower())
                        return meterConfiguration.ConfigSection[i];
                }
            }
            return configSection;
        }


    }
}
