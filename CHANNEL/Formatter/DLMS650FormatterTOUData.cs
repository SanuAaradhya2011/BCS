/* GKG JVVNL Current TOU Read */
using System.Windows.Forms;
using CAB.Entity;
using System.Collections.Generic;
using System;
using CAB.Framework;

namespace CAB.Channel.Formatter 
{
    class DLMS650FormatterTOUData : ReadBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void OnChannelStatusChange(string msg)
        {
            this.StatusMessage = msg;
            Application.DoEvents();
        }

        /// <summary>
        /// This method converts the Date time values(Hexadecimal format) for a parameter into proper date time string
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string DateTimeValue)
        {
            int num = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Seconds = 0;
            DateTime dateTime;
            try
            {
                // Extracting the year value
                num += 4;
                string data = DateTimeValue.Substring(num, 4);
                Year = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);
                num += 4;
                // Extracting the month value
                Month = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Day value
                Day = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 4;
                // Extracting the Hour value
                Hour = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Minutes value
                Minute = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;
                // Extracting the Seconds value
                Seconds = ConvertHexToDecimal(DateTimeValue.Substring(num, 2), 0);
                num += 2;

                dateTime = new DateTime(Year, Month, Day, Hour, Minute, Seconds);
            }
            catch 
            {

                dateTime = System.DateTime.Now;
            }
            
            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="master"></param>
        public void LoadTOUData(string[] data, BillingGeneralNFDLMSEntity masterEntity)
        {


            if (string.IsNullOrEmpty(data[0]))
            {
                return;
            }
            int currentDayID = 1;
            int weekNumber = 1;
            if (data.Length == 4)
            {
                DateTime meterDateTime = GetDateTime(data[3]);
                weekNumber = GetWeekProfileNumber(data[2], meterDateTime);
                currentDayID = GetCurrentDayProfileNo(data[1], meterDateTime, weekNumber);
            }

            string currentTOUdata = data[0];
            TOUEntity touEntity = new TOUEntity();
            List<TOU> touList = new List<TOU>();
            touEntity.tou = new List<TOU>();

            /*08
             * 0118
             *      0202
             *          1101
             *          010A
             *              0203090400000000090600000A0064FF120001
             *              0203090406000000090600000A0064FF120002
             *              020309040C000000090600000A0064FF120003
             *              0203090412000000090600000A0064FF120004
             *              0203090400000000090600000A0064FF120000
             *              0203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF1200000203090400000000090600000A0064FF120000*/

            int counter = 0;
            counter = counter + 2;  // for array
            int dayCount = ConvertHexToDecimal(currentTOUdata, counter);
            counter = counter + 2;  // for array length
            byte dayIndex = 0;

            // For Ruby Every Season have 6 day profile so 2nd Week should start from 7th Day Profile 
            if (dayCount > 2) // ruby (24 )
            {
                if (weekNumber == 2)
                {
                    currentDayID = 7;
                }
             }

            while (dayIndex < dayCount)
            {
                counter = counter + 4; // for Structure + length
                counter = counter + 2; //  unsigned
                int dayID = ConvertHexToDecimal(currentTOUdata, counter);
                counter = counter + 2; // day id
                if (dayID == currentDayID)
                {
                    byte dayActionIndex = 0;
                    counter = counter + 2;  // for array
                    int dayActionCount = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;  // for array length

                    while (dayActionIndex < dayActionCount)
                    {
                        TOU touItem = new TOU();
                        counter = counter + 4;      //0203 (struct of 3)
                        counter = counter + 4;      //090400000000 (String of 4 and HH,MM)
                        touItem.StartHour = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        touItem.StartMin = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        counter = counter + 4;

                        counter = counter + 16;  //090600000A0064FF (OBIS code)
                        counter = counter + 4;  //120005 ( tariff in last byte)
                        touItem.Tariff = ConvertHexToDecimal(currentTOUdata, counter);
                        counter = counter + 2;
                        touItem.SeasonNumber = Convert.ToByte(weekNumber);
                        if (touItem.Tariff == 0)
                            break;
                        // touEntity.tou.Add(touItem);
                        touList.Add(touItem);
                        dayActionIndex++;
                    }
                    break;
                }
                else
                {
                    byte dayActionIndex = 0;
                    counter = counter + 2;  // for array
                    int dayActionCount = ConvertHexToDecimal(currentTOUdata, counter);
                    counter = counter + 2;  // for array length

                    while (dayActionIndex < dayActionCount)
                    {
                        counter = counter + 38;
                        dayActionIndex++;
                    }
                }
                dayIndex++;
            }



            masterEntity.TOU = touList;

        }

        public byte ConvertHexToDecimal(string dataInStringFormat, int dataIndex)
        {
            string data = dataInStringFormat.Substring(dataIndex, 2);
            return byte.Parse(data, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calendarSeasonProfile"></param>
        /// <param name="currentDate"></param>
        /// <returns></returns>
        public int GetWeekProfileNumber(string seasonProfileData, DateTime currentDate)
        {
            int weekProfile = 1;
                      
                //08
                //0104
                    //0203
                        //090101
                        //090CFFFF0101FFFFFFFFFF800000
                        //090101
                    //0203090101090CFFFF0401FFFFFFFFFF800000090102
                    //0203090101090CFFFF0701FFFFFFFFFF800000090103
                    //0203090101090CFFFF0A01FFFFFFFFFF800000090104
            try
            {
                int counter = 0;
                counter = counter + 2;  // for array
                int seasonCount = ConvertHexToDecimal(seasonProfileData, counter);
                counter = counter + 2;  // for array length
                byte seasonIndex = 0;

                while (seasonIndex < seasonCount)
                {
                    counter = counter + 4; // for Structure + length
                    counter = counter + 6; // season profile name
                    counter = counter + 8; // for date 

                    int seasonMonth = ConvertHexToDecimal(seasonProfileData, counter);
                    int seasonDay = ConvertHexToDecimal(seasonProfileData, counter + 2);
                    DateTime dateTime = new DateTime(currentDate.Year, seasonMonth, seasonDay);
                    /*GKG : 146685 TOU Tariff issue */
                    //counter = counter + 20; // for date time
                    //counter = counter + 4;
                    //weekProfile = ConvertHexToDecimal(seasonProfileData, counter);
                    /*GKG : 146685 TOU Tariff issue */
                    if (dateTime > currentDate)
                    {
                        break;
                    }
                    /*GKG : 146685 TOU Tariff issue */
                    counter = counter + 20; // for date time
                    counter = counter + 4;
                    weekProfile = ConvertHexToDecimal(seasonProfileData, counter);
                     /*GKG : 146685 TOU Tariff issue */
                    counter = counter + 2;
                    seasonIndex++;

                }
            }
            catch
            {
                   // weekProfile = 1;
            }
            return weekProfile;
        }

        /// <summary>
        /// Gets the current day profile no from calendar's season profile, week profile and current data
        /// </summary>
        /// <param name="calendarSeasonProfile"></param>
        /// <param name="calendarWeekProfile"></param>
        /// <param name="master"></param>
        /// <returns></returns>
        public byte GetCurrentDayProfileNo(string weekProfileData, DateTime currentDate, int currentWeekNumber)
        {
            //080104
            //0208
                //0901011101110111081101111111111111
            //020809010211FF11FF11FF11FF11FF11FF11FF
            //020809010311FF11FF11FF11FF11FF11FF11FF
            //020809010411FF11FF11FF11FF11FF11FF11FF
     
            byte dayProfileNo = 1;
            try
            {
                int counter = 0;
                counter = counter + 2;  // for array
                int weekCount = ConvertHexToDecimal(weekProfileData, counter);
                counter = counter + 2;  // for array length
                byte weekIndex = 0;

                while (weekIndex <= weekCount)
                {
                    counter = counter + 4; // for Structure + length
                    counter = counter + 4; // for week profile name
                    weekIndex = ConvertHexToDecimal(weekProfileData, counter);
                    counter = counter + 2; 
                    if (weekIndex == currentWeekNumber)
                    {
                        int cDay = (int)(currentDate.DayOfWeek - 1);
                        for (int i = 0; i < 7; i++)
                        {
                            counter = counter + 2;  // enum
                          
                            if (i == cDay)
                            {
                                dayProfileNo = ConvertHexToDecimal(weekProfileData, counter);
                                break;
                            }
                            counter = counter + 2;  // day id
                        }

                        break;
                    }
                    else
                    {
                        counter = counter + 28;
                    }
                    weekIndex++;

                }
            }
            catch
            {
                dayProfileNo = 1;
            }

            if (dayProfileNo == 0xff)
            {
                dayProfileNo = 0x01;
            }
            return dayProfileNo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="calendarWeekProfile"></param>
        /// <param name="currentDate"></param>
        /// <param name="currentWeekNo"></param>
        /// <returns></returns>
        public int CurrentDayNo(string calendarWeekProfile, DateTime currentDate, int currentWeekNo)
        {
            int currentDayNo = 0;
            int index = 0;
            int counter = 0;
            int array = 0;
            int arrayLength = 0;
            int structure = 0;
            int structureLength = 0;
            int innerCounter = 0;
            int dlmsDataType = 0;
            int dataLength = 0;
            string[] dates = null;
            string value = string.Empty;
            array = ConvertHexToDecimal(calendarWeekProfile,index);
            index += 2;
            arrayLength = ConvertHexToDecimal(calendarWeekProfile, index);
            index += 2;
            while (counter < arrayLength)
            {
                structure = ConvertHexToDecimal(calendarWeekProfile, index);
                index += 2;
                structureLength = ConvertHexToDecimal(calendarWeekProfile, index);
                dates = new string[structureLength];
                index += 2;
                innerCounter = 0;
                while (innerCounter < structureLength)
                {
                    dlmsDataType = ConvertHexToDecimal(calendarWeekProfile, index);
                    index += 2;
                    if (dlmsDataType == 9)
                    {
                        dataLength = ConvertHexToDecimal(calendarWeekProfile, index);
                        index += 2;
                        value = calendarWeekProfile.Substring(index, dataLength * 2);
                        index += dataLength * 2;
                        //FillWeekEntity(value, innerCounter, week);
                    }
                    else if (dlmsDataType == 17)
                    {
                        value = calendarWeekProfile.Substring(index, 2);
                        index += 2;
                    }

                    innerCounter++;
                }
                counter++;
            }
           // currentDayNo = GetDayNo(weeks, currentDate, currentWeekNo);
            return currentDayNo;
        }
       

        
        ///// <summary>
        ///// Fills the season entity with the relevant data
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="innerCounter"></param>
        ///// <param name="season"></param>
        //public void FillSeasonEntity(string value, int innerCounter, SeasonEntity season)
        //{
        //    switch (innerCounter)
        //    {
        //        case 0:
        //            season.SeasonProfileName = value;
        //            break;
        //        case 1:
        //            //season.SeasonStart = Convert.ToInt32(GetDateTimeString(value).Substring(4, 4));
        //            break;
        //        case 2:
        //            season.WeekName = value;
        //            break;
        //    }
        //}
        /// <summary>
        /// Fills the season entity with the relevant data
        /// </summary>
        /// <param name="value"></param>
        /// <param name="innerCounter"></param>
        /// <param name="season"></param>
        //public void FillWeekEntity(string value, int innerCounter, WeekProfileEntity week)
        //{
        //    switch (innerCounter)
        //    {
        //        case 0:
        //            week.WeekProfileName = Convert.ToInt32(value, 16);
        //            break;
        //        case 1:
        //            week.Monday = Convert.ToInt32(value, 16);
        //            break;
        //        case 2:
        //            week.Tuesday = Convert.ToInt32(value, 16);
        //            break;
        //        case 3:
        //            week.Wednesday = Convert.ToInt32(value, 16);
        //            break;
        //        case 4:
        //            week.Thursday = Convert.ToInt32(value, 16);
        //            break;
        //        case 5:
        //            week.Friday = Convert.ToInt32(value, 16);
        //            break;
        //        case 6:
        //            week.Saturday = Convert.ToInt32(value, 16);
        //            break;
        //        case 7:
        //            week.Sunday = Convert.ToInt32(value, 16);
        //            break;
        //    }
        //}
       
        //public int GetDayNo(List<WeekProfileEntity> weeks, DateTime currentDate, int currentWeekNo)
        //{
        //    int dayNo = -1;
           
        //    if (currentWeekProfile != null)
        //    {
        //        DayOfWeek dayOfWeek = currentDate.DayOfWeek;
        //        switch (dayOfWeek)
        //        {
        //            case DayOfWeek.Monday:
        //                dayNo = currentWeekProfile.Monday;
        //                break;
        //            case DayOfWeek.Tuesday:
        //                dayNo = currentWeekProfile.Tuesday;
        //                break;
        //            case DayOfWeek.Wednesday:
        //                dayNo = currentWeekProfile.Wednesday;
        //                break;
        //                break;
        //            case DayOfWeek.Thursday:
        //                dayNo = currentWeekProfile.Thursday;
        //                break;
        //                break;
        //            case DayOfWeek.Friday:
        //                dayNo = currentWeekProfile.Friday;
        //                break;
        //            case DayOfWeek.Saturday:
        //                dayNo = currentWeekProfile.Saturday;
        //                break;
        //            case DayOfWeek.Sunday:
        //                dayNo = currentWeekProfile.Sunday;
        //                break;
        //        }
        //    }
        //    return dayNo;
        //}
    }
}
/* GKG JVVNL Current TOU Read */