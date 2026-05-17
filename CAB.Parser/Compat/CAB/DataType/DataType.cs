#region Namespaces
using CAB.Parser;
using CAB.Parser.Entity;
using System;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Abstract class for data type
    /// </summary>
    public abstract class DataType
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        /// <summary>
        /// length in bytes
        /// </summary>
        public int LengthInBytes { get; set; }
        #endregion

        #region Constructor
        
        #endregion

        #region Properties
        #endregion

        #region Public Methods
        /// <summary>
        /// Absract function for implementation in child classes
        /// </summary>
        /// 
        /// <returns></returns>
        public abstract string GetValue(ExtendedBinaryReader binaryReader, DataElementConfiguration elementConfig);
        

        

        #endregion

        #region Protecetd Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
       
        #endregion        
        
    }
}


