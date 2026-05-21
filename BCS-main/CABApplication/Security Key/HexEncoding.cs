///****************************************************************************
//'*
//'*  Projet       : Hex String to Byte Array Conversion
//
//'*  Environment  : Visual Studio 2008 and Above - C#.net
//'*
//'*------+----------+------------------------------------------------------------
//'*Vers |   Date    |    Programmer and Comments
//'*------+----------+------------------------------------------------------------
//'* 1.00 | 15/04/2019 | Mohsin Raza : creation.
//'*------+----------+------------------------------------------------------------
//'*      |          | XXXXX: Change Details
//'******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexEncodingAPI
{
    public class HexEncoding
    {
        /// <summary>
        /// Creates a byte array from the hexadecimal string
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            byte[] bytes = null;
            byte[] retBuf = null;

            try
            {
                hexString = hexString.ToUpper();
                hexString = hexString.Replace("0", "\x0");
                hexString = hexString.Replace("1", "\x1");
                hexString = hexString.Replace("2", "\x2");
                hexString = hexString.Replace("3", "\x3");
                hexString = hexString.Replace("4", "\x4");
                hexString = hexString.Replace("5", "\x5");
                hexString = hexString.Replace("6", "\x6");
                hexString = hexString.Replace("7", "\x7");
                hexString = hexString.Replace("8", "\x8");
                hexString = hexString.Replace("9", "\x9");
                hexString = hexString.Replace("A", "\xa");
                hexString = hexString.Replace("B", "\xb");
                hexString = hexString.Replace("C", "\xc");
                hexString = hexString.Replace("D", "\xd");
                hexString = hexString.Replace("E", "\xe");
                hexString = hexString.Replace("F", "\xf");

                if (hexString.Length == 0) return null;

                bytes = ASCIIEncoding.ASCII.GetBytes(hexString);

                retBuf = new byte[bytes.Length / 2];

                for (uint ibytecount = 0, icount = 0; ibytecount < bytes.Length; ibytecount++, icount++)
                {
                    // Convert to byte from Nibble (MSB*0x10+LSB)
                    retBuf[icount] = (byte)(bytes[ibytecount] * 0x10 + bytes[ibytecount + 1]);
                    ibytecount++;
                }
            }
            catch (Exception ex)
            {
                retBuf = null;
            }

            return retBuf;
        }

    }
}
