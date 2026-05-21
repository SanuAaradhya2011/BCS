#region Namespaces
using System;
using System.IO;
using System.Xml.Serialization;
#endregion
namespace CABFramework
{
    public class XmlSerializeDeSerialize
    {
        #region Nested Types
        #endregion

        #region Constants & Variables
        private StringReader strReader = null;
        private XmlSerializer xmlSerializer = null;
        private FileStream fileStream = null;
        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Used to Deserialize xml file into  C# object
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlClassType"></param>
        /// <returns></returns>
        public object DeserializeToObject(string xmlFilePath, Type xmlClassType)
        {            
            try
            {                
                if (!xmlFilePath.Contains("\\"))
                {
                    xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + xmlFilePath;
                }
                xmlSerializer = new XmlSerializer(xmlClassType);               
                string xmlString = string.Empty;
                using (StreamReader reader = new StreamReader(xmlFilePath))
                {
                    xmlString = reader.ReadToEnd();                    
                }               
                strReader = new StringReader(xmlString);
                return Convert.ChangeType(xmlSerializer.Deserialize(strReader), xmlClassType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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
        /// Check validity of current path
        /// </summary>
        /// <param name="folder"></param>
        private void CheckCurrentPath(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION OCCURED WHILE CREATING FOLDER : " + folder + " : " + ex.Message);
            }
        }
        /// <summary>
        /// Used to convert c# object into xml file .
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="xmlFilePath"></param>
        /// <param name="ObjectToSerialize"></param>
        /// <returns></returns>
        public bool SerializeObjectToFile(string folder, string xmlFilePath, object ObjectToSerialize)
        {
            
            XmlSerializer xmlSerializer = null;
            bool resultOfConversion = false;
            xmlFilePath = xmlFilePath.Trim();
            Console.WriteLine("File to serialize : " + xmlFilePath);
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
            nameSpaces.Add(string.Empty, string.Empty);
            if (!string.IsNullOrEmpty(folder))
            {
                CheckCurrentPath(folder);
            }
            else
            {
                Console.WriteLine("Creating folder : " + xmlFilePath.Substring(0, xmlFilePath.LastIndexOf('\\')));
                CheckCurrentPath(xmlFilePath.Substring(0, xmlFilePath.LastIndexOf('\\')).Trim());
            }
            try
            {              

                xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());               
                if (!string.IsNullOrEmpty(folder))
                    xmlFilePath = folder + "\\" + xmlFilePath;
                Console.WriteLine("SERIALIZING FILE TO " + folder);
                fileStream = new FileStream(xmlFilePath, FileMode.Create);
                Console.WriteLine("File : " + xmlFilePath);                
                xmlSerializer.Serialize(fileStream, ObjectToSerialize, nameSpaces);
                resultOfConversion = true;
                return resultOfConversion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION OCCURED WHILE SERIALIZING FILE : " + ex.Message);
                resultOfConversion = false;
                return resultOfConversion;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        /// <summary>
        /// Converts objects into string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
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
