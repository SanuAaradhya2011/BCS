/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									|
 * |											Date   : 12/03/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace CAB.Framework
{
    public class ExceptionSerializer
    {
        public ExceptionSerializer()
        {
        }

        public void DeleteFile(string filename)
        {
            File.Delete(filename);
        }

        public void SerializeObject(string filename, ExceptionSerialize objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);            
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public ExceptionSerialize DeSerializeObject(string filename)
        {
            ExceptionSerialize objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (ExceptionSerialize)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}