using System.Collections.Generic;

namespace BFramework.BehaviourTree
{
    /// <summary>
    /// 行为树类
    /// </summary>
    public class BehaviourTree
    {
        
    }

    /// <summary>
    /// 用于行为树节点间交互的印象类
    /// </summary>
    public class Cognition
    {
        private Dictionary<string, string> _memory;
        private int capacity;

        public Cognition(Dictionary<string, string> memory)
        {
            _memory = memory;
            Capacity = _memory.Count;
        }
        public Cognition(int capacity) : this(new Dictionary<string, string>(capacity)) { }

        public string this[string key]
        {
            get
            {
                if (_memory.ContainsKey(key))
                {
                    return _memory[key];
                }
                return null;
            }
            set
            {
                if (_memory.ContainsKey(key))
                {
                    _memory[key] = value;
                }
                else
                {
                    _memory.Add(key, value);
                }
            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
            private set
            {
                capacity = value;
            }
        }
    }

    /// <summary>
    /// 节点状态枚举类
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
    public interface IBehaviour
    {
        STATUS OnInitialize(Cognition cognition);
        void OnTerminate();
        STATUS Update();
    }

    public class Action : IBehaviour
    {

        public Action(BDelegate<Cognition, STATUS> method)
        {
            _tick = method;
        }
        public STATUS OnInitialize(Cognition cognition)
        {
            _cognition = cognition;
            return STATUS.RUNNING;
        }

        public void OnTerminate()
        {
            return;
        }

        public STATUS Update()
        {
            return _tick[_cognition];
        }

        private Cognition _cognition;
        private STATUS _status = STATUS.INVALID;
        private BDelegate<Cognition, STATUS> _tick;
    }
}
