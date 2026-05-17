using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using System.Data.Common;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{ 
    public class SearchControlDAL: DALBase
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(SearchControlDAL).ToString());
        public DataSet ListDataSet(string filter)
        {
            DataSet dataSet=null;
            if (!string.IsNullOrEmpty(filter))
            {
                string[] info = filter.Split('|');
                try
                {
                    IDataHelper helper = DatabaseFactory.GetHelper();
                    StringBuilder builder = new StringBuilder();                    
                    builder.Append(string.Concat("Select Distinct ",info[1],",",info[2]," from ",info[0]));
                    DataRequest request = new DataRequest(builder.ToString());
                    dataSet = new DataSet();
                    dataSet = helper.FillDataSet(request, dataSet);
                }
                catch (CABException ex)    //Exception log for catch block
                {
                    logger.Log(LOGLEVELS.Error, "ListDataSet(string filter)", ex);
                } 
            }
            return dataSet;  
        }



        public override IEntity InsertData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override IEntity InsertData(System.Collections.Generic.IList<IEntity> entities)
        {
            throw new System.NotImplementedException();
        }

        public override bool UpdateData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteData(IEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public override IEntity GetDetailData(int id)
        {
            throw new System.NotImplementedException();
        }

        public override System.Collections.Generic.IList<IEntity> ListData()
        {
            throw new System.NotImplementedException();
        }

        public override DataSet ListDataSet()
        {
            throw new System.NotImplementedException();
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new System.NotImplementedException();
        }
    }
}
