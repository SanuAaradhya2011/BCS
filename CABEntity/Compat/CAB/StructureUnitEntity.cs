using System;
using LNG.Framework.Entity;

namespace LNG.Entity
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class StructureUnitEntity : EntityBase
    {
        private int structureUnitInfoID;
        private int structureUnitID;
        private string structureUnitName;
        private string structureUnit;

        public int StructureUnitInfoID
        {
            get { return structureUnitInfoID; }
            set { structureUnitInfoID = value; }
        }
        public int StructureUnitID
        {
            get { return structureUnitID; }
            set { structureUnitID = value; }
        }
        public string StructureUnitName
        {
            get { return structureUnitName; }
            set { structureUnitName = value; }
        }
        public string StructureUnit
        {
            get { return structureUnit; }
            set { structureUnit = value; }
        }
    }
}

