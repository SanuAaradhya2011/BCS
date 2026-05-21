using System;
using LNG.Framework.Entity;


namespace LNG.Entity
{
    
    public class AdhocMasterEntity : EntityBase
    {
        private string _AdhocID;
        private string _descriptions;
        private string _oBISCODE;
        private string _cLASS;
        private string _aTTRIBUTE;
        private string _value;
        private string _unit;
        public String Adhoc_ID
        {
            get
            {
                return _AdhocID;
            }
            set
            {
                _AdhocID = value;
            }
        }
       
        public string Descriptions
        {
            get
            {
                return _descriptions;
            }
            set
            {
                _descriptions = value;
            }
        }
        
        public string OBISCODE
        {
            get
            {
                return _oBISCODE;
            }
            set
            {
                _oBISCODE = value;
            }
        }

        public string CLASS
        {
            get
            {
                return _cLASS;
            }
            set
            {
                _cLASS = value;
            }
        }
        public string ATTRIBUTE
        {
            get
            {
                return _aTTRIBUTE;
            }
            set
            {
                _aTTRIBUTE = value;
            }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }
        

    }
}

