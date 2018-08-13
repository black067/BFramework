using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BFramework.StateMachines
{

    /// <summary>
    /// 状态转换类
    /// </summary>
    public class Translation
    {

        /// <summary>
        /// 转移名字
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 来源状态
        /// </summary>
        public string FromState { get; set; }

        /// <summary>
        /// 目标状态
        /// </summary>
        public string TargetState { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        public BDelegate<object, string> CallbackAction { get; set; }

        /// <summary>
        /// 条件判断函数
        /// </summary>
        public BDelegate<object, bool> Condition
        {
            get; set;
        }
        
        /// <summary>
        /// 指定源状态与目标状态构建转移, 并设置其条件与回调函数
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="condition"></param>
        /// <param name="callback"></param>
        public Translation(string fromState, string targetState, BDelegate<object, bool> condition, BDelegate<object, string> callback)
        {
            Name = fromState + "TO" + targetState;
            FromState = fromState;
            TargetState = targetState;
            Condition = condition;
            CallbackAction = callback;
        }

        /// <summary>
        /// 指定源状态与目标状态构建转移, 并设置其条件与回调函数
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        public Translation(string fromState, string targetState, BDelegate<object, bool>.Method conditionMethod, BDelegate<object, string>.Method callbackMethod) : this(fromState, targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod)) { }

        /// <summary>
        /// 指定源状态与目标状态构建转移, 并设置其条件与回调函数
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        public Translation(string fromState, string targetState, BDelegate<object, bool>.Method conditionMethod, BDelegate<object, string>.MethodNone callbackMethod) : this(fromState, targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod)) { }

        /// <summary>
        /// 指定源状态与目标状态构建转移, 并设置其条件与回调函数
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        public Translation(string fromState, string targetState, BDelegate<object, bool>.MethodNone conditionMethod, BDelegate<object, string>.Method callbackMethod) : this(fromState, targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod)) { }

        /// <summary>
        /// 指定源状态与目标状态构建转移, 并设置其条件与回调函数
        /// </summary>
        /// <param name="fromState"></param>
        /// <param name="targetState"></param>
        /// <param name="conditionMethod"></param>
        /// <param name="callbackMethod"></param>
        public Translation(string fromState, string targetState, BDelegate<object, bool>.MethodNone conditionMethod, BDelegate<object, string>.MethodNone callbackMethod) : this(fromState, targetState, new BDelegate<object, bool>(conditionMethod), new BDelegate<object, string>(callbackMethod)) { }

        /// <summary>
        /// 判断是否满足转移发生条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Determine(object input)
        {
            if (Condition == null)
            {
                return false;
            }
            return Condition.Execute(input);
        }

        /// <summary>
        /// 执行回调函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Callback(object input)
        {
            if (CallbackAction == null)
            {
                return TargetState;
            }
            return CallbackAction.Execute(input);
        }
    }
}
