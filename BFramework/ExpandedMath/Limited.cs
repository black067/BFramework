
namespace BFramework.ExpandedMath
{

    /// <summary>
    /// 限制数，包含上限与下限。
    /// 两两运算时，结果的上限是两数中最大的上限，下限是两数中最小的上限。
    /// </summary>
    [System.Serializable]
    public class Limited
    {

        /// <summary>
        /// 实例化一个限制数, 给定其下限, 上限, 与初始值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// 从现有的数创建一个限制数
        /// </summary>
        /// <param name="value"></param>
        public Limited(Limited value) : this(value.Min, value.Max, value.Value) { }

        /// <summary>
        /// 创建默认限制数, 下限为0, 上限为100, 初始值为0
        /// </summary>
        public Limited() : this(0, 100, 0) { }

        float _min;
        float _max;
        float _value;

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value >= _min)
                {
                    if (value <= _max)
                    {
                        _value = value;
                        Overflow = 0;
                    }
                    else
                    {
                        _value = _max;
                        Overflow = value - _max;
                    }
                }
                else
                {
                    _value = _min;
                    Overflow = value - _min;
                }
            }
        }
        public float Max
        {
            get
            {
                return _max;
            }
            set
            {
                if (value >= _value)
                {
                    _max = value;
                }
                else if (value >= _min)
                {
                    _max = value;
                    Overflow = _value - _max;
                    _value = _max;
                }
                else
                {
                    _min = value * 0.5f;
                    _max = value;
                    Overflow = _value - _max;
                    _value = _max;
                }
            }
        }
        public float Min
        {
            get
            {
                return _min;
            }
            set
            {
                if (value <= _value)
                {
                    _min = value;
                }
                else if (value <= _max)
                {
                    _min = value;
                    Overflow = _value - _min;
                    _value = _min;
                }
                else
                {
                    _max = value * 2;
                    _min = value;
                    Overflow = _value - _min;
                    _value = _min;
                }
            }
        }

        public float Overflow { get; protected set; }

        public void SetMax()
        {
            _value = _max;
            Overflow = 0;
        }

        public void SetMin()
        {
            _value = _min;
            Overflow = 0;
        }

        public float GetPercentage()
        {
            return (Value - Min) / (Max - Min);
        }
        
        public override bool Equals(object obj)
        {
            var limited = obj as Limited;
            return limited != null &&
                   _min == limited._min &&
                   _max == limited._max &&
                   _value == limited._value &&
                   Value == limited.Value &&
                   Max == limited.Max &&
                   Min == limited.Min;
        }

        public override int GetHashCode()
        {
            var hashCode = -2087586221;
            hashCode = hashCode * -1521134295 + _min.GetHashCode();
            hashCode = hashCode * -1521134295 + _max.GetHashCode();
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            return hashCode;
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

        public static bool operator >= (Limited lhs, Limited rhs)
        {
            return lhs.Value >= rhs.Value;
        }
        public static bool operator <= (Limited lhs, Limited rhs)
        {
            return lhs.Value <= rhs.Value;
        }
        public static bool operator > (Limited lhs, Limited rhs)
        {
            return lhs.Value > rhs.Value;
        }
        public static bool operator < (Limited lhs, Limited rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator >=(Limited lhs, float rhs)
        {
            return lhs.Value >= rhs;
        }
        public static bool operator <=(Limited lhs, float rhs)
        {
            return lhs.Value <= rhs;
        }
        public static bool operator > (Limited lhs, float rhs)
        {
            return lhs.Value > rhs;
        }
        public static bool operator < (Limited lhs, float rhs)
        {
            return lhs.Value < rhs;
        }
    }

    public class LimitedInt
    {
        public LimitedInt(int min, int max, int value)
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
        public LimitedInt(LimitedInt value) : this(value.Min, value.Max, value.Value) { }
        public LimitedInt() : this(0, 100, 0) { }

        int _min;
        int _max;
        int _value;

        public int Value
        {
            get
            {
                return _value;
            }
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
        public int Max
        {
            get
            {
                return _max;
            }
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
                    _min = value / 2;
                    _max = value;
                    _value = value;
                }
            }
        }
        public int Min
        {
            get
            {
                return _min;
            }
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

        public bool OverflowP
        {
            get { return Value == Max; }
        }

        public bool OverflowN
        {
            get { return Value == Min; }
        }

        public int GetPercentage()
        {
            return (Value - Min) / (Max - Min);
        }

        public override bool Equals(object obj)
        {
            var limited = obj as LimitedInt;
            return limited != null &&
                   _min == limited._min &&
                   _max == limited._max &&
                   _value == limited._value &&
                   Value == limited.Value &&
                   Max == limited.Max &&
                   Min == limited.Min;
        }

        public override int GetHashCode()
        {
            var hashCode = -2087586221;
            hashCode = hashCode * -1521134295 + _min.GetHashCode();
            hashCode = hashCode * -1521134295 + _max.GetHashCode();
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            return hashCode;
        }
        
        public static LimitedInt operator +(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value + rhs.Value,
            };
        }
        public static LimitedInt operator -(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value - rhs.Value,
            };
        }
        public static LimitedInt operator *(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value * rhs.Value,
            };
        }
        public static LimitedInt operator /(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value / rhs.Value,
            };
        }

        public static LimitedInt operator +(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value + rhs
            };
        }
        public static LimitedInt operator -(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value - rhs
            };
        }
        public static LimitedInt operator *(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value * rhs
            };
        }
        public static LimitedInt operator /(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value / rhs
            };
        }

        public static bool operator ==(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Min == rhs.Min && lhs.Max == rhs.Max && lhs.Value == rhs.Value;
        }
        public static bool operator !=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Min != rhs.Min || lhs.Max != rhs.Max || lhs.Value != rhs.Value;
        }

        public static bool operator ==(LimitedInt lhs, int rhs)
        {
            return lhs.Value == rhs;
        }
        public static bool operator !=(LimitedInt lhs, int rhs)
        {
            return lhs.Value != rhs;
        }

        public static bool operator >=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value >= rhs.Value;
        }
        public static bool operator <=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value <= rhs.Value;
        }
        public static bool operator >(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value > rhs.Value;
        }
        public static bool operator <(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value < rhs.Value;
        }

        public static bool operator >=(LimitedInt lhs, int rhs)
        {
            return lhs.Value >= rhs;
        }
        public static bool operator <=(LimitedInt lhs, int rhs)
        {
            return lhs.Value <= rhs;
        }
        public static bool operator >(LimitedInt lhs, int rhs)
        {
            return lhs.Value > rhs;
        }
        public static bool operator <(LimitedInt lhs, int rhs)
        {
            return lhs.Value < rhs;
        }
    }
}
