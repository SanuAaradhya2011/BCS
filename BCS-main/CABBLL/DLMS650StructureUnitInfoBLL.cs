
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
using CAB.Framework.Utility;
using System;
namespace CAB.BLL
{
    public class DLMS650StructureUnitInfoBLL : IBLL
    {
        StructureUnitInfoDAL structureUnitInfoDAL;
        static StructureUnitInformation structureUnitInformation = null;
        private static object syncRoot = new Object();
        CABSerializer deSerializer = null;
        public DLMS650StructureUnitInfoBLL()
        {
            //structureUnitInfoDAL = new StructureUnitInfoDAL();
            //StructureUnitEntity structureUnitEntity = GetDetailData(1) as StructureUnitEntity;
            //if (structureUnitEntity == null)
            //    structureUnitInfoDAL.InsertDefaultUnit();
            lock (syncRoot)
            {
                if (structureUnitInformation == null)
                {
                    deSerializer = new CABSerializer();
                    structureUnitInformation = (StructureUnitInformation)deSerializer.DeserializeToObject("StructureUnitInformation.xml", typeof(StructureUnitInformation));
                }
            }
        }

        public IEntity GetDetailData(int id)
        {
            StructureUnitEntity matchedEntity = null;
            foreach (StructureUnitEntity infoEntity in structureUnitInformation.StructureUnitEntity)
            {
                if (infoEntity.StructureUnitID == id)
                {
                    matchedEntity = infoEntity;
                    break;
                }
            }
            return matchedEntity;
        }
    }
}




