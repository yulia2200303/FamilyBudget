using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace UI.Logic
{
    public static class SerializeWrapper<T> where T : class
    {
        public static string Serialize(T obj)
        {
            using (var memoryStream = new MemoryStream())
            using (var reader = new StreamReader(memoryStream))
            {
                var serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        public static T Deserialize(string xml)
        {
            using (var stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }
    }
}
