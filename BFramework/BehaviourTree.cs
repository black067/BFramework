using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFramework
{
    
    /// <summary>
    /// 行为树类
    /// </summary>
    public class BehaviourTree
    {

        /// <summary>
        /// 用于行为树节点间交互的印象类
        /// </summary>
        public class Cognition
        {
            public Dictionary<string, List<object>> Memory;
            public Dictionary<string, int> CapacityDictinary;
            public int MainCapacity;
            public Cognition(int capacity)
            {
                MainCapacity = capacity;
                Memory = new Dictionary<string, List<object>>(MainCapacity);
                CapacityDictinary = new Dictionary<string, int>(MainCapacity);
            }
            public bool AddNewRecordList(string key, int capacity)
            {
                if (!Memory.ContainsKey(key) && Memory.Count < MainCapacity)
                {
                    Memory.Add(key, new List<object>(capacity));
                    CapacityDictinary.Add(key, capacity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public bool AddRecord(string key, object record)
            {
                if (Memory.ContainsKey(key) && Memory[key].Count < CapacityDictinary[key])
                {
                    Memory[key].Add(record);
                    return true;
                }
                else
                {
                    return false;
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

        /// <summary>
        /// 节点状态
        /// </summary>
        public enum STATUS
        {
            INVALID = -1,
            SUSPENDED = 0,
            RUNNING = 1,
            SUCCESS = 2,
            FAILURE = 3,
        }
        
        /// <summary>
        /// 行为类
        /// </summary>
        public class Behaviour
        {

            /// <summary>
            /// 初始化
            /// </summary>
            public BDelegate<bool, STATUS> OnInitialize;

            /// <summary>
            /// 退出
            /// </summary>
            public BDelegate<STATUS, STATUS> OnTerminate;

            /// <summary>
            /// 刷新
            /// </summary>
            public BDelegate<STATUS, STATUS> Update;
            public STATUS Tick()
            {
                if(_status == STATUS.INVALID)
                {
                    _status = OnInitialize[true];
                }
                _status = Update[_status];
                if(_status != STATUS.RUNNING)
                {
                    _status = OnTerminate[_status];
                }
                return _status;
            }

            private STATUS _status;

            /// <summary>
            /// 行为的状态
            /// </summary>
            public STATUS Status { get => _status; }
            public BehaviourTree Tree;
        }

        public class Action: Behaviour { }
        public void Tick()
        {
            //Behaviour A = new Behaviour();
        }
    }
}
