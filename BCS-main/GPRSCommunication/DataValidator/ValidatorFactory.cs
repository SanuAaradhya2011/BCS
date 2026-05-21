using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GPRSCommunication.DataValidator
{
    class ValidatorFactory
    {
        
        /// <summary>
        /// //Returns the Validator instance for passed mete command type
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IResponseValidator GetValidator(MeterCommandType commandType)
        {
            IResponseValidator validator =null;
            switch(commandType)
            {
                case MeterCommandType.DLMS:
                   validator =  new DLMSValidator();
                   break;
                case MeterCommandType.FASTDOWNLOAD:
                   validator = new FastDownloadValidator();
                   break;
                case MeterCommandType.IEC:
                   validator = new IECValidator();
                   break;
            }
            return validator;
        }
    }
}
