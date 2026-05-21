#region Namespaces
using System;
using System.Collections.Generic;
#endregion


namespace CABCommunication.ApplicationLayer
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicationContextName
    {

        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public List<byte> Value { get; set; }
       
        #endregion

        #region Constructor
        public ApplicationContextName(ApplicationContext context)
        {
            Value = new List<byte>();
            Value.Add(0xA1);
            Value.Add(0x09);
            Value.Add(0x06);
            Value.Add(0x07);
            Value.Add(0x60);
            Value.Add(0x85);
            Value.Add(0x74);
            Value.Add(0x05);
            Value.Add(0x08);
            Value.Add(0x01);
            Value.Add(Convert.ToByte(context));
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }
}
