/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh										|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System;

namespace CAB.BLL
{
    public class ConsumerTypeBLL : IBLL
    {
        ConsumerTypeMasterDAL consumerTypeMasterDAL;

        public ConsumerTypeBLL()
        {
            consumerTypeMasterDAL = new ConsumerTypeMasterDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return consumerTypeMasterDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return consumerTypeMasterDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return consumerTypeMasterDAL.DeleteData(entity);
        }
        public IEntity GetDetailData(string id)
        {
            if (id.Equals(string.Empty))
                return null;
            return GetDetailData(Convert.ToInt32(id));
        }

        public IEntity GetDetailData(int id)
        {
            return consumerTypeMasterDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            DataSet dataSet = consumerTypeMasterDAL.ListDataSet();
            if (dataSet.Tables.Count < 1)
                return new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("SL. No", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("ConsumerType_ID", typeof(System.Int32)));
            table.Columns.Add(new DataColumn("Consumer Type", typeof(System.String)));
            int slno = 1;
            DataRow newRow;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                newRow = table.NewRow();
                newRow["SL. No"] = slno;
                newRow["ConsumerType_ID"] = row["ConsumerType_ID"];
                newRow["Consumer Type"] = row["ConsumerType_Name"];
                table.Rows.Add(newRow);
                slno++;
            }
            dataSet.Tables.Remove("table");
            dataSet.Tables.Add(table);
            return dataSet;
        }
         
        public IEntity ValidateType(string typeName)
        {
            return consumerTypeMasterDAL.GetDetailData(typeName);
        }

        public void InsertDefaultData()
        {
            consumerTypeMasterDAL.InsertDefaultConsumerType();
        }
    }
}


