using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication.DataValidator
{
    class IECValidator: IResponseValidator
    {
        #region IResponseValidator Members

        public bool validateResponse(byte[] response)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
