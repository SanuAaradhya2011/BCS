#region Namespaces
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CAB.Parser.Entity;
using CABCommunication.Common;
using System.Collections;
using System.Text;
using System.Linq;
#endregion
namespace CAB.Parser
{
    /// <summary>
    /// Parses DLMS Generic Individual 
    /// </summary>
    public class GenericParser
    {
        public enum CPP_Scalar { FF, FE, FD }; //FF-> 0, FE-> -1 , FD -> -2
        public enum CPP_Unit { A }; //0A-> INR
        public const byte Null_Type = 0x00;
        public const byte Array_Type = 0x01;
        public const byte Structure = 0x02;
        public const byte Boolean_Type = 0x03;
        public const byte BitString_Type = 0x04;
        public const byte DoubleLong_Type = 0x05;
        public const byte DoubleLongUnsigned_Type = 0x06;
        public const byte OctetString_Type = 0x09;
        public const byte VisibleString_Type = 10;
        public const byte BCD_Type = 13;
        public const byte Integer_Type = 15;
        public const byte Long_Type = 16;
        public const byte Unsigned_Type = 17;
        public const byte LongUnsigned_Type = 18;
        public const byte CompactArray_Type = 19;
        public const byte Long64_Type = 20;
        public const byte Long64Unsigned_Type = 21;
        public const byte Enum_Type = 22;
        public const byte Float32_Type = 23;
        public const byte Float64_Type = 24;
        public const byte DateTime_Type = 25;
        public const byte Date_Type = 26;
        public const byte Time_Type = 27;
        public const byte ExtendedOctetString = 0x82;
       
        public static string[] DLMSDataFormator(Result result, int nByteIndex, bool IsASCII)
        {
            try
            {
                bool bUnsignFlag = false;
                byte[] buffer = new byte[1];
                string data = "";
                string[] dataValue = new string[2];
                bool isASCIIString = false;
                int startdataIDX = nByteIndex;
                byte indexedDataType = (byte)result.RecieveDataBuffer[nByteIndex];
            SWITCHAGAIN:
                switch (indexedDataType)
                {
                    case (int)Null_Type: //DLMSDataStracture.Null_Type:                                    //0- NULL                   
                        break;
                    case (int)Array_Type:                                   //1- Array
                        buffer = new byte[result.RecieveDataBuffer[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Structure:                                    //2-Structure
                        buffer = new byte[result.RecieveDataBuffer[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Boolean_Type:                                 //3- Boolean
                        buffer = new byte[1]; nByteIndex += 1;
                        break;
                    case (int)BitString_Type:                              //4- Bit String
                        int bitLength = result.RecieveDataBuffer[nByteIndex + 1];
                        //if (Blockdata[nByteIndex + 1] == 0x81) nByteIndex += (Blockdata[nByteIndex + 2] / 8) + 3;
                        //else nByteIndex += (Blockdata[nByteIndex + 1] / 8) + 2;
                        //data = GetBitString(Blockdata);
                        //buffer = null;
                        break;
                    case (int)DoubleLong_Type:                             //5- Double Long -- 4Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)DoubleLongUnsigned_Type:                     //6- Double Long Unsigned -- 4Byte
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)OctetString_Type:                           //9- Oct String     
                        isASCIIString = true;
                        buffer = new byte[result.RecieveDataBuffer[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)VisibleString_Type:                        //10- Sequence of ASCII String 
                        isASCIIString = true;
                        buffer = new byte[result.RecieveDataBuffer[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)BCD_Type:                                  //13 - BCD
                        buffer = new byte[result.RecieveDataBuffer[nByteIndex + 1]]; nByteIndex += 2;
                        break;
                    case (int)Integer_Type:                              //15- Integer 1Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Unsigned_Type:                             //17- Unsigned 1Byte
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Long_Type:                                //16- Long Signed 2 byte
                        bUnsignFlag = true;
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)LongUnsigned_Type:                        //18- Unsigned 2 Byte
                        buffer = new byte[0x2]; nByteIndex++;
                        break;
                    case (int)CompactArray_Type:                        //19- Unsigned 2 Byte
                        indexedDataType = result.RecieveDataBuffer[nByteIndex + 1];
                        nByteIndex++;
                        goto SWITCHAGAIN;

                    case (int)Long64_Type:                             //20- Integer64   8 Byte
                        bUnsignFlag = true;
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Long64Unsigned_Type:                     //21- Unsigned64  8 Byte
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)Enum_Type:                               //22- Enum
                        buffer = new byte[0x1]; nByteIndex++;
                        break;
                    case (int)Float32_Type:                           //23- OCT String Len 4 
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                    case (int)Float64_Type:                           //24- OCT String Len 8 
                        buffer = new byte[0x8]; nByteIndex++;
                        break;
                    case (int)DateTime_Type:                         //25- OCT String Len 12 
                        buffer = new byte[0x0C]; nByteIndex++;
                        break;
                    case (int)Date_Type:
                        buffer = new byte[0x5]; nByteIndex++;
                        break;
                    case (int)Time_Type:
                        buffer = new byte[0x4]; nByteIndex++;
                        break;
                }
                if (buffer != null && buffer.Length > 0)
                {
                    int valuestart = nByteIndex;
                    Array.Copy(result.RecieveDataBuffer.ToArray(), valuestart, buffer, 0, buffer.Length);
                    if (result.RecieveDataBuffer[nByteIndex - 1] == 0x0C && !IsASCII) data = FormatDate(buffer);
                    else if (IsASCII && isASCIIString) data = FormatASCIIData(buffer);
                    else data = FormatData(buffer, bUnsignFlag);

                }
                dataValue[0] = data;
                dataValue[1] = nByteIndex.ToString();
                return dataValue;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string FormatDate(byte[] buffer)
        {
            if (buffer[0] != 0xFF)
            {
                int nYear = 0;

                nYear = (nYear | (int)buffer[0]) << 8;
                nYear = (nYear | (int)buffer[1]);
                string year = nYear.ToString("d4");

                string month = buffer[2].ToString("d2");
                string day = buffer[3].ToString("d2");
                string time = "";
                if (buffer[7] == 0xFF)
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2");
                else
                    time = buffer[5].ToString("d2") + ":" + buffer[6].ToString("d2") + ":" + buffer[7].ToString("d2");

                string date = day + "/" + month + "/" + year + " " + time;
                return date;
            }
            else
                return "00/00/0000 00:00:00";
        }
        public static string FormatData(byte[] buffer, bool isSignedDataType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte item in buffer) sb.Append(item.ToString("X2"));
            //----------Directly Convert To Unsigned Int64 and return if data type is DLMS Unsigned-------------
            if (!isSignedDataType) return Convert.ToUInt64(sb.ToString(), 16).ToString();
            //----------To Get Signed Value, Convert the data to the Desired Type Signed Value -----------------
            switch (buffer.Length)
            {
                case 1://One Byte signed integer
                    return sbyte.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 2://Two Byte signed integer
                    return Int16.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 4://Four Byte signed integer
                    return Int32.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                case 8://Eight Byte signed integer
                    return Int64.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber).ToString();
                default://---Do not Parse if not a appropriate numeric type
                    return sb.ToString();
            }
        }
        private static string FormatASCIIData(byte[] buffer)
        {
            string dataVal = string.Empty;
            int startDataindx = 0;
            string asciival = string.Empty;

            int stractcount = 0;
            int lengthodstruct = buffer.Length;//length of stract
            while (stractcount < lengthodstruct)
            {
                dataVal = buffer[startDataindx++].ToString("X");
                if (dataVal != "0") asciival = asciival + ((char)(Convert.ToInt32((dataVal), 16)));
                stractcount++;
            }

            return asciival;
        }

        private static string GetBitString(byte[] ReceiveBuffer)
        {
            int dataindexByte = 18;
            int recBytelen = ReceiveBuffer[dataindexByte + 1];
            if (ReceiveBuffer[dataindexByte + 1] == 0x81) { recBytelen = ReceiveBuffer[dataindexByte + 2]; dataindexByte += 3; }
            else dataindexByte += 2;
            byte[] lsobjectData = new byte[recBytelen / 8];
            Array.Copy(ReceiveBuffer, dataindexByte, lsobjectData, 0, lsobjectData.Length);
            List<byte> convertedByteList = ReverseBitsofByteList(lsobjectData);
            BitArray myarra = new BitArray(convertedByteList.ToArray());

            var builder = new StringBuilder();
            foreach (var bit in myarra.Cast<bool>())
                builder.Append(bit ? "1" : "0");
            return builder.ToString();
        }

        public static List<byte> ReverseBitsofByteList(byte[] recByteList)
        {
            List<byte> convertedlist = new List<byte>();
            try
            {
                foreach (byte item in recByteList)
                {
                    char[] bitarr = Convert.ToString(item, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(bitarr);
                    convertedlist.Add((byte)Convert.ToInt32(new string(bitarr), 2));
                }
                return convertedlist;
            }
            catch (Exception)
            {
                return null;
            }
        }


        #region Constructor
        //public GenericParser(bool isLittleEndian) 
        //    : base(isLittleEndian)
        //{
            
                   
        //}
        #endregion

        private int ConvertHexToDecimal(string val)
        {
            return int.Parse(val, System.Globalization.NumberStyles.HexNumber);
        }
    }
}


