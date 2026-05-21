/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 25/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;

namespace CAB.BLL
{
    public class CMRIMasterBLL : IBLL
    {
        CMRIMasterDAL cmriMasterDAL;

        public CMRIMasterBLL()
        {
            cmriMasterDAL = new CMRIMasterDAL();
        }

        public IEntity InsertData(IEntity entity)
        {
            return cmriMasterDAL.InsertData(entity);
        }

        public bool UpdateData(IEntity entity)
        {
            return cmriMasterDAL.UpdateData(entity);
        }

        public bool DeleteData(IEntity entity)
        {
            return cmriMasterDAL.DeleteData(entity);
        }

        public IEntity GetDetailData(int id)
        {
            return cmriMasterDAL.GetDetailData(id);
        }

        public DataSet ListDataSet()
        {
            return cmriMasterDAL.ListDataSet();
        }

        public bool ValidateCMRI(string cmriNumber)
        {
            return cmriMasterDAL.GetDetailData(cmriNumber);
        }

        public DataSet ComboList()
        {
            return cmriMasterDAL.ComboList();
        }

        public DataSet ListDataSet(string data)
        {
            return cmriMasterDAL.ListDataSet(data);
        }
    }
}
