using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.Tools
{
    public class BWatch
    {
        private DateTime _time0;

        public BWatch()
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

        public void Refresh()
        {
            _time0 = DateTime.Now;
        }

        public static string Now
        {
            get
            {
                DateTime now = DateTime.Now;
                return string.Format("[{0}-{1}-{2}-{3}-{4}-{5}]", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            }
        }

        public override string ToString()
        {
            double d = Click();
            Refresh();
            return string.Format("Delta time: {0} ms", d);
        }

        public string ToString(string format)
        {
            TimeSpan span = new TimeSpan((DateTime.Now - _time0).Ticks);
            Refresh();
            return string.Format("Delta time: {0}", string.Format(format, span));
        }
    }
}
