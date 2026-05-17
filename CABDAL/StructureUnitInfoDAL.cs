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
    public class StructureUnitInfoDAL : DALBase
    {
        private string StructureUnitID = "StructureUnitID";
        private string StructureUnitName = "StructureUnitName";
        private string StructureUnit = "StructureUnit";
        private string StructureUnitInfoID = "StructureUnitInfoID";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(StructureUnitInfoDAL).ToString());

        public void InsertDefaultUnit()
        {
            string[] qry = new string[26];
            qry[0] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(1,'Year','a')";
            qry[1] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(2,'Month','mo')";
            qry[2] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(3,'Week','wk')";
            qry[3] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(4,'Day','d')";
            qry[4] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(5,'Hour','h')";
            qry[5] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(6,'Minute','min.')";
            qry[6] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(7,'Second','s')";
            qry[7] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(11,'Metre','m')";
            qry[8] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(12,'Metre Per Second','m/s')";
            qry[9] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(19,'Litre','l')";
            qry[10] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(20,'Kilogram','kg')";
            qry[11] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(27,'Watt','w')";
            qry[12] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(28,'Volt-Ampere','VA')";
            qry[13] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(29,'Var','Var')";
            qry[14] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(30,'Watt-Hour','Wh')";
            qry[15] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(31,'Volt-Ampere-Hour','VAh')";
            qry[16] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(32,'Var-Hour','varh')";
            qry[17] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(33,'Ampere','A')";
            qry[18] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(35,'Volt','V')";
            qry[19] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(44,'Hertz','Hz')";
            qry[20] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(45,'Active Energy Metre Constant','1/(Wh)')";
            qry[21] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(46,'Reactive Energy Metre Constant','1/(varh)')";
            qry[22] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(52,'Apperent Energy Metre Constant','1/(VAh)')";
            qry[23] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(253,'Reserved','')";
            qry[24] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(254,'Other Unit','')";
            qry[25] = "Insert Into StructureUnitInfo(StructureUnitID,StructureUnitName,StructureUnit) values(255,'No Unit','')";
            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 26; i++)
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
            StructureUnitEntity structureUnitEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select StructureUnitInfoID,StructureUnitID,StructureUnitName,StructureUnit from StructureUnitInfo where ");
                builder.Append(string.Concat(StructureUnitID, "=", ParameterName(StructureUnitID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(StructureUnitID), id, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds == null)
                    return structureUnitEntity;
                if (ds.Tables.Count == 0)
                    return structureUnitEntity;

                if (ds.Tables[0].Rows.Count > 0)
                    structureUnitEntity = (StructureUnitEntity)RowToEntity(ds.Tables[0].Rows[0]);
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                structureUnitEntity = null;
            }
            return structureUnitEntity;
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
            StructureUnitEntity structureUnitEntity = new StructureUnitEntity();
            if (NotNullAndNotDBNull(row, StructureUnitInfoID)) structureUnitEntity.StructureUnitInfoID = Convert.ToInt32(row[StructureUnitInfoID]); 
            if (NotNullAndNotDBNull(row, StructureUnitID)) structureUnitEntity.StructureUnitID = Convert.ToInt32(row[StructureUnitID]);
            if (NotNullAndNotDBNull(row, StructureUnitName)) structureUnitEntity.StructureUnitName = Convert.ToString(row[StructureUnitName]);
            if (NotNullAndNotDBNull(row, StructureUnit)) structureUnitEntity.StructureUnit = Convert.ToString(row[StructureUnit]);
            return structureUnitEntity;
        }
    }
}
