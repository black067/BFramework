using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ExpandedMath
{
    //有条件限制的容器，在对 Value 赋值之前会检查是否符合条件
    public class Conditional<T>
    {
        private T _value;
        private List<BDelegate<T, bool>> _conditions;

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

        public bool Check(T value)
        {
            if (Conditions.Count < 1)
            {
                return true;
            }
            for (int i = 0, length = Conditions.Count; i < length; i++)
            {
                if (Conditions[i][value])
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

        public List<BDelegate<T, bool>> Conditions
        {
            get
            {
                return _conditions;
            }
            set
            {
                _conditions = value;
            }
        }
        
    }
}
