using System;
using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.PathFind
{
    /// <summary>
    /// 节点属性类, 用于保存节点的属性及估值, 继承自 ExpandedMath.Estimable 类
    /// </summary>
    [Serializable]
    public class Attribute : Estimable
    {
        public static List<string> KeysStatic { get; set; }
            = new List<string>()
            {
                "DIFFICULTY",
                "GVALUE",
                "HVALUE",
                "RESISTANCE",
                "TEMPERATURE"
            };
        
        /// <summary>
        /// 记录节点是否处于 Closed 列表中
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// 记录节点是否处于 Opened 列表中
        /// </summary>
        public bool Opened { get; set; }

        /// <summary>
        /// 记录节点的父节点
        /// </summary>
        public Node Parent { get; set; }

        /// <summary>
        /// 记录节点的开销
        /// </summary>
        public int Cost { get; set; }
        
        /// <summary>
        /// 实例化属性类
        /// </summary>
        public Attribute()
        {
            Closed = false;
            Opened = false;
            Parent = null;
            Cost = int.MaxValue;

            Dictionary = new Dictionary<string, int>();
            foreach (string key in KeysStatic)
            {
                Dictionary.Add(key, 0);
            }
        }
    }
}
