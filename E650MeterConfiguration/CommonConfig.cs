#region NameSpaces 
using System;
using System.Collections.Generic;
using System.Reflection;
#endregion
namespace CAB.E650MeterConfiguration
{
    /// <summary>
    /// Common Class used for programming parameters support
    /// </summary>
    public class CommonConfig
    {
        #region Nested Types
        #endregion

        #region Constants and Variables               
        #endregion

        #region Properties
        
        #endregion

        #region Constructor
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Uses reflection to  Get write data from
        /// corresponding class .
        /// </summary>
        /// <param name="className"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<byte> GetDataBuffer(string className, object value)
        {
            object[] parameters = new object[]{value};
            
            Type type = Type.GetType(className);
            PropertyInfo property = type.GetProperty("InputData");
            Type propertyType = property.PropertyType;   
            BaseConfig baseType = (BaseConfig)Activator.CreateInstance(type, new object[]{true});
            
            property.SetValue(baseType,value,null);
            MethodInfo method = type.GetMethod("GetDataBuffer");
            return method.Invoke(baseType, null) as List<byte>;
            
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods
        #endregion
    }
}
