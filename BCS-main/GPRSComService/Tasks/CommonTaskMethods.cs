using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPRSComService.DLMSLIB;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace GPRSComService.Tasks
{
    class CommonTaskMethods
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(GPRSComService.Tasks.CommonTaskMethods).ToString());

        public static byte[] ReadScalarProfile(byte atb, byte nProfileindex, ref COSEMLIB objCOSEMLIB, ref HDLCLIB objHDLCLIB)
        {
            byte[] HDLCCommand = new byte[200];
            byte HDLCIndex = 0;

            objCOSEMLIB.nBlockIndex = 0;
            objCOSEMLIB.nTotalPacketSize = 0;
            objCOSEMLIB.nBlockNumber = 0;
            objCOSEMLIB.nBlockTotalByteCount = 0;

            try
            {
                HDLCIndex = 0;
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddHDLCFrameTag(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddServerSAP(HDLCCommand, HDLCIndex, Constants.nServerSAP, Constants.nServerLowerMacAddress);
                HDLCIndex = objHDLCLIB.fAddClientSAP(HDLCCommand, HDLCIndex, Constants.nClientSAP);
                objHDLCLIB.fIncSend();
                HDLCIndex = objHDLCLIB.fAddCmdByte(HDLCCommand, HDLCIndex);
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);

                HDLCIndex = objCOSEMLIB.fAddLLCByte(HDLCCommand, HDLCIndex);

                if (nProfileindex == 0)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryInstantScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 1)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryBillingScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 2)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryLoadSurveyScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 3)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryTamperScalarProfile(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 4)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKW(HDLCCommand, HDLCIndex, atb);
                }
                else if (nProfileindex == 5)
                {
                    HDLCIndex = objCOSEMLIB.GetQueryCumulativeScalarProfileKVA(HDLCCommand, HDLCIndex, atb);
                }
                ////added for MVVNL
                //else if (nProfileindex == 6)
                //{
                //    if (isPUMA)
                //    {
                //        HDLCIndex = objCOSEMLIB.GetQueryMidNightSacalarProfile(HDLCCommand, HDLCIndex, atb);
                //    }
                //    else
                //    {
                //        HDLCIndex = objCOSEMLIB.GetQueryMidnightDataScalarProfile(HDLCCommand, HDLCIndex, atb);
                //    }


                //}
                ////added for MVVNL
                //// added for Accuracy Check
                //else if (nProfileindex == 7)
                //{
                //    HDLCIndex = objCOSEMLIB.GetQueryAccuracyCheckScalarProfile(HDLCCommand, HDLCIndex, atb);
                //}
                // added for Accuracy Check
                HDLCIndex = objHDLCLIB.fAddBlankFCS(HDLCCommand, HDLCIndex);
                objHDLCLIB.ffillLength(HDLCCommand, HDLCIndex);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, 8);
                objHDLCLIB.fFillFCS(HDLCCommand, 9, 10);
                objHDLCLIB.fGenerateFCS(HDLCCommand, 1, HDLCIndex - 3);
                objHDLCLIB.fFillFCS(HDLCCommand, HDLCIndex - 2, HDLCIndex - 1);
                HDLCIndex = objHDLCLIB.fAdd7E(HDLCCommand, HDLCIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Array.Resize(ref HDLCCommand, HDLCIndex );
            return HDLCCommand;
        }

        private static Profile generalProfile = null;
        private static Profile instantProfile = null;
        private static Profile billingProfile = null;
        private static Profile tamperProfile = null;
        private static Profile loadSurveyProfile = null;

        /// <summary>
        /// Return Profile object based on the type of Task General/Instant/Billing etc.
        /// </summary>
        /// <param name="methodType"></param>
        /// <returns></returns>
        public static Profile GetProfileXML(Type methodType)
        {
            Profile profile = null;
            if (methodType == typeof(GeneralTask))
            {
                profile = GetGeneralXML();
            }
            else if (methodType == typeof(InstantaneousTask))
            {
                profile = GetInstantaneousTask();
            }
            else if (methodType == typeof(BillingTask))
            {
                profile = GetBillingTask();
            }
            else if (methodType == typeof(TamperTask))
            {
                profile = GetTamperTask();
            }
            else if (methodType == typeof(LoadSurveyTask))
            {
                profile = GetLoadSurveyTask();
            }

            return profile;
        }

        /// <summary>
        /// Loads LoadSurveyProfile.xml and returns the Profile object by deserializing the xml file.
        /// </summary>
        /// <returns></returns>
        private static Profile GetLoadSurveyTask()
        {
            if (billingProfile == null)
            {
                try
                {
                    CABSerializer serializer = new CABSerializer();
                    billingProfile = (Profile)serializer.DeserializeToObject(@"LoadSurveyProfile.xml", typeof(Profile));
                    logger.Log(LOGLEVELS.Info, "Load survey task command structure loaded from xml file.");
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while loading Load survey command structure from xml file.", ex);
                }
            }
            return billingProfile;
        }

        /// <summary>
        /// Loads billingProfile.xml and returns the Profile object by deserializing the xml file.
        /// </summary>
        /// <returns></returns>
        private static Profile GetBillingTask()
        {
            if (billingProfile == null)
            {
                try
                {
                    CABSerializer serializer = new CABSerializer();
                    billingProfile = (Profile)serializer.DeserializeToObject(@"BillingProfile.xml", typeof(Profile));
                    logger.Log(LOGLEVELS.Info, "Billing task command structure loaded from xml file.");
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while loading Billing command structure from xml file.", ex);
                }
            }
            return billingProfile;
        }

        /// <summary>
        /// Loads Tamper.xml and returns the Profile object by deserializing the xml file.
        /// </summary>
        /// <returns></returns>
        private static Profile GetTamperTask()
        {
            if (tamperProfile == null)
            {
                try
                {
                    CABSerializer serializer = new CABSerializer();
                    tamperProfile = (Profile)serializer.DeserializeToObject(@"TamperProfile.xml", typeof(Profile));
                    logger.Log(LOGLEVELS.Info, "Tamper task command structure loaded from xml file.");
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while loading Tamper command structure from xml file.", ex);
                }
            }
            return tamperProfile;
        }

        /// <summary>
        /// Loads the InstantaneousProfile.xml and returns Profile object by deserializing the xml
        /// </summary>
        /// <returns></returns>
        private static Profile GetInstantaneousTask()
        {
            if (instantProfile == null)
            {
                try
                {
                    CABSerializer serializer = new CABSerializer();
                    instantProfile = (Profile)serializer.DeserializeToObject(@"InstantaneousProfile.xml", typeof(Profile));
                    logger.Log(LOGLEVELS.Info, "Instantaneous task command structure loaded from xml file.");
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while loading Instant command structure from xml file.", ex);
                }
            }
            return instantProfile;
        }

        /// <summary>
        /// Load the GeneralProfile.xml and return profile object after deserializing the xml.
        /// </summary>
        /// <returns></returns>
        private static Profile GetGeneralXML()
        {
            if (generalProfile == null)
            {
                try
                {
                    CABSerializer serializer = new CABSerializer();
                    generalProfile = (Profile)serializer.DeserializeToObject(@"GeneralProfile.xml", typeof(Profile));
                    logger.Log(LOGLEVELS.Info, "General task command structure loaded from xml file.");
                }
                catch (Exception ex)
                {
                    logger.Log(LOGLEVELS.Error, "Error while loading General command structure from General xml file.", ex);
                }
            }
            return generalProfile;
        }
       
    }
}
