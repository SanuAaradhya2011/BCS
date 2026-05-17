using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using Microsoft.Win32;
namespace CAB.Framework
{
    public class RegistryManager
    {
        RegistryKey registryKey = null;
        private const string PATH = "Path";
        public RegistryManager(RegistryKey registry)
        {
            registryKey = registry;
        }
        public string ReadKey(string fullPathKey)
        {
            try
            {
                RegistryKey nodeKey = registryKey.OpenSubKey(fullPathKey);
                object keyValue = nodeKey.GetValue(PATH, null);
                // Open a subKey as read-only
                if (keyValue != null)
                    return keyValue.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string ReadKey(string fullPathKey,string keyName)
        {
            try
            {
                RegistryKey nodeKey = registryKey.OpenSubKey(fullPathKey);
                object keyValue = nodeKey.GetValue(keyName, null);
                // Open a subKey as read-only
                if (keyValue != null)
                    return keyValue.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public void WriteKey(string fullPathKey, string value)
        {
            try
            {
                RegistryKey nodeKey = registryKey.OpenSubKey(fullPathKey, true);
                nodeKey.SetValue(PATH, value);
                // Open a subKey as read-only
            }
            catch (Exception ex)
            { 
                // do nothing
            }
        }
        public void WriteKey(string fullPathKey,string keyName,string value)
        {
            try
            {
                RegistryKey nodeKey = registryKey.OpenSubKey(fullPathKey, true);
                nodeKey.SetValue(keyName, value);
                // Open a subKey as read-only
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }
    }
}