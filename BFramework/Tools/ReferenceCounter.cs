using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.Tools
{
    public interface IReferenceCounter
    {
        int Counter { get; }
        void Retain(object owner = null);
        void Release(object owner = null);
        void OnZeroReference();
    }
    public class ReferenceCounter : IReferenceCounter
    {
        public ReferenceCounter()
        {
            Counter = 0;
        }

        public int Counter { get; private set; }

        public virtual void OnZeroReference() { }

        public void Release(object owner = null)
        {
            Counter--;
            if(Counter == 0)
            {
                OnZeroReference();
            }
        }

        public void Retain(object owner = null)
        {
            Counter++;
        }
    }
}
