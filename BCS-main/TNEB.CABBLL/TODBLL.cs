using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LTCTDAL;

namespace LTCTBLL
{
    public class TODBLL
    {
        public bool InsertData(string todData, string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            TODDAL todDAL = new TODDAL();
            todDAL.DeleteAllData(meterID, fileUploadID, MeterData_ID);
            return todDAL.InsertData(todData, meterID, fileUploadID, MeterData_ID);
        }
        public string GetData(string meterID, Int64 fileUploadID, Int64 MeterData_ID)
        {
            return new TODDAL().GetData(meterID, fileUploadID, MeterData_ID);
        }
    }
}
