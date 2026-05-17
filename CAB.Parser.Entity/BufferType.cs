using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAB.Parser.Entity
{
    /// <summary>
    /// enum is for storing the buffer type
    /// </summary>
    public enum BufferType
    {
        /// <summary>
        /// denotes the buffer type is of Non Profile DLMS Data
        /// </summary>
        DLMSNonProfileData = 1,
        /// <summary>
        /// denotes the buffer type is of DLMS data
        /// </summary>
        DLMSData = 7,
        /// <summary>
        /// denotes the buffer type is of fastdownload data 
        /// </summary>
        FastDownloadData = 255

    }
}
