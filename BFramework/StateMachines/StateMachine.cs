using System.Timers;
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
        /// 当前状态节点
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// 公用变量，用于状态之间传递参数
        /// </summary>
        public object Params { get; set; }

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
        /// 从任意状态出发的转移列表
        /// </summary>
        public List<Translation> AnyStateTranslations { get; set; } = new List<Translation>();

        /// <summary>
        /// 执行当前状态节点
        /// </summary>
        public void Run()
        {
            Run(Params);
        }

        /// <summary>
        /// 状态机有参数输入运行
        /// </summary>
        /// <param name="input"></param>
        public void Run(object input)
        {
            foreach(var t in AnyStateTranslations)
            {
                if (t.Determine(input))
                {
                    _nextState = t.Callback(input);
                    StateChangeCheck();
                    return;
                }
            }
            _nextState = States[Current].Act(input);
            StateChangeCheck();
        }

        bool StateChangeCheck()
        {
            if (_nextState != Current && States.ContainsKey(_nextState))
            {
                Current = _nextState;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加任意状态到某状态的转移
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddAnyStateTranslation(string targetState, BDelegate<object, bool> condition, BDelegate<object, string> callback)
        {
            Translation t = new Translation("ANY", targetState, condition, callback);
            AnyStateTranslations.Add(t);
            return t;
        }

        /// <summary>
        /// 添加一个任意状态到目标状态的转移, 并指定其条件与回调函数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddAnyStateTranslation(string targetState, BDelegate<object, bool>.Method condition, BDelegate<object, string>.Method callback)
        {
            Translation t = new Translation("ANY", targetState, condition, callback);
            AnyStateTranslations.Add(t);
            return t;
        }

        /// <summary>
        /// 添加一个任意状态到目标状态的转移, 并指定其条件与回调函数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddAnyStateTranslation(string targetState, BDelegate<object, bool>.Method condition, BDelegate<object, string>.MethodNone callback)
        {
            Translation t = new Translation("ANY", targetState, condition, callback);
            AnyStateTranslations.Add(t);
            return t;
        }

        /// <summary>
        /// 添加一个任意状态到目标状态的转移, 并指定其条件与回调函数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddAnyStateTranslation(string targetState, BDelegate<object, bool>.MethodNone condition, BDelegate<object, string>.Method callback)
        {
            Translation t = new Translation("ANY", targetState, condition, callback);
            AnyStateTranslations.Add(t);
            return t;
        }

        /// <summary>
        /// 添加一个任意状态到目标状态的转移, 并指定其条件与回调函数
        /// </summary>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Translation AddAnyStateTranslation(string targetState, BDelegate<object, bool>.MethodNone condition, BDelegate<object, string>.MethodNone callback)
        {
            Translation t = new Translation("ANY", targetState, condition, callback);
            AnyStateTranslations.Add(t);
            return t;
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
