#region Namespaces
using System;
#endregion

namespace CAB.Parser
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class StructureInfoEntity
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private int structureInfoID;
        private int structureID;
        private string structureName;
        private string className;
        private int valueInBit;
        private int valueInByte;
        private string signType;
        private bool signByteFieldSpecified;
        #endregion

        #region Properties
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
        public string ClassName
        {
            get { return className; }
            set { className = value; }
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
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        #endregion

        #region Protecetd Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }
}
