using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication.DataValidator
{
    class DLMSValidator: IResponseValidator
    {
        #region IResponseValidator Members

        /// <summary>
        /// Validate response for DLMS packet
        /// </summary>
        /// <param name="ReceiveBuffer"></param>
        /// <returns></returns>
        public bool validateResponse(byte[] ReceiveBuffer)
        {
            bool retValue = false;

            if (ReceiveBuffer.Length >= 3 && ReceiveBuffer.Length >= ReceiveBuffer[2] + 1)
            {
                if (ReceiveBuffer[ReceiveBuffer[2] + 1] == 0x7E)
                {
                    retValue = true;
                }
            }
            return retValue;
        }

        #endregion
    }
}
