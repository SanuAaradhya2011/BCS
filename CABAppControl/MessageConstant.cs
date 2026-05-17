using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;

namespace CAB.UI.Controls
{
    public class MessageConstant
    {
        public static string GetText(string translationKey)
        {
            ResourceManager resourceManager = new ResourceManager("CABAppControl.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            return resourceManager.GetString(translationKey);
        }

        public static string GetFilterText(string translationKey)
        {
            string msg = string.Empty;
            ResourceManager resourceManager = new ResourceManager("CABAppControl.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            if (translationKey.IndexOf('|') > 0)
            {
                string[] keys = translationKey.Split('|');
                for (int i = 0; i < keys.Length; i++)
                    msg = msg + resourceManager.GetString(keys[i]) + " ";
            }
            else
                msg = GetText(translationKey);
            return msg;
        }
    }
}
