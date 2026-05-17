using System;
using System.Collections.Generic;
using System.Text;
using CAB.IECFramework;
using CAB.IECFramework.Utility;
using System.Threading;
using System.Windows.Forms;
using CAB.UI.Controls;
using CAB.IECChannel.ReadOut;
using System.IO;

namespace CAB.IECChannel.CMRI
{
	public class PrepareCMRI : ReadBase
	{
		private IECChannelBase communications;

		public string ScheduleFileCommand { get; set; }
		public string TOUFileName { get; set; }
		public string TOUFileCommand {get;set;}

		public PrepareCMRI()
		{
			command = Command.GetInstance();
		}

		public IECChannelBase Channel
		{
			get { return communications; }
			set { communications = value; }
		}

		public bool PrepareCMRIData()
		{
            bool flag = false;
			char charACK;
			string sendcmd = "70726570617265636D7269"; //preparecmri
			string completeCommand = "676574636D70"; //getcmp this is the final Command for the CMRI Preparation
			int strPosition = 0;
			string ETX = "03";
            string baudRate = ConfigSettings.GetValue("BaudRate");
			communications.BaudRate = int.Parse(baudRate);
            communications.Parity = System.IO.Ports.Parity.None;
            communications.DataBits = 8;

			if (!communications.OpenPort())
			{
				this.StatusMessage = MessageConstant.GetText("M000038");
			}
			else
			{
				this.StatusMessage = "Preparing CMRI...";
				Application.DoEvents();
				communications.CommandID = 0;
				communications.Command = sendcmd;
				communications.OutBuffer = string.Empty;

				//First Command "preparecmri"
				if (communications.SendCommand())
				{
					communications.IsDataReceived = false;
					communications.CurrentTime = DateTime.Now;
					Application.DoEvents();

					do
					{
						if (communications.Timeout())
						{
							Application.DoEvents();
                            communications.ClosePort();
							break;
						}
					} while (true);//!communications.Timeout());

					if (communications.OutBuffer.Length == 0)
					{
						communications.OutBuffer = string.Empty;
                        return flag;
					}

					//Set the CMRI RTC
					if (communications.OutBuffer.Length >= 1)
					{
						charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
						if (charACK == 6)
						{
							communications.OutBuffer = string.Empty;
							communications.Command = GetSystemRtc() + ETX;
							if (communications.SendCommand())
							{
								communications.IsDataReceived = false;
								communications.CurrentTime = DateTime.Now;
							
								do
								{
									if (communications.Timeout())
									{
										Application.DoEvents();
                                        communications.ClosePort();
										break;
									}
								} while (true);//!communications.Timeout());
								
								//The query is sent for the second time for setting the System RTC if the value is not available 
								if (communications.OutBuffer.Length == 0)
								{
									communications.OutBuffer = string.Empty;
									communications.Command = GetSystemRtc() + ETX;
									if (communications.SendCommand())
									{
										communications.IsDataReceived = false;
										communications.CurrentTime = DateTime.Now;

										do
										{
											if (communications.Timeout())
											{
												Application.DoEvents();
                                                communications.ClosePort();
                                                break;
											}
										} while (true);//!communications.Timeout());
									}
								}

								if (communications.OutBuffer.Length == 0)
								{
									communications.OutBuffer = string.Empty;
                                    return flag;
								}

								//Sending the Schedule File from the Data Generated 
								if (communications.OutBuffer.Length >= 1)
								{
									charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
									if (charACK == 6)
									{
//										Thread.Sleep(400);
										communications.OutBuffer = string.Empty;

										/*If the Command for sending the schedule is greater
										 *than 1024 bytes then the value is sent in another packet

										 *Creating The Command in StrCmd Array in form of Pkt.we send 512 
										 *byte in each pkt but each Byte contain 2 charactor so 512*2=1024*/

										strPosition = 0;
										string[] strCmd = new string[1024];
										while (ScheduleFileCommand.Length > 0)
										{
                                            //if (ScheduleFileCommand.Length >= 1024)
                                            //{
                                            //    strCmd[strPosition] = ScheduleFileCommand.Substring(0, 1024);
                                            //    ScheduleFileCommand = ScheduleFileCommand.Substring(1024);
                                            //}
                                            //else
                                            //{
												strCmd[strPosition] = ScheduleFileCommand;
												ScheduleFileCommand = "";
											//}
											strPosition++;
										}

										strPosition = 0;
										communications.OutBuffer = string.Empty;
										Thread.Sleep(400);
										string cfgCommand;

										do
										{
											communications.OutBuffer = string.Empty;
											communications.Command = strCmd[strPosition];
                                            communications.CommandID = 3;
											communications.SendCommand();
											communications.ReadFlag = false;
											communications.IsDataReceived = false;
										    Thread.Sleep(500);
                                            communications.CurrentTime = DateTime.Now;
											cfgCommand = string.Empty;
											
                                            do
											{

												cfgCommand = communications.OutBuffer;
												if (cfgCommand.Length > 0)
													break;

											} while (!communications.Timeout());//Changed for the While(True) condition to time Out
											
                                            Application.DoEvents();

											if (communications.Timeout() == true)
												break;

											if (cfgCommand.Length > 0)
											{
												charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
												if (charACK != 6)
													break;
												strPosition++;
												this.StatusMessage = "Sending Schedule File...";
												Application.DoEvents();
											}

										} while (strCmd[strPosition] != null);

										//communications.OutBuffer = string.Empty;
										communications.ReadFlag = false;
										communications.IsDataReceived = false;
                                        Thread.Sleep(500);
										communications.CurrentTime = DateTime.Now;

										do
										{
											if (communications.OutBuffer.Length > 0)
												break;
										} while (!communications.Timeout()); //Instead of While (True) condition I have changed this

										if (communications.OutBuffer.Length == 0)
										{
											communications.OutBuffer = string.Empty;
											return flag;
										}

										if (communications.OutBuffer.Length > 0)
										{
											if (TOUFileName == string.Empty)
											{
												//Command for final if the Schedule is not available else the schedule is not sent 
												communications.Command = completeCommand; //getcmp 
												communications.SendCommand();
												flag = true;
											}
											else
											{
												Application.DoEvents();
                                                if (SendTOUFileToCMRI())
                                                    flag = true;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			communications.ClosePort();
			return flag;
		}

		//Configuring TOU File to the CMRI for Preparation
		public bool SendTOUFileToCMRI()
		{
            bool flag = false;
			char charACK;
			string sendCommand = "676574746F75"; //gettou
			int stringPosition = 0;

            string baudRate = ConfigSettings.GetValue("BaudRate");
            communications.BaudRate = int.Parse(baudRate);
            communications.Parity = System.IO.Ports.Parity.None;
            communications.DataBits = 8;

			if (!communications.OpenPort())
			{
				this.StatusMessage = MessageConstant.GetText("M000038");
                return flag;
			}
			else
			{
				communications.CommandID = 0;
				communications.Command = sendCommand; //gettou
				communications.OutBuffer = string.Empty;
				//first send the gettou

				if (communications.SendCommand())
				{
					communications.IsDataReceived = false;
                    Thread.Sleep(500);
					communications.CurrentTime = DateTime.Now;
                    
					do
					{
						if (communications.Timeout())
						{
							Application.DoEvents();
							break;
						}
					} while (!communications.Timeout());

					if (communications.OutBuffer.Length >= 1)
					{
						charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
						if (charACK == 6)
						{

							communications.OutBuffer = string.Empty;
							stringPosition = 0;
							/*Creating The Command in StrCmd Array in form of Pkt.we send 512 
										*byte in each pkt but each Byte contain 2 charactor so 512*2=1024*/
							string[] strCmd = new string[1024];
							while (TOUFileCommand.Length > 0)
							{
								if (TOUFileCommand.Length >= 1024)
								{
									strCmd[stringPosition] = TOUFileCommand.Substring(0, 1024);
									TOUFileCommand = TOUFileCommand.Substring(1024);
								}
								else
								{
									strCmd[stringPosition] = TOUFileCommand;
									TOUFileCommand = "";
								}
								stringPosition++;
							}

							stringPosition = 0;
							communications.OutBuffer = string.Empty;
							string tOUCommand;
							
							do
							{
								communications.OutBuffer = string.Empty;
								communications.Command = strCmd[stringPosition];
                                communications.CommandID = 0;
                                communications.SendCommand();
                                Thread.Sleep(800);
                                communications.ReadFlag = false;
                                communications.IsDataReceived = false;
                                communications.CurrentTime = DateTime.Now;
                                
                                
								tOUCommand = string.Empty;
								do
								{
									tOUCommand = communications.OutBuffer;
									if (tOUCommand.Length > 0)
										break;

                                } while (!communications.Timeout());

								if (communications.Timeout() == true)
									break;
								if (tOUCommand.Length > 0)
								{
									charACK = Convert.ToChar(communications.OutBuffer.Substring(0, 1));
									if (charACK != 6)
										break;
									stringPosition++;
									this.StatusMessage = "Sending Schedule File...";
									Application.DoEvents();
								}
							} while (strCmd[stringPosition] != null);

							communications.ReadFlag = false;
							communications.IsDataReceived = false;
                            Thread.Sleep(500);
							communications.CurrentTime = DateTime.Now;
							
							do
							{
                                if (communications.OutBuffer.Length > 0)
                                {
                                    flag = true;
                                    break;
                                }
							} while (communications.Timeout());
						}
					}
				}
			}
			communications.ClosePort();
            return flag; ;
		}

        //public bool ClearCMRIData()
        //{
        //    bool flag = false;
        //    string sendClearCommand = "636C726462"; //clrdb
        //    try
        //    {
        //        communications.BaudRate = 9600;
        //        if (!communications.OpenPort())
        //        {
        //            this.StatusMessage = MessageConstant.GetText("M000038");
        //            flag = false;
        //        }
        //        else
        //        {
        //            communications.CommandID = 0;
        //            communications.Command = sendClearCommand;
        //            communications.OutBuffer = string.Empty;

        //            if (communications.SendCommand())
        //            {
        //                communications.IsDataReceived = false;
        //                communications.CurrentTime = DateTime.Now;

        //                do
        //                {
        //                    if (communications.Timeout())
        //                    {
        //                        Application.DoEvents();
        //                        break;
        //                    }
        //                } while (true);//!communications.Timeout());

        //                if (communications.OutBuffer.Length > 0)
        //                    flag = true;
        //                else
        //                    flag = false;
        //                if (communications.OutBuffer == "clrdb")
        //                    flag = false;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        communications.Command = command.BreakCommand;
        //        communications.SendCommand();
        //        communications.DelayExecution();
        //        communications.ClosePort();
        //    }
        //    return flag;
        //}

        public string ClearCMRIData()
        {
            string flag = string.Empty;
            string sendClearCommand = "636C726462"; //clrdb
            try
            {
                communications.BaudRate = 9600;
                if (!communications.OpenPort())
                {
                    flag = MessageConstant.GetText("M000038");
                    this.StatusMessage = MessageConstant.GetText("M000038");

                }
                else
                {
                    communications.CommandID = 0;
                    communications.Command = sendClearCommand;
                    communications.OutBuffer = string.Empty;

                    if (communications.SendCommand())
                    {
                        communications.IsDataReceived = false;
                        communications.CurrentTime = DateTime.Now;

                        do
                        {
                            if (communications.Timeout())
                            {
                                flag = "Sign-On Failure!";
                                Application.DoEvents();
                                break;
                            }
                        } while (true);//!communications.Timeout());

                        if (communications.OutBuffer.Length > 0)
                            flag = string.Empty;
                        else
                            flag = "Sign-On Failure!";
                        if (communications.OutBuffer == "clrdb")
                            flag = "Error in Communication";
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                communications.Command = command.BreakCommand;
                communications.SendCommand();
                communications.DelayExecution();
                communications.ClosePort();
            }
            return flag;
        }


	   /******************************************************************************
        *    Function Name    : GetSystemRtc
        *    Description      : In This Function Read The System Current RTC and Prepare     
        *                       Update CMRI RTC Commands For Updating CMRI RTC.
        *******************************************************************************/
        public string GetSystemRtc()
        {
            string sysrtc = string.Empty;
            string strrtc = string.Empty;
            /*Day*/
            strrtc = DateTime.Now.Day.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Month*/
            strrtc = DateTime.Now.Month.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Year*/
            strrtc = DateTime.Now.Year.ToString("0000");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(2, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(3, 1)) + 30).ToString());
            /*Hour*/
            strrtc = DateTime.Now.Hour.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            /*Minut*/
            strrtc = DateTime.Now.Minute.ToString("00");
            sysrtc += ((Convert.ToInt16(strrtc.Substring(0, 1)) + 30).ToString()) + ((Convert.ToInt16(strrtc.Substring(1, 1)) + 30).ToString());
            return sysrtc;
        }
	}
}
