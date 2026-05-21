using System;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    public class OBISInfoEntity : EntityBase
    {
        private int structureOBISInfo;
        private int classID;
        private int attribute;
        private string  obisName;
        private string obisCode;

        public int StructureOBISInfo
        {
            get { return structureOBISInfo; }
            set { structureOBISInfo = value; }
        }
        public int ClassID
        {
            get { return classID; }
            set { classID = value; }
        }
        public int Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }
        public string OBISName
        {
            get { return obisName; }
            set { obisName = value; }
        }
        public string OBISCode
        {
            get { return obisCode; }
            set { obisCode = value; }
        }
    }
}

