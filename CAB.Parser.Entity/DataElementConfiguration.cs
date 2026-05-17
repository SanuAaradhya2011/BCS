#region Namespaces
using System;
using System.Runtime.Serialization;
#endregion


namespace CAB.Parser.Entity
{
    /// <summary>
    /// Class defining the structure of data element
    /// </summary>
    [DataContract]
    public class DataElementConfiguration              
    {

        #region Nested Types
        #endregion

        #region Properties

        /// <summary>
        /// Represents the sequence number of the data element in a data packet.
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC6", Order = 5)]
        public int SequenceNumber { get; set; }

        /// <summary>
        /// Represents the Scalar value for data element
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC7", Order = 6)]
        public int Scalar { get; set; }

        /// <summary>
        /// Represents the Unit for data element
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC8", Order = 7)]
        public string Unit { get; set; }

        /// <summary>
        /// Represents the data type associated to the data element
        /// </summary>
        public int DataTypeID { get; set; }


        /// <summary> 
        /// Represents the size of the data element 
        /// </summary> 
        public int NumberOfBytes
        {
            get
            {
                return LengthInBits / 8;
            }
        }

        /// <summary> 
        /// Represents the size of the dataz element in bit form
        /// </summary> 
        [DataMember(Name = "IncludedInPacketCRC9", Order = 8)]
        public int LengthInBits { get; set; }
        
        /// <summary>
        /// Represent the decimal place round off for the data element
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC10", Order = 9)]
        public int Precision { get; set; }

        /// <summary> 
        /// Represents the size of varibale legth data type elemnts
        /// </summary> 
        [DataMember(Name = "IncludedInPacketCRC11", Order = 10)]
        public int LengthOfDataType { get; set; }

        /// <summary>
        /// Represent the Data Definition ID associated with the data element
        /// </summary>
        public int DataDefID { get; set; }

        /// <summary>
        /// Represents the short name associated with the Data Definition ID
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC12", Order = 11)]
        public string DataDefShortName { get; set; }

        /// <summary>
        /// Represents the Unique ID associated with the Element
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC13", Order = 12)]
        public int DataElementID { get; set; }

        /// <summary>
        /// Represents the configuration identifier associated with the element
        /// </summary>
        ///[DataMember(Name = "IncludedInPacketCRC14", Order = 13)]
        public int DataElementConfigId { get; set; }

        #endregion

        #region Constants and Variables
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// This method helps in sorting the data elements, based on the sequence number
        /// returnValue = 1 denotes that the sorting is done in ascending order.
        /// returnValue = -1 denotes that the sorting is done in descending order.
        /// </summary>
        /// <param name="dataElementToCompare"></param>
        /// <returns></returns>
        public int CompareTo(object dataElementToCompare)
        {
            int returnValue = 0;
            DataElementConfiguration dataElement = (DataElementConfiguration)dataElementToCompare;
            if (this.SequenceNumber > dataElement.SequenceNumber)
            {
                returnValue = 1;
            }
            else
            {
                returnValue = -1;
            }

            return returnValue;
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