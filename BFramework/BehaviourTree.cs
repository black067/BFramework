using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    public class BehaviourTree
    {
        public class Cognition
        {
            public Dictionary<string, List<object>> Memory;
            public Dictionary<string, int> CacheDictinary;
            public Cognition()
            {
                Memory = new Dictionary<string, List<object>>();
                CacheDictinary = new Dictionary<string, int>();
            }
            public void AddNewRecordList(string key, int cache)
            {
                if (!Memory.ContainsKey(key))
                {
                    Memory.Add(key, new List<object>(cache));
                    CacheDictinary.Add(key, cache);
                }
            }
            public void AddRecord(string key, object record)
            {
                if (Memory.ContainsKey(key))
                {
                    Memory[key].Add(record);
                }
            }

            public List<object> this[string key]
            {
                get
                {
                    return Memory[key];
                }
            }
            public object this[string key, int index]
            {
                get
                {
                    if (Memory.ContainsKey(key))
                    {
                        if (index >= 0 && index <= Memory[key].Count)
                        {
                            return Memory[key][index];
                        }
                        else
                        {
                            return Memory[key][Memory[key].Count - 1];
                        }
                    }
                    else
                    {
                        return default(object);
                    }
                }
            }
        }

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
