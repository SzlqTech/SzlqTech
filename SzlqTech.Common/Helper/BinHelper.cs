using Newtonsoft.Json;
using SzlqTech.Common.Context;

namespace SzlqTech.Common.Helper
{
    public class BinHelper
    {
        private const int BuffSize = 8192;

        private const string aesKey = "sieractech1234qazwerdfghvcd234q6t43";

        public static void Serialize<T>(T entity, string path)
        {
            string contents = JsonConvert.SerializeObject(entity).AESEncrypt("sieractech1234qazwerdfghvcd234q6t43");
            File.WriteAllText(path, contents);
        }

        public static T? Deserialize<T>(string path) where T : class
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path).AESDecrypt("sieractech1234qazwerdfghvcd234q6t43"));
        }

        public static void Serialize<T>(T entity) where T : class
        {
            string path = ConfigContext.Root + "\\" + ConfigContext.Config + "\\" + typeof(T).Name + ".bin";
            Serialize(entity, path);
        }

        public static T? Deserialize<T>() where T : class
        {
            return Deserialize<T>(ConfigContext.Root + "\\" + ConfigContext.Config + "\\" + typeof(T).Name + ".bin");
        }

        public static void Save<T>(T entity) where T : class
        {
            string text = ConfigContext.Root + "\\" + ConfigContext.Config + "\\" + typeof(T).Name + ".bin";
            string text2 = Path.GetDirectoryName(text) ?? throw new InvalidOperationException("无法获取文件夹[" + text + "]");
            if (string.IsNullOrEmpty(text2))
            {
                throw new ArgumentNullException("文件路径为空");
            }

            if (!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }

            Serialize(entity, text);
        }

        public static void Save<T>(T entity, string path) where T : class
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("文件路径为空");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Serialize(entity, path);
        }

        public static T? Load<T>() where T : class, new()
        {
            string path = ConfigContext.Root + "\\" + ConfigContext.Config + "\\" + typeof(T).Name + ".bin";
            if (!File.Exists(path))
            {
                T val = new T();
                Save(val);
                return val;
            }

            return Deserialize<T>(path);
        }

        public static T? Load<T>(string path) where T : class, new()
        {
            if (!File.Exists(path))
            {
                T val = new T();
                Save(val);
                return val;
            }

            return Deserialize<T>(path);
        }
    }
}
