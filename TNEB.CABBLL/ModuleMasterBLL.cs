/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic;
using System.Collections;
using CAB.DALC.Data;
using CAB.Entity;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Entity;
using System.Data;

namespace CAB.BLL
{
	public class ModuleMasterBLL : IBLL
	{
		ModuleMasterDAL moduleMasterDAL;
		public ModuleMasterBLL()
		{
			moduleMasterDAL = new ModuleMasterDAL();
		}

		public DataSet GetAllData()
		{
			return moduleMasterDAL.GetAllData();
		}

        public void InsertDefaultData()
        {
            moduleMasterDAL.InsertDefaultModule();
        }

        public int GetModuleIdByName(string moduleName)
        {
            return moduleMasterDAL.GetModuleIdByName(moduleName);
        }
        public void DeleteAllData()
        {
            moduleMasterDAL.DeleteAllData();
        }
	}
}
