using System;
using System.Data;
using CAB.DALC.Data;
using CAB.Framework;
using System.IO.Ports;

namespace CAB.BLL
{
    public class SettingsBLL : IBLL
    {
        public DataSet GetPort()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row;
            for (int i=0;i<portNames.Length;i++)
            {
                row=dataTable.NewRow();
                row[0]=row[1]=portNames[i];
                dataTable.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetHours()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row;
            for (int i=0;i<=23;i++)
            {
                row=dataTable.NewRow();
                row[0]=row[1]=i.ToString("00");
                dataTable.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetMinutes()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row;
            for (int i = 1; i <= 59; i++)
            {
                row = dataTable.NewRow();
                row[0] = row[1] = i.ToString("00");
                dataTable.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetPeriod()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row; 
            row = dataTable.NewRow();
            row[0] = "Daily";
            row[1] = "D";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = "Weekly";
            row[1] = "W";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = "Monthly";
            row[1] = "M";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = "Month End";
            row[1] = "E";
            dataTable.Rows.Add(row);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetStatus()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row; 
            row = dataTable.NewRow();
            row[0] = "Active";
            row[1] = "1";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = "Inactive";
            row[1] = "0";
            dataTable.Rows.Add(row);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetDays()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String))); 
            for (int i = 1; i <= 31; i++)
            {
                DataRow row;
                row = dataTable.NewRow();
                row[0] =  ColText(i);
                row[1] = i.ToString();
                dataTable.Rows.Add(row); 
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        private string ColText(int num)
        {
            if (num == 1)
                return "1st Day";
            else if (num == 2)
                return "2nd Day";
            else if (num == 3)
                return "3rd Day";
            else
                return string.Concat( num.ToString(), "th Day");
        }
        public DataSet GetDayNames()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DateTime currentTime = System.DateTime.Now;
            for (int i = 1; i <= 7; i++)
            {
                DataRow row;
                row = dataTable.NewRow();
                row[0] =row[1]=currentTime.DayOfWeek.ToString();
                dataTable.Rows.Add(row);
                currentTime = currentTime.AddDays(1);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetBaudRate()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row;
            int counter = 150; 
            for (int i = 1; i < 7; i++)
            {
                row = dataTable.NewRow();
                row[0] = row[1] = counter = counter * 2;
                dataTable.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetAttempt()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            string[] portNames = SerialPort.GetPortNames();
            DataRow row; 
            for (int counter = 1; counter <= 5; counter++)
            {
                row = dataTable.NewRow();
                row[0] = row[1] = counter.ToString();
                dataTable.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetCommunicationMode()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            row = dataTable.NewRow();
            row[0] = row[1] = "RS 232";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = row[1] = "Optical";
            dataTable.Rows.Add(row);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
        public DataSet GetDateTypes()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            dataTable.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            row = dataTable.NewRow();
            row[0] = row[1] = "dd/MM/yyyy";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = row[1] = "MM/dd/yyyy";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = row[1] = "dd-MM-yyyy";
            dataTable.Rows.Add(row);
            row = dataTable.NewRow();
            row[0] = row[1] = "MM-dd-yyyy";
            dataTable.Rows.Add(row);
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }
    }
}
