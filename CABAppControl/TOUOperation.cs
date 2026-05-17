using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.UI.Controls;

namespace CABAppControl
{
    public partial class TOUOperation : UserControl
    {
        public bool isValidTOU;

        public bool buttonclicked { get; set; }

        public TOUOperation()
        {
            isValidTOU = true;
            InitializeComponent();
        }


        private DataGridView[] GetSeasonGridCollection()
        {
            DataGridView[] seasonGrids = new DataGridView[] 
            {
                gridS1Day1, gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6,
                gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, 
                gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6,
                gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6
            };
            return seasonGrids;
        }

        private DataGridView[] GetHolidayGridCollection()
        {
            DataGridView[] holidayGrids = new DataGridView[] 
            {
                gridHoliday1,gridHoliday2, gridHoliday3, gridHoliday4, gridHoliday5,
                gridHoliday6, gridHoliday7, gridHoliday8, gridHoliday9, gridHoliday10
            };
            return holidayGrids;
        }

        private DataGridView[] GetAssignmentGridCollection()
        {
            DataGridView[] dayAssignmentGrids = new DataGridView[] 
            {
                gridAssignmentS1, gridAssignmentS2, gridAssignmentS3, gridAssignmentS4
            };
            return dayAssignmentGrids;
        }

        private DateTimePicker[] GetActivationDateCollection()
        {
            DateTimePicker[] holidayActivationDates = new DateTimePicker[]
            {
                dtPicAcDate1, dtPicAcDate2, dtPicAcDate3, dtPicAcDate4, dtPicAcDate5,
                dtPicAcDate6, dtPicAcDate7, dtPicAcDate8, dtPicAcDate9, dtPicAcDate10 
            };
            return holidayActivationDates;
        }

        private void ResetSeasonGrids()
        {
            int rowCount = 0;
            foreach (DataGridView seasonGrid in GetSeasonGridCollection())
            {
                rowCount = 0;
                if (seasonGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        seasonGrid.Rows.Add();
                        seasonGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }

                if (buttonclicked == false)
                {
                    foreach (DataGridViewRow row in seasonGrid.Rows)
                    {
                        row.Cells["Start Hour"].Value = "00";
                        row.Cells["Start Minute"].Value = "00";
                        row.Cells["Rate"].Value = "00";
                    }
                }
            }
        }

        private void ResetHolidayGrids()
        {
            int rowCount = 0;
            foreach (DataGridView holidayGrid in GetHolidayGridCollection())
            {
                rowCount = 0;
                if (holidayGrid.Rows.Count == 0)
                {
                    while (rowCount < 10)
                    {
                        holidayGrid.Rows.Add();
                        holidayGrid.Rows[rowCount].Cells["SNo."].Value = (++rowCount).ToString();
                    }
                }
                foreach (DataGridViewRow row in holidayGrid.Rows)
                {
                    row.Cells["Start Hour"].Value = "00";
                    row.Cells["Start Minute"].Value = "00";
                    row.Cells["Rate"].Value = "00";
                }
            }

            foreach (DateTimePicker dtp in GetActivationDateCollection())
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
                dtp.Value = System.DateTime.Now;
            }
        }

        private void ResetDayAssignmentGrids()
        {
            foreach (DataGridView assignmentGrid in GetAssignmentGridCollection())
            {
                if (assignmentGrid.Rows.Count == 0)
                {
                    assignmentGrid.Rows.Add(7);
                    assignmentGrid.Rows[0].Cells[0].Value = "Sunday";
                    assignmentGrid.Rows[1].Cells[0].Value = "Monday";
                    assignmentGrid.Rows[2].Cells[0].Value = "Tuesday";
                    assignmentGrid.Rows[3].Cells[0].Value = "Wednesday";
                    assignmentGrid.Rows[4].Cells[0].Value = "Thursday";
                    assignmentGrid.Rows[5].Cells[0].Value = "Friday";
                    assignmentGrid.Rows[6].Cells[0].Value = "Saturday";
                }
                foreach (DataGridViewRow row in assignmentGrid.Rows)
                {
                    row.Cells[1].Value = "Day Table 1";
                    row.Cells[1].Value = "Day Table 1";
                }
            }
        }

        private void ResetFutureActivationGrid()
        {
            double dayIndex = 0;
            dtPickerFutureActivationDate.Format = DateTimePickerFormat.Custom;
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";//ConfigInfo.DateFormat();
            dtPickerFutureActivationDate.Value = System.DateTime.Now.AddDays(++dayIndex);

            if (gridActivation.Rows.Count == 0)
                gridActivation.Rows.Add(4);
            int rIndex = 0;
            int startDay = 1;
            foreach (DataGridViewRow row in gridActivation.Rows)
            {
                string sDay = startDay.ToString();
                if (sDay.Length < 2) { sDay = "0" + sDay; }
                DateTime dt = new DateTime(DateTime.Now.Year, 1, Int32.Parse(sDay));
                row.Cells[0].Value = dt;// Convert.ToDateTime(sDay + "/01/" + DateTime.Now.Year.ToString());
                startDay++;
                //row.Cells[0].Value = System.DateTime.Now.AddDays(++dayIndex);
                row.Cells[1].Value = "1";
            }
        }
        private void ResetAllGrids()
        {
            ResetSeasonGrids();
            ResetHolidayGrids();
            ResetDayAssignmentGrids();
            ResetFutureActivationGrid();
        }
        private void btnResetAll_Click(object sender, EventArgs e)
        {
            buttonclicked = false;
            ResetAllGrids();
        }

        private void UploadSeasonGrids()
        {
            DataGridView[] seasonGrids = new DataGridView[] { gridS1Day2, gridS1Day3, gridS1Day4, gridS1Day5, gridS1Day6, gridS2Day1, gridS2Day2, gridS2Day3, gridS2Day4, gridS2Day5, gridS2Day6, gridS3Day1, gridS3Day2, gridS3Day3, gridS3Day4, gridS3Day5, gridS3Day6, gridS4Day1, gridS4Day2, gridS4Day3, gridS4Day4, gridS4Day5, gridS4Day6 };
            foreach (DataGridView sGrid in seasonGrids)
            {
                for (int rowIndex = 0; rowIndex < gridS1Day1.Rows.Count; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < gridS1Day1.Columns.Count; colIndex++)
                    {
                        sGrid[colIndex, rowIndex].Value = gridS1Day1[colIndex, rowIndex].Value.ToString();
                    }
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            UploadSeasonGrids();
        }

        private void GridCellClick(DataGridView dataGrid)
        {
            if (dataGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                int colIndex = dataGrid.CurrentCell.ColumnIndex;
                int rowIndex = dataGrid.CurrentCell.RowIndex;
                dataGrid.Rows[rowIndex].Cells[colIndex] = comboCell;

                if (dataGrid.CurrentCell.ColumnIndex == 1)
                {
                    comboCell.Items.Add("T1");
                    comboCell.Items.Add("T2");
                    comboCell.Items.Add("T3");
                    comboCell.Items.Add("T4");
                    comboCell.Items.Add("T5");
                    comboCell.Items.Add("T6");
                    comboCell.Items.Add("T7");
                    comboCell.Items.Add("T8");
                    comboCell.Items.Add("00");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 2)
                {
                    comboCell.Items.Add("00");
                    comboCell.Items.Add("01");
                    comboCell.Items.Add("02");
                    comboCell.Items.Add("03");
                    comboCell.Items.Add("04");
                    comboCell.Items.Add("05");
                    comboCell.Items.Add("06");
                    comboCell.Items.Add("07");
                    comboCell.Items.Add("08");
                    comboCell.Items.Add("09");
                    comboCell.Items.Add("10");
                    comboCell.Items.Add("11");
                    comboCell.Items.Add("12");
                    comboCell.Items.Add("13");
                    comboCell.Items.Add("14");
                    comboCell.Items.Add("15");
                    comboCell.Items.Add("16");
                    comboCell.Items.Add("17");
                    comboCell.Items.Add("18");
                    comboCell.Items.Add("19");
                    comboCell.Items.Add("20");
                    comboCell.Items.Add("21");
                    comboCell.Items.Add("22");
                    comboCell.Items.Add("23");
                }
                else if (dataGrid.CurrentCell.ColumnIndex == 3)
                {
                    comboCell.Items.Add("00");
                    //comboCell.Items.Add("15");
                    comboCell.Items.Add("30");
                    //comboCell.Items.Add("45");
                }
            }
            int rIndex = dataGrid.CurrentCell.RowIndex;
            int count = 0;
            if (rIndex != 0)
            {

                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "23" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "30")
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = true;
                        dataGrid.Rows[count].Cells[2].ReadOnly = true;
                        dataGrid.Rows[count].Cells[3].ReadOnly = true;
                    }
                    //this.StatusMessage = "No more entries allowed as the day is complete";
                    MessageBox.Show("No more entries allowed as the day is complete", "BCS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    isValidTOU = true;
                    return;
                }
                else
                {
                    for (count = rIndex; count <= 9; count++)
                    {
                        dataGrid.Rows[count].Cells[1].ReadOnly = false;
                        dataGrid.Rows[count].Cells[2].ReadOnly = false;
                        dataGrid.Rows[count].Cells[3].ReadOnly = false;
                    }
                    //this.StatusMessage = " ";
                    isValidTOU = true;
                }

                int grdRowIndex = 0;
                for (grdRowIndex = 1; grdRowIndex < 9; grdRowIndex++) //changed on 11/01/2011 grdRowIndex <= 9 changed to grdRowIndex < 9 
                {
                    if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[grdRowIndex].Cells[2].Value) == "00")
                        {
                            //below condition commented on 19 sep 2011
                            if ((Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) != "00") || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == 30))// || (Convert.ToString(dataGrid.Rows[grdRowIndex - 1].Cells[2].Value) == "00" && Convert.ToInt16(dataGrid.Rows[grdRowIndex - 1].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[grdRowIndex].Cells[3].Value)))
                            {
                                dataGrid.Rows[grdRowIndex].Cells[3].ReadOnly = true;
                                do
                                {
                                    dataGrid.Rows[grdRowIndex + 1].Cells[1].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[2].ReadOnly = true;
                                    dataGrid.Rows[grdRowIndex + 1].Cells[3].ReadOnly = true;
                                    grdRowIndex++;
                                } while (grdRowIndex != 9);
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 7; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true; // this line added on  11/01/2011
                            do
                            {
                                dataGrid.Rows[rIndex + 2].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 2].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 8);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                for (rIndex = 1; rIndex <= 8; rIndex++)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                        {
                            dataGrid.Rows[rIndex - 1].Cells[3].ReadOnly = true;
                            do
                            {
                                dataGrid.Rows[rIndex + 1].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex + 1].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 9);
                            isValidTOU = false;
                            return;
                        }
                        else isValidTOU = true;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex > 1)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) && Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[3].Value) == Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value))
                    {
                        string val = Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value);
                        string val1 = Convert.ToString(dataGrid.Rows[rIndex - 2].Cells[1].Value);
                        if (val1 != "00" && val != "00")
                        {
                            do
                            {
                                dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                rIndex++;
                            } while (rIndex != 10);
                            isValidTOU = false;
                            return;
                        }
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[1].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    if (rIndex != 1 && (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "00"))
                    {
                        do
                        {
                            dataGrid.Rows[rIndex].Cells[1].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                            dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                            rIndex++;
                        } while (rIndex != 10);
                        isValidTOU = false;
                        return;
                    }
                    else
                    {
                        dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = true;
                    }
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) == "00")
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                    return;
                }
                else
                {
                    dataGrid.Rows[rIndex].Cells[2].ReadOnly = false;
                    dataGrid.Rows[rIndex].Cells[3].ReadOnly = false;
                    isValidTOU = true;
                }

                rIndex = dataGrid.CurrentCell.RowIndex;
                if (Convert.ToString(dataGrid.Rows[rIndex].Cells[1].Value) != "00" && Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == "00")
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) != "00" || (Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[2].Value) == "00" && Convert.ToString(dataGrid.Rows[rIndex - 1].Cells[3].Value) == "30"))// && Convert.ToString(dataGrid.Rows[rIndex].Cells[3].Value) == "00")//added on june 10
                    {
                        dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                        isValidTOU = false;
                        return;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 9)
                {
                    if (Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[1].Value) != "00")
                    {
                        if (Convert.ToString(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToString(dataGrid.Rows[rIndex + 1].Cells[2].Value))
                        {
                            if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) >= Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value))
                            {
                                dataGrid.Rows[rIndex].Cells[2].ReadOnly = true;
                                dataGrid.Rows[rIndex].Cells[3].ReadOnly = true;
                                isValidTOU = false;
                                return;
                            }
                        }
                    }
                    if (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex + 1].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }
                rIndex = dataGrid.CurrentCell.RowIndex;
                if (rIndex != 0)
                {
                    if (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[2].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[2].Value) && (Convert.ToInt16(dataGrid.Rows[rIndex - 1].Cells[3].Value) == Convert.ToInt16(dataGrid.Rows[rIndex].Cells[3].Value)))
                    {
                        isValidTOU = false;
                    }
                }
            }
        }


        private bool validateGridCell(DataGridView dtView, object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "T1") && (e.FormattedValue.ToString() != "T2") && (e.FormattedValue.ToString() != "T3") && (e.FormattedValue.ToString() != "T4") && (e.FormattedValue.ToString() != "T5") && (e.FormattedValue.ToString() != "T6") && (e.FormattedValue.ToString() != "T7") && (e.FormattedValue.ToString() != "T8"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }
                    }
                }
                if (e.ColumnIndex == 2)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else isValidTOU = true;
                        if (e.FormattedValue.ToString() != "" && (Convert.ToInt16(e.FormattedValue.ToString()) > 23))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }
                        if (e.RowIndex != 0)
                        {
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "30")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }

                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                        }
                        else isValidTOU = true;

                        if (e.RowIndex != 0 && e.RowIndex != 9)
                        {

                            if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value.ToString() != "00")
                            {
                                if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;


                                if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                {
                                    if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value) || Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex + 1].Value))
                                    {
                                        dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                        e.Cancel = true;
                                        isValidTOU = false;
                                    }
                                    else { isValidTOU = true; }
                                }
                            }

                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else isValidTOU = true;

                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value.ToString() == "00" && dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value.ToString() == "30")
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = false;
                                    return false;
                                }
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value) <= Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex + 1].Value))
                                {
                                    int currIndex = e.RowIndex;
                                    int rIndex = e.RowIndex + 1;
                                    if (rIndex < 10)
                                    {
                                        do
                                        {
                                            dtView.Rows[rIndex].Cells[1].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[2].ReadOnly = true;
                                            dtView.Rows[rIndex].Cells[3].ReadOnly = true;
                                            rIndex++;
                                        } while (rIndex != 10);
                                        isValidTOU = false;
                                        return false;
                                    }
                                }
                                else { isValidTOU = true; }
                            }
                        }
                        if (e.RowIndex == 9)
                        {
                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[2].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[2].Value) && Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[3].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[3].Value))
                            {
                                isValidTOU = false;
                            }
                        }
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    if (dtView[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
                    {
                        if (e.RowIndex == 0)
                        {
                            if (e.FormattedValue.ToString() != "00")
                            {
                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                                return false;
                            }
                            else isValidTOU = true;
                        }
                        if (e.FormattedValue.ToString() == "")
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        if (e.FormattedValue.ToString() != "" && (e.FormattedValue.ToString() != "00") && (e.FormattedValue.ToString() != "15") && (e.FormattedValue.ToString() != "30") && (e.FormattedValue.ToString() != "45"))
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            dtView.Rows[e.RowIndex].ErrorText = "";
                            isValidTOU = true;
                        }

                        if (e.FormattedValue.ToString() != "" && e.RowIndex > 0)
                        {
                            int index = e.RowIndex;
                            if (index != 9)
                            {
                                while (index != 10)
                                {
                                    if (dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 2].Value.ToString() != "00")
                                    {
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                        {
                                            if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                            {
                                                dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                e.Cancel = true;
                                                isValidTOU = false;
                                            }
                                            else isValidTOU = true;
                                        }
                                        else
                                        {
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) > Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex + 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                            if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                                            {
                                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                                {
                                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                                    e.Cancel = true;
                                                    isValidTOU = false;
                                                }
                                                else isValidTOU = true;
                                            }
                                        }
                                    }
                                    index++;
                                }
                            }
                            if (Convert.ToInt16(e.FormattedValue.ToString()) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                if (Convert.ToInt16(dtView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value) == Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex - 1].Value))
                                {
                                    dtView.Rows[e.RowIndex].ErrorText = "Invalid";
                                    e.Cancel = true;
                                    isValidTOU = false;
                                }
                                else isValidTOU = true;
                            }
                            else if (Convert.ToInt16(e.FormattedValue.ToString()) < Convert.ToInt16(dtView.Rows[e.RowIndex - 1].Cells[e.ColumnIndex].Value))
                            {
                                int rIndex = e.RowIndex + 1;
                                do
                                {
                                    dtView.Rows[rIndex].Cells[0].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[1].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[2].ReadOnly = false;
                                    dtView.Rows[rIndex].Cells[3].ReadOnly = false;
                                    rIndex++;
                                } while (rIndex != 10);
                                rIndex--;
                                //this.StatusMessage = "";
                                isValidTOU = false;
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                dtView.Rows[e.RowIndex].ErrorText = "Invalid Value ";
                e.Cancel = true;
                isValidTOU = false;
                return false;
            }
        }


        private void gridS1Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day1);
        }

        private void gridS1Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day2);
        }

        private void gridS1Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day3);
        }

        private void gridS1Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day4);
        }

        private void gridS1Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day5);
        }

        private void gridS1Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS1Day6);
        }

        private void gridS2Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day1);
        }

        private void gridS2Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day2);
        }

        private void gridS2Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day3);
        }

        private void gridS2Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day4);
        }

        private void gridS2Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day5);
        }

        private void gridS2Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS2Day6);
        }

        private void gridS3Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day1);
        }

        private void gridS3Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day2);
        }

        private void gridS3Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day3);
        }

        private void gridS3Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day4);
        }

        private void gridS3Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day5);
        }

        private void gridS3Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS3Day6);
        }

        private void gridS4Day1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day1);
        }

        private void gridS4Day2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day2);
        }

        private void gridS4Day3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day3);
        }

        private void gridS4Day4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day4);
        }

        private void gridS4Day5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day5);
        }

        private void gridS4Day6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridS4Day6);
        }

        private void gridHoliday1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday1);
        }

        private void gridHoliday2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday2);
        }

        private void gridHoliday3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday3);
        }

        private void gridHoliday4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday4);
        }

        private void gridHoliday5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday5);
        }

        private void gridHoliday6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday6);
        }

        private void gridHoliday7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday7);
        }

        private void gridHoliday8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday8);
        }

        private void gridHoliday9_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday9);
        }

        private void gridHoliday10_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridCellClick(gridHoliday10);
        }

        private void gridS1Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day1, sender, e);
        }

        private void gridS1Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day2, sender, e);
        }

        private void gridS1Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day3, sender, e);
        }

        private void gridS1Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day4, sender, e);
        }

        private void gridS1Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day5, sender, e);
        }

        private void gridS1Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS1Day6, sender, e);
        }

        private void gridS2Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day1, sender, e);
        }

        private void gridS2Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day2, sender, e);
        }

        private void gridS2Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day3, sender, e);
        }

        private void gridS2Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day4, sender, e);
        }

        private void gridS2Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day5, sender, e);
        }

        private void gridS2Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS2Day6, sender, e);
        }

        private void gridS3Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day1, sender, e);
        }

        private void gridS3Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day2, sender, e);
        }

        private void gridS3Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day3, sender, e);
        }

        private void gridS3Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day4, sender, e);
        }

        private void gridS3Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day5, sender, e);
        }

        private void gridS3Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS3Day6, sender, e);
        }

        private void gridS4Day1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day1, sender, e);
        }

        private void gridS4Day2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day2, sender, e);
        }

        private void gridS4Day3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day3, sender, e);
        }

        private void gridS4Day4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day4, sender, e);
        }

        private void gridS4Day5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day5, sender, e);
        }

        private void gridS4Day6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridS4Day6, sender, e);
        }

        private void gridHoliday1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday1, sender, e);
        }

        private void gridHoliday2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday2, sender, e);
        }

        private void gridHoliday3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday3, sender, e);
        }

        private void gridHoliday4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday4, sender, e);
        }

        private void gridHoliday5_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday5, sender, e);
        }

        private void gridHoliday6_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday6, sender, e);
        }

        private void gridHoliday7_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday7, sender, e);
        }

        private void gridHoliday8_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday8, sender, e);
        }

        private void gridHoliday9_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday9, sender, e);
        }

        private void gridHoliday10_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            validateGridCell(gridHoliday10, sender, e);
        }

        private void GridAssignValidate(DataGridView AssignGrid, object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (AssignGrid[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string gridVal = e.FormattedValue.ToString();
                        if (gridVal == "")
                        {
                            AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            if (gridVal != "Day Table 1" && gridVal != "Day Table 2" && gridVal != "Day Table 3" && gridVal != "Day Table 4" && gridVal != "Day Table 5" && gridVal != "Day Table 6")
                            {
                                AssignGrid.Rows[e.RowIndex].ErrorText = "Invalid";
                                e.Cancel = true;
                                isValidTOU = false;
                            }
                            else
                            {
                                isValidTOU = true;
                                AssignGrid.Rows[e.RowIndex].ErrorText = "";
                            }
                        }
                    }
                }
            }
        }


        private void GridAssignClick(DataGridView AssignGrid)
        {
            if (AssignGrid.CurrentCell.Style.ForeColor == Color.Red)
            {
                if (AssignGrid.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell dtComboCell = new DataGridViewComboBoxCell();
                    dtComboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    dtComboCell.Items.Add("Day Table 1");
                    dtComboCell.Items.Add("Day Table 2");
                    dtComboCell.Items.Add("Day Table 3");
                    dtComboCell.Items.Add("Day Table 4");
                    dtComboCell.Items.Add("Day Table 5");
                    dtComboCell.Items.Add("Day Table 6");
                    int rIndex = AssignGrid.CurrentCell.RowIndex;
                    int cIndex = AssignGrid.CurrentCell.ColumnIndex;
                    AssignGrid.Rows[rIndex].Cells[cIndex] = dtComboCell;
                }
            }
        }

        private void gridAssignmentS1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS1);
        }

        private void gridAssignmentS2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS2);
        }

        private void gridAssignmentS3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS3);
        }

        private void gridAssignmentS4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAssignClick(gridAssignmentS4);
        }

        private void gridAssignmentS1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS1, sender, e);
        }

        private void gridAssignmentS2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS2, sender, e);
        }

        private void gridAssignmentS3_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS3, sender, e);
        }

        private void gridAssignmentS4_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAssignValidate(gridAssignmentS4, sender, e);
        }

        private void GridAcCellClick()
        {
            if (gridActivation.CurrentCell.Style.ForeColor == Color.Red)
            {
                gridActivation.CurrentCell.Style.ForeColor = Color.Black;
                if (gridActivation.CurrentCell.ColumnIndex == 1)
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                    comboCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    comboCell.Items.Add("1");
                    comboCell.Items.Add("2");
                    comboCell.Items.Add("3");
                    comboCell.Items.Add("4");
                    int rIndex = gridActivation.CurrentCell.RowIndex;
                    gridActivation.Rows[rIndex].Cells[1] = comboCell;
                }
            }
        }

        private void gridActivation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridAcCellClick();
        }

        private void GridAcCellValidating(DataGridView gridActivation, object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gridActivation[e.ColumnIndex, e.RowIndex].IsInEditMode == true)
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gridActivation.Rows[e.RowIndex].Cells[0].Value.ToString() != "")
                        {
                            DateTime dt = new DateTime();
                            CalendarEditingControl ctl = gridActivation.EditingControl as CalendarEditingControl;
                            if (ctl != null)
                            {
                                bool isValid = DateTime.TryParse(ctl.Value.ToString(), out dt);
                                if (!isValid)
                                {
                                    gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                                    isValidTOU = false;
                                }
                                else
                                {
                                    isValidTOU = true;
                                    gridActivation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = ctl.Value;
                                    gridActivation.Rows[e.RowIndex].ErrorText = "";
                                }
                            }
                        }
                        else
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                    }
                    else if (e.ColumnIndex == 1)
                    {
                        if (Convert.ToInt16(e.FormattedValue.ToString()) < 1 || (Convert.ToInt16(e.FormattedValue.ToString()) > 4))
                        {
                            gridActivation.Rows[e.RowIndex].ErrorText = "Invalid";
                            e.Cancel = true;
                            isValidTOU = false;
                        }
                        else
                        {
                            isValidTOU = true;
                            gridActivation.Rows[e.RowIndex].ErrorText = "";
                        }
                    }
                }
            }
        }

        private void gridActivation_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            GridAcCellValidating(gridActivation, sender, e);
        }

        private DataGridViewComboBoxColumn GetSNo()
        {
            DataGridViewComboBoxColumn colSNo = new DataGridViewComboBoxColumn();
            colSNo.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colSNo.Name = "SNo.";
            colSNo.HeaderText = "SNo.";
            colSNo.Items.Add("1");
            colSNo.Items.Add("2");
            colSNo.Items.Add("3");
            colSNo.Items.Add("4");
            colSNo.Items.Add("5");
            colSNo.Items.Add("6");
            colSNo.Items.Add("7");
            colSNo.Items.Add("8");
            colSNo.Items.Add("9");
            colSNo.Items.Add("10");
            return colSNo;
        }

        private DataGridViewComboBoxColumn GetRates()
        {
            DataGridViewComboBoxColumn colRate = new DataGridViewComboBoxColumn();
            colRate.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colRate.Name = "Rate";
            colRate.HeaderText = "Rate";
            colRate.Items.Add("T1");
            colRate.Items.Add("T2");
            colRate.Items.Add("T3");
            colRate.Items.Add("T4");
            colRate.Items.Add("T5");
            colRate.Items.Add("T6");
            colRate.Items.Add("T7");
            colRate.Items.Add("T8");
            colRate.Items.Add("00");
            return colRate;
        }

        private DataGridViewComboBoxColumn GetStartHour()
        {
            DataGridViewComboBoxColumn colStartHour = new DataGridViewComboBoxColumn();
            colStartHour.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartHour.Name = "Start Hour";
            colStartHour.HeaderText = "Start Hour";
            colStartHour.Items.Add("00");
            colStartHour.Items.Add("01");
            colStartHour.Items.Add("02");
            colStartHour.Items.Add("03");
            colStartHour.Items.Add("04");
            colStartHour.Items.Add("05");
            colStartHour.Items.Add("06");
            colStartHour.Items.Add("07");
            colStartHour.Items.Add("08");
            colStartHour.Items.Add("09");
            colStartHour.Items.Add("10");
            colStartHour.Items.Add("11");
            colStartHour.Items.Add("12");
            colStartHour.Items.Add("13");
            colStartHour.Items.Add("14");
            colStartHour.Items.Add("15");
            colStartHour.Items.Add("16");
            colStartHour.Items.Add("17");
            colStartHour.Items.Add("18");
            colStartHour.Items.Add("19");
            colStartHour.Items.Add("20");
            colStartHour.Items.Add("21");
            colStartHour.Items.Add("22");
            colStartHour.Items.Add("23");
            return colStartHour;
        }

        private DataGridViewComboBoxColumn GetStartMinute()
        {
            DataGridViewComboBoxColumn colStartMinute = new DataGridViewComboBoxColumn();
            colStartMinute.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            colStartMinute.Name = "Start Minute";
            colStartMinute.HeaderText = "Start Minute";
            colStartMinute.Items.Add("00");
           // colStartMinute.Items.Add("15");
            colStartMinute.Items.Add("30");
           // colStartMinute.Items.Add("45");
            return colStartMinute;
        }

        private void SetTOUGrids()
        {
            DataGridView[] seasonGrids = GetSeasonGridCollection();
            DataGridView[] holidayGrids = GetHolidayGridCollection();

            foreach (DataGridView seasonGrid in seasonGrids)
            {
                if (seasonGrid.ColumnCount == 0)
                {
                    seasonGrid.Columns.Add(GetSNo());
                    seasonGrid.Columns.Add(GetRates());
                    seasonGrid.Columns.Add(GetStartHour());
                    seasonGrid.Columns.Add(GetStartMinute());
                    seasonGrid.Columns[0].ReadOnly = true;
                }
            }
            foreach (DataGridView holidayGrid in holidayGrids)
            {
                if (holidayGrid.ColumnCount == 0)
                {
                    holidayGrid.Columns.Add(GetSNo());
                    holidayGrid.Columns.Add(GetRates());
                    holidayGrid.Columns.Add(GetStartHour());
                    holidayGrid.Columns.Add(GetStartMinute());
                    holidayGrid.Columns[0].ReadOnly = true;
                }
            }
        }

        private void TOUOperation_Load(object sender, EventArgs e)
        {
            dtPickerFutureActivationDate.CustomFormat = "dd/MM/yyyy";
            SetTOUGrids();
            ResetAllGrids();
        }

    }
}
