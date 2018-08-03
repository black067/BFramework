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
        public Configuration(string name, List<string[]> nodeTypes, List<Segments> weights, int[] heightOffsets)
        {
            Name = name;
            NodeTypes = nodeTypes;
            Weights = weights;
            Height = Weights.Count;
            HeightOffsets = new Segments(heightOffsets);
        }

        public string Name;

        public List<string[]> NodeTypes;

        public List<Segments> Weights;

        public int Height;

        public Segments HeightOffsets;

        public const int MaxHeight = 2048;

        public int MinHeight = 10;

        public float Frequency = 0.03f;

        public float Amplitude = 2;

        public int Seed = 20180719;

        public string GetNodeTypeByHeight(int offset)
        {
            if (offset < 0)
            {
                return Default.Value.NodeTypeEmpty;
            }
            int i = HeightOffsets[offset];
            return NodeTypes[i][Weights[i].GetRandomIndex()];
        }

        public void Add(string[] nodeTypes, int[] weights)
        {
            NodeTypes.Add(nodeTypes);
            Weights.Add(new Segments(weights));
        }

        public static Configuration ReadCSV(string path)
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            List<string[]> nodeTypes = new List<string[]>();
            List<Segments> weights = new List<Segments>();
            List<int> heightOffsets = new List<int>();
            for (int i = 0; i < MaxHeight && !reader.EndOfStream; i++)
            {
                string str = reader.ReadLine();
                if (reader.EndOfStream)
                    break;
                List<string> nodeTypesSplitted = new List<string>();
                string[] typeSplitted = str.Split(',');
                string item = typeSplitted[0];
                heightOffsets.Add(int.Parse(item));

                for (int j = 1, length = typeSplitted.Length; j < length; j++)
                {
                    item = typeSplitted[j];
                    if (item != "" && item != " " && item != "\t" && item != "\n")
                        nodeTypesSplitted.Add(item);
                }
                nodeTypes.Add(nodeTypesSplitted.ToArray());

                string[] weightSplitted = reader.ReadLine().Split(',');
                int[] weightsTemp = new int[weightSplitted.Length - 1];
                for (int k = 1; k < weightSplitted.Length; k++)
                {
                    if (int.TryParse(weightSplitted[k], out int r))
                    {
                        weightsTemp[k - 1] = r;
                    }
                }
                weights.Add(new Segments(weightsTemp));
            }
            reader.Dispose();
            fileStream.Close();
            string name = Path.GetFileNameWithoutExtension(path);
            Configuration result = new Configuration(name, nodeTypes, weights, heightOffsets.ToArray());
            string serializeFilePath = name + Extension;
            Exporter<Configuration>.Save(serializeFilePath, result);
            return result;
        }
    }
}
