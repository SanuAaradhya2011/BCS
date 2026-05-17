#region Namespaces
using System;
using System.IO;
#endregion
namespace CAB.Parser
{   
    //contains the function which are easy to use for 
    //byte parsing with endianness.
    public class ExtendedBinaryReader : BinaryReader
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        #endregion

        #region Properties
        private bool IsLittleEndian
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public ExtendedBinaryReader(Stream lngStream,bool isLittleEndian)
            : base(lngStream)
        {
            IsLittleEndian = isLittleEndian;
        }
        #endregion       

        #region Public Methods
        /// <summary>
        /// Read bytes according to endian format, overrides Binary reader's ReadBytes function
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override byte[] ReadBytes(int count)
        {
            byte[] bytes = base.ReadBytes(count);
            if (IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
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


