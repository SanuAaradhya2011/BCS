/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using CAB.Framework.Entity;


namespace CAB.Entity
{
    public class AreaEntity : EntityBase
    {
        private long area_ID;
        private int region_ID;
        private int circle_ID;
        private int divsion_ID;
        private int cMRI_ID;
        public long Area_ID
        {
            get
            {
                return area_ID;
            }
            set
            {
                area_ID = value;
            }
        }
        public int Region_ID
        {
            get
            {
                return region_ID;
            }
            set
            {
                region_ID = value;
            }
        }
        public int Circle_ID
        {
            get
            {
                return circle_ID;
            }
            set
            {
                circle_ID = value;
            }
        }
        public int Divsion_ID
        {
            get
            {
                return divsion_ID;
            }
            set
            {
                divsion_ID = value;
            }
        }
        public int CMRI_ID
        {
            get
            {
                return cMRI_ID;
            }
            set
            {
                cMRI_ID = value;
            }
        }
    }
}


