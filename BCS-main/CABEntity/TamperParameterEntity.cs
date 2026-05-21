using System;
using System.Collections.Generic;
using System.Text;
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class TamperParameterEntity : EntityBase
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
                columnsNames = value;
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
