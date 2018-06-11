using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.DataStructure
{
    /// <summary>
    /// 有序数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortedList<T> where T : IComparable<T>
    {
        /// <summary>
        /// 顺序是正序(从小到大)还是逆序(从大到小)
        /// </summary>
        private enum ORDER
        {
            ASCENDING,
            DESCENDING,
        }
        
        /// <summary>
        /// 数组
        /// </summary>
        private List<T> _instance { get; set; }

        /// <summary>
        /// 向数组中添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            int startIndex = 0, 
                endIndex = _instance.Count - 1, 
                currentIndex = 0,
                compareResult;
            for (; startIndex <= endIndex;)
            {
                currentIndex = (startIndex + endIndex) / 2;
                compareResult = _instance[currentIndex].CompareTo(item);
                if (compareResult == 0)
                {
                    break;
                }
                else if (compareResult < 0)
                {
                    //起点后移
                    startIndex = currentIndex + 1;
                }
                else
                {
                    //终点前移
                    endIndex = currentIndex - 1;
                }
            }
            _instance.Insert(startIndex <= endIndex ? endIndex : currentIndex, item);
        }

        /// <summary>
        /// 从数组中取出第一个元素
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if(_instance.Count < 1)
            {
                return default(T);
            }
            T item = _instance[0];
            _instance.RemoveAt(0);
            return item;
        }
    }
}
