using System;
using BFramework.ExpandedMath;
using System.Collections.Generic;

namespace BFramework.World
{
    /// <summary>
    /// 节点属性类, 用于保存节点的属性及估值, 继承自 ExpandedMath.Estimable 类
    /// </summary>
    [Serializable]
    public class Properties : Estimable
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

        private string _nodeType;

        /// <summary>
        /// 空节点的类型值
        /// </summary>
        public static string EmptyValue { get; } = "EMPTY";

        /// <summary>
        /// 记录节点是否处于 Closed 列表中
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// 记录节点是否处于 Opened 列表中
        /// </summary>
        public bool Opened { get; set; }

        /// <summary>
        /// 节点是否为空
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// 节点的类型
        /// </summary>
        public string NodeType
        {
            get
            {
                return IsEmpty ? EmptyValue : _nodeType;
            }
            set
            {
                _nodeType = value;
                IsEmpty = _nodeType == EmptyValue;
            }
        }

        /// <summary>
        /// 记录节点的开销
        /// </summary>
        public int Cost { get; set; }
        
        /// <summary>
        /// 实例化属性类
        /// </summary>
        public Properties()
        {
            IsEmpty = true;
            NodeType = EmptyValue;
            Closed = false;
            Opened = false;
            Cost = int.MaxValue;

            Dictionary = new Dictionary<string, int>();
            foreach (string key in KeysStatic)
            {
                Dictionary.Add(key, 0);
            }
        }

        public Properties(bool closed, bool opened, string nodeType, int cost)
        {
            Closed = closed;
            Opened = opened;
            NodeType = nodeType;
            Cost = cost;

            Dictionary = new Dictionary<string, int>();
            foreach (string key in KeysStatic)
            {
                Dictionary.Add(key, 0);
            }
        }
    }
}
