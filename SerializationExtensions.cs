using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TurkishLanguageLibrary.Conversions
{
    public static class SerializationExtensions
    {
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();
        public static byte[] BinarySerialize(this object obj)
        {
            using(var stream=new MemoryStream())
            {
                binaryFormatter.Serialize(stream, obj);
                return stream.ToArray();

            }
        }
        public static T BinaryDeserialize<T>(this byte[] byteArray)
        {
            using (var stream = new MemoryStream(byteArray))
            {
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

         public static byte[] XmlSerialize(this object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
         public static T XmlDeSerialize<T>(this byte[] byteArray)
         {
             var serializer = new XmlSerializer(typeof(T));
             using (var stream = new MemoryStream(byteArray))
             {
                 return (T)serializer.Deserialize(stream);
             }
         }
    }
}
