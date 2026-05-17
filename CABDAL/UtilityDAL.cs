using System;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class UtilityDAL
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(UtilityDAL).ToString());
        
        public void InsertData(string utilityPassword, string utilityName)
        {
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert into utility(Utility_Name,Utility_Password) values('" + utilityName + "','" + utilityPassword + "')");
                DataRequest request = new DataRequest(builder.ToString());
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Utility Inserted"));
                Flag = true;
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(string utilityPassword, string utilityName)", ex);
            }
        }

        public DataSet GetUtilityPassword()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select Utility_Password from Utility");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Utility Password Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetUtilityPassword()", ex);
            }
            return dataSet;
        }

        /// <summary>
        /// Method to get the utlity 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetUtility()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT * from utility LIMIT 0,1;");
                DataRequest request = new DataRequest(builder.ToString());
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Utility Password Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetUtility()", ex);
            }
            return dataSet;
        }
    }
}
