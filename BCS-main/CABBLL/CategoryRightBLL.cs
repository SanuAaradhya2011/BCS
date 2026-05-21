 
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
    public class CategoryRightBLL : IBLL
    {
        CategoryRightDAL categoryRightDAL;

        public CategoryRightBLL()
        {
            categoryRightDAL = new CategoryRightDAL();
        }

        public DataSet GetRight(int categoryID)
        {
            return categoryRightDAL.ListDataSet(categoryID);
        }
        public void InsertDefaultRight()
        {
            categoryRightDAL.InsertDefaultRight();
        }
    }
}



