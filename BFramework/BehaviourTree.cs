using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    class BehaviourTree
    {
        public class Node
        {
            Node(String name)
            {
                _name = name;
            }

            String _name = "default";
            enum State
            {
                Stop = 0,
                Working = 1,
                Pause = 2,
            }
           
            public string Name { get => _name; private set => _name = value; }
        }
    }
}
