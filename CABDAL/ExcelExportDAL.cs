#region NameSpaces
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Framework;
using CAB.Framework.Entity;
using CABEntity;
using CAB.Entity;
using Hunt.EPIC.Logging;

#endregion

namespace CAB.DALC.Data
{
    public class ExcelExportDAL : DALBase
    {
        #region Constants & variables   
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(ExcelExportDAL).ToString());
        #endregion

        #region Public Methods             

        public override IEntity InsertData(IEntity entitiy)
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


        public DataSet GetDataForExcelExport(long meterDataID)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                DataRequest request = new DataRequest("GetDataForExcelExport", CommandType.StoredProcedure);
                request.AddParamter(ParameterName("meterDataId"), meterDataID, DbType.Int64);
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Data Selected for Excel Export."));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataForExcelExport(long meterDataID)", ex);
                dataSet = null;
            }
            return dataSet;
        }

        public override IEntity RowToEntity(DataRow row)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
