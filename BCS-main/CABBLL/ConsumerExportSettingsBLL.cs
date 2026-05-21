using System;
using System.Data;
using CAB.Framework;
using CAB.Framework.Utility;
using System.Collections.Generic;
using CAB.Framework.Entity;
using CAB.DALC.Data;
using CAB.Entity;

namespace CAB.BLL
{
    public class ConsumerExportSettingsBLL : IBLL
    {
        private ConsumerExportSettingsDAL consumerExportSettingsDAL = new ConsumerExportSettingsDAL();
        public string[] GetDBColumnName()
        {
            string[] param = new string[29];
            param[0] = "A.Consumer_Number as 'Consumer ID',";
            param[1] = "A.Consumer_Name as 'Consumer Name',";
            param[2] = "A.ConsumerType_ID as 'Consumer Type ID',";
            param[3] = "A.Consumer_Phone as 'Consumer Phone',";
            param[4] = "A.Consumer_HNumber as 'House Number',";
            param[5] = "A.Consumer_Street as 'Street',";
            param[6] = "A.Consumer_City as 'City',";
            param[7] = "A.Consumer_Email as 'Email',";
            param[8] = "B.Meter_ID as 'Meter ID',";
            param[9] = "B.MeterType_ID as 'Meter Type ID',";
            param[10] = "B.MeterModel_ID as 'Meter Model ID',";
            param[11] = "C.Meter_Location as 'Meter Location',";
            param[12] = "C.Meter_AllocationDate as 'Meter Allocation Date',";

            //following 3 parameters added to resolve bug 73549; 11th april 2012
            param[13] = "R.Region_Name as 'Region Name',";
            param[14] = "CR.Circle_Name as 'Circle Name',";
            param[15] = "D.Division_Name as 'Division Name',";

            param[16] = "B.Meter_EMF as 'EMF',";
            param[17] = "B.Meter_ContractDemand as 'Contract Demand',";
            param[18] = "B.MeterUnit_ID as 'Meter Unit ID',";
            
            //following 2 lines changed to include meter internal CT and PT ration for export(inst; 12 April 2012
            //param[19] = "MG.internalCTratio as 'Internal CT Ratio',";
            //param[20] = "MG.internalPTratio as 'Internal PT Ratio',";
            param[19] = "B.Meter_CTPrimary as 'Internal CT Ratio',";
            param[20] = "B.Meter_PTPrimary as 'Internal PT Ratio',";

            param[21] = "B.Meter_InstalledCTPrimary as 'Installed CT Primary',";
            param[22] = "B.Meter_InstalledCTSecondary as 'Installed CT Secondary',";
            param[23] = "B.Meter_InstalledPTPrimary as 'Installed PT Primary',";
            param[24] = "B.Meter_InstalledPTSecondary as 'Installed PT Secondary',";
            param[25] = "C.Communication_Type as 'Communication Type',";
            param[26] = "B.Meter_Phone as 'Meter Sim Number',";
            param[27] = "B.Meter_Status as 'Meter Status',"; 
            param[28] = "C.Status as 'Consumer Status' ,";
            return param;
        }
        public string[] GetDisplayColumnName()
        {
            string[] param = new string[29];
            param[0] = "Consumer ID";
            param[1] = "Consumer Name";
            param[2] = "Consumer Type";
            param[3] = "Telephone Number";
            param[4] = "House Number";
            param[5] = "Street";
            param[6] = "City/Town";

            //following 3 parameters added to resolve bug 73549; 11th april 2012
            param[7] = "Region Name";
            param[8] = "Circle Name";
            param[9] = "Division Name";

            param[10] = "E-mail"; 
            param[11] = "Meter ID";
            param[12] = "Meter Type";
            param[13] = "Meter Model";
            param[14] = "Meter Location";
            param[15] = "Installation Date"; 
            param[16] = "EMF";
            param[17] = "Contract Demand";
            param[18] = "Unit";
            param[19] = "CT Ratio";
            param[20] = "PT Ratio";
            param[21] = "Installed CT Primary";
            param[22] = "Installed CT Secondary";
            param[23] = "Installed PT Primary";
            param[24] = "Installed PT Secondary";
            param[25] = "Communication Type";
            param[26] = "Meter SIM Number";
            param[27] = "Meter Status"; 
            param[28] = "Consumer Status";
            return param;
        }
        public string GetDBColumn(string text)
        {
            string dbText = string.Empty;
            string[] DispCol = GetDisplayColumnName();
            string[] dbCol = GetDBColumnName();
            for (int counter = 0; counter < DispCol.Length; counter++)
            {
                if (text.Trim().Equals(DispCol[counter]))
                {
                    dbText = dbCol[counter];
                    break;
                }
            }
            return dbText;
        }
        public DataSet GetAllParameter()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("DisplayMember", typeof(System.String)));
            table.Columns.Add(new DataColumn("ValueMember", typeof(System.String)));
            DataRow row;
            string[] displayMember = GetDisplayColumnName();
            string[] valueMember = GetDBColumnName();
            for (int counter = 0; counter < displayMember.Length; counter++)
            {
                row = table.NewRow();
                row[0] = displayMember[counter];
                row[1] = valueMember[counter]; 
                table.Rows.Add(row);
            }
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(table);
            return dataSet;
        }
        public IEntity InsertData(IEntity entity)
        {
            return consumerExportSettingsDAL.InsertData(entity);
        }
        public bool UpdateData(IEntity entity)
        {
            return consumerExportSettingsDAL.UpdateData(entity);
        }
        
        public bool IsValidFile(string fileName)
        {
            return consumerExportSettingsDAL.ValidateFile(fileName);
        }
        public DataSet ListDataSet()
        {
            return consumerExportSettingsDAL.ListDataSet();
        }

        public string[] DetailData(int id)
        {
            ConsumerExportSettingsEntity entity = consumerExportSettingsDAL.GetDetailData(id) as ConsumerExportSettingsEntity;
            if (!string.IsNullOrEmpty(entity.ParametersName))
                return entity.ParametersName.Split(',');
            else
                return null;
        }
        public IEntity DetailData(string id)
        {
            return consumerExportSettingsDAL.GetDetailData(Convert.ToInt32(id) );
        }
        public void DeleteSettings(int settingsID)
        {
            consumerExportSettingsDAL.DeleteData(settingsID);
        }

        public DataSet GetParameterData(string qry)
        {
           return consumerExportSettingsDAL.GetParameterData(qry);
        }
    }
}
