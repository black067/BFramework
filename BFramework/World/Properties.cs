using System.Collections.Generic;
using BFramework.ExpandedMath;

namespace BFramework.World
{
    /// <summary>
    /// 节点属性类, 用于保存节点的属性及估值, 继承自 ExpandedMath.Estimable 类
    /// </summary>
    [System.Serializable]
    public class Properties : Estimable
    {
        public const string Extension = ".nodeType";
        /// <summary>
        /// 空节点的类型值
        /// </summary>
        public const string EmptyValue = Default.Value.NodeTypeEmpty;

        /// <summary>
        /// 默认的键值
        /// </summary>
        public static List<string> KeysStatic { get; set; } = Default.Properties.Keys.ToList();

        /// <summary>
        /// 节点的类型
        /// </summary>
        private string _nodeType { get; set; } = EmptyValue;

        /// <summary>
        /// 节点是否可见
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// 节点的类型
        /// </summary>
        public string NodeType
        {
            get
            {
                return _nodeType;
            }
            set
            {
                _nodeType = value;
                Visible = _nodeType != EmptyValue;
            }
        }

        /// <summary>
        /// 记录节点的开销
        /// </summary>
        public double Cost { get; set; } = double.MaxValue;

        /// <summary>
        /// 实例化空节点属性
        /// </summary>
        public Properties() : this(Default.Value.NodeTypeEmpty) { }
        
        /// <summary>
        /// 实例化空节点属性并声明其种类
        /// </summary>
        /// <param name="typeName"></param>
        public Properties(string typeName)
        {
            NodeType = typeName;
            Dictionary = new Dictionary<string, double>();
            foreach (string key in KeysStatic)
            {
                Dictionary.Add(key, 0);
            }
        }

        /// <summary>
        /// 实例化节点属性
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="dictionary"></param>
        public Properties(string typeName, Dictionary<string, double> dictionary)
        {
            NodeType = typeName;
            Dictionary = dictionary;
            foreach (string key in KeysStatic)
            {
                if (!Dictionary.ContainsKey(key))
                    Dictionary.Add(key, 0);
            }
        }

        public override Estimable Clone()
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            foreach(string key in Dictionary.Keys)
            {
                dic.Add(key, Dictionary[key]);
            }
            return new Estimable(dic);
        }
    }
}
