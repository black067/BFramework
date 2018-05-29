using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BFramework.Tools
{
    /// <summary>
    /// 序列化工具, 用于序列化保存或读取指定的类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Exporter<T>
    {
        public static string Save(T item)
        {
            string name = string.Format("{0}.{1}", item.GetHashCode(), item.GetType().Name);
            Save(name, item);
            return name;
        }

        public static void Save(string path, T item)
        {
            string purePath = Path.GetDirectoryName(path);
            if (purePath.Length > 0 && !Directory.Exists(purePath))
            {
                Directory.CreateDirectory(purePath);
            }
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (item != null)
            {
                binaryFormatter.Serialize(fileStream, item);
            }
            fileStream.Close();
        }
        
        public static T Load(string fileName)
        {
            Load(fileName, out T result);
            return result;
        }

        public static void Load(string path, out T result)
        {
            string purePath = Path.GetDirectoryName(path);
            if (purePath.Length > 0 && !Directory.Exists(purePath))
            {
                Directory.CreateDirectory(purePath);
            }
            if (!File.Exists(path))
            {
                result = default(T);
                Save(path, result);
                return;
            }
            FileStream fileStream = new FileStream(path, FileMode.Open);
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                result = (T)binaryFormatter.Deserialize(fileStream);
            }
            catch (Exception)
            {
                result = default(T);
            }
            finally
            {
                fileStream.Close();
            }
        }
    }

    public static class Exporter
    {
        public static void Save(string path, string text)
        {
            string purePath = Path.GetDirectoryName(path);
            if (purePath.Length > 0 && !Directory.Exists(purePath))
            {
                Directory.CreateDirectory(purePath);
            }
            if (!File.Exists(path))
            {
                FileStream file = File.Create(path);
                file.Close();
            }
            StreamWriter writer = new StreamWriter(path, true);
            StringReader reader = new StringReader(text);
            for (string line = reader.ReadLine(); line != null;
                line = reader.ReadLine())
            {
                writer.WriteLine(line);
            }
            reader.Close();
            writer.Flush();
            writer.Close();
        }
    }
}
