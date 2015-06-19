using Newtonsoft.Json;

namespace DMSoftware.Object
{
    public class ObjectUtils
    {
        public string Serialise<T>(T t, bool encrypt)
        {
            var json = JsonConvert.SerializeObject(t);
            return json;
        }
    }
}
