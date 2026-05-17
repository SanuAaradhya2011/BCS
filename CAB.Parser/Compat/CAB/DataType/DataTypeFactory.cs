#region Namespaces
using System;
using System.Collections.Generic;

#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Class is used for maintaining object pool of data types specified in protocol
    /// </summary>
    public class DataTypeFactory
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private Dictionary<int, DataType> dataTypeFactory = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public DataTypeFactory()
        {
            dataTypeFactory = new Dictionary<int, DataType>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// If available in the object pool return the value otherwise add in the object pool
        /// </summary>
        /// <param name="infoEntity"></param>
        /// <returns></returns>
        public DataType GetDataType(StructureInfoEntity infoEntity)
        {
            DataType dataType = null;
            dataTypeFactory.TryGetValue(infoEntity.StructureID, out dataType);
            if (dataType == null)
            {
                dataType = CreateInstance(infoEntity);
                dataTypeFactory.Add(infoEntity.StructureID, dataType);
            }
            return dataType;

        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Create instances of data type object, uses reflection for getting the obejct
        /// </summary>
        /// <param name="dataType"></param>
        private DataType CreateInstance(StructureInfoEntity infoEntity)
        {
            DataType dataType = null;
            Type type = Type.GetType(infoEntity.ClassName);
            dataType = (DataType)Activator.CreateInstance(type);
            dataType.LengthInBytes = infoEntity.ValueInByte;

            return dataType;
        }
        #endregion

    }
}


