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
        public static string Directory
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }

        public static void Save(string fileName, T obj)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, obj);
            fileStream.Close();
        }
        
        public static T Load(string fileName)
        {
            Load(fileName, out T result);
            return result;
        }

        public static void Load(string fileName, out T result)
        {
            if (!File.Exists(fileName))
            {
                result = default(T);
                Save(fileName, result);
                return;
            }
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                result = (T)binaryFormatter.Deserialize(fileStream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                fileStream.Close();
            }
        }
    }

    public static class Exporter
    {
        public static string Directory
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }
    }
}
