using System;

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

        /// <summary>
        /// 当前值
        /// </summary>
        public float current = -1;

        /// <summary>
        /// 偏移量
        /// </summary>
        public float offset = 0;

        /// <summary>
        /// 震荡速度
        /// </summary>
        public float rate = 0.1f;

        /// <summary>
        /// 计算结果
        /// </summary>
        public float result = 0;
        
        /// <summary>
        /// 构建一个振荡器, 给定其当前值, 偏移量, 速度
        /// </summary>
        /// <param name="current"></param>
        /// <param name="offset"></param>
        /// <param name="rate"></param>
        public Concussion(float current, float offset, float rate)
        {
            this.current = current;
            this.offset = offset;
            this.rate = rate;
        }

        /// <summary>
        /// 构建一个振荡器, 当前值为 -1, 偏移量为 0, 速度为 0.1f
        /// </summary>
        public Concussion() : this(-1, 0, 0.1f) { }

        /// <summary>
        /// 构建一个振荡器并指定其速度, 当前值为 -1, 偏移量为 0
        /// </summary>
        /// <param name="rate"></param>
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

        /// <summary>
        /// 将振荡器置 1
        /// </summary>
        /// <param name="positive"></param>
        public void SetOne(bool positive = true)
        {
            current = positive ? 0.5f : -0.5f;
            Count();
        }

        /// <summary>
        /// 将振荡器置 0
        /// </summary>
        /// <param name="positive"></param>
        public void SetZero(bool positive = true)
        {
            current = positive ? 1 : -1;
            Count();
        }
    }
}
