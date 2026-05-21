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
    public class StructureInfoDAL : DALBase
    {
        private string StructureInfoID = "StructureInfoID";
        private string StructureID = "StructureID";
        private string StructureName = "StructureName";
        private string ValueInBit = "ValueInBit";
        private string ValueInByte = "ValueInByte";
        private string SignType = "SignType";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(StructureInfoDAL).ToString());

        public void InsertDefaultStructure()
        {
            string[] qry = new string[18];
            qry[0] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(0,'Null-Date',0,0,'Null Value')";
            qry[1] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(1,'Array',8,1,'')";
            qry[2] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(2,'Structure',8,1,'')";
            qry[3] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(3,'Boolean',8,1,'B')";
            qry[4] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(5,'Double-Long',32,4,'P')";
            qry[5] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(6,'Double-Long-Unsigned',32,4,'PM')";
            qry[6] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(15,'Integer',8,1,'PM')";
            qry[7] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(16,'Long',16,2,'PM')";
            qry[8] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(17,'Unsigned',8,1,'P')";
            qry[9] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(18,'Long Unsigned',16,2,'P')";
            qry[10] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(20,'Long64',64,8,'PM')";
            qry[11] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(21,'Long64 Unsigned',64,8,'P')";
            qry[12] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(22,'Enum',8,1,'')";
            qry[13] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(23,'Float32',32,4,'P')";
            qry[14] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(24,'Float64',64,8,'P')";
            qry[15] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(25,'DateTime',96,12,'')";
            qry[16] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(26,'Date',40,5,'')";
            qry[17] = "Insert Into StructureInfo(StructureID,StructureName,ValueInBit,ValueInByte,SignType) values(27,'Time',32,4,'')";
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
            StructureInfoEntity structureInfoEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select StructureInfoID,StructureID,StructureName,ValueInBit,ValueInByte,SignType from StructureInfo where ");
                builder.Append(string.Concat(StructureID, "=", ParameterName(StructureID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(StructureID), id, DbType.Int32);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if(ds!=null)
                    if(ds.Tables.Count>0)
                if (ds.Tables[0].Rows.Count > 0)
                    structureInfoEntity = (StructureInfoEntity)RowToEntity(ds.Tables[0].Rows[0]); 
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                structureInfoEntity = null;
            }
            return structureInfoEntity;
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
            StructureInfoEntity structureInfoEntity = new StructureInfoEntity();
            if (NotNullAndNotDBNull(row, StructureInfoID)) structureInfoEntity.StructureInfoID = Convert.ToInt32(row[StructureInfoID]);
            if (NotNullAndNotDBNull(row, StructureID)) structureInfoEntity.StructureID = Convert.ToInt32(row[StructureID]); 
            if (NotNullAndNotDBNull(row, StructureName)) structureInfoEntity.StructureName = Convert.ToString(row[StructureName]);
            if (NotNullAndNotDBNull(row, ValueInBit)) structureInfoEntity.ValueInBit = Convert.ToInt32(row[ValueInBit]); 
            if (NotNullAndNotDBNull(row, ValueInByte)) structureInfoEntity.ValueInByte = Convert.ToInt32(row[ValueInByte]);
            if (NotNullAndNotDBNull(row, SignType)) structureInfoEntity.SignType = Convert.ToString(row[SignType]);
            return structureInfoEntity;
        }
    }
}
