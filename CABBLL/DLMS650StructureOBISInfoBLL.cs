
/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											           				            								|
 * | 																    											|
 * |----------------------------------------------------------------------------------------------------------------| */
using CAB.DALC.Data;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data;
using System.Collections.Generic;
using CAB.Entity;

namespace CAB.BLL
{
    public class DLMS650StructureOBISInfoBLL : IBLL
    {
        StructureOBISInfoDAL structureOBISInfoDAL;

        public DLMS650StructureOBISInfoBLL()
        {
            structureOBISInfoDAL = new StructureOBISInfoDAL();
            OBISInfoEntity oBISInfoEntity = GetDetailData("1.0.9.6.8.255") as OBISInfoEntity;
            if (oBISInfoEntity == null)
                structureOBISInfoDAL.InsertDefaultOBISStructure();
        }

        public IEntity GetDetailData(string id)
        {
            return structureOBISInfoDAL.GetDetailData(id);
        }
    }
}



