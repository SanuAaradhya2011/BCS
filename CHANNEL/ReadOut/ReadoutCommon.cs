using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CAB.Framework.Utility;
using CAB.Framework;

namespace CAB.Channel.ReadOut
{
    public enum DialogType
    {
        Common,
        TOU
    }
    public class ReadoutCommon
    {
        public static string GetFileName()
        {
            DateTime dt = System.DateTime.Now;
            return string.Concat(dt.Day.ToString("00") + dt.Month.ToString("00") + dt.Year.ToString("0000") + "_" + dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00"));
        }

        public static DialogResult ReadoutMessageBox(ref string filename, DialogType dialogType)
        {
            Form form = new Form();
            Label labelLoc = new Label();
            Label labelPath = new Label();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            labelLoc.Text = "Location";
            if (dialogType == DialogType.Common)
                labelPath.Text = ConfigInfo.GetLocation();
            else
                labelPath.Text = ConfigInfo.GetTOULocation();
            //if (ConfigInfo.GetApplicationType().Equals(ApplicationType.DLMS_LTCT_650) || ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
            //    form.Text = "E-250 BCS";
            //else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.DLMS_RUBY_250) || ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
            //    form.Text = "E-250 BCS";
            //else
            form.Text = "BCS";
            string namingConvention = ConfigInfo.FileNamingConvention();
            //string fname = System.DateTime.Now.ToString("ddMMyyyyHHmmss");
            //filename = fname.Substring(0, 8) + "_" + fname.Substring(8, 6);
            if (namingConvention.Trim().Equals("Default"))
            {
                label.Text = "File Name";
                textBox.Enabled = false;
            }
            else if (namingConvention.Trim().Equals("Default+MeterID"))
            {
                label.Text = "File Name";
                textBox.Enabled = false;
            }
            else if (namingConvention.Trim().Equals("Default+System"))
            {
                label.Text = "File Name";
                filename = string.Concat(System.Net.Dns.GetHostName(), "_", filename);
                textBox.Enabled = false;
            }
            else if (namingConvention.Trim().Equals("Custom"))
            {
                label.Text = "Enter File Name";
                filename = string.Empty;
                textBox.Enabled = true;
            }
            textBox.Text = filename;
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            labelLoc.SetBounds(15, 11, 62, 17);
            labelPath.SetBounds(83, 11, 520, 17);
            label.SetBounds(15, 65, 79, 17);
            Panel panel = new Panel();
            panel.BackColor = SystemColors.ActiveCaptionText;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(0, 42);
            panel.Size = new Size(610, 3);
            textBox.SetBounds(18, 98, 571, 22);
            buttonOk.SetBounds(386, 158, 91, 35);
            buttonCancel.SetBounds(498, 158, 91, 35);
            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.ClientSize = new Size(604, 207);
            form.Controls.AddRange(new Control[] { labelLoc, labelPath, panel, label, textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            bool Flag = false;
            DialogResult dialogResult;
            do
            {
                dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    Flag = true;
                    filename = textBox.Text.Trim();
                    if (string.IsNullOrEmpty(filename))
                    {
                        MessageBox.Show("File name can't be empty.", "E-250 BCS");
                        textBox.Focus();
                    }
                    else
                        Flag = false;
                }
                else
                    Flag = false;
            }
            while (Flag);
            return dialogResult;
        }

        public static bool ValidFileName(string filename)
        {
            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            return (filename.IndexOfAny(invalidFileChars) < 0);
        }

        public static bool CalculateBcc(string RecInpData, int endLen, string bccChar)
        {
            try
            {
                int Bcc = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytesBcc = encoding.GetBytes(RecInpData.Substring(0, 1));
                foreach (byte b in bytesBcc)
                    if (b == 2) { Bcc = 1; }

                long countbyt = 0;
                Byte[] bytes = encoding.GetBytes(RecInpData);
                foreach (byte b in bytes)
                {
                    if (countbyt <= endLen) Bcc = Bcc ^ b;
                    countbyt++;
                }
                bytes = encoding.GetBytes(bccChar);
                foreach (byte b in bytes)
                    if (Bcc != b) { return false; }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string CalculateFileBcc(string data)
        {
            long countbyt = 0;
            long bcc = 0;
            string checkSum = string.Empty;
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(data);
                foreach (byte b in bytes)
                {
                    if (countbyt <= bytes.Length) bcc = bcc ^ b;
                    countbyt++;
                }
                checkSum = Convert.ToChar(bcc).ToString();
            }
            catch (Exception)
            {
                checkSum = string.Empty;
            }
            return checkSum;
        }

        public static int GetBaudRate(string baudRateChar)
        {
            int baud = 0;
            switch (baudRateChar)
            {
                case "0":
                    baud = 300;
                    break;
               case "5":
                    baud = 9600;
                    break;
                case "6":
                    baud = 19200;
                    break;
                case "7":
                    baud = 38400;
                    break;
                default :
                    baud = 9600;
                    break;
            }
            return baud;
        }

        public static string DTMStringToHex(string GetStr)
        {
            int indecount = 0;
            string tempstr = "";
            char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            indecount = Int16.Parse(GetStr, System.Globalization.NumberStyles.HexNumber);
            tempstr = ((indecount / 10) + 30).ToString() + (indecount % 10 + 30).ToString();
            int num = 0;
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                num = 4;
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                num = 8;
            while (tempstr.Length < num)
            {
                tempstr = "30" + tempstr;
            }
            return tempstr;
        }

       
        public static string StrToHex(string GetStr)
        {
            int indecount = 0;
            int intmod = 0;
            string tempstr = string.Empty;
            char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            indecount = Convert.ToInt16(GetStr);

            intmod = (indecount % 16);
            if (intmod <= 9)
            {
                string strval = _hexChars[intmod].ToString();
                tempstr += ((indecount / 16) + 30).ToString() + (Convert.ToInt16(strval) + 30).ToString();
            }
            else
            {
                tempstr += ((indecount / 16) + 30).ToString() + "0" + (_hexChars[intmod]).ToString();

            }
            while (tempstr.Length < 8)
            {
                tempstr = "30" + tempstr;
            }
            return tempstr;
        }

        public static string SetPassword(string GetPass)
        {
            try
            {
                int passcnt = 0;
                string MeterPassword = string.Empty;
                while (passcnt < 8)
                {
                    if (passcnt < GetPass.Length) MeterPassword += (Convert.ToInt16(GetPass.Substring(passcnt, 1)) + 30).ToString();
                    else MeterPassword += "30";
                    passcnt++;
                }
                return MeterPassword;
            }
            catch
            {
                return ("3030303030303030");
            }
        }

        public static string ReturnBcc(string RecInpData)
        {
            try
            {
                char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
                int Bcc = 0;
                int countbyt = 0;
                string TempStr = string.Empty;
                while (countbyt < RecInpData.Length)
                {

                    TempStr += System.Convert.ToChar(System.Convert.ToUInt32(RecInpData.Substring(countbyt, 2), 16)).ToString(); ;
                    countbyt += 2;
                }
                countbyt = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(TempStr);
                foreach (byte b in bytes)
                {
                    if (countbyt <= RecInpData.Length) Bcc = Bcc ^ b;
                    countbyt++;
                }
                string retval = _hexChars[Bcc / 16].ToString() + _hexChars[Bcc % 16].ToString();

                return retval;
            }
            catch (Exception Ex)
            {
                return "False";
            }
        }

        private static string[] TamperRow()
        {
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                return TamperRow650();
            else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                return TamperRow250();
            else
                return null;
        }
        private static string[] TamperRow650()
        {
            string[] array = new string[37];
            array[0] = "R Phase Missing Potential";
            array[1] = "Y Phase Missing Potential";
            array[2] = "B Phase Missing Potential";
            array[3] = "R Phase Low/ Under Voltage";
            array[4] = "Y Phase Low/ Under Voltage";
            array[5] = "B Phase Low/ Under Voltage";
            array[6] = "R Phase High/ Over Voltage";
            array[7] = "Y Phase High/ Over Voltage";
            array[8] = "B Phase High/ Over Voltage";
            array[9] = "R Phase Current Reversal Tamper";
            array[10] = "Y Phase Current Reversal Tamper";
            array[11] = "B Phase Current Reversal Tamper";
            array[12] = "R Phase CT Open Tamper";
            array[13] = "Y Phase CT Open Tamper";
            array[14] = "B Phase CT Open Tamper";
            array[15] = "CT Bypass Tamper";
            array[16] = "Magnetic Influence Tamper";
            array[17] = "Neutral Disturbance Tamper";
            array[18] = "R Phase Voltage Imbalance";
            array[19] = "Y Phase Voltage Imbalance";
            array[20] = "B Phase Voltage Imbalance";
            array[21] = "R Phase Current Imbalance Tamper";
            array[22] = "Y Phase Current Imbalance Tamper";
            array[23] = "B Phase Current Imbalance Tamper";
            array[24] = "Current Phase Reversal Tamper";
            array[25] = "Voltage Phase Reversal Tamper";
            array[26] = "R Phase Low PF Tamper";
            array[27] = "Y Phase Low PF Tamper";
            array[28] = "B Phase Low PF Tamper";
            array[29] = "One Phase and Neutral Absent";
            array[30] = "Front Cover Opening Tamper";
            array[31] = "Terminal Cover Opening Tamper";
            array[32] = "R Phase Current Without Voltage Tamper";
            array[33] = "Y Phase Current Without Voltage Tamper";
            array[34] = "B Phase Current Without Voltage Tamper";
            array[35] = "Over Load Tamper";
            array[36] = "Low Load Tamper";
            return array;
        }
        private static string[] TamperRow250()
        {
            string[] array = new string[22];
            array[0] = "R Phase Missing Potential";
            array[1] = "Y Phase Missing Potential";
            array[2] = "B Phase Missing Potential";
            array[3] = "Voltage Phase Reversal Tamper";
            array[4] = "One Phase and Neutral Absent";
            array[5] = "Front Cover Opening Tamper";
            array[6] = "Terminal Cover Opening Tamper";
            array[7] = "Magnetic Influence Tamper";
            array[8] = "Neutral Disturbance Tamper";
            array[9] = "R Phase Current Reversal Tamper";
            array[10] = "Y Phase Current Reversal Tamper";
            array[11] = "B Phase Current Reversal Tamper";
            array[12] = "R Phase CT Open Tamper";
            array[13] = "Y Phase CT Open Tamper";
            array[14] = "B Phase CT Open Tamper";
            array[15] = "CT Bypass Tamper";
            array[16] = "R Phase Voltage Imbalance";
            array[17] = "Y Phase Voltage Imbalance";
            array[18] = "B Phase Voltage Imbalance";
            array[19] = "R Phase Current Imbalance Tamper";
            array[20] = "Y Phase Current Imbalance Tamper";
            array[21] = "B Phase Current Imbalance Tamper";
            return array;
        }

        private static string[] ProgrammingUpdatedRow()
        {
            string[] array = new string[19];
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                array = new string[22];
            array[0] = "Programming Updates";
            array[1] = "Time Stamp";
            array[2] = "Parameter 1";
            array[3] = "Parameter 2";
            array[4] = "Parameter 3";
            array[5] = "Parameter 4";
            array[6] = "Parameter 5";
            array[7] = "Parameter 6";
            array[8] = "Parameter 7";
            array[9] = "Parameter 8";
            array[10] = "Parameter 9";
            array[11] = "Parameter 10";
            array[12] = "Parameter 11";
            array[13] = "Parameter 12";
            array[14] = "Parameter 13";
            array[15] = "Parameter 14";
            array[16] = "Parameter 15";
            array[17] = "Parameter 16";
            array[18] = "Parameter 17";
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
            {
                array[19] = "Parameter 18";
                array[20] = "Parameter 19";
                array[21] = "Parameter 20";
            }
            return array;
        }

        private static string[] RTCUpdatedRow()
        {
            string[] array = new string[3];
            array[0] = "RTC Updates";
            array[1] = "Old Time Stamp";
            array[2] = "Updated Time Stamp";
            return array;
        }

        public static DataTable ConvertTamperData(string tampalert)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Tamper", typeof(System.String)));
            table.Columns.Add(new DataColumn("Status", typeof(System.String)));
            int counter = 0;
            tampalert = tampalert.Substring(1, tampalert.Length - 3);
            string revLim = string.Empty;
            char[] _hexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'F', 'B', 'C', 'D', 'E', 'F' };

            int num = 0;
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                num = 10;
            else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                num = 6;
            while (counter < num)
            {
                string H1 = tampalert.Substring(counter, 2).Substring(0, 1);
                string H2 = tampalert.Substring(counter, 2).Substring(1, 1);
                int intmod = 0;
                while (H1 != _hexChars[intmod].ToString()) { intmod++; }
                int decval = intmod * 16;
                intmod = 0;
                while (H2 != _hexChars[intmod].ToString()) { intmod++; }
                decval = decval + intmod;
                revLim += Convert.ToString(decval, 2).PadLeft(8, '0');
                counter += 2;
            }
            string[] tamperRow = TamperRow();
            DataRow newRow;
            num = 0;
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                num = 37;
            else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                num = 22;
            counter = 0;
            while (counter < 37)
            {
                newRow = table.NewRow();
                newRow["Tamper"] = tamperRow[counter].ToString();
                if (Convert.ToInt16(revLim.Substring(counter, 1)) == 0)
                    newRow["Status"] = "Absent";
                else
                    newRow["Status"] = "Present";
                table.Rows.Add(newRow);
                counter++;
            }
            return table;
        }

        private static string colText(int num)
        {
            if (num == 0)
                return "Last Time Stamp";
            else if (num == 1)
                return "2nd Last Time Stamp";
            else if (num == 2)
                return "3rd Last Time Stamp";
            else
                return string.Concat((num + 1).ToString(), "th Last Time Stamp");
        }

        public static DataSet ProgrammingTimeStamp(string data)
        {
            if (data.Trim() == string.Empty)
                return null;
            DataTable table = new DataTable();
            int col = 0;
            string[] programmingRow = ProgrammingUpdatedRow();
            for (col = 0; col < programmingRow.Length; col++)
                table.Columns.Add(new DataColumn(programmingRow[col], typeof(System.String)));

            int row = 0;
            for (row = 0; row <= 9; row++)
            {
                DataRow dataRow = table.NewRow();
                for (col = 0; col <= 17; col++)
                {
                    if (col == 0)
                        dataRow[col] = colText(row);
                    else
                        dataRow[col] = string.Empty;
                }
                table.Rows.Add(dataRow);
            }
            int dataIndex = 3;
            int MProgRowcnt = 0;
            int rowNo = 9;
            int paramIndex = 0;
            string[] progParam = new string[30];

            int colIndex = 0;
            int paramCount = 0;
            while (MProgRowcnt <= 9)
            {
                string programmingDate = data.Substring(dataIndex, 2) + "/" + data.Substring(dataIndex + 2, 2) + "/" + data.Substring(dataIndex + 4, 2);
                string programmingTime = data.Substring(dataIndex + 6, 2) + ":" + data.Substring(dataIndex + 8, 2);
                if (programmingDate != "00/00/00")
                {
                    table.Rows[rowNo][1] = string.Concat(programmingDate, "  ", programmingTime);
                }

                paramCount = 0;
                paramIndex = 0;

                int binVal = Convert.ToInt32(data.Substring(dataIndex + 10, 2), 16);
                if (binVal >= 128)
                {
                    binVal -= 128;
                }
                if (binVal >= 64)
                {
                    binVal -= 64;
                }
                if (binVal >= 32)
                {
                    binVal -= 32;
                }
                if (binVal >= 16)
                {
                    binVal -= 16;
                }
                if (binVal >= 8)
                {
                    binVal -= 8;
                }
                if (binVal >= 4)
                {
                    binVal -= 4;
                }
                if (binVal >= 2)
                {
                    binVal -= 2;
                }
                if (binVal == 1) //LPR
                {
                    progParam[paramIndex] = "LPR Parameters";
                    paramIndex += 1;
                    paramCount += 1;
                }

                binVal = Convert.ToInt32(data.Substring(dataIndex + 12, 2), 16);
                if (binVal >= 128)
                {
                    progParam[paramIndex] = "Daily Log Parameters";
                    paramIndex += 1;
                    binVal -= 128;
                    paramCount += 1;
                }
                if (binVal >= 64)
                {
                    progParam[paramIndex] = "Access Permissions";
                    paramIndex += 1;
                    binVal -= 64;
                    paramCount += 1;
                }
                if (binVal >= 32)
                {
                    progParam[paramIndex] = "Baud Rate";
                    paramIndex += 1;
                    binVal -= 32;
                    paramCount += 1;
                }
                if (binVal >= 16)
                {
                    progParam[paramIndex] = "KVAH Selection";
                    paramIndex += 1;
                    binVal -= 16;
                    paramCount += 1;
                }
                if (binVal >= 8)
                {
                    progParam[paramIndex] = "CT Ratio";
                    paramIndex += 1;
                    binVal -= 8;
                    paramCount += 1;
                }
                if (binVal >= 4)
                {
                    progParam[paramIndex] = "MD Reset Lock Out";
                    paramIndex += 1;
                    binVal -= 4;
                    paramCount += 1;
                }
                if (binVal >= 2)
                {
                    progParam[paramIndex] = "LCD Backlight";
                    paramIndex += 1;
                    binVal -= 2;
                    paramCount += 1;
                }
                if (binVal == 1)
                {
                    progParam[paramIndex] = "Resolution Parameters";
                    paramIndex += 1;
                    paramCount += 1;
                }

                binVal = Convert.ToInt32(data.Substring(dataIndex + 14, 2), 16);
                if (binVal >= 128)
                {
                    progParam[paramIndex] = "Tampers";
                    paramIndex += 1;
                    binVal -= 128;
                    paramCount += 1;
                }
                if (binVal >= 64)
                {
                    progParam[paramIndex] = "Load Survey";
                    paramIndex += 1;
                    binVal -= 64;
                    paramCount += 1;
                }
                if (binVal >= 32)
                {
                    progParam[paramIndex] = "Future TOU ";
                    paramIndex += 1;
                    binVal -= 32;
                    paramCount += 1;
                }
                if (binVal >= 16)
                {
                    progParam[paramIndex] = "Number of Billing";
                    paramIndex += 1;
                    binVal -= 16;
                    paramCount += 1;
                }
                if (binVal >= 8)
                {
                    progParam[paramIndex] = "Billing Date & Time";
                    paramIndex += 1;
                    binVal -= 8;
                    paramCount += 1;
                }
                if (binVal >= 4)
                {
                    progParam[paramIndex] = "Maximum Demand";
                    paramIndex += 1;
                    binVal -= 4;
                    paramCount += 1;
                }
                if (binVal >= 2)
                {
                    progParam[paramIndex] = "LCD Parameters";
                    paramIndex += 1;
                    binVal -= 2;
                    paramCount += 1;
                }
                if (binVal == 1)
                {
                    progParam[paramIndex] = "Meter ID";
                    paramIndex += 1;
                    paramCount += 1;
                }
                colIndex = 2;
                int count = 0;
                int index = paramIndex - 1;
                for (count = 0; count <= paramCount - 1; count++)
                {
                    table.Rows[rowNo][colIndex] = progParam[index];
                    colIndex += 1;
                    index--;
                }
                rowNo--;
                dataIndex += 16;
                MProgRowcnt++;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        public static DataSet RTCTimeStamp(string data)
        {
            DataTable table = new DataTable();
            int col = 0;
            string[] RTCRow = RTCUpdatedRow();
            for (col = 0; col < RTCRow.Length; col++)
                table.Columns.Add(new DataColumn(RTCRow[col], typeof(System.String)));
            int row = 0;
            for (row = 0; row <= 9; row++)
            {
                DataRow dataRow = table.NewRow();
                for (col = 0; col < 3; col++)
                {
                    if (col == 0)
                        dataRow[col] = colText(row);
                    else
                        dataRow[col] = string.Empty;
                }
                table.Rows.Add(dataRow);
            }

            int dataIndex = data.IndexOf("Sep") + 4;
            int noUpdates = Convert.ToInt32(data.Substring(dataIndex, 2), 16);
            dataIndex += 2;
            int count = 0;
            if (noUpdates > 10)
                noUpdates = 10;
            while (count < noUpdates)
            {
                string oldDate = data.Substring(dataIndex, 2) + "/" + data.Substring(dataIndex + 2, 2) + "/" + data.Substring(dataIndex + 4, 2);
                string oldTime = data.Substring(dataIndex + 6, 2) + ":" + data.Substring(dataIndex + 8, 2);
                string newDate = data.Substring(dataIndex + 10, 2) + "/" + data.Substring(dataIndex + 12, 2) + "/" + data.Substring(dataIndex + 14, 2);
                string newTime = data.Substring(dataIndex + 16, 2) + ":" + data.Substring(dataIndex + 18, 2);
                table.Rows[noUpdates - 1][1] = string.Concat(oldDate, "    ", oldTime);
                table.Rows[noUpdates - 1][2] = string.Concat(newDate, "    ", newTime);
                noUpdates--;
                dataIndex += 20;
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        private static string[] PhasorRow()
        {
            string[] array = new string[2];
            array[0] = "Parameter";
            array[1] = "Value";
            return array;
        }
        private static string[] PhasorColumnValues()
        {
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                return PhasorColumnValues650();
            else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                return PhasorColumnValues250();
            else
                return null;
        }
        private static string[] PhasorColumnValues650()
        {
            string[] array = new string[26];
            array[0] = "R Phase Voltage";
            array[1] = "Y Phase Voltage";
            array[2] = "B Phase Voltage";
            array[3] = "R Phase Current";
            array[4] = "Y Phase Current";
            array[5] = "B Phase Current";
            array[6] = "R Phase PF";
            array[7] = "Y Phase PF";
            array[8] = "B Phase PF";
            array[9] = "Total Instantaneous PF";
            array[10] = "Frequency";
            array[11] = "Phase Sequence";
            array[12] = "Total kW Direction";
            array[13] = "R Phase kW Direction";
            array[14] = "Y Phase kW Direction";
            array[15] = "B Phase kW Direction";
            array[16] = "R Phase Channel";
            array[17] = "Y Phase Channel";
            array[18] = "B Phase Channel";
            array[19] = "R Phase Lag/Lead";
            array[20] = "Y Phase Lag/Lead";
            array[21] = "B Phase Lag/Lead";
            array[22] = "Total";
            array[23] = "Y Phase Angle With R Phase";
            array[24] = "B Phase Angle With R Phase";
            array[25] = "Angle B/W Any 2 Phase Present";
            return array;
        }
        private static string[] PhasorColumnValues250()
        {
            string[] array = new string[30];
            array[0] = "R Phase Voltage";
            array[1] = "Y Phase Voltage";
            array[2] = "B Phase Voltage";
            array[3] = "R Phase Current";
            array[4] = "Y Phase Current";
            array[5] = "B Phase Current";
            array[6] = "Total Active Power";
            array[7] = "Total Inductive Power";
            array[8] = "Total Capacitive Power";
            array[9] = "Total Apparent Power";
            array[10] = "R Phase PF";
            array[11] = "Y Phase PF";
            array[12] = "B Phase PF";
            array[13] = "Total Instantaneous PF";
            array[14] = "Frequency";
            array[15] = "Phase Sequence";
            array[16] = "Total kW Direction";
            array[17] = "R Phase kW Direction";
            array[18] = "Y Phase kW Direction";
            array[19] = "B Phase kW Direction";
            array[20] = "R Phase Channel";
            array[21] = "Y Phase Channel";
            array[22] = "B Phase Channel";
            array[23] = "R Phase Lag/Lead";
            array[24] = "Y Phase Lag/Lead";
            array[25] = "B Phase Lag/Lead";
            array[26] = "Total";
            array[27] = "Y Phase Angle With R Phase";
            array[28] = "B Phase Angle With R Phase";
            array[29] = "Angle B/W Any 2 Phase Present";
            return array;
        }

        public static string PhasorFilterData(string data, int start, int end, int div, string format)
        {
            return (Convert.ToDouble((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString()) / div).ToString(format);
        }

        public static string PhasorFilterData(string data, int start, int end, double div)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber) * div).ToString());
        }

        public static string PhasorFilterData(string data, int start, int end)
        {
            return ((Int32.Parse(data.Substring(start, end), NumberStyles.HexNumber)).ToString());
        }
        public static DataSet DisplayPhasor(string PhasorPara)
        {
            if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_LTCT_650))
                return DisplayPhasor650(PhasorPara);
            else if (ConfigInfo.GetApplicationType().Equals(ApplicationType.IEC_RUBY_250))
                return DisplayPhasor250(PhasorPara);
            else
                return null;
        }
        public static DataSet DisplayPhasor650(string PhasorPara)
        {

            DataTable table = new DataTable();
            int col = 0;
            string[] phasorRow = PhasorRow();
            string[] phasorColumn = PhasorColumnValues650();

            for (col = 0; col < phasorRow.Length; col++)
                table.Columns.Add(new DataColumn(phasorRow[col], typeof(System.String)));
            for (int rowCount = 0; rowCount < phasorColumn.Length; rowCount++)
            {
                DataRow dataRow = table.NewRow();
                for (col = 0; col < 2; col++)
                {
                    if (col == 0)
                        dataRow[col] = phasorColumn[rowCount];
                    else
                        dataRow[col] = string.Empty;
                }
                table.Rows.Add(dataRow);
            }
            /*Voltage R y  b  Phase*/
            table.Rows[0][1] = PhasorFilterData(PhasorPara, 1, 4, 100, "0.00");
            table.Rows[1][1] = PhasorFilterData(PhasorPara, 5, 4, 100, "0.00");
            table.Rows[2][1] = PhasorFilterData(PhasorPara, 9, 4, 100, "0.00");
            /*Current R y  b  Phase*/
            table.Rows[3][1] = PhasorFilterData(PhasorPara, 13, 8, 1000, "0.000");
            table.Rows[4][1] = PhasorFilterData(PhasorPara, 21, 8, 1000, "0.000");
            table.Rows[5][1] = PhasorFilterData(PhasorPara, 29, 8, 1000, "0.000");
            /*PF R y  b  Phase*/
            table.Rows[6][1] = PhasorFilterData(PhasorPara, 37, 4, 10000, "0.00");
            table.Rows[7][1] = PhasorFilterData(PhasorPara, 41, 4, 10000, "0.00");
            table.Rows[8][1] = PhasorFilterData(PhasorPara, 45, 4, 10000, "0.00");
            /*Net PF */
            table.Rows[9][1] = PhasorFilterData(PhasorPara, 49, 8, 10000, "0.00");
            /*Frequency */
            table.Rows[10][1] = PhasorFilterData(PhasorPara, 57, 4, 100, "0.00");
            /*Phase Sequence */
            string tempval = ((Int32.Parse(PhasorPara.Substring(61, 2), NumberStyles.HexNumber)).ToString());
            if (Convert.ToInt16(tempval) > 0)
            {
                while (tempval.Length < 3)
                    tempval = "0" + tempval;
                if (tempval.Substring(0, 1) == "1") table.Rows[11][1] = "R";
                if (tempval.Substring(0, 1) == "2") table.Rows[11][1] = "Y";
                if (tempval.Substring(0, 1) == "3") table.Rows[11][1] = "B";
                if (tempval.Substring(1, 1) == "1") table.Rows[11][1] += "R";
                if (tempval.Substring(1, 1) == "2") table.Rows[11][1] += "Y";
                if (tempval.Substring(1, 1) == "3") table.Rows[11][1] += "B";
                if (tempval.Substring(2, 1) == "1") table.Rows[11][1] += "R";
                if (tempval.Substring(2, 1) == "2") table.Rows[11][1] += "Y";
                if (tempval.Substring(2, 1) == "3") table.Rows[11][1] += "B";
            }
            else
                table.Rows[11][1] = tempval;
            /*Total */

            table.Rows[12][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 63, 2)) == 0) ? "Import" : "Export";

            /*Iport/Export R y  b  Phase*/
            table.Rows[13][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 65, 2)) == 0) ? "Import" : "Export";
            table.Rows[14][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 67, 2)) == 0) ? "Import" : "Export";
            table.Rows[15][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 69, 2)) == 0) ? "Import" : "Export";

            /*Chaneel Missing R y  b  Phase*/
            table.Rows[16][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 71, 2)) == 0) ? "Absent" : "Present";
            table.Rows[17][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 73, 2)) == 0) ? "Absent" : "Present";
            table.Rows[18][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 75, 2)) == 0) ? "Absent" : "Present";

            /*Lag/ Lead R y  b  Phase*/
            table.Rows[19][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 77, 2)) == 0) ? "Lag" : "Lead";
            table.Rows[20][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 79, 2)) == 0) ? "Lag" : "Lead";
            table.Rows[21][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 81, 2)) == 0) ? "Lag" : "Lead";

            /*Lag/ Lead Total*/
            table.Rows[22][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 83, 2)) == 0) ? "Lag" : "Lead";

            /* Y B Phase Angle with respect to R Phase*/
            table.Rows[23][1] = PhasorFilterData(PhasorPara, 85, 2, 7.2);
            table.Rows[24][1] = PhasorFilterData(PhasorPara, 87, 2, 7.2);
            table.Rows[25][1] = PhasorFilterData(PhasorPara, 89, 2, 7.2);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public static DataSet DisplayPhasor250(string PhasorPara)
        {

            DataTable table = new DataTable();
            int col = 0;
            string[] phasorRow = PhasorRow();
            string[] phasorColumn = PhasorColumnValues250();

            for (col = 0; col < phasorRow.Length; col++)
                table.Columns.Add(new DataColumn(phasorRow[col], typeof(System.String)));
            for (int rowCount = 0; rowCount < phasorColumn.Length; rowCount++)
            {
                DataRow dataRow = table.NewRow();
                for (col = 0; col < 2; col++)
                {
                    if (col == 0)
                        dataRow[col] = phasorColumn[rowCount];
                    else
                        dataRow[col] = string.Empty;
                }
                table.Rows.Add(dataRow);
            }
            /*Voltage R y  b  Phase*/
            table.Rows[0][1] = PhasorFilterData(PhasorPara, 1, 4, 100, "0.00");
            table.Rows[1][1] = PhasorFilterData(PhasorPara, 5, 4, 100, "0.00");
            table.Rows[2][1] = PhasorFilterData(PhasorPara, 9, 4, 100, "0.00");

            /*Current R y  b  Phase*/
            table.Rows[3][1] = PhasorFilterData(PhasorPara, 13, 8, 1000, "0.000");
            table.Rows[4][1] = PhasorFilterData(PhasorPara, 21, 8, 1000, "0.000");
            table.Rows[5][1] = PhasorFilterData(PhasorPara, 29, 8, 1000, "0.000");

            ///*Resolution*/
            //table.Rows[6][1] = PhasorFilterData(PhasorPara, 37, 1);

            /*Total Active, Inductive, Capacitive and Apparent Power*/
            table.Rows[6][1] = PhasorFilterData(PhasorPara, 38, 8, 100000, "0.000");
            table.Rows[7][1] = PhasorFilterData(PhasorPara, 46, 8, 100000, "0.000");
            table.Rows[8][1] = PhasorFilterData(PhasorPara, 54, 8, 100000, "0.000");
            table.Rows[9][1] = PhasorFilterData(PhasorPara, 62, 8, 100000, "0.000");


            /*PF R y  b  Phase*/
            table.Rows[10][1] = PhasorFilterData(PhasorPara, 70, 4, 10000, "0.00");
            table.Rows[11][1] = PhasorFilterData(PhasorPara, 74, 4, 10000, "0.00");
            table.Rows[12][1] = PhasorFilterData(PhasorPara, 78, 4, 10000, "0.00");
            /*Net PF */
            table.Rows[13][1] = PhasorFilterData(PhasorPara, 82, 8, 10000, "0.00");
            /*Frequency */
            table.Rows[14][1] = PhasorFilterData(PhasorPara, 90, 4, 100, "0.00");
            /*Phase Sequence */
            string tempval = ((Int32.Parse(PhasorPara.Substring(94, 2), NumberStyles.HexNumber)).ToString());
            if (Convert.ToInt16(tempval) > 0)
            {
                while (tempval.Length < 3)
                    tempval = "0" + tempval;
                if (tempval.Substring(0, 1) == "1") table.Rows[15][1] = "R";
                if (tempval.Substring(0, 1) == "2") table.Rows[15][1] = "Y";
                if (tempval.Substring(0, 1) == "3") table.Rows[15][1] = "B";
                if (tempval.Substring(1, 1) == "1") table.Rows[15][1] += "R";
                if (tempval.Substring(1, 1) == "2") table.Rows[15][1] += "Y";
                if (tempval.Substring(1, 1) == "3") table.Rows[15][1] += "B";
                if (tempval.Substring(2, 1) == "1") table.Rows[15][1] += "R";
                if (tempval.Substring(2, 1) == "2") table.Rows[15][1] += "Y";
                if (tempval.Substring(2, 1) == "3") table.Rows[15][1] += "B";
            }
            else
                table.Rows[15][1] = tempval;

            /*Total */
            table.Rows[16][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 96, 2)) == 0) ? "Import" : "Export";

            /*Import/Export R y  b  Phase*/
            table.Rows[17][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 98, 2)) == 0) ? "Import" : "Export";
            table.Rows[18][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 100, 2)) == 0) ? "Import" : "Export";
            table.Rows[19][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 102, 2)) == 0) ? "Import" : "Export";

            /*Chaneel Missing R y  b  Phase*/
            table.Rows[20][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 104, 2)) == 0) ? "Absent" : "Present";
            table.Rows[21][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 106, 2)) == 0) ? "Absent" : "Present";
            table.Rows[22][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 108, 2)) == 0) ? "Absent" : "Present";

            /*Lag/ Lead R y  b  Phase*/
            table.Rows[23][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 110, 2)) == 0) ? "Lag" : "Lead";
            table.Rows[24][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 112, 2)) == 0) ? "Lag" : "Lead";
            table.Rows[25][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 114, 2)) == 0) ? "Lag" : "Lead";

            /*Lag/ Lead Total*/
            table.Rows[26][1] = (Convert.ToInt16(PhasorFilterData(PhasorPara, 116, 2)) == 0) ? "Lag" : "Lead";

            /* Y B Phase Angle with respect to R Phase*/
            table.Rows[27][1] = PhasorFilterData(PhasorPara, 118, 2, 7.2);
            table.Rows[28][1] = PhasorFilterData(PhasorPara, 120, 2, 7.2);
            table.Rows[29][1] = PhasorFilterData(PhasorPara, 122, 2, 7.2);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
    }
}
