#region NameSpaces
using System;
using System.Data;
using System.Text;
using ExceptionServices.Data;
using CAB.DALC.Data;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

#endregion

namespace LTCTDAL
{

    /// <summary>
    /// Class containing Tabname DB operations
    /// </summary>
    public class TabNameDAL : DALBase
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
       // private string meterid = ConfigInfo.ActiveMeterDataId;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TabNameDAL).ToString());
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods

        /// <summary>
        /// Method returns a dataset of tabs to be hidden
        /// </summary>
        /// <returns></returns>
        public DataSet GetTabsToHide(long meterid)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select AnalysisTab_ID , IsVisible from `dlms_ltct_650`.`tabname` where IsVisible = false and MeterData_ID= @meterid");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName ("@meterid"), meterid , DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Visibility Tab Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTabsToHide(long meterid)", ex);
            }
            return dataSet;
        }


        /// <summary>
        /// gets all the names of the tabs which are true in database
        /// </summary>
        /// <param name="meterid"></param>
        /// <returns></returns>
        public DataSet GetTabsToShow(long meterid)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("select AnalysisTab_ID , IsVisible from `dlms_ltct_650`.`tabname` where IsVisible = true and MeterData_ID= @meterid");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("@meterid"), meterid, DbType.Int64);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Visibility Tab Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTabsToShow(long meterid)", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Method deletes data from tabname table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void DeleteData(long meterid)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from `dlms_ltct_650`.`tabname` where MeterData_ID= @meterid");
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName("@meterid"), meterid, DbType.Int64);
            helper.ExecuteNonQuery(request);
        }


        /// <summary>
        /// This function inserts data in the tabname table
        /// </summary>
        /// <param name="MeterData_Id"></param>
        /// <param name="AnalysisTab_Id"></param>
        /// <param name="isVisible"></param>
        /// <param name="description"></param>
        public void UpdateTabName(long MeterData_Id, string AnalysisTab_Id, bool isVisible, string description)
        {
            IDataHelper helper = DatabaseFactory.GetHelper();
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into `dlms_ltct_650`.`tabname` (MeterData_ID, AnalysisTab_ID, IsVisible, Description) values(@MeterData_ID,@AnalysisTab_ID,@IsVisible,@Description)");
            DataRequest request = new DataRequest(builder.ToString());
            request.AddParamter(ParameterName("@MeterData_ID"), MeterData_Id, DbType.Int64);
            request.AddParamter(ParameterName("@AnalysisTab_ID"), AnalysisTab_Id, DbType.String);
            request.AddParamter(ParameterName("@IsVisible"), isVisible, DbType.Boolean);
            request.AddParamter(ParameterName("@Description"), description, DbType.String);
            helper.ExecuteNonQuery(request);

        }

        public override CAB.Framework.Entity.IEntity InsertData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity InsertData(System.Collections.Generic.IList<CAB.Framework.Entity.IEntity> entities)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteData(CAB.Framework.Entity.IEntity entity)
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity GetDetailData(int id)
        {
            throw new NotImplementedException();
        }

        public override System.Collections.Generic.IList<CAB.Framework.Entity.IEntity> ListData()
        {
            throw new NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new NotImplementedException();
        }

        public override CAB.Framework.Entity.IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }
}
