using System;
using System.Collections.Generic;

namespace BFramework.StateMachines
{
    /// <summary>
    /// 有限状态机
    /// </summary>
    public class StateMachine
    {
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
        /// 状态机中所有状态
        /// </summary>
        private Dictionary<string, State> _states;
        
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
        private string _current;

        /// <summary>
        /// 下一个状态
        /// </summary>
        private string _nextState;

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
            private set
            {
                _tags = value;
            }
        }
        /// <summary>
        /// 执行当前状态节点
        /// </summary>
        public void Run()
        {
            Run(Params);
        }
        public void Run(object input)
        {
            _nextState = States[Current].Action[input];
            ChangeTo(_nextState);
        }

        /// <summary>
        /// 改变状态节点
        /// </summary>
        /// <param name="name"></param>
        public void ChangeTo(string name)
        {
            if (name != Current && States.ContainsKey(name))
            {
                Current = name;
            }
        }
    }
}
