using System;
using System.IO;
using System.Collections.Generic;
using BFramework.Tools;
using BFramework.ExpandedMath;

namespace BFramework.World
{
    [Serializable]
    public class Configuration
    {
        public const string Extension = ".config";
        public Configuration(string name, string[][] nodeTypes, Segments[] weights)
        {
            Name = name;
            NodeTypes = nodeTypes;
            Weights = weights;
            Height = Weights.Length;
        }

        public string Name { get; set; }
        
        public string[][] NodeTypes { get; private set; }

        public Segments[] Weights { get; private set; }

        public int Height { get; private set; }

        public const int MaxHeight = 2048;

        public int MinHeight { get; set; } = 10;

        public float Frequency { get; set; } = 0.03f;

        public float Amplitude { get; set; } = 2;

        public int Seed { get; set; } = 20180516;

        public string GetNodeTypeByHeight(int height)
        {
            if(height >= Height) { return Default.Value.NodeTypeEmpty; }
            return NodeTypes[height][Weights[height].GetRandomIndex()];
        }
        
        public static Configuration ReadCSV(string path)
        {
            path = Directory.GetCurrentDirectory() + "\\" + path;
            FileStream fileStream = File.Open(path, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            List<string[]> nodeTypes = new List<string[]>();
            List<Segments> weights = new List<Segments>();
            for(int i = 0; i < MaxHeight && !reader.EndOfStream; i++)
            {
                string str = reader.ReadLine();
                if (reader.EndOfStream) break;
                List<string> nodeTypesSplitted = new List<string>();
                foreach (string item in str.Split(','))
                {
                    if (item != "" && item != " " && item != "\t" && item != "\n")
                        nodeTypesSplitted.Add(item);
                }
                nodeTypes.Add(nodeTypesSplitted.ToArray());

                string[] weightSplitted = reader.ReadLine().Split(',');

                int[] weightsTemp = new int[weightSplitted.Length];
                for(int j = 0; j < weightSplitted.Length; j++)
                {
                    if (int.TryParse(weightSplitted[j], out int r))
                    {
                        weightsTemp[j] = r;
                    }
                }
                weights.Add(new Segments(weightsTemp));
            }
            reader.Dispose();
            fileStream.Close();
            string name = Path.GetFileNameWithoutExtension(path);
            Configuration result = new Configuration(name, nodeTypes.ToArray(), weights.ToArray());
            string serializeFilePath = name + ".config";
            Exporter<Configuration>.Save(serializeFilePath, result);
            return result;
        }
    }
}
