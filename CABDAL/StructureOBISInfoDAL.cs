using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class StructureOBISInfoDAL : DALBase
    {
        private string StructureOBISInfoID = "StructureOBISInfoID";
        private string ClassID = "ClassID";
        private string Attribute = "Attribute";
        private string OBISName = "OBISName";
        private string OBISCode = "OBISCode";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(StructureOBISInfoDAL).ToString());
 
        public void InsertDefaultOBISStructure()
        {
            string[] qry = new string[71];
            qry[0] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(8,2,'Real Time Clock - Date and Time'','0.0.1.0.0.255')";
            qry[1] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'R Phase Current','1.0.31.27.0.255')";
            qry[2] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Y Phase Current','1.0.51.27.0.255')";
            qry[3] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'B Phase Current','1.0.71.27.0.255')";
            qry[4] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage R Phase','1.0.32.27.0.255')";
            qry[5] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage Y Phase','1.0.52.27.0.255')";
            qry[6] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage B Phase','1.0.72.27.0.255')";
            qry[7] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Block Energy - kWh','1.0.1.29.0.255')";
            qry[8] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Block Energy - kvarh - lag','1.0.5.29.0.255')";
            qry[9] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Block Energy - kvarh - lead','1.0.8.29.0.255')";
            qry[10] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Block Energy - kVAh','1.0.9.29.0.255')";
            qry[11] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'R Phase Current','1.0.31.7.0.255')";
            qry[12] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Y Phase Current','1.0.51.7.0.255')";
            qry[13] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'B Phase Current','1.0.71.7.0.255')";
            qry[14] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage R Phase','1.0.32.7.0.255')";
            qry[15] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage Y Phase','1.0.52.7.0.255')";
            qry[16] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Voltage B Phase','1.0.72.7.0.255')";
            qry[17] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Power Factor - R phase','1.0.33.7.0.255')";
            qry[18] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Power Factor - Y phase','1.0.53.7.0.255')";
            qry[19] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Power Factor - B phase','1.0.73.7.0.255')";
            qry[20] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Total Power Factor','1.0.13.7.0.255')";
            qry[21] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Frequency','1.0.14.7.0.255')";
            qry[22] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Apparent Power - KVA','1.0.9.7.0.255')";
            qry[23] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Signed Active Power - kW (+ Forward; -Reverse)','1.0.1.7.0.255')";
            qry[24] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Signed Reactive Power - kvar (+ Lag; - Lead)','1.0.3.7.0.255')";
            qry[25] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy - kWh','1.0.1.8.0.255')";
            qry[26] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy - kvarh(Lag)','1.0.5.8.0.255')";
            qry[27] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy - kvarh(Lead)','1.0.8.8.0.255')";
            qry[28] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy - kVAh','1.0.9.8.0.255')";
            qry[29] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(1,2,'Number of power failures','0.0.96.7.0.255')";
            qry[30] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative power-failure duration','0.0.94.91.8.255')";
            qry[31] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(1,2,'Cumulative Tamper count','0.0.94.91.0.255')";
            qry[32] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(1,2,'Cumulative Billing count','0.0.0.1.0.255')";
            qry[33] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(1,2,'Cumulative programming count','0.0.96.2.0.255')";
            qry[34] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Billing Date','0.0.0.1.2.255')";
            qry[35] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW','1.0.1.6.0.255')";
            qry[36] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA','1.0.9.6.0.255')";
            qry[37] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(1,2,'Voltage related events','0.0.96.11.0.255')";
            qry[38] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'System Power Factor for billing period (Average Power Factor)','1.0.13.0.0.255')";
            qry[39] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ1 (Tariff 1)','1.0.1.8.1.255')";
            qry[40] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ2','1.0.1.8.2.255')";
            qry[41] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ3','1.0.1.8.3.255')";
            qry[42] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ4','1.0.1.8.4.255')";
            qry[43] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ5','1.0.1.8.5.255')";
            qry[44] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ6','1.0.1.8.6.255')";
            qry[45] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ7','1.0.1.8.7.255')";
            qry[46] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kWh – TZ8','1.0.1.8.8.255')";
            qry[47] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ1','1.0.9.8.1.255')";
            qry[48] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ2','1.0.9.8.2.255')";
            qry[49] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ3','1.0.9.8.3.255')";
            qry[50] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ4','1.0.9.8.4.255')";
            qry[51] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ5','1.0.9.8.5.255')";
            qry[52] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ6','1.0.9.8.6.255')";
            qry[53] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ7','1.0.9.8.7.255')";
            qry[54] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(3,2,'Cumulative Energy – kVAH – TZ8','1.0.9.8.8.255')";
            qry[55] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ1 ','1.0.1.6.1.255')";
            qry[56] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ2','1.0.1.6.2.255')";
            qry[57] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ3','1.0.1.6.3.255')";
            qry[58] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ4','1.0.1.6.4.255')";
            qry[59] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ5','1.0.1.6.5.255')";
            qry[60] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ6','1.0.1.6.6.255')";
            qry[61] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ7','1.0.1.6.7.255')";
            qry[62] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kW – TZ8','1.0.1.6.8.255')";
            qry[63] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ1','1.0.9.6.1.255')";
            qry[64] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ2','1.0.9.6.2.255')";
            qry[65] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ3','1.0.9.6.3.255')";
            qry[66] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ4','1.0.9.6.4.255')";
            qry[67] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ5','1.0.9.6.5.255')";
            qry[68] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ6','1.0.9.6.6.255')";
            qry[69] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ7','1.0.9.6.7.255')";
            qry[70] = "Insert Into StructureOBISInfo(ClassID,Attribute,OBISName,OBISCode) values(4,2,'MD – kVA – TZ8','1.0.9.6.8.255')";

            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 18; i++)
            {
                DataRequest request = new DataRequest(qry[i]);
                helper.ExecuteNonQuery(request);
            }
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
        public IEntity GetDetailData(string obisCode)
        {
            OBISInfoEntity obisInfoEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select StructureOBISInfoID,ClassID,Attribute,OBISName,OBISCode from StructureOBISInfo where ");
                builder.Append(string.Concat(OBISCode, "=", ParameterName(OBISCode)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(OBISCode), obisCode, DbType.String, 100);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    obisInfoEntity = (OBISInfoEntity)RowToEntity(ds.Tables[0].Rows[0]);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string obisCode)", ex);
                obisInfoEntity = null;
            }
            return obisInfoEntity;
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
            if (row == null) return null;
            OBISInfoEntity obisInfoEntity = new OBISInfoEntity();
            if (NotNullAndNotDBNull(row, StructureOBISInfoID)) obisInfoEntity.StructureOBISInfo = Convert.ToInt32(row[StructureOBISInfoID]);
            if (NotNullAndNotDBNull(row, ClassID)) obisInfoEntity.ClassID = Convert.ToInt32(row[ClassID]);
            if (NotNullAndNotDBNull(row, Attribute)) obisInfoEntity.Attribute = Convert.ToInt32(row[Attribute]);
            if (NotNullAndNotDBNull(row, OBISName)) obisInfoEntity.OBISName = Convert.ToString(row[OBISName]);
            if (NotNullAndNotDBNull(row, OBISCode)) obisInfoEntity.OBISCode = Convert.ToString(row[OBISCode]);
            return obisInfoEntity;
        }
    }
}

