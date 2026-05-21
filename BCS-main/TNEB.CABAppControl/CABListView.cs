using System.Resources;
using System.Windows.Forms;
using System.Data;
using System;
using CAB.IECFramework.Utility;

namespace CAB.UI.Controls
{
    public partial class CABListView : ListView
    {
        private string comboDataType = null;
        private DataSet dataSet = null;
        public string ComboDataType
        {
            get
            {
                return comboDataType;
            }
            set
            {
                comboDataType = value;
            }
        }
        public DataSet ListData
        {
            get
            {
                return dataSet;
            }
            set
            {
                dataSet = value;
                LoadList();
            }
        }
        private void LoadList()
        {
            this.Items.Clear();
            if (dataSet == null)
                return;
            if (dataSet.Tables.Count < 1)
                return;
            int counter = 0;
            this.Columns.Add("DisplayMember", 20, HorizontalAlignment.Left);
            this.Columns.Add("ValueMember", 200, HorizontalAlignment.Left);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {

                if (comboDataType.Equals("RD"))
                {
                    string val = Convert.ToString(row[0]);
                    if (string.IsNullOrEmpty(val))
                        continue;
                    this.Items.Add(Convert.ToString(val), 0);
                    this.Items[counter++].SubItems.Add(Convert.ToString(val));
                }
                else
                {
                    this.Items.Add(Convert.ToString(row[0]), 0);
                    this.Items[counter++].SubItems.Add(Convert.ToString(row[0]));
                }
            }
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Width != 20)
                    this.Columns[i].Width = 250;
            }
        }

    }
}
 