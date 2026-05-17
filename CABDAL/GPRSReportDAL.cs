using System;
using System.Collections.Generic;
using System.Data;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class GPRSReportDAL : DALBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSReportDAL).ToString());

        public override IEntity InsertData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override IEntity RowToEntity(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to fetch the report data for GPRS Tasks
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetGRPSSearchResult(string searchType, DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GetGPRSSearchData", CommandType.StoredProcedure);
                request.AddParamter("searchBy", searchType.ToUpper(), DbType.String);
                request.AddParamter("startDate", fromDate.ToLongDateTimeCABFormat().ToString(), DbType.String);
                request.AddParamter("endDate", toDate.ToLongDateTimeCABFormat().ToString(), DbType.String);
                helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGRPSSearchResult(string searchType, DateTime fromDate, DateTime toDate)", ex);
            }

            return dataSet;
        }



        /// <summary>
        /// Method to fetch the report data for GPRS Tasks Log
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public DataSet GetGRPSTaskLog(DateTime fromDate, DateTime toDate)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GETRemoteCommunicationLogs", CommandType.StoredProcedure);                
                request.AddParamter("startDate", fromDate.ToLongDateTimeCABFormat().ToString(), DbType.String);
                request.AddParamter("endDate", toDate.ToLongDateTimeCABFormat().ToString(), DbType.String);
                helper.FillDataSet(request, dataSet);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetGRPSTaskLog(DateTime fromDate, DateTime toDate)", ex);
            }

            return dataSet;
        }




    }
}
