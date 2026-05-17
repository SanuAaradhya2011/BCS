#region Namespaces
using System;
using CAB.Serialization;

#endregion
namespace CAB.Parser
{

    public class StructureInfoManager
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private object syncRoot = new Object();
        private static StructureInfo structureInformation = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public StructureInfoManager(Serializer serializer)
        {
           lock (syncRoot)
            {
                if (structureInformation == null)
                {

                    structureInformation = (StructureInfo)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "StructureInfo.xml"), typeof(StructureInfo));
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get the structure info entity object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StructureInfoEntity GetUnitInfo(int id)
        {
            StructureInfoEntity InfoEntity = null;
            foreach (StructureInfoEntity infoEntity in structureInformation.StructureInfoEntity)
            {
                if (infoEntity.StructureID == id)
                {
                    InfoEntity = infoEntity;
                    break;
                }
            }
            return InfoEntity;
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
