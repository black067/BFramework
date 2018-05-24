using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace BFramework.Tools
{
    /// <summary>
    /// 由字符串快速设置/获取某个实例的属性值, 若实例有索引器, 需要标记为
    /// [System.Runtime.CompilerServices.IndexerName(Tools.Lever.INDEXEDPROPERTYTAG)]
    /// </summary>
    public class Lever
    {
        /// <summary>
        /// 索引器标签常量, 用于标记索引器
        /// </summary>
        public const string INDEXEDPROPERTYTAG = "IndexedProperty";

        private object _instance;

        /// <summary>
        /// 取得目标实例
        /// </summary>
        public object Instance
        {
            get
            {
                return Convert.ChangeType(_instance, Type);
            }
        }

        /// <summary>
        /// 目标的类型
        /// </summary>
        public Type Type
        {
            get; set;
        }

        public Dictionary<string, PropertyInfo> Table
        {
            get; private set;
        }

        /// <summary>
        /// 目标的所有 public 属性名列表
        /// </summary>
        public string[] Keys
        {
            get; private set;
        }

        private PropertyInfo _indexedProperty
        {
            get; set;
        }

        private bool _indexedPropertyIsNull
        {
            get; set;
        }

        private string[] _index
        {
            get;set;
        }
        
        /// <summary>
        /// 使用一个任意实例对 Lever 进行初始化
        /// </summary>
        /// <param name="target"></param>
        public Lever(object target)
        {
            _instance = target;
            Type = _instance.GetType();

            _indexedProperty = Type.GetProperty(INDEXEDPROPERTYTAG);
            _indexedPropertyIsNull = _indexedProperty == null;
            _index = new string[1];

            Table = new Dictionary<string, PropertyInfo>();
            List<string> KeysList = new List<string>();
            foreach (PropertyInfo p in Type.GetProperties())
            {
                if (!Table.ContainsKey(p.Name))
                {
                    Table.Add(p.Name, p);
                    KeysList.Add(p.Name);
                }
            }
            Keys = KeysList.ToArray();
        }

        /// <summary>
        /// 根据属性名字设定目标实例对应的值
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void SetValue(string accessor, object value, params object[] index)
        {
            if (Keys.Contains(accessor))
            {
                Table[accessor].SetValue(_instance, value, index);
                return;
            }
            if (!_indexedPropertyIsNull)
            {
                _index[0] = accessor;
                _indexedProperty.SetValue(_instance, value, _index);
            }
        }

        /// <summary>
        /// 根据属性名字获取目标实例对应的值
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetValue(string accessor, params object[] index)
        {
            if (Keys.Contains(accessor))
            {
                return Table[accessor].GetValue(_instance, index);
            }
            if (!_indexedPropertyIsNull)
            {
                _index[0] = accessor;
                return _indexedProperty.GetValue(_instance, _index);
            }
            return null;
        }

        public object this[string accessor]
        {
            get
            {
                return GetValue(accessor);
            }
            set
            {
                SetValue(accessor, value);
            }
        }
    }
}