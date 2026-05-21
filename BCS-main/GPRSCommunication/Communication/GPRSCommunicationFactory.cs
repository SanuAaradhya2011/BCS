using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication.Communication
{
    class GPRSCommunicationFactory
    {
        private static GPRSServiceCommunication serviceCall = new GPRSServiceCommunication();
        private static DBCommunication dbCall = new DBCommunication();

        /// <summary>
        /// Returns the instance of IGPRSCommunication object.
        /// If service call is configured the return the GPRSSericeCommunication else 
        /// Return DBCommunication instance.
        /// </summary>
        /// <param name="isServiceCall"></param>
        /// <returns></returns>
        public static IGPRSCommunication GetInstance(bool isServiceCall)
        {
            IGPRSCommunication retValue = null;
            if (isServiceCall)
            {
                retValue = serviceCall;
            }
            else
            {
                retValue = dbCall;
            }
            return retValue;
        }

    }
}
