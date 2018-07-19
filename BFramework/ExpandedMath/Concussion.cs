using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.ExpandedMath
{
    /// <summary>
    /// 震荡模拟
    /// </summary>
    [Serializable]
    public class Concussion
    {
        private static readonly float UPPER = 1;
        private static readonly float LOWER = -1;

        public float current = -1;
        public float offset = 0;
        public float rate = 0.1f;
        public float result = 0;
        
        public Concussion(float current, float offset, float rate)
        {
            this.current = current;
            this.offset = offset;
            this.rate = rate;
        }
        public Concussion() : this(-1, 0, 0.1f) { }
        public Concussion(float rate) : this(-1, 0, rate) { }

        /// <summary>
        /// 返回值 result 接近于 sin(current)
        /// </summary>
        /// <returns></returns>
        public float Count()
        {
            result = 4 * (current - current * (current >= 0 ? current : -current)) + offset;
            current += rate;
            if(current > UPPER)
            {
                current = LOWER;
            }
            return result;
        }

        public void SetOne(bool positive = true)
        {
            current = positive ? 0.5f : -0.5f;
            Count();
        }

        public void SetZero(bool positive = true)
        {
            current = positive ? 1 : -1;
            Count();
        }
    }
}
