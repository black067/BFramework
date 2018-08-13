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

        /// <summary>
        /// 保存一个物件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string Save(T item)
        {
            string name = string.Format("{0}.{1}", item.GetHashCode(), item.GetType().Name);
            Save(name, item);
            return name;
        }

        /// <summary>
        /// 保存物件到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="item"></param>
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
        
        /// <summary>
        /// 送指定路径读取物件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Load(string fileName)
        {
            Load(fileName, out T result);
            return result;
        }

        /// <summary>
        /// 从指定路径读取物件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
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

    /// <summary>
    /// 泛用序列化工具
    /// </summary>
    public static class Exporter
    {

        /// <summary>
        /// 保存文本到指定路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
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
