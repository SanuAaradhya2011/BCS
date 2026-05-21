using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNG.Entity
{
    public static class GlobalClass
    {
        private static string gBcsStatus;

        public static string GBcsStatus
        {
            get { return GlobalClass.gBcsStatus; }
            set { GlobalClass.gBcsStatus = value; }
        }
    }
}

