
namespace BFramework.ExpandedNumber
{
    public class Limited
    {
        public Limited(float min, float max, float value)
        {
            Min = min;
            Max = max;
            Value = value;
        }

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
    }
}
