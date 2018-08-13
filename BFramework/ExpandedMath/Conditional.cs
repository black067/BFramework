using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ExpandedMath
{
    /// <summary>
    /// 有条件限制的容器，在对 Value 赋值之前会检查是否符合条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Conditional<T>
    {
        private T _value;

        /// <summary>
        /// 给定初始值与条件构建一个容器
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        public Conditional(T value, params BDelegate<T, bool>[] conditions)
        {
            int length = conditions.Length;
            Conditions = new List<BDelegate<T, bool>>(length);
            for(int i = 0; i < length; i++)
            {
                Conditions.Add(conditions[i]);
            }
            Value = value;
        }

        /// <summary>
        /// 检查值是否满足条件
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Check(T value)
        {
            if (Conditions.Count < 1)
            {
                return true;
            }
            for (int i = 0, length = Conditions.Count; i < length; i++)
            {
                if (Conditions[i].Execute(value))
                {
                    if (i == length - 1)
                    {
                        return true;
                    }
                }
                else
                {
                    break;
                }
            }
            return false;
        }

        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = Check(value) ? value : _value;
            }
        }

        /// <summary>
        /// 条件列表
        /// </summary>
        public List<BDelegate<T, bool>> Conditions { get; set; }

    }
}
