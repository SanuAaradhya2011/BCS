using System;
using System.Reflection;
using System.ComponentModel;

namespace CAB.Framework
{
    /// <summary>
    /// Static class for extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Enum extension method to retrieve description attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();

            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return string.Empty;
        }


    }
}
