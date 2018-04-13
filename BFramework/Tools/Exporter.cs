using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace BFramework.Tools
{
    public static class Exporter<T>
    {
        public static void Save(string fileName, T obj)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, obj);
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

        public static void Load(string fileName, out T result)
        {
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
}
