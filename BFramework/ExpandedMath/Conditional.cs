using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ExpandedMath
{
    class Conditional<T>
    {
        private T _value;
        private List<BDelegate<T, bool>> _conditions;

        public Conditional(T value, params BDelegate<T, bool>[] conditions)
        {
            int length = conditions.Length;
            _conditions = new List<BDelegate<T, bool>>(length);
            for(int i = 0; i < length; i++)
            {
                _conditions.Add(conditions[i]);
            }
            Value = value;
        }

        public bool Check(T value)
        {
            if (_conditions.Count < 1)
            {
                return true;
            }
            for (int i = 0, length = _conditions.Count; i < length; i++)
            {
                if (_conditions[i][value])
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
    }
}
