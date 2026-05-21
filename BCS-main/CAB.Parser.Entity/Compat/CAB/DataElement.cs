#region Namespaces
#endregion

namespace CAB.Parser.Entity
{
    /// <summary>
    /// class is used to contain the 
    /// data elemnt info including the data definition Id 
    /// </summary>
    public class DataElement
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        /// <summary>
        /// data definition Id for the parsed column
        /// </summary>
        public int DataDefinitionID { get; set; }

        /// <summary>
        /// data element value for the parsed column
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// unit for the parsed column
        /// </summary>
        public string Unit { get; set; }
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

