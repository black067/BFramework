
namespace BFramework.ExpandedNumber
{
    public class Limited
    {
        public Limited(float min, float max, float value)
        {
            _min = min;
            _max = max;
            if (_min > _max)
            {
                _max = min;
                _min = max;
            }
            Value = value;
        }
        public Limited(Limited value) : this(value.Min, value.Max, value.Value) { }
        public Limited() : this(0, 100, 0) { }

        float _min;
        float _max;
        float _value;

        public float Value
        {
            get => _value;
            set
            {
                if (value >= _min)
                {
                    if (value <= _max)
                    {
                        _value = value;
                    }
                    else
                    {
                        _value = _max;
                    }
                }
                else
                {
                    _value = _min;
                }
            }
        }
        public float Max
        {
            get => _max;
            set
            {
                if (value >= _value)
                {
                    _max = value;
                }
                else if (value >= _min)
                {
                    _value = value;
                    _max = value;
                }
                else
                {
                    _min = value * 0.5f;
                    _max = value;
                    _value = value;
                }
            }
        }
        public float Min
        {
            get => _min;
            set
            {
                if (value <= _value)
                {
                    _min = value;
                }
                else if (value <= _max)
                {
                    _value = value;
                    _min = value;
                }
                else
                {
                    _max = value * 2;
                    _value = value;
                    _min = value;
                }
            }
        }

        public float GetPercentage()
        {
            return (Value - Min) / (Max - Min);
        }
        
        public static Limited operator + (Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value + rhs.Value,
            };
        }
        public static Limited operator - (Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value - rhs.Value,
            };
        }
        public static Limited operator * (Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value * rhs.Value,
            };
        }
        public static Limited operator / (Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value / rhs.Value,
            };
        }

        public static Limited operator + (Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value + rhs
            };
        }
        public static Limited operator - (Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value - rhs
            };
        }
        public static Limited operator * (Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value * rhs
            };
        }
        public static Limited operator / (Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value / rhs
            };
        }

        public static bool operator == (Limited lhs, Limited rhs)
        {
            return lhs.Min == rhs.Min && lhs.Max == rhs.Max && lhs.Value == rhs.Value;
        }
        public static bool operator != (Limited lhs, Limited rhs)
        {
            return lhs.Min != rhs.Min || lhs.Max != rhs.Max || lhs.Value != rhs.Value;
        }

        public static bool operator == (Limited lhs, float rhs)
        {
            return lhs.Value == rhs;
        }
        public static bool operator != (Limited lhs, float rhs)
        {
            return lhs.Value != rhs;
        }
    }
}
