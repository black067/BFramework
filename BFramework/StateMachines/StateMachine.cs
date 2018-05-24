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
        /// 下一个状态
        /// </summary>
        private string _nextState;

        /// <summary>
        /// 公用变量，用于状态之间传递参数
        /// </summary>
        private object _params;

        /// <summary>
        /// 当前状态节点
        /// </summary>
        public string Current
        {
            get;
            set;
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
            get;
            private set;
        }
        /// <summary>
        /// 用于记录状态节点的名称
        /// </summary>
        public List<string> Tags
        {
            get;
            private set;
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
            _nextState = States[Current].Act(input);
            if (_nextState != Current && States.ContainsKey(_nextState))
            {
                Current = _nextState;
            }
        }

        /// <summary>
        /// 添加状态转换
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        /// <returns></returns>
        public Translation AddTranslation(string fromState, string targetState, BDelegate<object, bool>.Method conditionMethod, BDelegate<object, string>.Method callbackMethod)
        {
            return States[fromState].AddTranslation(targetState, conditionMethod, callbackMethod);
        }

        /// <summary>
        /// 移除状态转换
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="translationName"></param>
        /// <returns></returns>
        public Translation RemoveTranslation(string stateName, string translationName)
        {
            return States[stateName].RemoveTranslation(translationName);
        }
    }
}
