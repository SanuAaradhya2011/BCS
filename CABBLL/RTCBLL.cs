using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.DALC.Data;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CABEntity;
using LTCTDAL;
using CAB.Framework;
using CAB.Framework.Entity;

namespace LTCTBLL
{
    public class RTCBLL
    {
        public string GetWriteCommand(DateTime dateTime)
        {
            string rtcCommandData = string.Empty;
           
            //Get HexValue.
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Second));
          
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Minute));
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Hour));
            
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue((int)dateTime.DayOfWeek));
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Day));
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Month));
            rtcCommandData += GetHexValueByEachSymbol(GetHexOfDecimalValue(dateTime.Year));

            return rtcCommandData;
        }
        private string GetHexOfDecimalValue(int decimaVal)
        {
            string tmp = decimaVal.ToString();
            if (tmp.Length > 2)
                tmp = tmp.Substring(2);
            decimaVal=Convert.ToInt32(tmp);
            return decimaVal.ToString();
        }
         
        private  string GetHexValueByEachSymbol(string inputString)
        {
            string tmp = string.Empty;
            while (inputString.Length < 2)
            { inputString = inputString.Insert(0, "0"); };

            foreach (char ch in inputString)
            {
                tmp += String.Format("{0:x2}", Convert.ToInt32(ch));
            }
            return tmp;
        }
        public IEntity InsertData(IEntity entity)
        {
            RTCDAL rtcDAL = new RTCDAL();
            //rtcDAL.DeleteAllData( entity);
            return rtcDAL.InsertData(entity);
        }
        public string GetData( Int64 MeterData_ID)
        {
            return new RTCDAL().GetData( MeterData_ID);
        }

         public bool DeleteData(long meterData_ID)
        {
            return new RTCDAL().DeleteData(meterData_ID);
        }

    }
}
