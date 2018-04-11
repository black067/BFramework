using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ExpandedMath
{
    public class Counter
    {
        public enum STATUS
        {
            NEGATIVE = -1,
            NONE = 0,
            POSITIVE = 1,
        }
        
        private int _value;
        private int _lowerLimit;
        private int _upperLimit;
        private int _delta;
        private STATUS _overflow;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Overflow = Check();
                switch (Overflow)
                {
                    case STATUS.NEGATIVE:
                        _value = LowerLimit;
                        break;
                    case STATUS.POSITIVE:
                        _value = UpperLimit;
                        break;
                }
            }
        }

        public int LowerLimit { get { return _lowerLimit; } set { _lowerLimit = value; } }
        public int UpperLimit { get { return _upperLimit; } set { _upperLimit = value; } }
        public int Delta { get { return _delta; } set { _delta = value; } }
        public STATUS Overflow { get { return _overflow; } set { _overflow = value; } }

        public STATUS Check()
        {
            return Value > _upperLimit ? STATUS.POSITIVE : (Value < _lowerLimit ? STATUS.NEGATIVE : STATUS.NONE);
        }

        public void Work()
        {
            Value += Delta;
        }

        public static Counter operator +(Counter counter, int rhs)
        {
            return counter;
        }
    }
}
