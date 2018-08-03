using System.IO;
using System.Collections.Generic;
using SimplexNoise;
using BFramework.ExpandedMath;
using BFramework.Tools;

namespace BFramework.World
{
    public class Generator
    {
        public int Seed { get { return Config.Seed; } set { Config.Seed = value; } }

        public int MinHeight { get { return Config.MinHeight; } set { Config.MinHeight = value; } }

        public float Frequency { get { return Config.Frequency; } set { Config.Frequency = value; } }

        public float Amplitude { get { return Config.Amplitude; } set { Config.Amplitude = value; } }

        private Vector[] Offsets { get; set; }

        public BDelegate<int> InitOffset { get; set; }

        public BDelegate<int, string> GetTypeByNoise { get; set; }

        public bool UseNoise;

        public Map Result { get; private set; }

        public Configuration Config;
        
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
            Prefab = new Dictionary<string, Properties>();
            FileInfo[] files = new DirectoryInfo(path).GetFiles();
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
                        break;
                }
            }

            InitOffset = InitOffset ?? new BDelegate<int>(DefaultOffsetInit);

            GetTypeByNoise = GetTypeByNoise ?? new BDelegate<int, string>(DefaultGetType);

            _initialized = true;
        }

        protected void PrefabInit(Properties[] properties)
        {
            Prefab = new Dictionary<string, Properties>();
            foreach (Properties item in properties)
            {
                Prefab.Add(item.NodeType, item);
            }
        }

        protected void ConfigInit(Configuration config)
        {
            Config = config;
        }

        public void Init(Properties[] properties, Configuration config)
        {
            if (_initialized)
            {
                return;
            }
            PrefabInit(properties);
            ConfigInit(config);
            InitOffset = InitOffset ?? new BDelegate<int>(DefaultOffsetInit);
            GetTypeByNoise = GetTypeByNoise ?? new BDelegate<int, string>(DefaultGetType);
            _initialized = true;
        }
        
        public Map Build(string name, VectorInt origin, int width, int height, int length)
        {
            InitOffset.Execute(Seed);

            Result = new Map(name, width, height, length, origin);

            for (I = 0; I < width; I++)
            {
                for (J = 0; J < height; J++)
                {
                    for (K = 0; K < length; K++)
                    {
                        SetNodeType(Result.Nodes[I, J, K]);
                    }
                }
            }

            return Result;
        }

        private string DefaultGetType(int height, int noiseHeight)
        {
            return Config.GetNodeTypeByHeight(noiseHeight - height);
        }

        private void DefaultOffsetInit(int seed)
        {
            BRandom.Init(seed);
            Offsets = new Vector[3];
            Offsets[0] = new Vector(BRandom.Value * 1000, BRandom.Value * 1000, BRandom.Value * 1000);
            Offsets[1] = new Vector(BRandom.Value * 1000, BRandom.Value * 1000, BRandom.Value * 1000);
            Offsets[2] = new Vector(BRandom.Value * 1000, BRandom.Value * 1000, BRandom.Value * 1000);
        }
        
        private int GenerateWaveHeight(float x, float y, float z)
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
                Config.GetNodeTypeByHeight(Config.MinHeight - node.Y);

            node.SetProerties(Prefab.ContainsKey(typeName) ? 
                Prefab[typeName] : 
                Default.Properties.Empty);
        }
    }
}
