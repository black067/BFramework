using SimplexNoise;
using BFramework.ExpandedMath;

namespace BFramework.World
{
    public class Generator
    {   
        public int Seed { get; set; }

        public float MinHeight { get; set; }

        public float Frequency { get; set; }

        public float Amplitude { get; set; }

        public Vector[] Offsets { get; set; }

        public Map Result { get; set; }

        public Map Build(string name, int width, int height, int length)
        {
            Random.Init(Seed);
            Offsets = new Vector[] { new Vector(), new Vector(), new Vector() };
            for(int i = 0; i < 3; i++)
            {
                Offsets[i].x = Random.Value * 1000;
                Offsets[i].y = Random.Value * 1000;
                Offsets[i].z = Random.Value * 1000;
            }

            Result = new Map(name, width, height, length);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        SetNodeType(i, j, k);
                    }
                }
            }
            return Result;
        }

        int GenerateHeight(Vector position)
        {
            //让随机种子，振幅，频率，应用于我们的噪音采样结果
            float x0 = (position.x + Offsets[0].x) * Frequency;
            float y0 = (position.y + Offsets[0].y) * Frequency;
            float z0 = (position.z + Offsets[0].z) * Frequency;

            float x1 = (position.x + Offsets[1].x) * Frequency * 2;
            float y1 = (position.y + Offsets[1].y) * Frequency * 2;
            float z1 = (position.z + Offsets[1].z) * Frequency * 2;

            float x2 = (position.x + Offsets[2].x) * Frequency / 4;
            float y2 = (position.y + Offsets[2].y) * Frequency / 4;
            float z2 = (position.z + Offsets[2].z) * Frequency / 4;

            float noise0 = Noise.Generate(x0, y0, z0) * Amplitude;
            float noise1 = Noise.Generate(x1, y1, z1) * Amplitude / 2;
            float noise2 = Noise.Generate(x2, y2, z2) * Amplitude / 4;

            //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
            return (int)(noise0 + noise1 + noise2 + MinHeight);
        }

        void SetNodeType(int x, int y, int z)
        {
            if(y >= Result.LengthY) { return; }
        }
    }
}
