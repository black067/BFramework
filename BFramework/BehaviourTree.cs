using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    public class BehaviourTree
    {
        public enum STATUS
        {
            SUSPENDED = 0,
            RUNNING = 1,
            SUCCESS = 2,
            FAILURE = 3,
        }
        
        public class Behaviour
        {
            private BehaviourTree _tree;
            public BDelegate<int, int> OnInitialize;
            public BDelegate<int, STATUS> OnTerminate;
            public BDelegate<STATUS, STATUS> Update;
            public BDelegate<Behaviour, STATUS> Tick;
            private STATUS _status;

            public STATUS Status { get => _status; set => _status = value; }
            public BehaviourTree Tree { get => _tree; set => _tree = value; }
        }

        public class Action: Behaviour { }
        public void Tick()
        {
            //Behaviour A = new Behaviour();
        }
    }
}
