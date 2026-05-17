#region Namespaces
#endregion

using System.Collections.Generic;
using System.Runtime.Serialization;
namespace CAB.Parser.Entity
{
    /// <summary>
    /// class is used to contain the 
    /// data packet info which is actually list of datapacketconfiguration
    /// </summary>
    [DataContract]
    public class DataPacket
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private List<DataPacketConfiguration> DataPacketConfigurationField;
        #endregion

        #region Properties
        [System.Xml.Serialization.XmlElementAttribute("DataPacketConfiguration", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<DataPacketConfiguration> DataPacketConfiguration 
        {
            get
            {
                return this.DataPacketConfigurationField;
            }
            set
            {
                this.DataPacketConfigurationField = value;
            }
        }        
        #endregion

        #region Constructor        
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

