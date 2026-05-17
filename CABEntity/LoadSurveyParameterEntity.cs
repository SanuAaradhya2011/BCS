/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class LoadSurveyParameterEntity : EntityBase
    {
        private string columnsNames;
        public string ColumnsNames
        {
            get
            {
                return columnsNames;
            }
            set
            {
                columnsNames=value;
            }
        }
        private long meterDataId;
        public long MeterDataId
        {
            get
            {
                return meterDataId;
            }
            set
            {
                meterDataId = value;
            }
        }
            
    }
}
