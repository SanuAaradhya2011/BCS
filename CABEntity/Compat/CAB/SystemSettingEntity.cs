using System;
using LNG.Framework.Entity;


namespace LNG.Entity.Base
{
    public class SystemSettingEntity
    {
        private int system_Setting_ID;
        private string name;
        private string system_Setting_Value;
 
        public int SystemSettingID
        {
            get
            {
                return system_Setting_ID;
             }
            set
            {
                system_Setting_ID = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Value
        {
            get
            {
                return system_Setting_Value;
            }
            set
            {
                system_Setting_Value = value;
            }
        }
    }
}

