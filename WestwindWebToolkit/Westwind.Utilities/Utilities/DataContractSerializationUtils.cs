using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace Westwind.Utilities
{
    /// <summary>
    /// Serialization routines that use the DataContractSerializer for 
    /// serialization. Use these on anything that has an explicitly defined
    /// DataContract or generated LINQ entities (LINQ to SQL/Entites etc.)
    /// </summary>
    public class DataContractSerializationUtils
    {
        [ThreadStatic]
        public static string LastError = string.Empty;

        /// <summary>
        /// Serializes to Xml String using the DataContractSerializer
        /// 
        /// Use on DataContracts and LINQ to Sql objects
        /// </summary>
        /// <param name="value"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public static string SerializeToXmlString(object value, bool throwExceptions)
        {
            DataContractSerializer ser = new DataContractSerializer(value.GetType(),null,int.MaxValue,true,false,null);     
            
            MemoryStream ms = new MemoryStream();
            if (throwExceptions)
                ser.WriteObject(ms, value);
            else
            {
                try
                {
                    ser.WriteObject(ms, value);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return null;
                }
            }
            return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
        }

        /// <summary>
        /// Serializes to Binary using the DataContractSerializer
        /// 
        /// Use on DataContracts and LINQ to Sql objects
        /// </summary>
        /// <param name="value"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object value, bool throwExceptions)
        {
            DataContractSerializer ser = new DataContractSerializer(value.GetType());
            MemoryStream ms = new MemoryStream();
            if (throwExceptions)
                using (XmlDictionaryWriter w = XmlDictionaryWriter.CreateBinaryWriter(ms))
                    ser.WriteObject(ms, value);
            else
            {
                try
                {
                    using (XmlDictionaryWriter w = XmlDictionaryWriter.CreateBinaryWriter(ms))
                        ser.WriteObject(ms, value);
                }
                catch(Exception ex)
                {

                    LastError = ex.Message; 
                    return null;
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        /// Serializes output into a stream that you pass in. You provide the stream
        /// and this method writes the data to it.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <param name="throwExceptions"></param>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static bool SerializeToStream(object value, Stream stream, bool binary, bool throwExceptions)
        {
            DataContractSerializer ser = new DataContractSerializer(value.GetType());            

            if (binary)
            {
                if (throwExceptions)
                    using (XmlDictionaryWriter w = XmlDictionaryWriter.CreateBinaryWriter(stream))
                        ser.WriteObject(w, value);
                else
                {
                    try
                    {
                        using (XmlDictionaryWriter w = XmlDictionaryWriter.CreateBinaryWriter(stream))
                            ser.WriteObject(stream, value);
                    }
                    catch(Exception ex)
                    {
                        LastError = ex.Message; 
                        return false;
                    }
                }
            }
            else
            {                
                if (throwExceptions)
                        ser.WriteObject(stream, value);
                else
                {
                    try
                    {                        
                        ser.WriteObject(stream, value);                       
                    }
                    catch(Exception ex)
                    {
                        LastError = ex.Message; 
                        return false;
                    }
                }
            }            

            return true;
        }


        /// <summary>
        /// Deserializes Xml from a string into an object
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DeserializeXmlString(string xml, Type type, bool throwExceptions)
        {
           using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml))  )
           {
               return DeserializeStream(ms,type,false,throwExceptions); 
           }           
        }

        /// <summary>
        /// Deserialize byte[] data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public static object DeserializeBinary(byte[] data, Type type, bool throwExceptions)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return DeserializeStream(ms, type, false, throwExceptions);
            }            
        }
        /// <summary>        
        /// Deserializes from a stream
        /// </summary>
        /// <param name="strem"></param>
        /// <param name="type"></param>
        public static object DeserializeStream(Stream stream, Type type, bool binary, bool throwExceptions)
        {
            DataContractSerializer ser = new DataContractSerializer(type);

            object value = null;
            if (throwExceptions)
                value = ser.ReadObject(stream);
            else
            {
                try
                {
                    value = ser.ReadObject(stream);
                }
                catch(Exception ex)
                {
                    LastError = ex.Message;
                    return null;
                }
            }

            return value;
        }


    }
}
