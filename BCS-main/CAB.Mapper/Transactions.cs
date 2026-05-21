#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;

using CAB.Entity;
using CAB.Parser;
using CAB.Parser.Entity;
#endregion
namespace CAB.Mapper
{
    /// <summary>
    /// The class is responsible for mapping parsed input entity to IEC Transaction entity
    /// </summary>
    public class Transactions
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private Dictionary<int, string> transactions = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public Transactions()
        {
            transactions = new Dictionary<int, string>() { 
            {152,"Maximum Demand"},
            {153,"Survey Integration Period"},
            {154,"Billing Date & Time"},
            {155,"Future TOU"},
            {156,"CT Ratio"}, 
            {157,"PT Ratio"},
            {158,"kVAh Selection"},
            {159,"MD Reset"}
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to prepare Transaction IEC entity from parsed common entity
        /// </summary>
        /// <param name="tamperData"></param>
        /// <returns></returns>
        public List<TransactionData> GetData(List<ProfileData> tamperData)
        {
            int totalProgrammingUpdates = 0;
            int timeStampCount = 10;
            List<TransactionData> resultEntity = new List<TransactionData>();
            TransactionData programmingData = new TransactionData();
            programmingData.programmingData = new List<ProgrammingEntity>();
            DataElement dataElement = new DataElement();
            ProfileData profileData = GetTransactions(tamperData);
            totalProgrammingUpdates = profileData.ListMeterDataPacket.Count > 10 ? 10 : profileData.ListMeterDataPacket.Count;

            #region Code for insertng empty records in database if the transactions are less than 10
            for (int counter = totalProgrammingUpdates; counter < 10; counter++)
            {
                string value = string.Empty;
                ProgrammingEntity programmingEntity = new ProgrammingEntity();
                programmingEntity.TotalProgrammingUpdates = totalProgrammingUpdates.ToString();
                programmingEntity.LastTimestamp = string.Empty;
                programmingEntity.UpdateSequence = ColText(timeStampCount + 1);
                programmingEntity.Description1 = string.Empty;
                programmingData.programmingData.Add(programmingEntity);
                timeStampCount--;
            }
            #endregion

            for (int counter = 0; counter < totalProgrammingUpdates; counter++)
            {
                string value = string.Empty;
                ProgrammingEntity programmingEntity = new ProgrammingEntity();
                programmingEntity.TotalProgrammingUpdates = totalProgrammingUpdates.ToString();               
                programmingEntity.UpdateSequence = ColText(timeStampCount);
                timeStampCount--;
                programmingEntity.LastTimestamp = profileData.ListMeterDataPacket[timeStampCount].ReadingDate.ToString("dd/MM/yyyy HH:mm:ss");


                //if the value belongs to Transaction
                if (transactions.TryGetValue(Convert.ToInt32(Common.GetDataElementByDataDefId
                    (profileData.ListMeterDataPacket[timeStampCount].ListDataElementValue, 76).Value), out value))
                {
                    programmingEntity.Description1 = value;
                    programmingData.programmingData.Add(programmingEntity);
                }
            }
            resultEntity.Add(programmingData);
            return resultEntity;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns ProfileData object containing only transaction data
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private ProfileData GetTransactions(List<ProfileData> tamperData)
        {
            ProfileData profileData = new ProfileData();
            profileData.ListMeterDataPacket = new List<MeterDataPacket>();
            profileData.ProfileId = tamperData[3].ProfileId;
            foreach (MeterDataPacket packet in tamperData[3].ListMeterDataPacket)
            {
                if (transactions.ContainsKey(Convert.ToInt32(packet.ListDataElementValue[0].Value)))
                {
                    profileData.ListMeterDataPacket.Add(packet);
                }
            }
            var list = (from packet in profileData.ListMeterDataPacket select packet).
                OrderByDescending<MeterDataPacket, DateTime>(item => item.ReadingDate);
            profileData.ListMeterDataPacket = list.ToList<MeterDataPacket>();
            return profileData;
        }
        /// <summary>
        /// Gets the formatted updated sequence
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string ColText(int num)
        {
            if (num == 1)
                return "Last Time Stamp";
            else if (num == 2)
                return "2nd Last Time Stamp";
            else if (num == 3)
                return "3rd Last Time Stamp";
            else
                return string.Concat((num).ToString(), "th Last Time Stamp");
        }
        #endregion

    }
}
