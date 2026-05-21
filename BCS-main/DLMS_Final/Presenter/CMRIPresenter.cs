using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.BCS.DLMS.Model;
using CAB.BCS.DLMS.Views;
using CAB.BCS.DLMS.Utility;
using DLMS_Final;
using System.IO;
using Utilities;
namespace CAB.BCS.DLMS.Presenter
{
    class CMRIPresenter
    {
        #region Variables
        private readonly ICMRI viewCMRI;
        CMRIModel model = new CMRIModel();
        DLMSConnection dlsmConnection = new DLMSConnection();
        string dataValue = string.Empty;
        string strTamperScalecapture = string.Empty;
        string strTamperScalebuffer = string.Empty;
        StreamWriter strmWriter;
        FileStream fileStream;
        string fileName;
        #endregion

        #region Constructor
        /// <summary>
        /// This is the presenter class constuructor and accpeting the view here.
        /// constructor type dependency injection is being used.
        /// </summary>
        /// <param name="view">Please pass the view for the presenter.</param>
        public CMRIPresenter(ICMRI view)
        {
            if (view == null)
                throw new ArgumentNullException(CoreUtility.GetMessageFromResourceFile("CMRIViewCanNotBeNull"));
            viewCMRI = view;

        }
        #endregion

       
        public bool ReadSAPlist()
        {
            try
            {
                return model.ReadSAPlist();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DisplaySAPList(byte[] Blockdata)
        {
            string data;
            int capture_object_definition;
            int i, j = 0, month, year = 0, date, hour, minute, second, nLength = 0, nByteIndex = 1;

            capture_object_definition = Blockdata[nByteIndex];
            for (i = 0; i < capture_object_definition; i++)
            {
                year = 0;
                nByteIndex += 7;
                nLength = Blockdata[nByteIndex++];
                data = string.Empty;
                for (j = 0; j < nLength; j++)
                {
                    data = data + Convert.ToChar(Blockdata[j + nByteIndex]);
                }
                nByteIndex = nByteIndex + (j - 1);
                nByteIndex += 3;

                year = (year | (int)Blockdata[nByteIndex++]) << 8;
                year = (year | (int)Blockdata[nByteIndex++]);
                month = Blockdata[nByteIndex++];
                date = Blockdata[nByteIndex++];

                nByteIndex++;

                hour = Blockdata[nByteIndex++];
                minute = Blockdata[nByteIndex++];
                second = Blockdata[nByteIndex++];
                if (Blockdata[nByteIndex] != 253)
                {
                    data = data + " " + date.ToString("d2") + "/" + month.ToString("d2") + "/" + year.ToString("d2") + " " + hour.ToString("d2") + ":" + minute.ToString("d2") + ":" + second.ToString("d2");
                    viewCMRI.ListCMRI.Items.Add(data, true);
                }
                nByteIndex += 3;
            }
        }

        private bool ReadInstantaneous()
        {
            bool isSucess = true;
            byte ret;
            byte[] attribute = new byte[] { 3, 2 };
            CoreUtility.GetIncrementedTimer();
            try
            {
                foreach (byte atb in attribute)
                {
                    ret = model.ReadInastantaneous(atb);
                    if (ret == 0x01)
                    {
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        for (int i = 0; i < length; i++)
                        {
                            dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        strmWriter.WriteLine("01" + dataValue);
                    }
                    else if (ret == 0x07)
                    {
                        strmWriter.WriteLine("010100");
                    }
                    else
                    {
                        ExecuteElse(out isSucess);
                        break;
                    }
                }

                ReadScalarProfiles(0, ref isSucess, attribute, "01");

                if (CoreUtility.IsPUMA)
                {
                    isSucess = ReadCumulativeKW(isSucess);
                    isSucess = ReadCumulativeKVA(isSucess);
                }
                //chkCMRIInstant.Enabled = true;//TODO:
                //chkCMRIInstant.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSucess;
        }

        private void ReadScalarProfiles(byte profileIndex, ref bool isSucess, byte[] attribute, string profileIdentificationTag)
        {
            byte ret;
            if (isSucess)
            {
                try
                {
                    foreach (byte atb in attribute)
                    {
                        ret = model.ReadScalarProfile(atb, profileIndex);
                        if (ret == 0x01)
                        {
                            dataValue = string.Empty;
                            int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                            for (int i = 0; i < length; i++)
                            {
                                dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                            }
                            strmWriter.WriteLine(profileIdentificationTag + dataValue);
                            if (profileIdentificationTag == "04")
                            {
                                if (profileIndex == 3)
                                    strTamperScalecapture = dataValue;
                                if (profileIndex == 2)
                                    strTamperScalebuffer = dataValue;
                            }
                        }
                        else if (ret == 0x07)
                        {
                            strmWriter.WriteLine(profileIdentificationTag + "0100");
                            if (profileIdentificationTag == "04")
                            {
                                if (profileIndex == 3)
                                    strTamperScalecapture = profileIdentificationTag + "0100";
                                if (profileIndex == 2)
                                    strTamperScalebuffer = profileIdentificationTag + "0100";
                            }
                        }
                        else
                        {
                            ExecuteElse(out isSucess);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool ReadCumulativeKVA(bool isSucess)
        {
            //btnReadAll.Enabled = false;                    
            byte retval2 = model.ReadCumulativeKVA(2);
            if (retval2 == 0x01)
            {
                dataValue = string.Empty;
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                int startIndex = 0;
                // Receive buffer[18] tells the datatype , 0x06 means long int.
                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                {
                    length = 4;
                    startIndex = 19;
                    for (int i = 0; i < length; i++)
                    {
                        dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                    }
                    strmWriter.WriteLine("01" + dataValue);
                }
                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                {
                    strmWriter.WriteLine("010100");
                }
                else
                {
                    ExecuteElse(out isSucess);
                }
            }
            else if (retval2 == 0x07)
            {
                strmWriter.WriteLine("01" + "00000000");
            }
            else
            {
                ExecuteElse(out isSucess);
            }
            retval2 = model.ReadScalarProfile(3, 5);
            if (retval2 == 0x01)
            {
                dataValue = string.Empty;
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                strmWriter.WriteLine("01" + dataValue);
            }
            else if (retval2 == 0x07)
            {
                strmWriter.WriteLine("01");
            }
            else
            {
                ExecuteElse(out isSucess);
            }
            return isSucess;
        }

        private bool ReadCumulativeKW(bool isSucess)
        {
            //btnReadAll.Enabled = false;   
            byte retval1 = model.ReadCumulativeKW(2);
            if (retval1 == 0x01)
            {
                dataValue = string.Empty;
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                int startIndex = 0;
                // Receive buffer[18] tells the datatype , 0x06 means long int.
                if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06)
                {
                    length = 4;
                    startIndex = 19;
                    for (int i = 0; i < length; i++)
                    {
                        dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                    }
                    strmWriter.WriteLine("01" + dataValue);
                }
                else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x01 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 0x00)
                {
                    strmWriter.WriteLine("010100");
                }
                else
                {
                    ExecuteElse(out isSucess);
                }

            }
            else if (retval1 == 0x07)
            {
                //write an empty line so that parser can predict that nothing in this line should be read
                strmWriter.WriteLine("01" + "00000000");
            }
            else
            {
                ExecuteElse(out isSucess);
            }

            retval1 = model.ReadScalarProfile(3, 4);
            if (retval1 == 0x01)
            {
                dataValue = string.Empty;
                int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                for (int i = 0; i < length; i++)
                {
                    dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                }
                strmWriter.WriteLine("01" + dataValue);
            }
            else if (retval1 == 0x07)
            {
                strmWriter.WriteLine("01");
            }
            else
            {
                ExecuteElse(out isSucess);
            }
            return isSucess;
        }

        private void ExecuteElse(out bool isSucess)
        {
            CoreUtility.StopTimer();
            CoreUtility.ExpMessage = CoreUtility.GetMessageFromResourceFile("CMRICONNECTIONFAILED");
            strmWriter.Close();
            fileStream.Close();
            File.Delete(fileName);
            viewCMRI.BtnCMRICancelEnabled = true;
            viewCMRI.BtnReadAllCMRIEnabled = true;
            dlsmConnection.Disconnect();
            SerialPortSettings.Default.ServerSAP = 0x01;
            isSucess = false;
        }

        private bool ReadBilling()
        {
            bool isSucess = true;
            byte[] attribute = new byte[] { 3, 2 };
            byte ret;
            foreach (byte atb in attribute)
            {
                ret = model.ReadBillingProfile(atb, viewCMRI.IsReadlast, viewCMRI.IsReadBetweenBilling, viewCMRI.BillingToDate, viewCMRI.BillingFromDate, viewCMRI.BillingLastFromDate);
                CoreUtility.GetIncrementedTimer();
                if (ret == 0x01)
                {
                    dataValue = string.Empty;
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strmWriter.WriteLine("02" + dataValue);
                }
                else if (ret == 0x07)
                {

                    strmWriter.WriteLine("020100");
                }
                else
                {
                    ExecuteElse(out isSucess);
                    break;
                }
            }
            ReadScalarProfiles(1, ref isSucess, attribute, "02");
            return isSucess;
        }

        private bool ReadLoadsurvey()
        {
            string dataValue = string.Empty;
            bool isSucess = true;
            byte[] attribute = new byte[] { 3, 2 };
            byte ret;
            foreach (byte atb in attribute)
            {
                ret = model.ReadLoadSurveyProfile(atb, viewCMRI.IsReadBetweenLoadSurvey, viewCMRI.LoadSurveyToDate, viewCMRI.LoadSurveyFromDate);
                CoreUtility.GetIncrementedTimer();
                if (ret == 0x01)
                {
                    dataValue = string.Empty;
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strmWriter.WriteLine("03" + dataValue);
                }
                else if (ret == 0x07)
                {

                    strmWriter.WriteLine("030100");
                }
                else
                {
                    ExecuteElse(out isSucess);
                    break;
                }
            }
            ReadScalarProfiles(2, ref isSucess, attribute, "03");
            return isSucess;

        }

        private bool ReadEventLog()
        {
            bool isSucess = true;
            byte[] attribute = new byte[] { 3, 2 };
            byte[] tamperCount = new byte[] { 0, 1, 2, 3, 4, 5 };
            byte ret;

            foreach (byte count in tamperCount)
            {
                foreach (byte atb in attribute)
                {
                    ret = model.ReadTamperProfile(atb, count, viewCMRI.IsReadLastEventLog, viewCMRI.IsReadBetweenEventLog, viewCMRI.EventToDate, viewCMRI.EventFromDate, viewCMRI.EventLastFromDate);
                    CoreUtility.GetIncrementedTimer();
                    if (ret == 0x01)
                    {
                        dataValue = string.Empty;
                        int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                        for (int i = 0; i < length; i++)
                        {
                            dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                        }
                        strmWriter.WriteLine("04" + dataValue);
                    }
                    else if (ret == 0x07)
                    {

                        strmWriter.WriteLine("040100");
                    }
                    else
                    {
                        ExecuteElse(out isSucess);
                        break;
                    }
                }
                if (count == 1)
                {
                    ReadScalarProfiles(3, ref isSucess, attribute, "04");
                }
                strmWriter.WriteLine("04" + strTamperScalecapture);
                strmWriter.WriteLine("04" + strTamperScalebuffer);
            }
            return isSucess;



        }
        private bool ReadGeneral()
        {
            int iIndex = 0;
            int nObjectCount = 0;
            iIndex = 0;
            nObjectCount = 7;
            bool isSucess = true;
            int length = 0;
            int startIndex = 0;
            while (iIndex < nObjectCount)
            {

                int ret = model.GetMeterID(iIndex);
                if (ret == 0x01)
                {
                    if (GlobalObjects.objHDLCLIB.fCheckFCS(GlobalObjects.objSerialComm.ReceiveBuffer) == true)
                    {
                        length = 0;
                        startIndex = 0;
                        dataValue = string.Empty;
                        if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                        {
                            length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x0A && GlobalObjects.objSerialComm.ReceiveBuffer[19] != 12)
                        {
                            length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x09 && GlobalObjects.objSerialComm.ReceiveBuffer[19] == 12)
                        {
                            length = GlobalObjects.objSerialComm.ReceiveBuffer[19];
                            startIndex = 20;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x12)
                        {
                            length = 2;
                            startIndex = 19;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x11)
                        {
                            length = 1;
                            startIndex = 19;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x06 || GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x05)
                        {
                            length = 4;
                            startIndex = 19;
                        }
                        else if (GlobalObjects.objSerialComm.ReceiveBuffer[18] == 0x15)
                        {
                            length = 8;
                            startIndex = 19;

                        }
                        for (int i = 0; i < length; i++)
                        {
                            dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objSerialComm.ReceiveBuffer[i + startIndex]);
                        }
                        strmWriter.WriteLine("05" + dataValue);

                    }
                    else
                    {
                        ExecuteElse(out isSucess);
                    }
                }
                else if (ret == 0x07)
                {

                    strmWriter.WriteLine("050100");
                }
                else if (ret == 0x00)
                {
                    ExecuteElse(out isSucess);
                }
                else
                {
                    DLMS_Final.DLMSMain.ActiveForm.Cursor = System.Windows.Forms.Cursors.Default;
                    break;
                }
                iIndex++;
            }
            return isSucess;
        }

        public bool ReadCMRIFile(int length)
        {
            bool IsSuccessful = false;
            string FileMeterdata;
            int meterIDIndex;
            string middleFileName;
            string meterId;
            int year;
            int month;
            int date;
            int hour;
            int minute;
            int seconds;
            string checkSum;
            DateTime fileDate;
            CoreUtility.StartTimer();
            SerialPortSettings.Default.ServerSAP = length;
            SerialPortSettings.Default.CommandTimeOut = 9000;
            SerialPortSettings.Default.IntercharacterDelay = 7000;
            fileName = AppDomain.CurrentDomain.BaseDirectory;

            if (dlsmConnection.Connect() != true)
            {
                CoreUtility.StopTimer();
                return IsSuccessful;
            }
            CoreUtility.GetIncrementedTimer();

            viewCMRI.ListCMRI.SetSelected(length - 2, true);
            meterIDIndex = viewCMRI.ListCMRI.Text.Length - 20;
            if (meterIDIndex < 7 || meterIDIndex > 16)
            {
                return IsSuccessful;
            }

            middleFileName = viewCMRI.ListCMRI.Text.Substring(0, meterIDIndex);
            fileName = fileName + middleFileName;
            fileName = fileName + "_" + string.Format("{0:00}", DateTime.Now.Day) + "_" + string.Format("{0:00}", DateTime.Now.Month) + "_" + string.Format("{0:0000}", DateTime.Now.Year) + "_" + string.Format("{0:00}", DateTime.Now.Hour) + "_" + string.Format("{0:00}", DateTime.Now.Minute) + "_" + string.Format("{0:00}", DateTime.Now.Second) + "_" + string.Format("{0:00}", DateTime.Now.Millisecond) + ".2NG";
            year = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 7, 4));
            month = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 4, 2));
            date = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 1, 2));
            hour = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 12, 2));
            minute = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 15, 2));
            seconds = Convert.ToInt16(viewCMRI.ListCMRI.Text.Substring(meterIDIndex + 18, 2));

            fileDate = new DateTime(year, month, date, hour, minute, seconds);
            meterId = Convert.ToString(meterIDIndex);
            while (meterId.Length < 2)
            { meterId = "0" + meterId; }
            FileMeterdata = meterId + middleFileName + string.Format("{0:0000}", fileDate.Year) + string.Format("{0:00}", fileDate.Month) + string.Format("{0:00}", fileDate.Day) + string.Format("{0:00}", fileDate.Hour) + string.Format("{0:00}", fileDate.Minute) + string.Format("{0:00}", fileDate.Second);

            fileStream = new FileStream(fileName, FileMode.Create);
            strmWriter = new StreamWriter(fileStream);

            try
            {
                strmWriter.WriteLine("00" + FileMeterdata);
                CoreUtility.GetIncrementedTimer();

                if (viewCMRI.IsInstantaneous)
                {
                    ReadInstantaneous();
                }
                else
                {
                    WriteBlankLine("01");
                }

                CoreUtility.GetIncrementedTimer();
                if (viewCMRI.IsBilling)
                {
                    ReadBilling();
                    viewCMRI.CheckCMRIBillingEnabled = true;
                }
                else
                {
                    WriteBlankLine("02");
                }

                if (viewCMRI.IsLoadSurvey)
                {
                    ReadLoadsurvey();
                    viewCMRI.CheckCMRILoadSurveyEnabled = true;
                }
                else
                {
                    WriteBlankLine("03");
                }

                CoreUtility.GetIncrementedTimer();
                if (viewCMRI.IsEventLog)
                {
                    ReadEventLog();
                    viewCMRI.CheckCMRITamperEnabled = true;

                }
                else
                {
                    WriteBlankLine("04");
                }

                CoreUtility.GetIncrementedTimer();
                if (viewCMRI.IsGeneral)
                {
                    ReadGeneral();
                    viewCMRI.CheckCMRINameplateEnabled = true;
                }
                else
                {
                    WriteBlankLine("05");
                }

                if (CoreUtility.IsMVVNL)
                {
                    if (viewCMRI.IsMidNightEnergies)
                    {
                        bool result = ReadMidnightData();
                        if (!result)
                        {
                            ExecuteElse(out result);
                            IsSuccessful = result;
                        }
                    }
                    else
                    {
                        WriteBlankLine("06");
                    }

                }

                strmWriter.Close();
                fileStream.Close();
                viewCMRI.BtnReadAllCMRIEnabled = true;
                viewCMRI.BtnCMRICancelEnabled = true;
                viewCMRI.BtnLoadListEnabled = true;
                viewCMRI.BtnReadAllEnabled = true;
                dlsmConnection.Disconnect();

                checkSum = CABFileOperation.GetMD5ChecksumForFile(fileName);
                fileStream = new FileStream(fileName, FileMode.Append);
                strmWriter = new StreamWriter(fileStream);
                strmWriter.WriteLine(checkSum);
                strmWriter.Close();
                fileStream.Close();
                CoreUtility.StopTimer();
                IsSuccessful = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsSuccessful;
        }

        private void WriteBlankLine(string lineTag)
        {
            for (byte x = 0; x < 4; x++)
                strmWriter.WriteLine(lineTag);
        }


        private bool ReadMidnightData()
        {
            bool isSucess = true;
            byte[] attribute = new byte[] { 3, 2 };
            byte ret;
            foreach (byte atb in attribute)
            {
                ret = model.ReadMidnightProfile(3);
                if (ret == 0x01)
                {
                    dataValue = string.Empty;
                    int length = GlobalObjects.objCOSEMLIB.nBlockTotalByteCount;
                    for (int i = 0; i < length; i++)
                    {
                        dataValue = dataValue + string.Format("{0:X2}", GlobalObjects.objCOSEMLIB.BlockBuffer[i]);
                    }
                    strmWriter.WriteLine("06" + dataValue);
                }
                else if (ret == 0x07)
                {

                    strmWriter.WriteLine("06");
                }
                else
                {
                    ExecuteElse(out isSucess);
                    break;
                }
            }
            ReadScalarProfiles(6, ref isSucess, attribute, "06");
            return isSucess;
        }

    }
}
