using System.IO;
using System.Collections.Generic;
using SimplexNoise;
using BFramework.ExpandedMath;
using BFramework.Tools;

namespace BFramework.World
{
    public class Generator
    {
        public int Seed { get; set; }

        public float MinHeight { get; set; } = 10;

        public float Frequency { get; set; } = 1f;

        public float Amplitude { get; set; } = 2;

        public Vector[] Offsets { get; set; }

        public BDelegate<int> InitOffset { get; set; }

        public BDelegate<int, string> GetTypeByNoise { get; set; }

        public bool UseNoise { get; set; }

        public Map Result { get; private set; }

        public Configuration Config { get; set; }

        public string Path { get; set; }

        public Dictionary<string, Properties> Prefab { get; set; }

        private bool _initialized { get; set; }

        private bool _offsetInitialized { get; set; }

        public int I { get; private set; }
        public int J { get; private set; }
        public int K { get; private set; }

        public bool Done { get; private set; }
        
        public void Init(string path)
        {
            if (_initialized)
            {
                return;
            }
            Path = path;
            Prefab = new Dictionary<string, Properties>();
            FileInfo[] files = new DirectoryInfo(Path).GetFiles();
            for(int i = files.Length - 1; i > -1; i--)
            {
                switch (files[i].Extension)
                {
                    case Properties.Extension:
                        Exporter<Properties>.Load(files[i].FullName, out Properties item);
                        Prefab.Add(item.NodeType, item);
                        break;
                    case Configuration.Extension:
                        Exporter<Configuration>.Load(files[i].FullName, out Configuration config);
                        Config = config;
                        Seed = Config.Seed;
                        MinHeight = Config.MinHeight;
                        Amplitude = Config.Amplitude;
                        Frequency = Config.Frequency;
                        break;
                }
            }

            if (InitOffset == null)
            {
                InitOffset = new BDelegate<int>(delegate (int seed)
                {
                    Random.Init(seed);
                    Offsets = new Vector[3];
                    Offsets[0] = new Vector(Random.Value * 1000, Random.Value * 1000, Random.Value * 1000);
                    Offsets[1] = new Vector(Random.Value * 1000, Random.Value * 1000, Random.Value * 1000);
                    Offsets[2] = new Vector(Random.Value * 1000, Random.Value * 1000, Random.Value * 1000);
                });
            }

            if(GetTypeByNoise == null)
            {
                GetTypeByNoise = new BDelegate<int, string>(delegate (int height, int noiseHeight)
                {
                    if (height > noiseHeight) { return Properties.EmptyValue; }
                    else if (height == noiseHeight) { return "GRASS"; }
                    else if (height < noiseHeight && height > noiseHeight - 5) { return "MUD"; }
                    else { return "ROCK"; }
                });
            }

            _initialized = true;
        }
        
        public Map Build(string name, int width, int height, int length)
        {
            InitOffset.Execute(Seed);

            Result = new Map(name, width, height, length);

            for (I = 0; I < width; I++)
            {
                for (J = 0; J < height; J++)
                {
                    for (K = 0; K < length; K++)
                    {
                        SetNodeType(Result[I, J, K]);
                    }
                }
            }

            return Result;
        }

        public void BuildSynchronizeInit(string name, int width, int height, int length)
        {
            InitOffset.Execute(Seed);
            Result = new Map(name, width, height, length);
            I = 0; J = 0; K = 0;
            Done = false;
        }

        int GenerateWaveHeight(float x, float y, float z)
        {
            float x0 = (x + Offsets[0].x) * Frequency;
            float y0 = (y + Offsets[0].y) * Frequency;
            float z0 = (z + Offsets[0].z) * Frequency;

            float x1 = (x + Offsets[1].x) * Frequency * 2;
            float y1 = (y + Offsets[1].y) * Frequency * 2;
            float z1 = (z + Offsets[1].z) * Frequency * 2;

            float x2 = (x + Offsets[2].x) * Frequency / 4;
            float y2 = (y + Offsets[2].y) * Frequency / 4;
            float z2 = (z + Offsets[2].z) * Frequency / 4;

            float noise0 = Noise.Generate(x0, y0, z0) * Amplitude;
            float noise1 = Noise.Generate(x1, y1, z1) * Amplitude / 2;
            float noise2 = Noise.Generate(x2, y2, z2) * Amplitude / 4;

            return (int)(noise0 + noise1 + noise2 + MinHeight);
        }
        
        public void SetNodeType(Node node)
        {
            string typeName = UseNoise ? 
                GetTypeByNoise.Execute(node.Y, GenerateWaveHeight(node.X, node.Y, node.Z)) : 
                Config.GetNodeTypeByHeight(node.Y);

            node.SetProerties(Prefab.ContainsKey(typeName) ? 
                Prefab[typeName] : 
                Default.Properties.Empty);
        }
    }
}
