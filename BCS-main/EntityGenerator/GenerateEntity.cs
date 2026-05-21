#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;

using CAB.Parser;
using CAB.Serialization;
using CAB.Framework.Utility;

#endregion

namespace CAB.EntityGenerator
{
    /// <summary>
    /// This class is responsible for generating final entity that will be
    /// used for mapping 2ng file data into database tables.
    /// This class will utilize parsing/Configuration module to do its tasks.
    /// </summary>
    public class GenerateEntity
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private static object syncRoot = new object();
        private DLMS commandRepository = null;
        private Serializer serializer = null;
        private const int unknowObisCode = -1;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public GenerateEntity()
        {
            serializer = new Serializer();
            if (commandRepository == null)
            {
                commandRepository = (DLMS)serializer.DeserializeToObject(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "CommandRepository.xml"), typeof(DLMS));
            }

        }
        #endregion

        #region Public Methods

        public List<ProfileData> GetProfileWiseEntityList(string fileContent, bool isDynamicData, int MeterModelType)
        {
            List<ProfileData> profileCollection = new List<ProfileData>();
            //Adding try catch to let the file get loaded till the last profile, 
            //when profile data is corrupt.
            try
            {
                ConfigInfo.EnergyResolution = 0;
                ConfigInfo.DemandResolution = 0;
                ProfileData individualProfile = null;
                string[] allProfileData = fileContent.Split('\n');
                BaseParser payloadParser = new BaseParser(true);
                int commandIndex = 0;
                int allProfileDataCounter;
                if (isDynamicData)
                {
                    allProfileDataCounter = 2;
                    commandIndex = 1;
                }
                else
                {
                    allProfileDataCounter = allProfileData.Length - 2;
                }
                while (commandIndex < allProfileDataCounter)
                {
                    if (!string.IsNullOrEmpty(allProfileData[commandIndex]))
                    {
                        individualProfile = new ProfileData();
                        DLMSCOMMAND command = null;                        
                        command = GetCommandFromCommandRepository(allProfileData[commandIndex].Substring(0, 16), MeterModelType);
                       
                        if (command == null)
                        {
                            commandIndex++;
                            continue;
                        }
                        individualProfile = payloadParser.ParseProfile(allProfileData, command, ref commandIndex);
                        if (individualProfile == null) continue;
                        individualProfile.ProfileId = Convert.ToInt32(command.TAGNO);
                        // this condition is used for unit testing
                      
                        profileCollection.Add(individualProfile);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return profileCollection;
        }


        /// <summary>
        /// /// Used to create collection of  ProfileData entity that will represent final 
        /// entity for an input 2NGfile data or dynamic reradout data .
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="isDynamicData"></param>
        /// <returns></returns>
        public List<ProfileData> GetProfileWiseEntityList(string fileContent,bool isDynamicData)
        {
            List<ProfileData> profileCollection = new List<ProfileData>();      
            //Adding try catch to let the file get loaded till the last profile, 
            //when profile data is corrupt.
            try
            {
                ConfigInfo.EnergyResolution = 0;
                ConfigInfo.DemandResolution = 0;
                ProfileData individualProfile = null;
                string[] allProfileData = fileContent.Split('\n');                
                BaseParser payloadParser = new BaseParser(true);
                int commandIndex = 0;
                int allProfileDataCounter;
                if (isDynamicData)
                {
                    allProfileDataCounter = 2;
                    commandIndex = 1;
                }
                else
                {
                    allProfileDataCounter = allProfileData.Length - 2;
                }
                while (commandIndex < allProfileDataCounter)
                {
                    if (!string.IsNullOrEmpty(allProfileData[commandIndex]))
                    {
                        individualProfile = new ProfileData();
                        DLMSCOMMAND command = null;                       
                        command = GetCommandFromCommandRepository(allProfileData[commandIndex].Substring(0, 16));                        
                        if (command == null)
                        {
                            commandIndex++;
                            continue; 
                        }
                        //**********Sapphire S2 Kvah,Autobilling,Softwarebilling Implementation with same OBIS****
                        if ("00.00.60.01.8E.FF" == command.OBISCODE)
                        {
                            int subprofilecounts = 4;
                            for (int i = 0; i < subprofilecounts; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        command.TAGNO = "107";
                                        command.CLASSNAME = "CAB.E650MeterConfiguration.KVAHSelectionS2,E650MeterConfiguration";
                                        break;
                                    case 1:
                                        command.TAGNO = "121";
                                        command.CLASSNAME = "CAB.E650MeterConfiguration.AutoLockUnlockS2,E650MeterConfiguration";
                                        break;
                                    case 2:
                                        command.TAGNO = "135";
                                        command.CLASSNAME = "CAB.E650MeterConfiguration.SoftwareBillingS2,E650MeterConfiguration";
                                        break;
                                    case 3:
                                        command.TAGNO = "189";
                                        command.CLASSNAME = "CAB.E650MeterConfiguration.ManualButtonMDResetS2,E650MeterConfiguration";
                                        break;
                                }
                                individualProfile = payloadParser.ParseProfile(allProfileData, command, ref commandIndex);
                               if (i< subprofilecounts-1) commandIndex -= 1;
                                if (individualProfile == null) continue;
                                individualProfile.ProfileId = Convert.ToInt32(command.TAGNO);
                                profileCollection.Add(individualProfile);
                            }
                        }
                        else
                        {
                            individualProfile = payloadParser.ParseProfile(allProfileData, command, ref commandIndex);
                            if (individualProfile == null) continue;
                            individualProfile.ProfileId = Convert.ToInt32(command.TAGNO);
                            // this condition is used for unit testing

                            if (command.OBISCODE == "00.00.60.01.BC.FF")
                            {
                                CommonMethods.GetDisplayProgrammingVariantFromSignature(individualProfile.ListMeterDataPacket[0].ListDataElementValue[0].Value);
                            }

                            profileCollection.Add(individualProfile);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return profileCollection;
        }

       

        private DLMSCOMMAND GetCommandFromCommandRepository(string obisInfo, int meterModel)
        {
            DLMSCOMMAND command = null;
            byte classID = Convert.ToByte(obisInfo.Substring(0, 2), 16);
            string obisCode = obisInfo.Substring(2, 12);
            byte attribute = Convert.ToByte(obisInfo.Substring(14, 2), 16);
            foreach (DLMSCOMMAND dlmsCommand in commandRepository.Items)
            {
                if ((dlmsCommand.OBISCODE.Replace(".", "").ToUpper().Replace("METERID", "FF") == obisCode)
                    && (Convert.ToByte(dlmsCommand.CLASS) == classID)
                    && (Convert.ToByte(dlmsCommand.ATTRIBUTE) == attribute)
                    && (dlmsCommand.METERMODEL == meterModel.ToString() || dlmsCommand.METERMODEL == "0"))
                {
                    command = dlmsCommand;
                    break;
                }
            }
            return command;
        }

        /// <summary>
        /// Gets DLMS Command object from command repository using OBIS code, classId , attribute.
        /// </summary>
        /// <param name="obisCode"></param>
        /// <param name="classID"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public DLMSCOMMAND GetCommandFromCommandRepository(string obisInfo)
        {
            
            DLMSCOMMAND command = null;
            byte classID = Convert.ToByte(obisInfo.Substring(0, 2), 16);
            string obisCode = obisInfo.Substring(2, 12);
            byte attribute = Convert.ToByte(obisInfo.Substring(14, 2), 16);
           
            foreach (DLMSCOMMAND dlmsCommand in commandRepository.Items)
            {
                if ((dlmsCommand.OBISCODE.Replace(".", "").ToUpper().Replace("METERID", "FF") == obisCode)
                    && (Convert.ToByte(dlmsCommand.CLASS) == classID)
                    && (Convert.ToByte(dlmsCommand.ATTRIBUTE) == attribute))
                {
                    command = dlmsCommand;
                    break;
                }
            }
            return command;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods




        /// <summary>
        /// Gets file content as string from input file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetFileContent(string filePath)
        {
            string fileContent = string.Empty;
            if (File.Exists(filePath))
            {
                StreamReader streamReader = new StreamReader(filePath);
                fileContent = streamReader.ReadToEnd();
                streamReader.Close();
            }
            return fileContent;
        }       
        #endregion
    }
}
