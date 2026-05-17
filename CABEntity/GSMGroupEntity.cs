
using System;
using CAB.Framework.Entity;
using System.Collections.Generic;
namespace CAB.Entity
{
    public class GSMGroupEntity : EntityBase
    {

        private long groupID;
        private string groupName;
        private string groupType;
        private string meterID;
        private int regionID;
        private int circleID;
        private int divisionID;
        private List<string> availableMeterList = null;
        private List<string> selectedMeterList = null;
        private List<MeterMasterEntity> meterSimList = null;
        public long GroupID
        {
            get
            {
                return groupID;
            }
            set
            {
                groupID = value;
            }
        }

        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                groupName = value;
            }
        }
        public string GroupType
        {
            get
            {
                return groupType;
            }
            set
            {
                groupType = value;
            }
        }
        public string MeterID
        {
            get
            {
                return meterID;
            }
            set
            {
                meterID = value;
            }
        }  


        public int RegionID
        {
            get
            {
                return regionID;
            }
            set
            {
                regionID = value;
            }
        }

        public int CircleID
        {
            get
            {
                return circleID;
            }
            set
            {
                circleID = value;
            }

        }
        public int DivisionID
        {
            get
            {
                return divisionID;
            }
            set
            {
                divisionID = value;
            }
        }
        public List<string> AvailableMeterList
        {
            get
            {
                return availableMeterList;
            }
            set
            {
                availableMeterList = value;
            }
        }
        public List<string> SelectedMeterList
        {
            get
            {
                return selectedMeterList;
            }
            set
            {
                selectedMeterList = value;
            }
        }
        public List<MeterMasterEntity> MeterSimList
        {
            get
            {
                return meterSimList;
            }
            set
            {
                meterSimList = value;
            }
        }
        public string CommunicationType { get; set; }
       
    }
}
