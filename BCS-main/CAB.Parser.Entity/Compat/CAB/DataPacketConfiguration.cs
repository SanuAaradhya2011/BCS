#region Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
#endregion

namespace CAB.Parser.Entity
{
    /// <summary>
    /// This class represents the properties that are required to describe a data packet
    /// </summary>
    [DataContract]
    public class DataPacketConfiguration
    {

        #region Nested Types

        /// <summary>
        /// This class helps in sorting data elements in ascending order
        /// </summary>
        [Serializable]
        private class SortingCriteria : IComparer<DataElementConfiguration>
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
            /// This method helps in sorting the data elements, based on the sequence number
            /// returnValue = 1 denotes that the sorting is done in ascending order.
            /// returnValue = -1 denotes that the sorting is done in descending order.
            /// </summary>
            /// <param name="dataElementX"></param>
            /// <param name="dataElementY"></param>
            /// <returns>int</returns>
            public int Compare(DataElementConfiguration dataElementX, DataElementConfiguration dataElementY)
            {
                int returnValue = 0;

                if (dataElementX.SequenceNumber > dataElementY.SequenceNumber)
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

        #endregion

        #region Constants and Variables
        private string MeterModelField;
        #endregion

        #region Properties

        /// <summary>
        /// Represents whether the flag for 
        /// RTC is enabled
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC1", Order = 0)]
        public bool IsDateTimeSensitive { get; set; }

        /// <summary>
        /// Represents the date Time data Type Id if IsaDateTimeSensitive flag is true
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC2", Order = 1)]
        public int DateTimeDataTypeID { get; set; }

        /// <summary>
        /// Represents whether the flag for
        /// NumberfRecords Enabled
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC3", Order = 2)]
        public bool IsNumberOfRecordsIncluded { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MeterModel
        {
            get
            {
                return this.MeterModelField;
            }
            set
            {
                this.MeterModelField = value;
            }
        }


        /// <summary>
        /// Represents the sorted list of data elements
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC4", Order = 3)]
        public List<DataElementConfiguration> DataElements
        {
            get;
            /// setting the "set" to private so that 
            /// this property should not be updated from outside this class.
            set;
        }


        /// <summary>
        /// Represents the packet length for all the dataElements
        /// </summary>
        [DataMember(Name = "IncludedInPacketCRC5", Order = 4)]
        public int PacketLength { get; set; }

        /// <summary>
        /// Represents whether the packet contains 
        /// multiple records
        /// </summary>
        [DataMember(Name="ContainMultilple", Order=5)]
        public bool ContainMultiple
        {
            get
            {
                return IsNumberOfRecordsIncluded;
            }
            set
            {
                IsNumberOfRecordsIncluded = value;
            }
        }    

        #endregion

        #region Constructor

        /// <summary>
        /// This is the constructor 
        /// </summary>
        public DataPacketConfiguration()
        {
            DataElements = new List<DataElementConfiguration>();
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

