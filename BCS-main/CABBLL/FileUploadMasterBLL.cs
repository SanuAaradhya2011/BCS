using System;
using System.Collections.Generic;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data.DataServices;
using CAB.Framework.Utility;

namespace CAB.BLL
{
    public class FileUploadMasterBLL : IBLL
    {
        FileUploadMasterDAL fileUploadMasterDAL;

        public FileUploadMasterBLL()
        {
            fileUploadMasterDAL = new FileUploadMasterDAL();
        }
        public IEntity InsertData(IEntity entity)
        {
            return fileUploadMasterDAL.InsertData(entity, true);
        }
       
        public IEntity ValidateFile(string fileName, long readingDateTime)
        {
            return fileUploadMasterDAL.GetDetailData(fileName, readingDateTime);
        }
        public IEntity ValidateFile(string fileName)
        {
            return fileUploadMasterDAL.GetDetailData(fileName);
        }
        public Dictionary<string, int> ListDataSet()
        {
            return fileUploadMasterDAL.ListDataSet(true);
        }
        public DataSet GetCABFileNames()
        {
            DataTable dt = fileUploadMasterDAL.AutoNumberedTable(fileUploadMasterDAL.GetCABFileNames().Tables[0]);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        public DataSet GetCABFileNamesBetweenDates(DateTime fromDate, DateTime toDate)
        {
            //DataTable dt = fileUploadMasterDAL.AutoNumberedTable(fileUploadMasterDAL.GetCABFileNamesBetweenDates(DateUtility.DateTimeToLong(fromDate),DateUtility.DateTimeToLong(toDate)).Tables[0]);
            //Pass the boolean ShowMeterModelNo to function so that it can query database accordingly 
            DataTable dt = fileUploadMasterDAL.AutoNumberedTable(fileUploadMasterDAL.GetCABFileNamesBetweenDates(DateUtility.DateTimeToLong(fromDate), DateUtility.DateTimeToLong(toDate), true).Tables[0]);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }
        /// <summary>
        /// To get the file name as per the meter data id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public DataSet GetCABFileNameWithMeterDataId(long MeterDataID)
        {
            DataSet ds = new DataSet();
            ds = fileUploadMasterDAL.GetCABFileNamesWithMeterDataID(MeterDataID);
            return ds;
        }
        public IEntity ListDataSet(int id)
        {
            return fileUploadMasterDAL.GetDetailData(id);
        }
        public DataSet ComboList()
        {
            return fileUploadMasterDAL.ComboList();
            //DataSet ds=
            //if (ds == null)
            //    return null;
            //if (ds.Tables.Count == 0)
            //    return null;
            //if (ds.Tables[0].Rows.Count == 0)
            //    return null;
            //DataTable table = new DataTable();
            //foreach (DataColumn col in ds.Tables[0].Columns)
            //{
            //table.Columns.Add(col.ColumnName);
            //    break;
            //}
            //DataRow row;
            //DataRow dataRow = ds.Tables[0].Rows[0];
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    row = table.NewRow();
            //    string val1 = Convert.ToString(dr[0]);
            //    string val = Convert.ToString(dr[1]);
            //    if (!string.IsNullOrEmpty(val))
            //    {
            //        string value = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(dr[1]));
            //        if (value.Length > 10)
            //            value = value.Substring(0, 10);
            //        val = val1 + " (" + value + ")";
            //    }
            //    else
            //        val = val1;
            //    row[0] = val;
            //    table.Rows.Add(row);
            //}
            //DataSet dataSet = new DataSet();
            //dataSet.Tables.Add(table);
            //return dataSet;
        }

        public bool DeleteData(IEntity entity)
        {
            return fileUploadMasterDAL.DeleteData(entity);
        }

        public void UpdateCMRIID(DataSet fuploadIDs, string cmriNo)
        {
            fileUploadMasterDAL.UpdateCMRIID(fuploadIDs, cmriNo);
        }
    }
}