using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
namespace CAB.Framework.Utility
{
    public class CABSerializer
    {
        public object DeserializeToObject(string xmlFilePath, Type xmlClassType)
        {
            StringReader strReader = null;
            //FileStream fileStream = null;
            XmlSerializer xmlSerializer = null;
            try
            {
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                if (!xmlFilePath.Contains("\\"))
                {
                    xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + xmlFilePath;

                }
                xmlSerializer = new XmlSerializer(xmlClassType);
                //// To read the file, create a FileStream.
                //// BhardwajG : Open file in read mode.
                //fileStream = new FileStream(xmlFilePath, FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
                // Call the Deserialize method and cast to the object type.
                string xmlString = string.Empty;
                using (StreamReader reader = new StreamReader(xmlFilePath))
                {
                    xmlString = reader.ReadToEnd();
                    // after leaving the using scope the streamreader is disposed freeing the file
                }

                // if the serializer needs a TextReader then you can wrap it here
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
                //if (fileStream != null)
                //{
                //    fileStream.Close();
                //}

            }
        }
        public void CheckCurrentPath(string folder)
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
        public bool SerializeObjectToFile(string folder, string xmlFilePath, object ObjectToSerialize)
        {
            FileStream fileStream = null;
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
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.

                xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());
                // To read the file, create a FileStream.
                if (!string.IsNullOrEmpty(folder))
                    xmlFilePath = folder + "\\" + xmlFilePath;
                Console.WriteLine("SERIALIZING FILE TO " + folder);
                fileStream = new FileStream(xmlFilePath, FileMode.Create);
                Console.WriteLine("File : " + xmlFilePath);
                // Call the Deserialize method and cast to the object type.
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
        public string SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }


    }
}
