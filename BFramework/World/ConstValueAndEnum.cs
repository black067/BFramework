using System.Reflection;
using System.Collections.Generic;
using BFramework.Tools;

namespace BFramework.World
{
    /// <summary>
    /// 方向的枚举
    /// </summary>
    public enum DIRECTION
    {
        CENTER = 0,
        LEFT = -1,
        RIGHT = 1,
        BOTTOM = -2,
        TOP = 2,
        BACK = -3,
        FORWARD = 3,
    }

    /// <summary>
    /// 储存默认值的类
    /// </summary>
    public static class Default
    {
        public static Dictionary<string, double> GetDefaultTable()
        {
            return new Dictionary<string, double>
            {
                { "DIFFICULTY", 0 },
                { "GVALUE", 1 },
                { "HVALUE", 1 },
                { "FRICTION", 0 },
                { "TEMPERATURE", 0 }
            };
        }

        /// <summary>
        /// 对应 Properties 类的默认与预设值
        /// </summary>
        public static class Properties
        {
            /// <summary>
            /// 键的默认值
            /// </summary>
            public static class Keys
            {
                public const string GValue = "GVALUE";
                public const string HValue = "HVALUE";
                public const string DynamicWeight = "DYNAMICWEIGHT";
                public const string Resistance = "RESISTANCE";
                public const string Difficulty = "DIFFICULTY";
                public const string Hardness = "HARDNESS";
                public const string Friction = "FRICTION";
                public const string Temperature = "TEMPERATURE";

                public static List<string> ToList()
                {
                    List<string> result = new List<string>();
                    System.Type type = typeof(Keys);
                    FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    foreach (FieldInfo fi in fieldInfos)
                    {
                        if (fi.IsLiteral && !fi.IsInitOnly)
                        {
                            result.Add(fi.Name.ToUpper());
                        }
                    }

                    return result;
                }
            }

            private static readonly object o = new object();
            private static Dictionary<string, World.Properties> _pairs;
            public static Dictionary<string, World.Properties> Pairs
            {
                get
                {
                    lock (o)
                    {
                        if (_pairs == null)
                        {
                            _pairs = new Dictionary<string, World.Properties>();
                            System.Type t = typeof(World.Properties);
                            PropertyInfo[] infos = typeof(Default.Properties).GetProperties(BindingFlags.Static | BindingFlags.Public);
                            foreach (var i in infos)
                            {
                                if (i.PropertyType == t)
                                {
                                    _pairs.Add(i.Name, i.GetValue(null, null) as World.Properties);
                                }
                            }
                        }
                    }
                    return _pairs;
                }
            }
            public static Dictionary<string, World.Properties>.KeyCollection PrefabKeys
            {
                get
                {
                    return Pairs.Keys;
                }
            }
            public static World.Properties GetPrefab(string key)
            {
                if(Pairs.TryGetValue(key, out World.Properties v))
                {
                    return v;
                }
                else
                {
                    foreach (var k in PrefabKeys)
                    {
                        if(string.Equals(key, k, System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            return Pairs[k];
                        }
                    }
                    return null;
                }
            }

            /// <summary>
            /// 空节点属性
            /// </summary>
            public static World.Properties Empty
            {
                get
                {
                    return
                        new World.Properties(Value.NodeTypeEmpty, new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 0 },
                            { "GVALUE", 0 },
                            { "HVALUE", 0 },
                            { "FRICTION", 0 },
                            { "TEMPERATURE", 0 }
                        });
                }
            }

            /// <summary>
            /// 障碍节点属性
            /// </summary>
            public static World.Properties Water
            {
                get
                {
                    return
                        new World.Properties("WATER", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 300 },
                            { "HARDNESS", 10 },
                            { "FRICTION", 2 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            public static World.Properties BaseRock
            {
                get
                {
                    return
                        new World.Properties("BASEROCK", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 999 },
                            { "HARDNESS", 99 },
                            { "FRICTION", 13 },
                            { "TEMPERATURE", 200 }
                        });
                }
            }

            public static World.Properties Rock
            {
                get
                {
                    return
                        new World.Properties("ROCK", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 999 },
                            { "HARDNESS", 60 },
                            { "FRICTION", 3 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            public static World.Properties Mud
            {
                get
                {
                    return
                        new World.Properties("MUD", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 800 },
                            { "HARDNESS", 30 },
                            { "FRICTION", 7 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            public static World.Properties Grass
            {
                get
                {
                    return
                        new World.Properties("GRASS", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 999 },
                            { "HARDNESS", 40 },
                            { "FRICTION", 4 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            public static World.Properties Road
            {
                get
                {
                    return
                        new World.Properties("ROAD", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 999 },
                            { "HARDNESS", 75 },
                            { "FRICTION", 1 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            /// <summary>
            /// 随机节点属性
            /// </summary>
            public static World.Properties Random
            {
                get
                {
                    return
                        new World.Properties("RANDOM", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", ExpandedMath.BRandom.Range(0, 999) },
                            { "GVALUE", 0 },
                            { "HVALUE", 0 },
                            { "HARDNESS", 35 },
                            { "FRICTION", 2 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            public static List<World.Properties> GetPrefabs()
            {
                List<World.Properties> result = new List<World.Properties>();
                System.Type type = typeof(Properties);
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    result.Add(p.GetGetMethod().Invoke(null, null) as World.Properties);
                }
                return result;
            }
        }

        public static Configuration GetConfiguration()
        {
            List<string[]> nodeTypes = new List<string[]>
            {
                new string[]{"GRASS"},
                new string[]{"GRASS"},
                new string[]{"GRASS", "MUD"},
                new string[]{"MUD"},
                new string[]{"MUD", "ROCK"},
                new string[]{"MUD", "ROCK"},
                new string[]{"MUD", "ROCK"},
                new string[]{"ROCK"},
            };
            int[] heightOffsets = new int[]
            {
                0, 1, 1, 1, 1, 1, 1, 1
            };
            List<ExpandedMath.Segments> weights = new List<ExpandedMath.Segments>
            {
                new ExpandedMath.Segments(1),
                new ExpandedMath.Segments(1),
                new ExpandedMath.Segments(1, 1),
                new ExpandedMath.Segments(1),
                new ExpandedMath.Segments(10, 1),
                new ExpandedMath.Segments(1, 1),
                new ExpandedMath.Segments(1, 10),
                new ExpandedMath.Segments(1)
            };
            return new Configuration("Default", nodeTypes, weights, heightOffsets);
        }

        public static class Value
        {
            public const string NodeTypeEmpty = "EMPTY";
        }

        public readonly static DIRECTION[] Directions = new DIRECTION[]
        {
            DIRECTION.LEFT,
            DIRECTION.RIGHT,
            DIRECTION.BACK,
            DIRECTION.FORWARD,
            DIRECTION.BOTTOM,
            DIRECTION.TOP
        };
    }
}
