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
        
        /// <summary>
        /// 状态节点的动作
        /// </summary>
        public BDelegate<Object, String> Action;

        public Dictionary<string, Translation> TranslationsDictionary { get; set; }
        
        /// <summary>
        /// 状态名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态节点对状态机的引用
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
    }

}
