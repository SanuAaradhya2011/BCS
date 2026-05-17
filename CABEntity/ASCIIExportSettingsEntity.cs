/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */
using System;
using System.Collections.Generic; 
using System.Text;
using CAB.Framework.Entity;

namespace CAB.Entity
{
    public class ASCIIExportSettingsEntity : EntityBase
    {
        private int asciiexportsettings_ID;
        private string fileName;
        private string delimeter;
        private string generalColumn;
        private string generalDBColumn;
        private string billingColumn;
        private string billingDBColumn;
        private string tamperColumn;
        private string tamberDBColumn;
        private string instantColumn;
        private string instantDBColum;
        private string loadSurveyColumn;
        private string loadSurveyDBColumn;

        public int Asciiexportsettings_ID
        {
            get { return asciiexportsettings_ID; }
            set { asciiexportsettings_ID = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public string Delimeter
        {
            get { return delimeter; }
            set { delimeter = value; }
        }
        public string GeneralColumn
        {
            get { return generalColumn; }
            set { generalColumn = value; }
        }
        public string GeneralDBColumn
        {
            get { return generalDBColumn; }
            set { generalDBColumn = value; }
        }
        public string BillingColumn
        {
            get { return billingColumn; }
            set { billingColumn = value; }
        }
        public string BillingDBColumn
        {
            get { return billingDBColumn; }
            set { billingDBColumn = value; }
        }
        public string TamperColumn
        {
            get { return tamperColumn; }
            set { tamperColumn = value; }
        }
        public string TamberDBColumn
        {
            get { return tamberDBColumn; }
            set { tamberDBColumn = value; }
        }
        public string InstantColumn
        {
            get { return instantColumn; }
            set { instantColumn = value; }
        }
        public string InstantDBColum
        {
            get { return instantDBColum; }
            set { instantDBColum = value; }
        }
        public string LoadSurveyColumn
        {
            get { return loadSurveyColumn; }
            set { loadSurveyColumn = value; }
        }
        public string LoadSurveyDBColumn
        {
            get { return loadSurveyDBColumn; }
            set { loadSurveyDBColumn = value; }
        }
        //added for MVVNL
        public string MidnightEnergiesColumn
        {
            get; set;
        }

        public string SelfDiagnosisColumn
        {
            get;
            set;
        }
        public string SelfDiagnosisDBColumn
        {
            get;
            set;
        }
        public string MidnightEnergiesDBColumn
        {
            get; set;
        }
        //added for MVVNL
    }
}
