using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    class BehaviourTree
    {
        public enum Status
        {
            SUSPENDED = 0,
            RUNNING = 1,
            SUCCESS = 2,
            FAILURE = 3,
        }
        public interface IBehaviour
        {
            void OnInitialize();
            Status Update();
            void OnTerminate(Status status);
        }
        
        public void Update()
        {

        }
    }
}
