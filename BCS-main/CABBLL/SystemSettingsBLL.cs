using System;
using System.Data;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Collections.Generic;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using CAB.Entity;

namespace CAB.BLL
{
    public class SystemSettingsBLL:IBLL
    {
        SystemSettingsDAL systemSettingsDAL = null;
        public SystemSettingsBLL()
        {
           systemSettingsDAL = new SystemSettingsDAL();
        }
        public void InsertSystemSettings()
        {
            systemSettingsDAL.InsertSystemSettings();
        }
        public void UpdateSetting(string name, string value)
        {
            systemSettingsDAL.UpdateSetting(name, value);
        }
        public string GetSettingValue(string name)
        {
            return systemSettingsDAL.GetSettingValue(name);
        }
        public bool UseMultiplePorts()
        {
            bool useMultiplePorts = false;
            if (systemSettingsDAL.GetSettingValue(SystemSettings.USE_MULTIPLE_PORTS).Equals("1"))
                useMultiplePorts = true;
            return useMultiplePorts;
        }
    }
}
