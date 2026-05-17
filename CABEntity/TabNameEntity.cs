#region NameSpaces
using CAB.Framework.Entity;
#endregion

namespace CABEntity
{
    /// <summary>
    /// This class is entity for Tabs data.
    /// </summary>
    class TabNameEntity : EntityBase
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private int meterDataID;
        private string analysisTabID;
        private bool isVisible;
        private string description;
        #endregion

        #region Properties
        public int MeterDataID
        {
            get
            {
                return meterDataID;
            }
            set
            {
                meterDataID = value;
            }
        }

        public string AnalysisTabID
        {
            get
            {
                return analysisTabID;
            }
            set
            {
                analysisTabID = value;
            }
        }
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }
}
