#region NameSpaces
using System.Data;
using CAB.Framework;
using LTCTDAL;
#endregion

namespace LTCTBLL
{
    /// <summary>
    /// Class containing function calls for Tabname DB operations
    /// </summary>
    public class TabNameBLL : IBLL
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        TabNameDAL tabNameDAL = null;
        DataSet dataSet = null;

        #endregion

        #region Properties
        #endregion

        #region Constructor
        public TabNameBLL()
        {
            tabNameDAL = new TabNameDAL();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetNoDataTabs(long meterId)
        {
            return tabNameDAL.GetTabsToHide(meterId );           
        }
        /// <summary>
        /// gets all the names of the tabs which are true in database
        /// </summary>
        /// <param name="meterId"></param>
        /// <returns></returns>
        public DataSet GetTabsToShow(long meterId)
        {
            return tabNameDAL.GetTabsToShow(meterId);
        }
        /// <summary>
        /// Method calls function to delete data from table tabname
        /// </summary>
        /// <param name="meterId"></param>
        public void DeleteTabnameData(long meterId)
        {
            tabNameDAL.DeleteData(meterId);
        }
        
        /// <summary>
        /// Method calls function to insert data in table tabname
        /// </summary>
        /// <param name="MeterData_Id"></param>
        /// <param name="AnalysisTab_Id"></param>
        /// <param name="isVisible"></param>
        /// <param name="description"></param>
        public void InsertIntoTabName(long MeterData_Id, string AnalysisTab_Id, bool isVisible, string description)
        {
            tabNameDAL.UpdateTabName(MeterData_Id, AnalysisTab_Id, isVisible, description);
        }
    
        #endregion

        #region Protected Methhods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion

    }

}
