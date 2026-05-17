/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.Framework.Entity;


namespace CAB.Entity
{
    public class ConsumerExportSettingsEntity : EntityBase
    {
        private int consumerExportSettings_ID;
        private string fileName;
        private string parametersName;
        private string parameterColumn;
        public int ConsumerExportSettings_ID
        {
            get
            {
                return consumerExportSettings_ID;
            }
            set
            {
                consumerExportSettings_ID = value;
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }
        public string ParametersName
        {
            get
            {
                return parametersName;
            }
            set
            {
                parametersName = value;
            }
        }
        public string ParameterColumn
        {
            get
            {
                return parameterColumn;
            }
            set
            {
                parameterColumn = value;
            }
        }
    }
}



