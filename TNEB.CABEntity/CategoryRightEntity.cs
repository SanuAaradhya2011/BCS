/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.IECFramework.Entity;

namespace CAB.Entity
{
    public class CategoryRightEntity : EntityBase
    {
        private int category_ID;
        private int module_ID;
        private int defaultRight;
        public int Category_ID
        {
            get
            {
                return category_ID;
            }
            set
            {
                category_ID = value;
            }
        }
        public int Module_ID
        {
            get
            {
                return module_ID;
            }
            set
            {
                module_ID = value;
            }
        }
        public int DefaultRight
        {
            get
            {
                return defaultRight;
            }
            set
            {
                defaultRight = value;
            }
        }
    }
}



