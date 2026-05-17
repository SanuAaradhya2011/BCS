using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class BillingParameterEntity : EntityBase
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

