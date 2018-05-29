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
                            { "TEMPERATURE", 20 }
                        });
                }
            }

            /// <summary>
            /// 障碍节点属性
            /// </summary>
            public static World.Properties Obstacle
            {
                get
                {
                    return
                        new World.Properties("OBSTACLE", new Dictionary<string, double>
                        {
                            { "DIFFICULTY", 999 },
                            { "GVALUE", 0 },
                            { "HVALUE", 0 },
                            { "FRICTION", 5 },
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
                            { "DIFFICULTY", ExpandedMath.Random.Range(0, 999) },
                            { "GVALUE", 0 },
                            { "HVALUE", 0 },
                            { "FRICTION", 2 },
                            { "TEMPERATURE", 20 }
                        });
                }
            }
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
