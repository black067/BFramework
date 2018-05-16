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

        public long Click()
        {
            return (DateTime.Now - _time0).Ticks;
        }

        public void Refresh()
        {
            _time0 = DateTime.Now;
        }

        public override string ToString()
        {
            long d = Click();
            Refresh();
            return string.Format("Delta time: {0}", d);
        }
    }
}
