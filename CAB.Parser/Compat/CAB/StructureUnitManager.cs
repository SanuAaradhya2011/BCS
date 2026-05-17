#region Namespaces
using System;

using CAB.Serialization;
#endregion
namespace CAB.Parser
{

    public class StructureUnitManager
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static object syncRoot = new Object();
        private static UnitInformation structureInformation = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public StructureUnitManager(Serializer serializer)
        {
           lock (syncRoot)
            {
                if (structureInformation == null)
                {

                    structureInformation = (UnitInformation)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory,"UnitInformation.xml"), typeof(UnitInformation));
                }
            }
        }
        #endregion

        #region Public Methods
        public string GetUnit(int id)
        {
            string unit = string.Empty;
            foreach (UnitEntity infoEntity in structureInformation.StructureUnitEntity)
            {
                if (infoEntity.StructureUnitID == id)
                {
                    unit = infoEntity.StructureUnit;
                    break;
                }
            }
            return unit;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }
}



