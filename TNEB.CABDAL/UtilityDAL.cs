using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using CAB.DALC.Data;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using CABEntity;

namespace LTCTDAL
{
    public class UtilityDAL : DALBase
    {
      
      private string Utility_Name = "UtilityName";
      private string Utility_Password = "UtilityPassword";

      public void InsertData(string UtilityPassword,string UtilityName)
      {
          bool Flag = false;
          try
          {
              IDataHelper helper = DatabaseFactory.GetHelper();
              StringBuilder builder = new StringBuilder();
              builder.Append("Insert into Utility(Utility_Name,Utility_Password) values(");
              builder.Append(string.Concat(ParameterName(Utility_Name), ","));
              builder.Append(string.Concat(ParameterName(Utility_Password), ")"));
              DataRequest request = new DataRequest(builder.ToString());
              request.AddParamter(ParameterName(Utility_Password), UtilityPassword, DbType.String, 15);
              request.AddParamter(ParameterName(Utility_Name), UtilityName, DbType.String, 20);
              helper.ExecuteNonQuery(request);
              UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Utility Inserted"));
              Flag = true;
          }
          catch (Exception ex)
          { 
          }
      }
      public DataSet  GetUtilityPassword()
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
          catch (Exception ex)
          {
 
          }
          return dataSet;
      }
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

      public override IEntity RowToEntity(DataRow row)
      {
          throw new NotImplementedException();
      }
    }
}
