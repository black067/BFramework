
namespace BFramework.ExpandedMath
{

    /// <summary>
    /// 限制数，包含上限与下限。
    /// 两两运算时，结果的上限是两数中最大的上限，下限是两数中最小的上限。
    /// </summary>
    [System.Serializable]
    public struct Limited
    {
        /// <summary>
        /// 实例化一个限制数, 给定其下限, 上限, 与初始值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        public Limited(float min, float max, float value) : this()
        {
            _min = min;
            _max = max;
            if (_min > _max)
            {
                _max = min;
                _min = max;
            }
            _value = value;
        }

        /// <summary>
        /// 从现有的数创建一个限制数
        /// </summary>
        /// <param name="value"></param>
        public Limited(Limited value) : this(value.Min, value.Max, value.Value) { }

        float _min;
        float _max;
        float _value;

        /// <summary>
        /// 值
        /// </summary>
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

        /// <summary>
        /// 上限
        /// </summary>
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

        /// <summary>
        /// 下限
        /// </summary>
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

        /// <summary>
        /// 溢出标记
        /// </summary>
        public float Overflow;

        /// <summary>
        /// 设置上限
        /// </summary>
        public void SetMax()
        {
            _value = _max;
            Overflow = 0;
        }

        /// <summary>
        /// 设置下限
        /// </summary>
        public void SetMin()
        {
            _value = _min;
            Overflow = 0;
        }

        /// <summary>
        /// 取得当前值的百分比
        /// </summary>
        /// <returns></returns>
        public float GetPercentage()
        {
            return (Value - Min) / (Max - Min);
        }

        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var limited = (Limited)obj;
            return _min == limited._min &&
                   _max == limited._max &&
                   _value == limited._value &&
                   Value == limited.Value &&
                   Max == limited.Max &&
                   Min == limited.Min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 运算符 + 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator +(Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value + rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator -(Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value - rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 * 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator *(Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value * rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 / 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator /(Limited lhs, Limited rhs)
        {
            return new Limited(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value / rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 + 的重载, 将浮点数与限制数的值相加, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator +(Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value + rhs
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 将浮点数与限制数的值相减, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator -(Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value - rhs
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 将浮点数与限制数的值相乘, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator *(Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value * rhs
            };
        }

        /// <summary>
        /// 运算符 / 的重载, 将浮点数与限制数的值相除, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Limited operator /(Limited lhs, float rhs)
        {
            return new Limited(lhs)
            {
                Value = lhs.Value / rhs
            };
        }

        /// <summary>
        /// 判断两个限制数是否相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Limited lhs, Limited rhs)
        {
            return lhs.Min == rhs.Min && lhs.Max == rhs.Max && lhs.Value == rhs.Value;
        }

        /// <summary>
        /// 判断两个限制数是否不相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Limited lhs, Limited rhs)
        {
            return lhs.Min != rhs.Min || lhs.Max != rhs.Max || lhs.Value != rhs.Value;
        }

        /// <summary>
        /// 判断限制数的值与浮点数是否相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Limited lhs, float rhs)
        {
            return lhs.Value == rhs;
        }

        /// <summary>
        /// 判断限制数的值与浮点数是否不相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Limited lhs, float rhs)
        {
            return lhs.Value != rhs;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >=(Limited lhs, Limited rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <=(Limited lhs, Limited rhs)
        {
            return lhs.Value <= rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(Limited lhs, Limited rhs)
        {
            return lhs.Value > rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(Limited lhs, Limited rhs)
        {
            return lhs.Value < rhs.Value;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >=(Limited lhs, float rhs)
        {
            return lhs.Value >= rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <=(Limited lhs, float rhs)
        {
            return lhs.Value <= rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(Limited lhs, float rhs)
        {
            return lhs.Value > rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(Limited lhs, float rhs)
        {
            return lhs.Value < rhs;
        }
    }

    /// <summary>
    /// 整数型限制数，包含上限与下限。
    /// 两两运算时，结果的上限是两数中最大的上限，下限是两数中最小的上限。
    /// </summary>
    [System.Serializable]
    public struct LimitedInt
    {
        /// <summary>
        /// 实例化一个限制数, 给定其下限, 上限, 与初始值
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="value"></param>
        public LimitedInt(int min, int max, int value) : this()
        {
            _min = min;
            _max = max;
            if (_min > _max)
            {
                _max = min;
                _min = max;
            }
            _value = value;
        }

        /// <summary>
        /// 从现有的数创建一个限制数
        /// </summary>
        /// <param name="value"></param>
        public LimitedInt(LimitedInt value) : this(value.Min, value.Max, value.Value) { }

        int _min;
        int _max;
        int _value;

        /// <summary>
        /// 值
        /// </summary>
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

        /// <summary>
        /// 上限
        /// </summary>
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
                    _max = value;
                    Overflow = _value - _max;
                    _value = _max;
                }
                else
                {
                    _min = value - value;
                    _max = value;
                    Overflow = _value - _max;
                    _value = _max;
                }
            }
        }

        /// <summary>
        /// 下限
        /// </summary>
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

        /// <summary>
        /// 溢出标记
        /// </summary>
        public int Overflow;

        /// <summary>
        /// 设置上限
        /// </summary>
        public void SetMax()
        {
            _value = _max;
            Overflow = 0;
        }

        /// <summary>
        /// 设置下限
        /// </summary>
        public void SetMin()
        {
            _value = _min;
            Overflow = 0;
        }

        /// <summary>
        /// 取得当前值的百分比
        /// </summary>
        /// <returns></returns>
        public float GetPercentage()
        {
            return (Value - Min) / (Max - Min);
        }

        /// <summary>
        /// 判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var limited = (LimitedInt)obj;
            return _min == limited._min &&
                   _max == limited._max &&
                   _value == limited._value &&
                   Value == limited.Value &&
                   Max == limited.Max &&
                   Min == limited.Min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 运算符 + 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator +(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value + rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator -(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value - rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 * 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator *(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value * rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 / 的重载, 合并两个限制数, 结果的上限为最大的上限, 结果下限为最小的下限
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator /(LimitedInt lhs, LimitedInt rhs)
        {
            return new LimitedInt(lhs)
            {
                Max = lhs.Max > rhs.Max ? lhs.Max : rhs.Max,
                Min = lhs.Min < rhs.Min ? lhs.Min : rhs.Min,
                Value = lhs.Value / rhs.Value,
            };
        }

        /// <summary>
        /// 运算符 + 的重载, 将浮点数与限制数的值相加, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator +(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value + rhs
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 将浮点数与限制数的值相减, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator -(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value - rhs
            };
        }

        /// <summary>
        /// 运算符 - 的重载, 将浮点数与限制数的值相乘, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator *(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value * rhs
            };
        }

        /// <summary>
        /// 运算符 / 的重载, 将浮点数与限制数的值相除, 得到一个新的数
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static LimitedInt operator /(LimitedInt lhs, int rhs)
        {
            return new LimitedInt(lhs)
            {
                Value = lhs.Value / rhs
            };
        }

        /// <summary>
        /// 判断两个限制数是否相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Min == rhs.Min && lhs.Max == rhs.Max && lhs.Value == rhs.Value;
        }

        /// <summary>
        /// 判断两个限制数是否不相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Min != rhs.Min || lhs.Max != rhs.Max || lhs.Value != rhs.Value;
        }

        /// <summary>
        /// 判断限制数的值与浮点数是否相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(LimitedInt lhs, int rhs)
        {
            return lhs.Value == rhs;
        }

        /// <summary>
        /// 判断限制数的值与浮点数是否不相等
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(LimitedInt lhs, int rhs)
        {
            return lhs.Value != rhs;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value >= rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <=(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value <= rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value > rhs.Value;
        }

        /// <summary>
        /// 通过值比较两个限制数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(LimitedInt lhs, LimitedInt rhs)
        {
            return lhs.Value < rhs.Value;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >=(LimitedInt lhs, int rhs)
        {
            return lhs.Value >= rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <=(LimitedInt lhs, int rhs)
        {
            return lhs.Value <= rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(LimitedInt lhs, int rhs)
        {
            return lhs.Value > rhs;
        }

        /// <summary>
        /// 通过值比较限制数与浮点数的大小
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(LimitedInt lhs, int rhs)
        {
            return lhs.Value < rhs;
        }
    }
}