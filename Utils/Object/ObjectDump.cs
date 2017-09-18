using System.Collections.Generic;
using System.IO;

namespace Utils.Object
{
    public static class ObjectExtensions
    {
        public static void Dump(this object obj, TextWriter writer)
        {
            if (obj == null)
            {
                writer.WriteLineAsync("Object is null");
                return;
            }

            writer.WriteLine($"Hash: {obj.GetHashCode()} Type: {obj.GetType()}");

            var props = GetProperties(obj);

            if (props.Count > 0)
            {
                writer.WriteLine("-------------------------");
            }

            foreach (var prop in props)
            {
                writer.WriteLineAsync($"{prop.Key}: {prop.Value}");
            }
        }

        private static Dictionary<string, string> GetProperties(object obj)
        {
            var props = new Dictionary<string, string>();
            if (obj == null)
                return props;

            var type = obj.GetType();
            foreach (var prop in type.GetProperties())
            {
                var val = prop.GetValue(obj, new object[] { });
                var valStr = val?.ToString() ?? "";
                props.Add(prop.Name, valStr);
            }

            return props;
        }
    }
}