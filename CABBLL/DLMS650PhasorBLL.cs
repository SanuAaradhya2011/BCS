using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.Framework;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using System.Data;
using CAB.Entity;
namespace CAB.BLL
{
    public class DLMS650PhasorBLL : IBLL
    {
        DLMS650CommonBLL commonBLL;
        DLMS650PhasorDAL dlms650PhasorDLL = null;
        public DLMS650PhasorBLL() {
            dlms650PhasorDLL = new DLMS650PhasorDAL();
            commonBLL = new DLMS650CommonBLL();
        }

        public IEntity GetPhasorDataEntity(int meterDataId)
        {
            PhasorEntity phasorEntity = null;
            DataSet dsPhasor = dlms650PhasorDLL.GetPhasorDetailData(meterDataId);
            dsPhasor = commonBLL.ApplyMultiplyFactor(meterDataId, dsPhasor);
            if (dsPhasor != null && dsPhasor.Tables[0] != null && dsPhasor.Tables[0].Rows.Count > 0)
            {
                phasorEntity = (PhasorEntity)dlms650PhasorDLL.RowToEntity(dsPhasor.Tables[0].Rows[0]);
            }
            return phasorEntity;
        }
        public IEntity GetPhasorDataEntity(DataTable dtTable)
        {
            return dlms650PhasorDLL.RowToEntity(dtTable.Rows[0], true);
        }

        public DataSet GetPhasorDataSet(int meterDataId)
        {
            return  dlms650PhasorDLL.GetPhasorDataByMeter(meterDataId.ToString());
        }
        public void InsertData(IEntity entity)
        {
             dlms650PhasorDLL.InsertData(entity);
        }
        public bool DeleteData(long meterDataID)
        {
            return dlms650PhasorDLL.DeleteData(meterDataID);
        }
    }
}
