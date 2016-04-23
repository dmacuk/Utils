using System.IO;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace Utils.File
{
    public class JsonUtils<T>
    {
        private readonly string _fileName;

        public JsonUtils(string fileName)
        {
            _fileName = fileName;
        }

        public static T ReadJsonString(string json)
        {
            using (var reader = new StringReader(json))
            {
                var serializer = new JsonSerializer<T>();
                return serializer.DeserializeFromReader(reader);
            }
        }

        public static string WriteJsonString(T obj)
        {
            var serializer = new JsonSerializer<T>();
            return serializer.SerializeToString(obj);
        }

        public T ReadObject()
        {
            using (var reader = new StreamReader(_fileName))
            {
                var serializer = new JsonSerializer<T>();
                return serializer.DeserializeFromReader(reader);
            }
        }

        public Task<T> ReadObjectAsync()
        {
            var t1 = new Task<T>(ReadObject);

            t1.Start();
            return t1;
        }

        public void WriteObject(T obj)
        {
            using (var writer = new StreamWriter(_fileName))
            {
                var serializer = new JsonSerializer<T>();
                serializer.SerializeToWriter(obj, writer);
            }
        }

        public Task WriteObjectAsync(T obj)
        {
            var t1 = new Task(() => { WriteObject(obj); });

            t1.Start();
            return t1;
        }
    }
}