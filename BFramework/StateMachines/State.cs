using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.StateMachines
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
        public State(string name):this(name, new BDelegate<object, string>(delegate (object o){return name;})){}


        /// <summary>
        /// 使用给定的名称与现有的方法新建状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public State(string name, BDelegate<object, string>.Method action) : this(name, new BDelegate<object, string>(action)) { }

        /// <summary>
        /// 使用字符串为名称，已有的委托新建一个状态
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public State(string name, BDelegate<object, string> action)
        {
            Name = name;
            Action = action;
            _translations = new Dictionary<string, Translation>();
        }
        
        /// <summary>
        /// 状态节点的动作
        /// </summary>
        public BDelegate<object, string> Action
        {
            get;set;
        }

        private Dictionary<string, Translation> _translations { get; set; }
        
        /// <summary>
        /// 状态名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态节点对状态机的指针
        /// </summary>
        public StateMachine StateMachine { get; private set; }
        /// <summary>
        /// 使用给定的状态机进行初始化
        /// </summary>
        /// <param name="machine"></param>
        public void Initiate(StateMachine machine)
        {
            StateMachine = machine;
        }

        public string Act(object input)
        {
            foreach (Translation t in _translations.Values)
            {
                if (t.Determine(input))
                {
                    return t.Callback(input);
                }
            }
            if (Action == null)
            {
                return Name;
            }
            return Action.Execute(input);
        }

        public Translation AddTranslation(string targetState, BDelegate<object, bool>.Method conditionMethod, BDelegate<object, string>.Method callbackMethod)
        {
            return AddTranslation(targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod));
        }

        public Translation AddTranslation(string targetState, BDelegate<object, bool> condition, BDelegate<object, string> callback)
        {
            Translation trans = new Translation(Name, targetState, condition, callback);
            _translations.Add(trans.Name, trans);
            return trans;
        }
        public Translation RemoveTranslation(string name)
        {
            if (_translations.ContainsKey(name))
            {
                Translation trans = _translations[name];
                _translations.Remove(name);
                return trans;
            }
            else
            {
                return null;
            }
        }
    }

}
