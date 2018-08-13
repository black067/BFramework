using System;

namespace BFramework.Tools
{

    /// <summary>
    /// 秒表类
    /// </summary>
    public class BStopWatch
    {
        private DateTime _time0;

        /// <summary>
        /// 构建一个新的秒表类并记录当前时间
        /// </summary>
        public BStopWatch()
        {
            _time0 = DateTime.Now;
        }

        /// <summary>
        /// 返回从计时器新建/上一次刷新到现在的时间
        /// </summary>
        /// <returns></returns>
        public double Click()
        {
            TimeSpan span = new TimeSpan((DateTime.Now - _time0).Ticks);
            return span.TotalMilliseconds;
        }

        /// <summary>
        /// 刷新当前时间
        /// </summary>
        public void Refresh()
        {
            _time0 = DateTime.Now;
        }

        /// <summary>
        /// 取得格式化的当前时间
        /// </summary>
        public static string Now
        {
            get
            {
                DateTime now = DateTime.Now;
                return string.Format("[{0}-{1}-{2}-{3}-{4}-{5}]", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            }
        }

        /// <summary>
        /// 取得当前时间的字符串并刷新秒表
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            double d = Click();
            Refresh();
            return string.Format("Delta time: {0} ms", d);
        }

        /// <summary>
        /// 格式化输出当前时间并刷新秒表
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            TimeSpan span = new TimeSpan((DateTime.Now - _time0).Ticks);
            Refresh();
            return string.Format("Delta time: {0}", string.Format(format, span));
        }
    }
}
