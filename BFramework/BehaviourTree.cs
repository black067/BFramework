using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    class BehaviourTree
    {
        public enum STATUS
        {
            SUSPENDED = 0,
            RUNNING = 1,
            SUCCESS = 2,
            FAILURE = 3,
        }
        public interface IBehaviour
        {
            void OnInitialize();
            STATUS Update();
            STATUS OnTerminate(STATUS status);
        }

        public class Behaviour : IBehaviour
        {
            private STATUS _status;

            public virtual void OnInitialize() { }

            public virtual STATUS OnTerminate(STATUS status)
            {
                return STATUS.SUCCESS;
            }

            public virtual STATUS Update()
            {
                return STATUS.SUCCESS;
            }

            public STATUS Tick()
            {
                if (_status != STATUS.RUNNING)
                {
                    OnInitialize();
                }
                _status = Update();
                if (_status != STATUS.RUNNING)
                {
                    _status = OnTerminate(_status);
                }
                return _status;
            }
        }

        public class Action : Behaviour { }

        public class Decorator : Behaviour
        {
            Behaviour _child;
            public Decorator(ref Behaviour behaviour)
            {
                _child = behaviour;
            }
        }

        public void Tick()
        {
            Behaviour A = new Behaviour();
        }
    }
}
