using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SerializationTest
{
    [Serializable]
    class SerializableClass : ISerializable
    {
        private List<string> someData = null;

        public SerializableClass()
        {
            someData = new List<string>() { "aa", "ba", "ca", "da", "ea", "fa" };
        }

        public SerializableClass(SerializationInfo info, StreamingContext context)
        {
            someData = (List<string>)info.GetValue("someData", typeof(List<String>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("someData", someData);
        }

        public List<string> Data
        {
            get
            {
                return someData;
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var instance = new SerializableClass();
            BinaryFormatter formatter = new BinaryFormatter();

            MemoryStream outputStream = new MemoryStream();

            formatter.Serialize(outputStream, instance);

            var serializedData = outputStream.ToArray();

            var inputStream = new MemoryStream(serializedData);

            var deserialized = (SerializableClass)formatter.Deserialize(inputStream);

            for (int i = 0; i < instance.Data.Count(); ++i)
            {
                if(false == instance.Data[i].Equals(deserialized.Data[i]))
                {
                    throw new SerializationException();
                }
            }
            Console.Out.WriteLine("Deserialization successfull");
            Console.In.Read();
        }
    }
}
