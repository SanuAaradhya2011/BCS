using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAB.EntityGenerator;
using CAB.Serialization;
using CAB.Parser;
using System.Drawing;
using System.Data;
using CAB.Entity;
using CAB.Framework.Utility;
using System.IO;
using Common.EntityMapper;
using System.Globalization;
using Hunt.EPIC.Logging;

namespace CABApplication.Export_Import
{
    public class HVDS_Readout_File
    {
        #region MemberVariable&Constructor


        private string _SourceFilePath = string.Empty;
        private string _MeterID = string.Empty;
        private string _FileText = string.Empty;
        private DateTime _ReadingDateTime = DateTime.MinValue;
        private int _MeterIdLength = 0;
        private DataGridViewCell _dgvStatuscell = null;    
        private HVDSSettings HVDSSetting = null;
        private static object lockxml = new object();
        private static object lockParser = new object();
        private string HVDS_Export_Directory = string.Empty;
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(HVDS_Readout_File).ToString());


        public DataGridViewCell dgvStatuscell
        {
            get
            {
                return _dgvStatuscell;
            }
            set
            {
                _dgvStatuscell = value;
            }
        }


        public int MeterIdLength
        {
            get
            {
                return _MeterIdLength;
            }
            set
            {
                _MeterIdLength = value;
            }
        }

        public string SourceFilePath
        {
            get
            {
                return _SourceFilePath;
            }
            set
            {
                _SourceFilePath = value;
            }
        }

        public string MeterID
        {
            get
            {
                return _MeterID;
            }
            set
            {
                _MeterID = value;
            }
        }

        public string FileText
        {
            get
            {
                return _FileText;
            }
            set
            {
                _FileText = value;
            }
        }

        public DateTime ReadingDateTime
        {
            get
            {
                return _ReadingDateTime;
            }
            set
            {
                _ReadingDateTime = value;
            }
        }



        public HVDS_Readout_File(string hVDS_Export_Directory)
        {
            Serializer serializer = new Serializer();
            HVDS_Export_Directory = hVDS_Export_Directory;
            //For urgent delivery Fixed Export, Design exist for Dynamic XML configuration Export     
            //lock (lockxml)
            //{
            //    if (HVDSSetting == null)
            //    {
            //        HVDSSetting = (HVDSSettings)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "HVDSExportParameters.xml"), typeof(HVDSSettings));
            //    }
            //}
        }

        #endregion



        private static List<ProfileData> ParseEntity(string FileText)
        {
            lock (lockParser)
            {
                GenerateEntity objGenerateEntity = new GenerateEntity();
                return objGenerateEntity.GetProfileWiseEntityList(FileText, false);
            }
        }


        private void Updatestatus(string Status, Color color)
        {
            try
            {
                DataGridView dgv = ((DataGridViewRow)_dgvStatuscell.OwningRow).DataGridView;
                Action a = () =>
                {

                    _dgvStatuscell.Style.BackColor = color;
                    _dgvStatuscell.Value = Status;
                };
                if (dgv.InvokeRequired)
                {
                    dgv.BeginInvoke(a);
                }
                else
                {
                    a();
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "Updatestatus(string Status, Color color)", ex);

            }
        }


        private DataSet CreateDetailTamperDataSet()
        {
            DataSet detailTamperData = new DataSet();
            DataTable tamperData = new DataTable();
            tamperData.Columns.Add("Description", typeof(string));
            tamperData.Columns.Add("OccEvent", typeof(string));
            tamperData.Columns.Add("ResEvent", typeof(string));
            tamperData.Columns.Add("OccDateTime", typeof(string));
            tamperData.Columns.Add("ResDateTime", typeof(string));
            tamperData.Columns.Add("OccRVoltage", typeof(string));
            tamperData.Columns.Add("ResRVoltage", typeof(string));
            tamperData.Columns.Add("OccYVoltage", typeof(string));
            tamperData.Columns.Add("ResYVoltage", typeof(string));
            tamperData.Columns.Add("OccBVoltage", typeof(string));
            tamperData.Columns.Add("ResBVoltage", typeof(string));
            tamperData.Columns.Add("OccPhaseVoltage", typeof(string));
            tamperData.Columns.Add("ResPhaseVoltage", typeof(string));
            tamperData.Columns.Add("OccRCurrent", typeof(string));
            tamperData.Columns.Add("ResRCurrent", typeof(string));
            tamperData.Columns.Add("OccYCurrent", typeof(string));
            tamperData.Columns.Add("ResYCurrent", typeof(string));
            tamperData.Columns.Add("OccBCurrent", typeof(string));
            tamperData.Columns.Add("ResBCurrent", typeof(string));
            tamperData.Columns.Add("OccPhaseCurrent", typeof(string));
            tamperData.Columns.Add("ResPhaseCurrent", typeof(string));
            tamperData.Columns.Add("OccNeutralCurrent", typeof(string));// Story - 349654 - Neutral current in Tamper
            tamperData.Columns.Add("ResNeutralCurrent", typeof(string));

            tamperData.Columns.Add("OccHighNeutralCurrent", typeof(string));// Story - pradipta_neu
            tamperData.Columns.Add("ResHighNeutralCurrent", typeof(string));

            tamperData.Columns.Add("OcckWr", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWr", typeof(string));

            tamperData.Columns.Add("OcckWy", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWy", typeof(string));

            tamperData.Columns.Add("OcckWb", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskWb", typeof(string));


            tamperData.Columns.Add("OcckVAr", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAr", typeof(string));

            tamperData.Columns.Add("OcckVAy", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAy", typeof(string));

            tamperData.Columns.Add("OcckVAb", typeof(string));// pradipta_neu
            tamperData.Columns.Add("ReskVAb", typeof(string));



            tamperData.Columns.Add("OccRPF", typeof(string));
            tamperData.Columns.Add("ResRPF", typeof(string));
            tamperData.Columns.Add("OccYPF", typeof(string));
            tamperData.Columns.Add("ResYPF", typeof(string));
            tamperData.Columns.Add("OccBPF", typeof(string));
            tamperData.Columns.Add("ResBPF", typeof(string));
            tamperData.Columns.Add("OccPF", typeof(string));
            tamperData.Columns.Add("ResPF", typeof(string));
            tamperData.Columns.Add("OccKwh", typeof(string));
            tamperData.Columns.Add("ResKwh", typeof(string));
            tamperData.Columns.Add("OccKVAh", typeof(string));
            tamperData.Columns.Add("ResKVAh", typeof(string));
            tamperData.Columns.Add("OcckWhImport", typeof(string));
            tamperData.Columns.Add("OcckWhExport", typeof(string));
            tamperData.Columns.Add("ReskWhImport", typeof(string));
            tamperData.Columns.Add("ReskWhExport", typeof(string));
            tamperData.Columns.Add("OcckVAhImport", typeof(string));
            tamperData.Columns.Add("OcckVAhExport", typeof(string));
            tamperData.Columns.Add("ReskVAhImport", typeof(string));
            tamperData.Columns.Add("ReskVAhExport", typeof(string));

            tamperData.Columns.Add("OcckvarhLag", typeof(string));
            tamperData.Columns.Add("OcckvarhLead", typeof(string));
            tamperData.Columns.Add("ReskvarhLag", typeof(string));
            tamperData.Columns.Add("ReskvarhLead", typeof(string));

            detailTamperData.Tables.Add(tamperData);
            return detailTamperData;
        }


        private DataSet GetDetailTamperData(List<DLMS650TamperEntity> TamperList)
        {
            DataSet detailTamperData = CreateDetailTamperDataSet();
            try
            {
                string DataUnavailable = "";
                string OccRVoltage = string.Empty;
                string ResRVoltage = string.Empty;
                string OccYVoltage = string.Empty;
                string ResYVoltage = string.Empty;
                string OccBVoltage = string.Empty;
                string ResBVoltage = string.Empty;
                string OccPhaseVoltage = string.Empty;
                string ResPhaseVoltage = string.Empty;
                string OccRCurrent = string.Empty;
                string ResRCurrent = string.Empty;
                string OccYCurrent = string.Empty;
                string ResYCurrent = string.Empty;
                string OccBCurrent = string.Empty;
                string ResBCurrent = string.Empty;
                string OccPhaseCurrent = string.Empty;
                string ResPhaseCurrent = string.Empty;
                string OccNeutralCurrent = string.Empty; // Story - 349654 - Neutral current in Tamper
                string ResNeutralCurrent = string.Empty;

                string OccHighNeutralCurrent = string.Empty; // pradipta_neu
                string ResHighNeutralCurrent = string.Empty;

                string OcckWr = string.Empty; // pradipta_neu
                string ReskWr = string.Empty;

                string OcckWy = string.Empty; // pradipta_neu
                string ReskWy = string.Empty;

                string OcckWb = string.Empty; // pradipta_neu
                string ReskWb = string.Empty;

                string OcckVAr = string.Empty; // pradipta_neu
                string ReskVAr = string.Empty;

                string OcckVAy = string.Empty; // pradipta_neu
                string ReskVAy = string.Empty;

                string OcckVAb = string.Empty; // pradipta_neu
                string ReskVAb = string.Empty;


                string OccRPF = string.Empty;
                string ResRPF = string.Empty;
                string OccYPF = string.Empty;
                string ResYPF = string.Empty;
                string OccBPF = string.Empty;
                string ResBPF = string.Empty;
                string OccPF = string.Empty;
                string ResPF = string.Empty;
                string OccKwh = string.Empty;
                string ResKwh = string.Empty;
                string OccKVAh = string.Empty;
                string ResKVAh = string.Empty;
                string OccDateTime = string.Empty;
                string ResDateTime = string.Empty;
                string OcckWhImport = string.Empty;
                string OcckWhExport = string.Empty;
                string ReskWhImport = string.Empty;
                string ReskWhExport = string.Empty;
                string OcckVAhImport = string.Empty;
                string OcckVAhExport = string.Empty;
                string ReskVAhImport = string.Empty;
                string ReskVAhExport = string.Empty;
                string OcckvarhLag = string.Empty;
                string ReskvarhLag = string.Empty;
                string OcckvarhLead = string.Empty;
                string ReskvarhLead = string.Empty;

                List<long> lstTargetTamperIds = new List<long> { 1, 3, 5, 7, 9, 11, 13, 47, 49, 51, 53, 55, 57, 59, 61, 63, 65, 67, 69, 91, 93, 101, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 184, 185, 186, 187, 188, 189, 190, 191, 192, 194, 200, 201, 203, 205, 207, 243, 245, 247, 249, 251, 256, 297, 301, 703, 704 };   //SarkarA code change start 20180308 // add Current Mismatch Tamper/end
                List<long> lstTamperIdsWithNoRestoration = new List<long> { 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 167, 168, 251, 190, 191, 192, 189, 184, 185, 186, 187, 188, 194, 200, 256 };


                foreach (long TamperID in lstTargetTamperIds)
                {
                    List<DLMS650TamperEntity> detailedTamperOccData = null;
                    List<DLMS650TamperEntity> detailedTamperResData = null;
                    if (lstTamperIdsWithNoRestoration.Contains(TamperID))
                    {
                        detailedTamperOccData = GetTamperDetailByTamperId(TamperList, TamperID);
                    }
                    else
                    {
                        detailedTamperOccData = GetTamperDetailByTamperId(TamperList, TamperID);
                        detailedTamperResData = GetTamperDetailByTamperId(TamperList, TamperID + 1);
                    }

                    if (detailedTamperOccData != null && detailedTamperOccData.Count > 0 && detailedTamperResData != null && detailedTamperResData.Count > 0)
                    {
                        int intOccRowCount = detailedTamperOccData.Count;
                        int intResRowCount = detailedTamperResData.Count;
                        int intMaxRows = intOccRowCount;// (intOccRowCount > intResRowCount) ? intOccRowCount : intResRowCount;
                        string strTamperCode = string.Empty;
                        //for (int i = 0; i < intMaxRows; i++)
                        int occCounter = 0;
                        int resCounter = 0;

                        //This condition applies when Occurance count is less than restoration count
                        if (intOccRowCount < intResRowCount)
                        {
                            //To prevent display of data where restoration is earlier that occurence : RollOver case .
                            if (Convert.ToInt64(detailedTamperResData[resCounter].DateTimeEvent)
                                < Convert.ToInt64(detailedTamperOccData[occCounter].DateTimeEvent))
                            {
                                /* User Story: 447861 Tamper Occurrance and Restoration issue in Tamper Report
                                 * Remove the first Restoration from the tamper restoration dataset */
                                detailedTamperResData.RemoveAt(resCounter);
                            }
                        }

                        while (occCounter < intMaxRows)
                        {

                            DataRow detailTamperRow = detailTamperData.Tables[0].NewRow();
                            strTamperCode = detailedTamperOccData[occCounter].EventCode.ToString();
                            detailTamperRow["Description"] = DataUnavailable;


                            if (occCounter <= intResRowCount - 1 && resCounter <= intOccRowCount - 1 && detailedTamperResData.Count - 1 >= resCounter
                                && detailedTamperOccData.Count - 1 >= occCounter)
                            {
                                //To prevent display of data where restoration is earlier that occurence : RollOver case.
                                if (Convert.ToInt64(detailedTamperResData[resCounter].DateTimeEvent)
                                    < Convert.ToInt64(detailedTamperOccData[occCounter].DateTimeEvent))
                                {
                                    resCounter++;
                                    continue;
                                }
                                //Filter only those tampers where corresponding phase current is greater than 0.
                                //if (chkVoltageTamperHavingCurrent.Checked)
                                //{
                                //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData[occCounter].CurrentIR) == 0)
                                //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData[occCounter].CurrentIY) == 0)
                                //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData[occCounter].CurrentIB) == 0))
                                //    {
                                //        occCounter++;
                                //        resCounter++;
                                //        continue;
                                //    }

                                //}
                                OccRVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVRN);
                                ResRVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVRN);
                                OccYVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVYN);
                                ResYVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVYN);
                                OccBVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVBN);
                                ResBVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVBN);
                                OccPhaseVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseVoltage);
                                ResPhaseVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].PhaseVoltage);
                                OccRCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIR);
                                ResRCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIR);
                                OccYCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIY);
                                ResYCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIY);
                                OccBCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIB);
                                ResBCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIB);
                                OccPhaseCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseCurrent);
                                ResPhaseCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].PhaseCurrent);
                                OccNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].NeutralCurrent); // Story - 349654 - Neutral current in Tamper
                                ResNeutralCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].NeutralCurrent);

                                OccHighNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].HighNeutralCurrent ); // pradipta_neu
                                ResHighNeutralCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].HighNeutralCurrent);


                                OccRPF = detailedTamperOccData[occCounter].PowerFactorRphase;
                                ResRPF = detailedTamperResData[resCounter].PowerFactorRphase;
                                OccYPF = detailedTamperOccData[occCounter].PowerFactorYphase;
                                ResYPF = detailedTamperResData[resCounter].PowerFactorYphase;
                                OccBPF = detailedTamperOccData[occCounter].PowerFactorBphase;
                                ResBPF = detailedTamperResData[resCounter].PowerFactorBphase;
                                OccPF = detailedTamperOccData[occCounter].TotalPowerFactor;
                                ResPF = detailedTamperResData[resCounter].TotalPowerFactor;
                                OccKwh = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWh);
                                ResKwh = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykWh);
                                OccKVAh = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAh);
                                ResKVAh = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykVAh);

                                OcckWhImport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWhImport);

                                OcckWhExport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWhExport);

                                ReskWhImport = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykWhImport);

                                ReskWhExport = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykWhExport);

                                OcckVAhImport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAhImport);

                                OcckVAhExport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAhExport);

                                ReskVAhImport = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykVAhImport);

                                ReskVAhExport = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykVAhExport);

                                OcckvarhLag = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykvarhLag);
                                ReskvarhLag = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykvarhLag);

                                OcckvarhLead = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykvarhLead);
                                ReskvarhLead = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykvarhLead);




                                // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                                //if (strTamperCode == "101")
                                //{
                                //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData[occCounter].Time Stamp"]));
                                //}
                                //else
                                //{
                                //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData[occCounter].Time Stamp"]));
                                //}
                                OccDateTime = detailedTamperOccData[occCounter].DateTimeEvent.ToString();
                                ResDateTime = detailedTamperResData[resCounter].DateTimeEvent.ToString();

                                detailTamperRow["OccEvent"] = detailedTamperOccData[occCounter].EventCode;
                                detailTamperRow["ResEvent"] = detailedTamperResData[resCounter].EventCode;
                                detailTamperRow["OccDateTime"] = OccDateTime;
                                detailTamperRow["ResDateTime"] = ResDateTime;
                                detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DataUnavailable : OccRVoltage;
                                detailTamperRow["ResRVoltage"] = ResRVoltage == string.Empty ? DataUnavailable : ResRVoltage;
                                detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DataUnavailable : OccYVoltage;
                                detailTamperRow["ResYVoltage"] = ResYVoltage == string.Empty ? DataUnavailable : ResYVoltage;
                                detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DataUnavailable : OccBVoltage;
                                detailTamperRow["ResBVoltage"] = ResBVoltage == string.Empty ? DataUnavailable : ResBVoltage;

                                detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DataUnavailable : OccPhaseVoltage;
                                detailTamperRow["ResPhaseVoltage"] = ResPhaseCurrent == string.Empty ? DataUnavailable : ResPhaseVoltage;

                                detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DataUnavailable : OccRCurrent;
                                detailTamperRow["ResRCurrent"] = ResRCurrent == string.Empty ? DataUnavailable : ResRCurrent;
                                detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DataUnavailable : OccYCurrent;
                                detailTamperRow["ResYCurrent"] = ResYCurrent == string.Empty ? DataUnavailable : ResYCurrent;
                                detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DataUnavailable : OccBCurrent;
                                detailTamperRow["ResBCurrent"] = ResBCurrent == string.Empty ? DataUnavailable : ResBCurrent;

                                detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DataUnavailable : OccPhaseCurrent;
                                detailTamperRow["ResPhaseCurrent"] = ResPhaseCurrent == string.Empty ? DataUnavailable : ResPhaseCurrent;

                                detailTamperRow["OccNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DataUnavailable : OccNeutralCurrent; // Story - 349654 - Neutral current in Tamper
                                detailTamperRow["ResNeutralCurrent"] = ResNeutralCurrent == string.Empty ? DataUnavailable : ResNeutralCurrent;

                                detailTamperRow["OccHighNeutralCurrent"] = OccHighNeutralCurrent == string.Empty ? DataUnavailable : OccHighNeutralCurrent; //pradipta_neu
                                detailTamperRow["ResHighNeutralCurrent"] = ResHighNeutralCurrent == string.Empty ? DataUnavailable : ResHighNeutralCurrent;


                                detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DataUnavailable : OccRPF;
                                detailTamperRow["ResRPF"] = ResRPF == string.Empty ? DataUnavailable : ResRPF;
                                detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DataUnavailable : OccYPF;
                                detailTamperRow["ResYPF"] = ResYPF == string.Empty ? DataUnavailable : ResYPF;
                                detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DataUnavailable : OccBPF;
                                detailTamperRow["ResBPF"] = ResBPF == string.Empty ? DataUnavailable : ResBPF;
                                detailTamperRow["OccPF"] = OccPF == string.Empty ? DataUnavailable : OccPF;
                                detailTamperRow["ResPF"] = ResPF == string.Empty ? DataUnavailable : ResPF;

                                detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DataUnavailable : OccKwh;
                                detailTamperRow["ResKwh"] = ResKwh == string.Empty ? DataUnavailable : ResKwh;
                                detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DataUnavailable : OccKVAh;
                                detailTamperRow["ResKVAh"] = ResKVAh == string.Empty ? DataUnavailable : ResKVAh;

                                detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DataUnavailable : OcckWhImport;
                                detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DataUnavailable : OcckWhExport;
                                detailTamperRow["ReskWhImport"] = ReskWhImport == string.Empty ? DataUnavailable : ReskWhImport;
                                detailTamperRow["ReskWhExport"] = ReskWhExport == string.Empty ? DataUnavailable : ReskWhExport;
                                detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DataUnavailable : OcckVAhImport;
                                detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DataUnavailable : OcckVAhExport;
                                detailTamperRow["ReskVAhImport"] = ReskVAhImport == string.Empty ? DataUnavailable : ReskVAhImport;
                                detailTamperRow["ReskVAhExport"] = ReskVAhExport == string.Empty ? DataUnavailable : ReskVAhExport;

                                detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DataUnavailable : OcckvarhLag;
                                detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DataUnavailable : OcckvarhLead;
                                detailTamperRow["ReskvarhLag"] = ReskvarhLag == string.Empty ? DataUnavailable : ReskvarhLag;
                                detailTamperRow["ReskvarhLead"] = ReskvarhLead == string.Empty ? DataUnavailable : ReskvarhLead;


                            }
                            else if (occCounter <= intOccRowCount)
                            {
                                //Filter only those tampers where corresponding phase current is greater than 0.
                                //if (chkVoltageTamperHavingCurrent.Checked)
                                //{
                                //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData[occCounter]["CurrentIR"]) == 0)
                                //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData[occCounter]["CurrentIY"]) == 0)
                                //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData[occCounter]["CurrentIB"]) == 0))
                                //    {
                                //        occCounter++;
                                //        resCounter++;
                                //        continue;
                                //    }

                                //}

                                OccRVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVRN);
                                OccYVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVYN);
                                OccBVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].VoltageVBN);
                                OccPhaseVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseVoltage);
                                OccRCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIR);
                                OccYCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIY);
                                OccBCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].CurrentIB);
                                OccPhaseCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseCurrent);
                                OccNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].NeutralCurrent); // Story - 349654 - Neutral current in Tamper

                                OccHighNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].HighNeutralCurrent); // pradipta_neu


                                OccRPF = detailedTamperOccData[occCounter].PowerFactorRphase;
                                OccYPF = detailedTamperOccData[occCounter].PowerFactorYphase;
                                OccBPF = detailedTamperOccData[occCounter].PowerFactorBphase;
                                OccPF = detailedTamperOccData[occCounter].TotalPowerFactor;
                                OccKwh = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWh);
                                OccKVAh = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAh);
                                OcckWhImport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWhImport);
                                OcckWhExport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykWhExport);
                                OcckVAhImport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAhImport);
                                OcckVAhExport = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykVAhExport);
                                OcckvarhLag = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykvarhLag);
                                OcckvarhLead = GetDataWithoutUnit(detailedTamperOccData[occCounter].CumulativeEnergykvarhLead);

                                // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                                //if (strTamperCode == "101")
                                //{
                                //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData[occCounter].Time Stamp"]));
                                //}
                                //else
                                //{
                                //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData[occCounter].Time Stamp"]));
                                //}
                                OccDateTime = detailedTamperOccData[occCounter].DateTimeEvent.ToString();
                                detailTamperRow["OccEvent"] = detailedTamperOccData[occCounter].EventCode;
                                detailTamperRow["ResEvent"] = DataUnavailable;
                                detailTamperRow["OccDateTime"] = OccDateTime;
                                detailTamperRow["ResDateTime"] = DataUnavailable;
                                detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DataUnavailable : OccRVoltage;
                                detailTamperRow["ResRVoltage"] = DataUnavailable;
                                detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DataUnavailable : OccYVoltage;
                                detailTamperRow["ResYVoltage"] = DataUnavailable;
                                detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DataUnavailable : OccBVoltage;
                                detailTamperRow["ResBVoltage"] = DataUnavailable;
                                detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DataUnavailable : OccPhaseVoltage;
                                detailTamperRow["ResPhaseVoltage"] = DataUnavailable;

                                detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DataUnavailable : OccRCurrent;
                                detailTamperRow["ResRCurrent"] = DataUnavailable;
                                detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DataUnavailable : OccYCurrent;
                                detailTamperRow["ResYCurrent"] = DataUnavailable;
                                detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DataUnavailable : OccBCurrent;
                                detailTamperRow["ResBCurrent"] = DataUnavailable;
                                detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DataUnavailable : OccPhaseCurrent;
                                detailTamperRow["ResPhaseCurrent"] = DataUnavailable;
                                detailTamperRow["OccNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DataUnavailable : OccNeutralCurrent;// Story - 349654 - Neutral current in Tamper
                                detailTamperRow["ResNeutralCurrent"] = DataUnavailable;

                                detailTamperRow["OccHighNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DataUnavailable : OccHighNeutralCurrent;//pradipta_neu
                                detailTamperRow["ResHighNeutralCurrent"] = DataUnavailable;


                                detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DataUnavailable : OccRPF;
                                detailTamperRow["ResRPF"] = DataUnavailable;
                                detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DataUnavailable : OccYPF;
                                detailTamperRow["ResYPF"] = DataUnavailable;
                                detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DataUnavailable : OccBPF;
                                detailTamperRow["ResBPF"] = DataUnavailable;
                                detailTamperRow["OccPF"] = OccPF == string.Empty ? DataUnavailable : OccPF;
                                detailTamperRow["ResPF"] = DataUnavailable;
                                detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DataUnavailable : OccKwh;
                                detailTamperRow["ResKwh"] = DataUnavailable;
                                detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DataUnavailable : OccKVAh;
                                detailTamperRow["ResKVah"] = DataUnavailable;


                                detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DataUnavailable : OcckWhImport;
                                detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DataUnavailable : OcckWhExport;
                                detailTamperRow["ReskWhImport"] = DataUnavailable;
                                detailTamperRow["ReskWhExport"] = DataUnavailable;
                                detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DataUnavailable : OcckVAhImport;
                                detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DataUnavailable : OcckVAhExport;
                                detailTamperRow["ReskVAhImport"] = DataUnavailable;
                                detailTamperRow["ReskVAhExport"] = DataUnavailable;
                                detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DataUnavailable : OcckvarhLag;
                                detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DataUnavailable : OcckvarhLead;
                                detailTamperRow["ReskvarhLag"] = DataUnavailable;
                                detailTamperRow["ReskvarhLead"] = DataUnavailable;
                            }
                            else
                            {

                                ResRVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVRN);
                                ResYVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVYN);
                                ResBVoltage = GetDataWithoutUnit(detailedTamperResData[resCounter].VoltageVBN);
                                ResPhaseVoltage = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseVoltage);
                                ResRCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIR);
                                ResYCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIY);
                                ResBCurrent = GetDataWithoutUnit(detailedTamperResData[resCounter].CurrentIB);
                                ResPhaseCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].PhaseCurrent);
                                ResNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].NeutralCurrent); // Story - 349654 - Neutral current in Tamper

                                ResHighNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[occCounter].HighNeutralCurrent); // pradipta_neu


                                ResRPF = detailedTamperResData[resCounter].PowerFactorRphase;
                                ResYPF = detailedTamperResData[resCounter].PowerFactorYphase;
                                ResBPF = detailedTamperResData[resCounter].PowerFactorBphase;
                                ResPF = detailedTamperResData[resCounter].TotalPowerFactor;
                                ResKwh = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykWh);
                                ResKVAh = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykVAh);
                                ResDateTime = ResDateTime = detailedTamperResData[resCounter].DateTimeEvent.ToString();
                                ReskvarhLag = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykvarhLag);
                                ReskvarhLead = GetDataWithoutUnit(detailedTamperResData[resCounter].CumulativeEnergykvarhLead);

                                detailTamperRow["OccEvent"] = DataUnavailable;
                                detailTamperRow["ResEvent"] = detailedTamperResData[resCounter].EventCode;
                                detailTamperRow["OccDateTime"] = DataUnavailable;
                                detailTamperRow["ResDateTime"] = ResDateTime;
                                detailTamperRow["OccRVoltage"] = DataUnavailable;
                                detailTamperRow["ResRVoltage"] = ResRVoltage == string.Empty ? DataUnavailable : ResRVoltage;
                                detailTamperRow["OccYVoltage"] = DataUnavailable;
                                detailTamperRow["ResYVoltage"] = ResYVoltage == string.Empty ? DataUnavailable : ResYVoltage;
                                detailTamperRow["OccBVoltage"] = DataUnavailable;
                                detailTamperRow["ResBVoltage"] = ResBVoltage == string.Empty ? DataUnavailable : ResBVoltage;
                                detailTamperRow["OccPhaseVoltage"] = DataUnavailable;
                                detailTamperRow["ResPhaseVoltage"] = ResPhaseVoltage == string.Empty ? DataUnavailable : ResPhaseVoltage;
                                detailTamperRow["OccRCurrent"] = DataUnavailable;
                                detailTamperRow["ResRCurrent"] = ResRCurrent == string.Empty ? DataUnavailable : ResRCurrent;
                                detailTamperRow["OccYCurrent"] = DataUnavailable;
                                detailTamperRow["ResYCurrent"] = ResYCurrent == string.Empty ? DataUnavailable : ResYCurrent;
                                detailTamperRow["OccBCurrent"] = DataUnavailable;
                                detailTamperRow["ResBCurrent"] = ResBCurrent == string.Empty ? DataUnavailable : ResBCurrent;
                                detailTamperRow["OccPhaseCurrent"] = DataUnavailable;
                                detailTamperRow["ResPhaseCurrent"] = ResPhaseCurrent == string.Empty ? DataUnavailable : ResPhaseCurrent;
                                detailTamperRow["OccNeutralCurrent"] = DataUnavailable;// Story - 349654 - Neutral current in Tamper
                                detailTamperRow["ResNeutralCurrent"] = ResNeutralCurrent == string.Empty ? DataUnavailable : ResNeutralCurrent;

                                detailTamperRow["OccHighNeutralCurrent"] = DataUnavailable;// pradipta_neu
                                detailTamperRow["ResHighNeutralCurrent"] = ResHighNeutralCurrent == string.Empty ? DataUnavailable : ResHighNeutralCurrent;


                                detailTamperRow["OccRPF"] = DataUnavailable;
                                detailTamperRow["ResRPF"] = ResRPF == string.Empty ? DataUnavailable : ResRPF;
                                detailTamperRow["OccYPF"] = DataUnavailable;
                                detailTamperRow["ResYPF"] = ResYPF == string.Empty ? DataUnavailable : ResYPF;
                                detailTamperRow["OccBPF"] = DataUnavailable;
                                detailTamperRow["ResBPF"] = ResBPF == string.Empty ? DataUnavailable : ResBPF;
                                detailTamperRow["OccPF"] = DataUnavailable;
                                detailTamperRow["ResPF"] = ResPF == string.Empty ? DataUnavailable : ResPF;
                                detailTamperRow["OccKwh"] = DataUnavailable;
                                detailTamperRow["ResKwh"] = ResKwh == string.Empty ? DataUnavailable : ResKwh;
                                detailTamperRow["OccKVAh"] = DataUnavailable;
                                detailTamperRow["ResKVAh"] = ResKVAh == string.Empty ? DataUnavailable : ResKVAh;


                                detailTamperRow["OcckWhImport"] = DataUnavailable;
                                detailTamperRow["OcckWhExport"] = DataUnavailable;
                                detailTamperRow["ReskWhImport"] = ReskWhImport == string.Empty ? DataUnavailable : ReskWhImport;
                                detailTamperRow["ReskWhExport"] = ReskWhExport == string.Empty ? DataUnavailable : ReskWhExport;
                                detailTamperRow["OcckVAhImport"] = DataUnavailable;
                                detailTamperRow["OcckVAhExport"] = DataUnavailable;
                                detailTamperRow["ReskVAhImport"] = ReskVAhImport == string.Empty ? DataUnavailable : ReskVAhImport;
                                detailTamperRow["ReskVAhExport"] = ReskVAhExport == string.Empty ? DataUnavailable : ReskVAhExport;

                                detailTamperRow["OcckvarhLag"] = DataUnavailable;
                                detailTamperRow["OcckvarhLead"] = DataUnavailable;
                                detailTamperRow["ReskvarhLag"] = ReskvarhLag == string.Empty ? DataUnavailable : ReskvarhLag;
                                detailTamperRow["ReskvarhLead"] = ReskvarhLead == string.Empty ? DataUnavailable : ReskvarhLead;
                            }

                            detailTamperData.Tables[0].Rows.Add(detailTamperRow);
                            occCounter++;
                            resCounter++;
                        }

                    }
                    else if (detailedTamperOccData != null && detailedTamperOccData.Count > 0)
                    {
                        int intOccRowCount = detailedTamperOccData.Count;
                        string strTamperCode = string.Empty;
                        for (int i = 0; i < intOccRowCount; i++)
                        {
                            DataRow detailTamperRow = detailTamperData.Tables[0].NewRow();
                            strTamperCode = detailedTamperOccData[i].EventCode.ToString();
                            detailTamperRow["Description"] = DataUnavailable;

                            //Filter only those tampers where corresponding phase current is greater than 0.
                            //if (chkVoltageTamperHavingCurrent.Checked)
                            //{
                            //    if ((intTamperIds == 1 && Convert.ToDouble(detailedTamperOccData[i]["CurrentIR"]) == 0)
                            //        || (intTamperIds == 3 && Convert.ToDouble(detailedTamperOccData[i]["CurrentIY"]) == 0)
                            //        || (intTamperIds == 5 && Convert.ToDouble(detailedTamperOccData[i]["CurrentIB"]) == 0))
                            //    {
                            //        continue;
                            //    }

                            //}
                            OccRVoltage = GetDataWithoutUnit(detailedTamperOccData[i].VoltageVRN);
                            OccYVoltage = GetDataWithoutUnit(detailedTamperOccData[i].VoltageVYN);
                            OccBVoltage = GetDataWithoutUnit(detailedTamperOccData[i].VoltageVBN);
                            OccPhaseVoltage = GetDataWithoutUnit(detailedTamperOccData[i].PhaseVoltage);
                            OccRCurrent = GetDataWithoutUnit(detailedTamperOccData[i].CurrentIR);
                            OccYCurrent = GetDataWithoutUnit(detailedTamperOccData[i].CurrentIY);
                            OccBCurrent = GetDataWithoutUnit(detailedTamperOccData[i].CurrentIB);
                            OccPhaseCurrent = GetDataWithoutUnit(detailedTamperOccData[i].PhaseCurrent);
                            OccNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[i].NeutralCurrent);// Story - 349654 - Neutral current in Tamper

                            OccHighNeutralCurrent = GetDataWithoutUnit(detailedTamperOccData[i].HighNeutralCurrent);// pradipta_neu


                            OccRPF = detailedTamperOccData[i].PowerFactorRphase;
                            OccYPF = detailedTamperOccData[i].PowerFactorYphase;
                            OccBPF = detailedTamperOccData[i].PowerFactorBphase;
                            OccPF = detailedTamperOccData[i].TotalPowerFactor;
                            OccKwh = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykWh);
                            OccKVAh = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykVAh);

                            OcckWhImport = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykWhImport);
                            OcckVAhImport = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykVAhImport);
                            OcckWhExport = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykWhExport);
                            OcckVAhExport = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykVAhExport);

                            OcckvarhLag = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykvarhLag);
                            OcckvarhLead = GetDataWithoutUnit(detailedTamperOccData[i].CumulativeEnergykvarhLead);


                            // WB utitlity requirement temporary check(substract five minute from power failure temper occurrence DateTime) removed
                            //if (strTamperCode == "101")
                            //{
                            //    OccDateTime = DateUtility.GetTamperOccurDateTimeMinusFiveMinute(Convert.ToInt64(detailedTamperOccData[i].Time Stamp"]));
                            //}
                            //else
                            //{
                            //    OccDateTime = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(detailedTamperOccData[i].Time Stamp"]));
                            //}
                            OccDateTime = detailedTamperOccData[i].DateTimeEvent.ToString();

                            detailTamperRow["OccEvent"] = detailedTamperOccData[i].EventCode;
                            detailTamperRow["ResEvent"] = DataUnavailable;
                            detailTamperRow["OccDateTime"] = OccDateTime;
                            detailTamperRow["ResDateTime"] = DataUnavailable;
                            detailTamperRow["OccRVoltage"] = OccRVoltage == string.Empty ? DataUnavailable : OccRVoltage;
                            detailTamperRow["ResRVoltage"] = DataUnavailable;
                            detailTamperRow["OccYVoltage"] = OccYVoltage == string.Empty ? DataUnavailable : OccYVoltage;
                            detailTamperRow["ResYVoltage"] = DataUnavailable;
                            detailTamperRow["OccBVoltage"] = OccBVoltage == string.Empty ? DataUnavailable : OccBVoltage;
                            detailTamperRow["ResBVoltage"] = DataUnavailable;
                            detailTamperRow["OccPhaseVoltage"] = OccPhaseVoltage == string.Empty ? DataUnavailable : OccPhaseVoltage;
                            detailTamperRow["ResPhaseVoltage"] = DataUnavailable;
                            detailTamperRow["OccRCurrent"] = OccRCurrent == string.Empty ? DataUnavailable : OccRCurrent;
                            detailTamperRow["ResRCurrent"] = DataUnavailable;
                            detailTamperRow["OccYCurrent"] = OccYCurrent == string.Empty ? DataUnavailable : OccYCurrent;
                            detailTamperRow["ResYCurrent"] = DataUnavailable;
                            detailTamperRow["OccBCurrent"] = OccBCurrent == string.Empty ? DataUnavailable : OccBCurrent;
                            detailTamperRow["ResBCurrent"] = DataUnavailable;
                            detailTamperRow["OccPhaseCurrent"] = OccPhaseCurrent == string.Empty ? DataUnavailable : OccPhaseCurrent;
                            detailTamperRow["ResPhaseCurrent"] = DataUnavailable;
                            detailTamperRow["OccNeutralCurrent"] = OccNeutralCurrent == string.Empty ? DataUnavailable : OccNeutralCurrent; // Story - 349654 - Neutral current in Tamper
                            detailTamperRow["ResNeutralCurrent"] = DataUnavailable;

                            detailTamperRow["OccHighNeutralCurrent"] = OccHighNeutralCurrent == string.Empty ? DataUnavailable : OccHighNeutralCurrent; //pradipta_neu
                            detailTamperRow["ResHighNeutralCurrent"] = DataUnavailable;


                            detailTamperRow["OccRPF"] = OccRPF == string.Empty ? DataUnavailable : OccRPF;
                            detailTamperRow["ResRPF"] = DataUnavailable;
                            detailTamperRow["OccYPF"] = OccYPF == string.Empty ? DataUnavailable : OccYPF;
                            detailTamperRow["ResYPF"] = DataUnavailable;
                            detailTamperRow["OccBPF"] = OccBPF == string.Empty ? DataUnavailable : OccBPF;
                            detailTamperRow["ResBPF"] = DataUnavailable;
                            detailTamperRow["OccPF"] = OccPF == string.Empty ? DataUnavailable : OccPF;
                            detailTamperRow["ResPF"] = DataUnavailable;
                            detailTamperRow["OccKwh"] = OccKwh == string.Empty ? DataUnavailable : OccKwh;
                            detailTamperRow["ResKwh"] = DataUnavailable;
                            detailTamperRow["OccKVAh"] = OccKVAh == string.Empty ? DataUnavailable : OccKVAh;
                            detailTamperRow["ResKVAh"] = DataUnavailable;

                            detailTamperRow["OcckWhImport"] = OcckWhImport == string.Empty ? DataUnavailable : OcckWhImport;
                            detailTamperRow["ReskWhImport"] = DataUnavailable;
                            detailTamperRow["OcckVAhImport"] = OcckVAhImport == string.Empty ? DataUnavailable : OcckVAhImport;
                            detailTamperRow["ReskVAhImport"] = DataUnavailable;

                            detailTamperRow["OcckWhExport"] = OcckWhExport == string.Empty ? DataUnavailable : OcckWhExport;
                            detailTamperRow["ReskWhExport"] = DataUnavailable;
                            detailTamperRow["OcckVAhExport"] = OcckVAhExport == string.Empty ? DataUnavailable : OcckVAhExport;
                            detailTamperRow["ReskVAhExport"] = DataUnavailable;

                            detailTamperRow["OcckvarhLag"] = OcckvarhLag == string.Empty ? DataUnavailable : OcckvarhLag;
                            detailTamperRow["ReskvarhLag"] = DataUnavailable;
                            detailTamperRow["OcckvarhLead"] = OcckvarhLead == string.Empty ? DataUnavailable : OcckvarhLead;
                            detailTamperRow["ReskvarhLead"] = DataUnavailable;


                            detailTamperData.Tables[0].Rows.Add(detailTamperRow);
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {

                logger.Log(LOGLEVELS.Error, "GetDetailTamperData(List<DLMS650TamperEntity> TamperList)", ex);
            }
            return detailTamperData;
        }



        #region Export


        public void ExportFile()
        {
            try
            {
                Updatestatus("Exporting Data...", Color.LightBlue);


                List<ProfileData> allData = ParseEntity(FileText);

                List<ProfileData> billingData = allData.Where(item => item.ProfileId == (int)ProfileId.Billing).ToList() as List<ProfileData>;
                List<ProfileData> generalData = allData.Where(item => item.ProfileId == (int)ProfileId.NamePlate).ToList() as List<ProfileData>;
                List<ProfileData> namePlateData = allData.Where(item => item.ProfileId == (int)ProfileId.NamePlateProfile).ToList() as List<ProfileData>;
                List<ProfileData> tamperData = allData.Where(item => item.ProfileId == (int)ProfileId.Tamper).ToList() as List<ProfileData>;
                List<ProfileData> instantData = allData.Where(item => item.ProfileId == (int)ProfileId.Instant).ToList() as List<ProfileData>;
                List<ProfileData> loadSurveyData = allData.Where(item => item.ProfileId == (int)ProfileId.LoadSurvey).ToList() as List<ProfileData>;
                List<ProfileData> midnightData = allData.Where(item => item.ProfileId == (int)ProfileId.Midnight).ToList() as List<ProfileData>;
                List<ProfileData> phasorData = allData.Where(item => item.ProfileId == (int)ProfileId.Phasor).ToList() as List<ProfileData>;
                //List<ProfileData> fraudEnergyData = allData.Where(item => item.ProfileId == (int)ProfileId.FraudEnergy).ToList() as List<ProfileData>;
                //List<ProfileData> anamolyData = allData.Where(item => item.ProfileId == (int)ProfileId.Anomaly).ToList() as List<ProfileData>;
                //List<ProfileData> kvahSelectionData = allData.Where(item => item.ProfileId == (int)ProfileId.KvahSelection).ToList() as List<ProfileData>;
                //List<ProfileData> rs232LockData = allData.Where(item => item.ProfileId == (int)ProfileId.RS232LockUnlock).ToList() as List<ProfileData>;
                //List<ProfileData> billingTypeData = allData.Where(item => item.ProfileId == (int)ProfileId.BillingType).ToList() as List<ProfileData>;
                //List<ProfileData> billingMonthTypeData = allData.Where(item => item.ProfileId == (int)ProfileId.BillingMonthType).ToList() as List<ProfileData>;
                //List<ProfileData> pushDisplayParameterData = allData.Where(item => item.ProfileId == (int)ProfileId.PushDisplayParameter).ToList() as List<ProfileData>;
                //List<ProfileData> scrollDisplayParameterData = allData.Where(item => item.ProfileId == (int)ProfileId.ScrollDisplyParameter).ToList() as List<ProfileData>;
                //List<ProfileData> highResolutionDisplayParameterData = allData.Where(item => item.ProfileId == (int)ProfileId.HighResolutionDisplayParameter).ToList() as List<ProfileData>;
                //List<ProfileData> displayTimeoutData = allData.Where(item => item.ProfileId == (int)ProfileId.DisplayTimeoutParameter).ToList() as List<ProfileData>;
                //List<ProfileData> dipData = allData.Where(item => item.ProfileId == (int)ProfileId.DIP).ToList() as List<ProfileData>;
                //List<ProfileData> resetLockOutData = allData.Where(item => item.ProfileId == (int)ProfileId.ResetLockOutDays).ToList() as List<ProfileData>;
                //List<ProfileData> autoLockData = allData.Where(item => item.ProfileId == (int)ProfileId.AutoLock).ToList() as List<ProfileData>;
                //List<ProfileData> passiveSeasonProfile = allData.Where(item => item.ProfileId == (int)ProfileId.PassiveSeasonProfile).ToList() as List<ProfileData>;
                //List<ProfileData> passiveWeekProfile = allData.Where(item => item.ProfileId == (int)ProfileId.PassiveWeekProfile).ToList() as List<ProfileData>;
                //List<ProfileData> passiveDayProfile = allData.Where(item => item.ProfileId == (int)ProfileId.PassiveDayProfile).ToList() as List<ProfileData>;
                //List<ProfileData> activeDayProfile = allData.Where(item => item.ProfileId == (int)ProfileId.ActiveDayProfile).ToList() as List<ProfileData>;
                //List<ProfileData> specialDayProfile = allData.Where(item => item.ProfileId == (int)ProfileId.SpecialDayProfileSmartMeter).ToList() as List<ProfileData>;
                //List<ProfileData> activeSeasonDayProfile = allData.Where(item => item.ProfileId == (int)ProfileId.ActiveDayProfile).ToList() as List<ProfileData>;
                //List<ProfileData> activeWeekDayProfile = allData.Where(item => item.ProfileId == (int)ProfileId.ActiveWeekProfile).ToList() as List<ProfileData>;
                //List<ProfileData> activationDate = allData.Where(item => item.ProfileId == (int)ProfileId.ActivationDate).ToList() as List<ProfileData>;
                //List<ProfileData> rtcData = allData.Where(item => item.ProfileId == (int)ProfileId.RTC).ToList() as List<ProfileData>;
                //List<ProfileData> CTRatioData = allData.Where(item => item.ProfileId == (int)ProfileId.CTRatio).ToList() as List<ProfileData>;
                //List<ProfileData> PTRatioData = allData.Where(item => item.ProfileId == (int)ProfileId.PTRatio).ToList() as List<ProfileData>;
                //List<ProfileData> lsipData = allData.Where(item => item.ProfileId == (int)ProfileId.SIP).ToList() as List<ProfileData>;
                //List<ProfileData> dipWithIPData = allData.Where(item => item.ProfileId == (int)ProfileId.DIP).ToList() as List<ProfileData>;
                //List<ProfileData> manualBillingData = allData.Where(item => item.ProfileId == (int)ProfileId.ManualBilling).ToList() as List<ProfileData>;
                //List<ProfileData> softwareBillingData = allData.Where(item => item.ProfileId == (int)ProfileId.SoftwareBilling).ToList() as List<ProfileData>;
                //List<ProfileData> disconnectControlData = allData.Where(item => item.ProfileId == (int)ProfileId.DisconnectControl).ToList() as List<ProfileData>;
                //List<ProfileData> loadControlData = allData.Where(item => item.ProfileId == (int)ProfileId.LoadControl).ToList() as List<ProfileData>;
                //List<ProfileData> loadControl1PSmartData = allData.Where(item => item.ProfileId == (int)ProfileId.LoadControl1PSmartMeter).ToList() as List<ProfileData>;
                //List<ProfileData> RS485Data = allData.Where(item => item.ProfileId == (int)ProfileId.RS485).ToList() as List<ProfileData>;


                General mapperGeneral = new General();
                Instantaneous mapperInstant = new Instantaneous();
                LoadSurvey mapperLoadSurevy = new LoadSurvey();
                DailyLoadProfile mapperMidnight = new DailyLoadProfile();
                BillingProfile mapperBilling = new BillingProfile();
                Phasor mapperPhasor = new Phasor();
                Tamper mapperTamper = new Tamper();
                //KVAHSelection mapperKvar = new KVAHSelection();
                //RS232LockUnlock mapperRS232 = new RS232LockUnlock();
                //BillingDateTime mapperBillingType = new BillingDateTime();
                //RealTimeClock mapperRTC = new RealTimeClock();
                //AutoLock mapperAutoLock = new AutoLock();
                //Anamoly mapperAnamoly = new Anamoly();
                //LSCapturePeriod mapperLSCapturePeriod = new LSCapturePeriod();
                //DemandIntegrationPeriod mapperDIP = new DemandIntegrationPeriod();
                //MDWithIp mapperDIPWithIP = new MDWithIp();
                //ManualBilling mapperManualBilling = new ManualBilling();
                //SoftwareBilling mapperSoftwareBilling = new SoftwareBilling();


                BillingGeneralNFDLMSEntity entityToHoldTempData = new BillingGeneralNFDLMSEntity();
                BillingGeneralNFDLMSEntity masterEntity = new BillingGeneralNFDLMSEntity();
                string MeterModelNumber = string.Empty;
                string meterVariant = string.Empty;
                masterEntity.MeterData = new MeterDataEntity();
                masterEntity.MeterData.UploadingDateTime = DateUtility.DateTimeToLong(System.DateTime.Now);
                masterEntity.MeterData.ReadingDateTime = DateUtility.DateTimeToLong(this.ReadingDateTime);

                //General Profile              
                if (generalData != null && generalData.Count > 0)
                {
                    masterEntity.General = mapperGeneral.GetMappedEntity(generalData);
                    MeterModelNumber = masterEntity.General.MeterModelNo;
                    meterVariant = masterEntity.General.NetMeterVariantInfo;
                    masterEntity.MeterData.MeterID = (masterEntity.General.MeterSerialNumber).Trim();
                }
                else
                {
                    Updatestatus("Corrupt File General", Color.Red);
                    //return;
                }


                //NamePlate Profile
                if (namePlateData != null && namePlateData.Count > 0)
                {
                    masterEntity.NamePlateProfile = mapperGeneral.GetMappedEntity(namePlateData);
                }
                else
                {
                    Updatestatus("Corrupt File NamePlate", Color.Red);
                    //return;
                }


                //Instant Profile
                if (instantData != null && instantData.Count > 0)
                {
                    masterEntity.Instant = mapperInstant.GetMappedEntity(instantData, masterEntity.General);
                }
                else
                {
                    Updatestatus("Corrupt File Instant", Color.Red);
                    //return;
                }

                //if (anamolyData != null && anamolyData.Count > 0)
                //{
                //    masterEntity.Anomaly = mapperAnamoly.GetMappedEntity(anamolyData, masterEntity.General);
                //}

                //Load Survey Profile
                if (loadSurveyData != null && loadSurveyData.Count > 0)
                {
                    try
                    {
                        entityToHoldTempData = mapperLoadSurevy.GetMappedEntity(loadSurveyData, meterVariant);
                        masterEntity.LoadSurvey = entityToHoldTempData.LoadSurvey;
                        masterEntity.LSParameterColumns = entityToHoldTempData.LSParameterColumns;
                    }
                    catch (Exception ex)    //Exception log for catch block
                    {

                        Updatestatus("Corrupt File LoadSurvey", Color.Red);
                        logger.Log(LOGLEVELS.Error, "ExportFile()", ex);
                        //return;
                    }
                }
                else
                {
                    Updatestatus("Corrupt File LoadSurvey", Color.Red);
                    //return;
                }


                //Billing Profile
                if (billingData != null && billingData.Count > 0)
                {
                    entityToHoldTempData = mapperBilling.GetMappedEntity(billingData, masterEntity.General);
                    masterEntity.Billing = entityToHoldTempData.Billing;
                    masterEntity.BillingParameterColumns = entityToHoldTempData.BillingParameterColumns;
                }
                else
                {
                    Updatestatus("Corrupt File Billing", Color.Red);
                    //return;
                }



                //Daily Survey Profile
                if (midnightData != null && midnightData.Count > 0)
                {
                    entityToHoldTempData = mapperMidnight.GetMappedEntity(midnightData, meterVariant);
                    masterEntity.MidnightData = entityToHoldTempData.MidnightData;
                    masterEntity.MidnightParameterColumns = entityToHoldTempData.MidnightParameterColumns;
                }
                else
                {
                    Updatestatus("Corrupt File Daily Survey", Color.Red);
                    //return;
                }


                //Tamper Profile
                if (tamperData != null && tamperData.Count > 0)
                {
                    entityToHoldTempData = mapperTamper.GetMappedEntity(tamperData);
                    masterEntity.Tamper = entityToHoldTempData.Tamper;
                    masterEntity.TamperParameterColumns = entityToHoldTempData.TamperParameterColumns;
                }
                else
                {
                    Updatestatus("Corrupt File Tamper", Color.Red);
                    //return;
                }

                //Phasor Profile
                if (phasorData != null && phasorData.Count > 0 && phasorData[0].ListMeterDataPacket.Count > 0)
                {

                    masterEntity.Phasor = mapperPhasor.GetMappedEntity(phasorData);
                }
                else
                {
                    Updatestatus("Corrupt File Phasor", Color.Red);
                    //return;
                }

              
                //Instant
                //For urgent delivery Fixed Export, Design exist for Dynamic XML configuration Export                
                Updatestatus("Exporting Instant...", Color.LightBlue);
                InstantFixedExportData(masterEntity);
                //InstantDynamicExportData(masterEntity.Instant);


                //Billing
                //For urgent delivery Fixed Export, Design exist for Dynamic XML configuration Export     
                Updatestatus("Exporting Billing...", Color.LightBlue);
                BillingFixedExportData(masterEntity);
                //BillingDynamicExportData(masterEntity.Billing);

               

                //Tamper
                //For urgent delivery Fixed Export, Design exist for Dynamic XML configuration Export         
                Updatestatus("Exporting Tamper...", Color.LightBlue);
                TamperFixedExportData(masterEntity);
                //TamperDynamicExportData(masterEntity.Tamper);



                //Load Survey
                //For urgent delivery Fixed Export, Design exist for Dynamic XML configuration Export  
                Updatestatus("Exporting Load Survey...", Color.LightBlue);
                LoadSurveyFixedExportData(masterEntity);
                //LoadSurveyDynamicExportData(masterEntity.LoadSurvey);

                                

                Updatestatus("Export Completed", Color.Green);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "ExportFile()", ex);
            }
        }

        private void TamperDynamicExportData(List<DLMS650TamperEntity> TamperParameter)
        {
            try
            {
                foreach (DLMS650TamperEntity itemTamperParameter in TamperParameter)
                {
                    foreach (HVDSSettingsProfile HVDSExportProfile in HVDSSetting.Items)
                    {
                        if (HVDSExportProfile.ProfileName.ToUpper() == "TAMPER" && HVDSExportProfile.Visible.ToUpper() == "TRUE")
                        {
                            foreach (HVDSSettingsProfileProfileParameter itemProfile in HVDSExportProfile.ProfileParameter)
                            {
                                if (itemProfile.Visible.ToUpper() == "TRUE")
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "TamperDynamicExportData(List<DLMS650TamperEntity> TamperParameter)", ex);
            }
        }

        private List<DLMS650TamperEntity> GetTamperDetailByTamperId(List<DLMS650TamperEntity> TamperList, long EventCode)
        {
            List<DLMS650TamperEntity> tampList = null;
            try
            {
                tampList = TamperList.FindAll(a => a.EventCode.Equals(EventCode));
                tampList = tampList.OrderBy(a => a.DateTimeEvent).ToList();
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTamperDetailByTamperId(List<DLMS650TamperEntity> TamperList, long EventCode)", ex);

            }
            return tampList;
        }

        private static string DateTimeDiff(DateTime dt1, DateTime dt2)
        {
            string Diff = string.Empty;
            try
            {         
                TimeSpan t = (dt1 - dt2);
                Diff = t.Days + " " + t.Hours.ToString("00") + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
              
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DateTimeDiff(DateTime dt1, DateTime dt2)", ex);

            }
            return Diff;
        }

        private void TamperFixedExportData(BillingGeneralNFDLMSEntity masterEntity)
        {
            try
            {
                string Seperator = ",";
                string EndLine = "\n";
                string MeterSerialNumber = masterEntity.General.MeterSerialNumber;
                long meterrtc = Convert.ToInt64(GetInstantColumnvalue(masterEntity, "Real Time Clock - Date and Time", false));
                DateTime meterRTC = DateUtility.LongToDateTime(meterrtc);                
                List<DLMS650TamperEntity> TamperList = masterEntity.Tamper;
                DataSet dsTamper = GetDetailTamperData(TamperList);
                DateTime dtcheck = new DateTime(this._ReadingDateTime.Year, this._ReadingDateTime.Month, this._ReadingDateTime.Day, 0, 0, 0);
                string Content = string.Empty;                
                foreach (DataRow item in dsTamper.Tables[0].Rows)
                {
                    DateTime dtOccur = DateUtility.LongToDateTime(Convert.ToInt64(item["OccDateTime"]));                                       
                    if (dtcheck > dtOccur)
                    {
                        string strRestor = string.Empty;
                        string DateDiff = string.Empty;
                        try
                        {
                            DateTime dtRestor = DateUtility.LongToDateTime(Convert.ToInt64(item["ResDateTime"]));
                            strRestor = dtRestor.ToString("dd/MM/yyyy HH:mm");
                            DateDiff = DateTimeDiff(dtRestor, dtOccur);
                        }
                        catch (Exception ex)    //Exception log for catch block
                        {
                            logger.Log(LOGLEVELS.Error, "TamperFixedExportData(BillingGeneralNFDLMSEntity masterEntity)", ex);
                        }
                        Content += MeterSerialNumber + Seperator + Convert.ToString(item["OccEvent"]) + Seperator + dtOccur.ToString("dd/MM/yyyy HH:mm") +
                           Seperator + strRestor + Seperator + DateDiff + EndLine;
                    }
                }
                if (!Directory.Exists(HVDS_Export_Directory + "\\" + MeterSerialNumber))
                {
                    Directory.CreateDirectory(HVDS_Export_Directory + "\\" + MeterSerialNumber);
                }
                if (Content != string.Empty)
                {
                    File.WriteAllText(HVDS_Export_Directory + "\\" + MeterSerialNumber + "\\" + MeterSerialNumber + meterRTC.ToString("_yyyyMMdd") + ".CST", Content);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Tamper Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "TamperFixedExportData(BillingGeneralNFDLMSEntity masterEntity)", ex);
            }
        }

        private void LoadSurveyDynamicExportData(List<DLMS650LoadSurveyEntity> LoadSurveyParameter)
        {
            try
            {
                foreach (DLMS650LoadSurveyEntity itemLoadSurveyParameter in LoadSurveyParameter)
                {
                    foreach (HVDSSettingsProfile HVDSExportProfile in HVDSSetting.Items)
                    {
                        if (HVDSExportProfile.ProfileName.ToUpper() == "LOAD SURVEY" && HVDSExportProfile.Visible.ToUpper() == "TRUE")
                        {
                            foreach (HVDSSettingsProfileProfileParameter itemProfile in HVDSExportProfile.ProfileParameter)
                            {
                                if (itemProfile.Visible.ToUpper() == "TRUE")
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "LoadSurveyDynamicExportData(List<DLMS650LoadSurveyEntity> LoadSurveyParameter)", ex);
            }
        }

        private void LoadSurveyFixedExportData(BillingGeneralNFDLMSEntity masterEntity)
        {
            try
            {
                string Seperator = ",";
                string DataUnavailable = "-1";
                string EndLine = "\n";
                TimeSpan MDInterval = new TimeSpan(0, 0, -30, 0);
                TimeSpan SIXTYDays = new TimeSpan(60, 0, 0, 0);
                string Content = "MeterNo" + Seperator + "Date and time" + Seperator + "Active(I) B-phase W(I)" + Seperator + "Active(I) Y-phase W(I)" + Seperator + "Active(I) R-phase W(I)" + Seperator + "Reactive Net B-phase VAr" + Seperator + "Reactive Net Y-phase VAr" + Seperator + "Reactive Net R-phase VAr" + Seperator + "Vb V" + Seperator + "Vy V" + Seperator + "Vr V" + Seperator + "No Power Duration Minutes" + Seperator + "Wh Abs" + Seperator + "DataStatus" + Seperator + "Units" + EndLine;


                string LoadSurveyColumn = masterEntity.LSParameterColumns.ColumnsNames;
                string MeterSerialNumber = masterEntity.General.MeterSerialNumber;
                List<DLMS650LoadSurveyEntity> lstLoadSurvey = masterEntity.LoadSurvey;
                lstLoadSurvey.RemoveAll(a => a.RealTimeClockDateandTime < 2000010100000);
                DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime endDate = new DateTime(2000, 1, 1, 0, 0, 0);
                if (lstLoadSurvey != null && lstLoadSurvey.Count > 0)
                {
                    lstLoadSurvey = lstLoadSurvey.OrderByDescending(a => a.RealTimeClockDateandTime).ToList();
                    DateTime PrevDateTime = DateUtility.LongToDateTime(Convert.ToInt64(lstLoadSurvey[0].RealTimeClockDateandTime));
                    endDate = DateUtility.LongToDateTime(Convert.ToInt64(lstLoadSurvey[0].RealTimeClockDateandTime));
                    startDate = DateUtility.LongToDateTime(Convert.ToInt64(lstLoadSurvey[lstLoadSurvey.Count - 1].RealTimeClockDateandTime));
                    foreach (DLMS650LoadSurveyEntity item in lstLoadSurvey)
                    {
                        DateTime dtRTC = DateUtility.LongToDateTime(Convert.ToInt64(item.RealTimeClockDateandTime));
                        if (((PrevDateTime - dtRTC) < SIXTYDays) && (PrevDateTime.Minute == 0) || (PrevDateTime.Minute == 30))
                        {
                            while (PrevDateTime.Add(MDInterval) > dtRTC)
                            {
                                PrevDateTime = PrevDateTime.Add(MDInterval);
                                Content += MeterSerialNumber + Seperator + PrevDateTime.ToString("dd/MM/yyyy HH:mm:ss") + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                                                     DataUnavailable + Seperator +
                                    //No Power Duration Minutes
                                                                     "30" + Seperator +
                                                                     
                                    //Block Energy 
                                                                     DataUnavailable + Seperator +

                                    //Data Status
                                                                     "2" + Seperator +
                                    //Unit
                                                                     "1" + Seperator +
                                                                     EndLine;
                               
                            }
                        }


                        Content += MeterSerialNumber + Seperator + dtRTC.ToString("dd/MM/yyyy HH:mm:ss") + Seperator;

                        //Active Power Phase Wise
                        Content += GetDataWithoutUnit(item.ActivePowerBPhase) + Seperator +
                                   GetDataWithoutUnit(item.ActivePowerYPhase) + Seperator +
                                   GetDataWithoutUnit(item.ActivePowerRPhase) + Seperator;

                        //Reactive Power Phase Wise
                        Content += GetDataWithoutUnit(item.ReactivePowerBPhase) + Seperator +
                                 GetDataWithoutUnit(item.ReactivePowerYPhase) + Seperator +
                                 GetDataWithoutUnit(item.ReactivePowerRPhase) + Seperator;

                        //Voltage Phase Wise
                        Content += GetDataWithoutUnit(item.BPhaseVoltage) + Seperator +
                                    GetDataWithoutUnit(item.YPhaseVoltage) + Seperator +
                                    GetDataWithoutUnit(item.RPhaseVoltage) + Seperator;                     

                        //No Power Duration Minutes
                        Content += RemoveUnit(item.PowerOffDurationLSIP) + Seperator;

                        //Block Energy 
                        Content += GetDataWithoutUnit(item.BlockEnergykWh) + Seperator;

                        //Data Status
                        Content += "1" + Seperator;

                        //Unit
                        Content += "1"+ Seperator ;


                        Content += EndLine;
                        PrevDateTime = dtRTC;
                    }

                    if (!Directory.Exists(HVDS_Export_Directory + "\\" + MeterSerialNumber))
                    {
                        Directory.CreateDirectory(HVDS_Export_Directory + "\\" + MeterSerialNumber);
                    }

                    if (Content != string.Empty)
                    {
                        File.WriteAllText(HVDS_Export_Directory + "\\" + MeterSerialNumber + "\\" + MeterSerialNumber + startDate.ToString("_yyyyMMdd") + endDate.ToString("_yyyyMMdd") + ".CSV", Content);
                    }
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Load Survey Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "LoadSurveyFixedExportData(BillingGeneralNFDLMSEntity masterEntity)", ex);
            }
        }

       private string LagLeadCheck(string Value)
       {
           string Result = Value;
           try 
	        {	        
        		if(Result.Contains("-"))
                {
                    Result = Result.Replace('-', ' ');
                    Result = Result + " Ld";
                }
                else if (Result.Contains("+"))
                {
                    Result = Result.Replace('+', ' ');
                    Result = Result + " Lg";
                }
                else
                {
                    Result = Result + " Lg";
                }
	        }
           catch (Exception ex)    //Exception log for catch block
	        {
                logger.Log(LOGLEVELS.Error, "LagLeadCheck(string Value)", ex);
        		
	        }
           return Result;
       }


        private void InstantFixedExportData(BillingGeneralNFDLMSEntity masterEntity)
        {
            try
            {
                string seperator = ",";
                string EndLine = "\n";
                string MeterSerialNumber = masterEntity.General.MeterSerialNumber;
                long meterrtc = Convert.ToInt64(GetInstantColumnvalue(masterEntity, "Real Time Clock - Date and Time",false));
                DateTime meterRTC = DateUtility.LongToDateTime(meterrtc);
                string MeterRTC = DateUtility.LongToStringDateTimeFormat(meterrtc);
                string MeterReadingDateTime = this._ReadingDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                //Meter Export Date Time
                string MeterUploadingDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                string MeterType = masterEntity.General.Metertype;
                string MeterFrequency = GetInstantColumnvalue(masterEntity, "Frequency", false);
                string PhaseSequence = string.Empty;
                string AngleRY = string.Empty;
                string AngleRB = string.Empty;
                if (masterEntity.Phasor != null)
                {
                     PhaseSequence = masterEntity.Phasor.PhaseSequence;
                     AngleRY = masterEntity.Phasor.AngleYR;
                     AngleRB = masterEntity.Phasor.AngleBR;
                }
                string VoltUnit = "V";// GetUnit(masterEntity, "Phase Voltage");
                string VoltVRN = GetInstantColumnvalue(masterEntity, "Voltage - VRN", false);
                string VoltVYN = GetInstantColumnvalue(masterEntity, "Voltage - VYN", false);
                string VoltVBN = GetInstantColumnvalue(masterEntity, "Voltage - VBN", false);               
                string LCurrent_R = GetInstantColumnvalue(masterEntity, "Current - IR", false);
                string ACurrent_R = " "; //GetInstantColumnvalue(masterEntity, "Current - IR", false);
                string RCurrent_R = " "; //GetInstantColumnvalue(masterEntity, "Current - IR", false);
                string LCurrent_Y = GetInstantColumnvalue(masterEntity, "Current - IY", false);
                string ACurrent_Y = " "; //GetInstantColumnvalue(masterEntity, "Current - IY", false);
                string RCurrent_Y = " "; //GetInstantColumnvalue(masterEntity, "Current - IY", false);
                string LCurrent_B = GetInstantColumnvalue(masterEntity, "Current - IB", false);
                string ACurrent_B = " "; //GetInstantColumnvalue(masterEntity, "Current - IB", false);
                string RCurrent_B = " "; //GetInstantColumnvalue(masterEntity, "Current - IB", false);
                string PowerFactor_R = GetInstantColumnvalue(masterEntity, "Signed Power Factor - R Phase (+Lag;-Lead)", false);
                PowerFactor_R = LagLeadCheck(PowerFactor_R);
                string PowerFactor_Y = GetInstantColumnvalue(masterEntity, "Signed Power Factor - Y Phase (+Lag;-Lead)", false);
                PowerFactor_Y = LagLeadCheck(PowerFactor_Y);
                string PowerFactor_B = GetInstantColumnvalue(masterEntity, "Signed Power Factor - B Phase (+Lag;-Lead)", false);
                PowerFactor_B = LagLeadCheck(PowerFactor_B);
                string PowerFactor_Avg = GetInstantColumnvalue(masterEntity, "Signed Power Factor (+Lag;-Lead)", false);
                PowerFactor_Avg = LagLeadCheck(PowerFactor_Avg);
                string Act_Power = GetInstantColumnvalue(masterEntity, "Active Power (ABS)",true);
                string React_Power = GetInstantColumnvalue(masterEntity, "Signed Reactive Power - kvar (+Lag;-Lead)",true);
                string EMF = "N";             
                string CT_Install = masterEntity.General.InternalCTratio;
                string PT_Install = masterEntity.General.InternalPTratio;
                string CT_Commissioned = masterEntity.General.InternalCTratio;
                string PT_Commissioned = masterEntity.General.InternalPTratio;


                string Content = "Meter Serial No" + seperator + "Dump Date & Time" + seperator + "MRI Date & Time" + seperator + "Meter Date & Time" + seperator + "Mtr_Type" + seperator + "Mtr_Freqency" + seperator + "Phase_seq" + seperator + "Volt_Unit" + seperator + "Volt_R" + seperator + "Volt_Y" + seperator + "Volt_B" + seperator + "LCurrent_R" + seperator + "ACurrent_R" + seperator + "RCurrent_R" + seperator + "LCurrent_Y" + seperator + "ACurrent_Y" + seperator + "RCurrent_Y" + seperator + "LCurrent_B" + seperator + "ACurrent_B" + seperator + "RCurrent_B" + seperator + "PowerFactor_R" + seperator + "PowerFactor_Y" + seperator + "PowerFactor_B" + seperator + "PowerFactor_Avg" + seperator + "Act_Power" + seperator + "React_Power" + seperator + "EMF_applied" + seperator + "Angle(R-Y)" + seperator + "Angle(R-B)" + seperator + "CT_Install" + seperator + "PT_Install" + seperator + "CT_Commissioned" + seperator + "PT_Commissioned" + EndLine;



                Content += MeterSerialNumber + seperator + MeterUploadingDate + seperator +
                    MeterReadingDateTime + seperator + MeterRTC + seperator + MeterType + seperator + MeterFrequency + seperator +
                                  PhaseSequence + seperator + VoltUnit + seperator + VoltVRN + seperator + VoltVYN + seperator +
                                   VoltVBN + seperator  + LCurrent_R + seperator + ACurrent_R + seperator +
                                   RCurrent_R + seperator + LCurrent_Y + seperator + ACurrent_Y + seperator + RCurrent_Y + seperator +
                                   LCurrent_B + seperator + ACurrent_B + seperator + RCurrent_B + seperator + PowerFactor_R + seperator +
                                   PowerFactor_Y + seperator + PowerFactor_B + seperator + PowerFactor_Avg + seperator + Act_Power + seperator +
                                   React_Power + seperator + EMF + seperator + AngleRY + seperator + AngleRB + seperator + CT_Install + seperator +
                                   PT_Install + seperator + CT_Commissioned + seperator + PT_Commissioned + EndLine;



                if (!Directory.Exists(HVDS_Export_Directory + "\\" + MeterSerialNumber))
                {
                    Directory.CreateDirectory(HVDS_Export_Directory + "\\" + MeterSerialNumber);
                }

                if (Content != string.Empty)
                {
                    File.WriteAllText(HVDS_Export_Directory + "\\" + MeterSerialNumber + "\\" + MeterSerialNumber + meterRTC.ToString("_yyyyMMdd_HHmm") + ".CSI", Content);
                }





            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Instant Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "InstantFixedExportData(BillingGeneralNFDLMSEntity masterEntity)", ex);
            }
        }

        private string GetInstantColumnvalue(BillingGeneralNFDLMSEntity masterEntity, string ClmName, bool WithUnit)
        {
            string ClmValue = string.Empty;
            try
            {
                DLMS650InstantaneousEntity Instant = masterEntity.Instant.Find(a => a.InstantPowerColumnName.Contains(ClmName));
                if (Instant != null)
                {
                    //Present Valuess
                    if (WithUnit)
                    {
                        ClmValue = Instant.InstantPowerColumnValue.Replace('*',' ');                        
                    }
                    else
                    {
                        ClmValue = GetDataWithoutUnit(Instant.InstantPowerColumnValue);
                    }
                }
                else
                {
                    //Default Value
                    ClmValue = string.Empty;
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetInstantColumnvalue(BillingGeneralNFDLMSEntity masterEntity, string ClmName, bool WithUnit)", ex);

            }
            return ClmValue;
        }

        private string GetUnit(BillingGeneralNFDLMSEntity masterEntity, string ClmName)
        {
            string Unit = string.Empty;
            try
            {
                DLMS650InstantaneousEntity Instant = masterEntity.Instant.Find(a => a.InstantPowerColumnName.Contains(ClmName));
                if (Instant != null)
                {
                    //Present Valuess
                    Unit = Instant.InstantPowerColumnValue.Split('*')[1];
                }
                else
                {
                    //Default Value

                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetUnit(BillingGeneralNFDLMSEntity masterEntity, string ClmName)", ex);

            }
            return Unit;
        }


        private string BillingPowerFactorCheck(string Value)
        {
            string Result = Value;
            try
            {
                Decimal val = Convert.ToDecimal(Value);
                val = val * 100;
                Result = FormatDataWidth(GetTruncateDataWithoutUnit(val.ToString()), "0000000000", "");
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "BillingPowerFactorCheck(string Value)", ex);
               
            }
            return Result;
        }



        private void BillingFixedExportData(BillingGeneralNFDLMSEntity masterEntity)
        {
            try
            {
                string resultBilling = string.Empty;
                string EndLine = "\n";
                string seperator = ",";
                string DataUnavailable = "        ";

                string MeterSerialNumber = masterEntity.General.MeterSerialNumber;
                string CumulativeEnergykWhTZ0 = string.Empty;
                string CumulativeEnergykWhTZ0Import = string.Empty;
                string MDkWTZ0 = string.Empty;
                string SystemPowerFactorforBillingPeriod = string.Empty;
                string MeterReadingDate = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(masterEntity.MeterData.ReadingDateTime));
                string MeterRTC = "01/01/2000 00:00:00";
                DLMS650InstantaneousEntity InstantRTC = masterEntity.Instant.Find(a => a.InstantPowerColumnName.Contains("Real Time Clock - Date and Time"));
                if (InstantRTC != null)
                {
                    //Present Valuess
                    MeterRTC = DateUtility.LongToStringDateTimeFormat(Convert.ToInt64(InstantRTC.InstantPowerColumnValue));
                }
                else
                {
                    //Default Value

                }
                //Meter Export Date Time
                string MeterUploadingDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                for (int i = 1; i <= 6; i++)
                {
                    DLMS650BillingEntity billing = masterEntity.Billing.Find(a => a.DataIndex == i);
                    if (billing != null)
                    {
                        //Present Values                        
                        MDkWTZ0 += FormatDataWidth(GetTruncateDataWithoutUnit(billing.MDkWTZ0), "0000000000", "") + seperator;
                        //formatting not required for Power Factor
                        //SystemPowerFactorforBillingPeriod += FormatDataWidth(GetTruncateDataWithoutUnit(billing.SystemPowerFactorforBillingPeriod), "0000000000", "") + seperator;
                        SystemPowerFactorforBillingPeriod += BillingPowerFactorCheck(billing.SystemPowerFactorforBillingPeriod) + seperator;
                        if (masterEntity.General.NetMeterVariantInfo == "4")
                        {
                            CumulativeEnergykWhTZ0Import += FormatDataWidth(GetTruncateDataWithoutUnit(billing.CumulativeEnergykWhTZ0Import), "0000000000", "") + seperator;
                            CumulativeEnergykWhTZ0 += FormatDataWidth(GetTruncateDataWithoutUnit(billing.CumulativeEnergykWhTZ0), "0000000000", "") + seperator;
                        }
                        else if (masterEntity.General.NetMeterVariantInfo == "3")
                        {
                            CumulativeEnergykWhTZ0Import += FormatDataWidth(GetTruncateDataWithoutUnit(billing.CumulativeEnergykWhTZ0), "0000000000", "") + seperator;
                            CumulativeEnergykWhTZ0 += DataUnavailable + seperator;
                        }
                        else if (masterEntity.General.NetMeterVariantInfo == "2")
                        {
                            CumulativeEnergykWhTZ0Import += DataUnavailable + seperator;
                            CumulativeEnergykWhTZ0 += FormatDataWidth(GetTruncateDataWithoutUnit(billing.CumulativeEnergykWhTZ0), "0000000000", "") + seperator;
                        }
                        else
                        {
                            CumulativeEnergykWhTZ0Import += DataUnavailable + seperator;
                            CumulativeEnergykWhTZ0 += FormatDataWidth(GetTruncateDataWithoutUnit(billing.CumulativeEnergykWhTZ0), "0000000000", "") + seperator;
                        }
                    }
                    else
                    {
                        //Default Values
                        CumulativeEnergykWhTZ0 += DataUnavailable + seperator;
                        MDkWTZ0 += DataUnavailable + seperator;
                        SystemPowerFactorforBillingPeriod += DataUnavailable + seperator;
                    }
                }

                string Content = "SERIAL NUMBER" + seperator + "ENERGY(BILL1)WH ABS" + seperator + "ENERGY(BILL2)WH ABS" + seperator + "ENERGY(BILL3)WH ABS" + seperator + "ENERGY(BILL4)WH ABS" + seperator + "ENERGY(BILL5)WH ABS" + seperator + "ENERGY(BILL6)WH ABS" + seperator + "AVG PF(BILL1)AVG PF BILL" + seperator + "AVG PF(BILL2)AVG PF BILL" + seperator + "AVG PF(BILL3)AVG PF BILL" + seperator + "AVG PF(BILL4)AVG PF BILL" + seperator + "AVG PF(BILL5)AVG PF BILL" + seperator + "AVG PF(BILL6)AVG PF BILL" + seperator + "MAX DEMAND(BILL1)W" + seperator + "MAX DEMAND(BILL2)W" + seperator + "MAX DEMAND(BILL3)W" + seperator + "MAX DEMAND(BILL4)W" + seperator + "MAX DEMAND(BILL5)W" + seperator + "MAX DEMAND(BILL6)W" + seperator + "MRI DATE" + seperator + "METER DATE" + seperator + "DUMP DATE" + seperator + "ENERGY(BILL1)WH(I)" + seperator + "ENERGY(BILL2)WH(I)" + seperator + "ENERGY(BILL3)WH(I)" + seperator + "ENERGY(BILL4)WH(I)" + seperator + "ENERGY(BILL5)WH(I)" + seperator + "ENERGY(BILL6)WH(I)" + EndLine;

                Content += MeterSerialNumber + seperator + CumulativeEnergykWhTZ0 + SystemPowerFactorforBillingPeriod + MDkWTZ0 + MeterReadingDate + seperator + MeterRTC + seperator + MeterUploadingDate + seperator + CumulativeEnergykWhTZ0Import;

                if (!Directory.Exists(HVDS_Export_Directory + "\\" + MeterSerialNumber))
                {
                    Directory.CreateDirectory(HVDS_Export_Directory + "\\" + MeterSerialNumber);
                }
                if (Content != string.Empty)
                {
                    File.WriteAllText(HVDS_Export_Directory + "\\" + MeterSerialNumber + "\\" + MeterSerialNumber + "_01_" + this._ReadingDateTime.Month.ToString("00") + "_" + this._ReadingDateTime.Year + ".TXT", Content);
                }

            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Billing Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "BillingFixedExportData(BillingGeneralNFDLMSEntity masterEntity)", ex);
            }
        }

        public string TruncateToPrecision(decimal targetValue, int precision)
        {
            string value = string.Empty;
            try
            {
                value = targetValue.ToString();
                decimal step = (decimal)Math.Pow(10, precision);
                int tmp = (int)Math.Truncate(step * targetValue);
                targetValue = tmp / step;
                value = string.Format("{0:F" + precision.ToString() + "}", targetValue);
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "TruncateToPrecision(decimal targetValue, int precision)", ex);
                return value;
            }
            return value;
        }

        private string FormatDataWidth(string record, string intZeros, string DecZeros)
        {
            string value = string.Empty;
            string decimalValue = string.Empty;
            try
            {
                double data = Convert.ToDouble(record);
                value = Convert.ToUInt64(TruncateToPrecision(Convert.ToDecimal(record), 0)).ToString(intZeros);

                if (record.ToString().Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                {
                    decimalValue = record.ToString().Split(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToCharArray())[1];
                    if (decimalValue.Length > 2)
                    {
                        decimalValue = decimalValue.Substring(0, 2);
                    }
                    else
                    {
                        decimalValue = Convert.ToDecimal(decimalValue).ToString(DecZeros);
                    }
                }
                else
                {
                    decimalValue = DecZeros;
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "FormatDataWidth(string record, string intZeros, string DecZeros)", ex);
            }
            return value + decimalValue;
        }

        private string GetDataWithoutUnit(string inputValue)
        {
            string Result = string.Empty;
            try
            {
                string[] inputArr = inputValue.Split('*');
                Result = inputArr[0];
                if (inputArr.Length > 1 && inputArr[1].Trim().ToUpper()[0] == 'K')
                {
                    Result = Convert.ToString(Convert.ToDecimal(inputArr[0]) * 1000);
                }
                if (inputArr.Length > 1 && inputArr[1].Trim().ToUpper()[0] == 'M')
                {
                    Result = Convert.ToString(Convert.ToDecimal(inputArr[0]) * 1000000);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDataWithoutUnit(string inputValue)", ex);

            }
            return Result;
        }


        private string RemoveUnit(string inputValue)
        {
            string Result = string.Empty;
            try
            {
                string[] inputArr = inputValue.Split('*');
                Result = inputArr[0];               
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "RemoveUnit(string inputValue)", ex);

            }
            return Result;
        }


        private string GetTruncateDataWithoutUnit(string inputValue)
        {
            string Result = string.Empty;
            try
            {
                string[] inputArr = inputValue.Split('*');
                Result = TruncateToPrecision(Convert.ToDecimal(inputArr[0]), 0);
                if (inputArr.Length > 1 && inputArr[1].Trim().ToUpper()[0] == 'K')
                {
                    Result = TruncateToPrecision(Convert.ToDecimal(inputArr[0]) * 1000, 0);
                }
                if (inputArr.Length > 1 && inputArr[1].Trim().ToUpper()[0] == 'M')
                {
                    Result = TruncateToPrecision(Convert.ToDecimal(inputArr[0]) * 1000000, 0);
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetTruncateDataWithoutUnit(string inputValue)", ex);

            }
            return Result;
        }

        private void InstantDynamicExportData(List<DLMS650InstantaneousEntity> InstantParameter)
        {
            try
            {
                foreach (DLMS650InstantaneousEntity itemInstantParameter in InstantParameter)
                {
                    foreach (HVDSSettingsProfile HVDSExportProfile in HVDSSetting.Items)
                    {
                        if (HVDSExportProfile.ProfileName.ToUpper() == "INSTANT" && HVDSExportProfile.Visible.ToUpper() == "TRUE")
                        {
                            foreach (HVDSSettingsProfileProfileParameter itemProfile in HVDSExportProfile.ProfileParameter)
                            {
                                if (itemProfile.Visible.ToUpper() == "TRUE")
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "InstantDynamicExportData(List<DLMS650InstantaneousEntity> InstantParameter)", ex);
            }
        }

        private void BillingDynamicExportData(List<DLMS650BillingEntity> BillingParameter)
        {
            string seperator = ",";
            try
            {

                foreach (DLMS650BillingEntity itemBillingParameter in BillingParameter)
                {
                    foreach (HVDSSettingsProfile HVDSExportProfile in HVDSSetting.Items)
                    {
                        if (HVDSExportProfile.ProfileName.ToUpper() == "BILLING" && HVDSExportProfile.Visible.ToUpper() == "TRUE")
                        {
                            foreach (HVDSSettingsProfileProfileParameter itemProfile in HVDSExportProfile.ProfileParameter)
                            {
                                if (itemProfile.Visible.ToUpper() == "TRUE")
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)    //Exception log for catch block
            {
                Updatestatus("Export Error", Color.Red);
                logger.Log(LOGLEVELS.Error, "BillingDynamicExportData(List<DLMS650BillingEntity> BillingParameter)", ex);
            }
        }

        #endregion
    }
}
