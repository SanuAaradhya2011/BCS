
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
    public class DLMS650StructureInfoBLL : IBLL
    {
        StructureInfoDAL structureInfoDAL;
        static StructureInformation structureInformation = null;
        private static object syncRoot = new Object();
        CABSerializer deSerializer = null;
        public DLMS650StructureInfoBLL()
        {
            //structureInfoDAL = new StructureInfoDAL();
            //StructureInfoEntity structureInfoEntity = GetDetailData(0) as StructureInfoEntity;
            //if (structureInfoEntity == null)
            //    structureInfoDAL.InsertDefaultStructure();
            lock (syncRoot)
            {
                if (structureInformation == null)
                {
                    deSerializer = new CABSerializer();

                    structureInformation = (StructureInformation)deSerializer.DeserializeToObject("StructureInformation.xml", typeof(StructureInformation));
                }
            }
        }

        public IEntity GetDetailData(int id)
        {
            StructureInfoEntity matchedEntity = null;
            foreach (StructureInfoEntity infoEntity in structureInformation.StructureInfoEntity)
            {
                if (infoEntity.StructureID == id)
                {
                    matchedEntity = infoEntity;
                    break;
                }
            }
            return matchedEntity;
        }
    }
}


