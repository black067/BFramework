using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace BFramework.ShootingGame
{

    /// <summary>
    /// 道具信息的展示者需要携带的方法
    /// </summary>
    public interface IInterpreter
    {

        /// <summary>
        /// 展示道具信息
        /// </summary>
        /// <param name="info"></param>
        void Show(ItemInfo info);
    }

    /// <summary>
    /// 道具信息
    /// </summary>
    [Serializable]
    public struct ItemInfo
    {
        static Dictionary<string, FieldInfo> _fieldInfos;

        /// <summary>
        /// 道具信息的字段字典, 可以从中取得字段信息
        /// </summary>
        public static Dictionary<string, FieldInfo> FieldInfos
        {
            get
            {
                if (_fieldInfos == null)
                {
                    _fieldInfos = new Dictionary<string, FieldInfo>();
                    FieldInfo[] infos = typeof(ItemInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var item in infos)
                    {
                        _fieldInfos.Add(item.Name, item);
                    }
                }
                return _fieldInfos;
            }
        }

        /// <summary>
        /// 道具的 ID
        /// </summary>
        public int id;

        /// <summary>
        /// 道具名
        /// </summary>
        public string name;

        /// <summary>
        /// 道具附带的效果(可读字符串形式)
        /// </summary>
        public string effectInString;

        /// <summary>
        /// 道具描述
        /// </summary>
        public string main;

        /// <summary>
        /// 道具剩余的可用次数(值为 -1 时即表示不限次数)
        /// </summary>
        public int usageCount;

        /// <summary>
        /// 道具是否可以无限使用
        /// </summary>
        public bool infinity;

        /// <summary>
        /// 道具是否可以堆叠
        /// </summary>
        public bool stackable;

        /// <summary>
        /// 道具被生产的时间
        /// </summary>
        public DateTime date_manufacture;

        /// <summary>
        /// 道具被获取的时间
        /// </summary>
        public DateTime date_obtain;
        
        /// <summary>
        /// 生成一个道具信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="main"></param>
        /// <param name="usageCount"></param>
        /// <param name="stackable"></param>
        public ItemInfo(int id, string name, string main, int usageCount, bool stackable) : this()
        {
            this.id = id;
            this.name = name;
            this.main = main;
            this.usageCount = usageCount;
            if(usageCount < 0)
            {
                infinity = true;
            }
            this.stackable = stackable;
            date_manufacture = DateTime.Now;
            date_obtain = date_manufacture;
        }

        /// <summary>
        /// 通过索引器取得道具的字段信息
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public FieldInfo this[string fieldName]
        {
            get
            {
                if (FieldInfos.TryGetValue(fieldName, out FieldInfo result))
                {
                    return result;
                }
                else
                {
                    foreach (var item in FieldInfos.Keys)
                    {
                        if (string.Equals(fieldName, item, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return FieldInfos[item];
                        }
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 尝试对指定名称的字段赋值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void TrySetValue(string fieldName, object value)
        {
            if(FieldInfos.TryGetValue(fieldName, out FieldInfo info) && info.FieldType == value.GetType())
            {
                info.SetValue(this, value);
            }
            else
            {
                foreach (var item in FieldInfos.Keys)
                {
                    if(string.Equals(fieldName, item, StringComparison.CurrentCultureIgnoreCase) && FieldInfos[item].FieldType == value.GetType())
                    {
                        FieldInfos[item].SetValue(this, value);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 尝试取得指定名称的字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string fieldName, out object value)
        {
            if (FieldInfos.TryGetValue(fieldName, out FieldInfo info))
            {
                value = info.GetValue(this);
                return true;
            }
            else
            {
                foreach (var item in FieldInfos.Keys)
                {
                    if (string.Equals(fieldName, item, StringComparison.CurrentCultureIgnoreCase))
                    {
                        value = FieldInfos[item].GetValue(this);
                        return true;
                    }
                }
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 将信息序列化为可读字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(base.ToString());
            builder.Append(" ,");
            foreach (var kvp in FieldInfos)
            {
                builder.Append(kvp.Key); builder.Append(" ");
                builder.Append(kvp.Value.GetValue(this).ToString()); builder.Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}
