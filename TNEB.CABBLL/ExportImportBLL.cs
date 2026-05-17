using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework;
using CAB.DALC.Data;
using CAB.IECFramework.Entity;
using CAB.Entity;
using System.Data.Common;
using System.Data;
namespace CAB.BLL
{
	public class ExportImportBLL : IBLL
	{
		//ConsumerMasterDAL consumerMasterDAL;
		MeterMasterDAL meterMasterDAL;
		//ConsumerMeterDAL consumerMeterDAL;

		//MeterDataDAL meterDataDAL;
		GeneralDAL generalDAL;
		InstantPowerDAL instantPowerDAL;
		BillingDAL billingDAL;
		LoadSurveyDAL loadSurveyDAL;
		//FraudEnergyDAL fraudEnergyDAL;
		TamperCounterDAL tamperCounterDAL;
		TamperCounterGeneralDAL tamperCounterGeneralDAL;
		TamperSnapShotDAL tamperSnapShotDAL;
		TariffDAL tariffDAL;

		private Int32 _meterDataID;

		public Int32 MeterDataID
		{
			get
			{
				return _meterDataID;
			}
			set
			{
				_meterDataID = value;
			}
		}

		public ExportImportBLL()
		{
			//consumerMasterDAL = new ConsumerMasterDAL();
			meterMasterDAL = new MeterMasterDAL();
			//consumerMeterDAL = new ConsumerMeterDAL();

			//meterDataDAL = new MeterDataDAL();
			generalDAL = new GeneralDAL();
			instantPowerDAL = new InstantPowerDAL();
			billingDAL = new BillingDAL();
			loadSurveyDAL = new LoadSurveyDAL();
			//fraudEnergyDAL = new FraudEnergyDAL();
			tamperCounterDAL = new TamperCounterDAL();
			tamperCounterGeneralDAL = new TamperCounterGeneralDAL();
			tamperSnapShotDAL = new TamperSnapShotDAL();
			tariffDAL = new TariffDAL();
		}

		public IEntity GetGeneralData()
		{
			return generalDAL.GetEntityData(MeterDataID);
		}

		public IEntity GetInstantPowerEntity()
		{
			return instantPowerDAL.GetEntityData(MeterDataID);
		}

		public List<string> GetColumnList(string tableName)
		{
			return generalDAL.GetColumnList(tableName);
		}

		public IEntity GetInstantData()
		{
			return instantPowerDAL.GetDetailData(MeterDataID);
		}

		public DataSet GetBillingData()
		{
			return billingDAL.ListDataSet(MeterDataID);
		}

		public DataSet GetLoadSurveyData()
		{
			return null;//loadSurveyDAL.ListDataSet(MeterDataID);
		}

        //public DataSet GetFraudEnergyData()
        //{
        //    return fraudEnergyDAL.ListDataSet(MeterDataID);
        //}

		public DataSet GetTamperCounterData()
		{
			return tamperCounterDAL.ListDataSet(MeterDataID);
		}

		public DataSet GetTamperCounterGeneralData()
		{
			return tamperCounterGeneralDAL.ListDataSet(MeterDataID);
		}

		public DataSet GetTamperSnapShotsData()
		{
			return tamperSnapShotDAL.ListDataSet(MeterDataID);
		}

		public DataSet GetTariffInformationData()
		{
			return tariffDAL.ListDataSet(MeterDataID);
		}

		public DataSet GetConsumerDetails()
		{
           return new DataSet();//return consumerMeterDAL.ListDataSet();
		}

		public bool CheckConsumerAvailable(IEntity entity)
		{
			//return consumerMasterDAL.ValidateConsumerNumber(entity);
           return true;
		}

		public bool CheckMeterIDAvailable(IEntity entity)
		{
			return meterMasterDAL.ValidateMeterNumber(entity);
		}

		public bool CheckConsumerMeterAvailable(IEntity entity)
		{
            return true;//consumerMeterDAL.GetDataAvailability(entity);
		}

		public bool InsertConsumerMasterValues(IEntity entity)
		{
            return true;//consumerMasterDAL.InsertData(entity);
		}

		public bool InsertMeterValues(IEntity entity)
		{
            return true;//meterMasterDAL.InsertData(entity);
		}

		public bool InsertConsumerMeterValues(IEntity entity)
		{
            return true;//consumerMeterDAL.InsertData(entity);
		}

		public bool UpdateConsumerMasterValues(IEntity entity)
		{
			return true;
		}

		public bool UpdateMeterValues(IEntity entity)
		{
			return meterMasterDAL.UpdateData(entity);
		}

		public bool UpdateConsumerMeterValues(IEntity entity)
		{
            return true;
               //return consumerMeterDAL.UpdateData(entity);
		}

		public DataTable GetMeterIDFileNameList()
		{
            DataSet dataSet = new DataSet();//meterDataDAL.GetList();
			DataTable dTable = new DataTable();
			if (dataSet != null)
			{
				DataRow dr;
				dTable.Columns.Add("S.No");
				dTable.Columns.Add("File Name");
				dTable.Columns.Add("Meter ID");
				dTable.Columns.Add("MeterData_ID");
				dTable.Columns.Add("isSelected", typeof(bool));

				foreach (DataRow drow in dataSet.Tables[0].Rows)
				{
					dr = dTable.NewRow();
					dr["S.No"] = drow["S.No"].ToString();
					dr["File Name"] = drow["FileName"].ToString();
					dr["Meter ID"] = drow["MeterID"].ToString();
					dr["MeterData_ID"] = drow["MeterData_ID"].ToString();
					dr["isSelected"] = false;
					dTable.Rows.Add(dr);
				}
			}
			return dTable;
		}
	}
}
