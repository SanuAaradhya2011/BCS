using System;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class StructureInfoEntity : EntityBase
    { 
        private int structureInfoID;
        private int structureID;
        private string structureName;
        private int valueInBit;
        private int valueInByte;
        private string signType;
        private bool signByteFieldSpecified;
        public int StructureInfoID
        {
            get { return structureInfoID; }
            set { structureInfoID = value; }
        }
        public int StructureID
        {
            get { return structureID; }
            set { structureID = value; }
        }
        public string StructureName
        {
            get { return structureName; }
            set { structureName = value; }
        }
        public int ValueInBit
        {
            get { return valueInBit; }
            set { valueInBit = value; }
        }
        public int ValueInByte
        {
            get { return valueInByte; }
            set { valueInByte = value; }
        }
        public string SignType
        {
            get { return signType; }
            set { signType = value; }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SignByteSpecified
        {
            get
            {
                return this.signByteFieldSpecified;
            }
            set
            {
                this.signByteFieldSpecified = value;
            }
        }
    }
}

