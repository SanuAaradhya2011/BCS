using System;
using System.Windows.Forms;
using CAB.BCS.DLMS.Model;
using CAB.BCS.DLMS.Utility;
using CAB.BCS.DLMS.Views;
using CAB.BLL;
using CAB.Framework;
using Utilities;
using System.Collections.Generic;
namespace CAB.BCS.DLMS.Presenter
{

    /// <summary>
    /// This clas is used as a presenter for the TOU view in the DLMS_Main.cs class.
    /// </summary>
    class TOUPresenter
    {
        #region Constants

        private const string ZONE = "Zone";
        private const string MONDAY = "Mon";
        private const string TUESDAY = "Tue";
        private const string WEDNESDAY = "Wed";
        private const string THURSDAY = "Thu";
        private const string FRIDAY = "Fri";
        private const string SATURDAY = "Sat";
        private const string SUNDAY = "Sun";
        private const string DAY = "Day";
        private const string Month = "Month";
        private const string TARIFF = "Tariff";
        private const string COLZONE = "colZone";
        private const string COLMONDAY = "colMon";
        private const string COLTUESDAY = "colTue";
        private const string COLWEDNESDAY = "colWed";
        private const string COLTHURSDAY = "colThu";
        private const string COLFRIDAY = "colFri";
        private const string COLSATURDAY = "colSat";
        private const string COLSUNDAY = "colSun";
        private const string COLDAY = "colDay";
        private const string COLMONTH = "colMonth";
        private const string COLSESSION = "colSeason";
        private const string WEEKPROFILE = "Week Profile";
        private const string COLTARIFF = "colTariff";
        private const string COLSTARTHOUR = "colStartHour";
        private const string STARTHOUR = "Start Hour";
        private const string COLSTARTMIN = "colStartMin";
        private const string STARTMIN = "Start Min";
        private const string WEEK = "Week";
       

        
        #endregion

        #region Variables
        TOUModel touModel = new TOUModel();
        DataGridView[] dayProfileGrids;
        DataGridView seasonProfileGrids;
        DataGridView weekProfileGrids;
        DataGridViewComboBoxColumn gridViewComboBoxCol = new DataGridViewComboBoxColumn();
        byte[] HDLCCommand = new byte[200];
        private readonly ITOUDefinition viewTOU;
        int rIndex = 0;
        int count = 0;
        int rcount = 0;
        int gIndex = 0;
        byte dayProfileCount;
        byte weekProfileCount;
        byte seasonProfileCount;
        List<byte> touData;
        private DateTime activationDate = DateTime.MinValue;
        #endregion

        #region Properties
        public DateTime ActivationDate
        {
            get
            {
                return activationDate;
            }
            set
            {
                activationDate = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// This is the presenter class constuructor and accpeting the view here.
        /// constructor type dependency injection is being used.
        /// </summary>
        /// <param name="view">Please pass the view for the presenter.</param>
        public TOUPresenter(ITOUDefinition view)
        {
            if (view == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("TOUVIEWCANNOTBENULL"));
            viewTOU = view;

            if (viewTOU.TOUGridNames == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("TOUGRIDSCANNOTBENULL"));
            dayProfileGrids = viewTOU.TOUGridNames;

            if (viewTOU.GridDayTables == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("TOUDAYTABLEGRIDCANNOTBENULL"));
            weekProfileGrids = viewTOU.GridDayTables;

            if (viewTOU.GridActivationDate == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("TOUACTIVATIONGRIDCANNOTBENULL"));
            seasonProfileGrids = viewTOU.GridActivationDate;

            if (UtilityDetails.ShowOneTOU)
            {
                dayProfileCount=1;
                weekProfileCount=1;
                seasonProfileCount=1;
            }
            else if (UtilityDetails.ShowTwoTOU)
            {
                dayProfileCount = 2;
                weekProfileCount = 2;
                seasonProfileCount = 2;
            }
            else
            {
                dayProfileCount = 24;
                weekProfileCount = 4;
                seasonProfileCount = 4;
            }

        }
        #endregion

        #region Public methods
        /// <summary>
        /// This method is used for resetting the all tou details from the grids.
        /// </summary>        
        public void ResetAllTOU()
        {
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rCount = 0; rCount < dayProfileGrids[gridCount].RowCount; rCount++)
                    {
                        for (int cCount = 1; cCount < dayProfileGrids[gridCount].ColumnCount; cCount++)
                        {
                            dayProfileGrids[gridCount].Rows[rCount].Cells[cCount].Value = null;
                        }
                    }
                }
                for (int rCount = 0; rCount < weekProfileCount; rCount++)
                {
                    for (int cCount = 1; cCount < weekProfileGrids.ColumnCount; cCount++)
                    {
                        weekProfileGrids.Rows[rCount].Cells[cCount].Value = null;
                    }
                }

                for (int rCount = 0; rCount < seasonProfileCount; rCount++)
                {
                    for (int cCount = 0; cCount < seasonProfileGrids.ColumnCount; cCount++)
                    {
                        seasonProfileGrids.Rows[rCount].Cells[cCount].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            viewTOU.FutureActivationDate = DateTime.Now;
        }

        /// <summary>
        /// This method is used to auto fill other tou grids if first tou grid is already filled.
        /// </summary>
        public void AutoFillTOU()
        {
            if (dayProfileGrids[0].Rows[0].Cells[COLTARIFF].Value != null && dayProfileGrids[0].Rows[0].Cells[COLTARIFF].Value.ToString() != string.Empty) 
            {
                for (int gridCount = 1; gridCount < dayProfileCount; gridCount++)
                {
                    for (int rowCount = 0; rowCount < dayProfileGrids[0].Rows.Count; rowCount++)
                    {
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLTARIFF].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value;
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value;
                        dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTMIN].Value = dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value;
                    }
                }
                for (int rowCount = 0; rowCount < weekProfileCount; rowCount++)
                {
                    for (int colCount = 0; colCount <= weekProfileGrids.ColumnCount - 1; colCount++)
                    {
                        if (colCount == 0)
                        {
                            weekProfileGrids.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString();
                        }
                        else
                        {
                            weekProfileGrids.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");
                        }
                    }
                }
                for (int rowCount = 0; rowCount < seasonProfileCount; rowCount++)
                {
                    for (int colCount = 0; colCount <= seasonProfileGrids.ColumnCount - 1; colCount++)
                    {
                        seasonProfileGrids.Rows[rowCount].Cells[colCount].Value = (rowCount + 1).ToString("00");
                    }
                }
            }
            
        }

        /// <summary>
        /// This method is used for clearing the all TOU datagrids.
        /// </summary>        
        public void ClearGrids()
        {
            for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
            {
                for (int rowCount = 0; rowCount < dayProfileGrids[gridCount].RowCount; rowCount++)
                {
                    dayProfileGrids[gridCount].Rows[rowCount].Cells[COLTARIFF].Value = null;
                    dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                    dayProfileGrids[gridCount].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                }
            }
            
            for (int rowCount = 0; rowCount < weekProfileCount; rowCount++)
            {
                for (int colCount = 1; colCount < weekProfileGrids.ColumnCount; colCount++)
                {
                    weekProfileGrids.Rows[rowCount].Cells[colCount].Value = null;
                }
            }
            for (int rowCount = 0; rowCount < seasonProfileCount; rowCount++)
            {

                seasonProfileGrids.Rows[rowCount].Cells[COLMONTH].Value = null;
                seasonProfileGrids.Rows[rowCount].Cells[COLDAY].Value = null;
                seasonProfileGrids.Rows[rowCount].Cells[COLSESSION].Value = null;
            }
        }

        /// <summary>
        /// This method is used to bind all the grids on the view load method.
        /// </summary>
        public void BindTOUGridCoulmun()
        {
            BindDayProfileGrid();
            BindWeekProfileGrid();
            BindSeasonProfileGrid();
        }

        /// <summary>
        /// This method is used to day profile  grids on the view load method.
        /// </summary>
        private void BindDayProfileGrid()
        {
            DataGridView GridName;
            try
            {
                for (int count = 0; count < dayProfileCount ; count++)
                {
                    GridName = new DataGridView();
                    GridName = dayProfileGrids[count];
                    GridName.ColumnCount = 0;
                    GridName.RowHeadersVisible = false;
                    GridName.Columns.Add(GetDataGridView(10, COLZONE, ZONE, 35));
                    GridName.Columns.Add(GetDataGridView(8, COLTARIFF, TARIFF, 39));
                    GridName.Columns.Add(GetDataGridView(23, COLSTARTHOUR, STARTHOUR, 39));
                    GridName.Columns.Add(GetDataGridView(3, COLSTARTMIN, STARTMIN, 39));
                    GridName.RowCount = 10;
                    for (int index = 0; index < GridName.RowCount; index++)
                    {
                        GridName.Rows[index].Cells[0].Value = (index + 1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used to bind season profile grid on view load method
        /// </summary>        
        private void BindSeasonProfileGrid()
        {
            try
            {
                seasonProfileGrids.Columns.Add(GetDataGridView(31,COLDAY,DAY,40));
                seasonProfileGrids.Columns.Add(GetDataGridView(12,COLMONTH,Month,40));
                seasonProfileGrids.Columns.Add(GetDataGridView(seasonProfileCount, COLSESSION, WEEKPROFILE, 50));
                seasonProfileGrids.RowCount = seasonProfileCount; 
                seasonProfileGrids.RowHeadersVisible = false;
                seasonProfileGrids.Rows[0].Cells[COLDAY].ReadOnly = true;
                seasonProfileGrids.Rows[0].Cells[COLMONTH].ReadOnly = true;
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// This method is used to bind week details grid on the view load method.
        /// </summary>      
        private void BindWeekProfileGrid()
        {
            try
            {
                weekProfileGrids.RowHeadersVisible = false;
                weekProfileGrids.Columns.Add(GetDataGridView(4, COLZONE, WEEK, 50));
                int dayIdinTOUGrid = dayProfileCount == 24 ? 6 : dayProfileCount;
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLMONDAY, MONDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLTUESDAY, TUESDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLWEDNESDAY, WEDNESDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLTHURSDAY, THURSDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLFRIDAY, FRIDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLSATURDAY, SATURDAY, 37));
                weekProfileGrids.Columns.Add(GetDataGridView(dayIdinTOUGrid, COLSUNDAY, SUNDAY, 37));

                weekProfileGrids.RowCount = weekProfileCount;
                for (int index = 0; index < weekProfileCount; index++)
                {
                    weekProfileGrids.Rows[index].Cells[COLZONE].Value = (index + 1).ToString();
                }
                weekProfileGrids.Columns[COLZONE].ReadOnly = true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Used to create columns for various profile data grids
        /// This method is called while adding columns to various data grids
        /// </summary>
        /// <param name="numberOfItems"></param>
        /// <param name="columnName"></param>
        /// <param name="headerText"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        DataGridViewComboBoxColumn GetDataGridView(int numberOfItems, string columnName, string headerText, int width)
        {
            int index = 1;
            DataGridViewComboBoxColumn gridViewComboBox = new DataGridViewComboBoxColumn();
            gridViewComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            gridViewComboBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gridViewComboBox.Width = width;
            gridViewComboBox.Name = columnName;
            gridViewComboBox.HeaderText = headerText;
            if (headerText == STARTHOUR || headerText == STARTMIN)
            {
                index = 0;
            }
            for (; index <= numberOfItems; index++)
            {
                if (headerText == TARIFF)
                {
                    gridViewComboBox.Items.Add("T" + index.ToString());
                }
                else if (headerText == STARTMIN)
                {
                    gridViewComboBox.Items.Add((index * 15).ToString("00"));
                }
                else if (headerText == WEEK || headerText == ZONE)
                {
                    gridViewComboBox.Items.Add(index.ToString());
                }
                else
                {
                    gridViewComboBox.Items.Add(index.ToString("00"));
                }
            }
            return gridViewComboBox;

        }
        
        /// <summary>
        /// Used to validate data in cells of all profile grids
        /// </summary>
        /// <returns></returns>
        public bool ValidateTOUGrids()
        {
            bool validTOU = true;
            validTOU=ValidateDayTOUGrids();
            validTOU=ValidateWeekTOUGrids();
            validTOU=ValidateSeasonTOUGrids();
            return validTOU;

        }
        
        /// <summary>
        ///  used to validate data of dat profile grid
        /// </summary>
        /// <returns></returns>
        private bool ValidateDayTOUGrids()
        {
            bool validTOU = true; 
            CoreUtility.ExpMessage = string.Empty;
            try
            {
                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {

                    if (dayProfileGrids[gridCount].Rows[0].Cells[COLTARIFF].Value == null)
                    {
                        if (UtilityDetails.ShowTwoTOU || UtilityDetails.ShowOneTOU)
                        {
                            CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("TODTABLECANNOTBEBLANK") + Symbols.NEWLINE;
                        }
                        else
                        {
                            CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("TOUDAYTABLECANNOTBEBLANK") + Symbols.NEWLINE;
                        }
                        validTOU = false;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validTOU;
        }
    
       
        /// <summary>
        ///  used to validate data of week profile grid
        /// </summary>
        /// <returns></returns>
        private bool ValidateWeekTOUGrids()
        {
            bool validTOU = true;            
            try
            {
                for (int rowCount = 0; rowCount < weekProfileGrids.RowCount; rowCount++)
                {
                    for (int colCount = 1; colCount < weekProfileGrids.ColumnCount; colCount++)
                    {
                        if (weekProfileGrids.Rows[rowCount].Cells[colCount].Value == null)
                        {
                            CoreUtility.ExpMessage += CoreUtility.GetMessageFromResourceFile("TOUWEEKTABLECANNOTBEBLANK") + Symbols.NEWLINE;
                            validTOU = false;
                            break;
                        }
                    }
                    if (!validTOU)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validTOU;
        }

        /// <summary>
        /// used to validate data of season profile grid
        /// </summary>
        /// <returns></returns>
        private bool ValidateSeasonTOUGrids()
        {
            bool validTOU = true;            
            try
            {
                for (int rowCount = 0; rowCount < seasonProfileGrids.RowCount; rowCount++)
                {
                    if (seasonProfileGrids.Rows[rowCount].Cells[COLMONTH].Value == null)
                    {
                        CoreUtility.ExpMessage += CoreUtility.GetMessageFromResourceFile("TOUSEASONTABLECANNOTBEBLANK");
                        validTOU = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return validTOU;
        }

        /// <summary>
        /// This method is called while click on the season grid from the view and validating the grid.
        /// </summary>
        public void SeasonGridCellClick()
        {
            try
            {
                rIndex = seasonProfileGrids.CurrentCell.RowIndex;
                if (weekProfileGrids.Rows[rIndex].Cells[7].Value == null)
                {
                    seasonProfileGrids.ReadOnly = true;
                }
                else
                {
                    seasonProfileGrids.ReadOnly = false;
                    if (seasonProfileGrids.Rows[rIndex].Cells[0].Value == null)
                    {
                        seasonProfileGrids.Rows[rIndex].Cells[1].Value = null;
                        seasonProfileGrids.Rows[rIndex].Cells[1].ReadOnly = true;
                    }
                    else
                    {
                        seasonProfileGrids.Rows[rIndex].Cells[1].ReadOnly = false;

                        for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                        {
                            if (dayProfileGrids[gridCount].Rows[0].Cells[1].Value == null
                                && dayProfileGrids[gridCount].Rows[0].Cells[2].Value == null
                                && dayProfileGrids[gridCount].Rows[0].Cells[3].Value == null)
                            {
                                seasonProfileGrids.ReadOnly = true;
                                break;
                            }
                            else
                            {
                                seasonProfileGrids.ReadOnly = false;

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// This method is used for validating day profile grid value on the cell click event. 
        /// </summary>
        public void DayGridCellClick()
        {
            DataGridView dataGrid = viewTOU.DataGridViewSenderObject as DataGridView;
            try
            {
                rcount = dataGrid.CurrentCell.RowIndex;
                if (rcount != 0 && dataGrid.Rows[rcount - 1].Cells[1].Value != null)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[2].Value != null && dataGrid.Rows[rcount - 1].Cells[3].Value != null)
                    {
                        if (dataGrid.Rows[rcount - 1].Cells[2].Value.ToString() == "23" && dataGrid.Rows[rcount - 1].Cells[3].Value.ToString() == "45")
                        {
                            for (count = rcount; count < 10; count++)
                            {
                                dataGrid.Rows[count].ReadOnly = true;
                            }
                            return;
                        }
                    }
                }
               
                for (gIndex = 0; gIndex < dayProfileCount ; gIndex++)
                {
                    for (int rowCount = 0; rowCount < 9; rowCount++)
                    {
                        if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value != null))
                        {
                            if ((dayProfileGrids[gIndex].Rows[rowCount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[2].Value.ToString()) 
                                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rowCount + 1].Cells[3].Value)))
                            {
                                while (rowCount < 8)
                                {
                                    dayProfileGrids[gIndex].Rows[rowCount + 2].ReadOnly = true;
                                    rowCount++;
                                }
                                return;
                            }
                        }
                    }
                }

                if (rcount != 0)
                {
                    if (dataGrid.Rows[rcount - 1].Cells[3].Value == null)
                    {
                        int rowCount = rcount;
                        while (rowCount < 10)
                        {
                            dataGrid.Rows[rowCount].ReadOnly = true;
                            rowCount++;
                        }
                        if (dataGrid.Rows[rcount - 1].Cells[1].Value == null && dataGrid.Rows[rcount - 1].Cells[2].Value == null)
                        { 
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        for (count = 1; count <= 3; count++)
                        {
                            dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                        }
                        rIndex = rcount + 1;
                        while (rIndex < 10)
                        {
                            dataGrid.Rows[rIndex].ReadOnly = false;
                            rIndex++;
                        }
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value == null)
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].Value = null;
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = true;
                    }
                    count = 0;
                    while (dayProfileGrids[count].Name != dataGrid.Name)
                    {
                        count++;
                    }

                    for (gIndex = 0; gIndex < dayProfileCount ; gIndex++)
                    {
                        for (rIndex = 0; rIndex < 10; rIndex++)
                        {
                            if (dayProfileGrids[gIndex].Rows[rIndex].Cells[1].Value != null 
                                && (dayProfileGrids[gIndex].Rows[rIndex].Cells[2].Value == null 
                                || dayProfileGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                            {
                                count = gIndex;
                                while (count < 3)
                                {
                                    dayProfileGrids[count + 1].ReadOnly = true;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfileGrids[count - 1].ReadOnly = true;
                                        count--;
                                    }
                                }
                                weekProfileGrids.ReadOnly = true;
                                seasonProfileGrids.ReadOnly = true;
                                return;
                            }
                            else
                            {
                                count = gIndex;
                                while (count < 3)
                                {
                                    dayProfileGrids[count + 1].ReadOnly = false;
                                    count++;
                                }

                                if (gIndex > 0)
                                {
                                    count = gIndex;
                                    while (count != 0)
                                    {
                                        dayProfileGrids[count - 1].ReadOnly = false;
                                        count--;
                                    }
                                }
                                weekProfileGrids.ReadOnly = false;
                                seasonProfileGrids.ReadOnly = false;

                            }
                        }
                    }
                    rIndex = rcount + 1;
                    return;
                }
                else
                {
                    for (count = 2; count <= 3; count++)
                    {
                        dataGrid.Rows[rcount].Cells[count].ReadOnly = false;
                    }
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }


                if (dataGrid.Rows[rcount].Cells[1].Value != null 
                    && dataGrid.Rows[rcount].Cells[2].Value == null)
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = true;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    dataGrid.Rows[rcount].Cells[3].ReadOnly = false;
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                if (dataGrid.Rows[rcount].Cells[1].Value != null 
                    && (dataGrid.Rows[rcount].Cells[2].Value == null 
                        && dataGrid.Rows[rcount].Cells[3].Value == null))
                {
                    rIndex = rcount + 1;
                    while (rIndex < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = true;
                        rIndex++;
                    }
                    return;
                }
                else
                {
                    rIndex = rcount + 1;
                    while (rIndex  < 10)
                    {
                        dataGrid.Rows[rIndex].ReadOnly = false;
                        rIndex++;
                    }
                }

                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rIndex = 0; rIndex < 10; rIndex++)
                    {
                        if (dayProfileGrids[gIndex].Rows[rIndex].Cells[1].Value != null 
                            && (dayProfileGrids[gIndex].Rows[rIndex].Cells[2].Value == null 
                            || dayProfileGrids[gIndex].Rows[rIndex].Cells[3].Value == null))
                        {
                            count = gIndex;
                            while (count < 3)
                            {
                                dayProfileGrids[count + 1].ReadOnly = true;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfileGrids[count - 1].ReadOnly = true;
                                    count--;
                                }
                            }
                            weekProfileGrids.ReadOnly = true;
                            seasonProfileGrids.ReadOnly = true;
                            return;
                        }
                        else
                        {
                            count = gIndex;
                            while (count < 3)
                            {
                                dayProfileGrids[count + 1].ReadOnly = false;
                                count++;
                            }

                            if (gIndex > 0)
                            {
                                count = gIndex;
                                while (count != 0)
                                {
                                    dayProfileGrids[count - 1].ReadOnly = false;
                                    count--;
                                }
                            }
                            weekProfileGrids.ReadOnly = false;
                            seasonProfileGrids.ReadOnly = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                dataGrid.Rows[0].Cells[2].ReadOnly = true;
                dataGrid.Rows[0].Cells[3].ReadOnly = true;
                dataGrid.Columns[0].ReadOnly = true;
            }

        }

        /// <summary>
        /// This method is used for week profile grid value  checks on the cell click event. 
        /// </summary>
        public void WeekGridCellClick()
        {
            DataGridView weekProfileGrids = viewTOU.DataGridViewSenderObject as DataGridView;
            try
            {
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                if (rIndex != 0 && seasonProfileGrids.Rows[rIndex - 1].Cells[1].Value == null)
                {
                    int rowIndex = rIndex;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.Rows[rIndex].ReadOnly = true;
                        rowIndex++;
                    }
                    return;
                }
                else
                {
                    int rowIndex = rIndex + 1;
                    while (rowIndex < weekProfileCount)
                    {
                        weekProfileGrids.ReadOnly = false;
                        rowIndex++;
                    }
                }

                int colIndex = weekProfileGrids.CurrentCell.ColumnIndex;
                if (colIndex != 0 && (weekProfileGrids.Rows[rIndex].Cells[colIndex - 1].Value == null))
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].Value = null;
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = true;
                    return;
                }
                else
                {
                    weekProfileGrids.Rows[rIndex].Cells[colIndex].ReadOnly = false;
                }

                for (int gridCount = 0; gridCount < dayProfileCount; gridCount++)
                {
                    if (dayProfileGrids[gridCount].Rows[0].Cells[1].Value == null
                        && dayProfileGrids[gridCount].Rows[0].Cells[2].Value == null
                        && dayProfileGrids[gridCount].Rows[0].Cells[3].Value == null)
                    {
                        weekProfileGrids.ReadOnly = true;
                        break;
                    }
                    else
                    {
                        weekProfileGrids.ReadOnly = false;

                    }
                }
                for (gIndex = 0; gIndex < dayProfileCount; gIndex++)
                {
                    for (rcount = 0; rcount < 9; rcount++)
                    {
                        if ((dayProfileGrids[gIndex].Rows[rcount].Cells[2].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rcount].Cells[3].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rcount + 1].Cells[2].Value != null) 
                            && (dayProfileGrids[gIndex].Rows[rcount + 1].Cells[3].Value != null))
                        {
                            if ((dayProfileGrids[gIndex].Rows[rcount].Cells[2].Value.ToString() == dayProfileGrids[gIndex].Rows[rcount + 1].Cells[2].Value.ToString()) 
                                && (Convert.ToInt16(dayProfileGrids[gIndex].Rows[rcount].Cells[3].Value) >= Convert.ToInt16(dayProfileGrids[gIndex].Rows[rcount + 1].Cells[3].Value)))
                            {
                                while (rcount < 8)
                                {
                                    dayProfileGrids[gIndex].Rows[rcount + 2].ReadOnly = true;
                                    rcount++;
                                }
                                weekProfileGrids.ReadOnly = true;
                                seasonProfileGrids.ReadOnly = true;
                                return;
                            }
                        }
                    }
                }
                rIndex = weekProfileGrids.CurrentCell.RowIndex;
                count = weekProfileGrids.CurrentCell.ColumnIndex;
                if (count >= 2)
                {
                    if (weekProfileGrids.Rows[rIndex].Cells[count].Value == null)
                    {
                        if (weekProfileGrids.Rows[rIndex].Cells[count - 1].Value == null)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = true;
                            seasonProfileGrids.Rows[rIndex].ReadOnly = true;
                        }
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = true;
                            count++;
                        }
                        return;
                    }
                    else
                    {
                        weekProfileGrids.Rows[rIndex].Cells[count].ReadOnly = false;
                        while (count < 7)
                        {
                            weekProfileGrids.Rows[rIndex].Cells[count + 1].ReadOnly = false;
                            count++;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                weekProfileGrids.Columns[0].ReadOnly = true;
            }
        }

        /// <summary>
        /// This method is used for validating day profile grid value on cell click event from the view.
        /// </summary>
        public void ValidateDayProfileCell()
        {
            DataGridView dtView = viewTOU.DataGridViewSenderObject as DataGridView;
            DataGridViewCellValidatingEventArgs e = viewTOU.EventArgs;
            try
            {
                if (dtView.CurrentCell.IsInEditMode == true)
                {
                    if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (e.ColumnIndex == 1)
                    {
                        if (e.FormattedValue == null)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                        }
                        if (e.FormattedValue.ToString() != "")
                        {
                            if (e.RowIndex == 0)
                            {
                                dtView.Rows[e.RowIndex].Cells[2].Value = "00";
                                dtView.Rows[e.RowIndex].Cells[3].Value = "00";
                                for (int colCount = 0; colCount <= weekProfileGrids.ColumnCount - 1; colCount++)
                                {
                                    weekProfileGrids.Rows[0].Cells[colCount].Value = "1";
                                }
                                seasonProfileGrids.Rows[0].Cells[0].Value = "01";
                                seasonProfileGrids.Rows[0].Cells[1].Value = "01";
                                seasonProfileGrids.Rows[0].Cells[2].Value = "1";

                            }
                        }
                        rcount = dtView.CurrentCell.RowIndex;
                        if (dtView.Rows[rcount].Cells[1].Value == null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            if (dtView.Rows[rcount].Cells[1].EditedFormattedValue.ToString() != "")
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                            else
                            {
                                int rowIndex = rcount + 1;
                                while (rowIndex < 10)
                                {
                                    dtView.Rows[rowIndex].ReadOnly = true;
                                    rowIndex++;
                                }
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null &&
                            dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }

                    }
                    if (e.ColumnIndex == 2)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                        }
                        if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                        {
                            e.Cancel = true;
                        }
                        else if (Convert.ToInt16(e.FormattedValue) > 23)
                        {
                            e.Cancel = true;
                        }

                        else
                        {

                        }
                        if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                e.Cancel = true;
                            }
                            else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    for (count = e.RowIndex + 2; count < 10; count++)
                                    {
                                        dtView.Rows[count].ReadOnly = true;
                                    }
                                }
                                
                            }
                        }
                        if (e.RowIndex != 0 && e.RowIndex != 1)
                        {
                            if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value != null)//added on 13 Aug
                            {
                                if (Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                {
                                    e.Cancel = true;
                                }

                                else if (e.FormattedValue.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value).ToString() == "45")
                                    {
                                        e.Cancel = true;
                                    }
                                    else if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        for (count = e.RowIndex + 1; count < 10; count++)
                                        {
                                            dtView.Rows[count].ReadOnly = true;
                                        }
                                       
                                    }
                                    
                                }
                            }
                        }
                        if (dtView.Rows[rcount].Cells[1].Value != null &&
                            (dtView.Rows[rcount].Cells[2].Value == null
                            || dtView.Rows[rcount].Cells[3].Value == null))
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = true;
                                rowIndex++;
                            }
                        }
                        else
                        {
                            int rowIndex = rcount + 1;
                            while (rowIndex < 10)
                            {
                                dtView.Rows[rowIndex].ReadOnly = false;
                                rowIndex++;
                            }
                        }
                    }
                    if (e.ColumnIndex == 3)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                e.Cancel = true;
                            }
                        }
                       
                        if (e.FormattedValue == null || Convert.ToInt16(e.FormattedValue) > 45)
                        {
                            e.Cancel = true;
                        }
                       
                        if (e.RowIndex != 9 && dtView.Rows[e.RowIndex + 1].Cells[1].Value != null)
                        {
                            if (Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString())
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        if (e.RowIndex != 0 && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                        {
                            if (dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value.ToString())
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                dtView.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                e.Cancel = true;
                throw ex;
            }
        }

        /// <summary>
        /// This method is used for validating week profile grid value on the cell click event. 
        /// </summary>
        public void ValidateWeekProfileCell()
        {
            DataGridView weekProfileGrids = viewTOU.DataGridViewSenderObject as DataGridView;
            DataGridViewCellValidatingEventArgs e = viewTOU.EventArgs;

            try
            {
                if (weekProfileGrids[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 1)
                        {
                            string gridVal = e.FormattedValue.ToString();
                            if (gridVal == "")
                            {
                                weekProfileGrids.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                                e.Cancel = true;
                            }
                            else
                            {
                                if (Convert.ToInt16(gridVal) < 1 || Convert.ToInt16(gridVal) > 6)
                                {
                                    weekProfileGrids.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                                    e.Cancel = true;
                                }
                                else
                                {
                                    weekProfileGrids.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        if (e.RowIndex == 0 && e.ColumnIndex == 7)
                        {
                            seasonProfileGrids.Rows[e.RowIndex].Cells[0].Value = "01";
                            seasonProfileGrids.Rows[e.RowIndex].Cells[1].Value = "01";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        /// <summary>
        /// This method is used for validating the season profile grid on cell click event.
        /// </summary>
        public void ValidateSeasonProfileCell()
        {

            DataGridViewCellValidatingEventArgs e = viewTOU.EventArgs;
            try
            {
                if (seasonProfileGrids[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }

                            if (e.FormattedValue.ToString() == "") { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 31))
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != seasonProfileCount - 1 
                                && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null 
                                && Convert.ToInt16(e.FormattedValue) >= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value) 
                                    == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0 && e.FormattedValue.ToString() != "" 
                                && e.FormattedValue.ToString() != null 
                                && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value)
                                    == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                        }
                        else if (e.ColumnIndex == 1)
                        {
                            if (e.RowIndex == 0)
                            {
                                if (e.FormattedValue.ToString() != "01")
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) > 29)
                            {
                                if (Convert.ToInt16(e.FormattedValue) == 2)
                                {
                                    seasonProfileGrids.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            if (e.FormattedValue.ToString() == "")
                            { }
                            else if (Convert.ToInt16(e.FormattedValue) < 1 || (Convert.ToInt16(e.FormattedValue) > 12))
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = CoreUtility.GetMessageFromResourceFile("INVALID");
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                seasonProfileGrids.Rows[e.RowIndex].ErrorText = "";
                            }
                            if (e.RowIndex != 0 && e.FormattedValue != null && e.FormattedValue.ToString() != "" 
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) 
                                    <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex - 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                            }

                            if (e.RowIndex != seasonProfileCount - 1 && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null
                                && Convert.ToInt16(e.FormattedValue) == Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) 
                                    >= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex + 1].Value))
                                {
                                    e.Cancel = true;
                                    return;
                                }

                                else
                                {
                                    e.Cancel = false;
                                }
                            }
                            else
                            {
                                e.Cancel = false;
                             }
                            if (e.RowIndex != seasonProfileCount - 1 
                                && seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value != null 
                                && Convert.ToInt16(e.FormattedValue) > Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex + 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.RowIndex != 0
                                && e.RowIndex != seasonProfileCount - 1 
                                && e.FormattedValue != null
                                && e.FormattedValue.ToString() != "" 
                                && Convert.ToInt16(e.FormattedValue) < Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                e.Cancel = false;
                            }
                            if (e.FormattedValue != null && e.FormattedValue.ToString() != "" && Convert.ToInt16(e.FormattedValue) == 2)
                            {
                                if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex - 1, e.RowIndex].Value) == 29)
                                {
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (e.RowIndex != 0 && e.ColumnIndex == 0)
                    {
                        if (e.RowIndex != 0 && e.FormattedValue != null 
                            && e.FormattedValue.ToString() != "" 
                            && Convert.ToInt16(e.FormattedValue) <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex, e.RowIndex - 1].Value))
                        {
                            if (Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex].Value) 
                                <= Convert.ToInt16(seasonProfileGrids[e.ColumnIndex + 1, e.RowIndex - 1].Value))
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    weekProfileGrids[1, count].ReadOnly = true;
                                    count++;
                                }
                                return;
                            }
                            else
                            {
                                int count = e.RowIndex + 1;
                                while (count < seasonProfileCount)
                                {
                                    weekProfileGrids[1, count].ReadOnly = false;
                                    count++;
                                }
                            }
                        }
                      
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling session profile details in TOU grids.
        /// </summary>
        public void FillSeasonProfileParameters(byte[] buffer)
        {
            try
            {
                int nIndex = 2;
                for (byte seasonCount = 0; seasonCount < seasonProfileCount; seasonCount++)
                {
                    nIndex += 4;
                    seasonProfileGrids.Rows[seasonCount].Cells[COLSESSION].Value = buffer[nIndex++].ToString("00");
                    nIndex += 4;
                    seasonProfileGrids.Rows[seasonCount].Cells[COLMONTH].Value = buffer[nIndex++].ToString("00");
                    seasonProfileGrids.Rows[seasonCount].Cells[COLDAY].Value = buffer[nIndex++].ToString("00");
                    nIndex += 11;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method is used for filling weekly profile details in TOU grids.
        /// </summary>
        public void FillWeekProfileParameters(byte[] buffer)
        {
            int nIndex = 2;
            try
            {
                for (byte weekCount = 0; weekCount < weekProfileCount; weekCount++)
                {
                    nIndex += 5;
                    for (byte colCount = 1; colCount < 8; colCount++)
                    {
                        nIndex++;
                        weekProfileGrids.Rows[weekCount].Cells[colCount].Value = buffer[nIndex++].ToString("00");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling day profile details in TOU grids.
        /// </summary>
        public void FillDayProfileParameters(byte[] buffer,int meterModel)
        {
            try
            {
                if (meterModel == NamePlateConstants.RubyE250Value && UtilityDetails.ShowTwoTOU)
                {
                    int nIndex = 2;

                    nIndex += 6;
                    for (byte rowCount = 0; rowCount < 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        if (tariff == 0)
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                            dayProfileGrids[0].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }

                    for (byte dayCount = 1; dayCount < 6; dayCount++)
                    {
                        nIndex += 6;
                        for (byte rowCount = 0; rowCount < 10; rowCount++)
                        {
                            nIndex += 4;
                            nIndex++;
                            nIndex++;
                            nIndex += 12;
                            nIndex++;
                          
                        }
                    }

                    nIndex += 6;
                    for (byte rowCount = 0; rowCount < 10; rowCount++)
                    {
                        nIndex += 4;
                        string startHour = buffer[nIndex++].ToString("d2");
                        string startMin = buffer[nIndex++].ToString("d2");
                        nIndex += 12;
                        int tariff = buffer[nIndex++];
                        if (tariff == 0)
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                        }
                        else
                        {
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                            dayProfileGrids[1].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                        }
                    }
                }
                else
                {
                    int nIndex = 2;
                    for (byte dayCount = 0; dayCount < dayProfileCount; dayCount++)
                    {
                        nIndex += 6;
                        for (byte rowCount = 0; rowCount < 10; rowCount++)
                        {
                            nIndex += 4;
                            string startHour = buffer[nIndex++].ToString("d2");
                            string startMin = buffer[nIndex++].ToString("d2");
                            nIndex += 12;
                            int tariff = buffer[nIndex++];
                            if (tariff == 0)
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = null;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = null;
                            }
                            else
                            {
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTHOUR].Value = startHour;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLSTARTMIN].Value = startMin;
                                dayProfileGrids[dayCount].Rows[rowCount].Cells[COLTARIFF].Value = "T" + tariff.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used for filling TOU activation date.
        /// </summary>
        /// <param name="buffer"></param>        
        public void FillTOUActivationDate(byte[] buffer)
        {
            int nIndex = 0x02;
            DateTime activationDate;
            int activationYear = 0;
            try
            {
                activationYear = (activationYear | (int)buffer[nIndex++]) << 8;
                activationYear = (activationYear | (int)buffer[nIndex++]);
                int activationMonth = buffer[nIndex++];
                int activationDay = buffer[nIndex];
                viewTOU.FutureActivationDate = DateTime.TryParse(activationDay.ToString() + "/" + activationMonth.ToString() + "/" + activationYear.ToString(), out activationDate) ? activationDate : DateTime.MinValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods


        

        
        #endregion

        /// <summary>
        /// Gets data for all tou profiles , day , season and week.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        public byte[] GetTOUBuffer(ActivityCalender attribute,int meterModel)
        {
            byte[] nTOUData;
            switch (attribute)
            { 
                case ActivityCalender.PassiveSeasonProfile:
                case ActivityCalender.ActiveSeasonProfile:
                    nTOUData = GetSeasonProfileBuffer(meterModel);
                    break;
                case ActivityCalender.PassiveWeekProfile:
                case ActivityCalender.ActiveWeekProfile:
                    nTOUData = GetWeekProfileBuffer(meterModel);
                    break;
                case ActivityCalender.PassiveDayProfile:
                case ActivityCalender.ActiveDayProfile:
                    nTOUData = GetDayProfileBuffer(meterModel);
                    break;
                case ActivityCalender.ActivationDate:
                    nTOUData = GetActivationDateBuffer(meterModel);
                    break;
                default :
                    nTOUData = null;
                    break;
            }

            return nTOUData;
        }

        /// <summary>
        /// Used to get season profile data buffer.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetSeasonProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(TOUConstants.Array );
            if (meterModel == NamePlateConstants.RubyE250Value)// ruby
            {
                touData.Add(TOUConstants.MaxSeason);
            }
            else
            {
                touData.Add(seasonProfileCount);
            }

            for (byte i = 0; i < seasonProfileCount; i++)
            {
                touData.Add(TOUConstants.Structure);
                touData.Add(0x03);

                touData.Add(0x09);
                touData.Add(0x01);
                if (Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLSESSION].Value) == 0x00)
                {
                    touData.Add(0x01);
                }
                else
                {
                    touData.Add(Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLSESSION].Value));
                }
                touData.Add(0x09);
                touData.Add(0x0C);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLMONTH].Value));
                touData.Add(Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLDAY].Value));
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0xFF);
                touData.Add(0x80);
                touData.Add(0x00);
                touData.Add(0x00);
                touData.Add(0x09);
                touData.Add(0x01);
                if (Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLSESSION].Value) == 0x00)
                {
                    touData.Add(0x01);
                }
                else
                {
                    touData.Add(Convert.ToByte(seasonProfileGrids.Rows[i].Cells[COLSESSION].Value));
                }
            }
            if (meterModel == NamePlateConstants.RubyE250Value)// ruby
            {
                for (int i = seasonProfileCount; i < TOUConstants.MaxSeason; i++)
                {
                    touData.Add(TOUConstants.Structure);
                    touData.Add(0x03);
                    touData.Add(0x09);
                    touData.Add(0x01);
                    touData.Add(0x00);
                    touData.Add(0x09);
                    touData.Add(0x0C);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0xFF);
                    touData.Add(0x80);
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x09);
                    touData.Add(0x01);
                    touData.Add(0x00);
                }
            }
            return touData.ToArray();
        }

        /// <summary>
        /// Used to get buffer data for week profile writing
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetWeekProfileBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(TOUConstants.Array);
            if (meterModel == NamePlateConstants.RubyE250Value)// ruby
            {
                touData.Add(TOUConstants.MaxWeek);
            }
            else
            {
               touData.Add(weekProfileCount);
            }
           
            for (byte i = 0; i < weekProfileCount; i++)
            {
                touData.Add(TOUConstants.Structure);
                touData.Add(0x08);

                touData.Add(0x09);
                touData.Add(0x01);
                touData.Add((byte)(i + 1));

                for (byte j = 1; j < 8; j++)
                {
                    touData.Add(0x11);
                    touData.Add(Convert.ToByte(weekProfileGrids.Rows[i].Cells[j].Value) == 0x00 ? 
                        (byte)0x01 : Convert.ToByte(weekProfileGrids.Rows[i].Cells[j].Value));
                }
            }
            if (meterModel == NamePlateConstants.RubyE250Value)// ruby
            {
                for (int i = weekProfileCount; i < 4; i++)
                {
                    touData.Add(0x02);
                    touData.Add(0x08);

                    touData.Add(0x09);
                    touData.Add(0x01);
                   touData.Add(0x00);

                    for (byte j = 1; j < 8; j++)
                    {
                        touData.Add(0x11);
                        touData.Add(0x00);
                    }
                }
            }

            return touData.ToArray();
        }
         
        /// <summary>
        /// Used to get buffer for writing day profile data.
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetDayProfileBuffer(int meterModel)
        {
            byte tempDayProfileCount = dayProfileCount;
            touData = new List<byte>();
            touData.Add(TOUConstants.Array);
            if (meterModel == NamePlateConstants.RubyE250Value)// ruby
            {
                if ( UtilityDetails.ShowTwoTOU)
                {
                    tempDayProfileCount = 1;
                }
                touData.Add(TOUConstants.MaxDay );
            }
            else
            {
                touData.Add(tempDayProfileCount);
            }
            
            for (byte i = 0; i < tempDayProfileCount; i++)
            {
                touData.Add(0x02);
                touData.Add(0x02);
                touData.Add(0x11);
                touData.Add((byte)(i+1));             //Day Id 
                touData.Add(0x01);
                touData.Add(0x0A);

                for (byte j = 0; j < 10; j++)
                {
                    touData.Add(0x02);
                    touData.Add(0x03);
                    touData.Add(0x09);
                    touData.Add(0x04);
                    touData.Add(Convert.ToByte(dayProfileGrids[i].Rows[j].Cells[COLSTARTHOUR].Value));      //   Slot Start Hour
                    touData.Add(Convert.ToByte(dayProfileGrids[i].Rows[j].Cells[COLSTARTMIN].Value));       //   Slot Start min
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x09);
                    touData.Add(0x06);
                    touData.Add(0x00);
                    touData.Add(0x00);
                    touData.Add(0x0A);
                    touData.Add(0x00);
                    touData.Add(0x64);
                    touData.Add(0xFF);
                    touData.Add(0x12);
                    touData.Add(0x00);
                    switch (Convert.ToString(dayProfileGrids[i].Rows[j].Cells[COLTARIFF].Value))
                    {
                        case "T1":
                            touData.Add(0x01);
                            break;
                        case "T2":
                            touData.Add(0x02);
                            break;
                        case "T3":
                            touData.Add(0x03);
                            break;
                        case "T4":
                            touData.Add(0x04);
                            break;
                        case "T5":
                            touData.Add(0x05);
                            break;
                        case "T6":
                            touData.Add(0x06);
                            break;
                        case "T7":
                            touData.Add(0x07);
                            break;
                        case "T8":
                            touData.Add(0x08);
                            break;
                        default:
                            touData.Add(0x00);
                            break;
                    }
                }
            }
            if (meterModel == NamePlateConstants.RubyE250Value)
            {
                if (UtilityDetails.ShowTwoTOU)
                {
                    for (byte i = tempDayProfileCount; i <= 5; i++)
                    {
                        touData.Add(0x02);
                        touData.Add(0x02);
                        touData.Add(0x11);
                        touData.Add(0x00);             //Day Id 
                        touData.Add(0x01);
                        touData.Add(0x0A);

                        for (byte j = 0; j < 10; j++)
                        {
                            touData.Add(0x02);
                            touData.Add(0x03);
                            touData.Add(0x09);
                            touData.Add(0x04);
                            touData.Add(0x00);      //   Slot Start Hour
                            touData.Add(0x00);       //   Slot Start min
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x09);
                            touData.Add(0x06);
                            touData.Add(0x00);
                            touData.Add(0x00);
                            touData.Add(0x0A);
                            touData.Add(0x00);
                            touData.Add(0x64);
                            touData.Add(0xFF);
                            touData.Add(0x12);
                            touData.Add(0x00);
                            touData.Add(0x00);
                        }
                        tempDayProfileCount++;
                    }

                    touData.Add(0x02);
                    touData.Add(0x02);
                    touData.Add(0x11);
                    touData.Add((byte)(tempDayProfileCount + 1));             //Day Id 
                    touData.Add(0x01);
                    touData.Add(0x0A);

                    for (byte j = 0; j < 10; j++)
                    {
                        touData.Add(0x02);
                        touData.Add(0x03);
                        touData.Add(0x09);
                        touData.Add(0x04);
                        touData.Add(Convert.ToByte(dayProfileGrids[1].Rows[j].Cells[COLSTARTHOUR].Value));      //   Slot Start Hour
                        touData.Add(Convert.ToByte(dayProfileGrids[1].Rows[j].Cells[COLSTARTMIN].Value));       //   Slot Start min
                        touData.Add(0x00);
                        touData.Add(0x00);
                        touData.Add(0x09);
                        touData.Add(0x06);
                        touData.Add(0x00);
                        touData.Add(0x00);
                        touData.Add(0x0A);
                        touData.Add(0x00);
                        touData.Add(0x64);
                        touData.Add(0xFF);
                        touData.Add(0x12);
                        touData.Add(0x00);
                        switch (Convert.ToString(dayProfileGrids[1].Rows[j].Cells[COLTARIFF].Value))
                        {
                            case "T1":
                                touData.Add(0x01);
                                break;
                            case "T2":
                                touData.Add(0x02);
                                break;
                            case "T3":
                                touData.Add(0x03);
                                break;
                            case "T4":
                                touData.Add(0x04);
                                break;
                            case "T5":
                                touData.Add(0x05);
                                break;
                            case "T6":
                                touData.Add(0x06);
                                break;
                            case "T7":
                                touData.Add(0x07);
                                break;
                            case "T8":
                                touData.Add(0x08);
                                break;
                            default:
                                touData.Add(0x00);
                                break;
                        }
                    }
                    //dayProfileCount++;

                }

                 for (byte i = tempDayProfileCount; i <= TOUConstants.MaxDay; i++)
                 {
                     touData.Add(0x02);
                     touData.Add(0x02);
                     touData.Add(0x11);
                     touData.Add(0x00);             //Day Id 
                     touData.Add(0x01);
                     touData.Add(0x0A);

                     for (byte j = 0; j < 10; j++)
                     {
                         touData.Add(0x02);
                         touData.Add(0x03);
                         touData.Add(0x09);
                         touData.Add(0x04);
                         touData.Add(0x00);      //   Slot Start Hour
                         touData.Add(0x00);       //   Slot Start min
                         touData.Add(0x00);
                         touData.Add(0x00);
                         touData.Add(0x09);
                         touData.Add(0x06);
                         touData.Add(0x00);
                         touData.Add(0x00);
                         touData.Add(0x0A);
                         touData.Add(0x00);
                         touData.Add(0x64);
                         touData.Add(0xFF);
                         touData.Add(0x12);
                         touData.Add(0x00);
                         touData.Add(0x00);
                     }
                 }
            }

            return touData.ToArray();
        }

        /// <summary>
        /// Used to get activation date buffer data 
        /// </summary>
        /// <param name="meterModel"></param>
        /// <returns></returns>
        private byte[] GetActivationDateBuffer(int meterModel)
        {
            touData = new List<byte>();
            touData.Add(0x09);
            touData.Add(0x0C);
            touData.Add(Convert.ToByte((activationDate.Year & 0xFF00) >> 8));
            touData.Add(Convert.ToByte(activationDate.Year & 0x00FF));
            touData.Add(Convert.ToByte(activationDate.Month)); //month
            touData.Add(Convert.ToByte(activationDate.Day));  //day of month   
            touData.Add(0xFF);  //day of week
            touData.Add(0xFF);  //hh
            touData.Add(0xFF);  //mm
            touData.Add(0xFF);  //ss
            touData.Add(0xFF);
            touData.Add(0x80);
            touData.Add(0x00);
            touData.Add(0x00);
            return touData.ToArray();

        }
       
    }
}
