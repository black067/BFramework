using System;
using System.Collections.Generic;
using System.Reflection;

namespace BFramework.Tools
{
    /// <summary>
    /// 由字符串快速设置/获取某个实例的相应成员的值
    /// </summary>
    public class Lever
    {
        private object _instance;

        /// <summary>
        /// 取得作用目标的实例
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

        /// <summary>
        /// 属性信息表
        /// </summary>
        public Dictionary<string, PropertyInfo> PropertyTable
        {
            get; private set;
        }

        /// <summary>
        /// 字段信息表
        /// </summary>
        public Dictionary<string, FieldInfo> FieldTable { get; private set; }

        /// <summary>
        /// 目标的所有 public 属性名列表
        /// </summary>
        public string[] Keys
        {
            get; private set;
        }
        
        /// <summary>
        /// 使用一个任意实例对 Lever 进行初始化
        /// </summary>
        /// <param name="target"></param>
        public Lever(object target)
        {
            _instance = target;
            Type = _instance.GetType();
            
            PropertyTable = new Dictionary<string, PropertyInfo>();
            FieldTable = new Dictionary<string, FieldInfo>();
            List<string> KeysList = new List<string>();
            foreach (PropertyInfo info in Type.GetProperties(BindingFlags.Instance|BindingFlags.Public))
            {
                if (!PropertyTable.ContainsKey(info.Name))
                {
                    PropertyTable.Add(info.Name, info);
                    KeysList.Add(info.Name);
                }
            }
            foreach (FieldInfo info in Type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!FieldTable.ContainsKey(info.Name))
                {
                    FieldTable.Add(info.Name, info);
                    KeysList.Add(info.Name);
                }
            }
            Keys = KeysList.ToArray();
        }

        /// <summary>
        /// 根据属性名字设定目标实例对应的值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void TrySetValue(string memberName, object value, params object[] index)
        {
            if (FieldTable.TryGetValue(memberName, out FieldInfo fieldInfo))
            {
                fieldInfo.SetValue(_instance, value);
            }
            else if(PropertyTable.TryGetValue(memberName, out PropertyInfo propertyInfo))
            {
                propertyInfo.SetValue(_instance, value, index);
            }
        }

        /// <summary>
        /// 根据属性名字获取目标实例对应的值
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool TryGetValue(string memberName, out object value, params object[] index)
        {
            if (FieldTable.TryGetValue(memberName, out FieldInfo fieldInfo))
            {
                value= fieldInfo.GetValue(_instance);
                return true;
            }
            else if (PropertyTable.TryGetValue(memberName, out PropertyInfo propertyInfo))
            {
                value= propertyInfo.GetValue(_instance, index);
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 通过 Lever 的索引器设置/取得目标实例的成员值
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public object this[string memberName]
        {
            get
            {
                if (TryGetValue(memberName, out object value))
                {
                    return value;
                }
                return null;
            }
            set
            {
                TrySetValue(memberName, value);
            }
        }
    }
}