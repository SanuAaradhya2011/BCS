using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisGyr.GSIS.CIM2ndEd.Service
{
    public enum SoapClientCredentialType
    {
        /// <summary>
        /// Not really an option in the implementation right now
        /// but reserving value just in case someone wants it.
        /// also may be useful for unit testing
        /// </summary>
        None = 0, 

        /// <summary>
        /// The proprietary soap header
        /// </summary>
        cabcon = 1,

        /// <summary>
        /// The WS-Security Oasis Username token profile #Password text standard
        /// http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0.pdf
        /// </summary>
        WSOasis10 = 2,

        /// <summary>
        /// 
        /// </summary>
        Basic = 3,
    }
}
