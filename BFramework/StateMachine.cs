using System;
using System.Collections.Generic;

namespace BFramework
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// 用于状态机的状态类
        /// </summary>
        public class State
        {

            /// <summary>
            /// 使用字符串为名称新建一个状态
            /// </summary>
            /// <param name="name"></param>
            public State(String name)
            {
                Name = name;
            }

            /// <summary>
            /// 使用字符串为名称，已有的委托新建一个状态
            /// </summary>
            /// <param name="name"></param>
            /// <param name="action"></param>
            public State(String name, BDelegate<Object, String> action)
            {
                Name = name;
                Action = action;
            }

            private String _name;

            /// <summary>
            /// 状态节点的动作
            /// </summary>
            public BDelegate<Object, String> Action;
            private StateMachine _stateMachine;

            /// <summary>
            /// 状态名
            /// </summary>
            public string Name
            {
                get
                {
                    return _name;
                }
                set { _name = value; }
            }
            /// <summary>
            /// 状态节点对状态机的引用
            /// </summary>
            public StateMachine StateMachine
            {
                get
                {
                    return _stateMachine;
                }
                private set { _stateMachine = value; }
            }
            /// <summary>
            /// 使用给定的状态机进行初始化
            /// </summary>
            /// <param name="machine"></param>
            public void Initiate(StateMachine machine)
            {
                StateMachine = machine;
            }
        }

        /// <summary>
        /// 由给定状态建立状态机
        /// </summary>
        /// <param name="states"></param>
        public StateMachine(params State[] states)
        {
            States = new Dictionary<string, State>(states.Length);
            Tags = new List<string>(states.Length);
            foreach (State s in states)
            {
                AddState(s);
            }
            Current = states[0].Name;
            Params = null;
        }

        /// <summary>
        /// 保存状态机中所有状态节点
        /// </summary>
        private Dictionary<String, State> _states;

        /// <summary>
        /// 添加状态节点
        /// </summary>
        /// <param name="state"></param>
        public void AddState(State state)
        {
            States.Add(state.Name, state);
            Tags.Add(state.Name);
            state.Initiate(this);
        }

        /// <summary>
        /// 当前状态节点
        /// </summary>
        private String _current;

        /// <summary>
        /// 下一个状态
        /// </summary>
        private String _nextState;

        /// <summary>
        /// 公用变量，用于状态之间传递参数
        /// </summary>
        private object _params;

        /// <summary>
        /// 用于记录状态节点的名称
        /// </summary>
        private List<string> _tags;

        /// <summary>
        /// 当前状态节点
        /// </summary>
        public string Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        /// <summary>
        /// 公用变量，用于状态之间传递参数
        /// </summary>
        public object Params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }
        /// <summary>
        /// 保存状态机中所有状态节点
        /// </summary>
        public Dictionary<string, State> States
        {
            get
            {
                return _states;
            }
            private set { _states = value; }
        }
        /// <summary>
        /// 用于记录状态节点的名称
        /// </summary>
        public List<string> Tags
        {
            get
            {
                return _tags;
            }
            private set { _tags = value; }
        }
        /// <summary>
        /// 执行当前状态节点
        /// </summary>
        public void Run()
        {
            _nextState = States[Current].Action[Params];
            ChangeTo(ref _nextState);
        }
        public void Run(object input)
        {
            _nextState = States[Current].Action[input];
            ChangeTo(ref _nextState);
        }

        /// <summary>
        /// 改变状态节点
        /// </summary>
        /// <param name="name"></param>
        public void ChangeTo(ref String name)
        {
            if (name != Current && States.ContainsKey(name))
            {
                Current = name;
            }
        }
    }
}
