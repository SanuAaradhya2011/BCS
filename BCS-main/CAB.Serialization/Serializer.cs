using System;
using System.Xml.Serialization;
using System.IO;

namespace CAB.Serialization
{
    /// <summary>
    /// Contains serialization and deserialization functions for an object 
    /// </summary>
    public class Serializer
    {
        #region Nested Types
        #endregion

        #region Constants and variables
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Deserialize an an object xml to object
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlClassType"></param>
        /// <returns></returns>
        public object DeserializeToObject(string xmlFilePath, Type xmlClassType)
        {
            string xmlString = string.Empty;
            StringReader strReader = null;
            XmlSerializer xmlSerializer = null;
            try
            {
                xmlSerializer = new XmlSerializer(xmlClassType);                          
                using (StreamReader reader = new StreamReader(xmlFilePath))
                {
                    xmlString = reader.ReadToEnd();
                }
                strReader = new StringReader(xmlString);
                return Convert.ChangeType(xmlSerializer.Deserialize(strReader), xmlClassType);
            }
            catch
            {
                throw;

            }
            finally
            {
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
        }
        /// <summary>
        /// Serializes a object to an specified path
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="ObjectToSerialize"></param>
        /// <returns></returns>
        public bool SerializeObjectToFile(string xmlFilePath, object ObjectToSerialize)
        {
            FileStream fileStream = null;
            XmlSerializer xmlSerializer = null;
            bool resultOfConversion = false;
            xmlFilePath = xmlFilePath.Trim();
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);                   
            try
            {
                xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());              
                fileStream = new FileStream(xmlFilePath, FileMode.Create);
                xmlSerializer.Serialize(fileStream, ObjectToSerialize, nameSpaces);
                resultOfConversion = true;
            }
            catch
            {
                resultOfConversion = false;
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return resultOfConversion;
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        #endregion
    }


}
