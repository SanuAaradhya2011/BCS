using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using CAB.UI.Controls;
using System.Windows.Forms;

namespace CAB.IECChannel.ReadOut
{
    public class ReadoutLoadSurvey :ReadBase
    {  
        private static string lcommandResponse;
        private static string responseForLoadSurvey;
        private bool isDateChanged; 
        public ReadoutLoadSurvey()
        {
            command = Command.GetInstance();
        }

        
     
        public bool IsDateChanged
        {
            get { return isDateChanged; }
            set { isDateChanged = value; }
        }
         
        public override string GetData()
        {
            IsSignOnFailure = false;
             string data=string.Empty;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    Application.DoEvents();
                    return string.Empty;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.DelayExecution();
                    communications.Command = command.ManufactureCommand;
                    communications.SendCommand();
                    communications.DelayExecution();
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            Application.DoEvents();
                            isCorruptedData = true;
                            return string.Empty;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    Thread.Sleep(200);
                    bool flag = false;
                    string TotalTempload = string.Empty;
                    int dayCount = 1;
                    string fromDate = startDate.Day.ToString("00") + startDate.Month.ToString("00") + startDate.Year.ToString("0000").Substring(2, 2);
                    string toDate = endDate.Day.ToString("00") + endDate.Month.ToString("00") + endDate.Year.ToString("0000").Substring(2, 2);
                    while (lcommandResponse.IndexOf(fromDate) == -1)
                    {
                        startDate = startDate.AddDays(1);
                        fromDate = startDate.Day.ToString("00") + startDate.Month.ToString("00") + startDate.Year.ToString("0000").Substring(2, 2);
                    }
                    while (lcommandResponse.IndexOf(toDate) == -1)
                    {
                        endDate = endDate.AddDays(-1);
                        toDate = endDate.Day.ToString("00") + endDate.Month.ToString("00") + endDate.Year.ToString("0000").Substring(2, 2);
                    }
                    lcommandResponse = lcommandResponse.Substring(lcommandResponse.IndexOf(fromDate));
                    if (lcommandResponse.IndexOf(toDate) > 0)
                        lcommandResponse = lcommandResponse.Substring(0, lcommandResponse.IndexOf(toDate) + 6);
                    if (lcommandResponse.Length < 6)
                        return "9";

                    for (int datecounter = 0; datecounter < lcommandResponse.Length; datecounter += 6)
                    {
                        string lcommandDate = lcommandResponse.Substring(datecounter, 6);
                        communications.TotalReadBytes = 0;
                        string commandDateString = String.Empty;
                        for (index = 0; index < lcommandDate.Length; index++)
                        {
                            commandDateString = string.Concat(commandDateString, Convert.ToString((Convert.ToInt32(lcommandDate.Substring(index, 1)) + 30)));
                        }
                        command.CommandDate = commandDateString;
                        if (communications.ReadFlag)
                        {
                            communications.CommandID = 2;
                            communications.ReadFlag = false;
                            communications.OutBuffer = string.Empty;
                            communications.CurrentTime = DateTime.Now;
                            communications.Command = command.ZCommand;

                            if (!communications.SendCommand())
                                return "2";
                            else
                            {
                                communications.DelayExecution();
                                if (!IsAborted)
                                {
                                    dayCount = dayCount + 1;
                                }
                                communications.CurrentTime = DateTime.Now;
                                do
                                {
                                    if (communications.Timeout())
                                    {
                                        this.StatusMessage = MessageConstant.GetText("M000040");
                                        if (IsAborted)
                                        {
                                            this.StatusMessage = "User Aborted.";
                                            Application.DoEvents();
                                            return string.Empty;
                                        }
                                        else
                                        {
                                            if (communications.OutBuffer.IndexOf(ReadoutConstant.NOCARRIER) > 0)
                                                flag = true;
                                            else
                                            {
                                                this.StatusMessage = MessageConstant.GetText("M000041");
                                                Application.DoEvents();
                                            }
                                        }
                                        if (flag)
                                        {
                                            this.StatusMessage = MessageConstant.GetText("M000059");
                                            Application.DoEvents();
                                            return "3";
                                        }
                                        return "1";

                                    }
                                    else
                                    {
                                        string Temperload = string.Empty;
                                        if (communications.ReadFlag)
                                        {
                                            int verifyBcc = 3;
                                            while (verifyBcc > 0)
                                            {
                                                Temperload = communications.OutBuffer;
                                                string resp = Temperload.Substring(1);
                                                bool bccResponse = ReadoutCommon.CalculateBcc(resp, resp.Length - 2, resp.Substring(resp.Length - 1, 1));
                                                if (!bccResponse)
                                                    verifyBcc--;
                                                else
                                                    break;
                                                if (verifyBcc <= 0)
                                                    return "3";
                                            }
                                            TotalTempload = string.Concat(TotalTempload, Temperload);
                                            break;
                                        }
                                    }
                                  //  this.StatusMessage = string.Concat("Day : ", (dayCount - 1).ToString(), "      ", "Total Read Bytes : ", communications.TotalReadBytes);
                                    Application.DoEvents();
                                    if (isAborted)
                                    {
                                        this.StatusMessage = "User Aborted.";
                                        Application.DoEvents();
                                        return string.Empty;
                                    }

                                } while (true);

                            }
                        }
                    }
                    data = Convert.ToChar(1).ToString() + "L" + communications.ResponseSignOn.Replace("\r\n", string.Empty) + "/" + ReadingDateTime + responseForLoadSurvey + TotalTempload + Convert.ToChar(4);
                }
                else
                {
                    IsSignOnFailure = true;
                    data = string.Empty;
                } 
            }
            catch (Exception)
            { 
                data = string.Empty;
            }
            finally
            {
                communications.DelayExecution();
                communications.ClosePort();
            }
            return data;
        }

        public override bool GeDate()
        {
            bool status = false;
            IsSignOnFailure = false;
            try
            {
                if (ConfigInfo.GetLocalMode().Equals("Optical"))
                    communications.BaudRate =Int32.Parse(ConfigInfo.GetBaudRate());
                if (!communications.OpenPort())
                {
                    this.StatusMessage = MessageConstant.GetText("M000038");
                    return status;
                }
                communications.CurrentTime = DateTime.Now;
                if (communications.SignOn())
                {
                    communications.DelayExecution();
                    communications.Command = command.ManufactureCommand;
                    communications.SendCommand();
                    if (communications.ResponseSignOn != string.Empty)
                    {
                        if (ValidationProvider.ValidateData(communications.ResponseSignOn, ValidationConstant.GeneralDateExpression) == false)
                        {
                            isCorruptedData = true;
                            this.StatusMessage = MessageConstant.GetText("M000039");
                            return status;
                        }
                    }
                    int index = communications.ResponseSignOn.IndexOf("/");
                    Thread.Sleep(200);
                    communications.ComPort.BaudRate = ReadoutCommon.GetBaudRate(communications.ResponseSignOn.Substring(index + 4, 1));
                    
                    communications.OutBuffer = string.Empty;
                    if (communications.ReadFlag == true)
                    {
                        communications.ReadFlag = false;
                        communications.OutBuffer = string.Empty;
                        communications.Command = command.LCommand;
                        communications.CommandID = 2;
                        if (!communications.SendCommand())
                            this.StatusMessage = MessageConstant.GetText("M000060");
                        else
                        {
                            do
                            {
                                if (communications.Timeout())
                                {
                                    this.StatusMessage = MessageConstant.GetText("M000040");
                                    if (IsAborted == true)
                                        IsAborted = false;
                                    else
                                        this.StatusMessage = MessageConstant.GetText("M000041");
                                    communications.ReadFlag = true;
                                }
                                if (communications.ReadFlag)
                                {
                                    if (communications.OutBuffer.Length > 26)
                                    {
                                        responseForLoadSurvey = communications.OutBuffer.Substring(0, 21);
                                        string stDate = string.Empty;
                                        lcommandResponse = string.Empty;
                                        for (index = 21; index < communications.OutBuffer.Length - 3; index += 6)
                                        {
                                            string resp = communications.OutBuffer.Substring(index, 6);
                                            lcommandResponse = lcommandResponse + resp;
                                            if (index == 21)
                                                stDate = communications.OutBuffer.Substring(index, 6);
                                        }
                                        status = true;
                                        string edDate = lcommandResponse.Substring(lcommandResponse.Length - 6, 6);
                                        this.StartDate = DateUtility.ConvertIntToDate(Convert.ToInt32(stDate.Substring(0, 2)), Convert.ToInt32(stDate.Substring(2, 2)), Convert.ToInt32(string.Concat("20", stDate.Substring(4, 2))));
                                        this.EndDate = DateUtility.ConvertIntToDate(Convert.ToInt32(edDate.Substring(0, 2)), Convert.ToInt32(edDate.Substring(2, 2)), Convert.ToInt32(string.Concat("20", edDate.Substring(4, 2))));
                                        this.StatusMessage = string.Empty;
                                        if (Convert.ToDouble((EndDate - StartDate).TotalSeconds) >= 0)
                                            IsDateChanged = true;
                                        else
                                            IsDateChanged = false;
                                    }
                                    else
                                    {
                                        IsDateChanged = false;
                                        return false;
                                    }
                                    break;
                                }
                            } while (true);
                        }
                    }
                }
                else
                {
                    IsSignOnFailure = true;
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            finally
            {
                communications.DelayExecution();
                communications.ClosePort();
            }
            return status;
        }
    }
}
